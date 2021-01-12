using J2N.Text;
using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Server;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using System;
using System.IO;

namespace Q2Sharp.Qcommon
{
	public sealed class Com
	{
		public static String debugContext = "";
		static String _debugContext = "";
		static Int32 com_argc;
		static String[] com_argv = new String[Defines.MAX_NUM_ARGVS];
		public abstract class RD_Flusher
		{
			public abstract void Rd_flush( Int32 target, StringBuffer buffer );
		}

		static Int32 rd_target;
		static StringBuffer rd_buffer;
		static Int32 rd_buffersize;
		static RD_Flusher rd_flusher;
		public static void BeginRedirect( Int32 target, StringBuffer buffer, Int32 buffersize, RD_Flusher flush )
		{
			if ( 0 == target || null == buffer || 0 == buffersize || null == flush )
				return;
			rd_target = target;
			rd_buffer = buffer;
			rd_buffersize = buffersize;
			rd_flusher = flush;
			rd_buffer.Length = 0;
		}

		public static void EndRedirect( )
		{
			rd_flusher.Rd_flush( rd_target, rd_buffer );
			rd_target = 0;
			rd_buffer = null;
			rd_buffersize = 0;
			rd_flusher = null;
		}

		static Boolean recursive = false;
		static String msg = "";
		public class ParseHelp
		{
			public ParseHelp( String in_renamed )
			{
				if ( in_renamed == null )
				{
					data = null;
					length = 0;
				}
				else
				{
					data = in_renamed.ToCharArray();
					length = data.Length;
				}

				index = 0;
			}

			public ParseHelp( Char[] in_renamed ) : this( in_renamed, 0 )
			{
			}

			public ParseHelp( Char[] in_renamed, Int32 offset )
			{
				data = in_renamed;
				index = offset;
				if ( data != null )
					length = data.Length;
				else
					length = 0;
			}

			public virtual Char Getchar( )
			{
				if ( index < length )
				{
					return data[index];
				}

				return ( Char ) 0;
			}

			public virtual Char Nextchar( )
			{
				index++;
				if ( index < length )
				{
					return data[index];
				}

				return ( Char ) 0;
			}

			public virtual Char Prevchar( )
			{
				if ( index > 0 )
				{
					index--;
					return data[index];
				}

				return ( Char ) 0;
			}

			public virtual Boolean IsEof( )
			{
				return index >= length;
			}

			public Int32 index;
			public Char[] data;
			private Int32 length;
			public virtual Char Skipwhites( )
			{
				var c = ( Char ) 0;
				while ( index < length && ( ( c = data[index] ) <= ' ' ) && c != 0 )
					index++;
				return c;
			}

			public virtual Char Skipwhitestoeol( )
			{
				var c = ( Char ) 0;
				while ( index < length && ( ( c = data[index] ) <= ' ' ) && c != '\\' && c != 0 )
					index++;
				return c;
			}

			public virtual Char Skiptoeol( )
			{
				var c = ( Char ) 0;
				while ( index < length && ( c = data[index] ) != '\\' && c != 0 )
					index++;
				return c;
			}
		}

		public static Char[] com_token = new Char[Defines.MAX_TOKEN_CHARS];
		public static String Parse( ParseHelp hlp )
		{
			Int32 c;
			var len = 0;
			if ( hlp.data == null )
			{
				return "";
			}

			while ( true )
			{
				hlp.Skipwhites();
				if ( hlp.IsEof() )
				{
					hlp.data = null;
					return "";
				}

				if ( hlp.Getchar() == '/' )
				{
					if ( hlp.Nextchar() == '/' )
					{
						hlp.Skiptoeol();
						continue;
					}
					else
					{
						hlp.Prevchar();
						break;
					}
				}
				else
					break;
			}

			if ( hlp.Getchar() == '\\' )
			{
				hlp.Nextchar();
				while ( true )
				{
					c = hlp.Getchar();
					hlp.Nextchar();
					if ( c == '\\' || c == 0 )
					{
						return new String( com_token, 0, len );
					}

					if ( len < Defines.MAX_TOKEN_CHARS )
					{
						com_token[len] = ( Char ) c;
						len++;
					}
				}
			}

			c = hlp.Getchar();
			do
			{
				if ( len < Defines.MAX_TOKEN_CHARS )
				{
					com_token[len] = ( Char ) c;
					len++;
				}

				c = hlp.Nextchar();
			}
			while ( c > 32 );
			if ( len == Defines.MAX_TOKEN_CHARS )
			{
				Com.Printf( "Token exceeded " + Defines.MAX_TOKEN_CHARS + " chars, discarded.\\n" );
				len = 0;
			}

			return new String( com_token, 0, len );
		}

		public static xcommand_t Error_f = new Anonymousxcommand_t();
		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				Error( Defines.ERR_FATAL, Cmd.Argv( 1 ) );
			}
		}

		public static void Error( Int32 code, String fmt )
		{
			Error( code, fmt, null );
		}

		public static void Error( Int32 code, String fmt, params Object[] parameters )
		{
			if ( recursive )
			{
				CoreSys.Error( "recursive error after: " + msg );
			}

			recursive = true;
			msg = Sprintf( fmt, parameters );
			if ( code == Defines.ERR_DISCONNECT )
			{
				CL.Drop();
				recursive = false;
				throw new longjmpException();
			}
			else if ( code == Defines.ERR_DROP )
			{
				Com.Printf( "********************\\nERROR: " + msg + "\\n********************\\n" );
				SV_MAIN.SV_Shutdown( "Server crashed: " + msg + "\\n", false );
				CL.Drop();
				recursive = false;
				throw new longjmpException();
			}
			else
			{
				SV_MAIN.SV_Shutdown( "Server fatal crashed: %s" + msg + "\\n", false );
				CL.Shutdown();
			}

			CoreSys.Error( msg );
		}

		public static void InitArgv( String[] args )
		{
			if ( args.Length > Defines.MAX_NUM_ARGVS )
			{
				Com.Error( Defines.ERR_FATAL, "argc > MAX_NUM_ARGVS" );
			}

			Com.com_argc = args.Length;
			for ( var i = 0; i < Com.com_argc; i++ )
			{
				if ( args[i].Length >= Defines.MAX_TOKEN_CHARS )
					Com.com_argv[i] = "";
				else
					Com.com_argv[i] = args[i];
			}
		}

		public static void DPrintf( String fmt )
		{
			_debugContext = debugContext;
			DPrintf( fmt, null );
			_debugContext = "";
		}

		public static void Dprintln( String fmt )
		{
			DPrintf( _debugContext + fmt + "\\n", null );
		}

		public static void Printf( String fmt )
		{
			Printf( _debugContext + fmt, null );
		}

		public static void DPrintf( String fmt, params Object[] parameters )
		{
			if ( Globals.developer == null || Globals.developer.value == 0 )
				return;
			_debugContext = debugContext;
			Printf( fmt, parameters );
			_debugContext = "";
		}

		public static void Printf( String fmt, params Object[] parameters )
		{
			var msg = Sprintf( _debugContext + fmt, parameters );
			if ( rd_target != 0 )
			{
				if ( ( msg.Length + rd_buffer.Length ) > ( rd_buffersize - 1 ) )
				{
					rd_flusher.Rd_flush( rd_target, rd_buffer );
					rd_buffer.Length = 0;
				}

				rd_buffer.Append( msg );
				return;
			}

			System.Console.Write( msg );
			CoreSys.ConsoleOutput( msg );
			if ( Globals.logfile_active != null && Globals.logfile_active.value != 0 )
			{
				String name;
				if ( Globals.logfile == null )
				{
					name = FS.Gamedir() + "/qconsole.log";
					if ( Globals.logfile_active.value > 2 )
						try
						{
							Globals.logfile = new QuakeFile( name, FileAccess.ReadWrite );
							Globals.logfile.Seek( Globals.logfile.Length );
						}
						catch ( Exception e )
						{
							e.PrintStackTrace();
						}
					else
						try
						{
							Globals.logfile = new QuakeFile( name, FileAccess.ReadWrite );
						}
						catch ( FileNotFoundException e1 )
						{
							e1.PrintStackTrace();
						}
				}

				if ( Globals.logfile != null )
					try
					{
						Globals.logfile.Write( msg );
					}
					catch ( IOException e )
					{
						e.PrintStackTrace();
					}
			}
		}

		public static void Println( String fmt )
		{
			Printf( _debugContext + fmt + "\\n" );
		}

		public static String Sprintf( String fmt, params Object[] parameters )
		{
			var msg = "";
			if ( parameters == null || parameters.Length == 0 )
			{
				msg = fmt;
			}
			else
			{
				msg = new PrintfFormat( fmt ).Sprintf( parameters );
			}

			return msg;
		}

		public static Int32 Argc( )
		{
			return Com.com_argc;
		}

		public static String Argv( Int32 arg )
		{
			if ( arg < 0 || arg >= Com.com_argc || Com.com_argv[arg].Length < 1 )
				return "";
			return Com.com_argv[arg];
		}

		public static void ClearArgv( Int32 arg )
		{
			if ( arg < 0 || arg >= Com.com_argc || Com.com_argv[arg].Length < 1 )
				return;
			Com.com_argv[arg] = "";
		}

		public static void Quit( )
		{
			SV_MAIN.SV_Shutdown( "Server quit\\n", false );
			CL.Shutdown();
			if ( Globals.logfile != null )
			{
				try
				{
					Globals.logfile.Close();
				}
				catch ( IOException e )
				{
				}

				Globals.logfile = null;
			}

			CoreSys.Quit();
		}

		public static void SetServerState( Int32 i )
		{
			Globals.server_state = i;
		}

		public static Int32 BlockChecksum( Byte[] buf, Int32 length )
		{
			return MD4.Com_BlockChecksum( buf, length );
		}

		public static String StripExtension( String string_renamed )
		{
			var i = string_renamed.LastIndexOf( '.' );
			if ( i < 0 )
				return string_renamed;
			return string_renamed.Substring( 0, i );
		}

		static Byte[] chktbl = new Byte[] { ( Byte ) 0x84, ( Byte ) 0x47, ( Byte ) 0x51, ( Byte ) 0xc1, ( Byte ) 0x93, ( Byte ) 0x22, ( Byte ) 0x21, ( Byte ) 0x24, ( Byte ) 0x2f, ( Byte ) 0x66, ( Byte ) 0x60, ( Byte ) 0x4d, ( Byte ) 0xb0, ( Byte ) 0x7c, ( Byte ) 0xda, ( Byte ) 0x88, ( Byte ) 0x54, ( Byte ) 0x15, ( Byte ) 0x2b, ( Byte ) 0xc6, ( Byte ) 0x6c, ( Byte ) 0x89, ( Byte ) 0xc5, ( Byte ) 0x9d, ( Byte ) 0x48, ( Byte ) 0xee, ( Byte ) 0xe6, ( Byte ) 0x8a, ( Byte ) 0xb5, ( Byte ) 0xf4, ( Byte ) 0xcb, ( Byte ) 0xfb, ( Byte ) 0xf1, ( Byte ) 0x0c, ( Byte ) 0x2e, ( Byte ) 0xa0, ( Byte ) 0xd7, ( Byte ) 0xc9, ( Byte ) 0x1f, ( Byte ) 0xd6, ( Byte ) 0x06, ( Byte ) 0x9a, ( Byte ) 0x09, ( Byte ) 0x41, ( Byte ) 0x54, ( Byte ) 0x67, ( Byte ) 0x46, ( Byte ) 0xc7, ( Byte ) 0x74, ( Byte ) 0xe3, ( Byte ) 0xc8, ( Byte ) 0xb6, ( Byte ) 0x5d, ( Byte ) 0xa6, ( Byte ) 0x36, ( Byte ) 0xc4, ( Byte ) 0xab, ( Byte ) 0x2c, ( Byte ) 0x7e, ( Byte ) 0x85, ( Byte ) 0xa8, ( Byte ) 0xa4, ( Byte ) 0xa6, ( Byte ) 0x4d, ( Byte ) 0x96, ( Byte ) 0x19, ( Byte ) 0x19, ( Byte ) 0x9a, ( Byte ) 0xcc, ( Byte ) 0xd8, ( Byte ) 0xac, ( Byte ) 0x39, ( Byte ) 0x5e, ( Byte ) 0x3c, ( Byte ) 0xf2, ( Byte ) 0xf5, ( Byte ) 0x5a, ( Byte ) 0x72, ( Byte ) 0xe5, ( Byte ) 0xa9, ( Byte ) 0xd1, ( Byte ) 0xb3, ( Byte ) 0x23, ( Byte ) 0x82, ( Byte ) 0x6f, ( Byte ) 0x29, ( Byte ) 0xcb, ( Byte ) 0xd1, ( Byte ) 0xcc, ( Byte ) 0x71, ( Byte ) 0xfb, ( Byte ) 0xea, ( Byte ) 0x92, ( Byte ) 0xeb, ( Byte ) 0x1c, ( Byte ) 0xca, ( Byte ) 0x4c, ( Byte ) 0x70, ( Byte ) 0xfe, ( Byte ) 0x4d, ( Byte ) 0xc9, ( Byte ) 0x67, ( Byte ) 0x43, ( Byte ) 0x47, ( Byte ) 0x94, ( Byte ) 0xb9, ( Byte ) 0x47, ( Byte ) 0xbc, ( Byte ) 0x3f, ( Byte ) 0x01, ( Byte ) 0xab, ( Byte ) 0x7b, ( Byte ) 0xa6, ( Byte ) 0xe2, ( Byte ) 0x76, ( Byte ) 0xef, ( Byte ) 0x5a, ( Byte ) 0x7a, ( Byte ) 0x29, ( Byte ) 0x0b, ( Byte ) 0x51, ( Byte ) 0x54, ( Byte ) 0x67, ( Byte ) 0xd8, ( Byte ) 0x1c, ( Byte ) 0x14, ( Byte ) 0x3e, ( Byte ) 0x29, ( Byte ) 0xec, ( Byte ) 0xe9, ( Byte ) 0x2d, ( Byte ) 0x48, ( Byte ) 0x67, ( Byte ) 0xff, ( Byte ) 0xed, ( Byte ) 0x54, ( Byte ) 0x4f, ( Byte ) 0x48, ( Byte ) 0xc0, ( Byte ) 0xaa, ( Byte ) 0x61, ( Byte ) 0xf7, ( Byte ) 0x78, ( Byte ) 0x12, ( Byte ) 0x03, ( Byte ) 0x7a, ( Byte ) 0x9e, ( Byte ) 0x8b, ( Byte ) 0xcf, ( Byte ) 0x83, ( Byte ) 0x7b, ( Byte ) 0xae, ( Byte ) 0xca, ( Byte ) 0x7b, ( Byte ) 0xd9, ( Byte ) 0xe9, ( Byte ) 0x53, ( Byte ) 0x2a, ( Byte ) 0xeb, ( Byte ) 0xd2, ( Byte ) 0xd8, ( Byte ) 0xcd, ( Byte ) 0xa3, ( Byte ) 0x10, ( Byte ) 0x25, ( Byte ) 0x78, ( Byte ) 0x5a, ( Byte ) 0xb5, ( Byte ) 0x23, ( Byte ) 0x06, ( Byte ) 0x93, ( Byte ) 0xb7, ( Byte ) 0x84, ( Byte ) 0xd2, ( Byte ) 0xbd, ( Byte ) 0x96, ( Byte ) 0x75, ( Byte ) 0xa5, ( Byte ) 0x5e, ( Byte ) 0xcf, ( Byte ) 0x4e, ( Byte ) 0xe9, ( Byte ) 0x50, ( Byte ) 0xa1, ( Byte ) 0xe6, ( Byte ) 0x9d, ( Byte ) 0xb1, ( Byte ) 0xe3, ( Byte ) 0x85, ( Byte ) 0x66, ( Byte ) 0x28, ( Byte ) 0x4e, ( Byte ) 0x43, ( Byte ) 0xdc, ( Byte ) 0x6e, ( Byte ) 0xbb, ( Byte ) 0x33, ( Byte ) 0x9e, ( Byte ) 0xf3, ( Byte ) 0x0d, ( Byte ) 0x00, ( Byte ) 0xc1, ( Byte ) 0xcf, ( Byte ) 0x67, ( Byte ) 0x34, ( Byte ) 0x06, ( Byte ) 0x7c, ( Byte ) 0x71, ( Byte ) 0xe3, ( Byte ) 0x63, ( Byte ) 0xb7, ( Byte ) 0xb7, ( Byte ) 0xdf, ( Byte ) 0x92, ( Byte ) 0xc4, ( Byte ) 0xc2, ( Byte ) 0x25, ( Byte ) 0x5c, ( Byte ) 0xff, ( Byte ) 0xc3, ( Byte ) 0x6e, ( Byte ) 0xfc, ( Byte ) 0xaa, ( Byte ) 0x1e, ( Byte ) 0x2a, ( Byte ) 0x48, ( Byte ) 0x11, ( Byte ) 0x1c, ( Byte ) 0x36, ( Byte ) 0x68, ( Byte ) 0x78, ( Byte ) 0x86, ( Byte ) 0x79, ( Byte ) 0x30, ( Byte ) 0xc3, ( Byte ) 0xd6, ( Byte ) 0xde, ( Byte ) 0xbc, ( Byte ) 0x3a, ( Byte ) 0x2a, ( Byte ) 0x6d, ( Byte ) 0x1e, ( Byte ) 0x46, ( Byte ) 0xdd, ( Byte ) 0xe0, ( Byte ) 0x80, ( Byte ) 0x1e, ( Byte ) 0x44, ( Byte ) 0x3b, ( Byte ) 0x6f, ( Byte ) 0xaf, ( Byte ) 0x31, ( Byte ) 0xda, ( Byte ) 0xa2, ( Byte ) 0xbd, ( Byte ) 0x77, ( Byte ) 0x06, ( Byte ) 0x56, ( Byte ) 0xc0, ( Byte ) 0xb7, ( Byte ) 0x92, ( Byte ) 0x4b, ( Byte ) 0x37, ( Byte ) 0xc0, ( Byte ) 0xfc, ( Byte ) 0xc2, ( Byte ) 0xd5, ( Byte ) 0xfb, ( Byte ) 0xa8, ( Byte ) 0xda, ( Byte ) 0xf5, ( Byte ) 0x57, ( Byte ) 0xa8, ( Byte ) 0x18, ( Byte ) 0xc0, ( Byte ) 0xdf, ( Byte ) 0xe7, ( Byte ) 0xaa, ( Byte ) 0x2a, ( Byte ) 0xe0, ( Byte ) 0x7c, ( Byte ) 0x6f, ( Byte ) 0x77, ( Byte ) 0xb1, ( Byte ) 0x26, ( Byte ) 0xba, ( Byte ) 0xf9, ( Byte ) 0x2e, ( Byte ) 0x1d, ( Byte ) 0x16, ( Byte ) 0xcb, ( Byte ) 0xb8, ( Byte ) 0xa2, ( Byte ) 0x44, ( Byte ) 0xd5, ( Byte ) 0x2f, ( Byte ) 0x1a, ( Byte ) 0x79, ( Byte ) 0x74, ( Byte ) 0x87, ( Byte ) 0x4b, ( Byte ) 0x00, ( Byte ) 0xc9, ( Byte ) 0x4a, ( Byte ) 0x3a, ( Byte ) 0x65, ( Byte ) 0x8f, ( Byte ) 0xe6, ( Byte ) 0x5d, ( Byte ) 0xe5, ( Byte ) 0x0a, ( Byte ) 0x77, ( Byte ) 0xd8, ( Byte ) 0x1a, ( Byte ) 0x14, ( Byte ) 0x41, ( Byte ) 0x75, ( Byte ) 0xb1, ( Byte ) 0xe2, ( Byte ) 0x50, ( Byte ) 0x2c, ( Byte ) 0x93, ( Byte ) 0x38, ( Byte ) 0x2b, ( Byte ) 0x6d, ( Byte ) 0xf3, ( Byte ) 0xf6, ( Byte ) 0xdb, ( Byte ) 0x1f, ( Byte ) 0xcd, ( Byte ) 0xff, ( Byte ) 0x14, ( Byte ) 0x70, ( Byte ) 0xe7, ( Byte ) 0x16, ( Byte ) 0xe8, ( Byte ) 0x3d, ( Byte ) 0xf0, ( Byte ) 0xe3, ( Byte ) 0xbc, ( Byte ) 0x5e, ( Byte ) 0xb6, ( Byte ) 0x3f, ( Byte ) 0xcc, ( Byte ) 0x81, ( Byte ) 0x24, ( Byte ) 0x67, ( Byte ) 0xf3, ( Byte ) 0x97, ( Byte ) 0x3b, ( Byte ) 0xfe, ( Byte ) 0x3a, ( Byte ) 0x96, ( Byte ) 0x85, ( Byte ) 0xdf, ( Byte ) 0xe4, ( Byte ) 0x6e, ( Byte ) 0x3c, ( Byte ) 0x85, ( Byte ) 0x05, ( Byte ) 0x0e, ( Byte ) 0xa3, ( Byte ) 0x2b, ( Byte ) 0x07, ( Byte ) 0xc8, ( Byte ) 0xbf, ( Byte ) 0xe5, ( Byte ) 0x13, ( Byte ) 0x82, ( Byte ) 0x62, ( Byte ) 0x08, ( Byte ) 0x61, ( Byte ) 0x69, ( Byte ) 0x4b, ( Byte ) 0x47, ( Byte ) 0x62, ( Byte ) 0x73, ( Byte ) 0x44, ( Byte ) 0x64, ( Byte ) 0x8e, ( Byte ) 0xe2, ( Byte ) 0x91, ( Byte ) 0xa6, ( Byte ) 0x9a, ( Byte ) 0xb7, ( Byte ) 0xe9, ( Byte ) 0x04, ( Byte ) 0xb6, ( Byte ) 0x54, ( Byte ) 0x0c, ( Byte ) 0xc5, ( Byte ) 0xa9, ( Byte ) 0x47, ( Byte ) 0xa6, ( Byte ) 0xc9, ( Byte ) 0x08, ( Byte ) 0xfe, ( Byte ) 0x4e, ( Byte ) 0xa6, ( Byte ) 0xcc, ( Byte ) 0x8a, ( Byte ) 0x5b, ( Byte ) 0x90, ( Byte ) 0x6f, ( Byte ) 0x2b, ( Byte ) 0x3f, ( Byte ) 0xb6, ( Byte ) 0x0a, ( Byte ) 0x96, ( Byte ) 0xc0, ( Byte ) 0x78, ( Byte ) 0x58, ( Byte ) 0x3c, ( Byte ) 0x76, ( Byte ) 0x6d, ( Byte ) 0x94, ( Byte ) 0x1a, ( Byte ) 0xe4, ( Byte ) 0x4e, ( Byte ) 0xb8, ( Byte ) 0x38, ( Byte ) 0xbb, ( Byte ) 0xf5, ( Byte ) 0xeb, ( Byte ) 0x29, ( Byte ) 0xd8, ( Byte ) 0xb0, ( Byte ) 0xf3, ( Byte ) 0x15, ( Byte ) 0x1e, ( Byte ) 0x99, ( Byte ) 0x96, ( Byte ) 0x3c, ( Byte ) 0x5d, ( Byte ) 0x63, ( Byte ) 0xd5, ( Byte ) 0xb1, ( Byte ) 0xad, ( Byte ) 0x52, ( Byte ) 0xb8, ( Byte ) 0x55, ( Byte ) 0x70, ( Byte ) 0x75, ( Byte ) 0x3e, ( Byte ) 0x1a, ( Byte ) 0xd5, ( Byte ) 0xda, ( Byte ) 0xf6, ( Byte ) 0x7a, ( Byte ) 0x48, ( Byte ) 0x7d, ( Byte ) 0x44, ( Byte ) 0x41, ( Byte ) 0xf9, ( Byte ) 0x11, ( Byte ) 0xce, ( Byte ) 0xd7, ( Byte ) 0xca, ( Byte ) 0xa5, ( Byte ) 0x3d, ( Byte ) 0x7a, ( Byte ) 0x79, ( Byte ) 0x7e, ( Byte ) 0x7d, ( Byte ) 0x25, ( Byte ) 0x1b, ( Byte ) 0x77, ( Byte ) 0xbc, ( Byte ) 0xf7, ( Byte ) 0xc7, ( Byte ) 0x0f, ( Byte ) 0x84, ( Byte ) 0x95, ( Byte ) 0x10, ( Byte ) 0x92, ( Byte ) 0x67, ( Byte ) 0x15, ( Byte ) 0x11, ( Byte ) 0x5a, ( Byte ) 0x5e, ( Byte ) 0x41, ( Byte ) 0x66, ( Byte ) 0x0f, ( Byte ) 0x38, ( Byte ) 0x03, ( Byte ) 0xb2, ( Byte ) 0xf1, ( Byte ) 0x5d, ( Byte ) 0xf8, ( Byte ) 0xab, ( Byte ) 0xc0, ( Byte ) 0x02, ( Byte ) 0x76, ( Byte ) 0x84, ( Byte ) 0x28, ( Byte ) 0xf4, ( Byte ) 0x9d, ( Byte ) 0x56, ( Byte ) 0x46, ( Byte ) 0x60, ( Byte ) 0x20, ( Byte ) 0xdb, ( Byte ) 0x68, ( Byte ) 0xa7, ( Byte ) 0xbb, ( Byte ) 0xee, ( Byte ) 0xac, ( Byte ) 0x15, ( Byte ) 0x01, ( Byte ) 0x2f, ( Byte ) 0x20, ( Byte ) 0x09, ( Byte ) 0xdb, ( Byte ) 0xc0, ( Byte ) 0x16, ( Byte ) 0xa1, ( Byte ) 0x89, ( Byte ) 0xf9, ( Byte ) 0x94, ( Byte ) 0x59, ( Byte ) 0x00, ( Byte ) 0xc1, ( Byte ) 0x76, ( Byte ) 0xbf, ( Byte ) 0xc1, ( Byte ) 0x4d, ( Byte ) 0x5d, ( Byte ) 0x2d, ( Byte ) 0xa9, ( Byte ) 0x85, ( Byte ) 0x2c, ( Byte ) 0xd6, ( Byte ) 0xd3, ( Byte ) 0x14, ( Byte ) 0xcc, ( Byte ) 0x02, ( Byte ) 0xc3, ( Byte ) 0xc2, ( Byte ) 0xfa, ( Byte ) 0x6b, ( Byte ) 0xb7, ( Byte ) 0xa6, ( Byte ) 0xef, ( Byte ) 0xdd, ( Byte ) 0x12, ( Byte ) 0x26, ( Byte ) 0xa4, ( Byte ) 0x63, ( Byte ) 0xe3, ( Byte ) 0x62, ( Byte ) 0xbd, ( Byte ) 0x56, ( Byte ) 0x8a, ( Byte ) 0x52, ( Byte ) 0x2b, ( Byte ) 0xb9, ( Byte ) 0xdf, ( Byte ) 0x09, ( Byte ) 0xbc, ( Byte ) 0x0e, ( Byte ) 0x97, ( Byte ) 0xa9, ( Byte ) 0xb0, ( Byte ) 0x82, ( Byte ) 0x46, ( Byte ) 0x08, ( Byte ) 0xd5, ( Byte ) 0x1a, ( Byte ) 0x8e, ( Byte ) 0x1b, ( Byte ) 0xa7, ( Byte ) 0x90, ( Byte ) 0x98, ( Byte ) 0xb9, ( Byte ) 0xbb, ( Byte ) 0x3c, ( Byte ) 0x17, ( Byte ) 0x9a, ( Byte ) 0xf2, ( Byte ) 0x82, ( Byte ) 0xba, ( Byte ) 0x64, ( Byte ) 0x0a, ( Byte ) 0x7f, ( Byte ) 0xca, ( Byte ) 0x5a, ( Byte ) 0x8c, ( Byte ) 0x7c, ( Byte ) 0xd3, ( Byte ) 0x79, ( Byte ) 0x09, ( Byte ) 0x5b, ( Byte ) 0x26, ( Byte ) 0xbb, ( Byte ) 0xbd, ( Byte ) 0x25, ( Byte ) 0xdf, ( Byte ) 0x3d, ( Byte ) 0x6f, ( Byte ) 0x9a, ( Byte ) 0x8f, ( Byte ) 0xee, ( Byte ) 0x21, ( Byte ) 0x66, ( Byte ) 0xb0, ( Byte ) 0x8d, ( Byte ) 0x84, ( Byte ) 0x4c, ( Byte ) 0x91, ( Byte ) 0x45, ( Byte ) 0xd4, ( Byte ) 0x77, ( Byte ) 0x4f, ( Byte ) 0xb3, ( Byte ) 0x8c, ( Byte ) 0xbc, ( Byte ) 0xa8, ( Byte ) 0x99, ( Byte ) 0xaa, ( Byte ) 0x19, ( Byte ) 0x53, ( Byte ) 0x7c, ( Byte ) 0x02, ( Byte ) 0x87, ( Byte ) 0xbb, ( Byte ) 0x0b, ( Byte ) 0x7c, ( Byte ) 0x1a, ( Byte ) 0x2d, ( Byte ) 0xdf, ( Byte ) 0x48, ( Byte ) 0x44, ( Byte ) 0x06, ( Byte ) 0xd6, ( Byte ) 0x7d, ( Byte ) 0x0c, ( Byte ) 0x2d, ( Byte ) 0x35, ( Byte ) 0x76, ( Byte ) 0xae, ( Byte ) 0xc4, ( Byte ) 0x5f, ( Byte ) 0x71, ( Byte ) 0x85, ( Byte ) 0x97, ( Byte ) 0xc4, ( Byte ) 0x3d, ( Byte ) 0xef, ( Byte ) 0x52, ( Byte ) 0xbe, ( Byte ) 0x00, ( Byte ) 0xe4, ( Byte ) 0xcd, ( Byte ) 0x49, ( Byte ) 0xd1, ( Byte ) 0xd1, ( Byte ) 0x1c, ( Byte ) 0x3c, ( Byte ) 0xd0, ( Byte ) 0x1c, ( Byte ) 0x42, ( Byte ) 0xaf, ( Byte ) 0xd4, ( Byte ) 0xbd, ( Byte ) 0x58, ( Byte ) 0x34, ( Byte ) 0x07, ( Byte ) 0x32, ( Byte ) 0xee, ( Byte ) 0xb9, ( Byte ) 0xb5, ( Byte ) 0xea, ( Byte ) 0xff, ( Byte ) 0xd7, ( Byte ) 0x8c, ( Byte ) 0x0d, ( Byte ) 0x2e, ( Byte ) 0x2f, ( Byte ) 0xaf, ( Byte ) 0x87, ( Byte ) 0xbb, ( Byte ) 0xe6, ( Byte ) 0x52, ( Byte ) 0x71, ( Byte ) 0x22, ( Byte ) 0xf5, ( Byte ) 0x25, ( Byte ) 0x17, ( Byte ) 0xa1, ( Byte ) 0x82, ( Byte ) 0x04, ( Byte ) 0xc2, ( Byte ) 0x4a, ( Byte ) 0xbd, ( Byte ) 0x57, ( Byte ) 0xc6, ( Byte ) 0xab, ( Byte ) 0xc8, ( Byte ) 0x35, ( Byte ) 0x0c, ( Byte ) 0x3c, ( Byte ) 0xd9, ( Byte ) 0xc2, ( Byte ) 0x43, ( Byte ) 0xdb, ( Byte ) 0x27, ( Byte ) 0x92, ( Byte ) 0xcf, ( Byte ) 0xb8, ( Byte ) 0x25, ( Byte ) 0x60, ( Byte ) 0xfa, ( Byte ) 0x21, ( Byte ) 0x3b, ( Byte ) 0x04, ( Byte ) 0x52, ( Byte ) 0xc8, ( Byte ) 0x96, ( Byte ) 0xba, ( Byte ) 0x74, ( Byte ) 0xe3, ( Byte ) 0x67, ( Byte ) 0x3e, ( Byte ) 0x8e, ( Byte ) 0x8d, ( Byte ) 0x61, ( Byte ) 0x90, ( Byte ) 0x92, ( Byte ) 0x59, ( Byte ) 0xb6, ( Byte ) 0x1a, ( Byte ) 0x1c, ( Byte ) 0x5e, ( Byte ) 0x21, ( Byte ) 0xc1, ( Byte ) 0x65, ( Byte ) 0xe5, ( Byte ) 0xa6, ( Byte ) 0x34, ( Byte ) 0x05, ( Byte ) 0x6f, ( Byte ) 0xc5, ( Byte ) 0x60, ( Byte ) 0xb1, ( Byte ) 0x83, ( Byte ) 0xc1, ( Byte ) 0xd5, ( Byte ) 0xd5, ( Byte ) 0xed, ( Byte ) 0xd9, ( Byte ) 0xc7, ( Byte ) 0x11, ( Byte ) 0x7b, ( Byte ) 0x49, ( Byte ) 0x7a, ( Byte ) 0xf9, ( Byte ) 0xf9, ( Byte ) 0x84, ( Byte ) 0x47, ( Byte ) 0x9b, ( Byte ) 0xe2, ( Byte ) 0xa5, ( Byte ) 0x82, ( Byte ) 0xe0, ( Byte ) 0xc2, ( Byte ) 0x88, ( Byte ) 0xd0, ( Byte ) 0xb2, ( Byte ) 0x58, ( Byte ) 0x88, ( Byte ) 0x7f, ( Byte ) 0x45, ( Byte ) 0x09, ( Byte ) 0x67, ( Byte ) 0x74, ( Byte ) 0x61, ( Byte ) 0xbf, ( Byte ) 0xe6, ( Byte ) 0x40, ( Byte ) 0xe2, ( Byte ) 0x9d, ( Byte ) 0xc2, ( Byte ) 0x47, ( Byte ) 0x05, ( Byte ) 0x89, ( Byte ) 0xed, ( Byte ) 0xcb, ( Byte ) 0xbb, ( Byte ) 0xb7, ( Byte ) 0x27, ( Byte ) 0xe7, ( Byte ) 0xdc, ( Byte ) 0x7a, ( Byte ) 0xfd, ( Byte ) 0xbf, ( Byte ) 0xa8, ( Byte ) 0xd0, ( Byte ) 0xaa, ( Byte ) 0x10, ( Byte ) 0x39, ( Byte ) 0x3c, ( Byte ) 0x20, ( Byte ) 0xf0, ( Byte ) 0xd3, ( Byte ) 0x6e, ( Byte ) 0xb1, ( Byte ) 0x72, ( Byte ) 0xf8, ( Byte ) 0xe6, ( Byte ) 0x0f, ( Byte ) 0xef, ( Byte ) 0x37, ( Byte ) 0xe5, ( Byte ) 0x09, ( Byte ) 0x33, ( Byte ) 0x5a, ( Byte ) 0x83, ( Byte ) 0x43, ( Byte ) 0x80, ( Byte ) 0x4f, ( Byte ) 0x65, ( Byte ) 0x2f, ( Byte ) 0x7c, ( Byte ) 0x8c, ( Byte ) 0x6a, ( Byte ) 0xa0, ( Byte ) 0x82, ( Byte ) 0x0c, ( Byte ) 0xd4, ( Byte ) 0xd4, ( Byte ) 0xfa, ( Byte ) 0x81, ( Byte ) 0x60, ( Byte ) 0x3d, ( Byte ) 0xdf, ( Byte ) 0x06, ( Byte ) 0xf1, ( Byte ) 0x5f, ( Byte ) 0x08, ( Byte ) 0x0d, ( Byte ) 0x6d, ( Byte ) 0x43, ( Byte ) 0xf2, ( Byte ) 0xe3, ( Byte ) 0x11, ( Byte ) 0x7d, ( Byte ) 0x80, ( Byte ) 0x32, ( Byte ) 0xc5, ( Byte ) 0xfb, ( Byte ) 0xc5, ( Byte ) 0xd9, ( Byte ) 0x27, ( Byte ) 0xec, ( Byte ) 0xc6, ( Byte ) 0x4e, ( Byte ) 0x65, ( Byte ) 0x27, ( Byte ) 0x76, ( Byte ) 0x87, ( Byte ) 0xa6, ( Byte ) 0xee, ( Byte ) 0xee, ( Byte ) 0xd7, ( Byte ) 0x8b, ( Byte ) 0xd1, ( Byte ) 0xa0, ( Byte ) 0x5c, ( Byte ) 0xb0, ( Byte ) 0x42, ( Byte ) 0x13, ( Byte ) 0x0e, ( Byte ) 0x95, ( Byte ) 0x4a, ( Byte ) 0xf2, ( Byte ) 0x06, ( Byte ) 0xc6, ( Byte ) 0x43, ( Byte ) 0x33, ( Byte ) 0xf4, ( Byte ) 0xc7, ( Byte ) 0xf8, ( Byte ) 0xe7, ( Byte ) 0x1f, ( Byte ) 0xdd, ( Byte ) 0xe4, ( Byte ) 0x46, ( Byte ) 0x4a, ( Byte ) 0x70, ( Byte ) 0x39, ( Byte ) 0x6c, ( Byte ) 0xd0, ( Byte ) 0xed, ( Byte ) 0xca, ( Byte ) 0xbe, ( Byte ) 0x60, ( Byte ) 0x3b, ( Byte ) 0xd1, ( Byte ) 0x7b, ( Byte ) 0x57, ( Byte ) 0x48, ( Byte ) 0xe5, ( Byte ) 0x3a, ( Byte ) 0x79, ( Byte ) 0xc1, ( Byte ) 0x69, ( Byte ) 0x33, ( Byte ) 0x53, ( Byte ) 0x1b, ( Byte ) 0x80, ( Byte ) 0xb8, ( Byte ) 0x91, ( Byte ) 0x7d, ( Byte ) 0xb4, ( Byte ) 0xf6, ( Byte ) 0x17, ( Byte ) 0x1a, ( Byte ) 0x1d, ( Byte ) 0x5a, ( Byte ) 0x32, ( Byte ) 0xd6, ( Byte ) 0xcc, ( Byte ) 0x71, ( Byte ) 0x29, ( Byte ) 0x3f, ( Byte ) 0x28, ( Byte ) 0xbb, ( Byte ) 0xf3, ( Byte ) 0x5e, ( Byte ) 0x71, ( Byte ) 0xb8, ( Byte ) 0x43, ( Byte ) 0xaf, ( Byte ) 0xf8, ( Byte ) 0xb9, ( Byte ) 0x64, ( Byte ) 0xef, ( Byte ) 0xc4, ( Byte ) 0xa5, ( Byte ) 0x6c, ( Byte ) 0x08, ( Byte ) 0x53, ( Byte ) 0xc7, ( Byte ) 0x00, ( Byte ) 0x10, ( Byte ) 0x39, ( Byte ) 0x4f, ( Byte ) 0xdd, ( Byte ) 0xe4, ( Byte ) 0xb6, ( Byte ) 0x19, ( Byte ) 0x27, ( Byte ) 0xfb, ( Byte ) 0xb8, ( Byte ) 0xf5, ( Byte ) 0x32, ( Byte ) 0x73, ( Byte ) 0xe5, ( Byte ) 0xcb, ( Byte ) 0x32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		static Byte[] chkb = new Byte[60 + 4];
		public static Byte BlockSequenceCRCByte( Byte[] base_renamed, Int32 offset, Int32 length, Int32 sequence )
		{
			if ( sequence < 0 )
				CoreSys.Error( "sequence < 0, this shouldn't happen\\n" );
			var p_ndx = ( sequence % ( 1024 - 4 ) );
			length = Math.Min( 60, length );
			System.Array.Copy( base_renamed, offset, chkb, 0, length );
			chkb[length] = chktbl[p_ndx + 0];
			chkb[length + 1] = chktbl[p_ndx + 1];
			chkb[length + 2] = chktbl[p_ndx + 2];
			chkb[length + 3] = chktbl[p_ndx + 3];
			length += 4;
			var crc = CRC.CRC_Block( chkb, length );
			var x = 0;
			for ( var n = 0; n < length; n++ )
				x += chkb[n] & 0xFF;
			crc ^= x;
			return ( Byte ) ( crc & 0xFF );
		}
	}
}
using Jake2.Game;
using System;
using System.Text;

namespace Jake2.Qcommon
{
	public sealed class Cbuf
	{
		private static readonly byte[] line = new byte[1024];
		private static readonly byte[] tmp = new byte[8192];
		public static void Init( )
		{
			SZ.Init( Globals.cmd_text, Globals.cmd_text_buf, Globals.cmd_text_buf.Length );
		}

		public static void InsertText( string text )
		{
			int templen = 0;
			templen = Globals.cmd_text.cursize;
			if ( templen != 0 )
			{
				System.Array.Copy( Globals.cmd_text.data, 0, tmp, 0, templen );
				SZ.Clear( Globals.cmd_text );
			}

			Cbuf.AddText( text );
			if ( templen != 0 )
			{
				SZ.Write( Globals.cmd_text, tmp, templen );
			}
		}

		public static void AddEarlyCommands( bool clear )
		{
			for ( int i = 0; i < Com.Argc(); i++ )
			{
				string s = Com.Argv( i );
				if ( !s.Equals( "+set" ) )
					continue;
				Cbuf.AddText( "set " + Com.Argv( i + 1 ) + " " + Com.Argv( i + 2 ) + "\\n" );
				if ( clear )
				{
					Com.ClearArgv( i );
					Com.ClearArgv( i + 1 );
					Com.ClearArgv( i + 2 );
				}

				i += 2;
			}
		}

		public static bool AddLateCommands( )
		{
			int i;
			int j = 0;
			bool ret = false;
			int s = 0;
			int argc = Com.Argc();
			for ( i = 1; i < argc; i++ )
			{
				s += Com.Argv( i ).Length;
			}

			if ( s == 0 )
				return false;
			string text = "";
			for ( i = 1; i < argc; i++ )
			{
				text += Com.Argv( i );
				if ( i != argc - 1 )
					text += " ";
			}

			string build = "";
			for ( i = 0; i < text.Length; i++ )
			{
				if ( text[i] == '+' )
				{
					i++;
					build += text.Substring( i, j );
					build += "\\n";
					i = j - 1;
				}
			}

			ret = ( build.Length != 0 );
			if ( ret )
				Cbuf.AddText( build );
			text = null;
			build = null;
			return ret;
		}

		public static void AddText( string text )
		{
			int l = text.Length;
			if ( Globals.cmd_text.cursize + l >= Globals.cmd_text.maxsize )
			{
				Com.Printf( "Cbuf_AddText: overflow\\n" );
				return;
			}

			SZ.Write( Globals.cmd_text, Encoding.ASCII.GetBytes( text ), l );
		}

		public static void Execute( )
		{
			byte[] text = null;
			Globals.alias_count = 0;
			while ( Globals.cmd_text.cursize != 0 )
			{
				text = Globals.cmd_text.data;
				int quotes = 0;
				int i;
				for ( i = 0; i < Globals.cmd_text.cursize; i++ )
				{
					if ( text[i] == '"' )
						quotes++;
					if ( !( quotes % 2 != 0 ) && text[i] == ';' )
						break;
					if ( text[i] == '\\' )
						break;
				}

				System.Array.Copy( text, 0, line, 0, i );
				line[i] = 0;
				if ( i == Globals.cmd_text.cursize )
					Globals.cmd_text.cursize = 0;
				else
				{
					i++;
					Globals.cmd_text.cursize -= i;
					System.Array.Copy( text, i, tmp, 0, Globals.cmd_text.cursize );
					System.Array.Copy( tmp, 0, text, 0, Globals.cmd_text.cursize );
					text[Globals.cmd_text.cursize] = ( Byte ) '\\';
				}

				string cmd = Encoding.ASCII.GetString( line );
				Cmd.ExecuteString( cmd );
				if ( Globals.cmd_wait )
				{
					Globals.cmd_wait = false;
					break;
				}
			}
		}

		public static void ExecuteText( int exec_when, string text )
		{
			switch ( exec_when )
			{
				case Defines.EXEC_NOW:
					Cmd.ExecuteString( text );
					break;
				case Defines.EXEC_INSERT:
					Cbuf.InsertText( text );
					break;
				case Defines.EXEC_APPEND:
					Cbuf.AddText( text );
					break;
				default:
					Com.Error( Defines.ERR_FATAL, "Cbuf_ExecuteText: bad exec_when" );
					break;
			}
		}

		public static void CopyToDefer( )
		{
			System.Array.Copy( Globals.cmd_text_buf, 0, Globals.defer_text_buf, 0, Globals.cmd_text.cursize );
			Globals.defer_text_buf[Globals.cmd_text.cursize] = 0;
			Globals.cmd_text.cursize = 0;
		}

		public static void InsertFromDefer( )
		{
			InsertText( Encoding.ASCII.GetString( Globals.defer_text_buf ).Trim() );
			Globals.defer_text_buf[0] = 0;
		}
	}
}
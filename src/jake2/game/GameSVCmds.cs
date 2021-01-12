using J2N.IO;
using J2N.Text;
using Jake2.Qcommon;
using Jake2.Util;
using System;
using System.IO;

namespace Jake2.Game
{
	public class GameSVCmds
	{
		public class ipfilter_t
		{
			public Int32 mask;
			public Int32 compare;
		}

		public static void Svcmd_Test_f( )
		{
			GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Svcmd_Test_f()\\n" );
		}

		public static readonly Int32 MAX_IPFILTERS = 1024;
		static GameSVCmds.ipfilter_t[] ipfilters = new GameSVCmds.ipfilter_t[MAX_IPFILTERS];
		static Int32 numipfilters;
		static GameSVCmds( )
		{
			for ( var n = 0; n < GameSVCmds.MAX_IPFILTERS; n++ )
				GameSVCmds.ipfilters[n] = new ipfilter_t();
		}

		static Boolean StringToFilter( String s, GameSVCmds.ipfilter_t f )
		{
			Byte[] b = new Byte[] { 0, 0, 0, 0 };
			Byte[] m = new Byte[] { 0, 0, 0, 0 };
			try
			{
				StringTokenizer tk = new StringTokenizer( s, ". " );
				for ( var n = 0; n < 4; n++ )
				{
					tk.MoveNext();
					b[n] = ( Byte ) Lib.Atoi( tk.Current );
					if ( b[n] != 0 )
						m[n] = 0;///-1;
				}

				f.mask = ByteBuffer.Wrap( m ).GetInt32();
				f.compare = ByteBuffer.Wrap( b ).GetInt32();
			}
			catch ( Exception e )
			{
				GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Bad filter address: " + s + "\\n" );
				return false;
			}

			return true;
		}

		public static Boolean SV_FilterPacket( String from )
		{
			Int32 i;
			Int32 in_renamed;
			Int32[] m = new[] { 0, 0, 0, 0 };
			var p = 0;
			Char c;
			i = 0;
			while ( p < from.Length && i < 4 )
			{
				m[i] = 0;
				c = from[p];
				while ( c >= '0' && c <= '9' )
				{
					m[i] = m[i] * 10 + ( c - '0' );
					c = from[p++];
				}

				if ( p == from.Length || c == ':' )
					break;
				i++;
				p++;
			}

			in_renamed = ( m[0] & 0xff ) | ( ( m[1] & 0xff ) << 8 ) | ( ( m[2] & 0xff ) << 16 ) | ( ( m[3] & 0xff ) << 24 );
			for ( i = 0; i < numipfilters; i++ )
				if ( ( in_renamed & ipfilters[i].mask ) == ipfilters[i].compare )
					return ( ( Int32 ) GameBase.filterban.value ) != 0;
			return ( ( Int32 ) 1 - GameBase.filterban.value ) != 0;
		}

		static void SVCmd_AddIP_f( )
		{
			Int32 i;
			if ( GameBase.gi.Argc() < 3 )
			{
				GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Usage:  addip <ip-mask>\\n" );
				return;
			}

			for ( i = 0; i < numipfilters; i++ )
				if ( ipfilters[i].compare == 0xffffffff )
					break;
			if ( i == numipfilters )
			{
				if ( numipfilters == MAX_IPFILTERS )
				{
					GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "IP filter list is full\\n" );
					return;
				}

				numipfilters++;
			}

			if ( !StringToFilter( GameBase.gi.Argv( 2 ), ipfilters[i] ) )
				ipfilters[i].compare = unchecked(( Int32 ) 0xffffffff);
		}

		static void SVCmd_RemoveIP_f( )
		{
			GameSVCmds.ipfilter_t f = new ipfilter_t();
			Int32 i, j;
			if ( GameBase.gi.Argc() < 3 )
			{
				GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Usage:  sv removeip <ip-mask>\\n" );
				return;
			}

			if ( !StringToFilter( GameBase.gi.Argv( 2 ), f ) )
				return;
			for ( i = 0; i < numipfilters; i++ )
				if ( ipfilters[i].mask == f.mask && ipfilters[i].compare == f.compare )
				{
					for ( j = i + 1; j < numipfilters; j++ )
						ipfilters[j - 1] = ipfilters[j];
					numipfilters--;
					GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Removed.\\n" );
					return;
				}

			GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Didn't find " + GameBase.gi.Argv( 2 ) + ".\\n" );
		}

		static void SVCmd_ListIP_f( )
		{
			Int32 i;
			Byte[] b;
			GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Filter list:\\n" );
			for ( i = 0; i < numipfilters; i++ )
			{
				b = Lib.GetIntBytes( ipfilters[i].compare );
				GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, ( b[0] & 0xff ) + "." + ( b[1] & 0xff ) + "." + ( b[2] & 0xff ) + "." + ( b[3] & 0xff ) );
			}
		}

		static void SVCmd_WriteIP_f( )
		{
			QuakeFile f;
			String name;
			Byte[] b;
			Int32 i;
			cvar_t game;
			game = GameBase.gi.Cvar_f( "game", "", 0 );
			if ( game.string_renamed == null )
				name = Defines.GAMEVERSION + "/listip.cfg";
			else
				name = game.string_renamed + "/listip.cfg";
			GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Writing " + name + ".\\n" );
			f = new QuakeFile( name, FileAccess.ReadWrite );//Lib.Fopen(name, "rw");
			if ( f == null )
			{
				GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Couldn't open " + name + "\\n" );
				return;
			}

			try
			{
				f.Write( "set filterban " + ( Int32 ) GameBase.filterban.value + "\\n" );
				for ( i = 0; i < numipfilters; i++ )
				{
					b = Lib.GetIntBytes( ipfilters[i].compare );
					f.Write( "sv addip " + ( b[0] & 0xff ) + "." + ( b[1] & 0xff ) + "." + ( b[2] & 0xff ) + "." + ( b[3] & 0xff ) + "\\n" );
				}
			}
			catch ( IOException e )
			{
				Com.Printf( "IOError in SVCmd_WriteIP_f:" + e );
			}

			f.Dispose();
		}

		public static void ServerCommand( )
		{
			String cmd;
			cmd = GameBase.gi.Argv( 1 );
			if ( Lib.Q_stricmp( cmd, "test" ) == 0 )
				Svcmd_Test_f();
			else if ( Lib.Q_stricmp( cmd, "addip" ) == 0 )
				SVCmd_AddIP_f();
			else if ( Lib.Q_stricmp( cmd, "removeip" ) == 0 )
				SVCmd_RemoveIP_f();
			else if ( Lib.Q_stricmp( cmd, "listip" ) == 0 )
				SVCmd_ListIP_f();
			else if ( Lib.Q_stricmp( cmd, "writeip" ) == 0 )
				SVCmd_WriteIP_f();
			else
				GameBase.gi.Cprintf( null, Defines.PRINT_HIGH, "Unknown server command \\\"" + cmd + "\\\"\\n" );
		}
	}
}
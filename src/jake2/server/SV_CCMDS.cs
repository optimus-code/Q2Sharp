using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Sys;
using Jake2.Util;
using System;
using System.IO;

namespace Jake2.Server
{
	public class SV_CCMDS
	{
		public static void SV_SetMaster_f( )
		{
			Int32 i, slot;
			if ( Globals.dedicated.value == 0 )
			{
				Com.Printf( "Only dedicated servers use masters.\\n" );
				return;
			}

			Cvar.Set( "public", "1" );
			for ( i = 1; i < Defines.MAX_MASTERS; i++ )
				SV_MAIN.master_adr[i] = new netadr_t();
			slot = 1;
			for ( i = 1; i < Cmd.Argc(); i++ )
			{
				if ( slot == Defines.MAX_MASTERS )
					break;
				if ( !NET.StringToAdr( Cmd.Argv( i ), SV_MAIN.master_adr[i] ) )
				{
					Com.Printf( "Bad address: " + Cmd.Argv( i ) + "\\n" );
					continue;
				}

				if ( SV_MAIN.master_adr[slot].port == 0 )
					SV_MAIN.master_adr[slot].port = Defines.PORT_MASTER;
				Com.Printf( "Master server at " + NET.AdrToString( SV_MAIN.master_adr[slot] ) + "\\n" );
				Com.Printf( "Sending a ping.\\n" );
				Netchan.OutOfBandPrint( Defines.NS_SERVER, SV_MAIN.master_adr[slot], "ping" );
				slot++;
			}

			SV_INIT.svs.last_heartbeat = -9999999;
		}

		public static Boolean SV_SetPlayer( )
		{
			client_t cl;
			Int32 i;
			Int32 idnum;
			String s;
			if ( Cmd.Argc() < 2 )
				return false;
			s = Cmd.Argv( 1 );
			if ( s[0] >= '0' && s[0] <= '9' )
			{
				idnum = Lib.Atoi( Cmd.Argv( 1 ) );
				if ( idnum < 0 || idnum >= SV_MAIN.maxclients.value )
				{
					Com.Printf( "Bad client slot: " + idnum + "\\n" );
					return false;
				}

				SV_MAIN.sv_client = SV_INIT.svs.clients[idnum];
				SV_USER.sv_player = SV_MAIN.sv_client.edict;
				if ( 0 == SV_MAIN.sv_client.state )
				{
					Com.Printf( "Client " + idnum + " is not active\\n" );
					return false;
				}

				return true;
			}

			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( 0 == cl.state )
					continue;
				if ( 0 == Lib.Strcmp( cl.name, s ) )
				{
					SV_MAIN.sv_client = cl;
					SV_USER.sv_player = SV_MAIN.sv_client.edict;
					return true;
				}
			}

			Com.Printf( "Userid " + s + " is not on the server\\n" );
			return false;
		}

		public static void Remove( String name )
		{
			try
			{
				File.Delete( name );
			}
			catch ( Exception e )
			{
			}
		}

		public static void SV_WipeSavegame( String savename )
		{
			Com.DPrintf( "SV_WipeSaveGame(" + savename + ")\\n" );
			var name = FS.Gamedir() + "/save/" + savename + "/server.ssv";
			Remove( name );
			name = FS.Gamedir() + "/save/" + savename + "/game.ssv";
			Remove( name );
			name = FS.Gamedir() + "/save/" + savename + "/*.sav";
			var f = CoreSys.FindFirst( name );
			while ( f != null )
			{
				File.Delete( f );
				f = CoreSys.FindNext();
			}

			CoreSys.FindClose();
			name = FS.Gamedir() + "/save/" + savename + "/*.sv2";
			f = CoreSys.FindFirst( name );
			while ( f != null )
			{
				File.Delete( f );
				f = CoreSys.FindNext();
			}

			CoreSys.FindClose();
		}

		public static void CopyFile( String src, String dst )
		{
			try
			{
				File.Copy( src, dst );
			}
			catch ( Exception e )
			{
				e.PrintStackTrace();
			}
		}

		public static void SV_CopySaveGame( String src, String dst )
		{
			Com.DPrintf( "SV_CopySaveGame(" + src + "," + dst + ")\\n" );
			SV_WipeSavegame( dst );
			var name = FS.Gamedir() + "/save/" + src + "/server.ssv";
			var name2 = FS.Gamedir() + "/save/" + dst + "/server.ssv";
			FS.CreatePath( name2 );
			CopyFile( name, name2 );
			name = FS.Gamedir() + "/save/" + src + "/game.ssv";
			name2 = FS.Gamedir() + "/save/" + dst + "/game.ssv";
			CopyFile( name, name2 );
			var name1 = FS.Gamedir() + "/save/" + src + "/";
			name = FS.Gamedir() + "/save/" + src + "/*.sav";
			var found = CoreSys.FindFirst( name );
			while ( found != null )
			{
				name = name1 + found;
				name2 = FS.Gamedir() + "/save/" + dst + "/" + found;
				CopyFile( name, name2 );
				name = name.Substring( 0, name.Length - 3 ) + "sv2";
				name2 = name2.Substring( 0, name2.Length - 3 ) + "sv2";
				CopyFile( name, name2 );
				found = CoreSys.FindNext();
			}

			CoreSys.FindClose();
		}

		public static void SV_WriteLevelFile( )
		{
			String name;
			QuakeFile f;
			Com.DPrintf( "SV_WriteLevelFile()\\n" );
			name = FS.Gamedir() + "/save/current/" + SV_INIT.sv.name + ".sv2";
			try
			{
				f = new QuakeFile( name, FileAccess.ReadWrite );
				for ( var i = 0; i < Defines.MAX_CONFIGSTRINGS; i++ )
					f.Write( SV_INIT.sv.configstrings[i] );
				CM.CM_WritePortalState( f );
				f.Dispose();
			}
			catch ( Exception e )
			{
				Com.Printf( "Failed to open " + name + "\\n" );
				e.PrintStackTrace();
			}

			name = FS.Gamedir() + "/save/current/" + SV_INIT.sv.name + ".sav";
			GameSave.WriteLevel( name );
		}

		public static void SV_ReadLevelFile( )
		{
			String name;
			QuakeFile f;
			Com.DPrintf( "SV_ReadLevelFile()\\n" );
			name = FS.Gamedir() + "/save/current/" + SV_INIT.sv.name + ".sv2";
			try
			{
				f = new QuakeFile( name, FileAccess.Read );
				for ( var n = 0; n < Defines.MAX_CONFIGSTRINGS; n++ )
					SV_INIT.sv.configstrings[n] = f.ReadString();
				CM.CM_ReadPortalState( f );
				f.Close();
			}
			catch ( IOException e1 )
			{
				Com.Printf( "Failed to open " + name + "\\n" );
				e1.PrintStackTrace();
			}

			name = FS.Gamedir() + "/save/current/" + SV_INIT.sv.name + ".sav";
			GameSave.ReadLevel( name );
		}

		public static void SV_WriteServerFile( Boolean autosave )
		{
			QuakeFile f;
			cvar_t var;
			String filename, name, string_renamed, comment;
			Com.DPrintf( "SV_WriteServerFile(" + ( autosave ? "true" : "false" ) + ")\\n" );
			filename = FS.Gamedir() + "/save/current/server.ssv";
			try
			{
				f = new QuakeFile( filename, FileAccess.ReadWrite );
				if ( !autosave )
				{
					var now = DateTime.Now;
					comment = Com.Sprintf( "%2i:%2i %2i/%2i  ", now.Hour, now.Minute, now.Month + 1, now.Day );
					comment += SV_INIT.sv.configstrings[Defines.CS_NAME];
				}
				else
				{
					comment = "ENTERING " + SV_INIT.sv.configstrings[Defines.CS_NAME];
				}

				f.Write( comment );
				f.Write( SV_INIT.svs.mapcmd );
				for ( var = Globals.cvar_vars; var != null; var = var.next )
				{
					if ( 0 == ( var.flags & Defines.CVAR_LATCH ) )
						continue;
					if ( var.name.Length >= Defines.MAX_OSPATH - 1 || var.string_renamed.Length >= 128 - 1 )
					{
						Com.Printf( "Cvar too long: " + var.name + " = " + var.string_renamed + "\\n" );
						continue;
					}

					name = var.name;
					string_renamed = var.string_renamed;
					try
					{
						f.Write( name );
						f.Write( string_renamed );
					}
					catch ( IOException e2 )
					{
					}
				}

				f.Write( ( String ) null );
				f.Dispose();
			}
			catch ( Exception e )
			{
				Com.Printf( "Couldn't write " + filename + "\\n" );
			}

			filename = FS.Gamedir() + "/save/current/game.ssv";
			GameSave.WriteGame( filename, autosave );
		}

		public static void SV_ReadServerFile( )
		{
			String filename = "", name = "", string_renamed, mapcmd;
			try
			{
				QuakeFile f;
				mapcmd = "";
				Com.DPrintf( "SV_ReadServerFile()\\n" );
				filename = FS.Gamedir() + "/save/current/server.ssv";
				f = new QuakeFile( filename, FileAccess.Read );
				f.ReadString();
				mapcmd = f.ReadString();
				while ( true )
				{
					name = f.ReadString();
					if ( name == null )
						break;
					string_renamed = f.ReadString();
					Com.DPrintf( "Set " + name + " = " + string_renamed + "\\n" );
					Cvar.ForceSet( name, string_renamed );
				}

				f.Close();
				SV_INIT.SV_InitGame();
				SV_INIT.svs.mapcmd = mapcmd;
				filename = FS.Gamedir() + "/save/current/game.ssv";
				GameSave.ReadGame( filename );
			}
			catch ( Exception e )
			{
				Com.Printf( "Couldn't read file " + filename + "\\n" );
				e.PrintStackTrace();
			}
		}

		public static void SV_DemoMap_f( )
		{
			SV_INIT.SV_Map( true, Cmd.Argv( 1 ), false );
		}

		public static void SV_GameMap_f( )
		{
			if ( Cmd.Argc() != 2 )
			{
				Com.Printf( "USAGE: gamemap <map>\\n" );
				return;
			}

			Com.DPrintf( "SV_GameMap(" + Cmd.Argv( 1 ) + ")\\n" );
			FS.CreatePath( FS.Gamedir() + "/save/current/" );
			var map = Cmd.Argv( 1 );
			if ( map[0] == '*' )
			{
				SV_WipeSavegame( "current" );
			}
			else
			{
				if ( SV_INIT.sv.state == Defines.ss_game )
				{
					client_t cl;
					Boolean[] savedInuse = new Boolean[( Int32 ) SV_MAIN.maxclients.value];
					for ( var i = 0; i < SV_MAIN.maxclients.value; i++ )
					{
						cl = SV_INIT.svs.clients[i];
						savedInuse[i] = cl.edict.inuse;
						cl.edict.inuse = false;
					}

					SV_WriteLevelFile();
					for ( var i = 0; i < SV_MAIN.maxclients.value; i++ )
					{
						cl = SV_INIT.svs.clients[i];
						cl.edict.inuse = savedInuse[i];
					}

					savedInuse = null;
				}
			}

			SV_INIT.SV_Map( false, Cmd.Argv( 1 ), false );
			SV_INIT.svs.mapcmd = Cmd.Argv( 1 );
			if ( 0 == Globals.dedicated.value )
			{
				SV_WriteServerFile( true );
				SV_CopySaveGame( "current", "save0" );
			}
		}

		public static void SV_Map_f( )
		{
			String map;
			String expanded;
			map = Cmd.Argv( 1 );
			if ( map.IndexOf( "." ) < 0 )
			{
				expanded = "maps/" + map + ".bsp";
				if ( FS.LoadFile( expanded ) == null )
				{
					Com.Printf( "Can't find " + expanded + "\\n" );
					return;
				}
			}

			SV_INIT.sv.state = Defines.ss_dead;
			SV_WipeSavegame( "current" );
			SV_GameMap_f();
		}

		public static void SV_Loadgame_f( )
		{
			if ( Cmd.Argc() != 2 )
			{
				Com.Printf( "USAGE: loadgame <directory>\\n" );
				return;
			}

			Com.Printf( "Loading game...\\n" );
			var dir = Cmd.Argv( 1 );
			if ( ( dir.IndexOf( ".." ) > -1 ) || ( dir.IndexOf( "/" ) > -1 ) || ( dir.IndexOf( "\\\\" ) > -1 ) )
			{
				Com.Printf( "Bad savedir.\\n" );
			}

			var name = FS.Gamedir() + "/save/" + Cmd.Argv( 1 ) + "/server.ssv";
			QuakeFile f;
			try
			{
				f = new QuakeFile( name, FileAccess.Read );
			}
			catch ( FileNotFoundException e )
			{
				Com.Printf( "No such savegame: " + name + "\\n" );
				return;
			}

			try
			{
				f.Dispose();
			}
			catch ( IOException e1 )
			{
				e1.PrintStackTrace();
			}

			SV_CopySaveGame( Cmd.Argv( 1 ), "current" );
			SV_ReadServerFile();
			SV_INIT.sv.state = Defines.ss_dead;
			SV_INIT.SV_Map( false, SV_INIT.svs.mapcmd, true );
		}

		public static void SV_Savegame_f( )
		{
			String dir;
			if ( SV_INIT.sv.state != Defines.ss_game )
			{
				Com.Printf( "You must be in a game to save.\\n" );
				return;
			}

			if ( Cmd.Argc() != 2 )
			{
				Com.Printf( "USAGE: savegame <directory>\\n" );
				return;
			}

			if ( Cvar.VariableValue( "deathmatch" ) != 0 )
			{
				Com.Printf( "Can't savegame in a deathmatch\\n" );
				return;
			}

			if ( 0 == Lib.Strcmp( Cmd.Argv( 1 ), "current" ) )
			{
				Com.Printf( "Can't save to 'current'\\n" );
				return;
			}

			if ( SV_MAIN.maxclients.value == 1 && SV_INIT.svs.clients[0].edict.client.ps.stats[Defines.STAT_HEALTH] <= 0 )
			{
				Com.Printf( "\\nCan't savegame while dead!\\n" );
				return;
			}

			dir = Cmd.Argv( 1 );
			if ( ( dir.IndexOf( ".." ) > -1 ) || ( dir.IndexOf( "/" ) > -1 ) || ( dir.IndexOf( "\\\\" ) > -1 ) )
			{
				Com.Printf( "Bad savedir.\\n" );
			}

			Com.Printf( "Saving game...\\n" );
			SV_WriteLevelFile();
			try
			{
				SV_WriteServerFile( false );
			}
			catch ( Exception e )
			{
				Com.Printf( "IOError in SV_WriteServerFile: " + e );
			}

			SV_CopySaveGame( "current", dir );
			Com.Printf( "Done.\\n" );
		}

		public static void SV_Kick_f( )
		{
			if ( !SV_INIT.svs.initialized )
			{
				Com.Printf( "No server running.\\n" );
				return;
			}

			if ( Cmd.Argc() != 2 )
			{
				Com.Printf( "Usage: kick <userid>\\n" );
				return;
			}

			if ( !SV_SetPlayer() )
				return;
			SV_SEND.SV_BroadcastPrintf( Defines.PRINT_HIGH, SV_MAIN.sv_client.name + " was kicked\\n" );
			SV_SEND.SV_ClientPrintf( SV_MAIN.sv_client, Defines.PRINT_HIGH, "You were kicked from the game\\n" );
			SV_MAIN.SV_DropClient( SV_MAIN.sv_client );
			SV_MAIN.sv_client.lastmessage = SV_INIT.svs.realtime;
		}

		public static void SV_Status_f( )
		{
			Int32 i, j, l;
			client_t cl;
			String s;
			Int32 ping;
			if ( SV_INIT.svs.clients == null )
			{
				Com.Printf( "No server running.\\n" );
				return;
			}

			Com.Printf( "map              : " + SV_INIT.sv.name + "\\n" );
			Com.Printf( "num score ping name            lastmsg address               qport \\n" );
			Com.Printf( "--- ----- ---- --------------- ------- --------------------- ------\\n" );
			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( 0 == cl.state )
					continue;
				Com.Printf( "%3i ", i );
				Com.Printf( "%5i ", cl.edict.client.ps.stats[Defines.STAT_FRAGS] );
				if ( cl.state == Defines.cs_connected )
					Com.Printf( "CNCT " );
				else if ( cl.state == Defines.cs_zombie )
					Com.Printf( "ZMBI " );
				else
				{
					ping = cl.ping < 9999 ? cl.ping : 9999;
					Com.Printf( "%4i ", ping );
				}

				Com.Printf( "%s", cl.name );
				l = 16 - cl.name.Length;
				for ( j = 0; j < l; j++ )
					Com.Printf( " " );
				Com.Printf( "%7i ", SV_INIT.svs.realtime - cl.lastmessage );
				s = NET.AdrToString( cl.netchan.remote_address );
				Com.Printf( s );
				l = 22 - s.Length;
				for ( j = 0; j < l; j++ )
					Com.Printf( " " );
				Com.Printf( "%5i", cl.netchan.qport );
				Com.Printf( "\\n" );
			}

			Com.Printf( "\\n" );
		}

		public static void SV_ConSay_f( )
		{
			client_t client;
			Int32 j;
			String p;
			String text;
			if ( Cmd.Argc() < 2 )
				return;
			text = "console: ";
			p = Cmd.Args();
			if ( p[0] == '"' )
			{
				p = p.Substring( 1, p.Length - 1 );
			}

			text += p;
			for ( j = 0; j < SV_MAIN.maxclients.value; j++ )
			{
				client = SV_INIT.svs.clients[j];
				if ( client.state != Defines.cs_spawned )
					continue;
				SV_SEND.SV_ClientPrintf( client, Defines.PRINT_CHAT, text + "\\n" );
			}
		}

		public static void SV_Heartbeat_f( )
		{
			SV_INIT.svs.last_heartbeat = -9999999;
		}

		public static void SV_Serverinfo_f( )
		{
			Com.Printf( "Server info settings:\\n" );
			Info.Print( Cvar.Serverinfo() );
		}

		public static void SV_DumpUser_f( )
		{
			if ( Cmd.Argc() != 2 )
			{
				Com.Printf( "Usage: info <userid>\\n" );
				return;
			}

			if ( !SV_SetPlayer() )
				return;
			Com.Printf( "userinfo\\n" );
			Com.Printf( "--------\\n" );
			Info.Print( SV_MAIN.sv_client.userinfo );
		}

		public static void SV_ServerRecord_f( )
		{
			String name;
			Byte[] buf_data = new Byte[32768];
			sizebuf_t buf = new sizebuf_t();
			Int32 len;
			Int32 i;
			if ( Cmd.Argc() != 2 )
			{
				Com.Printf( "serverrecord <demoname>\\n" );
				return;
			}

			if ( SV_INIT.svs.demofile != null )
			{
				Com.Printf( "Already recording.\\n" );
				return;
			}

			if ( SV_INIT.sv.state != Defines.ss_game )
			{
				Com.Printf( "You must be in a level to record.\\n" );
				return;
			}

			name = FS.Gamedir() + "/demos/" + Cmd.Argv( 1 ) + ".dm2";
			Com.Printf( "recording to " + name + ".\\n" );
			FS.CreatePath( name );
			try
			{
				SV_INIT.svs.demofile = new QuakeFile( name, FileAccess.ReadWrite );
			}
			catch ( Exception e )
			{
				Com.Printf( "ERROR: couldn't open.\\n" );
				return;
			}

			SZ.Init( SV_INIT.svs.demo_multicast, SV_INIT.svs.demo_multicast_buf, SV_INIT.svs.demo_multicast_buf.Length );
			SZ.Init( buf, buf_data, buf_data.Length );
			MSG.WriteByte( buf, Defines.svc_serverdata );
			MSG.WriteLong( buf, Defines.PROTOCOL_VERSION );
			MSG.WriteLong( buf, SV_INIT.svs.spawncount );
			MSG.WriteByte( buf, 2 );
			MSG.WriteString( buf, Cvar.VariableString( "gamedir" ) );
			MSG.WriteShort( buf, -1 );
			MSG.WriteString( buf, SV_INIT.sv.configstrings[Defines.CS_NAME] );
			for ( i = 0; i < Defines.MAX_CONFIGSTRINGS; i++ )
				if ( SV_INIT.sv.configstrings[i].Length == 0 )
				{
					MSG.WriteByte( buf, Defines.svc_configstring );
					MSG.WriteShort( buf, i );
					MSG.WriteString( buf, SV_INIT.sv.configstrings[i] );
				}

			Com.DPrintf( "signon message length: " + buf.cursize + "\\n" );
			len = EndianHandler.SwapInt( buf.cursize );
			try
			{
				SV_INIT.svs.demofile.Write( len );
				SV_INIT.svs.demofile.Write( buf.data, 0, buf.cursize );
			}
			catch ( IOException e1 )
			{
				e1.PrintStackTrace();
			}
		}

		public static void SV_ServerStop_f( )
		{
			if ( SV_INIT.svs.demofile == null )
			{
				Com.Printf( "Not doing a serverrecord.\\n" );
				return;
			}

			try
			{
				SV_INIT.svs.demofile.Close();
			}
			catch ( IOException e )
			{
				e.PrintStackTrace();
			}

			SV_INIT.svs.demofile = null;
			Com.Printf( "Recording completed.\\n" );
		}

		public static void SV_KillServer_f( )
		{
			if ( !SV_INIT.svs.initialized )
				return;
			SV_MAIN.SV_Shutdown( "Server was killed.\\n", false );
			NET.Config( false );
		}

		public static void SV_ServerCommand_f( )
		{
			GameSVCmds.ServerCommand();
		}

		public static void SV_InitOperatorCommands( )
		{
			Cmd.AddCommand( "heartbeat", new Anonymousxcommand_t() );
			Cmd.AddCommand( "kick", new Anonymousxcommand_t1() );
			Cmd.AddCommand( "status", new Anonymousxcommand_t2() );
			Cmd.AddCommand( "serverinfo", new Anonymousxcommand_t3() );
			Cmd.AddCommand( "dumpuser", new Anonymousxcommand_t4() );
			Cmd.AddCommand( "map", new Anonymousxcommand_t5() );
			Cmd.AddCommand( "demomap", new Anonymousxcommand_t6() );
			Cmd.AddCommand( "gamemap", new Anonymousxcommand_t7() );
			Cmd.AddCommand( "setmaster", new Anonymousxcommand_t8() );
			if ( Globals.dedicated.value != 0 )
				Cmd.AddCommand( "say", new Anonymousxcommand_t9() );
			Cmd.AddCommand( "serverrecord", new Anonymousxcommand_t10() );
			Cmd.AddCommand( "serverstop", new Anonymousxcommand_t11() );
			Cmd.AddCommand( "save", new Anonymousxcommand_t12() );
			Cmd.AddCommand( "load", new Anonymousxcommand_t13() );
			Cmd.AddCommand( "killserver", new Anonymousxcommand_t14() );
			Cmd.AddCommand( "sv", new Anonymousxcommand_t15() );
		}

		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				SV_Heartbeat_f();
			}
		}

		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				SV_Kick_f();
			}
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				SV_Status_f();
			}
		}

		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public override void Execute( )
			{
				SV_Serverinfo_f();
			}
		}

		private sealed class Anonymousxcommand_t4 : xcommand_t
		{
			public override void Execute( )
			{
				SV_DumpUser_f();
			}
		}

		private sealed class Anonymousxcommand_t5 : xcommand_t
		{
			public override void Execute( )
			{
				SV_Map_f();
			}
		}

		private sealed class Anonymousxcommand_t6 : xcommand_t
		{
			public override void Execute( )
			{
				SV_DemoMap_f();
			}
		}

		private sealed class Anonymousxcommand_t7 : xcommand_t
		{
			public override void Execute( )
			{
				SV_GameMap_f();
			}
		}

		private sealed class Anonymousxcommand_t8 : xcommand_t
		{
			public override void Execute( )
			{
				SV_SetMaster_f();
			}
		}

		private sealed class Anonymousxcommand_t9 : xcommand_t
		{
			public override void Execute( )
			{
				SV_ConSay_f();
			}
		}

		private sealed class Anonymousxcommand_t10 : xcommand_t
		{
			public override void Execute( )
			{
				SV_ServerRecord_f();
			}
		}

		private sealed class Anonymousxcommand_t11 : xcommand_t
		{
			public override void Execute( )
			{
				SV_ServerStop_f();
			}
		}

		private sealed class Anonymousxcommand_t12 : xcommand_t
		{
			public override void Execute( )
			{
				SV_Savegame_f();
			}
		}

		private sealed class Anonymousxcommand_t13 : xcommand_t
		{
			public override void Execute( )
			{
				SV_Loadgame_f();
			}
		}

		private sealed class Anonymousxcommand_t14 : xcommand_t
		{
			public override void Execute( )
			{
				SV_KillServer_f();
			}
		}

		private sealed class Anonymousxcommand_t15 : xcommand_t
		{
			public override void Execute( )
			{
				SV_ServerCommand_f();
			}
		}
	}
}
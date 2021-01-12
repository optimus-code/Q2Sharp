using Jake2.Client;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Sys;
using Jake2.Util;
using System;
using System.IO;

namespace Jake2.Server
{
	public class SV_INIT
	{
		public static Int32 SV_FindIndex( String name, Int32 start, Int32 max, Boolean create )
		{
			Int32 i;
			if ( name == null || name.Length == 0 )
				return 0;
			for ( i = 1; i < max && sv.configstrings[start + i] != null; i++ )
				if ( 0 == Lib.Strcmp( sv.configstrings[start + i], name ) )
					return i;
			if ( !create )
				return 0;
			if ( i == max )
				Com.Error( Defines.ERR_DROP, "*Index: overflow" );
			sv.configstrings[start + i] = name;
			if ( sv.state != Defines.ss_loading )
			{
				SZ.Clear( sv.multicast );
				MSG.WriteChar( sv.multicast, Defines.svc_configstring );
				MSG.WriteShort( sv.multicast, start + i );
				MSG.WriteString( sv.multicast, name );
				SV_SEND.SV_Multicast( Globals.vec3_origin, Defines.MULTICAST_ALL_R );
			}

			return i;
		}

		public static Int32 SV_ModelIndex( String name )
		{
			return SV_FindIndex( name, Defines.CS_MODELS, Defines.MAX_MODELS, true );
		}

		public static Int32 SV_SoundIndex( String name )
		{
			return SV_FindIndex( name, Defines.CS_SOUNDS, Defines.MAX_SOUNDS, true );
		}

		public static Int32 SV_ImageIndex( String name )
		{
			return SV_FindIndex( name, Defines.CS_IMAGES, Defines.MAX_IMAGES, true );
		}

		public static void SV_CreateBaseline( )
		{
			edict_t svent;
			Int32 entnum;
			for ( entnum = 1; entnum < GameBase.num_edicts; entnum++ )
			{
				svent = GameBase.g_edicts[entnum];
				if ( !svent.inuse )
					continue;
				if ( 0 == svent.s.modelindex && 0 == svent.s.sound && 0 == svent.s.effects )
					continue;
				svent.s.number = entnum;
				Math3D.VectorCopy( svent.s.origin, svent.s.old_origin );
				sv.baselines[entnum].Set( svent.s );
			}
		}

		public static void SV_CheckForSavegame( )
		{
			String name;
			FileStream f;
			Int32 i;
			if ( SV_MAIN.sv_noreload.value != 0 )
				return;
			if ( Cvar.VariableValue( "deathmatch" ) != 0 )
				return;
			name = FS.Gamedir() + "/save/current/" + sv.name + ".sav";
			try
			{
				f = File.OpenRead( name );
			}
			catch ( Exception e )
			{
				return;
			}

			try
			{
				f.Close();
			}
			catch ( Exception e1 )
			{
				e1.PrintStackTrace();
			}

			SV_WORLD.SV_ClearWorld();
			SV_CCMDS.SV_ReadLevelFile();
			if ( !sv.loadgame )
			{
				Int32 previousState;
				previousState = sv.state;
				sv.state = Defines.ss_loading;
				for ( i = 0; i < 100; i++ )
					GameBase.G_RunFrame();
				sv.state = previousState;
			}
		}

		public static void SV_SpawnServer( String server, String spawnpoint, Int32 serverstate, Boolean attractloop, Boolean loadgame )
		{
			Int32 i;
			var checksum = 0;
			if ( attractloop )
				Cvar.Set( "paused", "0" );
			Com.Printf( "------- Server Initialization -------\\n" );
			Com.DPrintf( "SpawnServer: " + server + "\\n" );
			if ( sv.demofile != null )
				try
				{
					sv.demofile.Close();
				}
				catch ( Exception e )
				{
				}

			svs.spawncount++;
			sv.state = Defines.ss_dead;
			Globals.server_state = sv.state;
			sv = new server_t();
			svs.realtime = 0;
			sv.loadgame = loadgame;
			sv.attractloop = attractloop;
			sv.configstrings[Defines.CS_NAME] = server;
			if ( Cvar.VariableValue( "deathmatch" ) != 0 )
			{
				sv.configstrings[Defines.CS_AIRACCEL] = "" + SV_MAIN.sv_airaccelerate.value;
				PMove.pm_airaccelerate = SV_MAIN.sv_airaccelerate.value;
			}
			else
			{
				sv.configstrings[Defines.CS_AIRACCEL] = "0";
				PMove.pm_airaccelerate = 0;
			}

			SZ.Init( sv.multicast, sv.multicast_buf, sv.multicast_buf.Length );
			sv.name = server;
			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				if ( svs.clients[i].state > Defines.cs_connected )
					svs.clients[i].state = Defines.cs_connected;
				svs.clients[i].lastframe = -1;
			}

			sv.time = 1000;
			sv.name = server;
			sv.configstrings[Defines.CS_NAME] = server;
			Int32[] iw = new[] { checksum };
			if ( serverstate != Defines.ss_game )
			{
				sv.models[1] = CM.CM_LoadMap( "", false, iw );
			}
			else
			{
				sv.configstrings[Defines.CS_MODELS + 1] = "maps/" + server + ".bsp";
				sv.models[1] = CM.CM_LoadMap( sv.configstrings[Defines.CS_MODELS + 1], false, iw );
			}

			checksum = iw[0];
			sv.configstrings[Defines.CS_MAPCHECKSUM] = "" + checksum;
			SV_WORLD.SV_ClearWorld();
			for ( i = 1; i < CM.CM_NumInlineModels(); i++ )
			{
				sv.configstrings[Defines.CS_MODELS + 1 + i] = "*" + i;
				sv.models[i + 1] = CM.InlineModel( sv.configstrings[Defines.CS_MODELS + 1 + i] );
			}

			sv.state = Defines.ss_loading;
			Globals.server_state = sv.state;
			GameSpawn.SpawnEntities( sv.name, CM.CM_EntityString(), spawnpoint );
			GameBase.G_RunFrame();
			GameBase.G_RunFrame();
			sv.state = serverstate;
			Globals.server_state = sv.state;
			SV_CreateBaseline();
			SV_CheckForSavegame();
			Cvar.FullSet( "mapname", sv.name, Defines.CVAR_SERVERINFO | Defines.CVAR_NOSET );
		}

		public static void SV_InitGame( )
		{
			Int32 i;
			edict_t ent;
			String idmaster;
			if ( svs.initialized )
			{
				SV_MAIN.SV_Shutdown( "Server restarted\\n", true );
			}
			else
			{
				CL.Drop();
				SCR.BeginLoadingPlaque();
			}

			Cvar.GetLatchedVars();
			svs.initialized = true;
			if ( Cvar.VariableValue( "coop" ) != 0 && Cvar.VariableValue( "deathmatch" ) != 0 )
			{
				Com.Printf( "Deathmatch and Coop both set, disabling Coop\\n" );
				Cvar.FullSet( "coop", "0", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			}

			if ( Globals.dedicated.value != 0 )
			{
				if ( 0 == Cvar.VariableValue( "coop" ) )
					Cvar.FullSet( "deathmatch", "1", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			}

			if ( Cvar.VariableValue( "deathmatch" ) != 0 )
			{
				if ( SV_MAIN.maxclients.value <= 1 )
					Cvar.FullSet( "maxclients", "8", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
				else if ( SV_MAIN.maxclients.value > Defines.MAX_CLIENTS )
					Cvar.FullSet( "maxclients", "" + Defines.MAX_CLIENTS, Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			}
			else if ( Cvar.VariableValue( "coop" ) != 0 )
			{
				if ( SV_MAIN.maxclients.value <= 1 || SV_MAIN.maxclients.value > 4 )
					Cvar.FullSet( "maxclients", "4", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			}
			else
			{
				Cvar.FullSet( "maxclients", "1", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			}

			svs.spawncount = Lib.Rand();
			svs.clients = new client_t[( Int32 ) SV_MAIN.maxclients.value];
			for ( var n = 0; n < svs.clients.Length; n++ )
			{
				svs.clients[n] = new client_t();
				svs.clients[n].serverindex = n;
			}

			svs.num_client_entities = ( ( Int32 ) SV_MAIN.maxclients.value ) * Defines.UPDATE_BACKUP * 64;
			svs.client_entities = new entity_state_t[svs.num_client_entities];
			for ( var n = 0; n < svs.client_entities.Length; n++ )
				svs.client_entities[n] = new entity_state_t( null );
			NET.Config( ( SV_MAIN.maxclients.value > 1 ) );
			svs.last_heartbeat = -99999;
			idmaster = "192.246.40.37:" + Defines.PORT_MASTER;
			NET.StringToAdr( idmaster, SV_MAIN.master_adr[0] );
			SV_GAME.SV_InitGameProgs();
			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				ent = GameBase.g_edicts[i + 1];
				svs.clients[i].edict = ent;
				svs.clients[i].lastcmd = new usercmd_t();
			}
		}

		private static String firstmap = "";
		public static void SV_Map( Boolean attractloop, String levelstring, Boolean loadgame )
		{
			Int32 l;
			String level, ch, spawnpoint;
			var pos = 0;

			sv.loadgame = loadgame;
			sv.attractloop = attractloop;
			if ( sv.state == Defines.ss_dead && !sv.loadgame )
				SV_InitGame();
			level = levelstring;
			var c = level.IndexOf( '+' );

			if ( c != -1 )
			{
				Cvar.Set( "nextserver", "gamemap \\\"" + level.Substring( c + 1 ) + "\\\"" );
				level = level.Substring( 0, c );
			}
			else
			{
				Cvar.Set( "nextserver", "" );
			}

			if ( firstmap.Length == 0 )
			{
				if ( !levelstring.EndsWith( ".cin" ) && !levelstring.EndsWith( ".pcx" ) && !levelstring.EndsWith( ".dm2" ) )
				{
					pos = levelstring.IndexOf( '+' );
					firstmap = levelstring.Substring( pos + 1 );
				}
			}

			if ( Cvar.VariableValue( "coop" ) != 0 && level.Equals( "victory.pcx" ) )
				Cvar.Set( "nextserver", "gamemap \\\"*" + firstmap + "\\\"" );
			pos = level.IndexOf( '$' );
			if ( pos != -1 )
			{
				spawnpoint = level.Substring( pos + 1 );
				level = level.Substring( 0, pos );
			}
			else
				spawnpoint = "";
			if ( level[0] == '*' )
				level = level.Substring( 1 );
			l = level.Length;
			if ( l > 4 && level.EndsWith( ".cin" ) )
			{
				SCR.BeginLoadingPlaque();
				SV_SEND.SV_BroadcastCommand( "changing\\n" );
				SV_SpawnServer( level, spawnpoint, Defines.ss_cinematic, attractloop, loadgame );
			}
			else if ( l > 4 && level.EndsWith( ".dm2" ) )
			{
				SCR.BeginLoadingPlaque();
				SV_SEND.SV_BroadcastCommand( "changing\\n" );
				SV_SpawnServer( level, spawnpoint, Defines.ss_demo, attractloop, loadgame );
			}
			else if ( l > 4 && level.EndsWith( ".pcx" ) )
			{
				SCR.BeginLoadingPlaque();
				SV_SEND.SV_BroadcastCommand( "changing\\n" );
				SV_SpawnServer( level, spawnpoint, Defines.ss_pic, attractloop, loadgame );
			}
			else
			{
				SCR.BeginLoadingPlaque();
				SV_SEND.SV_BroadcastCommand( "changing\\n" );
				SV_SEND.SV_SendClientMessages();
				SV_SpawnServer( level, spawnpoint, Defines.ss_game, attractloop, loadgame );
				Cbuf.CopyToDefer();
			}

			SV_SEND.SV_BroadcastCommand( "reconnect\\n" );
		}

		public static server_static_t svs = new server_static_t();
		public static server_t sv = new server_t();
	}
}
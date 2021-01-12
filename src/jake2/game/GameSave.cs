using Jake2.Util;
using System;
using System.IO;

namespace Jake2.Game
{
	public class GameSave
	{
		public static void CreateEdicts( )
		{
			GameBase.g_edicts = new edict_t[GameBase.game.maxentities];
			for ( var i = 0; i < GameBase.game.maxentities; i++ )
				GameBase.g_edicts[i] = new edict_t( i );
			GameBase.g_edicts = GameBase.g_edicts;
		}

		public static void CreateClients( )
		{
			GameBase.game.clients = new gclient_t[GameBase.game.maxclients];
			for ( var i = 0; i < GameBase.game.maxclients; i++ )
				GameBase.game.clients[i] = new gclient_t( i );
		}

		private static String[] preloadclasslist = new[] { "jake2.game.PlayerWeapon", "jake2.game.AIAdapter", "jake2.game.Cmd", "jake2.game.EdictFindFilter", "jake2.game.EdictIterator", "jake2.game.EndianHandler", "jake2.game.EntBlockedAdapter", "jake2.game.EntDieAdapter", "jake2.game.EntDodgeAdapter", "jake2.game.EntInteractAdapter", "jake2.game.EntPainAdapter", "jake2.game.EntThinkAdapter", "jake2.game.EntTouchAdapter", "jake2.game.EntUseAdapter", "jake2.game.GameAI", "jake2.game.GameBase", "jake2.game.GameChase", "jake2.game.GameCombat", "jake2.game.GameFunc", "jake2.game.GameMisc", "jake2.game.GameSVCmds", "jake2.game.GameSave", "jake2.game.GameSpawn", "jake2.game.GameTarget", "jake2.game.GameTrigger", "jake2.game.GameTurret", "jake2.game.GameUtil", "jake2.game.GameWeapon", "jake2.game.Info", "jake2.game.ItemDropAdapter", "jake2.game.ItemUseAdapter", "jake2.game.Monster", "jake2.game.PlayerClient", "jake2.game.PlayerHud", "jake2.game.PlayerTrail", "jake2.game.PlayerView", "jake2.game.SuperAdapter", "jake2.game.monsters.M_Actor", "jake2.game.monsters.M_Berserk", "jake2.game.monsters.M_Boss2", "jake2.game.monsters.M_Boss3", "jake2.game.monsters.M_Boss31", "jake2.game.monsters.M_Boss32", "jake2.game.monsters.M_Brain", "jake2.game.monsters.M_Chick", "jake2.game.monsters.M_Flash", "jake2.game.monsters.M_Flipper", "jake2.game.monsters.M_Float", "jake2.game.monsters.M_Flyer", "jake2.game.monsters.M_Gladiator", "jake2.game.monsters.M_Gunner", "jake2.game.monsters.M_Hover", "jake2.game.monsters.M_Infantry", "jake2.game.monsters.M_Insane", "jake2.game.monsters.M_Medic", "jake2.game.monsters.M_Mutant", "jake2.game.monsters.M_Parasite", "jake2.game.monsters.M_Player", "jake2.game.monsters.M_Soldier", "jake2.game.monsters.M_Supertank", "jake2.game.monsters.M_Tank", "jake2.game.GameItems", "jake2.game.GameItemList" };
		public static void InitGame( )
		{
			GameBase.gi.Dprintf( "==== InitGame ====\\n" );
			// TODO - Do we need this? Everything is static
			//for (int n = 0; n < preloadclasslist.Length; n++)
			//{
			//    try
			//    {
			//        Class.ForName(preloadclasslist[n]);
			//    }
			//    catch (Exception e)
			//    {
			//        Com.DPrintf("error loading class: " + e.GetMessage());
			//    }
			//}

			GameBase.gun_x = GameBase.gi.Cvar_f( "gun_x", "0", 0 );
			GameBase.gun_y = GameBase.gi.Cvar_f( "gun_y", "0", 0 );
			GameBase.gun_z = GameBase.gi.Cvar_f( "gun_z", "0", 0 );
			GameBase.sv_rollspeed = GameBase.gi.Cvar_f( "sv_rollspeed", "200", 0 );
			GameBase.sv_rollangle = GameBase.gi.Cvar_f( "sv_rollangle", "2", 0 );
			GameBase.sv_maxvelocity = GameBase.gi.Cvar_f( "sv_maxvelocity", "2000", 0 );
			GameBase.sv_gravity = GameBase.gi.Cvar_f( "sv_gravity", "800", 0 );
			Globals.dedicated = GameBase.gi.Cvar_f( "dedicated", "0", Defines.CVAR_NOSET );
			GameBase.sv_cheats = GameBase.gi.Cvar_f( "cheats", "0", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			GameBase.gi.Cvar_f( "gamename", Defines.GAMEVERSION, Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			GameBase.gi.Cvar_f( "gamedate", Globals.__DATE__, Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			GameBase.maxclients = GameBase.gi.Cvar_f( "maxclients", "4", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			GameBase.maxspectators = GameBase.gi.Cvar_f( "maxspectators", "4", Defines.CVAR_SERVERINFO );
			GameBase.deathmatch = GameBase.gi.Cvar_f( "deathmatch", "0", Defines.CVAR_LATCH );
			GameBase.coop = GameBase.gi.Cvar_f( "coop", "0", Defines.CVAR_LATCH );
			GameBase.skill = GameBase.gi.Cvar_f( "skill", "0", Defines.CVAR_LATCH );
			GameBase.maxentities = GameBase.gi.Cvar_f( "maxentities", "1024", Defines.CVAR_LATCH );
			GameBase.dmflags = GameBase.gi.Cvar_f( "dmflags", "0", Defines.CVAR_SERVERINFO );
			GameBase.fraglimit = GameBase.gi.Cvar_f( "fraglimit", "0", Defines.CVAR_SERVERINFO );
			GameBase.timelimit = GameBase.gi.Cvar_f( "timelimit", "0", Defines.CVAR_SERVERINFO );
			GameBase.password = GameBase.gi.Cvar_f( "password", "", Defines.CVAR_USERINFO );
			GameBase.spectator_password = GameBase.gi.Cvar_f( "spectator_password", "", Defines.CVAR_USERINFO );
			GameBase.needpass = GameBase.gi.Cvar_f( "needpass", "0", Defines.CVAR_SERVERINFO );
			GameBase.filterban = GameBase.gi.Cvar_f( "filterban", "1", 0 );
			GameBase.g_select_empty = GameBase.gi.Cvar_f( "g_select_empty", "0", Defines.CVAR_ARCHIVE );
			GameBase.run_pitch = GameBase.gi.Cvar_f( "run_pitch", "0.002", 0 );
			GameBase.run_roll = GameBase.gi.Cvar_f( "run_roll", "0.005", 0 );
			GameBase.bob_up = GameBase.gi.Cvar_f( "bob_up", "0.005", 0 );
			GameBase.bob_pitch = GameBase.gi.Cvar_f( "bob_pitch", "0.002", 0 );
			GameBase.bob_roll = GameBase.gi.Cvar_f( "bob_roll", "0.002", 0 );
			GameBase.flood_msgs = GameBase.gi.Cvar_f( "flood_msgs", "4", 0 );
			GameBase.flood_persecond = GameBase.gi.Cvar_f( "flood_persecond", "4", 0 );
			GameBase.flood_waitdelay = GameBase.gi.Cvar_f( "flood_waitdelay", "10", 0 );
			GameBase.sv_maplist = GameBase.gi.Cvar_f( "sv_maplist", "", 0 );
			GameItems.InitItems();
			GameBase.game.helpmessage1 = "";
			GameBase.game.helpmessage2 = "";
			GameBase.game.maxentities = ( Int32 ) GameBase.maxentities.value;
			CreateEdicts();
			GameBase.game.maxclients = ( Int32 ) GameBase.maxclients.value;
			CreateClients();
			GameBase.num_edicts = GameBase.game.maxclients + 1;
		}

		public static void WriteGame( String filename, Boolean autosave )
		{
			try
			{
				QuakeFile f;
				if ( !autosave )
					PlayerClient.SaveClientData();
				f = new QuakeFile( filename, FileAccess.ReadWrite );
				if ( f == null )
					GameBase.gi.Error( "Couldn't write to " + filename );
				GameBase.game.autosaved = autosave;
				GameBase.game.Write( f );
				GameBase.game.autosaved = false;
				for ( var i = 0; i < GameBase.game.maxclients; i++ )
					GameBase.game.clients[i].Write( f );
				f.Dispose();
			}
			catch ( Exception e )
			{
				e.PrintStackTrace();
			}
		}

		public static void ReadGame( String filename )
		{
			QuakeFile f = null;
			try
			{
				f = new QuakeFile( filename, FileAccess.Read );
				CreateEdicts();
				GameBase.game.Load( f );
				for ( var i = 0; i < GameBase.game.maxclients; i++ )
				{
					GameBase.game.clients[i] = new gclient_t( i );
					GameBase.game.clients[i].Read( f );
				}

				f.Close();
			}
			catch ( Exception e )
			{
				e.PrintStackTrace();
			}
		}

		public static void WriteLevel( String filename )
		{
			try
			{
				Int32 i;
				edict_t ent;
				QuakeFile f;
				f = new QuakeFile( filename, FileAccess.ReadWrite );
				if ( f == null )
					GameBase.gi.Error( "Couldn't open for writing: " + filename );
				GameBase.level.Write( f );
				for ( i = 0; i < GameBase.num_edicts; i++ )
				{
					ent = GameBase.g_edicts[i];
					if ( !ent.inuse )
						continue;
					f.Write( i );
					ent.Write( f );
				}

				i = -1;
				f.Write( -1 );
				f.Dispose();
			}
			catch ( Exception e )
			{
				e.PrintStackTrace();
			}
		}

		public static void ReadLevel( String filename )
		{
			try
			{
				edict_t ent;
				QuakeFile f = new QuakeFile( filename, FileAccess.Read );
				if ( f == null )
					GameBase.gi.Error( "Couldn't read level file " + filename );
				CreateEdicts();
				GameBase.num_edicts = ( Int32 ) GameBase.maxclients.value + 1;
				GameBase.level.Read( f );
				while ( true )
				{
					var entnum = f.ReadInt32();
					if ( entnum == -1 )
						break;
					if ( entnum >= GameBase.num_edicts )
						GameBase.num_edicts = entnum + 1;
					ent = GameBase.g_edicts[entnum];
					ent.Read( f );
					ent.Cleararealinks();
					GameBase.gi.Linkentity( ent );
				}

				f.Dispose();
				for ( var i = 0; i < GameBase.maxclients.value; i++ )
				{
					ent = GameBase.g_edicts[i + 1];
					ent.client = GameBase.game.clients[i];
					ent.client.pers.connected = false;
				}

				for ( var i = 0; i < GameBase.num_edicts; i++ )
				{
					ent = GameBase.g_edicts[i];
					if ( !ent.inuse )
						continue;
					if ( ent.classname != null )
						if ( Lib.Strcmp( ent.classname, "target_crosslevel_target" ) == 0 )
							ent.nextthink = GameBase.level.time + ent.delay;
				}
			}
			catch ( Exception e )
			{
				e.PrintStackTrace();
			}
		}
	}
}
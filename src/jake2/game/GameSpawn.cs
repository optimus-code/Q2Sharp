using J2N.Text;
using Q2Sharp.Game.Monsters;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using static Q2Sharp.Qcommon.Com;

namespace Q2Sharp.Game
{
	public class GameSpawn
	{
		static EntThinkAdapter SP_item_health = new AnonymousEntThinkAdapter();
		private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			public override String GetID( )
			{
				return "SP_item_health";
			}

			public override Boolean Think( edict_t ent )
			{
				GameItems.SP_item_health( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_item_health_small = new AnonymousEntThinkAdapter1();
		private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			public override String GetID( )
			{
				return "SP_item_health_small";
			}

			public override Boolean Think( edict_t ent )
			{
				GameItems.SP_item_health_small( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_item_health_large = new AnonymousEntThinkAdapter2();
		private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			public override String GetID( )
			{
				return "SP_item_health_large";
			}

			public override Boolean Think( edict_t ent )
			{
				GameItems.SP_item_health_large( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_item_health_mega = new AnonymousEntThinkAdapter3();
		private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			public override String GetID( )
			{
				return "SP_item_health_mega";
			}

			public override Boolean Think( edict_t ent )
			{
				GameItems.SP_item_health_mega( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_info_player_start = new AnonymousEntThinkAdapter4();
		private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_info_player_start";
			}

			public override Boolean Think( edict_t ent )
			{
				PlayerClient.SP_info_player_start( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_info_player_deathmatch = new AnonymousEntThinkAdapter5();
		private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_info_player_deathmatch";
			}

			public override Boolean Think( edict_t ent )
			{
				PlayerClient.SP_info_player_deathmatch( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_info_player_coop = new AnonymousEntThinkAdapter6();
		private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_info_player_coop";
			}

			public override Boolean Think( edict_t ent )
			{
				PlayerClient.SP_info_player_coop( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_info_player_intermission = new AnonymousEntThinkAdapter7();
		private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_info_player_intermission";
			}

			public override Boolean Think( edict_t ent )
			{
				PlayerClient.SP_info_player_intermission();
				return true;
			}
		}

		static EntThinkAdapter SP_func_plat = new AnonymousEntThinkAdapter8();
		private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_func_plat";
			}

			public override Boolean Think( edict_t ent )
			{
				GameFunc.SP_func_plat( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_func_water = new AnonymousEntThinkAdapter9();
		private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_func_water";
			}

			public override Boolean Think( edict_t ent )
			{
				GameFunc.SP_func_water( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_func_train = new AnonymousEntThinkAdapter10();
		private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_func_train";
			}

			public override Boolean Think( edict_t ent )
			{
				GameFunc.SP_func_train( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_func_clock = new AnonymousEntThinkAdapter11();
		private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_func_clock";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_func_clock( ent );
				return true;
			}
		}

		static EntThinkAdapter SP_worldspawn = new AnonymousEntThinkAdapter12();
		private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_worldspawn";
			}

			public override Boolean Think( edict_t ent )
			{
				ent.movetype = Defines.MOVETYPE_PUSH;
				ent.solid = Defines.SOLID_BSP;
				ent.inuse = true;
				ent.s.modelindex = 1;
				PlayerClient.InitBodyQue();
				GameItems.SetItemNames();
				if ( GameBase.st.nextmap != null )
					GameBase.level.nextmap = GameBase.st.nextmap;
				if ( ent.message != null && ent.message.Length > 0 )
				{
					GameBase.gi.Configstring( Defines.CS_NAME, ent.message );
					GameBase.level.level_name = ent.message;
				}
				else
					GameBase.level.level_name = GameBase.level.mapname;
				if ( GameBase.st.sky != null && GameBase.st.sky.Length > 0 )
					GameBase.gi.Configstring( Defines.CS_SKY, GameBase.st.sky );
				else
					GameBase.gi.Configstring( Defines.CS_SKY, "unit1_" );
				GameBase.gi.Configstring( Defines.CS_SKYROTATE, "" + GameBase.st.skyrotate );
				GameBase.gi.Configstring( Defines.CS_SKYAXIS, Lib.Vtos( GameBase.st.skyaxis ) );
				GameBase.gi.Configstring( Defines.CS_CDTRACK, "" + ent.sounds );
				GameBase.gi.Configstring( Defines.CS_MAXCLIENTS, "" + ( Int32 ) ( GameBase.maxclients.value ) );
				if ( GameBase.deathmatch.value != 0 )
					GameBase.gi.Configstring( Defines.CS_STATUSBAR, "" + dm_statusbar );
				else
					GameBase.gi.Configstring( Defines.CS_STATUSBAR, "" + single_statusbar );
				GameBase.gi.Imageindex( "i_help" );
				GameBase.level.pic_health = GameBase.gi.Imageindex( "i_health" );
				GameBase.gi.Imageindex( "help" );
				GameBase.gi.Imageindex( "field_3" );
				if ( "".Equals( GameBase.st.gravity ) )
					GameBase.gi.Cvar_set( "sv_gravity", "800" );
				else
					GameBase.gi.Cvar_set( "sv_gravity", GameBase.st.gravity );
				GameBase.snd_fry = GameBase.gi.Soundindex( "player/fry.wav" );
				GameItems.PrecacheItem( GameItems.FindItem( "Blaster" ) );
				GameBase.gi.Soundindex( "player/lava1.wav" );
				GameBase.gi.Soundindex( "player/lava2.wav" );
				GameBase.gi.Soundindex( "misc/pc_up.wav" );
				GameBase.gi.Soundindex( "misc/talk1.wav" );
				GameBase.gi.Soundindex( "misc/udeath.wav" );
				GameBase.gi.Soundindex( "items/respawn1.wav" );
				GameBase.gi.Soundindex( "*death1.wav" );
				GameBase.gi.Soundindex( "*death2.wav" );
				GameBase.gi.Soundindex( "*death3.wav" );
				GameBase.gi.Soundindex( "*death4.wav" );
				GameBase.gi.Soundindex( "*fall1.wav" );
				GameBase.gi.Soundindex( "*fall2.wav" );
				GameBase.gi.Soundindex( "*gurp1.wav" );
				GameBase.gi.Soundindex( "*gurp2.wav" );
				GameBase.gi.Soundindex( "*jump1.wav" );
				GameBase.gi.Soundindex( "*pain25_1.wav" );
				GameBase.gi.Soundindex( "*pain25_2.wav" );
				GameBase.gi.Soundindex( "*pain50_1.wav" );
				GameBase.gi.Soundindex( "*pain50_2.wav" );
				GameBase.gi.Soundindex( "*pain75_1.wav" );
				GameBase.gi.Soundindex( "*pain75_2.wav" );
				GameBase.gi.Soundindex( "*pain100_1.wav" );
				GameBase.gi.Soundindex( "*pain100_2.wav" );
				GameBase.gi.Modelindex( "#w_blaster.md2" );
				GameBase.gi.Modelindex( "#w_shotgun.md2" );
				GameBase.gi.Modelindex( "#w_sshotgun.md2" );
				GameBase.gi.Modelindex( "#w_machinegun.md2" );
				GameBase.gi.Modelindex( "#w_chaingun.md2" );
				GameBase.gi.Modelindex( "#a_grenades.md2" );
				GameBase.gi.Modelindex( "#w_glauncher.md2" );
				GameBase.gi.Modelindex( "#w_rlauncher.md2" );
				GameBase.gi.Modelindex( "#w_hyperblaster.md2" );
				GameBase.gi.Modelindex( "#w_railgun.md2" );
				GameBase.gi.Modelindex( "#w_bfg.md2" );
				GameBase.gi.Soundindex( "player/gasp1.wav" );
				GameBase.gi.Soundindex( "player/gasp2.wav" );
				GameBase.gi.Soundindex( "player/watr_in.wav" );
				GameBase.gi.Soundindex( "player/watr_out.wav" );
				GameBase.gi.Soundindex( "player/watr_un.wav" );
				GameBase.gi.Soundindex( "player/u_breath1.wav" );
				GameBase.gi.Soundindex( "player/u_breath2.wav" );
				GameBase.gi.Soundindex( "items/pkup.wav" );
				GameBase.gi.Soundindex( "world/land.wav" );
				GameBase.gi.Soundindex( "misc/h2ohit1.wav" );
				GameBase.gi.Soundindex( "items/damage.wav" );
				GameBase.gi.Soundindex( "items/protect.wav" );
				GameBase.gi.Soundindex( "items/protect4.wav" );
				GameBase.gi.Soundindex( "weapons/noammo.wav" );
				GameBase.gi.Soundindex( "infantry/inflies1.wav" );
				GameBase.sm_meat_index = GameBase.gi.Modelindex( "models/objects/gibs/sm_meat/tris.md2" );
				GameBase.gi.Modelindex( "models/objects/gibs/arm/tris.md2" );
				GameBase.gi.Modelindex( "models/objects/gibs/bone/tris.md2" );
				GameBase.gi.Modelindex( "models/objects/gibs/bone2/tris.md2" );
				GameBase.gi.Modelindex( "models/objects/gibs/chest/tris.md2" );
				GameBase.gi.Modelindex( "models/objects/gibs/skull/tris.md2" );
				GameBase.gi.Modelindex( "models/objects/gibs/head2/tris.md2" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 0, "m" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 1, "mmnmmommommnonmmonqnmmo" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 2, "abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 3, "mmmmmaaaaammmmmaaaaaabcdefgabcdefg" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 4, "mamamamamama" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 5, "jklmnopqrstuvwxyzyxwvutsrqponmlkj" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 6, "nmonqnmomnmomomno" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 7, "mmmaaaabcdefgmmmmaaaammmaamm" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 8, "mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 9, "aaaaaaaazzzzzzzz" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 10, "mmamammmmammamamaaamammma" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 11, "abcdefghijklmnopqrrqponmlkjihgfedcba" );
				GameBase.gi.Configstring( Defines.CS_LIGHTS + 63, "a" );
				return true;
			}
		}

		public static String ED_NewString( String string_renamed )
		{
			var l = string_renamed.Length;
			StringBuffer newb = new StringBuffer( l );
			for ( var i = 0; i < l; i++ )
			{
				var c = string_renamed[i];
				if ( c == '\\' && i < l - 1 )
				{
					c = string_renamed[++i];
					if ( c == 'n' )
						newb.Append( '\\' );
					else
						newb.Append( '\\' );
				}
				else
					newb.Append( c );
			}

			return newb.ToString();
		}

		static void ED_ParseField( String key, String value, edict_t ent )
		{
			if ( key.Equals( "nextmap" ) )
				Com.Println( "nextmap: " + value );
			if ( !GameBase.st.Set( key, value ) )
				if ( !ent.SetField( key, value ) )
					GameBase.gi.Dprintf( "??? The key [" + key + "] is not a field\\n" );
		}

		static void ED_ParseEdict( Com.ParseHelp ph, edict_t ent )
		{
			Boolean init;
			String keyname;
			String com_token;
			init = false;
			GameBase.st = new spawn_temp_t();
			while ( true )
			{
				com_token = Com.Parse( ph );
				if ( com_token.Equals( "}" ) )
					break;
				if ( ph.IsEof() )
					GameBase.gi.Error( "ED_ParseEntity: EOF without closing brace" );
				keyname = com_token;
				com_token = Com.Parse( ph );
				if ( ph.IsEof() )
					GameBase.gi.Error( "ED_ParseEntity: EOF without closing brace" );
				if ( com_token.Equals( "}" ) )
					GameBase.gi.Error( "ED_ParseEntity: closing brace without data" );
				init = true;
				if ( keyname[0] == '_' )
					continue;
				ED_ParseField( keyname.ToLower(), com_token, ent );
			}

			if ( !init )
			{
				GameUtil.G_ClearEdict( ent );
			}

			return;
		}

		static void G_FindTeams( )
		{
			edict_t e, e2, chain;
			Int32 i, j;
			Int32 c, c2;
			c = 0;
			c2 = 0;
			for ( i = 1; i < GameBase.num_edicts; i++ )
			{
				e = GameBase.g_edicts[i];
				if ( !e.inuse )
					continue;
				if ( e.team == null )
					continue;
				if ( ( e.flags & Defines.FL_TEAMSLAVE ) != 0 )
					continue;
				chain = e;
				e.teammaster = e;
				c++;
				c2++;
				for ( j = i + 1; j < GameBase.num_edicts; j++ )
				{
					e2 = GameBase.g_edicts[j];
					if ( !e2.inuse )
						continue;
					if ( null == e2.team )
						continue;
					if ( ( e2.flags & Defines.FL_TEAMSLAVE ) != 0 )
						continue;
					if ( 0 == Lib.Strcmp( e.team, e2.team ) )
					{
						c2++;
						chain.teamchain = e2;
						e2.teammaster = e;
						chain = e2;
						e2.flags |= Defines.FL_TEAMSLAVE;
					}
				}
			}
		}

		public static void SpawnEntities( String mapname, String entities, String spawnpoint )
		{
			Com.Dprintln( "SpawnEntities(), mapname=" + mapname );
			edict_t ent;
			Int32 inhibit;
			String com_token;
			Int32 i;
			Single skill_level;
			skill_level = ( Single ) Math.Floor( GameBase.skill.value );
			if ( skill_level < 0 )
				skill_level = 0;
			if ( skill_level > 3 )
				skill_level = 3;
			if ( GameBase.skill.value != skill_level )
				GameBase.gi.Cvar_forceset( "skill", "" + skill_level );
			PlayerClient.SaveClientData();
			GameBase.level = new level_locals_t();
			for ( var n = 0; n < GameBase.game.maxentities; n++ )
			{
				GameBase.g_edicts[n] = new edict_t( n );
			}

			GameBase.level.mapname = mapname;
			GameBase.game.spawnpoint = spawnpoint;
			for ( i = 0; i < GameBase.game.maxclients; i++ )
				GameBase.g_edicts[i + 1].client = GameBase.game.clients[i];
			ent = null;
			inhibit = 0;
			Com.ParseHelp ph = new ParseHelp( entities );
			while ( true )
			{
				com_token = Com.Parse( ph );
				if ( ph.IsEof() )
					break;
				if ( !com_token.StartsWith( "{" ) )
					GameBase.gi.Error( "ED_LoadFromFile: found " + com_token + " when expecting {" );
				if ( ent == null )
					ent = GameBase.g_edicts[0];
				else
					ent = GameUtil.G_Spawn();
				ED_ParseEdict( ph, ent );
				Com.DPrintf( "spawning ent[" + ent.index + "], classname=" + ent.classname + ", flags= " + ent.spawnflags.ToString( "X4" ) );
				if ( 0 == Lib.Q_stricmp( GameBase.level.mapname, "command" ) && 0 == Lib.Q_stricmp( ent.classname, "trigger_once" ) && 0 == Lib.Q_stricmp( ent.model, "*27" ) )
					ent.spawnflags &= ~Defines.SPAWNFLAG_NOT_HARD;
				if ( ent != GameBase.g_edicts[0] )
				{
					if ( GameBase.deathmatch.value != 0 )
					{
						if ( ( ent.spawnflags & Defines.SPAWNFLAG_NOT_DEATHMATCH ) != 0 )
						{
							Com.DPrintf( "->inhibited.\\n" );
							GameUtil.G_FreeEdict( ent );
							inhibit++;
							continue;
						}
					}
					else
					{
						if ( ( ( GameBase.skill.value == 0 ) && ( ent.spawnflags & Defines.SPAWNFLAG_NOT_EASY ) != 0 ) || ( ( GameBase.skill.value == 1 ) && ( ent.spawnflags & Defines.SPAWNFLAG_NOT_MEDIUM ) != 0 ) || ( ( ( GameBase.skill.value == 2 ) || ( GameBase.skill.value == 3 ) ) && ( ent.spawnflags & Defines.SPAWNFLAG_NOT_HARD ) != 0 ) )
						{
							Com.DPrintf( "->inhibited.\\n" );
							GameUtil.G_FreeEdict( ent );
							inhibit++;
							continue;
						}
					}

					ent.spawnflags &= ~( Defines.SPAWNFLAG_NOT_EASY | Defines.SPAWNFLAG_NOT_MEDIUM | Defines.SPAWNFLAG_NOT_HARD | Defines.SPAWNFLAG_NOT_COOP | Defines.SPAWNFLAG_NOT_DEATHMATCH );
				}

				ED_CallSpawn( ent );
				Com.DPrintf( "\\n" );
			}

			Com.DPrintf( "player skill level:" + GameBase.skill.value + "\\n" );
			Com.DPrintf( inhibit + " entities inhibited.\\n" );
			i = 1;
			G_FindTeams();
			PlayerTrail.Init();
		}

		static String single_statusbar = "yb\t-24 " + "xv\t0 " + "hnum " + "xv\t50 " + "pic 0 " + "if 2 " + "\txv\t100 " + "\tanum " + "\txv\t150 " + "\tpic 2 " + "endif " + "if 4 " + "\txv\t200 " + "\trnum " + "\txv\t250 " + "\tpic 4 " + "endif " + "if 6 " + "\txv\t296 " + "\tpic 6 " + "endif " + "yb\t-50 " + "if 7 " + "\txv\t0 " + "\tpic 7 " + "\txv\t26 " + "\tyb\t-42 " + "\tstat_string 8 " + "\tyb\t-50 " + "endif " + "if 9 " + "\txv\t262 " + "\tnum\t2\t10 " + "\txv\t296 " + "\tpic\t9 " + "endif " + "if 11 " + "\txv\t148 " + "\tpic\t11 " + "endif ";
		static String dm_statusbar = "yb\t-24 " + "xv\t0 " + "hnum " + "xv\t50 " + "pic 0 " + "if 2 " + "\txv\t100 " + "\tanum " + "\txv\t150 " + "\tpic 2 " + "endif " + "if 4 " + "\txv\t200 " + "\trnum " + "\txv\t250 " + "\tpic 4 " + "endif " + "if 6 " + "\txv\t296 " + "\tpic 6 " + "endif " + "yb\t-50 " + "if 7 " + "\txv\t0 " + "\tpic 7 " + "\txv\t26 " + "\tyb\t-42 " + "\tstat_string 8 " + "\tyb\t-50 " + "endif " + "if 9 " + "\txv\t246 " + "\tnum\t2\t10 " + "\txv\t296 " + "\tpic\t9 " + "endif " + "if 11 " + "\txv\t148 " + "\tpic\t11 " + "endif " + "xr\t-50 " + "yt 2 " + "num 3 14 " + "if 17 " + "xv 0 " + "yb -58 " + "string2 \\\"SPECTATOR MODE\\\" " + "endif " + "if 16 " + "xv 0 " + "yb -68 " + "string \\\"Chasing\\\" " + "xv 64 " + "stat_string 16 " + "endif ";
		static spawn_t[] spawns = new[] { new spawn_t( "item_health", SP_item_health ), new spawn_t( "item_health_small", SP_item_health_small ), new spawn_t( "item_health_large", SP_item_health_large ), new spawn_t( "item_health_mega", SP_item_health_mega ), new spawn_t( "info_player_start", SP_info_player_start ), new spawn_t( "info_player_deathmatch", SP_info_player_deathmatch ), new spawn_t( "info_player_coop", SP_info_player_coop ), new spawn_t( "info_player_intermission", SP_info_player_intermission ), new spawn_t( "func_plat", SP_func_plat ), new spawn_t( "func_button", GameFunc.SP_func_button ), new spawn_t( "func_door", GameFunc.SP_func_door ), new spawn_t( "func_door_secret", GameFunc.SP_func_door_secret ), new spawn_t( "func_door_rotating", GameFunc.SP_func_door_rotating ), new spawn_t( "func_rotating", GameFunc.SP_func_rotating ), new spawn_t( "func_train", SP_func_train ), new spawn_t( "func_water", SP_func_water ), new spawn_t( "func_conveyor", GameFunc.SP_func_conveyor ), new spawn_t( "func_areaportal", GameMisc.SP_func_areaportal ), new spawn_t( "func_clock", SP_func_clock ), new spawn_t( "func_wall", new AnonymousEntThinkAdapter13() ), new spawn_t( "func_object", new AnonymousEntThinkAdapter14() ), new spawn_t( "func_timer", new AnonymousEntThinkAdapter15() ), new spawn_t( "func_explosive", new AnonymousEntThinkAdapter16() ), new spawn_t( "func_killbox", GameFunc.SP_func_killbox ), new spawn_t( "trigger_always", new AnonymousEntThinkAdapter17() ), new spawn_t( "trigger_once", new AnonymousEntThinkAdapter18() ), new spawn_t( "trigger_multiple", new AnonymousEntThinkAdapter19() ), new spawn_t( "trigger_relay", new AnonymousEntThinkAdapter20() ), new spawn_t( "trigger_push", new AnonymousEntThinkAdapter21() ), new spawn_t( "trigger_hurt", new AnonymousEntThinkAdapter22() ), new spawn_t( "trigger_key", new AnonymousEntThinkAdapter23() ), new spawn_t( "trigger_counter", new AnonymousEntThinkAdapter24() ), new spawn_t( "trigger_elevator", GameFunc.SP_trigger_elevator ), new spawn_t( "trigger_gravity", new AnonymousEntThinkAdapter25() ), new spawn_t( "trigger_monsterjump", new AnonymousEntThinkAdapter26() ), new spawn_t( "target_temp_entity", new AnonymousEntThinkAdapter27() ), new spawn_t( "target_speaker", new AnonymousEntThinkAdapter28() ), new spawn_t( "target_explosion", new AnonymousEntThinkAdapter29() ), new spawn_t( "target_changelevel", new AnonymousEntThinkAdapter30() ), new spawn_t( "target_secret", new AnonymousEntThinkAdapter31() ), new spawn_t( "target_goal", new AnonymousEntThinkAdapter32() ), new spawn_t( "target_splash", new AnonymousEntThinkAdapter33() ), new spawn_t( "target_spawner", new AnonymousEntThinkAdapter34() ), new spawn_t( "target_blaster", new AnonymousEntThinkAdapter35() ), new spawn_t( "target_crosslevel_trigger", new AnonymousEntThinkAdapter36() ), new spawn_t( "target_crosslevel_target", new AnonymousEntThinkAdapter37() ), new spawn_t( "target_laser", new AnonymousEntThinkAdapter38() ), new spawn_t( "target_help", new AnonymousEntThinkAdapter39() ), new spawn_t( "target_actor", new AnonymousEntThinkAdapter40() ), new spawn_t( "target_lightramp", new AnonymousEntThinkAdapter41() ), new spawn_t( "target_earthquake", new AnonymousEntThinkAdapter42() ), new spawn_t( "target_character", new AnonymousEntThinkAdapter43() ), new spawn_t( "target_string", new AnonymousEntThinkAdapter44() ), new spawn_t( "worldspawn", SP_worldspawn ), new spawn_t( "viewthing", new AnonymousEntThinkAdapter45() ), new spawn_t( "light", new AnonymousEntThinkAdapter46() ), new spawn_t( "light_mine1", new AnonymousEntThinkAdapter47() ), new spawn_t( "light_mine2", new AnonymousEntThinkAdapter48() ), new spawn_t( "info_null", new AnonymousEntThinkAdapter49() ), new spawn_t( "func_group", new AnonymousEntThinkAdapter50() ), new spawn_t( "info_notnull", new AnonymousEntThinkAdapter51() ), new spawn_t( "path_corner", new AnonymousEntThinkAdapter52() ), new spawn_t( "point_combat", new AnonymousEntThinkAdapter53() ), new spawn_t( "misc_explobox", new AnonymousEntThinkAdapter54() ), new spawn_t( "misc_banner", new AnonymousEntThinkAdapter55() ), new spawn_t( "misc_satellite_dish", new AnonymousEntThinkAdapter56() ), new spawn_t( "misc_actor", new AnonymousEntThinkAdapter57() ), new spawn_t( "misc_gib_arm", new AnonymousEntThinkAdapter58() ), new spawn_t( "misc_gib_leg", new AnonymousEntThinkAdapter59() ), new spawn_t( "misc_gib_head", new AnonymousEntThinkAdapter60() ), new spawn_t( "misc_insane", new AnonymousEntThinkAdapter61() ), new spawn_t( "misc_deadsoldier", new AnonymousEntThinkAdapter62() ), new spawn_t( "misc_viper", new AnonymousEntThinkAdapter63() ), new spawn_t( "misc_viper_bomb", new AnonymousEntThinkAdapter64() ), new spawn_t( "misc_bigviper", new AnonymousEntThinkAdapter65() ), new spawn_t( "misc_strogg_ship", new AnonymousEntThinkAdapter66() ), new spawn_t( "misc_teleporter", new AnonymousEntThinkAdapter67() ), new spawn_t( "misc_teleporter_dest", GameMisc.SP_misc_teleporter_dest ), new spawn_t( "misc_blackhole", new AnonymousEntThinkAdapter68() ), new spawn_t( "misc_eastertank", new AnonymousEntThinkAdapter69() ), new spawn_t( "misc_easterchick", new AnonymousEntThinkAdapter70() ), new spawn_t( "misc_easterchick2", new AnonymousEntThinkAdapter71() ), new spawn_t( "monster_berserk", new AnonymousEntThinkAdapter72() ), new spawn_t( "monster_gladiator", new AnonymousEntThinkAdapter73() ), new spawn_t( "monster_gunner", new AnonymousEntThinkAdapter74() ), new spawn_t( "monster_infantry", new AnonymousEntThinkAdapter75() ), new spawn_t( "monster_soldier_light", M_Soldier.SP_monster_soldier_light ), new spawn_t( "monster_soldier", M_Soldier.SP_monster_soldier ), new spawn_t( "monster_soldier_ss", M_Soldier.SP_monster_soldier_ss ), new spawn_t( "monster_tank", M_Tank.SP_monster_tank ), new spawn_t( "monster_tank_commander", M_Tank.SP_monster_tank ), new spawn_t( "monster_medic", new AnonymousEntThinkAdapter76() ), new spawn_t( "monster_flipper", new AnonymousEntThinkAdapter77() ), new spawn_t( "monster_chick", new AnonymousEntThinkAdapter78() ), new spawn_t( "monster_parasite", M_Parasite.SP_monster_parasite ), new spawn_t( "monster_flyer", new AnonymousEntThinkAdapter79() ), new spawn_t( "monster_brain", new AnonymousEntThinkAdapter80() ), new spawn_t( "monster_floater", new AnonymousEntThinkAdapter81() ), new spawn_t( "monster_hover", new AnonymousEntThinkAdapter82() ), new spawn_t( "monster_mutant", M_Mutant.SP_monster_mutant ), new spawn_t( "monster_supertank", M_Supertank.SP_monster_supertank ), new spawn_t( "monster_boss2", new AnonymousEntThinkAdapter83() ), new spawn_t( "monster_boss3_stand", new AnonymousEntThinkAdapter84() ), new spawn_t( "monster_jorg", new AnonymousEntThinkAdapter85() ), new spawn_t( "monster_commander_body", new AnonymousEntThinkAdapter86() ), new spawn_t( "turret_breach", new AnonymousEntThinkAdapter87() ), new spawn_t( "turret_base", new AnonymousEntThinkAdapter88() ), new spawn_t( "turret_driver", new AnonymousEntThinkAdapter89() ), new spawn_t( null, null ) };
		private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "func_wall";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_func_wall( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_func_object";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_func_object( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_func_timer";
			}

			public override Boolean Think( edict_t ent )
			{
				GameFunc.SP_func_timer( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_func_explosive";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_func_explosive( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter17 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_always";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_always( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter18 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_once";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_once( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter19 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_multiple";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_multiple( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter20 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_relay";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_relay( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter21 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_push";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_push( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter22 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_hurt";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_hurt( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter23 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_key";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_key( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter24 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_counter";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_counter( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter25 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_gravity";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_gravity( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter26 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_trigger_monsterjump";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTrigger.SP_trigger_monsterjump( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter27 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_temp_entity";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_temp_entity( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter28 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_speaker";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_speaker( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter29 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_explosion";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_explosion( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter30 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_changelevel";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_changelevel( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter31 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_secret";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_secret( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter32 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_goal";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_goal( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter33 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_splash";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_splash( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter34 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_spawner";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_spawner( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter35 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_blaster";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_blaster( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter36 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_crosslevel_trigger";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_crosslevel_trigger( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter37 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_crosslevel_target";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_crosslevel_target( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter38 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_laser";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_laser( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter39 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_help";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_help( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter40 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_actor";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Actor.SP_target_actor( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter41 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_lightramp";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_lightramp( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter42 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_earthquake";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTarget.SP_target_earthquake( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter43 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_character";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_target_character( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter44 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_target_string";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_target_string( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter45 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_viewthing";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_viewthing( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter46 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_light";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_light( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter47 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_light_mine1";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_light_mine1( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter48 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_light_mine2";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_light_mine2( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter49 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_info_null";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_info_null( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter50 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_info_null";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_info_null( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter51 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "info_notnull";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_info_notnull( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter52 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_path_corner";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_path_corner( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter53 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_point_combat";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_point_combat( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter54 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_explobox";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_explobox( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter55 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_banner";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_banner( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter56 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_satellite_dish";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_satellite_dish( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter57 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_actor";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Actor.SP_misc_actor( ent );
				return false;
			}
		}

		private sealed class AnonymousEntThinkAdapter58 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_gib_arm";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_gib_arm( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter59 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_gib_leg";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_gib_leg( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter60 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_gib_head";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_gib_head( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter61 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_insane";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Insane.SP_misc_insane( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter62 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_deadsoldier";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_deadsoldier( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter63 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_viper";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_viper( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter64 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_viper_bomb";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_viper_bomb( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter65 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_bigviper";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_bigviper( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter66 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_strogg_ship";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_strogg_ship( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter67 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_teleporter";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_teleporter( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter68 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_blackhole";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_blackhole( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter69 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_eastertank";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_eastertank( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter70 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_easterchick";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_easterchick( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter71 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_misc_easterchick2";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_misc_easterchick2( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter72 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_berserk";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Berserk.SP_monster_berserk( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter73 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_gladiator";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Gladiator.SP_monster_gladiator( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter74 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_gunner";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Gunner.SP_monster_gunner( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter75 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_infantry";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Infantry.SP_monster_infantry( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter76 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_medic";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Medic.SP_monster_medic( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter77 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_flipper";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Flipper.SP_monster_flipper( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter78 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_chick";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Chick.SP_monster_chick( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter79 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_flyer";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Flyer.SP_monster_flyer( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter80 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_brain";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Brain.SP_monster_brain( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter81 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_floater";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Float.SP_monster_floater( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter82 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_hover";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Hover.SP_monster_hover( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter83 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_boss2";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Boss2.SP_monster_boss2( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter84 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_boss3_stand";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Boss3.SP_monster_boss3_stand( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter85 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_jorg";
			}

			public override Boolean Think( edict_t ent )
			{
				M_Boss31.SP_monster_jorg( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter86 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_monster_commander_body";
			}

			public override Boolean Think( edict_t ent )
			{
				GameMisc.SP_monster_commander_body( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter87 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_turret_breach";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTurret.SP_turret_breach( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter88 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_turret_base";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTurret.SP_turret_base( ent );
				return true;
			}
		}

		private sealed class AnonymousEntThinkAdapter89 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "SP_turret_driver";
			}

			public override Boolean Think( edict_t ent )
			{
				GameTurret.SP_turret_driver( ent );
				return true;
			}
		}

		public static void ED_CallSpawn( edict_t ent )
		{
			spawn_t s;
			gitem_t item;
			Int32 i;
			if ( null == ent.classname )
			{
				GameBase.gi.Dprintf( "ED_CallSpawn: null classname\\n" );
				return;
			}

			for ( i = 1; i < GameBase.game.num_items; i++ )
			{
				item = GameItemList.itemlist[i];
				if ( item == null )
					GameBase.gi.Error( "ED_CallSpawn: null item in pos " + i );
				if ( item.classname == null )
					continue;
				if ( item.classname.EqualsIgnoreCase( ent.classname ) )
				{
					GameItems.SpawnItem( ent, item );
					return;
				}
			}

			for ( i = 0; ( s = spawns[i] ) != null && s.name != null; i++ )
			{
				if ( s.name.EqualsIgnoreCase( ent.classname ) )
				{
					if ( s.spawn == null )
						GameBase.gi.Error( "ED_CallSpawn: null-spawn on index=" + i );
					s.spawn.Think( ent );
					return;
				}
			}

			GameBase.gi.Dprintf( ent.classname + " doesn't have a spawn function\\n" );
		}
	}
}
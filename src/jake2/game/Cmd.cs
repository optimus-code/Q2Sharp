using Q2Sharp.Game.Monsters;
using Q2Sharp.Qcommon;
using Q2Sharp.Server;
using Q2Sharp.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Q2Sharp.Qcommon.Com;

namespace Q2Sharp.Game
{
	public sealed class Cmd
	{
		static xcommand_t List_f = new Anonymousxcommand_t();
		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				cmd_function_t cmd = Cmd.cmd_functions;
				var i = 0;
				while ( cmd != null )
				{
					Com.Printf( cmd.name + '\\' );
					i++;
					cmd = cmd.next;
				}

				Com.Printf( i + " commands\\n" );
			}
		}

		static xcommand_t Exec_f = new Anonymousxcommand_t1();
		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				if ( Cmd.Argc() != 2 )
				{
					Com.Printf( "exec <filename> : execute a script file\\n" );
					return;
				}

				Byte[] f = null;
				f = FS.LoadFile( Cmd.Argv( 1 ) );
				if ( f == null )
				{
					Com.Printf( "couldn't exec " + Cmd.Argv( 1 ) + "\\n" );
					return;
				}

				Com.Printf( "execing " + Cmd.Argv( 1 ) + "\\n" );
				Cbuf.InsertText( Encoding.ASCII.GetString( f ) );
				FS.FreeFile( f );
			}
		}

		static xcommand_t Echo_f = new Anonymousxcommand_t2();
		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				for ( var i = 1; i < Cmd.Argc(); i++ )
				{
					Com.Printf( Cmd.Argv( i ) + " " );
				}

				Com.Printf( "'\\n" );
			}
		}

		static xcommand_t Alias_f = new Anonymousxcommand_t3();
		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public override void Execute( )
			{
				cmdalias_t a = null;
				if ( Cmd.Argc() == 1 )
				{
					Com.Printf( "Current alias commands:\\n" );
					for ( a = Globals.cmd_alias; a != null; a = a.next )
					{
						Com.Printf( a.name + " : " + a.value );
					}

					return;
				}

				var s = Cmd.Argv( 1 );
				if ( s.Length > Defines.MAX_ALIAS_NAME )
				{
					Com.Printf( "Alias name is too long\\n" );
					return;
				}

				for ( a = Globals.cmd_alias; a != null; a = a.next )
				{
					if ( s.EqualsIgnoreCase( a.name ) )
					{
						a.value = null;
						break;
					}
				}

				if ( a == null )
				{
					a = new cmdalias_t();
					a.next = Globals.cmd_alias;
					Globals.cmd_alias = a;
				}

				a.name = s;
				var cmd = "";
				var c = Cmd.Argc();
				for ( var i = 2; i < c; i++ )
				{
					cmd = cmd + Cmd.Argv( i );
					if ( i != ( c - 1 ) )
						cmd = cmd + " ";
				}

				cmd = cmd + "\\n";
				a.value = cmd;
			}
		}

		public static xcommand_t Wait_f = new Anonymousxcommand_t4();
		private sealed class Anonymousxcommand_t4 : xcommand_t
		{
			public override void Execute( )
			{
				Globals.cmd_wait = true;
			}
		}

		public static cmd_function_t cmd_functions = null;
		public static Int32 cmd_argc;
		public static String[] cmd_argv = new String[Defines.MAX_STRING_TOKENS];
		public static String cmd_args;
		public static readonly Int32 ALIAS_LOOP_COUNT = 16;
		public static void Init( )
		{
			Cmd.AddCommand( "exec", Exec_f );
			Cmd.AddCommand( "echo", Echo_f );
			Cmd.AddCommand( "cmdlist", List_f );
			Cmd.AddCommand( "alias", Alias_f );
			Cmd.AddCommand( "wait", Wait_f );
		}

		private static Char[] expanded = new Char[Defines.MAX_STRING_CHARS];
		private static Char[] temporary = new Char[Defines.MAX_STRING_CHARS];
		public static IComparer<Object> PlayerSort = new AnonymousComparator();
		private sealed class AnonymousComparator : IComparer<Object>
		{
			public Int32 Compare( Object o1, Object o2 )
			{
				var anum = ( ( Int32 ) o1 );
				var bnum = ( ( Int32 ) o2 );
				Int32 anum1 = GameBase.game.clients[anum].ps.stats[Defines.STAT_FRAGS];
				Int32 bnum1 = GameBase.game.clients[bnum].ps.stats[Defines.STAT_FRAGS];
				if ( anum1 < bnum1 )
					return -1;
				if ( anum1 > bnum1 )
					return 1;
				return 0;
			}
		}

		public static Char[] MacroExpandString( Char[] text, Int32 len )
		{
			Int32 i, j, count;
			Boolean inquote;
			Char[] scan;
			String token;
			inquote = false;
			scan = text;
			if ( len >= Defines.MAX_STRING_CHARS )
			{
				Com.Printf( "Line exceeded " + Defines.MAX_STRING_CHARS + " chars, discarded.\\n" );
				return null;
			}

			count = 0;
			for ( i = 0; i < len; i++ )
			{
				if ( scan[i] == '"' )
					inquote = !inquote;
				if ( inquote )
					continue;
				if ( scan[i] != '$' )
					continue;
				Com.ParseHelp ph = new ParseHelp( text, i + 1 );
				token = Com.Parse( ph );
				if ( ph.data == null )
					continue;
				token = Cvar.VariableString( token );
				j = token.Length;
				len += j;
				if ( len >= Defines.MAX_STRING_CHARS )
				{
					Com.Printf( "Expanded line exceeded " + Defines.MAX_STRING_CHARS + " chars, discarded.\\n" );
					return null;
				}

				System.Array.Copy( scan, 0, temporary, 0, i );
				System.Array.Copy( token.ToCharArray(), 0, temporary, i, token.Length );
				System.Array.Copy( ph.data, ph.index, temporary, i + j, len - ph.index - j );
				System.Array.Copy( temporary, 0, expanded, 0, 0 );
				scan = expanded;
				i--;
				if ( ++count == 100 )
				{
					Com.Printf( "Macro expansion loop, discarded.\\n" );
					return null;
				}
			}

			if ( inquote )
			{
				Com.Printf( "Line has unmatched quote, discarded.\\n" );
				return null;
			}

			return scan;
		}

		public static void TokenizeString( Char[] text, Boolean macroExpand )
		{
			String com_token;
			cmd_argc = 0;
			cmd_args = "";
			var len = Lib.Strlen( text );
			if ( macroExpand )
				text = MacroExpandString( text, len );
			if ( text == null )
				return;
			len = Lib.Strlen( text );
			Com.ParseHelp ph = new ParseHelp( text );
			while ( true )
			{
				var c = ph.Skipwhitestoeol();
				if ( c == '\\' )
				{
					c = ph.Nextchar();
					break;
				}

				if ( c == 0 )
					return;
				if ( cmd_argc == 1 )
				{
					cmd_args = new String( text, ph.index, len - ph.index );
					cmd_args.Trim();
				}

				com_token = Com.Parse( ph );
				if ( ph.data == null )
					return;
				if ( cmd_argc < Defines.MAX_STRING_TOKENS )
				{
					cmd_argv[cmd_argc] = com_token;
					cmd_argc++;
				}
			}
		}

		public static void AddCommand( String cmd_name, xcommand_t function )
		{
			cmd_function_t cmd;
			if ( ( Cvar.VariableString( cmd_name ) ).Length > 0 )
			{
				Com.Printf( "Cmd_AddCommand: " + cmd_name + " already defined as a var\\n" );
				return;
			}

			for ( cmd = cmd_functions; cmd != null; cmd = cmd.next )
			{
				if ( cmd_name.Equals( cmd.name ) )
				{
					Com.Printf( "Cmd_AddCommand: " + cmd_name + " already defined\\n" );
					return;
				}
			}

			cmd = new cmd_function_t();
			cmd.name = cmd_name;
			cmd.function = function;
			cmd.next = cmd_functions;
			cmd_functions = cmd;
		}

		public static void RemoveCommand( String cmd_name )
		{
			cmd_function_t cmd, back = null;
			back = cmd = cmd_functions;
			while ( true )
			{
				if ( cmd == null )
				{
					Com.Printf( "Cmd_RemoveCommand: " + cmd_name + " not added\\n" );
					return;
				}

				if ( 0 == Lib.Strcmp( cmd_name, cmd.name ) )
				{
					if ( cmd == cmd_functions )
						cmd_functions = cmd.next;
					else
						back.next = cmd.next;
					return;
				}

				back = cmd;
				cmd = cmd.next;
			}
		}

		public static Boolean Exists( String cmd_name )
		{
			cmd_function_t cmd;
			for ( cmd = cmd_functions; cmd != null; cmd = cmd.next )
			{
				if ( cmd.name.Equals( cmd_name ) )
					return true;
			}

			return false;
		}

		public static Int32 Argc( )
		{
			return cmd_argc;
		}

		public static String Argv( Int32 i )
		{
			if ( i < 0 || i >= cmd_argc )
				return "";
			return cmd_argv[i];
		}

		public static String Args( )
		{
			return new String( cmd_args );
		}

		public static void ExecuteString( String text )
		{
			cmd_function_t cmd;
			cmdalias_t a;
			TokenizeString( text.ToCharArray(), true );
			if ( Argc() == 0 )
				return;
			for ( cmd = cmd_functions; cmd != null; cmd = cmd.next )
			{
				if ( cmd_argv[0].EqualsIgnoreCase( cmd.name ) )
				{
					if ( null == cmd.function )
					{
						Cmd.ExecuteString( "cmd " + text );
					}
					else
					{
						cmd.function.Execute();
					}

					return;
				}
			}

			for ( a = Globals.cmd_alias; a != null; a = a.next )
			{
				if ( cmd_argv[0].EqualsIgnoreCase( a.name ) )
				{
					if ( ++Globals.alias_count == ALIAS_LOOP_COUNT )
					{
						Com.Printf( "ALIAS_LOOP_COUNT\\n" );
						return;
					}

					Cbuf.InsertText( a.value );
					return;
				}
			}

			if ( Cvar.Command() )
				return;
			Cmd.ForwardToServer();
		}

		public static void Give_f( edict_t ent )
		{
			String name;
			gitem_t it;
			Int32 index;
			Int32 i;
			Boolean give_all;
			edict_t it_ent;
			if ( GameBase.deathmatch.value != 0 && GameBase.sv_cheats.value == 0 )
			{
				SV_GAME.PF_cprintfhigh( ent, "You must run the server with '+set cheats 1' to enable this command.\\n" );
				return;
			}

			name = Cmd.Args();
			if ( 0 == Lib.Q_stricmp( name, "all" ) )
				give_all = true;
			else
				give_all = false;
			if ( give_all || 0 == Lib.Q_stricmp( Cmd.Argv( 1 ), "health" ) )
			{
				if ( Cmd.Argc() == 3 )
					ent.health = Lib.Atoi( Cmd.Argv( 2 ) );
				else
					ent.health = ent.max_health;
				if ( !give_all )
					return;
			}

			if ( give_all || 0 == Lib.Q_stricmp( name, "weapons" ) )
			{
				for ( i = 1; i < GameBase.game.num_items; i++ )
				{
					it = GameItemList.itemlist[i];
					if ( null == it.pickup )
						continue;
					if ( 0 == ( it.flags & Defines.IT_WEAPON ) )
						continue;
					ent.client.pers.inventory[i] += 1;
				}

				if ( !give_all )
					return;
			}

			if ( give_all || 0 == Lib.Q_stricmp( name, "ammo" ) )
			{
				for ( i = 1; i < GameBase.game.num_items; i++ )
				{
					it = GameItemList.itemlist[i];
					if ( null == it.pickup )
						continue;
					if ( 0 == ( it.flags & Defines.IT_AMMO ) )
						continue;
					GameItems.Add_Ammo( ent, it, 1000 );
				}

				if ( !give_all )
					return;
			}

			if ( give_all || Lib.Q_stricmp( name, "armor" ) == 0 )
			{
				gitem_armor_t info;
				it = GameItems.FindItem( "Jacket Armor" );
				ent.client.pers.inventory[GameItems.ITEM_INDEX( it )] = 0;
				it = GameItems.FindItem( "Combat Armor" );
				ent.client.pers.inventory[GameItems.ITEM_INDEX( it )] = 0;
				it = GameItems.FindItem( "Body Armor" );
				info = ( gitem_armor_t ) it.info;
				ent.client.pers.inventory[GameItems.ITEM_INDEX( it )] = info.max_count;
				if ( !give_all )
					return;
			}

			if ( give_all || Lib.Q_stricmp( name, "Power Shield" ) == 0 )
			{
				it = GameItems.FindItem( "Power Shield" );
				it_ent = GameUtil.G_Spawn();
				it_ent.classname = it.classname;
				GameItems.SpawnItem( it_ent, it );
				GameItems.Touch_Item( it_ent, ent, GameBase.dummyplane, null );
				if ( it_ent.inuse )
					GameUtil.G_FreeEdict( it_ent );
				if ( !give_all )
					return;
			}

			if ( give_all )
			{
				for ( i = 1; i < GameBase.game.num_items; i++ )
				{
					it = GameItemList.itemlist[i];
					if ( it.pickup != null )
						continue;
					if ( ( it.flags & ( Defines.IT_ARMOR | Defines.IT_WEAPON | Defines.IT_AMMO ) ) != 0 )
						continue;
					ent.client.pers.inventory[i] = 1;
				}

				return;
			}

			it = GameItems.FindItem( name );
			if ( it == null )
			{
				name = Cmd.Argv( 1 );
				it = GameItems.FindItem( name );
				if ( it == null )
				{
					SV_GAME.PF_cprintf( ent, Defines.PRINT_HIGH, "unknown item\\n" );
					return;
				}
			}

			if ( it.pickup == null )
			{
				SV_GAME.PF_cprintf( ent, Defines.PRINT_HIGH, "non-pickup item\\n" );
				return;
			}

			index = GameItems.ITEM_INDEX( it );
			if ( ( it.flags & Defines.IT_AMMO ) != 0 )
			{
				if ( Cmd.Argc() == 3 )
					ent.client.pers.inventory[index] = Lib.Atoi( Cmd.Argv( 2 ) );
				else
					ent.client.pers.inventory[index] += it.quantity;
			}
			else
			{
				it_ent = GameUtil.G_Spawn();
				it_ent.classname = it.classname;
				GameItems.SpawnItem( it_ent, it );
				GameItems.Touch_Item( it_ent, ent, GameBase.dummyplane, null );
				if ( it_ent.inuse )
					GameUtil.G_FreeEdict( it_ent );
			}
		}

		public static void God_f( edict_t ent )
		{
			String msg;
			if ( GameBase.deathmatch.value != 0 && GameBase.sv_cheats.value == 0 )
			{
				SV_GAME.PF_cprintfhigh( ent, "You must run the server with '+set cheats 1' to enable this command.\\n" );
				return;
			}

			ent.flags ^= Defines.FL_GODMODE;
			if ( 0 == ( ent.flags & Defines.FL_GODMODE ) )
				msg = "godmode OFF\\n";
			else
				msg = "godmode ON\\n";
			SV_GAME.PF_cprintf( ent, Defines.PRINT_HIGH, msg );
		}

		public static void Notarget_f( edict_t ent )
		{
			String msg;
			if ( GameBase.deathmatch.value != 0 && GameBase.sv_cheats.value == 0 )
			{
				SV_GAME.PF_cprintfhigh( ent, "You must run the server with '+set cheats 1' to enable this command.\\n" );
				return;
			}

			ent.flags ^= Defines.FL_NOTARGET;
			if ( 0 == ( ent.flags & Defines.FL_NOTARGET ) )
				msg = "notarget OFF\\n";
			else
				msg = "notarget ON\\n";
			SV_GAME.PF_cprintfhigh( ent, msg );
		}

		public static void Noclip_f( edict_t ent )
		{
			String msg;
			if ( GameBase.deathmatch.value != 0 && GameBase.sv_cheats.value == 0 )
			{
				SV_GAME.PF_cprintfhigh( ent, "You must run the server with '+set cheats 1' to enable this command.\\n" );
				return;
			}

			if ( ent.movetype == Defines.MOVETYPE_NOCLIP )
			{
				ent.movetype = Defines.MOVETYPE_WALK;
				msg = "noclip OFF\\n";
			}
			else
			{
				ent.movetype = Defines.MOVETYPE_NOCLIP;
				msg = "noclip ON\\n";
			}

			SV_GAME.PF_cprintfhigh( ent, msg );
		}

		public static void Use_f( edict_t ent )
		{
			Int32 index;
			gitem_t it;
			String s;
			s = Cmd.Args();
			it = GameItems.FindItem( s );
			Com.Dprintln( "using:" + s );
			if ( it == null )
			{
				SV_GAME.PF_cprintfhigh( ent, "unknown item: " + s + "\\n" );
				return;
			}

			if ( it.use == null )
			{
				SV_GAME.PF_cprintfhigh( ent, "Item is not usable.\\n" );
				return;
			}

			index = GameItems.ITEM_INDEX( it );
			if ( 0 == ent.client.pers.inventory[index] )
			{
				SV_GAME.PF_cprintfhigh( ent, "Out of item: " + s + "\\n" );
				return;
			}

			it.use.Use( ent, it );
		}

		public static void Drop_f( edict_t ent )
		{
			Int32 index;
			gitem_t it;
			String s;
			s = Cmd.Args();
			it = GameItems.FindItem( s );
			if ( it == null )
			{
				SV_GAME.PF_cprintfhigh( ent, "unknown item: " + s + "\\n" );
				return;
			}

			if ( it.drop == null )
			{
				SV_GAME.PF_cprintf( ent, Defines.PRINT_HIGH, "Item is not dropable.\\n" );
				return;
			}

			index = GameItems.ITEM_INDEX( it );
			if ( 0 == ent.client.pers.inventory[index] )
			{
				SV_GAME.PF_cprintfhigh( ent, "Out of item: " + s + "\\n" );
				return;
			}

			it.drop.Drop( ent, it );
		}

		public static void Inven_f( edict_t ent )
		{
			Int32 i;
			gclient_t cl;
			cl = ent.client;
			cl.showscores = false;
			cl.showhelp = false;
			if ( cl.showinventory )
			{
				cl.showinventory = false;
				return;
			}

			cl.showinventory = true;
			GameBase.gi.WriteByte( Defines.svc_inventory );
			for ( i = 0; i < Defines.MAX_ITEMS; i++ )
			{
				GameBase.gi.WriteShort( cl.pers.inventory[i] );
			}

			GameBase.gi.Unicast( ent, true );
		}

		public static void InvUse_f( edict_t ent )
		{
			gitem_t it;
			Cmd.ValidateSelectedItem( ent );
			if ( ent.client.pers.selected_item == -1 )
			{
				SV_GAME.PF_cprintfhigh( ent, "No item to use.\\n" );
				return;
			}

			it = GameItemList.itemlist[ent.client.pers.selected_item];
			if ( it.use == null )
			{
				SV_GAME.PF_cprintfhigh( ent, "Item is not usable.\\n" );
				return;
			}

			it.use.Use( ent, it );
		}

		public static void WeapPrev_f( edict_t ent )
		{
			gclient_t cl;
			Int32 i, index;
			gitem_t it;
			Int32 selected_weapon;
			cl = ent.client;
			if ( cl.pers.weapon == null )
				return;
			selected_weapon = GameItems.ITEM_INDEX( cl.pers.weapon );
			for ( i = 1; i <= Defines.MAX_ITEMS; i++ )
			{
				index = ( selected_weapon + i ) % Defines.MAX_ITEMS;
				if ( 0 == cl.pers.inventory[index] )
					continue;
				it = GameItemList.itemlist[index];
				if ( it.use == null )
					continue;
				if ( 0 == ( it.flags & Defines.IT_WEAPON ) )
					continue;
				it.use.Use( ent, it );
				if ( cl.pers.weapon == it )
					return;
			}
		}

		public static void WeapNext_f( edict_t ent )
		{
			gclient_t cl;
			Int32 i, index;
			gitem_t it;
			Int32 selected_weapon;
			cl = ent.client;
			if ( null == cl.pers.weapon )
				return;
			selected_weapon = GameItems.ITEM_INDEX( cl.pers.weapon );
			for ( i = 1; i <= Defines.MAX_ITEMS; i++ )
			{
				index = ( selected_weapon + Defines.MAX_ITEMS - i ) % Defines.MAX_ITEMS;
				if ( index == 0 )
					index++;
				if ( 0 == cl.pers.inventory[index] )
					continue;
				it = GameItemList.itemlist[index];
				if ( null == it.use )
					continue;
				if ( 0 == ( it.flags & Defines.IT_WEAPON ) )
					continue;
				it.use.Use( ent, it );
				if ( cl.pers.weapon == it )
					return;
			}
		}

		public static void WeapLast_f( edict_t ent )
		{
			gclient_t cl;
			Int32 index;
			gitem_t it;
			cl = ent.client;
			if ( null == cl.pers.weapon || null == cl.pers.lastweapon )
				return;
			index = GameItems.ITEM_INDEX( cl.pers.lastweapon );
			if ( 0 == cl.pers.inventory[index] )
				return;
			it = GameItemList.itemlist[index];
			if ( null == it.use )
				return;
			if ( 0 == ( it.flags & Defines.IT_WEAPON ) )
				return;
			it.use.Use( ent, it );
		}

		public static void InvDrop_f( edict_t ent )
		{
			gitem_t it;
			Cmd.ValidateSelectedItem( ent );
			if ( ent.client.pers.selected_item == -1 )
			{
				SV_GAME.PF_cprintfhigh( ent, "No item to drop.\\n" );
				return;
			}

			it = GameItemList.itemlist[ent.client.pers.selected_item];
			if ( it.drop == null )
			{
				SV_GAME.PF_cprintfhigh( ent, "Item is not dropable.\\n" );
				return;
			}

			it.drop.Drop( ent, it );
		}

		public static void Score_f( edict_t ent )
		{
			ent.client.showinventory = false;
			ent.client.showhelp = false;
			if ( 0 == GameBase.deathmatch.value && 0 == GameBase.coop.value )
				return;
			if ( ent.client.showscores )
			{
				ent.client.showscores = false;
				return;
			}

			ent.client.showscores = true;
			PlayerHud.DeathmatchScoreboard( ent );
		}

		public static void Help_f( edict_t ent )
		{
			if ( GameBase.deathmatch.value != 0 )
			{
				Score_f( ent );
				return;
			}

			ent.client.showinventory = false;
			ent.client.showscores = false;
			if ( ent.client.showhelp && ( ent.client.pers.game_helpchanged == GameBase.game.helpchanged ) )
			{
				ent.client.showhelp = false;
				return;
			}

			ent.client.showhelp = true;
			ent.client.pers.helpchanged = 0;
			PlayerHud.HelpComputer( ent );
		}

		public static void Kill_f( edict_t ent )
		{
			if ( ( GameBase.level.time - ent.client.respawn_time ) < 5 )
				return;
			ent.flags &= ~Defines.FL_GODMODE;
			ent.health = 0;
			GameBase.meansOfDeath = Defines.MOD_SUICIDE;
			PlayerClient.player_die.Die( ent, ent, ent, 100000, Globals.vec3_origin );
		}

		public static void PutAway_f( edict_t ent )
		{
			ent.client.showscores = false;
			ent.client.showhelp = false;
			ent.client.showinventory = false;
		}

		public static void Players_f( edict_t ent )
		{
			Int32 i;
			Int32 count;
			String small;
			String large;
			Int32[] index = new Int32[256];
			count = 0;
			for ( i = 0; i < GameBase.maxclients.value; i++ )
			{
				if ( GameBase.game.clients[i].pers.connected )
				{
					index[count] = i;
					count++;
				}
			}

			index = index.OrderBy( p => p, Cmd.PlayerSort ).ToArray();

			large = "";
			for ( i = 0; i < count; i++ )
			{
				small = GameBase.game.clients[index[i]].ps.stats[Defines.STAT_FRAGS] + " " + GameBase.game.clients[index[i]].pers.netname + "\\n";
				if ( small.Length + large.Length > 1024 - 100 )
				{
					large += "...\\n";
					break;
				}

				large += small;
			}

			SV_GAME.PF_cprintfhigh( ent, large + "\\n" + count + " players\\n" );
		}

		public static void Wave_f( edict_t ent )
		{
			Int32 i;
			i = Lib.Atoi( Cmd.Argv( 1 ) );
			if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
				return;
			if ( ent.client.anim_priority > Defines.ANIM_WAVE )
				return;
			ent.client.anim_priority = Defines.ANIM_WAVE;
			switch ( i )
			{
				case 0:
					SV_GAME.PF_cprintfhigh( ent, "flipoff\\n" );
					ent.s.frame = M_Player.FRAME_flip01 - 1;
					ent.client.anim_end = M_Player.FRAME_flip12;
					break;
				case 1:
					SV_GAME.PF_cprintfhigh( ent, "salute\\n" );
					ent.s.frame = M_Player.FRAME_salute01 - 1;
					ent.client.anim_end = M_Player.FRAME_salute11;
					break;
				case 2:
					SV_GAME.PF_cprintfhigh( ent, "taunt\\n" );
					ent.s.frame = M_Player.FRAME_taunt01 - 1;
					ent.client.anim_end = M_Player.FRAME_taunt17;
					break;
				case 3:
					SV_GAME.PF_cprintfhigh( ent, "wave\\n" );
					ent.s.frame = M_Player.FRAME_wave01 - 1;
					ent.client.anim_end = M_Player.FRAME_wave11;
					break;
				case 4:
				default:
					SV_GAME.PF_cprintfhigh( ent, "point\\n" );
					ent.s.frame = M_Player.FRAME_point01 - 1;
					ent.client.anim_end = M_Player.FRAME_point12;
					break;
			}
		}

		public static void ShowPosition_f( edict_t ent )
		{
			SV_GAME.PF_cprintfhigh( ent, "pos=" + Lib.Vtofsbeaty( ent.s.origin ) + "\\n" );
		}

		public static void Say_f( edict_t ent, Boolean team, Boolean arg0 )
		{
			Int32 i, j;
			edict_t other;
			String text;
			gclient_t cl;
			if ( Cmd.Argc() < 2 && !arg0 )
				return;
			if ( 0 == ( ( Int32 ) ( GameBase.dmflags.value ) & ( Defines.DF_MODELTEAMS | Defines.DF_SKINTEAMS ) ) )
				team = false;
			if ( team )
				text = "(" + ent.client.pers.netname + "): ";
			else
				text = "" + ent.client.pers.netname + ": ";
			if ( arg0 )
			{
				text += Cmd.Argv( 0 );
				text += " ";
				text += Cmd.Args();
			}
			else
			{
				if ( Cmd.Args().StartsWith( "\\\"" ) )
					text += Cmd.Args().Substring( 1, Cmd.Args().Length - 1 );
				else
					text += Cmd.Args();
			}

			if ( text.Length > 150 )
				text = text.Substring( 0, 150 );
			text += "\\n";
			if ( GameBase.flood_msgs.value != 0 )
			{
				cl = ent.client;
				if ( GameBase.level.time < cl.flood_locktill )
				{
					SV_GAME.PF_cprintfhigh( ent, "You can't talk for " + ( Int32 ) ( cl.flood_locktill - GameBase.level.time ) + " more seconds\\n" );
					return;
				}

				i = ( Int32 ) ( cl.flood_whenhead - GameBase.flood_msgs.value + 1 );
				if ( i < 0 )
					i = ( 10 ) + i;
				if ( cl.flood_when[i] != 0 && GameBase.level.time - cl.flood_when[i] < GameBase.flood_persecond.value )
				{
					cl.flood_locktill = GameBase.level.time + GameBase.flood_waitdelay.value;
					SV_GAME.PF_cprintf( ent, Defines.PRINT_CHAT, "Flood protection:  You can't talk for " + ( Int32 ) GameBase.flood_waitdelay.value + " seconds.\\n" );
					return;
				}

				cl.flood_whenhead = ( cl.flood_whenhead + 1 ) % 10;
				cl.flood_when[cl.flood_whenhead] = GameBase.level.time;
			}

			if ( Globals.dedicated.value != 0 )
				SV_GAME.PF_cprintf( null, Defines.PRINT_CHAT, "" + text + "" );
			for ( j = 1; j <= GameBase.game.maxclients; j++ )
			{
				other = GameBase.g_edicts[j];
				if ( !other.inuse )
					continue;
				if ( other.client == null )
					continue;
				if ( team )
				{
					if ( !GameUtil.OnSameTeam( ent, other ) )
						continue;
				}

				SV_GAME.PF_cprintf( other, Defines.PRINT_CHAT, "" + text + "" );
			}
		}

		public static void PlayerList_f( edict_t ent )
		{
			Int32 i;
			String st;
			String text;
			edict_t e2;
			text = "";
			for ( i = 0; i < GameBase.maxclients.value; i++ )
			{
				e2 = GameBase.g_edicts[1 + i];
				if ( !e2.inuse )
					continue;
				st = "" + ( GameBase.level.framenum - e2.client.resp.enterframe ) / 600 + ":" + ( ( GameBase.level.framenum - e2.client.resp.enterframe ) % 600 ) / 10 + " " + e2.client.ping + " " + e2.client.resp.score + " " + e2.client.pers.netname + " " + ( e2.client.resp.spectator ? " (spectator)" : "" ) + "\\n";
				if ( text.Length + st.Length > 1024 - 50 )
				{
					text += "And more...\\n";
					SV_GAME.PF_cprintfhigh( ent, "" + text + "" );
					return;
				}

				text += st;
			}

			SV_GAME.PF_cprintfhigh( ent, text );
		}

		public static void ForwardToServer( )
		{
			String cmd;
			cmd = Cmd.Argv( 0 );
			if ( Globals.cls.state <= Defines.ca_connected || cmd[0] == '-' || cmd[0] == '+' )
			{
				Com.Printf( "Unknown command \\\"" + cmd + "\\\"\\n" );
				return;
			}

			MSG.WriteByte( Globals.cls.netchan.message, Defines.clc_stringcmd );
			SZ.Print( Globals.cls.netchan.message, cmd );
			if ( Cmd.Argc() > 1 )
			{
				SZ.Print( Globals.cls.netchan.message, " " );
				SZ.Print( Globals.cls.netchan.message, Cmd.Args() );
			}
		}

		public static ArrayList CompleteCommand( String partial )
		{
			ArrayList cmds = new ArrayList();
			for ( cmd_function_t cmd = cmd_functions; cmd != null; cmd = cmd.next )
				if ( cmd.name.StartsWith( partial ) )
					cmds.Add( cmd.name );
			for ( cmdalias_t a = Globals.cmd_alias; a != null; a = a.next )
				if ( a.name.StartsWith( partial ) )
					cmds.Add( a.name );
			return cmds;
		}

		public static void ClientCommand( edict_t ent )
		{
			String cmd;
			if ( ent.client == null )
				return;
			cmd = GameBase.gi.Argv( 0 ).ToLower();
			if ( cmd.Equals( "players" ) )
			{
				Players_f( ent );
				return;
			}

			if ( cmd.Equals( "say" ) )
			{
				Say_f( ent, false, false );
				return;
			}

			if ( cmd.Equals( "say_team" ) )
			{
				Say_f( ent, true, false );
				return;
			}

			if ( cmd.Equals( "score" ) )
			{
				Score_f( ent );
				return;
			}

			if ( cmd.Equals( "help" ) )
			{
				Help_f( ent );
				return;
			}

			if ( GameBase.level.intermissiontime != 0 )
				return;
			if ( cmd.Equals( "use" ) )
				Use_f( ent );
			else if ( cmd.Equals( "drop" ) )
				Drop_f( ent );
			else if ( cmd.Equals( "give" ) )
				Give_f( ent );
			else if ( cmd.Equals( "god" ) )
				God_f( ent );
			else if ( cmd.Equals( "notarget" ) )
				Notarget_f( ent );
			else if ( cmd.Equals( "noclip" ) )
				Noclip_f( ent );
			else if ( cmd.Equals( "inven" ) )
				Inven_f( ent );
			else if ( cmd.Equals( "invnext" ) )
				GameItems.SelectNextItem( ent, -1 );
			else if ( cmd.Equals( "invprev" ) )
				GameItems.SelectPrevItem( ent, -1 );
			else if ( cmd.Equals( "invnextw" ) )
				GameItems.SelectNextItem( ent, Defines.IT_WEAPON );
			else if ( cmd.Equals( "invprevw" ) )
				GameItems.SelectPrevItem( ent, Defines.IT_WEAPON );
			else if ( cmd.Equals( "invnextp" ) )
				GameItems.SelectNextItem( ent, Defines.IT_POWERUP );
			else if ( cmd.Equals( "invprevp" ) )
				GameItems.SelectPrevItem( ent, Defines.IT_POWERUP );
			else if ( cmd.Equals( "invuse" ) )
				InvUse_f( ent );
			else if ( cmd.Equals( "invdrop" ) )
				InvDrop_f( ent );
			else if ( cmd.Equals( "weapprev" ) )
				WeapPrev_f( ent );
			else if ( cmd.Equals( "weapnext" ) )
				WeapNext_f( ent );
			else if ( cmd.Equals( "weaplast" ) )
				WeapLast_f( ent );
			else if ( cmd.Equals( "kill" ) )
				Kill_f( ent );
			else if ( cmd.Equals( "putaway" ) )
				PutAway_f( ent );
			else if ( cmd.Equals( "wave" ) )
				Wave_f( ent );
			else if ( cmd.Equals( "playerlist" ) )
				PlayerList_f( ent );
			else if ( cmd.Equals( "showposition" ) )
				ShowPosition_f( ent );
			else
				Say_f( ent, false, true );
		}

		public static void ValidateSelectedItem( edict_t ent )
		{
			gclient_t cl = ent.client;
			if ( cl.pers.inventory[cl.pers.selected_item] != 0 )
				return;
			GameItems.SelectNextItem( ent, -1 );
		}
	}
}
using J2N.Text;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Sound;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using static Q2Sharp.Qcommon.Com;

namespace Q2Sharp.Client
{
	public abstract class keyfunc_t
	{
		public abstract String Execute( Int32 key );
	}

	public sealed class Menu : Key
	{
		public static Int32 m_main_cursor;
		public static readonly Int32 NUM_CURSOR_FRAMES = 15;
		public static readonly String menu_in_sound = "misc/menu1.wav";
		public static readonly String menu_move_sound = "misc/menu2.wav";
		public static readonly String menu_out_sound = "misc/menu3.wav";
		public static Boolean m_entersound;
		public static xcommand_t m_drawfunc;
		public static keyfunc_t m_keyfunc;
		public static readonly Int32 MAX_MENU_DEPTH = 8;
		public class menulayer_t
		{
			public xcommand_t draw;
			public keyfunc_t key;
		}

		public class menuframework_s
		{
			public Int32 x, y;
			public Int32 cursor;
			public Int32 nitems;
			public Int32 nslots;
			public menucommon_s[] items = new menucommon_s[64];
			public String statusbar;
			public mcallback cursordraw;
		}

		public abstract class mcallback
		{
			public abstract void Execute( Object self );
		}

		public class menucommon_s
		{
			public Int32 type;
			public String name = "";
			public Int32 x, y;
			public menuframework_s parent;
			public Int32 cursor_offset;
			public Int32[] localdata = new[] { 0, 0, 0, 0 };
			public Int32 flags;
			public Int32 n = -1;
			public String statusbar;
			public mcallback callback;
			public mcallback statusbarfunc;
			public mcallback ownerdraw;
			public mcallback cursordraw;
		}

		public class menufield_s : menucommon_s
		{
			public StringBuffer buffer;
			public Int32 cursor;
			public Int32 length;
			public Int32 visible_length;
			public Int32 visible_offset;
		}

		public class menuslider_s : menucommon_s
		{
			public Single minvalue;
			public Single maxvalue;
			public Single curvalue;
			public Single range;
		}

		public class menulist_s : menucommon_s
		{
			public Int32 curvalue;
			public String[] itemnames;
		}

		public class menuaction_s : menucommon_s
		{
		}

		public class menuseparator_s : menucommon_s
		{
		}

		public static menulayer_t[] m_layers = new menulayer_t[MAX_MENU_DEPTH];
		public static Int32 m_menudepth;
		public static void Banner( String name )
		{
			Globals.re.DrawGetPicSize( out var dim, name );
			Globals.re.DrawPic( viddef.GetWidth() / 2 - dim.Width / 2, viddef.GetHeight() / 2 - 110, name );
		}

		public static void PushMenu( xcommand_t draw, keyfunc_t key )
		{
			Int32 i;
			if ( Cvar.VariableValue( "maxclients" ) == 1 && Globals.server_state != 0 )
				Cvar.Set( "paused", "1" );
			for ( i = 0; i < m_menudepth; i++ )
				if ( m_layers[i].draw == draw && m_layers[i].key == key )
				{
					m_menudepth = i;
				}

			if ( i == m_menudepth )
			{
				if ( m_menudepth >= MAX_MENU_DEPTH )
					Com.Error( ERR_FATAL, "PushMenu: MAX_MENU_DEPTH" );
				m_layers[m_menudepth].draw = draw;
				m_layers[m_menudepth].key = key;
			}

			m_menudepth++;
			m_drawfunc = draw;
			m_keyfunc = key;
			m_entersound = true;
			cls.key_dest = key_menu;
		}

		public static void ForceMenuOff( )
		{
			m_drawfunc = null;
			m_keyfunc = null;
			cls.key_dest = key_game;
			m_menudepth = 0;
			Key.ClearStates();
			Cvar.Set( "paused", "0" );
		}

		public static void PopMenu( )
		{
			S.StartLocalSound( menu_out_sound );
			m_menudepth--;
			if ( m_menudepth < 0 )
				Com.Error( ERR_FATAL, "PopMenu: depth < 1" );
			if ( 0 < m_menudepth )
			{
				m_drawfunc = m_layers[m_menudepth - 1].draw;
				m_keyfunc = m_layers[m_menudepth - 1].key;
			}

			if ( 0 == m_menudepth )
				ForceMenuOff();
		}

		static String Default_MenuKey( menuframework_s m, Int32 key )
		{
			String sound = null;
			menucommon_s item;
			if ( m != null )
			{
				if ( ( item = ( ( menucommon_s ) Menu_ItemAtCursor( m ) ) ) != null )
				{
					if ( item.type == MTYPE_FIELD )
					{
						if ( Field_Key( ( menufield_s ) item, key ) )
							return null;
					}
				}
			}

			switch ( key )

			{
				case K_ESCAPE:
					PopMenu();
					return menu_out_sound;
				case K_KP_UPARROW:
				case K_UPARROW:
					if ( m != null )
					{
						m.cursor--;
						Menu_AdjustCursor( m, -1 );
						sound = menu_move_sound;
					}

					break;
				case K_TAB:
					if ( m != null )
					{
						m.cursor++;
						Menu_AdjustCursor( m, 1 );
						sound = menu_move_sound;
					}

					break;
				case K_KP_DOWNARROW:
				case K_DOWNARROW:
					if ( m != null )
					{
						m.cursor++;
						Menu_AdjustCursor( m, 1 );
						sound = menu_move_sound;
					}

					break;
				case K_KP_LEFTARROW:
				case K_LEFTARROW:
					if ( m != null )
					{
						Menu_SlideItem( m, -1 );
						sound = menu_move_sound;
					}

					break;
				case K_KP_RIGHTARROW:
				case K_RIGHTARROW:
					if ( m != null )
					{
						Menu_SlideItem( m, 1 );
						sound = menu_move_sound;
					}

					break;
				case K_MOUSE1:
				case K_MOUSE2:
				case K_MOUSE3:
				case K_JOY1:
				case K_JOY2:
				case K_JOY3:
				case K_JOY4:
				case K_KP_ENTER:
				case K_ENTER:
					if ( m != null )
						Menu_SelectItem( m );
					sound = menu_move_sound;
					break;
			}

			return sound;
		}

		public static void DrawCharacter( Int32 cx, Int32 cy, Int32 num )
		{
			re.DrawChar( cx + ( ( viddef.GetWidth() - 320 ) >> 1 ), cy + ( ( viddef.GetHeight() - 240 ) >> 1 ), num );
		}

		public static void Print( Int32 cx, Int32 cy, String str )
		{
			for ( var n = 0; n < str.Length; n++ )
			{
				DrawCharacter( cx, cy, str[n] + 128 );
				cx += 8;
			}
		}

		public static void PrintWhite( Int32 cx, Int32 cy, String str )
		{
			for ( var n = 0; n < str.Length; n++ )
			{
				DrawCharacter( cx, cy, str[n] );
				cx += 8;
			}
		}

		public static void DrawPic( Int32 x, Int32 y, String pic )
		{
			re.DrawPic( x + ( ( viddef.GetWidth() - 320 ) >> 1 ), y + ( ( viddef.GetHeight() - 240 ) >> 1 ), pic );
		}

		static Boolean cached;
		static void DrawCursor( Int32 x, Int32 y, Int32 f )
		{
			f = Math.Abs( f );
			if ( !cached )
			{
				for ( var i = 0; i < NUM_CURSOR_FRAMES; i++ )
				{
					re.RegisterPic( "m_cursor" + i );
				}

				cached = true;
			}

			re.DrawPic( x, y, "m_cursor" + f );
		}

		public static void DrawTextBox( Int32 x, Int32 y, Int32 width, Int32 lines )
		{
			Int32 cx, cy;
			Int32 n;
			cx = x;
			cy = y;
			DrawCharacter( cx, cy, 1 );
			for ( n = 0; n < lines; n++ )
			{
				cy += 8;
				DrawCharacter( cx, cy, 4 );
			}

			DrawCharacter( cx, cy + 8, 7 );
			cx += 8;
			while ( width > 0 )
			{
				cy = y;
				DrawCharacter( cx, cy, 2 );
				for ( n = 0; n < lines; n++ )
				{
					cy += 8;
					DrawCharacter( cx, cy, 5 );
				}

				DrawCharacter( cx, cy + 8, 8 );
				width -= 1;
				cx += 8;
			}

			cy = y;
			DrawCharacter( cx, cy, 3 );
			for ( n = 0; n < lines; n++ )
			{
				cy += 8;
				DrawCharacter( cx, cy, 6 );
			}

			DrawCharacter( cx, cy + 8, 9 );
		}

		static readonly Int32 MAIN_ITEMS = 5;
		static xcommand_t Main_Draw_Cmd = new Anonymousxcommand_t();
		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				Main_Draw();
			}
		}

		static void Main_Draw( )
		{
			Int32 i;
			Int32 w, h;
			Int32 ystart;
			Int32 xoffset;
			var widest = -1;
			var totalheight = 0;
			String litname;
			String[] names = new[] { "m_main_game", "m_main_multiplayer", "m_main_options", "m_main_video", "m_main_quit" };
			Size dim = new Size();
			for ( i = 0; i < names.Length; i++ )
			{
				Globals.re.DrawGetPicSize( out dim, names[i] );
				w = dim.Width;
				h = dim.Height;
				if ( w > widest )
					widest = w;
				totalheight += ( h + 12 );
			}

			ystart = ( Globals.viddef.GetHeight() / 2 - 110 );
			xoffset = ( Globals.viddef.GetWidth() - widest + 70 ) / 2;
			for ( i = 0; i < names.Length; i++ )
			{
				if ( i != m_main_cursor )
					Globals.re.DrawPic( xoffset, ystart + i * 40 + 13, names[i] );
			}

			litname = names[m_main_cursor] + "_sel";
			Globals.re.DrawPic( xoffset, ystart + m_main_cursor * 40 + 13, litname );
			DrawCursor( xoffset - 25, ystart + m_main_cursor * 40 + 11, ( Int32 ) ( ( Globals.cls.realtime / 100 ) ) % NUM_CURSOR_FRAMES );
			Globals.re.DrawGetPicSize( out dim, "m_main_plaque" );
			w = dim.Width;
			h = dim.Height;
			Globals.re.DrawPic( xoffset - 30 - w, ystart, "m_main_plaque" );
			Globals.re.DrawPic( xoffset - 30 - w, ystart + h + 5, "m_main_logo" );
		}

		static keyfunc_t Main_Key_Cmd = new Anonymouskeyfunc_t();
		private sealed class Anonymouskeyfunc_t : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Main_Key( key );
			}
		}

		static String Main_Key( Int32 key )
		{
			var sound = menu_move_sound;
			switch ( key )

			{
				case Key.K_ESCAPE:
					PopMenu();
					break;
				case Key.K_KP_DOWNARROW:
				case Key.K_DOWNARROW:
					if ( ++m_main_cursor >= MAIN_ITEMS )
						m_main_cursor = 0;
					return sound;
				case Key.K_KP_UPARROW:
				case Key.K_UPARROW:
					if ( --m_main_cursor < 0 )
						m_main_cursor = MAIN_ITEMS - 1;
					return sound;
				case Key.K_KP_ENTER:
				case Key.K_ENTER:
					m_entersound = true;
					switch ( m_main_cursor )

					{
						case 0:
							Menu_Game_f();
							break;
						case 1:
							Menu_Multiplayer_f();
							break;
						case 2:
							Menu_Options_f();
							break;
						case 3:
							Menu_Video_f();
							break;
						case 4:
							Menu_Quit_f();
							break;
					}
					break;
			}

			return null;
		}

		static xcommand_t Menu_Main = new Anonymousxcommand_t1();
		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_Main_f();
			}
		}

		public static void Menu_Main_f( )
		{
			PushMenu( new Anonymousxcommand_t2(), new Anonymouskeyfunc_t1() );
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				Main_Draw();
			}
		}

		private sealed class Anonymouskeyfunc_t1 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Main_Key( key );
			}
		}

		static menuframework_s s_multiplayer_menu = new menuframework_s();
		static menuaction_s s_join_network_server_action = new menuaction_s();
		static menuaction_s s_start_network_server_action = new menuaction_s();
		static menuaction_s s_player_setup_action = new menuaction_s();
		static void Multiplayer_MenuDraw( )
		{
			Banner( "m_banner_multiplayer" );
			Menu_AdjustCursor( s_multiplayer_menu, 1 );
			Menu_Draw( s_multiplayer_menu );
		}

		static void PlayerSetupFunc( Object unused )
		{
			Menu_PlayerConfig_f();
		}

		static void JoinNetworkServerFunc( Object unused )
		{
			Menu_JoinServer_f();
		}

		static void StartNetworkServerFunc( Object unused )
		{
			Menu_StartServer_f();
		}

		static void Multiplayer_MenuInit( )
		{
			s_multiplayer_menu.x = ( Int32 ) ( viddef.GetWidth() * 0.5F - 64 );
			s_multiplayer_menu.nitems = 0;
			s_join_network_server_action.type = MTYPE_ACTION;
			s_join_network_server_action.flags = QMF_LEFT_JUSTIFY;
			s_join_network_server_action.x = 0;
			s_join_network_server_action.y = 0;
			s_join_network_server_action.name = " join network server";
			s_join_network_server_action.callback = new Anonymousmcallback();
			s_start_network_server_action.type = MTYPE_ACTION;
			s_start_network_server_action.flags = QMF_LEFT_JUSTIFY;
			s_start_network_server_action.x = 0;
			s_start_network_server_action.y = 10;
			s_start_network_server_action.name = " start network server";
			s_start_network_server_action.callback = new Anonymousmcallback1();
			s_player_setup_action.type = MTYPE_ACTION;
			s_player_setup_action.flags = QMF_LEFT_JUSTIFY;
			s_player_setup_action.x = 0;
			s_player_setup_action.y = 20;
			s_player_setup_action.name = " player setup";
			s_player_setup_action.callback = new Anonymousmcallback2();
			Menu_AddItem( s_multiplayer_menu, s_join_network_server_action );
			Menu_AddItem( s_multiplayer_menu, s_start_network_server_action );
			Menu_AddItem( s_multiplayer_menu, s_player_setup_action );
			Menu_SetStatusBar( s_multiplayer_menu, null );
			Menu_Center( s_multiplayer_menu );
		}

		private sealed class Anonymousmcallback : mcallback
		{
			public override void Execute( Object o )
			{
				JoinNetworkServerFunc( o );
			}
		}

		private sealed class Anonymousmcallback1 : mcallback
		{
			public override void Execute( Object o )
			{
				StartNetworkServerFunc( o );
			}
		}

		private sealed class Anonymousmcallback2 : mcallback
		{
			public override void Execute( Object o )
			{
				PlayerSetupFunc( o );
			}
		}

		static String Multiplayer_MenuKey( Int32 key )
		{
			return Default_MenuKey( s_multiplayer_menu, key );
		}

		static xcommand_t Menu_Multiplayer = new Anonymousxcommand_t3();
		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_Multiplayer_f();
			}
		}

		static void Menu_Multiplayer_f( )
		{
			Multiplayer_MenuInit();
			PushMenu( new Anonymousxcommand_t4(), new Anonymouskeyfunc_t2() );
		}

		private sealed class Anonymousxcommand_t4 : xcommand_t
		{
			public override void Execute( )
			{
				Multiplayer_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t2 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Multiplayer_MenuKey( key );
			}
		}

		static String[][] bindnames = new String[][] { new[] { "+attack", "attack" }, new[] { "weapnext", "next weapon" }, new[] { "+forward", "walk forward" }, new[] { "+back", "backpedal" }, new[] { "+left", "turn left" }, new[] { "+right", "turn right" }, new[] { "+speed", "run" }, new[] { "+moveleft", "step left" }, new[] { "+moveright", "step right" }, new[] { "+strafe", "sidestep" }, new[] { "+lookup", "look up" }, new[] { "+lookdown", "look down" }, new[] { "centerview", "center view" }, new[] { "+mlook", "mouse look" }, new[] { "+klook", "keyboard look" }, new[] { "+moveup", "up / jump" }, new[] { "+movedown", "down / crouch" }, new[] { "inven", "inventory" }, new[] { "invuse", "use item" }, new[] { "invdrop", "drop item" }, new[] { "invprev", "prev item" }, new[] { "invnext", "next item" }, new[] { "cmd help", "help computer" }, new String[] { null, null } };
		Int32 keys_cursor;
		static Boolean bind_grab;
		static menuframework_s s_keys_menu = new menuframework_s();
		static menuaction_s s_keys_attack_action = new menuaction_s();
		static menuaction_s s_keys_change_weapon_action = new menuaction_s();
		static menuaction_s s_keys_walk_forward_action = new menuaction_s();
		static menuaction_s s_keys_backpedal_action = new menuaction_s();
		static menuaction_s s_keys_turn_left_action = new menuaction_s();
		static menuaction_s s_keys_turn_right_action = new menuaction_s();
		static menuaction_s s_keys_run_action = new menuaction_s();
		static menuaction_s s_keys_step_left_action = new menuaction_s();
		static menuaction_s s_keys_step_right_action = new menuaction_s();
		static menuaction_s s_keys_sidestep_action = new menuaction_s();
		static menuaction_s s_keys_look_up_action = new menuaction_s();
		static menuaction_s s_keys_look_down_action = new menuaction_s();
		static menuaction_s s_keys_center_view_action = new menuaction_s();
		static menuaction_s s_keys_mouse_look_action = new menuaction_s();
		static menuaction_s s_keys_keyboard_look_action = new menuaction_s();
		static menuaction_s s_keys_move_up_action = new menuaction_s();
		static menuaction_s s_keys_move_down_action = new menuaction_s();
		static menuaction_s s_keys_inventory_action = new menuaction_s();
		static menuaction_s s_keys_inv_use_action = new menuaction_s();
		static menuaction_s s_keys_inv_drop_action = new menuaction_s();
		static menuaction_s s_keys_inv_prev_action = new menuaction_s();
		static menuaction_s s_keys_inv_next_action = new menuaction_s();
		static menuaction_s s_keys_help_computer_action = new menuaction_s();
		static void UnbindCommand( String command )
		{
			Int32 j;
			String b;
			for ( j = 0; j < 256; j++ )
			{
				b = keybindings[j];
				if ( b == null )
					continue;
				if ( b.Equals( command ) )
					Key.SetBinding( j, "" );
			}
		}

		static void FindKeysForCommand( String command, Int32[] twokeys )
		{
			Int32 count;
			Int32 j;
			String b;
			twokeys[0] = twokeys[1] = -1;
			count = 0;
			for ( j = 0; j < 256; j++ )
			{
				b = keybindings[j];
				if ( b == null )
					continue;
				if ( b.Equals( command ) )
				{
					twokeys[count] = j;
					count++;
					if ( count == 2 )
						break;
				}
			}
		}

		static void KeyCursorDrawFunc( menuframework_s menu )
		{
			if ( bind_grab )
				re.DrawChar( menu.x, menu.y + menu.cursor * 9, '=' );
			else
				re.DrawChar( menu.x, menu.y + menu.cursor * 9, 12 + ( ( Int32 ) ( Timer.Milliseconds() / 250 ) & 1 ) );
		}

		static void DrawKeyBindingFunc( Object self )
		{
			Int32[] keys = new[] { 0, 0 };
			menuaction_s a = ( menuaction_s ) self;
			FindKeysForCommand( bindnames[a.localdata[0]][0], keys );
			if ( keys[0] == -1 )
			{
				Menu_DrawString( a.x + a.parent.x + 16, a.y + a.parent.y, "???" );
			}
			else
			{
				Int32 x;
				String name;
				name = Key.KeynumToString( keys[0] );
				Menu_DrawString( a.x + a.parent.x + 16, a.y + a.parent.y, name );
				x = name.Length * 8;
				if ( keys[1] != -1 )
				{
					Menu_DrawString( a.x + a.parent.x + 24 + x, a.y + a.parent.y, "or" );
					Menu_DrawString( a.x + a.parent.x + 48 + x, a.y + a.parent.y, Key.KeynumToString( keys[1] ) );
				}
			}
		}

		static void KeyBindingFunc( Object self )
		{
			menuaction_s a = ( menuaction_s ) self;
			Int32[] keys = new[] { 0, 0 };
			FindKeysForCommand( bindnames[a.localdata[0]][0], keys );
			if ( keys[1] != -1 )
				UnbindCommand( bindnames[a.localdata[0]][0] );
			bind_grab = true;
			Menu_SetStatusBar( s_keys_menu, "press a key or button for this action" );
		}

		static void Keys_MenuInit( )
		{
			var y = 0;
			var i = 0;
			s_keys_menu.x = ( Int32 ) ( viddef.GetWidth() * 0.5 );
			s_keys_menu.nitems = 0;
			s_keys_menu.cursordraw = new Anonymousmcallback3();
			s_keys_attack_action.type = MTYPE_ACTION;
			s_keys_attack_action.flags = QMF_GRAYED;
			s_keys_attack_action.x = 0;
			s_keys_attack_action.y = y;
			s_keys_attack_action.ownerdraw = new Anonymousmcallback4();
			s_keys_attack_action.localdata[0] = i;
			s_keys_attack_action.name = bindnames[s_keys_attack_action.localdata[0]][1];
			s_keys_change_weapon_action.type = MTYPE_ACTION;
			s_keys_change_weapon_action.flags = QMF_GRAYED;
			s_keys_change_weapon_action.x = 0;
			s_keys_change_weapon_action.y = y += 9;
			s_keys_change_weapon_action.ownerdraw = new Anonymousmcallback5();
			s_keys_change_weapon_action.localdata[0] = ++i;
			s_keys_change_weapon_action.name = bindnames[s_keys_change_weapon_action.localdata[0]][1];
			s_keys_walk_forward_action.type = MTYPE_ACTION;
			s_keys_walk_forward_action.flags = QMF_GRAYED;
			s_keys_walk_forward_action.x = 0;
			s_keys_walk_forward_action.y = y += 9;
			s_keys_walk_forward_action.ownerdraw = new Anonymousmcallback6();
			s_keys_walk_forward_action.localdata[0] = ++i;
			s_keys_walk_forward_action.name = bindnames[s_keys_walk_forward_action.localdata[0]][1];
			s_keys_backpedal_action.type = MTYPE_ACTION;
			s_keys_backpedal_action.flags = QMF_GRAYED;
			s_keys_backpedal_action.x = 0;
			s_keys_backpedal_action.y = y += 9;
			s_keys_backpedal_action.ownerdraw = new Anonymousmcallback7();
			s_keys_backpedal_action.localdata[0] = ++i;
			s_keys_backpedal_action.name = bindnames[s_keys_backpedal_action.localdata[0]][1];
			s_keys_turn_left_action.type = MTYPE_ACTION;
			s_keys_turn_left_action.flags = QMF_GRAYED;
			s_keys_turn_left_action.x = 0;
			s_keys_turn_left_action.y = y += 9;
			s_keys_turn_left_action.ownerdraw = new Anonymousmcallback8();
			s_keys_turn_left_action.localdata[0] = ++i;
			s_keys_turn_left_action.name = bindnames[s_keys_turn_left_action.localdata[0]][1];
			s_keys_turn_right_action.type = MTYPE_ACTION;
			s_keys_turn_right_action.flags = QMF_GRAYED;
			s_keys_turn_right_action.x = 0;
			s_keys_turn_right_action.y = y += 9;
			s_keys_turn_right_action.ownerdraw = new Anonymousmcallback9();
			s_keys_turn_right_action.localdata[0] = ++i;
			s_keys_turn_right_action.name = bindnames[s_keys_turn_right_action.localdata[0]][1];
			s_keys_run_action.type = MTYPE_ACTION;
			s_keys_run_action.flags = QMF_GRAYED;
			s_keys_run_action.x = 0;
			s_keys_run_action.y = y += 9;
			s_keys_run_action.ownerdraw = new Anonymousmcallback10();
			s_keys_run_action.localdata[0] = ++i;
			s_keys_run_action.name = bindnames[s_keys_run_action.localdata[0]][1];
			s_keys_step_left_action.type = MTYPE_ACTION;
			s_keys_step_left_action.flags = QMF_GRAYED;
			s_keys_step_left_action.x = 0;
			s_keys_step_left_action.y = y += 9;
			s_keys_step_left_action.ownerdraw = new Anonymousmcallback11();
			s_keys_step_left_action.localdata[0] = ++i;
			s_keys_step_left_action.name = bindnames[s_keys_step_left_action.localdata[0]][1];
			s_keys_step_right_action.type = MTYPE_ACTION;
			s_keys_step_right_action.flags = QMF_GRAYED;
			s_keys_step_right_action.x = 0;
			s_keys_step_right_action.y = y += 9;
			s_keys_step_right_action.ownerdraw = new Anonymousmcallback12();
			s_keys_step_right_action.localdata[0] = ++i;
			s_keys_step_right_action.name = bindnames[s_keys_step_right_action.localdata[0]][1];
			s_keys_sidestep_action.type = MTYPE_ACTION;
			s_keys_sidestep_action.flags = QMF_GRAYED;
			s_keys_sidestep_action.x = 0;
			s_keys_sidestep_action.y = y += 9;
			s_keys_sidestep_action.ownerdraw = new Anonymousmcallback13();
			s_keys_sidestep_action.localdata[0] = ++i;
			s_keys_sidestep_action.name = bindnames[s_keys_sidestep_action.localdata[0]][1];
			s_keys_look_up_action.type = MTYPE_ACTION;
			s_keys_look_up_action.flags = QMF_GRAYED;
			s_keys_look_up_action.x = 0;
			s_keys_look_up_action.y = y += 9;
			s_keys_look_up_action.ownerdraw = new Anonymousmcallback14();
			s_keys_look_up_action.localdata[0] = ++i;
			s_keys_look_up_action.name = bindnames[s_keys_look_up_action.localdata[0]][1];
			s_keys_look_down_action.type = MTYPE_ACTION;
			s_keys_look_down_action.flags = QMF_GRAYED;
			s_keys_look_down_action.x = 0;
			s_keys_look_down_action.y = y += 9;
			s_keys_look_down_action.ownerdraw = new Anonymousmcallback15();
			s_keys_look_down_action.localdata[0] = ++i;
			s_keys_look_down_action.name = bindnames[s_keys_look_down_action.localdata[0]][1];
			s_keys_center_view_action.type = MTYPE_ACTION;
			s_keys_center_view_action.flags = QMF_GRAYED;
			s_keys_center_view_action.x = 0;
			s_keys_center_view_action.y = y += 9;
			s_keys_center_view_action.ownerdraw = new Anonymousmcallback16();
			s_keys_center_view_action.localdata[0] = ++i;
			s_keys_center_view_action.name = bindnames[s_keys_center_view_action.localdata[0]][1];
			s_keys_mouse_look_action.type = MTYPE_ACTION;
			s_keys_mouse_look_action.flags = QMF_GRAYED;
			s_keys_mouse_look_action.x = 0;
			s_keys_mouse_look_action.y = y += 9;
			s_keys_mouse_look_action.ownerdraw = new Anonymousmcallback17();
			s_keys_mouse_look_action.localdata[0] = ++i;
			s_keys_mouse_look_action.name = bindnames[s_keys_mouse_look_action.localdata[0]][1];
			s_keys_keyboard_look_action.type = MTYPE_ACTION;
			s_keys_keyboard_look_action.flags = QMF_GRAYED;
			s_keys_keyboard_look_action.x = 0;
			s_keys_keyboard_look_action.y = y += 9;
			s_keys_keyboard_look_action.ownerdraw = new Anonymousmcallback18();
			s_keys_keyboard_look_action.localdata[0] = ++i;
			s_keys_keyboard_look_action.name = bindnames[s_keys_keyboard_look_action.localdata[0]][1];
			s_keys_move_up_action.type = MTYPE_ACTION;
			s_keys_move_up_action.flags = QMF_GRAYED;
			s_keys_move_up_action.x = 0;
			s_keys_move_up_action.y = y += 9;
			s_keys_move_up_action.ownerdraw = new Anonymousmcallback19();
			s_keys_move_up_action.localdata[0] = ++i;
			s_keys_move_up_action.name = bindnames[s_keys_move_up_action.localdata[0]][1];
			s_keys_move_down_action.type = MTYPE_ACTION;
			s_keys_move_down_action.flags = QMF_GRAYED;
			s_keys_move_down_action.x = 0;
			s_keys_move_down_action.y = y += 9;
			s_keys_move_down_action.ownerdraw = new Anonymousmcallback20();
			s_keys_move_down_action.localdata[0] = ++i;
			s_keys_move_down_action.name = bindnames[s_keys_move_down_action.localdata[0]][1];
			s_keys_inventory_action.type = MTYPE_ACTION;
			s_keys_inventory_action.flags = QMF_GRAYED;
			s_keys_inventory_action.x = 0;
			s_keys_inventory_action.y = y += 9;
			s_keys_inventory_action.ownerdraw = new Anonymousmcallback21();
			s_keys_inventory_action.localdata[0] = ++i;
			s_keys_inventory_action.name = bindnames[s_keys_inventory_action.localdata[0]][1];
			s_keys_inv_use_action.type = MTYPE_ACTION;
			s_keys_inv_use_action.flags = QMF_GRAYED;
			s_keys_inv_use_action.x = 0;
			s_keys_inv_use_action.y = y += 9;
			s_keys_inv_use_action.ownerdraw = new Anonymousmcallback22();
			s_keys_inv_use_action.localdata[0] = ++i;
			s_keys_inv_use_action.name = bindnames[s_keys_inv_use_action.localdata[0]][1];
			s_keys_inv_drop_action.type = MTYPE_ACTION;
			s_keys_inv_drop_action.flags = QMF_GRAYED;
			s_keys_inv_drop_action.x = 0;
			s_keys_inv_drop_action.y = y += 9;
			s_keys_inv_drop_action.ownerdraw = new Anonymousmcallback23();
			s_keys_inv_drop_action.localdata[0] = ++i;
			s_keys_inv_drop_action.name = bindnames[s_keys_inv_drop_action.localdata[0]][1];
			s_keys_inv_prev_action.type = MTYPE_ACTION;
			s_keys_inv_prev_action.flags = QMF_GRAYED;
			s_keys_inv_prev_action.x = 0;
			s_keys_inv_prev_action.y = y += 9;
			s_keys_inv_prev_action.ownerdraw = new Anonymousmcallback24();
			s_keys_inv_prev_action.localdata[0] = ++i;
			s_keys_inv_prev_action.name = bindnames[s_keys_inv_prev_action.localdata[0]][1];
			s_keys_inv_next_action.type = MTYPE_ACTION;
			s_keys_inv_next_action.flags = QMF_GRAYED;
			s_keys_inv_next_action.x = 0;
			s_keys_inv_next_action.y = y += 9;
			s_keys_inv_next_action.ownerdraw = new Anonymousmcallback25();
			s_keys_inv_next_action.localdata[0] = ++i;
			s_keys_inv_next_action.name = bindnames[s_keys_inv_next_action.localdata[0]][1];
			s_keys_help_computer_action.type = MTYPE_ACTION;
			s_keys_help_computer_action.flags = QMF_GRAYED;
			s_keys_help_computer_action.x = 0;
			s_keys_help_computer_action.y = y += 9;
			s_keys_help_computer_action.ownerdraw = new Anonymousmcallback26();
			s_keys_help_computer_action.localdata[0] = ++i;
			s_keys_help_computer_action.name = bindnames[s_keys_help_computer_action.localdata[0]][1];
			Menu_AddItem( s_keys_menu, s_keys_attack_action );
			Menu_AddItem( s_keys_menu, s_keys_change_weapon_action );
			Menu_AddItem( s_keys_menu, s_keys_walk_forward_action );
			Menu_AddItem( s_keys_menu, s_keys_backpedal_action );
			Menu_AddItem( s_keys_menu, s_keys_turn_left_action );
			Menu_AddItem( s_keys_menu, s_keys_turn_right_action );
			Menu_AddItem( s_keys_menu, s_keys_run_action );
			Menu_AddItem( s_keys_menu, s_keys_step_left_action );
			Menu_AddItem( s_keys_menu, s_keys_step_right_action );
			Menu_AddItem( s_keys_menu, s_keys_sidestep_action );
			Menu_AddItem( s_keys_menu, s_keys_look_up_action );
			Menu_AddItem( s_keys_menu, s_keys_look_down_action );
			Menu_AddItem( s_keys_menu, s_keys_center_view_action );
			Menu_AddItem( s_keys_menu, s_keys_mouse_look_action );
			Menu_AddItem( s_keys_menu, s_keys_keyboard_look_action );
			Menu_AddItem( s_keys_menu, s_keys_move_up_action );
			Menu_AddItem( s_keys_menu, s_keys_move_down_action );
			Menu_AddItem( s_keys_menu, s_keys_inventory_action );
			Menu_AddItem( s_keys_menu, s_keys_inv_use_action );
			Menu_AddItem( s_keys_menu, s_keys_inv_drop_action );
			Menu_AddItem( s_keys_menu, s_keys_inv_prev_action );
			Menu_AddItem( s_keys_menu, s_keys_inv_next_action );
			Menu_AddItem( s_keys_menu, s_keys_help_computer_action );
			Menu_SetStatusBar( s_keys_menu, "enter to change, backspace to clear" );
			Menu_Center( s_keys_menu );
		}

		private sealed class Anonymousmcallback3 : mcallback
		{
			public override void Execute( Object o )
			{
				KeyCursorDrawFunc( ( menuframework_s ) o );
			}
		}

		private sealed class Anonymousmcallback4 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback5 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback6 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback7 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback8 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback9 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback10 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback11 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback12 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback13 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback14 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback15 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback16 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback17 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback18 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback19 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback20 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback21 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback22 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback23 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback24 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback25 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		private sealed class Anonymousmcallback26 : mcallback
		{
			public override void Execute( Object o )
			{
				DrawKeyBindingFunc( o );
			}
		}

		static xcommand_t Keys_MenuDraw = new Anonymousxcommand_t5();
		private sealed class Anonymousxcommand_t5 : xcommand_t
		{
			public override void Execute( )
			{
				Keys_MenuDraw_f();
			}
		}

		static void Keys_MenuDraw_f( )
		{
			Menu_AdjustCursor( s_keys_menu, 1 );
			Menu_Draw( s_keys_menu );
		}

		static keyfunc_t Keys_MenuKey = new Anonymouskeyfunc_t3();
		private sealed class Anonymouskeyfunc_t3 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Keys_MenuKey_f( key );
			}
		}

		static String Keys_MenuKey_f( Int32 key )
		{
			menuaction_s item = ( menuaction_s ) Menu_ItemAtCursor( s_keys_menu );
			if ( bind_grab )
			{
				if ( key != K_ESCAPE && key != '`' )
				{
					String cmd;
					cmd = "bind \\\"" + Key.KeynumToString( key ) + "\\\" \\\"" + bindnames[item.localdata[0]][0] + "\\\"";
					Cbuf.InsertText( cmd );
				}

				Menu_SetStatusBar( s_keys_menu, "enter to change, backspace to clear" );
				bind_grab = false;
				return menu_out_sound;
			}

			switch ( key )

			{
				case K_KP_ENTER:
				case K_ENTER:
					KeyBindingFunc( item );
					return menu_in_sound;
				case K_BACKSPACE:
				case K_DEL:
				case K_KP_DEL:
					UnbindCommand( bindnames[item.localdata[0]][0] );
					return menu_out_sound;
				default:
					return Default_MenuKey( s_keys_menu, key );
			}
		}

		static xcommand_t Menu_Keys = new Anonymousxcommand_t6();
		private sealed class Anonymousxcommand_t6 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_Keys_f();
			}
		}

		static void Menu_Keys_f( )
		{
			Keys_MenuInit();
			PushMenu( new Anonymousxcommand_t7(), new Anonymouskeyfunc_t4() );
		}

		private sealed class Anonymousxcommand_t7 : xcommand_t
		{
			public override void Execute( )
			{
				Keys_MenuDraw_f();
			}
		}

		private sealed class Anonymouskeyfunc_t4 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Keys_MenuKey_f( key );
			}
		}

		static cvar_t win_noalttab;
		static menuframework_s s_options_menu = new menuframework_s();
		static menuaction_s s_options_defaults_action = new menuaction_s();
		static menuaction_s s_options_customize_options_action = new menuaction_s();
		static menuslider_s s_options_sensitivity_slider = new menuslider_s();
		static menulist_s s_options_freelook_box = new menulist_s();
		static menulist_s s_options_noalttab_box = new menulist_s();
		static menulist_s s_options_alwaysrun_box = new menulist_s();
		static menulist_s s_options_invertmouse_box = new menulist_s();
		static menulist_s s_options_lookspring_box = new menulist_s();
		static menulist_s s_options_lookstrafe_box = new menulist_s();
		static menulist_s s_options_crosshair_box = new menulist_s();
		static menuslider_s s_options_sfxvolume_slider = new menuslider_s();
		static menulist_s s_options_joystick_box = new menulist_s();
		static menulist_s s_options_cdvolume_box = new menulist_s();
		static menulist_s s_options_quality_list = new menulist_s();
		static menuaction_s s_options_console_action = new menuaction_s();
		static void CrosshairFunc( Object unused )
		{
			Cvar.SetValue( "crosshair", s_options_crosshair_box.curvalue );
		}

		static void JoystickFunc( Object unused )
		{
			Cvar.SetValue( "in_joystick", s_options_joystick_box.curvalue );
		}

		static void CustomizeControlsFunc( Object unused )
		{
			Menu_Keys_f();
		}

		static void AlwaysRunFunc( Object unused )
		{
			Cvar.SetValue( "cl_run", s_options_alwaysrun_box.curvalue );
		}

		static void FreeLookFunc( Object unused )
		{
			Cvar.SetValue( "freelook", s_options_freelook_box.curvalue );
		}

		static void MouseSpeedFunc( Object unused )
		{
			Cvar.SetValue( "sensitivity", s_options_sensitivity_slider.curvalue / 2F );
		}

		static void NoAltTabFunc( Object unused )
		{
			Cvar.SetValue( "win_noalttab", s_options_noalttab_box.curvalue );
		}

		static Single ClampCvar( Single min, Single max, Single value )
		{
			if ( value < min )
				return min;
			if ( value > max )
				return max;
			return value;
		}

		static void ControlsSetMenuItemValues( )
		{
			s_options_sfxvolume_slider.curvalue = Cvar.VariableValue( "s_volume" ) * 10;
			s_options_cdvolume_box.curvalue = 1 - ( ( Int32 ) Cvar.VariableValue( "cd_nocd" ) );
			var s = Cvar.VariableString( "s_impl" );
			for ( var i = 0; i < s_drivers.Length; i++ )
			{
				if ( s.Equals( s_drivers[i] ) )
				{
					s_options_quality_list.curvalue = i;
				}
			}

			s_options_sensitivity_slider.curvalue = ( sensitivity.value ) * 2;
			Cvar.SetValue( "cl_run", ClampCvar( 0, 1, cl_run.value ) );
			s_options_alwaysrun_box.curvalue = ( Int32 ) cl_run.value;
			s_options_invertmouse_box.curvalue = m_pitch.value < 0 ? 1 : 0;
			Cvar.SetValue( "lookspring", ClampCvar( 0, 1, lookspring.value ) );
			s_options_lookspring_box.curvalue = ( Int32 ) lookspring.value;
			Cvar.SetValue( "lookstrafe", ClampCvar( 0, 1, lookstrafe.value ) );
			s_options_lookstrafe_box.curvalue = ( Int32 ) lookstrafe.value;
			Cvar.SetValue( "freelook", ClampCvar( 0, 1, freelook.value ) );
			s_options_freelook_box.curvalue = ( Int32 ) freelook.value;
			Cvar.SetValue( "crosshair", ClampCvar( 0, 3, Globals.crosshair.value ) );
			s_options_crosshair_box.curvalue = ( Int32 ) Globals.crosshair.value;
			Cvar.SetValue( "in_joystick", ClampCvar( 0, 1, in_joystick.value ) );
			s_options_joystick_box.curvalue = ( Int32 ) in_joystick.value;
			s_options_noalttab_box.curvalue = ( Int32 ) win_noalttab.value;
		}

		static void ControlsResetDefaultsFunc( Object unused )
		{
			Cbuf.AddText( "exec default.cfg\\n" );
			Cbuf.Execute();
			ControlsSetMenuItemValues();
		}

		static void InvertMouseFunc( Object unused )
		{
			Cvar.SetValue( "m_pitch", -m_pitch.value );
		}

		static void LookspringFunc( Object unused )
		{
			Cvar.SetValue( "lookspring", 1 - lookspring.value );
		}

		static void LookstrafeFunc( Object unused )
		{
			Cvar.SetValue( "lookstrafe", 1 - lookstrafe.value );
		}

		static void UpdateVolumeFunc( Object unused )
		{
			Cvar.SetValue( "s_volume", s_options_sfxvolume_slider.curvalue / 10 );
		}

		static void UpdateCDVolumeFunc( Object unused )
		{
			Cvar.SetValue( "cd_nocd", 1 - s_options_cdvolume_box.curvalue );
		}

		static void ConsoleFunc( Object unused )
		{
			if ( cl.attractloop )
			{
				Cbuf.AddText( "killserver\\n" );
				return;
			}

			Key.ClearTyping();
			Con.ClearNotify();
			ForceMenuOff();
			cls.key_dest = key_console;
		}

		static void UpdateSoundQualityFunc( Object unused )
		{
			var driverNotChanged = false;
			var current = s_drivers[s_options_quality_list.curvalue];
			driverNotChanged = S.GetDriverName().Equals( current );
			if ( driverNotChanged )
			{
				re.EndFrame();
				return;
			}
			else
			{
				Cvar.Set( "s_impl", current );
				DrawTextBox( 8, 120 - 48, 36, 3 );
				Print( 16 + 16, 120 - 48 + 8, "Restarting the sound system. This" );
				Print( 16 + 16, 120 - 48 + 16, "could take up to a minute, so" );
				Print( 16 + 16, 120 - 48 + 24, "please be patient." );
				re.EndFrame();
				CL.Snd_Restart_f.Execute();
			}
		}

		static String[] cd_music_items = new[] { "disabled", "enabled" };
		static String[] compatibility_items = new[] { "max compatibility", "max performance" };
		static String[] yesno_names = new[] { "no", "yes" };
		static String[] crosshair_names = new[] { "none", "cross", "dot", "angle" };
		static String[] s_labels;
		static String[] s_drivers;
		static void Options_MenuInit( )
		{
			s_drivers = S.GetDriverNames();
			s_labels = new String[s_drivers.Length];
			for ( var i = 0; i < s_drivers.Length; i++ )
			{
				if ( "dummy".Equals( s_drivers[i] ) )
				{
					s_labels[i] = "off";
				}
				else
				{
					s_labels[i] = s_drivers[i];
				}
			}

			win_noalttab = Cvar.Get( "win_noalttab", "0", CVAR_ARCHIVE );
			s_options_menu.x = viddef.GetWidth() / 2;
			s_options_menu.y = viddef.GetHeight() / 2 - 58;
			s_options_menu.nitems = 0;
			s_options_sfxvolume_slider.type = MTYPE_SLIDER;
			s_options_sfxvolume_slider.x = 0;
			s_options_sfxvolume_slider.y = 0;
			s_options_sfxvolume_slider.name = "effects volume";
			s_options_sfxvolume_slider.callback = new Anonymousmcallback27();
			s_options_sfxvolume_slider.minvalue = 0;
			s_options_sfxvolume_slider.maxvalue = 10;
			s_options_sfxvolume_slider.curvalue = Cvar.VariableValue( "s_volume" ) * 10;
			s_options_cdvolume_box.type = MTYPE_SPINCONTROL;
			s_options_cdvolume_box.x = 0;
			s_options_cdvolume_box.y = 10;
			s_options_cdvolume_box.name = "CD music";
			s_options_cdvolume_box.callback = new Anonymousmcallback28();
			s_options_cdvolume_box.itemnames = cd_music_items;
			s_options_cdvolume_box.curvalue = 1 - ( Int32 ) Cvar.VariableValue( "cd_nocd" );
			s_options_quality_list.type = MTYPE_SPINCONTROL;
			s_options_quality_list.x = 0;
			s_options_quality_list.y = 20;
			s_options_quality_list.name = "sound";
			s_options_quality_list.callback = new Anonymousmcallback29();
			s_options_quality_list.itemnames = s_labels;
			s_options_sensitivity_slider.type = MTYPE_SLIDER;
			s_options_sensitivity_slider.x = 0;
			s_options_sensitivity_slider.y = 50;
			s_options_sensitivity_slider.name = "mouse speed";
			s_options_sensitivity_slider.callback = new Anonymousmcallback30();
			s_options_sensitivity_slider.minvalue = 2;
			s_options_sensitivity_slider.maxvalue = 22;
			s_options_alwaysrun_box.type = MTYPE_SPINCONTROL;
			s_options_alwaysrun_box.x = 0;
			s_options_alwaysrun_box.y = 60;
			s_options_alwaysrun_box.name = "always run";
			s_options_alwaysrun_box.callback = new Anonymousmcallback31();
			s_options_alwaysrun_box.itemnames = yesno_names;
			s_options_invertmouse_box.type = MTYPE_SPINCONTROL;
			s_options_invertmouse_box.x = 0;
			s_options_invertmouse_box.y = 70;
			s_options_invertmouse_box.name = "invert mouse";
			s_options_invertmouse_box.callback = new Anonymousmcallback32();
			s_options_invertmouse_box.itemnames = yesno_names;
			s_options_lookspring_box.type = MTYPE_SPINCONTROL;
			s_options_lookspring_box.x = 0;
			s_options_lookspring_box.y = 80;
			s_options_lookspring_box.name = "lookspring";
			s_options_lookspring_box.callback = new Anonymousmcallback33();
			s_options_lookspring_box.itemnames = yesno_names;
			s_options_lookstrafe_box.type = MTYPE_SPINCONTROL;
			s_options_lookstrafe_box.x = 0;
			s_options_lookstrafe_box.y = 90;
			s_options_lookstrafe_box.name = "lookstrafe";
			s_options_lookstrafe_box.callback = new Anonymousmcallback34();
			s_options_lookstrafe_box.itemnames = yesno_names;
			s_options_freelook_box.type = MTYPE_SPINCONTROL;
			s_options_freelook_box.x = 0;
			s_options_freelook_box.y = 100;
			s_options_freelook_box.name = "free look";
			s_options_freelook_box.callback = new Anonymousmcallback35();
			s_options_freelook_box.itemnames = yesno_names;
			s_options_crosshair_box.type = MTYPE_SPINCONTROL;
			s_options_crosshair_box.x = 0;
			s_options_crosshair_box.y = 110;
			s_options_crosshair_box.name = "crosshair";
			s_options_crosshair_box.callback = new Anonymousmcallback36();
			s_options_crosshair_box.itemnames = crosshair_names;
			s_options_joystick_box.type = MTYPE_SPINCONTROL;
			s_options_joystick_box.x = 0;
			s_options_joystick_box.y = 120;
			s_options_joystick_box.name = "use joystick";
			s_options_joystick_box.callback = new Anonymousmcallback37();
			s_options_joystick_box.itemnames = yesno_names;
			s_options_customize_options_action.type = MTYPE_ACTION;
			s_options_customize_options_action.x = 0;
			s_options_customize_options_action.y = 140;
			s_options_customize_options_action.name = "customize controls";
			s_options_customize_options_action.callback = new Anonymousmcallback38();
			s_options_defaults_action.type = MTYPE_ACTION;
			s_options_defaults_action.x = 0;
			s_options_defaults_action.y = 150;
			s_options_defaults_action.name = "reset defaults";
			s_options_defaults_action.callback = new Anonymousmcallback39();
			s_options_console_action.type = MTYPE_ACTION;
			s_options_console_action.x = 0;
			s_options_console_action.y = 160;
			s_options_console_action.name = "go to console";
			s_options_console_action.callback = new Anonymousmcallback40();
			ControlsSetMenuItemValues();
			Menu_AddItem( s_options_menu, s_options_sfxvolume_slider );
			Menu_AddItem( s_options_menu, s_options_cdvolume_box );
			Menu_AddItem( s_options_menu, s_options_quality_list );
			Menu_AddItem( s_options_menu, s_options_sensitivity_slider );
			Menu_AddItem( s_options_menu, s_options_alwaysrun_box );
			Menu_AddItem( s_options_menu, s_options_invertmouse_box );
			Menu_AddItem( s_options_menu, s_options_lookspring_box );
			Menu_AddItem( s_options_menu, s_options_lookstrafe_box );
			Menu_AddItem( s_options_menu, s_options_freelook_box );
			Menu_AddItem( s_options_menu, s_options_crosshair_box );
			Menu_AddItem( s_options_menu, s_options_customize_options_action );
			Menu_AddItem( s_options_menu, s_options_defaults_action );
			Menu_AddItem( s_options_menu, s_options_console_action );
		}

		private sealed class Anonymousmcallback27 : mcallback
		{
			public override void Execute( Object o )
			{
				UpdateVolumeFunc( o );
			}
		}

		private sealed class Anonymousmcallback28 : mcallback
		{
			public override void Execute( Object o )
			{
				UpdateCDVolumeFunc( o );
			}
		}

		private sealed class Anonymousmcallback29 : mcallback
		{
			public override void Execute( Object o )
			{
				UpdateSoundQualityFunc( o );
			}
		}

		private sealed class Anonymousmcallback30 : mcallback
		{
			public override void Execute( Object o )
			{
				MouseSpeedFunc( o );
			}
		}

		private sealed class Anonymousmcallback31 : mcallback
		{
			public override void Execute( Object o )
			{
				AlwaysRunFunc( o );
			}
		}

		private sealed class Anonymousmcallback32 : mcallback
		{
			public override void Execute( Object o )
			{
				InvertMouseFunc( o );
			}
		}

		private sealed class Anonymousmcallback33 : mcallback
		{
			public override void Execute( Object o )
			{
				LookspringFunc( o );
			}
		}

		private sealed class Anonymousmcallback34 : mcallback
		{
			public override void Execute( Object o )
			{
				LookstrafeFunc( o );
			}
		}

		private sealed class Anonymousmcallback35 : mcallback
		{
			public override void Execute( Object o )
			{
				FreeLookFunc( o );
			}
		}

		private sealed class Anonymousmcallback36 : mcallback
		{
			public override void Execute( Object o )
			{
				CrosshairFunc( o );
			}
		}

		private sealed class Anonymousmcallback37 : mcallback
		{
			public override void Execute( Object o )
			{
				JoystickFunc( o );
			}
		}

		private sealed class Anonymousmcallback38 : mcallback
		{
			public override void Execute( Object o )
			{
				CustomizeControlsFunc( o );
			}
		}

		private sealed class Anonymousmcallback39 : mcallback
		{
			public override void Execute( Object o )
			{
				ControlsResetDefaultsFunc( o );
			}
		}

		private sealed class Anonymousmcallback40 : mcallback
		{
			public override void Execute( Object o )
			{
				ConsoleFunc( o );
			}
		}

		static void Options_MenuDraw( )
		{
			Banner( "m_banner_options" );
			Menu_AdjustCursor( s_options_menu, 1 );
			Menu_Draw( s_options_menu );
		}

		static String Options_MenuKey( Int32 key )
		{
			return Default_MenuKey( s_options_menu, key );
		}

		static xcommand_t Menu_Options = new Anonymousxcommand_t8();
		private sealed class Anonymousxcommand_t8 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_Options_f();
			}
		}

		static void Menu_Options_f( )
		{
			Options_MenuInit();
			PushMenu( new Anonymousxcommand_t9(), new Anonymouskeyfunc_t5() );
		}

		private sealed class Anonymousxcommand_t9 : xcommand_t
		{
			public override void Execute( )
			{
				Options_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t5 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Options_MenuKey( key );
			}
		}

		static xcommand_t Menu_Video = new Anonymousxcommand_t10();
		private sealed class Anonymousxcommand_t10 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_Video_f();
			}
		}

		static void Menu_Video_f( )
		{
			VID.MenuInit();
			PushMenu( new Anonymousxcommand_t11(), new Anonymouskeyfunc_t6() );
		}

		private sealed class Anonymousxcommand_t11 : xcommand_t
		{
			public override void Execute( )
			{
				VID.MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t6 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return VID.MenuKey( key );
			}
		}

		static Int32 credits_start_time;
		static String[] creditsIndex = new String[256];
		static String creditsBuffer;
		static String[] idcredits = new[] { "+QUAKE II BY ID SOFTWARE", "", "+PROGRAMMING", "John Carmack", "John Cash", "Brian Hook", "", "+JAVA PORT BY BYTONIC", "Carsten Weisse", "Holger Zickner", "Rene Stoeckel", "", "+ART", "Adrian Carmack", "Kevin Cloud", "Paul Steed", "", "+LEVEL DESIGN", "Tim Willits", "American McGee", "Christian Antkow", "Paul Jaquays", "Brandon James", "", "+BIZ", "Todd Hollenshead", "Barrett (Bear) Alexander", "Donna Jackson", "", "", "+SPECIAL THANKS", "Ben Donges for beta testing", "", "", "", "", "", "", "+ADDITIONAL SUPPORT", "", "+LINUX PORT AND CTF", "Dave \\\"Zoid\\\" Kirsch", "", "+CINEMATIC SEQUENCES", "Ending Cinematic by Blur Studio - ", "Venice, CA", "", "Environment models for Introduction", "Cinematic by Karl Dolgener", "", "Assistance with environment design", "by Cliff Iwai", "", "+SOUND EFFECTS AND MUSIC", "Sound Design by Soundelux Media Labs.", "Music Composed and Produced by", "Soundelux Media Labs.  Special thanks", "to Bill Brown, Tom Ozanich, Brian", "Celano, Jeff Eisner, and The Soundelux", "Players.", "", "\\\"Level Music\\\" by Sonic Mayhem", "www.sonicmayhem.com", "", "\\\"Quake II Theme Song\\\"", "(C) 1997 Rob Zombie. All Rights", "Reserved.", "", "Track 10 (\\\"Climb\\\") by Jer Sypult", "", "Voice of computers by", "Carly Staehlin-Taylor", "", "+THANKS TO ACTIVISION", "+IN PARTICULAR:", "", "John Tam", "Steve Rosenthal", "Marty Stratton", "Henk Hartong", "", "Quake II(tm) (C)1997 Id Software, Inc.", "All Rights Reserved.  Distributed by", "Activision, Inc. under license.", "Quake II(tm), the Id Software name,", "the \\\"Q II\\\"(tm) logo and id(tm)", "logo are trademarks of Id Software,", "Inc. Activision(R) is a registered", "trademark of Activision, Inc. All", "other trademarks and trade names are", "properties of their respective owners.", null };
		static String[] credits = idcredits;
		static String[] xatcredits = new[] { "+QUAKE II MISSION PACK: THE RECKONING", "+BY", "+XATRIX ENTERTAINMENT, INC.", "", "+DESIGN AND DIRECTION", "Drew Markham", "", "+PRODUCED BY", "Greg Goodrich", "", "+PROGRAMMING", "Rafael Paiz", "", "+LEVEL DESIGN / ADDITIONAL GAME DESIGN", "Alex Mayberry", "", "+LEVEL DESIGN", "Mal Blackwell", "Dan Koppel", "", "+ART DIRECTION", "Michael \\\"Maxx\\\" Kaufman", "", "+COMPUTER GRAPHICS SUPERVISOR AND", "+CHARACTER ANIMATION DIRECTION", "Barry Dempsey", "", "+SENIOR ANIMATOR AND MODELER", "Jason Hoover", "", "+CHARACTER ANIMATION AND", "+MOTION CAPTURE SPECIALIST", "Amit Doron", "", "+ART", "Claire Praderie-Markham", "Viktor Antonov", "Corky Lehmkuhl", "", "+INTRODUCTION ANIMATION", "Dominique Drozdz", "", "+ADDITIONAL LEVEL DESIGN", "Aaron Barber", "Rhett Baldwin", "", "+3D CHARACTER ANIMATION TOOLS", "Gerry Tyra, SA Technology", "", "+ADDITIONAL EDITOR TOOL PROGRAMMING", "Robert Duffy", "", "+ADDITIONAL PROGRAMMING", "Ryan Feltrin", "", "+PRODUCTION COORDINATOR", "Victoria Sylvester", "", "+SOUND DESIGN", "Gary Bradfield", "", "+MUSIC BY", "Sonic Mayhem", "", "", "", "+SPECIAL THANKS", "+TO", "+OUR FRIENDS AT ID SOFTWARE", "", "John Carmack", "John Cash", "Brian Hook", "Adrian Carmack", "Kevin Cloud", "Paul Steed", "Tim Willits", "Christian Antkow", "Paul Jaquays", "Brandon James", "Todd Hollenshead", "Barrett (Bear) Alexander", "Dave \\\"Zoid\\\" Kirsch", "Donna Jackson", "", "", "", "+THANKS TO ACTIVISION", "+IN PARTICULAR:", "", "Marty Stratton", "Henk \\\"The Original Ripper\\\" Hartong", "Kevin Kraff", "Jamey Gottlieb", "Chris Hepburn", "", "+AND THE GAME TESTERS", "", "Tim Vanlaw", "Doug Jacobs", "Steven Rosenthal", "David Baker", "Chris Campbell", "Aaron Casillas", "Steve Elwell", "Derek Johnstone", "Igor Krinitskiy", "Samantha Lee", "Michael Spann", "Chris Toft", "Juan Valdes", "", "+THANKS TO INTERGRAPH COMPUTER SYTEMS", "+IN PARTICULAR:", "", "Michael T. Nicolaou", "", "", "Quake II Mission Pack: The Reckoning", "(tm) (C)1998 Id Software, Inc. All", "Rights Reserved. Developed by Xatrix", "Entertainment, Inc. for Id Software,", "Inc. Distributed by Activision Inc.", "under license. Quake(R) is a", "registered trademark of Id Software,", "Inc. Quake II Mission Pack: The", "Reckoning(tm), Quake II(tm), the Id", "Software name, the \\\"Q II\\\"(tm) logo", "and id(tm) logo are trademarks of Id", "Software, Inc. Activision(R) is a", "registered trademark of Activision,", "Inc. Xatrix(R) is a registered", "trademark of Xatrix Entertainment,", "Inc. All other trademarks and trade", "names are properties of their", "respective owners.", null };
		static String[] roguecredits = new[] { "+QUAKE II MISSION PACK 2: GROUND ZERO", "+BY", "+ROGUE ENTERTAINMENT, INC.", "", "+PRODUCED BY", "Jim Molinets", "", "+PROGRAMMING", "Peter Mack", "Patrick Magruder", "", "+LEVEL DESIGN", "Jim Molinets", "Cameron Lamprecht", "Berenger Fish", "Robert Selitto", "Steve Tietze", "Steve Thoms", "", "+ART DIRECTION", "Rich Fleider", "", "+ART", "Rich Fleider", "Steve Maines", "Won Choi", "", "+ANIMATION SEQUENCES", "Creat Studios", "Steve Maines", "", "+ADDITIONAL LEVEL DESIGN", "Rich Fleider", "Steve Maines", "Peter Mack", "", "+SOUND", "James Grunke", "", "+GROUND ZERO THEME", "+AND", "+MUSIC BY", "Sonic Mayhem", "", "+VWEP MODELS", "Brent \\\"Hentai\\\" Dill", "", "", "", "+SPECIAL THANKS", "+TO", "+OUR FRIENDS AT ID SOFTWARE", "", "John Carmack", "John Cash", "Brian Hook", "Adrian Carmack", "Kevin Cloud", "Paul Steed", "Tim Willits", "Christian Antkow", "Paul Jaquays", "Brandon James", "Todd Hollenshead", "Barrett (Bear) Alexander", "Katherine Anna Kang", "Donna Jackson", "Dave \\\"Zoid\\\" Kirsch", "", "", "", "+THANKS TO ACTIVISION", "+IN PARTICULAR:", "", "Marty Stratton", "Henk Hartong", "Mitch Lasky", "Steve Rosenthal", "Steve Elwell", "", "+AND THE GAME TESTERS", "", "The Ranger Clan", "Dave \\\"Zoid\\\" Kirsch", "Nihilistic Software", "Robert Duffy", "", "And Countless Others", "", "", "", "Quake II Mission Pack 2: Ground Zero", "(tm) (C)1998 Id Software, Inc. All", "Rights Reserved. Developed by Rogue", "Entertainment, Inc. for Id Software,", "Inc. Distributed by Activision Inc.", "under license. Quake(R) is a", "registered trademark of Id Software,", "Inc. Quake II Mission Pack 2: Ground", "Zero(tm), Quake II(tm), the Id", "Software name, the \\\"Q II\\\"(tm) logo", "and id(tm) logo are trademarks of Id", "Software, Inc. Activision(R) is a", "registered trademark of Activision,", "Inc. Rogue(R) is a registered", "trademark of Rogue Entertainment,", "Inc. All other trademarks and trade", "names are properties of their", "respective owners.", null };
		public static void Credits_MenuDraw( )
		{
			Int32 i, y;
			for ( i = 0, y = ( Int32 ) ( viddef.GetHeight() - ( ( cls.realtime - credits_start_time ) / 40F ) ); credits[i] != null && y < viddef.GetHeight(); y += 10, i++ )
			{
				Int32 j, stringoffset = 0;
				var bold = false;
				if ( y <= -8 )
					continue;
				if ( credits[i].Length > 0 && credits[i][0] == '+' )
				{
					bold = true;
					stringoffset = 1;
				}
				else
				{
					bold = false;
					stringoffset = 0;
				}

				for ( j = 0; j + stringoffset < credits[i].Length; j++ )
				{
					Int32 x;
					x = ( viddef.GetWidth() - credits[i].Length * 8 - stringoffset * 8 ) / 2 + ( j + stringoffset ) * 8;
					if ( bold )
						re.DrawChar( x, y, credits[i][j + stringoffset] + 128 );
					else
						re.DrawChar( x, y, credits[i][j + stringoffset] );
				}
			}

			if ( y < 0 )
				credits_start_time = cls.realtime;
		}

		public static String Credits_Key( Int32 key )
		{
			switch ( key )

			{
				case K_ESCAPE:
					PopMenu();
					break;
			}

			return menu_out_sound;
		}

		static xcommand_t Menu_Credits = new Anonymousxcommand_t12();
		private sealed class Anonymousxcommand_t12 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_Credits_f();
			}
		}

		static void Menu_Credits_f( )
		{
			Int32 n;
			var isdeveloper = 0;
			Byte[] b = FS.LoadFile( "credits" );
			if ( b != null )
			{
				creditsBuffer = Encoding.ASCII.GetString( b );
				String[] line = creditsBuffer.Split( "\\r\\n" );
				for ( n = 0; n < line.Length; n++ )
				{
					creditsIndex[n] = line[n];
				}

				creditsIndex[n] = null;
				credits = creditsIndex;
			}
			else
			{
				isdeveloper = FS.Developer_searchpath( 1 );
				if ( isdeveloper == 1 )
					credits = xatcredits;
				else if ( isdeveloper == 2 )
					credits = roguecredits;
				else
				{
					credits = idcredits;
				}
			}

			credits_start_time = cls.realtime;
			PushMenu( new Anonymousxcommand_t13(), new Anonymouskeyfunc_t7() );
		}

		private sealed class Anonymousxcommand_t13 : xcommand_t
		{
			public override void Execute( )
			{
				Credits_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t7 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Credits_Key( key );
			}
		}

		static Int32 m_game_cursor;
		static menuframework_s s_game_menu = new menuframework_s();
		static menuaction_s s_easy_game_action = new menuaction_s();
		static menuaction_s s_medium_game_action = new menuaction_s();
		static menuaction_s s_hard_game_action = new menuaction_s();
		static menuaction_s s_load_game_action = new menuaction_s();
		static menuaction_s s_save_game_action = new menuaction_s();
		static menuaction_s s_credits_action = new menuaction_s();
		static menuseparator_s s_blankline = new menuseparator_s();
		static void StartGame( )
		{
			cl.servercount = -1;
			ForceMenuOff();
			Cvar.SetValue( "deathmatch", 0 );
			Cvar.SetValue( "coop", 0 );
			Cvar.SetValue( "gamerules", 0 );
			Cbuf.AddText( "loading ; killserver ; wait ; newgame\\n" );
			cls.key_dest = key_game;
		}

		static void EasyGameFunc( Object data )
		{
			Cvar.ForceSet( "skill", "0" );
			StartGame();
		}

		static void MediumGameFunc( Object data )
		{
			Cvar.ForceSet( "skill", "1" );
			StartGame();
		}

		static void HardGameFunc( Object data )
		{
			Cvar.ForceSet( "skill", "2" );
			StartGame();
		}

		static void LoadGameFunc( Object unused )
		{
			Menu_LoadGame_f();
		}

		static void SaveGameFunc( Object unused )
		{
			Menu_SaveGame_f();
		}

		static void CreditsFunc( Object unused )
		{
			Menu_Credits_f();
		}

		static String[] difficulty_names = new[] { "easy", "medium", "fuckin shitty hard" };
		static void Game_MenuInit( )
		{
			s_game_menu.x = ( Int32 ) ( viddef.GetWidth() * 0.5 );
			s_game_menu.nitems = 0;
			s_easy_game_action.type = MTYPE_ACTION;
			s_easy_game_action.flags = QMF_LEFT_JUSTIFY;
			s_easy_game_action.x = 0;
			s_easy_game_action.y = 0;
			s_easy_game_action.name = "easy";
			s_easy_game_action.callback = new Anonymousmcallback41();
			s_medium_game_action.type = MTYPE_ACTION;
			s_medium_game_action.flags = QMF_LEFT_JUSTIFY;
			s_medium_game_action.x = 0;
			s_medium_game_action.y = 10;
			s_medium_game_action.name = "medium";
			s_medium_game_action.callback = new Anonymousmcallback42();
			s_hard_game_action.type = MTYPE_ACTION;
			s_hard_game_action.flags = QMF_LEFT_JUSTIFY;
			s_hard_game_action.x = 0;
			s_hard_game_action.y = 20;
			s_hard_game_action.name = "hard";
			s_hard_game_action.callback = new Anonymousmcallback43();
			s_blankline.type = MTYPE_SEPARATOR;
			s_load_game_action.type = MTYPE_ACTION;
			s_load_game_action.flags = QMF_LEFT_JUSTIFY;
			s_load_game_action.x = 0;
			s_load_game_action.y = 40;
			s_load_game_action.name = "load game";
			s_load_game_action.callback = new Anonymousmcallback44();
			s_save_game_action.type = MTYPE_ACTION;
			s_save_game_action.flags = QMF_LEFT_JUSTIFY;
			s_save_game_action.x = 0;
			s_save_game_action.y = 50;
			s_save_game_action.name = "save game";
			s_save_game_action.callback = new Anonymousmcallback45();
			s_credits_action.type = MTYPE_ACTION;
			s_credits_action.flags = QMF_LEFT_JUSTIFY;
			s_credits_action.x = 0;
			s_credits_action.y = 60;
			s_credits_action.name = "credits";
			s_credits_action.callback = new Anonymousmcallback46();
			Menu_AddItem( s_game_menu, s_easy_game_action );
			Menu_AddItem( s_game_menu, s_medium_game_action );
			Menu_AddItem( s_game_menu, s_hard_game_action );
			Menu_AddItem( s_game_menu, s_blankline );
			Menu_AddItem( s_game_menu, s_load_game_action );
			Menu_AddItem( s_game_menu, s_save_game_action );
			Menu_AddItem( s_game_menu, s_blankline );
			Menu_AddItem( s_game_menu, s_credits_action );
			Menu_Center( s_game_menu );
		}

		private sealed class Anonymousmcallback41 : mcallback
		{
			public override void Execute( Object o )
			{
				EasyGameFunc( o );
			}
		}

		private sealed class Anonymousmcallback42 : mcallback
		{
			public override void Execute( Object o )
			{
				MediumGameFunc( o );
			}
		}

		private sealed class Anonymousmcallback43 : mcallback
		{
			public override void Execute( Object o )
			{
				HardGameFunc( o );
			}
		}

		private sealed class Anonymousmcallback44 : mcallback
		{
			public override void Execute( Object o )
			{
				LoadGameFunc( o );
			}
		}

		private sealed class Anonymousmcallback45 : mcallback
		{
			public override void Execute( Object o )
			{
				SaveGameFunc( o );
			}
		}

		private sealed class Anonymousmcallback46 : mcallback
		{
			public override void Execute( Object o )
			{
				CreditsFunc( o );
			}
		}

		static void Game_MenuDraw( )
		{
			Banner( "m_banner_game" );
			Menu_AdjustCursor( s_game_menu, 1 );
			Menu_Draw( s_game_menu );
		}

		static String Game_MenuKey( Int32 key )
		{
			return Default_MenuKey( s_game_menu, key );
		}

		static xcommand_t Menu_Game = new Anonymousxcommand_t14();
		private sealed class Anonymousxcommand_t14 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_Game_f();
			}
		}

		static void Menu_Game_f( )
		{
			Game_MenuInit();
			PushMenu( new Anonymousxcommand_t15(), new Anonymouskeyfunc_t8() );
			m_game_cursor = 1;
		}

		private sealed class Anonymousxcommand_t15 : xcommand_t
		{
			public override void Execute( )
			{
				Game_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t8 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Game_MenuKey( key );
			}
		}

		public static readonly Int32 MAX_SAVEGAMES = 15;
		static menuframework_s s_savegame_menu = new menuframework_s();
		static menuframework_s s_loadgame_menu = new menuframework_s();
		static menuaction_s[] s_loadgame_actions = new menuaction_s[MAX_SAVEGAMES];
		static menuaction_s[] s_savegame_actions = new menuaction_s[MAX_SAVEGAMES];
		static String[] m_savestrings = new String[MAX_SAVEGAMES];
		static menuframework_s s_joinserver_menu = new menuframework_s();
		static menuseparator_s s_joinserver_server_title = new menuseparator_s();
		static menuaction_s s_joinserver_search_action = new menuaction_s();
		static menuaction_s s_joinserver_address_book_action = new menuaction_s();
		static netadr_t[] local_server_netadr = new netadr_t[MAX_LOCAL_SERVERS];
		static String[] local_server_names = new String[MAX_LOCAL_SERVERS];
		static menuaction_s[] s_joinserver_server_actions = new menuaction_s[MAX_LOCAL_SERVERS];
		static menuframework_s s_addressbook_menu = new menuframework_s();
		static menufield_s[] s_addressbook_fields = new menufield_s[NUM_ADDRESSBOOK_ENTRIES];

		static Menu( )
		{
			for ( var n = 0; n < MAX_SAVEGAMES; n++ )
				s_loadgame_actions[n] = new menuaction_s();

			for ( var n = 0; n < MAX_SAVEGAMES; n++ )
				m_savestrings[n] = "";

			for ( var n = 0; n < MAX_SAVEGAMES; n++ )
				s_savegame_actions[n] = new menuaction_s();

			for ( var n = 0; n < MAX_LOCAL_SERVERS; n++ )
			{
				local_server_netadr[n] = new netadr_t();
				local_server_names[n] = "";
				s_joinserver_server_actions[n] = new menuaction_s();
				s_joinserver_server_actions[n].n = n;
			}

			for ( var n = 0; n < NUM_ADDRESSBOOK_ENTRIES; n++ )
				s_addressbook_fields[n] = new menufield_s();
		}

		static Boolean[] m_savevalid = new Boolean[MAX_SAVEGAMES];
		static void Create_Savestrings( )
		{
			Int32 i;
			QuakeFile f;
			String name;
			for ( i = 0; i < MAX_SAVEGAMES; i++ )
			{
				m_savestrings[i] = "<EMPTY>";
				name = FS.Gamedir() + "/save/save" + i + "/server.ssv";
				try
				{
					f = new QuakeFile( name, FileAccess.Read );
					if ( f == null )
					{
						m_savestrings[i] = "<EMPTY>";
						m_savevalid[i] = false;
					}
					else
					{
						var str = f.ReadString();
						if ( str != null )
							m_savestrings[i] = str;
						f.Close();
						m_savevalid[i] = true;
					}
				}
				catch ( Exception e )
				{
					m_savestrings[i] = "<EMPTY>";
					m_savevalid[i] = false;
				}
			}
		}

		static void LoadGameCallback( Object self )
		{
			menuaction_s a = ( menuaction_s ) self;
			if ( m_savevalid[a.localdata[0]] )
				Cbuf.AddText( "load save" + a.localdata[0] + "\\n" );
			ForceMenuOff();
		}

		static void LoadGame_MenuInit( )
		{
			Int32 i;
			s_loadgame_menu.x = viddef.GetWidth() / 2 - 120;
			s_loadgame_menu.y = viddef.GetHeight() / 2 - 58;
			s_loadgame_menu.nitems = 0;
			Create_Savestrings();
			for ( i = 0; i < MAX_SAVEGAMES; i++ )
			{
				s_loadgame_actions[i].name = m_savestrings[i];
				s_loadgame_actions[i].flags = QMF_LEFT_JUSTIFY;
				s_loadgame_actions[i].localdata[0] = i;
				s_loadgame_actions[i].callback = new Anonymousmcallback47();
				s_loadgame_actions[i].x = 0;
				s_loadgame_actions[i].y = ( i ) * 10;
				if ( i > 0 )
					s_loadgame_actions[i].y += 10;
				s_loadgame_actions[i].type = MTYPE_ACTION;
				Menu_AddItem( s_loadgame_menu, s_loadgame_actions[i] );
			}
		}

		private sealed class Anonymousmcallback47 : mcallback
		{
			public override void Execute( Object o )
			{
				LoadGameCallback( o );
			}
		}

		static void LoadGame_MenuDraw( )
		{
			Banner( "m_banner_load_game" );
			Menu_Draw( s_loadgame_menu );
		}

		static String LoadGame_MenuKey( Int32 key )
		{
			if ( key == K_ESCAPE || key == K_ENTER )
			{
				s_savegame_menu.cursor = s_loadgame_menu.cursor - 1;
				if ( s_savegame_menu.cursor < 0 )
					s_savegame_menu.cursor = 0;
			}

			return Default_MenuKey( s_loadgame_menu, key );
		}

		static xcommand_t Menu_LoadGame = new Anonymousxcommand_t16();
		private sealed class Anonymousxcommand_t16 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_LoadGame_f();
			}
		}

		static void Menu_LoadGame_f( )
		{
			LoadGame_MenuInit();
			PushMenu( new Anonymousxcommand_t17(), new Anonymouskeyfunc_t9() );
		}

		private sealed class Anonymousxcommand_t17 : xcommand_t
		{
			public override void Execute( )
			{
				LoadGame_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t9 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return LoadGame_MenuKey( key );
			}
		}

		static void SaveGameCallback( Object self )
		{
			menuaction_s a = ( menuaction_s ) self;
			Cbuf.AddText( "save save" + a.localdata[0] + "\\n" );
			ForceMenuOff();
		}

		static void SaveGame_MenuDraw( )
		{
			Banner( "m_banner_save_game" );
			Menu_AdjustCursor( s_savegame_menu, 1 );
			Menu_Draw( s_savegame_menu );
		}

		static void SaveGame_MenuInit( )
		{
			Int32 i;
			s_savegame_menu.x = viddef.GetWidth() / 2 - 120;
			s_savegame_menu.y = viddef.GetHeight() / 2 - 58;
			s_savegame_menu.nitems = 0;
			Create_Savestrings();
			for ( i = 0; i < MAX_SAVEGAMES - 1; i++ )
			{
				s_savegame_actions[i].name = m_savestrings[i + 1];
				s_savegame_actions[i].localdata[0] = i + 1;
				s_savegame_actions[i].flags = QMF_LEFT_JUSTIFY;
				s_savegame_actions[i].callback = new Anonymousmcallback48();
				s_savegame_actions[i].x = 0;
				s_savegame_actions[i].y = ( i ) * 10;
				s_savegame_actions[i].type = MTYPE_ACTION;
				Menu_AddItem( s_savegame_menu, s_savegame_actions[i] );
			}
		}

		private sealed class Anonymousmcallback48 : mcallback
		{
			public override void Execute( Object o )
			{
				SaveGameCallback( o );
			}
		}

		static String SaveGame_MenuKey( Int32 key )
		{
			if ( key == K_ENTER || key == K_ESCAPE )
			{
				s_loadgame_menu.cursor = s_savegame_menu.cursor - 1;
				if ( s_loadgame_menu.cursor < 0 )
					s_loadgame_menu.cursor = 0;
			}

			return Default_MenuKey( s_savegame_menu, key );
		}

		static xcommand_t Menu_SaveGame = new Anonymousxcommand_t18();
		private sealed class Anonymousxcommand_t18 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_SaveGame_f();
			}
		}

		static void Menu_SaveGame_f( )
		{
			if ( 0 == Globals.server_state )
				return;
			SaveGame_MenuInit();
			PushMenu( new Anonymousxcommand_t19(), new Anonymouskeyfunc_t10() );
			Create_Savestrings();
		}

		private sealed class Anonymousxcommand_t19 : xcommand_t
		{
			public override void Execute( )
			{
				SaveGame_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t10 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return SaveGame_MenuKey( key );
			}
		}


		static Int32 m_num_servers;
		public static void AddToServerList( netadr_t adr, String info )
		{
			Int32 i;
			if ( m_num_servers == MAX_LOCAL_SERVERS )
				return;
			var x = info.Trim();
			for ( i = 0; i < m_num_servers; i++ )
				if ( x.Equals( local_server_names[i] ) )
					return;
			local_server_netadr[m_num_servers].Set( adr );
			local_server_names[m_num_servers] = x;
			s_joinserver_server_actions[m_num_servers].name = x;
			m_num_servers++;
		}

		static void JoinServerFunc( Object self )
		{
			String buffer;
			Int32 index;
			index = ( ( menucommon_s ) self ).n;
			if ( Lib.Q_stricmp( local_server_names[index], NO_SERVER_STRING ) == 0 )
				return;
			if ( index >= m_num_servers )
				return;
			buffer = "connect " + NET.AdrToString( local_server_netadr[index] ) + "\\n";
			Cbuf.AddText( buffer );
			ForceMenuOff();
		}

		static void AddressBookFunc( Object self )
		{
			Menu_AddressBook_f();
		}

		static void NullCursorDraw( Object self )
		{
		}

		static void SearchLocalGames( )
		{
			Int32 i;
			m_num_servers = 0;
			for ( i = 0; i < MAX_LOCAL_SERVERS; i++ )
				local_server_names[i] = NO_SERVER_STRING;
			DrawTextBox( 8, 120 - 48, 36, 3 );
			Print( 16 + 16, 120 - 48 + 8, "Searching for local servers, this" );
			Print( 16 + 16, 120 - 48 + 16, "could take up to a minute, so" );
			Print( 16 + 16, 120 - 48 + 24, "please be patient." );
			re.EndFrame();
			CL.PingServers_f.Execute();
		}

		static void SearchLocalGamesFunc( Object self )
		{
			SearchLocalGames();
		}

		static void JoinServer_MenuInit( )
		{
			Int32 i;
			s_joinserver_menu.x = ( Int32 ) ( viddef.GetWidth() * 0.5 - 120 );
			s_joinserver_menu.nitems = 0;
			s_joinserver_address_book_action.type = MTYPE_ACTION;
			s_joinserver_address_book_action.name = "address book";
			s_joinserver_address_book_action.flags = QMF_LEFT_JUSTIFY;
			s_joinserver_address_book_action.x = 0;
			s_joinserver_address_book_action.y = 0;
			s_joinserver_address_book_action.callback = new Anonymousmcallback49();
			s_joinserver_search_action.type = MTYPE_ACTION;
			s_joinserver_search_action.name = "refresh server list";
			s_joinserver_search_action.flags = QMF_LEFT_JUSTIFY;
			s_joinserver_search_action.x = 0;
			s_joinserver_search_action.y = 10;
			s_joinserver_search_action.callback = new Anonymousmcallback50();
			s_joinserver_search_action.statusbar = "search for servers";
			s_joinserver_server_title.type = MTYPE_SEPARATOR;
			s_joinserver_server_title.name = "connect to...";
			s_joinserver_server_title.x = 80;
			s_joinserver_server_title.y = 30;
			for ( i = 0; i < MAX_LOCAL_SERVERS; i++ )
			{
				s_joinserver_server_actions[i].type = MTYPE_ACTION;
				local_server_names[i] = NO_SERVER_STRING;
				s_joinserver_server_actions[i].name = local_server_names[i];
				s_joinserver_server_actions[i].flags = QMF_LEFT_JUSTIFY;
				s_joinserver_server_actions[i].x = 0;
				s_joinserver_server_actions[i].y = 40 + i * 10;
				s_joinserver_server_actions[i].callback = new Anonymousmcallback51();
				s_joinserver_server_actions[i].statusbar = "press ENTER to connect";
			}

			Menu_AddItem( s_joinserver_menu, s_joinserver_address_book_action );
			Menu_AddItem( s_joinserver_menu, s_joinserver_server_title );
			Menu_AddItem( s_joinserver_menu, s_joinserver_search_action );
			for ( i = 0; i < 8; i++ )
				Menu_AddItem( s_joinserver_menu, s_joinserver_server_actions[i] );
			Menu_Center( s_joinserver_menu );
			SearchLocalGames();
		}

		private sealed class Anonymousmcallback49 : mcallback
		{
			public override void Execute( Object o )
			{
				AddressBookFunc( o );
			}
		}

		private sealed class Anonymousmcallback50 : mcallback
		{
			public override void Execute( Object o )
			{
				SearchLocalGamesFunc( o );
			}
		}

		private sealed class Anonymousmcallback51 : mcallback
		{
			public override void Execute( Object o )
			{
				JoinServerFunc( o );
			}
		}

		static void JoinServer_MenuDraw( )
		{
			Banner( "m_banner_join_server" );
			Menu_Draw( s_joinserver_menu );
		}

		static String JoinServer_MenuKey( Int32 key )
		{
			return Default_MenuKey( s_joinserver_menu, key );
		}

		static xcommand_t Menu_JoinServer = new Anonymousxcommand_t20();
		private sealed class Anonymousxcommand_t20 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_JoinServer_f();
			}
		}

		static void Menu_JoinServer_f( )
		{
			JoinServer_MenuInit();
			PushMenu( new Anonymousxcommand_t21(), new Anonymouskeyfunc_t11() );
		}

		private sealed class Anonymousxcommand_t21 : xcommand_t
		{
			public override void Execute( )
			{
				JoinServer_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t11 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return JoinServer_MenuKey( key );
			}
		}

		static menuframework_s s_startserver_menu = new menuframework_s();
		static String[] mapnames;
		static Int32 nummaps;
		static menuaction_s s_startserver_start_action = new menuaction_s();
		static menuaction_s s_startserver_dmoptions_action = new menuaction_s();
		static menufield_s s_timelimit_field = new menufield_s();
		static menufield_s s_fraglimit_field = new menufield_s();
		static menufield_s s_maxclients_field = new menufield_s();
		static menufield_s s_hostname_field = new menufield_s();
		static menulist_s s_startmap_list = new menulist_s();
		static menulist_s s_rules_box = new menulist_s();
		static void DMOptionsFunc( Object self )
		{
			if ( s_rules_box.curvalue == 1 )
				return;
			Menu_DMOptions_f();
		}

		static void RulesChangeFunc( Object self )
		{
			if ( s_rules_box.curvalue == 0 )
			{
				s_maxclients_field.statusbar = null;
				s_startserver_dmoptions_action.statusbar = null;
			}
			else if ( s_rules_box.curvalue == 1 )
			{
				s_maxclients_field.statusbar = "4 maximum for cooperative";
				if ( Lib.Atoi( s_maxclients_field.buffer.ToString() ) > 4 )
					s_maxclients_field.buffer = new StringBuffer( "4" );
				s_startserver_dmoptions_action.statusbar = "N/A for cooperative";
			}
			else if ( FS.Developer_searchpath( 2 ) == 2 )
			{
				if ( s_rules_box.curvalue == 2 )
				{
					s_maxclients_field.statusbar = null;
					s_startserver_dmoptions_action.statusbar = null;
				}
			}
		}

		static void StartServerActionFunc( Object self )
		{
			String startmap;
			Int32 timelimit;
			Int32 fraglimit;
			Int32 maxclients;
			String spot;
			var x = mapnames[s_startmap_list.curvalue];
			var pos = x.IndexOf( '\\' );
			if ( pos == -1 )
				startmap = x;
			else
				startmap = x.Substring( pos + 1, x.Length );
			maxclients = Lib.Atoi( s_maxclients_field.buffer.ToString() );
			timelimit = Lib.Atoi( s_timelimit_field.buffer.ToString() );
			fraglimit = Lib.Atoi( s_fraglimit_field.buffer.ToString() );
			Cvar.SetValue( "maxclients", ClampCvar( 0, maxclients, maxclients ) );
			Cvar.SetValue( "timelimit", ClampCvar( 0, timelimit, timelimit ) );
			Cvar.SetValue( "fraglimit", ClampCvar( 0, fraglimit, fraglimit ) );
			Cvar.Set( "hostname", s_hostname_field.buffer.ToString() );
			if ( ( s_rules_box.curvalue < 2 ) || ( FS.Developer_searchpath( 2 ) != 2 ) )
			{
				Cvar.SetValue( "deathmatch", 1 - ( Int32 ) ( s_rules_box.curvalue ) );
				Cvar.SetValue( "coop", s_rules_box.curvalue );
				Cvar.SetValue( "gamerules", 0 );
			}
			else
			{
				Cvar.SetValue( "deathmatch", 1 );
				Cvar.SetValue( "coop", 0 );
				Cvar.SetValue( "gamerules", s_rules_box.curvalue );
			}

			spot = null;
			if ( s_rules_box.curvalue == 1 )
			{
				if ( Lib.Q_stricmp( startmap, "bunk1" ) == 0 )
					spot = "start";
				else if ( Lib.Q_stricmp( startmap, "mintro" ) == 0 )
					spot = "start";
				else if ( Lib.Q_stricmp( startmap, "fact1" ) == 0 )
					spot = "start";
				else if ( Lib.Q_stricmp( startmap, "power1" ) == 0 )
					spot = "pstart";
				else if ( Lib.Q_stricmp( startmap, "biggun" ) == 0 )
					spot = "bstart";
				else if ( Lib.Q_stricmp( startmap, "hangar1" ) == 0 )
					spot = "unitstart";
				else if ( Lib.Q_stricmp( startmap, "city1" ) == 0 )
					spot = "unitstart";
				else if ( Lib.Q_stricmp( startmap, "boss1" ) == 0 )
					spot = "bosstart";
			}

			if ( spot != null )
			{
				if ( Globals.server_state != 0 )
					Cbuf.AddText( "disconnect\\n" );
				Cbuf.AddText( "gamemap \\\"*" + startmap + "$" + spot + "\\\"\\n" );
			}
			else
			{
				Cbuf.AddText( "map " + startmap + "\\n" );
			}

			ForceMenuOff();
		}

		static String[] dm_coop_names = new[] { "deathmatch", "cooperative" };
		static String[] dm_coop_names_rogue = new[] { "deathmatch", "cooperative", "tag" };
		static void StartServer_MenuInit( )
		{
			Byte[] buffer = null;
			String mapsname;
			String s;
			Int32 i;
			FileStream fp;
			mapsname = FS.Gamedir() + "/maps.lst";
			if ( ( fp = Lib.Fopen( mapsname, FileMode.Open, FileAccess.Read ) ) == null )
			{
				buffer = FS.LoadFile( "maps.lst" );
				if ( buffer == null )
					Com.Error( ERR_DROP, "couldn't find maps.lst\\n" );
			}
			else
			{
				try
				{
					var len = ( Int32 ) fp.Length;
					buffer = new Byte[len];
					fp.Read( buffer, 0, len );
				}
				catch ( Exception e )
				{
					Com.Error( ERR_DROP, "couldn't load maps.lst\\n" );
				}
			}

			s = Encoding.ASCII.GetString( buffer );
			String[] lines = s.Split( "\\r\\n" );
			nummaps = lines.Length;
			if ( nummaps == 0 )
				Com.Error( ERR_DROP, "no maps in maps.lst\\n" );
			mapnames = new String[nummaps];
			for ( i = 0; i < nummaps; i++ )
			{
				String shortname, longname, scratch;
				Com.ParseHelp ph = new ParseHelp( lines[i] );
				shortname = Com.Parse( ph ).ToUpper();
				longname = Com.Parse( ph );
				scratch = longname + "\\n" + shortname;
				mapnames[i] = scratch;
			}

			if ( fp != null )
			{
				Lib.Fclose( fp );
				fp = null;
			}
			else
			{
				FS.FreeFile( buffer );
			}

			s_startserver_menu.x = ( Int32 ) ( viddef.GetWidth() * 0.5 );
			s_startserver_menu.nitems = 0;
			s_startmap_list.type = MTYPE_SPINCONTROL;
			s_startmap_list.x = 0;
			s_startmap_list.y = 0;
			s_startmap_list.name = "initial map";
			s_startmap_list.itemnames = mapnames;
			s_rules_box.type = MTYPE_SPINCONTROL;
			s_rules_box.x = 0;
			s_rules_box.y = 20;
			s_rules_box.name = "rules";
			if ( FS.Developer_searchpath( 2 ) == 2 )
				s_rules_box.itemnames = dm_coop_names_rogue;
			else
				s_rules_box.itemnames = dm_coop_names;
			if ( Cvar.VariableValue( "coop" ) != 0 )
				s_rules_box.curvalue = 1;
			else
				s_rules_box.curvalue = 0;
			s_rules_box.callback = new Anonymousmcallback52();
			s_timelimit_field.type = MTYPE_FIELD;
			s_timelimit_field.name = "time limit";
			s_timelimit_field.flags = QMF_NUMBERSONLY;
			s_timelimit_field.x = 0;
			s_timelimit_field.y = 36;
			s_timelimit_field.statusbar = "0 = no limit";
			s_timelimit_field.length = 3;
			s_timelimit_field.visible_length = 3;
			s_timelimit_field.buffer = new StringBuffer( Cvar.VariableString( "timelimit" ) );
			s_fraglimit_field.type = MTYPE_FIELD;
			s_fraglimit_field.name = "frag limit";
			s_fraglimit_field.flags = QMF_NUMBERSONLY;
			s_fraglimit_field.x = 0;
			s_fraglimit_field.y = 54;
			s_fraglimit_field.statusbar = "0 = no limit";
			s_fraglimit_field.length = 3;
			s_fraglimit_field.visible_length = 3;
			s_fraglimit_field.buffer = new StringBuffer( Cvar.VariableString( "fraglimit" ) );
			s_maxclients_field.type = MTYPE_FIELD;
			s_maxclients_field.name = "max players";
			s_maxclients_field.flags = QMF_NUMBERSONLY;
			s_maxclients_field.x = 0;
			s_maxclients_field.y = 72;
			s_maxclients_field.statusbar = null;
			s_maxclients_field.length = 3;
			s_maxclients_field.visible_length = 3;
			if ( Cvar.VariableValue( "maxclients" ) == 1 )
				s_maxclients_field.buffer = new StringBuffer( "8" );
			else
				s_maxclients_field.buffer = new StringBuffer( Cvar.VariableString( "maxclients" ) );
			s_hostname_field.type = MTYPE_FIELD;
			s_hostname_field.name = "hostname";
			s_hostname_field.flags = 0;
			s_hostname_field.x = 0;
			s_hostname_field.y = 90;
			s_hostname_field.statusbar = null;
			s_hostname_field.length = 12;
			s_hostname_field.visible_length = 12;
			s_hostname_field.buffer = new StringBuffer( Cvar.VariableString( "hostname" ) );
			s_hostname_field.cursor = s_hostname_field.buffer.Length;
			s_startserver_dmoptions_action.type = MTYPE_ACTION;
			s_startserver_dmoptions_action.name = " deathmatch flags";
			s_startserver_dmoptions_action.flags = QMF_LEFT_JUSTIFY;
			s_startserver_dmoptions_action.x = 24;
			s_startserver_dmoptions_action.y = 108;
			s_startserver_dmoptions_action.statusbar = null;
			s_startserver_dmoptions_action.callback = new Anonymousmcallback53();
			s_startserver_start_action.type = MTYPE_ACTION;
			s_startserver_start_action.name = " begin";
			s_startserver_start_action.flags = QMF_LEFT_JUSTIFY;
			s_startserver_start_action.x = 24;
			s_startserver_start_action.y = 128;
			s_startserver_start_action.callback = new Anonymousmcallback54();
			Menu_AddItem( s_startserver_menu, s_startmap_list );
			Menu_AddItem( s_startserver_menu, s_rules_box );
			Menu_AddItem( s_startserver_menu, s_timelimit_field );
			Menu_AddItem( s_startserver_menu, s_fraglimit_field );
			Menu_AddItem( s_startserver_menu, s_maxclients_field );
			Menu_AddItem( s_startserver_menu, s_hostname_field );
			Menu_AddItem( s_startserver_menu, s_startserver_dmoptions_action );
			Menu_AddItem( s_startserver_menu, s_startserver_start_action );
			Menu_Center( s_startserver_menu );
			RulesChangeFunc( null );
		}

		private sealed class Anonymousmcallback52 : mcallback
		{
			public override void Execute( Object o )
			{
				RulesChangeFunc( o );
			}
		}

		private sealed class Anonymousmcallback53 : mcallback
		{
			public override void Execute( Object o )
			{
				DMOptionsFunc( o );
			}
		}

		private sealed class Anonymousmcallback54 : mcallback
		{
			public override void Execute( Object o )
			{
				StartServerActionFunc( o );
			}
		}

		static void StartServer_MenuDraw( )
		{
			Menu_Draw( s_startserver_menu );
		}

		static String StartServer_MenuKey( Int32 key )
		{
			if ( key == K_ESCAPE )
			{
				if ( mapnames != null )
				{
					Int32 i;
					for ( i = 0; i < nummaps; i++ )
						mapnames[i] = null;
				}

				mapnames = null;
				nummaps = 0;
			}

			return Default_MenuKey( s_startserver_menu, key );
		}

		static xcommand_t Menu_StartServer = new Anonymousxcommand_t22();
		private sealed class Anonymousxcommand_t22 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_StartServer_f();
			}
		}

		static xcommand_t startServer_MenuDraw = new Anonymousxcommand_t23();
		private sealed class Anonymousxcommand_t23 : xcommand_t
		{
			public override void Execute( )
			{
				StartServer_MenuDraw();
			}
		}

		static keyfunc_t startServer_MenuKey = new Anonymouskeyfunc_t12();
		private sealed class Anonymouskeyfunc_t12 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return StartServer_MenuKey( key );
			}
		}

		static void Menu_StartServer_f( )
		{
			StartServer_MenuInit();
			PushMenu( startServer_MenuDraw, startServer_MenuKey );
		}

		static String dmoptions_statusbar;
		static menuframework_s s_dmoptions_menu = new menuframework_s();
		static menulist_s s_friendlyfire_box = new menulist_s();
		static menulist_s s_falls_box = new menulist_s();
		static menulist_s s_weapons_stay_box = new menulist_s();
		static menulist_s s_instant_powerups_box = new menulist_s();
		static menulist_s s_powerups_box = new menulist_s();
		static menulist_s s_health_box = new menulist_s();
		static menulist_s s_spawn_farthest_box = new menulist_s();
		static menulist_s s_teamplay_box = new menulist_s();
		static menulist_s s_samelevel_box = new menulist_s();
		static menulist_s s_force_respawn_box = new menulist_s();
		static menulist_s s_armor_box = new menulist_s();
		static menulist_s s_allow_exit_box = new menulist_s();
		static menulist_s s_infinite_ammo_box = new menulist_s();
		static menulist_s s_fixed_fov_box = new menulist_s();
		static menulist_s s_quad_drop_box = new menulist_s();
		static menulist_s s_no_mines_box = new menulist_s();
		static menulist_s s_no_nukes_box = new menulist_s();
		static menulist_s s_stack_double_box = new menulist_s();
		static menulist_s s_no_spheres_box = new menulist_s();
		static void Setvalue( Int32 flags )
		{
			Cvar.SetValue( "dmflags", flags );
			dmoptions_statusbar = "dmflags = " + flags;
		}

		static void DMFlagCallback( Object self )
		{
			menulist_s f = ( menulist_s ) self;
			Int32 flags;
			var bit = 0;
			flags = ( Int32 ) Cvar.VariableValue( "dmflags" );
			if ( f == s_friendlyfire_box )
			{
				if ( f.curvalue != 0 )
					flags &= ~DF_NO_FRIENDLY_FIRE;
				else
					flags |= DF_NO_FRIENDLY_FIRE;
				Setvalue( flags );
				return;
			}
			else if ( f == s_falls_box )
			{
				if ( f.curvalue != 0 )
					flags &= ~DF_NO_FALLING;
				else
					flags |= DF_NO_FALLING;
				Setvalue( flags );
				return;
			}
			else if ( f == s_weapons_stay_box )
			{
				bit = DF_WEAPONS_STAY;
			}
			else if ( f == s_instant_powerups_box )
			{
				bit = DF_INSTANT_ITEMS;
			}
			else if ( f == s_allow_exit_box )
			{
				bit = DF_ALLOW_EXIT;
			}
			else if ( f == s_powerups_box )
			{
				if ( f.curvalue != 0 )
					flags &= ~DF_NO_ITEMS;
				else
					flags |= DF_NO_ITEMS;
				Setvalue( flags );
				return;
			}
			else if ( f == s_health_box )
			{
				if ( f.curvalue != 0 )
					flags &= ~DF_NO_HEALTH;
				else
					flags |= DF_NO_HEALTH;
				Setvalue( flags );
				return;
			}
			else if ( f == s_spawn_farthest_box )
			{
				bit = DF_SPAWN_FARTHEST;
			}
			else if ( f == s_teamplay_box )
			{
				if ( f.curvalue == 1 )
				{
					flags |= DF_SKINTEAMS;
					flags &= ~DF_MODELTEAMS;
				}
				else if ( f.curvalue == 2 )
				{
					flags |= DF_MODELTEAMS;
					flags &= ~DF_SKINTEAMS;
				}
				else
				{
					flags &= ~( DF_MODELTEAMS | DF_SKINTEAMS );
				}

				Setvalue( flags );
				return;
			}
			else if ( f == s_samelevel_box )
			{
				bit = DF_SAME_LEVEL;
			}
			else if ( f == s_force_respawn_box )
			{
				bit = DF_FORCE_RESPAWN;
			}
			else if ( f == s_armor_box )
			{
				if ( f.curvalue != 0 )
					flags &= ~DF_NO_ARMOR;
				else
					flags |= DF_NO_ARMOR;
				Setvalue( flags );
				return;
			}
			else if ( f == s_infinite_ammo_box )
			{
				bit = DF_INFINITE_AMMO;
			}
			else if ( f == s_fixed_fov_box )
			{
				bit = DF_FIXED_FOV;
			}
			else if ( f == s_quad_drop_box )
			{
				bit = DF_QUAD_DROP;
			}
			else if ( FS.Developer_searchpath( 2 ) == 2 )
			{
				if ( f == s_no_mines_box )
				{
					bit = DF_NO_MINES;
				}
				else if ( f == s_no_nukes_box )
				{
					bit = DF_NO_NUKES;
				}
				else if ( f == s_stack_double_box )
				{
					bit = DF_NO_STACK_DOUBLE;
				}
				else if ( f == s_no_spheres_box )
				{
					bit = DF_NO_SPHERES;
				}
			}

			if ( f != null )
			{
				if ( f.curvalue == 0 )
					flags &= ~bit;
				else
					flags |= bit;
			}

			Cvar.SetValue( "dmflags", flags );
			dmoptions_statusbar = "dmflags = " + flags;
		}

		static String[] teamplay_names = new[] { "disabled", "by skin", "by model" };
		static void DMOptions_MenuInit( )
		{
			var dmflags = ( Int32 ) Cvar.VariableValue( "dmflags" );
			var y = 0;
			s_dmoptions_menu.x = ( Int32 ) ( viddef.GetWidth() * 0.5 );
			s_dmoptions_menu.nitems = 0;
			s_falls_box.type = MTYPE_SPINCONTROL;
			s_falls_box.x = 0;
			s_falls_box.y = y;
			s_falls_box.name = "falling damage";
			s_falls_box.callback = new Anonymousmcallback55();
			s_falls_box.itemnames = yes_no_names;
			s_falls_box.curvalue = ( dmflags & DF_NO_FALLING ) == 0 ? 1 : 0;
			s_weapons_stay_box.type = MTYPE_SPINCONTROL;
			s_weapons_stay_box.x = 0;
			s_weapons_stay_box.y = y += 10;
			s_weapons_stay_box.name = "weapons stay";
			s_weapons_stay_box.callback = new Anonymousmcallback56();
			s_weapons_stay_box.itemnames = yes_no_names;
			s_weapons_stay_box.curvalue = ( dmflags & DF_WEAPONS_STAY ) != 0 ? 1 : 0;
			s_instant_powerups_box.type = MTYPE_SPINCONTROL;
			s_instant_powerups_box.x = 0;
			s_instant_powerups_box.y = y += 10;
			s_instant_powerups_box.name = "instant powerups";
			s_instant_powerups_box.callback = new Anonymousmcallback57();
			s_instant_powerups_box.itemnames = yes_no_names;
			s_instant_powerups_box.curvalue = ( dmflags & DF_INSTANT_ITEMS ) != 0 ? 1 : 0;
			s_powerups_box.type = MTYPE_SPINCONTROL;
			s_powerups_box.x = 0;
			s_powerups_box.y = y += 10;
			s_powerups_box.name = "allow powerups";
			s_powerups_box.callback = new Anonymousmcallback58();
			s_powerups_box.itemnames = yes_no_names;
			s_powerups_box.curvalue = ( dmflags & DF_NO_ITEMS ) == 0 ? 1 : 0;
			s_health_box.type = MTYPE_SPINCONTROL;
			s_health_box.x = 0;
			s_health_box.y = y += 10;
			s_health_box.callback = new Anonymousmcallback59();
			s_health_box.name = "allow health";
			s_health_box.itemnames = yes_no_names;
			s_health_box.curvalue = ( dmflags & DF_NO_HEALTH ) == 0 ? 1 : 0;
			s_armor_box.type = MTYPE_SPINCONTROL;
			s_armor_box.x = 0;
			s_armor_box.y = y += 10;
			s_armor_box.name = "allow armor";
			s_armor_box.callback = new Anonymousmcallback60();
			s_armor_box.itemnames = yes_no_names;
			s_armor_box.curvalue = ( dmflags & DF_NO_ARMOR ) == 0 ? 1 : 0;
			s_spawn_farthest_box.type = MTYPE_SPINCONTROL;
			s_spawn_farthest_box.x = 0;
			s_spawn_farthest_box.y = y += 10;
			s_spawn_farthest_box.name = "spawn farthest";
			s_spawn_farthest_box.callback = new Anonymousmcallback61();
			s_spawn_farthest_box.itemnames = yes_no_names;
			s_spawn_farthest_box.curvalue = ( dmflags & DF_SPAWN_FARTHEST ) != 0 ? 1 : 0;
			s_samelevel_box.type = MTYPE_SPINCONTROL;
			s_samelevel_box.x = 0;
			s_samelevel_box.y = y += 10;
			s_samelevel_box.name = "same map";
			s_samelevel_box.callback = new Anonymousmcallback62();
			s_samelevel_box.itemnames = yes_no_names;
			s_samelevel_box.curvalue = ( dmflags & DF_SAME_LEVEL ) != 0 ? 1 : 0;
			s_force_respawn_box.type = MTYPE_SPINCONTROL;
			s_force_respawn_box.x = 0;
			s_force_respawn_box.y = y += 10;
			s_force_respawn_box.name = "force respawn";
			s_force_respawn_box.callback = new Anonymousmcallback63();
			s_force_respawn_box.itemnames = yes_no_names;
			s_force_respawn_box.curvalue = ( dmflags & DF_FORCE_RESPAWN ) != 0 ? 1 : 0;
			s_teamplay_box.type = MTYPE_SPINCONTROL;
			s_teamplay_box.x = 0;
			s_teamplay_box.y = y += 10;
			s_teamplay_box.name = "teamplay";
			s_teamplay_box.callback = new Anonymousmcallback64();
			s_teamplay_box.itemnames = teamplay_names;
			s_allow_exit_box.type = MTYPE_SPINCONTROL;
			s_allow_exit_box.x = 0;
			s_allow_exit_box.y = y += 10;
			s_allow_exit_box.name = "allow exit";
			s_allow_exit_box.callback = new Anonymousmcallback65();
			s_allow_exit_box.itemnames = yes_no_names;
			s_allow_exit_box.curvalue = ( dmflags & DF_ALLOW_EXIT ) != 0 ? 1 : 0;
			s_infinite_ammo_box.type = MTYPE_SPINCONTROL;
			s_infinite_ammo_box.x = 0;
			s_infinite_ammo_box.y = y += 10;
			s_infinite_ammo_box.name = "infinite ammo";
			s_infinite_ammo_box.callback = new Anonymousmcallback66();
			s_infinite_ammo_box.itemnames = yes_no_names;
			s_infinite_ammo_box.curvalue = ( dmflags & DF_INFINITE_AMMO ) != 0 ? 1 : 0;
			s_fixed_fov_box.type = MTYPE_SPINCONTROL;
			s_fixed_fov_box.x = 0;
			s_fixed_fov_box.y = y += 10;
			s_fixed_fov_box.name = "fixed FOV";
			s_fixed_fov_box.callback = new Anonymousmcallback67();
			s_fixed_fov_box.itemnames = yes_no_names;
			s_fixed_fov_box.curvalue = ( dmflags & DF_FIXED_FOV ) != 0 ? 1 : 0;
			s_quad_drop_box.type = MTYPE_SPINCONTROL;
			s_quad_drop_box.x = 0;
			s_quad_drop_box.y = y += 10;
			s_quad_drop_box.name = "quad drop";
			s_quad_drop_box.callback = new Anonymousmcallback68();
			s_quad_drop_box.itemnames = yes_no_names;
			s_quad_drop_box.curvalue = ( dmflags & DF_QUAD_DROP ) != 0 ? 1 : 0;
			s_friendlyfire_box.type = MTYPE_SPINCONTROL;
			s_friendlyfire_box.x = 0;
			s_friendlyfire_box.y = y += 10;
			s_friendlyfire_box.name = "friendly fire";
			s_friendlyfire_box.callback = new Anonymousmcallback69();
			s_friendlyfire_box.itemnames = yes_no_names;
			s_friendlyfire_box.curvalue = ( dmflags & DF_NO_FRIENDLY_FIRE ) == 0 ? 1 : 0;
			if ( FS.Developer_searchpath( 2 ) == 2 )
			{
				s_no_mines_box.type = MTYPE_SPINCONTROL;
				s_no_mines_box.x = 0;
				s_no_mines_box.y = y += 10;
				s_no_mines_box.name = "remove mines";
				s_no_mines_box.callback = new Anonymousmcallback70();
				s_no_mines_box.itemnames = yes_no_names;
				s_no_mines_box.curvalue = ( dmflags & DF_NO_MINES ) != 0 ? 1 : 0;
				s_no_nukes_box.type = MTYPE_SPINCONTROL;
				s_no_nukes_box.x = 0;
				s_no_nukes_box.y = y += 10;
				s_no_nukes_box.name = "remove nukes";
				s_no_nukes_box.callback = new Anonymousmcallback71();
				s_no_nukes_box.itemnames = yes_no_names;
				s_no_nukes_box.curvalue = ( dmflags & DF_NO_NUKES ) != 0 ? 1 : 0;
				s_stack_double_box.type = MTYPE_SPINCONTROL;
				s_stack_double_box.x = 0;
				s_stack_double_box.y = y += 10;
				s_stack_double_box.name = "2x/4x stacking off";
				s_stack_double_box.callback = new Anonymousmcallback72();
				s_stack_double_box.itemnames = yes_no_names;
				s_stack_double_box.curvalue = ( dmflags & DF_NO_STACK_DOUBLE );
				s_no_spheres_box.type = MTYPE_SPINCONTROL;
				s_no_spheres_box.x = 0;
				s_no_spheres_box.y = y += 10;
				s_no_spheres_box.name = "remove spheres";
				s_no_spheres_box.callback = new Anonymousmcallback73();
				s_no_spheres_box.itemnames = yes_no_names;
				s_no_spheres_box.curvalue = ( dmflags & DF_NO_SPHERES ) != 0 ? 1 : 0;
			}

			Menu_AddItem( s_dmoptions_menu, s_falls_box );
			Menu_AddItem( s_dmoptions_menu, s_weapons_stay_box );
			Menu_AddItem( s_dmoptions_menu, s_instant_powerups_box );
			Menu_AddItem( s_dmoptions_menu, s_powerups_box );
			Menu_AddItem( s_dmoptions_menu, s_health_box );
			Menu_AddItem( s_dmoptions_menu, s_armor_box );
			Menu_AddItem( s_dmoptions_menu, s_spawn_farthest_box );
			Menu_AddItem( s_dmoptions_menu, s_samelevel_box );
			Menu_AddItem( s_dmoptions_menu, s_force_respawn_box );
			Menu_AddItem( s_dmoptions_menu, s_teamplay_box );
			Menu_AddItem( s_dmoptions_menu, s_allow_exit_box );
			Menu_AddItem( s_dmoptions_menu, s_infinite_ammo_box );
			Menu_AddItem( s_dmoptions_menu, s_fixed_fov_box );
			Menu_AddItem( s_dmoptions_menu, s_quad_drop_box );
			Menu_AddItem( s_dmoptions_menu, s_friendlyfire_box );
			if ( FS.Developer_searchpath( 2 ) == 2 )
			{
				Menu_AddItem( s_dmoptions_menu, s_no_mines_box );
				Menu_AddItem( s_dmoptions_menu, s_no_nukes_box );
				Menu_AddItem( s_dmoptions_menu, s_stack_double_box );
				Menu_AddItem( s_dmoptions_menu, s_no_spheres_box );
			}

			Menu_Center( s_dmoptions_menu );
			DMFlagCallback( null );
			Menu_SetStatusBar( s_dmoptions_menu, dmoptions_statusbar );
		}

		private sealed class Anonymousmcallback55 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback56 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback57 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback58 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback59 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback60 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback61 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback62 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback63 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback64 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback65 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback66 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback67 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback68 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback69 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback70 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback71 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback72 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		private sealed class Anonymousmcallback73 : mcallback
		{
			public override void Execute( Object o )
			{
				DMFlagCallback( o );
			}
		}

		static void DMOptions_MenuDraw( )
		{
			Menu_Draw( s_dmoptions_menu );
		}

		static String DMOptions_MenuKey( Int32 key )
		{
			return Default_MenuKey( s_dmoptions_menu, key );
		}

		static xcommand_t Menu_DMOptions = new Anonymousxcommand_t24();
		private sealed class Anonymousxcommand_t24 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_DMOptions_f();
			}
		}

		static void Menu_DMOptions_f( )
		{
			DMOptions_MenuInit();
			PushMenu( new Anonymousxcommand_t25(), new Anonymouskeyfunc_t13() );
		}

		private sealed class Anonymousxcommand_t25 : xcommand_t
		{
			public override void Execute( )
			{
				DMOptions_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t13 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return DMOptions_MenuKey( key );
			}
		}

		static menuframework_s s_downloadoptions_menu = new menuframework_s();
		static menuseparator_s s_download_title = new menuseparator_s();
		static menulist_s s_allow_download_box = new menulist_s();
		static menulist_s s_allow_download_maps_box = new menulist_s();
		static menulist_s s_allow_download_models_box = new menulist_s();
		static menulist_s s_allow_download_players_box = new menulist_s();
		static menulist_s s_allow_download_sounds_box = new menulist_s();
		static void DownloadCallback( Object self )
		{
			menulist_s f = ( menulist_s ) self;
			if ( f == s_allow_download_box )
			{
				Cvar.SetValue( "allow_download", f.curvalue );
			}
			else if ( f == s_allow_download_maps_box )
			{
				Cvar.SetValue( "allow_download_maps", f.curvalue );
			}
			else if ( f == s_allow_download_models_box )
			{
				Cvar.SetValue( "allow_download_models", f.curvalue );
			}
			else if ( f == s_allow_download_players_box )
			{
				Cvar.SetValue( "allow_download_players", f.curvalue );
			}
			else if ( f == s_allow_download_sounds_box )
			{
				Cvar.SetValue( "allow_download_sounds", f.curvalue );
			}
		}

		static String[] yes_no_names = new[] { "no", "yes" };
		static void DownloadOptions_MenuInit( )
		{
			var y = 0;
			s_downloadoptions_menu.x = ( Int32 ) ( viddef.GetWidth() * 0.5 );
			s_downloadoptions_menu.nitems = 0;
			s_download_title.type = MTYPE_SEPARATOR;
			s_download_title.name = "Download Options";
			s_download_title.x = 48;
			s_download_title.y = y;
			s_allow_download_box.type = MTYPE_SPINCONTROL;
			s_allow_download_box.x = 0;
			s_allow_download_box.y = y += 20;
			s_allow_download_box.name = "allow downloading";
			s_allow_download_box.callback = new Anonymousmcallback74();
			s_allow_download_box.itemnames = yes_no_names;
			s_allow_download_box.curvalue = ( Cvar.VariableValue( "allow_download" ) != 0 ) ? 1 : 0;
			s_allow_download_maps_box.type = MTYPE_SPINCONTROL;
			s_allow_download_maps_box.x = 0;
			s_allow_download_maps_box.y = y += 20;
			s_allow_download_maps_box.name = "maps";
			s_allow_download_maps_box.callback = new Anonymousmcallback75();
			s_allow_download_maps_box.itemnames = yes_no_names;
			s_allow_download_maps_box.curvalue = ( Cvar.VariableValue( "allow_download_maps" ) != 0 ) ? 1 : 0;
			s_allow_download_players_box.type = MTYPE_SPINCONTROL;
			s_allow_download_players_box.x = 0;
			s_allow_download_players_box.y = y += 10;
			s_allow_download_players_box.name = "player models/skins";
			s_allow_download_players_box.callback = new Anonymousmcallback76();
			s_allow_download_players_box.itemnames = yes_no_names;
			s_allow_download_players_box.curvalue = ( Cvar.VariableValue( "allow_download_players" ) != 0 ) ? 1 : 0;
			s_allow_download_models_box.type = MTYPE_SPINCONTROL;
			s_allow_download_models_box.x = 0;
			s_allow_download_models_box.y = y += 10;
			s_allow_download_models_box.name = "models";
			s_allow_download_models_box.callback = new Anonymousmcallback77();
			s_allow_download_models_box.itemnames = yes_no_names;
			s_allow_download_models_box.curvalue = ( Cvar.VariableValue( "allow_download_models" ) != 0 ) ? 1 : 0;
			s_allow_download_sounds_box.type = MTYPE_SPINCONTROL;
			s_allow_download_sounds_box.x = 0;
			s_allow_download_sounds_box.y = y += 10;
			s_allow_download_sounds_box.name = "sounds";
			s_allow_download_sounds_box.callback = new Anonymousmcallback78();
			s_allow_download_sounds_box.itemnames = yes_no_names;
			s_allow_download_sounds_box.curvalue = ( Cvar.VariableValue( "allow_download_sounds" ) != 0 ) ? 1 : 0;
			Menu_AddItem( s_downloadoptions_menu, s_download_title );
			Menu_AddItem( s_downloadoptions_menu, s_allow_download_box );
			Menu_AddItem( s_downloadoptions_menu, s_allow_download_maps_box );
			Menu_AddItem( s_downloadoptions_menu, s_allow_download_players_box );
			Menu_AddItem( s_downloadoptions_menu, s_allow_download_models_box );
			Menu_AddItem( s_downloadoptions_menu, s_allow_download_sounds_box );
			Menu_Center( s_downloadoptions_menu );
			if ( s_downloadoptions_menu.cursor == 0 )
				s_downloadoptions_menu.cursor = 1;
		}

		private sealed class Anonymousmcallback74 : mcallback
		{
			public override void Execute( Object o )
			{
				DownloadCallback( o );
			}
		}

		private sealed class Anonymousmcallback75 : mcallback
		{
			public override void Execute( Object o )
			{
				DownloadCallback( o );
			}
		}

		private sealed class Anonymousmcallback76 : mcallback
		{
			public override void Execute( Object o )
			{
				DownloadCallback( o );
			}
		}

		private sealed class Anonymousmcallback77 : mcallback
		{
			public override void Execute( Object o )
			{
				DownloadCallback( o );
			}
		}

		private sealed class Anonymousmcallback78 : mcallback
		{
			public override void Execute( Object o )
			{
				DownloadCallback( o );
			}
		}

		static void DownloadOptions_MenuDraw( )
		{
			Menu_Draw( s_downloadoptions_menu );
		}

		static String DownloadOptions_MenuKey( Int32 key )
		{
			return Default_MenuKey( s_downloadoptions_menu, key );
		}

		static xcommand_t Menu_DownloadOptions = new Anonymousxcommand_t26();
		private sealed class Anonymousxcommand_t26 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_DownloadOptions_f();
			}
		}

		static void Menu_DownloadOptions_f( )
		{
			DownloadOptions_MenuInit();
			PushMenu( new Anonymousxcommand_t27(), new Anonymouskeyfunc_t14() );
		}

		private sealed class Anonymousxcommand_t27 : xcommand_t
		{
			public override void Execute( )
			{
				DownloadOptions_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t14 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return DownloadOptions_MenuKey( key );
			}
		}

		static void AddressBook_MenuInit( )
		{
			s_addressbook_menu.x = viddef.GetWidth() / 2 - 142;
			s_addressbook_menu.y = viddef.GetHeight() / 2 - 58;
			s_addressbook_menu.nitems = 0;
			for ( var i = 0; i < NUM_ADDRESSBOOK_ENTRIES; i++ )
			{
				cvar_t adr = Cvar.Get( "adr" + i, "", CVAR_ARCHIVE );
				s_addressbook_fields[i].type = MTYPE_FIELD;
				s_addressbook_fields[i].name = null;
				s_addressbook_fields[i].callback = null;
				s_addressbook_fields[i].x = 0;
				s_addressbook_fields[i].y = i * 18 + 0;
				s_addressbook_fields[i].localdata[0] = i;
				s_addressbook_fields[i].cursor = adr.string_renamed.Length;
				s_addressbook_fields[i].length = 60;
				s_addressbook_fields[i].visible_length = 30;
				s_addressbook_fields[i].buffer = new StringBuffer( adr.string_renamed );
				Menu_AddItem( s_addressbook_menu, s_addressbook_fields[i] );
			}
		}

		static keyfunc_t AddressBook_MenuKey = new Anonymouskeyfunc_t15();
		private sealed class Anonymouskeyfunc_t15 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return AddressBook_MenuKey_f( key );
			}
		}

		static String AddressBook_MenuKey_f( Int32 key )
		{
			if ( key == K_ESCAPE )
			{
				for ( var index = 0; index < NUM_ADDRESSBOOK_ENTRIES; index++ )
				{
					Cvar.Set( "adr" + index, s_addressbook_fields[index].buffer.ToString() );
				}
			}

			return Default_MenuKey( s_addressbook_menu, key );
		}

		static xcommand_t AddressBook_MenuDraw = new Anonymousxcommand_t28();
		private sealed class Anonymousxcommand_t28 : xcommand_t
		{
			public override void Execute( )
			{
				AddressBook_MenuDraw_f();
			}
		}

		static void AddressBook_MenuDraw_f( )
		{
			Banner( "m_banner_addressbook" );
			Menu_Draw( s_addressbook_menu );
		}

		static xcommand_t Menu_AddressBook = new Anonymousxcommand_t29();
		private sealed class Anonymousxcommand_t29 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_AddressBook_f();
			}
		}

		static void Menu_AddressBook_f( )
		{
			AddressBook_MenuInit();
			PushMenu( new Anonymousxcommand_t30(), new Anonymouskeyfunc_t16() );
		}

		private sealed class Anonymousxcommand_t30 : xcommand_t
		{
			public override void Execute( )
			{
				AddressBook_MenuDraw_f();
			}
		}

		private sealed class Anonymouskeyfunc_t16 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return AddressBook_MenuKey_f( key );
			}
		}

		static menuframework_s s_player_config_menu = new menuframework_s();
		static menufield_s s_player_name_field = new menufield_s();
		static menulist_s s_player_model_box = new menulist_s();
		static menulist_s s_player_skin_box = new menulist_s();
		static menulist_s s_player_handedness_box = new menulist_s();
		static menulist_s s_player_rate_box = new menulist_s();
		static menuseparator_s s_player_skin_title = new menuseparator_s();
		static menuseparator_s s_player_model_title = new menuseparator_s();
		static menuseparator_s s_player_hand_title = new menuseparator_s();
		static menuseparator_s s_player_rate_title = new menuseparator_s();
		static menuaction_s s_player_download_action = new menuaction_s();
		public class playermodelinfo_s
		{
			public Int32 nskins;
			public String[] skindisplaynames;
			public String displayname;
			public String directory;
		}

		static playermodelinfo_s[] s_pmi = new playermodelinfo_s[MAX_PLAYERMODELS];
		static String[] s_pmnames = new String[MAX_PLAYERMODELS];
		static Int32 s_numplayermodels;
		static Int32[] rate_tbl = new[] { 2500, 3200, 5000, 10000, 25000, 0 };
		static String[] rate_names = new[] { "28.8 Modem", "33.6 Modem", "Single ISDN", "Dual ISDN/Cable", "T1/LAN", "User defined" };
		static void DownloadOptionsFunc( Object self )
		{
			Menu_DownloadOptions_f();
		}

		static void HandednessCallback( Object unused )
		{
			Cvar.SetValue( "hand", s_player_handedness_box.curvalue );
		}

		static void RateCallback( Object unused )
		{
			if ( s_player_rate_box.curvalue != rate_tbl.Length - 1 )
				Cvar.SetValue( "rate", rate_tbl[s_player_rate_box.curvalue] );
		}

		static void ModelCallback( Object unused )
		{
			s_player_skin_box.itemnames = s_pmi[s_player_model_box.curvalue].skindisplaynames;
			s_player_skin_box.curvalue = 0;
		}

		static Boolean IconOfSkinExists( String skin, String[] pcxfiles, Int32 npcxfiles )
		{
			String scratch;
			scratch = skin;
			var pos = scratch.LastIndexOf( '.' );
			if ( pos != -1 )
				scratch = scratch.Substring( 0, pos ) + "_i.pcx";
			else
				scratch += "_i.pcx";
			for ( var i = 0; i < npcxfiles; i++ )
			{
				if ( pcxfiles[i].Equals( scratch ) )
					return true;
			}

			return false;
		}

		static Boolean PlayerConfig_ScanDirectories( )
		{
			String findname;
			String scratch;
			Int32 ndirs = 0, npms = 0;
			Int32 a, b, c;
			String[] dirnames;
			String path = null;
			Int32 i;
			s_numplayermodels = 0;
			do
			{
				path = FS.NextPath( path );
				findname = path + "/players/*.*";
				if ( ( dirnames = FS.ListFiles( findname, 0, SFF_SUBDIR ) ) != null )
				{
					ndirs = dirnames.Length;
					break;
				}
			}
			while ( path != null );
			if ( dirnames == null )
				return false;
			npms = ndirs;
			if ( npms > MAX_PLAYERMODELS )
				npms = MAX_PLAYERMODELS;
			for ( i = 0; i < npms; i++ )
			{
				Int32 k, s;
				String[] pcxnames;
				String[] skinnames;
				Int32 npcxfiles;
				var nskins = 0;
				if ( dirnames[i] == null )
					continue;
				scratch = dirnames[i];
				scratch += "/tris.md2";
				if ( CoreSys.FindFirst( scratch ) == null )
				{
					dirnames[i] = null;
					CoreSys.FindClose();
					continue;
				}

				CoreSys.FindClose();
				scratch = dirnames[i] + "/*.pcx";
				pcxnames = FS.ListFiles( scratch, 0, 0 );
				npcxfiles = pcxnames.Length;
				if ( pcxnames == null )
				{
					dirnames[i] = null;
					continue;
				}

				for ( k = 0; k < npcxfiles - 1; k++ )
				{
					if ( !pcxnames[k].EndsWith( "_i.pcx" ) )
					{
						if ( IconOfSkinExists( pcxnames[k], pcxnames, npcxfiles - 1 ) )
						{
							nskins++;
						}
					}
				}

				if ( nskins == 0 )
					continue;
				skinnames = new String[nskins + 1];
				for ( s = 0, k = 0; k < npcxfiles - 1; k++ )
				{
					if ( pcxnames[k].IndexOf( "_i.pcx" ) < 0 )
					{
						if ( IconOfSkinExists( pcxnames[k], pcxnames, npcxfiles - 1 ) )
						{
							a = pcxnames[k].LastIndexOf( '/' );
							b = pcxnames[k].LastIndexOf( '\\' );
							if ( a > b )
								c = a;
							else
								c = b;
							scratch = pcxnames[k].Substring( c + 1, pcxnames[k].Length );
							var pos = scratch.LastIndexOf( '.' );
							if ( pos != -1 )
								scratch = scratch.Substring( 0, pos );
							skinnames[s] = scratch;
							s++;
						}
					}
				}

				if ( s_pmi[s_numplayermodels] == null )
					s_pmi[s_numplayermodels] = new playermodelinfo_s();
				s_pmi[s_numplayermodels].nskins = nskins;
				s_pmi[s_numplayermodels].skindisplaynames = skinnames;
				a = dirnames[i].LastIndexOf( '/' );
				b = dirnames[i].LastIndexOf( '\\' );
				if ( a > b )
					c = a;
				else
					c = b;
				s_pmi[s_numplayermodels].displayname = dirnames[i].Substring( c + 1 );
				s_pmi[s_numplayermodels].directory = dirnames[i].Substring( c + 1 );
				s_numplayermodels++;
			}

			return true;
		}

		static Int32 Pmicmpfnc( Object _a, Object _b )
		{
			playermodelinfo_s a = ( playermodelinfo_s ) _a;
			playermodelinfo_s b = ( playermodelinfo_s ) _b;
			if ( a.directory.Equals( "male" ) )
				return -1;
			else if ( b.directory.Equals( "male" ) )
				return 1;
			if ( a.directory.Equals( "female" ) )
				return -1;
			else if ( b.directory.Equals( "female" ) )
				return 1;
			return a.directory.CompareTo( b.directory );
		}

		static String[] handedness = new[] { "right", "left", "center" };
		static Boolean PlayerConfig_MenuInit( )
		{
			String currentdirectory;
			String currentskin;
			var i = 0;
			var currentdirectoryindex = 0;
			var currentskinindex = 0;
			cvar_t hand = Cvar.Get( "hand", "0", CVAR_USERINFO | CVAR_ARCHIVE );
			PlayerConfig_ScanDirectories();
			if ( s_numplayermodels == 0 )
				return false;
			if ( hand.value < 0 || hand.value > 2 )
				Cvar.SetValue( "hand", 0 );
			currentdirectory = skin.string_renamed;
			if ( currentdirectory.LastIndexOf( '/' ) != -1 )
			{
				currentskin = Lib.RightFrom( currentdirectory, '/' );
				currentdirectory = Lib.LeftFrom( currentdirectory, '/' );
			}
			else if ( currentdirectory.LastIndexOf( '\\' ) != -1 )
			{
				currentskin = Lib.RightFrom( currentdirectory, '\\' );
				currentdirectory = Lib.LeftFrom( currentdirectory, '\\' );
			}
			else
			{
				currentdirectory = "male";
				currentskin = "grunt";
			}
			s_pmi = s_pmi.OrderBy( p => p, new AnonymousComparator() ).ToArray();

			s_pmnames = new String[MAX_PLAYERMODELS];
			for ( i = 0; i < s_numplayermodels; i++ )
			{
				s_pmnames[i] = s_pmi[i].displayname;
				if ( Lib.Q_stricmp( s_pmi[i].directory, currentdirectory ) == 0 )
				{
					Int32 j;
					currentdirectoryindex = i;
					for ( j = 0; j < s_pmi[i].nskins; j++ )
					{
						if ( Lib.Q_stricmp( s_pmi[i].skindisplaynames[j], currentskin ) == 0 )
						{
							currentskinindex = j;
							break;
						}
					}
				}
			}

			s_player_config_menu.x = viddef.GetWidth() / 2 - 95;
			s_player_config_menu.y = viddef.GetHeight() / 2 - 97;
			s_player_config_menu.nitems = 0;
			s_player_name_field.type = MTYPE_FIELD;
			s_player_name_field.name = "name";
			s_player_name_field.callback = null;
			s_player_name_field.x = 0;
			s_player_name_field.y = 0;
			s_player_name_field.length = 20;
			s_player_name_field.visible_length = 20;
			s_player_name_field.buffer = new StringBuffer( name.string_renamed );
			s_player_name_field.cursor = name.string_renamed.Length;
			s_player_model_title.type = MTYPE_SEPARATOR;
			s_player_model_title.name = "model";
			s_player_model_title.x = -8;
			s_player_model_title.y = 60;
			s_player_model_box.type = MTYPE_SPINCONTROL;
			s_player_model_box.x = -56;
			s_player_model_box.y = 70;
			s_player_model_box.callback = new Anonymousmcallback79();
			s_player_model_box.cursor_offset = -48;
			s_player_model_box.curvalue = currentdirectoryindex;
			s_player_model_box.itemnames = s_pmnames;
			s_player_skin_title.type = MTYPE_SEPARATOR;
			s_player_skin_title.name = "skin";
			s_player_skin_title.x = -16;
			s_player_skin_title.y = 84;
			s_player_skin_box.type = MTYPE_SPINCONTROL;
			s_player_skin_box.x = -56;
			s_player_skin_box.y = 94;
			s_player_skin_box.name = null;
			s_player_skin_box.callback = null;
			s_player_skin_box.cursor_offset = -48;
			s_player_skin_box.curvalue = currentskinindex;
			s_player_skin_box.itemnames = s_pmi[currentdirectoryindex].skindisplaynames;
			s_player_hand_title.type = MTYPE_SEPARATOR;
			s_player_hand_title.name = "handedness";
			s_player_hand_title.x = 32;
			s_player_hand_title.y = 108;
			s_player_handedness_box.type = MTYPE_SPINCONTROL;
			s_player_handedness_box.x = -56;
			s_player_handedness_box.y = 118;
			s_player_handedness_box.name = null;
			s_player_handedness_box.cursor_offset = -48;
			s_player_handedness_box.callback = new Anonymousmcallback80();
			s_player_handedness_box.curvalue = ( Int32 ) Cvar.VariableValue( "hand" );
			s_player_handedness_box.itemnames = handedness;
			for ( i = 0; i < rate_tbl.Length - 1; i++ )
				if ( Cvar.VariableValue( "rate" ) == rate_tbl[i] )
					break;
			s_player_rate_title.type = MTYPE_SEPARATOR;
			s_player_rate_title.name = "connect speed";
			s_player_rate_title.x = 56;
			s_player_rate_title.y = 156;
			s_player_rate_box.type = MTYPE_SPINCONTROL;
			s_player_rate_box.x = -56;
			s_player_rate_box.y = 166;
			s_player_rate_box.name = null;
			s_player_rate_box.cursor_offset = -48;
			s_player_rate_box.callback = new Anonymousmcallback81();
			s_player_rate_box.curvalue = i;
			s_player_rate_box.itemnames = rate_names;
			s_player_download_action.type = MTYPE_ACTION;
			s_player_download_action.name = "download options";
			s_player_download_action.flags = QMF_LEFT_JUSTIFY;
			s_player_download_action.x = -24;
			s_player_download_action.y = 186;
			s_player_download_action.statusbar = null;
			s_player_download_action.callback = new Anonymousmcallback82();
			Menu_AddItem( s_player_config_menu, s_player_name_field );
			Menu_AddItem( s_player_config_menu, s_player_model_title );
			Menu_AddItem( s_player_config_menu, s_player_model_box );
			if ( s_player_skin_box.itemnames != null )
			{
				Menu_AddItem( s_player_config_menu, s_player_skin_title );
				Menu_AddItem( s_player_config_menu, s_player_skin_box );
			}

			Menu_AddItem( s_player_config_menu, s_player_hand_title );
			Menu_AddItem( s_player_config_menu, s_player_handedness_box );
			Menu_AddItem( s_player_config_menu, s_player_rate_title );
			Menu_AddItem( s_player_config_menu, s_player_rate_box );
			Menu_AddItem( s_player_config_menu, s_player_download_action );
			return true;
		}

		private sealed class AnonymousComparator : IComparer<Object>
		{
			public Int32 Compare( Object o1, Object o2 )
			{
				return Pmicmpfnc( o1, o2 );
			}
		}

		private sealed class Anonymousmcallback79 : mcallback
		{
			public override void Execute( Object o )
			{
				ModelCallback( o );
			}
		}

		private sealed class Anonymousmcallback80 : mcallback
		{
			public override void Execute( Object o )
			{
				HandednessCallback( o );
			}
		}

		private sealed class Anonymousmcallback81 : mcallback
		{
			public override void Execute( Object o )
			{
				RateCallback( o );
			}
		}

		private sealed class Anonymousmcallback82 : mcallback
		{
			public override void Execute( Object o )
			{
				DownloadOptionsFunc( o );
			}
		}

		static Int32 yaw;
		private static readonly entity_t entity = new entity_t();
		static void PlayerConfig_MenuDraw( )
		{
			refdef_t refdef = new refdef_t();
			String scratch;
			refdef.x = viddef.GetWidth() / 2;
			refdef.y = viddef.GetHeight() / 2 - 72;
			refdef.width = 144;
			refdef.height = 168;
			refdef.fov_x = 40;
			refdef.fov_y = Math3D.CalcFov( refdef.fov_x, refdef.width, refdef.height );
			refdef.time = cls.realtime * 0.001F;
			if ( s_pmi[s_player_model_box.curvalue].skindisplaynames != null )
			{
				entity.Clear();
				scratch = "players/" + s_pmi[s_player_model_box.curvalue].directory + "/tris.md2";
				entity.model = re.RegisterModel( scratch );
				scratch = "players/" + s_pmi[s_player_model_box.curvalue].directory + "/" + s_pmi[s_player_model_box.curvalue].skindisplaynames[s_player_skin_box.curvalue] + ".pcx";
				entity.skin = re.RegisterSkin( scratch );
				entity.flags = RF_FULLBRIGHT;
				entity.origin[0] = 80;
				entity.origin[1] = 0;
				entity.origin[2] = 0;
				Math3D.VectorCopy( entity.origin, entity.oldorigin );
				entity.frame = 0;
				entity.oldframe = 0;
				entity.backlerp = 0F;
				entity.angles[1] = yaw++;
				if ( ++yaw > 360 )
					yaw -= 360;
				refdef.areabits = null;
				refdef.num_entities = 1;
				refdef.entities = new entity_t[] { entity };
				refdef.lightstyles = null;
				refdef.rdflags = RDF_NOWORLDMODEL;
				Menu_Draw( s_player_config_menu );
				DrawTextBox( ( Int32 ) ( ( refdef.x ) * ( 320F / viddef.GetWidth() ) - 8 ), ( Int32 ) ( ( viddef.GetHeight() / 2 ) * ( 240F / viddef.GetHeight() ) - 77 ), refdef.width / 8, refdef.height / 8 );
				refdef.height += 4;
				re.RenderFrame( refdef );
				scratch = "/players/" + s_pmi[s_player_model_box.curvalue].directory + "/" + s_pmi[s_player_model_box.curvalue].skindisplaynames[s_player_skin_box.curvalue] + "_i.pcx";
				re.DrawPic( s_player_config_menu.x - 40, refdef.y, scratch );
			}
		}

		static String PlayerConfig_MenuKey( Int32 key )
		{
			Int32 i;
			if ( key == K_ESCAPE )
			{
				String scratch;
				Cvar.Set( "name", s_player_name_field.buffer.ToString() );
				scratch = s_pmi[s_player_model_box.curvalue].directory + "/" + s_pmi[s_player_model_box.curvalue].skindisplaynames[s_player_skin_box.curvalue];
				Cvar.Set( "skin", scratch );
				for ( i = 0; i < s_numplayermodels; i++ )
				{
					Int32 j;
					for ( j = 0; j < s_pmi[i].nskins; j++ )
					{
						if ( s_pmi[i].skindisplaynames[j] != null )
							s_pmi[i].skindisplaynames[j] = null;
					}

					s_pmi[i].skindisplaynames = null;
					s_pmi[i].nskins = 0;
				}
			}

			return Default_MenuKey( s_player_config_menu, key );
		}

		static xcommand_t Menu_PlayerConfig = new Anonymousxcommand_t31();
		private sealed class Anonymousxcommand_t31 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_PlayerConfig_f();
			}
		}

		static void Menu_PlayerConfig_f( )
		{
			if ( !PlayerConfig_MenuInit() )
			{
				Menu_SetStatusBar( s_multiplayer_menu, "No valid player models found" );
				return;
			}

			Menu_SetStatusBar( s_multiplayer_menu, null );
			PushMenu( new Anonymousxcommand_t32(), new Anonymouskeyfunc_t17() );
		}

		private sealed class Anonymousxcommand_t32 : xcommand_t
		{
			public override void Execute( )
			{
				PlayerConfig_MenuDraw();
			}
		}

		private sealed class Anonymouskeyfunc_t17 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return PlayerConfig_MenuKey( key );
			}
		}

		static String Quit_Key( Int32 key )
		{
			switch ( key )

			{
				case K_ESCAPE:
				case 'n':
				case 'N':
					PopMenu();
					break;
				case 'Y':
				case 'y':
					cls.key_dest = key_console;
					CL.Quit_f.Execute();
					break;
				default:
					break;
			}

			return null;
		}

		static void Quit_Draw( )
		{
			Int32 w, h;
			re.DrawGetPicSize( out var d, "quit" );
			w = d.Width;
			h = d.Height;
			re.DrawPic( ( viddef.GetWidth() - w ) / 2, ( viddef.GetHeight() - h ) / 2, "quit" );
		}

		static xcommand_t Menu_Quit = new Anonymousxcommand_t33();
		private sealed class Anonymousxcommand_t33 : xcommand_t
		{
			public override void Execute( )
			{
				Menu_Quit_f();
			}
		}

		static void Menu_Quit_f( )
		{
			PushMenu( new Anonymousxcommand_t34(), new Anonymouskeyfunc_t18() );
		}

		private sealed class Anonymousxcommand_t34 : xcommand_t
		{
			public override void Execute( )
			{
				Quit_Draw();
			}
		}

		private sealed class Anonymouskeyfunc_t18 : keyfunc_t
		{
			public override String Execute( Int32 key )
			{
				return Quit_Key( key );
			}
		}

		public new static void Init( )
		{
			Cmd.AddCommand( "menu_main", Menu_Main );
			Cmd.AddCommand( "menu_game", Menu_Game );
			Cmd.AddCommand( "menu_loadgame", Menu_LoadGame );
			Cmd.AddCommand( "menu_savegame", Menu_SaveGame );
			Cmd.AddCommand( "menu_joinserver", Menu_JoinServer );
			Cmd.AddCommand( "menu_addressbook", Menu_AddressBook );
			Cmd.AddCommand( "menu_startserver", Menu_StartServer );
			Cmd.AddCommand( "menu_dmoptions", Menu_DMOptions );
			Cmd.AddCommand( "menu_playerconfig", Menu_PlayerConfig );
			Cmd.AddCommand( "menu_downloadoptions", Menu_DownloadOptions );
			Cmd.AddCommand( "menu_credits", Menu_Credits );
			Cmd.AddCommand( "menu_multiplayer", Menu_Multiplayer );
			Cmd.AddCommand( "menu_video", Menu_Video );
			Cmd.AddCommand( "menu_options", Menu_Options );
			Cmd.AddCommand( "menu_keys", Menu_Keys );
			Cmd.AddCommand( "menu_quit", Menu_Quit );
			for ( var i = 0; i < m_layers.Length; i++ )
			{
				m_layers[i] = new menulayer_t();
			}
		}

		public static void Draw( )
		{
			if ( cls.key_dest != key_menu )
				return;
			SCR.DirtyScreen();
			if ( cl.cinematictime > 0 )
				re.DrawFill( 0, 0, viddef.GetWidth(), viddef.GetHeight(), 0 );
			else
				re.DrawFadeScreen();
			m_drawfunc.Execute();
			if ( m_entersound )
			{
				S.StartLocalSound( menu_in_sound );
				m_entersound = false;
			}
		}

		public static void Keydown( Int32 key )
		{
			String s;
			if ( m_keyfunc != null )
				if ( ( s = m_keyfunc.Execute( key ) ) != null )
					S.StartLocalSound( s );
		}

		public static void Action_DoEnter( menuaction_s a )
		{
			if ( a.callback != null )
				a.callback.Execute( a );
		}

		public static void Action_Draw( menuaction_s a )
		{
			if ( ( a.flags & QMF_LEFT_JUSTIFY ) != 0 )
			{
				if ( ( a.flags & QMF_GRAYED ) != 0 )
					Menu_DrawStringDark( a.x + a.parent.x + LCOLUMN_OFFSET, a.y + a.parent.y, a.name );
				else
					Menu_DrawString( a.x + a.parent.x + LCOLUMN_OFFSET, a.y + a.parent.y, a.name );
			}
			else
			{
				if ( ( a.flags & QMF_GRAYED ) != 0 )
					Menu_DrawStringR2LDark( a.x + a.parent.x + LCOLUMN_OFFSET, a.y + a.parent.y, a.name );
				else
					Menu_DrawStringR2L( a.x + a.parent.x + LCOLUMN_OFFSET, a.y + a.parent.y, a.name );
			}

			if ( a.ownerdraw != null )
				a.ownerdraw.Execute( a );
		}

		public static Boolean Field_DoEnter( menufield_s f )
		{
			if ( f.callback != null )
			{
				f.callback.Execute( f );
				return true;
			}

			return false;
		}

		public static void Field_Draw( menufield_s f )
		{
			Int32 i;
			String tempbuffer;
			if ( f.name != null )
				Menu_DrawStringR2LDark( f.x + f.parent.x + LCOLUMN_OFFSET, f.y + f.parent.y, f.name );
			var s = f.buffer.ToString();
			tempbuffer = s.Substring( f.visible_offset, s.Length );
			re.DrawChar( f.x + f.parent.x + 16, f.y + f.parent.y - 4, 18 );
			re.DrawChar( f.x + f.parent.x + 16, f.y + f.parent.y + 4, 24 );
			re.DrawChar( f.x + f.parent.x + 24 + f.visible_length * 8, f.y + f.parent.y - 4, 20 );
			re.DrawChar( f.x + f.parent.x + 24 + f.visible_length * 8, f.y + f.parent.y + 4, 26 );
			for ( i = 0; i < f.visible_length; i++ )
			{
				re.DrawChar( f.x + f.parent.x + 24 + i * 8, f.y + f.parent.y - 4, 19 );
				re.DrawChar( f.x + f.parent.x + 24 + i * 8, f.y + f.parent.y + 4, 25 );
			}

			Menu_DrawString( f.x + f.parent.x + 24, f.y + f.parent.y, tempbuffer );
			if ( Menu_ItemAtCursor( f.parent ) == f )
			{
				Int32 offset;
				if ( f.visible_offset != 0 )
					offset = f.visible_length;
				else
					offset = f.cursor;
				if ( ( ( ( Int32 ) ( Timer.Milliseconds() / 250 ) ) & 1 ) != 0 )
				{
					re.DrawChar( f.x + f.parent.x + ( offset + 2 ) * 8 + 8, f.y + f.parent.y, 11 );
				}
				else
				{
					re.DrawChar( f.x + f.parent.x + ( offset + 2 ) * 8 + 8, f.y + f.parent.y, ' ' );
				}
			}
		}

		public static Boolean Field_Key( menufield_s f, Int32 k )
		{
			var key = ( Char ) k;
			switch ( ( Int32 ) key )

			{
				case K_KP_SLASH:
					key = '/';
					break;
				case K_KP_MINUS:
					key = '-';
					break;
				case K_KP_PLUS:
					key = '+';
					break;
				case K_KP_HOME:
					key = '7';
					break;
				case K_KP_UPARROW:
					key = '8';
					break;
				case K_KP_PGUP:
					key = '9';
					break;
				case K_KP_LEFTARROW:
					key = '4';
					break;
				case K_KP_5:
					key = '5';
					break;
				case K_KP_RIGHTARROW:
					key = '6';
					break;
				case K_KP_END:
					key = '1';
					break;
				case K_KP_DOWNARROW:
					key = '2';
					break;
				case K_KP_PGDN:
					key = '3';
					break;
				case K_KP_INS:
					key = '0';
					break;
				case K_KP_DEL:
					key = '.';
					break;
			}

			if ( key > 127 )
			{
				switch ( ( Int32 ) key )

				{
					case K_DEL:
					default:
						return false;
				}
			}

			if ( ( Char.ToUpper( key ) == 'V' && keydown[K_CTRL] ) || ( ( ( key == K_INS ) || ( key == K_KP_INS ) ) && keydown[K_SHIFT] ) )
			{
				String cbd;
				if ( ( cbd = CoreSys.GetClipboardData() ) != null )
				{
					String[] lines = cbd.Split( "\\r\\n" );
					if ( lines.Length > 0 && lines[0].Length != 0 )
					{
						f.buffer = new StringBuffer( lines[0] );
						f.cursor = f.buffer.Length;
						f.visible_offset = f.cursor - f.visible_length;
						if ( f.visible_offset < 0 )
							f.visible_offset = 0;
					}
				}

				return true;
			}

			switch ( ( Int32 ) key )

			{
				case K_KP_LEFTARROW:
				case K_LEFTARROW:
				case K_BACKSPACE:
					if ( f.cursor > 0 )
					{
						f.buffer.Delete( f.cursor - 1, 1 );
						f.cursor--;
						if ( f.visible_offset != 0 )
						{
							f.visible_offset--;
						}
					}

					break;
				case K_KP_DEL:
				case K_DEL:
					f.buffer.Delete( f.cursor, 1 );
					break;
				case K_KP_ENTER:
				case K_ENTER:
				case K_ESCAPE:
				case K_TAB:
					return false;
				case K_SPACE:
				default:
					if ( !Char.IsDigit( key ) && ( f.flags & QMF_NUMBERSONLY ) != 0 )
						return false;
					if ( f.cursor < f.length )
					{
						f.buffer.Append( key );
						f.cursor++;
						if ( f.cursor > f.visible_length )
						{
							f.visible_offset++;
						}
					}
					break;
			}

			return true;
		}

		public static void Menu_AddItem( menuframework_s menu, menucommon_s item )
		{
			if ( menu.nitems == 0 )
				menu.nslots = 0;
			if ( menu.nitems < MAXMENUITEMS )
			{
				menu.items[menu.nitems] = item;
				( ( menucommon_s ) menu.items[menu.nitems] ).parent = menu;
				menu.nitems++;
			}

			menu.nslots = Menu_TallySlots( menu );
		}

		public static void Menu_AdjustCursor( menuframework_s m, Int32 dir )
		{
			menucommon_s citem;
			if ( m.cursor >= 0 && m.cursor < m.nitems )
			{
				if ( ( citem = Menu_ItemAtCursor( m ) ) != null )
				{
					if ( citem.type != MTYPE_SEPARATOR )
						return;
				}
			}

			if ( dir == 1 )
			{
				while ( true )
				{
					citem = Menu_ItemAtCursor( m );
					if ( citem != null )
						if ( citem.type != MTYPE_SEPARATOR )
							break;
					m.cursor += dir;
					if ( m.cursor >= m.nitems )
						m.cursor = 0;
				}
			}
			else
			{
				while ( true )
				{
					citem = Menu_ItemAtCursor( m );
					if ( citem != null )
						if ( citem.type != MTYPE_SEPARATOR )
							break;
					m.cursor += dir;
					if ( m.cursor < 0 )
						m.cursor = m.nitems - 1;
				}
			}
		}

		public static void Menu_Center( menuframework_s menu )
		{
			Int32 height;
			height = ( ( menucommon_s ) menu.items[menu.nitems - 1] ).y;
			height += 10;
			menu.y = ( viddef.GetHeight() - height ) / 2;
		}

		public static void Menu_Draw( menuframework_s menu )
		{
			Int32 i;
			menucommon_s item;
			for ( i = 0; i < menu.nitems; i++ )
			{
				switch ( ( ( menucommon_s ) menu.items[i] ).type )
				{
					case MTYPE_FIELD:
						Field_Draw( ( menufield_s ) menu.items[i] );
						break;
					case MTYPE_SLIDER:
						Slider_Draw( ( menuslider_s ) menu.items[i] );
						break;
					case MTYPE_LIST:
						MenuList_Draw( ( menulist_s ) menu.items[i] );
						break;
					case MTYPE_SPINCONTROL:
						SpinControl_Draw( ( menulist_s ) menu.items[i] );
						break;
					case MTYPE_ACTION:
						Action_Draw( ( menuaction_s ) menu.items[i] );
						break;
					case MTYPE_SEPARATOR:
						Separator_Draw( ( menuseparator_s ) menu.items[i] );
						break;
				}
			}

			item = Menu_ItemAtCursor( menu );
			if ( item != null && item.cursordraw != null )
			{
				item.cursordraw.Execute( item );
			}
			else if ( menu.cursordraw != null )
			{
				menu.cursordraw.Execute( menu );
			}
			else if ( item != null && item.type != MTYPE_FIELD )
			{
				if ( ( item.flags & QMF_LEFT_JUSTIFY ) != 0 )
				{
					re.DrawChar( menu.x + item.x - 24 + item.cursor_offset, menu.y + item.y, 12 + ( ( Int32 ) ( Timer.Milliseconds() / 250 ) & 1 ) );
				}
				else
				{
					re.DrawChar( menu.x + item.cursor_offset, menu.y + item.y, 12 + ( ( Int32 ) ( Timer.Milliseconds() / 250 ) & 1 ) );
				}
			}

			if ( item != null )
			{
				if ( item.statusbarfunc != null )
					item.statusbarfunc.Execute( item );
				else if ( item.statusbar != null )
					Menu_DrawStatusBar( item.statusbar );
				else
					Menu_DrawStatusBar( menu.statusbar );
			}
			else
			{
				Menu_DrawStatusBar( menu.statusbar );
			}
		}

		public static void Menu_DrawStatusBar( String string_renamed )
		{
			if ( string_renamed != null )
			{
				var l = string_renamed.Length;
				var maxrow = viddef.GetHeight() / 8;
				var maxcol = viddef.GetWidth() / 8;
				var col = maxcol / 2 - l / 2;
				re.DrawFill( 0, viddef.GetHeight() - 8, viddef.GetWidth(), 8, 4 );
				Menu_DrawString( col * 8, viddef.GetHeight() - 8, string_renamed );
			}
			else
			{
				re.DrawFill( 0, viddef.GetHeight() - 8, viddef.GetWidth(), 8, 0 );
			}
		}

		public static void Menu_DrawString( Int32 x, Int32 y, String string_renamed )
		{
			Int32 i;
			for ( i = 0; i < string_renamed.Length; i++ )
			{
				re.DrawChar( ( x + i * 8 ), y, string_renamed[i] );
			}
		}

		public static void Menu_DrawStringDark( Int32 x, Int32 y, String string_renamed )
		{
			Int32 i;
			for ( i = 0; i < string_renamed.Length; i++ )
			{
				re.DrawChar( ( x + i * 8 ), y, string_renamed[i] + 128 );
			}
		}

		public static void Menu_DrawStringR2L( Int32 x, Int32 y, String string_renamed )
		{
			Int32 i;
			var l = string_renamed.Length;
			for ( i = 0; i < l; i++ )
			{
				re.DrawChar( ( x - i * 8 ), y, string_renamed[l - i - 1] );
			}
		}

		public static void Menu_DrawStringR2LDark( Int32 x, Int32 y, String string_renamed )
		{
			Int32 i;
			var l = string_renamed.Length;
			for ( i = 0; i < l; i++ )
			{
				re.DrawChar( ( x - i * 8 ), y, string_renamed[l - i - 1] + 128 );
			}
		}

		public static menucommon_s Menu_ItemAtCursor( menuframework_s m )
		{
			if ( m.cursor < 0 || m.cursor >= m.nitems )
				return null;
			return ( menucommon_s ) m.items[m.cursor];
		}

		public static Boolean Menu_SelectItem( menuframework_s s )
		{
			menucommon_s item = Menu_ItemAtCursor( s );
			if ( item != null )
			{
				switch ( item.type )

				{
					case MTYPE_FIELD:
						return Field_DoEnter( ( menufield_s ) item );
					case MTYPE_ACTION:
						Action_DoEnter( ( menuaction_s ) item );
						return true;
					case MTYPE_LIST:
						return false;
					case MTYPE_SPINCONTROL:
						return false;
				}
			}

			return false;
		}

		public static void Menu_SetStatusBar( menuframework_s m, String string_renamed )
		{
			m.statusbar = string_renamed;
		}

		public static void Menu_SlideItem( menuframework_s s, Int32 dir )
		{
			menucommon_s item = ( menucommon_s ) Menu_ItemAtCursor( s );
			if ( item != null )
			{
				switch ( item.type )

				{
					case MTYPE_SLIDER:
						Slider_DoSlide( ( menuslider_s ) item, dir );
						break;
					case MTYPE_SPINCONTROL:
						SpinControl_DoSlide( ( menulist_s ) item, dir );
						break;
				}
			}
		}

		public static Int32 Menu_TallySlots( menuframework_s menu )
		{
			Int32 i;
			var total = 0;
			for ( i = 0; i < menu.nitems; i++ )
			{
				if ( ( ( menucommon_s ) menu.items[i] ).type == MTYPE_LIST )
				{
					var nitems = 0;
					String[] n = ( ( menulist_s ) menu.items[i] ).itemnames;
					while ( n[nitems] != null )
						nitems++;
					total += nitems;
				}
				else
				{
					total++;
				}
			}

			return total;
		}

		public static void Menulist_DoEnter( menulist_s l )
		{
			Int32 start;
			start = l.y / 10 + 1;
			l.curvalue = l.parent.cursor - start;
			if ( l.callback != null )
				l.callback.Execute( l );
		}

		public static void MenuList_Draw( menulist_s l )
		{
			String[] n;
			var y = 0;
			Menu_DrawStringR2LDark( l.x + l.parent.x + LCOLUMN_OFFSET, l.y + l.parent.y, l.name );
			n = l.itemnames;
			re.DrawFill( l.x - 112 + l.parent.x, l.parent.y + l.y + l.curvalue * 10 + 10, 128, 10, 16 );
			var i = 0;
			while ( n[i] != null )
			{
				Menu_DrawStringR2LDark( l.x + l.parent.x + LCOLUMN_OFFSET, l.y + l.parent.y + y + 10, n[i] );
				i++;
				y += 10;
			}
		}

		public static void Separator_Draw( menuseparator_s s )
		{
			if ( s.name != null )
				Menu_DrawStringR2LDark( s.x + s.parent.x, s.y + s.parent.y, s.name );
		}

		public static void Slider_DoSlide( menuslider_s s, Int32 dir )
		{
			s.curvalue += dir;
			if ( s.curvalue > s.maxvalue )
				s.curvalue = s.maxvalue;
			else if ( s.curvalue < s.minvalue )
				s.curvalue = s.minvalue;
			if ( s.callback != null )
				s.callback.Execute( s );
		}

		public static readonly Int32 SLIDER_RANGE = 10;
		public static void Slider_Draw( menuslider_s s )
		{
			Int32 i;
			Menu_DrawStringR2LDark( s.x + s.parent.x + LCOLUMN_OFFSET, s.y + s.parent.y, s.name );
			s.range = ( s.curvalue - s.minvalue ) / ( Single ) ( s.maxvalue - s.minvalue );
			if ( s.range < 0 )
				s.range = 0;
			if ( s.range > 1 )
				s.range = 1;
			re.DrawChar( s.x + s.parent.x + RCOLUMN_OFFSET, s.y + s.parent.y, 128 );
			for ( i = 0; i < SLIDER_RANGE; i++ )
				re.DrawChar( RCOLUMN_OFFSET + s.x + i * 8 + s.parent.x + 8, s.y + s.parent.y, 129 );
			re.DrawChar( RCOLUMN_OFFSET + s.x + i * 8 + s.parent.x + 8, s.y + s.parent.y, 130 );
			re.DrawChar( ( Int32 ) ( 8 + RCOLUMN_OFFSET + s.parent.x + s.x + ( SLIDER_RANGE - 1 ) * 8 * s.range ), s.y + s.parent.y, 131 );
		}

		public static void SpinControl_DoEnter( menulist_s s )
		{
			s.curvalue++;
			if ( s.itemnames[s.curvalue] == null )
				s.curvalue = 0;
			if ( s.callback != null )
				s.callback.Execute( s );
		}

		public static void SpinControl_DoSlide( menulist_s s, Int32 dir )
		{
			s.curvalue += dir;
			if ( s.curvalue < 0 )
				s.curvalue = 0;
			else if ( s.curvalue >= s.itemnames.Length || s.itemnames[s.curvalue] == null )
				s.curvalue--;
			if ( s.callback != null )
				s.callback.Execute( s );
		}

		public static void SpinControl_Draw( menulist_s s )
		{
			if ( s.name != null )
			{
				Menu_DrawStringR2LDark( s.x + s.parent.x + LCOLUMN_OFFSET, s.y + s.parent.y, s.name );
			}

			if ( s.itemnames[s.curvalue].IndexOf( '\\' ) == -1 )
			{
				Menu_DrawString( RCOLUMN_OFFSET + s.x + s.parent.x, s.y + s.parent.y, s.itemnames[s.curvalue] );
			}
			else
			{
				String line1, line2;
				line1 = Lib.LeftFrom( s.itemnames[s.curvalue], '\\' );
				Menu_DrawString( RCOLUMN_OFFSET + s.x + s.parent.x, s.y + s.parent.y, line1 );
				line2 = Lib.RightFrom( s.itemnames[s.curvalue], '\\' );
				var pos = line2.IndexOf( '\\' );
				if ( pos != -1 )
					line2 = line2.Substring( 0, pos );
				Menu_DrawString( RCOLUMN_OFFSET + s.x + s.parent.x, s.y + s.parent.y + 10, line2 );
			}
		}
	}
}
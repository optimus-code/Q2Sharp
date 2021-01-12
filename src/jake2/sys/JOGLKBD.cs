using Jake2.Client;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace Jake2.Sys
{
	public sealed class JOGLKBD : KBD
	{
		public static InputListener listener = new InputListener();
		public static Cursor emptyCursor;
		public static GameWindow c = null;
		public static Int32 win_w2 = 0;
		public static Int32 win_h2 = 0;

		public override void Init( )
		{
		}

		public static void Init( GameWindow component )
		{
			c = component;
			HandleCreateAndConfigureNotify( component );
		}

		public override void Update( )
		{
			HandleEvents();
		}

		public override void Close( )
		{
		}

		private void HandleEvents( )
		{
			Int32 key;
			Jake2InputEvent event_renamed;
			while ( ( event_renamed = InputListener.NextEvent() ) != null )
			{
				switch ( event_renamed.type )
				{
					case InputEvent.KeyPress:
					case InputEvent.KeyRelease:
						Do_Key_Event( XLateKey( ( KeyboardKeyEventArgs ) event_renamed.ev ), event_renamed.type == InputEvent.KeyPress );
						break;
					case InputEvent.MotionNotify:
						if ( IN.mouse_active )
						{
							mx = ( Int32 ) ( ( ( MouseMoveEventArgs ) event_renamed.ev ).X - win_w2 ) * 2;
							my = ( Int32 ) ( ( ( MouseMoveEventArgs ) event_renamed.ev ).Y - win_h2 ) * 2;
						}
						else
						{
							mx = 0;
							my = 0;
						}

						break;
					case InputEvent.ButtonPress:
						key = MouseEventToKey( ( MouseButtonEventArgs ) event_renamed.ev );
						Do_Key_Event( key, true );
						break;
					case InputEvent.ButtonRelease:
						key = MouseEventToKey( ( MouseButtonEventArgs ) event_renamed.ev );
						Do_Key_Event( key, false );
						break;
					case InputEvent.WheelMoved:
						var dir = ( Int32 ) ( ( MouseWheelEventArgs ) event_renamed.ev ).OffsetY;
						if ( dir > 0 )
						{
							Do_Key_Event( Key.K_MWHEELDOWN, true );
							Do_Key_Event( Key.K_MWHEELDOWN, false );
						}
						else
						{
							Do_Key_Event( Key.K_MWHEELUP, true );
							Do_Key_Event( Key.K_MWHEELUP, false );
						}

						break;
					case InputEvent.CreateNotify:
					case InputEvent.ConfigureNotify:
						HandleCreateAndConfigureNotify( event_renamed.gameWindow );
						break;
				}
			}

			if ( mx != 0 || my != 0 )
			{
				c.MousePosition = new OpenTK.Mathematics.Vector2( win_x + win_w2, win_y + win_h2 );
			}
		}

		private static void HandleCreateAndConfigureNotify( GameWindow component )
		{
			if ( !Globals.appletMode )
			{
				win_x = 0;
				win_y = 0;
				win_w2 = component.ClientSize.X / 2;
				win_h2 = component.ClientSize.Y / 2;
				var left = 0;
				var top = 0;
				if ( component != null )
				{
					win_x += component.Location.X;
					win_y += component.Location.Y;
				}

				win_x += left;
				win_y += top;
				win_w2 -= left / 2;
				win_h2 -= top / 2;
			}
			else
			{
				win_x = 0;
				win_y = 0;
				win_w2 = component.ClientSize.X / 2;
				win_h2 = component.ClientSize.Y / 2;
				var p = component.Location;
				win_x = p.X;
				win_y = p.Y;
			}
		}

		private Int32 MouseEventToKey( MouseButtonEventArgs ev )
		{
			switch ( ev.Button )
			{
				case MouseButton.Button3:
					return Key.K_MOUSE2;
				case MouseButton.Button2:
					return Key.K_MOUSE3;
				default:
					return Key.K_MOUSE1;
			}
		}

		private static Int32 XLateKey( KeyboardKeyEventArgs ev )
		{
			var key = 0;
			var code = ev.Key;
			switch ( code )
			{
				case Keys.PageUp:
					key = Key.K_PGUP;
					break;
				case Keys.PageDown:
					key = Key.K_PGDN;
					break;
				case Keys.Home:
					key = Key.K_HOME;
					break;
				case Keys.End:
					key = Key.K_END;
					break;
				case Keys.KeyPad4:
					if ( !ev.Modifiers.HasFlag( KeyModifiers.NumLock ) )
						key = Key.K_KP_LEFTARROW;
					else
						key = '4';
					break;
				case Keys.Left:
					key = Key.K_LEFTARROW;
					break;
				case Keys.KeyPad6:
					if ( !ev.Modifiers.HasFlag( KeyModifiers.NumLock ) )
						key = Key.K_KP_RIGHTARROW;
					else
						key = '6';
					break;
				case Keys.Right:
					key = Key.K_RIGHTARROW;
					break;
				case Keys.KeyPad5:
					if ( !ev.Modifiers.HasFlag( KeyModifiers.NumLock ) )
						key = Key.K_KP_DOWNARROW;
					else
						key = '5';
					break;
				case Keys.Down:
					key = Key.K_DOWNARROW;
					break;
				case Keys.KeyPad8:
					if ( !ev.Modifiers.HasFlag( KeyModifiers.NumLock ) )
						key = Key.K_KP_UPARROW;
					else
						key = '8';
					break;
				case Keys.Up:
					key = Key.K_UPARROW;
					break;
				case Keys.Escape:
					key = Key.K_ESCAPE;
					break;
				case Keys.Enter:
					key = Key.K_ENTER;
					break;
				case Keys.Tab:
					key = Key.K_TAB;
					break;
				case Keys.F1:
					key = Key.K_F1;
					break;
				case Keys.F2:
					key = Key.K_F2;
					break;
				case Keys.F3:
					key = Key.K_F3;
					break;
				case Keys.F4:
					key = Key.K_F4;
					break;
				case Keys.F5:
					key = Key.K_F5;
					break;
				case Keys.F6:
					key = Key.K_F6;
					break;
				case Keys.F7:
					key = Key.K_F7;
					break;
				case Keys.F8:
					key = Key.K_F8;
					break;
				case Keys.F9:
					key = Key.K_F9;
					break;
				case Keys.F10:
					key = Key.K_F10;
					break;
				case Keys.F11:
					key = Key.K_F11;
					break;
				case Keys.F12:
					key = Key.K_F12;
					break;
				case Keys.Backspace:
					key = Key.K_BACKSPACE;
					break;
				case Keys.Delete:
					key = Key.K_DEL;
					break;
				case Keys.Pause:
					key = Key.K_PAUSE;
					break;
				case Keys.RightShift:
				case Keys.LeftShift:
					key = Key.K_SHIFT;
					break;
				case Keys.RightControl:
				case Keys.LeftControl:
					key = Key.K_CTRL;
					break;
				case Keys.LeftAlt:
				case Keys.RightAlt:
					key = Key.K_ALT;
					break;
				case Keys.Insert:
					key = Key.K_INS;
					break;
				case Keys.GraveAccent:
					key = '`';
					break;
				default:
					key = KeyToCharCode( ev.Key, ev.Modifiers.HasFlag( KeyModifiers.Shift ) || ev.Modifiers.HasFlag( KeyModifiers.CapsLock ) );
					//if (key >= 'A' && key <= 'Z')
					//    key = key - 'A' + 'a';
					break;
			}

			if ( key > 255 )
				key = 0;
			return key;
		}

		private static Int32 KeyToCharCode( Keys key, Boolean isCapsOn )
		{
			var result = -1;

			switch ( key )
			{
				case Keys.Space:
					result = ' ';
					break;
				case Keys.Apostrophe:
					result = '\'';
					break;
				case Keys.Comma:
					result = ',';
					break;
				case Keys.Minus:
					result = '-';
					break;
				case Keys.Period:
					result = '.';
					break;
				case Keys.Slash:
					result = '/';
					break;
				case Keys.D0:
					result = '0';
					break;
				case Keys.D1:
					result = '1';
					break;
				case Keys.D2:
					result = '2';
					break;
				case Keys.D3:
					result = '3';
					break;
				case Keys.D4:
					result = '4';
					break;
				case Keys.D5:
					result = '5';
					break;
				case Keys.D6:
					result = '6';
					break;
				case Keys.D7:
					result = '7';
					break;
				case Keys.D8:
					result = '8';
					break;
				case Keys.D9:
					result = '9';
					break;
				case Keys.Semicolon:
					result = ';';
					break;
				case Keys.Equal:
					result = '=';
					break;
				case Keys.A:
					result = isCapsOn ? 'A' : 'a';
					break;
				case Keys.B:
					result = isCapsOn ? 'B' : 'b';
					break;
				case Keys.C:
					result = isCapsOn ? 'C' : 'c';
					break;
				case Keys.D:
					result = isCapsOn ? 'D' : 'd';
					break;
				case Keys.E:
					result = isCapsOn ? 'E' : 'E';
					break;
				case Keys.F:
					result = isCapsOn ? 'F' : 'f';
					break;
				case Keys.G:
					result = isCapsOn ? 'G' : 'g';
					break;
				case Keys.H:
					result = isCapsOn ? 'H' : 'h';
					break;
				case Keys.I:
					result = isCapsOn ? 'I' : 'i';
					break;
				case Keys.J:
					result = isCapsOn ? 'J' : 'j';
					break;
				case Keys.K:
					result = isCapsOn ? 'K' : 'k';
					break;
				case Keys.L:
					result = isCapsOn ? 'L' : 'l';
					break;
				case Keys.M:
					result = isCapsOn ? 'M' : 'm';
					break;
				case Keys.N:
					result = isCapsOn ? 'N' : 'n';
					break;
				case Keys.O:
					result = isCapsOn ? 'O' : 'o';
					break;
				case Keys.P:
					result = isCapsOn ? 'P' : 'p';
					break;
				case Keys.Q:
					result = isCapsOn ? 'Q' : 'q';
					break;
				case Keys.R:
					result = isCapsOn ? 'R' : 'r';
					break;
				case Keys.S:
					result = isCapsOn ? 'S' : 's';
					break;
				case Keys.T:
					result = isCapsOn ? 'T' : 't';
					break;
				case Keys.U:
					result = isCapsOn ? 'U' : 'u';
					break;
				case Keys.V:
					result = isCapsOn ? 'V' : 'v';
					break;
				case Keys.X:
					result = isCapsOn ? 'X' : 'x';
					break;
				case Keys.Y:
					result = isCapsOn ? 'Y' : 'y';
					break;
				case Keys.Z:
					result = isCapsOn ? 'z' : 'z';
					break;
				case Keys.LeftBracket:
					result = isCapsOn ? '{' : '[';
					break;
				case Keys.RightBracket:
					result = isCapsOn ? '}' : ']';
					break;
			}

			return result;
		}

		public override void Do_Key_Event( Int32 key, Boolean down )
		{
			Key.Event( key, down, Timer.Milliseconds() );
		}

		public void CenterMouse( )
		{
			c.MousePosition = new OpenTK.Mathematics.Vector2( win_x + win_w2, win_y + win_h2 );
		}

		public override void InstallGrabs( )
		{
			//if (emptyCursor == null)
			//{
			//    ImageIcon emptyIcon = new ImageIcon(new byte[0]);
			//    emptyCursor = c.GetToolkit().CreateCustomCursor(emptyIcon.GetImage(), new Point(0, 0), "emptyCursor");
			//}

			//c.SetCursor(emptyCursor);
			c.Cursor = MouseCursor.Empty;
			CenterMouse();
		}

		public override void UninstallGrabs( )
		{
			c.Cursor = MouseCursor.Default;
		}
	}
}
using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Sys
{
    public class LWJGLKBD : KBD
    {
        private char[] lwjglKeycodeMap = null;
        private int[] pressed = null;
        private KeyboardState LastKeyboardState;

        public override void Init()
        {
            try
            {
            //    if (!Keyboard.IsCreated())
            //        Keyboard.Create();
            //    if (!Mouse.IsCreated())
            //        Mouse.Create();
                if (lwjglKeycodeMap == null)
                    lwjglKeycodeMap = new char[256];
                if (pressed == null)
                    pressed = new int[256];
                lastRepeat = Timer.Milliseconds();
            }
            catch (Exception e)
            {
            }
        }

        public override void Update()
        {
            HandleEvents();
        }

        public override void Close()
        {
            //Keyboard.Destroy();
            //Mouse.Destroy();
            lwjglKeycodeMap = null;
            pressed = null;
        }

        private void HandleEvents()
        {
            var keyboardState = JOGLKBD.c.KeyboardState;
           
            if ( keyboardState == null )
            {
                Cbuf.ExecuteText(Defines.EXEC_APPEND, "quit");
            }

            var snapshot = keyboardState.GetSnapshot();

            while ( LastKeyboardState != snapshot )
            {
                int key = Keyboard.GetEventKey();
                char ch = Keyboard.GetEventCharacter();
                bool down = Keyboard.GetEventKeyState();
                if (down)
                {
                    lwjglKeycodeMap[key] = ch;
                    pressed[key] = Globals.sys_frame_time;
                }
                else
                {
                    pressed[key] = 0;
                }

                Do_Key_Event(XLateKey(key, ch), down);

                LastKeyboardState = snapshot;
            }

            GenerateRepeats();
            if (IN.mouse_active)
            {
                mx = Mouse.GetDX() << 1;
                my = -Mouse.GetDY() << 1;
            }
            else
            {
                mx = 0;
                my = 0;
            }

            while (Mouse.Next())
            {
                int button = Mouse.GetEventButton();
                if (button >= 0)
                {
                    Do_Key_Event(Key.K_MOUSE1 + button, Mouse.GetEventButtonState());
                }
                else
                {
                    button = Mouse.GetEventDWheel();
                    if (button > 0)
                    {
                        Do_Key_Event(Key.K_MWHEELUP, true);
                        Do_Key_Event(Key.K_MWHEELUP, false);
                    }
                    else if (button < 0)
                    {
                        Do_Key_Event(Key.K_MWHEELDOWN, true);
                        Do_Key_Event(Key.K_MWHEELDOWN, false);
                    }
                }
            }
        }

        private static int lastRepeat;
        private void GenerateRepeats()
        {
            int time = Globals.sys_frame_time;
            if (time - lastRepeat > 50)
            {
                for (int i = 0; i < pressed.Length; i++)
                {
                    if (pressed[i] > 0 && time - pressed[i] > 500)
                        Do_Key_Event(XLateKey(i, lwjglKeycodeMap[i]), true);
                }

                lastRepeat = time;
            }
        }

        private int XLateKey(int code, int ch)
        {
            int key = 0;
            switch ( code )
            {
                case Keyboard.KEY_PRIOR:
                    key = Key.K_PGUP;
                    break;
                case Keyboard.KEY_NEXT:
                    key = Key.K_PGDN;
                    break;
                case Keyboard.KEY_HOME:
                    key = Key.K_HOME;
                    break;
                case Keyboard.KEY_END:
                    key = Key.K_END;
                    break;
                case Keyboard.KEY_LEFT:
                    key = Key.K_LEFTARROW;
                    break;
                case Keyboard.KEY_RIGHT:
                    key = Key.K_RIGHTARROW;
                    break;
                case Keyboard.KEY_DOWN:
                    key = Key.K_DOWNARROW;
                    break;
                case Keyboard.KEY_UP:
                    key = Key.K_UPARROW;
                    break;
                case Keyboard.KEY_ESCAPE:
                    key = Key.K_ESCAPE;
                    break;
                case Keyboard.KEY_RETURN:
                    key = Key.K_ENTER;
                    break;
                case Keyboard.KEY_TAB:
                    key = Key.K_TAB;
                    break;
                case Keyboard.KEY_F1:
                    key = Key.K_F1;
                    break;
                case Keyboard.KEY_F2:
                    key = Key.K_F2;
                    break;
                case Keyboard.KEY_F3:
                    key = Key.K_F3;
                    break;
                case Keyboard.KEY_F4:
                    key = Key.K_F4;
                    break;
                case Keyboard.KEY_F5:
                    key = Key.K_F5;
                    break;
                case Keyboard.KEY_F6:
                    key = Key.K_F6;
                    break;
                case Keyboard.KEY_F7:
                    key = Key.K_F7;
                    break;
                case Keyboard.KEY_F8:
                    key = Key.K_F8;
                    break;
                case Keyboard.KEY_F9:
                    key = Key.K_F9;
                    break;
                case Keyboard.KEY_F10:
                    key = Key.K_F10;
                    break;
                case Keyboard.KEY_F11:
                    key = Key.K_F11;
                    break;
                case Keyboard.KEY_F12:
                    key = Key.K_F12;
                    break;
                case Keyboard.KEY_BACK:
                    key = Key.K_BACKSPACE;
                    break;
                case Keyboard.KEY_DELETE:
                    key = Key.K_DEL;
                    break;
                case Keyboard.KEY_PAUSE:
                    key = Key.K_PAUSE;
                    break;
                case Keyboard.KEY_RSHIFT:
                case Keyboard.KEY_LSHIFT:
                    key = Key.K_SHIFT;
                    break;
                case Keyboard.KEY_RCONTROL:
                case Keyboard.KEY_LCONTROL:
                    key = Key.K_CTRL;
                    break;
                case Keyboard.KEY_LMENU:
                case Keyboard.KEY_RMENU:
                    key = Key.K_ALT;
                    break;
                case Keyboard.KEY_INSERT:
                    key = Key.K_INS;
                    break;
                case Keyboard.KEY_GRAVE:
                case Keyboard.KEY_CIRCUMFLEX:
                    key = '`';
                    break;
                default:
                    key = lwjglKeycodeMap[code];
                    if (key >= 'A' && key <= 'Z')
                        key = key - 'A' + 'a';
                    break;
            }

            if (key > 255)
                key = 0;
            return key;
        }

        public override void Do_Key_Event(int key, bool down)
        {
            Key.Event(key, down, Timer.Milliseconds());
        }

        public override void InstallGrabs()
        {
            Mouse.SetGrabbed(true);
        }

        public override void UninstallGrabs()
        {
            Mouse.SetGrabbed(false);
        }
    }
}
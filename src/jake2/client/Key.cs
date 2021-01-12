using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class Key : Globals
    {
        public const int K_ALT = 132;
        public const int K_CTRL = 133;
        public const int K_SHIFT = 134;
        public const int K_F1 = 135;
        public const int K_F2 = 136;
        public const int K_F3 = 137;
        public const int K_F4 = 138;
        public const int K_F5 = 139;
        public const int K_F6 = 140;
        public const int K_F7 = 141;
        public const int K_F8 = 142;
        public const int K_F9 = 143;
        public const int K_F10 = 144;
        public const int K_F11 = 145;
        public const int K_F12 = 146;
        public const int K_INS = 147;
        public const int K_DEL = 148;
        public const int K_PGDN = 149;
        public const int K_PGUP = 150;
        public const int K_HOME = 151;
        public const int K_END = 152;
        public const int K_KP_HOME = 160;
        public const int K_KP_UPARROW = 161;
        public const int K_KP_PGUP = 162;
        public const int K_KP_LEFTARROW = 163;
        public const int K_KP_5 = 164;
        public const int K_KP_RIGHTARROW = 165;
        public const int K_KP_END = 166;
        public const int K_KP_DOWNARROW = 167;
        public const int K_KP_PGDN = 168;
        public const int K_KP_ENTER = 169;
        public const int K_KP_INS = 170;
        public const int K_KP_DEL = 171;
        public const int K_KP_SLASH = 172;
        public const int K_KP_MINUS = 173;
        public const int K_KP_PLUS = 174;
        public const int K_PAUSE = 255;
        public const int K_MOUSE1 = 200;
        public const int K_MOUSE2 = 201;
        public const int K_MOUSE3 = 202;
        public const int K_JOY1 = 203;
        public const int K_JOY2 = 204;
        public const int K_JOY3 = 205;
        public const int K_JOY4 = 206;
        public const int K_MWHEELDOWN = 239;
        public const int K_MWHEELUP = 240;
        public static int anykeydown = 0;
        public static int key_waiting;
        public static int history_line = 0;
        public static bool shift_down = false;
        public static int[] key_repeats = new int[256];
        public static bool[] menubound = new bool[256];
        public static bool[] consolekeys = new bool[256];
        public static String[] keynames = new string[256];
        static Key()
        {
            keynames[K_TAB] = "TAB";
            keynames[K_ENTER] = "ENTER";
            keynames[K_ESCAPE] = "ESCAPE";
            keynames[K_SPACE] = "SPACE";
            keynames[K_BACKSPACE] = "BACKSPACE";
            keynames[K_UPARROW] = "UPARROW";
            keynames[K_DOWNARROW] = "DOWNARROW";
            keynames[K_LEFTARROW] = "LEFTARROW";
            keynames[K_RIGHTARROW] = "RIGHTARROW";
            keynames[K_ALT] = "ALT";
            keynames[K_CTRL] = "CTRL";
            keynames[K_SHIFT] = "SHIFT";
            keynames[K_F1] = "F1";
            keynames[K_F2] = "F2";
            keynames[K_F3] = "F3";
            keynames[K_F4] = "F4";
            keynames[K_F5] = "F5";
            keynames[K_F6] = "F6";
            keynames[K_F7] = "F7";
            keynames[K_F8] = "F8";
            keynames[K_F9] = "F9";
            keynames[K_F10] = "F10";
            keynames[K_F11] = "F11";
            keynames[K_F12] = "F12";
            keynames[K_INS] = "INS";
            keynames[K_DEL] = "DEL";
            keynames[K_PGDN] = "PGDN";
            keynames[K_PGUP] = "PGUP";
            keynames[K_HOME] = "HOME";
            keynames[K_END] = "END";
            keynames[K_MOUSE1] = "MOUSE1";
            keynames[K_MOUSE2] = "MOUSE2";
            keynames[K_MOUSE3] = "MOUSE3";
            keynames[K_KP_HOME] = "KP_HOME";
            keynames[K_KP_UPARROW] = "KP_UPARROW";
            keynames[K_KP_PGUP] = "KP_PGUP";
            keynames[K_KP_LEFTARROW] = "KP_LEFTARROW";
            keynames[K_KP_5] = "KP_5";
            keynames[K_KP_RIGHTARROW] = "KP_RIGHTARROW";
            keynames[K_KP_END] = "KP_END";
            keynames[K_KP_DOWNARROW] = "KP_DOWNARROW";
            keynames[K_KP_PGDN] = "KP_PGDN";
            keynames[K_KP_ENTER] = "KP_ENTER";
            keynames[K_KP_INS] = "KP_INS";
            keynames[K_KP_DEL] = "KP_DEL";
            keynames[K_KP_SLASH] = "KP_SLASH";
            keynames[K_KP_PLUS] = "KP_PLUS";
            keynames[K_KP_MINUS] = "KP_MINUS";
            keynames[K_MWHEELUP] = "MWHEELUP";
            keynames[K_MWHEELDOWN] = "MWHEELDOWN";
            keynames[K_PAUSE] = "PAUSE";
            keynames[';'] = "SEMICOLON";
            keynames[0] = "NULL";
        }

        public static void Init()
        {
            for (int i = 0; i < 32; i++)
            {
                Globals.key_lines[i][0] = (byte)']';
                Globals.key_lines[i][1] = 0;
            }

            Globals.key_linepos = 1;
            for (int i = 32; i < 128; i++)
                consolekeys[i] = true;
            consolekeys[K_ENTER] = true;
            consolekeys[K_KP_ENTER] = true;
            consolekeys[K_TAB] = true;
            consolekeys[K_LEFTARROW] = true;
            consolekeys[K_KP_LEFTARROW] = true;
            consolekeys[K_RIGHTARROW] = true;
            consolekeys[K_KP_RIGHTARROW] = true;
            consolekeys[K_UPARROW] = true;
            consolekeys[K_KP_UPARROW] = true;
            consolekeys[K_DOWNARROW] = true;
            consolekeys[K_KP_DOWNARROW] = true;
            consolekeys[K_BACKSPACE] = true;
            consolekeys[K_HOME] = true;
            consolekeys[K_KP_HOME] = true;
            consolekeys[K_END] = true;
            consolekeys[K_KP_END] = true;
            consolekeys[K_PGUP] = true;
            consolekeys[K_KP_PGUP] = true;
            consolekeys[K_PGDN] = true;
            consolekeys[K_KP_PGDN] = true;
            consolekeys[K_SHIFT] = true;
            consolekeys[K_INS] = true;
            consolekeys[K_KP_INS] = true;
            consolekeys[K_KP_DEL] = true;
            consolekeys[K_KP_SLASH] = true;
            consolekeys[K_KP_PLUS] = true;
            consolekeys[K_KP_MINUS] = true;
            consolekeys[K_KP_5] = true;
            consolekeys['`'] = false;
            consolekeys['~'] = false;
            menubound[K_ESCAPE] = true;
            for (int i = 0; i < 12; i++)
                menubound[K_F1 + i] = true;
            Cmd.AddCommand("bind", Key.Bind_f);
            Cmd.AddCommand("unbind", Key.Unbind_f);
            Cmd.AddCommand("unbindall", Key.Unbindall_f);
            Cmd.AddCommand("bindlist", Key.Bindlist_f);
        }

        public static void ClearTyping()
        {
            Globals.key_lines[Globals.edit_line][1] = 0;
            Globals.key_linepos = 1;
        }

        public static void Event(int key, bool down, int time)
        {
            string kb;
            string cmd;
            if (key_waiting == -1)
            {
                if (down)
                    key_waiting = key;
                return;
            }

            if (down)
            {
                key_repeats[key]++;
                if (key_repeats[key] > 1 && Globals.cls.key_dest == Defines.key_game && !(Globals.cls.state == Defines.ca_disconnected))
                    return;
                if (key >= 200 && Globals.keybindings[key] == null)
                    Com.Printf(Key.KeynumToString(key) + " is unbound, hit F4 to set.\\n");
            }
            else
            {
                key_repeats[key] = 0;
            }

            if (key == K_SHIFT)
                shift_down = down;
            if (key == '`' || key == '~')
            {
                if (!down)
                    return;
                Con.ToggleConsole_f.Execute();
                return;
            }

            if (Globals.cl.attractloop && Globals.cls.key_dest != Defines.key_menu && !(key >= K_F1 && key <= K_F12))
                key = K_ESCAPE;
            if (key == K_ESCAPE)
            {
                if (!down)
                    return;
                if (Globals.cl.frame.playerstate.stats[Defines.STAT_LAYOUTS] != 0 && Globals.cls.key_dest == Defines.key_game)
                {
                    Cbuf.AddText("cmd putaway\\n");
                    return;
                }

                switch (Globals.cls.key_dest)

                {
                    case Defines.key_message:
                        Key.Message(key);
                        break;
                    case Defines.key_menu:
                        Menu.Keydown(key);
                        break;
                    case Defines.key_game:
                    case Defines.key_console:
                        Menu.Menu_Main_f();
                        break;
                    default:
                        Com.Error(Defines.ERR_FATAL, "Bad cls.key_dest");
                        break;
                }

                return;
            }

            Globals.keydown[key] = down;
            if (down)
            {
                if (key_repeats[key] == 1)
                    Key.anykeydown++;
            }
            else
            {
                Key.anykeydown--;
                if (Key.anykeydown < 0)
                    Key.anykeydown = 0;
            }

            if (!down)
            {
                kb = Globals.keybindings[key];
                if (kb != null && kb.Length > 0 && kb[0] == '+')
                {
                    cmd = "-" + kb.Substring(1) + " " + key + " " + time + "\\n";
                    Cbuf.AddText(cmd);
                }

                return;
            }

            if ((Globals.cls.key_dest == Defines.key_menu && menubound[key]) || (Globals.cls.key_dest == Defines.key_console && !consolekeys[key]) || (Globals.cls.key_dest == Defines.key_game && (Globals.cls.state == Defines.ca_active || !consolekeys[key])))
            {
                kb = Globals.keybindings[key];
                if (kb != null)
                {
                    if (kb.Length > 0 && kb[0] == '+')
                    {
                        cmd = kb + " " + key + " " + time + "\\n";
                        Cbuf.AddText(cmd);
                    }
                    else
                    {
                        Cbuf.AddText(kb + "\\n");
                    }
                }

                return;
            }

            if (!down)
                return;
            switch (Globals.cls.key_dest)

            {
                case Defines.key_message:
                    Key.Message(key);
                    break;
                case Defines.key_menu:
                    Menu.Keydown(key);
                    break;
                case Defines.key_game:
                case Defines.key_console:
                    Key.Console(key);
                    break;
                default:
                    Com.Error(Defines.ERR_FATAL, "Bad cls.key_dest");
                    break;
            }
        }

        public static string KeynumToString(int keynum)
        {
            if (keynum < 0 || keynum > 255)
                return "<KEY NOT FOUND>";
            if (keynum > 32 && keynum < 127)
                return ((char)keynum).ToString();
            if (keynames[keynum] != null)
                return keynames[keynum];
            return "<UNKNOWN KEYNUM>";
        }

        static int StringToKeynum(string str)
        {
            if (str == null)
                return -1;
            if (str.Length == 1)
                return str[0];
            for (int i = 0; i < keynames.Length; i++)
            {
                if (str.EqualsIgnoreCase(keynames[i]))
                    return i;
            }

            return -1;
        }

        public static void Message(int key)
        {
            if (key == K_ENTER || key == K_KP_ENTER)
            {
                if (Globals.chat_team)
                    Cbuf.AddText("say_team \\\"");
                else
                    Cbuf.AddText("say \\\"");
                Cbuf.AddText(Globals.chat_buffer);
                Cbuf.AddText("\\\"\\n");
                Globals.cls.key_dest = Defines.key_game;
                Globals.chat_buffer = "";
                return;
            }

            if (key == K_ESCAPE)
            {
                Globals.cls.key_dest = Defines.key_game;
                Globals.chat_buffer = "";
                return;
            }

            if (key < 32 || key > 127)
                return;
            if (key == K_BACKSPACE)
            {
                if (Globals.chat_buffer.Length > 2)
                {
                    Globals.chat_buffer = Globals.chat_buffer.Substring(0, Globals.chat_buffer.Length - 2);
                }
                else
                    Globals.chat_buffer = "";
                return;
            }

            if (Globals.chat_buffer.Length > Defines.MAXCMDLINE)
                return;
            Globals.chat_buffer += (char)key;
        }

        public static void Console(int key)
        {
            switch (key)

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

            if (key == 'l')
            {
                if (Globals.keydown[K_CTRL])
                {
                    Cbuf.AddText("clear\\n");
                    return;
                }
            }

            if (key == K_ENTER || key == K_KP_ENTER)
            {
                if (Globals.key_lines[Globals.edit_line][1] == '\\' || Globals.key_lines[Globals.edit_line][1] == '/')
                    Cbuf.AddText(Encoding.ASCII.GetString(Globals.key_lines[Globals.edit_line], 2, Lib.Strlen(Globals.key_lines[Globals.edit_line]) - 2));
                else
                    Cbuf.AddText(Encoding.ASCII.GetString(Globals.key_lines[Globals.edit_line], 1, Lib.Strlen(Globals.key_lines[Globals.edit_line]) - 1));
                Cbuf.AddText("\\n");
                Com.Printf(Encoding.ASCII.GetString(Globals.key_lines[Globals.edit_line], 0, Lib.Strlen(Globals.key_lines[Globals.edit_line])) + "\\n");
                Globals.edit_line = (Globals.edit_line + 1) & 31;
                history_line = Globals.edit_line;
                Globals.key_lines[Globals.edit_line][0] = (byte)']';
                Globals.key_linepos = 1;
                if (Globals.cls.state == Defines.ca_disconnected)
                    SCR.UpdateScreen();
                return;
            }

            if (key == K_TAB)
            {
                CompleteCommand();
                return;
            }

            if ((key == K_BACKSPACE) || (key == K_LEFTARROW) || (key == K_KP_LEFTARROW) || ((key == 'h') && (Globals.keydown[K_CTRL])))
            {
                if (Globals.key_linepos > 1)
                    Globals.key_linepos--;
                return;
            }

            if ((key == K_UPARROW) || (key == K_KP_UPARROW) || ((key == 'p') && Globals.keydown[K_CTRL]))
            {
                do
                {
                    history_line = (history_line - 1) & 31;
                }
                while (history_line != Globals.edit_line && Globals.key_lines[history_line][1] == 0);
                if (history_line == Globals.edit_line)
                    history_line = (Globals.edit_line + 1) & 31;
                System.Array.Copy(Globals.key_lines[history_line], 0, Globals.key_lines[Globals.edit_line], 0, Globals.key_lines[Globals.edit_line].Length);
                Globals.key_linepos = Lib.Strlen(Globals.key_lines[Globals.edit_line]);
                return;
            }

            if ((key == K_DOWNARROW) || (key == K_KP_DOWNARROW) || ((key == 'n') && Globals.keydown[K_CTRL]))
            {
                if (history_line == Globals.edit_line)
                    return;
                do
                {
                    history_line = (history_line + 1) & 31;
                }
                while (history_line != Globals.edit_line && Globals.key_lines[history_line][1] == 0);
                if (history_line == Globals.edit_line)
                {
                    Globals.key_lines[Globals.edit_line][0] = (byte)']';
                    Globals.key_linepos = 1;
                }
                else
                {
                    System.Array.Copy(Globals.key_lines[history_line], 0, Globals.key_lines[Globals.edit_line], 0, Globals.key_lines[Globals.edit_line].Length);
                    Globals.key_linepos = Lib.Strlen(Globals.key_lines[Globals.edit_line]);
                }

                return;
            }

            if (key == K_PGUP || key == K_KP_PGUP)
            {
                Globals.con.display -= 2;
                return;
            }

            if (key == K_PGDN || key == K_KP_PGDN)
            {
                Globals.con.display += 2;
                if (Globals.con.display > Globals.con.current)
                    Globals.con.display = Globals.con.current;
                return;
            }

            if (key == K_HOME || key == K_KP_HOME)
            {
                Globals.con.display = Globals.con.current - Globals.con.totallines + 10;
                return;
            }

            if (key == K_END || key == K_KP_END)
            {
                Globals.con.display = Globals.con.current;
                return;
            }

            if (key < 32 || key > 127)
                return;
            if (Globals.key_linepos < Defines.MAXCMDLINE - 1)
            {
                Globals.key_lines[Globals.edit_line][Globals.key_linepos] = (byte)key;
                Globals.key_linepos++;
                Globals.key_lines[Globals.edit_line][Globals.key_linepos] = 0;
            }
        }

        private static void PrintCompletions(string type, ArrayList compl)
        {
            Com.Printf(type);
            for (int i = 0; i < compl.Count; i++)
            {
                Com.Printf((string)compl[i] + " ");
            }

            Com.Printf("\\n");
        }

        static void CompleteCommand()
        {
            int start = 1;
            if (key_lines[edit_line][start] == '\\' || key_lines[edit_line][start] == '/')
                start++;
            int end = start;
            while (key_lines[edit_line][end] != 0)
                end++;
            string s = Encoding.ASCII.GetString(key_lines[edit_line], start, end - start);
            Vector cmds = Cmd.CompleteCommand(s);
            Vector vars = Cvar.CompleteVariable(s);
            int c = cmds.Size();
            int v = vars.Size();
            if ((c + v) > 1)
            {
                if (c > 0)
                    PrintCompletions("\\nCommands:\\n", cmds);
                if (v > 0)
                    PrintCompletions("\\nVariables:\\n", vars);
                return;
            }
            else if (c == 1)
            {
                s = (string)cmds.Get(0);
            }
            else if (v == 1)
            {
                s = (string)vars.Get(0);
            }
            else
                return;
            key_lines[edit_line][1] = (byte)'/';
            byte[] bytes = Lib.StringToBytes(s);
            System.Array.Copy(bytes, 0, key_lines[edit_line], 2, bytes.Length);
            key_linepos = bytes.Length + 2;
            key_lines[edit_line][key_linepos++] = Convert.ToByte( ' ' );
            key_lines[edit_line][key_linepos] = 0;
            return;
        }

        public static xcommand_t Bind_f = new Anonymousxcommand_t();
        private sealed class Anonymousxcommand_t : xcommand_t
        {
            public override void Execute()
            {
                Key_Bind_f();
            }
        }

        static void Key_Bind_f()
        {
            int c = Cmd.Argc();
            if (c < 2)
            {
                Com.Printf("bind <key> [command] : attach a command to a key\\n");
                return;
            }

            int b = StringToKeynum(Cmd.Argv(1));
            if (b == -1)
            {
                Com.Printf("\\\"" + Cmd.Argv(1) + "\\\" isn't a valid key\\n");
                return;
            }

            if (c == 2)
            {
                if (Globals.keybindings[b] != null)
                    Com.Printf("\\\"" + Cmd.Argv(1) + "\\\" = \\\"" + Globals.keybindings[b] + "\\\"\\n");
                else
                    Com.Printf("\\\"" + Cmd.Argv(1) + "\\\" is not bound\\n");
                return;
            }

            string cmd = "";
            for (int i = 2; i < c; i++)
            {
                cmd += Cmd.Argv(i);
                if (i != (c - 1))
                    cmd += " ";
            }

            SetBinding(b, cmd);
        }

        public static void SetBinding(int keynum, string binding)
        {
            if (keynum == -1)
                return;
            Globals.keybindings[keynum] = null;
            Globals.keybindings[keynum] = binding;
        }

        static xcommand_t Unbind_f = new Anonymousxcommand_t1();
        private sealed class Anonymousxcommand_t1 : xcommand_t
        {
            public override void Execute()
            {
                Key_Unbind_f();
            }
        }

        static void Key_Unbind_f()
        {
            if (Cmd.Argc() != 2)
            {
                Com.Printf("unbind <key> : remove commands from a key\\n");
                return;
            }

            int b = Key.StringToKeynum(Cmd.Argv(1));
            if (b == -1)
            {
                Com.Printf("\\\"" + Cmd.Argv(1) + "\\\" isn't a valid key\\n");
                return;
            }

            Key.SetBinding(b, null);
        }

        static xcommand_t Unbindall_f = new Anonymousxcommand_t2();
        private sealed class Anonymousxcommand_t2 : xcommand_t
        {
            public override void Execute()
            {
                Key_Unbindall_f();
            }
        }

        static void Key_Unbindall_f()
        {
            for (int i = 0; i < 256; i++)
                Key.SetBinding(i, null);
        }

        static xcommand_t Bindlist_f = new Anonymousxcommand_t3();
        private sealed class Anonymousxcommand_t3 : xcommand_t
        {
            public override void Execute()
            {
                Key_Bindlist_f();
            }
        }

        static void Key_Bindlist_f()
        {
            for (int i = 0; i < 256; i++)
                if (Globals.keybindings[i] != null && Globals.keybindings[i].Length != 0)
                    Com.Printf(Key.KeynumToString(i) + " \\\"" + Globals.keybindings[i] + "\\\"\\n");
        }

        public static void ClearStates()
        {
            int i;
            Key.anykeydown = 0;
            for (i = 0; i < 256; i++)
            {
                if (keydown[i] || key_repeats[i] != 0)
                    Event(i, false, 0);
                keydown[i] = false;
                key_repeats[i] = 0;
            }
        }

        public static void WriteBindings(QuakeFile f)
        {
            for (int i = 0; i < 256; i++)
                if (keybindings[i] != null && keybindings[i].Length > 0)
                    try
                    {
                        f.Write("bind " + KeynumToString(i) + " \\\"" + keybindings[i] + "\\\"\\n");
                    }
                    catch (IOException e)
                    {
                    }
        }
    }
}
using Jake2.Qcommon;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public class CL_inv
    {
        public static void ParseInventory()
        {
            int i;
            for (i = 0; i < Defines.MAX_ITEMS; i++)
                Globals.cl.inventory[i] = MSG.ReadShort(Globals.net_message);
        }

        static void Inv_DrawString(int x, int y, string string_renamed)
        {
            for (int i = 0; i < string_renamed.Length; i++)
            {
                Globals.re.DrawChar(x, y, string_renamed[i]);
                x += 8;
            }
        }

        static string GetHighBitString(string s)
        {
            byte[] b = Lib.StringToBytes(s);
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)(b[i] | 128);
            }

            return Lib.BytesToString(b);
        }

        static readonly int DISPLAY_ITEMS = 17;
        public static void DrawInventory()
        {
            int i, j;
            int num, selected_num, item;
            int[] index = new int[Defines.MAX_ITEMS];
            string string_renamed;
            int x, y;
            string binding;
            string bind;
            int selected;
            int top;
            selected = Globals.cl.frame.playerstate.stats[Defines.STAT_SELECTED_ITEM];
            num = 0;
            selected_num = 0;
            for (i = 0; i < Defines.MAX_ITEMS; i++)
            {
                if (i == selected)
                    selected_num = num;
                if (Globals.cl.inventory[i] != 0)
                {
                    index[num] = i;
                    num++;
                }
            }

            top = selected_num - DISPLAY_ITEMS / 2;
            if (num - top < DISPLAY_ITEMS)
                top = num - DISPLAY_ITEMS;
            if (top < 0)
                top = 0;
            x = (Globals.viddef.GetWidth() - 256) / 2;
            y = (Globals.viddef.GetHeight() - 240) / 2;
            SCR.DirtyScreen();
            Globals.re.DrawPic(x, y + 8, "inventory");
            y += 24;
            x += 24;
            Inv_DrawString(x, y, "hotkey ### item");
            Inv_DrawString(x, y + 8, "------ --- ----");
            y += 16;
            for (i = top; i < num && i < top + DISPLAY_ITEMS; i++)
            {
                item = index[i];
                binding = "use " + Globals.cl.configstrings[Defines.CS_ITEMS + item];
                bind = "";
                for (j = 0; j < 256; j++)
                    if (Globals.keybindings[j] != null && Globals.keybindings[j].Equals(binding))
                    {
                        bind = Key.KeynumToString(j);
                        break;
                    }

                string_renamed = Com.Sprintf("%6s %3i %s", bind, Globals.cl.inventory[item], Globals.cl.configstrings[Defines.CS_ITEMS + item]);
                if (item != selected)
                    string_renamed = GetHighBitString(string_renamed);
                else
                {
                    if (((int)(Globals.cls.realtime * 10) & 1) != 0)
                        Globals.re.DrawChar(x - 8, y, 15);
                }

                Inv_DrawString(x, y, string_renamed);
                y += 8;
            }
        }
    }
}
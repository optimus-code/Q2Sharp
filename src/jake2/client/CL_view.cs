using J2N.Text;
using Q2Sharp.Qcommon;
using Q2Sharp.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class CL_view
    {
        public static int num_cl_weaponmodels;
        public static String[] cl_weaponmodels = new string[Defines.MAX_CLIENTWEAPONMODELS];
        public static void PrepRefresh()
        {
            string mapname;
            int i;
            string name;
            float rotate;
            float[] axis = new float[3];
            if ((i = Globals.cl.configstrings[Defines.CS_MODELS + 1].Length) == 0)
                return;
            SCR.AddDirtyPoint(0, 0);
            SCR.AddDirtyPoint(Globals.viddef.GetWidth() - 1, Globals.viddef.GetHeight() - 1);
            mapname = Globals.cl.configstrings[Defines.CS_MODELS + 1].Substring(5, i - 4);
            Com.Printf("Map: " + mapname + "\\r");
            SCR.UpdateScreen();
            Globals.re.BeginRegistration(mapname);
            Com.Printf("                                     \\r");
            Com.Printf("pics\\r");
            SCR.UpdateScreen();
            SCR.TouchPics();
            Com.Printf("                                     \\r");
            CL_tent.RegisterTEntModels();
            num_cl_weaponmodels = 1;
            cl_weaponmodels[0] = "weapon.md2";
            for (i = 1; i < Defines.MAX_MODELS && Globals.cl.configstrings[Defines.CS_MODELS + i].Length != 0; i++)
            {
                name = new string (Globals.cl.configstrings[Defines.CS_MODELS + i]);
                if (name.Length > 37)
                    name = name.Substring(0, 36);
                if (name[0] != '*')
                    Com.Printf(name + "\\r");
                SCR.UpdateScreen();
                CoreSys.SendKeyEvents();
                if (name[0] == '#')
                {
                    if (num_cl_weaponmodels < Defines.MAX_CLIENTWEAPONMODELS)
                    {
                        cl_weaponmodels[num_cl_weaponmodels] = Globals.cl.configstrings[Defines.CS_MODELS + i].Substring(1);
                        num_cl_weaponmodels++;
                    }
                }
                else
                {
                    Globals.cl.model_draw[i] = Globals.re.RegisterModel(Globals.cl.configstrings[Defines.CS_MODELS + i]);
                    if (name[0] == '*')
                        Globals.cl.model_clip[i] = CM.InlineModel(Globals.cl.configstrings[Defines.CS_MODELS + i]);
                    else
                        Globals.cl.model_clip[i] = null;
                }

                if (name[0] != '*')
                    Com.Printf("                                     \\r");
            }

            Com.Printf("images\\r");
            SCR.UpdateScreen();
            for (i = 1; i < Defines.MAX_IMAGES && Globals.cl.configstrings[Defines.CS_IMAGES + i].Length > 0; i++)
            {
                Globals.cl.image_precache[i] = Globals.re.RegisterPic(Globals.cl.configstrings[Defines.CS_IMAGES + i]);
                CoreSys.SendKeyEvents();
            }

            Com.Printf("                                     \\r");
            for (i = 0; i < Defines.MAX_CLIENTS; i++)
            {
                if (Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i].Length == 0)
                    continue;
                Com.Printf("client " + i + '\\');
                SCR.UpdateScreen();
                CoreSys.SendKeyEvents();
                CL_parse.ParseClientinfo(i);
                Com.Printf("                                     \\r");
            }

            CL_parse.LoadClientinfo(Globals.cl.baseclientinfo, "unnamed\\\\male/grunt");
            Com.Printf("sky\\r");
            SCR.UpdateScreen();
            rotate = float.Parse(Globals.cl.configstrings[Defines.CS_SKYROTATE]);
            StringTokenizer st = new StringTokenizer(Globals.cl.configstrings[Defines.CS_SKYAXIS]);
            st.MoveNext();
            axis[0] = float.Parse( st.Current);
            st.MoveNext();
            axis[1] = float.Parse( st.Current);
            st.MoveNext();
            axis[2] = float.Parse( st.Current);
            Globals.re.SetSky(Globals.cl.configstrings[Defines.CS_SKY], rotate, axis);
            Com.Printf("                                     \\r");
            Globals.re.EndRegistration();
            Con.ClearNotify();
            SCR.UpdateScreen();
            Globals.cl.refresh_prepped = true;
            Globals.cl.force_refdef = true;
        }

        public static void AddNetgraph()
        {
            int i;
            int in_renamed;
            int ping;
            if (SCR.scr_debuggraph.value == 0F || SCR.scr_timegraph.value == 0F)
                return;
            for (i = 0; i < Globals.cls.netchan.dropped; i++)
                SCR.DebugGraph(30, 0x40);
            for (i = 0; i < Globals.cl.surpressCount; i++)
                SCR.DebugGraph(30, 0xdf);
            in_renamed = Globals.cls.netchan.incoming_acknowledged & (Defines.CMD_BACKUP - 1);
            ping = (int)(Globals.cls.realtime - Globals.cl.cmd_time[in_renamed]);
            ping /= 30;
            if (ping > 30)
                ping = 30;
            SCR.DebugGraph(ping, 0xd0);
        }
    }
}
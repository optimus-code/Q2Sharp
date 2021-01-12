using J2N.Text;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Render;
using Q2Sharp.Sound;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using static Q2Sharp.Client.Menu;

namespace Q2Sharp.Client
{
    public class VID : Globals
    {
        static cvar_t vid_gamma;
        static cvar_t vid_ref;
        static cvar_t vid_xpos;
        static cvar_t vid_ypos;
        static cvar_t vid_width;
        static cvar_t vid_height;
        static cvar_t vid_fullscreen;
        static bool reflib_active = false;
        public static void Printf(int print_level, string fmt)
        {
            Printf(print_level, fmt, null);
        }

        public static void Printf(int print_level, string fmt, params object[] parameters)
        {
            if (print_level == Defines.PRINT_ALL)
				Qcommon.Com.Printf(fmt, parameters);
            else
                Com.DPrintf(fmt, parameters);
        }

        static void Restart_f()
        {
            vid_modes[11].width = (int)vid_width.value;
            vid_modes[11].height = (int)vid_height.value;
            vid_ref.modified = true;
        }

        static vidmode_t[] vid_modes = new[]{new vidmode_t("Mode 0: 320x240", 320, 240, 0), new vidmode_t("Mode 1: 400x300", 400, 300, 1), new vidmode_t("Mode 2: 512x384", 512, 384, 2), new vidmode_t("Mode 3: 640x480", 640, 480, 3), new vidmode_t("Mode 4: 800x600", 800, 600, 4), new vidmode_t("Mode 5: 960x720", 960, 720, 5), new vidmode_t("Mode 6: 1024x768", 1024, 768, 6), new vidmode_t("Mode 7: 1152x864", 1152, 864, 7), new vidmode_t("Mode 8: 1280x1024", 1280, 1024, 8), new vidmode_t("Mode 9: 1600x1200", 1600, 1200, 9), new vidmode_t("Mode 10: 2048x1536", 2048, 1536, 10), new vidmode_t("Mode 11: user", 640, 480, 11)};
        static vidmode_t[] fs_modes;
        public static bool GetModeInfo(out Size dim, int mode)
        {
            if (fs_modes == null)
                InitModeList();
            vidmode_t[] modes = vid_modes;
            if (vid_fullscreen.value != 0F)
                modes = fs_modes;

            dim = new Size();

            if (mode < 0 || mode >= modes.Length)
                return false;

            dim.Width = modes[mode].width;
            dim.Height = modes[mode].height;
            return true;
        }

        public static void NewWindow(int width, int height)
        {
            Globals.viddef.SetSize(width, height);
        }

        static void FreeReflib()
        {
            if (Globals.re != null)
            {
                Globals.re.GetKeyboardHandler().Close();
                IN.Shutdown();
            }

            Globals.re = null;
            reflib_active = false;
        }

        static bool LoadRefresh(string name, bool fast)
        {
            if (reflib_active)
            {
                Globals.re.GetKeyboardHandler().Close();
                IN.Shutdown();
                Globals.re.Shutdown();
                FreeReflib();
            }

            Com.Printf("------- Loading " + name + " -------\\n");
            bool found = false;
            String[] driverNames = Renderer.GetDriverNames();
            for (int i = 0; i < driverNames.Length; i++)
            {
                if (driverNames[i].Equals(name))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Com.Printf("LoadLibrary(\\\"" + name + "\\\") failed\\n");
                return false;
            }

            Com.Printf("LoadLibrary(\\\"" + name + "\\\")\\n");
            Globals.re = Renderer.GetDriver(name, fast);
            if (Globals.re == null)
            {
                Com.Error(Defines.ERR_FATAL, name + " can't load but registered");
            }

            if (Globals.re.ApiVersion() != Defines.API_VERSION)
            {
                FreeReflib();
                Com.Error(Defines.ERR_FATAL, name + " has incompatible api_version");
            }

            IN.Real_IN_Init();
            if (!Globals.re.Init((int)vid_xpos.value, (int)vid_ypos.value))
            {
                Globals.re.Shutdown();
                FreeReflib();
                return false;
            }

            Globals.re.GetKeyboardHandler().Init();
            Com.Printf("------------------------------------\\n");
            reflib_active = true;
            return true;
        }

        public static void CheckChanges()
        {
            Globals.viddef.Update();
            if (vid_ref.modified)
            {
                S.StopAllSounds();
            }

            while (vid_ref.modified)
            {
                vid_ref.modified = false;
                vid_fullscreen.modified = true;
                Globals.cl.refresh_prepped = false;
                Globals.cls.disable_screen = 1F;
                if (!LoadRefresh(vid_ref.string_renamed, true))
                {
                    string renderer;
                    if (vid_ref.string_renamed.Equals(Renderer.GetPreferedName()))
                    {
                        renderer = Renderer.GetDefaultName();
                    }
                    else
                    {
                        renderer = Renderer.GetPreferedName();
                    }

                    if (vid_ref.string_renamed.Equals(Renderer.GetDefaultName()))
                    {
                        renderer = vid_ref.string_renamed;
                        Com.Printf("Refresh failed\\n");
                        gl_mode = Cvar.Get("gl_mode", "0", 0);
                        if (gl_mode.value != 0F)
                        {
                            Com.Printf("Trying mode 0\\n");
                            Cvar.SetValue("gl_mode", 0);
                            if (!LoadRefresh(vid_ref.string_renamed, false))
                                Com.Error(Defines.ERR_FATAL, "Couldn't fall back to " + renderer + " refresh!");
                        }
                        else
                            Com.Error(Defines.ERR_FATAL, "Couldn't fall back to " + renderer + " refresh!");
                    }

                    Cvar.Set("vid_ref", renderer);
                    if (Globals.cls.key_dest != Defines.key_console)
                    {
                        try
                        {
                            Con.ToggleConsole_f.Execute();
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }

                Globals.cls.disable_screen = 0F;
            }
        }

        public static void Init()
        {
            vid_ref = Cvar.Get("vid_ref", Renderer.GetPreferedName(), CVAR_ARCHIVE);
            vid_xpos = Cvar.Get("vid_xpos", "3", CVAR_ARCHIVE);
            vid_ypos = Cvar.Get("vid_ypos", "22", CVAR_ARCHIVE);
            vid_width = Cvar.Get("vid_width", "640", CVAR_ARCHIVE);
            vid_height = Cvar.Get("vid_height", "480", CVAR_ARCHIVE);
            vid_fullscreen = Cvar.Get("vid_fullscreen", "0", CVAR_ARCHIVE);
            vid_gamma = Cvar.Get("vid_gamma", "1", CVAR_ARCHIVE);
            vid_modes[11].width = (int)vid_width.value;
            vid_modes[11].height = (int)vid_height.value;
            Cmd.AddCommand("vid_restart", new Anonymousxcommand_t());
            CheckChanges();
        }

        private sealed class Anonymousxcommand_t : xcommand_t
        {
            public override void Execute()
            {
                Restart_f();
            }
        }

        public static void Shutdown()
        {
            if (reflib_active)
            {
                Globals.re.GetKeyboardHandler().Close();
                IN.Shutdown();
                Globals.re.Shutdown();
                FreeReflib();
            }
        }

        static readonly int REF_OPENGL_JOGL = 0;
        static readonly int REF_OPENGL_FASTJOGL = 1;
        static readonly int REF_OPENGL_LWJGL = 2;
        static cvar_t gl_mode;
        static cvar_t gl_driver;
        static cvar_t gl_picmip;
        static cvar_t gl_ext_palettedtexture;
        static cvar_t gl_swapinterval;
        static Menu.menuframework_s s_opengl_menu = new menuframework_s();
        static Menu.menuframework_s s_current_menu;
        static Menu.menulist_s s_mode_list = new menulist_s();
        static Menu.menulist_s s_ref_list = new menulist_s();
        static Menu.menuslider_s s_tq_slider = new menuslider_s();
        static Menu.menuslider_s s_screensize_slider = new menuslider_s();
        static Menu.menuslider_s s_brightness_slider = new menuslider_s();
        static Menu.menulist_s s_fs_box = new menulist_s();
        static Menu.menulist_s s_stipple_box = new menulist_s();
        static Menu.menulist_s s_paletted_texture_box = new menulist_s();
        static Menu.menulist_s s_vsync_box = new menulist_s();
        static Menu.menulist_s s_windowed_mouse = new menulist_s();
        static Menu.menuaction_s s_apply_action = new menuaction_s();
        static Menu.menuaction_s s_defaults_action = new menuaction_s();
        static void DriverCallback(Object unused)
        {
            s_current_menu = s_opengl_menu;
        }

        static void ScreenSizeCallback(Object s)
        {
            Menu.menuslider_s slider = (Menu.menuslider_s)s;
            Cvar.SetValue("viewsize", slider.curvalue * 10);
        }

        static void BrightnessCallback(Object s)
        {
            Menu.menuslider_s slider = (Menu.menuslider_s)s;
            if (vid_ref.string_renamed.EqualsIgnoreCase("soft") || vid_ref.string_renamed.EqualsIgnoreCase("softx"))
            {
                float gamma = (0.8F - (slider.curvalue / 10F - 0.5F)) + 0.5F;
                Cvar.SetValue("vid_gamma", gamma);
            }
        }

        static void ResetDefaults(Object unused)
        {
            MenuInit();
        }

        static void ApplyChanges(Object unused)
        {
            float gamma = (0.4F - (s_brightness_slider.curvalue / 20F - 0.25F)) + 0.7F;
            float modulate = s_brightness_slider.curvalue * 0.2F;
            Cvar.SetValue("vid_gamma", gamma);
            Cvar.SetValue("gl_modulate", modulate);
            Cvar.SetValue("gl_picmip", 3 - s_tq_slider.curvalue);
            Cvar.SetValue("vid_fullscreen", s_fs_box.curvalue);
            Cvar.SetValue("gl_swapinterval", (int)s_vsync_box.curvalue);
            gl_swapinterval.modified = true;
            Cvar.SetValue("gl_ext_palettedtexture", s_paletted_texture_box.curvalue);
            Cvar.SetValue("gl_mode", s_mode_list.curvalue);
            Cvar.Set("vid_ref", drivers[s_ref_list.curvalue]);
            Cvar.Set("gl_driver", drivers[s_ref_list.curvalue]);
            if (gl_driver.modified)
                vid_ref.modified = true;
            Menu.ForceMenuOff();
        }

        static readonly String[] resolutions = new[]{"[320 240  ]", "[400 300  ]", "[512 384  ]", "[640 480  ]", "[800 600  ]", "[960 720  ]", "[1024 768 ]", "[1152 864 ]", "[1280 1024]", "[1600 1200]", "[2048 1536]", "user mode"};
        static String[] fs_resolutions;
        static int mode_x;
        static String[] refs;
        static String[] drivers;
        static readonly String[] yesno_names = new[]{"no", "yes"};
        static void InitModeList()
        {
            VideoMode[] modes = re.GetModeList();
            fs_resolutions = new string[modes.Length];
            fs_modes = new vidmode_t[modes.Length];
            for (int i = 0; i < modes.Length; i++)
            {
                VideoMode m = modes[i];
                StringBuffer sb = new StringBuffer(18);
                sb.Append('[');
                sb.Append(m.Width);
                sb.Append(' ');
                sb.Append(m.Height);
                while (sb.Length < 10)
                    sb.Append(' ');
                sb.Append(']');
                fs_resolutions[i] = sb.ToString();
                sb.Length = 0;
                sb.Append("Mode ");
                sb.Append(i);
                sb.Append(':');
                sb.Append(m.Width);
                sb.Append('x');
                sb.Append(m.Height);
                fs_modes[i] = new vidmode_t(sb.ToString(), m.Width, m.Height, i);
            }
        }

        private static void InitRefs()
        {
            drivers = Renderer.GetDriverNames();
            refs = new string[drivers.Length];
            StringBuffer sb = new StringBuffer();
            for (int i = 0; i < drivers.Length; i++)
            {
                sb.Length = 0;
                sb.Append("[OpenGL ").Append(drivers[i]);
                while (sb.Length < 16)
                    sb.Append(" ");
                sb.Append("]");
                refs[i] = sb.ToString();
            }
        }

        public static void MenuInit()
        {
            InitRefs();
            if (gl_driver == null)
                gl_driver = Cvar.Get("gl_driver", Renderer.GetPreferedName(), 0);
            if (gl_picmip == null)
                gl_picmip = Cvar.Get("gl_picmip", "0", 0);
            if (gl_mode == null)
                gl_mode = Cvar.Get("gl_mode", "3", 0);
            if (gl_ext_palettedtexture == null)
                gl_ext_palettedtexture = Cvar.Get("gl_ext_palettedtexture", "1", CVAR_ARCHIVE);
            if (gl_swapinterval == null)
                gl_swapinterval = Cvar.Get("gl_swapinterval", "0", CVAR_ARCHIVE);
            s_mode_list.curvalue = (int)gl_mode.value;
            if (vid_fullscreen.value != 0F)
            {
                s_mode_list.itemnames = fs_resolutions;
                if (s_mode_list.curvalue >= fs_resolutions.Length - 1)
                {
                    s_mode_list.curvalue = 0;
                }

                mode_x = fs_modes[s_mode_list.curvalue].width;
            }
            else
            {
                s_mode_list.itemnames = resolutions;
                if (s_mode_list.curvalue >= resolutions.Length - 1)
                {
                    s_mode_list.curvalue = 0;
                }

                mode_x = vid_modes[s_mode_list.curvalue].width;
            }

            if (SCR.scr_viewsize == null)
                SCR.scr_viewsize = Cvar.Get("viewsize", "100", CVAR_ARCHIVE);
            s_screensize_slider.curvalue = (int)(SCR.scr_viewsize.value / 10);
            for (int i = 0; i < drivers.Length; i++)
            {
                if (vid_ref.string_renamed.Equals(drivers[i]))
                {
                    s_ref_list.curvalue = i;
                }
            }

            s_opengl_menu.x = (int)(viddef.GetWidth() * 0.5F);
            s_opengl_menu.nitems = 0;
            s_ref_list.type = MTYPE_SPINCONTROL;
            s_ref_list.name = "driver";
            s_ref_list.x = 0;
            s_ref_list.y = 0;
            s_ref_list.callback = new Anonymousmcallback();
            s_ref_list.itemnames = refs;
            s_mode_list.type = MTYPE_SPINCONTROL;
            s_mode_list.name = "video mode";
            s_mode_list.x = 0;
            s_mode_list.y = 10;
            s_screensize_slider.type = MTYPE_SLIDER;
            s_screensize_slider.x = 0;
            s_screensize_slider.y = 20;
            s_screensize_slider.name = "screen size";
            s_screensize_slider.minvalue = 3;
            s_screensize_slider.maxvalue = 12;
            s_screensize_slider.callback = new Anonymousmcallback1();
            s_brightness_slider.type = MTYPE_SLIDER;
            s_brightness_slider.x = 0;
            s_brightness_slider.y = 30;
            s_brightness_slider.name = "brightness";
            s_brightness_slider.callback = new Anonymousmcallback2();
            s_brightness_slider.minvalue = 5;
            s_brightness_slider.maxvalue = 13;
            s_brightness_slider.curvalue = (1.3F - vid_gamma.value + 0.5F) * 10;
            s_fs_box.type = MTYPE_SPINCONTROL;
            s_fs_box.x = 0;
            s_fs_box.y = 40;
            s_fs_box.name = "fullscreen";
            s_fs_box.itemnames = yesno_names;
            s_fs_box.curvalue = (int)vid_fullscreen.value;
            s_fs_box.callback = new Anonymousmcallback3();
            s_tq_slider.type = MTYPE_SLIDER;
            s_tq_slider.x = 0;
            s_tq_slider.y = 60;
            s_tq_slider.name = "texture quality";
            s_tq_slider.minvalue = 0;
            s_tq_slider.maxvalue = 3;
            s_tq_slider.curvalue = 3 - gl_picmip.value;
            s_paletted_texture_box.type = MTYPE_SPINCONTROL;
            s_paletted_texture_box.x = 0;
            s_paletted_texture_box.y = 70;
            s_paletted_texture_box.name = "8-bit textures";
            s_paletted_texture_box.itemnames = yesno_names;
            s_paletted_texture_box.curvalue = (int)gl_ext_palettedtexture.value;
            s_vsync_box.type = MTYPE_SPINCONTROL;
            s_vsync_box.x = 0;
            s_vsync_box.y = 80;
            s_vsync_box.name = "sync every frame";
            s_vsync_box.itemnames = yesno_names;
            s_vsync_box.curvalue = (int)gl_swapinterval.value;
            s_defaults_action.type = MTYPE_ACTION;
            s_defaults_action.name = "reset to default";
            s_defaults_action.x = 0;
            s_defaults_action.y = 100;
            s_defaults_action.callback = new Anonymousmcallback4();
            s_apply_action.type = MTYPE_ACTION;
            s_apply_action.name = "apply";
            s_apply_action.x = 0;
            s_apply_action.y = 110;
            s_apply_action.callback = new Anonymousmcallback5();
            Menu.Menu_AddItem(s_opengl_menu, s_ref_list);
            Menu.Menu_AddItem(s_opengl_menu, s_mode_list);
            Menu.Menu_AddItem(s_opengl_menu, s_screensize_slider);
            Menu.Menu_AddItem(s_opengl_menu, s_brightness_slider);
            Menu.Menu_AddItem(s_opengl_menu, s_fs_box);
            Menu.Menu_AddItem(s_opengl_menu, s_tq_slider);
            Menu.Menu_AddItem(s_opengl_menu, s_paletted_texture_box);
            Menu.Menu_AddItem(s_opengl_menu, s_vsync_box);
            Menu.Menu_AddItem(s_opengl_menu, s_defaults_action);
            Menu.Menu_AddItem(s_opengl_menu, s_apply_action);
            Menu.Menu_Center(s_opengl_menu);
            s_opengl_menu.x -= 8;
        }

        private sealed class Anonymousmcallback : mcallback
        {
            public override void Execute(Object self)
            {
                DriverCallback(self);
            }
        }

        private sealed class Anonymousmcallback1 : mcallback
        {
            public override void Execute(Object self)
            {
                ScreenSizeCallback(self);
            }
        }

        private sealed class Anonymousmcallback2 : mcallback
        {
            public override void Execute(Object self)
            {
                BrightnessCallback(self);
            }
        }

        private sealed class Anonymousmcallback3 : mcallback
        {
            public override void Execute(Object o)
            {
                int fs = ((Menu.menulist_s)o).curvalue;
                if (fs == 0)
                {
                    s_mode_list.itemnames = resolutions;
                    int i = vid_modes.Length - 2;
                    while (i > 0 && vid_modes[i].width > mode_x)
                        i--;
                    s_mode_list.curvalue = i;
                }
                else
                {
                    s_mode_list.itemnames = fs_resolutions;
                    int i = fs_modes.Length - 1;
                    while (i > 0 && fs_modes[i].width > mode_x)
                        i--;
                    s_mode_list.curvalue = i;
                }
            }
        }

        private sealed class Anonymousmcallback4 : mcallback
        {
            public override void Execute(Object self)
            {
                ResetDefaults(self);
            }
        }

        private sealed class Anonymousmcallback5 : mcallback
        {
            public override void Execute(Object self)
            {
                ApplyChanges(self);
            }
        }

        public static void MenuDraw()
        {
            s_current_menu = s_opengl_menu;
            re.DrawGetPicSize(out var dim, "m_banner_video");
            re.DrawPic(viddef.GetWidth() / 2 - dim.Width / 2, viddef.GetHeight() / 2 - 110, "m_banner_video");
            Menu.Menu_AdjustCursor(s_current_menu, 1);
            Menu.Menu_Draw(s_current_menu);
        }

        public static string MenuKey(int key)
        {
            Menu.menuframework_s m = s_current_menu;
            string sound = "misc/menu1.wav";
            switch (key)

            {
                case K_ESCAPE:
                    Menu.PopMenu();
                    return null;
                case K_UPARROW:
                    m.cursor--;
                    Menu.Menu_AdjustCursor(m, -1);
                    break;
                case K_DOWNARROW:
                    m.cursor++;
                    Menu.Menu_AdjustCursor(m, 1);
                    break;
                case K_LEFTARROW:
                    Menu.Menu_SlideItem(m, -1);
                    break;
                case K_RIGHTARROW:
                    Menu.Menu_SlideItem(m, 1);
                    break;
                case K_ENTER:
                    Menu.Menu_SelectItem(m);
                    break;
            }

            return sound;
        }
    }
}
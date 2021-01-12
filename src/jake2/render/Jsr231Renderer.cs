using Q2Sharp.Client;
using Q2Sharp.Render.Opengl;
using Q2Sharp.Sys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    sealed class Jsr231Renderer : Jsr231Driver, Irefexport_t, IRef
    {
        public static readonly string DRIVER_NAME = "jsr231";
        private KBD kbd = new JOGLKBD();
        private IRenderAPI impl;
        static Jsr231Renderer()
        {
            Renderer.Register(new Jsr231Renderer());
        }

        private Jsr231Renderer()
        {
        }

        public override bool Init(int vid_xpos, int vid_ypos)
        {
            impl.SetGLDriver(this);
            if (!impl.R_Init(vid_xpos, vid_ypos))
                return false;
            Activate();
            return impl.R_Init2();
        }

        public override void Shutdown()
        {
            impl.R_Shutdown();
        }

        public void BeginRegistration(string map)
        {
            Activate();
            impl.R_BeginRegistration(map);
        }

        public model_t RegisterModel(string name)
        {
            Activate();
            return impl.R_RegisterModel(name);
        }

        public image_t RegisterSkin(string name)
        {
            Activate();
            return impl.R_RegisterSkin(name);
        }

        public image_t RegisterPic(string name)
        {
            Activate();
            return impl.Draw_FindPic(name);
        }

        public void SetSky(string name, float rotate, float[] axis)
        {
            Activate();
            impl.R_SetSky(name, rotate, axis);
        }

        public void EndRegistration()
        {
            Activate();
            impl.R_EndRegistration();
        }

        public void RenderFrame(refdef_t fd)
        {
            impl.R_RenderFrame(fd);
        }

        public void DrawGetPicSize(out Size dim, string name)
        {
            impl.Draw_GetPicSize(out dim, name);
        }

        public void DrawPic(int x, int y, string name)
        {
            impl.Draw_Pic(x, y, name);
        }

        public void DrawStretchPic(int x, int y, int w, int h, string name)
        {
            impl.Draw_StretchPic(x, y, w, h, name);
        }

        public void DrawChar(int x, int y, int num)
        {
            Activate();
            impl.Draw_Char(x, y, num);
        }

        public void DrawTileClear(int x, int y, int w, int h, string name)
        {
            impl.Draw_TileClear(x, y, w, h, name);
        }

        public void DrawFill(int x, int y, int w, int h, int c)
        {
            impl.Draw_Fill(x, y, w, h, c);
        }

        public void DrawFadeScreen()
        {
            impl.Draw_FadeScreen();
        }

        public void DrawStretchRaw(int x, int y, int w, int h, int cols, int rows, byte[] data)
        {
            impl.Draw_StretchRaw(x, y, w, h, cols, rows, data);
        }

        public void CinematicSetPalette(byte[] palette)
        {
            impl.R_SetPalette(palette);
        }

        public override void BeginFrame(float camera_separation)
        {
            impl.R_BeginFrame(camera_separation);
        }

        public override void EndFrame()
        {
            EndFrame();
        }

        public override void AppActivate(bool activate)
        {
            AppActivate(activate);
        }

        public override void Screenshot()
        {
            Activate();
            impl.GL_ScreenShot_f();
        }

        public int ApiVersion()
        {
            return Defines.API_VERSION;
        }

        public KBD GetKeyboardHandler()
        {
            return kbd;
        }

        public string GetName()
        {
            return DRIVER_NAME;
        }

        public override string ToString()
        {
            return DRIVER_NAME;
        }

        public Irefexport_t GetRefAPI(IRenderAPI renderer)
        {
            this.impl = renderer;
            return this;
        }
    }
}
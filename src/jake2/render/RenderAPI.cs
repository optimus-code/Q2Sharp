using Q2Sharp.Client;
using Q2Sharp.Render.Opengl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public interface IRenderAPI
    {
        void SetGLDriver(IGLDriver impl);
        bool R_Init(int vid_xpos, int vid_ypos);
        bool R_Init2();
        void R_Shutdown();
        void R_BeginRegistration(string map);
        model_t R_RegisterModel(string name);
        image_t R_RegisterSkin(string name);
        image_t Draw_FindPic(string name);
        void R_SetSky(string name, float rotate, float[] axis);
        void R_EndRegistration();
        void R_RenderFrame(refdef_t fd);
        void Draw_GetPicSize(out Size dim, string name);
        void Draw_Pic(int x, int y, string name);
        void Draw_StretchPic(int x, int y, int w, int h, string name);
        void Draw_Char(int x, int y, int num);
        void Draw_TileClear(int x, int y, int w, int h, string name);
        void Draw_Fill(int x, int y, int w, int h, int c);
        void Draw_FadeScreen();
        void Draw_StretchRaw(int x, int y, int w, int h, int cols, int rows, byte[] data);
        void R_SetPalette(byte[] palette);
        void R_BeginFrame(float camera_separation);
        void GL_ScreenShot_f();
    }
}
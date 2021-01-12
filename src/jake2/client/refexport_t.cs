using Jake2.Qcommon;
using Jake2.Render;
using Jake2.Sys;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public interface Irefexport_t
    {
        bool Init(int vid_xpos, int vid_ypos);
        void Shutdown();
        void BeginRegistration(string map);
        model_t RegisterModel(string name);
        image_t RegisterSkin(string name);
        image_t RegisterPic(string name);
        void SetSky(string name, float rotate, float[] axis);
        void EndRegistration();
        void RenderFrame(refdef_t fd);
        void DrawGetPicSize( out Size dim, string name);
        void DrawPic(int x, int y, string name);
        void DrawStretchPic(int x, int y, int w, int h, string name);
        void DrawChar(int x, int y, int num);
        void DrawTileClear(int x, int y, int w, int h, string name);
        void DrawFill(int x, int y, int w, int h, int c);
        void DrawFadeScreen();
        void DrawStretchRaw(int x, int y, int w, int h, int cols, int rows, byte[] data);
        void CinematicSetPalette(byte[] palette);
        void BeginFrame(float camera_separation);
        void EndFrame();
        void AppActivate(bool activate);
        void UpdateScreen(xcommand_t callback);
        int ApiVersion();
        VideoMode[] GetModeList();
        KBD GetKeyboardHandler();
    }
}
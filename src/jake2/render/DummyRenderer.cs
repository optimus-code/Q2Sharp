using Jake2.Client;
using Jake2.Qcommon;
using Jake2.Sys;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Jake2.Render
{
    public class DummyRenderer : Irefexport_t
    {
        public virtual bool Init(int vid_xpos, int vid_ypos)
        {
            return false;
        }

        public virtual void Shutdown()
        {
        }

        public virtual void BeginRegistration(string map)
        {
        }

        public virtual model_t RegisterModel(string name)
        {
            return null;
        }

        public virtual image_t RegisterSkin(string name)
        {
            return null;
        }

        public virtual image_t RegisterPic(string name)
        {
            return null;
        }

        public virtual void SetSky(string name, float rotate, float[] axis)
        {
        }

        public virtual void EndRegistration()
        {
        }

        public virtual void RenderFrame(refdef_t fd)
        {
        }

        public virtual void DrawGetPicSize( out Size dim, string name)
        {
            dim = default;
        }

        public virtual void DrawPic(int x, int y, string name)
        {
        }

        public virtual void DrawStretchPic(int x, int y, int w, int h, string name)
        {
        }

        public virtual void DrawChar(int x, int y, int num)
        {
        }

        public virtual void DrawTileClear(int x, int y, int w, int h, string name)
        {
        }

        public virtual void DrawFill(int x, int y, int w, int h, int c)
        {
        }

        public virtual void DrawFadeScreen()
        {
        }

        public virtual void DrawStretchRaw(int x, int y, int w, int h, int cols, int rows, byte[] data)
        {
        }

        public virtual void CinematicSetPalette(byte[] palette)
        {
        }

        public virtual void BeginFrame(float camera_separation)
        {
        }

        public virtual void EndFrame()
        {
        }

        public virtual void AppActivate(bool activate)
        {
        }

        public virtual void UpdateScreen(xcommand_t callback)
        {
            callback.Execute();
        }

        public virtual int ApiVersion()
        {
            return 0;
        }

        public virtual VideoMode[] GetModeList()
        {
            return null;
        }

        public virtual KBD GetKeyboardHandler()
        {
            return null;
        }
    }
}
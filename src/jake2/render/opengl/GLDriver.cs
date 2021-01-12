using Q2Sharp.Qcommon;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render.Opengl
{
    public interface IGLDriver
    {
        bool Init(int xpos, int ypos);
        int SetMode(Size dim, int mode, bool fullscreen);
        void Shutdown();
        void BeginFrame(float camera_separation);
        void EndFrame();
        void AppActivate(bool activate);
        void EnableLogging(bool enable);
        void LogNewFrame();
        VideoMode[] GetModeList();
        void UpdateScreen(xcommand_t callback);
        void Screenshot();
    }
}
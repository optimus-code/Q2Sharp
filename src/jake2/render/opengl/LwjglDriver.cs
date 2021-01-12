using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render.Opengl
{
    public abstract class LwjglDriver : LwjglGL, IGLDriver
    {
        public LwjglDriver()
        {
        }

        private VideoMode oldDisplayMode;
        int window_xpos, window_ypos;
        private java.awt.VideoMode ToAwtDisplayMode(VideoMode m)
        {
            return new VideoMode(m.GetWidth(), m.GetHeight(), m.GetBitsPerPixel(), m.GetFrequency());
        }

        public virtual VideoMode[] GetModeList()
        {
            VideoMode[] modes;
            try
            {
                modes = Display.GetAvailableDisplayModes();
            }
            catch (LWJGLException e)
            {
                Com.Println(e.GetMessage());
                return new java.awt.VideoMode[0];
            }

            LinkedList l = new LinkedList();
            l.Add(ToAwtDisplayMode(oldDisplayMode));
            for (int i = 0; i < modes.length; i++)
            {
                VideoMode m = modes[i];
                if (m.GetBitsPerPixel() != oldDisplayMode.GetBitsPerPixel())
                    continue;
                if (m.GetFrequency() > oldDisplayMode.GetFrequency())
                    continue;
                if (m.GetHeight() < 240 || m.GetWidth() < 320)
                    continue;
                int j = 0;
                java.awt.VideoMode ml = null;
                for (j = 0; j < l.Size(); j++)
                {
                    ml = (java.awt.VideoMode)l.Get(j);
                    if (ml.GetWidth() > m.GetWidth())
                        break;
                    if (ml.GetWidth() == m.GetWidth() && ml.GetHeight() >= m.GetHeight())
                        break;
                }

                if (j == l.Size())
                {
                    l.AddLast(ToAwtDisplayMode(m));
                }
                else if (ml.GetWidth() > m.GetWidth() || ml.GetHeight() > m.GetHeight())
                {
                    l.Add(j, ToAwtDisplayMode(m));
                }
                else if (m.GetFrequency() > ml.GetRefreshRate())
                {
                    l.Remove(j);
                    l.Add(j, ToAwtDisplayMode(m));
                }
            }

            java.awt.VideoMode[] ma = new java.awt.VideoMode[l.Size()];
            l.ToArray(ma);
            return ma;
        }

        public virtual VideoMode[] GetLWJGLModeList()
        {
            VideoMode[] modes;
            try
            {
                modes = Display.GetAvailableDisplayModes();
            }
            catch (LWJGLException e)
            {
                Com.Println(e.GetMessage());
                return new VideoMode[0];
            }

            LinkedList l = new LinkedList();
            l.Add(oldDisplayMode);
            for (int i = 0; i < modes.length; i++)
            {
                VideoMode m = modes[i];
                if (m.GetBitsPerPixel() != oldDisplayMode.GetBitsPerPixel())
                    continue;
                if (m.GetFrequency() > Math.Max(60, oldDisplayMode.GetFrequency()))
                    continue;
                if (m.GetHeight() < 240 || m.GetWidth() < 320)
                    continue;
                if (m.GetHeight() > oldDisplayMode.GetHeight() || m.GetWidth() > oldDisplayMode.GetWidth())
                    continue;
                int j = 0;
                VideoMode ml = null;
                for (j = 0; j < l.Size(); j++)
                {
                    ml = (VideoMode)l.Get(j);
                    if (ml.GetWidth() > m.GetWidth())
                        break;
                    if (ml.GetWidth() == m.GetWidth() && ml.GetHeight() >= m.GetHeight())
                        break;
                }

                if (j == l.Size())
                {
                    l.AddLast(m);
                }
                else if (ml.GetWidth() > m.GetWidth() || ml.GetHeight() > m.GetHeight())
                {
                    l.Add(j, m);
                }
                else if (m.GetFrequency() > ml.GetFrequency())
                {
                    l.Remove(j);
                    l.Add(j, m);
                }
            }

            VideoMode[] ma = new VideoMode[l.Size()];
            l.ToArray(ma);
            return ma;
        }

        private VideoMode FindDisplayMode(Size dim)
        {
            VideoMode mode = null;
            VideoMode m = null;
            VideoMode[] modes = GetLWJGLModeList();
            int w = dim.Width;
            int h = dim.Height;
            for (int i = 0; i < modes.Length; i++)
            {
                m = modes[i];
                if (m.GetWidth() == w && m.GetHeight() == h)
                {
                    mode = m;
                    break;
                }
            }

            if (mode == null)
                mode = oldDisplayMode;
            return mode;
        }

        public virtual string GetModeString(VideoMode m)
        {
            StringBuffer sb = new StringBuffer();
            sb.Append(m.GetWidth());
            sb.Append('x');
            sb.Append(m.GetHeight());
            sb.Append('x');
            sb.Append(m.GetBitsPerPixel());
            sb.Append('@');
            sb.Append(m.GetFrequency());
            sb.Append("Hz");
            return sb.ToString();
        }

        public virtual int SetMode( Size dim, int mode, bool fullscreen)
        {
            VID.Printf(Defines.PRINT_ALL, "Initializing OpenGL display\\n");
            VID.Printf(Defines.PRINT_ALL, "...setting mode " + mode + ":");
            if (oldDisplayMode == null)
            {
                oldDisplayMode = Display.GetDisplayMode();
            }

            if (!VID.GetModeInfo(out var newDim, mode))
            {
                VID.Printf(Defines.PRINT_ALL, " invalid mode\\n");
                return Base.rserr_invalid_mode;
            }

            VID.Printf(Defines.PRINT_ALL, " " + newDim.Width + " " + newDim.Height + '\\');
            Shutdown();
            Display.SetTitle("Jake2 (lwjgl)");
            VideoMode VideoMode = FindDisplayMode(newDim);
            newDim.Width = VideoMode.GetWidth();
            newDim.Height = VideoMode.GetHeight();
            if (fullscreen)
            {
                try
                {
                    Display.SetDisplayMode(VideoMode);
                }
                catch (LWJGLException e)
                {
                    return Base.rserr_invalid_mode;
                }

                Display.SetLocation(0, 0);
                try
                {
                    Display.SetFullscreen(fullscreen);
                }
                catch (LWJGLException e)
                {
                    return Base.rserr_invalid_fullscreen;
                }

                VID.Printf(Defines.PRINT_ALL, "...setting fullscreen " + GetModeString(VideoMode) + '\\');
            }
            else
            {
                try
                {
                    Display.SetDisplayMode(VideoMode);
                }
                catch (LWJGLException e)
                {
                    return Base.rserr_invalid_mode;
                }

                try
                {
                    Display.SetFullscreen(false);
                }
                catch (LWJGLException e)
                {
                    return Base.rserr_invalid_fullscreen;
                }
            }

            Base.SetVid(newDim.width, newDim.height);
            try
            {
                Display.Create();
            }
            catch (LWJGLException e)
            {
                return Base.rserr_unknown;
            }

            VID.NewWindow(newDim.width, newDim.height);
            return Base.rserr_ok;
        }

        public virtual void Shutdown()
        {
            if (oldDisplayMode != null && Display.IsFullscreen())
            {
                try
                {
                    Display.SetDisplayMode(oldDisplayMode);
                }
                catch (Exception e)
                {
                    e.PrintStackTrace();
                }
            }

            while (Display.IsCreated())
            {
                Display.Destroy();
            }
        }

        public virtual bool Init(int xpos, int ypos)
        {
            window_xpos = xpos;
            window_ypos = ypos;
            return true;
        }

        public virtual void BeginFrame(float camera_separation)
        {
        }

        public virtual void EndFrame()
        {
            GlFlush();
            Display.Update();
        }

        public virtual void AppActivate(bool activate)
        {
        }

        public virtual void EnableLogging(bool enable)
        {
        }

        public virtual void LogNewFrame()
        {
        }

        public void UpdateScreen(xcommand_t callback)
        {
            callback.Execute();
        }

		public virtual void Screenshot( )
		{
			throw new NotImplementedException();
		}
	}
}
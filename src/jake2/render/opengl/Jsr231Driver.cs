using J2N.Text;
using Jake2.Client;
using Jake2.Qcommon;
using Jake2.Sys;
using Jake2.Util;
using Jake2Sharp.util;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jake2.Render.Opengl
{
    public abstract class Jsr231Driver : IGLDriver
    {
        public Jsr231Driver()
        {
        }

        private VideoMode oldDisplayMode;
        private volatile Display display;
        private volatile GameWindow window;
        int window_xpos, window_ypos;
        public virtual VideoMode[] GetModeList()
        {
            VideoMode[] modes = null;
            unsafe
            {
                var ptr = ( OpenTK.Windowing.GraphicsLibraryFramework.Monitor* ) window.CurrentMonitor.Pointer.ToPointer();
                modes = GLFW.GetVideoModes( ptr );
            }

            LinkedList<VideoMode> l = new LinkedList<VideoMode>();

            l.AddLast(oldDisplayMode);
            for (int i = 0; i < modes.Length; i++)
            {
                VideoMode m = modes[i];
                if (m.GetBitDepth() != oldDisplayMode.GetBitDepth())
                    continue;
                if (m.RefreshRate > oldDisplayMode.RefreshRate )
                    continue;
                if (m.Height < 240 || m.Width < 320)
                    continue;
                int j = 0;
                VideoMode ml = null;
                for (j = 0; j < l.Count; j++)
                {
                    ml = (VideoMode)l.ElementAt(j);
                    if (ml.Width > m.Width)
                        break;
                    if (ml.Width == m.Width && ml.Height >= m.Height)
                        break;
                }

                if (j == l.Count)
                {
                    l.AddLast(m);
                }
                else if (ml.Width > m.Width || ml.Height > m.Height)
                {
                    l.Add(j, m);
                }
                else if (m.RefreshRate > ml.RefreshRate)
                {
                    l.Remove(j);
                    l.Add(j, m);
                }
            }

            VideoMode[] ma = new VideoMode[l.Count];
            l.ToArray(ma);
            return ma;
        }

        public virtual VideoMode FindDisplayMode(Size dim)
        {
            VideoMode mode = null;
            VideoMode m = null;
            VideoMode[] modes = GetModeList();
            int w = dim.Width;
            int h = dim.Height;
            for (int i = 0; i < modes.Length; i++)
            {
                m = modes[i];
                if (m.Width == w && m.Height == h)
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
            sb.Append(m.Width);
            sb.Append('x');
            sb.Append(m.Height);
            sb.Append('x');
            sb.Append(m.GetBitDepth());
            sb.Append('@');
            sb.Append(m.RefreshRate);
            sb.Append("Hz");
            return sb.ToString();
        }

        public virtual int SetMode(Size dim, int mode, bool fullscreen)
        {
            VID.Printf(Defines.PRINT_ALL, "Initializing OpenGL display\\n");
            VID.Printf(Defines.PRINT_ALL, "...setting mode " + mode + ":");
            //if (Globals.appletMode && container == null)
            //{
            //    container = (Container)Globals.applet;
            //}

            if (device == null)
            {
                GraphicsEnvironment env = GraphicsEnvironment.GetLocalGraphicsEnvironment();
                device = env.GetDefaultScreenDevice();
            }

            if (oldDisplayMode == null)
            {
                oldDisplayMode = device.GetDisplayMode();
            }

            if (!VID.GetModeInfo(out var newDim, mode))
            {
                VID.Printf(Defines.PRINT_ALL, " invalid mode\\n");
                return Base.rserr_invalid_mode;
            }

            VID.Printf(Defines.PRINT_ALL, " " + newDim.Width + " " + newDim.Height + '\\');
            if (!Globals.appletMode)
            {
                if (window != null)
                    Shutdown();
                window = new Frame("Jake2 (jsr231)");
                container = window;
                ImageIcon icon = new ImageIcon(GetType().GetResource("/icon-small.png"));
                window.SetIconImage(icon.GetImage());
                window.SetLayout(new GridBagLayout());
                window.AddWindowListener(new AnonymousWindowAdapter(this));
            }

            if (Globals.appletMode)
            {
                Shutdown();
                fullscreen = false;
                JOGLKBD.Init(container);
            }

            Display canvas = new Display(new GLCapabilities());
            canvas.SetFocusTraversalKeysEnabled(false);
            canvas.SetSize(newDim.Width, newDim.Height);
            GridBagConstraints gbc = new GridBagConstraints();
            gbc.fill = GridBagConstraints.BOTH;
            gbc.weightx = gbc.weighty = 1;
            container.AddComponentListener(JOGLKBD.listener);
            canvas.AddKeyListener(JOGLKBD.listener);
            canvas.AddMouseListener(JOGLKBD.listener);
            canvas.AddMouseMotionListener(JOGLKBD.listener);
            canvas.AddMouseWheelListener(JOGLKBD.listener);
            if (fullscreen)
            {
                container.Add(canvas, gbc);
                VideoMode VideoMode = FindDisplayMode(newDim);
                newDim.Width = VideoMode.GetWidth();
                newDim.Height = VideoMode.GetHeight();
                window.SetUndecorated(true);
                window.SetResizable(false);
                device.SetFullScreenWindow(window);
                if (device.IsFullScreenSupported())
                    device.SetDisplayMode(VideoMode);
                window.SetLocation(0, 0);
                window.SetSize(VideoMode.GetWidth(), VideoMode.GetHeight());
                canvas.SetSize(VideoMode.GetWidth(), VideoMode.GetHeight());
                VID.Printf(Defines.PRINT_ALL, "...setting fullscreen " + GetModeString(VideoMode) + '\\');
            }
            else
            {
                if (!Globals.appletMode)
                {
                    container.Add(canvas, gbc);
                    Frame f2 = window;
                    try
                    {
                        EventQueue.InvokeAndWait(new AnonymousRunnable(this));
                    }
                    catch (Exception e)
                    {
                        e.PrintStackTrace();
                    }
                }
                else
                {
                    Display fd = canvas;
                    try
                    {
                        EventQueue.InvokeAndWait(new AnonymousRunnable1(this));
                    }
                    catch (Exception e)
                    {
                        e.PrintStackTrace();
                    }
                }
            }

            if (!Globals.appletMode)
            {
                while (!canvas.IsDisplayable() || !window.IsDisplayable())
                {
                    try
                    {
                        Thread.Sleep(100);
                    }
                    catch (InterruptedException e)
                    {
                    }
                }
            }

            canvas.RequestFocus();
            this.display = canvas;
            SetGL(display.GetGL());
            Init(0, 0);
            return Base.rserr_ok;
        }

        private sealed class AnonymousWindowAdapter : WindowAdapter
        {
            public AnonymousWindowAdapter(Jsr231Driver parent)
            {
                this.parent = parent;
            }

            private readonly Jsr231Driver parent;
            public void WindowClosing(WindowEvent e)
            {
                Cbuf.ExecuteText(Defines.EXEC_APPEND, "quit");
            }
        }

        private sealed class AnonymousRunnable : Runnable
        {
            public AnonymousRunnable(Jsr231Driver parent)
            {
                this.parent = parent;
            }

            private readonly Jsr231Driver parent;
            protected override void Execute( )
            {
                f2.Pack();
                f2.SetResizable(false);
                f2.SetVisible(true);
            }
        }

        private sealed class AnonymousRunnable1 : Runnable
        {
            public AnonymousRunnable1(Jsr231Driver parent)
            {
                this.parent = parent;
            }

            private readonly Jsr231Driver parent;
            protected override void Execute( )
            {
                parent.container.Add(fd, BorderLayout.CENTER);
                SizeChangeListener listener = Globals.sizeChangeListener;
                if (listener != null)
                {
                    listener.SizeChanged(newDim.width, newDim.height);
                }

                fd.SetSize(newDim.width, newDim.height);
            }
        }

        public virtual void Shutdown()
        {
            if (!Globals.appletMode)
            {
                try
                {
                    EventQueue.InvokeAndWait(new AnonymousRunnable2(this));
                }
                catch (Exception e)
                {
                    e.PrintStackTrace();
                }

                if (window != null)
                {
                    if (display != null)
                        display.Destroy();
                    window.Dispose();
                    while (window.IsDisplayable())
                    {
                        try
                        {
                            Thread.Sleep(100);
                        }
                        catch (InterruptedException e)
                        {
                        }
                    }
                }
            }
            else
            {
                if (display != null)
                {
                    display.Destroy();
                    container.Remove(display);
                }
            }

            display = null;
        }

        private sealed class AnonymousRunnable2 : Runnable
        {
            public AnonymousRunnable2(Jsr231Driver parent)
            {
                this.parent = parent;
            }

            private readonly Jsr231Driver parent;
            protected override void Execute()
            {
                if (oldDisplayMode != null && device.GetFullScreenWindow() != null)
                {
                    try
                    {
                        if (device.IsFullScreenSupported())
                        {
                            if (!device.GetDisplayMode().Equals(oldDisplayMode))
                                device.SetDisplayMode(oldDisplayMode);
                        }

                        device.SetFullScreenWindow(null);
                    }
                    catch (Exception e)
                    {
                        e.PrintStackTrace();
                    }
                }
            }
        }

        public virtual bool Init(int xpos, int ypos)
        {
            window_xpos = xpos;
            window_ypos = ypos;
            BeginFrame(0F);
            GlViewport(0, 0, display.GetWidth(), display.GetHeight());
            GlClearColor(0, 0, 0, 0);
            GlClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            EndFrame();
            BeginFrame(0F);
            GlViewport(0, 0, display.GetWidth(), display.GetHeight());
            GlClearColor(0, 0, 0, 0);
            GlClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            EndFrame();
            return true;
        }

        public virtual void BeginFrame(float camera_separation)
        {
            display.Activate();
        }

        public virtual void EndFrame()
        {
            GlFlush();
            display.Update();
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

        public virtual void UpdateScreen(xcommand_t callback)
        {
            callback.Execute();
        }

        protected virtual void Activate()
        {
            display.Activate();
        }

		public virtual void Screenshot( )
		{
			throw new NotImplementedException();
		}

		private class Display : Canvas
        {
            private GLDrawable drawable;
            private GLContext context;
            public Display(GLCapabilities capabilities): base(Unwrap((AWTGraphicsConfiguration)GLDrawableFactory.GetFactory().ChooseGraphicsConfiguration(capabilities, null, null)))
            {
                drawable = GLDrawableFactory.GetFactory().GetGLDrawable(this, capabilities, null);
                context = drawable.CreateContext(null);
            }

            public virtual void SetBounds(int x, int y, int width, int height)
            {
                int mask = ~0x03;
                if ((width & 0x03) != 0)
                {
                    width &= mask;
                    width += 4;
                }

                base.SetBounds(x, y, width, height);
                Base.SetVid(width, height);
                VID.NewWindow(width, height);
            }

            public virtual void Paint(Graphics g)
            {
            }

            public virtual void Update(Graphics g)
            {
            }

            public virtual void AddNotify()
            {
                base.AddNotify();
                base.SetBackground(Color.BLACK);
                drawable.SetRealized(true);
            }

            public virtual void RemoveNotify()
            {
                if (drawable != null)
                {
                    drawable.SetRealized(false);
                    drawable = null;
                }

                base.RemoveNotify();
            }

            public virtual void Activate()
            {
                if (GLContext.GetCurrent() != context)
                    context.MakeCurrent();
            }

            private void Release()
            {
                if (GLContext.GetCurrent() == context)
                    context.Release();
            }

            public virtual void Update()
            {
                Release();
                drawable.SwapBuffers();
            }

            public virtual void Destroy()
            {
                if (context != null)
                {
                    Release();
                    context.Destroy();
                    context = null;
                }
            }

            private static GraphicsConfiguration Unwrap(AWTGraphicsConfiguration config)
            {
                if (config == null)
                {
                    return null;
                }

                return config.GetGraphicsConfiguration();
            }
        }
    }
}
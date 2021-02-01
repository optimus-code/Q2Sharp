using J2N.Text;
using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using Image = OpenTK.Windowing.Common.Input.Image;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace Q2Sharp.Render.Opengl
{
    public abstract class JoglDriver : IGLDriver
    {
        private VideoMode? oldDisplayMode;
        GameWindow window;
        int window_xpos, window_ypos;
        protected bool post_init = false;
        protected bool contextInUse = false;
        protected static xcommand_t INIT_CALLBACK;

        public JoglDriver( )
        {
            INIT_CALLBACK = new Anonymousxcommand_t( this );
        }

        private sealed class Anonymousxcommand_t : xcommand_t
        {
            public Anonymousxcommand_t(JoglDriver parent)
            {
                this.parent = parent;
            }

            private readonly JoglDriver parent;
            public override void Execute()
            {
                GL.ClearColor(0f, 0f, 0f, 0f);
                GL.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
                if (!parent.post_init )
                {
                    VID.Printf(Defines.PRINT_ALL, "Missing multi-texturing for FastJOGL renderer\\n");
                }

                parent.EndFrame();
            }
        }

        xcommand_t callback = INIT_CALLBACK;
        public virtual VideoMode[] GetModeList()
        {
            VideoMode[] modes = null;
            unsafe
            {
                var ptr = ( OpenTK.Windowing.GraphicsLibraryFramework.Monitor* ) window.CurrentMonitor.Pointer.ToPointer();
                modes = GLFW.GetVideoModes( ptr );
            }

            LinkedList<VideoMode> l = new LinkedList<VideoMode>();
            l.AddLast(oldDisplayMode.Value);
            for (int i = 0; i < modes.Length; i++)
            {
                VideoMode m = modes[i];
                if (m.RedBits + m.GreenBits + m.BlueBits != oldDisplayMode.Value.RedBits + oldDisplayMode.Value.GreenBits + oldDisplayMode.Value.BlueBits)
                    continue;
                if (m.RefreshRate > oldDisplayMode.Value.RefreshRate )
                    continue;
                if (m.Width < 240 || m.Width < 320)
                    continue;
                int j = 0;
                VideoMode ml = default;
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
                    l.AddBefore(l.Find(ml), m);
                }
                else if (m.RefreshRate > ml.RefreshRate)
                {
                    l.AddBefore(l.Find(ml), m);
                    l.Remove(l.Find(ml));
                }
            }

            VideoMode[] ma = new VideoMode[l.Count];
            ma = l.ToArray();
            return ma;
        }

        public virtual VideoMode FindDisplayMode(Size dim)
        {
            VideoMode? mode = null;
            VideoMode m = default;
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
            return mode.Value;
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

            unsafe {
                Monitor* device = GLFW.GetPrimaryMonitor();
                if (oldDisplayMode == null)
                {
                    oldDisplayMode = GLFW.GetVideoMode(device)[0];
                }
            }

            if (!VID.GetModeInfo(out var newDim, mode))
            {
                VID.Printf(Defines.PRINT_ALL, " invalid mode\\n");
                return Base.rserr_invalid_mode;
            }

            VID.Printf(Defines.PRINT_ALL, " " + newDim.Width + " " + newDim.Height + '\\');
            Shutdown();

            var newInstance = ( window == null );

            if ( !newInstance )
                window.Dispose();

            window = new GameWindow( new GameWindowSettings(), new NativeWindowSettings 
            {
                Title = "Q2Sharp (jogl)",
                Size = new OpenTK.Mathematics.Vector2i(newDim.Width, newDim.Height),
                StartVisible = false,
                WindowBorder = OpenTK.Windowing.Common.WindowBorder.Fixed,
                IsFullscreen = false
                //Icon = new OpenTK.Windowing.Common.Input.WindowIcon(new OpenTK.Windowing.Common.Input.Image(32,32,null)),
            } );

            window.RenderFrame += ( t ) => Program.Frame();
            Program.RunWindow += ( ) => window.Run();

            //ImageIcon icon = new ImageIcon(GetType().GetResource("/icon-small.png"));
            Bitmap bitmap = (Bitmap) Bitmap.FromStream(GetType().Assembly.GetManifestResourceStream("/icon-small.png"));
            byte[] pixels = new byte[bitmap.Width * bitmap.Height];

            for (int y = 0; y < bitmap.Height; y++)
            for (int x = 0; x < bitmap.Width; x++)
            {
                var color = bitmap.GetPixel(x, y);
                Array.Copy(new byte[] { color.R, color.G, color.B, color.A }, 0, pixels, (y * bitmap.Width + x) * 4, 4);
            }
            
            Image icon = new Image(bitmap.Width, bitmap.Height, pixels);
            window.Icon = new WindowIcon(icon);

            window.Minimized += ( e ) => JOGLKBD.listener.ComponentHidden( e );
            window.Maximized += ( e ) =>
            {
                JOGLKBD.c = window;
                JOGLKBD.listener.ComponentShown( e );
            };
            window.Move += ( e ) => JOGLKBD.listener.ComponentMoved( e );
            window.Closing += ( e ) => Cbuf.ExecuteText( Defines.EXEC_APPEND, "quit" );
            window.Resize += ( e ) => JOGLKBD.listener.ComponentResized( e );
            window.KeyDown += ( e ) => JOGLKBD.listener.KeyPressed( e );
            window.KeyUp += ( e ) => JOGLKBD.listener.KeyReleased( e );
            window.TextInput += ( e ) => JOGLKBD.listener.KeyTyped( e );
            window.MouseEnter += ( ) => JOGLKBD.listener.MouseEntered();
            window.MouseLeave += ( ) => JOGLKBD.listener.MouseExited();
            window.MouseMove += ( e ) => JOGLKBD.listener.MouseMoved( e );
            window.MouseDown += ( e ) => JOGLKBD.listener.MousePressed( e );
            window.MouseUp += ( e ) => JOGLKBD.listener.MouseReleased( e );
            window.MouseWheel += ( e ) => JOGLKBD.listener.MouseWheelMoved( e );
            //window.drag += ( e ) => JOGLKBD.listener.MouseDragged( e );

            if ( fullscreen)
            {
                window.WindowState = OpenTK.Windowing.Common.WindowState.Fullscreen;
                VideoMode VideoMode = FindDisplayMode(newDim);
                newDim.Width = VideoMode.Width;
                newDim.Height = VideoMode.Height;
                window.WindowState = WindowState.Fullscreen;
                if (window.IsFullscreen)
                {
                    unsafe
                    {
                        GLFW.SetWindowSize(window.WindowPtr, VideoMode.Width, VideoMode.Height);
                    }
                }
                window.Location = new OpenTK.Mathematics.Vector2i();
                window.Size = new OpenTK.Mathematics.Vector2i(VideoMode.Width, VideoMode.Height);
                VID.Printf(Defines.PRINT_ALL, "...setting fullscreen " + GetModeString(VideoMode) + '\\');
                window.IsVisible = true;
            }
            else
            {
                window.Location = new OpenTK.Mathematics.Vector2i( window_xpos, window_ypos );
                window.IsVisible = true;
            }

            Base.SetVid(newDim.Width, newDim.Height);
            VID.NewWindow(newDim.Width, newDim.Height);

            return Base.rserr_ok;
        }

        public virtual void Shutdown()
        {
            if (oldDisplayMode != null)
            {
                try
                {
                    if (window.IsFullscreen)
                    {
                        unsafe
                        {
                            GLFW.SetWindowSize(window.WindowPtr, oldDisplayMode.Value.Width, oldDisplayMode.Value.Height);
                        }
                    }
                    window.WindowState = WindowState.Normal;
                }
                catch (Exception e)
                {
                    e.PrintStackTrace();
                }
            }

            if (window != null)
                window.Dispose();

            post_init = false;
            callback = INIT_CALLBACK;
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
            GL.Flush();
            window.SwapBuffers();
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

        public virtual void UpdateScreen()
        {
            this.callback = INIT_CALLBACK;
            //window.Display();
        }

        public virtual void UpdateScreen(xcommand_t callback)
        {
            this.callback = callback;
            //window.Display();
        }

        public virtual void Init()
        {
        }

        public virtual void Display()
        {
            contextInUse = true;
            callback.Execute();
            contextInUse = false;
        }

        public virtual void DisplayChanged(bool arg1, bool arg2)
        {
        }

        public virtual void Reshape(int x, int y, int width, int height)
        {
        }

		public virtual void Screenshot( )
		{
			throw new NotImplementedException();
		}
	}
}
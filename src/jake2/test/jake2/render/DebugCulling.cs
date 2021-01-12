using Q2Sharp.Client;
using Q2Sharp.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public class DebugCulling
    {
        static readonly float INSTANT_PARTICLE = -10000F;
        static readonly float PARTICLE_GRAVITY = 40F;
        String[] args;
        Irefexport_t re;
        viddef_t viddef;
        int framecount = 0;
        public DebugCulling(String[] args)
        {
            this.args = args;
        }

        public static void Main(String[] args)
        {
            DebugCulling test = new DebugCulling(args);
            test.Init();
            test.Run();
        }

        public virtual void Init()
        {
            Qcommon.Init(new string {"$Id: DebugCulling.java,v 1.6 2008-03-02 14:56:21 cawe Exp $"});
            VID.Shutdown();
            this.re = Renderer.GetDriver("jogl");
            re.Init(0, 0);
            viddef = Globals.viddef;
        }

        float fps = 0F;
        long start = 0;
        long startTime;
        public virtual void Run()
        {
            startTime = System.CurrentTimeMillis();
            while (true)
            {
                re.UpdateScreen(null);
                re.GetKeyboardHandler().Update();
            }
        }

        int currentState = 0;
        public virtual void UpdateScreen()
        {
            re.BeginFrame(0F);
            switch (currentState)

            {
                case 0:
                    re.DrawStretchPic(0, 0, viddef.GetWidth(), viddef.GetHeight(), "conback");
                    re.DrawPic(viddef.GetWidth() / 2 - 50, viddef.GetHeight() / 2, "loading");
                    currentState = 1;
                    break;
                case 1:
                    re.SetSky("space1", 0, new float {0, 0, 0});
                    re.BeginRegistration("ColorTest");
                    re.EndRegistration();
                    currentState = 2;
                default:
                    if (framecount % 500 == 0)
                    {
                        long time = System.CurrentTimeMillis();
                        fps = 500000F / (time - start);
                        start = time;
                    }

                    string text = fps + " fps";
                    TestMap();
                    DrawString(10, viddef.GetHeight() - 16, text);
            }

            re.EndFrame();
            framecount++;
        }

        private float fov_x = 90;
        private refdef_t refdef;
        private void TestMap()
        {
            if (refdef == null)
            {
                refdef = new refdef_t();
                refdef.x = 0;
                refdef.y = 0;
                refdef.width = viddef.GetWidth();
                refdef.height = viddef.GetHeight();
                refdef.fov_x = fov_x;
                refdef.fov_y = CalcFov(fov_x, refdef.width - 10, refdef.height - 10);
                refdef.vieworg = new float[]{0, 0, 0};
                refdef.viewangles[0] = 0;
                refdef.viewangles[1] = 90;
                refdef.viewangles[2] = 0;
                refdef.blend = new float[] { 0F, 0F, 0F, 0F};
                refdef.areabits = null;
                refdef.num_entities = 0;
                refdef.entities = null;
                lightstyle_t light = new lightstyle_t();
                light.rgb = new float[] { 1F, 1F, 1F};
                light.white = 3F;
                refdef.lightstyles = new lightstyle_t[Defines.MAX_LIGHTSTYLES];
                for (int i = 0; i < Defines.MAX_LIGHTSTYLES; i++)
                {
                    refdef.lightstyles[i] = new lightstyle_t();
                    refdef.lightstyles[i].rgb = new float[] { 1F, 1F, 1F};
                    refdef.lightstyles[i].white = 3F;
                }
            }

            refdef.time = Time() * 0.001F;
            refdef.viewangles[0] += KBD.my * 0.1F;
            refdef.viewangles[1] -= KBD.mx * 0.1F;
            refdef.vieworg[0] = 0;
            refdef.vieworg[1] = -79;
            refdef.vieworg[2] = -131;
            refdef.vieworg[0] += 1F / 16;
            refdef.vieworg[1] += 1F / 16;
            refdef.vieworg[2] += 1F / 16;
            re.RenderFrame(refdef);
        }

        private float CalcFov(float fov_x, float width, float height)
        {
            double a;
            double x;
            if (fov_x < 1 || fov_x > 179)
                Com.Error(Defines.ERR_DROP, "Bad fov: " + fov_x);
            x = width / Math.Tan(fov_x / 360 * Math.PI);
            a = Math.Atan(height / x);
            a = a * 360 / Math.PI;
            return (float)a;
        }

        private void DrawString(int x, int y, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                re.DrawChar(x + 8 * i, y, (int)text[i]);
            }
        }

        private int Time()
        {
            return (int)(System.CurrentTimeMillis() - startTime);
        }
    }
}
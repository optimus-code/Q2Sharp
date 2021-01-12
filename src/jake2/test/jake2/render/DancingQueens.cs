using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using Q2Sharp.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public class DancingQueens
    {
        String[] args;
        Irefexport_t re;
        viddef_t viddef;
        int framecount = 0;
        public DancingQueens(String[] args)
        {
            this.args = args;
        }

        public static void Main(String[] args)
        {
            DancingQueens test = new DancingQueens(args);
            test.Init();
            test.Run();
        }

        KBD kbd;
        public virtual void Init()
        {
            Globals.dedicated = Cvar.Get("dedicated", "0", Qcommon.CVAR_NOSET);
            Program.Q2Dialog = new Q2DataDialog();
            Locale.SetDefault(Locale.US);
            Program.Q2Dialog.SetVisible(true);
            string DRIVER = "lwjgl";
            Qcommon.Init(new string {"DancingQueens", "+set", "gl_mode", "4", "+set", "vid_fullscreen", "0", "+set", "vid_ref", DRIVER});
            VID.Shutdown();
            String[] names = Renderer.GetDriverNames();
            System.Diagnostics.Debug.WriteLine("Registered Drivers: " + Arrays.AsList(names));
            this.re = Renderer.GetDriver(DRIVER);
            Globals.re = this.re;
            System.Diagnostics.Debug.WriteLine("Use driver: " + re);
            System.out_renamed.Flush();
            re.Init(0, 0);
            kbd = re.GetKeyboardHandler();
            kbd.Init();
            Cbuf.AddText("unbind t");
            Cbuf.Execute();
            Cmd.AddCommand("togglemouse", togglemouse);
            Cbuf.AddText("bind t togglemouse");
            Cbuf.Execute();
            Globals.cls.key_dest = Defines.key_game;
            Globals.cls.state = Defines.ca_active;
            viddef = Globals.viddef;
            fov_y = Math3D.CalcFov(fov_x, viddef.GetWidth(), viddef.GetHeight());
        }

        float fps = 0F;
        long start = 0;
        public virtual void UpdateScreen()
        {
            re.BeginFrame(0F);
            if (framecount % 500 == 0)
            {
                long time = System.CurrentTimeMillis();
                fps = 500000F / (time - start);
                start = time;
            }

            string text = ((int)(fps + 0.5F)) + " fps";
            TestModel();
            DrawString(10, viddef.GetHeight() - 16, text);
            re.EndFrame();
            framecount++;
        }

        long startTime;
        public virtual void Run()
        {
            startTime = System.CurrentTimeMillis();
            xcommand_t callback = new Anonymousxcommand_t(this);
            while (true)
            {
                re.UpdateScreen(callback);
                kbd.Update();
                Cbuf.Execute();
            }
        }

        private sealed class Anonymousxcommand_t : xcommand_t
        {
            public Anonymousxcommand_t(DancingQueens parent)
            {
                this.parent = parent;
            }

            private readonly DancingQueens parent;
            public override void Execute()
            {
                UpdateScreen();
            }
        }

        private float yaw = 0;
        private entity_t[] models;
        private static readonly String[] skinNames = new[]{"players/female/athena", "players/female/lotus", "players/female/venus", "players/female/voodoo", "players/female/cobalt", "players/female/lotus", "players/female/brianna"};
        private float fov_x = 50;
        private float fov_y;
        private void TestModel()
        {
            refdef_t refdef = new refdef_t();
            refdef.x = 0;
            refdef.y = 0;
            refdef.width = viddef.GetWidth();
            refdef.height = viddef.GetHeight();
            refdef.fov_x = fov_x;
            refdef.fov_y = fov_y;
            refdef.time = 1F * 0.001F;
            if (models == null)
            {
                models = new entity_t[12];
                entity_t m = null;
                for (int i = 0; i < models.length; i++)
                {
                    m = GetModel(skinNames[i % skinNames.length]);
                    m.origin[0] += 30 * i;
                    m.origin[1] += ((i % 4)) * 30 - 20;
                    models[i] = m;
                }
            }

            yaw = Time() * 0.1F;
            if (yaw > 360)
                yaw -= 360;
            if (yaw < 0)
                yaw += 360;
            for (int i = 0; i < models.length; i++)
            {
                models[i].frame = (Time() / 70) % models[i].model.numframes;
                models[i].angles[1] = yaw;
                models[i].origin[0] += KBD.my;
                models[i].origin[1] += KBD.mx;
            }

            refdef.areabits = null;
            refdef.num_entities = models.length;
            refdef.entities = models;
            refdef.lightstyles = null;
            refdef.rdflags = Defines.RDF_NOWORLDMODEL;
            re.RenderFrame(refdef);
        }

        private entity_t GetModel(string name)
        {
            entity_t entity = new entity_t();
            string modelName = "players/female/tris.md2";
            string modelSkin = name + ".pcx";
            entity.model = re.RegisterModel(modelName);
            entity.skin = re.RegisterSkin(modelSkin);
            entity.flags = Defines.RF_FULLBRIGHT;
            entity.origin[0] = 80;
            entity.origin[1] = 0;
            entity.origin[2] = 0;
            Math3D.VectorCopy(entity.origin, entity.oldorigin);
            entity.frame = 0;
            entity.oldframe = 0;
            entity.backlerp = 0F;
            return entity;
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

        static xcommand_t togglemouse = new Anonymousxcommand_t1(this);
        private sealed class Anonymousxcommand_t1 : xcommand_t
        {
            public Anonymousxcommand_t1(DancingQueens parent)
            {
                this.parent = parent;
            }

            private readonly DancingQueens parent;
            public override void Execute()
            {
                IN.ToggleMouse();
            }
        }
    }
}
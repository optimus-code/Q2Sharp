using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public class TestMap
    {
        static readonly float INSTANT_PARTICLE = -10000F;
        static readonly float PARTICLE_GRAVITY = 40F;
        String[] args;
        Irefexport_t re;
        viddef_t viddef;
        int framecount = 0;
        public TestMap(String[] args)
        {
            this.args = args;
        }

        public static void Main(String[] args)
        {
            TestMap test = new TestMap(args);
            test.Init();
            test.Run();
        }

        public virtual void Init()
        {
            Globals.dedicated = Cvar.Get("dedicated", "0", Qcommon.CVAR_NOSET);
            if (Globals.dedicated.value != 1F)
            {
                Program.Q2Dialog = new Q2DataDialog();
                Locale.SetDefault(Locale.US);
                Program.Q2Dialog.SetVisible(true);
            }

            Qcommon.Init(new string[]{"TestMap"});
            VID.Shutdown();
            Globals.re = this.re = Renderer.GetDriver("jsr231", true);
            re.Init(0, 0);
            Cmd.AddCommand("+tforward", forward_down);
            Cmd.AddCommand("-tforward", forward_up);
            Cbuf.AddText("bind UPARROW +tforward");
            Cbuf.Execute();
            Cmd.AddCommand("+tbackward", backward_down);
            Cmd.AddCommand("-tbackward", backward_up);
            Cbuf.AddText("bind DOWNARROW +tbackward");
            Cbuf.Execute();
            Cmd.AddCommand("+tleft", left_down);
            Cmd.AddCommand("-tleft", left_up);
            Cbuf.AddText("bind LEFTARROW +tleft");
            Cbuf.Execute();
            Cmd.AddCommand("+tright", right_down);
            Cmd.AddCommand("-tright", right_up);
            Cbuf.AddText("bind RIGHTARROW +tright");
            Cbuf.Execute();
            Cmd.AddCommand("togglemouse", togglemouse);
            Cbuf.AddText("bind t togglemouse");
            Cbuf.Execute();
            Globals.cls.key_dest = Defines.key_game;
            Globals.cls.state = Defines.ca_active;
            viddef = Globals.viddef;
        }

        float fps = 0F;
        long start = 0;
        long startTime;
        public virtual void Run()
        {
            startTime = System.CurrentTimeMillis();
            xcommand_t callback = new Anonymousxcommand_t(this);
            while (true)
            {
                re.UpdateScreen(callback);
                re.GetKeyboardHandler().Update();
                Cbuf.Execute();
            }
        }

        private sealed class Anonymousxcommand_t : xcommand_t
        {
            public Anonymousxcommand_t(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                parent.UpdateScreen();
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
                    re.BeginRegistration("demo1");
                    re.SetSky("space1", 0, new float[] {0, 0, 0});
                    currentState = 2;
                    break;
                default:
                    if (framecount % 500 == 0)
                    {
                        long time = System.CurrentTimeMillis();
                        fps = 500000F / (time - start);
                        start = time;
                    }

                    string text = fps + " fps";
                    RunTest();
                    DrawString(10, viddef.GetHeight() - 16, text);
                    break;
            }

            re.EndFrame();
            framecount++;
        }

        static readonly int FORWARD = 2;
        static readonly int FORWARD_MASK = ~FORWARD;
        static readonly int BACKWARD = 4;
        static readonly int BACKWARD_MASK = ~BACKWARD;
        static readonly int LEFT = 8;
        static readonly int LEFT_MASK = ~LEFT;
        static readonly int RIGHT = 16;
        static readonly int RIGHT_MASK = ~RIGHT;
        int movePlayer = 0;
        xcommand_t forward_down = new Anonymousxcommand_t1(this);
        private sealed class Anonymousxcommand_t1 : xcommand_t
        {
            public Anonymousxcommand_t1(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                movePlayer |= FORWARD;
                movePlayer &= BACKWARD_MASK;
            }
        }

        xcommand_t forward_up = new Anonymousxcommand_t2(this);
        private sealed class Anonymousxcommand_t2 : xcommand_t
        {
            public Anonymousxcommand_t2(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                movePlayer &= FORWARD_MASK;
            }
        }

        xcommand_t backward_down = new Anonymousxcommand_t3(this);
        private sealed class Anonymousxcommand_t3 : xcommand_t
        {
            public Anonymousxcommand_t3(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                movePlayer |= BACKWARD;
                movePlayer &= FORWARD_MASK;
            }
        }

        xcommand_t backward_up = new Anonymousxcommand_t4(this);
        private sealed class Anonymousxcommand_t4 : xcommand_t
        {
            public Anonymousxcommand_t4(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                movePlayer &= BACKWARD_MASK;
            }
        }

        xcommand_t left_down = new Anonymousxcommand_t5(this);
        private sealed class Anonymousxcommand_t5 : xcommand_t
        {
            public Anonymousxcommand_t5(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                movePlayer |= LEFT;
                movePlayer &= RIGHT_MASK;
            }
        }

        xcommand_t left_up = new Anonymousxcommand_t6(this);
        private sealed class Anonymousxcommand_t6 : xcommand_t
        {
            public Anonymousxcommand_t6(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                movePlayer &= LEFT_MASK;
            }
        }

        xcommand_t right_down = new Anonymousxcommand_t7(this);
        private sealed class Anonymousxcommand_t7 : xcommand_t
        {
            public Anonymousxcommand_t7(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                movePlayer |= RIGHT;
                movePlayer &= LEFT_MASK;
            }
        }

        xcommand_t right_up = new Anonymousxcommand_t8(this);
        private sealed class Anonymousxcommand_t8 : xcommand_t
        {
            public Anonymousxcommand_t8(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                movePlayer &= RIGHT_MASK;
            }
        }

        private float fov_x = 90;
        private refdef_t refdef;
        private entity_t ent;
        float[] vpn = new float[]{0, 0, 0};
        float[] vright = new float[]{0, 0, 0};
        float[] vup = new float[]{0, 0, 0};
        private void RunTest()
        {
            if (refdef == null)
            {
                refdef = new refdef_t();
                refdef.x = 0;
                refdef.y = 0;
                refdef.width = viddef.GetWidth();
                refdef.height = viddef.GetHeight();
                refdef.fov_x = (Globals.fov == null) ? this.fov_x : Globals.fov.value;
                refdef.fov_x = this.fov_x;
                refdef.fov_y = Math3D.CalcFov(refdef.fov_x, refdef.width, refdef.height);
                refdef.vieworg = new float[] {140, -140, 50};
                refdef.viewangles = new float[] {0, 0, 0};
                refdef.blend = new float[] {0F, 0F, 0F, 0F};
                refdef.areabits = null;
                ent = new entity_t();
                model_t weapon = re.RegisterModel("models/monsters/soldier/tris.md2");
                image_t weaponSkin = re.RegisterSkin("models/monsters/soldier/skin.pcx");
                ent.model = weapon;
                ent.skin = weaponSkin;
                ent.origin = new float[] {-60, 80, 25};
                Math3D.VectorCopy(ent.origin, ent.oldorigin);
                ent.angles = new float[] {0, 300, 0};
                refdef.entities = new entity_t[]{ent};
                refdef.num_entities = refdef.entities.Length;
                lightstyle_t light = new lightstyle_t();
                light.rgb = new float[] {1F, 1F, 1F};
                light.white = 3F;
                refdef.lightstyles = new lightstyle_t[Defines.MAX_LIGHTSTYLES];
                for (int i = 0; i < Defines.MAX_LIGHTSTYLES; i++)
                {
                    refdef.lightstyles[i] = new lightstyle_t();
                    refdef.lightstyles[i].rgb = new float[] {1F, 1F, 1F};
                    refdef.lightstyles[i].white = 3F;
                }

                refdef.viewangles[1] = 130;
                refdef.time = Time() * 0.001F;
            }

            refdef.viewangles[0] += KBD.my * 0.1F;
            refdef.viewangles[1] -= KBD.mx * 0.1F;
            float dt = Time() * 0.001F - refdef.time;
            if (movePlayer != 0)
            {
                float velocity = 150F * dt;
                Math3D.AngleVectors(refdef.viewangles, vpn, vright, vup);
                if ((movePlayer & FORWARD_MASK) != 0)
                    Math3D.VectorMA(refdef.vieworg, -velocity, vpn, refdef.vieworg);
                if ((movePlayer & BACKWARD_MASK) != 0)
                    Math3D.VectorMA(refdef.vieworg, velocity, vpn, refdef.vieworg);
                if ((movePlayer & LEFT_MASK) != 0)
                    Math3D.VectorMA(refdef.vieworg, velocity, vright, refdef.vieworg);
                if ((movePlayer & RIGHT_MASK) != 0)
                    Math3D.VectorMA(refdef.vieworg, -velocity, vright, refdef.vieworg);
                refdef.vieworg[0] += 1F / 16;
                refdef.vieworg[1] += 1F / 16;
                refdef.vieworg[2] += 1F / 16;
            }

            refdef.time = Time() * 0.001F;
            r_numparticles = 0;
            float[] diff = new float[]{0, 0, 0};
            Math3D.VectorSubtract(refdef.vieworg, ent.origin, diff);
            if (Math3D.VectorLength(diff) < 250 && active_particles.Size() == 0)
            {
                RailTrail(ent.origin, refdef.vieworg);
            }
            else
            {
                if (active_particles.Size() > 0)
                {
                    ent.frame = (int)((Time() * 0.013F) % 15);
                    Math3D.VectorNormalize(diff);
                    Math3D.Vectoangles(diff, ent.angles);
                    AnimateParticles();
                    refdef.num_particles = r_numparticles;
                }
                else
                {
                    ent.frame = 0;
                    refdef.num_particles = 0;
                }
            }

            refdef.num_dlights = 0;
            re.RenderFrame(refdef);
        }

        private LinkedList<cparticle_t> active_particles = new LinkedList<cparticle_t>();
        private void AnimateParticles()
        {
            float alpha;
            float time, time2;
            float[] org = new float[]{0, 0, 0};
            int color;
            time = 0F;
            var toRemove = new List<LinkedListNode<cparticle_t>>();
            var node = active_particles.First;
            while(node != null)
			{
                var p = node.Value;
                if ( p.alphavel != INSTANT_PARTICLE )
                {
                    time = ( Time() - p.time ) * 0.001F;
                    alpha = p.alpha + time * p.alphavel;
                    if ( alpha <= 0 )
                    {
                        toRemove.Add( node );
                        continue;
                    }
                }
                else
                {
                    alpha = p.alpha;
                }

                if ( alpha > 1 )
                    alpha = 1;
                color = ( int ) p.color;
                time2 = time * time;
                org[0] = p.org[0] + p.vel[0] * time + p.accel[0] * time2;
                org[1] = p.org[1] + p.vel[1] * time + p.accel[1] * time2;
                org[2] = p.org[2] + p.vel[2] * time + p.accel[2] * time2;
                AddParticle( org, color, alpha );
                if ( p.alphavel == INSTANT_PARTICLE )
                {
                    p.alphavel = 0F;
                    p.alpha = 0F;
                }
                node = node.Next;
            }

            foreach ( var p in toRemove )
                active_particles.Remove(p);
        }

        private void RailTrail(float[] start, float[] end)
        {
            float[] move = new float[]{0, 0, 0};
            float[] vec = new float[]{0, 0, 0};
            float len;
            int j;
            cparticle_t p;
            float dec;
            float[] right = new float[]{0, 0, 0};
            float[] up = new float[]{0, 0, 0};
            int i;
            float d, c, s;
            float[] dir = new float[]{0, 0, 0};
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            Math3D.MakeNormalVectors(vec, right, up);
            for (i = 0; i < len; i++)
            {
                p = new cparticle_t();
                p.time = Time();
                Math3D.VectorClear(p.accel);
                d = i * 0.1F;
                c = (float)Math.Cos(d);
                s = (float)Math.Sin(d);
                Math3D.VectorScale(right, c, dir);
                Math3D.VectorMA(dir, s, up, dir);
                p.alpha = 1F;
                p.alphavel = -1F / (1 + ( float ) Globals.rnd.NextDouble() * 0.2F);
                p.color = 0x74 + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + dir[j] * 3;
                    p.vel[j] = dir[j] * 6;
                }

                Math3D.VectorAdd(move, vec, move);
                active_particles.Add(p);
            }

            dec = 0.75F;
            Math3D.VectorScale(vec, dec, vec);
            Math3D.VectorCopy(start, move);
            while (len > 0)
            {
                len -= dec;
                p = new cparticle_t();
                p.time = Time();
                Math3D.VectorClear(p.accel);
                p.alpha = 1F;
                p.alphavel = -1F / (0.6F + ( float ) Globals.rnd.NextDouble() * 0.2F);
                p.color = 0x0 + Lib.Rand() & 15;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + Lib.Crand() * 3;
                    p.vel[j] = Lib.Crand() * 3;
                    p.accel[j] = 0;
                }

                Math3D.VectorAdd(move, vec, move);
                active_particles.Add(p);
            }
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

        static xcommand_t togglemouse = new Anonymousxcommand_t9(this);
        private sealed class Anonymousxcommand_t9 : xcommand_t
        {
            public Anonymousxcommand_t9(TestMap parent)
            {
                this.parent = parent;
            }

            private readonly TestMap parent;
            public override void Execute()
            {
                IN.ToggleMouse();
            }
        }

        int r_numparticles = 0;
        public virtual void AddParticle(float[] org, int color, float alpha)
        {
            if (r_numparticles >= Defines.MAX_PARTICLES)
                return;
            int i = r_numparticles++;
            int c = particle_t.colorTable[color];
            c |= (int)(alpha * 255) << 24;
            particle_t.colorArray.Put(i, c);
            i *= 3;
            SingleBuffer vertexBuf = particle_t.vertexArray;
            vertexBuf.Put(i++, org[0]);
            vertexBuf.Put(i++, org[1]);
            vertexBuf.Put(i++, org[2]);
        }
    }
}
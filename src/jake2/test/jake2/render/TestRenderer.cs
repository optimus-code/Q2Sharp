using J2N.IO;
using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Q2Sharp.Render
{
    public class TestRenderer
    {
        String[] args;
        Irefexport_t re;
        viddef_t viddef;
        int framecount = 0;
        static int testnr = 0;
        public TestRenderer(String[] args)
        {
            this.args = args;
        }

        public static void Main(String[] args)
        {
            TestRenderer test = new TestRenderer(args);
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
            string DRIVER = "jsr231";
            Qcommon.Init(new string []{"TestRenderer", "+set", "gl_mode", "6", "+set", "vid_fullscreen", "0", "+set", "vid_ref", DRIVER});
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
            Cmd.AddCommand("nexttest", nexttest);
            Cbuf.AddText("bind n nexttest");
            Cbuf.Execute();
            Globals.cls.key_dest = Defines.key_game;
            Globals.cls.state = Defines.ca_active;
        }

        float fps = 0F;
        long start = 0;
        public virtual void UpdateScreen()
        {
            re.BeginFrame(0F);
            viddef = Globals.viddef;
            re.DrawStretchPic(0, 0, viddef.GetWidth(), viddef.GetHeight(), "conback");
            if (framecount % 500 == 0)
            {
                long time = System.CurrentTimeMillis();
                fps = 500000F / (time - start);
                start = time;
            }

            string text = ((int)(fps + 0.5F)) + " fps";
            for (int i = 0; i < text.Length; i++)
            {
                re.DrawChar(10 + 8 * i, viddef.GetHeight() / 2, (int)text[i]);
            }

            re.DrawGetPicSize(out var wal, "/textures/e1u1/basemap.wal");
            re.DrawPic(0, viddef.GetHeight() - wal.Height, "/textures/e1u1/basemap.wal");
            switch (testnr)

            {
                case 0:
                    TestParticles();
                    break;
                case 1:
                    TestModel();
                    break;
                case 2:
                    TestSprites();
                    break;
                case 3:
                    TestBeam();
                    break;
            }

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
                try
                {
                    Thread.Sleep(5);
                }
                catch (Exception e)
                {
                }
            }
        }

        private sealed class Anonymousxcommand_t : xcommand_t
        {
            public Anonymousxcommand_t(TestRenderer parent)
            {
                this.parent = parent;
            }

            private readonly TestRenderer parent;
            public override void Execute()
            {
                parent.UpdateScreen();
            }
        }

        private int yaw = 0;
        private void TestModel()
        {
            refdef_t refdef = new refdef_t();
            refdef.x = viddef.GetWidth() / 2;
            refdef.y = viddef.GetHeight() / 2 - 72;
            refdef.width = 144 * 2;
            refdef.height = 168 * 2;
            refdef.fov_x = 40;
            refdef.fov_y = Math3D.CalcFov(refdef.fov_x, refdef.width, refdef.height);
            refdef.time = 1F * 0.001F;
            entity_t entity = new entity_t();
            string modelName = "players/female/tris.md2";
            string modelSkin = "players/female/athena.pcx";
            string modelImage = "/players/female/athena_i.pcx";
            string modelImage1 = "/players/female/brianna_i.pcx";
            string modelImage2 = "/players/female/cobalt_i.pcx";
            string modelImage3 = "/players/female/lotus_i.pcx";
            entity.model = re.RegisterModel(modelName);
            DrawString(refdef.x, refdef.y - 20, (entity.model != null) ? modelName : "DEBUG: NullModel");
            entity.skin = re.RegisterSkin(modelSkin);
            entity.flags = Defines.RF_FULLBRIGHT;
            entity.origin[0] = 80;
            entity.origin[1] = 0;
            entity.origin[2] = 0;
            Math3D.VectorCopy(entity.origin, entity.oldorigin);
            entity.frame = (framecount / 3) % ((qfiles.dmdl_t)entity.model.extradata).num_frames;
            entity.oldframe = 0;
            entity.backlerp = 0F;
            yaw += KBD.mx;
            KBD.mx = 0;
            if (yaw > 360)
                yaw -= 360;
            if (yaw < 0)
                yaw += 360;
            entity.angles[1] = yaw;
            refdef.areabits = null;
            refdef.num_entities = 1;
            refdef.entities = new entity_t[]{entity};
            refdef.lightstyles = null;
            refdef.rdflags = Defines.RDF_NOWORLDMODEL;
            M_DrawTextBox((int)((refdef.x) * (320F / viddef.GetWidth()) - 8), (int)((viddef.GetHeight() / 2) * (240F / viddef.GetHeight()) - 77), refdef.width / 8, refdef.height / 8);
            refdef.height += 4;
            re.RenderFrame(refdef);
            re.DrawPic(refdef.x - 80, refdef.y, modelImage);
            re.DrawPic(refdef.x - 80, refdef.y + 47, modelImage1);
            re.DrawPic(refdef.x - 80, refdef.y + 94, modelImage2);
            re.DrawPic(refdef.x - 80, refdef.y + 141, modelImage3);
        }

        private String[] sprites = new[]{"sprites/s_bfg1.sp2", "sprites/s_bfg2.sp2", "sprites/s_bfg3.sp2", "sprites/s_explod.sp2", "sprites/s_explo2.sp2", "sprites/s_explo3.sp2", "sprites/s_flash.sp2", "sprites/s_bubble.sp2"};
        private int spriteCount = 0;
        private bool loading = true;
        private void TestSprites()
        {
            if (loading)
            {
                re.DrawPic(viddef.GetWidth() / 2 - 50, viddef.GetHeight() / 2, "loading");
                string name = sprites[spriteCount];
                DrawString(viddef.GetWidth() / 2 - 50, viddef.GetHeight() / 2 + 50, name);
                re.RegisterModel(name);
                loading = ++spriteCount < sprites.Length;
                return;
            }

            refdef_t refdef = new refdef_t();
            refdef.x = viddef.GetWidth() / 2;
            refdef.y = viddef.GetHeight() / 2 - 72;
            refdef.width = 144 * 2;
            refdef.height = 168 * 2;
            refdef.fov_x = 40;
            refdef.fov_y = Math3D.CalcFov(refdef.fov_x, refdef.width, refdef.height);
            refdef.time = 1F * 0.001F;
            entity_t entity = new entity_t();
            string modelName = sprites[(framecount / 30) % sprites.Length];
            DrawString(refdef.x, refdef.y - 20, modelName);
            entity.model = re.RegisterModel(modelName);
            entity.flags = Defines.RF_FULLBRIGHT;
            entity.origin[0] = 80 - (framecount % 200) + 200;
            entity.origin[1] = 0 + (float)(40 * Math.Sin(Math.ToRadians(framecount)));
            entity.origin[2] = 0 + 20;
            Math3D.VectorCopy(entity.origin, entity.oldorigin);
            entity.frame = framecount / 2;
            entity.oldframe = 0;
            entity.backlerp = 0F;
            refdef.areabits = null;
            refdef.num_entities = 1;
            refdef.entities = new entity_t{entity};
            refdef.lightstyles = null;
            refdef.rdflags = Defines.RDF_NOWORLDMODEL;
            M_DrawTextBox((int)((refdef.x) * (320F / viddef.GetWidth()) - 8), (int)((viddef.GetHeight() / 2) * (240F / viddef.GetHeight()) - 77), refdef.width / 8, refdef.height / 8);
            refdef.height += 4;
            re.RenderFrame(refdef);
        }

        private void TestBeam()
        {
            refdef_t refdef = new refdef_t();
            refdef.x = viddef.GetWidth() / 2;
            refdef.y = viddef.GetHeight() / 2 - 72;
            refdef.width = 144 * 2;
            refdef.height = 168 * 2;
            refdef.fov_x = 40;
            refdef.fov_y = Math3D.CalcFov(refdef.fov_x, refdef.width, refdef.height);
            refdef.time = 1F * 0.001F;
            entity_t entity = new entity_t();
            DrawString(refdef.x, refdef.y - 20, "Beam Test");
            entity.flags = Defines.RF_BEAM;
            entity.origin[0] = 200;
            entity.origin[1] = 0 + (float)(80 * Math.Sin(4 * Math.ToRadians(framecount)));
            entity.origin[2] = 20 + (float)(40 * Math.Cos(4 * Math.ToRadians(framecount)));
            entity.oldorigin[0] = 20;
            entity.oldorigin[1] = 0;
            entity.oldorigin[2] = -20;
            entity.frame = 3;
            entity.oldframe = 0;
            entity.backlerp = 0F;
            entity.alpha = 0.6F;
            int[] color = new[]{0xd0, 0xd1, 0xe0, 0xb0};
            entity.skinnum = color[framecount / 2 % 4];
            entity.model = null;
            refdef.areabits = null;
            refdef.num_entities = 1;
            refdef.entities = new entity_t[]{entity};
            refdef.lightstyles = null;
            refdef.rdflags = Defines.RDF_NOWORLDMODEL;
            M_DrawTextBox((int)((refdef.x) * (320F / viddef.GetWidth()) - 8), (int)((viddef.GetHeight() / 2) * (240F / viddef.GetHeight()) - 77), refdef.width / 8, refdef.height / 8);
            refdef.height += 4;
            re.RenderFrame(refdef);
        }

        private LinkedList<cparticle_t> active_particles = new LinkedList<cparticle_t>();
        private bool explode = false;
        private float[] target;
        private void TestParticles()
        {
            r_numparticles = 0;
            if (active_particles.Count == 0)
            {
                if (explode)
                    Explosion(target);
                else
                {
                    target = new float[] {150 + Lib.Crand() * 80, Lib.Crand() * 40, Lib.Crand() * 40};
                    RailTrail(new float[] {30, -20, -20}, target);
                }

                explode = !explode;
            }

            refdef_t refdef = new refdef_t();
            refdef.x = viddef.GetWidth() / 2;
            refdef.y = viddef.GetHeight() / 2 - 72;
            refdef.width = 400;
            refdef.height = 400;
            refdef.fov_x = 50;
            refdef.fov_y = Math3D.CalcFov(refdef.fov_x, refdef.width, refdef.height);
            refdef.time = 1F * 0.001F;
            AnimateParticles();
            DrawString(refdef.x, refdef.y - 20, "active particles: " + r_numparticles);
            refdef.num_particles = r_numparticles;
            refdef.areabits = null;
            refdef.num_entities = 0;
            refdef.entities = null;
            refdef.lightstyles = null;
            refdef.rdflags = Defines.RDF_NOWORLDMODEL;
            M_DrawTextBox((int)((refdef.x) * (320F / viddef.GetWidth()) - 8), (int)((viddef.GetHeight() / 2) * (240F / viddef.GetHeight()) - 77), refdef.width / 8, refdef.height / 8);
            refdef.height += 4;
            re.RenderFrame(refdef);
        }

        private void DrawString(int x, int y, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                re.DrawChar(x + 8 * i, y, (int)text[i]);
            }
        }

        private void M_DrawTextBox(int x, int y, int width, int lines)
        {
            int cx, cy;
            int n;
            cx = x;
            cy = y;
            M_DrawCharacter(cx, cy, 1);
            for (n = 0; n < lines; n++)
            {
                cy += 8;
                M_DrawCharacter(cx, cy, 4);
            }

            M_DrawCharacter(cx, cy + 8, 7);
            cx += 8;
            while (width > 0)
            {
                cy = y;
                M_DrawCharacter(cx, cy, 2);
                for (n = 0; n < lines; n++)
                {
                    cy += 8;
                    M_DrawCharacter(cx, cy, 5);
                }

                M_DrawCharacter(cx, cy + 8, 8);
                width -= 1;
                cx += 8;
            }

            cy = y;
            M_DrawCharacter(cx, cy, 3);
            for (n = 0; n < lines; n++)
            {
                cy += 8;
                M_DrawCharacter(cx, cy, 6);
            }

            M_DrawCharacter(cx, cy + 8, 9);
        }

        private void M_DrawCharacter(int cx, int cy, int num)
        {
            re.DrawChar(cx + ((viddef.GetWidth() - 320) >> 1), cy + ((viddef.GetHeight() - 240) >> 1), num);
        }

        long endtime;
        private void Explosion(float[] org)
        {
            cparticle_t p;
            for (int i = 0; i < 256; i++)
            {
                p = new cparticle_t();
                p.time = Time() * 1F;
                p.color = 223 - (Lib.Rand() & 7);
                for (int j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + (float)(Lib.Rand() % 32) - 16;
                    p.vel[j] = (float)(Lib.Rand() % 384) - 192;
                }

                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -0.8F / (0.5F + ( float ) Globals.rnd.NextDouble() * 0.3F);
                active_particles.AddLast(p);
            }
        }

        static readonly float INSTANT_PARTICLE = -10000F;
        static readonly float PARTICLE_GRAVITY = 40F;
        private void AnimateParticles()
        {
            cparticle_t p;
            float alpha;
            float time, time2;
            float[] org = new float[]{0, 0, 0};
            int color;
            time = 0F;
            for (Iterator it = active_particles.iterator(); it.HasNext();)
            {
                p = (cparticle_t)it.Next();
                if (p.alphavel != INSTANT_PARTICLE)
                {
                    time = (Time() - p.time) * 0.001F;
                    alpha = p.alpha + time * p.alphavel;
                    if (alpha <= 0)
                    {
                        it.Remove();
                        continue;
                    }
                }
                else
                {
                    alpha = p.alpha;
                }

                if (alpha > 1)
                    alpha = 1;
                color = (int)p.color;
                time2 = time * time;
                org[0] = p.org[0] + p.vel[0] * time + p.accel[0] * time2;
                org[1] = p.org[1] + p.vel[1] * time + p.accel[1] * time2;
                org[2] = p.org[2] + p.vel[2] * time + p.accel[2] * time2;
                AddParticle(org, color, alpha);
                if (p.alphavel == INSTANT_PARTICLE)
                {
                    p.alphavel = 0F;
                    p.alpha = 0F;
                }
            }
        }

        private void Heatbeam(float[] start, float[] forward)
        {
            float[] v_up = new float[]{0, 0, 10};
            float[] v_right = new float[]{0, 10, 0};
            float[] move = new float[]{0, 0, 0};
            float[] vec = new float[]{0, 0, 0};
            float len;
            int j;
            cparticle_t p;
            float[] right = new float[]{0, 0, 0};
            float[] up = new float[]{0, 0, 0};
            int i;
            float c, s;
            float[] dir = new float[]{0, 0, 0};
            float ltime;
            float step = 32F, rstep;
            float start_pt;
            float rot;
            float variance;
            float[] end = new float[]{0, 0, 0};
            Math3D.VectorMA(start, 4096, forward, end);
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            Math3D.VectorCopy(v_right, right);
            Math3D.VectorCopy(v_up, up);
            Math3D.VectorMA(move, -0.5F, right, move);
            Math3D.VectorMA(move, -0.5F, up, move);
            ltime = (float)Time() / 1000F;
            start_pt = (ltime * 96F) % step;
            Math3D.VectorMA(move, start_pt, vec, move);
            Math3D.VectorScale(vec, step, vec);
            rstep = (float)Math.PI / 10F;
            for (i = (int)start_pt; i < len; i += step)
            {
                if (i > step * 5)
                    break;
                for (rot = 0; rot < Math.PI * 2; rot += rstep)
                {
                    p = new cparticle_t();
                    p.time = Time();
                    Math3D.VectorClear(p.accel);
                    variance = 0.5F;
                    c = (float)Math.Cos(rot) * variance;
                    s = (float)Math.Sin(rot) * variance;
                    if (i < 10)
                    {
                        Math3D.VectorScale(right, c * (i / 10F), dir);
                        Math3D.VectorMA(dir, s * (i / 10F), up, dir);
                    }
                    else
                    {
                        Math3D.VectorScale(right, c, dir);
                        Math3D.VectorMA(dir, s, up, dir);
                    }

                    p.alpha = 0.8F;
                    p.alphavel = -1000F;
                    p.color = 0x74 - (Lib.Rand() & 7);
                    for (j = 0; j < 3; j++)
                    {
                        p.org[j] = move[j] + dir[j] * 3;
                        p.vel[j] = 0;
                    }

                    active_particles.Add(p);
                }

                Math3D.VectorAdd(move, vec, move);
            }
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
                active_particles.AddLast(p);
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
                active_particles.AddLast(p);
            }
        }

        private int Time()
        {
            return (int)(System.CurrentTimeMillis() - startTime);
        }

        static xcommand_t nexttest = new Anonymousxcommand_t1(this);
        private sealed class Anonymousxcommand_t1 : xcommand_t
        {
            public Anonymousxcommand_t1(TestRenderer parent)
            {
                this.parent = parent;
            }

            private readonly TestRenderer parent;
            public override void Execute()
            {
                testnr++;
                testnr = testnr % 3;
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
            vertexBuf.Put(i, org[0]);
            vertexBuf.Put(i + 1, org[1]);
            vertexBuf.Put(i + 2, org[2]);
        }
    }
}
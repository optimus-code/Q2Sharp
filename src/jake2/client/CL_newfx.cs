using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public class CL_newfx
    {
        public static void Flashlight(int ent, float[] pos)
        {
            CL_fx.cdlight_t dl;
            dl = CL_fx.AllocDlight(ent);
            Math3D.VectorCopy(pos, dl.origin);
            dl.radius = 400;
            dl.minlight = 250;
            dl.die = Globals.cl.time + 100;
            dl.color[0] = 1;
            dl.color[1] = 1;
            dl.color[2] = 1;
        }

        public static void ColorFlash(float[] pos, int ent, int intensity, float r, float g, float b)
        {
            CL_fx.cdlight_t dl;
            if ((Globals.vidref_val == Defines.VIDREF_SOFT) && ((r < 0) || (g < 0) || (b < 0)))
            {
                intensity = -intensity;
                r = -r;
                g = -g;
                b = -b;
            }

            dl = CL_fx.AllocDlight(ent);
            Math3D.VectorCopy(pos, dl.origin);
            dl.radius = intensity;
            dl.minlight = 250;
            dl.die = Globals.cl.time + 100;
            dl.color[0] = r;
            dl.color[1] = g;
            dl.color[2] = b;
        }

        private static readonly float[] move = new float[]{0, 0, 0};
        private static readonly float[] vec = new float[]{0, 0, 0};
        private static readonly float[] right = new float[]{0, 0, 0};
        private static readonly float[] up = new float[]{0, 0, 0};
        public static void DebugTrail(float[] start, float[] end)
        {
            float len;
            cparticle_t p;
            float dec;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            Math3D.MakeNormalVectors(vec, right, up);
            dec = 3;
            Math3D.VectorScale(vec, dec, vec);
            Math3D.VectorCopy(start, move);
            while (len > 0)
            {
                len -= dec;
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                p.time = Globals.cl.time;
                Math3D.VectorClear(p.accel);
                Math3D.VectorClear(p.vel);
                p.alpha = 1F;
                p.alphavel = -0.1F;
                p.color = 0x74 + (Lib.Rand() & 7);
                Math3D.VectorCopy(move, p.org);
                Math3D.VectorAdd(move, vec, move);
            }
        }

        public static void ForceWall(float[] start, float[] end, int color)
        {
            float len;
            int j;
            cparticle_t p;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            Math3D.VectorScale(vec, 4, vec);
            while (len > 0)
            {
                len -= 4;
                if (CL_fx.free_particles == null)
                    return;
                if ((float)Globals.rnd.NextDouble() > 0.3)
                {
                    p = CL_fx.free_particles;
                    CL_fx.free_particles = p.next;
                    p.next = CL_fx.active_particles;
                    CL_fx.active_particles = p;
                    Math3D.VectorClear(p.accel);
                    p.time = Globals.cl.time;
                    p.alpha = 1F;
                    p.alphavel = -1F / (3F + (float)Globals.rnd.NextDouble() * 0.5F);
                    p.color = color;
                    for (j = 0; j < 3; j++)
                    {
                        p.org[j] = move[j] + Lib.Crand() * 3;
                        p.accel[j] = 0;
                    }

                    p.vel[0] = 0;
                    p.vel[1] = 0;
                    p.vel[2] = -40 - (Lib.Crand() * 10);
                }

                Math3D.VectorAdd(move, vec, move);
            }
        }

        public static void BubbleTrail2(float[] start, float[] end, int dist)
        {
            float len;
            int i, j;
            cparticle_t p;
            int dec;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            dec = dist;
            Math3D.VectorScale(vec, dec, vec);
            for (i = 0; i < len; i += dec)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = -1F / (1 + (float)Globals.rnd.NextDouble() * 0.1F);
                p.color = 4 + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + Lib.Crand() * 2;
                    p.vel[j] = Lib.Crand() * 10;
                }

                p.org[2] -= 4;
                p.vel[2] += 20;
                Math3D.VectorAdd(move, vec, move);
            }
        }

        private static readonly float[] dir = new float[]{0, 0, 0};
        private static readonly float[] end = new float[]{0, 0, 0};
        public static void Heatbeam(float[] start, float[] forward)
        {
            float len;
            int j;
            cparticle_t p;
            int i;
            float c, s;
            float ltime;
            float step = 32F, rstep;
            float start_pt;
            float rot;
            float variance;
            Math3D.VectorMA(start, 4096, forward, end);
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            Math3D.VectorCopy(Globals.cl.v_right, right);
            Math3D.VectorCopy(Globals.cl.v_up, up);
            if (Globals.vidref_val == Defines.VIDREF_GL)
            {
                Math3D.VectorMA(move, -0.5F, right, move);
                Math3D.VectorMA(move, -0.5F, up, move);
            }

            ltime = (float)Globals.cl.time / 1000F;
            start_pt = ltime * 96F % step;
            Math3D.VectorMA(move, start_pt, vec, move);
            Math3D.VectorScale(vec, step, vec);
            rstep = (float)(Math.PI / 10);
            float M_PI2 = (float)(Math.PI * 2);
            for (i = (int)start_pt; i < len; i += ( int ) step)
            {
                if (i > step * 5)
                    break;
                for (rot = 0; rot < M_PI2; rot += rstep)
                {
                    if (CL_fx.free_particles == null)
                        return;
                    p = CL_fx.free_particles;
                    CL_fx.free_particles = p.next;
                    p.next = CL_fx.active_particles;
                    CL_fx.active_particles = p;
                    p.time = Globals.cl.time;
                    Math3D.VectorClear(p.accel);
                    variance = 0.5F;
                    c = (float)(Math.Cos(rot) * variance);
                    s = (float)(Math.Sin(rot) * variance);
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

                    p.alpha = 0.5F;
                    p.alphavel = -1000F;
                    p.color = 223 - (Lib.Rand() & 7);
                    for (j = 0; j < 3; j++)
                    {
                        p.org[j] = move[j] + dir[j] * 3;
                        p.vel[j] = 0;
                    }
                }

                Math3D.VectorAdd(move, vec, move);
            }
        }

        private static readonly float[] r = new float[]{0, 0, 0};
        private static readonly float[] u = new float[]{0, 0, 0};
        public static void ParticleSteamEffect(float[] org, float[] dir, int color, int count, int magnitude)
        {
            int i, j;
            cparticle_t p;
            float d;
            Math3D.MakeNormalVectors(dir, r, u);
            for (i = 0; i < count; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                p.time = Globals.cl.time;
                p.color = color + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + magnitude * 0.1F * Lib.Crand();
                }

                Math3D.VectorScale(dir, magnitude, p.vel);
                d = Lib.Crand() * magnitude / 3;
                Math3D.VectorMA(p.vel, d, r, p.vel);
                d = Lib.Crand() * magnitude / 3;
                Math3D.VectorMA(p.vel, d, u, p.vel);
                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -CL_fx.PARTICLE_GRAVITY / 2;
                p.alpha = 1F;
                p.alphavel = -1F / (0.5F + (float)Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void ParticleSteamEffect2(cl_sustain_t self)
        {
            int i, j;
            cparticle_t p;
            float d;
            Math3D.VectorCopy(self.dir, dir);
            Math3D.MakeNormalVectors(dir, r, u);
            for (i = 0; i < self.count; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                p.time = Globals.cl.time;
                p.color = self.color + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = self.org[j] + self.magnitude * 0.1F * Lib.Crand();
                }

                Math3D.VectorScale(dir, self.magnitude, p.vel);
                d = Lib.Crand() * self.magnitude / 3;
                Math3D.VectorMA(p.vel, d, r, p.vel);
                d = Lib.Crand() * self.magnitude / 3;
                Math3D.VectorMA(p.vel, d, u, p.vel);
                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -CL_fx.PARTICLE_GRAVITY / 2;
                p.alpha = 1F;
                p.alphavel = -1F / (0.5F + (float)Globals.rnd.NextDouble() * 0.3F);
            }

            self.nextthink += self.thinkinterval;
        }

        private static readonly float[] forward = new float[]{0, 0, 0};
        private static readonly float[] angle_dir = new float[]{0, 0, 0};
        public static void TrackerTrail(float[] start, float[] end, int particleColor)
        {
            float len;
            cparticle_t p;
            int dec;
            float dist;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            Math3D.VectorCopy(vec, forward);
            Math3D.Vectoangles(forward, angle_dir);
            Math3D.AngleVectors(angle_dir, forward, right, up);
            dec = 3;
            Math3D.VectorScale(vec, 3, vec);
            while (len > 0)
            {
                len -= dec;
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = -2F;
                p.color = particleColor;
                dist = Math3D.DotProduct(move, forward);
                Math3D.VectorMA(move, (float)(8 * Math.Cos(dist)), up, p.org);
                for (int j = 0; j < 3; j++)
                {
                    p.vel[j] = 0;
                    p.accel[j] = 0;
                }

                p.vel[2] = 5;
                Math3D.VectorAdd(move, vec, move);
            }
        }

        public static void Tracker_Shell(float[] origin)
        {
            cparticle_t p;
            for (int i = 0; i < 300; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = CL_fx.INSTANT_PARTICLE;
                p.color = 0;
                dir[0] = Lib.Crand();
                dir[1] = Lib.Crand();
                dir[2] = Lib.Crand();
                Math3D.VectorNormalize(dir);
                Math3D.VectorMA(origin, 40, dir, p.org);
            }
        }

        public static void MonsterPlasma_Shell(float[] origin)
        {
            cparticle_t p;
            for (int i = 0; i < 40; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = CL_fx.INSTANT_PARTICLE;
                p.color = 0xe0;
                dir[0] = Lib.Crand();
                dir[1] = Lib.Crand();
                dir[2] = Lib.Crand();
                Math3D.VectorNormalize(dir);
                Math3D.VectorMA(origin, 10, dir, p.org);
            }
        }

        private static int[] wb_colortable = new[]{2 * 8, 13 * 8, 21 * 8, 18 * 8};
        public static void Widowbeamout(cl_sustain_t self)
        {
            int i;
            cparticle_t p;
            float ratio;
            ratio = 1F - (((float)self.endtime - (float)Globals.cl.time) / 2100F);
            for (i = 0; i < 300; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = CL_fx.INSTANT_PARTICLE;
                p.color = wb_colortable[Lib.Rand() & 3];
                dir[0] = Lib.Crand();
                dir[1] = Lib.Crand();
                dir[2] = Lib.Crand();
                Math3D.VectorNormalize(dir);
                Math3D.VectorMA(self.org, (45F * ratio), dir, p.org);
            }
        }

        private static int[] nb_colortable = new[]{110, 112, 114, 116};
        public static void Nukeblast(cl_sustain_t self)
        {
            int i;
            cparticle_t p;
            float ratio;
            ratio = 1F - (((float)self.endtime - (float)Globals.cl.time) / 1000F);
            for (i = 0; i < 700; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = CL_fx.INSTANT_PARTICLE;
                p.color = nb_colortable[Lib.Rand() & 3];
                dir[0] = Lib.Crand();
                dir[1] = Lib.Crand();
                dir[2] = Lib.Crand();
                Math3D.VectorNormalize(dir);
                Math3D.VectorMA(self.org, (200F * ratio), dir, p.org);
            }
        }

        private static int[] ws_colortable = new[]{2 * 8, 13 * 8, 21 * 8, 18 * 8};
        public static void WidowSplash(float[] org)
        {
            int i;
            cparticle_t p;
            for (i = 0; i < 256; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                p.time = Globals.cl.time;
                p.color = ws_colortable[Lib.Rand() & 3];
                dir[0] = Lib.Crand();
                dir[1] = Lib.Crand();
                dir[2] = Lib.Crand();
                Math3D.VectorNormalize(dir);
                Math3D.VectorMA(org, 45F, dir, p.org);
                Math3D.VectorMA(Globals.vec3_origin, 40F, dir, p.vel);
                p.accel[0] = p.accel[1] = 0;
                p.alpha = 1F;
                p.alphavel = -0.8F / (0.5F + (float)Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void TagTrail(float[] start, float[] end, float color)
        {
            float len;
            int j;
            cparticle_t p;
            int dec;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            dec = 5;
            Math3D.VectorScale(vec, 5, vec);
            while (len >= 0)
            {
                len -= dec;
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = -1F / (0.8F + (float)Globals.rnd.NextDouble() * 0.2F);
                p.color = color;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + Lib.Crand() * 16;
                    p.vel[j] = Lib.Crand() * 5;
                    p.accel[j] = 0;
                }

                Math3D.VectorAdd(move, vec, move);
            }
        }

        public static void ColorExplosionParticles(float[] org, int color, int run)
        {
            int i, j;
            cparticle_t p;
            for (i = 0; i < 128; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                p.time = Globals.cl.time;
                p.color = color + (Lib.Rand() % run);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + ((Lib.Rand() % 32) - 16);
                    p.vel[j] = (Lib.Rand() % 256) - 128;
                }

                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -CL_fx.PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -0.4F / (0.6F + (float)Globals.rnd.NextDouble() * 0.2F);
            }
        }

        public static void ParticleSmokeEffect(float[] org, float[] dir, int color, int count, int magnitude)
        {
            int i, j;
            cparticle_t p;
            float d;
            Math3D.MakeNormalVectors(dir, r, u);
            for (i = 0; i < count; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                p.time = Globals.cl.time;
                p.color = color + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + magnitude * 0.1F * Lib.Crand();
                }

                Math3D.VectorScale(dir, magnitude, p.vel);
                d = Lib.Crand() * magnitude / 3;
                Math3D.VectorMA(p.vel, d, r, p.vel);
                d = Lib.Crand() * magnitude / 3;
                Math3D.VectorMA(p.vel, d, u, p.vel);
                p.accel[0] = p.accel[1] = p.accel[2] = 0;
                p.alpha = 1F;
                p.alphavel = -1F / (0.5F + (float)Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void BlasterParticles2(float[] org, float[] dir, long color)
        {
            int i, j;
            cparticle_t p;
            float d;
            int count;
            count = 40;
            for (i = 0; i < count; i++)
            {
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                p.time = Globals.cl.time;
                p.color = color + (Lib.Rand() & 7);
                d = Lib.Rand() & 15;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + ((Lib.Rand() & 7) - 4) + d * dir[j];
                    p.vel[j] = dir[j] * 30 + Lib.Crand() * 40;
                }

                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -CL_fx.PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -1F / (0.5F + (float)Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void BlasterTrail2(float[] start, float[] end)
        {
            float len;
            int j;
            cparticle_t p;
            int dec;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            dec = 5;
            Math3D.VectorScale(vec, 5, vec);
            while (len > 0)
            {
                len -= dec;
                if (CL_fx.free_particles == null)
                    return;
                p = CL_fx.free_particles;
                CL_fx.free_particles = p.next;
                p.next = CL_fx.active_particles;
                CL_fx.active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = -1F / (0.3F + (float)Globals.rnd.NextDouble() * 0.2F);
                p.color = 0xd0;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + Lib.Crand();
                    p.vel[j] = Lib.Crand() * 5;
                    p.accel[j] = 0;
                }

                Math3D.VectorAdd(move, vec, move);
            }
        }
    }
}
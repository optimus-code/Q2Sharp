using Q2Sharp.Game;
using Q2Sharp.Game.Monsters;
using Q2Sharp.Qcommon;
using Q2Sharp.Sound;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class CL_fx
    {
        public class cdlight_t
        {
            public int key;
            public float[] color = new float[]{0, 0, 0};
            public float[] origin = new float[]{0, 0, 0};
            public float radius;
            public float die;
            public float minlight;
            public virtual void Clear()
            {
                radius = minlight = color[0] = color[1] = color[2] = 0;
            }
        }

        static cparticle_t[] particles = new cparticle_t[Defines.MAX_PARTICLES];
        static cdlight_t[] cl_dlights = new cdlight_t[Defines.MAX_DLIGHTS];
        static CL_fx()
        {
            for (int i = 0; i < particles.Length; i++)
                particles[i] = new cparticle_t();

            for ( int i = 0; i < cl_lightstyle.Length; i++ )
            {
                cl_lightstyle[i] = new clightstyle_t();
            }

            for ( int i = 0; i < cl_dlights.Length; i++ )
                cl_dlights[i] = new cdlight_t();
        }

        public static int cl_numparticles = Defines.MAX_PARTICLES;
        public static readonly float INSTANT_PARTICLE = -10000F;
        public static float[][] avelocities = Lib.CreateJaggedArray<float[][]>( Defines.NUMVERTEXNORMALS, 3 );
        public static clightstyle_t[] cl_lightstyle = new clightstyle_t[Defines.MAX_LIGHTSTYLES];
        public static int lastofs;
        public class clightstyle_t
        {
            public int length;
            public float[] value = new float[3];
            public float[] map = new float[Defines.MAX_QPATH];
            public virtual void Clear()
            {
                value[0] = value[1] = value[2] = length = 0;
                for (int i = 0; i < map.Length; i++)
                    map[i] = 0F;
            }
        }

        static void ClearDlights()
        {
            for (int i = 0; i < cl_dlights.Length; i++)
            {
                cl_dlights[i].Clear();
            }
        }

        static void ClearLightStyles()
        {
            for (int i = 0; i < cl_lightstyle.Length; i++)
                cl_lightstyle[i].Clear();
            lastofs = -1;
        }

        public static void RunLightStyles()
        {
            clightstyle_t ls;
            int ofs = Globals.cl.time / 100;
            if (ofs == lastofs)
                return;
            lastofs = ofs;
            for (int i = 0; i < cl_lightstyle.Length; i++)
            {
                ls = cl_lightstyle[i];
                if (ls.length == 0)
                {
                    ls.value[0] = ls.value[1] = ls.value[2] = 1F;
                    continue;
                }

                if (ls.length == 1)
                    ls.value[0] = ls.value[1] = ls.value[2] = ls.map[0];
                else
                    ls.value[0] = ls.value[1] = ls.value[2] = ls.map[ofs % ls.length];
            }
        }

        public static void SetLightstyle(int i)
        {
            string s;
            int j, k;
            s = Globals.cl.configstrings[i + Defines.CS_LIGHTS];
            j = s.Length;
            if (j >= Defines.MAX_QPATH)
                Com.Error(Defines.ERR_DROP, "svc_lightstyle length=" + j);
            cl_lightstyle[i].length = j;
            for (k = 0; k < j; k++)
                cl_lightstyle[i].map[k] = (float)(s[k] - 'a') / (float)('m' - 'a');
        }

        public static void AddLightStyles()
        {
            clightstyle_t ls;
            for (int i = 0; i < cl_lightstyle.Length; i++)
            {
                ls = cl_lightstyle[i];
                V.AddLightStyle(i, ls.value[0], ls.value[1], ls.value[2]);
            }
        }

        public static cdlight_t AllocDlight(int key)
        {
            int i;
            cdlight_t dl;
            if (key != 0)
            {
                for (i = 0; i < Defines.MAX_DLIGHTS; i++)
                {
                    dl = cl_dlights[i];
                    if (dl.key == key)
                    {
                        dl.Clear();
                        dl.key = key;
                        return dl;
                    }
                }
            }

            for (i = 0; i < Defines.MAX_DLIGHTS; i++)
            {
                dl = cl_dlights[i];
                if (dl.die < Globals.cl.time)
                {
                    dl.Clear();
                    dl.key = key;
                    return dl;
                }
            }

            dl = cl_dlights[0];
            dl.Clear();
            dl.key = key;
            return dl;
        }

        public static void RunDLights()
        {
            cdlight_t dl;
            for (int i = 0; i < Defines.MAX_DLIGHTS; i++)
            {
                dl = cl_dlights[i];
                if (dl.radius == 0F)
                    continue;
                if (dl.die < Globals.cl.time)
                {
                    dl.radius = 0F;
                    return;
                }
            }
        }

        private static readonly float[] fv = new float[]{0, 0, 0};
        private static readonly float[] rv = new float[]{0, 0, 0};
        public static void ParseMuzzleFlash()
        {
            float volume;
            string soundname;
            int i = MSG.ReadShort(Globals.net_message);
            if (i < 1 || i >= Defines.MAX_EDICTS)
                Com.Error(Defines.ERR_DROP, "CL_ParseMuzzleFlash: bad entity");
            int weapon = MSG.ReadByte(Globals.net_message);
            int silenced = weapon & Defines.MZ_SILENCED;
            weapon &= ~Defines.MZ_SILENCED;
            centity_t pl = Globals.cl_entities[i];
            cdlight_t dl = AllocDlight(i);
            Math3D.VectorCopy(pl.current.origin, dl.origin);
            Math3D.AngleVectors(pl.current.angles, fv, rv, null);
            Math3D.VectorMA(dl.origin, 18, fv, dl.origin);
            Math3D.VectorMA(dl.origin, 16, rv, dl.origin);
            if (silenced != 0)
                dl.radius = 100 + (Globals.rnd.Next() & 31);
            else
                dl.radius = 200 + (Globals.rnd.Next() & 31);
            dl.minlight = 32;
            dl.die = Globals.cl.time;
            if (silenced != 0)
                volume = 0.2F;
            else
                volume = 1;
            switch (weapon)

            {
                case Defines.MZ_BLASTER:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/blastf1a.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_BLUEHYPERBLASTER:
                    dl.color[0] = 0;
                    dl.color[1] = 0;
                    dl.color[2] = 1;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/hyprbf1a.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_HYPERBLASTER:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/hyprbf1a.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_MACHINEGUN:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    soundname = "weapons/machgf" + ((Globals.rnd.Next(5)) + 1) + "b.wav";
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound(soundname), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_SHOTGUN:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/shotgf1b.wav"), volume, Defines.ATTN_NORM, 0);
                    S.StartSound(null, i, Defines.CHAN_AUTO, S.RegisterSound("weapons/shotgr1b.wav"), volume, Defines.ATTN_NORM, 0.1F);
                    break;
                case Defines.MZ_SSHOTGUN:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/sshotf1b.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_CHAINGUN1:
                    dl.radius = 200 + (Globals.rnd.Next() & 31);
                    dl.color[0] = 1;
                    dl.color[1] = 0.25F;
                    dl.color[2] = 0;
                    soundname = "weapons/machgf" + ((Globals.rnd.Next(5)) + 1) + "b.wav";
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound(soundname), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_CHAINGUN2:
                    dl.radius = 225 + (Globals.rnd.Next() & 31);
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 0.1F;
                    soundname = "weapons/machgf" + ((Globals.rnd.Next(5)) + 1) + "b.wav";
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound(soundname), volume, Defines.ATTN_NORM, 0);
                    soundname = "weapons/machgf" + ((Globals.rnd.Next(5)) + 1) + "b.wav";
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound(soundname), volume, Defines.ATTN_NORM, 0.05F);
                    break;
                case Defines.MZ_CHAINGUN3:
                    dl.radius = 250 + (Globals.rnd.Next() & 31);
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 0.1F;
                    soundname = "weapons/machgf" + ((Globals.rnd.Next(5)) + 1) + "b.wav";
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound(soundname), volume, Defines.ATTN_NORM, 0);
                    soundname = "weapons/machgf" + ((Globals.rnd.Next(5)) + 1) + "b.wav";
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound(soundname), volume, Defines.ATTN_NORM, 0.033F);
                    soundname = "weapons/machgf" + ((Globals.rnd.Next(5)) + 1) + "b.wav";
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound(soundname), volume, Defines.ATTN_NORM, 0.066F);
                    break;
                case Defines.MZ_RAILGUN:
                    dl.color[0] = 0.5F;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 1F;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/railgf1a.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_ROCKET:
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0.2F;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/rocklf1a.wav"), volume, Defines.ATTN_NORM, 0);
                    S.StartSound(null, i, Defines.CHAN_AUTO, S.RegisterSound("weapons/rocklr1b.wav"), volume, Defines.ATTN_NORM, 0.1F);
                    break;
                case Defines.MZ_GRENADE:
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/grenlf1a.wav"), volume, Defines.ATTN_NORM, 0);
                    S.StartSound(null, i, Defines.CHAN_AUTO, S.RegisterSound("weapons/grenlr1b.wav"), volume, Defines.ATTN_NORM, 0.1F);
                    break;
                case Defines.MZ_BFG:
                    dl.color[0] = 0;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/bfg__f1y.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_LOGIN:
                    dl.color[0] = 0;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 1F;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/grenlf1a.wav"), 1, Defines.ATTN_NORM, 0);
                    LogoutEffect(pl.current.origin, weapon);
                    break;
                case Defines.MZ_LOGOUT:
                    dl.color[0] = 1;
                    dl.color[1] = 0;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 1F;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/grenlf1a.wav"), 1, Defines.ATTN_NORM, 0);
                    LogoutEffect(pl.current.origin, weapon);
                    break;
                case Defines.MZ_RESPAWN:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 1F;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/grenlf1a.wav"), 1, Defines.ATTN_NORM, 0);
                    LogoutEffect(pl.current.origin, weapon);
                    break;
                case Defines.MZ_PHALANX:
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0.5F;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/plasshot.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_IONRIPPER:
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0.5F;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/rippfire.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_ETF_RIFLE:
                    dl.color[0] = 0.9F;
                    dl.color[1] = 0.7F;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/nail1.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_SHOTGUN2:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/shotg2.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_HEATBEAM:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 100;
                    break;
                case Defines.MZ_BLASTER2:
                    dl.color[0] = 0;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/blastf1a.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_TRACKER:
                    dl.color[0] = -1;
                    dl.color[1] = -1;
                    dl.color[2] = -1;
                    S.StartSound(null, i, Defines.CHAN_WEAPON, S.RegisterSound("weapons/disint2.wav"), volume, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ_NUKE1:
                    dl.color[0] = 1;
                    dl.color[1] = 0;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 100;
                    break;
                case Defines.MZ_NUKE2:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 100;
                    break;
                case Defines.MZ_NUKE4:
                    dl.color[0] = 0;
                    dl.color[1] = 0;
                    dl.color[2] = 1;
                    dl.die = Globals.cl.time + 100;
                    break;
                case Defines.MZ_NUKE8:
                    dl.color[0] = 0;
                    dl.color[1] = 1;
                    dl.color[2] = 1;
                    dl.die = Globals.cl.time + 100;
                    break;
            }
        }

        private static readonly float[] origin = new float[]{0, 0, 0};
        private static readonly float[] forward = new float[]{0, 0, 0};
        private static readonly float[] right = new float[]{0, 0, 0};
        public static void ParseMuzzleFlash2()
        {
            string soundname;
            int ent = MSG.ReadShort(Globals.net_message);
            if (ent < 1 || ent >= Defines.MAX_EDICTS)
                Com.Error(Defines.ERR_DROP, "CL_ParseMuzzleFlash2: bad entity");
            int flash_number = MSG.ReadByte(Globals.net_message);
            Math3D.AngleVectors(Globals.cl_entities[ent].current.angles, forward, right, null);
            origin[0] = Globals.cl_entities[ent].current.origin[0] + forward[0] * M_Flash.monster_flash_offset[flash_number][0] + right[0] * M_Flash.monster_flash_offset[flash_number][1];
            origin[1] = Globals.cl_entities[ent].current.origin[1] + forward[1] * M_Flash.monster_flash_offset[flash_number][0] + right[1] * M_Flash.monster_flash_offset[flash_number][1];
            origin[2] = Globals.cl_entities[ent].current.origin[2] + forward[2] * M_Flash.monster_flash_offset[flash_number][0] + right[2] * M_Flash.monster_flash_offset[flash_number][1] + M_Flash.monster_flash_offset[flash_number][2];
            cdlight_t dl = AllocDlight(ent);
            Math3D.VectorCopy(origin, dl.origin);
            dl.radius = 200 + (Globals.rnd.Next() & 31);
            dl.minlight = 32;
            dl.die = Globals.cl.time;
            switch (flash_number)

            {
                case Defines.MZ2_INFANTRY_MACHINEGUN_1:
                case Defines.MZ2_INFANTRY_MACHINEGUN_2:
                case Defines.MZ2_INFANTRY_MACHINEGUN_3:
                case Defines.MZ2_INFANTRY_MACHINEGUN_4:
                case Defines.MZ2_INFANTRY_MACHINEGUN_5:
                case Defines.MZ2_INFANTRY_MACHINEGUN_6:
                case Defines.MZ2_INFANTRY_MACHINEGUN_7:
                case Defines.MZ2_INFANTRY_MACHINEGUN_8:
                case Defines.MZ2_INFANTRY_MACHINEGUN_9:
                case Defines.MZ2_INFANTRY_MACHINEGUN_10:
                case Defines.MZ2_INFANTRY_MACHINEGUN_11:
                case Defines.MZ2_INFANTRY_MACHINEGUN_12:
                case Defines.MZ2_INFANTRY_MACHINEGUN_13:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("infantry/infatck1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_SOLDIER_MACHINEGUN_1:
                case Defines.MZ2_SOLDIER_MACHINEGUN_2:
                case Defines.MZ2_SOLDIER_MACHINEGUN_3:
                case Defines.MZ2_SOLDIER_MACHINEGUN_4:
                case Defines.MZ2_SOLDIER_MACHINEGUN_5:
                case Defines.MZ2_SOLDIER_MACHINEGUN_6:
                case Defines.MZ2_SOLDIER_MACHINEGUN_7:
                case Defines.MZ2_SOLDIER_MACHINEGUN_8:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("soldier/solatck3.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_GUNNER_MACHINEGUN_1:
                case Defines.MZ2_GUNNER_MACHINEGUN_2:
                case Defines.MZ2_GUNNER_MACHINEGUN_3:
                case Defines.MZ2_GUNNER_MACHINEGUN_4:
                case Defines.MZ2_GUNNER_MACHINEGUN_5:
                case Defines.MZ2_GUNNER_MACHINEGUN_6:
                case Defines.MZ2_GUNNER_MACHINEGUN_7:
                case Defines.MZ2_GUNNER_MACHINEGUN_8:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("gunner/gunatck2.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_ACTOR_MACHINEGUN_1:
                case Defines.MZ2_SUPERTANK_MACHINEGUN_1:
                case Defines.MZ2_SUPERTANK_MACHINEGUN_2:
                case Defines.MZ2_SUPERTANK_MACHINEGUN_3:
                case Defines.MZ2_SUPERTANK_MACHINEGUN_4:
                case Defines.MZ2_SUPERTANK_MACHINEGUN_5:
                case Defines.MZ2_SUPERTANK_MACHINEGUN_6:
                case Defines.MZ2_TURRET_MACHINEGUN:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("infantry/infatck1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_BOSS2_MACHINEGUN_L1:
                case Defines.MZ2_BOSS2_MACHINEGUN_L2:
                case Defines.MZ2_BOSS2_MACHINEGUN_L3:
                case Defines.MZ2_BOSS2_MACHINEGUN_L4:
                case Defines.MZ2_BOSS2_MACHINEGUN_L5:
                case Defines.MZ2_CARRIER_MACHINEGUN_L1:
                case Defines.MZ2_CARRIER_MACHINEGUN_L2:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("infantry/infatck1.wav"), 1, Defines.ATTN_NONE, 0);
                    break;
                case Defines.MZ2_SOLDIER_BLASTER_1:
                case Defines.MZ2_SOLDIER_BLASTER_2:
                case Defines.MZ2_SOLDIER_BLASTER_3:
                case Defines.MZ2_SOLDIER_BLASTER_4:
                case Defines.MZ2_SOLDIER_BLASTER_5:
                case Defines.MZ2_SOLDIER_BLASTER_6:
                case Defines.MZ2_SOLDIER_BLASTER_7:
                case Defines.MZ2_SOLDIER_BLASTER_8:
                case Defines.MZ2_TURRET_BLASTER:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("soldier/solatck2.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_FLYER_BLASTER_1:
                case Defines.MZ2_FLYER_BLASTER_2:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("flyer/flyatck3.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_MEDIC_BLASTER_1:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("medic/medatck1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_HOVER_BLASTER_1:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("hover/hovatck1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_FLOAT_BLASTER_1:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("floater/fltatck1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_SOLDIER_SHOTGUN_1:
                case Defines.MZ2_SOLDIER_SHOTGUN_2:
                case Defines.MZ2_SOLDIER_SHOTGUN_3:
                case Defines.MZ2_SOLDIER_SHOTGUN_4:
                case Defines.MZ2_SOLDIER_SHOTGUN_5:
                case Defines.MZ2_SOLDIER_SHOTGUN_6:
                case Defines.MZ2_SOLDIER_SHOTGUN_7:
                case Defines.MZ2_SOLDIER_SHOTGUN_8:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    CL_tent.SmokeAndFlash(origin);
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("soldier/solatck1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_TANK_BLASTER_1:
                case Defines.MZ2_TANK_BLASTER_2:
                case Defines.MZ2_TANK_BLASTER_3:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("tank/tnkatck3.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_TANK_MACHINEGUN_1:
                case Defines.MZ2_TANK_MACHINEGUN_2:
                case Defines.MZ2_TANK_MACHINEGUN_3:
                case Defines.MZ2_TANK_MACHINEGUN_4:
                case Defines.MZ2_TANK_MACHINEGUN_5:
                case Defines.MZ2_TANK_MACHINEGUN_6:
                case Defines.MZ2_TANK_MACHINEGUN_7:
                case Defines.MZ2_TANK_MACHINEGUN_8:
                case Defines.MZ2_TANK_MACHINEGUN_9:
                case Defines.MZ2_TANK_MACHINEGUN_10:
                case Defines.MZ2_TANK_MACHINEGUN_11:
                case Defines.MZ2_TANK_MACHINEGUN_12:
                case Defines.MZ2_TANK_MACHINEGUN_13:
                case Defines.MZ2_TANK_MACHINEGUN_14:
                case Defines.MZ2_TANK_MACHINEGUN_15:
                case Defines.MZ2_TANK_MACHINEGUN_16:
                case Defines.MZ2_TANK_MACHINEGUN_17:
                case Defines.MZ2_TANK_MACHINEGUN_18:
                case Defines.MZ2_TANK_MACHINEGUN_19:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    soundname = "tank/tnkatk2" + (char)('a' + Globals.rnd.Next(5)) + ".wav";
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound(soundname), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_CHICK_ROCKET_1:
                case Defines.MZ2_TURRET_ROCKET:
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0.2F;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("chick/chkatck2.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_TANK_ROCKET_1:
                case Defines.MZ2_TANK_ROCKET_2:
                case Defines.MZ2_TANK_ROCKET_3:
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0.2F;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("tank/tnkatck1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_SUPERTANK_ROCKET_1:
                case Defines.MZ2_SUPERTANK_ROCKET_2:
                case Defines.MZ2_SUPERTANK_ROCKET_3:
                case Defines.MZ2_BOSS2_ROCKET_1:
                case Defines.MZ2_BOSS2_ROCKET_2:
                case Defines.MZ2_BOSS2_ROCKET_3:
                case Defines.MZ2_BOSS2_ROCKET_4:
                case Defines.MZ2_CARRIER_ROCKET_1:
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0.2F;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("tank/rocket.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_GUNNER_GRENADE_1:
                case Defines.MZ2_GUNNER_GRENADE_2:
                case Defines.MZ2_GUNNER_GRENADE_3:
                case Defines.MZ2_GUNNER_GRENADE_4:
                    dl.color[0] = 1;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("gunner/gunatck3.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_GLADIATOR_RAILGUN_1:
                case Defines.MZ2_CARRIER_RAILGUN:
                case Defines.MZ2_WIDOW_RAIL:
                    dl.color[0] = 0.5F;
                    dl.color[1] = 0.5F;
                    dl.color[2] = 1F;
                    break;
                case Defines.MZ2_MAKRON_BFG:
                    dl.color[0] = 0.5F;
                    dl.color[1] = 1;
                    dl.color[2] = 0.5F;
                    break;
                case Defines.MZ2_MAKRON_BLASTER_1:
                case Defines.MZ2_MAKRON_BLASTER_2:
                case Defines.MZ2_MAKRON_BLASTER_3:
                case Defines.MZ2_MAKRON_BLASTER_4:
                case Defines.MZ2_MAKRON_BLASTER_5:
                case Defines.MZ2_MAKRON_BLASTER_6:
                case Defines.MZ2_MAKRON_BLASTER_7:
                case Defines.MZ2_MAKRON_BLASTER_8:
                case Defines.MZ2_MAKRON_BLASTER_9:
                case Defines.MZ2_MAKRON_BLASTER_10:
                case Defines.MZ2_MAKRON_BLASTER_11:
                case Defines.MZ2_MAKRON_BLASTER_12:
                case Defines.MZ2_MAKRON_BLASTER_13:
                case Defines.MZ2_MAKRON_BLASTER_14:
                case Defines.MZ2_MAKRON_BLASTER_15:
                case Defines.MZ2_MAKRON_BLASTER_16:
                case Defines.MZ2_MAKRON_BLASTER_17:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("makron/blaster.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_JORG_MACHINEGUN_L1:
                case Defines.MZ2_JORG_MACHINEGUN_L2:
                case Defines.MZ2_JORG_MACHINEGUN_L3:
                case Defines.MZ2_JORG_MACHINEGUN_L4:
                case Defines.MZ2_JORG_MACHINEGUN_L5:
                case Defines.MZ2_JORG_MACHINEGUN_L6:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("boss3/xfire.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_JORG_MACHINEGUN_R1:
                case Defines.MZ2_JORG_MACHINEGUN_R2:
                case Defines.MZ2_JORG_MACHINEGUN_R3:
                case Defines.MZ2_JORG_MACHINEGUN_R4:
                case Defines.MZ2_JORG_MACHINEGUN_R5:
                case Defines.MZ2_JORG_MACHINEGUN_R6:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    break;
                case Defines.MZ2_JORG_BFG_1:
                    dl.color[0] = 0.5F;
                    dl.color[1] = 1;
                    dl.color[2] = 0.5F;
                    break;
                case Defines.MZ2_BOSS2_MACHINEGUN_R1:
                case Defines.MZ2_BOSS2_MACHINEGUN_R2:
                case Defines.MZ2_BOSS2_MACHINEGUN_R3:
                case Defines.MZ2_BOSS2_MACHINEGUN_R4:
                case Defines.MZ2_BOSS2_MACHINEGUN_R5:
                case Defines.MZ2_CARRIER_MACHINEGUN_R1:
                case Defines.MZ2_CARRIER_MACHINEGUN_R2:
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    ParticleEffect(origin, Globals.vec3_origin, 0, 40);
                    CL_tent.SmokeAndFlash(origin);
                    break;
                case Defines.MZ2_STALKER_BLASTER:
                case Defines.MZ2_DAEDALUS_BLASTER:
                case Defines.MZ2_MEDIC_BLASTER_2:
                case Defines.MZ2_WIDOW_BLASTER:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP1:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP2:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP3:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP4:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP5:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP6:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP7:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP8:
                case Defines.MZ2_WIDOW_BLASTER_SWEEP9:
                case Defines.MZ2_WIDOW_BLASTER_100:
                case Defines.MZ2_WIDOW_BLASTER_90:
                case Defines.MZ2_WIDOW_BLASTER_80:
                case Defines.MZ2_WIDOW_BLASTER_70:
                case Defines.MZ2_WIDOW_BLASTER_60:
                case Defines.MZ2_WIDOW_BLASTER_50:
                case Defines.MZ2_WIDOW_BLASTER_40:
                case Defines.MZ2_WIDOW_BLASTER_30:
                case Defines.MZ2_WIDOW_BLASTER_20:
                case Defines.MZ2_WIDOW_BLASTER_10:
                case Defines.MZ2_WIDOW_BLASTER_0:
                case Defines.MZ2_WIDOW_BLASTER_10L:
                case Defines.MZ2_WIDOW_BLASTER_20L:
                case Defines.MZ2_WIDOW_BLASTER_30L:
                case Defines.MZ2_WIDOW_BLASTER_40L:
                case Defines.MZ2_WIDOW_BLASTER_50L:
                case Defines.MZ2_WIDOW_BLASTER_60L:
                case Defines.MZ2_WIDOW_BLASTER_70L:
                case Defines.MZ2_WIDOW_RUN_1:
                case Defines.MZ2_WIDOW_RUN_2:
                case Defines.MZ2_WIDOW_RUN_3:
                case Defines.MZ2_WIDOW_RUN_4:
                case Defines.MZ2_WIDOW_RUN_5:
                case Defines.MZ2_WIDOW_RUN_6:
                case Defines.MZ2_WIDOW_RUN_7:
                case Defines.MZ2_WIDOW_RUN_8:
                    dl.color[0] = 0;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("tank/tnkatck3.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_WIDOW_DISRUPTOR:
                    dl.color[0] = -1;
                    dl.color[1] = -1;
                    dl.color[2] = -1;
                    S.StartSound(null, ent, Defines.CHAN_WEAPON, S.RegisterSound("weapons/disint2.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.MZ2_WIDOW_PLASMABEAM:
                case Defines.MZ2_WIDOW2_BEAMER_1:
                case Defines.MZ2_WIDOW2_BEAMER_2:
                case Defines.MZ2_WIDOW2_BEAMER_3:
                case Defines.MZ2_WIDOW2_BEAMER_4:
                case Defines.MZ2_WIDOW2_BEAMER_5:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_1:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_2:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_3:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_4:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_5:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_6:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_7:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_8:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_9:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_10:
                case Defines.MZ2_WIDOW2_BEAM_SWEEP_11:
                    dl.radius = 300 + (Globals.rnd.Next() & 100);
                    dl.color[0] = 1;
                    dl.color[1] = 1;
                    dl.color[2] = 0;
                    dl.die = Globals.cl.time + 200;
                    break;
            }
        }

        public static void AddDLights()
        {
            cdlight_t dl;
            if (Globals.vidref_val == Defines.VIDREF_GL)
            {
                for (int i = 0; i < Defines.MAX_DLIGHTS; i++)
                {
                    dl = cl_dlights[i];
                    if (dl.radius == 0F)
                        continue;
                    V.AddLight(dl.origin, dl.radius, dl.color[0], dl.color[1], dl.color[2]);
                }
            }
            else
            {
                for (int i = 0; i < Defines.MAX_DLIGHTS; i++)
                {
                    dl = cl_dlights[i];
                    if (dl.radius == 0F)
                        continue;
                    if ((dl.color[0] < 0) || (dl.color[1] < 0) || (dl.color[2] < 0))
                    {
                        dl.radius = -(dl.radius);
                        dl.color[0] = 1;
                        dl.color[1] = 1;
                        dl.color[2] = 1;
                    }

                    V.AddLight(dl.origin, dl.radius, dl.color[0], dl.color[1], dl.color[2]);
                }
            }
        }

        static void ClearParticles()
        {
            free_particles = particles[0];
            active_particles = null;
            for (int i = 0; i < particles.Length - 1; i++)
                particles[i].next = particles[i + 1];
            particles[particles.Length - 1].next = null;
        }

        public static void ParticleEffect(float[] org, float[] dir, int color, int count)
        {
            int j;
            cparticle_t p;
            float d;
            for (int i = 0; i < count; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = color + (Lib.Rand() & 7);
                d = Lib.Rand() & 31;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + ((Lib.Rand() & 7) - 4) + d * dir[j];
                    p.vel[j] = Lib.Crand() * 20;
                }

                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -1F / (0.5F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void ParticleEffect2(float[] org, float[] dir, int color, int count)
        {
            int j;
            cparticle_t p;
            float d;
            for (int i = 0; i < count; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = color;
                d = Lib.Rand() & 7;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + ((Lib.Rand() & 7) - 4) + d * dir[j];
                    p.vel[j] = Lib.Crand() * 20;
                }

                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -1F / (0.5F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void ParticleEffect3(float[] org, float[] dir, int color, int count)
        {
            int j;
            cparticle_t p;
            float d;
            for (int i = 0; i < count; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = color;
                d = Lib.Rand() & 7;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + ((Lib.Rand() & 7) - 4) + d * dir[j];
                    p.vel[j] = Lib.Crand() * 20;
                }

                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -1F / (0.5F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void TeleporterParticles(entity_state_t ent)
        {
            int j;
            cparticle_t p;
            for (int i = 0; i < 8; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = 0xdb;
                for (j = 0; j < 2; j++)
                {
                    p.org[j] = ent.origin[j] - 16 + (Lib.Rand() & 31);
                    p.vel[j] = Lib.Crand() * 14;
                }

                p.org[2] = ent.origin[2] - 8 + (Lib.Rand() & 7);
                p.vel[2] = 80 + (Lib.Rand() & 7);
                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -0.5F;
            }
        }

        static void LogoutEffect(float[] org, int type)
        {
            int j;
            cparticle_t p;
            for (int i = 0; i < 500; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                if (type == Defines.MZ_LOGIN)
                    p.color = 0xd0 + (Lib.Rand() & 7);
                else if (type == Defines.MZ_LOGOUT)
                    p.color = 0x40 + (Lib.Rand() & 7);
                else
                    p.color = 0xe0 + (Lib.Rand() & 7);
                p.org[0] = org[0] - 16 + ( float ) Globals.rnd.NextDouble() * 32;
                p.org[1] = org[1] - 16 + ( float ) Globals.rnd.NextDouble() * 32;
                p.org[2] = org[2] - 24 + ( float ) Globals.rnd.NextDouble() * 56;
                for (j = 0; j < 3; j++)
                    p.vel[j] = Lib.Crand() * 20;
                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -1F / (1F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        static void ItemRespawnParticles(float[] org)
        {
            int j;
            cparticle_t p;
            for (int i = 0; i < 64; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = 0xd4 + (Lib.Rand() & 3);
                p.org[0] = org[0] + Lib.Crand() * 8;
                p.org[1] = org[1] + Lib.Crand() * 8;
                p.org[2] = org[2] + Lib.Crand() * 8;
                for (j = 0; j < 3; j++)
                    p.vel[j] = Lib.Crand() * 8;
                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -PARTICLE_GRAVITY * 0.2F;
                p.alpha = 1F;
                p.alphavel = -1F / (1F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void ExplosionParticles(float[] org)
        {
            int j;
            cparticle_t p;
            for (int i = 0; i < 256; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = 0xe0 + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + ((Lib.Rand() % 32) - 16);
                    p.vel[j] = (Lib.Rand() % 384) - 192;
                }

                p.accel[0] = p.accel[1] = 0F;
                p.accel[2] = -PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -0.8F / (0.5F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void BigTeleportParticles(float[] org)
        {
            cparticle_t p;
            float angle, dist;
            for (int i = 0; i < 4096; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = colortable[Lib.Rand() & 3];
                angle = (float)(Math.PI * 2 * (Lib.Rand() & 1023) / 1023);
                dist = Lib.Rand() & 31;
                p.org[0] = (float)(org[0] + Math.Cos(angle) * dist);
                p.vel[0] = (float)(Math.Cos(angle) * (70 + (Lib.Rand() & 63)));
                p.accel[0] = (float)(-Math.Cos(angle) * 100);
                p.org[1] = (float)(org[1] + Math.Sin(angle) * dist);
                p.vel[1] = (float)(Math.Sin(angle) * (70 + (Lib.Rand() & 63)));
                p.accel[1] = (float)(-Math.Sin(angle) * 100);
                p.org[2] = org[2] + 8 + (Lib.Rand() % 90);
                p.vel[2] = -100 + (Lib.Rand() & 31);
                p.accel[2] = PARTICLE_GRAVITY * 4;
                p.alpha = 1F;
                p.alphavel = -0.3F / (0.5F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        public static void BlasterParticles(float[] org, float[] dir)
        {
            int j;
            cparticle_t p;
            float d;
            int count = 40;
            for (int i = 0; i < count; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = 0xe0 + (Lib.Rand() & 7);
                d = Lib.Rand() & 15;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + ((Lib.Rand() & 7) - 4) + d * dir[j];
                    p.vel[j] = dir[j] * 30 + Lib.Crand() * 40;
                }

                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -1F / (0.5F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        private static readonly float[] move = new float[]{0, 0, 0};
        private static readonly float[] vec = new float[]{0, 0, 0};
        public static void BlasterTrail(float[] start, float[] end)
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
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = -1F / (0.3F + ( float ) Globals.rnd.NextDouble() * 0.2F);
                p.color = 0xe0;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + Lib.Crand();
                    p.vel[j] = Lib.Crand() * 5;
                    p.accel[j] = 0;
                }

                Math3D.VectorAdd(move, vec, move);
            }
        }

        public static void FlagTrail(float[] start, float[] end, float color)
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
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = -1F / (0.8F + ( float ) Globals.rnd.NextDouble() * 0.2F);
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

        public static void DiminishingTrail(float[] start, float[] end, centity_t old, int flags)
        {
            cparticle_t p;
            float orgscale;
            float velscale;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            float len = Math3D.VectorNormalize(vec);
            float dec = 0.5F;
            Math3D.VectorScale(vec, dec, vec);
            if (old.trailcount > 900)
            {
                orgscale = 4;
                velscale = 15;
            }
            else if (old.trailcount > 800)
            {
                orgscale = 2;
                velscale = 10;
            }
            else
            {
                orgscale = 1;
                velscale = 5;
            }

            while (len > 0)
            {
                len -= dec;
                if (free_particles == null)
                    return;
                if ((Lib.Rand() & 1023) < old.trailcount)
                {
                    p = free_particles;
                    free_particles = p.next;
                    p.next = active_particles;
                    active_particles = p;
                    Math3D.VectorClear(p.accel);
                    p.time = Globals.cl.time;
                    if ((flags & Defines.EF_GIB) != 0)
                    {
                        p.alpha = 1F;
                        p.alphavel = -1F / (1F + ( float ) Globals.rnd.NextDouble() * 0.4F);
                        p.color = 0xe8 + (Lib.Rand() & 7);
                        for (int j = 0; j < 3; j++)
                        {
                            p.org[j] = move[j] + Lib.Crand() * orgscale;
                            p.vel[j] = Lib.Crand() * velscale;
                            p.accel[j] = 0;
                        }

                        p.vel[2] -= PARTICLE_GRAVITY;
                    }
                    else if ((flags & Defines.EF_GREENGIB) != 0)
                    {
                        p.alpha = 1F;
                        p.alphavel = -1F / (1F + ( float ) Globals.rnd.NextDouble() * 0.4F);
                        p.color = 0xdb + (Lib.Rand() & 7);
                        for (int j = 0; j < 3; j++)
                        {
                            p.org[j] = move[j] + Lib.Crand() * orgscale;
                            p.vel[j] = Lib.Crand() * velscale;
                            p.accel[j] = 0;
                        }

                        p.vel[2] -= PARTICLE_GRAVITY;
                    }
                    else
                    {
                        p.alpha = 1F;
                        p.alphavel = -1F / (1F + ( float ) Globals.rnd.NextDouble() * 0.2F);
                        p.color = 4 + (Lib.Rand() & 7);
                        for (int j = 0; j < 3; j++)
                        {
                            p.org[j] = move[j] + Lib.Crand() * orgscale;
                            p.vel[j] = Lib.Crand() * velscale;
                        }

                        p.accel[2] = 20;
                    }
                }

                old.trailcount -= 5;
                if (old.trailcount < 100)
                    old.trailcount = 100;
                Math3D.VectorAdd(move, vec, move);
            }
        }

        public static void RocketTrail(float[] start, float[] end, centity_t old)
        {
            float len;
            int j;
            cparticle_t p;
            float dec;
            DiminishingTrail(start, end, old, Defines.EF_ROCKET);
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            dec = 1;
            Math3D.VectorScale(vec, dec, vec);
            while (len > 0)
            {
                len -= dec;
                if (free_particles == null)
                    return;
                if ((Lib.Rand() & 7) == 0)
                {
                    p = free_particles;
                    free_particles = p.next;
                    p.next = active_particles;
                    active_particles = p;
                    Math3D.VectorClear(p.accel);
                    p.time = Globals.cl.time;
                    p.alpha = 1F;
                    p.alphavel = -1F / (1F + ( float ) Globals.rnd.NextDouble() * 0.2F);
                    p.color = 0xdc + (Lib.Rand() & 3);
                    for (j = 0; j < 3; j++)
                    {
                        p.org[j] = move[j] + Lib.Crand() * 5;
                        p.vel[j] = Lib.Crand() * 20;
                    }

                    p.accel[2] = -PARTICLE_GRAVITY;
                }

                Math3D.VectorAdd(move, vec, move);
            }
        }

        public static void RailTrail(float[] start, float[] end)
        {
            float len;
            int j;
            cparticle_t p;
            float dec;
            float[] right = new float[3];
            float[] up = new float[3];
            int i;
            float d, c, s;
            float[] dir = new float[3];
            byte clr = 0x74;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            Math3D.MakeNormalVectors(vec, right, up);
            for (i = 0; i < len; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                Math3D.VectorClear(p.accel);
                d = i * 0.1F;
                c = (float)Math.Cos(d);
                s = (float)Math.Sin(d);
                Math3D.VectorScale(right, c, dir);
                Math3D.VectorMA(dir, s, up, dir);
                p.alpha = 1F;
                p.alphavel = -1F / (1F + ( float ) Globals.rnd.NextDouble() * 0.2F);
                p.color = clr + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + dir[j] * 3;
                    p.vel[j] = dir[j] * 6;
                }

                Math3D.VectorAdd(move, vec, move);
            }

            dec = 0.75F;
            Math3D.VectorScale(vec, dec, vec);
            Math3D.VectorCopy(start, move);
            while (len > 0)
            {
                len -= dec;
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
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
            }
        }

        public static void IonripperTrail(float[] start, float[] ent)
        {
            float len;
            int j;
            cparticle_t p;
            int dec;
            int left = 0;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(ent, start, vec);
            len = Math3D.VectorNormalize(vec);
            dec = 5;
            Math3D.VectorScale(vec, 5, vec);
            while (len > 0)
            {
                len -= dec;
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 0.5F;
                p.alphavel = -1F / (0.3F + ( float ) Globals.rnd.NextDouble() * 0.2F);
                p.color = 0xe4 + (Lib.Rand() & 3);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j];
                    p.accel[j] = 0;
                }

                if (left != 0)
                {
                    left = 0;
                    p.vel[0] = 10;
                }
                else
                {
                    left = 1;
                    p.vel[0] = -10;
                }

                p.vel[1] = 0;
                p.vel[2] = 0;
                Math3D.VectorAdd(move, vec, move);
            }
        }

        public static void BubbleTrail(float[] start, float[] end)
        {
            float len;
            int i, j;
            cparticle_t p;
            int dec;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            dec = 32;
            Math3D.VectorScale(vec, dec, vec);
            for (i = 0; i < len; i += dec)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = -1F / (1F + ( float ) Globals.rnd.NextDouble() * 0.2F);
                p.color = 4 + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + Lib.Crand() * 2;
                    p.vel[j] = Lib.Crand() * 5;
                }

                p.vel[2] += 6;
                Math3D.VectorAdd(move, vec, move);
            }
        }

        static void FlyParticles(float[] origin, int count)
        {
            int i;
            cparticle_t p;
            float angle;
            float sp, sy, cp, cy;
            float dist = 64;
            float ltime;
            if (count > Defines.NUMVERTEXNORMALS)
                count = Defines.NUMVERTEXNORMALS;
            if (avelocities[0][0] == 0F)
            {
                for (i = 0; i < Defines.NUMVERTEXNORMALS; i++)
                {
                    avelocities[i][0] = (Lib.Rand() & 255) * 0.01F;
                    avelocities[i][1] = (Lib.Rand() & 255) * 0.01F;
                    avelocities[i][2] = (Lib.Rand() & 255) * 0.01F;
                }
            }

            ltime = Globals.cl.time / 1000F;
            for (i = 0; i < count; i += 2)
            {
                angle = ltime * avelocities[i][0];
                sy = (float)Math.Sin(angle);
                cy = (float)Math.Cos(angle);
                angle = ltime * avelocities[i][1];
                sp = (float)Math.Sin(angle);
                cp = (float)Math.Cos(angle);
                angle = ltime * avelocities[i][2];
                forward[0] = cp * cy;
                forward[1] = cp * sy;
                forward[2] = -sp;
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                dist = (float)Math.Sin(ltime + i) * 64;
                p.org[0] = origin[0] + Globals.bytedirs[i][0] * dist + forward[0] * BEAMLENGTH;
                p.org[1] = origin[1] + Globals.bytedirs[i][1] * dist + forward[1] * BEAMLENGTH;
                p.org[2] = origin[2] + Globals.bytedirs[i][2] * dist + forward[2] * BEAMLENGTH;
                Math3D.VectorClear(p.vel);
                Math3D.VectorClear(p.accel);
                p.color = 0;
                p.alpha = 1;
                p.alphavel = -100;
            }
        }

        public static void FlyEffect(centity_t ent, float[] origin)
        {
            int n;
            int count;
            int starttime;
            if (ent.fly_stoptime < Globals.cl.time)
            {
                starttime = Globals.cl.time;
                ent.fly_stoptime = Globals.cl.time + 60000;
            }
            else
            {
                starttime = ent.fly_stoptime - 60000;
            }

            n = Globals.cl.time - starttime;
            if (n < 20000)
                count = (int)((n * 162) / 20000);
            else
            {
                n = ent.fly_stoptime - Globals.cl.time;
                if (n < 20000)
                    count = (int)((n * 162) / 20000);
                else
                    count = 162;
            }

            FlyParticles(origin, count);
        }

        private static readonly float[] v = new float[]{0, 0, 0};
        public static void BfgParticles(entity_t ent)
        {
            int i;
            cparticle_t p;
            float angle;
            float sp, sy, cp, cy;
            float dist = 64;
            float ltime;
            if (avelocities[0][0] == 0F)
            {
                for (i = 0; i < Defines.NUMVERTEXNORMALS; i++)
                {
                    avelocities[i][0] = (Lib.Rand() & 255) * 0.01F;
                    avelocities[i][1] = (Lib.Rand() & 255) * 0.01F;
                    avelocities[i][2] = (Lib.Rand() & 255) * 0.01F;
                }
            }

            ltime = Globals.cl.time / 1000F;
            for (i = 0; i < Defines.NUMVERTEXNORMALS; i++)
            {
                angle = ltime * avelocities[i][0];
                sy = (float)Math.Sin(angle);
                cy = (float)Math.Cos(angle);
                angle = ltime * avelocities[i][1];
                sp = (float)Math.Sin(angle);
                cp = (float)Math.Cos(angle);
                angle = ltime * avelocities[i][2];
                forward[0] = cp * cy;
                forward[1] = cp * sy;
                forward[2] = -sp;
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                dist = (float)(Math.Sin(ltime + i) * 64);
                p.org[0] = ent.origin[0] + Globals.bytedirs[i][0] * dist + forward[0] * BEAMLENGTH;
                p.org[1] = ent.origin[1] + Globals.bytedirs[i][1] * dist + forward[1] * BEAMLENGTH;
                p.org[2] = ent.origin[2] + Globals.bytedirs[i][2] * dist + forward[2] * BEAMLENGTH;
                Math3D.VectorClear(p.vel);
                Math3D.VectorClear(p.accel);
                Math3D.VectorSubtract(p.org, ent.origin, v);
                dist = Math3D.VectorLength(v) / 90F;
                p.color = (float)Math.Floor(0xd0 + dist * 7);
                p.alpha = 1F - dist;
                p.alphavel = -100;
            }
        }

        private static readonly float[] start = new float[]{0, 0, 0};
        private static readonly float[] end = new float[]{0, 0, 0};
        public static void TrapParticles(entity_t ent)
        {
            float len;
            int j;
            cparticle_t p;
            int dec;
            ent.origin[2] -= 14;
            Math3D.VectorCopy(ent.origin, start);
            Math3D.VectorCopy(ent.origin, end);
            end[2] += 64;
            Math3D.VectorCopy(start, move);
            Math3D.VectorSubtract(end, start, vec);
            len = Math3D.VectorNormalize(vec);
            dec = 5;
            Math3D.VectorScale(vec, 5, vec);
            while (len > 0)
            {
                len -= dec;
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                Math3D.VectorClear(p.accel);
                p.time = Globals.cl.time;
                p.alpha = 1F;
                p.alphavel = -1F / (0.3F + ( float ) Globals.rnd.NextDouble() * 0.2F);
                p.color = 0xe0;
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = move[j] + Lib.Crand();
                    p.vel[j] = Lib.Crand() * 15;
                    p.accel[j] = 0;
                }

                p.accel[2] = PARTICLE_GRAVITY;
                Math3D.VectorAdd(move, vec, move);
            }

            int i, k;
            float vel;
            float[] dir = new float[3];
            float[] org = new float[3];
            ent.origin[2] += 14;
            Math3D.VectorCopy(ent.origin, org);
            for (i = -2; i <= 2; i += 4)
                for (j = -2; j <= 2; j += 4)
                    for (k = -2; k <= 4; k += 4)
                    {
                        if (free_particles == null)
                            return;
                        p = free_particles;
                        free_particles = p.next;
                        p.next = active_particles;
                        active_particles = p;
                        p.time = Globals.cl.time;
                        p.color = 0xe0 + (Lib.Rand() & 3);
                        p.alpha = 1F;
                        p.alphavel = -1F / (0.3F + (Lib.Rand() & 7) * 0.02F);
                        p.org[0] = org[0] + i + ((Lib.Rand() & 23) * Lib.Crand());
                        p.org[1] = org[1] + j + ((Lib.Rand() & 23) * Lib.Crand());
                        p.org[2] = org[2] + k + ((Lib.Rand() & 23) * Lib.Crand());
                        dir[0] = j * 8;
                        dir[1] = i * 8;
                        dir[2] = k * 8;
                        Math3D.VectorNormalize(dir);
                        vel = 50 + Lib.Rand() & 63;
                        Math3D.VectorScale(dir, vel, p.vel);
                        p.accel[0] = p.accel[1] = 0;
                        p.accel[2] = -PARTICLE_GRAVITY;
                    }
        }

        public static void BFGExplosionParticles(float[] org)
        {
            int j;
            cparticle_t p;
            for (int i = 0; i < 256; i++)
            {
                if (free_particles == null)
                    return;
                p = free_particles;
                free_particles = p.next;
                p.next = active_particles;
                active_particles = p;
                p.time = Globals.cl.time;
                p.color = 0xd0 + (Lib.Rand() & 7);
                for (j = 0; j < 3; j++)
                {
                    p.org[j] = org[j] + ((Lib.Rand() % 32) - 16);
                    p.vel[j] = (Lib.Rand() % 384) - 192;
                }

                p.accel[0] = p.accel[1] = 0;
                p.accel[2] = -PARTICLE_GRAVITY;
                p.alpha = 1F;
                p.alphavel = -0.8F / (0.5F + ( float ) Globals.rnd.NextDouble() * 0.3F);
            }
        }

        private static readonly float[] dir = new float[]{0, 0, 0};
        public static void TeleportParticles(float[] org)
        {
            cparticle_t p;
            float vel;
            for (int i = -16; i <= 16; i += 4)
                for (int j = -16; j <= 16; j += 4)
                    for (int k = -16; k <= 32; k += 4)
                    {
                        if (free_particles == null)
                            return;
                        p = free_particles;
                        free_particles = p.next;
                        p.next = active_particles;
                        active_particles = p;
                        p.time = Globals.cl.time;
                        p.color = 7 + (Lib.Rand() & 7);
                        p.alpha = 1F;
                        p.alphavel = -1F / (0.3F + (Lib.Rand() & 7) * 0.02F);
                        p.org[0] = org[0] + i + (Lib.Rand() & 3);
                        p.org[1] = org[1] + j + (Lib.Rand() & 3);
                        p.org[2] = org[2] + k + (Lib.Rand() & 3);
                        dir[0] = j * 8;
                        dir[1] = i * 8;
                        dir[2] = k * 8;
                        Math3D.VectorNormalize(dir);
                        vel = 50 + (Lib.Rand() & 63);
                        Math3D.VectorScale(dir, vel, p.vel);
                        p.accel[0] = p.accel[1] = 0;
                        p.accel[2] = -PARTICLE_GRAVITY;
                    }
        }

        private static readonly float[] org = new float[]{0, 0, 0};
        public static void AddParticles()
        {
            cparticle_t p, next;
            float alpha;
            float time = 0F;
            float time2;
            int color;
            cparticle_t active, tail;
            active = null;
            tail = null;
            for (p = active_particles; p != null; p = next)
            {
                next = p.next;
                if (p.alphavel != INSTANT_PARTICLE)
                {
                    time = (Globals.cl.time - p.time) * 0.001F;
                    alpha = p.alpha + time * p.alphavel;
                    if (alpha <= 0)
                    {
                        p.next = free_particles;
                        free_particles = p;
                        continue;
                    }
                }
                else
                {
                    alpha = p.alpha;
                }

                p.next = null;
                if (tail == null)
                    active = tail = p;
                else
                {
                    tail.next = p;
                    tail = p;
                }

                if (alpha > 1)
                    alpha = 1;
                color = (int)p.color;
                time2 = time * time;
                org[0] = p.org[0] + p.vel[0] * time + p.accel[0] * time2;
                org[1] = p.org[1] + p.vel[1] * time + p.accel[1] * time2;
                org[2] = p.org[2] + p.vel[2] * time + p.accel[2] * time2;
                V.AddParticle(org, color, alpha);
                if (p.alphavel == INSTANT_PARTICLE)
                {
                    p.alphavel = 0F;
                    p.alpha = 0F;
                }
            }

            active_particles = active;
        }

        public static void EntityEvent(entity_state_t ent)
        {
            switch (ent.event_renamed)

            {
                case Defines.EV_ITEM_RESPAWN:
                    S.StartSound(null, ent.number, Defines.CHAN_WEAPON, S.RegisterSound("items/respawn1.wav"), 1, Defines.ATTN_IDLE, 0);
                    ItemRespawnParticles(ent.origin);
                    break;
                case Defines.EV_PLAYER_TELEPORT:
                    S.StartSound(null, ent.number, Defines.CHAN_WEAPON, S.RegisterSound("misc/tele1.wav"), 1, Defines.ATTN_IDLE, 0);
                    TeleportParticles(ent.origin);
                    break;
                case Defines.EV_FOOTSTEP:
                    if (Globals.cl_footsteps.value != 0F)
                        S.StartSound(null, ent.number, Defines.CHAN_BODY, CL_tent.cl_sfx_footsteps[Lib.Rand() & 3], 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.EV_FALLSHORT:
                    S.StartSound(null, ent.number, Defines.CHAN_AUTO, S.RegisterSound("player/land1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.EV_FALL:
                    S.StartSound(null, ent.number, Defines.CHAN_AUTO, S.RegisterSound("*fall2.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
                case Defines.EV_FALLFAR:
                    S.StartSound(null, ent.number, Defines.CHAN_AUTO, S.RegisterSound("*fall1.wav"), 1, Defines.ATTN_NORM, 0);
                    break;
            }
        }

        public static void ClearEffects()
        {
            ClearParticles();
            ClearDlights();
            ClearLightStyles();
        }

        public static readonly int PARTICLE_GRAVITY = 40;
        public static cparticle_t active_particles, free_particles;
        private static int[] colortable = new[]{2 * 8, 13 * 8, 21 * 8, 18 * 8};
        private static readonly int BEAMLENGTH = 16;
    }
}
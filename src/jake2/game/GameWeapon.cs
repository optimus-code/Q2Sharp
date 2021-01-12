using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class GameWeapon
    {
        static EntTouchAdapter blaster_touch = new AnonymousEntTouchAdapter();
        private sealed class AnonymousEntTouchAdapter : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "blaster_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                int mod;
                if (other == self.owner)
                    return;
                if (surf != null && (surf.flags & Defines.SURF_SKY) != 0)
                {
                    GameUtil.G_FreeEdict(self);
                    return;
                }

                if (self.owner.client != null)
                    PlayerWeapon.PlayerNoise(self.owner, self.s.origin, Defines.PNOISE_IMPACT);
                if (other.takedamage != 0)
                {
                    if ((self.spawnflags & 1) != 0)
                        mod = Defines.MOD_HYPERBLASTER;
                    else
                        mod = Defines.MOD_BLASTER;
                    float[] normal;
                    if (plane == null)
                        normal = new float[3];
                    else
                        normal = plane.normal;
                    GameCombat.T_Damage(other, self, self.owner, self.velocity, self.s.origin, normal, self.dmg, 1, Defines.DAMAGE_ENERGY, mod);
                }
                else
                {
                    GameBase.gi.WriteByte(Defines.svc_temp_entity);
                    GameBase.gi.WriteByte(Defines.TE_BLASTER);
                    GameBase.gi.WritePosition(self.s.origin);
                    if (plane == null)
                        GameBase.gi.WriteDir(Globals.vec3_origin);
                    else
                        GameBase.gi.WriteDir(plane.normal);
                    GameBase.gi.Multicast(self.s.origin, Defines.MULTICAST_PVS);
                }

                GameUtil.G_FreeEdict(self);
            }
        }

        static EntThinkAdapter Grenade_Explode = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "Grenade_Explode";
            }

            public override bool Think(edict_t ent)
            {
                float[] origin = new float[]{0, 0, 0};
                int mod;
                if (ent.owner.client != null)
                    PlayerWeapon.PlayerNoise(ent.owner, ent.s.origin, Defines.PNOISE_IMPACT);
                if (ent.enemy != null)
                {
                    float points = 0;
                    float[] v = new float[]{0, 0, 0};
                    float[] dir = new float[]{0, 0, 0};
                    Math3D.VectorAdd(ent.enemy.mins, ent.enemy.maxs, v);
                    Math3D.VectorMA(ent.enemy.s.origin, 0.5F, v, v);
                    Math3D.VectorSubtract(ent.s.origin, v, v);
                    points = ent.dmg - 0.5F * Math3D.VectorLength(v);
                    Math3D.VectorSubtract(ent.enemy.s.origin, ent.s.origin, dir);
                    if ((ent.spawnflags & 1) != 0)
                        mod = Defines.MOD_HANDGRENADE;
                    else
                        mod = Defines.MOD_GRENADE;
                    GameCombat.T_Damage(ent.enemy, ent, ent.owner, dir, ent.s.origin, Globals.vec3_origin, (int)points, (int)points, Defines.DAMAGE_RADIUS, mod);
                }

                if ((ent.spawnflags & 2) != 0)
                    mod = Defines.MOD_HELD_GRENADE;
                else if ((ent.spawnflags & 1) != 0)
                    mod = Defines.MOD_HG_SPLASH;
                else
                    mod = Defines.MOD_G_SPLASH;
                GameCombat.T_RadiusDamage(ent, ent.owner, ent.dmg, ent.enemy, ent.dmg_radius, mod);
                Math3D.VectorMA(ent.s.origin, -0.02F, ent.velocity, origin);
                GameBase.gi.WriteByte(Defines.svc_temp_entity);
                if (ent.waterlevel != 0)
                {
                    if (ent.groundentity != null)
                        GameBase.gi.WriteByte(Defines.TE_GRENADE_EXPLOSION_WATER);
                    else
                        GameBase.gi.WriteByte(Defines.TE_ROCKET_EXPLOSION_WATER);
                }
                else
                {
                    if (ent.groundentity != null)
                        GameBase.gi.WriteByte(Defines.TE_GRENADE_EXPLOSION);
                    else
                        GameBase.gi.WriteByte(Defines.TE_ROCKET_EXPLOSION);
                }

                GameBase.gi.WritePosition(origin);
                GameBase.gi.Multicast(ent.s.origin, Defines.MULTICAST_PHS);
                GameUtil.G_FreeEdict(ent);
                return true;
            }
        }

        static EntTouchAdapter Grenade_Touch = new AnonymousEntTouchAdapter1();
        private sealed class AnonymousEntTouchAdapter1 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "Grenade_Touch";
            }

            public override void Touch(edict_t ent, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (other == ent.owner)
                    return;
                if (surf != null && 0 != (surf.flags & Defines.SURF_SKY))
                {
                    GameUtil.G_FreeEdict(ent);
                    return;
                }

                if (other.takedamage == 0)
                {
                    if ((ent.spawnflags & 1) != 0)
                    {
                        if (Lib.Random() > 0.5F)
                            GameBase.gi.Sound(ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex("weapons/hgrenb1a.wav"), 1, Defines.ATTN_NORM, 0);
                        else
                            GameBase.gi.Sound(ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex("weapons/hgrenb2a.wav"), 1, Defines.ATTN_NORM, 0);
                    }
                    else
                    {
                        GameBase.gi.Sound(ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex("weapons/grenlb1b.wav"), 1, Defines.ATTN_NORM, 0);
                    }

                    return;
                }

                ent.enemy = other;
                Grenade_Explode.Think(ent);
            }
        }

        static EntTouchAdapter rocket_touch = new AnonymousEntTouchAdapter2();
        private sealed class AnonymousEntTouchAdapter2 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "rocket_touch";
            }

            public override void Touch(edict_t ent, edict_t other, cplane_t plane, csurface_t surf)
            {
                float[] origin = new float[]{0, 0, 0};
                int n;
                if (other == ent.owner)
                    return;
                if (surf != null && (surf.flags & Defines.SURF_SKY) != 0)
                {
                    GameUtil.G_FreeEdict(ent);
                    return;
                }

                if (ent.owner.client != null)
                    PlayerWeapon.PlayerNoise(ent.owner, ent.s.origin, Defines.PNOISE_IMPACT);
                Math3D.VectorMA(ent.s.origin, -0.02F, ent.velocity, origin);
                if (other.takedamage != 0)
                {
                    GameCombat.T_Damage(other, ent, ent.owner, ent.velocity, ent.s.origin, plane.normal, ent.dmg, 0, 0, Defines.MOD_ROCKET);
                }
                else
                {
                    if (GameBase.deathmatch.value == 0 && 0 == GameBase.coop.value)
                    {
                        if ((surf != null) && 0 == (surf.flags & (Defines.SURF_WARP | Defines.SURF_TRANS33 | Defines.SURF_TRANS66 | Defines.SURF_FLOWING)))
                        {
                            n = Lib.Rand() % 5;
                            while (n-- > 0)
                                GameMisc.ThrowDebris(ent, "models/objects/debris2/tris.md2", 2, ent.s.origin);
                        }
                    }
                }

                GameCombat.T_RadiusDamage(ent, ent.owner, ent.radius_dmg, other, ent.dmg_radius, Defines.MOD_R_SPLASH);
                GameBase.gi.WriteByte(Defines.svc_temp_entity);
                if (ent.waterlevel != 0)
                    GameBase.gi.WriteByte(Defines.TE_ROCKET_EXPLOSION_WATER);
                else
                    GameBase.gi.WriteByte(Defines.TE_ROCKET_EXPLOSION);
                GameBase.gi.WritePosition(origin);
                GameBase.gi.Multicast(ent.s.origin, Defines.MULTICAST_PHS);
                GameUtil.G_FreeEdict(ent);
            }
        }

        static EntThinkAdapter bfg_explode = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "bfg_explode";
            }

            public override bool Think(edict_t self)
            {
                edict_t ent;
                float points;
                float[] v = new float[]{0, 0, 0};
                float dist;
                EdictIterator edit = null;
                if (self.s.frame == 0)
                {
                    ent = null;
                    while ((edit = GameBase.Findradius(edit, self.s.origin, self.dmg_radius)) != null)
                    {
                        ent = edit.o;
                        if (ent.takedamage == 0)
                            continue;
                        if (ent == self.owner)
                            continue;
                        if (!GameCombat.CanDamage(ent, self))
                            continue;
                        if (!GameCombat.CanDamage(ent, self.owner))
                            continue;
                        Math3D.VectorAdd(ent.mins, ent.maxs, v);
                        Math3D.VectorMA(ent.s.origin, 0.5F, v, v);
                        Math3D.VectorSubtract(self.s.origin, v, v);
                        dist = Math3D.VectorLength(v);
                        points = (float)(self.radius_dmg * (1 - Math.Sqrt(dist / self.dmg_radius)));
                        if (ent == self.owner)
                            points = points * 0.5F;
                        GameBase.gi.WriteByte(Defines.svc_temp_entity);
                        GameBase.gi.WriteByte(Defines.TE_BFG_EXPLOSION);
                        GameBase.gi.WritePosition(ent.s.origin);
                        GameBase.gi.Multicast(ent.s.origin, Defines.MULTICAST_PHS);
                        GameCombat.T_Damage(ent, self, self.owner, self.velocity, ent.s.origin, Globals.vec3_origin, (int)points, 0, Defines.DAMAGE_ENERGY, Defines.MOD_BFG_EFFECT);
                    }
                }

                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                self.s.frame++;
                if (self.s.frame == 5)
                    self.think = GameUtil.G_FreeEdictA;
                return true;
            }
        }

        static EntTouchAdapter bfg_touch = new AnonymousEntTouchAdapter3();
        private sealed class AnonymousEntTouchAdapter3 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "bfg_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (other == self.owner)
                    return;
                if (surf != null && (surf.flags & Defines.SURF_SKY) != 0)
                {
                    GameUtil.G_FreeEdict(self);
                    return;
                }

                if (self.owner.client != null)
                    PlayerWeapon.PlayerNoise(self.owner, self.s.origin, Defines.PNOISE_IMPACT);
                if (other.takedamage != 0)
                    GameCombat.T_Damage(other, self, self.owner, self.velocity, self.s.origin, plane.normal, 200, 0, 0, Defines.MOD_BFG_BLAST);
                GameCombat.T_RadiusDamage(self, self.owner, 200, other, 100, Defines.MOD_BFG_BLAST);
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, GameBase.gi.Soundindex("weapons/bfg__x1b.wav"), 1, Defines.ATTN_NORM, 0);
                self.solid = Defines.SOLID_NOT;
                self.touch = null;
                Math3D.VectorMA(self.s.origin, -1 * Defines.FRAMETIME, self.velocity, self.s.origin);
                Math3D.VectorClear(self.velocity);
                self.s.modelindex = GameBase.gi.Modelindex("sprites/s_bfg3.sp2");
                self.s.frame = 0;
                self.s.sound = 0;
                self.s.effects &= ~Defines.EF_ANIM_ALLFAST;
                self.think = bfg_explode;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                self.enemy = other;
                GameBase.gi.WriteByte(Defines.svc_temp_entity);
                GameBase.gi.WriteByte(Defines.TE_BFG_BIGEXPLOSION);
                GameBase.gi.WritePosition(self.s.origin);
                GameBase.gi.Multicast(self.s.origin, Defines.MULTICAST_PVS);
            }
        }

        static EntThinkAdapter bfg_think = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "bfg_think";
            }

            public override bool Think(edict_t self)
            {
                edict_t ent;
                edict_t ignore;
                float[] point = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                float[] start = new float[]{0, 0, 0};
                float[] end = new float[]{0, 0, 0};
                int dmg;
                trace_t tr;
                if (GameBase.deathmatch.value != 0)
                    dmg = 5;
                else
                    dmg = 10;
                EdictIterator edit = null;
                while ((edit = GameBase.Findradius(edit, self.s.origin, 256)) != null)
                {
                    ent = edit.o;
                    if (ent == self)
                        continue;
                    if (ent == self.owner)
                        continue;
                    if (ent.takedamage == 0)
                        continue;
                    if (0 == (ent.svflags & Defines.SVF_MONSTER) && (null == ent.client) && (Lib.Strcmp(ent.classname, "misc_explobox") != 0))
                        continue;
                    Math3D.VectorMA(ent.absmin, 0.5F, ent.size, point);
                    Math3D.VectorSubtract(point, self.s.origin, dir);
                    Math3D.VectorNormalize(dir);
                    ignore = self;
                    Math3D.VectorCopy(self.s.origin, start);
                    Math3D.VectorMA(start, 2048, dir, end);
                    while (true)
                    {
                        tr = GameBase.gi.Trace(start, null, null, end, ignore, Defines.CONTENTS_SOLID | Defines.CONTENTS_MONSTER | Defines.CONTENTS_DEADMONSTER);
                        if (null == tr.ent)
                            break;
                        if ((tr.ent.takedamage != 0) && 0 == (tr.ent.flags & Defines.FL_IMMUNE_LASER) && (tr.ent != self.owner))
                            GameCombat.T_Damage(tr.ent, self, self.owner, dir, tr.endpos, Globals.vec3_origin, dmg, 1, Defines.DAMAGE_ENERGY, Defines.MOD_BFG_LASER);
                        if (0 == (tr.ent.svflags & Defines.SVF_MONSTER) && (null == tr.ent.client))
                        {
                            GameBase.gi.WriteByte(Defines.svc_temp_entity);
                            GameBase.gi.WriteByte(Defines.TE_LASER_SPARKS);
                            GameBase.gi.WriteByte(4);
                            GameBase.gi.WritePosition(tr.endpos);
                            GameBase.gi.WriteDir(tr.plane.normal);
                            GameBase.gi.WriteByte(self.s.skinnum);
                            GameBase.gi.Multicast(tr.endpos, Defines.MULTICAST_PVS);
                            break;
                        }

                        ignore = tr.ent;
                        Math3D.VectorCopy(tr.endpos, start);
                    }

                    GameBase.gi.WriteByte(Defines.svc_temp_entity);
                    GameBase.gi.WriteByte(Defines.TE_BFG_LASER);
                    GameBase.gi.WritePosition(self.s.origin);
                    GameBase.gi.WritePosition(tr.endpos);
                    GameBase.gi.Multicast(self.s.origin, Defines.MULTICAST_PHS);
                }

                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                return true;
            }
        }

        static void Check_dodge(edict_t self, float[] start, float[] dir, int speed)
        {
            float[] end = new float[]{0, 0, 0};
            float[] v = new float[]{0, 0, 0};
            trace_t tr;
            float eta;
            if (GameBase.skill.value == 0)
            {
                if (Lib.Random() > 0.25)
                    return;
            }

            Math3D.VectorMA(start, 8192, dir, end);
            tr = GameBase.gi.Trace(start, null, null, end, self, Defines.MASK_SHOT);
            if ((tr.ent != null) && (tr.ent.svflags & Defines.SVF_MONSTER) != 0 && (tr.ent.health > 0) && (null != tr.ent.monsterinfo.dodge) && GameUtil.Infront(tr.ent, self))
            {
                Math3D.VectorSubtract(tr.endpos, start, v);
                eta = (Math3D.VectorLength(v) - tr.ent.maxs[0]) / speed;
                tr.ent.monsterinfo.dodge.Dodge(tr.ent, self, eta);
            }
        }

        public static bool Fire_hit(edict_t self, float[] aim, int damage, int kick)
        {
            trace_t tr;
            float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0}, up = new float[]{0, 0, 0};
            float[] v = new float[]{0, 0, 0};
            float[] point = new float[]{0, 0, 0};
            float range;
            float[] dir = new float[]{0, 0, 0};
            Math3D.VectorSubtract(self.enemy.s.origin, self.s.origin, dir);
            range = Math3D.VectorLength(dir);
            if (range > aim[0])
                return false;
            if (aim[1] > self.mins[0] && aim[1] < self.maxs[0])
            {
                range -= self.enemy.maxs[0];
            }
            else
            {
                if (aim[1] < 0)
                    aim[1] = self.enemy.mins[0];
                else
                    aim[1] = self.enemy.maxs[0];
            }

            Math3D.VectorMA(self.s.origin, range, dir, point);
            tr = GameBase.gi.Trace(self.s.origin, null, null, point, self, Defines.MASK_SHOT);
            if (tr.fraction < 1)
            {
                if (0 == tr.ent.takedamage)
                    return false;
                if ((tr.ent.svflags & Defines.SVF_MONSTER) != 0 || (tr.ent.client != null))
                    tr.ent = self.enemy;
            }

            Math3D.AngleVectors(self.s.angles, forward, right, up);
            Math3D.VectorMA(self.s.origin, range, forward, point);
            Math3D.VectorMA(point, aim[1], right, point);
            Math3D.VectorMA(point, aim[2], up, point);
            Math3D.VectorSubtract(point, self.enemy.s.origin, dir);
            GameCombat.T_Damage(tr.ent, self, self, dir, point, Globals.vec3_origin, damage, kick / 2, Defines.DAMAGE_NO_KNOCKBACK, Defines.MOD_HIT);
            if (0 == (tr.ent.svflags & Defines.SVF_MONSTER) && (null == tr.ent.client))
                return false;
            Math3D.VectorMA(self.enemy.absmin, 0.5F, self.enemy.size, v);
            Math3D.VectorSubtract(v, point, v);
            Math3D.VectorNormalize(v);
            Math3D.VectorMA(self.enemy.velocity, kick, v, self.enemy.velocity);
            if (self.enemy.velocity[2] > 0)
                self.enemy.groundentity = null;
            return true;
        }

        public static void Fire_lead(edict_t self, float[] start, float[] aimdir, int damage, int kick, int te_impact, int hspread, int vspread, int mod)
        {
            trace_t tr;
            float[] dir = new float[]{0, 0, 0};
            float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0}, up = new float[]{0, 0, 0};
            float[] end = new float[]{0, 0, 0};
            float r;
            float u;
            float[] water_start = new float[]{0, 0, 0};
            bool water = false;
            int content_mask = Defines.MASK_SHOT | Defines.MASK_WATER;
            tr = GameBase.gi.Trace(self.s.origin, null, null, start, self, Defines.MASK_SHOT);
            if (!(tr.fraction < 1))
            {
                Math3D.Vectoangles(aimdir, dir);
                Math3D.AngleVectors(dir, forward, right, up);
                r = Lib.Crandom() * hspread;
                u = Lib.Crandom() * vspread;
                Math3D.VectorMA(start, 8192, forward, end);
                Math3D.VectorMA(end, r, right, end);
                Math3D.VectorMA(end, u, up, end);
                if ((GameBase.gi.pointcontents.Pointcontents(start) & Defines.MASK_WATER) != 0)
                {
                    water = true;
                    Math3D.VectorCopy(start, water_start);
                    content_mask &= ~Defines.MASK_WATER;
                }

                tr = GameBase.gi.Trace(start, null, null, end, self, content_mask);
                if ((tr.contents & Defines.MASK_WATER) != 0)
                {
                    int color;
                    water = true;
                    Math3D.VectorCopy(tr.endpos, water_start);
                    if (!Math3D.VectorEquals(start, tr.endpos))
                    {
                        if ((tr.contents & Defines.CONTENTS_WATER) != 0)
                        {
                            if (Lib.Strcmp(tr.surface.name, "*brwater") == 0)
                                color = Defines.SPLASH_BROWN_WATER;
                            else
                                color = Defines.SPLASH_BLUE_WATER;
                        }
                        else if ((tr.contents & Defines.CONTENTS_SLIME) != 0)
                            color = Defines.SPLASH_SLIME;
                        else if ((tr.contents & Defines.CONTENTS_LAVA) != 0)
                            color = Defines.SPLASH_LAVA;
                        else
                            color = Defines.SPLASH_UNKNOWN;
                        if (color != Defines.SPLASH_UNKNOWN)
                        {
                            GameBase.gi.WriteByte(Defines.svc_temp_entity);
                            GameBase.gi.WriteByte(Defines.TE_SPLASH);
                            GameBase.gi.WriteByte(8);
                            GameBase.gi.WritePosition(tr.endpos);
                            GameBase.gi.WriteDir(tr.plane.normal);
                            GameBase.gi.WriteByte(color);
                            GameBase.gi.Multicast(tr.endpos, Defines.MULTICAST_PVS);
                        }

                        Math3D.VectorSubtract(end, start, dir);
                        Math3D.Vectoangles(dir, dir);
                        Math3D.AngleVectors(dir, forward, right, up);
                        r = Lib.Crandom() * hspread * 2;
                        u = Lib.Crandom() * vspread * 2;
                        Math3D.VectorMA(water_start, 8192, forward, end);
                        Math3D.VectorMA(end, r, right, end);
                        Math3D.VectorMA(end, u, up, end);
                    }

                    tr = GameBase.gi.Trace(water_start, null, null, end, self, Defines.MASK_SHOT);
                }
            }

            if (!((tr.surface != null) && 0 != (tr.surface.flags & Defines.SURF_SKY)))
            {
                if (tr.fraction < 1)
                {
                    if (tr.ent.takedamage != 0)
                    {
                        GameCombat.T_Damage(tr.ent, self, self, aimdir, tr.endpos, tr.plane.normal, damage, kick, Defines.DAMAGE_BULLET, mod);
                    }
                    else
                    {
                        if (!"sky".Equals(tr.surface.name))
                        {
                            GameBase.gi.WriteByte(Defines.svc_temp_entity);
                            GameBase.gi.WriteByte(te_impact);
                            GameBase.gi.WritePosition(tr.endpos);
                            GameBase.gi.WriteDir(tr.plane.normal);
                            GameBase.gi.Multicast(tr.endpos, Defines.MULTICAST_PVS);
                            if (self.client != null)
                                PlayerWeapon.PlayerNoise(self, tr.endpos, Defines.PNOISE_IMPACT);
                        }
                    }
                }
            }

            if (water)
            {
                float[] pos = new float[]{0, 0, 0};
                Math3D.VectorSubtract(tr.endpos, water_start, dir);
                Math3D.VectorNormalize(dir);
                Math3D.VectorMA(tr.endpos, -2, dir, pos);
                if ((GameBase.gi.pointcontents.Pointcontents(pos) & Defines.MASK_WATER) != 0)
                    Math3D.VectorCopy(pos, tr.endpos);
                else
                    tr = GameBase.gi.Trace(pos, null, null, water_start, tr.ent, Defines.MASK_WATER);
                Math3D.VectorAdd(water_start, tr.endpos, pos);
                Math3D.VectorScale(pos, 0.5F, pos);
                GameBase.gi.WriteByte(Defines.svc_temp_entity);
                GameBase.gi.WriteByte(Defines.TE_BUBBLETRAIL);
                GameBase.gi.WritePosition(water_start);
                GameBase.gi.WritePosition(tr.endpos);
                GameBase.gi.Multicast(pos, Defines.MULTICAST_PVS);
            }
        }

        public static void Fire_bullet(edict_t self, float[] start, float[] aimdir, int damage, int kick, int hspread, int vspread, int mod)
        {
            Fire_lead(self, start, aimdir, damage, kick, Defines.TE_GUNSHOT, hspread, vspread, mod);
        }

        public static void Fire_shotgun(edict_t self, float[] start, float[] aimdir, int damage, int kick, int hspread, int vspread, int count, int mod)
        {
            int i;
            for (i = 0; i < count; i++)
                Fire_lead(self, start, aimdir, damage, kick, Defines.TE_SHOTGUN, hspread, vspread, mod);
        }

        public static void Fire_blaster(edict_t self, float[] start, float[] dir, int damage, int speed, int effect, bool hyper)
        {
            edict_t bolt;
            trace_t tr;
            Math3D.VectorNormalize(dir);
            bolt = GameUtil.G_Spawn();
            bolt.svflags = Defines.SVF_DEADMONSTER;
            Math3D.VectorCopy(start, bolt.s.origin);
            Math3D.VectorCopy(start, bolt.s.old_origin);
            Math3D.Vectoangles(dir, bolt.s.angles);
            Math3D.VectorScale(dir, speed, bolt.velocity);
            bolt.movetype = Defines.MOVETYPE_FLYMISSILE;
            bolt.clipmask = Defines.MASK_SHOT;
            bolt.solid = Defines.SOLID_BBOX;
            bolt.s.effects |= effect;
            Math3D.VectorClear(bolt.mins);
            Math3D.VectorClear(bolt.maxs);
            bolt.s.modelindex = GameBase.gi.Modelindex("models/objects/laser/tris.md2");
            bolt.s.sound = GameBase.gi.Soundindex("misc/lasfly.wav");
            bolt.owner = self;
            bolt.touch = blaster_touch;
            bolt.nextthink = GameBase.level.time + 2;
            bolt.think = GameUtil.G_FreeEdictA;
            bolt.dmg = damage;
            bolt.classname = "bolt";
            if (hyper)
                bolt.spawnflags = 1;
            GameBase.gi.Linkentity(bolt);
            if (self.client != null)
                Check_dodge(self, bolt.s.origin, dir, speed);
            tr = GameBase.gi.Trace(self.s.origin, null, null, bolt.s.origin, bolt, Defines.MASK_SHOT);
            if (tr.fraction < 1)
            {
                Math3D.VectorMA(bolt.s.origin, -10, dir, bolt.s.origin);
                bolt.touch.Touch(bolt, tr.ent, GameBase.dummyplane, null);
            }
        }

        public static void Fire_grenade(edict_t self, float[] start, float[] aimdir, int damage, int speed, float timer, float damage_radius)
        {
            edict_t grenade;
            float[] dir = new float[]{0, 0, 0};
            float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0}, up = new float[]{0, 0, 0};
            Math3D.Vectoangles(aimdir, dir);
            Math3D.AngleVectors(dir, forward, right, up);
            grenade = GameUtil.G_Spawn();
            Math3D.VectorCopy(start, grenade.s.origin);
            Math3D.VectorScale(aimdir, speed, grenade.velocity);
            Math3D.VectorMA(grenade.velocity, 200F + Lib.Crandom() * 10F, up, grenade.velocity);
            Math3D.VectorMA(grenade.velocity, Lib.Crandom() * 10F, right, grenade.velocity);
            Math3D.VectorSet(grenade.avelocity, 300, 300, 300);
            grenade.movetype = Defines.MOVETYPE_BOUNCE;
            grenade.clipmask = Defines.MASK_SHOT;
            grenade.solid = Defines.SOLID_BBOX;
            grenade.s.effects |= Defines.EF_GRENADE;
            Math3D.VectorClear(grenade.mins);
            Math3D.VectorClear(grenade.maxs);
            grenade.s.modelindex = GameBase.gi.Modelindex("models/objects/grenade/tris.md2");
            grenade.owner = self;
            grenade.touch = Grenade_Touch;
            grenade.nextthink = GameBase.level.time + timer;
            grenade.think = Grenade_Explode;
            grenade.dmg = damage;
            grenade.dmg_radius = damage_radius;
            grenade.classname = "grenade";
            GameBase.gi.Linkentity(grenade);
        }

        public static void Fire_grenade2(edict_t self, float[] start, float[] aimdir, int damage, int speed, float timer, float damage_radius, bool held)
        {
            edict_t grenade;
            float[] dir = new float[]{0, 0, 0};
            float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0}, up = new float[]{0, 0, 0};
            Math3D.Vectoangles(aimdir, dir);
            Math3D.AngleVectors(dir, forward, right, up);
            grenade = GameUtil.G_Spawn();
            Math3D.VectorCopy(start, grenade.s.origin);
            Math3D.VectorScale(aimdir, speed, grenade.velocity);
            Math3D.VectorMA(grenade.velocity, 200F + Lib.Crandom() * 10F, up, grenade.velocity);
            Math3D.VectorMA(grenade.velocity, Lib.Crandom() * 10F, right, grenade.velocity);
            Math3D.VectorSet(grenade.avelocity, 300F, 300F, 300F);
            grenade.movetype = Defines.MOVETYPE_BOUNCE;
            grenade.clipmask = Defines.MASK_SHOT;
            grenade.solid = Defines.SOLID_BBOX;
            grenade.s.effects |= Defines.EF_GRENADE;
            Math3D.VectorClear(grenade.mins);
            Math3D.VectorClear(grenade.maxs);
            grenade.s.modelindex = GameBase.gi.Modelindex("models/objects/grenade2/tris.md2");
            grenade.owner = self;
            grenade.touch = Grenade_Touch;
            grenade.nextthink = GameBase.level.time + timer;
            grenade.think = Grenade_Explode;
            grenade.dmg = damage;
            grenade.dmg_radius = damage_radius;
            grenade.classname = "hgrenade";
            if (held)
                grenade.spawnflags = 3;
            else
                grenade.spawnflags = 1;
            grenade.s.sound = GameBase.gi.Soundindex("weapons/hgrenc1b.wav");
            if (timer <= 0)
                Grenade_Explode.Think(grenade);
            else
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, GameBase.gi.Soundindex("weapons/hgrent1a.wav"), 1, Defines.ATTN_NORM, 0);
                GameBase.gi.Linkentity(grenade);
            }
        }

        public static void Fire_rocket(edict_t self, float[] start, float[] dir, int damage, int speed, float damage_radius, int radius_damage)
        {
            edict_t rocket;
            rocket = GameUtil.G_Spawn();
            Math3D.VectorCopy(start, rocket.s.origin);
            Math3D.VectorCopy(dir, rocket.movedir);
            Math3D.Vectoangles(dir, rocket.s.angles);
            Math3D.VectorScale(dir, speed, rocket.velocity);
            rocket.movetype = Defines.MOVETYPE_FLYMISSILE;
            rocket.clipmask = Defines.MASK_SHOT;
            rocket.solid = Defines.SOLID_BBOX;
            rocket.s.effects |= Defines.EF_ROCKET;
            Math3D.VectorClear(rocket.mins);
            Math3D.VectorClear(rocket.maxs);
            rocket.s.modelindex = GameBase.gi.Modelindex("models/objects/rocket/tris.md2");
            rocket.owner = self;
            rocket.touch = rocket_touch;
            rocket.nextthink = GameBase.level.time + 8000 / speed;
            rocket.think = GameUtil.G_FreeEdictA;
            rocket.dmg = damage;
            rocket.radius_dmg = radius_damage;
            rocket.dmg_radius = damage_radius;
            rocket.s.sound = GameBase.gi.Soundindex("weapons/rockfly.wav");
            rocket.classname = "rocket";
            if (self.client != null)
                Check_dodge(self, rocket.s.origin, dir, speed);
            GameBase.gi.Linkentity(rocket);
        }

        public static void Fire_rail(edict_t self, float[] start, float[] aimdir, int damage, int kick)
        {
            float[] from = new float[]{0, 0, 0};
            float[] end = new float[]{0, 0, 0};
            trace_t tr = null;
            edict_t ignore;
            int mask;
            bool water;
            Math3D.VectorMA(start, 8192F, aimdir, end);
            Math3D.VectorCopy(start, from);
            ignore = self;
            water = false;
            mask = Defines.MASK_SHOT | Defines.CONTENTS_SLIME | Defines.CONTENTS_LAVA;
            while (ignore != null)
            {
                tr = GameBase.gi.Trace(from, null, null, end, ignore, mask);
                if ((tr.contents & (Defines.CONTENTS_SLIME | Defines.CONTENTS_LAVA)) != 0)
                {
                    mask &= ~(Defines.CONTENTS_SLIME | Defines.CONTENTS_LAVA);
                    water = true;
                }
                else
                {
                    if ((tr.ent.svflags & Defines.SVF_MONSTER) != 0 || (tr.ent.client != null) || (tr.ent.solid == Defines.SOLID_BBOX))
                        ignore = tr.ent;
                    else
                        ignore = null;
                    if ((tr.ent != self) && (tr.ent.takedamage != 0))
                        GameCombat.T_Damage(tr.ent, self, self, aimdir, tr.endpos, tr.plane.normal, damage, kick, 0, Defines.MOD_RAILGUN);
                }

                Math3D.VectorCopy(tr.endpos, from);
            }

            GameBase.gi.WriteByte(Defines.svc_temp_entity);
            GameBase.gi.WriteByte(Defines.TE_RAILTRAIL);
            GameBase.gi.WritePosition(start);
            GameBase.gi.WritePosition(tr.endpos);
            GameBase.gi.Multicast(self.s.origin, Defines.MULTICAST_PHS);
            if (water)
            {
                GameBase.gi.WriteByte(Defines.svc_temp_entity);
                GameBase.gi.WriteByte(Defines.TE_RAILTRAIL);
                GameBase.gi.WritePosition(start);
                GameBase.gi.WritePosition(tr.endpos);
                GameBase.gi.Multicast(tr.endpos, Defines.MULTICAST_PHS);
            }

            if (self.client != null)
                PlayerWeapon.PlayerNoise(self, tr.endpos, Defines.PNOISE_IMPACT);
        }

        public static void Fire_bfg(edict_t self, float[] start, float[] dir, int damage, int speed, float damage_radius)
        {
            edict_t bfg;
            bfg = GameUtil.G_Spawn();
            Math3D.VectorCopy(start, bfg.s.origin);
            Math3D.VectorCopy(dir, bfg.movedir);
            Math3D.Vectoangles(dir, bfg.s.angles);
            Math3D.VectorScale(dir, speed, bfg.velocity);
            bfg.movetype = Defines.MOVETYPE_FLYMISSILE;
            bfg.clipmask = Defines.MASK_SHOT;
            bfg.solid = Defines.SOLID_BBOX;
            bfg.s.effects |= Defines.EF_BFG | Defines.EF_ANIM_ALLFAST;
            Math3D.VectorClear(bfg.mins);
            Math3D.VectorClear(bfg.maxs);
            bfg.s.modelindex = GameBase.gi.Modelindex("sprites/s_bfg1.sp2");
            bfg.owner = self;
            bfg.touch = bfg_touch;
            bfg.nextthink = GameBase.level.time + 8000 / speed;
            bfg.think = GameUtil.G_FreeEdictA;
            bfg.radius_dmg = damage;
            bfg.dmg_radius = damage_radius;
            bfg.classname = "bfg blast";
            bfg.s.sound = GameBase.gi.Soundindex("weapons/bfg__l1a.wav");
            bfg.think = bfg_think;
            bfg.nextthink = GameBase.level.time + Defines.FRAMETIME;
            bfg.teammaster = bfg;
            bfg.teamchain = null;
            if (self.client != null)
                Check_dodge(self, bfg.s.origin, dir, speed);
            GameBase.gi.Linkentity(bfg);
        }
    }
}
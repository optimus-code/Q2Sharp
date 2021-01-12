using Q2Sharp.Game.Monsters;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class GameTurret
    {
        public static void AnglesNormalize(float[] vec)
        {
            while (vec[0] > 360)
                vec[0] -= 360;
            while (vec[0] < 0)
                vec[0] += 360;
            while (vec[1] > 360)
                vec[1] -= 360;
            while (vec[1] < 0)
                vec[1] += 360;
        }

        public static float SnapToEights(float x)
        {
            var result = x * 8f;
            if ( result > 0)
                result += 0.5f;
            else
                result -= 0.5f;
            return 0.125F * (int) result;
        }

        public static void Turret_breach_fire(edict_t self)
        {
            float[] f = new float[]{0, 0, 0}, r = new float[]{0, 0, 0}, u = new float[]{0, 0, 0};
            float[] start = new float[]{0, 0, 0};
            int damage;
            int speed;
            Math3D.AngleVectors(self.s.angles, f, r, u);
            Math3D.VectorMA(self.s.origin, self.move_origin[0], f, start);
            Math3D.VectorMA(start, self.move_origin[1], r, start);
            Math3D.VectorMA(start, self.move_origin[2], u, start);
            damage = (int)(100 + Lib.Random() * 50);
            speed = (int)(550 + 50 * GameBase.skill.value);
            GameWeapon.Fire_rocket(self.teammaster.owner, start, f, damage, speed, 150, damage);
            GameBase.gi.Positioned_sound(start, self, Defines.CHAN_WEAPON, GameBase.gi.Soundindex("weapons/rocklf1a.wav"), 1, Defines.ATTN_NORM, 0);
        }

        public static void SP_turret_breach(edict_t self)
        {
            self.solid = Defines.SOLID_BSP;
            self.movetype = Defines.MOVETYPE_PUSH;
            GameBase.gi.Setmodel(self, self.model);
            if (self.speed == 0)
                self.speed = 50;
            if (self.dmg == 0)
                self.dmg = 10;
            if (GameBase.st.minpitch == 0)
                GameBase.st.minpitch = -30;
            if (GameBase.st.maxpitch == 0)
                GameBase.st.maxpitch = 30;
            if (GameBase.st.maxyaw == 0)
                GameBase.st.maxyaw = 360;
            self.pos1[Defines.PITCH] = -1 * GameBase.st.minpitch;
            self.pos1[Defines.YAW] = GameBase.st.minyaw;
            self.pos2[Defines.PITCH] = -1 * GameBase.st.maxpitch;
            self.pos2[Defines.YAW] = GameBase.st.maxyaw;
            self.ideal_yaw = self.s.angles[Defines.YAW];
            self.move_angles[Defines.YAW] = self.ideal_yaw;
            self.blocked = turret_blocked;
            self.think = turret_breach_finish_init;
            self.nextthink = GameBase.level.time + Defines.FRAMETIME;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_turret_base(edict_t self)
        {
            self.solid = Defines.SOLID_BSP;
            self.movetype = Defines.MOVETYPE_PUSH;
            GameBase.gi.Setmodel(self, self.model);
            self.blocked = turret_blocked;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_turret_driver(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            self.movetype = Defines.MOVETYPE_PUSH;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/infantry/tris.md2");
            Math3D.VectorSet(self.mins, -16, -16, -24);
            Math3D.VectorSet(self.maxs, 16, 16, 32);
            self.health = 100;
            self.gib_health = 0;
            self.mass = 200;
            self.viewheight = 24;
            self.die = turret_driver_die;
            self.monsterinfo.stand = M_Infantry.infantry_stand;
            self.flags |= Defines.FL_NO_KNOCKBACK;
            GameBase.level.total_monsters++;
            self.svflags |= Defines.SVF_MONSTER;
            self.s.renderfx |= Defines.RF_FRAMELERP;
            self.takedamage = Defines.DAMAGE_AIM;
            self.use = GameUtil.monster_use;
            self.clipmask = Defines.MASK_MONSTERSOLID;
            Math3D.VectorCopy(self.s.origin, self.s.old_origin);
            self.monsterinfo.aiflags |= Defines.AI_STAND_GROUND | Defines.AI_DUCKED;
            if (GameBase.st.item != null)
            {
                self.item = GameItems.FindItemByClassname(GameBase.st.item);
                if (self.item == null)
                    GameBase.gi.Dprintf(self.classname + " at " + Lib.Vtos(self.s.origin) + " has bad item: " + GameBase.st.item + "\\n");
            }

            self.think = turret_driver_link;
            self.nextthink = GameBase.level.time + Defines.FRAMETIME;
            GameBase.gi.Linkentity(self);
        }

        static EntBlockedAdapter turret_blocked = new AnonymousEntBlockedAdapter();
        private sealed class AnonymousEntBlockedAdapter : EntBlockedAdapter
		{			
            public override string GetID()
            {
                return "turret_blocked";
            }

            public override void Blocked(edict_t self, edict_t other)
            {
                edict_t attacker;
                if (other.takedamage != 0)
                {
                    if (self.teammaster.owner != null)
                        attacker = self.teammaster.owner;
                    else
                        attacker = self.teammaster;
                    GameCombat.T_Damage(other, self, attacker, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, self.teammaster.dmg, 10, 0, Defines.MOD_CRUSH);
                }
            }
        }

        static EntThinkAdapter turret_breach_think = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "turret_breach_think";
            }

            public override bool Think(edict_t self)
            {
                edict_t ent;
                float[] current_angles = new float[]{0, 0, 0};
                float[] delta = new float[]{0, 0, 0};
                Math3D.VectorCopy(self.s.angles, current_angles);
                AnglesNormalize(current_angles);
                AnglesNormalize(self.move_angles);
                if (self.move_angles[Defines.PITCH] > 180)
                    self.move_angles[Defines.PITCH] -= 360;
                if (self.move_angles[Defines.PITCH] > self.pos1[Defines.PITCH])
                    self.move_angles[Defines.PITCH] = self.pos1[Defines.PITCH];
                else if (self.move_angles[Defines.PITCH] < self.pos2[Defines.PITCH])
                    self.move_angles[Defines.PITCH] = self.pos2[Defines.PITCH];
                if ((self.move_angles[Defines.YAW] < self.pos1[Defines.YAW]) || (self.move_angles[Defines.YAW] > self.pos2[Defines.YAW]))
                {
                    float dmin, dmax;
                    dmin = Math.Abs(self.pos1[Defines.YAW] - self.move_angles[Defines.YAW]);
                    if (dmin < -180)
                        dmin += 360;
                    else if (dmin > 180)
                        dmin -= 360;
                    dmax = Math.Abs(self.pos2[Defines.YAW] - self.move_angles[Defines.YAW]);
                    if (dmax < -180)
                        dmax += 360;
                    else if (dmax > 180)
                        dmax -= 360;
                    if (Math.Abs(dmin) < Math.Abs(dmax))
                        self.move_angles[Defines.YAW] = self.pos1[Defines.YAW];
                    else
                        self.move_angles[Defines.YAW] = self.pos2[Defines.YAW];
                }

                Math3D.VectorSubtract(self.move_angles, current_angles, delta);
                if (delta[0] < -180)
                    delta[0] += 360;
                else if (delta[0] > 180)
                    delta[0] -= 360;
                if (delta[1] < -180)
                    delta[1] += 360;
                else if (delta[1] > 180)
                    delta[1] -= 360;
                delta[2] = 0;
                if (delta[0] > self.speed * Defines.FRAMETIME)
                    delta[0] = self.speed * Defines.FRAMETIME;
                if (delta[0] < -1 * self.speed * Defines.FRAMETIME)
                    delta[0] = -1 * self.speed * Defines.FRAMETIME;
                if (delta[1] > self.speed * Defines.FRAMETIME)
                    delta[1] = self.speed * Defines.FRAMETIME;
                if (delta[1] < -1 * self.speed * Defines.FRAMETIME)
                    delta[1] = -1 * self.speed * Defines.FRAMETIME;
                Math3D.VectorScale(delta, 1F / Defines.FRAMETIME, self.avelocity);
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                for (ent = self.teammaster; ent != null; ent = ent.teamchain)
                    ent.avelocity[1] = self.avelocity[1];
                if (self.owner != null)
                {
                    float angle;
                    float target_z;
                    float diff;
                    float[] target = new float[]{0, 0, 0};
                    float[] dir = new float[]{0, 0, 0};
                    self.owner.avelocity[0] = self.avelocity[0];
                    self.owner.avelocity[1] = self.avelocity[1];
                    angle = self.s.angles[1] + self.owner.move_origin[1];
                    angle *= (float)(Math.PI * 2 / 360);
                    target[0] = GameTurret.SnapToEights((float)(self.s.origin[0] + Math.Cos(angle) * self.owner.move_origin[0]));
                    target[1] = GameTurret.SnapToEights((float)(self.s.origin[1] + Math.Sin(angle) * self.owner.move_origin[0]));
                    target[2] = self.owner.s.origin[2];
                    Math3D.VectorSubtract(target, self.owner.s.origin, dir);
                    self.owner.velocity[0] = dir[0] * 1F / Defines.FRAMETIME;
                    self.owner.velocity[1] = dir[1] * 1F / Defines.FRAMETIME;
                    angle = self.s.angles[Defines.PITCH] * (float)(Math.PI * 2F / 360F);
                    target_z = GameTurret.SnapToEights((float)(self.s.origin[2] + self.owner.move_origin[0] * Math.Tan(angle) + self.owner.move_origin[2]));
                    diff = target_z - self.owner.s.origin[2];
                    self.owner.velocity[2] = diff * 1F / Defines.FRAMETIME;
                    if ((self.spawnflags & 65536) != 0)
                    {
                        Turret_breach_fire(self);
                        self.spawnflags &= ~65536;
                    }
                }

                return true;
            }
        }

        static EntThinkAdapter turret_breach_finish_init = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "turret_breach_finish_init";
            }

            public override bool Think(edict_t self)
            {
                if (self.target == null)
                {
                    GameBase.gi.Dprintf(self.classname + " at " + Lib.Vtos(self.s.origin) + " needs a target\\n");
                }
                else
                {
                    self.target_ent = GameBase.G_PickTarget(self.target);
                    Math3D.VectorSubtract(self.target_ent.s.origin, self.s.origin, self.move_origin);
                    GameUtil.G_FreeEdict(self.target_ent);
                }

                self.teammaster.dmg = self.dmg;
                self.think = turret_breach_think;
                self.think.Think(self);
                return true;
            }
        }

        static EntDieAdapter turret_driver_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "turret_driver_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                self.target_ent.move_angles[0] = 0;
                self.teammaster = null;
                self.flags &= ~Defines.FL_TEAMSLAVE;
                self.target_ent.owner = null;
                self.target_ent.teammaster.owner = null;
                M_Infantry.infantry_die.Die(self, inflictor, attacker, damage, null);
            }
        }

        static EntThinkAdapter turret_driver_think = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "turret_driver_think";
            }

            public override bool Think(edict_t self)
            {
                float[] target = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                float reaction_time;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                if (self.enemy != null && (!self.enemy.inuse || self.enemy.health <= 0))
                    self.enemy = null;
                if (null == self.enemy)
                {
                    if (!GameUtil.FindTarget(self))
                        return true;
                    self.monsterinfo.trail_time = GameBase.level.time;
                    self.monsterinfo.aiflags &= ~Defines.AI_LOST_SIGHT;
                }
                else
                {
                    if (GameUtil.Visible(self, self.enemy))
                    {
                        if ((self.monsterinfo.aiflags & Defines.AI_LOST_SIGHT) != 0)
                        {
                            self.monsterinfo.trail_time = GameBase.level.time;
                            self.monsterinfo.aiflags &= ~Defines.AI_LOST_SIGHT;
                        }
                    }
                    else
                    {
                        self.monsterinfo.aiflags |= Defines.AI_LOST_SIGHT;
                        return true;
                    }
                }

                Math3D.VectorCopy(self.enemy.s.origin, target);
                target[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(target, self.target_ent.s.origin, dir);
                Math3D.Vectoangles(dir, self.target_ent.move_angles);
                if (GameBase.level.time < self.monsterinfo.attack_finished)
                    return true;
                reaction_time = (3 - GameBase.skill.value) * 1F;
                if ((GameBase.level.time - self.monsterinfo.trail_time) < reaction_time)
                    return true;
                self.monsterinfo.attack_finished = GameBase.level.time + reaction_time + 1F;
                self.target_ent.spawnflags |= 65536;
                return true;
            }
        }

        public static EntThinkAdapter turret_driver_link = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "turret_driver_link";
            }

            public override bool Think(edict_t self)
            {
                float[] vec = new float[]{0, 0, 0};
                self.think = turret_driver_think;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                self.target_ent = GameBase.G_PickTarget(self.target);
                self.target_ent.owner = self;
                self.target_ent.teammaster.owner = self;
                Math3D.VectorCopy(self.target_ent.s.angles, self.s.angles);
                vec[0] = self.target_ent.s.origin[0] - self.s.origin[0];
                vec[1] = self.target_ent.s.origin[1] - self.s.origin[1];
                vec[2] = 0;
                self.move_origin[0] = Math3D.VectorLength(vec);
                Math3D.VectorSubtract(self.s.origin, self.target_ent.s.origin, vec);
                Math3D.Vectoangles(vec, vec);
                AnglesNormalize(vec);
                self.move_origin[1] = vec[1];
                self.move_origin[2] = self.s.origin[2] - self.target_ent.s.origin[2];
               
                self.teammaster = self.target_ent.teammaster;
                self.flags |= Defines.FL_TEAMSLAVE;
                return true;
            }
        }
    }
}
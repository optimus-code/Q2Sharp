using Jake2.Client;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class GameMisc
    {
        public static void SP_path_corner(edict_t self)
        {
            if (self.targetname == null)
            {
                GameBase.gi.Dprintf("path_corner with no targetname at " + Lib.Vtos(self.s.origin) + "\\n");
                GameUtil.G_FreeEdict(self);
                return;
            }

            self.solid = Defines.SOLID_TRIGGER;
            self.touch = path_corner_touch;
            Math3D.VectorSet(self.mins, -8, -8, -8);
            Math3D.VectorSet(self.maxs, 8, 8, 8);
            self.svflags |= Defines.SVF_NOCLIENT;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_point_combat(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            self.solid = Defines.SOLID_TRIGGER;
            self.touch = point_combat_touch;
            Math3D.VectorSet(self.mins, -8, -8, -16);
            Math3D.VectorSet(self.maxs, 8, 8, 16);
            self.svflags = Defines.SVF_NOCLIENT;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_viewthing(edict_t ent)
        {
            GameBase.gi.Dprintf("viewthing spawned\\n");
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            ent.s.renderfx = Defines.RF_FRAMELERP;
            Math3D.VectorSet(ent.mins, -16, -16, -24);
            Math3D.VectorSet(ent.maxs, 16, 16, 32);
            ent.s.modelindex = GameBase.gi.Modelindex("models/objects/banner/tris.md2");
            GameBase.gi.Linkentity(ent);
            ent.nextthink = GameBase.level.time + 0.5F;
            ent.think = TH_viewthing;
            return;
        }

        public static void SP_info_null(edict_t self)
        {
            GameUtil.G_FreeEdict(self);
        }

        public static void SP_info_notnull(edict_t self)
        {
            Math3D.VectorCopy(self.s.origin, self.absmin);
            Math3D.VectorCopy(self.s.origin, self.absmax);
        }

        public static void SP_light(edict_t self)
        {
            if (null == self.targetname || GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            if (self.style >= 32)
            {
                self.use = light_use;
                if ((self.spawnflags & START_OFF) != 0)
                    GameBase.gi.Configstring(Defines.CS_LIGHTS + self.style, "a");
                else
                    GameBase.gi.Configstring(Defines.CS_LIGHTS + self.style, "m");
            }
        }

        public static void SP_func_wall(edict_t self)
        {
            self.movetype = Defines.MOVETYPE_PUSH;
            GameBase.gi.Setmodel(self, self.model);
            if ((self.spawnflags & 8) != 0)
                self.s.effects |= Defines.EF_ANIM_ALL;
            if ((self.spawnflags & 16) != 0)
                self.s.effects |= Defines.EF_ANIM_ALLFAST;
            if ((self.spawnflags & 7) == 0)
            {
                self.solid = Defines.SOLID_BSP;
                GameBase.gi.Linkentity(self);
                return;
            }

            if (0 == (self.spawnflags & 1))
            {
                GameBase.gi.Dprintf("func_wall missing TRIGGER_SPAWN\\n");
                self.spawnflags |= 1;
            }

            if ((self.spawnflags & 4) != 0)
            {
                if (0 == (self.spawnflags & 2))
                {
                    GameBase.gi.Dprintf("func_wall START_ON without TOGGLE\\n");
                    self.spawnflags |= 2;
                }
            }

            self.use = func_wall_use;
            if ((self.spawnflags & 4) != 0)
            {
                self.solid = Defines.SOLID_BSP;
            }
            else
            {
                self.solid = Defines.SOLID_NOT;
                self.svflags |= Defines.SVF_NOCLIENT;
            }

            GameBase.gi.Linkentity(self);
        }

        public static void SP_func_object(edict_t self)
        {
            GameBase.gi.Setmodel(self, self.model);
            self.mins[0] += 1;
            self.mins[1] += 1;
            self.mins[2] += 1;
            self.maxs[0] -= 1;
            self.maxs[1] -= 1;
            self.maxs[2] -= 1;
            if (self.dmg == 0)
                self.dmg = 100;
            if (self.spawnflags == 0)
            {
                self.solid = Defines.SOLID_BSP;
                self.movetype = Defines.MOVETYPE_PUSH;
                self.think = func_object_release;
                self.nextthink = GameBase.level.time + 2 * Defines.FRAMETIME;
            }
            else
            {
                self.solid = Defines.SOLID_NOT;
                self.movetype = Defines.MOVETYPE_PUSH;
                self.use = func_object_use;
                self.svflags |= Defines.SVF_NOCLIENT;
            }

            if ((self.spawnflags & 2) != 0)
                self.s.effects |= Defines.EF_ANIM_ALL;
            if ((self.spawnflags & 4) != 0)
                self.s.effects |= Defines.EF_ANIM_ALLFAST;
            self.clipmask = Defines.MASK_MONSTERSOLID;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_func_explosive(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            self.movetype = Defines.MOVETYPE_PUSH;
            GameBase.gi.Modelindex("models/objects/debris1/tris.md2");
            GameBase.gi.Modelindex("models/objects/debris2/tris.md2");
            GameBase.gi.Setmodel(self, self.model);
            if ((self.spawnflags & 1) != 0)
            {
                self.svflags |= Defines.SVF_NOCLIENT;
                self.solid = Defines.SOLID_NOT;
                self.use = func_explosive_spawn;
            }
            else
            {
                self.solid = Defines.SOLID_BSP;
                if (self.targetname != null)
                    self.use = func_explosive_use;
            }

            if ((self.spawnflags & 2) != 0)
                self.s.effects |= Defines.EF_ANIM_ALL;
            if ((self.spawnflags & 4) != 0)
                self.s.effects |= Defines.EF_ANIM_ALLFAST;
            if (self.use != func_explosive_use)
            {
                if (self.health == 0)
                    self.health = 100;
                self.die = func_explosive_explode;
                self.takedamage = Defines.DAMAGE_YES;
            }

            GameBase.gi.Linkentity(self);
        }

        public static void SP_misc_explobox(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            GameBase.gi.Modelindex("models/objects/debris1/tris.md2");
            GameBase.gi.Modelindex("models/objects/debris2/tris.md2");
            GameBase.gi.Modelindex("models/objects/debris3/tris.md2");
            self.solid = Defines.SOLID_BBOX;
            self.movetype = Defines.MOVETYPE_STEP;
            self.model = "models/objects/barrels/tris.md2";
            self.s.modelindex = GameBase.gi.Modelindex(self.model);
            Math3D.VectorSet(self.mins, -16, -16, 0);
            Math3D.VectorSet(self.maxs, 16, 16, 40);
            if (self.mass == 0)
                self.mass = 400;
            if (0 == self.health)
                self.health = 10;
            if (0 == self.dmg)
                self.dmg = 150;
            self.die = barrel_delay;
            self.takedamage = Defines.DAMAGE_YES;
            self.monsterinfo.aiflags = Defines.AI_NOSTEP;
            self.touch = barrel_touch;
            self.think = M.M_droptofloor;
            self.nextthink = GameBase.level.time + 2 * Defines.FRAMETIME;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_misc_blackhole(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_NOT;
            Math3D.VectorSet(ent.mins, -64, -64, 0);
            Math3D.VectorSet(ent.maxs, 64, 64, 8);
            ent.s.modelindex = GameBase.gi.Modelindex("models/objects/black/tris.md2");
            ent.s.renderfx = Defines.RF_TRANSLUCENT;
            ent.use = misc_blackhole_use;
            ent.think = misc_blackhole_think;
            ent.nextthink = GameBase.level.time + 2 * Defines.FRAMETIME;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_eastertank(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            Math3D.VectorSet(ent.mins, -32, -32, -16);
            Math3D.VectorSet(ent.maxs, 32, 32, 32);
            ent.s.modelindex = GameBase.gi.Modelindex("models/monsters/tank/tris.md2");
            ent.s.frame = 254;
            ent.think = misc_eastertank_think;
            ent.nextthink = GameBase.level.time + 2 * Defines.FRAMETIME;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_easterchick(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            Math3D.VectorSet(ent.mins, -32, -32, 0);
            Math3D.VectorSet(ent.maxs, 32, 32, 32);
            ent.s.modelindex = GameBase.gi.Modelindex("models/monsters/bitch/tris.md2");
            ent.s.frame = 208;
            ent.think = misc_easterchick_think;
            ent.nextthink = GameBase.level.time + 2 * Defines.FRAMETIME;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_easterchick2(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            Math3D.VectorSet(ent.mins, -32, -32, 0);
            Math3D.VectorSet(ent.maxs, 32, 32, 32);
            ent.s.modelindex = GameBase.gi.Modelindex("models/monsters/bitch/tris.md2");
            ent.s.frame = 248;
            ent.think = misc_easterchick2_think;
            ent.nextthink = GameBase.level.time + 2 * Defines.FRAMETIME;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_monster_commander_body(edict_t self)
        {
            self.movetype = Defines.MOVETYPE_NONE;
            self.solid = Defines.SOLID_BBOX;
            self.model = "models/monsters/commandr/tris.md2";
            self.s.modelindex = GameBase.gi.Modelindex(self.model);
            Math3D.VectorSet(self.mins, -32, -32, 0);
            Math3D.VectorSet(self.maxs, 32, 32, 48);
            self.use = commander_body_use;
            self.takedamage = Defines.DAMAGE_YES;
            self.flags = Defines.FL_GODMODE;
            self.s.renderfx |= Defines.RF_FRAMELERP;
            GameBase.gi.Linkentity(self);
            GameBase.gi.Soundindex("tank/thud.wav");
            GameBase.gi.Soundindex("tank/pain.wav");
            self.think = commander_body_drop;
            self.nextthink = GameBase.level.time + 5 * Defines.FRAMETIME;
        }

        public static void SP_misc_banner(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_NOT;
            ent.s.modelindex = GameBase.gi.Modelindex("models/objects/banner/tris.md2");
            ent.s.frame = Lib.Rand() % 16;
            GameBase.gi.Linkentity(ent);
            ent.think = misc_banner_think;
            ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
        }

        public static void SP_misc_deadsoldier(edict_t ent)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(ent);
                return;
            }

            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            ent.s.modelindex = GameBase.gi.Modelindex("models/deadbods/dude/tris.md2");
            if ((ent.spawnflags & 2) != 0)
                ent.s.frame = 1;
            else if ((ent.spawnflags & 4) != 0)
                ent.s.frame = 2;
            else if ((ent.spawnflags & 8) != 0)
                ent.s.frame = 3;
            else if ((ent.spawnflags & 16) != 0)
                ent.s.frame = 4;
            else if ((ent.spawnflags & 32) != 0)
                ent.s.frame = 5;
            else
                ent.s.frame = 0;
            Math3D.VectorSet(ent.mins, -16, -16, 0);
            Math3D.VectorSet(ent.maxs, 16, 16, 16);
            ent.deadflag = Defines.DEAD_DEAD;
            ent.takedamage = Defines.DAMAGE_YES;
            ent.svflags |= Defines.SVF_MONSTER | Defines.SVF_DEADMONSTER;
            ent.die = misc_deadsoldier_die;
            ent.monsterinfo.aiflags |= Defines.AI_GOOD_GUY;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_viper(edict_t ent)
        {
            if (null == ent.target)
            {
                GameBase.gi.Dprintf("misc_viper without a target at " + Lib.Vtos(ent.absmin) + "\\n");
                GameUtil.G_FreeEdict(ent);
                return;
            }

            if (0 == ent.speed)
                ent.speed = 300;
            ent.movetype = Defines.MOVETYPE_PUSH;
            ent.solid = Defines.SOLID_NOT;
            ent.s.modelindex = GameBase.gi.Modelindex("models/ships/viper/tris.md2");
            Math3D.VectorSet(ent.mins, -16, -16, 0);
            Math3D.VectorSet(ent.maxs, 16, 16, 32);
            ent.think = GameFunc.func_train_find;
            ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
            ent.use = misc_viper_use;
            ent.svflags |= Defines.SVF_NOCLIENT;
            ent.moveinfo.accel = ent.moveinfo.decel = ent.moveinfo.speed = ent.speed;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_bigviper(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            Math3D.VectorSet(ent.mins, -176, -120, -24);
            Math3D.VectorSet(ent.maxs, 176, 120, 72);
            ent.s.modelindex = GameBase.gi.Modelindex("models/ships/bigviper/tris.md2");
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_viper_bomb(edict_t self)
        {
            self.movetype = Defines.MOVETYPE_NONE;
            self.solid = Defines.SOLID_NOT;
            Math3D.VectorSet(self.mins, -8, -8, -8);
            Math3D.VectorSet(self.maxs, 8, 8, 8);
            self.s.modelindex = GameBase.gi.Modelindex("models/objects/bomb/tris.md2");
            if (self.dmg == 0)
                self.dmg = 1000;
            self.use = misc_viper_bomb_use;
            self.svflags |= Defines.SVF_NOCLIENT;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_misc_strogg_ship(edict_t ent)
        {
            if (null == ent.target)
            {
                GameBase.gi.Dprintf(ent.classname + " without a target at " + Lib.Vtos(ent.absmin) + "\\n");
                GameUtil.G_FreeEdict(ent);
                return;
            }

            if (0 == ent.speed)
                ent.speed = 300;
            ent.movetype = Defines.MOVETYPE_PUSH;
            ent.solid = Defines.SOLID_NOT;
            ent.s.modelindex = GameBase.gi.Modelindex("models/ships/strogg1/tris.md2");
            Math3D.VectorSet(ent.mins, -16, -16, 0);
            Math3D.VectorSet(ent.maxs, 16, 16, 32);
            ent.think = GameFunc.func_train_find;
            ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
            ent.use = misc_strogg_ship_use;
            ent.svflags |= Defines.SVF_NOCLIENT;
            ent.moveinfo.accel = ent.moveinfo.decel = ent.moveinfo.speed = ent.speed;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_satellite_dish(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            Math3D.VectorSet(ent.mins, -64, -64, 0);
            Math3D.VectorSet(ent.maxs, 64, 64, 128);
            ent.s.modelindex = GameBase.gi.Modelindex("models/objects/satellite/tris.md2");
            ent.use = misc_satellite_dish_use;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_light_mine1(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            ent.s.modelindex = GameBase.gi.Modelindex("models/objects/minelite/light1/tris.md2");
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_light_mine2(edict_t ent)
        {
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.solid = Defines.SOLID_BBOX;
            ent.s.modelindex = GameBase.gi.Modelindex("models/objects/minelite/light2/tris.md2");
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_gib_arm(edict_t ent)
        {
            GameBase.gi.Setmodel(ent, "models/objects/gibs/arm/tris.md2");
            ent.solid = Defines.SOLID_NOT;
            ent.s.effects |= Defines.EF_GIB;
            ent.takedamage = Defines.DAMAGE_YES;
            ent.die = gib_die;
            ent.movetype = Defines.MOVETYPE_TOSS;
            ent.svflags |= Defines.SVF_MONSTER;
            ent.deadflag = Defines.DEAD_DEAD;
            ent.avelocity[0] = Lib.Random() * 200;
            ent.avelocity[1] = Lib.Random() * 200;
            ent.avelocity[2] = Lib.Random() * 200;
            ent.think = GameUtil.G_FreeEdictA;
            ent.nextthink = GameBase.level.time + 30;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_gib_leg(edict_t ent)
        {
            GameBase.gi.Setmodel(ent, "models/objects/gibs/leg/tris.md2");
            ent.solid = Defines.SOLID_NOT;
            ent.s.effects |= Defines.EF_GIB;
            ent.takedamage = Defines.DAMAGE_YES;
            ent.die = gib_die;
            ent.movetype = Defines.MOVETYPE_TOSS;
            ent.svflags |= Defines.SVF_MONSTER;
            ent.deadflag = Defines.DEAD_DEAD;
            ent.avelocity[0] = Lib.Random() * 200;
            ent.avelocity[1] = Lib.Random() * 200;
            ent.avelocity[2] = Lib.Random() * 200;
            ent.think = GameUtil.G_FreeEdictA;
            ent.nextthink = GameBase.level.time + 30;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_misc_gib_head(edict_t ent)
        {
            GameBase.gi.Setmodel(ent, "models/objects/gibs/head/tris.md2");
            ent.solid = Defines.SOLID_NOT;
            ent.s.effects |= Defines.EF_GIB;
            ent.takedamage = Defines.DAMAGE_YES;
            ent.die = gib_die;
            ent.movetype = Defines.MOVETYPE_TOSS;
            ent.svflags |= Defines.SVF_MONSTER;
            ent.deadflag = Defines.DEAD_DEAD;
            ent.avelocity[0] = Lib.Random() * 200;
            ent.avelocity[1] = Lib.Random() * 200;
            ent.avelocity[2] = Lib.Random() * 200;
            ent.think = GameUtil.G_FreeEdictA;
            ent.nextthink = GameBase.level.time + 30;
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_target_character(edict_t self)
        {
            self.movetype = Defines.MOVETYPE_PUSH;
            GameBase.gi.Setmodel(self, self.model);
            self.solid = Defines.SOLID_BSP;
            self.s.frame = 12;
            GameBase.gi.Linkentity(self);
            return;
        }

        public static void SP_target_string(edict_t self)
        {
            if (self.message == null)
                self.message = "";
            self.use = target_string_use;
        }

        public static void Func_clock_reset(edict_t self)
        {
            self.activator = null;
            if ((self.spawnflags & 1) != 0)
            {
                self.health = 0;
                self.wait = self.count;
            }
            else if ((self.spawnflags & 2) != 0)
            {
                self.health = self.count;
                self.wait = 0;
            }
        }

        public static void Func_clock_format_countdown(edict_t self)
        {
            if (self.style == 0)
            {
                self.message = "" + self.health;
                return;
            }

            if (self.style == 1)
            {
                self.message = "" + self.health / 60 + ":" + self.health % 60;
                return;
            }

            if (self.style == 2)
            {
                self.message = "" + self.health / 3600 + ":" + (self.health - (self.health / 3600) * 3600) / 60 + ":" + self.health % 60;
                return;
            }
        }

        public static void SP_func_clock(edict_t self)
        {
            if (self.target == null)
            {
                GameBase.gi.Dprintf(self.classname + " with no target at " + Lib.Vtos(self.s.origin) + "\\n");
                GameUtil.G_FreeEdict(self);
                return;
            }

            if ((self.spawnflags & 2) != 0 && (0 == self.count))
            {
                GameBase.gi.Dprintf(self.classname + " with no count at " + Lib.Vtos(self.s.origin) + "\\n");
                GameUtil.G_FreeEdict(self);
                return;
            }

            if ((self.spawnflags & 1) != 0 && (0 == self.count))
                self.count = 60 * 60;
            Func_clock_reset(self);
            self.message = new string( "" );
            self.think = func_clock_think;
            if ((self.spawnflags & 4) != 0)
                self.use = func_clock_use;
            else
                self.nextthink = GameBase.level.time + 1;
        }

        public static void SP_misc_teleporter(edict_t ent)
        {
            edict_t trig;
            if (ent.target == null)
            {
                GameBase.gi.Dprintf("teleporter without a target.\\n");
                GameUtil.G_FreeEdict(ent);
                return;
            }

            GameBase.gi.Setmodel(ent, "models/objects/dmspot/tris.md2");
            ent.s.skinnum = 1;
            ent.s.effects = Defines.EF_TELEPORTER;
            ent.s.sound = GameBase.gi.Soundindex("world/amb10.wav");
            ent.solid = Defines.SOLID_BBOX;
            Math3D.VectorSet(ent.mins, -32, -32, -24);
            Math3D.VectorSet(ent.maxs, 32, 32, -16);
            GameBase.gi.Linkentity(ent);
            trig = GameUtil.G_Spawn();
            trig.touch = teleporter_touch;
            trig.solid = Defines.SOLID_TRIGGER;
            trig.target = ent.target;
            trig.owner = ent;
            Math3D.VectorCopy(ent.s.origin, trig.s.origin);
            Math3D.VectorSet(trig.mins, -8, -8, 8);
            Math3D.VectorSet(trig.maxs, 8, 8, 24);
            GameBase.gi.Linkentity(trig);
        }

        public static void VelocityForDamage(int damage, float[] v)
        {
            v[0] = 100F * Lib.Crandom();
            v[1] = 100F * Lib.Crandom();
            v[2] = 200F + 100F * Lib.Random();
            if (damage < 50)
                Math3D.VectorScale(v, 0.7F, v);
            else
                Math3D.VectorScale(v, 1.2F, v);
        }

        public static void BecomeExplosion1(edict_t self)
        {
            GameBase.gi.WriteByte(Defines.svc_temp_entity);
            GameBase.gi.WriteByte(Defines.TE_EXPLOSION1);
            GameBase.gi.WritePosition(self.s.origin);
            GameBase.gi.Multicast(self.s.origin, Defines.MULTICAST_PVS);
            GameUtil.G_FreeEdict(self);
        }

        public static void BecomeExplosion2(edict_t self)
        {
            GameBase.gi.WriteByte(Defines.svc_temp_entity);
            GameBase.gi.WriteByte(Defines.TE_EXPLOSION2);
            GameBase.gi.WritePosition(self.s.origin);
            GameBase.gi.Multicast(self.s.origin, Defines.MULTICAST_PVS);
            GameUtil.G_FreeEdict(self);
        }

        public static void ThrowGib(edict_t self, string gibname, int damage, int type)
        {
            edict_t gib;
            float[] vd = new float[]{0, 0, 0};
            float[] origin = new float[]{0, 0, 0};
            float[] size = new float[]{0, 0, 0};
            float vscale;
            gib = GameUtil.G_Spawn();
            Math3D.VectorScale(self.size, 0.5F, size);
            Math3D.VectorAdd(self.absmin, size, origin);
            gib.s.origin[0] = origin[0] + Lib.Crandom() * size[0];
            gib.s.origin[1] = origin[1] + Lib.Crandom() * size[1];
            gib.s.origin[2] = origin[2] + Lib.Crandom() * size[2];
            GameBase.gi.Setmodel(gib, gibname);
            gib.solid = Defines.SOLID_NOT;
            gib.s.effects |= Defines.EF_GIB;
            gib.flags |= Defines.FL_NO_KNOCKBACK;
            gib.takedamage = Defines.DAMAGE_YES;
            gib.die = gib_die;
            if (type == Defines.GIB_ORGANIC)
            {
                gib.movetype = Defines.MOVETYPE_TOSS;
                gib.touch = gib_touch;
                vscale = 0.5F;
            }
            else
            {
                gib.movetype = Defines.MOVETYPE_BOUNCE;
                vscale = 1F;
            }

            VelocityForDamage(damage, vd);
            Math3D.VectorMA(self.velocity, vscale, vd, gib.velocity);
            ClipGibVelocity(gib);
            gib.avelocity[0] = Lib.Random() * 600;
            gib.avelocity[1] = Lib.Random() * 600;
            gib.avelocity[2] = Lib.Random() * 600;
            gib.think = GameUtil.G_FreeEdictA;
            gib.nextthink = GameBase.level.time + 10 + Lib.Random() * 10;
            GameBase.gi.Linkentity(gib);
        }

        public static void ThrowHead(edict_t self, string gibname, int damage, int type)
        {
            float[] vd = new float[]{0, 0, 0};
            float vscale;
            self.s.skinnum = 0;
            self.s.frame = 0;
            Math3D.VectorClear(self.mins);
            Math3D.VectorClear(self.maxs);
            self.s.modelindex2 = 0;
            GameBase.gi.Setmodel(self, gibname);
            self.solid = Defines.SOLID_NOT;
            self.s.effects |= Defines.EF_GIB;
            self.s.effects &= ~Defines.EF_FLIES;
            self.s.sound = 0;
            self.flags |= Defines.FL_NO_KNOCKBACK;
            self.svflags &= ~Defines.SVF_MONSTER;
            self.takedamage = Defines.DAMAGE_YES;
            self.die = gib_die;
            if (type == Defines.GIB_ORGANIC)
            {
                self.movetype = Defines.MOVETYPE_TOSS;
                self.touch = gib_touch;
                vscale = 0.5F;
            }
            else
            {
                self.movetype = Defines.MOVETYPE_BOUNCE;
                vscale = 1F;
            }

            VelocityForDamage(damage, vd);
            Math3D.VectorMA(self.velocity, vscale, vd, self.velocity);
            ClipGibVelocity(self);
            self.avelocity[Defines.YAW] = Lib.Crandom() * 600F;
            self.think = GameUtil.G_FreeEdictA;
            self.nextthink = GameBase.level.time + 10 + Lib.Random() * 10;
            GameBase.gi.Linkentity(self);
        }

        public static void ThrowClientHead(edict_t self, int damage)
        {
            float[] vd = new float[]{0, 0, 0};
            string gibname;
            if ((Lib.Rand() & 1) != 0)
            {
                gibname = "models/objects/gibs/head2/tris.md2";
                self.s.skinnum = 1;
            }
            else
            {
                gibname = "models/objects/gibs/skull/tris.md2";
                self.s.skinnum = 0;
            }

            self.s.origin[2] += 32;
            self.s.frame = 0;
            GameBase.gi.Setmodel(self, gibname);
            Math3D.VectorSet(self.mins, -16, -16, 0);
            Math3D.VectorSet(self.maxs, 16, 16, 16);
            self.takedamage = Defines.DAMAGE_NO;
            self.solid = Defines.SOLID_NOT;
            self.s.effects = Defines.EF_GIB;
            self.s.sound = 0;
            self.flags |= Defines.FL_NO_KNOCKBACK;
            self.movetype = Defines.MOVETYPE_BOUNCE;
            VelocityForDamage(damage, vd);
            Math3D.VectorAdd(self.velocity, vd, self.velocity);
            if (self.client != null)
            {
                self.client.anim_priority = Defines.ANIM_DEATH;
                self.client.anim_end = self.s.frame;
            }
            else
            {
                self.think = null;
                self.nextthink = 0;
            }

            GameBase.gi.Linkentity(self);
        }

        public static void ThrowDebris(edict_t self, string modelname, float speed, float[] origin)
        {
            edict_t chunk;
            float[] v = new float[]{0, 0, 0};
            chunk = GameUtil.G_Spawn();
            Math3D.VectorCopy(origin, chunk.s.origin);
            GameBase.gi.Setmodel(chunk, modelname);
            v[0] = 100 * Lib.Crandom();
            v[1] = 100 * Lib.Crandom();
            v[2] = 100 + 100 * Lib.Crandom();
            Math3D.VectorMA(self.velocity, speed, v, chunk.velocity);
            chunk.movetype = Defines.MOVETYPE_BOUNCE;
            chunk.solid = Defines.SOLID_NOT;
            chunk.avelocity[0] = Lib.Random() * 600;
            chunk.avelocity[1] = Lib.Random() * 600;
            chunk.avelocity[2] = Lib.Random() * 600;
            chunk.think = GameUtil.G_FreeEdictA;
            chunk.nextthink = GameBase.level.time + 5 + Lib.Random() * 5;
            chunk.s.frame = 0;
            chunk.flags = 0;
            chunk.classname = "debris";
            chunk.takedamage = Defines.DAMAGE_YES;
            chunk.die = debris_die;
            GameBase.gi.Linkentity(chunk);
        }

        public static void ClipGibVelocity(edict_t ent)
        {
            if (ent.velocity[0] < -300)
                ent.velocity[0] = -300;
            else if (ent.velocity[0] > 300)
                ent.velocity[0] = 300;
            if (ent.velocity[1] < -300)
                ent.velocity[1] = -300;
            else if (ent.velocity[1] > 300)
                ent.velocity[1] = 300;
            if (ent.velocity[2] < 200)
                ent.velocity[2] = 200;
            else if (ent.velocity[2] > 500)
                ent.velocity[2] = 500;
        }

        public static EntUseAdapter Use_Areaportal = new AnonymousEntUseAdapter();
        private sealed class AnonymousEntUseAdapter : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "use_areaportal";
            }

            public override void Use(edict_t ent, edict_t other, edict_t activator)
            {
                ent.count ^= 1;
                GameBase.gi.SetAreaPortalState(ent.style, ent.count != 0);
            }
        }

        public static EntThinkAdapter SP_func_areaportal = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_func_areaportal";
            }

            public override bool Think(edict_t ent)
            {
                ent.use = Use_Areaportal;
                ent.count = 0;
                return true;
            }
        }

        public static EntTouchAdapter path_corner_touch = new AnonymousEntTouchAdapter();
        private sealed class AnonymousEntTouchAdapter : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "path_corner_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                float[] v = new float[]{0, 0, 0};
                edict_t next;
                if (other.movetarget != self)
                    return;
                if (other.enemy != null)
                    return;
                if (self.pathtarget != null)
                {
                    string savetarget;
                    savetarget = self.target;
                    self.target = self.pathtarget;
                    GameUtil.G_UseTargets(self, other);
                    self.target = savetarget;
                }

                if (self.target != null)
                    next = GameBase.G_PickTarget(self.target);
                else
                    next = null;
                if ((next != null) && (next.spawnflags & 1) != 0)
                {
                    Math3D.VectorCopy(next.s.origin, v);
                    v[2] += next.mins[2];
                    v[2] -= other.mins[2];
                    Math3D.VectorCopy(v, other.s.origin);
                    next = GameBase.G_PickTarget(next.target);
                    other.s.event_renamed = Defines.EV_OTHER_TELEPORT;
                }

                other.goalentity = other.movetarget = next;
                if (self.wait != 0)
                {
                    other.monsterinfo.pausetime = GameBase.level.time + self.wait;
                    other.monsterinfo.stand.Think(other);
                    return;
                }

                if (other.movetarget == null)
                {
                    other.monsterinfo.pausetime = GameBase.level.time + 100000000;
                    other.monsterinfo.stand.Think(other);
                }
                else
                {
                    Math3D.VectorSubtract(other.goalentity.s.origin, other.s.origin, v);
                    other.ideal_yaw = Math3D.Vectoyaw(v);
                }
            }
        }

        public static EntTouchAdapter point_combat_touch = new AnonymousEntTouchAdapter1();
        private sealed class AnonymousEntTouchAdapter1 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "point_combat_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                edict_t activator;
                if (other.movetarget != self)
                    return;
                if (self.target != null)
                {
                    other.target = self.target;
                    other.goalentity = other.movetarget = GameBase.G_PickTarget(other.target);
                    if (null == other.goalentity)
                    {
                        GameBase.gi.Dprintf(self.classname + " at " + Lib.Vtos(self.s.origin) + " target " + self.target + " does not exist\\n");
                        other.movetarget = self;
                    }

                    self.target = null;
                }
                else if ((self.spawnflags & 1) != 0 && 0 == (other.flags & (Defines.FL_SWIM | Defines.FL_FLY)))
                {
                    other.monsterinfo.pausetime = GameBase.level.time + 100000000;
                    other.monsterinfo.aiflags |= Defines.AI_STAND_GROUND;
                    other.monsterinfo.stand.Think(other);
                }

                if (other.movetarget == self)
                {
                    other.target = null;
                    other.movetarget = null;
                    other.goalentity = other.enemy;
                    other.monsterinfo.aiflags &= ~Defines.AI_COMBAT_POINT;
                }

                if (self.pathtarget != null)
                {
                    string savetarget;
                    savetarget = self.target;
                    self.target = self.pathtarget;
                    if (other.enemy != null && other.enemy.client != null)
                        activator = other.enemy;
                    else if (other.oldenemy != null && other.oldenemy.client != null)
                        activator = other.oldenemy;
                    else if (other.activator != null && other.activator.client != null)
                        activator = other.activator;
                    else
                        activator = other;
                    GameUtil.G_UseTargets(self, activator);
                    self.target = savetarget;
                }
            }
        }

        public static EntThinkAdapter TH_viewthing = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "th_viewthing";
            }

            public override bool Think(edict_t ent)
            {
                ent.s.frame = (ent.s.frame + 1) % 7;
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                return true;
            }
        }

        public static readonly int START_OFF = 1;
        public static EntUseAdapter light_use = new AnonymousEntUseAdapter1();
        private sealed class AnonymousEntUseAdapter1 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "light_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if ((self.spawnflags & START_OFF) != 0)
                {
                    GameBase.gi.Configstring(Defines.CS_LIGHTS + self.style, "m");
                    self.spawnflags &= ~START_OFF;
                }
                else
                {
                    GameBase.gi.Configstring(Defines.CS_LIGHTS + self.style, "a");
                    self.spawnflags |= START_OFF;
                }
            }
        }

        static EntUseAdapter func_wall_use = new AnonymousEntUseAdapter2();
        private sealed class AnonymousEntUseAdapter2 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "func_wall_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if (self.solid == Defines.SOLID_NOT)
                {
                    self.solid = Defines.SOLID_BSP;
                    self.svflags &= ~Defines.SVF_NOCLIENT;
                    GameUtil.KillBox(self);
                }
                else
                {
                    self.solid = Defines.SOLID_NOT;
                    self.svflags |= Defines.SVF_NOCLIENT;
                }

                GameBase.gi.Linkentity(self);
                if (0 == (self.spawnflags & 2))
                    self.use = null;
            }
        }

        static EntTouchAdapter func_object_touch = new AnonymousEntTouchAdapter2();
        private sealed class AnonymousEntTouchAdapter2 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "func_object_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (plane == null)
                    return;
                if (plane.normal[2] < 1)
                    return;
                if (other.takedamage == Defines.DAMAGE_NO)
                    return;
                GameCombat.T_Damage(other, self, self, Globals.vec3_origin, self.s.origin, Globals.vec3_origin, self.dmg, 1, 0, Defines.MOD_CRUSH);
            }
        }

        static EntThinkAdapter func_object_release = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "func_object_release";
            }

            public override bool Think(edict_t self)
            {
                self.movetype = Defines.MOVETYPE_TOSS;
                self.touch = func_object_touch;
                return true;
            }
        }

        static EntUseAdapter func_object_use = new AnonymousEntUseAdapter3();
        private sealed class AnonymousEntUseAdapter3 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "func_object_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.solid = Defines.SOLID_BSP;
                self.svflags &= ~Defines.SVF_NOCLIENT;
                self.use = null;
                GameUtil.KillBox(self);
                func_object_release.Think(self);
            }
        }

        public static EntDieAdapter func_explosive_explode = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "func_explosive_explode";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                float[] origin = new float[]{0, 0, 0};
                float[] chunkorigin = new float[]{0, 0, 0};
                float[] size = new float[]{0, 0, 0};
                int count;
                int mass;
                Math3D.VectorScale(self.size, 0.5F, size);
                Math3D.VectorAdd(self.absmin, size, origin);
                Math3D.VectorCopy(origin, self.s.origin);
                self.takedamage = Defines.DAMAGE_NO;
                if (self.dmg != 0)
                    GameCombat.T_RadiusDamage(self, attacker, self.dmg, null, self.dmg + 40, Defines.MOD_EXPLOSIVE);
                Math3D.VectorSubtract(self.s.origin, inflictor.s.origin, self.velocity);
                Math3D.VectorNormalize(self.velocity);
                Math3D.VectorScale(self.velocity, 150, self.velocity);
                Math3D.VectorScale(size, 0.5F, size);
                mass = self.mass;
                if (0 == mass)
                    mass = 75;
                if (mass >= 100)
                {
                    count = mass / 100;
                    if (count > 8)
                        count = 8;
                    while (count-- != 0)
                    {
                        chunkorigin[0] = origin[0] + Lib.Crandom() * size[0];
                        chunkorigin[1] = origin[1] + Lib.Crandom() * size[1];
                        chunkorigin[2] = origin[2] + Lib.Crandom() * size[2];
                        ThrowDebris(self, "models/objects/debris1/tris.md2", 1, chunkorigin);
                    }
                }

                count = mass / 25;
                if (count > 16)
                    count = 16;
                while (count-- != 0)
                {
                    chunkorigin[0] = origin[0] + Lib.Crandom() * size[0];
                    chunkorigin[1] = origin[1] + Lib.Crandom() * size[1];
                    chunkorigin[2] = origin[2] + Lib.Crandom() * size[2];
                    ThrowDebris(self, "models/objects/debris2/tris.md2", 2, chunkorigin);
                }

                GameUtil.G_UseTargets(self, attacker);
                if (self.dmg != 0)
                    BecomeExplosion1(self);
                else
                    GameUtil.G_FreeEdict(self);
            }
        }

        public static EntUseAdapter func_explosive_use = new AnonymousEntUseAdapter4();
        private sealed class AnonymousEntUseAdapter4 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "func_explosive_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                func_explosive_explode.Die(self, self, other, self.health, Globals.vec3_origin);
            }
        }

        public static EntUseAdapter func_explosive_spawn = new AnonymousEntUseAdapter5();
        private sealed class AnonymousEntUseAdapter5 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "func_explosive_spawn";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.solid = Defines.SOLID_BSP;
                self.svflags &= ~Defines.SVF_NOCLIENT;
                self.use = null;
                GameUtil.KillBox(self);
                GameBase.gi.Linkentity(self);
            }
        }

        public static EntTouchAdapter barrel_touch = new AnonymousEntTouchAdapter3();
        private sealed class AnonymousEntTouchAdapter3 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "barrel_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                float ratio;
                float[] v = new float[]{0, 0, 0};
                if ((null == other.groundentity) || (other.groundentity == self))
                    return;
                ratio = (float)other.mass / (float)self.mass;
                Math3D.VectorSubtract(self.s.origin, other.s.origin, v);
                M.M_walkmove(self, Math3D.Vectoyaw(v), 20 * ratio * Defines.FRAMETIME);
            }
        }

        public static EntThinkAdapter barrel_explode = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "barrel_explode";
            }

            public override bool Think(edict_t self)
            {
                float[] org = new float[]{0, 0, 0};
                float spd;
                float[] save = new float[]{0, 0, 0};
                GameCombat.T_RadiusDamage(self, self.activator, self.dmg, null, self.dmg + 40, Defines.MOD_BARREL);
                Math3D.VectorCopy(self.s.origin, save);
                Math3D.VectorMA(self.absmin, 0.5F, self.size, self.s.origin);
                spd = 1.5F * (float)self.dmg / 200F;
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris1/tris.md2", spd, org);
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris1/tris.md2", spd, org);
                spd = 1.75F * (float)self.dmg / 200F;
                Math3D.VectorCopy(self.absmin, org);
                ThrowDebris(self, "models/objects/debris3/tris.md2", spd, org);
                Math3D.VectorCopy(self.absmin, org);
                org[0] += self.size[0];
                ThrowDebris(self, "models/objects/debris3/tris.md2", spd, org);
                Math3D.VectorCopy(self.absmin, org);
                org[1] += self.size[1];
                ThrowDebris(self, "models/objects/debris3/tris.md2", spd, org);
                Math3D.VectorCopy(self.absmin, org);
                org[0] += self.size[0];
                org[1] += self.size[1];
                ThrowDebris(self, "models/objects/debris3/tris.md2", spd, org);
                spd = 2 * self.dmg / 200;
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris2/tris.md2", spd, org);
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris2/tris.md2", spd, org);
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris2/tris.md2", spd, org);
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris2/tris.md2", spd, org);
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris2/tris.md2", spd, org);
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris2/tris.md2", spd, org);
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris2/tris.md2", spd, org);
                org[0] = self.s.origin[0] + Lib.Crandom() * self.size[0];
                org[1] = self.s.origin[1] + Lib.Crandom() * self.size[1];
                org[2] = self.s.origin[2] + Lib.Crandom() * self.size[2];
                ThrowDebris(self, "models/objects/debris2/tris.md2", spd, org);
                Math3D.VectorCopy(save, self.s.origin);
                if (self.groundentity != null)
                    BecomeExplosion2(self);
                else
                    BecomeExplosion1(self);
                return true;
            }
        }

        public static EntDieAdapter barrel_delay = new AnonymousEntDieAdapter1();
        private sealed class AnonymousEntDieAdapter1 : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "barrel_delay";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                self.takedamage = Defines.DAMAGE_NO;
                self.nextthink = GameBase.level.time + 2 * Defines.FRAMETIME;
                self.think = barrel_explode;
                self.activator = attacker;
            }
        }

        static EntUseAdapter misc_blackhole_use = new AnonymousEntUseAdapter6();
        private sealed class AnonymousEntUseAdapter6 : EntUseAdapter
		{			
            public override string GetID()
            {
                return "misc_blavkhole_use";
            }

            public override void Use(edict_t ent, edict_t other, edict_t activator)
            {
                GameUtil.G_FreeEdict(ent);
            }
        }

        static EntThinkAdapter misc_blackhole_think = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "misc_blackhole_think";
            }

            public override bool Think(edict_t self)
            {
                if (++self.s.frame < 19)
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                else
                {
                    self.s.frame = 0;
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                }

                return true;
            }
        }

        static EntThinkAdapter misc_eastertank_think = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "misc_eastertank_think";
            }

            public override bool Think(edict_t self)
            {
                if (++self.s.frame < 293)
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                else
                {
                    self.s.frame = 254;
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                }

                return true;
            }
        }

        static EntThinkAdapter misc_easterchick_think = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "misc_easterchick_think";
            }

            public override bool Think(edict_t self)
            {
                if (++self.s.frame < 247)
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                else
                {
                    self.s.frame = 208;
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                }

                return true;
            }
        }

        static EntThinkAdapter misc_easterchick2_think = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "misc_easterchick2_think";
            }

            public override bool Think(edict_t self)
            {
                if (++self.s.frame < 287)
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                else
                {
                    self.s.frame = 248;
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                }

                return true;
            }
        }

        public static EntThinkAdapter commander_body_think = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "commander_body_think";
            }

            public override bool Think(edict_t self)
            {
                if (++self.s.frame < 24)
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                else
                    self.nextthink = 0;
                if (self.s.frame == 22)
                    GameBase.gi.Sound(self, Defines.CHAN_BODY, GameBase.gi.Soundindex("tank/thud.wav"), 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        public static EntUseAdapter commander_body_use = new AnonymousEntUseAdapter7();
        private sealed class AnonymousEntUseAdapter7 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "commander_body_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.think = commander_body_think;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                GameBase.gi.Sound(self, Defines.CHAN_BODY, GameBase.gi.Soundindex("tank/pain.wav"), 1, Defines.ATTN_NORM, 0);
            }
        }

        public static EntThinkAdapter commander_body_drop = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "commander_body_group";
            }

            public override bool Think(edict_t self)
            {
                self.movetype = Defines.MOVETYPE_TOSS;
                self.s.origin[2] += 2;
                return true;
            }
        }

        static EntThinkAdapter misc_banner_think = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "misc_banner_think";
            }

            public override bool Think(edict_t ent)
            {
                ent.s.frame = (ent.s.frame + 1) % 16;
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                return true;
            }
        }

        static EntDieAdapter misc_deadsoldier_die = new AnonymousEntDieAdapter2();
        private sealed class AnonymousEntDieAdapter2 : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "misc_deadsoldier_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                int n;
                if (self.health > -80)
                    return;
                GameBase.gi.Sound(self, Defines.CHAN_BODY, GameBase.gi.Soundindex("misc/udeath.wav"), 1, Defines.ATTN_NORM, 0);
                for (n = 0; n < 4; n++)
                    ThrowGib(self, "models/objects/gibs/sm_meat/tris.md2", damage, Defines.GIB_ORGANIC);
                ThrowHead(self, "models/objects/gibs/head2/tris.md2", damage, Defines.GIB_ORGANIC);
            }
        }

        static EntUseAdapter misc_viper_use = new AnonymousEntUseAdapter8();
        private sealed class AnonymousEntUseAdapter8 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "misc_viper_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.svflags &= ~Defines.SVF_NOCLIENT;
                self.use = GameFunc.train_use;
                GameFunc.train_use.Use(self, other, activator);
            }
        }

        static EntTouchAdapter misc_viper_bomb_touch = new AnonymousEntTouchAdapter4();
        private sealed class AnonymousEntTouchAdapter4 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "misc_viper_bomb_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                GameUtil.G_UseTargets(self, self.activator);
                self.s.origin[2] = self.absmin[2] + 1;
                GameCombat.T_RadiusDamage(self, self, self.dmg, null, self.dmg + 40, Defines.MOD_BOMB);
                BecomeExplosion2(self);
            }
        }

        static EntThinkAdapter misc_viper_bomb_prethink = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "misc_viper_bomb_prethink";
            }

            public override bool Think(edict_t self)
            {
                float[] v = new float[]{0, 0, 0};
                float diff;
                self.groundentity = null;
                diff = self.timestamp - GameBase.level.time;
                if (diff < -1)
                    diff = -1F;
                Math3D.VectorScale(self.moveinfo.dir, 1F + diff, v);
                v[2] = diff;
                diff = self.s.angles[2];
                Math3D.Vectoangles(v, self.s.angles);
                self.s.angles[2] = diff + 10;
                return true;
            }
        }

        static EntUseAdapter misc_viper_bomb_use = new AnonymousEntUseAdapter9();
        private sealed class AnonymousEntUseAdapter9 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "misc_viper_bomb_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                edict_t viper = null;
                self.solid = Defines.SOLID_BBOX;
                self.svflags &= ~Defines.SVF_NOCLIENT;
                self.s.effects |= Defines.EF_ROCKET;
                self.use = null;
                self.movetype = Defines.MOVETYPE_TOSS;
                self.prethink = misc_viper_bomb_prethink;
                self.touch = misc_viper_bomb_touch;
                self.activator = activator;
                EdictIterator es = null;
                es = GameBase.G_Find(es, GameBase.findByClass, "misc_viper");
                if (es != null)
                    viper = es.o;
                Math3D.VectorScale(viper.moveinfo.dir, viper.moveinfo.speed, self.velocity);
                self.timestamp = GameBase.level.time;
                Math3D.VectorCopy(viper.moveinfo.dir, self.moveinfo.dir);
            }
        }

        static EntUseAdapter misc_strogg_ship_use = new AnonymousEntUseAdapter10();
        private sealed class AnonymousEntUseAdapter10 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "misc_strogg_ship_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.svflags &= ~Defines.SVF_NOCLIENT;
                self.use = GameFunc.train_use;
                GameFunc.train_use.Use(self, other, activator);
            }
        }

        static EntThinkAdapter misc_satellite_dish_think = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "misc_satellite_dish_think";
            }

            public override bool Think(edict_t self)
            {
                self.s.frame++;
                if (self.s.frame < 38)
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                return true;
            }
        }

        static EntUseAdapter misc_satellite_dish_use = new AnonymousEntUseAdapter11();
        private sealed class AnonymousEntUseAdapter11 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "misc_satellite_dish_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.s.frame = 0;
                self.think = misc_satellite_dish_think;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
            }
        }

        static EntUseAdapter target_string_use = new AnonymousEntUseAdapter12();
        private sealed class AnonymousEntUseAdapter12 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "target_string_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                edict_t e;
                int n, l;
                char c;
                l = self.message.Length;
                for (e = self.teammaster; e != null; e = e.teamchain)
                {
                    if (e.count == 0)
                        continue;
                    n = e.count - 1;
                    if (n >= l)
                    {
                        e.s.frame = 12;
                        continue;
                    }

                    c = self.message[n];
                    if (c >= '0' && c <= '9')
                        e.s.frame = c - '0';
                    else if (c == '-')
                        e.s.frame = 10;
                    else if (c == ':')
                        e.s.frame = 11;
                    else
                        e.s.frame = 12;
                }
            }
        }

        public static readonly int CLOCK_MESSAGE_SIZE = 16;
        public static EntThinkAdapter func_clock_think = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "func_clock_think";
            }

            public override bool Think(edict_t self)
            {
                if (null == self.enemy)
                {
                    EdictIterator es = null;
                    es = GameBase.G_Find(es, GameBase.findByTarget, self.target);
                    if (es != null)
                        self.enemy = es.o;
                    if (self.enemy == null)
                        return true;
                }

                if ((self.spawnflags & 1) != 0)
                {
                    Func_clock_format_countdown(self);
                    self.health++;
                }
                else if ((self.spawnflags & 2) != 0)
                {
                    Func_clock_format_countdown(self);
                    self.health--;
                }
                else
                {
                    var now = DateTime.Now;
                    self.message = "" + now.Hour + ":" + now.Minute + ":" + now.Second; // Make better later TODO
                }

                self.enemy.message = self.message;
                self.enemy.use.Use(self.enemy, self, self);
                if (((self.spawnflags & 1) != 0 && (self.health > self.wait)) || ((self.spawnflags & 2) != 0 && (self.health < self.wait)))
                {
                    if (self.pathtarget != null)
                    {
                        string savetarget;
                        string savemessage;
                        savetarget = self.target;
                        savemessage = self.message;
                        self.target = self.pathtarget;
                        self.message = null;
                        GameUtil.G_UseTargets(self, self.activator);
                        self.target = savetarget;
                        self.message = savemessage;
                    }

                    if (0 == (self.spawnflags & 8))
                        return true;
                    Func_clock_reset(self);
                    if ((self.spawnflags & 4) != 0)
                        return true;
                }

                self.nextthink = GameBase.level.time + 1;
                return true;
            }
        }

        public static EntUseAdapter func_clock_use = new AnonymousEntUseAdapter13();
        private sealed class AnonymousEntUseAdapter13 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "func_clock_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if (0 == (self.spawnflags & 8))
                    self.use = null;
                if (self.activator != null)
                    return;
                self.activator = activator;
                self.think.Think(self);
            }
        }

        static EntTouchAdapter teleporter_touch = new AnonymousEntTouchAdapter5();
        private sealed class AnonymousEntTouchAdapter5 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "teleporter_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                edict_t dest;
                int i;
                if (other.client == null)
                    return;
                EdictIterator es = null;
                dest = GameBase.G_Find(null, GameBase.findByTarget, self.target).o;
                if (dest == null)
                {
                    GameBase.gi.Dprintf("Couldn't find destination\\n");
                    return;
                }

                GameBase.gi.Unlinkentity(other);
                Math3D.VectorCopy(dest.s.origin, other.s.origin);
                Math3D.VectorCopy(dest.s.origin, other.s.old_origin);
                other.s.origin[2] += 10;
                Math3D.VectorClear(other.velocity);
                other.client.ps.pmove.pm_time = 160 >> 3;
                other.client.ps.pmove.pm_flags |= ( Byte ) pmove_t.PMF_TIME_TELEPORT;
                self.owner.s.event_renamed = Defines.EV_PLAYER_TELEPORT;
                other.s.event_renamed = Defines.EV_PLAYER_TELEPORT;
                for (i = 0; i < 3; i++)
                {
                    other.client.ps.pmove.delta_angles[i] = (short)Math3D.ANGLE2SHORT(dest.s.angles[i] - other.client.resp.cmd_angles[i]);
                }

                Math3D.VectorClear(other.s.angles);
                Math3D.VectorClear(other.client.ps.viewangles);
                Math3D.VectorClear(other.client.v_angle);
                GameUtil.KillBox(other);
                GameBase.gi.Linkentity(other);
            }
        }

        public static EntThinkAdapter SP_misc_teleporter_dest = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "SP_misc_teleporter_dest";
            }

            public override bool Think(edict_t ent)
            {
                GameBase.gi.Setmodel(ent, "models/objects/dmspot/tris.md2");
                ent.s.skinnum = 0;
                ent.solid = Defines.SOLID_BBOX;
                Math3D.VectorSet(ent.mins, -32, -32, -24);
                Math3D.VectorSet(ent.maxs, 32, 32, -16);
                GameBase.gi.Linkentity(ent);
                return true;
            }
        }

        public static EntThinkAdapter gib_think = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gib_think";
            }

            public override bool Think(edict_t self)
            {
                self.s.frame++;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                if (self.s.frame == 10)
                {
                    self.think = GameUtil.G_FreeEdictA;
                    self.nextthink = GameBase.level.time + 8 + ( float ) Globals.rnd.NextDouble() * 10;
                }

                return true;
            }
        }

        public static EntTouchAdapter gib_touch = new AnonymousEntTouchAdapter6();
        private sealed class AnonymousEntTouchAdapter6 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "gib_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                float[] normal_angles = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                if (null == self.groundentity)
                    return;
                self.touch = null;
                if (plane != null)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, GameBase.gi.Soundindex("misc/fhit3.wav"), 1, Defines.ATTN_NORM, 0);
                    Math3D.Vectoangles(plane.normal, normal_angles);
                    Math3D.AngleVectors(normal_angles, null, right, null);
                    Math3D.Vectoangles(right, self.s.angles);
                    if (self.s.modelindex == GameBase.sm_meat_index)
                    {
                        self.s.frame++;
                        self.think = gib_think;
                        self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                    }
                }
            }
        }

        public static EntDieAdapter gib_die = new AnonymousEntDieAdapter3();
        private sealed class AnonymousEntDieAdapter3 : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "gib_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                GameUtil.G_FreeEdict(self);
            }
        }

        public static EntDieAdapter debris_die = new AnonymousEntDieAdapter4();
        private sealed class AnonymousEntDieAdapter4 : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "debris_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                GameUtil.G_FreeEdict(self);
            }
        }
    }
}
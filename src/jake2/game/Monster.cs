using Jake2.Client;
using Jake2.Qcommon;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class Monster
    {
        public static void Monster_fire_bullet(edict_t self, float[] start, float[] dir, int damage, int kick, int hspread, int vspread, int flashtype)
        {
            GameWeapon.Fire_bullet(self, start, dir, damage, kick, hspread, vspread, Defines.MOD_UNKNOWN);
            GameBase.gi.WriteByte(Defines.svc_muzzleflash2);
            GameBase.gi.WriteShort(self.index);
            GameBase.gi.WriteByte(flashtype);
            GameBase.gi.Multicast(start, Defines.MULTICAST_PVS);
        }

        public static void Monster_fire_shotgun(edict_t self, float[] start, float[] aimdir, int damage, int kick, int hspread, int vspread, int count, int flashtype)
        {
            GameWeapon.Fire_shotgun(self, start, aimdir, damage, kick, hspread, vspread, count, Defines.MOD_UNKNOWN);
            GameBase.gi.WriteByte(Defines.svc_muzzleflash2);
            GameBase.gi.WriteShort(self.index);
            GameBase.gi.WriteByte(flashtype);
            GameBase.gi.Multicast(start, Defines.MULTICAST_PVS);
        }

        public static void Monster_fire_blaster(edict_t self, float[] start, float[] dir, int damage, int speed, int flashtype, int effect)
        {
            GameWeapon.Fire_blaster(self, start, dir, damage, speed, effect, false);
            GameBase.gi.WriteByte(Defines.svc_muzzleflash2);
            GameBase.gi.WriteShort(self.index);
            GameBase.gi.WriteByte(flashtype);
            GameBase.gi.Multicast(start, Defines.MULTICAST_PVS);
        }

        public static void Monster_fire_grenade(edict_t self, float[] start, float[] aimdir, int damage, int speed, int flashtype)
        {
            GameWeapon.Fire_grenade(self, start, aimdir, damage, speed, 2.5F, damage + 40);
            GameBase.gi.WriteByte(Defines.svc_muzzleflash2);
            GameBase.gi.WriteShort(self.index);
            GameBase.gi.WriteByte(flashtype);
            GameBase.gi.Multicast(start, Defines.MULTICAST_PVS);
        }

        public static void Monster_fire_rocket(edict_t self, float[] start, float[] dir, int damage, int speed, int flashtype)
        {
            GameWeapon.Fire_rocket(self, start, dir, damage, speed, damage + 20, damage);
            GameBase.gi.WriteByte(Defines.svc_muzzleflash2);
            GameBase.gi.WriteShort(self.index);
            GameBase.gi.WriteByte(flashtype);
            GameBase.gi.Multicast(start, Defines.MULTICAST_PVS);
        }

        public static void Monster_fire_railgun(edict_t self, float[] start, float[] aimdir, int damage, int kick, int flashtype)
        {
            GameWeapon.Fire_rail(self, start, aimdir, damage, kick);
            GameBase.gi.WriteByte(Defines.svc_muzzleflash2);
            GameBase.gi.WriteShort(self.index);
            GameBase.gi.WriteByte(flashtype);
            GameBase.gi.Multicast(start, Defines.MULTICAST_PVS);
        }

        public static void Monster_fire_bfg(edict_t self, float[] start, float[] aimdir, int damage, int speed, int kick, float damage_radius, int flashtype)
        {
            GameWeapon.Fire_bfg(self, start, aimdir, damage, speed, damage_radius);
            GameBase.gi.WriteByte(Defines.svc_muzzleflash2);
            GameBase.gi.WriteShort(self.index);
            GameBase.gi.WriteByte(flashtype);
            GameBase.gi.Multicast(start, Defines.MULTICAST_PVS);
        }

        public static void Monster_death_use(edict_t self)
        {
            self.flags &= ~(Defines.FL_FLY | Defines.FL_SWIM);
            self.monsterinfo.aiflags &= Defines.AI_GOOD_GUY;
            if (self.item != null)
            {
                GameItems.Drop_Item(self, self.item);
                self.item = null;
            }

            if (self.deathtarget != null)
                self.target = self.deathtarget;
            if (self.target == null)
                return;
            GameUtil.G_UseTargets(self, self.enemy);
        }

        public static bool Monster_start(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return false;
            }

            if ((self.spawnflags & 4) != 0 && 0 == (self.monsterinfo.aiflags & Defines.AI_GOOD_GUY))
            {
                self.spawnflags &= ~4;
                self.spawnflags |= 1;
            }

            if (0 == (self.monsterinfo.aiflags & Defines.AI_GOOD_GUY))
                GameBase.level.total_monsters++;
            self.nextthink = GameBase.level.time + Defines.FRAMETIME;
            self.svflags |= Defines.SVF_MONSTER;
            self.s.renderfx |= Defines.RF_FRAMELERP;
            self.takedamage = Defines.DAMAGE_AIM;
            self.air_finished = GameBase.level.time + 12;
            self.use = GameUtil.monster_use;
            self.max_health = self.health;
            self.clipmask = Defines.MASK_MONSTERSOLID;
            self.s.skinnum = 0;
            self.deadflag = Defines.DEAD_NO;
            self.svflags &= ~Defines.SVF_DEADMONSTER;
            if (null == self.monsterinfo.checkattack)
                self.monsterinfo.checkattack = GameUtil.M_CheckAttack;
            Math3D.VectorCopy(self.s.origin, self.s.old_origin);
            if (GameBase.st.item != null && GameBase.st.item.Length > 0)
            {
                self.item = GameItems.FindItemByClassname(GameBase.st.item);
                if (self.item == null)
                    GameBase.gi.Dprintf("monster_start:" + self.classname + " at " + Lib.Vtos(self.s.origin) + " has bad item: " + GameBase.st.item + "\\n");
            }

            if (self.monsterinfo.currentmove != null)
                self.s.frame = self.monsterinfo.currentmove.firstframe + (Lib.Rand() % (self.monsterinfo.currentmove.lastframe - self.monsterinfo.currentmove.firstframe + 1));
            return true;
        }

        public static void Monster_start_go(edict_t self)
        {
            float[] v = new float[]{0, 0, 0};
            if (self.health <= 0)
                return;
            if (self.target != null)
            {
                bool notcombat;
                bool fixup;
                edict_t target = null;
                notcombat = false;
                fixup = false;
                EdictIterator edit = null;
                while ((edit = GameBase.G_Find(edit, GameBase.findByTarget, self.target)) != null)
                {
                    target = edit.o;
                    if (Lib.Strcmp(target.classname, "point_combat") == 0)
                    {
                        self.combattarget = self.target;
                        fixup = true;
                    }
                    else
                    {
                        notcombat = true;
                    }
                }

                if (notcombat && self.combattarget != null)
                    GameBase.gi.Dprintf(self.classname + " at " + Lib.Vtos(self.s.origin) + " has target with mixed types\\n");
                if (fixup)
                    self.target = null;
            }

            if (self.combattarget != null)
            {
                edict_t target = null;
                EdictIterator edit = null;
                while ((edit = GameBase.G_Find(edit, GameBase.findByTarget, self.combattarget)) != null)
                {
                    target = edit.o;
                    if (Lib.Strcmp(target.classname, "point_combat") != 0)
                    {
                        GameBase.gi.Dprintf(self.classname + " at " + Lib.Vtos(self.s.origin) + " has bad combattarget " + self.combattarget + " : " + target.classname + " at " + Lib.Vtos(target.s.origin));
                    }
                }
            }

            if (self.target != null)
            {
                self.goalentity = self.movetarget = GameBase.G_PickTarget(self.target);
                if (null == self.movetarget)
                {
                    GameBase.gi.Dprintf(self.classname + " can't find target " + self.target + " at " + Lib.Vtos(self.s.origin) + "\\n");
                    self.target = null;
                    self.monsterinfo.pausetime = 100000000;
                    self.monsterinfo.stand.Think(self);
                }
                else if (Lib.Strcmp(self.movetarget.classname, "path_corner") == 0)
                {
                    Math3D.VectorSubtract(self.goalentity.s.origin, self.s.origin, v);
                    self.ideal_yaw = self.s.angles[Defines.YAW] = Math3D.Vectoyaw(v);
                    self.monsterinfo.walk.Think(self);
                    self.target = null;
                }
                else
                {
                    self.goalentity = self.movetarget = null;
                    self.monsterinfo.pausetime = 100000000;
                    self.monsterinfo.stand.Think(self);
                }
            }
            else
            {
                self.monsterinfo.pausetime = 100000000;
                self.monsterinfo.stand.Think(self);
            }

            self.think = Monster.monster_think;
            self.nextthink = GameBase.level.time + Defines.FRAMETIME;
        }

        public static EntThinkAdapter monster_think = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "monster_think";
            }

            public override bool Think(edict_t self)
            {
                M.M_MoveFrame(self);
                if (self.linkcount != self.monsterinfo.linkcount)
                {
                    self.monsterinfo.linkcount = self.linkcount;
                    M.M_CheckGround(self);
                }

                M.M_CatagorizePosition(self);
                M.M_WorldEffects(self);
                M.M_SetEffects(self);
                return true;
            }
        }

        public static EntThinkAdapter monster_triggered_spawn = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "monster_trigger_spawn";
            }

            public override bool Think(edict_t self)
            {
                self.s.origin[2] += 1;
                GameUtil.KillBox(self);
                self.solid = Defines.SOLID_BBOX;
                self.movetype = Defines.MOVETYPE_STEP;
                self.svflags &= ~Defines.SVF_NOCLIENT;
                self.air_finished = GameBase.level.time + 12;
                GameBase.gi.Linkentity(self);
                Monster.Monster_start_go(self);
                if (self.enemy != null && 0 == (self.spawnflags & 1) && 0 == (self.enemy.flags & Defines.FL_NOTARGET))
                {
                    GameUtil.FoundTarget(self);
                }
                else
                {
                    self.enemy = null;
                }

                return true;
            }
        }

        public static EntUseAdapter monster_triggered_spawn_use = new AnonymousEntUseAdapter();
        private sealed class AnonymousEntUseAdapter : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "monster_trigger_spawn_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.think = monster_triggered_spawn;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                if (activator.client != null)
                    self.enemy = activator;
                self.use = GameUtil.monster_use;
            }
        }

        public static EntThinkAdapter monster_triggered_start = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "monster_triggered_start";
            }

            public override bool Think(edict_t self)
            {
                if (self.index == 312)
                    Com.Printf("monster_triggered_start\\n");
                self.solid = Defines.SOLID_NOT;
                self.movetype = Defines.MOVETYPE_NONE;
                self.svflags |= Defines.SVF_NOCLIENT;
                self.nextthink = 0;
                self.use = monster_triggered_spawn_use;
                return true;
            }
        }
    }
}
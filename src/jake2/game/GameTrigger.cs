using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class GameTrigger
    {
        public static void InitTrigger(edict_t self)
        {
            if (!Math3D.VectorEquals(self.s.angles, Globals.vec3_origin))
                GameBase.G_SetMovedir(self.s.angles, self.movedir);
            self.solid = Defines.SOLID_TRIGGER;
            self.movetype = Defines.MOVETYPE_NONE;
            GameBase.gi.Setmodel(self, self.model);
            self.svflags = Defines.SVF_NOCLIENT;
        }

        public static void Multi_trigger(edict_t ent)
        {
            if (ent.nextthink != 0)
                return;
            GameUtil.G_UseTargets(ent, ent.activator);
            if (ent.wait > 0)
            {
                ent.think = multi_wait;
                ent.nextthink = GameBase.level.time + ent.wait;
            }
            else
            {
                ent.touch = null;
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                ent.think = GameUtil.G_FreeEdictA;
            }
        }

        public static void SP_trigger_multiple(edict_t ent)
        {
            if (ent.sounds == 1)
                ent.noise_index = GameBase.gi.Soundindex("misc/secret.wav");
            else if (ent.sounds == 2)
                ent.noise_index = GameBase.gi.Soundindex("misc/talk.wav");
            else if (ent.sounds == 3)
                ent.noise_index = GameBase.gi.Soundindex("misc/trigger1.wav");
            if (ent.wait == 0)
                ent.wait = 0.2F;
            ent.touch = Touch_Multi;
            ent.movetype = Defines.MOVETYPE_NONE;
            ent.svflags |= Defines.SVF_NOCLIENT;
            if ((ent.spawnflags & 4) != 0)
            {
                ent.solid = Defines.SOLID_NOT;
                ent.use = trigger_enable;
            }
            else
            {
                ent.solid = Defines.SOLID_TRIGGER;
                ent.use = Use_Multi;
            }

            if (!Math3D.VectorEquals(ent.s.angles, Globals.vec3_origin))
                GameBase.G_SetMovedir(ent.s.angles, ent.movedir);
            GameBase.gi.Setmodel(ent, ent.model);
            GameBase.gi.Linkentity(ent);
        }

        public static void SP_trigger_once(edict_t ent)
        {
            if ((ent.spawnflags & 1) != 0)
            {
                float[] v = new float[]{0, 0, 0};
                Math3D.VectorMA(ent.mins, 0.5F, ent.size, v);
                ent.spawnflags &= ~1;
                ent.spawnflags |= 4;
                GameBase.gi.Dprintf("fixed TRIGGERED flag on " + ent.classname + " at " + Lib.Vtos(v) + "\\n");
            }

            ent.wait = -1;
            SP_trigger_multiple(ent);
        }

        public static void SP_trigger_relay(edict_t self)
        {
            self.use = trigger_relay_use;
        }

        public static void SP_trigger_key(edict_t self)
        {
            if (GameBase.st.item == null)
            {
                GameBase.gi.Dprintf("no key item for trigger_key at " + Lib.Vtos(self.s.origin) + "\\n");
                return;
            }

            self.item = GameItems.FindItemByClassname(GameBase.st.item);
            if (null == self.item)
            {
                GameBase.gi.Dprintf("item " + GameBase.st.item + " not found for trigger_key at " + Lib.Vtos(self.s.origin) + "\\n");
                return;
            }

            if (self.target == null)
            {
                GameBase.gi.Dprintf(self.classname + " at " + Lib.Vtos(self.s.origin) + " has no target\\n");
                return;
            }

            GameBase.gi.Soundindex("misc/keytry.wav");
            GameBase.gi.Soundindex("misc/keyuse.wav");
            self.use = trigger_key_use;
        }

        public static void SP_trigger_counter(edict_t self)
        {
            self.wait = -1;
            if (0 == self.count)
                self.count = 2;
            self.use = trigger_counter_use;
        }

        public static void SP_trigger_always(edict_t ent)
        {
            if (ent.delay < 0.2F)
                ent.delay = 0.2F;
            GameUtil.G_UseTargets(ent, ent);
        }

        public static void SP_trigger_push(edict_t self)
        {
            InitTrigger(self);
            windsound = GameBase.gi.Soundindex("misc/windfly.wav");
            self.touch = trigger_push_touch;
            if (0 == self.speed)
                self.speed = 1000;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_trigger_hurt(edict_t self)
        {
            InitTrigger(self);
            self.noise_index = GameBase.gi.Soundindex("world/electro.wav");
            self.touch = hurt_touch;
            if (0 == self.dmg)
                self.dmg = 5;
            if ((self.spawnflags & 1) != 0)
                self.solid = Defines.SOLID_NOT;
            else
                self.solid = Defines.SOLID_TRIGGER;
            if ((self.spawnflags & 2) != 0)
                self.use = hurt_use;
            GameBase.gi.Linkentity(self);
        }

        public static void SP_trigger_gravity(edict_t self)
        {
            if (GameBase.st.gravity == null)
            {
                GameBase.gi.Dprintf("trigger_gravity without gravity set at " + Lib.Vtos(self.s.origin) + "\\n");
                GameUtil.G_FreeEdict(self);
                return;
            }

            InitTrigger(self);
            self.gravity = Lib.Atoi(GameBase.st.gravity);
            self.touch = trigger_gravity_touch;
        }

        public static void SP_trigger_monsterjump(edict_t self)
        {
            if (0 == self.speed)
                self.speed = 200;
            if (0 == GameBase.st.height)
                GameBase.st.height = 200;
            if (self.s.angles[Defines.YAW] == 0)
                self.s.angles[Defines.YAW] = 360;
            InitTrigger(self);
            self.touch = trigger_monsterjump_touch;
            self.movedir[2] = GameBase.st.height;
        }

        public static EntThinkAdapter multi_wait = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "multi_wait";
            }

            public override bool Think(edict_t ent)
            {
                ent.nextthink = 0;
                return true;
            }
        }

        static EntUseAdapter Use_Multi = new AnonymousEntUseAdapter();
        private sealed class AnonymousEntUseAdapter : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "Use_Multi";
            }

            public override void Use(edict_t ent, edict_t other, edict_t activator)
            {
                ent.activator = activator;
                Multi_trigger(ent);
            }
        }

        static EntTouchAdapter Touch_Multi = new AnonymousEntTouchAdapter();
        private sealed class AnonymousEntTouchAdapter : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "Touch_Multi";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (other.client != null)
                {
                    if ((self.spawnflags & 2) != 0)
                        return;
                }
                else if ((other.svflags & Defines.SVF_MONSTER) != 0)
                {
                    if (0 == (self.spawnflags & 1))
                        return;
                }
                else
                    return;
                if (!Math3D.VectorEquals(self.movedir, Globals.vec3_origin))
                {
                    float[] forward = new float[]{0, 0, 0};
                    Math3D.AngleVectors(other.s.angles, forward, null, null);
                    if (Math3D.DotProduct(forward, self.movedir) < 0)
                        return;
                }

                self.activator = other;
                Multi_trigger(self);
            }
        }

        static EntUseAdapter trigger_enable = new AnonymousEntUseAdapter1();
        private sealed class AnonymousEntUseAdapter1 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "trigger_enable";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.solid = Defines.SOLID_TRIGGER;
                self.use = Use_Multi;
                GameBase.gi.Linkentity(self);
            }
        }

        public static EntUseAdapter trigger_relay_use = new AnonymousEntUseAdapter2();
        private sealed class AnonymousEntUseAdapter2 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "trigger_relay_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                GameUtil.G_UseTargets(self, activator);
            }
        }

        static EntUseAdapter trigger_key_use = new AnonymousEntUseAdapter3();
        private sealed class AnonymousEntUseAdapter3 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "trigger_key_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                int index;
                if (self.item == null)
                    return;
                if (activator.client == null)
                    return;
                index = GameItems.ITEM_INDEX(self.item);
                if (activator.client.pers.inventory[index] == 0)
                {
                    if (GameBase.level.time < self.touch_debounce_time)
                        return;
                    self.touch_debounce_time = GameBase.level.time + 5F;
                    GameBase.gi.Centerprintf(activator, "You need the " + self.item.pickup_name);
                    GameBase.gi.Sound(activator, Defines.CHAN_AUTO, GameBase.gi.Soundindex("misc/keytry.wav"), 1, Defines.ATTN_NORM, 0);
                    return;
                }

                GameBase.gi.Sound(activator, Defines.CHAN_AUTO, GameBase.gi.Soundindex("misc/keyuse.wav"), 1, Defines.ATTN_NORM, 0);
                if (GameBase.coop.value != 0)
                {
                    int player;
                    edict_t ent;
                    if (Lib.Strcmp(self.item.classname, "key_power_cube") == 0)
                    {
                        int cube;
                        for (cube = 0; cube < 8; cube++)
                            if ((activator.client.pers.power_cubes & (1 << cube)) != 0)
                                break;
                        for (player = 1; player <= GameBase.game.maxclients; player++)
                        {
                            ent = GameBase.g_edicts[player];
                            if (!ent.inuse)
                                continue;
                            if (null == ent.client)
                                continue;
                            if ((ent.client.pers.power_cubes & (1 << cube)) != 0)
                            {
                                ent.client.pers.inventory[index]--;
                                ent.client.pers.power_cubes &= ~(1 << cube);
                            }
                        }
                    }
                    else
                    {
                        for (player = 1; player <= GameBase.game.maxclients; player++)
                        {
                            ent = GameBase.g_edicts[player];
                            if (!ent.inuse)
                                continue;
                            if (ent.client == null)
                                continue;
                            ent.client.pers.inventory[index] = 0;
                        }
                    }
                }
                else
                {
                    activator.client.pers.inventory[index]--;
                }

                GameUtil.G_UseTargets(self, activator);
                self.use = null;
            }
        }

        static EntUseAdapter trigger_counter_use = new AnonymousEntUseAdapter4();
        private sealed class AnonymousEntUseAdapter4 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "trigger_counter_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if (self.count == 0)
                    return;
                self.count--;
                if (self.count != 0)
                {
                    if (0 == (self.spawnflags & 1))
                    {
                        GameBase.gi.Centerprintf(activator, self.count + " more to go...");
                        GameBase.gi.Sound(activator, Defines.CHAN_AUTO, GameBase.gi.Soundindex("misc/talk1.wav"), 1, Defines.ATTN_NORM, 0);
                    }

                    return;
                }

                if (0 == (self.spawnflags & 1))
                {
                    GameBase.gi.Centerprintf(activator, "Sequence completed!");
                    GameBase.gi.Sound(activator, Defines.CHAN_AUTO, GameBase.gi.Soundindex("misc/talk1.wav"), 1, Defines.ATTN_NORM, 0);
                }

                self.activator = activator;
                Multi_trigger(self);
            }
        }

        public static readonly int PUSH_ONCE = 1;
        public static int windsound;
        static EntTouchAdapter trigger_push_touch = new AnonymousEntTouchAdapter1();
        private sealed class AnonymousEntTouchAdapter1 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "trigger_push_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (Lib.Strcmp(other.classname, "grenade") == 0)
                {
                    Math3D.VectorScale(self.movedir, self.speed * 10, other.velocity);
                }
                else if (other.health > 0)
                {
                    Math3D.VectorScale(self.movedir, self.speed * 10, other.velocity);
                    if (other.client != null)
                    {
                        Math3D.VectorCopy(other.velocity, other.client.oldvelocity);
                        if (other.fly_sound_debounce_time < GameBase.level.time)
                        {
                            other.fly_sound_debounce_time = GameBase.level.time + 1.5F;
                            GameBase.gi.Sound(other, Defines.CHAN_AUTO, windsound, 1, Defines.ATTN_NORM, 0);
                        }
                    }
                }

                if ((self.spawnflags & PUSH_ONCE) != 0)
                    GameUtil.G_FreeEdict(self);
            }
        }

        static EntUseAdapter hurt_use = new AnonymousEntUseAdapter5();
        private sealed class AnonymousEntUseAdapter5 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "hurt_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if (self.solid == Defines.SOLID_NOT)
                    self.solid = Defines.SOLID_TRIGGER;
                else
                    self.solid = Defines.SOLID_NOT;
                GameBase.gi.Linkentity(self);
                if (0 == (self.spawnflags & 2))
                    self.use = null;
            }
        }

        static EntTouchAdapter hurt_touch = new AnonymousEntTouchAdapter2();
        private sealed class AnonymousEntTouchAdapter2 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "hurt_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                int dflags;
                if (other.takedamage == 0)
                    return;
                if (self.timestamp > GameBase.level.time)
                    return;
                if ((self.spawnflags & 16) != 0)
                    self.timestamp = GameBase.level.time + 1;
                else
                    self.timestamp = GameBase.level.time + Defines.FRAMETIME;
                if (0 == (self.spawnflags & 4))
                {
                    if ((GameBase.level.framenum % 10) == 0)
                        GameBase.gi.Sound(other, Defines.CHAN_AUTO, self.noise_index, 1, Defines.ATTN_NORM, 0);
                }

                if ((self.spawnflags & 8) != 0)
                    dflags = Defines.DAMAGE_NO_PROTECTION;
                else
                    dflags = 0;
                GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, self.dmg, self.dmg, dflags, Defines.MOD_TRIGGER_HURT);
            }
        }

        static EntTouchAdapter trigger_gravity_touch = new AnonymousEntTouchAdapter3();
        private sealed class AnonymousEntTouchAdapter3 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "trigger_gravity_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                other.gravity = self.gravity;
            }
        }

        static EntTouchAdapter trigger_monsterjump_touch = new AnonymousEntTouchAdapter4();
        private sealed class AnonymousEntTouchAdapter4 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "trigger_monsterjump_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if ((other.flags & (Defines.FL_FLY | Defines.FL_SWIM)) != 0)
                    return;
                if ((other.svflags & Defines.SVF_DEADMONSTER) != 0)
                    return;
                if (0 == (other.svflags & Defines.SVF_MONSTER))
                    return;
                other.velocity[0] = self.movedir[0] * self.speed;
                other.velocity[1] = self.movedir[1] * self.speed;
                if (other.groundentity != null)
                    return;
                other.groundentity = null;
                other.velocity[2] = self.movedir[2];
            }
        }
    }
}
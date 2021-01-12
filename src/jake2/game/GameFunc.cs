using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class GameFunc
    {
        static void Move_Calc(edict_t ent, float[] dest, EntThinkAdapter func)
        {
            Math3D.VectorClear(ent.velocity);
            Math3D.VectorSubtract(dest, ent.s.origin, ent.moveinfo.dir);
            ent.moveinfo.remaining_distance = Math3D.VectorNormalize(ent.moveinfo.dir);
            ent.moveinfo.endfunc = func;
            if (ent.moveinfo.speed == ent.moveinfo.accel && ent.moveinfo.speed == ent.moveinfo.decel)
            {
                if (GameBase.level.current_entity == ((ent.flags & Defines.FL_TEAMSLAVE) != 0 ? ent.teammaster : ent))
                {
                    Move_Begin.Think(ent);
                }
                else
                {
                    ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                    ent.think = Move_Begin;
                }
            }
            else
            {
                ent.moveinfo.current_speed = 0;
                ent.think = Think_AccelMove;
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
            }
        }

        static void AngleMove_Calc(edict_t ent, EntThinkAdapter func)
        {
            Math3D.VectorClear(ent.avelocity);
            ent.moveinfo.endfunc = func;
            if (GameBase.level.current_entity == ((ent.flags & Defines.FL_TEAMSLAVE) != 0 ? ent.teammaster : ent))
            {
                AngleMove_Begin.Think(ent);
            }
            else
            {
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                ent.think = AngleMove_Begin;
            }
        }

        static float AccelerationDistance(float target, float rate)
        {
            return target * ((target / rate) + 1) / 2;
        }

        static void Plat_CalcAcceleratedMove(moveinfo_t moveinfo)
        {
            float accel_dist;
            float decel_dist;
            moveinfo.move_speed = moveinfo.speed;
            if (moveinfo.remaining_distance < moveinfo.accel)
            {
                moveinfo.current_speed = moveinfo.remaining_distance;
                return;
            }

            accel_dist = AccelerationDistance(moveinfo.speed, moveinfo.accel);
            decel_dist = AccelerationDistance(moveinfo.speed, moveinfo.decel);
            if ((moveinfo.remaining_distance - accel_dist - decel_dist) < 0)
            {
                float f;
                f = (moveinfo.accel + moveinfo.decel) / (moveinfo.accel * moveinfo.decel);
                moveinfo.move_speed = (float)((-2 + Math.Sqrt(4 - 4 * f * (-2 * moveinfo.remaining_distance))) / (2 * f));
                decel_dist = AccelerationDistance(moveinfo.move_speed, moveinfo.decel);
            }

            moveinfo.decel_distance = decel_dist;
        }

        static void Plat_Accelerate(moveinfo_t moveinfo)
        {
            if (moveinfo.remaining_distance <= moveinfo.decel_distance)
            {
                if (moveinfo.remaining_distance < moveinfo.decel_distance)
                {
                    if (moveinfo.next_speed != 0)
                    {
                        moveinfo.current_speed = moveinfo.next_speed;
                        moveinfo.next_speed = 0;
                        return;
                    }

                    if (moveinfo.current_speed > moveinfo.decel)
                        moveinfo.current_speed -= moveinfo.decel;
                }

                return;
            }

            if (moveinfo.current_speed == moveinfo.move_speed)
                if ((moveinfo.remaining_distance - moveinfo.current_speed) < moveinfo.decel_distance)
                {
                    float p1_distance;
                    float p2_distance;
                    float distance;
                    p1_distance = moveinfo.remaining_distance - moveinfo.decel_distance;
                    p2_distance = moveinfo.move_speed * (1F - (p1_distance / moveinfo.move_speed));
                    distance = p1_distance + p2_distance;
                    moveinfo.current_speed = moveinfo.move_speed;
                    moveinfo.next_speed = moveinfo.move_speed - moveinfo.decel * (p2_distance / distance);
                    return;
                }

            if (moveinfo.current_speed < moveinfo.speed)
            {
                float old_speed;
                float p1_distance;
                float p1_speed;
                float p2_distance;
                float distance;
                old_speed = moveinfo.current_speed;
                moveinfo.current_speed += moveinfo.accel;
                if (moveinfo.current_speed > moveinfo.speed)
                    moveinfo.current_speed = moveinfo.speed;
                if ((moveinfo.remaining_distance - moveinfo.current_speed) >= moveinfo.decel_distance)
                    return;
                p1_distance = moveinfo.remaining_distance - moveinfo.decel_distance;
                p1_speed = (old_speed + moveinfo.move_speed) / 2F;
                p2_distance = moveinfo.move_speed * (1F - (p1_distance / p1_speed));
                distance = p1_distance + p2_distance;
                moveinfo.current_speed = (p1_speed * (p1_distance / distance)) + (moveinfo.move_speed * (p2_distance / distance));
                moveinfo.next_speed = moveinfo.move_speed - moveinfo.decel * (p2_distance / distance);
                return;
            }

            return;
        }

        static void Plat_go_up(edict_t ent)
        {
            if (0 == (ent.flags & Defines.FL_TEAMSLAVE))
            {
                if (ent.moveinfo.sound_start != 0)
                    GameBase.gi.Sound(ent, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, ent.moveinfo.sound_start, 1, Defines.ATTN_STATIC, 0);
                ent.s.sound = ent.moveinfo.sound_middle;
            }

            ent.moveinfo.state = STATE_UP;
            Move_Calc(ent, ent.moveinfo.start_origin, plat_hit_top);
        }

        static void Plat_spawn_inside_trigger(edict_t ent)
        {
            edict_t trigger;
            float[] tmin = new float[]{0, 0, 0}, tmax = new float[]{0, 0, 0};
            trigger = GameUtil.G_Spawn();
            trigger.touch = Touch_Plat_Center;
            trigger.movetype = Defines.MOVETYPE_NONE;
            trigger.solid = Defines.SOLID_TRIGGER;
            trigger.enemy = ent;
            tmin[0] = ent.mins[0] + 25;
            tmin[1] = ent.mins[1] + 25;
            tmin[2] = ent.mins[2];
            tmax[0] = ent.maxs[0] - 25;
            tmax[1] = ent.maxs[1] - 25;
            tmax[2] = ent.maxs[2] + 8;
            tmin[2] = tmax[2] - (ent.pos1[2] - ent.pos2[2] + GameBase.st.lip);
            if ((ent.spawnflags & PLAT_LOW_TRIGGER) != 0)
                tmax[2] = tmin[2] + 8;
            if (tmax[0] - tmin[0] <= 0)
            {
                tmin[0] = (ent.mins[0] + ent.maxs[0]) * 0.5F;
                tmax[0] = tmin[0] + 1;
            }

            if (tmax[1] - tmin[1] <= 0)
            {
                tmin[1] = (ent.mins[1] + ent.maxs[1]) * 0.5F;
                tmax[1] = tmin[1] + 1;
            }

            Math3D.VectorCopy(tmin, trigger.mins);
            Math3D.VectorCopy(tmax, trigger.maxs);
            GameBase.gi.Linkentity(trigger);
        }

        public static void SP_func_plat(edict_t ent)
        {
            Math3D.VectorClear(ent.s.angles);
            ent.solid = Defines.SOLID_BSP;
            ent.movetype = Defines.MOVETYPE_PUSH;
            GameBase.gi.Setmodel(ent, ent.model);
            ent.blocked = plat_blocked;
            if (0 == ent.speed)
                ent.speed = 20;
            else
                ent.speed *= 0.1f;
            if (ent.accel == 0)
                ent.accel = 5;
            else
                ent.accel *= 0.1f;
            if (ent.decel == 0)
                ent.decel = 5;
            else
                ent.decel *= 0.1f;
            if (ent.dmg == 0)
                ent.dmg = 2;
            if (GameBase.st.lip == 0)
                GameBase.st.lip = 8;
            Math3D.VectorCopy(ent.s.origin, ent.pos1);
            Math3D.VectorCopy(ent.s.origin, ent.pos2);
            if (GameBase.st.height != 0)
                ent.pos2[2] -= GameBase.st.height;
            else
                ent.pos2[2] -= (ent.maxs[2] - ent.mins[2]) - GameBase.st.lip;
            ent.use = Use_Plat;
            Plat_spawn_inside_trigger(ent);
            if (ent.targetname != null)
            {
                ent.moveinfo.state = STATE_UP;
            }
            else
            {
                Math3D.VectorCopy(ent.pos2, ent.s.origin);
                GameBase.gi.Linkentity(ent);
                ent.moveinfo.state = STATE_BOTTOM;
            }

            ent.moveinfo.speed = ent.speed;
            ent.moveinfo.accel = ent.accel;
            ent.moveinfo.decel = ent.decel;
            ent.moveinfo.wait = ent.wait;
            Math3D.VectorCopy(ent.pos1, ent.moveinfo.start_origin);
            Math3D.VectorCopy(ent.s.angles, ent.moveinfo.start_angles);
            Math3D.VectorCopy(ent.pos2, ent.moveinfo.end_origin);
            Math3D.VectorCopy(ent.s.angles, ent.moveinfo.end_angles);
            ent.moveinfo.sound_start = GameBase.gi.Soundindex("plats/pt1_strt.wav");
            ent.moveinfo.sound_middle = GameBase.gi.Soundindex("plats/pt1_mid.wav");
            ent.moveinfo.sound_end = GameBase.gi.Soundindex("plats/pt1_end.wav");
        }

        static void Door_use_areaportals(edict_t self, bool open)
        {
            edict_t t = null;
            if (self.target == null)
                return;
            EdictIterator edit = null;
            while ((edit = GameBase.G_Find(edit, GameBase.findByTarget, self.target)) != null)
            {
                t = edit.o;
                if (Lib.Q_stricmp(t.classname, "func_areaportal") == 0)
                {
                    GameBase.gi.SetAreaPortalState(t.style, open);
                }
            }
        }

        static void Door_go_up(edict_t self, edict_t activator)
        {
            if (self.moveinfo.state == STATE_UP)
                return;
            if (self.moveinfo.state == STATE_TOP)
            {
                if (self.moveinfo.wait >= 0)
                    self.nextthink = GameBase.level.time + self.moveinfo.wait;
                return;
            }

            if (0 == (self.flags & Defines.FL_TEAMSLAVE))
            {
                if (self.moveinfo.sound_start != 0)
                    GameBase.gi.Sound(self, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, self.moveinfo.sound_start, 1, Defines.ATTN_STATIC, 0);
                self.s.sound = self.moveinfo.sound_middle;
            }

            self.moveinfo.state = STATE_UP;
            if (Lib.Strcmp(self.classname, "func_door") == 0)
                Move_Calc(self, self.moveinfo.end_origin, door_hit_top);
            else if (Lib.Strcmp(self.classname, "func_door_rotating") == 0)
                AngleMove_Calc(self, door_hit_top);
            GameUtil.G_UseTargets(self, activator);
            Door_use_areaportals(self, true);
        }

        public static void SP_func_water(edict_t self)
        {
            float[] abs_movedir = new float[]{0, 0, 0};
            GameBase.G_SetMovedir(self.s.angles, self.movedir);
            self.movetype = Defines.MOVETYPE_PUSH;
            self.solid = Defines.SOLID_BSP;
            GameBase.gi.Setmodel(self, self.model);
            switch ( self.sounds )
            {
                default:
                    break;
                case 1:
                    self.moveinfo.sound_start = GameBase.gi.Soundindex("world/mov_watr.wav");
                    self.moveinfo.sound_end = GameBase.gi.Soundindex("world/stp_watr.wav");
                    break;
                case 2:
                    self.moveinfo.sound_start = GameBase.gi.Soundindex("world/mov_watr.wav");
                    self.moveinfo.sound_end = GameBase.gi.Soundindex("world/stp_watr.wav");
                    break;
            }

            Math3D.VectorCopy(self.s.origin, self.pos1);
            abs_movedir[0] = Math.Abs(self.movedir[0]);
            abs_movedir[1] = Math.Abs(self.movedir[1]);
            abs_movedir[2] = Math.Abs(self.movedir[2]);
            self.moveinfo.distance = abs_movedir[0] * self.size[0] + abs_movedir[1] * self.size[1] + abs_movedir[2] * self.size[2] - GameBase.st.lip;
            Math3D.VectorMA(self.pos1, self.moveinfo.distance, self.movedir, self.pos2);
            if ((self.spawnflags & DOOR_START_OPEN) != 0)
            {
                Math3D.VectorCopy(self.pos2, self.s.origin);
                Math3D.VectorCopy(self.pos1, self.pos2);
                Math3D.VectorCopy(self.s.origin, self.pos1);
            }

            Math3D.VectorCopy(self.pos1, self.moveinfo.start_origin);
            Math3D.VectorCopy(self.s.angles, self.moveinfo.start_angles);
            Math3D.VectorCopy(self.pos2, self.moveinfo.end_origin);
            Math3D.VectorCopy(self.s.angles, self.moveinfo.end_angles);
            self.moveinfo.state = STATE_BOTTOM;
            if (0 == self.speed)
                self.speed = 25;
            self.moveinfo.accel = self.moveinfo.decel = self.moveinfo.speed = self.speed;
            if (0 == self.wait)
                self.wait = -1;
            self.moveinfo.wait = self.wait;
            self.use = door_use;
            if (self.wait == -1)
                self.spawnflags |= DOOR_TOGGLE;
            self.classname = "func_door";
            GameBase.gi.Linkentity(self);
        }

        static void Train_resume(edict_t self)
        {
            edict_t ent;
            float[] dest = new float[]{0, 0, 0};
            ent = self.target_ent;
            Math3D.VectorSubtract(ent.s.origin, self.mins, dest);
            self.moveinfo.state = STATE_TOP;
            Math3D.VectorCopy(self.s.origin, self.moveinfo.start_origin);
            Math3D.VectorCopy(dest, self.moveinfo.end_origin);
            Move_Calc(self, dest, train_wait);
            self.spawnflags |= TRAIN_START_ON;
        }

        public static void SP_func_train(edict_t self)
        {
            self.movetype = Defines.MOVETYPE_PUSH;
            Math3D.VectorClear(self.s.angles);
            self.blocked = train_blocked;
            if ((self.spawnflags & TRAIN_BLOCK_STOPS) != 0)
                self.dmg = 0;
            else
            {
                if (0 == self.dmg)
                    self.dmg = 100;
            }

            self.solid = Defines.SOLID_BSP;
            GameBase.gi.Setmodel(self, self.model);
            if (GameBase.st.noise != null)
                self.moveinfo.sound_middle = GameBase.gi.Soundindex(GameBase.st.noise);
            if (0 == self.speed)
                self.speed = 100;
            self.moveinfo.speed = self.speed;
            self.moveinfo.accel = self.moveinfo.decel = self.moveinfo.speed;
            self.use = train_use;
            GameBase.gi.Linkentity(self);
            if (self.target != null)
            {
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                self.think = func_train_find;
            }
            else
            {
                GameBase.gi.Dprintf("func_train without a target at " + Lib.Vtos(self.absmin) + "\\n");
            }
        }

        public static void SP_func_timer(edict_t self)
        {
            if (0 == self.wait)
                self.wait = 1F;
            self.use = func_timer_use;
            self.think = func_timer_think;
            if (self.random >= self.wait)
            {
                self.random = self.wait - Defines.FRAMETIME;
                GameBase.gi.Dprintf("func_timer at " + Lib.Vtos(self.s.origin) + " has random >= wait\\n");
            }

            if ((self.spawnflags & 1) != 0)
            {
                self.nextthink = GameBase.level.time + 1F + GameBase.st.pausetime + self.delay + self.wait + Lib.Crandom() * self.random;
                self.activator = self;
            }

            self.svflags = Defines.SVF_NOCLIENT;
        }

        public static readonly int PLAT_LOW_TRIGGER = 1;
        public static readonly int STATE_TOP = 0;
        public static readonly int STATE_BOTTOM = 1;
        public static readonly int STATE_UP = 2;
        public static readonly int STATE_DOWN = 3;
        public static readonly int DOOR_START_OPEN = 1;
        public static readonly int DOOR_REVERSE = 2;
        public static readonly int DOOR_CRUSHER = 4;
        public static readonly int DOOR_NOMONSTER = 8;
        public static readonly int DOOR_TOGGLE = 32;
        public static readonly int DOOR_X_AXIS = 64;
        public static readonly int DOOR_Y_AXIS = 128;
        static EntThinkAdapter Move_Done = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "move_done";
            }

            public override bool Think(edict_t ent)
            {
                Math3D.VectorClear(ent.velocity);
                ent.moveinfo.endfunc.Think(ent);
                return true;
            }
        }

        static EntThinkAdapter Move_Final = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "move_final";
            }

            public override bool Think(edict_t ent)
            {
                if (ent.moveinfo.remaining_distance == 0)
                {
                    Move_Done.Think(ent);
                    return true;
                }

                Math3D.VectorScale(ent.moveinfo.dir, ent.moveinfo.remaining_distance / Defines.FRAMETIME, ent.velocity);
                ent.think = Move_Done;
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                return true;
            }
        }

        static EntThinkAdapter Move_Begin = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "move_begin";
            }

            public override bool Think(edict_t ent)
            {
                float frames;
                if ((ent.moveinfo.speed * Defines.FRAMETIME) >= ent.moveinfo.remaining_distance)
                {
                    Move_Final.Think(ent);
                    return true;
                }

                Math3D.VectorScale(ent.moveinfo.dir, ent.moveinfo.speed, ent.velocity);
                frames = (float)Math.Floor((ent.moveinfo.remaining_distance / ent.moveinfo.speed) / Defines.FRAMETIME);
                ent.moveinfo.remaining_distance -= frames * ent.moveinfo.speed * Defines.FRAMETIME;
                ent.nextthink = GameBase.level.time + (frames * Defines.FRAMETIME);
                ent.think = Move_Final;
                return true;
            }
        }

        static EntThinkAdapter AngleMove_Done = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "agnle_move_done";
            }

            public override bool Think(edict_t ent)
            {
                Math3D.VectorClear(ent.avelocity);
                ent.moveinfo.endfunc.Think(ent);
                return true;
            }
        }

        static EntThinkAdapter AngleMove_Final = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "angle_move_final";
            }

            public override bool Think(edict_t ent)
            {
                float[] move = new float[]{0, 0, 0};
                if (ent.moveinfo.state == STATE_UP)
                    Math3D.VectorSubtract(ent.moveinfo.end_angles, ent.s.angles, move);
                else
                    Math3D.VectorSubtract(ent.moveinfo.start_angles, ent.s.angles, move);
                if (Math3D.VectorEquals(move, Globals.vec3_origin))
                {
                    AngleMove_Done.Think(ent);
                    return true;
                }

                Math3D.VectorScale(move, 1F / Defines.FRAMETIME, ent.avelocity);
                ent.think = AngleMove_Done;
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                return true;
            }
        }

        static EntThinkAdapter AngleMove_Begin = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "angle_move_begin";
            }

            public override bool Think(edict_t ent)
            {
                float[] destdelta = new float[]{0, 0, 0};
                float len;
                float traveltime;
                float frames;
                if (ent.moveinfo.state == STATE_UP)
                    Math3D.VectorSubtract(ent.moveinfo.end_angles, ent.s.angles, destdelta);
                else
                    Math3D.VectorSubtract(ent.moveinfo.start_angles, ent.s.angles, destdelta);
                len = Math3D.VectorLength(destdelta);
                traveltime = len / ent.moveinfo.speed;
                if (traveltime < Defines.FRAMETIME)
                {
                    AngleMove_Final.Think(ent);
                    return true;
                }

                frames = (float)(Math.Floor(traveltime / Defines.FRAMETIME));
                Math3D.VectorScale(destdelta, 1F / traveltime, ent.avelocity);
                ent.nextthink = GameBase.level.time + frames * Defines.FRAMETIME;
                ent.think = AngleMove_Final;
                return true;
            }
        }

        static EntThinkAdapter Think_AccelMove = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "thinc_accelmove";
            }

            public override bool Think(edict_t ent)
            {
                ent.moveinfo.remaining_distance -= ent.moveinfo.current_speed;
                if (ent.moveinfo.current_speed == 0)
                    Plat_CalcAcceleratedMove(ent.moveinfo);
                Plat_Accelerate(ent.moveinfo);
                if (ent.moveinfo.remaining_distance <= ent.moveinfo.current_speed)
                {
                    Move_Final.Think(ent);
                    return true;
                }

                Math3D.VectorScale(ent.moveinfo.dir, ent.moveinfo.current_speed * 10, ent.velocity);
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                ent.think = Think_AccelMove;
                return true;
            }
        }

        static EntThinkAdapter plat_hit_top = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "plat_hit_top";
            }

            public override bool Think(edict_t ent)
            {
                if (0 == (ent.flags & Defines.FL_TEAMSLAVE))
                {
                    if (ent.moveinfo.sound_end != 0)
                        GameBase.gi.Sound(ent, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, ent.moveinfo.sound_end, 1, Defines.ATTN_STATIC, 0);
                    ent.s.sound = 0;
                }

                ent.moveinfo.state = STATE_TOP;
                ent.think = plat_go_down;
                ent.nextthink = GameBase.level.time + 3;
                return true;
            }
        }

        static EntThinkAdapter plat_hit_bottom = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "plat_hit_bottom";
            }

            public override bool Think(edict_t ent)
            {
                if (0 == (ent.flags & Defines.FL_TEAMSLAVE))
                {
                    if (ent.moveinfo.sound_end != 0)
                        GameBase.gi.Sound(ent, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, ent.moveinfo.sound_end, 1, Defines.ATTN_STATIC, 0);
                    ent.s.sound = 0;
                }

                ent.moveinfo.state = STATE_BOTTOM;
                return true;
            }
        }

        static EntThinkAdapter plat_go_down = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "plat_go_down";
            }

            public override bool Think(edict_t ent)
            {
                if (0 == (ent.flags & Defines.FL_TEAMSLAVE))
                {
                    if (ent.moveinfo.sound_start != 0)
                        GameBase.gi.Sound(ent, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, ent.moveinfo.sound_start, 1, Defines.ATTN_STATIC, 0);
                    ent.s.sound = ent.moveinfo.sound_middle;
                }

                ent.moveinfo.state = STATE_DOWN;
                Move_Calc(ent, ent.moveinfo.end_origin, plat_hit_bottom);
                return true;
            }
        }

        static EntBlockedAdapter plat_blocked = new AnonymousEntBlockedAdapter();
        private sealed class AnonymousEntBlockedAdapter : EntBlockedAdapter
		{
			
            public override string GetID()
            {
                return "plat_blocked";
            }

            public override void Blocked(edict_t self, edict_t other)
            {
                if (0 == (other.svflags & Defines.SVF_MONSTER) && (null == other.client))
                {
                    GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, 100000, 1, 0, Defines.MOD_CRUSH);
                    if (other != null)
                        GameMisc.BecomeExplosion1(other);
                    return;
                }

                GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, self.dmg, 1, 0, Defines.MOD_CRUSH);
                if (self.moveinfo.state == STATE_UP)
                    plat_go_down.Think(self);
                else if (self.moveinfo.state == STATE_DOWN)
                    Plat_go_up(self);
            }
        }

        static EntUseAdapter Use_Plat = new AnonymousEntUseAdapter();
        private sealed class AnonymousEntUseAdapter : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "use_plat";
            }

            public override void Use(edict_t ent, edict_t other, edict_t activator)
            {
                if (ent.think != null)
                    return;
                plat_go_down.Think(ent);
            }
        }

        static EntTouchAdapter Touch_Plat_Center = new AnonymousEntTouchAdapter();
        private sealed class AnonymousEntTouchAdapter : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "touch_plat_center";
            }

            public override void Touch(edict_t ent, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (other.client == null)
                    return;
                if (other.health <= 0)
                    return;
                ent = ent.enemy;
                if (ent.moveinfo.state == STATE_BOTTOM)
                    Plat_go_up(ent);
                else if (ent.moveinfo.state == STATE_TOP)
                {
                    ent.nextthink = GameBase.level.time + 1;
                }
            }
        }

        static EntBlockedAdapter rotating_blocked = new AnonymousEntBlockedAdapter1();
        private sealed class AnonymousEntBlockedAdapter1 : EntBlockedAdapter
		{
			
            public override string GetID()
            {
                return "rotating_blocked";
            }

            public override void Blocked(edict_t self, edict_t other)
            {
                GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, self.dmg, 1, 0, Defines.MOD_CRUSH);
            }
        }

        static EntTouchAdapter rotating_touch = new AnonymousEntTouchAdapter1();
        private sealed class AnonymousEntTouchAdapter1 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "rotating_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (self.avelocity[0] != 0 || self.avelocity[1] != 0 || self.avelocity[2] != 0)
                    GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, self.dmg, 1, 0, Defines.MOD_CRUSH);
            }
        }

        static EntUseAdapter rotating_use = new AnonymousEntUseAdapter1();
        private sealed class AnonymousEntUseAdapter1 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "rotating_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if (!Math3D.VectorEquals(self.avelocity, Globals.vec3_origin))
                {
                    self.s.sound = 0;
                    Math3D.VectorClear(self.avelocity);
                    self.touch = null;
                }
                else
                {
                    self.s.sound = self.moveinfo.sound_middle;
                    Math3D.VectorScale(self.movedir, self.speed, self.avelocity);
                    if ((self.spawnflags & 16) != 0)
                        self.touch = rotating_touch;
                }
            }
        }

        public static EntThinkAdapter SP_func_rotating = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_func_rotating";
            }

            public override bool Think(edict_t ent)
            {
                ent.solid = Defines.SOLID_BSP;
                if ((ent.spawnflags & 32) != 0)
                    ent.movetype = Defines.MOVETYPE_STOP;
                else
                    ent.movetype = Defines.MOVETYPE_PUSH;
                Math3D.VectorClear(ent.movedir);
                if ((ent.spawnflags & 4) != 0)
                    ent.movedir[2] = 1F;
                else if ((ent.spawnflags & 8) != 0)
                    ent.movedir[0] = 1F;
                else
                    ent.movedir[1] = 1F;
                if ((ent.spawnflags & 2) != 0)
                    Math3D.VectorNegate(ent.movedir, ent.movedir);
                if (0 == ent.speed)
                    ent.speed = 100;
                if (0 == ent.dmg)
                    ent.dmg = 2;
                ent.use = rotating_use;
                if (ent.dmg != 0)
                    ent.blocked = rotating_blocked;
                if ((ent.spawnflags & 1) != 0)
                    ent.use.Use(ent, null, null);
                if ((ent.spawnflags & 64) != 0)
                    ent.s.effects |= Defines.EF_ANIM_ALL;
                if ((ent.spawnflags & 128) != 0)
                    ent.s.effects |= Defines.EF_ANIM_ALLFAST;
                GameBase.gi.Setmodel(ent, ent.model);
                GameBase.gi.Linkentity(ent);
                return true;
            }
        }

        static EntThinkAdapter button_done = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "button_done";
            }

            public override bool Think(edict_t self)
            {
                self.moveinfo.state = STATE_BOTTOM;
                self.s.effects &= ~Defines.EF_ANIM23;
                self.s.effects |= Defines.EF_ANIM01;
                return true;
            }
        }

        static EntThinkAdapter button_return = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "button_return";
            }

            public override bool Think(edict_t self)
            {
                self.moveinfo.state = STATE_DOWN;
                Move_Calc(self, self.moveinfo.start_origin, button_done);
                self.s.frame = 0;
                if (self.health != 0)
                    self.takedamage = Defines.DAMAGE_YES;
                return true;
            }
        }

        static EntThinkAdapter button_wait = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "button_wait";
            }

            public override bool Think(edict_t self)
            {
                self.moveinfo.state = STATE_TOP;
                self.s.effects &= ~Defines.EF_ANIM01;
                self.s.effects |= Defines.EF_ANIM23;
                GameUtil.G_UseTargets(self, self.activator);
                self.s.frame = 1;
                if (self.moveinfo.wait >= 0)
                {
                    self.nextthink = GameBase.level.time + self.moveinfo.wait;
                    self.think = button_return;
                }

                return true;
            }
        }

        static EntThinkAdapter button_fire = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "button_fire";
            }

            public override bool Think(edict_t self)
            {
                if (self.moveinfo.state == STATE_UP || self.moveinfo.state == STATE_TOP)
                    return true;
                self.moveinfo.state = STATE_UP;
                if (self.moveinfo.sound_start != 0 && 0 == (self.flags & Defines.FL_TEAMSLAVE))
                    GameBase.gi.Sound(self, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, self.moveinfo.sound_start, 1, Defines.ATTN_STATIC, 0);
                Move_Calc(self, self.moveinfo.end_origin, button_wait);
                return true;
            }
        }

        static EntUseAdapter button_use = new AnonymousEntUseAdapter2();
        private sealed class AnonymousEntUseAdapter2 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "button_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.activator = activator;
                button_fire.Think(self);
                return;
            }
        }

        static EntTouchAdapter button_touch = new AnonymousEntTouchAdapter2();
        private sealed class AnonymousEntTouchAdapter2 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "button_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (null == other.client)
                    return;
                if (other.health <= 0)
                    return;
                self.activator = other;
                button_fire.Think(self);
            }
        }

        static EntDieAdapter button_killed = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "button_killed";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                self.activator = attacker;
                self.health = self.max_health;
                self.takedamage = Defines.DAMAGE_NO;
                button_fire.Think(self);
            }
        }

        public static EntThinkAdapter SP_func_button = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_func_button";
            }

            public override bool Think(edict_t ent)
            {
                float[] abs_movedir = new float[]{0, 0, 0};
                float dist;
                GameBase.G_SetMovedir(ent.s.angles, ent.movedir);
                ent.movetype = Defines.MOVETYPE_STOP;
                ent.solid = Defines.SOLID_BSP;
                GameBase.gi.Setmodel(ent, ent.model);
                if (ent.sounds != 1)
                    ent.moveinfo.sound_start = GameBase.gi.Soundindex("switches/butn2.wav");
                if (0 == ent.speed)
                    ent.speed = 40;
                if (0 == ent.accel)
                    ent.accel = ent.speed;
                if (0 == ent.decel)
                    ent.decel = ent.speed;
                if (0 == ent.wait)
                    ent.wait = 3;
                if (0 == GameBase.st.lip)
                    GameBase.st.lip = 4;
                Math3D.VectorCopy(ent.s.origin, ent.pos1);
                abs_movedir[0] = (float)Math.Abs(ent.movedir[0]);
                abs_movedir[1] = (float)Math.Abs(ent.movedir[1]);
                abs_movedir[2] = (float)Math.Abs(ent.movedir[2]);
                dist = abs_movedir[0] * ent.size[0] + abs_movedir[1] * ent.size[1] + abs_movedir[2] * ent.size[2] - GameBase.st.lip;
                Math3D.VectorMA(ent.pos1, dist, ent.movedir, ent.pos2);
                ent.use = button_use;
                ent.s.effects |= Defines.EF_ANIM01;
                if (ent.health != 0)
                {
                    ent.max_health = ent.health;
                    ent.die = button_killed;
                    ent.takedamage = Defines.DAMAGE_YES;
                }
                else if (null == ent.targetname)
                    ent.touch = button_touch;
                ent.moveinfo.state = STATE_BOTTOM;
                ent.moveinfo.speed = ent.speed;
                ent.moveinfo.accel = ent.accel;
                ent.moveinfo.decel = ent.decel;
                ent.moveinfo.wait = ent.wait;
                Math3D.VectorCopy(ent.pos1, ent.moveinfo.start_origin);
                Math3D.VectorCopy(ent.s.angles, ent.moveinfo.start_angles);
                Math3D.VectorCopy(ent.pos2, ent.moveinfo.end_origin);
                Math3D.VectorCopy(ent.s.angles, ent.moveinfo.end_angles);
                GameBase.gi.Linkentity(ent);
                return true;
            }
        }

        static EntThinkAdapter door_hit_top = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_hit_top";
            }

            public override bool Think(edict_t self)
            {
                if (0 == (self.flags & Defines.FL_TEAMSLAVE))
                {
                    if (self.moveinfo.sound_end != 0)
                        GameBase.gi.Sound(self, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, self.moveinfo.sound_end, 1, Defines.ATTN_STATIC, 0);
                    self.s.sound = 0;
                }

                self.moveinfo.state = STATE_TOP;
                if ((self.spawnflags & DOOR_TOGGLE) != 0)
                    return true;
                if (self.moveinfo.wait >= 0)
                {
                    self.think = door_go_down;
                    self.nextthink = GameBase.level.time + self.moveinfo.wait;
                }

                return true;
            }
        }

        static EntThinkAdapter door_hit_bottom = new AnonymousEntThinkAdapter17();
        private sealed class AnonymousEntThinkAdapter17 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_hit_bottom";
            }

            public override bool Think(edict_t self)
            {
                if (0 == (self.flags & Defines.FL_TEAMSLAVE))
                {
                    if (self.moveinfo.sound_end != 0)
                        GameBase.gi.Sound(self, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, self.moveinfo.sound_end, 1, Defines.ATTN_STATIC, 0);
                    self.s.sound = 0;
                }

                self.moveinfo.state = STATE_BOTTOM;
                Door_use_areaportals(self, false);
                return true;
            }
        }

        static EntThinkAdapter door_go_down = new AnonymousEntThinkAdapter18();
        private sealed class AnonymousEntThinkAdapter18 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_go_down";
            }

            public override bool Think(edict_t self)
            {
                if (0 == (self.flags & Defines.FL_TEAMSLAVE))
                {
                    if (self.moveinfo.sound_start != 0)
                        GameBase.gi.Sound(self, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, self.moveinfo.sound_start, 1, Defines.ATTN_STATIC, 0);
                    self.s.sound = self.moveinfo.sound_middle;
                }

                if (self.max_health != 0)
                {
                    self.takedamage = Defines.DAMAGE_YES;
                    self.health = self.max_health;
                }

                self.moveinfo.state = STATE_DOWN;
                if (Lib.Strcmp(self.classname, "func_door") == 0)
                    Move_Calc(self, self.moveinfo.start_origin, door_hit_bottom);
                else if (Lib.Strcmp(self.classname, "func_door_rotating") == 0)
                    AngleMove_Calc(self, door_hit_bottom);
                return true;
            }
        }

        static EntUseAdapter door_use = new AnonymousEntUseAdapter3();
        private sealed class AnonymousEntUseAdapter3 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "door_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                edict_t ent;
                if ((self.flags & Defines.FL_TEAMSLAVE) != 0)
                    return;
                if ((self.spawnflags & DOOR_TOGGLE) != 0)
                {
                    if (self.moveinfo.state == STATE_UP || self.moveinfo.state == STATE_TOP)
                    {
                        for (ent = self; ent != null; ent = ent.teamchain)
                        {
                            ent.message = null;
                            ent.touch = null;
                            door_go_down.Think(ent);
                        }

                        return;
                    }
                }

                for (ent = self; ent != null; ent = ent.teamchain)
                {
                    ent.message = null;
                    ent.touch = null;
                    Door_go_up(ent, activator);
                }
            }
        }

        static EntTouchAdapter Touch_DoorTrigger = new AnonymousEntTouchAdapter3();
        private sealed class AnonymousEntTouchAdapter3 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "touch_door_trigger";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (other.health <= 0)
                    return;
                if (0 == (other.svflags & Defines.SVF_MONSTER) && (null == other.client))
                    return;
                if (0 != (self.owner.spawnflags & DOOR_NOMONSTER) && 0 != (other.svflags & Defines.SVF_MONSTER))
                    return;
                if (GameBase.level.time < self.touch_debounce_time)
                    return;
                self.touch_debounce_time = GameBase.level.time + 1F;
                door_use.Use(self.owner, other, other);
            }
        }

        static EntThinkAdapter Think_CalcMoveSpeed = new AnonymousEntThinkAdapter19();
        private sealed class AnonymousEntThinkAdapter19 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "think_calc_movespeed";
            }

            public override bool Think(edict_t self)
            {
                edict_t ent;
                float min;
                float time;
                float newspeed;
                float ratio;
                float dist;
                if ((self.flags & Defines.FL_TEAMSLAVE) != 0)
                    return true;
                min = Math.Abs(self.moveinfo.distance);
                for (ent = self.teamchain; ent != null; ent = ent.teamchain)
                {
                    dist = Math.Abs(ent.moveinfo.distance);
                    if (dist < min)
                        min = dist;
                }

                time = min / self.moveinfo.speed;
                for (ent = self; ent != null; ent = ent.teamchain)
                {
                    newspeed = Math.Abs(ent.moveinfo.distance) / time;
                    ratio = newspeed / ent.moveinfo.speed;
                    if (ent.moveinfo.accel == ent.moveinfo.speed)
                        ent.moveinfo.accel = newspeed;
                    else
                        ent.moveinfo.accel *= ratio;
                    if (ent.moveinfo.decel == ent.moveinfo.speed)
                        ent.moveinfo.decel = newspeed;
                    else
                        ent.moveinfo.decel *= ratio;
                    ent.moveinfo.speed = newspeed;
                }

                return true;
            }
        }

        static EntThinkAdapter Think_SpawnDoorTrigger = new AnonymousEntThinkAdapter20();
        private sealed class AnonymousEntThinkAdapter20 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "think_spawn_door_trigger";
            }

            public override bool Think(edict_t ent)
            {
                edict_t other;
                float[] mins = new float[]{0, 0, 0}, maxs = new float[]{0, 0, 0};
                if ((ent.flags & Defines.FL_TEAMSLAVE) != 0)
                    return true;
                Math3D.VectorCopy(ent.absmin, mins);
                Math3D.VectorCopy(ent.absmax, maxs);
                for (other = ent.teamchain; other != null; other = other.teamchain)
                {
                    GameBase.AddPointToBounds(other.absmin, mins, maxs);
                    GameBase.AddPointToBounds(other.absmax, mins, maxs);
                }

                mins[0] -= 60;
                mins[1] -= 60;
                maxs[0] += 60;
                maxs[1] += 60;
                other = GameUtil.G_Spawn();
                Math3D.VectorCopy(mins, other.mins);
                Math3D.VectorCopy(maxs, other.maxs);
                other.owner = ent;
                other.solid = Defines.SOLID_TRIGGER;
                other.movetype = Defines.MOVETYPE_NONE;
                other.touch = Touch_DoorTrigger;
                GameBase.gi.Linkentity(other);
                if ((ent.spawnflags & DOOR_START_OPEN) != 0)
                    Door_use_areaportals(ent, true);
                Think_CalcMoveSpeed.Think(ent);
                return true;
            }
        }

        static EntBlockedAdapter door_blocked = new AnonymousEntBlockedAdapter2();
        private sealed class AnonymousEntBlockedAdapter2 : EntBlockedAdapter
		{
			
            public override string GetID()
            {
                return "door_blocked";
            }

            public override void Blocked(edict_t self, edict_t other)
            {
                edict_t ent;
                if (0 == (other.svflags & Defines.SVF_MONSTER) && (null == other.client))
                {
                    GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, 100000, 1, 0, Defines.MOD_CRUSH);
                    if (other != null)
                        GameMisc.BecomeExplosion1(other);
                    return;
                }

                GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, self.dmg, 1, 0, Defines.MOD_CRUSH);
                if ((self.spawnflags & DOOR_CRUSHER) != 0)
                    return;
                if (self.moveinfo.wait >= 0)
                {
                    if (self.moveinfo.state == STATE_DOWN)
                    {
                        for (ent = self.teammaster; ent != null; ent = ent.teamchain)
                            Door_go_up(ent, ent.activator);
                    }
                    else
                    {
                        for (ent = self.teammaster; ent != null; ent = ent.teamchain)
                            door_go_down.Think(ent);
                    }
                }
            }
        }

        static EntDieAdapter door_killed = new AnonymousEntDieAdapter1();
        private sealed class AnonymousEntDieAdapter1 : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "door_killed";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                edict_t ent;
                for (ent = self.teammaster; ent != null; ent = ent.teamchain)
                {
                    ent.health = ent.max_health;
                    ent.takedamage = Defines.DAMAGE_NO;
                }

                door_use.Use(self.teammaster, attacker, attacker);
            }
        }

        static EntTouchAdapter door_touch = new AnonymousEntTouchAdapter4();
        private sealed class AnonymousEntTouchAdapter4 : EntTouchAdapter
		{
			
            public override string GetID()
            {
                return "door_touch";
            }

            public override void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf)
            {
                if (null == other.client)
                    return;
                if (GameBase.level.time < self.touch_debounce_time)
                    return;
                self.touch_debounce_time = GameBase.level.time + 5F;
                GameBase.gi.Centerprintf(other, self.message);
                GameBase.gi.Sound(other, Defines.CHAN_AUTO, GameBase.gi.Soundindex("misc/talk1.wav"), 1, Defines.ATTN_NORM, 0);
            }
        }

        public static EntThinkAdapter SP_func_door = new AnonymousEntThinkAdapter21();
        private sealed class AnonymousEntThinkAdapter21 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_func_door";
            }

            public override bool Think(edict_t ent)
            {
                float[] abs_movedir = new float[]{0, 0, 0};
                if (ent.sounds != 1)
                {
                    ent.moveinfo.sound_start = GameBase.gi.Soundindex("doors/dr1_strt.wav");
                    ent.moveinfo.sound_middle = GameBase.gi.Soundindex("doors/dr1_mid.wav");
                    ent.moveinfo.sound_end = GameBase.gi.Soundindex("doors/dr1_end.wav");
                }

                GameBase.G_SetMovedir(ent.s.angles, ent.movedir);
                ent.movetype = Defines.MOVETYPE_PUSH;
                ent.solid = Defines.SOLID_BSP;
                GameBase.gi.Setmodel(ent, ent.model);
                ent.blocked = door_blocked;
                ent.use = door_use;
                if (0 == ent.speed)
                    ent.speed = 100;
                if (GameBase.deathmatch.value != 0)
                    ent.speed *= 2;
                if (0 == ent.accel)
                    ent.accel = ent.speed;
                if (0 == ent.decel)
                    ent.decel = ent.speed;
                if (0 == ent.wait)
                    ent.wait = 3;
                if (0 == GameBase.st.lip)
                    GameBase.st.lip = 8;
                if (0 == ent.dmg)
                    ent.dmg = 2;
                Math3D.VectorCopy(ent.s.origin, ent.pos1);
                abs_movedir[0] = Math.Abs(ent.movedir[0]);
                abs_movedir[1] = Math.Abs(ent.movedir[1]);
                abs_movedir[2] = Math.Abs(ent.movedir[2]);
                ent.moveinfo.distance = abs_movedir[0] * ent.size[0] + abs_movedir[1] * ent.size[1] + abs_movedir[2] * ent.size[2] - GameBase.st.lip;
                Math3D.VectorMA(ent.pos1, ent.moveinfo.distance, ent.movedir, ent.pos2);
                if ((ent.spawnflags & DOOR_START_OPEN) != 0)
                {
                    Math3D.VectorCopy(ent.pos2, ent.s.origin);
                    Math3D.VectorCopy(ent.pos1, ent.pos2);
                    Math3D.VectorCopy(ent.s.origin, ent.pos1);
                }

                ent.moveinfo.state = STATE_BOTTOM;
                if (ent.health != 0)
                {
                    ent.takedamage = Defines.DAMAGE_YES;
                    ent.die = door_killed;
                    ent.max_health = ent.health;
                }
                else if (ent.targetname != null && ent.message != null)
                {
                    GameBase.gi.Soundindex("misc/talk.wav");
                    ent.touch = door_touch;
                }

                ent.moveinfo.speed = ent.speed;
                ent.moveinfo.accel = ent.accel;
                ent.moveinfo.decel = ent.decel;
                ent.moveinfo.wait = ent.wait;
                Math3D.VectorCopy(ent.pos1, ent.moveinfo.start_origin);
                Math3D.VectorCopy(ent.s.angles, ent.moveinfo.start_angles);
                Math3D.VectorCopy(ent.pos2, ent.moveinfo.end_origin);
                Math3D.VectorCopy(ent.s.angles, ent.moveinfo.end_angles);
                if ((ent.spawnflags & 16) != 0)
                    ent.s.effects |= Defines.EF_ANIM_ALL;
                if ((ent.spawnflags & 64) != 0)
                    ent.s.effects |= Defines.EF_ANIM_ALLFAST;
                if (null == ent.team)
                    ent.teammaster = ent;
                GameBase.gi.Linkentity(ent);
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                if (ent.health != 0 || ent.targetname != null)
                    ent.think = Think_CalcMoveSpeed;
                else
                    ent.think = Think_SpawnDoorTrigger;
                return true;
            }
        }

        public static EntThinkAdapter SP_func_door_rotating = new AnonymousEntThinkAdapter22();
        private sealed class AnonymousEntThinkAdapter22 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_func_door_rotating";
            }

            public override bool Think(edict_t ent)
            {
                Math3D.VectorClear(ent.s.angles);
                Math3D.VectorClear(ent.movedir);
                if ((ent.spawnflags & DOOR_X_AXIS) != 0)
                    ent.movedir[2] = 1F;
                else if ((ent.spawnflags & DOOR_Y_AXIS) != 0)
                    ent.movedir[0] = 1F;
                else
                    ent.movedir[1] = 1F;
                if ((ent.spawnflags & DOOR_REVERSE) != 0)
                    Math3D.VectorNegate(ent.movedir, ent.movedir);
                if (0 == GameBase.st.distance)
                {
                    GameBase.gi.Dprintf(ent.classname + " at " + Lib.Vtos(ent.s.origin) + " with no distance set\\n");
                    GameBase.st.distance = 90;
                }

                Math3D.VectorCopy(ent.s.angles, ent.pos1);
                Math3D.VectorMA(ent.s.angles, GameBase.st.distance, ent.movedir, ent.pos2);
                ent.moveinfo.distance = GameBase.st.distance;
                ent.movetype = Defines.MOVETYPE_PUSH;
                ent.solid = Defines.SOLID_BSP;
                GameBase.gi.Setmodel(ent, ent.model);
                ent.blocked = door_blocked;
                ent.use = door_use;
                if (0 == ent.speed)
                    ent.speed = 100;
                if (0 == ent.accel)
                    ent.accel = ent.speed;
                if (0 == ent.decel)
                    ent.decel = ent.speed;
                if (0 == ent.wait)
                    ent.wait = 3;
                if (0 == ent.dmg)
                    ent.dmg = 2;
                if (ent.sounds != 1)
                {
                    ent.moveinfo.sound_start = GameBase.gi.Soundindex("doors/dr1_strt.wav");
                    ent.moveinfo.sound_middle = GameBase.gi.Soundindex("doors/dr1_mid.wav");
                    ent.moveinfo.sound_end = GameBase.gi.Soundindex("doors/dr1_end.wav");
                }

                if ((ent.spawnflags & DOOR_START_OPEN) != 0)
                {
                    Math3D.VectorCopy(ent.pos2, ent.s.angles);
                    Math3D.VectorCopy(ent.pos1, ent.pos2);
                    Math3D.VectorCopy(ent.s.angles, ent.pos1);
                    Math3D.VectorNegate(ent.movedir, ent.movedir);
                }

                if (ent.health != 0)
                {
                    ent.takedamage = Defines.DAMAGE_YES;
                    ent.die = door_killed;
                    ent.max_health = ent.health;
                }

                if (ent.targetname != null && ent.message != null)
                {
                    GameBase.gi.Soundindex("misc/talk.wav");
                    ent.touch = door_touch;
                }

                ent.moveinfo.state = STATE_BOTTOM;
                ent.moveinfo.speed = ent.speed;
                ent.moveinfo.accel = ent.accel;
                ent.moveinfo.decel = ent.decel;
                ent.moveinfo.wait = ent.wait;
                Math3D.VectorCopy(ent.s.origin, ent.moveinfo.start_origin);
                Math3D.VectorCopy(ent.pos1, ent.moveinfo.start_angles);
                Math3D.VectorCopy(ent.s.origin, ent.moveinfo.end_origin);
                Math3D.VectorCopy(ent.pos2, ent.moveinfo.end_angles);
                if ((ent.spawnflags & 16) != 0)
                    ent.s.effects |= Defines.EF_ANIM_ALL;
                if (ent.team == null)
                    ent.teammaster = ent;
                GameBase.gi.Linkentity(ent);
                ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
                if (ent.health != 0 || ent.targetname != null)
                    ent.think = Think_CalcMoveSpeed;
                else
                    ent.think = Think_SpawnDoorTrigger;
                return true;
            }
        }

        public static readonly int TRAIN_START_ON = 1;
        public static readonly int TRAIN_TOGGLE = 2;
        public static readonly int TRAIN_BLOCK_STOPS = 4;
        static EntBlockedAdapter train_blocked = new AnonymousEntBlockedAdapter3();
        private sealed class AnonymousEntBlockedAdapter3 : EntBlockedAdapter
		{
			
            public override string GetID()
            {
                return "train_blocked";
            }

            public override void Blocked(edict_t self, edict_t other)
            {
                if (0 == (other.svflags & Defines.SVF_MONSTER) && (null == other.client))
                {
                    GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, 100000, 1, 0, Defines.MOD_CRUSH);
                    if (other != null)
                        GameMisc.BecomeExplosion1(other);
                    return;
                }

                if (GameBase.level.time < self.touch_debounce_time)
                    return;
                if (self.dmg == 0)
                    return;
                self.touch_debounce_time = GameBase.level.time + 0.5F;
                GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, self.dmg, 1, 0, Defines.MOD_CRUSH);
            }
        }

        static EntThinkAdapter train_wait = new AnonymousEntThinkAdapter23();
        private sealed class AnonymousEntThinkAdapter23 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "train_wait";
            }

            public override bool Think(edict_t self)
            {
                if (self.target_ent.pathtarget != null)
                {
                    string savetarget;
                    edict_t ent;
                    ent = self.target_ent;
                    savetarget = ent.target;
                    ent.target = ent.pathtarget;
                    GameUtil.G_UseTargets(ent, self.activator);
                    ent.target = savetarget;
                    if (!self.inuse)
                        return true;
                }

                if (self.moveinfo.wait != 0)
                {
                    if (self.moveinfo.wait > 0)
                    {
                        self.nextthink = GameBase.level.time + self.moveinfo.wait;
                        self.think = train_next;
                    }
                    else if (0 != (self.spawnflags & TRAIN_TOGGLE))
                    {
                        train_next.Think(self);
                        self.spawnflags &= ~TRAIN_START_ON;
                        Math3D.VectorClear(self.velocity);
                        self.nextthink = 0;
                    }

                    if (0 == (self.flags & Defines.FL_TEAMSLAVE))
                    {
                        if (self.moveinfo.sound_end != 0)
                            GameBase.gi.Sound(self, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, self.moveinfo.sound_end, 1, Defines.ATTN_STATIC, 0);
                        self.s.sound = 0;
                    }
                }
                else
                {
                    train_next.Think(self);
                }

                return true;
            }
        }

        static EntThinkAdapter train_next = new AnonymousEntThinkAdapter24();
        private sealed class AnonymousEntThinkAdapter24 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "train_next";
            }

            public override bool Think(edict_t self)
            {
                edict_t ent = null;
                float[] dest = new float[]{0, 0, 0};
                bool first;
                first = true;
                bool dogoto = true;
                while (dogoto)
                {
                    if (null == self.target)
                    {
                        return true;
                    }

                    ent = GameBase.G_PickTarget(self.target);
                    if (null == ent)
                    {
                        GameBase.gi.Dprintf("train_next: bad target " + self.target + "\\n");
                        return true;
                    }

                    self.target = ent.target;
                    dogoto = false;
                    if ((ent.spawnflags & 1) != 0)
                    {
                        if (!first)
                        {
                            GameBase.gi.Dprintf("connected teleport path_corners, see " + ent.classname + " at " + Lib.Vtos(ent.s.origin) + "\\n");
                            return true;
                        }

                        first = false;
                        Math3D.VectorSubtract(ent.s.origin, self.mins, self.s.origin);
                        Math3D.VectorCopy(self.s.origin, self.s.old_origin);
                        self.s.event_renamed = Defines.EV_OTHER_TELEPORT;
                        GameBase.gi.Linkentity(self);
                        dogoto = true;
                    }
                }

                self.moveinfo.wait = ent.wait;
                self.target_ent = ent;
                if (0 == (self.flags & Defines.FL_TEAMSLAVE))
                {
                    if (self.moveinfo.sound_start != 0)
                        GameBase.gi.Sound(self, Defines.CHAN_NO_PHS_ADD + Defines.CHAN_VOICE, self.moveinfo.sound_start, 1, Defines.ATTN_STATIC, 0);
                    self.s.sound = self.moveinfo.sound_middle;
                }

                Math3D.VectorSubtract(ent.s.origin, self.mins, dest);
                self.moveinfo.state = STATE_TOP;
                Math3D.VectorCopy(self.s.origin, self.moveinfo.start_origin);
                Math3D.VectorCopy(dest, self.moveinfo.end_origin);
                Move_Calc(self, dest, train_wait);
                self.spawnflags |= TRAIN_START_ON;
                return true;
            }
        }

        public static EntThinkAdapter func_train_find = new AnonymousEntThinkAdapter25();
        private sealed class AnonymousEntThinkAdapter25 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "func_train_find";
            }

            public override bool Think(edict_t self)
            {
                edict_t ent;
                if (null == self.target)
                {
                    GameBase.gi.Dprintf("train_find: no target\\n");
                    return true;
                }

                ent = GameBase.G_PickTarget(self.target);
                if (null == ent)
                {
                    GameBase.gi.Dprintf("train_find: target " + self.target + " not found\\n");
                    return true;
                }

                self.target = ent.target;
                Math3D.VectorSubtract(ent.s.origin, self.mins, self.s.origin);
                GameBase.gi.Linkentity(self);
                if (null == self.targetname)
                    self.spawnflags |= TRAIN_START_ON;
                if ((self.spawnflags & TRAIN_START_ON) != 0)
                {
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                    self.think = train_next;
                    self.activator = self;
                }

                return true;
            }
        }

        public static EntUseAdapter train_use = new AnonymousEntUseAdapter4();
        private sealed class AnonymousEntUseAdapter4 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "train_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.activator = activator;
                if ((self.spawnflags & TRAIN_START_ON) != 0)
                {
                    if (0 == (self.spawnflags & TRAIN_TOGGLE))
                        return;
                    self.spawnflags &= ~TRAIN_START_ON;
                    Math3D.VectorClear(self.velocity);
                    self.nextthink = 0;
                }
                else
                {
                    if (self.target_ent != null)
                        Train_resume(self);
                    else
                        train_next.Think(self);
                }
            }
        }

        static EntUseAdapter trigger_elevator_use = new AnonymousEntUseAdapter5();
        private sealed class AnonymousEntUseAdapter5 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "trigger_elevator_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                edict_t target;
                if (0 != self.movetarget.nextthink)
                {
                    return;
                }

                if (null == other.pathtarget)
                {
                    GameBase.gi.Dprintf("elevator used with no pathtarget\\n");
                    return;
                }

                target = GameBase.G_PickTarget(other.pathtarget);
                if (null == target)
                {
                    GameBase.gi.Dprintf("elevator used with bad pathtarget: " + other.pathtarget + "\\n");
                    return;
                }

                self.movetarget.target_ent = target;
                Train_resume(self.movetarget);
            }
        }

        static EntThinkAdapter trigger_elevator_init = new AnonymousEntThinkAdapter26();
        private sealed class AnonymousEntThinkAdapter26 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "trigger_elevator_init";
            }

            public override bool Think(edict_t self)
            {
                if (null == self.target)
                {
                    GameBase.gi.Dprintf("trigger_elevator has no target\\n");
                    return true;
                }

                self.movetarget = GameBase.G_PickTarget(self.target);
                if (null == self.movetarget)
                {
                    GameBase.gi.Dprintf("trigger_elevator unable to find target " + self.target + "\\n");
                    return true;
                }

                if (Lib.Strcmp(self.movetarget.classname, "func_train") != 0)
                {
                    GameBase.gi.Dprintf("trigger_elevator target " + self.target + " is not a train\\n");
                    return true;
                }

                self.use = trigger_elevator_use;
                self.svflags = Defines.SVF_NOCLIENT;
                return true;
            }
        }

        public static EntThinkAdapter SP_trigger_elevator = new AnonymousEntThinkAdapter27();
        private sealed class AnonymousEntThinkAdapter27 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_trigger_elevator";
            }

            public override bool Think(edict_t self)
            {
                self.think = trigger_elevator_init;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                return true;
            }
        }

        static EntThinkAdapter func_timer_think = new AnonymousEntThinkAdapter28();
        private sealed class AnonymousEntThinkAdapter28 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "func_timer_think";
            }

            public override bool Think(edict_t self)
            {
                GameUtil.G_UseTargets(self, self.activator);
                self.nextthink = GameBase.level.time + self.wait + Lib.Crandom() * self.random;
                return true;
            }
        }

        static EntUseAdapter func_timer_use = new AnonymousEntUseAdapter6();
        private sealed class AnonymousEntUseAdapter6 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "func_timer_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                self.activator = activator;
                if (self.nextthink != 0)
                {
                    self.nextthink = 0;
                    return;
                }

                if (self.delay != 0)
                    self.nextthink = GameBase.level.time + self.delay;
                else
                    func_timer_think.Think(self);
            }
        }

        static EntUseAdapter func_conveyor_use = new AnonymousEntUseAdapter7();
        private sealed class AnonymousEntUseAdapter7 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "func_conveyor_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if ((self.spawnflags & 1) != 0)
                {
                    self.speed = 0;
                    self.spawnflags &= ~1;
                }
                else
                {
                    self.speed = self.count;
                    self.spawnflags |= 1;
                }

                if (0 == (self.spawnflags & 2))
                    self.count = 0;
            }
        }

        public static EntThinkAdapter SP_func_conveyor = new AnonymousEntThinkAdapter29();
        private sealed class AnonymousEntThinkAdapter29 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_func_conveyor";
            }

            public override bool Think(edict_t self)
            {
                if (0 == self.speed)
                    self.speed = 100;
                if (0 == (self.spawnflags & 1))
                {
                    self.count = (int)self.speed;
                    self.speed = 0;
                }

                self.use = func_conveyor_use;
                GameBase.gi.Setmodel(self, self.model);
                self.solid = Defines.SOLID_BSP;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        public static readonly int SECRET_ALWAYS_SHOOT = 1;
        public static readonly int SECRET_1ST_LEFT = 2;
        public static readonly int SECRET_1ST_DOWN = 4;
        static EntUseAdapter door_secret_use = new AnonymousEntUseAdapter8();
        private sealed class AnonymousEntUseAdapter8 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if (!Math3D.VectorEquals(self.s.origin, Globals.vec3_origin))
                    return;
                Move_Calc(self, self.pos1, door_secret_move1);
                Door_use_areaportals(self, true);
            }
        }

        static EntThinkAdapter door_secret_move1 = new AnonymousEntThinkAdapter30();
        private sealed class AnonymousEntThinkAdapter30 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_move1";
            }

            public override bool Think(edict_t self)
            {
                self.nextthink = GameBase.level.time + 1F;
                self.think = door_secret_move2;
                return true;
            }
        }

        static EntThinkAdapter door_secret_move2 = new AnonymousEntThinkAdapter31();
        private sealed class AnonymousEntThinkAdapter31 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_move2";
            }

            public override bool Think(edict_t self)
            {
                Move_Calc(self, self.pos2, door_secret_move3);
                return true;
            }
        }

        static EntThinkAdapter door_secret_move3 = new AnonymousEntThinkAdapter32();
        private sealed class AnonymousEntThinkAdapter32 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_move3";
            }

            public override bool Think(edict_t self)
            {
                if (self.wait == -1)
                    return true;
                self.nextthink = GameBase.level.time + self.wait;
                self.think = door_secret_move4;
                return true;
            }
        }

        static EntThinkAdapter door_secret_move4 = new AnonymousEntThinkAdapter33();
        private sealed class AnonymousEntThinkAdapter33 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_move4";
            }

            public override bool Think(edict_t self)
            {
                Move_Calc(self, self.pos1, door_secret_move5);
                return true;
            }
        }

        static EntThinkAdapter door_secret_move5 = new AnonymousEntThinkAdapter34();
        private sealed class AnonymousEntThinkAdapter34 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_move5";
            }

            public override bool Think(edict_t self)
            {
                self.nextthink = GameBase.level.time + 1F;
                self.think = door_secret_move6;
                return true;
            }
        }

        static EntThinkAdapter door_secret_move6 = new AnonymousEntThinkAdapter35();
        private sealed class AnonymousEntThinkAdapter35 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_move6";
            }

            public override bool Think(edict_t self)
            {
                Move_Calc(self, Globals.vec3_origin, door_secret_done);
                return true;
            }
        }

        static EntThinkAdapter door_secret_done = new AnonymousEntThinkAdapter36();
        private sealed class AnonymousEntThinkAdapter36 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_move7";
            }

            public override bool Think(edict_t self)
            {
                if (null == (self.targetname) || 0 != (self.spawnflags & SECRET_ALWAYS_SHOOT))
                {
                    self.health = 0;
                    self.takedamage = Defines.DAMAGE_YES;
                }

                Door_use_areaportals(self, false);
                return true;
            }
        }

        static EntBlockedAdapter door_secret_blocked = new AnonymousEntBlockedAdapter4();
        private sealed class AnonymousEntBlockedAdapter4 : EntBlockedAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_blocked";
            }

            public override void Blocked(edict_t self, edict_t other)
            {
                if (0 == (other.svflags & Defines.SVF_MONSTER) && (null == other.client))
                {
                    GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, 100000, 1, 0, Defines.MOD_CRUSH);
                    if (other != null)
                        GameMisc.BecomeExplosion1(other);
                    return;
                }

                if (GameBase.level.time < self.touch_debounce_time)
                    return;
                self.touch_debounce_time = GameBase.level.time + 0.5F;
                GameCombat.T_Damage(other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, self.dmg, 1, 0, Defines.MOD_CRUSH);
            }
        }

        static EntDieAdapter door_secret_die = new AnonymousEntDieAdapter2();
        private sealed class AnonymousEntDieAdapter2 : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "door_secret_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                self.takedamage = Defines.DAMAGE_NO;
                door_secret_use.Use(self, attacker, attacker);
            }
        }

        public static EntThinkAdapter SP_func_door_secret = new AnonymousEntThinkAdapter37();
        private sealed class AnonymousEntThinkAdapter37 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_func_door_secret";
            }

            public override bool Think(edict_t ent)
            {
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0}, up = new float[]{0, 0, 0};
                float side;
                float width;
                float length;
                ent.moveinfo.sound_start = GameBase.gi.Soundindex("doors/dr1_strt.wav");
                ent.moveinfo.sound_middle = GameBase.gi.Soundindex("doors/dr1_mid.wav");
                ent.moveinfo.sound_end = GameBase.gi.Soundindex("doors/dr1_end.wav");
                ent.movetype = Defines.MOVETYPE_PUSH;
                ent.solid = Defines.SOLID_BSP;
                GameBase.gi.Setmodel(ent, ent.model);
                ent.blocked = door_secret_blocked;
                ent.use = door_secret_use;
                if (null == (ent.targetname) || 0 != (ent.spawnflags & SECRET_ALWAYS_SHOOT))
                {
                    ent.health = 0;
                    ent.takedamage = Defines.DAMAGE_YES;
                    ent.die = door_secret_die;
                }

                if (0 == ent.dmg)
                    ent.dmg = 2;
                if (0 == ent.wait)
                    ent.wait = 5;
                ent.moveinfo.accel = ent.moveinfo.decel = ent.moveinfo.speed = 50;
                Math3D.AngleVectors(ent.s.angles, forward, right, up);
                Math3D.VectorClear(ent.s.angles);
                side = 1F - (ent.spawnflags & SECRET_1ST_LEFT);
                if ((ent.spawnflags & SECRET_1ST_DOWN) != 0)
                    width = Math.Abs(Math3D.DotProduct(up, ent.size));
                else
                    width = Math.Abs(Math3D.DotProduct(right, ent.size));
                length = Math.Abs(Math3D.DotProduct(forward, ent.size));
                if ((ent.spawnflags & SECRET_1ST_DOWN) != 0)
                    Math3D.VectorMA(ent.s.origin, -1 * width, up, ent.pos1);
                else
                    Math3D.VectorMA(ent.s.origin, side * width, right, ent.pos1);
                Math3D.VectorMA(ent.pos1, length, forward, ent.pos2);
                if (ent.health != 0)
                {
                    ent.takedamage = Defines.DAMAGE_YES;
                    ent.die = door_killed;
                    ent.max_health = ent.health;
                }
                else if (ent.targetname != null && ent.message != null)
                {
                    GameBase.gi.Soundindex("misc/talk.wav");
                    ent.touch = door_touch;
                }

                ent.classname = "func_door";
                GameBase.gi.Linkentity(ent);
                return true;
            }
        }

        static EntUseAdapter use_killbox = new AnonymousEntUseAdapter9();
        private sealed class AnonymousEntUseAdapter9 : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "use_killbox";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                GameUtil.KillBox(self);
            }
        }

        public static EntThinkAdapter SP_func_killbox = new AnonymousEntThinkAdapter38();
        private sealed class AnonymousEntThinkAdapter38 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "sp_func_killbox";
            }

            public override bool Think(edict_t ent)
            {
                GameBase.gi.Setmodel(ent, ent.model);
                ent.use = use_killbox;
                ent.svflags = Defines.SVF_NOCLIENT;
                return true;
            }
        }
    }
}
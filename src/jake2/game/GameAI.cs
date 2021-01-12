using Jake2.Client;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class GameAI
    {
        public static void AttackFinished(edict_t self, float time)
        {
            self.monsterinfo.attack_finished = GameBase.level.time + time;
        }

        public static void Ai_turn(edict_t self, float dist)
        {
            if (dist != 0)
                M.M_walkmove(self, self.s.angles[Defines.YAW], dist);
            if (GameUtil.FindTarget(self))
                return;
            M.M_ChangeYaw(self);
        }

        public static bool FacingIdeal(edict_t self)
        {
            float delta;
            delta = Math3D.Anglemod(self.s.angles[Defines.YAW] - self.ideal_yaw);
            if (delta > 45 && delta < 315)
                return false;
            return true;
        }

        public static void Ai_run_melee(edict_t self)
        {
            self.ideal_yaw = enemy_yaw;
            M.M_ChangeYaw(self);
            if (FacingIdeal(self))
            {
                self.monsterinfo.melee.Think(self);
                self.monsterinfo.attack_state = Defines.AS_STRAIGHT;
            }
        }

        public static void Ai_run_missile(edict_t self)
        {
            self.ideal_yaw = enemy_yaw;
            M.M_ChangeYaw(self);
            if (FacingIdeal(self))
            {
                self.monsterinfo.attack.Think(self);
                self.monsterinfo.attack_state = Defines.AS_STRAIGHT;
            }
        }

        public static void Ai_run_slide(edict_t self, float distance)
        {
            float ofs;
            self.ideal_yaw = enemy_yaw;
            M.M_ChangeYaw(self);
            if (self.monsterinfo.lefty != 0)
                ofs = 90;
            else
                ofs = -90;
            if (M.M_walkmove(self, self.ideal_yaw + ofs, distance))
                return;
            self.monsterinfo.lefty = 1 - self.monsterinfo.lefty;
            M.M_walkmove(self, self.ideal_yaw - ofs, distance);
        }

        public static bool Ai_checkattack(edict_t self, float dist)
        {
            float[] temp = new[]{0f, 0f, 0f};
            bool hesDeadJim;
            if (self.goalentity != null)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_COMBAT_POINT) != 0)
                    return false;
                if ((self.monsterinfo.aiflags & Defines.AI_SOUND_TARGET) != 0)
                {
                    if ((GameBase.level.time - self.enemy.teleport_time) > 5)
                    {
                        if (self.goalentity == self.enemy)
                            if (self.movetarget != null)
                                self.goalentity = self.movetarget;
                            else
                                self.goalentity = null;
                        self.monsterinfo.aiflags &= ~Defines.AI_SOUND_TARGET;
                        if ((self.monsterinfo.aiflags & Defines.AI_TEMP_STAND_GROUND) != 0)
                            self.monsterinfo.aiflags &= ~(Defines.AI_STAND_GROUND | Defines.AI_TEMP_STAND_GROUND);
                    }
                    else
                    {
                        self.show_hostile = (int)GameBase.level.time + 1;
                        return false;
                    }
                }
            }

            enemy_vis = false;
            hesDeadJim = false;
            if ((null == self.enemy) || (!self.enemy.inuse))
            {
                hesDeadJim = true;
            }
            else if ((self.monsterinfo.aiflags & Defines.AI_MEDIC) != 0)
            {
                if (self.enemy.health > 0)
                {
                    hesDeadJim = true;
                    self.monsterinfo.aiflags &= ~Defines.AI_MEDIC;
                }
            }
            else
            {
                if ((self.monsterinfo.aiflags & Defines.AI_BRUTAL) != 0)
                {
                    if (self.enemy.health <= -80)
                        hesDeadJim = true;
                }
                else
                {
                    if (self.enemy.health <= 0)
                        hesDeadJim = true;
                }
            }

            if (hesDeadJim)
            {
                self.enemy = null;
                if (self.oldenemy != null && self.oldenemy.health > 0)
                {
                    self.enemy = self.oldenemy;
                    self.oldenemy = null;
                    HuntTarget(self);
                }
                else
                {
                    if (self.movetarget != null)
                    {
                        self.goalentity = self.movetarget;
                        self.monsterinfo.walk.Think(self);
                    }
                    else
                    {
                        self.monsterinfo.pausetime = GameBase.level.time + 100000000;
                        self.monsterinfo.stand.Think(self);
                    }

                    return true;
                }
            }

            self.show_hostile = (int)GameBase.level.time + 1;
            enemy_vis = GameUtil.Visible(self, self.enemy);
            if (enemy_vis)
            {
                self.monsterinfo.search_time = GameBase.level.time + 5;
                Math3D.VectorCopy(self.enemy.s.origin, self.monsterinfo.last_sighting);
            }

            enemy_infront = GameUtil.Infront(self, self.enemy);
            enemy_range = GameUtil.Range(self, self.enemy);
            Math3D.VectorSubtract(self.enemy.s.origin, self.s.origin, temp);
            enemy_yaw = Math3D.Vectoyaw(temp);
            if (self.monsterinfo.attack_state == Defines.AS_MISSILE)
            {
                Ai_run_missile(self);
                return true;
            }

            if (self.monsterinfo.attack_state == Defines.AS_MELEE)
            {
                Ai_run_melee(self);
                return true;
            }

            if (!enemy_vis)
                return false;
            return self.monsterinfo.checkattack.Think(self);
        }

        static void Ai_walk(edict_t self, float dist)
        {
            M.M_MoveToGoal(self, dist);
            if (GameUtil.FindTarget(self))
                return;
            if ((self.monsterinfo.search != null) && (GameBase.level.time > self.monsterinfo.idle_time))
            {
                if (self.monsterinfo.idle_time != 0)
                {
                    self.monsterinfo.search.Think(self);
                    self.monsterinfo.idle_time = GameBase.level.time + 15 + Lib.Random() * 15;
                }
                else
                {
                    self.monsterinfo.idle_time = GameBase.level.time + Lib.Random() * 15;
                }
            }
        }

        public static void AI_SetSightClient()
        {
            edict_t ent;
            int start, check;
            if (GameBase.level.sight_client == null)
                start = 1;
            else
                start = GameBase.level.sight_client.index;
            check = start;
            while (true)
            {
                check++;
                if (check > GameBase.game.maxclients)
                    check = 1;
                ent = GameBase.g_edicts[check];
                if (ent.inuse && ent.health > 0 && (ent.flags & Defines.FL_NOTARGET) == 0)
                {
                    GameBase.level.sight_client = ent;
                    return;
                }

                if (check == start)
                {
                    GameBase.level.sight_client = null;
                    return;
                }
            }
        }

        static void Ai_move(edict_t self, float dist)
        {
            M.M_walkmove(self, self.s.angles[Defines.YAW], dist);
        }

        public static void HuntTarget(edict_t self)
        {
            float[] vec = new[]{0f, 0f, 0f};
            self.goalentity = self.enemy;
            if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                self.monsterinfo.stand.Think(self);
            else
                self.monsterinfo.run.Think(self);
            Math3D.VectorSubtract(self.enemy.s.origin, self.s.origin, vec);
            self.ideal_yaw = Math3D.Vectoyaw(vec);
            if (0 == (self.monsterinfo.aiflags & Defines.AI_STAND_GROUND))
                GameUtil.AttackFinished(self, 1);
        }

        public static EntThinkAdapter walkmonster_start_go = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "walkmonster_start_go";
            }

            public override bool Think(edict_t self)
            {
                if (0 == (self.spawnflags & 2) && GameBase.level.time < 1)
                {
                    M.M_droptofloor.Think(self);
                    if (self.groundentity != null)
                        if (!M.M_walkmove(self, 0, 0))
                            GameBase.gi.Dprintf(self.classname + " in solid at " + Lib.Vtos(self.s.origin) + "\\n");
                }

                if (0 == self.yaw_speed)
                    self.yaw_speed = 40;
                self.viewheight = 25;
                Monster.Monster_start_go(self);
                if ((self.spawnflags & 2) != 0)
                    Monster.monster_triggered_start.Think(self);
                return true;
            }
        }

        public static EntThinkAdapter walkmonster_start = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "walkmonster_start";
            }

            public override bool Think(edict_t self)
            {
                self.think = walkmonster_start_go;
                Monster.Monster_start(self);
                return true;
            }
        }

        public static EntThinkAdapter flymonster_start_go = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flymonster_start_go";
            }

            public override bool Think(edict_t self)
            {
                if (!M.M_walkmove(self, 0, 0))
                    GameBase.gi.Dprintf(self.classname + " in solid at " + Lib.Vtos(self.s.origin) + "\\n");
                if (0 == self.yaw_speed)
                    self.yaw_speed = 20;
                self.viewheight = 25;
                Monster.Monster_start_go(self);
                if ((self.spawnflags & 2) != 0)
                    Monster.monster_triggered_start.Think(self);
                return true;
            }
        }

        public static EntThinkAdapter flymonster_start = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flymonster_start";
            }

            public override bool Think(edict_t self)
            {
                self.flags |= Defines.FL_FLY;
                self.think = flymonster_start_go;
                Monster.Monster_start(self);
                return true;
            }
        }

        public static EntThinkAdapter swimmonster_start_go = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "swimmonster_start_go";
            }

            public override bool Think(edict_t self)
            {
                if (0 == self.yaw_speed)
                    self.yaw_speed = 20;
                self.viewheight = 10;
                Monster.Monster_start_go(self);
                if ((self.spawnflags & 2) != 0)
                    Monster.monster_triggered_start.Think(self);
                return true;
            }
        }

        public static EntThinkAdapter swimmonster_start = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "swimmonster_start";
            }

            public override bool Think(edict_t self)
            {
                self.flags |= Defines.FL_SWIM;
                self.think = swimmonster_start_go;
                Monster.Monster_start(self);
                return true;
            }
        }

        public static AIAdapter ai_turn = new AnonymousAIAdapter();
        private sealed class AnonymousAIAdapter : AIAdapter
        {
            public override string GetID()
            {
                return "ai_turn";
            }

            public override void Ai(edict_t self, float dist)
            {
                if (dist != 0)
                    M.M_walkmove(self, self.s.angles[Defines.YAW], dist);
                if (GameUtil.FindTarget(self))
                    return;
                M.M_ChangeYaw(self);
            }
        }

        public static AIAdapter ai_move = new AnonymousAIAdapter1();
        private sealed class AnonymousAIAdapter1 : AIAdapter
        {
            public override string GetID()
            {
                return "ai_move";
            }

            public override void Ai(edict_t self, float dist)
            {
                M.M_walkmove(self, self.s.angles[Defines.YAW], dist);
            }
        }

        public static AIAdapter ai_walk = new AnonymousAIAdapter2();
        private sealed class AnonymousAIAdapter2 : AIAdapter
        {
            public override string GetID()
            {
                return "ai_walk";
            }

            public override void Ai(edict_t self, float dist)
            {
                M.M_MoveToGoal(self, dist);
                if (GameUtil.FindTarget(self))
                    return;
                if ((self.monsterinfo.search != null) && (GameBase.level.time > self.monsterinfo.idle_time))
                {
                    if (self.monsterinfo.idle_time != 0)
                    {
                        self.monsterinfo.search.Think(self);
                        self.monsterinfo.idle_time = GameBase.level.time + 15 + ( float ) Globals.rnd.NextDouble() * 15;
                    }
                    else
                    {
                        self.monsterinfo.idle_time = GameBase.level.time + ( float ) Globals.rnd.NextDouble() * 15;
                    }
                }
            }
        }

        public static AIAdapter ai_stand = new AnonymousAIAdapter3();
        private sealed class AnonymousAIAdapter3 : AIAdapter
        {
            public override string GetID()
            {
                return "ai_stand";
            }

            public override void Ai(edict_t self, float dist)
            {
                float[] v = new float[]{0, 0, 0};
                if (dist != 0)
                    M.M_walkmove(self, self.s.angles[Defines.YAW], dist);
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                {
                    if (self.enemy != null)
                    {
                        Math3D.VectorSubtract(self.enemy.s.origin, self.s.origin, v);
                        self.ideal_yaw = Math3D.Vectoyaw(v);
                        if (self.s.angles[Defines.YAW] != self.ideal_yaw && 0 != (self.monsterinfo.aiflags & Defines.AI_TEMP_STAND_GROUND))
                        {
                            self.monsterinfo.aiflags &= ~(Defines.AI_STAND_GROUND | Defines.AI_TEMP_STAND_GROUND);
                            self.monsterinfo.run.Think(self);
                        }

                        M.M_ChangeYaw(self);
                        Ai_checkattack(self, 0);
                    }
                    else
                        GameUtil.FindTarget(self);
                    return;
                }

                if (GameUtil.FindTarget(self))
                    return;
                if (GameBase.level.time > self.monsterinfo.pausetime)
                {
                    self.monsterinfo.walk.Think(self);
                    return;
                }

                if (0 == (self.spawnflags & 1) && (self.monsterinfo.idle != null) && (GameBase.level.time > self.monsterinfo.idle_time))
                {
                    if (self.monsterinfo.idle_time != 0)
                    {
                        self.monsterinfo.idle.Think(self);
                        self.monsterinfo.idle_time = GameBase.level.time + 15 + ( float ) Globals.rnd.NextDouble() * 15;
                    }
                    else
                    {
                        self.monsterinfo.idle_time = GameBase.level.time + ( float ) Globals.rnd.NextDouble() * 15;
                    }
                }
            }
        }

        public static AIAdapter ai_charge = new AnonymousAIAdapter4();
        private sealed class AnonymousAIAdapter4 : AIAdapter
        {
            public override string GetID()
            {
                return "ai_charge";
            }

            public override void Ai(edict_t self, float dist)
            {
                float[] v = new float[]{0, 0, 0};
                Math3D.VectorSubtract(self.enemy.s.origin, self.s.origin, v);
                self.ideal_yaw = Math3D.Vectoyaw(v);
                M.M_ChangeYaw(self);
                if (dist != 0)
                    M.M_walkmove(self, self.s.angles[Defines.YAW], dist);
            }
        }

        public static AIAdapter ai_run = new AnonymousAIAdapter5();
        private sealed class AnonymousAIAdapter5 : AIAdapter
        {
            public override string GetID()
            {
                return "ai_run";
            }

            public override void Ai(edict_t self, float dist)
            {
                float[] v = new float[]{0, 0, 0};
                edict_t tempgoal;
                edict_t save;
                bool new1;
                edict_t marker;
                float d1, d2;
                trace_t tr;
                float[] v_forward = new float[]{0, 0, 0}, v_right = new float[]{0, 0, 0};
                float left, center, right;
                float[] left_target = new float[]{0, 0, 0}, right_target = new float[]{0, 0, 0};
                if ((self.monsterinfo.aiflags & Defines.AI_COMBAT_POINT) != 0)
                {
                    M.M_MoveToGoal(self, dist);
                    return;
                }

                if ((self.monsterinfo.aiflags & Defines.AI_SOUND_TARGET) != 0)
                {
                    Math3D.VectorSubtract(self.s.origin, self.enemy.s.origin, v);
                    if (Math3D.VectorLength(v) < 64)
                    {
                        self.monsterinfo.stand.Think(self);
                        self.spawnflags &= ~1;
                        self.enemy = null;
                    }
                    else
                        M.M_MoveToGoal(self, dist);
                    if (!GameUtil.FindTarget(self))
                        return;
                }

                if (Ai_checkattack(self, dist))
                    return;
                if (self.monsterinfo.attack_state == Defines.AS_SLIDING)
                {
                    Ai_run_slide(self, dist);
                    return;
                }

                if (enemy_vis)
                {
                    M.M_MoveToGoal(self, dist);
                    self.monsterinfo.aiflags &= ~Defines.AI_LOST_SIGHT;
                    Math3D.VectorCopy(self.enemy.s.origin, self.monsterinfo.last_sighting);
                    self.monsterinfo.trail_time = GameBase.level.time;
                    return;
                }

                if (GameBase.coop.value != 0)
                {
                    if (GameUtil.FindTarget(self))
                        return;
                }

                if ((self.monsterinfo.search_time != 0) && (GameBase.level.time > (self.monsterinfo.search_time + 20)))
                {
                    M.M_MoveToGoal(self, dist);
                    self.monsterinfo.search_time = 0;
                    return;
                }

                save = self.goalentity;
                tempgoal = GameUtil.G_Spawn();
                self.goalentity = tempgoal;
                new1 = false;
                if (0 == (self.monsterinfo.aiflags & Defines.AI_LOST_SIGHT))
                {
                    self.monsterinfo.aiflags |= (Defines.AI_LOST_SIGHT | Defines.AI_PURSUIT_LAST_SEEN);
                    self.monsterinfo.aiflags &= ~(Defines.AI_PURSUE_NEXT | Defines.AI_PURSUE_TEMP);
                    new1 = true;
                }

                if ((self.monsterinfo.aiflags & Defines.AI_PURSUE_NEXT) != 0)
                {
                    self.monsterinfo.aiflags &= ~Defines.AI_PURSUE_NEXT;
                    self.monsterinfo.search_time = GameBase.level.time + 5;
                    if ((self.monsterinfo.aiflags & Defines.AI_PURSUE_TEMP) != 0)
                    {
                        self.monsterinfo.aiflags &= ~Defines.AI_PURSUE_TEMP;
                        marker = null;
                        Math3D.VectorCopy(self.monsterinfo.saved_goal, self.monsterinfo.last_sighting);
                        new1 = true;
                    }
                    else if ((self.monsterinfo.aiflags & Defines.AI_PURSUIT_LAST_SEEN) != 0)
                    {
                        self.monsterinfo.aiflags &= ~Defines.AI_PURSUIT_LAST_SEEN;
                        marker = PlayerTrail.PickFirst(self);
                    }
                    else
                    {
                        marker = PlayerTrail.PickNext(self);
                    }

                    if (marker != null)
                    {
                        Math3D.VectorCopy(marker.s.origin, self.monsterinfo.last_sighting);
                        self.monsterinfo.trail_time = marker.timestamp;
                        self.s.angles[Defines.YAW] = self.ideal_yaw = marker.s.angles[Defines.YAW];
                        new1 = true;
                    }
                }

                Math3D.VectorSubtract(self.s.origin, self.monsterinfo.last_sighting, v);
                d1 = Math3D.VectorLength(v);
                if (d1 <= dist)
                {
                    self.monsterinfo.aiflags |= Defines.AI_PURSUE_NEXT;
                    dist = d1;
                }

                Math3D.VectorCopy(self.monsterinfo.last_sighting, self.goalentity.s.origin);
                if (new1)
                {
                    tr = GameBase.gi.Trace(self.s.origin, self.mins, self.maxs, self.monsterinfo.last_sighting, self, Defines.MASK_PLAYERSOLID);
                    if (tr.fraction < 1)
                    {
                        Math3D.VectorSubtract(self.goalentity.s.origin, self.s.origin, v);
                        d1 = Math3D.VectorLength(v);
                        center = tr.fraction;
                        d2 = d1 * ((center + 1) / 2);
                        self.s.angles[Defines.YAW] = self.ideal_yaw = Math3D.Vectoyaw(v);
                        Math3D.AngleVectors(self.s.angles, v_forward, v_right, null);
                        Math3D.VectorSet(v, d2, -16, 0);
                        Math3D.G_ProjectSource(self.s.origin, v, v_forward, v_right, left_target);
                        tr = GameBase.gi.Trace(self.s.origin, self.mins, self.maxs, left_target, self, Defines.MASK_PLAYERSOLID);
                        left = tr.fraction;
                        Math3D.VectorSet(v, d2, 16, 0);
                        Math3D.G_ProjectSource(self.s.origin, v, v_forward, v_right, right_target);
                        tr = GameBase.gi.Trace(self.s.origin, self.mins, self.maxs, right_target, self, Defines.MASK_PLAYERSOLID);
                        right = tr.fraction;
                        center = (d1 * center) / d2;
                        if (left >= center && left > right)
                        {
                            if (left < 1)
                            {
                                Math3D.VectorSet(v, d2 * left * 0.5F, -16F, 0F);
                                Math3D.G_ProjectSource(self.s.origin, v, v_forward, v_right, left_target);
                            }

                            Math3D.VectorCopy(self.monsterinfo.last_sighting, self.monsterinfo.saved_goal);
                            self.monsterinfo.aiflags |= Defines.AI_PURSUE_TEMP;
                            Math3D.VectorCopy(left_target, self.goalentity.s.origin);
                            Math3D.VectorCopy(left_target, self.monsterinfo.last_sighting);
                            Math3D.VectorSubtract(self.goalentity.s.origin, self.s.origin, v);
                            self.s.angles[Defines.YAW] = self.ideal_yaw = Math3D.Vectoyaw(v);
                        }
                        else if (right >= center && right > left)
                        {
                            if (right < 1)
                            {
                                Math3D.VectorSet(v, d2 * right * 0.5F, 16F, 0F);
                                Math3D.G_ProjectSource(self.s.origin, v, v_forward, v_right, right_target);
                            }

                            Math3D.VectorCopy(self.monsterinfo.last_sighting, self.monsterinfo.saved_goal);
                            self.monsterinfo.aiflags |= Defines.AI_PURSUE_TEMP;
                            Math3D.VectorCopy(right_target, self.goalentity.s.origin);
                            Math3D.VectorCopy(right_target, self.monsterinfo.last_sighting);
                            Math3D.VectorSubtract(self.goalentity.s.origin, self.s.origin, v);
                            self.s.angles[Defines.YAW] = self.ideal_yaw = Math3D.Vectoyaw(v);
                        }
                    }
                }

                M.M_MoveToGoal(self, dist);
                GameUtil.G_FreeEdict(tempgoal);
                if (self != null)
                    self.goalentity = save;
            }
        }

        static bool enemy_vis;
        static bool enemy_infront;
        public static int enemy_range;
        static float enemy_yaw;
    }
}
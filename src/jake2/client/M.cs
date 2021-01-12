using Jake2.Game;
using Jake2.Server;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public sealed class M
    {
        public static void M_CheckGround(edict_t ent)
        {
            float[] point = new float[]{0, 0, 0};
            trace_t trace;
            if ((ent.flags & (Defines.FL_SWIM | Defines.FL_FLY)) != 0)
                return;
            if (ent.velocity[2] > 100)
            {
                ent.groundentity = null;
                return;
            }

            point[0] = ent.s.origin[0];
            point[1] = ent.s.origin[1];
            point[2] = ent.s.origin[2] - 0.25F;
            trace = GameBase.gi.Trace(ent.s.origin, ent.mins, ent.maxs, point, ent, Defines.MASK_MONSTERSOLID);
            if (trace.plane.normal[2] < 0.7 && !trace.startsolid)
            {
                ent.groundentity = null;
                return;
            }

            if (!trace.startsolid && !trace.allsolid)
            {
                Math3D.VectorCopy(trace.endpos, ent.s.origin);
                ent.groundentity = trace.ent;
                ent.groundentity_linkcount = trace.ent.linkcount;
                ent.velocity[2] = 0;
            }
        }

        public static bool M_CheckBottom(edict_t ent)
        {
            float[] mins = new float[]{0, 0, 0};
            float[] maxs = new float[]{0, 0, 0};
            float[] start = new float[]{0, 0, 0};
            float[] stop = new float[]{0, 0, 0};
            trace_t trace;
            int x, y;
            float mid, bottom;
            Math3D.VectorAdd(ent.s.origin, ent.mins, mins);
            Math3D.VectorAdd(ent.s.origin, ent.maxs, maxs);
            start[2] = mins[2] - 1;
            for (x = 0; x <= 1; x++)
                for (y = 0; y <= 1; y++)
                {
                    start[0] = x != 0 ? maxs[0] : mins[0];
                    start[1] = y != 0 ? maxs[1] : mins[1];
                    if (GameBase.gi.pointcontents.Pointcontents(start) != Defines.CONTENTS_SOLID)
                    {
                        GameBase.c_no++;
                        start[2] = mins[2];
                        start[0] = stop[0] = (mins[0] + maxs[0]) * 0.5F;
                        start[1] = stop[1] = (mins[1] + maxs[1]) * 0.5F;
                        stop[2] = start[2] - 2 * GameBase.STEPSIZE;
                        trace = GameBase.gi.Trace(start, Globals.vec3_origin, Globals.vec3_origin, stop, ent, Defines.MASK_MONSTERSOLID);
                        if (trace.fraction == 1)
                            return false;
                        mid = bottom = trace.endpos[2];
                        for (x = 0; x <= 1; x++)
                            for (y = 0; y <= 1; y++)
                            {
                                start[0] = stop[0] = x != 0 ? maxs[0] : mins[0];
                                start[1] = stop[1] = y != 0 ? maxs[1] : mins[1];
                                trace = GameBase.gi.Trace(start, Globals.vec3_origin, Globals.vec3_origin, stop, ent, Defines.MASK_MONSTERSOLID);
                                if (trace.fraction != 1 && trace.endpos[2] > bottom)
                                    bottom = trace.endpos[2];
                                if (trace.fraction == 1 || mid - trace.endpos[2] > GameBase.STEPSIZE)
                                    return false;
                            }

                        GameBase.c_yes++;
                        return true;
                    }
                }

            GameBase.c_yes++;
            return true;
        }

        public static void M_ChangeYaw(edict_t ent)
        {
            float ideal;
            float current;
            float move;
            float speed;
            current = Math3D.Anglemod(ent.s.angles[Defines.YAW]);
            ideal = ent.ideal_yaw;
            if (current == ideal)
                return;
            move = ideal - current;
            speed = ent.yaw_speed;
            if (ideal > current)
            {
                if (move >= 180)
                    move = move - 360;
            }
            else
            {
                if (move <= -180)
                    move = move + 360;
            }

            if (move > 0)
            {
                if (move > speed)
                    move = speed;
            }
            else
            {
                if (move < -speed)
                    move = -speed;
            }

            ent.s.angles[Defines.YAW] = Math3D.Anglemod(current + move);
        }

        public static void M_MoveToGoal(edict_t ent, float dist)
        {
            edict_t goal = ent.goalentity;
            if (ent.groundentity == null && (ent.flags & (Defines.FL_FLY | Defines.FL_SWIM)) == 0)
                return;
            if (ent.enemy != null && SV.SV_CloseEnough(ent, ent.enemy, dist))
                return;
            if ((Lib.Rand() & 3) == 1 || !SV.SV_StepDirection(ent, ent.ideal_yaw, dist))
            {
                if (ent.inuse)
                    SV.SV_NewChaseDir(ent, goal, dist);
            }
        }

        public static bool M_walkmove(edict_t ent, float yaw, float dist)
        {
            float[] move = new float[]{0, 0, 0};
            if ((ent.groundentity == null) && (ent.flags & (Defines.FL_FLY | Defines.FL_SWIM)) == 0)
                return false;
            yaw = (float)(yaw * Math.PI * 2 / 360);
            move[0] = (float)Math.Cos(yaw) * dist;
            move[1] = (float)Math.Sin(yaw) * dist;
            move[2] = 0;
            return SV.SV_movestep(ent, move, true);
        }

        public static void M_CatagorizePosition(edict_t ent)
        {
            float[] point = new float[]{0, 0, 0};
            int cont;
            point[0] = ent.s.origin[0];
            point[1] = ent.s.origin[1];
            point[2] = ent.s.origin[2] + ent.mins[2] + 1;
            cont = GameBase.gi.pointcontents.Pointcontents(point);
            if (0 == (cont & Defines.MASK_WATER))
            {
                ent.waterlevel = 0;
                ent.watertype = 0;
                return;
            }

            ent.watertype = cont;
            ent.waterlevel = 1;
            point[2] += 26;
            cont = GameBase.gi.pointcontents.Pointcontents(point);
            if (0 == (cont & Defines.MASK_WATER))
                return;
            ent.waterlevel = 2;
            point[2] += 22;
            cont = GameBase.gi.pointcontents.Pointcontents(point);
            if (0 != (cont & Defines.MASK_WATER))
                ent.waterlevel = 3;
        }

        public static void M_WorldEffects(edict_t ent)
        {
            int dmg;
            if (ent.health > 0)
            {
                if (0 == (ent.flags & Defines.FL_SWIM))
                {
                    if (ent.waterlevel < 3)
                    {
                        ent.air_finished = GameBase.level.time + 12;
                    }
                    else if (ent.air_finished < GameBase.level.time)
                    {
                        if (ent.pain_debounce_time < GameBase.level.time)
                        {
                            dmg = (int)(2F + 2F * Math.Floor(GameBase.level.time - ent.air_finished));
                            if (dmg > 15)
                                dmg = 15;
                            GameCombat.T_Damage(ent, GameBase.g_edicts[0], GameBase.g_edicts[0], Globals.vec3_origin, ent.s.origin, Globals.vec3_origin, dmg, 0, Defines.DAMAGE_NO_ARMOR, Defines.MOD_WATER);
                            ent.pain_debounce_time = GameBase.level.time + 1;
                        }
                    }
                }
                else
                {
                    if (ent.waterlevel > 0)
                    {
                        ent.air_finished = GameBase.level.time + 9;
                    }
                    else if (ent.air_finished < GameBase.level.time)
                    {
                        if (ent.pain_debounce_time < GameBase.level.time)
                        {
                            dmg = (int)(2 + 2 * Math.Floor(GameBase.level.time - ent.air_finished));
                            if (dmg > 15)
                                dmg = 15;
                            GameCombat.T_Damage(ent, GameBase.g_edicts[0], GameBase.g_edicts[0], Globals.vec3_origin, ent.s.origin, Globals.vec3_origin, dmg, 0, Defines.DAMAGE_NO_ARMOR, Defines.MOD_WATER);
                            ent.pain_debounce_time = GameBase.level.time + 1;
                        }
                    }
                }
            }

            if (ent.waterlevel == 0)
            {
                if ((ent.flags & Defines.FL_INWATER) != 0)
                {
                    GameBase.gi.Sound(ent, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/watr_out.wav"), 1, Defines.ATTN_NORM, 0);
                    ent.flags &= ~Defines.FL_INWATER;
                }

                return;
            }

            if ((ent.watertype & Defines.CONTENTS_LAVA) != 0 && 0 == (ent.flags & Defines.FL_IMMUNE_LAVA))
            {
                if (ent.damage_debounce_time < GameBase.level.time)
                {
                    ent.damage_debounce_time = GameBase.level.time + 0.2F;
                    GameCombat.T_Damage(ent, GameBase.g_edicts[0], GameBase.g_edicts[0], Globals.vec3_origin, ent.s.origin, Globals.vec3_origin, 10 * ent.waterlevel, 0, 0, Defines.MOD_LAVA);
                }
            }

            if ((ent.watertype & Defines.CONTENTS_SLIME) != 0 && 0 == (ent.flags & Defines.FL_IMMUNE_SLIME))
            {
                if (ent.damage_debounce_time < GameBase.level.time)
                {
                    ent.damage_debounce_time = GameBase.level.time + 1;
                    GameCombat.T_Damage(ent, GameBase.g_edicts[0], GameBase.g_edicts[0], Globals.vec3_origin, ent.s.origin, Globals.vec3_origin, 4 * ent.waterlevel, 0, 0, Defines.MOD_SLIME);
                }
            }

            if (0 == (ent.flags & Defines.FL_INWATER))
            {
                if (0 == (ent.svflags & Defines.SVF_DEADMONSTER))
                {
                    if ((ent.watertype & Defines.CONTENTS_LAVA) != 0)
                        if (( float ) Globals.rnd.NextDouble() <= 0.5)
                            GameBase.gi.Sound(ent, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/lava1.wav"), 1, Defines.ATTN_NORM, 0);
                        else
                            GameBase.gi.Sound(ent, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/lava2.wav"), 1, Defines.ATTN_NORM, 0);
                    else if ((ent.watertype & Defines.CONTENTS_SLIME) != 0)
                        GameBase.gi.Sound(ent, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/watr_in.wav"), 1, Defines.ATTN_NORM, 0);
                    else if ((ent.watertype & Defines.CONTENTS_WATER) != 0)
                        GameBase.gi.Sound(ent, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/watr_in.wav"), 1, Defines.ATTN_NORM, 0);
                }

                ent.flags |= Defines.FL_INWATER;
                ent.damage_debounce_time = 0;
            }
        }

        public static EntThinkAdapter M_droptofloor = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "m_drop_to_floor";
            }

            public override bool Think(edict_t ent)
            {
                float[] end = new float[]{0, 0, 0};
                trace_t trace;
                ent.s.origin[2] += 1;
                Math3D.VectorCopy(ent.s.origin, end);
                end[2] -= 256;
                trace = GameBase.gi.Trace(ent.s.origin, ent.mins, ent.maxs, end, ent, Defines.MASK_MONSTERSOLID);
                if (trace.fraction == 1 || trace.allsolid)
                    return true;
                Math3D.VectorCopy(trace.endpos, ent.s.origin);
                GameBase.gi.Linkentity(ent);
                M.M_CheckGround(ent);
                M_CatagorizePosition(ent);
                return true;
            }
        }

        public static void M_SetEffects(edict_t ent)
        {
            ent.s.effects &= ~(Defines.EF_COLOR_SHELL | Defines.EF_POWERSCREEN);
            ent.s.renderfx &= ~(Defines.RF_SHELL_RED | Defines.RF_SHELL_GREEN | Defines.RF_SHELL_BLUE);
            if ((ent.monsterinfo.aiflags & Defines.AI_RESURRECTING) != 0)
            {
                ent.s.effects |= Defines.EF_COLOR_SHELL;
                ent.s.renderfx |= Defines.RF_SHELL_RED;
            }

            if (ent.health <= 0)
                return;
            if (ent.powerarmor_time > GameBase.level.time)
            {
                if (ent.monsterinfo.power_armor_type == Defines.POWER_ARMOR_SCREEN)
                {
                    ent.s.effects |= Defines.EF_POWERSCREEN;
                }
                else if (ent.monsterinfo.power_armor_type == Defines.POWER_ARMOR_SHIELD)
                {
                    ent.s.effects |= Defines.EF_COLOR_SHELL;
                    ent.s.renderfx |= Defines.RF_SHELL_GREEN;
                }
            }
        }

        public static void M_MoveFrame(edict_t self)
        {
            mmove_t move;
            int index;
            move = self.monsterinfo.currentmove;
            self.nextthink = GameBase.level.time + Defines.FRAMETIME;
            if ((self.monsterinfo.nextframe != 0) && (self.monsterinfo.nextframe >= move.firstframe) && (self.monsterinfo.nextframe <= move.lastframe))
            {
                self.s.frame = self.monsterinfo.nextframe;
                self.monsterinfo.nextframe = 0;
            }
            else
            {
                if (self.s.frame == move.lastframe)
                {
                    if (move.endfunc != null)
                    {
                        move.endfunc.Think(self);
                        move = self.monsterinfo.currentmove;
                        if ((self.svflags & Defines.SVF_DEADMONSTER) != 0)
                            return;
                    }
                }

                if (self.s.frame < move.firstframe || self.s.frame > move.lastframe)
                {
                    self.monsterinfo.aiflags &= ~Defines.AI_HOLD_FRAME;
                    self.s.frame = move.firstframe;
                }
                else
                {
                    if (0 == (self.monsterinfo.aiflags & Defines.AI_HOLD_FRAME))
                    {
                        self.s.frame++;
                        if (self.s.frame > move.lastframe)
                            self.s.frame = move.firstframe;
                    }
                }
            }

            index = self.s.frame - move.firstframe;
            if (move.frame[index].ai != null)
                if (0 == (self.monsterinfo.aiflags & Defines.AI_HOLD_FRAME))
                    move.frame[index].ai.Ai(self, move.frame[index].dist * self.monsterinfo.scale);
                else
                    move.frame[index].ai.Ai(self, 0);
            if (move.frame[index].think != null)
                move.frame[index].think.Think(self);
        }

        public static EntThinkAdapter M_FliesOff = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "m_fliesoff";
            }

            public override bool Think(edict_t self)
            {
                self.s.effects &= ~Defines.EF_FLIES;
                self.s.sound = 0;
                return true;
            }
        }

        public static EntThinkAdapter M_FliesOn = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "m_flies_on";
            }

            public override bool Think(edict_t self)
            {
                if (self.waterlevel != 0)
                    return true;
                self.s.effects |= Defines.EF_FLIES;
                self.s.sound = GameBase.gi.Soundindex("infantry/inflies1.wav");
                self.think = M_FliesOff;
                self.nextthink = GameBase.level.time + 60;
                return true;
            }
        }

        public static EntThinkAdapter M_FlyCheck = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "m_fly_check";
            }

            public override bool Think(edict_t self)
            {
                if (self.waterlevel != 0)
                    return true;
                if (( float ) Globals.rnd.NextDouble() > 0.5)
                    return true;
                self.think = M_FliesOn;
                self.nextthink = GameBase.level.time + 5 + 10 * ( float ) Globals.rnd.NextDouble();
                return true;
            }
        }
    }
}
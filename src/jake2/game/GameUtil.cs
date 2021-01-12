using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class GameUtil
    {
        public static void CheckClassname(edict_t ent)
        {
            if (ent.classname == null)
            {
                Com.Printf("edict with classname = null: " + ent.index);
            }
        }

        public static void G_UseTargets(edict_t ent, edict_t activator)
        {
            edict_t t;
            CheckClassname(ent);
            if (ent.delay != 0)
            {
                t = G_Spawn();
                t.classname = "DelayedUse";
                t.nextthink = GameBase.level.time + ent.delay;
                t.think = Think_Delay;
                t.activator = activator;
                if (activator == null)
                    GameBase.gi.Dprintf("Think_Delay with no activator\\n");
                t.message = ent.message;
                t.target = ent.target;
                t.killtarget = ent.killtarget;
                return;
            }

            if ((ent.message != null) && (activator.svflags & Defines.SVF_MONSTER) == 0)
            {
                GameBase.gi.Centerprintf(activator, "" + ent.message);
                if (ent.noise_index != 0)
                    GameBase.gi.Sound(activator, Defines.CHAN_AUTO, ent.noise_index, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(activator, Defines.CHAN_AUTO, GameBase.gi.Soundindex("misc/talk1.wav"), 1, Defines.ATTN_NORM, 0);
            }

            EdictIterator edit = null;
            if (ent.killtarget != null)
            {
                while ((edit = GameBase.G_Find(edit, GameBase.findByTarget, ent.killtarget)) != null)
                {
                    t = edit.o;
                    G_FreeEdict(t);
                    if (!ent.inuse)
                    {
                        GameBase.gi.Dprintf("entity was removed while using killtargets\\n");
                        return;
                    }
                }
            }

            if (ent.target != null)
            {
                edit = null;
                while ((edit = GameBase.G_Find(edit, GameBase.findByTarget, ent.target)) != null)
                {
                    t = edit.o;
                    if (Lib.Q_stricmp("func_areaportal", t.classname) == 0 && (Lib.Q_stricmp("func_door", ent.classname) == 0 || Lib.Q_stricmp("func_door_rotating", ent.classname) == 0))
                        continue;
                    if (t == ent)
                    {
                        GameBase.gi.Dprintf("WARNING: Entity used itself.\\n");
                    }
                    else
                    {
                        if (t.use != null)
                            t.use.Use(t, ent, activator);
                    }

                    if (!ent.inuse)
                    {
                        GameBase.gi.Dprintf("entity was removed while using targets\\n");
                        return;
                    }
                }
            }
        }

        public static void G_InitEdict(edict_t e, int i)
        {
            e.inuse = true;
            e.classname = "noclass";
            e.gravity = 1F;
            e.s = new entity_state_t(e);
            e.s.number = i;
            e.index = i;
        }

        public static edict_t G_Spawn()
        {
            int i;
            edict_t e = null;
            for (i = (int)GameBase.maxclients.value + 1; i < GameBase.num_edicts; i++)
            {
                e = GameBase.g_edicts[i];
                if (!e.inuse && (e.freetime < 2 || GameBase.level.time - e.freetime > 0.5))
                {
                    e = GameBase.g_edicts[i] = new edict_t(i);
                    G_InitEdict(e, i);
                    return e;
                }
            }

            if (i == GameBase.game.maxentities)
                GameBase.gi.Error("ED_Alloc: no free edicts");
            e = GameBase.g_edicts[i] = new edict_t(i);
            GameBase.num_edicts++;
            G_InitEdict(e, i);
            return e;
        }

        public static void G_FreeEdict(edict_t ed)
        {
            GameBase.gi.Unlinkentity(ed);
            if (ed.index <= (GameBase.maxclients.value + Defines.BODY_QUEUE_SIZE))
            {
                return;
            }

            GameBase.g_edicts[ed.index] = new edict_t(ed.index);
            ed.classname = "freed";
            ed.freetime = GameBase.level.time;
            ed.inuse = false;
        }

        public static void G_ClearEdict(edict_t ent)
        {
            int i = ent.index;
            GameBase.g_edicts[i] = new edict_t(i);
        }

        public static bool KillBox(edict_t ent)
        {
            trace_t tr;
            while (true)
            {
                tr = GameBase.gi.Trace(ent.s.origin, ent.mins, ent.maxs, ent.s.origin, null, Defines.MASK_PLAYERSOLID);
                if (tr.ent == null || tr.ent == GameBase.g_edicts[0])
                    break;
                GameCombat.T_Damage(tr.ent, ent, ent, Globals.vec3_origin, ent.s.origin, Globals.vec3_origin, 100000, 0, Defines.DAMAGE_NO_PROTECTION, Defines.MOD_TELEFRAG);
                if (tr.ent.solid != 0)
                    return false;
            }

            return true;
        }

        public static bool OnSameTeam(edict_t ent1, edict_t ent2)
        {
            if (0 == ((int)(GameBase.dmflags.value) & (Defines.DF_MODELTEAMS | Defines.DF_SKINTEAMS)))
                return false;
            if (ClientTeam(ent1).Equals(ClientTeam(ent2)))
                return true;
            return false;
        }

        static string ClientTeam(edict_t ent)
        {
            string value;
            if (ent.client == null)
                return "";
            value = Info.Info_ValueForKey(ent.client.pers.userinfo, "skin");
            int p = value.IndexOf("/");
            if (p == -1)
                return value;
            if (((int)(GameBase.dmflags.value) & Defines.DF_MODELTEAMS) != 0)
            {
                return value.Substring(0, p);
            }

            return value.Substring(p + 1, value.Length);
        }

        public static void ValidateSelectedItem(edict_t ent)
        {
            gclient_t cl;
            cl = ent.client;
            if (cl.pers.inventory[cl.pers.selected_item] != 0)
                return;
            GameItems.SelectNextItem(ent, -1);
        }

        public static int Range(edict_t self, edict_t other)
        {
            float[] v = new float[]{0, 0, 0};
            float len;
            Math3D.VectorSubtract(self.s.origin, other.s.origin, v);
            len = Math3D.VectorLength(v);
            if (len < Defines.MELEE_DISTANCE)
                return Defines.RANGE_MELEE;
            if (len < 500)
                return Defines.RANGE_NEAR;
            if (len < 1000)
                return Defines.RANGE_MID;
            return Defines.RANGE_FAR;
        }

        public static void AttackFinished(edict_t self, float time)
        {
            self.monsterinfo.attack_finished = GameBase.level.time + time;
        }

        public static bool Infront(edict_t self, edict_t other)
        {
            float[] vec = new float[]{0, 0, 0};
            float dot;
            float[] forward = new float[]{0, 0, 0};
            Math3D.AngleVectors(self.s.angles, forward, null, null);
            Math3D.VectorSubtract(other.s.origin, self.s.origin, vec);
            Math3D.VectorNormalize(vec);
            dot = Math3D.DotProduct(vec, forward);
            if (dot > 0.3)
                return true;
            return false;
        }

        public static bool Visible(edict_t self, edict_t other)
        {
            float[] spot1 = new float[]{0, 0, 0};
            float[] spot2 = new float[]{0, 0, 0};
            trace_t trace;
            Math3D.VectorCopy(self.s.origin, spot1);
            spot1[2] += self.viewheight;
            Math3D.VectorCopy(other.s.origin, spot2);
            spot2[2] += other.viewheight;
            trace = GameBase.gi.Trace(spot1, Globals.vec3_origin, Globals.vec3_origin, spot2, self, Defines.MASK_OPAQUE);
            if (trace.fraction == 1)
                return true;
            return false;
        }

        public static bool FindTarget(edict_t self)
        {
            edict_t client;
            bool heardit;
            int r;
            if ((self.monsterinfo.aiflags & Defines.AI_GOOD_GUY) != 0)
            {
                if (self.goalentity != null && self.goalentity.inuse && self.goalentity.classname != null)
                {
                    if (self.goalentity.classname.Equals("target_actor"))
                        return false;
                }

                return false;
            }

            if ((self.monsterinfo.aiflags & Defines.AI_COMBAT_POINT) != 0)
                return false;
            heardit = false;
            if ((GameBase.level.sight_entity_framenum >= (GameBase.level.framenum - 1)) && 0 == (self.spawnflags & 1))
            {
                client = GameBase.level.sight_entity;
                if (client.enemy == self.enemy)
                    return false;
            }
            else if (GameBase.level.sound_entity_framenum >= (GameBase.level.framenum - 1))
            {
                client = GameBase.level.sound_entity;
                heardit = true;
            }
            else if (null != (self.enemy) && (GameBase.level.sound2_entity_framenum >= (GameBase.level.framenum - 1)) && 0 != (self.spawnflags & 1))
            {
                client = GameBase.level.sound2_entity;
                heardit = true;
            }
            else
            {
                client = GameBase.level.sight_client;
                if (client == null)
                    return false;
            }

            if (!client.inuse)
                return false;
            if (client.client != null)
            {
                if ((client.flags & Defines.FL_NOTARGET) != 0)
                    return false;
            }
            else if ((client.svflags & Defines.SVF_MONSTER) != 0)
            {
                if (client.enemy == null)
                    return false;
                if ((client.enemy.flags & Defines.FL_NOTARGET) != 0)
                    return false;
            }
            else if (heardit)
            {
                if ((client.owner.flags & Defines.FL_NOTARGET) != 0)
                    return false;
            }
            else
                return false;
            if (!heardit)
            {
                r = Range(self, client);
                if (r == Defines.RANGE_FAR)
                    return false;
                if (client.light_level <= 5)
                    return false;
                if (!Visible(self, client))
                    return false;
                if (r == Defines.RANGE_NEAR)
                {
                    if (client.show_hostile < GameBase.level.time && !Infront(self, client))
                        return false;
                }
                else if (r == Defines.RANGE_MID)
                {
                    if (!Infront(self, client))
                        return false;
                }

                if (client == self.enemy)
                    return true;
                self.enemy = client;
                if (!self.enemy.classname.Equals("player_noise"))
                {
                    self.monsterinfo.aiflags &= ~Defines.AI_SOUND_TARGET;
                    if (self.enemy.client == null)
                    {
                        self.enemy = self.enemy.enemy;
                        if (self.enemy.client == null)
                        {
                            self.enemy = null;
                            return false;
                        }
                    }
                }
            }
            else
            {
                float[] temp = new float[]{0, 0, 0};
                if ((self.spawnflags & 1) != 0)
                {
                    if (!Visible(self, client))
                        return false;
                }
                else
                {
                    if (!GameBase.gi.InPHS(self.s.origin, client.s.origin))
                        return false;
                }

                Math3D.VectorSubtract(client.s.origin, self.s.origin, temp);
                if (Math3D.VectorLength(temp) > 1000)
                    return false;
                if (client.areanum != self.areanum)
                    if (!GameBase.gi.AreasConnected(self.areanum, client.areanum))
                        return false;
                self.ideal_yaw = Math3D.Vectoyaw(temp);
                M.M_ChangeYaw(self);
                self.monsterinfo.aiflags |= Defines.AI_SOUND_TARGET;
                if (client == self.enemy)
                    return true;
                self.enemy = client;
            }

            FoundTarget(self);
            if (0 == (self.monsterinfo.aiflags & Defines.AI_SOUND_TARGET) && (self.monsterinfo.sight != null))
                self.monsterinfo.sight.Interact(self, self.enemy);
            return true;
        }

        public static void FoundTarget(edict_t self)
        {
            if (self.enemy.client != null)
            {
                GameBase.level.sight_entity = self;
                GameBase.level.sight_entity_framenum = GameBase.level.framenum;
                GameBase.level.sight_entity.light_level = 128;
            }

            self.show_hostile = (int)GameBase.level.time + 1;
            Math3D.VectorCopy(self.enemy.s.origin, self.monsterinfo.last_sighting);
            self.monsterinfo.trail_time = GameBase.level.time;
            if (self.combattarget == null)
            {
                GameAI.HuntTarget(self);
                return;
            }

            self.goalentity = self.movetarget = GameBase.G_PickTarget(self.combattarget);
            if (self.movetarget == null)
            {
                self.goalentity = self.movetarget = self.enemy;
                GameAI.HuntTarget(self);
                GameBase.gi.Dprintf("" + self.classname + "at " + Lib.Vtos(self.s.origin) + ", combattarget " + self.combattarget + " not found\\n");
                return;
            }

            self.combattarget = null;
            self.monsterinfo.aiflags |= Defines.AI_COMBAT_POINT;
            self.movetarget.targetname = null;
            self.monsterinfo.pausetime = 0;
            self.monsterinfo.run.Think(self);
        }

        public static EntThinkAdapter Think_Delay = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "Think_Delay";
            }

            public override bool Think(edict_t ent)
            {
                G_UseTargets(ent, ent.activator);
                G_FreeEdict(ent);
                return true;
            }
        }

        public static EntThinkAdapter G_FreeEdictA = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "G_FreeEdictA";
            }

            public override bool Think(edict_t ent)
            {
                G_FreeEdict(ent);
                return false;
            }
        }

        public static EntThinkAdapter MegaHealth_think = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "MegaHealth_think";
            }

            public override bool Think(edict_t self)
            {
                if (self.owner.health > self.owner.max_health)
                {
                    self.nextthink = GameBase.level.time + 1;
                    self.owner.health -= 1;
                    return false;
                }

                if (!((self.spawnflags & Defines.DROPPED_ITEM) != 0) && (GameBase.deathmatch.value != 0))
                    GameItems.SetRespawn(self, 20);
                else
                    G_FreeEdict(self);
                return false;
            }
        }

        public static EntThinkAdapter M_CheckAttack = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "M_CheckAttack";
            }

            public override bool Think(edict_t self)
            {
                float[] spot1 = new float[]{0, 0, 0};
                float[] spot2 = new float[]{0, 0, 0};
                float chance;
                trace_t tr;
                if (self.enemy.health > 0)
                {
                    Math3D.VectorCopy(self.s.origin, spot1);
                    spot1[2] += self.viewheight;
                    Math3D.VectorCopy(self.enemy.s.origin, spot2);
                    spot2[2] += self.enemy.viewheight;
                    tr = GameBase.gi.Trace(spot1, null, null, spot2, self, Defines.CONTENTS_SOLID | Defines.CONTENTS_MONSTER | Defines.CONTENTS_SLIME | Defines.CONTENTS_LAVA | Defines.CONTENTS_WINDOW);
                    if (tr.ent != self.enemy)
                        return false;
                }

                if (GameAI.enemy_range == Defines.RANGE_MELEE)
                {
                    if (GameBase.skill.value == 0 && (Lib.Rand() & 3) != 0)
                        return false;
                    if (self.monsterinfo.melee != null)
                        self.monsterinfo.attack_state = Defines.AS_MELEE;
                    else
                        self.monsterinfo.attack_state = Defines.AS_MISSILE;
                    return true;
                }

                if (self.monsterinfo.attack == null)
                    return false;
                if (GameBase.level.time < self.monsterinfo.attack_finished)
                    return false;
                if (GameAI.enemy_range == Defines.RANGE_FAR)
                    return false;
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                {
                    chance = 0.4F;
                }
                else if (GameAI.enemy_range == Defines.RANGE_MELEE)
                {
                    chance = 0.2F;
                }
                else if (GameAI.enemy_range == Defines.RANGE_NEAR)
                {
                    chance = 0.1F;
                }
                else if (GameAI.enemy_range == Defines.RANGE_MID)
                {
                    chance = 0.02F;
                }
                else
                {
                    return false;
                }

                if (GameBase.skill.value == 0)
                    chance *= 0.5f;
                else if (GameBase.skill.value >= 2)
                    chance *= 2;
                if (Lib.Random() < chance)
                {
                    self.monsterinfo.attack_state = Defines.AS_MISSILE;
                    self.monsterinfo.attack_finished = GameBase.level.time + 2 * Lib.Random();
                    return true;
                }

                if ((self.flags & Defines.FL_FLY) != 0)
                {
                    if (Lib.Random() < 0.3F)
                        self.monsterinfo.attack_state = Defines.AS_SLIDING;
                    else
                        self.monsterinfo.attack_state = Defines.AS_STRAIGHT;
                }

                return false;
            }
        }

        public static EntUseAdapter monster_use = new AnonymousEntUseAdapter();
        private sealed class AnonymousEntUseAdapter : EntUseAdapter
		{
			
            public override string GetID()
            {
                return "monster_use";
            }

            public override void Use(edict_t self, edict_t other, edict_t activator)
            {
                if (self.enemy != null)
                    return;
                if (self.health <= 0)
                    return;
                if ((activator.flags & Defines.FL_NOTARGET) != 0)
                    return;
                if ((null == activator.client) && 0 == (activator.monsterinfo.aiflags & Defines.AI_GOOD_GUY))
                    return;
                self.enemy = activator;
                FoundTarget(self);
            }
        }
    }
}
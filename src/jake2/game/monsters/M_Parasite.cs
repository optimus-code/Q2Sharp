using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Parasite
    {
        public static readonly int FRAME_break01 = 0;
        public static readonly int FRAME_break02 = 1;
        public static readonly int FRAME_break03 = 2;
        public static readonly int FRAME_break04 = 3;
        public static readonly int FRAME_break05 = 4;
        public static readonly int FRAME_break06 = 5;
        public static readonly int FRAME_break07 = 6;
        public static readonly int FRAME_break08 = 7;
        public static readonly int FRAME_break09 = 8;
        public static readonly int FRAME_break10 = 9;
        public static readonly int FRAME_break11 = 10;
        public static readonly int FRAME_break12 = 11;
        public static readonly int FRAME_break13 = 12;
        public static readonly int FRAME_break14 = 13;
        public static readonly int FRAME_break15 = 14;
        public static readonly int FRAME_break16 = 15;
        public static readonly int FRAME_break17 = 16;
        public static readonly int FRAME_break18 = 17;
        public static readonly int FRAME_break19 = 18;
        public static readonly int FRAME_break20 = 19;
        public static readonly int FRAME_break21 = 20;
        public static readonly int FRAME_break22 = 21;
        public static readonly int FRAME_break23 = 22;
        public static readonly int FRAME_break24 = 23;
        public static readonly int FRAME_break25 = 24;
        public static readonly int FRAME_break26 = 25;
        public static readonly int FRAME_break27 = 26;
        public static readonly int FRAME_break28 = 27;
        public static readonly int FRAME_break29 = 28;
        public static readonly int FRAME_break30 = 29;
        public static readonly int FRAME_break31 = 30;
        public static readonly int FRAME_break32 = 31;
        public static readonly int FRAME_death101 = 32;
        public static readonly int FRAME_death102 = 33;
        public static readonly int FRAME_death103 = 34;
        public static readonly int FRAME_death104 = 35;
        public static readonly int FRAME_death105 = 36;
        public static readonly int FRAME_death106 = 37;
        public static readonly int FRAME_death107 = 38;
        public static readonly int FRAME_drain01 = 39;
        public static readonly int FRAME_drain02 = 40;
        public static readonly int FRAME_drain03 = 41;
        public static readonly int FRAME_drain04 = 42;
        public static readonly int FRAME_drain05 = 43;
        public static readonly int FRAME_drain06 = 44;
        public static readonly int FRAME_drain07 = 45;
        public static readonly int FRAME_drain08 = 46;
        public static readonly int FRAME_drain09 = 47;
        public static readonly int FRAME_drain10 = 48;
        public static readonly int FRAME_drain11 = 49;
        public static readonly int FRAME_drain12 = 50;
        public static readonly int FRAME_drain13 = 51;
        public static readonly int FRAME_drain14 = 52;
        public static readonly int FRAME_drain15 = 53;
        public static readonly int FRAME_drain16 = 54;
        public static readonly int FRAME_drain17 = 55;
        public static readonly int FRAME_drain18 = 56;
        public static readonly int FRAME_pain101 = 57;
        public static readonly int FRAME_pain102 = 58;
        public static readonly int FRAME_pain103 = 59;
        public static readonly int FRAME_pain104 = 60;
        public static readonly int FRAME_pain105 = 61;
        public static readonly int FRAME_pain106 = 62;
        public static readonly int FRAME_pain107 = 63;
        public static readonly int FRAME_pain108 = 64;
        public static readonly int FRAME_pain109 = 65;
        public static readonly int FRAME_pain110 = 66;
        public static readonly int FRAME_pain111 = 67;
        public static readonly int FRAME_run01 = 68;
        public static readonly int FRAME_run02 = 69;
        public static readonly int FRAME_run03 = 70;
        public static readonly int FRAME_run04 = 71;
        public static readonly int FRAME_run05 = 72;
        public static readonly int FRAME_run06 = 73;
        public static readonly int FRAME_run07 = 74;
        public static readonly int FRAME_run08 = 75;
        public static readonly int FRAME_run09 = 76;
        public static readonly int FRAME_run10 = 77;
        public static readonly int FRAME_run11 = 78;
        public static readonly int FRAME_run12 = 79;
        public static readonly int FRAME_run13 = 80;
        public static readonly int FRAME_run14 = 81;
        public static readonly int FRAME_run15 = 82;
        public static readonly int FRAME_stand01 = 83;
        public static readonly int FRAME_stand02 = 84;
        public static readonly int FRAME_stand03 = 85;
        public static readonly int FRAME_stand04 = 86;
        public static readonly int FRAME_stand05 = 87;
        public static readonly int FRAME_stand06 = 88;
        public static readonly int FRAME_stand07 = 89;
        public static readonly int FRAME_stand08 = 90;
        public static readonly int FRAME_stand09 = 91;
        public static readonly int FRAME_stand10 = 92;
        public static readonly int FRAME_stand11 = 93;
        public static readonly int FRAME_stand12 = 94;
        public static readonly int FRAME_stand13 = 95;
        public static readonly int FRAME_stand14 = 96;
        public static readonly int FRAME_stand15 = 97;
        public static readonly int FRAME_stand16 = 98;
        public static readonly int FRAME_stand17 = 99;
        public static readonly int FRAME_stand18 = 100;
        public static readonly int FRAME_stand19 = 101;
        public static readonly int FRAME_stand20 = 102;
        public static readonly int FRAME_stand21 = 103;
        public static readonly int FRAME_stand22 = 104;
        public static readonly int FRAME_stand23 = 105;
        public static readonly int FRAME_stand24 = 106;
        public static readonly int FRAME_stand25 = 107;
        public static readonly int FRAME_stand26 = 108;
        public static readonly int FRAME_stand27 = 109;
        public static readonly int FRAME_stand28 = 110;
        public static readonly int FRAME_stand29 = 111;
        public static readonly int FRAME_stand30 = 112;
        public static readonly int FRAME_stand31 = 113;
        public static readonly int FRAME_stand32 = 114;
        public static readonly int FRAME_stand33 = 115;
        public static readonly int FRAME_stand34 = 116;
        public static readonly int FRAME_stand35 = 117;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_die;
        static int sound_launch;
        static int sound_impact;
        static int sound_suck;
        static int sound_reelin;
        static int sound_sight;
        static int sound_tap;
        static int sound_scratch;
        static int sound_search;
        static EntThinkAdapter parasite_launch = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_launch";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_launch, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter parasite_reel_in = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_reel_in";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_reelin, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntInteractAdapter parasite_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "parasite_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter parasite_tap = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_tap";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_tap, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static EntThinkAdapter parasite_scratch = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_scratch";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_scratch, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static EntThinkAdapter parasite_search = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_search";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_search, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static EntThinkAdapter parasite_start_walk = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_start_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = parasite_move_start_walk;
                return true;
            }
        }

        static EntThinkAdapter parasite_walk = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = parasite_move_walk;
                return true;
            }
        }

        static EntThinkAdapter parasite_stand = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = parasite_move_stand;
                return true;
            }
        }

        static EntThinkAdapter parasite_end_fidget = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_end_fidget";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = parasite_move_end_fidget;
                return true;
            }
        }

        static EntThinkAdapter parasite_do_fidget = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_do_fidget";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = parasite_move_fidget;
                return true;
            }
        }

        static EntThinkAdapter parasite_refidget = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_refidget";
            }

            public override bool Think(edict_t self)
            {
                if (Lib.Random() <= 0.8)
                    self.monsterinfo.currentmove = parasite_move_fidget;
                else
                    self.monsterinfo.currentmove = parasite_move_end_fidget;
                return true;
            }
        }

        static EntThinkAdapter parasite_idle = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_idle";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = parasite_move_start_fidget;
                return true;
            }
        }

        static EntThinkAdapter parasite_start_run = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_start_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = parasite_move_stand;
                else
                    self.monsterinfo.currentmove = parasite_move_start_run;
                return true;
            }
        }

        static EntThinkAdapter parasite_run = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = parasite_move_stand;
                else
                    self.monsterinfo.currentmove = parasite_move_run;
                return true;
            }
        }

        static mframe_t[] parasite_frames_start_fidget = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t parasite_move_start_fidget = new mmove_t(FRAME_stand18, FRAME_stand21, parasite_frames_start_fidget, parasite_do_fidget);
        static mframe_t[] parasite_frames_fidget = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, parasite_scratch), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, parasite_scratch), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t parasite_move_fidget = new mmove_t(FRAME_stand22, FRAME_stand27, parasite_frames_fidget, parasite_refidget);
        static mframe_t[] parasite_frames_end_fidget = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, parasite_scratch), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t parasite_move_end_fidget = new mmove_t(FRAME_stand28, FRAME_stand35, parasite_frames_end_fidget, parasite_stand);
        static mframe_t[] parasite_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, parasite_tap), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, parasite_tap), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, parasite_tap), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, parasite_tap), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, parasite_tap), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, parasite_tap)};
        static mmove_t parasite_move_stand = new mmove_t(FRAME_stand01, FRAME_stand17, parasite_frames_stand, parasite_stand);
        static mframe_t[] parasite_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 30, null), new mframe_t(GameAI.ai_run, 30, null), new mframe_t(GameAI.ai_run, 22, null), new mframe_t(GameAI.ai_run, 19, null), new mframe_t(GameAI.ai_run, 24, null), new mframe_t(GameAI.ai_run, 28, null), new mframe_t(GameAI.ai_run, 25, null)};
        static mmove_t parasite_move_run = new mmove_t(FRAME_run03, FRAME_run09, parasite_frames_run, null);
        static mframe_t[] parasite_frames_start_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 30, null)};
        static mmove_t parasite_move_start_run = new mmove_t(FRAME_run01, FRAME_run02, parasite_frames_start_run, parasite_run);
        static mframe_t[] parasite_frames_stop_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 20, null), new mframe_t(GameAI.ai_run, 20, null), new mframe_t(GameAI.ai_run, 12, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 0, null)};
        static mmove_t parasite_move_stop_run = new mmove_t(FRAME_run10, FRAME_run15, parasite_frames_stop_run, null);
        static mframe_t[] parasite_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 30, null), new mframe_t(GameAI.ai_walk, 30, null), new mframe_t(GameAI.ai_walk, 22, null), new mframe_t(GameAI.ai_walk, 19, null), new mframe_t(GameAI.ai_walk, 24, null), new mframe_t(GameAI.ai_walk, 28, null), new mframe_t(GameAI.ai_walk, 25, null)};
        static mmove_t parasite_move_walk = new mmove_t(FRAME_run03, FRAME_run09, parasite_frames_walk, parasite_walk);
        static mframe_t[] parasite_frames_start_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 30, parasite_walk)};
        static mmove_t parasite_move_start_walk = new mmove_t(FRAME_run01, FRAME_run02, parasite_frames_start_walk, null);
        static mframe_t[] parasite_frames_stop_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 20, null), new mframe_t(GameAI.ai_walk, 20, null), new mframe_t(GameAI.ai_walk, 12, null), new mframe_t(GameAI.ai_walk, 10, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null)};
        static mmove_t parasite_move_stop_walk = new mmove_t(FRAME_run10, FRAME_run15, parasite_frames_stop_walk, null);
        static mframe_t[] parasite_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 6, null), new mframe_t(GameAI.ai_move, 16, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, -7, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t parasite_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain111, parasite_frames_pain1, parasite_start_run);
        static EntPainAdapter parasite_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "parasite_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                if (self.health < (self.max_health / 2))
                    self.s.skinnum = 1;
                if (GameBase.level.time < self.pain_debounce_time)
                    return;
                self.pain_debounce_time = GameBase.level.time + 3;
                if (GameBase.skill.value == 3)
                    return;
                if (Lib.Random() < 0.5)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                self.monsterinfo.currentmove = parasite_move_pain1;
            }
        }

        static EntThinkAdapter parasite_drain_attack = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_drain_attack";
            }

            public override bool Think(edict_t self)
            {
                float[] offset = new float[]{0, 0, 0}, start = new float[]{0, 0, 0}, f = new float[]{0, 0, 0}, r = new float[]{0, 0, 0}, end = new float[]{0, 0, 0}, dir = new float[]{0, 0, 0};
                trace_t tr;
                int damage;
                Math3D.AngleVectors(self.s.angles, f, r, null);
                Math3D.VectorSet(offset, 24, 0, 6);
                Math3D.G_ProjectSource(self.s.origin, offset, f, r, start);
                Math3D.VectorCopy(self.enemy.s.origin, end);
                if (!Parasite_drain_attack_ok(start, end))
                {
                    end[2] = self.enemy.s.origin[2] + self.enemy.maxs[2] - 8;
                    if (!Parasite_drain_attack_ok(start, end))
                    {
                        end[2] = self.enemy.s.origin[2] + self.enemy.mins[2] + 8;
                        if (!Parasite_drain_attack_ok(start, end))
                            return true;
                    }
                }

                Math3D.VectorCopy(self.enemy.s.origin, end);
                tr = GameBase.gi.Trace(start, null, null, end, self, Defines.MASK_SHOT);
                if (tr.ent != self.enemy)
                    return true;
                if (self.s.frame == FRAME_drain03)
                {
                    damage = 5;
                    GameBase.gi.Sound(self.enemy, Defines.CHAN_AUTO, sound_impact, 1, Defines.ATTN_NORM, 0);
                }
                else
                {
                    if (self.s.frame == FRAME_drain04)
                        GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_suck, 1, Defines.ATTN_NORM, 0);
                    damage = 2;
                }

                GameBase.gi.WriteByte(Defines.svc_temp_entity);
                GameBase.gi.WriteByte(Defines.TE_PARASITE_ATTACK);
                GameBase.gi.WriteShort(self.index);
                GameBase.gi.WritePosition(start);
                GameBase.gi.WritePosition(end);
                GameBase.gi.Multicast(self.s.origin, Defines.MULTICAST_PVS);
                Math3D.VectorSubtract(start, end, dir);
                GameCombat.T_Damage(self.enemy, self, self, dir, self.enemy.s.origin, Globals.vec3_origin, damage, 0, Defines.DAMAGE_NO_KNOCKBACK, Defines.MOD_UNKNOWN);
                return true;
            }
        }

        static mframe_t[] parasite_frames_drain = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, parasite_launch), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 15, parasite_drain_attack), new mframe_t(GameAI.ai_charge, 0, parasite_drain_attack), new mframe_t(GameAI.ai_charge, 0, parasite_drain_attack), new mframe_t(GameAI.ai_charge, 0, parasite_drain_attack), new mframe_t(GameAI.ai_charge, 0, parasite_drain_attack), new mframe_t(GameAI.ai_charge, -2, parasite_drain_attack), new mframe_t(GameAI.ai_charge, -2, parasite_drain_attack), new mframe_t(GameAI.ai_charge, -3, parasite_drain_attack), new mframe_t(GameAI.ai_charge, -2, parasite_drain_attack), new mframe_t(GameAI.ai_charge, 0, parasite_drain_attack), new mframe_t(GameAI.ai_charge, -1, parasite_drain_attack), new mframe_t(GameAI.ai_charge, 0, parasite_reel_in), new mframe_t(GameAI.ai_charge, -2, null), new mframe_t(GameAI.ai_charge, -2, null), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t parasite_move_drain = new mmove_t(FRAME_drain01, FRAME_drain18, parasite_frames_drain, parasite_start_run);
        static mframe_t[] parasite_frames_break = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 3, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -18, null), new mframe_t(GameAI.ai_charge, 3, null), new mframe_t(GameAI.ai_charge, 9, null), new mframe_t(GameAI.ai_charge, 6, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -18, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 8, null), new mframe_t(GameAI.ai_charge, 9, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -18, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, 11, null), new mframe_t(GameAI.ai_charge, -2, null), new mframe_t(GameAI.ai_charge, -5, null), new mframe_t(GameAI.ai_charge, 1, null)};
        static mmove_t parasite_move_break = new mmove_t(FRAME_break01, FRAME_break32, parasite_frames_break, parasite_start_run);
        static EntThinkAdapter parasite_attack = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_attack";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = parasite_move_drain;
                return true;
            }
        }

        static EntThinkAdapter parasite_dead = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "parasite_dead";
            }

            public override bool Think(edict_t self)
            {
                Math3D.VectorSet(self.mins, -16, -16, -24);
                Math3D.VectorSet(self.maxs, 16, 16, -8);
                self.movetype = Defines.MOVETYPE_TOSS;
                self.svflags |= Defines.SVF_DEADMONSTER;
                self.nextthink = 0;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        static mframe_t[] parasite_frames_death = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t parasite_move_death = new mmove_t(FRAME_death101, FRAME_death107, parasite_frames_death, parasite_dead);
        static EntDieAdapter parasite_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "parasite_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                int n;
                if (self.health <= self.gib_health)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, GameBase.gi.Soundindex("misc/udeath.wav"), 1, Defines.ATTN_NORM, 0);
                    for (n = 0; n < 2; n++)
                        GameMisc.ThrowGib(self, "models/objects/gibs/bone/tris.md2", damage, Defines.GIB_ORGANIC);
                    for (n = 0; n < 4; n++)
                        GameMisc.ThrowGib(self, "models/objects/gibs/sm_meat/tris.md2", damage, Defines.GIB_ORGANIC);
                    GameMisc.ThrowHead(self, "models/objects/gibs/head2/tris.md2", damage, Defines.GIB_ORGANIC);
                    self.deadflag = Defines.DEAD_DEAD;
                    return;
                }

                if (self.deadflag == Defines.DEAD_DEAD)
                    return;
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_die, 1, Defines.ATTN_NORM, 0);
                self.deadflag = Defines.DEAD_DEAD;
                self.takedamage = Defines.DAMAGE_YES;
                self.monsterinfo.currentmove = parasite_move_death;
            }
        }

        public static EntThinkAdapter SP_monster_parasite = new AnonymousEntThinkAdapter17();
        private sealed class AnonymousEntThinkAdapter17 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "SP_monster_parasite";
            }

            public override bool Think(edict_t self)
            {
                if (GameBase.deathmatch.value != 0)
                {
                    GameUtil.G_FreeEdict(self);
                    return true;
                }

                sound_pain1 = GameBase.gi.Soundindex("parasite/parpain1.wav");
                sound_pain2 = GameBase.gi.Soundindex("parasite/parpain2.wav");
                sound_die = GameBase.gi.Soundindex("parasite/pardeth1.wav");
                sound_launch = GameBase.gi.Soundindex("parasite/paratck1.wav");
                sound_impact = GameBase.gi.Soundindex("parasite/paratck2.wav");
                sound_suck = GameBase.gi.Soundindex("parasite/paratck3.wav");
                sound_reelin = GameBase.gi.Soundindex("parasite/paratck4.wav");
                sound_sight = GameBase.gi.Soundindex("parasite/parsght1.wav");
                sound_tap = GameBase.gi.Soundindex("parasite/paridle1.wav");
                sound_scratch = GameBase.gi.Soundindex("parasite/paridle2.wav");
                sound_search = GameBase.gi.Soundindex("parasite/parsrch1.wav");
                self.s.modelindex = GameBase.gi.Modelindex("models/monsters/parasite/tris.md2");
                Math3D.VectorSet(self.mins, -16, -16, -24);
                Math3D.VectorSet(self.maxs, 16, 16, 24);
                self.movetype = Defines.MOVETYPE_STEP;
                self.solid = Defines.SOLID_BBOX;
                self.health = 175;
                self.gib_health = -50;
                self.mass = 250;
                self.pain = parasite_pain;
                self.die = parasite_die;
                self.monsterinfo.stand = parasite_stand;
                self.monsterinfo.walk = parasite_start_walk;
                self.monsterinfo.run = parasite_start_run;
                self.monsterinfo.attack = parasite_attack;
                self.monsterinfo.sight = parasite_sight;
                self.monsterinfo.idle = parasite_idle;
                GameBase.gi.Linkentity(self);
                self.monsterinfo.currentmove = parasite_move_stand;
                self.monsterinfo.scale = MODEL_SCALE;
                GameAI.walkmonster_start.Think(self);
                return true;
            }
        }

        static bool Parasite_drain_attack_ok(float[] start, float[] end)
        {
            float[] dir = new float[]{0, 0, 0}, angles = new float[]{0, 0, 0};
            Math3D.VectorSubtract(start, end, dir);
            if (Math3D.VectorLength(dir) > 256)
                return false;
            Math3D.Vectoangles(dir, angles);
            if (angles[0] < -180)
                angles[0] += 360;
            if (Math.Abs(angles[0]) > 30)
                return false;
            return true;
        }
    }
}
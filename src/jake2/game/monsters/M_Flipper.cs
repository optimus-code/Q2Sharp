using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game.Monsters
{
    public class M_Flipper
    {
        public static readonly int FRAME_flpbit01 = 0;
        public static readonly int FRAME_flpbit02 = 1;
        public static readonly int FRAME_flpbit03 = 2;
        public static readonly int FRAME_flpbit04 = 3;
        public static readonly int FRAME_flpbit05 = 4;
        public static readonly int FRAME_flpbit06 = 5;
        public static readonly int FRAME_flpbit07 = 6;
        public static readonly int FRAME_flpbit08 = 7;
        public static readonly int FRAME_flpbit09 = 8;
        public static readonly int FRAME_flpbit10 = 9;
        public static readonly int FRAME_flpbit11 = 10;
        public static readonly int FRAME_flpbit12 = 11;
        public static readonly int FRAME_flpbit13 = 12;
        public static readonly int FRAME_flpbit14 = 13;
        public static readonly int FRAME_flpbit15 = 14;
        public static readonly int FRAME_flpbit16 = 15;
        public static readonly int FRAME_flpbit17 = 16;
        public static readonly int FRAME_flpbit18 = 17;
        public static readonly int FRAME_flpbit19 = 18;
        public static readonly int FRAME_flpbit20 = 19;
        public static readonly int FRAME_flptal01 = 20;
        public static readonly int FRAME_flptal02 = 21;
        public static readonly int FRAME_flptal03 = 22;
        public static readonly int FRAME_flptal04 = 23;
        public static readonly int FRAME_flptal05 = 24;
        public static readonly int FRAME_flptal06 = 25;
        public static readonly int FRAME_flptal07 = 26;
        public static readonly int FRAME_flptal08 = 27;
        public static readonly int FRAME_flptal09 = 28;
        public static readonly int FRAME_flptal10 = 29;
        public static readonly int FRAME_flptal11 = 30;
        public static readonly int FRAME_flptal12 = 31;
        public static readonly int FRAME_flptal13 = 32;
        public static readonly int FRAME_flptal14 = 33;
        public static readonly int FRAME_flptal15 = 34;
        public static readonly int FRAME_flptal16 = 35;
        public static readonly int FRAME_flptal17 = 36;
        public static readonly int FRAME_flptal18 = 37;
        public static readonly int FRAME_flptal19 = 38;
        public static readonly int FRAME_flptal20 = 39;
        public static readonly int FRAME_flptal21 = 40;
        public static readonly int FRAME_flphor01 = 41;
        public static readonly int FRAME_flphor02 = 42;
        public static readonly int FRAME_flphor03 = 43;
        public static readonly int FRAME_flphor04 = 44;
        public static readonly int FRAME_flphor05 = 45;
        public static readonly int FRAME_flphor06 = 46;
        public static readonly int FRAME_flphor07 = 47;
        public static readonly int FRAME_flphor08 = 48;
        public static readonly int FRAME_flphor09 = 49;
        public static readonly int FRAME_flphor10 = 50;
        public static readonly int FRAME_flphor11 = 51;
        public static readonly int FRAME_flphor12 = 52;
        public static readonly int FRAME_flphor13 = 53;
        public static readonly int FRAME_flphor14 = 54;
        public static readonly int FRAME_flphor15 = 55;
        public static readonly int FRAME_flphor16 = 56;
        public static readonly int FRAME_flphor17 = 57;
        public static readonly int FRAME_flphor18 = 58;
        public static readonly int FRAME_flphor19 = 59;
        public static readonly int FRAME_flphor20 = 60;
        public static readonly int FRAME_flphor21 = 61;
        public static readonly int FRAME_flphor22 = 62;
        public static readonly int FRAME_flphor23 = 63;
        public static readonly int FRAME_flphor24 = 64;
        public static readonly int FRAME_flpver01 = 65;
        public static readonly int FRAME_flpver02 = 66;
        public static readonly int FRAME_flpver03 = 67;
        public static readonly int FRAME_flpver04 = 68;
        public static readonly int FRAME_flpver05 = 69;
        public static readonly int FRAME_flpver06 = 70;
        public static readonly int FRAME_flpver07 = 71;
        public static readonly int FRAME_flpver08 = 72;
        public static readonly int FRAME_flpver09 = 73;
        public static readonly int FRAME_flpver10 = 74;
        public static readonly int FRAME_flpver11 = 75;
        public static readonly int FRAME_flpver12 = 76;
        public static readonly int FRAME_flpver13 = 77;
        public static readonly int FRAME_flpver14 = 78;
        public static readonly int FRAME_flpver15 = 79;
        public static readonly int FRAME_flpver16 = 80;
        public static readonly int FRAME_flpver17 = 81;
        public static readonly int FRAME_flpver18 = 82;
        public static readonly int FRAME_flpver19 = 83;
        public static readonly int FRAME_flpver20 = 84;
        public static readonly int FRAME_flpver21 = 85;
        public static readonly int FRAME_flpver22 = 86;
        public static readonly int FRAME_flpver23 = 87;
        public static readonly int FRAME_flpver24 = 88;
        public static readonly int FRAME_flpver25 = 89;
        public static readonly int FRAME_flpver26 = 90;
        public static readonly int FRAME_flpver27 = 91;
        public static readonly int FRAME_flpver28 = 92;
        public static readonly int FRAME_flpver29 = 93;
        public static readonly int FRAME_flppn101 = 94;
        public static readonly int FRAME_flppn102 = 95;
        public static readonly int FRAME_flppn103 = 96;
        public static readonly int FRAME_flppn104 = 97;
        public static readonly int FRAME_flppn105 = 98;
        public static readonly int FRAME_flppn201 = 99;
        public static readonly int FRAME_flppn202 = 100;
        public static readonly int FRAME_flppn203 = 101;
        public static readonly int FRAME_flppn204 = 102;
        public static readonly int FRAME_flppn205 = 103;
        public static readonly int FRAME_flpdth01 = 104;
        public static readonly int FRAME_flpdth02 = 105;
        public static readonly int FRAME_flpdth03 = 106;
        public static readonly int FRAME_flpdth04 = 107;
        public static readonly int FRAME_flpdth05 = 108;
        public static readonly int FRAME_flpdth06 = 109;
        public static readonly int FRAME_flpdth07 = 110;
        public static readonly int FRAME_flpdth08 = 111;
        public static readonly int FRAME_flpdth09 = 112;
        public static readonly int FRAME_flpdth10 = 113;
        public static readonly int FRAME_flpdth11 = 114;
        public static readonly int FRAME_flpdth12 = 115;
        public static readonly int FRAME_flpdth13 = 116;
        public static readonly int FRAME_flpdth14 = 117;
        public static readonly int FRAME_flpdth15 = 118;
        public static readonly int FRAME_flpdth16 = 119;
        public static readonly int FRAME_flpdth17 = 120;
        public static readonly int FRAME_flpdth18 = 121;
        public static readonly int FRAME_flpdth19 = 122;
        public static readonly int FRAME_flpdth20 = 123;
        public static readonly int FRAME_flpdth21 = 124;
        public static readonly int FRAME_flpdth22 = 125;
        public static readonly int FRAME_flpdth23 = 126;
        public static readonly int FRAME_flpdth24 = 127;
        public static readonly int FRAME_flpdth25 = 128;
        public static readonly int FRAME_flpdth26 = 129;
        public static readonly int FRAME_flpdth27 = 130;
        public static readonly int FRAME_flpdth28 = 131;
        public static readonly int FRAME_flpdth29 = 132;
        public static readonly int FRAME_flpdth30 = 133;
        public static readonly int FRAME_flpdth31 = 134;
        public static readonly int FRAME_flpdth32 = 135;
        public static readonly int FRAME_flpdth33 = 136;
        public static readonly int FRAME_flpdth34 = 137;
        public static readonly int FRAME_flpdth35 = 138;
        public static readonly int FRAME_flpdth36 = 139;
        public static readonly int FRAME_flpdth37 = 140;
        public static readonly int FRAME_flpdth38 = 141;
        public static readonly int FRAME_flpdth39 = 142;
        public static readonly int FRAME_flpdth40 = 143;
        public static readonly int FRAME_flpdth41 = 144;
        public static readonly int FRAME_flpdth42 = 145;
        public static readonly int FRAME_flpdth43 = 146;
        public static readonly int FRAME_flpdth44 = 147;
        public static readonly int FRAME_flpdth45 = 148;
        public static readonly int FRAME_flpdth46 = 149;
        public static readonly int FRAME_flpdth47 = 150;
        public static readonly int FRAME_flpdth48 = 151;
        public static readonly int FRAME_flpdth49 = 152;
        public static readonly int FRAME_flpdth50 = 153;
        public static readonly int FRAME_flpdth51 = 154;
        public static readonly int FRAME_flpdth52 = 155;
        public static readonly int FRAME_flpdth53 = 156;
        public static readonly int FRAME_flpdth54 = 157;
        public static readonly int FRAME_flpdth55 = 158;
        public static readonly int FRAME_flpdth56 = 159;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_chomp;
        static int sound_attack;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_death;
        static int sound_idle;
        static int sound_search;
        static int sound_sight;
        static mframe_t[] flipper_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t flipper_move_stand = new mmove_t(FRAME_flphor01, FRAME_flphor01, flipper_frames_stand, null);
        static EntThinkAdapter flipper_stand = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flipper_move_stand;
                return true;
            }
        }

        public static readonly int FLIPPER_RUN_SPEED = 24;
        static mframe_t[] flipper_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null), new mframe_t(GameAI.ai_run, FLIPPER_RUN_SPEED, null)};
        static mmove_t flipper_move_run_loop = new mmove_t(FRAME_flpver06, FRAME_flpver29, flipper_frames_run, null);
        static EntThinkAdapter flipper_run_loop = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_run_loop";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flipper_move_run_loop;
                return true;
            }
        }

        static mframe_t[] flipper_frames_run_start = new mframe_t[]{new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, null)};
        static mmove_t flipper_move_run_start = new mmove_t(FRAME_flpver01, FRAME_flpver06, flipper_frames_run_start, flipper_run_loop);
        static EntThinkAdapter flipper_run = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_run";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flipper_move_run_start;
                return true;
            }
        }

        static mframe_t[] flipper_frames_walk = new mframe_t[] { new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null)};
        static mmove_t flipper_move_walk = new mmove_t(FRAME_flphor01, FRAME_flphor24, flipper_frames_walk, null);
        static EntThinkAdapter flipper_walk = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flipper_move_walk;
                return true;
            }
        }

        static mframe_t[] flipper_frames_start_run = new mframe_t[] { new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 8, flipper_run)};
        static mmove_t flipper_move_start_run = new mmove_t(FRAME_flphor01, FRAME_flphor05, flipper_frames_start_run, null);
        static EntThinkAdapter flipper_start_run = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_start_run";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flipper_move_start_run;
                return true;
            }
        }

        static mframe_t[] flipper_frames_pain2 = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flipper_move_pain2 = new mmove_t(FRAME_flppn101, FRAME_flppn105, flipper_frames_pain2, flipper_run);
        static mframe_t[] flipper_frames_pain1 = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flipper_move_pain1 = new mmove_t(FRAME_flppn201, FRAME_flppn205, flipper_frames_pain1, flipper_run);
        static EntThinkAdapter flipper_bite = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_bite";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, 0, 0);
                GameWeapon.Fire_hit(self, aim, 5, 0);
                return true;
            }
        }

        static EntThinkAdapter flipper_preattack = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_preattack";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_chomp, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] flipper_frames_attack = new mframe_t[] { new mframe_t(GameAI.ai_charge, 0, flipper_preattack), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, flipper_bite), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, flipper_bite), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t flipper_move_attack = new mmove_t(FRAME_flpbit01, FRAME_flpbit20, flipper_frames_attack, flipper_run);
        static EntThinkAdapter flipper_melee = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_melee";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flipper_move_attack;
                return true;
            }
        }

        static EntPainAdapter flipper_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "flipper_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                int n;
                if (self.health < (self.max_health / 2))
                    self.s.skinnum = 1;
                if (GameBase.level.time < self.pain_debounce_time)
                    return;
                self.pain_debounce_time = GameBase.level.time + 3;
                if (GameBase.skill.value == 3)
                    return;
                n = (Lib.Rand() + 1) % 2;
                if (n == 0)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = flipper_move_pain1;
                }
                else
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = flipper_move_pain2;
                }

                return;
            }
        }

        static EntThinkAdapter flipper_dead = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "flipper_dead";
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

        static mframe_t[] flipper_frames_death = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flipper_move_death = new mmove_t(FRAME_flpdth01, FRAME_flpdth56, flipper_frames_death, flipper_dead);
        static EntInteractAdapter flipper_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "flipper_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntDieAdapter flipper_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "flipper_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                int n;
                if (self.health <= self.gib_health)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, GameBase.gi.Soundindex("misc/udeath.wav"), 1, Defines.ATTN_NORM, 0);
                    for (n = 0; n < 2; n++)
                        GameMisc.ThrowGib(self, "models/objects/gibs/bone/tris.md2", damage, Defines.GIB_ORGANIC);
                    for (n = 0; n < 2; n++)
                        GameMisc.ThrowGib(self, "models/objects/gibs/sm_meat/tris.md2", damage, Defines.GIB_ORGANIC);
                    GameMisc.ThrowHead(self, "models/objects/gibs/sm_meat/tris.md2", damage, Defines.GIB_ORGANIC);
                    self.deadflag = Defines.DEAD_DEAD;
                    return;
                }

                if (self.deadflag == Defines.DEAD_DEAD)
                    return;
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death, 1, Defines.ATTN_NORM, 0);
                self.deadflag = Defines.DEAD_DEAD;
                self.takedamage = Defines.DAMAGE_YES;
                self.monsterinfo.currentmove = flipper_move_death;
            }
        }

        public static void SP_monster_flipper(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_pain1 = GameBase.gi.Soundindex("flipper/flppain1.wav");
            sound_pain2 = GameBase.gi.Soundindex("flipper/flppain2.wav");
            sound_death = GameBase.gi.Soundindex("flipper/flpdeth1.wav");
            sound_chomp = GameBase.gi.Soundindex("flipper/flpatck1.wav");
            sound_attack = GameBase.gi.Soundindex("flipper/flpatck2.wav");
            sound_idle = GameBase.gi.Soundindex("flipper/flpidle1.wav");
            sound_search = GameBase.gi.Soundindex("flipper/flpsrch1.wav");
            sound_sight = GameBase.gi.Soundindex("flipper/flpsght1.wav");
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/flipper/tris.md2");
            Math3D.VectorSet(self.mins, -16, -16, 0);
            Math3D.VectorSet(self.maxs, 16, 16, 32);
            self.health = 50;
            self.gib_health = -30;
            self.mass = 100;
            self.pain = flipper_pain;
            self.die = flipper_die;
            self.monsterinfo.stand = flipper_stand;
            self.monsterinfo.walk = flipper_walk;
            self.monsterinfo.run = flipper_start_run;
            self.monsterinfo.melee = flipper_melee;
            self.monsterinfo.sight = flipper_sight;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = flipper_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.swimmonster_start.Think(self);
        }
    }
}
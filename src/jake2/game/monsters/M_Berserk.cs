using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Berserk
    {
        public static readonly int FRAME_stand1 = 0;
        public static readonly int FRAME_stand2 = 1;
        public static readonly int FRAME_stand3 = 2;
        public static readonly int FRAME_stand4 = 3;
        public static readonly int FRAME_stand5 = 4;
        public static readonly int FRAME_standb1 = 5;
        public static readonly int FRAME_standb2 = 6;
        public static readonly int FRAME_standb3 = 7;
        public static readonly int FRAME_standb4 = 8;
        public static readonly int FRAME_standb5 = 9;
        public static readonly int FRAME_standb6 = 10;
        public static readonly int FRAME_standb7 = 11;
        public static readonly int FRAME_standb8 = 12;
        public static readonly int FRAME_standb9 = 13;
        public static readonly int FRAME_standb10 = 14;
        public static readonly int FRAME_standb11 = 15;
        public static readonly int FRAME_standb12 = 16;
        public static readonly int FRAME_standb13 = 17;
        public static readonly int FRAME_standb14 = 18;
        public static readonly int FRAME_standb15 = 19;
        public static readonly int FRAME_standb16 = 20;
        public static readonly int FRAME_standb17 = 21;
        public static readonly int FRAME_standb18 = 22;
        public static readonly int FRAME_standb19 = 23;
        public static readonly int FRAME_standb20 = 24;
        public static readonly int FRAME_walkc1 = 25;
        public static readonly int FRAME_walkc2 = 26;
        public static readonly int FRAME_walkc3 = 27;
        public static readonly int FRAME_walkc4 = 28;
        public static readonly int FRAME_walkc5 = 29;
        public static readonly int FRAME_walkc6 = 30;
        public static readonly int FRAME_walkc7 = 31;
        public static readonly int FRAME_walkc8 = 32;
        public static readonly int FRAME_walkc9 = 33;
        public static readonly int FRAME_walkc10 = 34;
        public static readonly int FRAME_walkc11 = 35;
        public static readonly int FRAME_run1 = 36;
        public static readonly int FRAME_run2 = 37;
        public static readonly int FRAME_run3 = 38;
        public static readonly int FRAME_run4 = 39;
        public static readonly int FRAME_run5 = 40;
        public static readonly int FRAME_run6 = 41;
        public static readonly int FRAME_att_a1 = 42;
        public static readonly int FRAME_att_a2 = 43;
        public static readonly int FRAME_att_a3 = 44;
        public static readonly int FRAME_att_a4 = 45;
        public static readonly int FRAME_att_a5 = 46;
        public static readonly int FRAME_att_a6 = 47;
        public static readonly int FRAME_att_a7 = 48;
        public static readonly int FRAME_att_a8 = 49;
        public static readonly int FRAME_att_a9 = 50;
        public static readonly int FRAME_att_a10 = 51;
        public static readonly int FRAME_att_a11 = 52;
        public static readonly int FRAME_att_a12 = 53;
        public static readonly int FRAME_att_a13 = 54;
        public static readonly int FRAME_att_b1 = 55;
        public static readonly int FRAME_att_b2 = 56;
        public static readonly int FRAME_att_b3 = 57;
        public static readonly int FRAME_att_b4 = 58;
        public static readonly int FRAME_att_b5 = 59;
        public static readonly int FRAME_att_b6 = 60;
        public static readonly int FRAME_att_b7 = 61;
        public static readonly int FRAME_att_b8 = 62;
        public static readonly int FRAME_att_b9 = 63;
        public static readonly int FRAME_att_b10 = 64;
        public static readonly int FRAME_att_b11 = 65;
        public static readonly int FRAME_att_b12 = 66;
        public static readonly int FRAME_att_b13 = 67;
        public static readonly int FRAME_att_b14 = 68;
        public static readonly int FRAME_att_b15 = 69;
        public static readonly int FRAME_att_b16 = 70;
        public static readonly int FRAME_att_b17 = 71;
        public static readonly int FRAME_att_b18 = 72;
        public static readonly int FRAME_att_b19 = 73;
        public static readonly int FRAME_att_b20 = 74;
        public static readonly int FRAME_att_b21 = 75;
        public static readonly int FRAME_att_c1 = 76;
        public static readonly int FRAME_att_c2 = 77;
        public static readonly int FRAME_att_c3 = 78;
        public static readonly int FRAME_att_c4 = 79;
        public static readonly int FRAME_att_c5 = 80;
        public static readonly int FRAME_att_c6 = 81;
        public static readonly int FRAME_att_c7 = 82;
        public static readonly int FRAME_att_c8 = 83;
        public static readonly int FRAME_att_c9 = 84;
        public static readonly int FRAME_att_c10 = 85;
        public static readonly int FRAME_att_c11 = 86;
        public static readonly int FRAME_att_c12 = 87;
        public static readonly int FRAME_att_c13 = 88;
        public static readonly int FRAME_att_c14 = 89;
        public static readonly int FRAME_att_c15 = 90;
        public static readonly int FRAME_att_c16 = 91;
        public static readonly int FRAME_att_c17 = 92;
        public static readonly int FRAME_att_c18 = 93;
        public static readonly int FRAME_att_c19 = 94;
        public static readonly int FRAME_att_c20 = 95;
        public static readonly int FRAME_att_c21 = 96;
        public static readonly int FRAME_att_c22 = 97;
        public static readonly int FRAME_att_c23 = 98;
        public static readonly int FRAME_att_c24 = 99;
        public static readonly int FRAME_att_c25 = 100;
        public static readonly int FRAME_att_c26 = 101;
        public static readonly int FRAME_att_c27 = 102;
        public static readonly int FRAME_att_c28 = 103;
        public static readonly int FRAME_att_c29 = 104;
        public static readonly int FRAME_att_c30 = 105;
        public static readonly int FRAME_att_c31 = 106;
        public static readonly int FRAME_att_c32 = 107;
        public static readonly int FRAME_att_c33 = 108;
        public static readonly int FRAME_att_c34 = 109;
        public static readonly int FRAME_r_att1 = 110;
        public static readonly int FRAME_r_att2 = 111;
        public static readonly int FRAME_r_att3 = 112;
        public static readonly int FRAME_r_att4 = 113;
        public static readonly int FRAME_r_att5 = 114;
        public static readonly int FRAME_r_att6 = 115;
        public static readonly int FRAME_r_att7 = 116;
        public static readonly int FRAME_r_att8 = 117;
        public static readonly int FRAME_r_att9 = 118;
        public static readonly int FRAME_r_att10 = 119;
        public static readonly int FRAME_r_att11 = 120;
        public static readonly int FRAME_r_att12 = 121;
        public static readonly int FRAME_r_att13 = 122;
        public static readonly int FRAME_r_att14 = 123;
        public static readonly int FRAME_r_att15 = 124;
        public static readonly int FRAME_r_att16 = 125;
        public static readonly int FRAME_r_att17 = 126;
        public static readonly int FRAME_r_att18 = 127;
        public static readonly int FRAME_r_attb1 = 128;
        public static readonly int FRAME_r_attb2 = 129;
        public static readonly int FRAME_r_attb3 = 130;
        public static readonly int FRAME_r_attb4 = 131;
        public static readonly int FRAME_r_attb5 = 132;
        public static readonly int FRAME_r_attb6 = 133;
        public static readonly int FRAME_r_attb7 = 134;
        public static readonly int FRAME_r_attb8 = 135;
        public static readonly int FRAME_r_attb9 = 136;
        public static readonly int FRAME_r_attb10 = 137;
        public static readonly int FRAME_r_attb11 = 138;
        public static readonly int FRAME_r_attb12 = 139;
        public static readonly int FRAME_r_attb13 = 140;
        public static readonly int FRAME_r_attb14 = 141;
        public static readonly int FRAME_r_attb15 = 142;
        public static readonly int FRAME_r_attb16 = 143;
        public static readonly int FRAME_r_attb17 = 144;
        public static readonly int FRAME_r_attb18 = 145;
        public static readonly int FRAME_slam1 = 146;
        public static readonly int FRAME_slam2 = 147;
        public static readonly int FRAME_slam3 = 148;
        public static readonly int FRAME_slam4 = 149;
        public static readonly int FRAME_slam5 = 150;
        public static readonly int FRAME_slam6 = 151;
        public static readonly int FRAME_slam7 = 152;
        public static readonly int FRAME_slam8 = 153;
        public static readonly int FRAME_slam9 = 154;
        public static readonly int FRAME_slam10 = 155;
        public static readonly int FRAME_slam11 = 156;
        public static readonly int FRAME_slam12 = 157;
        public static readonly int FRAME_slam13 = 158;
        public static readonly int FRAME_slam14 = 159;
        public static readonly int FRAME_slam15 = 160;
        public static readonly int FRAME_slam16 = 161;
        public static readonly int FRAME_slam17 = 162;
        public static readonly int FRAME_slam18 = 163;
        public static readonly int FRAME_slam19 = 164;
        public static readonly int FRAME_slam20 = 165;
        public static readonly int FRAME_slam21 = 166;
        public static readonly int FRAME_slam22 = 167;
        public static readonly int FRAME_slam23 = 168;
        public static readonly int FRAME_duck1 = 169;
        public static readonly int FRAME_duck2 = 170;
        public static readonly int FRAME_duck3 = 171;
        public static readonly int FRAME_duck4 = 172;
        public static readonly int FRAME_duck5 = 173;
        public static readonly int FRAME_duck6 = 174;
        public static readonly int FRAME_duck7 = 175;
        public static readonly int FRAME_duck8 = 176;
        public static readonly int FRAME_duck9 = 177;
        public static readonly int FRAME_duck10 = 178;
        public static readonly int FRAME_fall1 = 179;
        public static readonly int FRAME_fall2 = 180;
        public static readonly int FRAME_fall3 = 181;
        public static readonly int FRAME_fall4 = 182;
        public static readonly int FRAME_fall5 = 183;
        public static readonly int FRAME_fall6 = 184;
        public static readonly int FRAME_fall7 = 185;
        public static readonly int FRAME_fall8 = 186;
        public static readonly int FRAME_fall9 = 187;
        public static readonly int FRAME_fall10 = 188;
        public static readonly int FRAME_fall11 = 189;
        public static readonly int FRAME_fall12 = 190;
        public static readonly int FRAME_fall13 = 191;
        public static readonly int FRAME_fall14 = 192;
        public static readonly int FRAME_fall15 = 193;
        public static readonly int FRAME_fall16 = 194;
        public static readonly int FRAME_fall17 = 195;
        public static readonly int FRAME_fall18 = 196;
        public static readonly int FRAME_fall19 = 197;
        public static readonly int FRAME_fall20 = 198;
        public static readonly int FRAME_painc1 = 199;
        public static readonly int FRAME_painc2 = 200;
        public static readonly int FRAME_painc3 = 201;
        public static readonly int FRAME_painc4 = 202;
        public static readonly int FRAME_painb1 = 203;
        public static readonly int FRAME_painb2 = 204;
        public static readonly int FRAME_painb3 = 205;
        public static readonly int FRAME_painb4 = 206;
        public static readonly int FRAME_painb5 = 207;
        public static readonly int FRAME_painb6 = 208;
        public static readonly int FRAME_painb7 = 209;
        public static readonly int FRAME_painb8 = 210;
        public static readonly int FRAME_painb9 = 211;
        public static readonly int FRAME_painb10 = 212;
        public static readonly int FRAME_painb11 = 213;
        public static readonly int FRAME_painb12 = 214;
        public static readonly int FRAME_painb13 = 215;
        public static readonly int FRAME_painb14 = 216;
        public static readonly int FRAME_painb15 = 217;
        public static readonly int FRAME_painb16 = 218;
        public static readonly int FRAME_painb17 = 219;
        public static readonly int FRAME_painb18 = 220;
        public static readonly int FRAME_painb19 = 221;
        public static readonly int FRAME_painb20 = 222;
        public static readonly int FRAME_death1 = 223;
        public static readonly int FRAME_death2 = 224;
        public static readonly int FRAME_death3 = 225;
        public static readonly int FRAME_death4 = 226;
        public static readonly int FRAME_death5 = 227;
        public static readonly int FRAME_death6 = 228;
        public static readonly int FRAME_death7 = 229;
        public static readonly int FRAME_death8 = 230;
        public static readonly int FRAME_death9 = 231;
        public static readonly int FRAME_death10 = 232;
        public static readonly int FRAME_death11 = 233;
        public static readonly int FRAME_death12 = 234;
        public static readonly int FRAME_death13 = 235;
        public static readonly int FRAME_deathc1 = 236;
        public static readonly int FRAME_deathc2 = 237;
        public static readonly int FRAME_deathc3 = 238;
        public static readonly int FRAME_deathc4 = 239;
        public static readonly int FRAME_deathc5 = 240;
        public static readonly int FRAME_deathc6 = 241;
        public static readonly int FRAME_deathc7 = 242;
        public static readonly int FRAME_deathc8 = 243;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_pain;
        static int sound_die;
        static int sound_idle;
        static int sound_punch;
        static int sound_sight;
        static int sound_search;
        static EntInteractAdapter berserk_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "berserk_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter berserk_search = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_search";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter berserk_fidget = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_fidget";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    return true;
                if (Lib.Random() > 0.15F)
                    return true;
                self.monsterinfo.currentmove = berserk_move_stand_fidget;
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_idle, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static mframe_t[] berserk_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, berserk_fidget), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t berserk_move_stand = new mmove_t(FRAME_stand1, FRAME_stand5, berserk_frames_stand, null);
        static EntThinkAdapter berserk_stand = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = berserk_move_stand;
                return true;
            }
        }

        static mframe_t[] berserk_frames_stand_fidget = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t berserk_move_stand_fidget = new mmove_t(FRAME_standb1, FRAME_standb20, berserk_frames_stand_fidget, berserk_stand);
        static mframe_t[] berserk_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 9.1F, null), new mframe_t(GameAI.ai_walk, 6.3F, null), new mframe_t(GameAI.ai_walk, 4.9F, null), new mframe_t(GameAI.ai_walk, 6.7F, null), new mframe_t(GameAI.ai_walk, 6F, null), new mframe_t(GameAI.ai_walk, 8.2F, null), new mframe_t(GameAI.ai_walk, 7.2F, null), new mframe_t(GameAI.ai_walk, 6.1F, null), new mframe_t(GameAI.ai_walk, 4.9F, null), new mframe_t(GameAI.ai_walk, 4.7F, null), new mframe_t(GameAI.ai_walk, 4.7F, null), new mframe_t(GameAI.ai_walk, 4.8F, null)};
        static mmove_t berserk_move_walk = new mmove_t(FRAME_walkc1, FRAME_walkc11, berserk_frames_walk, null);
        static EntThinkAdapter berserk_walk = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = berserk_move_walk;
                return true;
            }
        }

        static mframe_t[] berserk_frames_run1 = new mframe_t[]{new mframe_t(GameAI.ai_run, 21, null), new mframe_t(GameAI.ai_run, 11, null), new mframe_t(GameAI.ai_run, 21, null), new mframe_t(GameAI.ai_run, 25, null), new mframe_t(GameAI.ai_run, 18, null), new mframe_t(GameAI.ai_run, 19, null)};
        static mmove_t berserk_move_run1 = new mmove_t(FRAME_run1, FRAME_run6, berserk_frames_run1, null);
        static EntThinkAdapter berserk_run = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = berserk_move_stand;
                else
                    self.monsterinfo.currentmove = berserk_move_run1;
                return true;
            }
        }

        static EntThinkAdapter berserk_attack_spike = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_attack_spike";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new[]{Defines.MELEE_DISTANCE, 0F, -24F};
                GameWeapon.Fire_hit(self, aim, (15 + (Lib.Rand() % 6)), 400);
                return true;
            }
        }

        static EntThinkAdapter berserk_swing = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_swing";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_punch, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] berserk_frames_attack_spike = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, berserk_swing), new mframe_t(GameAI.ai_charge, 0, berserk_attack_spike), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t berserk_move_attack_spike = new mmove_t(FRAME_att_c1, FRAME_att_c8, berserk_frames_attack_spike, berserk_run);
        static EntThinkAdapter berserk_attack_club = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_attack_club";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, self.mins[0], -4);
                GameWeapon.Fire_hit(self, aim, (5 + (Lib.Rand() % 6)), 400);
                return true;
            }
        }

        static mframe_t[] berserk_frames_attack_club = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, berserk_swing), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, berserk_attack_club), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t berserk_move_attack_club = new mmove_t(FRAME_att_c9, FRAME_att_c20, berserk_frames_attack_club, berserk_run);
        static EntThinkAdapter berserk_strike = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_strike";
            }

            public override bool Think(edict_t self)
            {
                return true;
            }
        }

        static mframe_t[] berserk_frames_attack_strike = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, berserk_swing), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, berserk_strike), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 9.7F, null), new mframe_t(GameAI.ai_move, 13.6F, null)};
        static mmove_t berserk_move_attack_strike = new mmove_t(FRAME_att_c21, FRAME_att_c34, berserk_frames_attack_strike, berserk_run);
        static EntThinkAdapter berserk_melee = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_melee";
            }

            public override bool Think(edict_t self)
            {
                if ((Lib.Rand() % 2) == 0)
                    self.monsterinfo.currentmove = berserk_move_attack_spike;
                else
                    self.monsterinfo.currentmove = berserk_move_attack_club;
                return true;
            }
        }

        static mframe_t[] berserk_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t berserk_move_pain1 = new mmove_t(FRAME_painc1, FRAME_painc4, berserk_frames_pain1, berserk_run);
        static mframe_t[] berserk_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t berserk_move_pain2 = new mmove_t(FRAME_painb1, FRAME_painb20, berserk_frames_pain2, berserk_run);
        static EntPainAdapter berserk_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "berserk_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                if (self.health < (self.max_health / 2))
                    self.s.skinnum = 1;
                if (GameBase.level.time < self.pain_debounce_time)
                    return;
                self.pain_debounce_time = GameBase.level.time + 3;
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain, 1, Defines.ATTN_NORM, 0);
                if (GameBase.skill.value == 3)
                    return;
                if ((damage < 20) || (Lib.Random() < 0.5))
                    self.monsterinfo.currentmove = berserk_move_pain1;
                else
                    self.monsterinfo.currentmove = berserk_move_pain2;
            }
        }

        static EntThinkAdapter berserk_dead = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "berserk_dead";
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

        static mframe_t[] berserk_frames_death1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t berserk_move_death1 = new mmove_t(FRAME_death1, FRAME_death13, berserk_frames_death1, berserk_dead);
        static mframe_t[] berserk_frames_death2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t berserk_move_death2 = new mmove_t(FRAME_deathc1, FRAME_deathc8, berserk_frames_death2, berserk_dead);
        static EntDieAdapter berserk_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "berserk_die";
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
                if (damage >= 50)
                    self.monsterinfo.currentmove = berserk_move_death1;
                else
                    self.monsterinfo.currentmove = berserk_move_death2;
            }
        }

        public static void SP_monster_berserk(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_pain = GameBase.gi.Soundindex("berserk/berpain2.wav");
            sound_die = GameBase.gi.Soundindex("berserk/berdeth2.wav");
            sound_idle = GameBase.gi.Soundindex("berserk/beridle1.wav");
            sound_punch = GameBase.gi.Soundindex("berserk/attack.wav");
            sound_search = GameBase.gi.Soundindex("berserk/bersrch1.wav");
            sound_sight = GameBase.gi.Soundindex("berserk/sight.wav");
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/berserk/tris.md2");
            Math3D.VectorSet(self.mins, -16, -16, -24);
            Math3D.VectorSet(self.maxs, 16, 16, 32);
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.health = 240;
            self.gib_health = -60;
            self.mass = 250;
            self.pain = berserk_pain;
            self.die = berserk_die;
            self.monsterinfo.stand = berserk_stand;
            self.monsterinfo.walk = berserk_walk;
            self.monsterinfo.run = berserk_run;
            self.monsterinfo.dodge = null;
            self.monsterinfo.attack = null;
            self.monsterinfo.melee = berserk_melee;
            self.monsterinfo.sight = berserk_sight;
            self.monsterinfo.search = berserk_search;
            self.monsterinfo.currentmove = berserk_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameBase.gi.Linkentity(self);
            GameAI.walkmonster_start.Think(self);
        }
    }
}
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Gunner
    {
        public static readonly int FRAME_stand01 = 0;
        public static readonly int FRAME_stand02 = 1;
        public static readonly int FRAME_stand03 = 2;
        public static readonly int FRAME_stand04 = 3;
        public static readonly int FRAME_stand05 = 4;
        public static readonly int FRAME_stand06 = 5;
        public static readonly int FRAME_stand07 = 6;
        public static readonly int FRAME_stand08 = 7;
        public static readonly int FRAME_stand09 = 8;
        public static readonly int FRAME_stand10 = 9;
        public static readonly int FRAME_stand11 = 10;
        public static readonly int FRAME_stand12 = 11;
        public static readonly int FRAME_stand13 = 12;
        public static readonly int FRAME_stand14 = 13;
        public static readonly int FRAME_stand15 = 14;
        public static readonly int FRAME_stand16 = 15;
        public static readonly int FRAME_stand17 = 16;
        public static readonly int FRAME_stand18 = 17;
        public static readonly int FRAME_stand19 = 18;
        public static readonly int FRAME_stand20 = 19;
        public static readonly int FRAME_stand21 = 20;
        public static readonly int FRAME_stand22 = 21;
        public static readonly int FRAME_stand23 = 22;
        public static readonly int FRAME_stand24 = 23;
        public static readonly int FRAME_stand25 = 24;
        public static readonly int FRAME_stand26 = 25;
        public static readonly int FRAME_stand27 = 26;
        public static readonly int FRAME_stand28 = 27;
        public static readonly int FRAME_stand29 = 28;
        public static readonly int FRAME_stand30 = 29;
        public static readonly int FRAME_stand31 = 30;
        public static readonly int FRAME_stand32 = 31;
        public static readonly int FRAME_stand33 = 32;
        public static readonly int FRAME_stand34 = 33;
        public static readonly int FRAME_stand35 = 34;
        public static readonly int FRAME_stand36 = 35;
        public static readonly int FRAME_stand37 = 36;
        public static readonly int FRAME_stand38 = 37;
        public static readonly int FRAME_stand39 = 38;
        public static readonly int FRAME_stand40 = 39;
        public static readonly int FRAME_stand41 = 40;
        public static readonly int FRAME_stand42 = 41;
        public static readonly int FRAME_stand43 = 42;
        public static readonly int FRAME_stand44 = 43;
        public static readonly int FRAME_stand45 = 44;
        public static readonly int FRAME_stand46 = 45;
        public static readonly int FRAME_stand47 = 46;
        public static readonly int FRAME_stand48 = 47;
        public static readonly int FRAME_stand49 = 48;
        public static readonly int FRAME_stand50 = 49;
        public static readonly int FRAME_stand51 = 50;
        public static readonly int FRAME_stand52 = 51;
        public static readonly int FRAME_stand53 = 52;
        public static readonly int FRAME_stand54 = 53;
        public static readonly int FRAME_stand55 = 54;
        public static readonly int FRAME_stand56 = 55;
        public static readonly int FRAME_stand57 = 56;
        public static readonly int FRAME_stand58 = 57;
        public static readonly int FRAME_stand59 = 58;
        public static readonly int FRAME_stand60 = 59;
        public static readonly int FRAME_stand61 = 60;
        public static readonly int FRAME_stand62 = 61;
        public static readonly int FRAME_stand63 = 62;
        public static readonly int FRAME_stand64 = 63;
        public static readonly int FRAME_stand65 = 64;
        public static readonly int FRAME_stand66 = 65;
        public static readonly int FRAME_stand67 = 66;
        public static readonly int FRAME_stand68 = 67;
        public static readonly int FRAME_stand69 = 68;
        public static readonly int FRAME_stand70 = 69;
        public static readonly int FRAME_walk01 = 70;
        public static readonly int FRAME_walk02 = 71;
        public static readonly int FRAME_walk03 = 72;
        public static readonly int FRAME_walk04 = 73;
        public static readonly int FRAME_walk05 = 74;
        public static readonly int FRAME_walk06 = 75;
        public static readonly int FRAME_walk07 = 76;
        public static readonly int FRAME_walk08 = 77;
        public static readonly int FRAME_walk09 = 78;
        public static readonly int FRAME_walk10 = 79;
        public static readonly int FRAME_walk11 = 80;
        public static readonly int FRAME_walk12 = 81;
        public static readonly int FRAME_walk13 = 82;
        public static readonly int FRAME_walk14 = 83;
        public static readonly int FRAME_walk15 = 84;
        public static readonly int FRAME_walk16 = 85;
        public static readonly int FRAME_walk17 = 86;
        public static readonly int FRAME_walk18 = 87;
        public static readonly int FRAME_walk19 = 88;
        public static readonly int FRAME_walk20 = 89;
        public static readonly int FRAME_walk21 = 90;
        public static readonly int FRAME_walk22 = 91;
        public static readonly int FRAME_walk23 = 92;
        public static readonly int FRAME_walk24 = 93;
        public static readonly int FRAME_run01 = 94;
        public static readonly int FRAME_run02 = 95;
        public static readonly int FRAME_run03 = 96;
        public static readonly int FRAME_run04 = 97;
        public static readonly int FRAME_run05 = 98;
        public static readonly int FRAME_run06 = 99;
        public static readonly int FRAME_run07 = 100;
        public static readonly int FRAME_run08 = 101;
        public static readonly int FRAME_runs01 = 102;
        public static readonly int FRAME_runs02 = 103;
        public static readonly int FRAME_runs03 = 104;
        public static readonly int FRAME_runs04 = 105;
        public static readonly int FRAME_runs05 = 106;
        public static readonly int FRAME_runs06 = 107;
        public static readonly int FRAME_attak101 = 108;
        public static readonly int FRAME_attak102 = 109;
        public static readonly int FRAME_attak103 = 110;
        public static readonly int FRAME_attak104 = 111;
        public static readonly int FRAME_attak105 = 112;
        public static readonly int FRAME_attak106 = 113;
        public static readonly int FRAME_attak107 = 114;
        public static readonly int FRAME_attak108 = 115;
        public static readonly int FRAME_attak109 = 116;
        public static readonly int FRAME_attak110 = 117;
        public static readonly int FRAME_attak111 = 118;
        public static readonly int FRAME_attak112 = 119;
        public static readonly int FRAME_attak113 = 120;
        public static readonly int FRAME_attak114 = 121;
        public static readonly int FRAME_attak115 = 122;
        public static readonly int FRAME_attak116 = 123;
        public static readonly int FRAME_attak117 = 124;
        public static readonly int FRAME_attak118 = 125;
        public static readonly int FRAME_attak119 = 126;
        public static readonly int FRAME_attak120 = 127;
        public static readonly int FRAME_attak121 = 128;
        public static readonly int FRAME_attak201 = 129;
        public static readonly int FRAME_attak202 = 130;
        public static readonly int FRAME_attak203 = 131;
        public static readonly int FRAME_attak204 = 132;
        public static readonly int FRAME_attak205 = 133;
        public static readonly int FRAME_attak206 = 134;
        public static readonly int FRAME_attak207 = 135;
        public static readonly int FRAME_attak208 = 136;
        public static readonly int FRAME_attak209 = 137;
        public static readonly int FRAME_attak210 = 138;
        public static readonly int FRAME_attak211 = 139;
        public static readonly int FRAME_attak212 = 140;
        public static readonly int FRAME_attak213 = 141;
        public static readonly int FRAME_attak214 = 142;
        public static readonly int FRAME_attak215 = 143;
        public static readonly int FRAME_attak216 = 144;
        public static readonly int FRAME_attak217 = 145;
        public static readonly int FRAME_attak218 = 146;
        public static readonly int FRAME_attak219 = 147;
        public static readonly int FRAME_attak220 = 148;
        public static readonly int FRAME_attak221 = 149;
        public static readonly int FRAME_attak222 = 150;
        public static readonly int FRAME_attak223 = 151;
        public static readonly int FRAME_attak224 = 152;
        public static readonly int FRAME_attak225 = 153;
        public static readonly int FRAME_attak226 = 154;
        public static readonly int FRAME_attak227 = 155;
        public static readonly int FRAME_attak228 = 156;
        public static readonly int FRAME_attak229 = 157;
        public static readonly int FRAME_attak230 = 158;
        public static readonly int FRAME_pain101 = 159;
        public static readonly int FRAME_pain102 = 160;
        public static readonly int FRAME_pain103 = 161;
        public static readonly int FRAME_pain104 = 162;
        public static readonly int FRAME_pain105 = 163;
        public static readonly int FRAME_pain106 = 164;
        public static readonly int FRAME_pain107 = 165;
        public static readonly int FRAME_pain108 = 166;
        public static readonly int FRAME_pain109 = 167;
        public static readonly int FRAME_pain110 = 168;
        public static readonly int FRAME_pain111 = 169;
        public static readonly int FRAME_pain112 = 170;
        public static readonly int FRAME_pain113 = 171;
        public static readonly int FRAME_pain114 = 172;
        public static readonly int FRAME_pain115 = 173;
        public static readonly int FRAME_pain116 = 174;
        public static readonly int FRAME_pain117 = 175;
        public static readonly int FRAME_pain118 = 176;
        public static readonly int FRAME_pain201 = 177;
        public static readonly int FRAME_pain202 = 178;
        public static readonly int FRAME_pain203 = 179;
        public static readonly int FRAME_pain204 = 180;
        public static readonly int FRAME_pain205 = 181;
        public static readonly int FRAME_pain206 = 182;
        public static readonly int FRAME_pain207 = 183;
        public static readonly int FRAME_pain208 = 184;
        public static readonly int FRAME_pain301 = 185;
        public static readonly int FRAME_pain302 = 186;
        public static readonly int FRAME_pain303 = 187;
        public static readonly int FRAME_pain304 = 188;
        public static readonly int FRAME_pain305 = 189;
        public static readonly int FRAME_death01 = 190;
        public static readonly int FRAME_death02 = 191;
        public static readonly int FRAME_death03 = 192;
        public static readonly int FRAME_death04 = 193;
        public static readonly int FRAME_death05 = 194;
        public static readonly int FRAME_death06 = 195;
        public static readonly int FRAME_death07 = 196;
        public static readonly int FRAME_death08 = 197;
        public static readonly int FRAME_death09 = 198;
        public static readonly int FRAME_death10 = 199;
        public static readonly int FRAME_death11 = 200;
        public static readonly int FRAME_duck01 = 201;
        public static readonly int FRAME_duck02 = 202;
        public static readonly int FRAME_duck03 = 203;
        public static readonly int FRAME_duck04 = 204;
        public static readonly int FRAME_duck05 = 205;
        public static readonly int FRAME_duck06 = 206;
        public static readonly int FRAME_duck07 = 207;
        public static readonly int FRAME_duck08 = 208;
        public static readonly float MODEL_SCALE = 1.15F;
        static int sound_pain;
        static int sound_pain2;
        static int sound_death;
        static int sound_idle;
        static int sound_open;
        static int sound_search;
        static int sound_sight;
        static EntThinkAdapter gunner_idlesound = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_idlesound";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static EntInteractAdapter gunner_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "gunner_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter gunner_search = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_search";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] gunner_frames_fidget = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, gunner_idlesound), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static EntThinkAdapter gunner_stand = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = gunner_move_stand;
                return true;
            }
        }

        static mmove_t gunner_move_fidget = new mmove_t(FRAME_stand31, FRAME_stand70, gunner_frames_fidget, gunner_stand);
        static EntThinkAdapter gunner_fidget = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_fidget";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    return true;
                if (Lib.Random() <= 0.05)
                    self.monsterinfo.currentmove = gunner_move_fidget;
                return true;
            }
        }

        static mframe_t[] gunner_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, gunner_fidget), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, gunner_fidget), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, gunner_fidget)};
        static mmove_t gunner_move_stand = new mmove_t(FRAME_stand01, FRAME_stand30, gunner_frames_stand, null);
        static mframe_t[] gunner_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 4, null)};
        static mmove_t gunner_move_walk = new mmove_t(FRAME_walk07, FRAME_walk19, gunner_frames_walk, null);
        static EntThinkAdapter gunner_walk = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = gunner_move_walk;
                return true;
            }
        }

        static mframe_t[] gunner_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 26, null), new mframe_t(GameAI.ai_run, 9, null), new mframe_t(GameAI.ai_run, 9, null), new mframe_t(GameAI.ai_run, 9, null), new mframe_t(GameAI.ai_run, 15, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 13, null), new mframe_t(GameAI.ai_run, 6, null)};
        static mmove_t gunner_move_run = new mmove_t(FRAME_run01, FRAME_run08, gunner_frames_run, null);
        static EntThinkAdapter gunner_run = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = gunner_move_stand;
                else
                    self.monsterinfo.currentmove = gunner_move_run;
                return true;
            }
        }

        static mframe_t[] gunner_frames_runandshoot = new mframe_t[]{new mframe_t(GameAI.ai_run, 32, null), new mframe_t(GameAI.ai_run, 15, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 18, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 20, null)};
        static mmove_t gunner_move_runandshoot = new mmove_t(FRAME_runs01, FRAME_runs06, gunner_frames_runandshoot, null);
        static EntThinkAdapter gunner_runandshoot = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_runandshoot";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = gunner_move_runandshoot;
                return true;
            }
        }

        static mframe_t[] gunner_frames_pain3 = new mframe_t[]{new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 1, null)};
        static mmove_t gunner_move_pain3 = new mmove_t(FRAME_pain301, FRAME_pain305, gunner_frames_pain3, gunner_run);
        static mframe_t[] gunner_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 11, null), new mframe_t(GameAI.ai_move, 6, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -7, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -7, null)};
        static mmove_t gunner_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain208, gunner_frames_pain2, gunner_run);
        static mframe_t[] gunner_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t gunner_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain118, gunner_frames_pain1, gunner_run);
        static EntPainAdapter gunner_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "gunner_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                if (self.health < (self.max_health / 2))
                    self.s.skinnum = 1;
                if (GameBase.level.time < self.pain_debounce_time)
                    return;
                self.pain_debounce_time = GameBase.level.time + 3;
                if ((Lib.Rand() & 1) != 0)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                if (GameBase.skill.value == 3)
                    return;
                if (damage <= 10)
                    self.monsterinfo.currentmove = gunner_move_pain3;
                else if (damage <= 25)
                    self.monsterinfo.currentmove = gunner_move_pain2;
                else
                    self.monsterinfo.currentmove = gunner_move_pain1;
            }
        }

        static EntThinkAdapter gunner_dead = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_dead";
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

        static mframe_t[] gunner_frames_death = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -7, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 8, null), new mframe_t(GameAI.ai_move, 6, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t gunner_move_death = new mmove_t(FRAME_death01, FRAME_death11, gunner_frames_death, gunner_dead);
        static EntDieAdapter gunner_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "gunner_die";
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
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death, 1, Defines.ATTN_NORM, 0);
                self.deadflag = Defines.DEAD_DEAD;
                self.takedamage = Defines.DAMAGE_YES;
                self.monsterinfo.currentmove = gunner_move_death;
            }
        }

        static EntThinkAdapter gunner_duck_down = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_duck_down";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_DUCKED) != 0)
                    return true;
                self.monsterinfo.aiflags |= Defines.AI_DUCKED;
                if (GameBase.skill.value >= 2)
                {
                    if (Lib.Random() > 0.5)
                        GunnerGrenade.Think(self);
                }

                self.maxs[2] -= 32;
                self.takedamage = Defines.DAMAGE_YES;
                self.monsterinfo.pausetime = GameBase.level.time + 1;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        static EntThinkAdapter gunner_duck_hold = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_duck_hold";
            }

            public override bool Think(edict_t self)
            {
                if (GameBase.level.time >= self.monsterinfo.pausetime)
                    self.monsterinfo.aiflags &= ~Defines.AI_HOLD_FRAME;
                else
                    self.monsterinfo.aiflags |= Defines.AI_HOLD_FRAME;
                return true;
            }
        }

        static EntThinkAdapter gunner_duck_up = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_duck_up";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.aiflags &= ~Defines.AI_DUCKED;
                self.maxs[2] += 32;
                self.takedamage = Defines.DAMAGE_AIM;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        static mframe_t[] gunner_frames_duck = new mframe_t[]{new mframe_t(GameAI.ai_move, 1, gunner_duck_down), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, gunner_duck_hold), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 0, gunner_duck_up), new mframe_t(GameAI.ai_move, -1, null)};
        static mmove_t gunner_move_duck = new mmove_t(FRAME_duck01, FRAME_duck08, gunner_frames_duck, gunner_run);
        static EntDodgeAdapter gunner_dodge = new AnonymousEntDodgeAdapter();
        private sealed class AnonymousEntDodgeAdapter : EntDodgeAdapter
		{
			
            public override string GetID()
            {
                return "gunner_dodge";
            }

            public override void Dodge(edict_t self, edict_t attacker, float eta)
            {
                if (Lib.Random() > 0.25)
                    return;
                if (self.enemy == null)
                    self.enemy = attacker;
                self.monsterinfo.currentmove = gunner_move_duck;
            }
        }

        static EntThinkAdapter gunner_opengun = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_opengun";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_open, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static EntThinkAdapter GunnerFire = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "GunnerFire";
            }

            public override bool Think(edict_t self)
            {
                float[] start = new float[]{0, 0, 0};
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] target = new float[]{0, 0, 0};
                float[] aim = new float[]{0, 0, 0};
                int flash_number;
                flash_number = Defines.MZ2_GUNNER_MACHINEGUN_1 + (self.s.frame - FRAME_attak216);
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_number], forward, right, start);
                Math3D.VectorCopy(self.enemy.s.origin, target);
                Math3D.VectorMA(target, -0.2F, self.enemy.velocity, target);
                target[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(target, start, aim);
                Math3D.VectorNormalize(aim);
                Monster.Monster_fire_bullet(self, start, aim, 3, 4, Defines.DEFAULT_BULLET_HSPREAD, Defines.DEFAULT_BULLET_VSPREAD, flash_number);
                return true;
            }
        }

        static EntThinkAdapter GunnerGrenade = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "GunnerGrenade";
            }

            public override bool Think(edict_t self)
            {
                float[] start = new float[]{0, 0, 0};
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] aim = new float[]{0, 0, 0};
                int flash_number;
                if (self.s.frame == FRAME_attak105)
                    flash_number = Defines.MZ2_GUNNER_GRENADE_1;
                else if (self.s.frame == FRAME_attak108)
                    flash_number = Defines.MZ2_GUNNER_GRENADE_2;
                else if (self.s.frame == FRAME_attak111)
                    flash_number = Defines.MZ2_GUNNER_GRENADE_3;
                else
                    flash_number = Defines.MZ2_GUNNER_GRENADE_4;
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_number], forward, right, start);
                Math3D.VectorCopy(forward, aim);
                Monster.Monster_fire_grenade(self, start, aim, 50, 600, flash_number);
                return true;
            }
        }

        static EntThinkAdapter gunner_attack = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_attack";
            }

            public override bool Think(edict_t self)
            {
                if (GameUtil.Range(self, self.enemy) == Defines.RANGE_MELEE)
                {
                    self.monsterinfo.currentmove = gunner_move_attack_chain;
                }
                else
                {
                    if (Lib.Random() <= 0.5)
                        self.monsterinfo.currentmove = gunner_move_attack_grenade;
                    else
                        self.monsterinfo.currentmove = gunner_move_attack_chain;
                }

                return true;
            }
        }

        static EntThinkAdapter gunner_fire_chain = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_fire_chain";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = gunner_move_fire_chain;
                return true;
            }
        }

        static mframe_t[] gunner_frames_attack_chain = new mframe_t[] { new mframe_t(GameAI.ai_charge, 0, gunner_opengun), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t gunner_move_attack_chain = new mmove_t(FRAME_attak209, FRAME_attak215, gunner_frames_attack_chain, gunner_fire_chain);
        static mframe_t[] gunner_frames_fire_chain = new mframe_t[] { new mframe_t(GameAI.ai_charge, 0, GunnerFire), new mframe_t(GameAI.ai_charge, 0, GunnerFire), new mframe_t(GameAI.ai_charge, 0, GunnerFire), new mframe_t(GameAI.ai_charge, 0, GunnerFire), new mframe_t(GameAI.ai_charge, 0, GunnerFire), new mframe_t(GameAI.ai_charge, 0, GunnerFire), new mframe_t(GameAI.ai_charge, 0, GunnerFire), new mframe_t(GameAI.ai_charge, 0, GunnerFire)};
        static EntThinkAdapter gunner_refire_chain = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gunner_refire_chain";
            }

            public override bool Think(edict_t self)
            {
                if (self.enemy.health > 0)
                    if (GameUtil.Visible(self, self.enemy))
                        if (Lib.Random() <= 0.5)
                        {
                            self.monsterinfo.currentmove = gunner_move_fire_chain;
                            return true;
                        }

                self.monsterinfo.currentmove = gunner_move_endfire_chain;
                return true;
            }
        }

        static mmove_t gunner_move_fire_chain = new mmove_t(FRAME_attak216, FRAME_attak223, gunner_frames_fire_chain, gunner_refire_chain);
        static mframe_t[] gunner_frames_endfire_chain = new mframe_t[] { new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t gunner_move_endfire_chain = new mmove_t(FRAME_attak224, FRAME_attak230, gunner_frames_endfire_chain, gunner_run);
        static mframe_t[] gunner_frames_attack_grenade = new mframe_t[] { new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, GunnerGrenade), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, GunnerGrenade), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, GunnerGrenade), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, GunnerGrenade), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t gunner_move_attack_grenade = new mmove_t(FRAME_attak101, FRAME_attak121, gunner_frames_attack_grenade, gunner_run);
        public static void SP_monster_gunner(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_death = GameBase.gi.Soundindex("gunner/death1.wav");
            sound_pain = GameBase.gi.Soundindex("gunner/gunpain2.wav");
            sound_pain2 = GameBase.gi.Soundindex("gunner/gunpain1.wav");
            sound_idle = GameBase.gi.Soundindex("gunner/gunidle1.wav");
            sound_open = GameBase.gi.Soundindex("gunner/gunatck1.wav");
            sound_search = GameBase.gi.Soundindex("gunner/gunsrch1.wav");
            sound_sight = GameBase.gi.Soundindex("gunner/sight1.wav");
            GameBase.gi.Soundindex("gunner/gunatck2.wav");
            GameBase.gi.Soundindex("gunner/gunatck3.wav");
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/gunner/tris.md2");
            Math3D.VectorSet(self.mins, -16, -16, -24);
            Math3D.VectorSet(self.maxs, 16, 16, 32);
            self.health = 175;
            self.gib_health = -70;
            self.mass = 200;
            self.pain = gunner_pain;
            self.die = gunner_die;
            self.monsterinfo.stand = gunner_stand;
            self.monsterinfo.walk = gunner_walk;
            self.monsterinfo.run = gunner_run;
            self.monsterinfo.dodge = gunner_dodge;
            self.monsterinfo.attack = gunner_attack;
            self.monsterinfo.melee = null;
            self.monsterinfo.sight = gunner_sight;
            self.monsterinfo.search = gunner_search;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = gunner_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.walkmonster_start.Think(self);
        }
    }
}
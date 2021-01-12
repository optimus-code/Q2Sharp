using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Chick
    {
        public static readonly int FRAME_attak101 = 0;
        public static readonly int FRAME_attak102 = 1;
        public static readonly int FRAME_attak103 = 2;
        public static readonly int FRAME_attak104 = 3;
        public static readonly int FRAME_attak105 = 4;
        public static readonly int FRAME_attak106 = 5;
        public static readonly int FRAME_attak107 = 6;
        public static readonly int FRAME_attak108 = 7;
        public static readonly int FRAME_attak109 = 8;
        public static readonly int FRAME_attak110 = 9;
        public static readonly int FRAME_attak111 = 10;
        public static readonly int FRAME_attak112 = 11;
        public static readonly int FRAME_attak113 = 12;
        public static readonly int FRAME_attak114 = 13;
        public static readonly int FRAME_attak115 = 14;
        public static readonly int FRAME_attak116 = 15;
        public static readonly int FRAME_attak117 = 16;
        public static readonly int FRAME_attak118 = 17;
        public static readonly int FRAME_attak119 = 18;
        public static readonly int FRAME_attak120 = 19;
        public static readonly int FRAME_attak121 = 20;
        public static readonly int FRAME_attak122 = 21;
        public static readonly int FRAME_attak123 = 22;
        public static readonly int FRAME_attak124 = 23;
        public static readonly int FRAME_attak125 = 24;
        public static readonly int FRAME_attak126 = 25;
        public static readonly int FRAME_attak127 = 26;
        public static readonly int FRAME_attak128 = 27;
        public static readonly int FRAME_attak129 = 28;
        public static readonly int FRAME_attak130 = 29;
        public static readonly int FRAME_attak131 = 30;
        public static readonly int FRAME_attak132 = 31;
        public static readonly int FRAME_attak201 = 32;
        public static readonly int FRAME_attak202 = 33;
        public static readonly int FRAME_attak203 = 34;
        public static readonly int FRAME_attak204 = 35;
        public static readonly int FRAME_attak205 = 36;
        public static readonly int FRAME_attak206 = 37;
        public static readonly int FRAME_attak207 = 38;
        public static readonly int FRAME_attak208 = 39;
        public static readonly int FRAME_attak209 = 40;
        public static readonly int FRAME_attak210 = 41;
        public static readonly int FRAME_attak211 = 42;
        public static readonly int FRAME_attak212 = 43;
        public static readonly int FRAME_attak213 = 44;
        public static readonly int FRAME_attak214 = 45;
        public static readonly int FRAME_attak215 = 46;
        public static readonly int FRAME_attak216 = 47;
        public static readonly int FRAME_death101 = 48;
        public static readonly int FRAME_death102 = 49;
        public static readonly int FRAME_death103 = 50;
        public static readonly int FRAME_death104 = 51;
        public static readonly int FRAME_death105 = 52;
        public static readonly int FRAME_death106 = 53;
        public static readonly int FRAME_death107 = 54;
        public static readonly int FRAME_death108 = 55;
        public static readonly int FRAME_death109 = 56;
        public static readonly int FRAME_death110 = 57;
        public static readonly int FRAME_death111 = 58;
        public static readonly int FRAME_death112 = 59;
        public static readonly int FRAME_death201 = 60;
        public static readonly int FRAME_death202 = 61;
        public static readonly int FRAME_death203 = 62;
        public static readonly int FRAME_death204 = 63;
        public static readonly int FRAME_death205 = 64;
        public static readonly int FRAME_death206 = 65;
        public static readonly int FRAME_death207 = 66;
        public static readonly int FRAME_death208 = 67;
        public static readonly int FRAME_death209 = 68;
        public static readonly int FRAME_death210 = 69;
        public static readonly int FRAME_death211 = 70;
        public static readonly int FRAME_death212 = 71;
        public static readonly int FRAME_death213 = 72;
        public static readonly int FRAME_death214 = 73;
        public static readonly int FRAME_death215 = 74;
        public static readonly int FRAME_death216 = 75;
        public static readonly int FRAME_death217 = 76;
        public static readonly int FRAME_death218 = 77;
        public static readonly int FRAME_death219 = 78;
        public static readonly int FRAME_death220 = 79;
        public static readonly int FRAME_death221 = 80;
        public static readonly int FRAME_death222 = 81;
        public static readonly int FRAME_death223 = 82;
        public static readonly int FRAME_duck01 = 83;
        public static readonly int FRAME_duck02 = 84;
        public static readonly int FRAME_duck03 = 85;
        public static readonly int FRAME_duck04 = 86;
        public static readonly int FRAME_duck05 = 87;
        public static readonly int FRAME_duck06 = 88;
        public static readonly int FRAME_duck07 = 89;
        public static readonly int FRAME_pain101 = 90;
        public static readonly int FRAME_pain102 = 91;
        public static readonly int FRAME_pain103 = 92;
        public static readonly int FRAME_pain104 = 93;
        public static readonly int FRAME_pain105 = 94;
        public static readonly int FRAME_pain201 = 95;
        public static readonly int FRAME_pain202 = 96;
        public static readonly int FRAME_pain203 = 97;
        public static readonly int FRAME_pain204 = 98;
        public static readonly int FRAME_pain205 = 99;
        public static readonly int FRAME_pain301 = 100;
        public static readonly int FRAME_pain302 = 101;
        public static readonly int FRAME_pain303 = 102;
        public static readonly int FRAME_pain304 = 103;
        public static readonly int FRAME_pain305 = 104;
        public static readonly int FRAME_pain306 = 105;
        public static readonly int FRAME_pain307 = 106;
        public static readonly int FRAME_pain308 = 107;
        public static readonly int FRAME_pain309 = 108;
        public static readonly int FRAME_pain310 = 109;
        public static readonly int FRAME_pain311 = 110;
        public static readonly int FRAME_pain312 = 111;
        public static readonly int FRAME_pain313 = 112;
        public static readonly int FRAME_pain314 = 113;
        public static readonly int FRAME_pain315 = 114;
        public static readonly int FRAME_pain316 = 115;
        public static readonly int FRAME_pain317 = 116;
        public static readonly int FRAME_pain318 = 117;
        public static readonly int FRAME_pain319 = 118;
        public static readonly int FRAME_pain320 = 119;
        public static readonly int FRAME_pain321 = 120;
        public static readonly int FRAME_stand101 = 121;
        public static readonly int FRAME_stand102 = 122;
        public static readonly int FRAME_stand103 = 123;
        public static readonly int FRAME_stand104 = 124;
        public static readonly int FRAME_stand105 = 125;
        public static readonly int FRAME_stand106 = 126;
        public static readonly int FRAME_stand107 = 127;
        public static readonly int FRAME_stand108 = 128;
        public static readonly int FRAME_stand109 = 129;
        public static readonly int FRAME_stand110 = 130;
        public static readonly int FRAME_stand111 = 131;
        public static readonly int FRAME_stand112 = 132;
        public static readonly int FRAME_stand113 = 133;
        public static readonly int FRAME_stand114 = 134;
        public static readonly int FRAME_stand115 = 135;
        public static readonly int FRAME_stand116 = 136;
        public static readonly int FRAME_stand117 = 137;
        public static readonly int FRAME_stand118 = 138;
        public static readonly int FRAME_stand119 = 139;
        public static readonly int FRAME_stand120 = 140;
        public static readonly int FRAME_stand121 = 141;
        public static readonly int FRAME_stand122 = 142;
        public static readonly int FRAME_stand123 = 143;
        public static readonly int FRAME_stand124 = 144;
        public static readonly int FRAME_stand125 = 145;
        public static readonly int FRAME_stand126 = 146;
        public static readonly int FRAME_stand127 = 147;
        public static readonly int FRAME_stand128 = 148;
        public static readonly int FRAME_stand129 = 149;
        public static readonly int FRAME_stand130 = 150;
        public static readonly int FRAME_stand201 = 151;
        public static readonly int FRAME_stand202 = 152;
        public static readonly int FRAME_stand203 = 153;
        public static readonly int FRAME_stand204 = 154;
        public static readonly int FRAME_stand205 = 155;
        public static readonly int FRAME_stand206 = 156;
        public static readonly int FRAME_stand207 = 157;
        public static readonly int FRAME_stand208 = 158;
        public static readonly int FRAME_stand209 = 159;
        public static readonly int FRAME_stand210 = 160;
        public static readonly int FRAME_stand211 = 161;
        public static readonly int FRAME_stand212 = 162;
        public static readonly int FRAME_stand213 = 163;
        public static readonly int FRAME_stand214 = 164;
        public static readonly int FRAME_stand215 = 165;
        public static readonly int FRAME_stand216 = 166;
        public static readonly int FRAME_stand217 = 167;
        public static readonly int FRAME_stand218 = 168;
        public static readonly int FRAME_stand219 = 169;
        public static readonly int FRAME_stand220 = 170;
        public static readonly int FRAME_stand221 = 171;
        public static readonly int FRAME_stand222 = 172;
        public static readonly int FRAME_stand223 = 173;
        public static readonly int FRAME_stand224 = 174;
        public static readonly int FRAME_stand225 = 175;
        public static readonly int FRAME_stand226 = 176;
        public static readonly int FRAME_stand227 = 177;
        public static readonly int FRAME_stand228 = 178;
        public static readonly int FRAME_stand229 = 179;
        public static readonly int FRAME_stand230 = 180;
        public static readonly int FRAME_walk01 = 181;
        public static readonly int FRAME_walk02 = 182;
        public static readonly int FRAME_walk03 = 183;
        public static readonly int FRAME_walk04 = 184;
        public static readonly int FRAME_walk05 = 185;
        public static readonly int FRAME_walk06 = 186;
        public static readonly int FRAME_walk07 = 187;
        public static readonly int FRAME_walk08 = 188;
        public static readonly int FRAME_walk09 = 189;
        public static readonly int FRAME_walk10 = 190;
        public static readonly int FRAME_walk11 = 191;
        public static readonly int FRAME_walk12 = 192;
        public static readonly int FRAME_walk13 = 193;
        public static readonly int FRAME_walk14 = 194;
        public static readonly int FRAME_walk15 = 195;
        public static readonly int FRAME_walk16 = 196;
        public static readonly int FRAME_walk17 = 197;
        public static readonly int FRAME_walk18 = 198;
        public static readonly int FRAME_walk19 = 199;
        public static readonly int FRAME_walk20 = 200;
        public static readonly int FRAME_walk21 = 201;
        public static readonly int FRAME_walk22 = 202;
        public static readonly int FRAME_walk23 = 203;
        public static readonly int FRAME_walk24 = 204;
        public static readonly int FRAME_walk25 = 205;
        public static readonly int FRAME_walk26 = 206;
        public static readonly int FRAME_walk27 = 207;
        public static readonly int FRAME_recln201 = 208;
        public static readonly int FRAME_recln202 = 209;
        public static readonly int FRAME_recln203 = 210;
        public static readonly int FRAME_recln204 = 211;
        public static readonly int FRAME_recln205 = 212;
        public static readonly int FRAME_recln206 = 213;
        public static readonly int FRAME_recln207 = 214;
        public static readonly int FRAME_recln208 = 215;
        public static readonly int FRAME_recln209 = 216;
        public static readonly int FRAME_recln210 = 217;
        public static readonly int FRAME_recln211 = 218;
        public static readonly int FRAME_recln212 = 219;
        public static readonly int FRAME_recln213 = 220;
        public static readonly int FRAME_recln214 = 221;
        public static readonly int FRAME_recln215 = 222;
        public static readonly int FRAME_recln216 = 223;
        public static readonly int FRAME_recln217 = 224;
        public static readonly int FRAME_recln218 = 225;
        public static readonly int FRAME_recln219 = 226;
        public static readonly int FRAME_recln220 = 227;
        public static readonly int FRAME_recln221 = 228;
        public static readonly int FRAME_recln222 = 229;
        public static readonly int FRAME_recln223 = 230;
        public static readonly int FRAME_recln224 = 231;
        public static readonly int FRAME_recln225 = 232;
        public static readonly int FRAME_recln226 = 233;
        public static readonly int FRAME_recln227 = 234;
        public static readonly int FRAME_recln228 = 235;
        public static readonly int FRAME_recln229 = 236;
        public static readonly int FRAME_recln230 = 237;
        public static readonly int FRAME_recln231 = 238;
        public static readonly int FRAME_recln232 = 239;
        public static readonly int FRAME_recln233 = 240;
        public static readonly int FRAME_recln234 = 241;
        public static readonly int FRAME_recln235 = 242;
        public static readonly int FRAME_recln236 = 243;
        public static readonly int FRAME_recln237 = 244;
        public static readonly int FRAME_recln238 = 245;
        public static readonly int FRAME_recln239 = 246;
        public static readonly int FRAME_recln240 = 247;
        public static readonly int FRAME_recln101 = 248;
        public static readonly int FRAME_recln102 = 249;
        public static readonly int FRAME_recln103 = 250;
        public static readonly int FRAME_recln104 = 251;
        public static readonly int FRAME_recln105 = 252;
        public static readonly int FRAME_recln106 = 253;
        public static readonly int FRAME_recln107 = 254;
        public static readonly int FRAME_recln108 = 255;
        public static readonly int FRAME_recln109 = 256;
        public static readonly int FRAME_recln110 = 257;
        public static readonly int FRAME_recln111 = 258;
        public static readonly int FRAME_recln112 = 259;
        public static readonly int FRAME_recln113 = 260;
        public static readonly int FRAME_recln114 = 261;
        public static readonly int FRAME_recln115 = 262;
        public static readonly int FRAME_recln116 = 263;
        public static readonly int FRAME_recln117 = 264;
        public static readonly int FRAME_recln118 = 265;
        public static readonly int FRAME_recln119 = 266;
        public static readonly int FRAME_recln120 = 267;
        public static readonly int FRAME_recln121 = 268;
        public static readonly int FRAME_recln122 = 269;
        public static readonly int FRAME_recln123 = 270;
        public static readonly int FRAME_recln124 = 271;
        public static readonly int FRAME_recln125 = 272;
        public static readonly int FRAME_recln126 = 273;
        public static readonly int FRAME_recln127 = 274;
        public static readonly int FRAME_recln128 = 275;
        public static readonly int FRAME_recln129 = 276;
        public static readonly int FRAME_recln130 = 277;
        public static readonly int FRAME_recln131 = 278;
        public static readonly int FRAME_recln132 = 279;
        public static readonly int FRAME_recln133 = 280;
        public static readonly int FRAME_recln134 = 281;
        public static readonly int FRAME_recln135 = 282;
        public static readonly int FRAME_recln136 = 283;
        public static readonly int FRAME_recln137 = 284;
        public static readonly int FRAME_recln138 = 285;
        public static readonly int FRAME_recln139 = 286;
        public static readonly int FRAME_recln140 = 287;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_missile_prelaunch;
        static int sound_missile_launch;
        static int sound_melee_swing;
        static int sound_melee_hit;
        static int sound_missile_reload;
        static int sound_death1;
        static int sound_death2;
        static int sound_fall_down;
        static int sound_idle1;
        static int sound_idle2;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_pain3;
        static int sound_sight;
        static int sound_search;
        static EntThinkAdapter ChickMoan = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "ChickMoan";
            }

            public override bool Think(edict_t self)
            {
                if (Lib.Random() < 0.5)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle1, 1, Defines.ATTN_IDLE, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle2, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static mframe_t[] chick_frames_fidget = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, ChickMoan), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static EntThinkAdapter chick_stand = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = chick_move_stand;
                return true;
            }
        }

        static mmove_t chick_move_fidget = new mmove_t(FRAME_stand201, FRAME_stand230, chick_frames_fidget, chick_stand);
        static EntThinkAdapter chick_fidget = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_fidget";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    return true;
                if (Lib.Random() <= 0.3)
                    self.monsterinfo.currentmove = chick_move_fidget;
                return true;
            }
        }

        static mframe_t[] chick_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, chick_fidget)};
        static mmove_t chick_move_stand = new mmove_t(FRAME_stand101, FRAME_stand130, chick_frames_stand, null);
        static EntThinkAdapter chick_run = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                {
                    self.monsterinfo.currentmove = chick_move_stand;
                    return true;
                }

                if (self.monsterinfo.currentmove == chick_move_walk || self.monsterinfo.currentmove == chick_move_start_run)
                {
                    self.monsterinfo.currentmove = chick_move_run;
                }
                else
                {
                    self.monsterinfo.currentmove = chick_move_start_run;
                }

                return true;
            }
        }

        static mframe_t[] chick_frames_start_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 1, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, -1, null), new mframe_t(GameAI.ai_run, -1, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 1, null), new mframe_t(GameAI.ai_run, 3, null), new mframe_t(GameAI.ai_run, 6, null), new mframe_t(GameAI.ai_run, 3, null)};
        static mmove_t chick_move_start_run = new mmove_t(FRAME_walk01, FRAME_walk10, chick_frames_start_run, chick_run);
        static mframe_t[] chick_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 6, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 13, null), new mframe_t(GameAI.ai_run, 5, null), new mframe_t(GameAI.ai_run, 7, null), new mframe_t(GameAI.ai_run, 4, null), new mframe_t(GameAI.ai_run, 11, null), new mframe_t(GameAI.ai_run, 5, null), new mframe_t(GameAI.ai_run, 9, null), new mframe_t(GameAI.ai_run, 7, null)};
        static mmove_t chick_move_run = new mmove_t(FRAME_walk11, FRAME_walk20, chick_frames_run, null);
        static mframe_t[] chick_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 8, null), new mframe_t(GameAI.ai_walk, 13, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 11, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 9, null), new mframe_t(GameAI.ai_walk, 7, null)};
        static mmove_t chick_move_walk = new mmove_t(FRAME_walk11, FRAME_walk20, chick_frames_walk, null);
        static EntThinkAdapter chick_walk = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = chick_move_walk;
                return true;
            }
        }

        static mframe_t[] chick_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t chick_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain105, chick_frames_pain1, chick_run);
        static mframe_t[] chick_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t chick_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain205, chick_frames_pain2, chick_run);
        static mframe_t[] chick_frames_pain3 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 11, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, 7, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -8, null), new mframe_t(GameAI.ai_move, 2, null)};
        static mmove_t chick_move_pain3 = new mmove_t(FRAME_pain301, FRAME_pain321, chick_frames_pain3, chick_run);
        static EntPainAdapter chick_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "chick_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                float r;
                if (self.health < (self.max_health / 2))
                    self.s.skinnum = 1;
                if (GameBase.level.time < self.pain_debounce_time)
                    return;
                self.pain_debounce_time = GameBase.level.time + 3;
                r = Lib.Random();
                if (r < 0.33)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                else if (r < 0.66)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain3, 1, Defines.ATTN_NORM, 0);
                if (GameBase.skill.value == 3)
                    return;
                if (damage <= 10)
                    self.monsterinfo.currentmove = chick_move_pain1;
                else if (damage <= 25)
                    self.monsterinfo.currentmove = chick_move_pain2;
                else
                    self.monsterinfo.currentmove = chick_move_pain3;
                return;
            }
        }

        static EntThinkAdapter chick_dead = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_dead";
            }

            public override bool Think(edict_t self)
            {
                Math3D.VectorSet(self.mins, -16, -16, 0);
                Math3D.VectorSet(self.maxs, 16, 16, 16);
                self.movetype = Defines.MOVETYPE_TOSS;
                self.svflags |= Defines.SVF_DEADMONSTER;
                self.nextthink = 0;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        static mframe_t[] chick_frames_death2 = new mframe_t[]{new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 10, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 15, null), new mframe_t(GameAI.ai_move, 14, null), new mframe_t(GameAI.ai_move, 1, null)};
        static mmove_t chick_move_death2 = new mmove_t(FRAME_death201, FRAME_death223, chick_frames_death2, chick_dead);
        static mframe_t[] chick_frames_death1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -7, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 11, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t chick_move_death1 = new mmove_t(FRAME_death101, FRAME_death112, chick_frames_death1, chick_dead);
        static EntDieAdapter chick_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "chick_die";
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
                self.deadflag = Defines.DEAD_DEAD;
                self.takedamage = Defines.DAMAGE_YES;
                n = Lib.Rand() % 2;
                if (n == 0)
                {
                    self.monsterinfo.currentmove = chick_move_death1;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death1, 1, Defines.ATTN_NORM, 0);
                }
                else
                {
                    self.monsterinfo.currentmove = chick_move_death2;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death2, 1, Defines.ATTN_NORM, 0);
                }
            }
        }

        static EntThinkAdapter chick_duck_down = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_duck_down";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_DUCKED) != 0)
                    return true;
                self.monsterinfo.aiflags |= Defines.AI_DUCKED;
                self.maxs[2] -= 32;
                self.takedamage = Defines.DAMAGE_YES;
                self.monsterinfo.pausetime = GameBase.level.time + 1;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        static EntThinkAdapter chick_duck_hold = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_duck_hold";
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

        static EntThinkAdapter chick_duck_up = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_duck_up";
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

        static mframe_t[] chick_frames_duck = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, chick_duck_down), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 4, chick_duck_hold), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, -5, chick_duck_up), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 1, null)};
        static mmove_t chick_move_duck = new mmove_t(FRAME_duck01, FRAME_duck07, chick_frames_duck, chick_run);
        static EntDodgeAdapter chick_dodge = new AnonymousEntDodgeAdapter();
        private sealed class AnonymousEntDodgeAdapter : EntDodgeAdapter
		{
			
            public override string GetID()
            {
                return "chick_dodge";
            }

            public override void Dodge(edict_t self, edict_t attacker, float eta)
            {
                if (Lib.Random() > 0.25)
                    return;
                if (self.enemy != null)
                    self.enemy = attacker;
                self.monsterinfo.currentmove = chick_move_duck;
                return;
            }
        }

        static EntThinkAdapter ChickSlash = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "ChickSlash";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, self.mins[0], 10);
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_melee_swing, 1, Defines.ATTN_NORM, 0);
                GameWeapon.Fire_hit(self, aim, (10 + (Lib.Rand() % 6)), 100);
                return true;
            }
        }

        static EntThinkAdapter ChickRocket = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "ChickRocket";
            }

            public override bool Think(edict_t self)
            {
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] start = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                float[] vec = new float[]{0, 0, 0};
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[Defines.MZ2_CHICK_ROCKET_1], forward, right, start);
                Math3D.VectorCopy(self.enemy.s.origin, vec);
                vec[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(vec, start, dir);
                Math3D.VectorNormalize(dir);
                Monster.Monster_fire_rocket(self, start, dir, 50, 500, Defines.MZ2_CHICK_ROCKET_1);
                return true;
            }
        }

        static EntThinkAdapter Chick_PreAttack1 = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "Chick_PreAttack1";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_missile_prelaunch, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter ChickReload = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "ChickReload";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_missile_reload, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter chick_attack1 = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_attack1";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = chick_move_attack1;
                return true;
            }
        }

        static EntThinkAdapter chick_rerocket = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_rerocket";
            }

            public override bool Think(edict_t self)
            {
                if (self.enemy.health > 0)
                {
                    if (GameUtil.Range(self, self.enemy) > Defines.RANGE_MELEE)
                        if (GameUtil.Visible(self, self.enemy))
                            if (Lib.Random() <= 0.6)
                            {
                                self.monsterinfo.currentmove = chick_move_attack1;
                                return true;
                            }
                }

                self.monsterinfo.currentmove = chick_move_end_attack1;
                return true;
            }
        }

        static mframe_t[] chick_frames_start_attack1 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, Chick_PreAttack1), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 3, null), new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, 7, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, chick_attack1)};
        static mmove_t chick_move_start_attack1 = new mmove_t(FRAME_attak101, FRAME_attak113, chick_frames_start_attack1, null);
        static mframe_t[] chick_frames_attack1 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 19, ChickRocket), new mframe_t(GameAI.ai_charge, -6, null), new mframe_t(GameAI.ai_charge, -5, null), new mframe_t(GameAI.ai_charge, -2, null), new mframe_t(GameAI.ai_charge, -7, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 10, ChickReload), new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, 6, null), new mframe_t(GameAI.ai_charge, 6, null), new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, 3, chick_rerocket)};
        static mmove_t chick_move_attack1 = new mmove_t(FRAME_attak114, FRAME_attak127, chick_frames_attack1, null);
        static mframe_t[] chick_frames_end_attack1 = new mframe_t[]{new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -6, null), new mframe_t(GameAI.ai_charge, -4, null), new mframe_t(GameAI.ai_charge, -2, null)};
        static mmove_t chick_move_end_attack1 = new mmove_t(FRAME_attak128, FRAME_attak132, chick_frames_end_attack1, chick_run);
        static EntThinkAdapter chick_reslash = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_reslash";
            }

            public override bool Think(edict_t self)
            {
                if (self.enemy.health > 0)
                {
                    if (GameUtil.Range(self, self.enemy) == Defines.RANGE_MELEE)
                        if (Lib.Random() <= 0.9)
                        {
                            self.monsterinfo.currentmove = chick_move_slash;
                            return true;
                        }
                        else
                        {
                            self.monsterinfo.currentmove = chick_move_end_slash;
                            return true;
                        }
                }

                self.monsterinfo.currentmove = chick_move_end_slash;
                return true;
            }
        }

        static mframe_t[] chick_frames_slash = new mframe_t[]{new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 7, ChickSlash), new mframe_t(GameAI.ai_charge, -7, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, -2, chick_reslash)};
        static mmove_t chick_move_slash = new mmove_t(FRAME_attak204, FRAME_attak212, chick_frames_slash, null);
        static mframe_t[] chick_frames_end_slash = new mframe_t[]{new mframe_t(GameAI.ai_charge, -6, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, -6, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t chick_move_end_slash = new mmove_t(FRAME_attak213, FRAME_attak216, chick_frames_end_slash, chick_run);
        static EntThinkAdapter chick_slash = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_slash";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = chick_move_slash;
                return true;
            }
        }

        static mframe_t[] chick_frames_start_slash = new mframe_t[]{new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 8, null), new mframe_t(GameAI.ai_charge, 3, null)};
        static mmove_t chick_move_start_slash = new mmove_t(FRAME_attak201, FRAME_attak203, chick_frames_start_slash, chick_slash);
        static EntThinkAdapter chick_melee = new AnonymousEntThinkAdapter17();
        private sealed class AnonymousEntThinkAdapter17 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_melee";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = chick_move_start_slash;
                return true;
            }
        }

        static EntThinkAdapter chick_attack = new AnonymousEntThinkAdapter18();
        private sealed class AnonymousEntThinkAdapter18 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "chick_attack";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = chick_move_start_attack1;
                return true;
            }
        }

        static EntInteractAdapter chick_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "chick_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        public static void SP_monster_chick(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_missile_prelaunch = GameBase.gi.Soundindex("chick/chkatck1.wav");
            sound_missile_launch = GameBase.gi.Soundindex("chick/chkatck2.wav");
            sound_melee_swing = GameBase.gi.Soundindex("chick/chkatck3.wav");
            sound_melee_hit = GameBase.gi.Soundindex("chick/chkatck4.wav");
            sound_missile_reload = GameBase.gi.Soundindex("chick/chkatck5.wav");
            sound_death1 = GameBase.gi.Soundindex("chick/chkdeth1.wav");
            sound_death2 = GameBase.gi.Soundindex("chick/chkdeth2.wav");
            sound_fall_down = GameBase.gi.Soundindex("chick/chkfall1.wav");
            sound_idle1 = GameBase.gi.Soundindex("chick/chkidle1.wav");
            sound_idle2 = GameBase.gi.Soundindex("chick/chkidle2.wav");
            sound_pain1 = GameBase.gi.Soundindex("chick/chkpain1.wav");
            sound_pain2 = GameBase.gi.Soundindex("chick/chkpain2.wav");
            sound_pain3 = GameBase.gi.Soundindex("chick/chkpain3.wav");
            sound_sight = GameBase.gi.Soundindex("chick/chksght1.wav");
            sound_search = GameBase.gi.Soundindex("chick/chksrch1.wav");
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/bitch/tris.md2");
            Math3D.VectorSet(self.mins, -16, -16, 0);
            Math3D.VectorSet(self.maxs, 16, 16, 56);
            self.health = 175;
            self.gib_health = -70;
            self.mass = 200;
            self.pain = chick_pain;
            self.die = chick_die;
            self.monsterinfo.stand = chick_stand;
            self.monsterinfo.walk = chick_walk;
            self.monsterinfo.run = chick_run;
            self.monsterinfo.dodge = chick_dodge;
            self.monsterinfo.attack = chick_attack;
            self.monsterinfo.melee = chick_melee;
            self.monsterinfo.sight = chick_sight;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = chick_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.walkmonster_start.Think(self);
        }
    }
}
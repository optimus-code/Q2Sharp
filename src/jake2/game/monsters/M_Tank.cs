using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Tank
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
        public static readonly int FRAME_walk01 = 30;
        public static readonly int FRAME_walk02 = 31;
        public static readonly int FRAME_walk03 = 32;
        public static readonly int FRAME_walk04 = 33;
        public static readonly int FRAME_walk05 = 34;
        public static readonly int FRAME_walk06 = 35;
        public static readonly int FRAME_walk07 = 36;
        public static readonly int FRAME_walk08 = 37;
        public static readonly int FRAME_walk09 = 38;
        public static readonly int FRAME_walk10 = 39;
        public static readonly int FRAME_walk11 = 40;
        public static readonly int FRAME_walk12 = 41;
        public static readonly int FRAME_walk13 = 42;
        public static readonly int FRAME_walk14 = 43;
        public static readonly int FRAME_walk15 = 44;
        public static readonly int FRAME_walk16 = 45;
        public static readonly int FRAME_walk17 = 46;
        public static readonly int FRAME_walk18 = 47;
        public static readonly int FRAME_walk19 = 48;
        public static readonly int FRAME_walk20 = 49;
        public static readonly int FRAME_walk21 = 50;
        public static readonly int FRAME_walk22 = 51;
        public static readonly int FRAME_walk23 = 52;
        public static readonly int FRAME_walk24 = 53;
        public static readonly int FRAME_walk25 = 54;
        public static readonly int FRAME_attak101 = 55;
        public static readonly int FRAME_attak102 = 56;
        public static readonly int FRAME_attak103 = 57;
        public static readonly int FRAME_attak104 = 58;
        public static readonly int FRAME_attak105 = 59;
        public static readonly int FRAME_attak106 = 60;
        public static readonly int FRAME_attak107 = 61;
        public static readonly int FRAME_attak108 = 62;
        public static readonly int FRAME_attak109 = 63;
        public static readonly int FRAME_attak110 = 64;
        public static readonly int FRAME_attak111 = 65;
        public static readonly int FRAME_attak112 = 66;
        public static readonly int FRAME_attak113 = 67;
        public static readonly int FRAME_attak114 = 68;
        public static readonly int FRAME_attak115 = 69;
        public static readonly int FRAME_attak116 = 70;
        public static readonly int FRAME_attak117 = 71;
        public static readonly int FRAME_attak118 = 72;
        public static readonly int FRAME_attak119 = 73;
        public static readonly int FRAME_attak120 = 74;
        public static readonly int FRAME_attak121 = 75;
        public static readonly int FRAME_attak122 = 76;
        public static readonly int FRAME_attak201 = 77;
        public static readonly int FRAME_attak202 = 78;
        public static readonly int FRAME_attak203 = 79;
        public static readonly int FRAME_attak204 = 80;
        public static readonly int FRAME_attak205 = 81;
        public static readonly int FRAME_attak206 = 82;
        public static readonly int FRAME_attak207 = 83;
        public static readonly int FRAME_attak208 = 84;
        public static readonly int FRAME_attak209 = 85;
        public static readonly int FRAME_attak210 = 86;
        public static readonly int FRAME_attak211 = 87;
        public static readonly int FRAME_attak212 = 88;
        public static readonly int FRAME_attak213 = 89;
        public static readonly int FRAME_attak214 = 90;
        public static readonly int FRAME_attak215 = 91;
        public static readonly int FRAME_attak216 = 92;
        public static readonly int FRAME_attak217 = 93;
        public static readonly int FRAME_attak218 = 94;
        public static readonly int FRAME_attak219 = 95;
        public static readonly int FRAME_attak220 = 96;
        public static readonly int FRAME_attak221 = 97;
        public static readonly int FRAME_attak222 = 98;
        public static readonly int FRAME_attak223 = 99;
        public static readonly int FRAME_attak224 = 100;
        public static readonly int FRAME_attak225 = 101;
        public static readonly int FRAME_attak226 = 102;
        public static readonly int FRAME_attak227 = 103;
        public static readonly int FRAME_attak228 = 104;
        public static readonly int FRAME_attak229 = 105;
        public static readonly int FRAME_attak230 = 106;
        public static readonly int FRAME_attak231 = 107;
        public static readonly int FRAME_attak232 = 108;
        public static readonly int FRAME_attak233 = 109;
        public static readonly int FRAME_attak234 = 110;
        public static readonly int FRAME_attak235 = 111;
        public static readonly int FRAME_attak236 = 112;
        public static readonly int FRAME_attak237 = 113;
        public static readonly int FRAME_attak238 = 114;
        public static readonly int FRAME_attak301 = 115;
        public static readonly int FRAME_attak302 = 116;
        public static readonly int FRAME_attak303 = 117;
        public static readonly int FRAME_attak304 = 118;
        public static readonly int FRAME_attak305 = 119;
        public static readonly int FRAME_attak306 = 120;
        public static readonly int FRAME_attak307 = 121;
        public static readonly int FRAME_attak308 = 122;
        public static readonly int FRAME_attak309 = 123;
        public static readonly int FRAME_attak310 = 124;
        public static readonly int FRAME_attak311 = 125;
        public static readonly int FRAME_attak312 = 126;
        public static readonly int FRAME_attak313 = 127;
        public static readonly int FRAME_attak314 = 128;
        public static readonly int FRAME_attak315 = 129;
        public static readonly int FRAME_attak316 = 130;
        public static readonly int FRAME_attak317 = 131;
        public static readonly int FRAME_attak318 = 132;
        public static readonly int FRAME_attak319 = 133;
        public static readonly int FRAME_attak320 = 134;
        public static readonly int FRAME_attak321 = 135;
        public static readonly int FRAME_attak322 = 136;
        public static readonly int FRAME_attak323 = 137;
        public static readonly int FRAME_attak324 = 138;
        public static readonly int FRAME_attak325 = 139;
        public static readonly int FRAME_attak326 = 140;
        public static readonly int FRAME_attak327 = 141;
        public static readonly int FRAME_attak328 = 142;
        public static readonly int FRAME_attak329 = 143;
        public static readonly int FRAME_attak330 = 144;
        public static readonly int FRAME_attak331 = 145;
        public static readonly int FRAME_attak332 = 146;
        public static readonly int FRAME_attak333 = 147;
        public static readonly int FRAME_attak334 = 148;
        public static readonly int FRAME_attak335 = 149;
        public static readonly int FRAME_attak336 = 150;
        public static readonly int FRAME_attak337 = 151;
        public static readonly int FRAME_attak338 = 152;
        public static readonly int FRAME_attak339 = 153;
        public static readonly int FRAME_attak340 = 154;
        public static readonly int FRAME_attak341 = 155;
        public static readonly int FRAME_attak342 = 156;
        public static readonly int FRAME_attak343 = 157;
        public static readonly int FRAME_attak344 = 158;
        public static readonly int FRAME_attak345 = 159;
        public static readonly int FRAME_attak346 = 160;
        public static readonly int FRAME_attak347 = 161;
        public static readonly int FRAME_attak348 = 162;
        public static readonly int FRAME_attak349 = 163;
        public static readonly int FRAME_attak350 = 164;
        public static readonly int FRAME_attak351 = 165;
        public static readonly int FRAME_attak352 = 166;
        public static readonly int FRAME_attak353 = 167;
        public static readonly int FRAME_attak401 = 168;
        public static readonly int FRAME_attak402 = 169;
        public static readonly int FRAME_attak403 = 170;
        public static readonly int FRAME_attak404 = 171;
        public static readonly int FRAME_attak405 = 172;
        public static readonly int FRAME_attak406 = 173;
        public static readonly int FRAME_attak407 = 174;
        public static readonly int FRAME_attak408 = 175;
        public static readonly int FRAME_attak409 = 176;
        public static readonly int FRAME_attak410 = 177;
        public static readonly int FRAME_attak411 = 178;
        public static readonly int FRAME_attak412 = 179;
        public static readonly int FRAME_attak413 = 180;
        public static readonly int FRAME_attak414 = 181;
        public static readonly int FRAME_attak415 = 182;
        public static readonly int FRAME_attak416 = 183;
        public static readonly int FRAME_attak417 = 184;
        public static readonly int FRAME_attak418 = 185;
        public static readonly int FRAME_attak419 = 186;
        public static readonly int FRAME_attak420 = 187;
        public static readonly int FRAME_attak421 = 188;
        public static readonly int FRAME_attak422 = 189;
        public static readonly int FRAME_attak423 = 190;
        public static readonly int FRAME_attak424 = 191;
        public static readonly int FRAME_attak425 = 192;
        public static readonly int FRAME_attak426 = 193;
        public static readonly int FRAME_attak427 = 194;
        public static readonly int FRAME_attak428 = 195;
        public static readonly int FRAME_attak429 = 196;
        public static readonly int FRAME_pain101 = 197;
        public static readonly int FRAME_pain102 = 198;
        public static readonly int FRAME_pain103 = 199;
        public static readonly int FRAME_pain104 = 200;
        public static readonly int FRAME_pain201 = 201;
        public static readonly int FRAME_pain202 = 202;
        public static readonly int FRAME_pain203 = 203;
        public static readonly int FRAME_pain204 = 204;
        public static readonly int FRAME_pain205 = 205;
        public static readonly int FRAME_pain301 = 206;
        public static readonly int FRAME_pain302 = 207;
        public static readonly int FRAME_pain303 = 208;
        public static readonly int FRAME_pain304 = 209;
        public static readonly int FRAME_pain305 = 210;
        public static readonly int FRAME_pain306 = 211;
        public static readonly int FRAME_pain307 = 212;
        public static readonly int FRAME_pain308 = 213;
        public static readonly int FRAME_pain309 = 214;
        public static readonly int FRAME_pain310 = 215;
        public static readonly int FRAME_pain311 = 216;
        public static readonly int FRAME_pain312 = 217;
        public static readonly int FRAME_pain313 = 218;
        public static readonly int FRAME_pain314 = 219;
        public static readonly int FRAME_pain315 = 220;
        public static readonly int FRAME_pain316 = 221;
        public static readonly int FRAME_death101 = 222;
        public static readonly int FRAME_death102 = 223;
        public static readonly int FRAME_death103 = 224;
        public static readonly int FRAME_death104 = 225;
        public static readonly int FRAME_death105 = 226;
        public static readonly int FRAME_death106 = 227;
        public static readonly int FRAME_death107 = 228;
        public static readonly int FRAME_death108 = 229;
        public static readonly int FRAME_death109 = 230;
        public static readonly int FRAME_death110 = 231;
        public static readonly int FRAME_death111 = 232;
        public static readonly int FRAME_death112 = 233;
        public static readonly int FRAME_death113 = 234;
        public static readonly int FRAME_death114 = 235;
        public static readonly int FRAME_death115 = 236;
        public static readonly int FRAME_death116 = 237;
        public static readonly int FRAME_death117 = 238;
        public static readonly int FRAME_death118 = 239;
        public static readonly int FRAME_death119 = 240;
        public static readonly int FRAME_death120 = 241;
        public static readonly int FRAME_death121 = 242;
        public static readonly int FRAME_death122 = 243;
        public static readonly int FRAME_death123 = 244;
        public static readonly int FRAME_death124 = 245;
        public static readonly int FRAME_death125 = 246;
        public static readonly int FRAME_death126 = 247;
        public static readonly int FRAME_death127 = 248;
        public static readonly int FRAME_death128 = 249;
        public static readonly int FRAME_death129 = 250;
        public static readonly int FRAME_death130 = 251;
        public static readonly int FRAME_death131 = 252;
        public static readonly int FRAME_death132 = 253;
        public static readonly int FRAME_recln101 = 254;
        public static readonly int FRAME_recln102 = 255;
        public static readonly int FRAME_recln103 = 256;
        public static readonly int FRAME_recln104 = 257;
        public static readonly int FRAME_recln105 = 258;
        public static readonly int FRAME_recln106 = 259;
        public static readonly int FRAME_recln107 = 260;
        public static readonly int FRAME_recln108 = 261;
        public static readonly int FRAME_recln109 = 262;
        public static readonly int FRAME_recln110 = 263;
        public static readonly int FRAME_recln111 = 264;
        public static readonly int FRAME_recln112 = 265;
        public static readonly int FRAME_recln113 = 266;
        public static readonly int FRAME_recln114 = 267;
        public static readonly int FRAME_recln115 = 268;
        public static readonly int FRAME_recln116 = 269;
        public static readonly int FRAME_recln117 = 270;
        public static readonly int FRAME_recln118 = 271;
        public static readonly int FRAME_recln119 = 272;
        public static readonly int FRAME_recln120 = 273;
        public static readonly int FRAME_recln121 = 274;
        public static readonly int FRAME_recln122 = 275;
        public static readonly int FRAME_recln123 = 276;
        public static readonly int FRAME_recln124 = 277;
        public static readonly int FRAME_recln125 = 278;
        public static readonly int FRAME_recln126 = 279;
        public static readonly int FRAME_recln127 = 280;
        public static readonly int FRAME_recln128 = 281;
        public static readonly int FRAME_recln129 = 282;
        public static readonly int FRAME_recln130 = 283;
        public static readonly int FRAME_recln131 = 284;
        public static readonly int FRAME_recln132 = 285;
        public static readonly int FRAME_recln133 = 286;
        public static readonly int FRAME_recln134 = 287;
        public static readonly int FRAME_recln135 = 288;
        public static readonly int FRAME_recln136 = 289;
        public static readonly int FRAME_recln137 = 290;
        public static readonly int FRAME_recln138 = 291;
        public static readonly int FRAME_recln139 = 292;
        public static readonly int FRAME_recln140 = 293;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_thud;
        static int sound_pain;
        static int sound_idle;
        static int sound_die;
        static int sound_step;
        static int sound_sight;
        static int sound_windup;
        static int sound_strike;
        static EntInteractAdapter tank_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "tank_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter tank_footstep = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_footstep";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_step, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter tank_thud = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_thud";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_thud, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter tank_windup = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_windup";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_windup, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter tank_idle = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_idle";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static mframe_t[] tank_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t tank_move_stand = new mmove_t(FRAME_stand01, FRAME_stand30, tank_frames_stand, null);
        static EntThinkAdapter tank_stand = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = tank_move_stand;
                return true;
            }
        }

        static EntThinkAdapter tank_run = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_run";
            }

            public override bool Think(edict_t self)
            {
                if (self.enemy != null && self.enemy.client != null)
                    self.monsterinfo.aiflags |= Defines.AI_BRUTAL;
                else
                    self.monsterinfo.aiflags &= ~Defines.AI_BRUTAL;
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                {
                    self.monsterinfo.currentmove = tank_move_stand;
                    return true;
                }

                if (self.monsterinfo.currentmove == tank_move_walk || self.monsterinfo.currentmove == tank_move_start_run)
                {
                    self.monsterinfo.currentmove = tank_move_run;
                }
                else
                {
                    self.monsterinfo.currentmove = tank_move_start_run;
                }

                return true;
            }
        }

        static EntThinkAdapter tank_walk = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = tank_move_walk;
                return true;
            }
        }

        static mframe_t[] tank_frames_start_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 11, tank_footstep)};
        static mmove_t tank_move_start_walk = new mmove_t(FRAME_walk01, FRAME_walk04, tank_frames_start_walk, tank_walk);
        static mframe_t[] tank_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, tank_footstep), new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 6, tank_footstep)};
        static mmove_t tank_move_walk = new mmove_t(FRAME_walk05, FRAME_walk20, tank_frames_walk, null);
        static mframe_t[] tank_frames_stop_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 4, tank_footstep)};
        static mmove_t tank_move_stop_walk = new mmove_t(FRAME_walk21, FRAME_walk25, tank_frames_stop_walk, tank_stand);
        static mframe_t[] tank_frames_start_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 6, null), new mframe_t(GameAI.ai_run, 6, null), new mframe_t(GameAI.ai_run, 11, tank_footstep)};
        static mmove_t tank_move_start_run = new mmove_t(FRAME_walk01, FRAME_walk04, tank_frames_start_run, tank_run);
        static mframe_t[] tank_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 4, null), new mframe_t(GameAI.ai_run, 5, null), new mframe_t(GameAI.ai_run, 3, null), new mframe_t(GameAI.ai_run, 2, null), new mframe_t(GameAI.ai_run, 5, null), new mframe_t(GameAI.ai_run, 5, null), new mframe_t(GameAI.ai_run, 4, null), new mframe_t(GameAI.ai_run, 4, tank_footstep), new mframe_t(GameAI.ai_run, 3, null), new mframe_t(GameAI.ai_run, 5, null), new mframe_t(GameAI.ai_run, 4, null), new mframe_t(GameAI.ai_run, 5, null), new mframe_t(GameAI.ai_run, 7, null), new mframe_t(GameAI.ai_run, 7, null), new mframe_t(GameAI.ai_run, 6, null), new mframe_t(GameAI.ai_run, 6, tank_footstep)};
        static mmove_t tank_move_run = new mmove_t(FRAME_walk05, FRAME_walk20, tank_frames_run, null);
        static mframe_t[] tank_frames_stop_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 3, null), new mframe_t(GameAI.ai_run, 3, null), new mframe_t(GameAI.ai_run, 2, null), new mframe_t(GameAI.ai_run, 2, null), new mframe_t(GameAI.ai_run, 4, tank_footstep)};
        static mmove_t tank_move_stop_run = new mmove_t(FRAME_walk21, FRAME_walk25, tank_frames_stop_run, tank_walk);
        static mframe_t[] tank_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t tank_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain104, tank_frames_pain1, tank_run);
        static mframe_t[] tank_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t tank_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain205, tank_frames_pain2, tank_run);
        static mframe_t[] tank_frames_pain3 = new mframe_t[]{new mframe_t(GameAI.ai_move, -7, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, tank_footstep)};
        static mmove_t tank_move_pain3 = new mmove_t(FRAME_pain301, FRAME_pain316, tank_frames_pain3, tank_run);
        static EntPainAdapter tank_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "tank_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                if (self.health < (self.max_health / 2))
                    self.s.skinnum |= 1;
                if (damage <= 10)
                    return;
                if (GameBase.level.time < self.pain_debounce_time)
                    return;
                if (damage <= 30)
                    if (Lib.Random() > 0.2)
                        return;
                if (GameBase.skill.value >= 2)
                {
                    if ((self.s.frame >= FRAME_attak301) && (self.s.frame <= FRAME_attak330))
                        return;
                    if ((self.s.frame >= FRAME_attak101) && (self.s.frame <= FRAME_attak116))
                        return;
                }

                self.pain_debounce_time = GameBase.level.time + 3;
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain, 1, Defines.ATTN_NORM, 0);
                if (GameBase.skill.value == 3)
                    return;
                if (damage <= 30)
                    self.monsterinfo.currentmove = tank_move_pain1;
                else if (damage <= 60)
                    self.monsterinfo.currentmove = tank_move_pain2;
                else
                    self.monsterinfo.currentmove = tank_move_pain3;
            }
        }

        static EntThinkAdapter TankBlaster = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "TankBlaster";
            }

            public override bool Think(edict_t self)
            {
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] start = new float[]{0, 0, 0};
                float[] end = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                int flash_number;
                if (self.s.frame == FRAME_attak110)
                    flash_number = Defines.MZ2_TANK_BLASTER_1;
                else if (self.s.frame == FRAME_attak113)
                    flash_number = Defines.MZ2_TANK_BLASTER_2;
                else
                    flash_number = Defines.MZ2_TANK_BLASTER_3;
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_number], forward, right, start);
                Math3D.VectorCopy(self.enemy.s.origin, end);
                end[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(end, start, dir);
                Monster.Monster_fire_blaster(self, start, dir, 30, 800, flash_number, Defines.EF_BLASTER);
                return true;
            }
        }

        static EntThinkAdapter TankStrike = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "TankStrike";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_strike, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter TankRocket = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "TankRocket";
            }

            public override bool Think(edict_t self)
            {
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] start = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                float[] vec = new float[]{0, 0, 0};
                int flash_number;
                if (self.s.frame == FRAME_attak324)
                    flash_number = Defines.MZ2_TANK_ROCKET_1;
                else if (self.s.frame == FRAME_attak327)
                    flash_number = Defines.MZ2_TANK_ROCKET_2;
                else
                    flash_number = Defines.MZ2_TANK_ROCKET_3;
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_number], forward, right, start);
                Math3D.VectorCopy(self.enemy.s.origin, vec);
                vec[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(vec, start, dir);
                Math3D.VectorNormalize(dir);
                Monster.Monster_fire_rocket(self, start, dir, 50, 550, flash_number);
                return true;
            }
        }

        static EntThinkAdapter TankMachineGun = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "TankMachineGun";
            }

            public override bool Think(edict_t self)
            {
                float[] dir = new float[]{0, 0, 0};
                float[] vec = new float[]{0, 0, 0};
                float[] start = new float[]{0, 0, 0};
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                int flash_number;
                flash_number = Defines.MZ2_TANK_MACHINEGUN_1 + (self.s.frame - FRAME_attak406);
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_number], forward, right, start);
                if (self.enemy != null)
                {
                    Math3D.VectorCopy(self.enemy.s.origin, vec);
                    vec[2] += self.enemy.viewheight;
                    Math3D.VectorSubtract(vec, start, vec);
                    Math3D.Vectoangles(vec, vec);
                    dir[0] = vec[0];
                }
                else
                {
                    dir[0] = 0;
                }

                if (self.s.frame <= FRAME_attak415)
                    dir[1] = self.s.angles[1] - 8 * (self.s.frame - FRAME_attak411);
                else
                    dir[1] = self.s.angles[1] + 8 * (self.s.frame - FRAME_attak419);
                dir[2] = 0;
                Math3D.AngleVectors(dir, forward, null, null);
                Monster.Monster_fire_bullet(self, start, forward, 20, 4, Defines.DEFAULT_BULLET_HSPREAD, Defines.DEFAULT_BULLET_VSPREAD, flash_number);
                return true;
            }
        }

        static EntThinkAdapter tank_reattack_blaster = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_reattack_blaster";
            }

            public override bool Think(edict_t self)
            {
                if (GameBase.skill.value >= 2)
                    if (GameUtil.Visible(self, self.enemy))
                        if (self.enemy.health > 0)
                            if (Lib.Random() <= 0.6)
                            {
                                self.monsterinfo.currentmove = tank_move_reattack_blast;
                                return true;
                            }

                self.monsterinfo.currentmove = tank_move_attack_post_blast;
                return true;
            }
        }

        static mframe_t[] tank_frames_attack_blast = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, -2, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, TankBlaster), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, TankBlaster), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, TankBlaster)};
        static mmove_t tank_move_attack_blast = new mmove_t(FRAME_attak101, FRAME_attak116, tank_frames_attack_blast, tank_reattack_blaster);
        static mframe_t[] tank_frames_reattack_blast = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, TankBlaster), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, TankBlaster)};
        static mmove_t tank_move_reattack_blast = new mmove_t(FRAME_attak111, FRAME_attak116, tank_frames_reattack_blast, tank_reattack_blaster);
        static mframe_t[] tank_frames_attack_post_blast = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, -2, tank_footstep)};
        static mmove_t tank_move_attack_post_blast = new mmove_t(FRAME_attak117, FRAME_attak122, tank_frames_attack_post_blast, tank_run);
        static EntThinkAdapter tank_poststrike = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_poststrike";
            }

            public override bool Think(edict_t self)
            {
                self.enemy = null;
                tank_run.Think(self);
                return true;
            }
        }

        static EntThinkAdapter tank_doattack_rocket = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_doattack_rocket";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = tank_move_attack_fire_rocket;
                return true;
            }
        }

        static EntThinkAdapter tank_refire_rocket = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_refire_rocket";
            }

            public override bool Think(edict_t self)
            {
                if (GameBase.skill.value >= 2)
                    if (self.enemy.health > 0)
                        if (GameUtil.Visible(self, self.enemy))
                            if (Lib.Random() <= 0.4)
                            {
                                self.monsterinfo.currentmove = tank_move_attack_fire_rocket;
                                return true;
                            }

                self.monsterinfo.currentmove = tank_move_attack_post_rocket;
                return true;
            }
        }

        static mframe_t[] tank_frames_attack_strike = new mframe_t[]{new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 6, null), new mframe_t(GameAI.ai_move, 7, null), new mframe_t(GameAI.ai_move, 9, tank_footstep), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 2, tank_footstep), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 0, tank_windup), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, TankStrike), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -10, null), new mframe_t(GameAI.ai_move, -10, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -2, tank_footstep)};
        static mmove_t tank_move_attack_strike = new mmove_t(FRAME_attak201, FRAME_attak238, tank_frames_attack_strike, tank_poststrike);
        static mframe_t[] tank_frames_attack_pre_rocket = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, 7, null), new mframe_t(GameAI.ai_charge, 7, null), new mframe_t(GameAI.ai_charge, 7, tank_footstep), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -3, null)};
        static mmove_t tank_move_attack_pre_rocket = new mmove_t(FRAME_attak301, FRAME_attak321, tank_frames_attack_pre_rocket, tank_doattack_rocket);
        static mframe_t[] tank_frames_attack_fire_rocket = new mframe_t[]{new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, TankRocket), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, TankRocket), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -1, TankRocket)};
        static mmove_t tank_move_attack_fire_rocket = new mmove_t(FRAME_attak322, FRAME_attak330, tank_frames_attack_fire_rocket, tank_refire_rocket);
        static mframe_t[] tank_frames_attack_post_rocket = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, 3, null), new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -9, null), new mframe_t(GameAI.ai_charge, -8, null), new mframe_t(GameAI.ai_charge, -7, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, -1, tank_footstep), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t tank_move_attack_post_rocket = new mmove_t(FRAME_attak331, FRAME_attak353, tank_frames_attack_post_rocket, tank_run);
        static mframe_t[] tank_frames_attack_chain = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(null, 0, TankMachineGun), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t tank_move_attack_chain = new mmove_t(FRAME_attak401, FRAME_attak429, tank_frames_attack_chain, tank_run);
        static EntThinkAdapter tank_attack = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_attack";
            }

            public override bool Think(edict_t self)
            {
                float[] vec = new float[]{0, 0, 0};
                float range;
                float r;
                if (self.enemy.health < 0)
                {
                    self.monsterinfo.currentmove = tank_move_attack_strike;
                    self.monsterinfo.aiflags &= ~Defines.AI_BRUTAL;
                    return true;
                }

                Math3D.VectorSubtract(self.enemy.s.origin, self.s.origin, vec);
                range = Math3D.VectorLength(vec);
                r = Lib.Random();
                if (range <= 125)
                {
                    if (r < 0.4)
                        self.monsterinfo.currentmove = tank_move_attack_chain;
                    else
                        self.monsterinfo.currentmove = tank_move_attack_blast;
                }
                else if (range <= 250)
                {
                    if (r < 0.5)
                        self.monsterinfo.currentmove = tank_move_attack_chain;
                    else
                        self.monsterinfo.currentmove = tank_move_attack_blast;
                }
                else
                {
                    if (r < 0.33)
                        self.monsterinfo.currentmove = tank_move_attack_chain;
                    else if (r < 0.66)
                    {
                        self.monsterinfo.currentmove = tank_move_attack_pre_rocket;
                        self.pain_debounce_time = GameBase.level.time + 5F;
                    }
                    else
                        self.monsterinfo.currentmove = tank_move_attack_blast;
                }

                return true;
            }
        }

        static EntThinkAdapter tank_dead = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "tank_dead";
            }

            public override bool Think(edict_t self)
            {
                Math3D.VectorSet(self.mins, -16, -16, -16);
                Math3D.VectorSet(self.maxs, 16, 16, -0);
                self.movetype = Defines.MOVETYPE_TOSS;
                self.svflags |= Defines.SVF_DEADMONSTER;
                self.nextthink = 0;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        static mframe_t[] tank_frames_death1 = new mframe_t[]{new mframe_t(GameAI.ai_move, -7, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 6, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -7, null), new mframe_t(GameAI.ai_move, -15, tank_thud), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t tank_move_death = new mmove_t(FRAME_death101, FRAME_death132, tank_frames_death1, tank_dead);
        static EntDieAdapter tank_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "tank_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                int n;
                if (self.health <= self.gib_health)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, GameBase.gi.Soundindex("misc/udeath.wav"), 1, Defines.ATTN_NORM, 0);
                    for (n = 0; n < 1; n++)
                        GameMisc.ThrowGib(self, "models/objects/gibs/sm_meat/tris.md2", damage, Defines.GIB_ORGANIC);
                    for (n = 0; n < 4; n++)
                        GameMisc.ThrowGib(self, "models/objects/gibs/sm_metal/tris.md2", damage, Defines.GIB_METALLIC);
                    GameMisc.ThrowGib(self, "models/objects/gibs/chest/tris.md2", damage, Defines.GIB_ORGANIC);
                    GameMisc.ThrowHead(self, "models/objects/gibs/gear/tris.md2", damage, Defines.GIB_METALLIC);
                    self.deadflag = Defines.DEAD_DEAD;
                    return;
                }

                if (self.deadflag == Defines.DEAD_DEAD)
                    return;
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_die, 1, Defines.ATTN_NORM, 0);
                self.deadflag = Defines.DEAD_DEAD;
                self.takedamage = Defines.DAMAGE_YES;
                self.monsterinfo.currentmove = tank_move_death;
            }
        }

        public static EntThinkAdapter SP_monster_tank = new AnonymousEntThinkAdapter17();
        private sealed class AnonymousEntThinkAdapter17 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "SP_monster_tank";
            }

            public override bool Think(edict_t self)
            {
                if (GameBase.deathmatch.value != 0)
                {
                    GameUtil.G_FreeEdict(self);
                    return true;
                }

                self.s.modelindex = GameBase.gi.Modelindex("models/monsters/tank/tris.md2");
                Math3D.VectorSet(self.mins, -32, -32, -16);
                Math3D.VectorSet(self.maxs, 32, 32, 72);
                self.movetype = Defines.MOVETYPE_STEP;
                self.solid = Defines.SOLID_BBOX;
                sound_pain = GameBase.gi.Soundindex("tank/tnkpain2.wav");
                sound_thud = GameBase.gi.Soundindex("tank/tnkdeth2.wav");
                sound_idle = GameBase.gi.Soundindex("tank/tnkidle1.wav");
                sound_die = GameBase.gi.Soundindex("tank/death.wav");
                sound_step = GameBase.gi.Soundindex("tank/step.wav");
                sound_windup = GameBase.gi.Soundindex("tank/tnkatck4.wav");
                sound_strike = GameBase.gi.Soundindex("tank/tnkatck5.wav");
                sound_sight = GameBase.gi.Soundindex("tank/sight1.wav");
                GameBase.gi.Soundindex("tank/tnkatck1.wav");
                GameBase.gi.Soundindex("tank/tnkatk2a.wav");
                GameBase.gi.Soundindex("tank/tnkatk2b.wav");
                GameBase.gi.Soundindex("tank/tnkatk2c.wav");
                GameBase.gi.Soundindex("tank/tnkatk2d.wav");
                GameBase.gi.Soundindex("tank/tnkatk2e.wav");
                GameBase.gi.Soundindex("tank/tnkatck3.wav");
                if (Lib.Strcmp(self.classname, "monster_tank_commander") == 0)
                {
                    self.health = 1000;
                    self.gib_health = -225;
                }
                else
                {
                    self.health = 750;
                    self.gib_health = -200;
                }

                self.mass = 500;
                self.pain = tank_pain;
                self.die = tank_die;
                self.monsterinfo.stand = tank_stand;
                self.monsterinfo.walk = tank_walk;
                self.monsterinfo.run = tank_run;
                self.monsterinfo.dodge = null;
                self.monsterinfo.attack = tank_attack;
                self.monsterinfo.melee = null;
                self.monsterinfo.sight = tank_sight;
                self.monsterinfo.idle = tank_idle;
                GameBase.gi.Linkentity(self);
                self.monsterinfo.currentmove = tank_move_stand;
                self.monsterinfo.scale = MODEL_SCALE;
                GameAI.walkmonster_start.Think(self);
                if (Lib.Strcmp(self.classname, "monster_tank_commander") == 0)
                    self.s.skinnum = 2;
                return true;
            }
        }
    }
}
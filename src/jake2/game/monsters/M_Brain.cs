using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Brain
    {
        public static readonly int FRAME_walk101 = 0;
        public static readonly int FRAME_walk102 = 1;
        public static readonly int FRAME_walk103 = 2;
        public static readonly int FRAME_walk104 = 3;
        public static readonly int FRAME_walk105 = 4;
        public static readonly int FRAME_walk106 = 5;
        public static readonly int FRAME_walk107 = 6;
        public static readonly int FRAME_walk108 = 7;
        public static readonly int FRAME_walk109 = 8;
        public static readonly int FRAME_walk110 = 9;
        public static readonly int FRAME_walk111 = 10;
        public static readonly int FRAME_walk112 = 11;
        public static readonly int FRAME_walk113 = 12;
        public static readonly int FRAME_walk201 = 13;
        public static readonly int FRAME_walk202 = 14;
        public static readonly int FRAME_walk203 = 15;
        public static readonly int FRAME_walk204 = 16;
        public static readonly int FRAME_walk205 = 17;
        public static readonly int FRAME_walk206 = 18;
        public static readonly int FRAME_walk207 = 19;
        public static readonly int FRAME_walk208 = 20;
        public static readonly int FRAME_walk209 = 21;
        public static readonly int FRAME_walk210 = 22;
        public static readonly int FRAME_walk211 = 23;
        public static readonly int FRAME_walk212 = 24;
        public static readonly int FRAME_walk213 = 25;
        public static readonly int FRAME_walk214 = 26;
        public static readonly int FRAME_walk215 = 27;
        public static readonly int FRAME_walk216 = 28;
        public static readonly int FRAME_walk217 = 29;
        public static readonly int FRAME_walk218 = 30;
        public static readonly int FRAME_walk219 = 31;
        public static readonly int FRAME_walk220 = 32;
        public static readonly int FRAME_walk221 = 33;
        public static readonly int FRAME_walk222 = 34;
        public static readonly int FRAME_walk223 = 35;
        public static readonly int FRAME_walk224 = 36;
        public static readonly int FRAME_walk225 = 37;
        public static readonly int FRAME_walk226 = 38;
        public static readonly int FRAME_walk227 = 39;
        public static readonly int FRAME_walk228 = 40;
        public static readonly int FRAME_walk229 = 41;
        public static readonly int FRAME_walk230 = 42;
        public static readonly int FRAME_walk231 = 43;
        public static readonly int FRAME_walk232 = 44;
        public static readonly int FRAME_walk233 = 45;
        public static readonly int FRAME_walk234 = 46;
        public static readonly int FRAME_walk235 = 47;
        public static readonly int FRAME_walk236 = 48;
        public static readonly int FRAME_walk237 = 49;
        public static readonly int FRAME_walk238 = 50;
        public static readonly int FRAME_walk239 = 51;
        public static readonly int FRAME_walk240 = 52;
        public static readonly int FRAME_attak101 = 53;
        public static readonly int FRAME_attak102 = 54;
        public static readonly int FRAME_attak103 = 55;
        public static readonly int FRAME_attak104 = 56;
        public static readonly int FRAME_attak105 = 57;
        public static readonly int FRAME_attak106 = 58;
        public static readonly int FRAME_attak107 = 59;
        public static readonly int FRAME_attak108 = 60;
        public static readonly int FRAME_attak109 = 61;
        public static readonly int FRAME_attak110 = 62;
        public static readonly int FRAME_attak111 = 63;
        public static readonly int FRAME_attak112 = 64;
        public static readonly int FRAME_attak113 = 65;
        public static readonly int FRAME_attak114 = 66;
        public static readonly int FRAME_attak115 = 67;
        public static readonly int FRAME_attak116 = 68;
        public static readonly int FRAME_attak117 = 69;
        public static readonly int FRAME_attak118 = 70;
        public static readonly int FRAME_attak201 = 71;
        public static readonly int FRAME_attak202 = 72;
        public static readonly int FRAME_attak203 = 73;
        public static readonly int FRAME_attak204 = 74;
        public static readonly int FRAME_attak205 = 75;
        public static readonly int FRAME_attak206 = 76;
        public static readonly int FRAME_attak207 = 77;
        public static readonly int FRAME_attak208 = 78;
        public static readonly int FRAME_attak209 = 79;
        public static readonly int FRAME_attak210 = 80;
        public static readonly int FRAME_attak211 = 81;
        public static readonly int FRAME_attak212 = 82;
        public static readonly int FRAME_attak213 = 83;
        public static readonly int FRAME_attak214 = 84;
        public static readonly int FRAME_attak215 = 85;
        public static readonly int FRAME_attak216 = 86;
        public static readonly int FRAME_attak217 = 87;
        public static readonly int FRAME_pain101 = 88;
        public static readonly int FRAME_pain102 = 89;
        public static readonly int FRAME_pain103 = 90;
        public static readonly int FRAME_pain104 = 91;
        public static readonly int FRAME_pain105 = 92;
        public static readonly int FRAME_pain106 = 93;
        public static readonly int FRAME_pain107 = 94;
        public static readonly int FRAME_pain108 = 95;
        public static readonly int FRAME_pain109 = 96;
        public static readonly int FRAME_pain110 = 97;
        public static readonly int FRAME_pain111 = 98;
        public static readonly int FRAME_pain112 = 99;
        public static readonly int FRAME_pain113 = 100;
        public static readonly int FRAME_pain114 = 101;
        public static readonly int FRAME_pain115 = 102;
        public static readonly int FRAME_pain116 = 103;
        public static readonly int FRAME_pain117 = 104;
        public static readonly int FRAME_pain118 = 105;
        public static readonly int FRAME_pain119 = 106;
        public static readonly int FRAME_pain120 = 107;
        public static readonly int FRAME_pain121 = 108;
        public static readonly int FRAME_pain201 = 109;
        public static readonly int FRAME_pain202 = 110;
        public static readonly int FRAME_pain203 = 111;
        public static readonly int FRAME_pain204 = 112;
        public static readonly int FRAME_pain205 = 113;
        public static readonly int FRAME_pain206 = 114;
        public static readonly int FRAME_pain207 = 115;
        public static readonly int FRAME_pain208 = 116;
        public static readonly int FRAME_pain301 = 117;
        public static readonly int FRAME_pain302 = 118;
        public static readonly int FRAME_pain303 = 119;
        public static readonly int FRAME_pain304 = 120;
        public static readonly int FRAME_pain305 = 121;
        public static readonly int FRAME_pain306 = 122;
        public static readonly int FRAME_death101 = 123;
        public static readonly int FRAME_death102 = 124;
        public static readonly int FRAME_death103 = 125;
        public static readonly int FRAME_death104 = 126;
        public static readonly int FRAME_death105 = 127;
        public static readonly int FRAME_death106 = 128;
        public static readonly int FRAME_death107 = 129;
        public static readonly int FRAME_death108 = 130;
        public static readonly int FRAME_death109 = 131;
        public static readonly int FRAME_death110 = 132;
        public static readonly int FRAME_death111 = 133;
        public static readonly int FRAME_death112 = 134;
        public static readonly int FRAME_death113 = 135;
        public static readonly int FRAME_death114 = 136;
        public static readonly int FRAME_death115 = 137;
        public static readonly int FRAME_death116 = 138;
        public static readonly int FRAME_death117 = 139;
        public static readonly int FRAME_death118 = 140;
        public static readonly int FRAME_death201 = 141;
        public static readonly int FRAME_death202 = 142;
        public static readonly int FRAME_death203 = 143;
        public static readonly int FRAME_death204 = 144;
        public static readonly int FRAME_death205 = 145;
        public static readonly int FRAME_duck01 = 146;
        public static readonly int FRAME_duck02 = 147;
        public static readonly int FRAME_duck03 = 148;
        public static readonly int FRAME_duck04 = 149;
        public static readonly int FRAME_duck05 = 150;
        public static readonly int FRAME_duck06 = 151;
        public static readonly int FRAME_duck07 = 152;
        public static readonly int FRAME_duck08 = 153;
        public static readonly int FRAME_defens01 = 154;
        public static readonly int FRAME_defens02 = 155;
        public static readonly int FRAME_defens03 = 156;
        public static readonly int FRAME_defens04 = 157;
        public static readonly int FRAME_defens05 = 158;
        public static readonly int FRAME_defens06 = 159;
        public static readonly int FRAME_defens07 = 160;
        public static readonly int FRAME_defens08 = 161;
        public static readonly int FRAME_stand01 = 162;
        public static readonly int FRAME_stand02 = 163;
        public static readonly int FRAME_stand03 = 164;
        public static readonly int FRAME_stand04 = 165;
        public static readonly int FRAME_stand05 = 166;
        public static readonly int FRAME_stand06 = 167;
        public static readonly int FRAME_stand07 = 168;
        public static readonly int FRAME_stand08 = 169;
        public static readonly int FRAME_stand09 = 170;
        public static readonly int FRAME_stand10 = 171;
        public static readonly int FRAME_stand11 = 172;
        public static readonly int FRAME_stand12 = 173;
        public static readonly int FRAME_stand13 = 174;
        public static readonly int FRAME_stand14 = 175;
        public static readonly int FRAME_stand15 = 176;
        public static readonly int FRAME_stand16 = 177;
        public static readonly int FRAME_stand17 = 178;
        public static readonly int FRAME_stand18 = 179;
        public static readonly int FRAME_stand19 = 180;
        public static readonly int FRAME_stand20 = 181;
        public static readonly int FRAME_stand21 = 182;
        public static readonly int FRAME_stand22 = 183;
        public static readonly int FRAME_stand23 = 184;
        public static readonly int FRAME_stand24 = 185;
        public static readonly int FRAME_stand25 = 186;
        public static readonly int FRAME_stand26 = 187;
        public static readonly int FRAME_stand27 = 188;
        public static readonly int FRAME_stand28 = 189;
        public static readonly int FRAME_stand29 = 190;
        public static readonly int FRAME_stand30 = 191;
        public static readonly int FRAME_stand31 = 192;
        public static readonly int FRAME_stand32 = 193;
        public static readonly int FRAME_stand33 = 194;
        public static readonly int FRAME_stand34 = 195;
        public static readonly int FRAME_stand35 = 196;
        public static readonly int FRAME_stand36 = 197;
        public static readonly int FRAME_stand37 = 198;
        public static readonly int FRAME_stand38 = 199;
        public static readonly int FRAME_stand39 = 200;
        public static readonly int FRAME_stand40 = 201;
        public static readonly int FRAME_stand41 = 202;
        public static readonly int FRAME_stand42 = 203;
        public static readonly int FRAME_stand43 = 204;
        public static readonly int FRAME_stand44 = 205;
        public static readonly int FRAME_stand45 = 206;
        public static readonly int FRAME_stand46 = 207;
        public static readonly int FRAME_stand47 = 208;
        public static readonly int FRAME_stand48 = 209;
        public static readonly int FRAME_stand49 = 210;
        public static readonly int FRAME_stand50 = 211;
        public static readonly int FRAME_stand51 = 212;
        public static readonly int FRAME_stand52 = 213;
        public static readonly int FRAME_stand53 = 214;
        public static readonly int FRAME_stand54 = 215;
        public static readonly int FRAME_stand55 = 216;
        public static readonly int FRAME_stand56 = 217;
        public static readonly int FRAME_stand57 = 218;
        public static readonly int FRAME_stand58 = 219;
        public static readonly int FRAME_stand59 = 220;
        public static readonly int FRAME_stand60 = 221;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_chest_open;
        static int sound_tentacles_extend;
        static int sound_tentacles_retract;
        static int sound_death;
        static int sound_idle1;
        static int sound_idle2;
        static int sound_idle3;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_sight;
        static int sound_search;
        static int sound_melee1;
        static int sound_melee2;
        static int sound_melee3;
        static EntInteractAdapter brain_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "brain_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter brain_search = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_search";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] brain_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t brain_move_stand = new mmove_t(FRAME_stand01, FRAME_stand30, brain_frames_stand, null);
        static EntThinkAdapter brain_stand = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = brain_move_stand;
                return true;
            }
        }

        static mframe_t[] brain_frames_idle = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t brain_move_idle = new mmove_t(FRAME_stand31, FRAME_stand60, brain_frames_idle, brain_stand);
        static EntThinkAdapter brain_idle = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_idle";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_AUTO, sound_idle3, 1, Defines.ATTN_IDLE, 0);
                self.monsterinfo.currentmove = brain_move_idle;
                return true;
            }
        }

        static mframe_t[] brain_frames_walk1 = new mframe_t[]{new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 1, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 9, null), new mframe_t(GameAI.ai_walk, -4, null), new mframe_t(GameAI.ai_walk, -1, null), new mframe_t(GameAI.ai_walk, 2, null)};
        static mmove_t brain_move_walk1 = new mmove_t(FRAME_walk101, FRAME_walk111, brain_frames_walk1, null);
        static EntThinkAdapter brain_walk = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = brain_move_walk1;
                return true;
            }
        }

        static EntThinkAdapter brain_duck_down = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_duck_down";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_DUCKED) != 0)
                    return true;
                self.monsterinfo.aiflags |= Defines.AI_DUCKED;
                self.maxs[2] -= 32;
                self.takedamage = Defines.DAMAGE_YES;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        static EntThinkAdapter brain_duck_hold = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_duck_hold";
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

        static EntThinkAdapter brain_duck_up = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_duck_up";
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

        static EntDodgeAdapter brain_dodge = new AnonymousEntDodgeAdapter();
        private sealed class AnonymousEntDodgeAdapter : EntDodgeAdapter
		{
			
            public override string GetID()
            {
                return "brain_dodge";
            }

            public override void Dodge(edict_t self, edict_t attacker, float eta)
            {
                if (Lib.Random() > 0.25)
                    return;
                if (self.enemy == null)
                    self.enemy = attacker;
                self.monsterinfo.pausetime = GameBase.level.time + eta + 0.5F;
                self.monsterinfo.currentmove = brain_move_duck;
                return;
            }
        }

        static mframe_t[] brain_frames_death2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 9, null), new mframe_t(GameAI.ai_move, 0, null)};
        static EntThinkAdapter brain_dead = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_dead";
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

        static mmove_t brain_move_death2 = new mmove_t(FRAME_death201, FRAME_death205, brain_frames_death2, brain_dead);
        static mframe_t[] brain_frames_death1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 9, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t brain_move_death1 = new mmove_t(FRAME_death101, FRAME_death118, brain_frames_death1, brain_dead);
        static EntThinkAdapter brain_swing_right = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_swing_right";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_melee1, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter brain_hit_right = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_hit_right";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, self.maxs[0], 8);
                if (GameWeapon.Fire_hit(self, aim, (15 + (Lib.Rand() % 5)), 40))
                    GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_melee3, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter brain_swing_left = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_swing_left";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_melee2, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter brain_hit_left = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_hit_left";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, self.mins[0], 8);
                if (GameWeapon.Fire_hit(self, aim, (15 + (Lib.Rand() % 5)), 40))
                    GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_melee3, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter brain_chest_open = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_chest_open";
            }

            public override bool Think(edict_t self)
            {
                self.spawnflags &= ~65536;
                self.monsterinfo.power_armor_type = Defines.POWER_ARMOR_NONE;
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_chest_open, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter brain_tentacle_attack = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_tentacle_attack";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, 0, 8);
                if (GameWeapon.Fire_hit(self, aim, (10 + (Lib.Rand() % 5)), -600) && GameBase.skill.value > 0)
                    self.spawnflags |= 65536;
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_tentacles_retract, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] brain_frames_attack1 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 8, null), new mframe_t(GameAI.ai_charge, 3, null), new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -3, brain_swing_right), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -5, null), new mframe_t(GameAI.ai_charge, -7, brain_hit_right), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 6, brain_swing_left), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 2, brain_hit_left), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 6, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, -11, null)};
        static EntThinkAdapter brain_chest_closed = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_chest_closed";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.power_armor_type = Defines.POWER_ARMOR_SCREEN;
                if ((self.spawnflags & 65536) != 0)
                {
                    self.spawnflags &= ~65536;
                    self.monsterinfo.currentmove = brain_move_attack1;
                }

                return true;
            }
        }

        static mframe_t[] brain_frames_attack2 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, -4, null), new mframe_t(GameAI.ai_charge, -4, null), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 0, brain_chest_open), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 13, brain_tentacle_attack), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -9, brain_chest_closed), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, 3, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, -6, null)};
        static EntThinkAdapter brain_melee = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_melee";
            }

            public override bool Think(edict_t self)
            {
                if (Lib.Random() <= 0.5)
                    self.monsterinfo.currentmove = brain_move_attack1;
                else
                    self.monsterinfo.currentmove = brain_move_attack2;
                return true;
            }
        }

        static mframe_t[] brain_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 9, null), new mframe_t(GameAI.ai_run, 2, null), new mframe_t(GameAI.ai_run, 3, null), new mframe_t(GameAI.ai_run, 3, null), new mframe_t(GameAI.ai_run, 1, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, -4, null), new mframe_t(GameAI.ai_run, -1, null), new mframe_t(GameAI.ai_run, 2, null)};
        static mmove_t brain_move_run = new mmove_t(FRAME_walk101, FRAME_walk111, brain_frames_run, null);
        static EntThinkAdapter brain_run = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "brain_run";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.power_armor_type = Defines.POWER_ARMOR_SCREEN;
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = brain_move_stand;
                else
                    self.monsterinfo.currentmove = brain_move_run;
                return true;
            }
        }

        static mframe_t[] brain_frames_defense = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t brain_move_defense = new mmove_t(FRAME_defens01, FRAME_defens08, brain_frames_defense, null);
        static mframe_t[] brain_frames_pain3 = new mframe_t[]{new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -4, null)};
        static mmove_t brain_move_pain3 = new mmove_t(FRAME_pain301, FRAME_pain306, brain_frames_pain3, brain_run);
        static mframe_t[] brain_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, -2, null)};
        static mmove_t brain_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain208, brain_frames_pain2, brain_run);
        static mframe_t[] brain_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 7, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, -1, null)};
        static mmove_t brain_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain121, brain_frames_pain1, brain_run);
        static mframe_t[] brain_frames_duck = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -2, brain_duck_down), new mframe_t(GameAI.ai_move, 17, brain_duck_hold), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -1, brain_duck_up), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, -6, null)};
        static mmove_t brain_move_duck = new mmove_t(FRAME_duck01, FRAME_duck08, brain_frames_duck, brain_run);
        static EntPainAdapter brain_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "brain_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                float r;
                if (self.health < (self.max_health / 2))
                    self.s.skinnum = 1;
                if (GameBase.level.time < self.pain_debounce_time)
                    return;
                self.pain_debounce_time = GameBase.level.time + 3;
                if (GameBase.skill.value == 3)
                    return;
                r = Lib.Random();
                if (r < 0.33)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = brain_move_pain1;
                }
                else if (r < 0.66)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = brain_move_pain2;
                }
                else
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = brain_move_pain3;
                }
            }
        }

        static EntDieAdapter brain_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "brain_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                int n;
                self.s.effects = 0;
                self.monsterinfo.power_armor_type = Defines.POWER_ARMOR_NONE;
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
                if (Lib.Random() <= 0.5)
                    self.monsterinfo.currentmove = brain_move_death1;
                else
                    self.monsterinfo.currentmove = brain_move_death2;
            }
        }

        static mmove_t brain_move_attack1 = new mmove_t(FRAME_attak101, FRAME_attak118, brain_frames_attack1, brain_run);
        static mmove_t brain_move_attack2 = new mmove_t(FRAME_attak201, FRAME_attak217, brain_frames_attack2, brain_run);
        public static void SP_monster_brain(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_chest_open = GameBase.gi.Soundindex("brain/brnatck1.wav");
            sound_tentacles_extend = GameBase.gi.Soundindex("brain/brnatck2.wav");
            sound_tentacles_retract = GameBase.gi.Soundindex("brain/brnatck3.wav");
            sound_death = GameBase.gi.Soundindex("brain/brndeth1.wav");
            sound_idle1 = GameBase.gi.Soundindex("brain/brnidle1.wav");
            sound_idle2 = GameBase.gi.Soundindex("brain/brnidle2.wav");
            sound_idle3 = GameBase.gi.Soundindex("brain/brnlens1.wav");
            sound_pain1 = GameBase.gi.Soundindex("brain/brnpain1.wav");
            sound_pain2 = GameBase.gi.Soundindex("brain/brnpain2.wav");
            sound_sight = GameBase.gi.Soundindex("brain/brnsght1.wav");
            sound_search = GameBase.gi.Soundindex("brain/brnsrch1.wav");
            sound_melee1 = GameBase.gi.Soundindex("brain/melee1.wav");
            sound_melee2 = GameBase.gi.Soundindex("brain/melee2.wav");
            sound_melee3 = GameBase.gi.Soundindex("brain/melee3.wav");
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/brain/tris.md2");
            Math3D.VectorSet(self.mins, -16, -16, -24);
            Math3D.VectorSet(self.maxs, 16, 16, 32);
            self.health = 300;
            self.gib_health = -150;
            self.mass = 400;
            self.pain = brain_pain;
            self.die = brain_die;
            self.monsterinfo.stand = brain_stand;
            self.monsterinfo.walk = brain_walk;
            self.monsterinfo.run = brain_run;
            self.monsterinfo.dodge = brain_dodge;
            self.monsterinfo.melee = brain_melee;
            self.monsterinfo.sight = brain_sight;
            self.monsterinfo.search = brain_search;
            self.monsterinfo.idle = brain_idle;
            self.monsterinfo.power_armor_type = Defines.POWER_ARMOR_SCREEN;
            self.monsterinfo.power_armor_power = 100;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = brain_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.walkmonster_start.Think(self);
        }
    }
}
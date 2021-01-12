using Jake2.Client;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game.Monsters
{
    public class M_Infantry
    {
        public static readonly int FRAME_gun02 = 0;
        public static readonly int FRAME_stand01 = 1;
        public static readonly int FRAME_stand02 = 2;
        public static readonly int FRAME_stand03 = 3;
        public static readonly int FRAME_stand04 = 4;
        public static readonly int FRAME_stand05 = 5;
        public static readonly int FRAME_stand06 = 6;
        public static readonly int FRAME_stand07 = 7;
        public static readonly int FRAME_stand08 = 8;
        public static readonly int FRAME_stand09 = 9;
        public static readonly int FRAME_stand10 = 10;
        public static readonly int FRAME_stand11 = 11;
        public static readonly int FRAME_stand12 = 12;
        public static readonly int FRAME_stand13 = 13;
        public static readonly int FRAME_stand14 = 14;
        public static readonly int FRAME_stand15 = 15;
        public static readonly int FRAME_stand16 = 16;
        public static readonly int FRAME_stand17 = 17;
        public static readonly int FRAME_stand18 = 18;
        public static readonly int FRAME_stand19 = 19;
        public static readonly int FRAME_stand20 = 20;
        public static readonly int FRAME_stand21 = 21;
        public static readonly int FRAME_stand22 = 22;
        public static readonly int FRAME_stand23 = 23;
        public static readonly int FRAME_stand24 = 24;
        public static readonly int FRAME_stand25 = 25;
        public static readonly int FRAME_stand26 = 26;
        public static readonly int FRAME_stand27 = 27;
        public static readonly int FRAME_stand28 = 28;
        public static readonly int FRAME_stand29 = 29;
        public static readonly int FRAME_stand30 = 30;
        public static readonly int FRAME_stand31 = 31;
        public static readonly int FRAME_stand32 = 32;
        public static readonly int FRAME_stand33 = 33;
        public static readonly int FRAME_stand34 = 34;
        public static readonly int FRAME_stand35 = 35;
        public static readonly int FRAME_stand36 = 36;
        public static readonly int FRAME_stand37 = 37;
        public static readonly int FRAME_stand38 = 38;
        public static readonly int FRAME_stand39 = 39;
        public static readonly int FRAME_stand40 = 40;
        public static readonly int FRAME_stand41 = 41;
        public static readonly int FRAME_stand42 = 42;
        public static readonly int FRAME_stand43 = 43;
        public static readonly int FRAME_stand44 = 44;
        public static readonly int FRAME_stand45 = 45;
        public static readonly int FRAME_stand46 = 46;
        public static readonly int FRAME_stand47 = 47;
        public static readonly int FRAME_stand48 = 48;
        public static readonly int FRAME_stand49 = 49;
        public static readonly int FRAME_stand50 = 50;
        public static readonly int FRAME_stand51 = 51;
        public static readonly int FRAME_stand52 = 52;
        public static readonly int FRAME_stand53 = 53;
        public static readonly int FRAME_stand54 = 54;
        public static readonly int FRAME_stand55 = 55;
        public static readonly int FRAME_stand56 = 56;
        public static readonly int FRAME_stand57 = 57;
        public static readonly int FRAME_stand58 = 58;
        public static readonly int FRAME_stand59 = 59;
        public static readonly int FRAME_stand60 = 60;
        public static readonly int FRAME_stand61 = 61;
        public static readonly int FRAME_stand62 = 62;
        public static readonly int FRAME_stand63 = 63;
        public static readonly int FRAME_stand64 = 64;
        public static readonly int FRAME_stand65 = 65;
        public static readonly int FRAME_stand66 = 66;
        public static readonly int FRAME_stand67 = 67;
        public static readonly int FRAME_stand68 = 68;
        public static readonly int FRAME_stand69 = 69;
        public static readonly int FRAME_stand70 = 70;
        public static readonly int FRAME_stand71 = 71;
        public static readonly int FRAME_walk01 = 72;
        public static readonly int FRAME_walk02 = 73;
        public static readonly int FRAME_walk03 = 74;
        public static readonly int FRAME_walk04 = 75;
        public static readonly int FRAME_walk05 = 76;
        public static readonly int FRAME_walk06 = 77;
        public static readonly int FRAME_walk07 = 78;
        public static readonly int FRAME_walk08 = 79;
        public static readonly int FRAME_walk09 = 80;
        public static readonly int FRAME_walk10 = 81;
        public static readonly int FRAME_walk11 = 82;
        public static readonly int FRAME_walk12 = 83;
        public static readonly int FRAME_walk13 = 84;
        public static readonly int FRAME_walk14 = 85;
        public static readonly int FRAME_walk15 = 86;
        public static readonly int FRAME_walk16 = 87;
        public static readonly int FRAME_walk17 = 88;
        public static readonly int FRAME_walk18 = 89;
        public static readonly int FRAME_walk19 = 90;
        public static readonly int FRAME_walk20 = 91;
        public static readonly int FRAME_run01 = 92;
        public static readonly int FRAME_run02 = 93;
        public static readonly int FRAME_run03 = 94;
        public static readonly int FRAME_run04 = 95;
        public static readonly int FRAME_run05 = 96;
        public static readonly int FRAME_run06 = 97;
        public static readonly int FRAME_run07 = 98;
        public static readonly int FRAME_run08 = 99;
        public static readonly int FRAME_pain101 = 100;
        public static readonly int FRAME_pain102 = 101;
        public static readonly int FRAME_pain103 = 102;
        public static readonly int FRAME_pain104 = 103;
        public static readonly int FRAME_pain105 = 104;
        public static readonly int FRAME_pain106 = 105;
        public static readonly int FRAME_pain107 = 106;
        public static readonly int FRAME_pain108 = 107;
        public static readonly int FRAME_pain109 = 108;
        public static readonly int FRAME_pain110 = 109;
        public static readonly int FRAME_pain201 = 110;
        public static readonly int FRAME_pain202 = 111;
        public static readonly int FRAME_pain203 = 112;
        public static readonly int FRAME_pain204 = 113;
        public static readonly int FRAME_pain205 = 114;
        public static readonly int FRAME_pain206 = 115;
        public static readonly int FRAME_pain207 = 116;
        public static readonly int FRAME_pain208 = 117;
        public static readonly int FRAME_pain209 = 118;
        public static readonly int FRAME_pain210 = 119;
        public static readonly int FRAME_duck01 = 120;
        public static readonly int FRAME_duck02 = 121;
        public static readonly int FRAME_duck03 = 122;
        public static readonly int FRAME_duck04 = 123;
        public static readonly int FRAME_duck05 = 124;
        public static readonly int FRAME_death101 = 125;
        public static readonly int FRAME_death102 = 126;
        public static readonly int FRAME_death103 = 127;
        public static readonly int FRAME_death104 = 128;
        public static readonly int FRAME_death105 = 129;
        public static readonly int FRAME_death106 = 130;
        public static readonly int FRAME_death107 = 131;
        public static readonly int FRAME_death108 = 132;
        public static readonly int FRAME_death109 = 133;
        public static readonly int FRAME_death110 = 134;
        public static readonly int FRAME_death111 = 135;
        public static readonly int FRAME_death112 = 136;
        public static readonly int FRAME_death113 = 137;
        public static readonly int FRAME_death114 = 138;
        public static readonly int FRAME_death115 = 139;
        public static readonly int FRAME_death116 = 140;
        public static readonly int FRAME_death117 = 141;
        public static readonly int FRAME_death118 = 142;
        public static readonly int FRAME_death119 = 143;
        public static readonly int FRAME_death120 = 144;
        public static readonly int FRAME_death201 = 145;
        public static readonly int FRAME_death202 = 146;
        public static readonly int FRAME_death203 = 147;
        public static readonly int FRAME_death204 = 148;
        public static readonly int FRAME_death205 = 149;
        public static readonly int FRAME_death206 = 150;
        public static readonly int FRAME_death207 = 151;
        public static readonly int FRAME_death208 = 152;
        public static readonly int FRAME_death209 = 153;
        public static readonly int FRAME_death210 = 154;
        public static readonly int FRAME_death211 = 155;
        public static readonly int FRAME_death212 = 156;
        public static readonly int FRAME_death213 = 157;
        public static readonly int FRAME_death214 = 158;
        public static readonly int FRAME_death215 = 159;
        public static readonly int FRAME_death216 = 160;
        public static readonly int FRAME_death217 = 161;
        public static readonly int FRAME_death218 = 162;
        public static readonly int FRAME_death219 = 163;
        public static readonly int FRAME_death220 = 164;
        public static readonly int FRAME_death221 = 165;
        public static readonly int FRAME_death222 = 166;
        public static readonly int FRAME_death223 = 167;
        public static readonly int FRAME_death224 = 168;
        public static readonly int FRAME_death225 = 169;
        public static readonly int FRAME_death301 = 170;
        public static readonly int FRAME_death302 = 171;
        public static readonly int FRAME_death303 = 172;
        public static readonly int FRAME_death304 = 173;
        public static readonly int FRAME_death305 = 174;
        public static readonly int FRAME_death306 = 175;
        public static readonly int FRAME_death307 = 176;
        public static readonly int FRAME_death308 = 177;
        public static readonly int FRAME_death309 = 178;
        public static readonly int FRAME_block01 = 179;
        public static readonly int FRAME_block02 = 180;
        public static readonly int FRAME_block03 = 181;
        public static readonly int FRAME_block04 = 182;
        public static readonly int FRAME_block05 = 183;
        public static readonly int FRAME_attak101 = 184;
        public static readonly int FRAME_attak102 = 185;
        public static readonly int FRAME_attak103 = 186;
        public static readonly int FRAME_attak104 = 187;
        public static readonly int FRAME_attak105 = 188;
        public static readonly int FRAME_attak106 = 189;
        public static readonly int FRAME_attak107 = 190;
        public static readonly int FRAME_attak108 = 191;
        public static readonly int FRAME_attak109 = 192;
        public static readonly int FRAME_attak110 = 193;
        public static readonly int FRAME_attak111 = 194;
        public static readonly int FRAME_attak112 = 195;
        public static readonly int FRAME_attak113 = 196;
        public static readonly int FRAME_attak114 = 197;
        public static readonly int FRAME_attak115 = 198;
        public static readonly int FRAME_attak201 = 199;
        public static readonly int FRAME_attak202 = 200;
        public static readonly int FRAME_attak203 = 201;
        public static readonly int FRAME_attak204 = 202;
        public static readonly int FRAME_attak205 = 203;
        public static readonly int FRAME_attak206 = 204;
        public static readonly int FRAME_attak207 = 205;
        public static readonly int FRAME_attak208 = 206;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_die1;
        static int sound_die2;
        static int sound_gunshot;
        static int sound_weapon_cock;
        static int sound_punch_swing;
        static int sound_punch_hit;
        static int sound_sight;
        static int sound_search;
        static int sound_idle;
        static mframe_t[] infantry_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t infantry_move_stand = new mmove_t(FRAME_stand50, FRAME_stand71, infantry_frames_stand, null);
        public static EntThinkAdapter infantry_stand = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = infantry_move_stand;
                return true;
            }
        }

        static mframe_t[] infantry_frames_fidget = new mframe_t[]{new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, 3, null), new mframe_t(GameAI.ai_stand, 6, null), new mframe_t(GameAI.ai_stand, 3, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, -1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, -2, null), new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, -1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, -1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, -1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, -1, null), new mframe_t(GameAI.ai_stand, -1, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, -3, null), new mframe_t(GameAI.ai_stand, -2, null), new mframe_t(GameAI.ai_stand, -3, null), new mframe_t(GameAI.ai_stand, -3, null), new mframe_t(GameAI.ai_stand, -2, null)};
        static mmove_t infantry_move_fidget = new mmove_t(FRAME_stand01, FRAME_stand49, infantry_frames_fidget, infantry_stand);
        static EntThinkAdapter infantry_fidget = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_fidget";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = infantry_move_fidget;
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static mframe_t[] infantry_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 5, null)};
        static mmove_t infantry_move_walk = new mmove_t(FRAME_walk03, FRAME_walk14, infantry_frames_walk, null);
        static EntThinkAdapter infantry_walk = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = infantry_move_walk;
                return true;
            }
        }

        static mframe_t[] infantry_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 20, null), new mframe_t(GameAI.ai_run, 5, null), new mframe_t(GameAI.ai_run, 7, null), new mframe_t(GameAI.ai_run, 30, null), new mframe_t(GameAI.ai_run, 35, null), new mframe_t(GameAI.ai_run, 2, null), new mframe_t(GameAI.ai_run, 6, null)};
        static mmove_t infantry_move_run = new mmove_t(FRAME_run01, FRAME_run08, infantry_frames_run, null);
        static EntThinkAdapter infantry_run = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = infantry_move_stand;
                else
                    self.monsterinfo.currentmove = infantry_move_run;
                return true;
            }
        }

        static mframe_t[] infantry_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 6, null), new mframe_t(GameAI.ai_move, 2, null)};
        static mmove_t infantry_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain110, infantry_frames_pain1, infantry_run);
        static mframe_t[] infantry_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, 2, null)};
        static mmove_t infantry_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain210, infantry_frames_pain2, infantry_run);
        static EntPainAdapter infantry_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "infantry_pain";
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
                n = Lib.Rand() % 2;
                if (n == 0)
                {
                    self.monsterinfo.currentmove = infantry_move_pain1;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                }
                else
                {
                    self.monsterinfo.currentmove = infantry_move_pain2;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                }
            }
        }

        static float[][] aimangles = new float[][]{new[]{0F, 5F, 0F}, new[]{10F, 15F, 0F}, new[]{20F, 25F, 0F}, new[]{25F, 35F, 0F}, new[]{30F, 40F, 0F}, new[]{30F, 45F, 0F}, new[]{25F, 50F, 0F}, new[]{20F, 40F, 0F}, new[]{15F, 35F, 0F}, new[]{40F, 35F, 0F}, new[]{70F, 35F, 0F}, new[]{90F, 35F, 0F}};
        static EntThinkAdapter InfantryMachineGun = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "InfantryMachineGun";
            }

            public override bool Think(edict_t self)
            {
                float[] start = new float[]{0, 0, 0}, target = new float[]{0, 0, 0};
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] vec = new float[]{0, 0, 0};
                int flash_number;
                if (self.s.frame == FRAME_attak111)
                {
                    flash_number = Defines.MZ2_INFANTRY_MACHINEGUN_1;
                    Math3D.AngleVectors(self.s.angles, forward, right, null);
                    Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_number], forward, right, start);
                    if (self.enemy != null)
                    {
                        Math3D.VectorMA(self.enemy.s.origin, -0.2F, self.enemy.velocity, target);
                        target[2] += self.enemy.viewheight;
                        Math3D.VectorSubtract(target, start, forward);
                        Math3D.VectorNormalize(forward);
                    }
                    else
                    {
                        Math3D.AngleVectors(self.s.angles, forward, right, null);
                    }
                }
                else
                {
                    flash_number = Defines.MZ2_INFANTRY_MACHINEGUN_2 + (self.s.frame - FRAME_death211);
                    Math3D.AngleVectors(self.s.angles, forward, right, null);
                    Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_number], forward, right, start);
                    Math3D.VectorSubtract(self.s.angles, aimangles[flash_number - Defines.MZ2_INFANTRY_MACHINEGUN_2], vec);
                    Math3D.AngleVectors(vec, forward, null, null);
                }

                Monster.Monster_fire_bullet(self, start, forward, 3, 4, Defines.DEFAULT_BULLET_HSPREAD, Defines.DEFAULT_BULLET_VSPREAD, flash_number);
                return true;
            }
        }

        static EntInteractAdapter infantry_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "infantry_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter infantry_dead = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_dead";
            }

            public override bool Think(edict_t self)
            {
                Math3D.VectorSet(self.mins, -16, -16, -24);
                Math3D.VectorSet(self.maxs, 16, 16, -8);
                self.movetype = Defines.MOVETYPE_TOSS;
                self.svflags |= Defines.SVF_DEADMONSTER;
                GameBase.gi.Linkentity(self);
                M.M_FlyCheck.Think(self);
                return true;
            }
        }

        static mframe_t[] infantry_frames_death1 = new mframe_t[]{new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 9, null), new mframe_t(GameAI.ai_move, 9, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -3, null)};
        static mmove_t infantry_move_death1 = new mmove_t(FRAME_death101, FRAME_death120, infantry_frames_death1, infantry_dead);
        static mframe_t[] infantry_frames_death2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -2, InfantryMachineGun), new mframe_t(GameAI.ai_move, -2, InfantryMachineGun), new mframe_t(GameAI.ai_move, -3, InfantryMachineGun), new mframe_t(GameAI.ai_move, -1, InfantryMachineGun), new mframe_t(GameAI.ai_move, -2, InfantryMachineGun), new mframe_t(GameAI.ai_move, 0, InfantryMachineGun), new mframe_t(GameAI.ai_move, 2, InfantryMachineGun), new mframe_t(GameAI.ai_move, 2, InfantryMachineGun), new mframe_t(GameAI.ai_move, 3, InfantryMachineGun), new mframe_t(GameAI.ai_move, -10, InfantryMachineGun), new mframe_t(GameAI.ai_move, -7, InfantryMachineGun), new mframe_t(GameAI.ai_move, -8, InfantryMachineGun), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t infantry_move_death2 = new mmove_t(FRAME_death201, FRAME_death225, infantry_frames_death2, infantry_dead);
        static mframe_t[] infantry_frames_death3 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, -11, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -11, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t infantry_move_death3 = new mmove_t(FRAME_death301, FRAME_death309, infantry_frames_death3, infantry_dead);
        public static EntDieAdapter infantry_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "infantry_die";
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
                n = Lib.Rand() % 3;
                if (n == 0)
                {
                    self.monsterinfo.currentmove = infantry_move_death1;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_die2, 1, Defines.ATTN_NORM, 0);
                }
                else if (n == 1)
                {
                    self.monsterinfo.currentmove = infantry_move_death2;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_die1, 1, Defines.ATTN_NORM, 0);
                }
                else
                {
                    self.monsterinfo.currentmove = infantry_move_death3;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_die2, 1, Defines.ATTN_NORM, 0);
                }
            }
        }

        static EntThinkAdapter infantry_duck_down = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_duck_down";
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

        static EntThinkAdapter infantry_duck_hold = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_duck_hold";
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

        static EntThinkAdapter infantry_duck_up = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_duck_up";
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

        static mframe_t[] infantry_frames_duck = new mframe_t[]{new mframe_t(GameAI.ai_move, -2, infantry_duck_down), new mframe_t(GameAI.ai_move, -5, infantry_duck_hold), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 4, infantry_duck_up), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t infantry_move_duck = new mmove_t(FRAME_duck01, FRAME_duck05, infantry_frames_duck, infantry_run);
        static EntDodgeAdapter infantry_dodge = new AnonymousEntDodgeAdapter();
        private sealed class AnonymousEntDodgeAdapter : EntDodgeAdapter
		{
			
            public override string GetID()
            {
                return "infantry_dodge";
            }

            public override void Dodge(edict_t self, edict_t attacker, float eta)
            {
                if (Lib.Random() > 0.25)
                    return;
                if (null == self.enemy)
                    self.enemy = attacker;
                self.monsterinfo.currentmove = infantry_move_duck;
            }
        }

        static EntThinkAdapter infantry_cock_gun = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_cock_gun";
            }

            public override bool Think(edict_t self)
            {
                int n;
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_weapon_cock, 1, Defines.ATTN_NORM, 0);
                n = (Lib.Rand() & 15) + 3 + 7;
                self.monsterinfo.pausetime = GameBase.level.time + n * Defines.FRAMETIME;
                return true;
            }
        }

        static EntThinkAdapter infantry_fire = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_fire";
            }

            public override bool Think(edict_t self)
            {
                InfantryMachineGun.Think(self);
                if (GameBase.level.time >= self.monsterinfo.pausetime)
                    self.monsterinfo.aiflags &= ~Defines.AI_HOLD_FRAME;
                else
                    self.monsterinfo.aiflags |= Defines.AI_HOLD_FRAME;
                return true;
            }
        }

        static mframe_t[] infantry_frames_attack1 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, 0, infantry_cock_gun), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, -2, null), new mframe_t(GameAI.ai_charge, -3, null), new mframe_t(GameAI.ai_charge, 1, infantry_fire), new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, -1, null), new mframe_t(GameAI.ai_charge, -2, null), new mframe_t(GameAI.ai_charge, -3, null)};
        static mmove_t infantry_move_attack1 = new mmove_t(FRAME_attak101, FRAME_attak115, infantry_frames_attack1, infantry_run);
        static EntThinkAdapter infantry_swing = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_swing";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_punch_swing, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter infantry_smack = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_smack";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, 0, 0);
                if (GameWeapon.Fire_hit(self, aim, (5 + (Lib.Rand() % 5)), 50))
                    GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_punch_hit, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] infantry_frames_attack2 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 3, null), new mframe_t(GameAI.ai_charge, 6, null), new mframe_t(GameAI.ai_charge, 0, infantry_swing), new mframe_t(GameAI.ai_charge, 8, null), new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, 8, infantry_smack), new mframe_t(GameAI.ai_charge, 6, null), new mframe_t(GameAI.ai_charge, 3, null)};
        static mmove_t infantry_move_attack2 = new mmove_t(FRAME_attak201, FRAME_attak208, infantry_frames_attack2, infantry_run);
        static EntThinkAdapter infantry_attack = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "infantry_attack";
            }

            public override bool Think(edict_t self)
            {
                if (GameUtil.Range(self, self.enemy) == Defines.RANGE_MELEE)
                    self.monsterinfo.currentmove = infantry_move_attack2;
                else
                    self.monsterinfo.currentmove = infantry_move_attack1;
                return true;
            }
        }

        public static void SP_monster_infantry(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_pain1 = GameBase.gi.Soundindex("infantry/infpain1.wav");
            sound_pain2 = GameBase.gi.Soundindex("infantry/infpain2.wav");
            sound_die1 = GameBase.gi.Soundindex("infantry/infdeth1.wav");
            sound_die2 = GameBase.gi.Soundindex("infantry/infdeth2.wav");
            sound_gunshot = GameBase.gi.Soundindex("infantry/infatck1.wav");
            sound_weapon_cock = GameBase.gi.Soundindex("infantry/infatck3.wav");
            sound_punch_swing = GameBase.gi.Soundindex("infantry/infatck2.wav");
            sound_punch_hit = GameBase.gi.Soundindex("infantry/melee2.wav");
            sound_sight = GameBase.gi.Soundindex("infantry/infsght1.wav");
            sound_search = GameBase.gi.Soundindex("infantry/infsrch1.wav");
            sound_idle = GameBase.gi.Soundindex("infantry/infidle1.wav");
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/infantry/tris.md2");
            Math3D.VectorSet(self.mins, -16, -16, -24);
            Math3D.VectorSet(self.maxs, 16, 16, 32);
            self.health = 100;
            self.gib_health = -40;
            self.mass = 200;
            self.pain = infantry_pain;
            self.die = infantry_die;
            self.monsterinfo.stand = infantry_stand;
            self.monsterinfo.walk = infantry_walk;
            self.monsterinfo.run = infantry_run;
            self.monsterinfo.dodge = infantry_dodge;
            self.monsterinfo.attack = infantry_attack;
            self.monsterinfo.melee = null;
            self.monsterinfo.sight = infantry_sight;
            self.monsterinfo.idle = infantry_fidget;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = infantry_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.walkmonster_start.Think(self);
        }
    }
}
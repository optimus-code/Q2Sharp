using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game.Monsters
{
    public class M_Boss31
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
        public static readonly int FRAME_attak201 = 18;
        public static readonly int FRAME_attak202 = 19;
        public static readonly int FRAME_attak203 = 20;
        public static readonly int FRAME_attak204 = 21;
        public static readonly int FRAME_attak205 = 22;
        public static readonly int FRAME_attak206 = 23;
        public static readonly int FRAME_attak207 = 24;
        public static readonly int FRAME_attak208 = 25;
        public static readonly int FRAME_attak209 = 26;
        public static readonly int FRAME_attak210 = 27;
        public static readonly int FRAME_attak211 = 28;
        public static readonly int FRAME_attak212 = 29;
        public static readonly int FRAME_attak213 = 30;
        public static readonly int FRAME_death01 = 31;
        public static readonly int FRAME_death02 = 32;
        public static readonly int FRAME_death03 = 33;
        public static readonly int FRAME_death04 = 34;
        public static readonly int FRAME_death05 = 35;
        public static readonly int FRAME_death06 = 36;
        public static readonly int FRAME_death07 = 37;
        public static readonly int FRAME_death08 = 38;
        public static readonly int FRAME_death09 = 39;
        public static readonly int FRAME_death10 = 40;
        public static readonly int FRAME_death11 = 41;
        public static readonly int FRAME_death12 = 42;
        public static readonly int FRAME_death13 = 43;
        public static readonly int FRAME_death14 = 44;
        public static readonly int FRAME_death15 = 45;
        public static readonly int FRAME_death16 = 46;
        public static readonly int FRAME_death17 = 47;
        public static readonly int FRAME_death18 = 48;
        public static readonly int FRAME_death19 = 49;
        public static readonly int FRAME_death20 = 50;
        public static readonly int FRAME_death21 = 51;
        public static readonly int FRAME_death22 = 52;
        public static readonly int FRAME_death23 = 53;
        public static readonly int FRAME_death24 = 54;
        public static readonly int FRAME_death25 = 55;
        public static readonly int FRAME_death26 = 56;
        public static readonly int FRAME_death27 = 57;
        public static readonly int FRAME_death28 = 58;
        public static readonly int FRAME_death29 = 59;
        public static readonly int FRAME_death30 = 60;
        public static readonly int FRAME_death31 = 61;
        public static readonly int FRAME_death32 = 62;
        public static readonly int FRAME_death33 = 63;
        public static readonly int FRAME_death34 = 64;
        public static readonly int FRAME_death35 = 65;
        public static readonly int FRAME_death36 = 66;
        public static readonly int FRAME_death37 = 67;
        public static readonly int FRAME_death38 = 68;
        public static readonly int FRAME_death39 = 69;
        public static readonly int FRAME_death40 = 70;
        public static readonly int FRAME_death41 = 71;
        public static readonly int FRAME_death42 = 72;
        public static readonly int FRAME_death43 = 73;
        public static readonly int FRAME_death44 = 74;
        public static readonly int FRAME_death45 = 75;
        public static readonly int FRAME_death46 = 76;
        public static readonly int FRAME_death47 = 77;
        public static readonly int FRAME_death48 = 78;
        public static readonly int FRAME_death49 = 79;
        public static readonly int FRAME_death50 = 80;
        public static readonly int FRAME_pain101 = 81;
        public static readonly int FRAME_pain102 = 82;
        public static readonly int FRAME_pain103 = 83;
        public static readonly int FRAME_pain201 = 84;
        public static readonly int FRAME_pain202 = 85;
        public static readonly int FRAME_pain203 = 86;
        public static readonly int FRAME_pain301 = 87;
        public static readonly int FRAME_pain302 = 88;
        public static readonly int FRAME_pain303 = 89;
        public static readonly int FRAME_pain304 = 90;
        public static readonly int FRAME_pain305 = 91;
        public static readonly int FRAME_pain306 = 92;
        public static readonly int FRAME_pain307 = 93;
        public static readonly int FRAME_pain308 = 94;
        public static readonly int FRAME_pain309 = 95;
        public static readonly int FRAME_pain310 = 96;
        public static readonly int FRAME_pain311 = 97;
        public static readonly int FRAME_pain312 = 98;
        public static readonly int FRAME_pain313 = 99;
        public static readonly int FRAME_pain314 = 100;
        public static readonly int FRAME_pain315 = 101;
        public static readonly int FRAME_pain316 = 102;
        public static readonly int FRAME_pain317 = 103;
        public static readonly int FRAME_pain318 = 104;
        public static readonly int FRAME_pain319 = 105;
        public static readonly int FRAME_pain320 = 106;
        public static readonly int FRAME_pain321 = 107;
        public static readonly int FRAME_pain322 = 108;
        public static readonly int FRAME_pain323 = 109;
        public static readonly int FRAME_pain324 = 110;
        public static readonly int FRAME_pain325 = 111;
        public static readonly int FRAME_stand01 = 112;
        public static readonly int FRAME_stand02 = 113;
        public static readonly int FRAME_stand03 = 114;
        public static readonly int FRAME_stand04 = 115;
        public static readonly int FRAME_stand05 = 116;
        public static readonly int FRAME_stand06 = 117;
        public static readonly int FRAME_stand07 = 118;
        public static readonly int FRAME_stand08 = 119;
        public static readonly int FRAME_stand09 = 120;
        public static readonly int FRAME_stand10 = 121;
        public static readonly int FRAME_stand11 = 122;
        public static readonly int FRAME_stand12 = 123;
        public static readonly int FRAME_stand13 = 124;
        public static readonly int FRAME_stand14 = 125;
        public static readonly int FRAME_stand15 = 126;
        public static readonly int FRAME_stand16 = 127;
        public static readonly int FRAME_stand17 = 128;
        public static readonly int FRAME_stand18 = 129;
        public static readonly int FRAME_stand19 = 130;
        public static readonly int FRAME_stand20 = 131;
        public static readonly int FRAME_stand21 = 132;
        public static readonly int FRAME_stand22 = 133;
        public static readonly int FRAME_stand23 = 134;
        public static readonly int FRAME_stand24 = 135;
        public static readonly int FRAME_stand25 = 136;
        public static readonly int FRAME_stand26 = 137;
        public static readonly int FRAME_stand27 = 138;
        public static readonly int FRAME_stand28 = 139;
        public static readonly int FRAME_stand29 = 140;
        public static readonly int FRAME_stand30 = 141;
        public static readonly int FRAME_stand31 = 142;
        public static readonly int FRAME_stand32 = 143;
        public static readonly int FRAME_stand33 = 144;
        public static readonly int FRAME_stand34 = 145;
        public static readonly int FRAME_stand35 = 146;
        public static readonly int FRAME_stand36 = 147;
        public static readonly int FRAME_stand37 = 148;
        public static readonly int FRAME_stand38 = 149;
        public static readonly int FRAME_stand39 = 150;
        public static readonly int FRAME_stand40 = 151;
        public static readonly int FRAME_stand41 = 152;
        public static readonly int FRAME_stand42 = 153;
        public static readonly int FRAME_stand43 = 154;
        public static readonly int FRAME_stand44 = 155;
        public static readonly int FRAME_stand45 = 156;
        public static readonly int FRAME_stand46 = 157;
        public static readonly int FRAME_stand47 = 158;
        public static readonly int FRAME_stand48 = 159;
        public static readonly int FRAME_stand49 = 160;
        public static readonly int FRAME_stand50 = 161;
        public static readonly int FRAME_stand51 = 162;
        public static readonly int FRAME_walk01 = 163;
        public static readonly int FRAME_walk02 = 164;
        public static readonly int FRAME_walk03 = 165;
        public static readonly int FRAME_walk04 = 166;
        public static readonly int FRAME_walk05 = 167;
        public static readonly int FRAME_walk06 = 168;
        public static readonly int FRAME_walk07 = 169;
        public static readonly int FRAME_walk08 = 170;
        public static readonly int FRAME_walk09 = 171;
        public static readonly int FRAME_walk10 = 172;
        public static readonly int FRAME_walk11 = 173;
        public static readonly int FRAME_walk12 = 174;
        public static readonly int FRAME_walk13 = 175;
        public static readonly int FRAME_walk14 = 176;
        public static readonly int FRAME_walk15 = 177;
        public static readonly int FRAME_walk16 = 178;
        public static readonly int FRAME_walk17 = 179;
        public static readonly int FRAME_walk18 = 180;
        public static readonly int FRAME_walk19 = 181;
        public static readonly int FRAME_walk20 = 182;
        public static readonly int FRAME_walk21 = 183;
        public static readonly int FRAME_walk22 = 184;
        public static readonly int FRAME_walk23 = 185;
        public static readonly int FRAME_walk24 = 186;
        public static readonly int FRAME_walk25 = 187;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_pain3;
        static int sound_idle;
        static int sound_death;
        static int sound_search1;
        static int sound_search2;
        static int sound_search3;
        static int sound_attack1;
        static int sound_attack2;
        static int sound_firegun;
        static int sound_step_left;
        static int sound_step_right;
        static int sound_death_hit;
        static EntThinkAdapter jorg_search = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_search";
            }

            public override bool Think(edict_t self)
            {
                float r;
                r = Lib.Random();
                if (r <= 0.3)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search1, 1, Defines.ATTN_NORM, 0);
                else if (r <= 0.6)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search2, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search3, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter jorg_idle = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_idle";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter jorg_death_hit = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_death_hit";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_death_hit, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter jorg_step_left = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_step_left";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_step_left, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter jorg_step_right = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_step_right";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_BODY, sound_step_right, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter jorg_stand = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = jorg_move_stand;
                return true;
            }
        }

        static EntThinkAdapter jorg_reattack1 = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_reattack1";
            }

            public override bool Think(edict_t self)
            {
                if (GameUtil.Visible(self, self.enemy))
                    if (Lib.Random() < 0.9)
                        self.monsterinfo.currentmove = jorg_move_attack1;
                    else
                    {
                        self.s.sound = 0;
                        self.monsterinfo.currentmove = jorg_move_end_attack1;
                    }
                else
                {
                    self.s.sound = 0;
                    self.monsterinfo.currentmove = jorg_move_end_attack1;
                }

                return true;
            }
        }

        static EntThinkAdapter jorg_attack1 = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_attack1";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = jorg_move_attack1;
                return true;
            }
        }

        static EntPainAdapter jorg_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "jorg_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                if (self.health < (self.max_health / 2))
                    self.s.skinnum = 1;
                self.s.sound = 0;
                if (GameBase.level.time < self.pain_debounce_time)
                    return;
                if (damage <= 40)
                    if (Lib.Random() <= 0.6)
                        return;
                if ((self.s.frame >= FRAME_attak101) && (self.s.frame <= FRAME_attak108))
                    if (Lib.Random() <= 0.005)
                        return;
                if ((self.s.frame >= FRAME_attak109) && (self.s.frame <= FRAME_attak114))
                    if (Lib.Random() <= 5E-05)
                        return;
                if ((self.s.frame >= FRAME_attak201) && (self.s.frame <= FRAME_attak208))
                    if (Lib.Random() <= 0.005)
                        return;
                self.pain_debounce_time = GameBase.level.time + 3;
                if (GameBase.skill.value == 3)
                    return;
                if (damage <= 50)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = jorg_move_pain1;
                }
                else if (damage <= 100)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = jorg_move_pain2;
                }
                else
                {
                    if (Lib.Random() <= 0.3)
                    {
                        GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain3, 1, Defines.ATTN_NORM, 0);
                        self.monsterinfo.currentmove = jorg_move_pain3;
                    }
                }
            }
        }

        static EntThinkAdapter jorgBFG = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorgBFG";
            }

            public override bool Think(edict_t self)
            {
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] start = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                float[] vec = new float[]{0, 0, 0};
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[Defines.MZ2_JORG_BFG_1], forward, right, start);
                Math3D.VectorCopy(self.enemy.s.origin, vec);
                vec[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(vec, start, dir);
                Math3D.VectorNormalize(dir);
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_attack2, 1, Defines.ATTN_NORM, 0);
                Monster.Monster_fire_bfg(self, start, dir, 50, 300, 100, 200, Defines.MZ2_JORG_BFG_1);
                return true;
            }
        }

        static EntThinkAdapter jorg_firebullet_right = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_firebullet_right";
            }

            public override bool Think(edict_t self)
            {
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0}, target = new float[]{0, 0, 0};
                float[] start = new float[]{0, 0, 0};
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[Defines.MZ2_JORG_MACHINEGUN_R1], forward, right, start);
                Math3D.VectorMA(self.enemy.s.origin, -0.2F, self.enemy.velocity, target);
                target[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(target, start, forward);
                Math3D.VectorNormalize(forward);
                Monster.Monster_fire_bullet(self, start, forward, 6, 4, Defines.DEFAULT_BULLET_HSPREAD, Defines.DEFAULT_BULLET_VSPREAD, Defines.MZ2_JORG_MACHINEGUN_R1);
                return true;
            }
        }

        static EntThinkAdapter jorg_firebullet_left = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_firebullet_left";
            }

            public override bool Think(edict_t self)
            {
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0}, target = new float[]{0, 0, 0};
                float[] start = new float[]{0, 0, 0};
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[Defines.MZ2_JORG_MACHINEGUN_L1], forward, right, start);
                Math3D.VectorMA(self.enemy.s.origin, -0.2F, self.enemy.velocity, target);
                target[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(target, start, forward);
                Math3D.VectorNormalize(forward);
                Monster.Monster_fire_bullet(self, start, forward, 6, 4, Defines.DEFAULT_BULLET_HSPREAD, Defines.DEFAULT_BULLET_VSPREAD, Defines.MZ2_JORG_MACHINEGUN_L1);
                return true;
            }
        }

        static EntThinkAdapter jorg_firebullet = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_firebullet";
            }

            public override bool Think(edict_t self)
            {
                jorg_firebullet_left.Think(self);
                jorg_firebullet_right.Think(self);
                return true;
            }
        }

        static EntThinkAdapter jorg_attack = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_attack";
            }

            public override bool Think(edict_t self)
            {
                float[] vec = new float[]{0, 0, 0};
                float range = 0;
                Math3D.VectorSubtract(self.enemy.s.origin, self.s.origin, vec);
                range = Math3D.VectorLength(vec);
                if (Lib.Random() <= 0.75)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_attack1, 1, Defines.ATTN_NORM, 0);
                    self.s.sound = GameBase.gi.Soundindex("boss3/w_loop.wav");
                    self.monsterinfo.currentmove = jorg_move_start_attack1;
                }
                else
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_attack2, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = jorg_move_attack2;
                }

                return true;
            }
        }

        static EntThinkAdapter jorg_dead = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_dead";
            }

            public override bool Think(edict_t self)
            {
                return true;
            }
        }

        static EntDieAdapter jorg_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "jorg_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death, 1, Defines.ATTN_NORM, 0);
                self.deadflag = Defines.DEAD_DEAD;
                self.takedamage = Defines.DAMAGE_NO;
                self.s.sound = 0;
                self.count = 0;
                self.monsterinfo.currentmove = jorg_move_death;
                return;
            }
        }

        static EntThinkAdapter Jorg_CheckAttack = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "Jorg_CheckAttack";
            }

            public override bool Think(edict_t self)
            {
                float[] spot1 = new float[]{0, 0, 0}, spot2 = new float[]{0, 0, 0};
                float[] temp = new float[]{0, 0, 0};
                float chance;
                trace_t tr;
                int enemy_range;
                float enemy_yaw;
                if (self.enemy.health > 0)
                {
                    Math3D.VectorCopy(self.s.origin, spot1);
                    spot1[2] += self.viewheight;
                    Math3D.VectorCopy(self.enemy.s.origin, spot2);
                    spot2[2] += self.enemy.viewheight;
                    tr = GameBase.gi.Trace(spot1, null, null, spot2, self, Defines.CONTENTS_SOLID | Defines.CONTENTS_MONSTER | Defines.CONTENTS_SLIME | Defines.CONTENTS_LAVA);
                    if (tr.ent != self.enemy)
                        return false;
                }

                enemy_range = GameUtil.Range(self, self.enemy);
                Math3D.VectorSubtract(self.enemy.s.origin, self.s.origin, temp);
                enemy_yaw = Math3D.Vectoyaw(temp);
                self.ideal_yaw = enemy_yaw;
                if (enemy_range == Defines.RANGE_MELEE)
                {
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
                if (enemy_range == Defines.RANGE_FAR)
                    return false;
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                {
                    chance = 0.4F;
                }
                else if (enemy_range == Defines.RANGE_MELEE)
                {
                    chance = 0.8F;
                }
                else if (enemy_range == Defines.RANGE_NEAR)
                {
                    chance = 0.4F;
                }
                else if (enemy_range == Defines.RANGE_MID)
                {
                    chance = 0.2F;
                }
                else
                {
                    return false;
                }

                if (Lib.Random() < chance)
                {
                    self.monsterinfo.attack_state = Defines.AS_MISSILE;
                    self.monsterinfo.attack_finished = GameBase.level.time + 2 * Lib.Random();
                    return true;
                }

                if ((self.flags & Defines.FL_FLY) != 0)
                {
                    if (Lib.Random() < 0.3)
                        self.monsterinfo.attack_state = Defines.AS_SLIDING;
                    else
                        self.monsterinfo.attack_state = Defines.AS_STRAIGHT;
                }

                return false;
            }
        }

        static mframe_t[] jorg_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, jorg_idle), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 19, null), new mframe_t(GameAI.ai_stand, 11, jorg_step_left), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 6, null), new mframe_t(GameAI.ai_stand, 9, jorg_step_right), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, -2, null), new mframe_t(GameAI.ai_stand, -17, jorg_step_left), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, -12, null), new mframe_t(GameAI.ai_stand, -14, jorg_step_right)};
        static mmove_t jorg_move_stand = new mmove_t(FRAME_stand01, FRAME_stand51, jorg_frames_stand, null);
        static mframe_t[] jorg_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 17, jorg_step_left), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 12, null), new mframe_t(GameAI.ai_run, 8, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 33, jorg_step_right), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 0, null), new mframe_t(GameAI.ai_run, 9, null), new mframe_t(GameAI.ai_run, 9, null), new mframe_t(GameAI.ai_run, 9, null)};
        static mmove_t jorg_move_run = new mmove_t(FRAME_walk06, FRAME_walk19, jorg_frames_run, null);
        static mframe_t[] jorg_frames_start_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 9, null), new mframe_t(GameAI.ai_walk, 15, null)};
        static mmove_t jorg_move_start_walk = new mmove_t(FRAME_walk01, FRAME_walk05, jorg_frames_start_walk, null);
        static mframe_t[] jorg_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 17, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 12, null), new mframe_t(GameAI.ai_walk, 8, null), new mframe_t(GameAI.ai_walk, 10, null), new mframe_t(GameAI.ai_walk, 33, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 9, null), new mframe_t(GameAI.ai_walk, 9, null), new mframe_t(GameAI.ai_walk, 9, null)};
        static mmove_t jorg_move_walk = new mmove_t(FRAME_walk06, FRAME_walk19, jorg_frames_walk, null);
        static mframe_t[] jorg_frames_end_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 11, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 8, null), new mframe_t(GameAI.ai_walk, -8, null)};
        static mmove_t jorg_move_end_walk = new mmove_t(FRAME_walk20, FRAME_walk25, jorg_frames_end_walk, null);
        static EntThinkAdapter jorg_walk = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = jorg_move_walk;
                return true;
            }
        }

        static EntThinkAdapter jorg_run = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "jorg_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = jorg_move_stand;
                else
                    self.monsterinfo.currentmove = jorg_move_run;
                return true;
            }
        }

        static mframe_t[] jorg_frames_pain3 = new mframe_t[] { new mframe_t(GameAI.ai_move, -28, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, -3, jorg_step_left), new mframe_t(GameAI.ai_move, -9, null), new mframe_t(GameAI.ai_move, 0, jorg_step_right), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -7, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, -11, null), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 10, null), new mframe_t(GameAI.ai_move, 11, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 10, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 10, null), new mframe_t(GameAI.ai_move, 7, jorg_step_left), new mframe_t(GameAI.ai_move, 17, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, jorg_step_right)};
        static mmove_t jorg_move_pain3 = new mmove_t(FRAME_pain301, FRAME_pain325, jorg_frames_pain3, jorg_run);
        static mframe_t[] jorg_frames_pain2 = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t jorg_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain203, jorg_frames_pain2, jorg_run);
        static mframe_t[] jorg_frames_pain1 = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t jorg_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain103, jorg_frames_pain1, jorg_run);
        static mframe_t[] jorg_frames_death1 = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, M_Boss32.MakronToss), new mframe_t(GameAI.ai_move, 0, M_Supertank.BossExplode)};
        static mmove_t jorg_move_death = new mmove_t(FRAME_death01, FRAME_death50, jorg_frames_death1, jorg_dead);
        static mframe_t[] jorg_frames_attack2 = new mframe_t[] { new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, jorgBFG), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t jorg_move_attack2 = new mmove_t(FRAME_attak201, FRAME_attak213, jorg_frames_attack2, jorg_run);
        static mframe_t[] jorg_frames_start_attack1 = new mframe_t[] { new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t jorg_move_start_attack1 = new mmove_t(FRAME_attak101, FRAME_attak108, jorg_frames_start_attack1, jorg_attack1);
        static mframe_t[] jorg_frames_attack1 = new mframe_t[] { new mframe_t(GameAI.ai_charge, 0, jorg_firebullet), new mframe_t(GameAI.ai_charge, 0, jorg_firebullet), new mframe_t(GameAI.ai_charge, 0, jorg_firebullet), new mframe_t(GameAI.ai_charge, 0, jorg_firebullet), new mframe_t(GameAI.ai_charge, 0, jorg_firebullet), new mframe_t(GameAI.ai_charge, 0, jorg_firebullet)};
        static mmove_t jorg_move_attack1 = new mmove_t(FRAME_attak109, FRAME_attak114, jorg_frames_attack1, jorg_reattack1);
        static mframe_t[] jorg_frames_end_attack1 = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t jorg_move_end_attack1 = new mmove_t(FRAME_attak115, FRAME_attak118, jorg_frames_end_attack1, jorg_run);
        public static void SP_monster_jorg(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_pain1 = GameBase.gi.Soundindex("boss3/bs3pain1.wav");
            sound_pain2 = GameBase.gi.Soundindex("boss3/bs3pain2.wav");
            sound_pain3 = GameBase.gi.Soundindex("boss3/bs3pain3.wav");
            sound_death = GameBase.gi.Soundindex("boss3/bs3deth1.wav");
            sound_attack1 = GameBase.gi.Soundindex("boss3/bs3atck1.wav");
            sound_attack2 = GameBase.gi.Soundindex("boss3/bs3atck2.wav");
            sound_search1 = GameBase.gi.Soundindex("boss3/bs3srch1.wav");
            sound_search2 = GameBase.gi.Soundindex("boss3/bs3srch2.wav");
            sound_search3 = GameBase.gi.Soundindex("boss3/bs3srch3.wav");
            sound_idle = GameBase.gi.Soundindex("boss3/bs3idle1.wav");
            sound_step_left = GameBase.gi.Soundindex("boss3/step1.wav");
            sound_step_right = GameBase.gi.Soundindex("boss3/step2.wav");
            sound_firegun = GameBase.gi.Soundindex("boss3/xfire.wav");
            sound_death_hit = GameBase.gi.Soundindex("boss3/d_hit.wav");
            M_Boss32.MakronPrecache();
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/boss3/rider/tris.md2");
            self.s.modelindex2 = GameBase.gi.Modelindex("models/monsters/boss3/jorg/tris.md2");
            Math3D.VectorSet(self.mins, -80, -80, 0);
            Math3D.VectorSet(self.maxs, 80, 80, 140);
            self.health = 3000;
            self.gib_health = -2000;
            self.mass = 1000;
            self.pain = jorg_pain;
            self.die = jorg_die;
            self.monsterinfo.stand = jorg_stand;
            self.monsterinfo.walk = jorg_walk;
            self.monsterinfo.run = jorg_run;
            self.monsterinfo.dodge = null;
            self.monsterinfo.attack = jorg_attack;
            self.monsterinfo.search = jorg_search;
            self.monsterinfo.melee = null;
            self.monsterinfo.sight = null;
            self.monsterinfo.checkattack = Jorg_CheckAttack;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = jorg_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.walkmonster_start.Think(self);
        }
    }
}
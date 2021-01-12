using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Hover
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
        public static readonly int FRAME_forwrd01 = 30;
        public static readonly int FRAME_forwrd02 = 31;
        public static readonly int FRAME_forwrd03 = 32;
        public static readonly int FRAME_forwrd04 = 33;
        public static readonly int FRAME_forwrd05 = 34;
        public static readonly int FRAME_forwrd06 = 35;
        public static readonly int FRAME_forwrd07 = 36;
        public static readonly int FRAME_forwrd08 = 37;
        public static readonly int FRAME_forwrd09 = 38;
        public static readonly int FRAME_forwrd10 = 39;
        public static readonly int FRAME_forwrd11 = 40;
        public static readonly int FRAME_forwrd12 = 41;
        public static readonly int FRAME_forwrd13 = 42;
        public static readonly int FRAME_forwrd14 = 43;
        public static readonly int FRAME_forwrd15 = 44;
        public static readonly int FRAME_forwrd16 = 45;
        public static readonly int FRAME_forwrd17 = 46;
        public static readonly int FRAME_forwrd18 = 47;
        public static readonly int FRAME_forwrd19 = 48;
        public static readonly int FRAME_forwrd20 = 49;
        public static readonly int FRAME_forwrd21 = 50;
        public static readonly int FRAME_forwrd22 = 51;
        public static readonly int FRAME_forwrd23 = 52;
        public static readonly int FRAME_forwrd24 = 53;
        public static readonly int FRAME_forwrd25 = 54;
        public static readonly int FRAME_forwrd26 = 55;
        public static readonly int FRAME_forwrd27 = 56;
        public static readonly int FRAME_forwrd28 = 57;
        public static readonly int FRAME_forwrd29 = 58;
        public static readonly int FRAME_forwrd30 = 59;
        public static readonly int FRAME_forwrd31 = 60;
        public static readonly int FRAME_forwrd32 = 61;
        public static readonly int FRAME_forwrd33 = 62;
        public static readonly int FRAME_forwrd34 = 63;
        public static readonly int FRAME_forwrd35 = 64;
        public static readonly int FRAME_stop101 = 65;
        public static readonly int FRAME_stop102 = 66;
        public static readonly int FRAME_stop103 = 67;
        public static readonly int FRAME_stop104 = 68;
        public static readonly int FRAME_stop105 = 69;
        public static readonly int FRAME_stop106 = 70;
        public static readonly int FRAME_stop107 = 71;
        public static readonly int FRAME_stop108 = 72;
        public static readonly int FRAME_stop109 = 73;
        public static readonly int FRAME_stop201 = 74;
        public static readonly int FRAME_stop202 = 75;
        public static readonly int FRAME_stop203 = 76;
        public static readonly int FRAME_stop204 = 77;
        public static readonly int FRAME_stop205 = 78;
        public static readonly int FRAME_stop206 = 79;
        public static readonly int FRAME_stop207 = 80;
        public static readonly int FRAME_stop208 = 81;
        public static readonly int FRAME_takeof01 = 82;
        public static readonly int FRAME_takeof02 = 83;
        public static readonly int FRAME_takeof03 = 84;
        public static readonly int FRAME_takeof04 = 85;
        public static readonly int FRAME_takeof05 = 86;
        public static readonly int FRAME_takeof06 = 87;
        public static readonly int FRAME_takeof07 = 88;
        public static readonly int FRAME_takeof08 = 89;
        public static readonly int FRAME_takeof09 = 90;
        public static readonly int FRAME_takeof10 = 91;
        public static readonly int FRAME_takeof11 = 92;
        public static readonly int FRAME_takeof12 = 93;
        public static readonly int FRAME_takeof13 = 94;
        public static readonly int FRAME_takeof14 = 95;
        public static readonly int FRAME_takeof15 = 96;
        public static readonly int FRAME_takeof16 = 97;
        public static readonly int FRAME_takeof17 = 98;
        public static readonly int FRAME_takeof18 = 99;
        public static readonly int FRAME_takeof19 = 100;
        public static readonly int FRAME_takeof20 = 101;
        public static readonly int FRAME_takeof21 = 102;
        public static readonly int FRAME_takeof22 = 103;
        public static readonly int FRAME_takeof23 = 104;
        public static readonly int FRAME_takeof24 = 105;
        public static readonly int FRAME_takeof25 = 106;
        public static readonly int FRAME_takeof26 = 107;
        public static readonly int FRAME_takeof27 = 108;
        public static readonly int FRAME_takeof28 = 109;
        public static readonly int FRAME_takeof29 = 110;
        public static readonly int FRAME_takeof30 = 111;
        public static readonly int FRAME_land01 = 112;
        public static readonly int FRAME_pain101 = 113;
        public static readonly int FRAME_pain102 = 114;
        public static readonly int FRAME_pain103 = 115;
        public static readonly int FRAME_pain104 = 116;
        public static readonly int FRAME_pain105 = 117;
        public static readonly int FRAME_pain106 = 118;
        public static readonly int FRAME_pain107 = 119;
        public static readonly int FRAME_pain108 = 120;
        public static readonly int FRAME_pain109 = 121;
        public static readonly int FRAME_pain110 = 122;
        public static readonly int FRAME_pain111 = 123;
        public static readonly int FRAME_pain112 = 124;
        public static readonly int FRAME_pain113 = 125;
        public static readonly int FRAME_pain114 = 126;
        public static readonly int FRAME_pain115 = 127;
        public static readonly int FRAME_pain116 = 128;
        public static readonly int FRAME_pain117 = 129;
        public static readonly int FRAME_pain118 = 130;
        public static readonly int FRAME_pain119 = 131;
        public static readonly int FRAME_pain120 = 132;
        public static readonly int FRAME_pain121 = 133;
        public static readonly int FRAME_pain122 = 134;
        public static readonly int FRAME_pain123 = 135;
        public static readonly int FRAME_pain124 = 136;
        public static readonly int FRAME_pain125 = 137;
        public static readonly int FRAME_pain126 = 138;
        public static readonly int FRAME_pain127 = 139;
        public static readonly int FRAME_pain128 = 140;
        public static readonly int FRAME_pain201 = 141;
        public static readonly int FRAME_pain202 = 142;
        public static readonly int FRAME_pain203 = 143;
        public static readonly int FRAME_pain204 = 144;
        public static readonly int FRAME_pain205 = 145;
        public static readonly int FRAME_pain206 = 146;
        public static readonly int FRAME_pain207 = 147;
        public static readonly int FRAME_pain208 = 148;
        public static readonly int FRAME_pain209 = 149;
        public static readonly int FRAME_pain210 = 150;
        public static readonly int FRAME_pain211 = 151;
        public static readonly int FRAME_pain212 = 152;
        public static readonly int FRAME_pain301 = 153;
        public static readonly int FRAME_pain302 = 154;
        public static readonly int FRAME_pain303 = 155;
        public static readonly int FRAME_pain304 = 156;
        public static readonly int FRAME_pain305 = 157;
        public static readonly int FRAME_pain306 = 158;
        public static readonly int FRAME_pain307 = 159;
        public static readonly int FRAME_pain308 = 160;
        public static readonly int FRAME_pain309 = 161;
        public static readonly int FRAME_death101 = 162;
        public static readonly int FRAME_death102 = 163;
        public static readonly int FRAME_death103 = 164;
        public static readonly int FRAME_death104 = 165;
        public static readonly int FRAME_death105 = 166;
        public static readonly int FRAME_death106 = 167;
        public static readonly int FRAME_death107 = 168;
        public static readonly int FRAME_death108 = 169;
        public static readonly int FRAME_death109 = 170;
        public static readonly int FRAME_death110 = 171;
        public static readonly int FRAME_death111 = 172;
        public static readonly int FRAME_backwd01 = 173;
        public static readonly int FRAME_backwd02 = 174;
        public static readonly int FRAME_backwd03 = 175;
        public static readonly int FRAME_backwd04 = 176;
        public static readonly int FRAME_backwd05 = 177;
        public static readonly int FRAME_backwd06 = 178;
        public static readonly int FRAME_backwd07 = 179;
        public static readonly int FRAME_backwd08 = 180;
        public static readonly int FRAME_backwd09 = 181;
        public static readonly int FRAME_backwd10 = 182;
        public static readonly int FRAME_backwd11 = 183;
        public static readonly int FRAME_backwd12 = 184;
        public static readonly int FRAME_backwd13 = 185;
        public static readonly int FRAME_backwd14 = 186;
        public static readonly int FRAME_backwd15 = 187;
        public static readonly int FRAME_backwd16 = 188;
        public static readonly int FRAME_backwd17 = 189;
        public static readonly int FRAME_backwd18 = 190;
        public static readonly int FRAME_backwd19 = 191;
        public static readonly int FRAME_backwd20 = 192;
        public static readonly int FRAME_backwd21 = 193;
        public static readonly int FRAME_backwd22 = 194;
        public static readonly int FRAME_backwd23 = 195;
        public static readonly int FRAME_backwd24 = 196;
        public static readonly int FRAME_attak101 = 197;
        public static readonly int FRAME_attak102 = 198;
        public static readonly int FRAME_attak103 = 199;
        public static readonly int FRAME_attak104 = 200;
        public static readonly int FRAME_attak105 = 201;
        public static readonly int FRAME_attak106 = 202;
        public static readonly int FRAME_attak107 = 203;
        public static readonly int FRAME_attak108 = 204;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_death1;
        static int sound_death2;
        static int sound_sight;
        static int sound_search1;
        static int sound_search2;
        static EntThinkAdapter hover_reattack = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_reattack";
            }

            public override bool Think(edict_t self)
            {
                if (self.enemy.health > 0)
                    if (GameUtil.Visible(self, self.enemy))
                        if (Lib.Random() <= 0.6)
                        {
                            self.monsterinfo.currentmove = hover_move_attack1;
                            return true;
                        }

                self.monsterinfo.currentmove = hover_move_end_attack;
                return true;
            }
        }

        static EntThinkAdapter hover_fire_blaster = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_fire_blaster";
            }

            public override bool Think(edict_t self)
            {
                float[] start = new float[]{0, 0, 0};
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] end = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                int effect;
                if (self.s.frame == FRAME_attak104)
                    effect = Defines.EF_HYPERBLASTER;
                else
                    effect = 0;
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[Defines.MZ2_HOVER_BLASTER_1], forward, right, start);
                Math3D.VectorCopy(self.enemy.s.origin, end);
                end[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(end, start, dir);
                Monster.Monster_fire_blaster(self, start, dir, 1, 1000, Defines.MZ2_HOVER_BLASTER_1, effect);
                return true;
            }
        }

        static EntThinkAdapter hover_stand = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = hover_move_stand;
                return true;
            }
        }

        static EntThinkAdapter hover_run = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = hover_move_stand;
                else
                    self.monsterinfo.currentmove = hover_move_run;
                return true;
            }
        }

        static EntThinkAdapter hover_walk = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = hover_move_walk;
                return true;
            }
        }

        static EntThinkAdapter hover_start_attack = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_start_attack";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = hover_move_start_attack;
                return true;
            }
        }

        static EntThinkAdapter hover_attack = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_attack";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = hover_move_attack1;
                return true;
            }
        }

        static EntPainAdapter hover_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "hover_pain";
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
                if (damage <= 25)
                {
                    if (Lib.Random() < 0.5)
                    {
                        GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                        self.monsterinfo.currentmove = hover_move_pain3;
                    }
                    else
                    {
                        GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                        self.monsterinfo.currentmove = hover_move_pain2;
                    }
                }
                else
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = hover_move_pain1;
                }
            }
        }

        static EntThinkAdapter hover_deadthink = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_deadthink";
            }

            public override bool Think(edict_t self)
            {
                if (null == self.groundentity && GameBase.level.time < self.timestamp)
                {
                    self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                    return true;
                }

                GameMisc.BecomeExplosion1(self);
                return true;
            }
        }

        static EntThinkAdapter hover_dead = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_dead";
            }

            public override bool Think(edict_t self)
            {
                Math3D.VectorSet(self.mins, -16, -16, -24);
                Math3D.VectorSet(self.maxs, 16, 16, -8);
                self.movetype = Defines.MOVETYPE_TOSS;
                self.think = hover_deadthink;
                self.nextthink = GameBase.level.time + Defines.FRAMETIME;
                self.timestamp = GameBase.level.time + 15;
                GameBase.gi.Linkentity(self);
                return true;
            }
        }

        static EntDieAdapter hover_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "hover_die";
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
                if (Lib.Random() < 0.5)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death1, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death2, 1, Defines.ATTN_NORM, 0);
                self.deadflag = Defines.DEAD_DEAD;
                self.takedamage = Defines.DAMAGE_YES;
                self.monsterinfo.currentmove = hover_move_death1;
            }
        }

        static EntInteractAdapter hover_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "hover_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter hover_search = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "hover_search";
            }

            public override bool Think(edict_t self)
            {
                if (Lib.Random() < 0.5)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search1, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search2, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] hover_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t hover_move_stand = new mmove_t(FRAME_stand01, FRAME_stand30, hover_frames_stand, null);
        static mframe_t[] hover_frames_stop1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t hover_move_stop1 = new mmove_t(FRAME_stop101, FRAME_stop109, hover_frames_stop1, null);
        static mframe_t[] hover_frames_stop2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t hover_move_stop2 = new mmove_t(FRAME_stop201, FRAME_stop208, hover_frames_stop2, null);
        static mframe_t[] hover_frames_takeoff = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, -9, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t hover_move_takeoff = new mmove_t(FRAME_takeof01, FRAME_takeof30, hover_frames_takeoff, null);
        static mframe_t[] hover_frames_pain3 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t hover_move_pain3 = new mmove_t(FRAME_pain301, FRAME_pain309, hover_frames_pain3, hover_run);
        static mframe_t[] hover_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t hover_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain212, hover_frames_pain2, hover_run);
        static mframe_t[] hover_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, -8, null), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 7, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 4, null)};
        static mmove_t hover_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain128, hover_frames_pain1, hover_run);
        static mframe_t[] hover_frames_land = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t hover_move_land = new mmove_t(FRAME_land01, FRAME_land01, hover_frames_land, null);
        static mframe_t[] hover_frames_forward = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t hover_move_forward = new mmove_t(FRAME_forwrd01, FRAME_forwrd35, hover_frames_forward, null);
        static mframe_t[] hover_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null)};
        static mmove_t hover_move_walk = new mmove_t(FRAME_forwrd01, FRAME_forwrd35, hover_frames_walk, null);
        static mframe_t[] hover_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null)};
        static mmove_t hover_move_run = new mmove_t(FRAME_forwrd01, FRAME_forwrd35, hover_frames_run, null);
        static mframe_t[] hover_frames_death1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -10, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 7, null)};
        static mmove_t hover_move_death1 = new mmove_t(FRAME_death101, FRAME_death111, hover_frames_death1, hover_dead);
        static mframe_t[] hover_frames_backward = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t hover_move_backward = new mmove_t(FRAME_backwd01, FRAME_backwd24, hover_frames_backward, null);
        static mframe_t[] hover_frames_start_attack = new mframe_t[]{new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 1, null)};
        static mmove_t hover_move_start_attack = new mmove_t(FRAME_attak101, FRAME_attak103, hover_frames_start_attack, hover_attack);
        static mframe_t[] hover_frames_attack1 = new mframe_t[]{new mframe_t(GameAI.ai_charge, -10, hover_fire_blaster), new mframe_t(GameAI.ai_charge, -10, hover_fire_blaster), new mframe_t(GameAI.ai_charge, 0, hover_reattack)};
        static mmove_t hover_move_attack1 = new mmove_t(FRAME_attak104, FRAME_attak106, hover_frames_attack1, null);
        static mframe_t[] hover_frames_end_attack = new mframe_t[]{new mframe_t(GameAI.ai_charge, 1, null), new mframe_t(GameAI.ai_charge, 1, null)};
        static mmove_t hover_move_end_attack = new mmove_t(FRAME_attak107, FRAME_attak108, hover_frames_end_attack, hover_run);
        public static void SP_monster_hover(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_pain1 = GameBase.gi.Soundindex("hover/hovpain1.wav");
            sound_pain2 = GameBase.gi.Soundindex("hover/hovpain2.wav");
            sound_death1 = GameBase.gi.Soundindex("hover/hovdeth1.wav");
            sound_death2 = GameBase.gi.Soundindex("hover/hovdeth2.wav");
            sound_sight = GameBase.gi.Soundindex("hover/hovsght1.wav");
            sound_search1 = GameBase.gi.Soundindex("hover/hovsrch1.wav");
            sound_search2 = GameBase.gi.Soundindex("hover/hovsrch2.wav");
            GameBase.gi.Soundindex("hover/hovatck1.wav");
            self.s.sound = GameBase.gi.Soundindex("hover/hovidle1.wav");
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/hover/tris.md2");
            Math3D.VectorSet(self.mins, -24, -24, -24);
            Math3D.VectorSet(self.maxs, 24, 24, 32);
            self.health = 240;
            self.gib_health = -100;
            self.mass = 150;
            self.pain = hover_pain;
            self.die = hover_die;
            self.monsterinfo.stand = hover_stand;
            self.monsterinfo.walk = hover_walk;
            self.monsterinfo.run = hover_run;
            self.monsterinfo.attack = hover_start_attack;
            self.monsterinfo.sight = hover_sight;
            self.monsterinfo.search = hover_search;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = hover_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.flymonster_start.Think(self);
        }
    }
}
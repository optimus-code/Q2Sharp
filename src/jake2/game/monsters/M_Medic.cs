using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game.Monsters
{
    public class M_Medic
    {
        public static readonly int FRAME_walk1 = 0;
        public static readonly int FRAME_walk2 = 1;
        public static readonly int FRAME_walk3 = 2;
        public static readonly int FRAME_walk4 = 3;
        public static readonly int FRAME_walk5 = 4;
        public static readonly int FRAME_walk6 = 5;
        public static readonly int FRAME_walk7 = 6;
        public static readonly int FRAME_walk8 = 7;
        public static readonly int FRAME_walk9 = 8;
        public static readonly int FRAME_walk10 = 9;
        public static readonly int FRAME_walk11 = 10;
        public static readonly int FRAME_walk12 = 11;
        public static readonly int FRAME_wait1 = 12;
        public static readonly int FRAME_wait2 = 13;
        public static readonly int FRAME_wait3 = 14;
        public static readonly int FRAME_wait4 = 15;
        public static readonly int FRAME_wait5 = 16;
        public static readonly int FRAME_wait6 = 17;
        public static readonly int FRAME_wait7 = 18;
        public static readonly int FRAME_wait8 = 19;
        public static readonly int FRAME_wait9 = 20;
        public static readonly int FRAME_wait10 = 21;
        public static readonly int FRAME_wait11 = 22;
        public static readonly int FRAME_wait12 = 23;
        public static readonly int FRAME_wait13 = 24;
        public static readonly int FRAME_wait14 = 25;
        public static readonly int FRAME_wait15 = 26;
        public static readonly int FRAME_wait16 = 27;
        public static readonly int FRAME_wait17 = 28;
        public static readonly int FRAME_wait18 = 29;
        public static readonly int FRAME_wait19 = 30;
        public static readonly int FRAME_wait20 = 31;
        public static readonly int FRAME_wait21 = 32;
        public static readonly int FRAME_wait22 = 33;
        public static readonly int FRAME_wait23 = 34;
        public static readonly int FRAME_wait24 = 35;
        public static readonly int FRAME_wait25 = 36;
        public static readonly int FRAME_wait26 = 37;
        public static readonly int FRAME_wait27 = 38;
        public static readonly int FRAME_wait28 = 39;
        public static readonly int FRAME_wait29 = 40;
        public static readonly int FRAME_wait30 = 41;
        public static readonly int FRAME_wait31 = 42;
        public static readonly int FRAME_wait32 = 43;
        public static readonly int FRAME_wait33 = 44;
        public static readonly int FRAME_wait34 = 45;
        public static readonly int FRAME_wait35 = 46;
        public static readonly int FRAME_wait36 = 47;
        public static readonly int FRAME_wait37 = 48;
        public static readonly int FRAME_wait38 = 49;
        public static readonly int FRAME_wait39 = 50;
        public static readonly int FRAME_wait40 = 51;
        public static readonly int FRAME_wait41 = 52;
        public static readonly int FRAME_wait42 = 53;
        public static readonly int FRAME_wait43 = 54;
        public static readonly int FRAME_wait44 = 55;
        public static readonly int FRAME_wait45 = 56;
        public static readonly int FRAME_wait46 = 57;
        public static readonly int FRAME_wait47 = 58;
        public static readonly int FRAME_wait48 = 59;
        public static readonly int FRAME_wait49 = 60;
        public static readonly int FRAME_wait50 = 61;
        public static readonly int FRAME_wait51 = 62;
        public static readonly int FRAME_wait52 = 63;
        public static readonly int FRAME_wait53 = 64;
        public static readonly int FRAME_wait54 = 65;
        public static readonly int FRAME_wait55 = 66;
        public static readonly int FRAME_wait56 = 67;
        public static readonly int FRAME_wait57 = 68;
        public static readonly int FRAME_wait58 = 69;
        public static readonly int FRAME_wait59 = 70;
        public static readonly int FRAME_wait60 = 71;
        public static readonly int FRAME_wait61 = 72;
        public static readonly int FRAME_wait62 = 73;
        public static readonly int FRAME_wait63 = 74;
        public static readonly int FRAME_wait64 = 75;
        public static readonly int FRAME_wait65 = 76;
        public static readonly int FRAME_wait66 = 77;
        public static readonly int FRAME_wait67 = 78;
        public static readonly int FRAME_wait68 = 79;
        public static readonly int FRAME_wait69 = 80;
        public static readonly int FRAME_wait70 = 81;
        public static readonly int FRAME_wait71 = 82;
        public static readonly int FRAME_wait72 = 83;
        public static readonly int FRAME_wait73 = 84;
        public static readonly int FRAME_wait74 = 85;
        public static readonly int FRAME_wait75 = 86;
        public static readonly int FRAME_wait76 = 87;
        public static readonly int FRAME_wait77 = 88;
        public static readonly int FRAME_wait78 = 89;
        public static readonly int FRAME_wait79 = 90;
        public static readonly int FRAME_wait80 = 91;
        public static readonly int FRAME_wait81 = 92;
        public static readonly int FRAME_wait82 = 93;
        public static readonly int FRAME_wait83 = 94;
        public static readonly int FRAME_wait84 = 95;
        public static readonly int FRAME_wait85 = 96;
        public static readonly int FRAME_wait86 = 97;
        public static readonly int FRAME_wait87 = 98;
        public static readonly int FRAME_wait88 = 99;
        public static readonly int FRAME_wait89 = 100;
        public static readonly int FRAME_wait90 = 101;
        public static readonly int FRAME_run1 = 102;
        public static readonly int FRAME_run2 = 103;
        public static readonly int FRAME_run3 = 104;
        public static readonly int FRAME_run4 = 105;
        public static readonly int FRAME_run5 = 106;
        public static readonly int FRAME_run6 = 107;
        public static readonly int FRAME_paina1 = 108;
        public static readonly int FRAME_paina2 = 109;
        public static readonly int FRAME_paina3 = 110;
        public static readonly int FRAME_paina4 = 111;
        public static readonly int FRAME_paina5 = 112;
        public static readonly int FRAME_paina6 = 113;
        public static readonly int FRAME_paina7 = 114;
        public static readonly int FRAME_paina8 = 115;
        public static readonly int FRAME_painb1 = 116;
        public static readonly int FRAME_painb2 = 117;
        public static readonly int FRAME_painb3 = 118;
        public static readonly int FRAME_painb4 = 119;
        public static readonly int FRAME_painb5 = 120;
        public static readonly int FRAME_painb6 = 121;
        public static readonly int FRAME_painb7 = 122;
        public static readonly int FRAME_painb8 = 123;
        public static readonly int FRAME_painb9 = 124;
        public static readonly int FRAME_painb10 = 125;
        public static readonly int FRAME_painb11 = 126;
        public static readonly int FRAME_painb12 = 127;
        public static readonly int FRAME_painb13 = 128;
        public static readonly int FRAME_painb14 = 129;
        public static readonly int FRAME_painb15 = 130;
        public static readonly int FRAME_duck1 = 131;
        public static readonly int FRAME_duck2 = 132;
        public static readonly int FRAME_duck3 = 133;
        public static readonly int FRAME_duck4 = 134;
        public static readonly int FRAME_duck5 = 135;
        public static readonly int FRAME_duck6 = 136;
        public static readonly int FRAME_duck7 = 137;
        public static readonly int FRAME_duck8 = 138;
        public static readonly int FRAME_duck9 = 139;
        public static readonly int FRAME_duck10 = 140;
        public static readonly int FRAME_duck11 = 141;
        public static readonly int FRAME_duck12 = 142;
        public static readonly int FRAME_duck13 = 143;
        public static readonly int FRAME_duck14 = 144;
        public static readonly int FRAME_duck15 = 145;
        public static readonly int FRAME_duck16 = 146;
        public static readonly int FRAME_death1 = 147;
        public static readonly int FRAME_death2 = 148;
        public static readonly int FRAME_death3 = 149;
        public static readonly int FRAME_death4 = 150;
        public static readonly int FRAME_death5 = 151;
        public static readonly int FRAME_death6 = 152;
        public static readonly int FRAME_death7 = 153;
        public static readonly int FRAME_death8 = 154;
        public static readonly int FRAME_death9 = 155;
        public static readonly int FRAME_death10 = 156;
        public static readonly int FRAME_death11 = 157;
        public static readonly int FRAME_death12 = 158;
        public static readonly int FRAME_death13 = 159;
        public static readonly int FRAME_death14 = 160;
        public static readonly int FRAME_death15 = 161;
        public static readonly int FRAME_death16 = 162;
        public static readonly int FRAME_death17 = 163;
        public static readonly int FRAME_death18 = 164;
        public static readonly int FRAME_death19 = 165;
        public static readonly int FRAME_death20 = 166;
        public static readonly int FRAME_death21 = 167;
        public static readonly int FRAME_death22 = 168;
        public static readonly int FRAME_death23 = 169;
        public static readonly int FRAME_death24 = 170;
        public static readonly int FRAME_death25 = 171;
        public static readonly int FRAME_death26 = 172;
        public static readonly int FRAME_death27 = 173;
        public static readonly int FRAME_death28 = 174;
        public static readonly int FRAME_death29 = 175;
        public static readonly int FRAME_death30 = 176;
        public static readonly int FRAME_attack1 = 177;
        public static readonly int FRAME_attack2 = 178;
        public static readonly int FRAME_attack3 = 179;
        public static readonly int FRAME_attack4 = 180;
        public static readonly int FRAME_attack5 = 181;
        public static readonly int FRAME_attack6 = 182;
        public static readonly int FRAME_attack7 = 183;
        public static readonly int FRAME_attack8 = 184;
        public static readonly int FRAME_attack9 = 185;
        public static readonly int FRAME_attack10 = 186;
        public static readonly int FRAME_attack11 = 187;
        public static readonly int FRAME_attack12 = 188;
        public static readonly int FRAME_attack13 = 189;
        public static readonly int FRAME_attack14 = 190;
        public static readonly int FRAME_attack15 = 191;
        public static readonly int FRAME_attack16 = 192;
        public static readonly int FRAME_attack17 = 193;
        public static readonly int FRAME_attack18 = 194;
        public static readonly int FRAME_attack19 = 195;
        public static readonly int FRAME_attack20 = 196;
        public static readonly int FRAME_attack21 = 197;
        public static readonly int FRAME_attack22 = 198;
        public static readonly int FRAME_attack23 = 199;
        public static readonly int FRAME_attack24 = 200;
        public static readonly int FRAME_attack25 = 201;
        public static readonly int FRAME_attack26 = 202;
        public static readonly int FRAME_attack27 = 203;
        public static readonly int FRAME_attack28 = 204;
        public static readonly int FRAME_attack29 = 205;
        public static readonly int FRAME_attack30 = 206;
        public static readonly int FRAME_attack31 = 207;
        public static readonly int FRAME_attack32 = 208;
        public static readonly int FRAME_attack33 = 209;
        public static readonly int FRAME_attack34 = 210;
        public static readonly int FRAME_attack35 = 211;
        public static readonly int FRAME_attack36 = 212;
        public static readonly int FRAME_attack37 = 213;
        public static readonly int FRAME_attack38 = 214;
        public static readonly int FRAME_attack39 = 215;
        public static readonly int FRAME_attack40 = 216;
        public static readonly int FRAME_attack41 = 217;
        public static readonly int FRAME_attack42 = 218;
        public static readonly int FRAME_attack43 = 219;
        public static readonly int FRAME_attack44 = 220;
        public static readonly int FRAME_attack45 = 221;
        public static readonly int FRAME_attack46 = 222;
        public static readonly int FRAME_attack47 = 223;
        public static readonly int FRAME_attack48 = 224;
        public static readonly int FRAME_attack49 = 225;
        public static readonly int FRAME_attack50 = 226;
        public static readonly int FRAME_attack51 = 227;
        public static readonly int FRAME_attack52 = 228;
        public static readonly int FRAME_attack53 = 229;
        public static readonly int FRAME_attack54 = 230;
        public static readonly int FRAME_attack55 = 231;
        public static readonly int FRAME_attack56 = 232;
        public static readonly int FRAME_attack57 = 233;
        public static readonly int FRAME_attack58 = 234;
        public static readonly int FRAME_attack59 = 235;
        public static readonly int FRAME_attack60 = 236;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_idle1;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_die;
        static int sound_sight;
        static int sound_search;
        static int sound_hook_launch;
        static int sound_hook_hit;
        static int sound_hook_heal;
        static int sound_hook_retract;
        static edict_t Medic_FindDeadMonster(edict_t self)
        {
            edict_t ent = null;
            edict_t best = null;
            EdictIterator edit = null;
            while ((edit = GameBase.Findradius(edit, self.s.origin, 1024)) != null)
            {
                ent = edit.o;
                if (ent == self)
                    continue;
                if (0 == (ent.svflags & Defines.SVF_MONSTER))
                    continue;
                if ((ent.monsterinfo.aiflags & Defines.AI_GOOD_GUY) != 0)
                    continue;
                if (ent.owner == null)
                    continue;
                if (ent.health > 0)
                    continue;
                if (ent.nextthink == 0)
                    continue;
                if (!GameUtil.Visible(self, ent))
                    continue;
                if (best == null)
                {
                    best = ent;
                    continue;
                }

                if (ent.max_health <= best.max_health)
                    continue;
                best = ent;
            }

            return best;
        }

        static EntThinkAdapter medic_idle = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_idle";
            }

            public override bool Think(edict_t self)
            {
                edict_t ent;
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle1, 1, Defines.ATTN_IDLE, 0);
                ent = Medic_FindDeadMonster(self);
                if (ent != null)
                {
                    self.enemy = ent;
                    self.enemy.owner = self;
                    self.monsterinfo.aiflags |= Defines.AI_MEDIC;
                    GameUtil.FoundTarget(self);
                }

                return true;
            }
        }

        static EntThinkAdapter medic_search = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_search";
            }

            public override bool Think(edict_t self)
            {
                edict_t ent;
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search, 1, Defines.ATTN_IDLE, 0);
                if (self.oldenemy == null)
                {
                    ent = Medic_FindDeadMonster(self);
                    if (ent != null)
                    {
                        self.oldenemy = self.enemy;
                        self.enemy = ent;
                        self.enemy.owner = self;
                        self.monsterinfo.aiflags |= Defines.AI_MEDIC;
                        GameUtil.FoundTarget(self);
                    }
                }

                return true;
            }
        }

        static EntInteractAdapter medic_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "medic_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] medic_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, medic_idle), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t medic_move_stand = new mmove_t(FRAME_wait1, FRAME_wait90, medic_frames_stand, null);
        static EntThinkAdapter medic_stand = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = medic_move_stand;
                return true;
            }
        }

        static mframe_t[] medic_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 6.2F, null), new mframe_t(GameAI.ai_walk, 18.1F, null), new mframe_t(GameAI.ai_walk, 1, null), new mframe_t(GameAI.ai_walk, 9, null), new mframe_t(GameAI.ai_walk, 10, null), new mframe_t(GameAI.ai_walk, 9, null), new mframe_t(GameAI.ai_walk, 11, null), new mframe_t(GameAI.ai_walk, 11.6F, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 9.9F, null), new mframe_t(GameAI.ai_walk, 14, null), new mframe_t(GameAI.ai_walk, 9.3F, null)};
        static mmove_t medic_move_walk = new mmove_t(FRAME_walk1, FRAME_walk12, medic_frames_walk, null);
        static EntThinkAdapter medic_walk = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = medic_move_walk;
                return true;
            }
        }

        static mframe_t[] medic_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 18, null), new mframe_t(GameAI.ai_run, 22.5F, null), new mframe_t(GameAI.ai_run, 25.4F, null), new mframe_t(GameAI.ai_run, 23.4F, null), new mframe_t(GameAI.ai_run, 24, null), new mframe_t(GameAI.ai_run, 35.6F, null)};
        static mmove_t medic_move_run = new mmove_t(FRAME_run1, FRAME_run6, medic_frames_run, null);
        static EntThinkAdapter medic_run = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_run";
            }

            public override bool Think(edict_t self)
            {
                if (0 == (self.monsterinfo.aiflags & Defines.AI_MEDIC))
                {
                    edict_t ent;
                    ent = Medic_FindDeadMonster(self);
                    if (ent != null)
                    {
                        self.oldenemy = self.enemy;
                        self.enemy = ent;
                        self.enemy.owner = self;
                        self.monsterinfo.aiflags |= Defines.AI_MEDIC;
                        GameUtil.FoundTarget(self);
                        return true;
                    }
                }

                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = medic_move_stand;
                else
                    self.monsterinfo.currentmove = medic_move_run;
                return true;
            }
        }

        static mframe_t[] medic_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t medic_move_pain1 = new mmove_t(FRAME_paina1, FRAME_paina8, medic_frames_pain1, medic_run);
        static mframe_t[] medic_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t medic_move_pain2 = new mmove_t(FRAME_painb1, FRAME_painb15, medic_frames_pain2, medic_run);
        static EntPainAdapter medic_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "medic_pain";
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
                {
                    self.monsterinfo.currentmove = medic_move_pain1;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                }
                else
                {
                    self.monsterinfo.currentmove = medic_move_pain2;
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                }
            }
        }

        static EntThinkAdapter medic_fire_blaster = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_fire_blaster";
            }

            public override bool Think(edict_t self)
            {
                float[] start = new float[]{0, 0, 0};
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                float[] end = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                int effect;
                if ((self.s.frame == FRAME_attack9) || (self.s.frame == FRAME_attack12))
                    effect = Defines.EF_BLASTER;
                else if ((self.s.frame == FRAME_attack19) || (self.s.frame == FRAME_attack22) || (self.s.frame == FRAME_attack25) || (self.s.frame == FRAME_attack28))
                    effect = Defines.EF_HYPERBLASTER;
                else
                    effect = 0;
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[Defines.MZ2_MEDIC_BLASTER_1], forward, right, start);
                Math3D.VectorCopy(self.enemy.s.origin, end);
                end[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(end, start, dir);
                Monster.Monster_fire_blaster(self, start, dir, 2, 1000, Defines.MZ2_MEDIC_BLASTER_1, effect);
                return true;
            }
        }

        static EntThinkAdapter medic_dead = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_dead";
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

        static mframe_t[] medic_frames_death = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t medic_move_death = new mmove_t(FRAME_death1, FRAME_death30, medic_frames_death, medic_dead);
        static EntDieAdapter medic_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "medic_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                int n;
                if ((self.enemy != null) && (self.enemy.owner == self))
                    self.enemy.owner = null;
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
                self.monsterinfo.currentmove = medic_move_death;
            }
        }

        static EntThinkAdapter medic_duck_down = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_duck_down";
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

        static EntThinkAdapter medic_duck_hold = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_duck_hold";
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

        static EntThinkAdapter medic_duck_up = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_duck_up";
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

        static mframe_t[] medic_frames_duck = new mframe_t[]{new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, medic_duck_down), new mframe_t(GameAI.ai_move, -1, medic_duck_hold), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, medic_duck_up), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null)};
        static mmove_t medic_move_duck = new mmove_t(FRAME_duck1, FRAME_duck16, medic_frames_duck, medic_run);
        static EntDodgeAdapter medic_dodge = new AnonymousEntDodgeAdapter();
        private sealed class AnonymousEntDodgeAdapter : EntDodgeAdapter
		{
			
            public override string GetID()
            {
                return "medic_dodge";
            }

            public override void Dodge(edict_t self, edict_t attacker, float eta)
            {
                if (Lib.Random() > 0.25)
                    return;
                if (self.enemy == null)
                    self.enemy = attacker;
                self.monsterinfo.currentmove = medic_move_duck;
            }
        }

        static mframe_t[] medic_frames_attackHyperBlaster = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster)};
        static mmove_t medic_move_attackHyperBlaster = new mmove_t(FRAME_attack15, FRAME_attack30, medic_frames_attackHyperBlaster, medic_run);
        static EntThinkAdapter medic_continue = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_continue";
            }

            public override bool Think(edict_t self)
            {
                if (GameUtil.Visible(self, self.enemy))
                    if (Lib.Random() <= 0.95)
                        self.monsterinfo.currentmove = medic_move_attackHyperBlaster;
                return true;
            }
        }

        static mframe_t[] medic_frames_attackBlaster = new[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, 3, null), new mframe_t(GameAI.ai_charge, 2, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, medic_fire_blaster), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, medic_continue)};
        static mmove_t medic_move_attackBlaster = new mmove_t(FRAME_attack1, FRAME_attack14, medic_frames_attackBlaster, medic_run);
        static EntThinkAdapter medic_hook_launch = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_hook_launch";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_hook_launch, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static float[][] medic_cable_offsets = new float[][]{new[]{45F, -9.2F, 15.5F}, new[]{48.4F, -9.7F, 15.2F}, new[]{47.8F, -9.8F, 15.8F}, new[]{47.3F, -9.3F, 14.3F}, new[]{45.4F, -10.1F, 13.1F}, new[]{41.9F, -12.7F, 12F}, new[]{37.8F, -15.8F, 11.2F}, new[]{34.3F, -18.4F, 10.7F}, new[]{32.7F, -19.7F, 10.4F}, new[]{32.7F, -19.7F, 10.4F}};
        static EntThinkAdapter medic_cable_attack = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_cable_attack";
            }

            public override bool Think(edict_t self)
            {
                float[] offset = new float[]{0, 0, 0}, start = new float[]{0, 0, 0}, end = new float[]{0, 0, 0}, f = new float[]{0, 0, 0}, r = new float[]{0, 0, 0};
                trace_t tr;
                float[] dir = new float[]{0, 0, 0}, angles = new float[]{0, 0, 0};
                float distance;
                if (!self.enemy.inuse)
                    return true;
                Math3D.AngleVectors(self.s.angles, f, r, null);
                Math3D.VectorCopy(medic_cable_offsets[self.s.frame - FRAME_attack42], offset);
                Math3D.G_ProjectSource(self.s.origin, offset, f, r, start);
                Math3D.VectorSubtract(start, self.enemy.s.origin, dir);
                distance = Math3D.VectorLength(dir);
                if (distance > 256)
                    return true;
                Math3D.Vectoangles(dir, angles);
                if (angles[0] < -180)
                    angles[0] += 360;
                if (Math.Abs(angles[0]) > 45)
                    return true;
                tr = GameBase.gi.Trace(start, null, null, self.enemy.s.origin, self, Defines.MASK_SHOT);
                if (tr.fraction != 1 && tr.ent != self.enemy)
                    return true;
                if (self.s.frame == FRAME_attack43)
                {
                    GameBase.gi.Sound(self.enemy, Defines.CHAN_AUTO, sound_hook_hit, 1, Defines.ATTN_NORM, 0);
                    self.enemy.monsterinfo.aiflags |= Defines.AI_RESURRECTING;
                }
                else if (self.s.frame == FRAME_attack50)
                {
                    self.enemy.spawnflags = 0;
                    self.enemy.monsterinfo.aiflags = 0;
                    self.enemy.target = null;
                    self.enemy.targetname = null;
                    self.enemy.combattarget = null;
                    self.enemy.deathtarget = null;
                    self.enemy.owner = self;
                    GameSpawn.ED_CallSpawn(self.enemy);
                    self.enemy.owner = null;
                    if (self.enemy.think != null)
                    {
                        self.enemy.nextthink = GameBase.level.time;
                        self.enemy.think.Think(self.enemy);
                    }

                    self.enemy.monsterinfo.aiflags |= Defines.AI_RESURRECTING;
                    if (self.oldenemy != null && self.oldenemy.client != null)
                    {
                        self.enemy.enemy = self.oldenemy;
                        GameUtil.FoundTarget(self.enemy);
                    }
                }
                else
                {
                    if (self.s.frame == FRAME_attack44)
                        GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_hook_heal, 1, Defines.ATTN_NORM, 0);
                }

                Math3D.VectorMA(start, 8, f, start);
                Math3D.VectorCopy(self.enemy.s.origin, end);
                end[2] = self.enemy.absmin[2] + self.enemy.size[2] / 2;
                GameBase.gi.WriteByte(Defines.svc_temp_entity);
                GameBase.gi.WriteByte(Defines.TE_MEDIC_CABLE_ATTACK);
                GameBase.gi.WriteShort(self.index);
                GameBase.gi.WritePosition(start);
                GameBase.gi.WritePosition(end);
                GameBase.gi.Multicast(self.s.origin, Defines.MULTICAST_PVS);
                return true;
            }
        }

        static EntThinkAdapter medic_hook_retract = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_hook_retract";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_hook_retract, 1, Defines.ATTN_NORM, 0);
                self.enemy.monsterinfo.aiflags &= ~Defines.AI_RESURRECTING;
                return true;
            }
        }

        static mframe_t[] medic_frames_attackCable = new mframe_t[]{new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, 4.4F, null), new mframe_t(GameAI.ai_charge, 4.7F, null), new mframe_t(GameAI.ai_charge, 5, null), new mframe_t(GameAI.ai_charge, 6, null), new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_move, 0, medic_hook_launch), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, 0, medic_cable_attack), new mframe_t(GameAI.ai_move, -15, medic_hook_retract), new mframe_t(GameAI.ai_move, -1.5F, null), new mframe_t(GameAI.ai_move, -1.2F, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, -2, null), new mframe_t(GameAI.ai_move, 0.3F, null), new mframe_t(GameAI.ai_move, 0.7F, null), new mframe_t(GameAI.ai_move, 1.2F, null), new mframe_t(GameAI.ai_move, 1.3F, null)};
        static mmove_t medic_move_attackCable = new mmove_t(FRAME_attack33, FRAME_attack60, medic_frames_attackCable, medic_run);
        static EntThinkAdapter medic_attack = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_attack";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_MEDIC) != 0)
                    self.monsterinfo.currentmove = medic_move_attackCable;
                else
                    self.monsterinfo.currentmove = medic_move_attackBlaster;
                return true;
            }
        }

        static EntThinkAdapter medic_checkattack = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "medic_checkattack";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_MEDIC) != 0)
                {
                    medic_attack.Think(self);
                    return true;
                }

                return GameUtil.M_CheckAttack.Think(self);
            }
        }

        public static void SP_monster_medic(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_idle1 = GameBase.gi.Soundindex("medic/idle.wav");
            sound_pain1 = GameBase.gi.Soundindex("medic/medpain1.wav");
            sound_pain2 = GameBase.gi.Soundindex("medic/medpain2.wav");
            sound_die = GameBase.gi.Soundindex("medic/meddeth1.wav");
            sound_sight = GameBase.gi.Soundindex("medic/medsght1.wav");
            sound_search = GameBase.gi.Soundindex("medic/medsrch1.wav");
            sound_hook_launch = GameBase.gi.Soundindex("medic/medatck2.wav");
            sound_hook_hit = GameBase.gi.Soundindex("medic/medatck3.wav");
            sound_hook_heal = GameBase.gi.Soundindex("medic/medatck4.wav");
            sound_hook_retract = GameBase.gi.Soundindex("medic/medatck5.wav");
            GameBase.gi.Soundindex("medic/medatck1.wav");
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/medic/tris.md2");
            Math3D.VectorSet(self.mins, -24, -24, -24);
            Math3D.VectorSet(self.maxs, 24, 24, 32);
            self.health = 300;
            self.gib_health = -130;
            self.mass = 400;
            self.pain = medic_pain;
            self.die = medic_die;
            self.monsterinfo.stand = medic_stand;
            self.monsterinfo.walk = medic_walk;
            self.monsterinfo.run = medic_run;
            self.monsterinfo.dodge = medic_dodge;
            self.monsterinfo.attack = medic_attack;
            self.monsterinfo.melee = null;
            self.monsterinfo.sight = medic_sight;
            self.monsterinfo.idle = medic_idle;
            self.monsterinfo.search = medic_search;
            self.monsterinfo.checkattack = medic_checkattack;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = medic_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.walkmonster_start.Think(self);
        }
    }
}
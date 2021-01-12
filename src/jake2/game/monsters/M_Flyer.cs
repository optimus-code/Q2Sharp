using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Flyer
    {
        public static readonly int ACTION_nothing = 0;
        public static readonly int ACTION_attack1 = 1;
        public static readonly int ACTION_attack2 = 2;
        public static readonly int ACTION_run = 3;
        public static readonly int ACTION_walk = 4;
        public static readonly int FRAME_start01 = 0;
        public static readonly int FRAME_start02 = 1;
        public static readonly int FRAME_start03 = 2;
        public static readonly int FRAME_start04 = 3;
        public static readonly int FRAME_start05 = 4;
        public static readonly int FRAME_start06 = 5;
        public static readonly int FRAME_stop01 = 6;
        public static readonly int FRAME_stop02 = 7;
        public static readonly int FRAME_stop03 = 8;
        public static readonly int FRAME_stop04 = 9;
        public static readonly int FRAME_stop05 = 10;
        public static readonly int FRAME_stop06 = 11;
        public static readonly int FRAME_stop07 = 12;
        public static readonly int FRAME_stand01 = 13;
        public static readonly int FRAME_stand02 = 14;
        public static readonly int FRAME_stand03 = 15;
        public static readonly int FRAME_stand04 = 16;
        public static readonly int FRAME_stand05 = 17;
        public static readonly int FRAME_stand06 = 18;
        public static readonly int FRAME_stand07 = 19;
        public static readonly int FRAME_stand08 = 20;
        public static readonly int FRAME_stand09 = 21;
        public static readonly int FRAME_stand10 = 22;
        public static readonly int FRAME_stand11 = 23;
        public static readonly int FRAME_stand12 = 24;
        public static readonly int FRAME_stand13 = 25;
        public static readonly int FRAME_stand14 = 26;
        public static readonly int FRAME_stand15 = 27;
        public static readonly int FRAME_stand16 = 28;
        public static readonly int FRAME_stand17 = 29;
        public static readonly int FRAME_stand18 = 30;
        public static readonly int FRAME_stand19 = 31;
        public static readonly int FRAME_stand20 = 32;
        public static readonly int FRAME_stand21 = 33;
        public static readonly int FRAME_stand22 = 34;
        public static readonly int FRAME_stand23 = 35;
        public static readonly int FRAME_stand24 = 36;
        public static readonly int FRAME_stand25 = 37;
        public static readonly int FRAME_stand26 = 38;
        public static readonly int FRAME_stand27 = 39;
        public static readonly int FRAME_stand28 = 40;
        public static readonly int FRAME_stand29 = 41;
        public static readonly int FRAME_stand30 = 42;
        public static readonly int FRAME_stand31 = 43;
        public static readonly int FRAME_stand32 = 44;
        public static readonly int FRAME_stand33 = 45;
        public static readonly int FRAME_stand34 = 46;
        public static readonly int FRAME_stand35 = 47;
        public static readonly int FRAME_stand36 = 48;
        public static readonly int FRAME_stand37 = 49;
        public static readonly int FRAME_stand38 = 50;
        public static readonly int FRAME_stand39 = 51;
        public static readonly int FRAME_stand40 = 52;
        public static readonly int FRAME_stand41 = 53;
        public static readonly int FRAME_stand42 = 54;
        public static readonly int FRAME_stand43 = 55;
        public static readonly int FRAME_stand44 = 56;
        public static readonly int FRAME_stand45 = 57;
        public static readonly int FRAME_attak101 = 58;
        public static readonly int FRAME_attak102 = 59;
        public static readonly int FRAME_attak103 = 60;
        public static readonly int FRAME_attak104 = 61;
        public static readonly int FRAME_attak105 = 62;
        public static readonly int FRAME_attak106 = 63;
        public static readonly int FRAME_attak107 = 64;
        public static readonly int FRAME_attak108 = 65;
        public static readonly int FRAME_attak109 = 66;
        public static readonly int FRAME_attak110 = 67;
        public static readonly int FRAME_attak111 = 68;
        public static readonly int FRAME_attak112 = 69;
        public static readonly int FRAME_attak113 = 70;
        public static readonly int FRAME_attak114 = 71;
        public static readonly int FRAME_attak115 = 72;
        public static readonly int FRAME_attak116 = 73;
        public static readonly int FRAME_attak117 = 74;
        public static readonly int FRAME_attak118 = 75;
        public static readonly int FRAME_attak119 = 76;
        public static readonly int FRAME_attak120 = 77;
        public static readonly int FRAME_attak121 = 78;
        public static readonly int FRAME_attak201 = 79;
        public static readonly int FRAME_attak202 = 80;
        public static readonly int FRAME_attak203 = 81;
        public static readonly int FRAME_attak204 = 82;
        public static readonly int FRAME_attak205 = 83;
        public static readonly int FRAME_attak206 = 84;
        public static readonly int FRAME_attak207 = 85;
        public static readonly int FRAME_attak208 = 86;
        public static readonly int FRAME_attak209 = 87;
        public static readonly int FRAME_attak210 = 88;
        public static readonly int FRAME_attak211 = 89;
        public static readonly int FRAME_attak212 = 90;
        public static readonly int FRAME_attak213 = 91;
        public static readonly int FRAME_attak214 = 92;
        public static readonly int FRAME_attak215 = 93;
        public static readonly int FRAME_attak216 = 94;
        public static readonly int FRAME_attak217 = 95;
        public static readonly int FRAME_bankl01 = 96;
        public static readonly int FRAME_bankl02 = 97;
        public static readonly int FRAME_bankl03 = 98;
        public static readonly int FRAME_bankl04 = 99;
        public static readonly int FRAME_bankl05 = 100;
        public static readonly int FRAME_bankl06 = 101;
        public static readonly int FRAME_bankl07 = 102;
        public static readonly int FRAME_bankr01 = 103;
        public static readonly int FRAME_bankr02 = 104;
        public static readonly int FRAME_bankr03 = 105;
        public static readonly int FRAME_bankr04 = 106;
        public static readonly int FRAME_bankr05 = 107;
        public static readonly int FRAME_bankr06 = 108;
        public static readonly int FRAME_bankr07 = 109;
        public static readonly int FRAME_rollf01 = 110;
        public static readonly int FRAME_rollf02 = 111;
        public static readonly int FRAME_rollf03 = 112;
        public static readonly int FRAME_rollf04 = 113;
        public static readonly int FRAME_rollf05 = 114;
        public static readonly int FRAME_rollf06 = 115;
        public static readonly int FRAME_rollf07 = 116;
        public static readonly int FRAME_rollf08 = 117;
        public static readonly int FRAME_rollf09 = 118;
        public static readonly int FRAME_rollr01 = 119;
        public static readonly int FRAME_rollr02 = 120;
        public static readonly int FRAME_rollr03 = 121;
        public static readonly int FRAME_rollr04 = 122;
        public static readonly int FRAME_rollr05 = 123;
        public static readonly int FRAME_rollr06 = 124;
        public static readonly int FRAME_rollr07 = 125;
        public static readonly int FRAME_rollr08 = 126;
        public static readonly int FRAME_rollr09 = 127;
        public static readonly int FRAME_defens01 = 128;
        public static readonly int FRAME_defens02 = 129;
        public static readonly int FRAME_defens03 = 130;
        public static readonly int FRAME_defens04 = 131;
        public static readonly int FRAME_defens05 = 132;
        public static readonly int FRAME_defens06 = 133;
        public static readonly int FRAME_pain101 = 134;
        public static readonly int FRAME_pain102 = 135;
        public static readonly int FRAME_pain103 = 136;
        public static readonly int FRAME_pain104 = 137;
        public static readonly int FRAME_pain105 = 138;
        public static readonly int FRAME_pain106 = 139;
        public static readonly int FRAME_pain107 = 140;
        public static readonly int FRAME_pain108 = 141;
        public static readonly int FRAME_pain109 = 142;
        public static readonly int FRAME_pain201 = 143;
        public static readonly int FRAME_pain202 = 144;
        public static readonly int FRAME_pain203 = 145;
        public static readonly int FRAME_pain204 = 146;
        public static readonly int FRAME_pain301 = 147;
        public static readonly int FRAME_pain302 = 148;
        public static readonly int FRAME_pain303 = 149;
        public static readonly int FRAME_pain304 = 150;
        public static readonly float MODEL_SCALE = 1F;
        static int nextmove;
        static int sound_sight;
        static int sound_idle;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_slash;
        static int sound_sproing;
        static int sound_die;
        public static EntInteractAdapter flyer_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
        {
            public override string GetID()
            {
                return "flyer_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter flyer_idle = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_idle";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static EntThinkAdapter flyer_pop_blades = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_pop_blades";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sproing, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] flyer_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t flyer_move_stand = new mmove_t(FRAME_stand01, FRAME_stand45, flyer_frames_stand, null);
        static mframe_t[] flyer_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null)};
        static mmove_t flyer_move_walk = new mmove_t(FRAME_stand01, FRAME_stand45, flyer_frames_walk, null);
        static mframe_t[] flyer_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 10, null)};
        static mmove_t flyer_move_run = new mmove_t(FRAME_stand01, FRAME_stand45, flyer_frames_run, null);
        static EntThinkAdapter flyer_run = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = flyer_move_stand;
                else
                    self.monsterinfo.currentmove = flyer_move_run;
                return true;
            }
        }

        static EntThinkAdapter flyer_walk = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flyer_move_walk;
                return true;
            }
        }

        static EntThinkAdapter flyer_stand = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flyer_move_stand;
                return true;
            }
        }

        static EntThinkAdapter flyer_nextmove = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_nextmove";
            }

            public override bool Think(edict_t self)
            {
                if (nextmove == ACTION_attack1)
                    self.monsterinfo.currentmove = flyer_move_start_melee;
                else if (nextmove == ACTION_attack2)
                    self.monsterinfo.currentmove = flyer_move_attack2;
                else if (nextmove == ACTION_run)
                    self.monsterinfo.currentmove = flyer_move_run;
                return true;
            }
        }

        static mframe_t[] flyer_frames_start = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, flyer_nextmove)};
        static mmove_t flyer_move_start = new mmove_t(FRAME_start01, FRAME_start06, flyer_frames_start, null);
        static mframe_t[] flyer_frames_stop = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, flyer_nextmove)};
        static mmove_t flyer_move_stop = new mmove_t(FRAME_stop01, FRAME_stop07, flyer_frames_stop, null);
        static EntThinkAdapter flyer_stop = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_stop";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flyer_move_stop;
                return true;
            }
        }

        static EntThinkAdapter flyer_start = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_start";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flyer_move_start;
                return true;
            }
        }

        static mframe_t[] flyer_frames_rollright = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flyer_move_rollright = new mmove_t(FRAME_rollr01, FRAME_rollr09, flyer_frames_rollright, null);
        static mframe_t[] flyer_frames_rollleft = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flyer_move_rollleft = new mmove_t(FRAME_rollf01, FRAME_rollf09, flyer_frames_rollleft, null);
        static mframe_t[] flyer_frames_pain3 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flyer_move_pain3 = new mmove_t(FRAME_pain301, FRAME_pain304, flyer_frames_pain3, flyer_run);
        static mframe_t[] flyer_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flyer_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain204, flyer_frames_pain2, flyer_run);
        static mframe_t[] flyer_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flyer_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain109, flyer_frames_pain1, flyer_run);
        static mframe_t[] flyer_frames_defense = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flyer_move_defense = new mmove_t(FRAME_defens01, FRAME_defens06, flyer_frames_defense, null);
        static mframe_t[] flyer_frames_bankright = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flyer_move_bankright = new mmove_t(FRAME_bankr01, FRAME_bankr07, flyer_frames_bankright, null);
        static mframe_t[] flyer_frames_bankleft = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t flyer_move_bankleft = new mmove_t(FRAME_bankl01, FRAME_bankl07, flyer_frames_bankleft, null);
        static EntThinkAdapter flyer_fireleft = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_fireleft";
            }

            public override bool Think(edict_t self)
            {
                Flyer_fire(self, Defines.MZ2_FLYER_BLASTER_1);
                return true;
            }
        }

        static EntThinkAdapter flyer_fireright = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_fireright";
            }

            public override bool Think(edict_t self)
            {
                Flyer_fire(self, Defines.MZ2_FLYER_BLASTER_2);
                return true;
            }
        }

        static mframe_t[] flyer_frames_attack2 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, -10, flyer_fireleft), new mframe_t(GameAI.ai_charge, -10, flyer_fireright), new mframe_t(GameAI.ai_charge, -10, flyer_fireleft), new mframe_t(GameAI.ai_charge, -10, flyer_fireright), new mframe_t(GameAI.ai_charge, -10, flyer_fireleft), new mframe_t(GameAI.ai_charge, -10, flyer_fireright), new mframe_t(GameAI.ai_charge, -10, flyer_fireleft), new mframe_t(GameAI.ai_charge, -10, flyer_fireright), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t flyer_move_attack2 = new mmove_t(FRAME_attak201, FRAME_attak217, flyer_frames_attack2, flyer_run);
        static EntThinkAdapter flyer_slash_left = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_slash_left";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, self.mins[0], 0);
                GameWeapon.Fire_hit(self, aim, 5, 0);
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_slash, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter flyer_slash_right = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_slash_right";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, self.maxs[0], 0);
                GameWeapon.Fire_hit(self, aim, 5, 0);
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_slash, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter flyer_loop_melee = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_loop_melee";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flyer_move_loop_melee;
                return true;
            }
        }

        static mframe_t[] flyer_frames_start_melee = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, flyer_pop_blades), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t flyer_move_start_melee = new mmove_t(FRAME_attak101, FRAME_attak106, flyer_frames_start_melee, flyer_loop_melee);
        static mframe_t[] flyer_frames_end_melee = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t flyer_move_end_melee = new mmove_t(FRAME_attak119, FRAME_attak121, flyer_frames_end_melee, flyer_run);
        static mframe_t[] flyer_frames_loop_melee = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, flyer_slash_left), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, flyer_slash_right), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static EntThinkAdapter flyer_check_melee = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_check_melee";
            }

            public override bool Think(edict_t self)
            {
                if (GameUtil.Range(self, self.enemy) == Defines.RANGE_MELEE)
                    if (Lib.Random() <= 0.8)
                        self.monsterinfo.currentmove = flyer_move_loop_melee;
                    else
                        self.monsterinfo.currentmove = flyer_move_end_melee;
                else
                    self.monsterinfo.currentmove = flyer_move_end_melee;
                return true;
            }
        }

        static mmove_t flyer_move_loop_melee = new mmove_t(FRAME_attak107, FRAME_attak118, flyer_frames_loop_melee, flyer_check_melee);
        static EntThinkAdapter flyer_attack = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_attack";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flyer_move_attack2;
                return true;
            }
        }

        static EntThinkAdapter flyer_setstart = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_setstart";
            }

            public override bool Think(edict_t self)
            {
                nextmove = ACTION_run;
                self.monsterinfo.currentmove = flyer_move_start;
                return true;
            }
        }

        static EntThinkAdapter flyer_melee = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
        {
            public override string GetID()
            {
                return "flyer_melee";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = flyer_move_start_melee;
                return true;
            }
        }

        static EntPainAdapter flyer_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
        {
            public override string GetID()
            {
                return "flyer_pain";
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
                n = Lib.Rand() % 3;
                if (n == 0)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = flyer_move_pain1;
                }
                else if (n == 1)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = flyer_move_pain2;
                }
                else
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                    self.monsterinfo.currentmove = flyer_move_pain3;
                }
            }
        }

        static EntDieAdapter flyer_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
        {
            public override string GetID()
            {
                return "flyer_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_die, 1, Defines.ATTN_NORM, 0);
                GameMisc.BecomeExplosion1(self);
            }
        }

        static void Flyer_fire(edict_t self, int flash_number)
        {
            float[] start = new float[]{0, 0, 0};
            float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
            float[] end = new float[]{0, 0, 0};
            float[] dir = new float[]{0, 0, 0};
            int effect;
            if ((self.s.frame == FRAME_attak204) || (self.s.frame == FRAME_attak207) || (self.s.frame == FRAME_attak210))
                effect = Defines.EF_HYPERBLASTER;
            else
                effect = 0;
            Math3D.AngleVectors(self.s.angles, forward, right, null);
            Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_number], forward, right, start);
            Math3D.VectorCopy(self.enemy.s.origin, end);
            end[2] += self.enemy.viewheight;
            Math3D.VectorSubtract(end, start, dir);
            Monster.Monster_fire_blaster(self, start, dir, 1, 1000, flash_number, effect);
        }

        public static void SP_monster_flyer(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            if (GameBase.level.mapname.EqualsIgnoreCase("jail5") && (self.s.origin[2] == -104))
            {
                self.targetname = self.target;
                self.target = null;
            }

            sound_sight = GameBase.gi.Soundindex("flyer/flysght1.wav");
            sound_idle = GameBase.gi.Soundindex("flyer/flysrch1.wav");
            sound_pain1 = GameBase.gi.Soundindex("flyer/flypain1.wav");
            sound_pain2 = GameBase.gi.Soundindex("flyer/flypain2.wav");
            sound_slash = GameBase.gi.Soundindex("flyer/flyatck2.wav");
            sound_sproing = GameBase.gi.Soundindex("flyer/flyatck1.wav");
            sound_die = GameBase.gi.Soundindex("flyer/flydeth1.wav");
            GameBase.gi.Soundindex("flyer/flyatck3.wav");
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/flyer/tris.md2");
            Math3D.VectorSet(self.mins, -16, -16, -24);
            Math3D.VectorSet(self.maxs, 16, 16, 32);
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.sound = GameBase.gi.Soundindex("flyer/flyidle1.wav");
            self.health = 50;
            self.mass = 50;
            self.pain = flyer_pain;
            self.die = flyer_die;
            self.monsterinfo.stand = flyer_stand;
            self.monsterinfo.walk = flyer_walk;
            self.monsterinfo.run = flyer_run;
            self.monsterinfo.attack = flyer_attack;
            self.monsterinfo.melee = flyer_melee;
            self.monsterinfo.sight = flyer_sight;
            self.monsterinfo.idle = flyer_idle;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = flyer_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.flymonster_start.Think(self);
        }
    }
}
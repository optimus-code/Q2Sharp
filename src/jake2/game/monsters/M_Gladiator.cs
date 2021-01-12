using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game.Monsters
{
    public class M_Gladiator
    {
        public static readonly int FRAME_stand1 = 0;
        public static readonly int FRAME_stand2 = 1;
        public static readonly int FRAME_stand3 = 2;
        public static readonly int FRAME_stand4 = 3;
        public static readonly int FRAME_stand5 = 4;
        public static readonly int FRAME_stand6 = 5;
        public static readonly int FRAME_stand7 = 6;
        public static readonly int FRAME_walk1 = 7;
        public static readonly int FRAME_walk2 = 8;
        public static readonly int FRAME_walk3 = 9;
        public static readonly int FRAME_walk4 = 10;
        public static readonly int FRAME_walk5 = 11;
        public static readonly int FRAME_walk6 = 12;
        public static readonly int FRAME_walk7 = 13;
        public static readonly int FRAME_walk8 = 14;
        public static readonly int FRAME_walk9 = 15;
        public static readonly int FRAME_walk10 = 16;
        public static readonly int FRAME_walk11 = 17;
        public static readonly int FRAME_walk12 = 18;
        public static readonly int FRAME_walk13 = 19;
        public static readonly int FRAME_walk14 = 20;
        public static readonly int FRAME_walk15 = 21;
        public static readonly int FRAME_walk16 = 22;
        public static readonly int FRAME_run1 = 23;
        public static readonly int FRAME_run2 = 24;
        public static readonly int FRAME_run3 = 25;
        public static readonly int FRAME_run4 = 26;
        public static readonly int FRAME_run5 = 27;
        public static readonly int FRAME_run6 = 28;
        public static readonly int FRAME_melee1 = 29;
        public static readonly int FRAME_melee2 = 30;
        public static readonly int FRAME_melee3 = 31;
        public static readonly int FRAME_melee4 = 32;
        public static readonly int FRAME_melee5 = 33;
        public static readonly int FRAME_melee6 = 34;
        public static readonly int FRAME_melee7 = 35;
        public static readonly int FRAME_melee8 = 36;
        public static readonly int FRAME_melee9 = 37;
        public static readonly int FRAME_melee10 = 38;
        public static readonly int FRAME_melee11 = 39;
        public static readonly int FRAME_melee12 = 40;
        public static readonly int FRAME_melee13 = 41;
        public static readonly int FRAME_melee14 = 42;
        public static readonly int FRAME_melee15 = 43;
        public static readonly int FRAME_melee16 = 44;
        public static readonly int FRAME_melee17 = 45;
        public static readonly int FRAME_attack1 = 46;
        public static readonly int FRAME_attack2 = 47;
        public static readonly int FRAME_attack3 = 48;
        public static readonly int FRAME_attack4 = 49;
        public static readonly int FRAME_attack5 = 50;
        public static readonly int FRAME_attack6 = 51;
        public static readonly int FRAME_attack7 = 52;
        public static readonly int FRAME_attack8 = 53;
        public static readonly int FRAME_attack9 = 54;
        public static readonly int FRAME_pain1 = 55;
        public static readonly int FRAME_pain2 = 56;
        public static readonly int FRAME_pain3 = 57;
        public static readonly int FRAME_pain4 = 58;
        public static readonly int FRAME_pain5 = 59;
        public static readonly int FRAME_pain6 = 60;
        public static readonly int FRAME_death1 = 61;
        public static readonly int FRAME_death2 = 62;
        public static readonly int FRAME_death3 = 63;
        public static readonly int FRAME_death4 = 64;
        public static readonly int FRAME_death5 = 65;
        public static readonly int FRAME_death6 = 66;
        public static readonly int FRAME_death7 = 67;
        public static readonly int FRAME_death8 = 68;
        public static readonly int FRAME_death9 = 69;
        public static readonly int FRAME_death10 = 70;
        public static readonly int FRAME_death11 = 71;
        public static readonly int FRAME_death12 = 72;
        public static readonly int FRAME_death13 = 73;
        public static readonly int FRAME_death14 = 74;
        public static readonly int FRAME_death15 = 75;
        public static readonly int FRAME_death16 = 76;
        public static readonly int FRAME_death17 = 77;
        public static readonly int FRAME_death18 = 78;
        public static readonly int FRAME_death19 = 79;
        public static readonly int FRAME_death20 = 80;
        public static readonly int FRAME_death21 = 81;
        public static readonly int FRAME_death22 = 82;
        public static readonly int FRAME_painup1 = 83;
        public static readonly int FRAME_painup2 = 84;
        public static readonly int FRAME_painup3 = 85;
        public static readonly int FRAME_painup4 = 86;
        public static readonly int FRAME_painup5 = 87;
        public static readonly int FRAME_painup6 = 88;
        public static readonly int FRAME_painup7 = 89;
        public static readonly float MODEL_SCALE = 1F;
        static int sound_pain1;
        static int sound_pain2;
        static int sound_die;
        static int sound_gun;
        static int sound_cleaver_swing;
        static int sound_cleaver_hit;
        static int sound_cleaver_miss;
        static int sound_idle;
        static int sound_search;
        static int sound_sight;
        static EntThinkAdapter gladiator_idle = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_idle";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static EntInteractAdapter gladiator_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter gladiator_search = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_search";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_search, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static EntThinkAdapter gladiator_cleaver_swing = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_cleaver_swing";
            }

            public override bool Think(edict_t self)
            {
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_cleaver_swing, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] gladiator_frames_stand = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t gladiator_move_stand = new mmove_t(FRAME_stand1, FRAME_stand7, gladiator_frames_stand, null);
        static EntThinkAdapter gladiator_stand = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_stand";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = gladiator_move_stand;
                return true;
            }
        }

        static mframe_t[] gladiator_frames_walk = new mframe_t[]{new mframe_t(GameAI.ai_walk, 15, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 8, null), new mframe_t(GameAI.ai_walk, 12, null), new mframe_t(GameAI.ai_walk, 8, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 1, null), new mframe_t(GameAI.ai_walk, 8, null)};
        static mmove_t gladiator_move_walk = new mmove_t(FRAME_walk1, FRAME_walk16, gladiator_frames_walk, null);
        static EntThinkAdapter gladiator_walk = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_walk";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = gladiator_move_walk;
                return true;
            }
        }

        static mframe_t[] gladiator_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 23, null), new mframe_t(GameAI.ai_run, 14, null), new mframe_t(GameAI.ai_run, 14, null), new mframe_t(GameAI.ai_run, 21, null), new mframe_t(GameAI.ai_run, 12, null), new mframe_t(GameAI.ai_run, 13, null)};
        static mmove_t gladiator_move_run = new mmove_t(FRAME_run1, FRAME_run6, gladiator_frames_run, null);
        static EntThinkAdapter gladiator_run = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                    self.monsterinfo.currentmove = gladiator_move_stand;
                else
                    self.monsterinfo.currentmove = gladiator_move_run;
                return true;
            }
        }

        static EntThinkAdapter GaldiatorMelee = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "GaldiatorMelee";
            }

            public override bool Think(edict_t self)
            {
                float[] aim = new float[]{0, 0, 0};
                Math3D.VectorSet(aim, Defines.MELEE_DISTANCE, self.mins[0], -4);
                if (GameWeapon.Fire_hit(self, aim, (20 + (Lib.Rand() % 5)), 300))
                    GameBase.gi.Sound(self, Defines.CHAN_AUTO, sound_cleaver_hit, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_AUTO, sound_cleaver_miss, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] gladiator_frames_attack_melee = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, gladiator_cleaver_swing), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, GaldiatorMelee), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, gladiator_cleaver_swing), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, GaldiatorMelee), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t gladiator_move_attack_melee = new mmove_t(FRAME_melee1, FRAME_melee17, gladiator_frames_attack_melee, gladiator_run);
        static EntThinkAdapter gladiator_melee = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_melee";
            }

            public override bool Think(edict_t self)
            {
                self.monsterinfo.currentmove = gladiator_move_attack_melee;
                return true;
            }
        }

        static EntThinkAdapter GladiatorGun = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "GladiatorGun";
            }

            public override bool Think(edict_t self)
            {
                float[] start = new float[]{0, 0, 0};
                float[] dir = new float[]{0, 0, 0};
                float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0};
                Math3D.AngleVectors(self.s.angles, forward, right, null);
                Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[Defines.MZ2_GLADIATOR_RAILGUN_1], forward, right, start);
                Math3D.VectorSubtract(self.pos1, start, dir);
                Math3D.VectorNormalize(dir);
                Monster.Monster_fire_railgun(self, start, dir, 50, 100, Defines.MZ2_GLADIATOR_RAILGUN_1);
                return true;
            }
        }

        static mframe_t[] gladiator_frames_attack_gun = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, GladiatorGun), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t gladiator_move_attack_gun = new mmove_t(FRAME_attack1, FRAME_attack9, gladiator_frames_attack_gun, gladiator_run);
        static EntThinkAdapter gladiator_attack = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_attack";
            }

            public override bool Think(edict_t self)
            {
                float range;
                float[] v = new float[]{0, 0, 0};
                Math3D.VectorSubtract(self.s.origin, self.enemy.s.origin, v);
                range = Math3D.VectorLength(v);
                if (range <= (Defines.MELEE_DISTANCE + 32))
                    return true;
                GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_gun, 1, Defines.ATTN_NORM, 0);
                Math3D.VectorCopy(self.enemy.s.origin, self.pos1);
                self.pos1[2] += self.enemy.viewheight;
                self.monsterinfo.currentmove = gladiator_move_attack_gun;
                return true;
            }
        }

        static mframe_t[] gladiator_frames_pain = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t gladiator_move_pain = new mmove_t(FRAME_pain1, FRAME_pain6, gladiator_frames_pain, gladiator_run);
        static mframe_t[] gladiator_frames_pain_air = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t gladiator_move_pain_air = new mmove_t(FRAME_painup1, FRAME_painup7, gladiator_frames_pain_air, gladiator_run);
        static EntPainAdapter gladiator_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
	{

            public override string GetID()
            {
                return "gladiator_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                if (self.health < (self.max_health / 2))
                    self.s.skinnum = 1;
                if (GameBase.level.time < self.pain_debounce_time)
                {
                    if ((self.velocity[2] > 100) && (self.monsterinfo.currentmove == gladiator_move_pain))
                        self.monsterinfo.currentmove = gladiator_move_pain_air;
                    return;
                }

                self.pain_debounce_time = GameBase.level.time + 3;
                if (Lib.Random() < 0.5)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain1, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain2, 1, Defines.ATTN_NORM, 0);
                if (GameBase.skill.value == 3)
                    return;
                if (self.velocity[2] > 100)
                    self.monsterinfo.currentmove = gladiator_move_pain_air;
                else
                    self.monsterinfo.currentmove = gladiator_move_pain;
            }
        }

        static EntThinkAdapter gladiator_dead = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_dead";
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

        static mframe_t[] gladiator_frames_death = new mframe_t[] { new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t gladiator_move_death = new mmove_t(FRAME_death1, FRAME_death22, gladiator_frames_death, gladiator_dead);
        static EntDieAdapter gladiator_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "gladiator_die";
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
                self.monsterinfo.currentmove = gladiator_move_death;
            }
        }

        public static void SP_monster_gladiator(edict_t self)
        {
            if (GameBase.deathmatch.value != 0)
            {
                GameUtil.G_FreeEdict(self);
                return;
            }

            sound_pain1 = GameBase.gi.Soundindex("gladiator/pain.wav");
            sound_pain2 = GameBase.gi.Soundindex("gladiator/gldpain2.wav");
            sound_die = GameBase.gi.Soundindex("gladiator/glddeth2.wav");
            sound_gun = GameBase.gi.Soundindex("gladiator/railgun.wav");
            sound_cleaver_swing = GameBase.gi.Soundindex("gladiator/melee1.wav");
            sound_cleaver_hit = GameBase.gi.Soundindex("gladiator/melee2.wav");
            sound_cleaver_miss = GameBase.gi.Soundindex("gladiator/melee3.wav");
            sound_idle = GameBase.gi.Soundindex("gladiator/gldidle1.wav");
            sound_search = GameBase.gi.Soundindex("gladiator/gldsrch1.wav");
            sound_sight = GameBase.gi.Soundindex("gladiator/sight.wav");
            self.movetype = Defines.MOVETYPE_STEP;
            self.solid = Defines.SOLID_BBOX;
            self.s.modelindex = GameBase.gi.Modelindex("models/monsters/gladiatr/tris.md2");
            Math3D.VectorSet(self.mins, -32, -32, -24);
            Math3D.VectorSet(self.maxs, 32, 32, 64);
            self.health = 400;
            self.gib_health = -175;
            self.mass = 400;
            self.pain = gladiator_pain;
            self.die = gladiator_die;
            self.monsterinfo.stand = gladiator_stand;
            self.monsterinfo.walk = gladiator_walk;
            self.monsterinfo.run = gladiator_run;
            self.monsterinfo.dodge = null;
            self.monsterinfo.attack = gladiator_attack;
            self.monsterinfo.melee = gladiator_melee;
            self.monsterinfo.sight = gladiator_sight;
            self.monsterinfo.idle = gladiator_idle;
            self.monsterinfo.search = gladiator_search;
            GameBase.gi.Linkentity(self);
            self.monsterinfo.currentmove = gladiator_move_stand;
            self.monsterinfo.scale = MODEL_SCALE;
            GameAI.walkmonster_start.Think(self);
        }
    }
}
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game.Monsters
{
    public class M_Soldier
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
        public static readonly int FRAME_attak201 = 12;
        public static readonly int FRAME_attak202 = 13;
        public static readonly int FRAME_attak203 = 14;
        public static readonly int FRAME_attak204 = 15;
        public static readonly int FRAME_attak205 = 16;
        public static readonly int FRAME_attak206 = 17;
        public static readonly int FRAME_attak207 = 18;
        public static readonly int FRAME_attak208 = 19;
        public static readonly int FRAME_attak209 = 20;
        public static readonly int FRAME_attak210 = 21;
        public static readonly int FRAME_attak211 = 22;
        public static readonly int FRAME_attak212 = 23;
        public static readonly int FRAME_attak213 = 24;
        public static readonly int FRAME_attak214 = 25;
        public static readonly int FRAME_attak215 = 26;
        public static readonly int FRAME_attak216 = 27;
        public static readonly int FRAME_attak217 = 28;
        public static readonly int FRAME_attak218 = 29;
        public static readonly int FRAME_attak301 = 30;
        public static readonly int FRAME_attak302 = 31;
        public static readonly int FRAME_attak303 = 32;
        public static readonly int FRAME_attak304 = 33;
        public static readonly int FRAME_attak305 = 34;
        public static readonly int FRAME_attak306 = 35;
        public static readonly int FRAME_attak307 = 36;
        public static readonly int FRAME_attak308 = 37;
        public static readonly int FRAME_attak309 = 38;
        public static readonly int FRAME_attak401 = 39;
        public static readonly int FRAME_attak402 = 40;
        public static readonly int FRAME_attak403 = 41;
        public static readonly int FRAME_attak404 = 42;
        public static readonly int FRAME_attak405 = 43;
        public static readonly int FRAME_attak406 = 44;
        public static readonly int FRAME_duck01 = 45;
        public static readonly int FRAME_duck02 = 46;
        public static readonly int FRAME_duck03 = 47;
        public static readonly int FRAME_duck04 = 48;
        public static readonly int FRAME_duck05 = 49;
        public static readonly int FRAME_pain101 = 50;
        public static readonly int FRAME_pain102 = 51;
        public static readonly int FRAME_pain103 = 52;
        public static readonly int FRAME_pain104 = 53;
        public static readonly int FRAME_pain105 = 54;
        public static readonly int FRAME_pain201 = 55;
        public static readonly int FRAME_pain202 = 56;
        public static readonly int FRAME_pain203 = 57;
        public static readonly int FRAME_pain204 = 58;
        public static readonly int FRAME_pain205 = 59;
        public static readonly int FRAME_pain206 = 60;
        public static readonly int FRAME_pain207 = 61;
        public static readonly int FRAME_pain301 = 62;
        public static readonly int FRAME_pain302 = 63;
        public static readonly int FRAME_pain303 = 64;
        public static readonly int FRAME_pain304 = 65;
        public static readonly int FRAME_pain305 = 66;
        public static readonly int FRAME_pain306 = 67;
        public static readonly int FRAME_pain307 = 68;
        public static readonly int FRAME_pain308 = 69;
        public static readonly int FRAME_pain309 = 70;
        public static readonly int FRAME_pain310 = 71;
        public static readonly int FRAME_pain311 = 72;
        public static readonly int FRAME_pain312 = 73;
        public static readonly int FRAME_pain313 = 74;
        public static readonly int FRAME_pain314 = 75;
        public static readonly int FRAME_pain315 = 76;
        public static readonly int FRAME_pain316 = 77;
        public static readonly int FRAME_pain317 = 78;
        public static readonly int FRAME_pain318 = 79;
        public static readonly int FRAME_pain401 = 80;
        public static readonly int FRAME_pain402 = 81;
        public static readonly int FRAME_pain403 = 82;
        public static readonly int FRAME_pain404 = 83;
        public static readonly int FRAME_pain405 = 84;
        public static readonly int FRAME_pain406 = 85;
        public static readonly int FRAME_pain407 = 86;
        public static readonly int FRAME_pain408 = 87;
        public static readonly int FRAME_pain409 = 88;
        public static readonly int FRAME_pain410 = 89;
        public static readonly int FRAME_pain411 = 90;
        public static readonly int FRAME_pain412 = 91;
        public static readonly int FRAME_pain413 = 92;
        public static readonly int FRAME_pain414 = 93;
        public static readonly int FRAME_pain415 = 94;
        public static readonly int FRAME_pain416 = 95;
        public static readonly int FRAME_pain417 = 96;
        public static readonly int FRAME_run01 = 97;
        public static readonly int FRAME_run02 = 98;
        public static readonly int FRAME_run03 = 99;
        public static readonly int FRAME_run04 = 100;
        public static readonly int FRAME_run05 = 101;
        public static readonly int FRAME_run06 = 102;
        public static readonly int FRAME_run07 = 103;
        public static readonly int FRAME_run08 = 104;
        public static readonly int FRAME_run09 = 105;
        public static readonly int FRAME_run10 = 106;
        public static readonly int FRAME_run11 = 107;
        public static readonly int FRAME_run12 = 108;
        public static readonly int FRAME_runs01 = 109;
        public static readonly int FRAME_runs02 = 110;
        public static readonly int FRAME_runs03 = 111;
        public static readonly int FRAME_runs04 = 112;
        public static readonly int FRAME_runs05 = 113;
        public static readonly int FRAME_runs06 = 114;
        public static readonly int FRAME_runs07 = 115;
        public static readonly int FRAME_runs08 = 116;
        public static readonly int FRAME_runs09 = 117;
        public static readonly int FRAME_runs10 = 118;
        public static readonly int FRAME_runs11 = 119;
        public static readonly int FRAME_runs12 = 120;
        public static readonly int FRAME_runs13 = 121;
        public static readonly int FRAME_runs14 = 122;
        public static readonly int FRAME_runs15 = 123;
        public static readonly int FRAME_runs16 = 124;
        public static readonly int FRAME_runs17 = 125;
        public static readonly int FRAME_runs18 = 126;
        public static readonly int FRAME_runt01 = 127;
        public static readonly int FRAME_runt02 = 128;
        public static readonly int FRAME_runt03 = 129;
        public static readonly int FRAME_runt04 = 130;
        public static readonly int FRAME_runt05 = 131;
        public static readonly int FRAME_runt06 = 132;
        public static readonly int FRAME_runt07 = 133;
        public static readonly int FRAME_runt08 = 134;
        public static readonly int FRAME_runt09 = 135;
        public static readonly int FRAME_runt10 = 136;
        public static readonly int FRAME_runt11 = 137;
        public static readonly int FRAME_runt12 = 138;
        public static readonly int FRAME_runt13 = 139;
        public static readonly int FRAME_runt14 = 140;
        public static readonly int FRAME_runt15 = 141;
        public static readonly int FRAME_runt16 = 142;
        public static readonly int FRAME_runt17 = 143;
        public static readonly int FRAME_runt18 = 144;
        public static readonly int FRAME_runt19 = 145;
        public static readonly int FRAME_stand101 = 146;
        public static readonly int FRAME_stand102 = 147;
        public static readonly int FRAME_stand103 = 148;
        public static readonly int FRAME_stand104 = 149;
        public static readonly int FRAME_stand105 = 150;
        public static readonly int FRAME_stand106 = 151;
        public static readonly int FRAME_stand107 = 152;
        public static readonly int FRAME_stand108 = 153;
        public static readonly int FRAME_stand109 = 154;
        public static readonly int FRAME_stand110 = 155;
        public static readonly int FRAME_stand111 = 156;
        public static readonly int FRAME_stand112 = 157;
        public static readonly int FRAME_stand113 = 158;
        public static readonly int FRAME_stand114 = 159;
        public static readonly int FRAME_stand115 = 160;
        public static readonly int FRAME_stand116 = 161;
        public static readonly int FRAME_stand117 = 162;
        public static readonly int FRAME_stand118 = 163;
        public static readonly int FRAME_stand119 = 164;
        public static readonly int FRAME_stand120 = 165;
        public static readonly int FRAME_stand121 = 166;
        public static readonly int FRAME_stand122 = 167;
        public static readonly int FRAME_stand123 = 168;
        public static readonly int FRAME_stand124 = 169;
        public static readonly int FRAME_stand125 = 170;
        public static readonly int FRAME_stand126 = 171;
        public static readonly int FRAME_stand127 = 172;
        public static readonly int FRAME_stand128 = 173;
        public static readonly int FRAME_stand129 = 174;
        public static readonly int FRAME_stand130 = 175;
        public static readonly int FRAME_stand301 = 176;
        public static readonly int FRAME_stand302 = 177;
        public static readonly int FRAME_stand303 = 178;
        public static readonly int FRAME_stand304 = 179;
        public static readonly int FRAME_stand305 = 180;
        public static readonly int FRAME_stand306 = 181;
        public static readonly int FRAME_stand307 = 182;
        public static readonly int FRAME_stand308 = 183;
        public static readonly int FRAME_stand309 = 184;
        public static readonly int FRAME_stand310 = 185;
        public static readonly int FRAME_stand311 = 186;
        public static readonly int FRAME_stand312 = 187;
        public static readonly int FRAME_stand313 = 188;
        public static readonly int FRAME_stand314 = 189;
        public static readonly int FRAME_stand315 = 190;
        public static readonly int FRAME_stand316 = 191;
        public static readonly int FRAME_stand317 = 192;
        public static readonly int FRAME_stand318 = 193;
        public static readonly int FRAME_stand319 = 194;
        public static readonly int FRAME_stand320 = 195;
        public static readonly int FRAME_stand321 = 196;
        public static readonly int FRAME_stand322 = 197;
        public static readonly int FRAME_stand323 = 198;
        public static readonly int FRAME_stand324 = 199;
        public static readonly int FRAME_stand325 = 200;
        public static readonly int FRAME_stand326 = 201;
        public static readonly int FRAME_stand327 = 202;
        public static readonly int FRAME_stand328 = 203;
        public static readonly int FRAME_stand329 = 204;
        public static readonly int FRAME_stand330 = 205;
        public static readonly int FRAME_stand331 = 206;
        public static readonly int FRAME_stand332 = 207;
        public static readonly int FRAME_stand333 = 208;
        public static readonly int FRAME_stand334 = 209;
        public static readonly int FRAME_stand335 = 210;
        public static readonly int FRAME_stand336 = 211;
        public static readonly int FRAME_stand337 = 212;
        public static readonly int FRAME_stand338 = 213;
        public static readonly int FRAME_stand339 = 214;
        public static readonly int FRAME_walk101 = 215;
        public static readonly int FRAME_walk102 = 216;
        public static readonly int FRAME_walk103 = 217;
        public static readonly int FRAME_walk104 = 218;
        public static readonly int FRAME_walk105 = 219;
        public static readonly int FRAME_walk106 = 220;
        public static readonly int FRAME_walk107 = 221;
        public static readonly int FRAME_walk108 = 222;
        public static readonly int FRAME_walk109 = 223;
        public static readonly int FRAME_walk110 = 224;
        public static readonly int FRAME_walk111 = 225;
        public static readonly int FRAME_walk112 = 226;
        public static readonly int FRAME_walk113 = 227;
        public static readonly int FRAME_walk114 = 228;
        public static readonly int FRAME_walk115 = 229;
        public static readonly int FRAME_walk116 = 230;
        public static readonly int FRAME_walk117 = 231;
        public static readonly int FRAME_walk118 = 232;
        public static readonly int FRAME_walk119 = 233;
        public static readonly int FRAME_walk120 = 234;
        public static readonly int FRAME_walk121 = 235;
        public static readonly int FRAME_walk122 = 236;
        public static readonly int FRAME_walk123 = 237;
        public static readonly int FRAME_walk124 = 238;
        public static readonly int FRAME_walk125 = 239;
        public static readonly int FRAME_walk126 = 240;
        public static readonly int FRAME_walk127 = 241;
        public static readonly int FRAME_walk128 = 242;
        public static readonly int FRAME_walk129 = 243;
        public static readonly int FRAME_walk130 = 244;
        public static readonly int FRAME_walk131 = 245;
        public static readonly int FRAME_walk132 = 246;
        public static readonly int FRAME_walk133 = 247;
        public static readonly int FRAME_walk201 = 248;
        public static readonly int FRAME_walk202 = 249;
        public static readonly int FRAME_walk203 = 250;
        public static readonly int FRAME_walk204 = 251;
        public static readonly int FRAME_walk205 = 252;
        public static readonly int FRAME_walk206 = 253;
        public static readonly int FRAME_walk207 = 254;
        public static readonly int FRAME_walk208 = 255;
        public static readonly int FRAME_walk209 = 256;
        public static readonly int FRAME_walk210 = 257;
        public static readonly int FRAME_walk211 = 258;
        public static readonly int FRAME_walk212 = 259;
        public static readonly int FRAME_walk213 = 260;
        public static readonly int FRAME_walk214 = 261;
        public static readonly int FRAME_walk215 = 262;
        public static readonly int FRAME_walk216 = 263;
        public static readonly int FRAME_walk217 = 264;
        public static readonly int FRAME_walk218 = 265;
        public static readonly int FRAME_walk219 = 266;
        public static readonly int FRAME_walk220 = 267;
        public static readonly int FRAME_walk221 = 268;
        public static readonly int FRAME_walk222 = 269;
        public static readonly int FRAME_walk223 = 270;
        public static readonly int FRAME_walk224 = 271;
        public static readonly int FRAME_death101 = 272;
        public static readonly int FRAME_death102 = 273;
        public static readonly int FRAME_death103 = 274;
        public static readonly int FRAME_death104 = 275;
        public static readonly int FRAME_death105 = 276;
        public static readonly int FRAME_death106 = 277;
        public static readonly int FRAME_death107 = 278;
        public static readonly int FRAME_death108 = 279;
        public static readonly int FRAME_death109 = 280;
        public static readonly int FRAME_death110 = 281;
        public static readonly int FRAME_death111 = 282;
        public static readonly int FRAME_death112 = 283;
        public static readonly int FRAME_death113 = 284;
        public static readonly int FRAME_death114 = 285;
        public static readonly int FRAME_death115 = 286;
        public static readonly int FRAME_death116 = 287;
        public static readonly int FRAME_death117 = 288;
        public static readonly int FRAME_death118 = 289;
        public static readonly int FRAME_death119 = 290;
        public static readonly int FRAME_death120 = 291;
        public static readonly int FRAME_death121 = 292;
        public static readonly int FRAME_death122 = 293;
        public static readonly int FRAME_death123 = 294;
        public static readonly int FRAME_death124 = 295;
        public static readonly int FRAME_death125 = 296;
        public static readonly int FRAME_death126 = 297;
        public static readonly int FRAME_death127 = 298;
        public static readonly int FRAME_death128 = 299;
        public static readonly int FRAME_death129 = 300;
        public static readonly int FRAME_death130 = 301;
        public static readonly int FRAME_death131 = 302;
        public static readonly int FRAME_death132 = 303;
        public static readonly int FRAME_death133 = 304;
        public static readonly int FRAME_death134 = 305;
        public static readonly int FRAME_death135 = 306;
        public static readonly int FRAME_death136 = 307;
        public static readonly int FRAME_death201 = 308;
        public static readonly int FRAME_death202 = 309;
        public static readonly int FRAME_death203 = 310;
        public static readonly int FRAME_death204 = 311;
        public static readonly int FRAME_death205 = 312;
        public static readonly int FRAME_death206 = 313;
        public static readonly int FRAME_death207 = 314;
        public static readonly int FRAME_death208 = 315;
        public static readonly int FRAME_death209 = 316;
        public static readonly int FRAME_death210 = 317;
        public static readonly int FRAME_death211 = 318;
        public static readonly int FRAME_death212 = 319;
        public static readonly int FRAME_death213 = 320;
        public static readonly int FRAME_death214 = 321;
        public static readonly int FRAME_death215 = 322;
        public static readonly int FRAME_death216 = 323;
        public static readonly int FRAME_death217 = 324;
        public static readonly int FRAME_death218 = 325;
        public static readonly int FRAME_death219 = 326;
        public static readonly int FRAME_death220 = 327;
        public static readonly int FRAME_death221 = 328;
        public static readonly int FRAME_death222 = 329;
        public static readonly int FRAME_death223 = 330;
        public static readonly int FRAME_death224 = 331;
        public static readonly int FRAME_death225 = 332;
        public static readonly int FRAME_death226 = 333;
        public static readonly int FRAME_death227 = 334;
        public static readonly int FRAME_death228 = 335;
        public static readonly int FRAME_death229 = 336;
        public static readonly int FRAME_death230 = 337;
        public static readonly int FRAME_death231 = 338;
        public static readonly int FRAME_death232 = 339;
        public static readonly int FRAME_death233 = 340;
        public static readonly int FRAME_death234 = 341;
        public static readonly int FRAME_death235 = 342;
        public static readonly int FRAME_death301 = 343;
        public static readonly int FRAME_death302 = 344;
        public static readonly int FRAME_death303 = 345;
        public static readonly int FRAME_death304 = 346;
        public static readonly int FRAME_death305 = 347;
        public static readonly int FRAME_death306 = 348;
        public static readonly int FRAME_death307 = 349;
        public static readonly int FRAME_death308 = 350;
        public static readonly int FRAME_death309 = 351;
        public static readonly int FRAME_death310 = 352;
        public static readonly int FRAME_death311 = 353;
        public static readonly int FRAME_death312 = 354;
        public static readonly int FRAME_death313 = 355;
        public static readonly int FRAME_death314 = 356;
        public static readonly int FRAME_death315 = 357;
        public static readonly int FRAME_death316 = 358;
        public static readonly int FRAME_death317 = 359;
        public static readonly int FRAME_death318 = 360;
        public static readonly int FRAME_death319 = 361;
        public static readonly int FRAME_death320 = 362;
        public static readonly int FRAME_death321 = 363;
        public static readonly int FRAME_death322 = 364;
        public static readonly int FRAME_death323 = 365;
        public static readonly int FRAME_death324 = 366;
        public static readonly int FRAME_death325 = 367;
        public static readonly int FRAME_death326 = 368;
        public static readonly int FRAME_death327 = 369;
        public static readonly int FRAME_death328 = 370;
        public static readonly int FRAME_death329 = 371;
        public static readonly int FRAME_death330 = 372;
        public static readonly int FRAME_death331 = 373;
        public static readonly int FRAME_death332 = 374;
        public static readonly int FRAME_death333 = 375;
        public static readonly int FRAME_death334 = 376;
        public static readonly int FRAME_death335 = 377;
        public static readonly int FRAME_death336 = 378;
        public static readonly int FRAME_death337 = 379;
        public static readonly int FRAME_death338 = 380;
        public static readonly int FRAME_death339 = 381;
        public static readonly int FRAME_death340 = 382;
        public static readonly int FRAME_death341 = 383;
        public static readonly int FRAME_death342 = 384;
        public static readonly int FRAME_death343 = 385;
        public static readonly int FRAME_death344 = 386;
        public static readonly int FRAME_death345 = 387;
        public static readonly int FRAME_death401 = 388;
        public static readonly int FRAME_death402 = 389;
        public static readonly int FRAME_death403 = 390;
        public static readonly int FRAME_death404 = 391;
        public static readonly int FRAME_death405 = 392;
        public static readonly int FRAME_death406 = 393;
        public static readonly int FRAME_death407 = 394;
        public static readonly int FRAME_death408 = 395;
        public static readonly int FRAME_death409 = 396;
        public static readonly int FRAME_death410 = 397;
        public static readonly int FRAME_death411 = 398;
        public static readonly int FRAME_death412 = 399;
        public static readonly int FRAME_death413 = 400;
        public static readonly int FRAME_death414 = 401;
        public static readonly int FRAME_death415 = 402;
        public static readonly int FRAME_death416 = 403;
        public static readonly int FRAME_death417 = 404;
        public static readonly int FRAME_death418 = 405;
        public static readonly int FRAME_death419 = 406;
        public static readonly int FRAME_death420 = 407;
        public static readonly int FRAME_death421 = 408;
        public static readonly int FRAME_death422 = 409;
        public static readonly int FRAME_death423 = 410;
        public static readonly int FRAME_death424 = 411;
        public static readonly int FRAME_death425 = 412;
        public static readonly int FRAME_death426 = 413;
        public static readonly int FRAME_death427 = 414;
        public static readonly int FRAME_death428 = 415;
        public static readonly int FRAME_death429 = 416;
        public static readonly int FRAME_death430 = 417;
        public static readonly int FRAME_death431 = 418;
        public static readonly int FRAME_death432 = 419;
        public static readonly int FRAME_death433 = 420;
        public static readonly int FRAME_death434 = 421;
        public static readonly int FRAME_death435 = 422;
        public static readonly int FRAME_death436 = 423;
        public static readonly int FRAME_death437 = 424;
        public static readonly int FRAME_death438 = 425;
        public static readonly int FRAME_death439 = 426;
        public static readonly int FRAME_death440 = 427;
        public static readonly int FRAME_death441 = 428;
        public static readonly int FRAME_death442 = 429;
        public static readonly int FRAME_death443 = 430;
        public static readonly int FRAME_death444 = 431;
        public static readonly int FRAME_death445 = 432;
        public static readonly int FRAME_death446 = 433;
        public static readonly int FRAME_death447 = 434;
        public static readonly int FRAME_death448 = 435;
        public static readonly int FRAME_death449 = 436;
        public static readonly int FRAME_death450 = 437;
        public static readonly int FRAME_death451 = 438;
        public static readonly int FRAME_death452 = 439;
        public static readonly int FRAME_death453 = 440;
        public static readonly int FRAME_death501 = 441;
        public static readonly int FRAME_death502 = 442;
        public static readonly int FRAME_death503 = 443;
        public static readonly int FRAME_death504 = 444;
        public static readonly int FRAME_death505 = 445;
        public static readonly int FRAME_death506 = 446;
        public static readonly int FRAME_death507 = 447;
        public static readonly int FRAME_death508 = 448;
        public static readonly int FRAME_death509 = 449;
        public static readonly int FRAME_death510 = 450;
        public static readonly int FRAME_death511 = 451;
        public static readonly int FRAME_death512 = 452;
        public static readonly int FRAME_death513 = 453;
        public static readonly int FRAME_death514 = 454;
        public static readonly int FRAME_death515 = 455;
        public static readonly int FRAME_death516 = 456;
        public static readonly int FRAME_death517 = 457;
        public static readonly int FRAME_death518 = 458;
        public static readonly int FRAME_death519 = 459;
        public static readonly int FRAME_death520 = 460;
        public static readonly int FRAME_death521 = 461;
        public static readonly int FRAME_death522 = 462;
        public static readonly int FRAME_death523 = 463;
        public static readonly int FRAME_death524 = 464;
        public static readonly int FRAME_death601 = 465;
        public static readonly int FRAME_death602 = 466;
        public static readonly int FRAME_death603 = 467;
        public static readonly int FRAME_death604 = 468;
        public static readonly int FRAME_death605 = 469;
        public static readonly int FRAME_death606 = 470;
        public static readonly int FRAME_death607 = 471;
        public static readonly int FRAME_death608 = 472;
        public static readonly int FRAME_death609 = 473;
        public static readonly int FRAME_death610 = 474;
        public static readonly float MODEL_SCALE = 1.2F;
        static int sound_idle;
        static int sound_sight1;
        static int sound_sight2;
        static int sound_pain_light;
        static int sound_pain;
        static int sound_pain_ss;
        static int sound_death_light;
        static int sound_death;
        static int sound_death_ss;
        static int sound_cock;
        static EntThinkAdapter soldier_dead = new AnonymousEntThinkAdapter();
        private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_dead";
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

        static EntDieAdapter soldier_die = new AnonymousEntDieAdapter();
        private sealed class AnonymousEntDieAdapter : EntDieAdapter
		{
			
            public override string GetID()
            {
                return "soldier_die";
            }

            public override void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point)
            {
                int n;
                if (self.health <= self.gib_health)
                {
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, GameBase.gi.Soundindex("misc/udeath.wav"), 1, Defines.ATTN_NORM, 0);
                    for (n = 0; n < 3; n++)
                        GameMisc.ThrowGib(self, "models/objects/gibs/sm_meat/tris.md2", damage, Defines.GIB_ORGANIC);
                    GameMisc.ThrowGib(self, "models/objects/gibs/chest/tris.md2", damage, Defines.GIB_ORGANIC);
                    GameMisc.ThrowHead(self, "models/objects/gibs/head2/tris.md2", damage, Defines.GIB_ORGANIC);
                    self.deadflag = Defines.DEAD_DEAD;
                    return;
                }

                if (self.deadflag == Defines.DEAD_DEAD)
                    return;
                self.deadflag = Defines.DEAD_DEAD;
                self.takedamage = Defines.DAMAGE_YES;
                self.s.skinnum |= 1;
                if (self.s.skinnum == 1)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death_light, 1, Defines.ATTN_NORM, 0);
                else if (self.s.skinnum == 3)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_death_ss, 1, Defines.ATTN_NORM, 0);
                if (Math.Abs((self.s.origin[2] + self.viewheight) - point[2]) <= 4)
                {
                    self.monsterinfo.currentmove = soldier_move_death3;
                    return;
                }

                n = Lib.Rand() % 5;
                if (n == 0)
                    self.monsterinfo.currentmove = soldier_move_death1;
                else if (n == 1)
                    self.monsterinfo.currentmove = soldier_move_death2;
                else if (n == 2)
                    self.monsterinfo.currentmove = soldier_move_death4;
                else if (n == 3)
                    self.monsterinfo.currentmove = soldier_move_death5;
                else
                    self.monsterinfo.currentmove = soldier_move_death6;
            }
        }

        static EntThinkAdapter soldier_attack1_refire1 = new AnonymousEntThinkAdapter1();
        private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_attack1_refire1";
            }

            public override bool Think(edict_t self)
            {
                if (self.s.skinnum > 1)
                    return true;
                if (self.enemy.health <= 0)
                    return true;
                if (((GameBase.skill.value == 3) && (Lib.Random() < 0.5)) || (GameUtil.Range(self, self.enemy) == Defines.RANGE_MELEE))
                    self.monsterinfo.nextframe = FRAME_attak102;
                else
                    self.monsterinfo.nextframe = FRAME_attak110;
                return true;
            }
        }

        static EntThinkAdapter soldier_attack1_refire2 = new AnonymousEntThinkAdapter2();
        private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_attack1_refire2";
            }

            public override bool Think(edict_t self)
            {
                if (self.s.skinnum < 2)
                    return true;
                if (self.enemy.health <= 0)
                    return true;
                if (((GameBase.skill.value == 3) && (Lib.Random() < 0.5)) || (GameUtil.Range(self, self.enemy) == Defines.RANGE_MELEE))
                    self.monsterinfo.nextframe = FRAME_attak102;
                return true;
            }
        }

        static EntThinkAdapter soldier_attack2_refire1 = new AnonymousEntThinkAdapter3();
        private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_attack2_refire1";
            }

            public override bool Think(edict_t self)
            {
                if (self.s.skinnum > 1)
                    return true;
                if (self.enemy.health <= 0)
                    return true;
                if (((GameBase.skill.value == 3) && (Lib.Random() < 0.5)) || (GameUtil.Range(self, self.enemy) == Defines.RANGE_MELEE))
                    self.monsterinfo.nextframe = FRAME_attak204;
                else
                    self.monsterinfo.nextframe = FRAME_attak216;
                return true;
            }
        }

        static EntThinkAdapter soldier_attack2_refire2 = new AnonymousEntThinkAdapter4();
        private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_attack2_refire2";
            }

            public override bool Think(edict_t self)
            {
                if (self.s.skinnum < 2)
                    return true;
                if (self.enemy.health <= 0)
                    return true;
                if (((GameBase.skill.value == 3) && (Lib.Random() < 0.5)) || (GameUtil.Range(self, self.enemy) == Defines.RANGE_MELEE))
                    self.monsterinfo.nextframe = FRAME_attak204;
                return true;
            }
        }

        static EntThinkAdapter soldier_attack3_refire = new AnonymousEntThinkAdapter5();
        private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_attack3_refire";
            }

            public override bool Think(edict_t self)
            {
                if ((GameBase.level.time + 0.4) < self.monsterinfo.pausetime)
                    self.monsterinfo.nextframe = FRAME_attak303;
                return true;
            }
        }

        static EntThinkAdapter soldier_attack6_refire = new AnonymousEntThinkAdapter6();
        private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_attack6_refire";
            }

            public override bool Think(edict_t self)
            {
                if (self.enemy.health <= 0)
                    return true;
                if (GameUtil.Range(self, self.enemy) < Defines.RANGE_MID)
                    return true;
                if (GameBase.skill.value == 3)
                    self.monsterinfo.nextframe = FRAME_runs03;
                return true;
            }
        }

        static EntThinkAdapter soldier_fire8 = new AnonymousEntThinkAdapter7();
        private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_fire8";
            }

            public override bool Think(edict_t self)
            {
                Soldier_fire(self, 7);
                return true;
            }
        }

        static EntThinkAdapter soldier_fire1 = new AnonymousEntThinkAdapter8();
        private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_fire1";
            }

            public override bool Think(edict_t self)
            {
                Soldier_fire(self, 0);
                return true;
            }
        }

        static EntThinkAdapter soldier_fire2 = new AnonymousEntThinkAdapter9();
        private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_fire2";
            }

            public override bool Think(edict_t self)
            {
                Soldier_fire(self, 1);
                return true;
            }
        }

        static EntThinkAdapter soldier_duck_down = new AnonymousEntThinkAdapter10();
        private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_duck_down";
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

        static EntThinkAdapter soldier_fire3 = new AnonymousEntThinkAdapter11();
        private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_fire3";
            }

            public override bool Think(edict_t self)
            {
                soldier_duck_down.Think(self);
                Soldier_fire(self, 2);
                return true;
            }
        }

        static EntThinkAdapter soldier_fire4 = new AnonymousEntThinkAdapter12();
        private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_fire4";
            }

            public override bool Think(edict_t self)
            {
                Soldier_fire(self, 3);
                return true;
            }
        }

        static EntThinkAdapter soldier_fire6 = new AnonymousEntThinkAdapter13();
        private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_fire6";
            }

            public override bool Think(edict_t self)
            {
                Soldier_fire(self, 5);
                return true;
            }
        }

        static EntThinkAdapter soldier_fire7 = new AnonymousEntThinkAdapter14();
        private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_fire7";
            }

            public override bool Think(edict_t self)
            {
                Soldier_fire(self, 6);
                return true;
            }
        }

        static EntThinkAdapter soldier_idle = new AnonymousEntThinkAdapter15();
        private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_idle";
            }

            public override bool Think(edict_t self)
            {
                if (Lib.Random() > 0.8)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_idle, 1, Defines.ATTN_IDLE, 0);
                return true;
            }
        }

        static EntThinkAdapter soldier_stand = new AnonymousEntThinkAdapter16();
        private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_stand";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.currentmove == soldier_move_stand3) || (Lib.Random() < 0.8))
                    self.monsterinfo.currentmove = soldier_move_stand1;
                else
                    self.monsterinfo.currentmove = soldier_move_stand3;
                return true;
            }
        }

        static EntThinkAdapter soldier_walk1_random = new AnonymousEntThinkAdapter17();
        private sealed class AnonymousEntThinkAdapter17 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_walk1_random";
            }

            public override bool Think(edict_t self)
            {
                if (Lib.Random() > 0.1)
                    self.monsterinfo.nextframe = FRAME_walk101;
                return true;
            }
        }

        static EntThinkAdapter soldier_walk = new AnonymousEntThinkAdapter18();
        private sealed class AnonymousEntThinkAdapter18 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_walk";
            }

            public override bool Think(edict_t self)
            {
                if (Lib.Random() < 0.5)
                    self.monsterinfo.currentmove = soldier_move_walk1;
                else
                    self.monsterinfo.currentmove = soldier_move_walk2;
                return true;
            }
        }

        static EntThinkAdapter soldier_run = new AnonymousEntThinkAdapter19();
        private sealed class AnonymousEntThinkAdapter19 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_run";
            }

            public override bool Think(edict_t self)
            {
                if ((self.monsterinfo.aiflags & Defines.AI_STAND_GROUND) != 0)
                {
                    self.monsterinfo.currentmove = soldier_move_stand1;
                    return true;
                }

                if (self.monsterinfo.currentmove == soldier_move_walk1 || self.monsterinfo.currentmove == soldier_move_walk2 || self.monsterinfo.currentmove == soldier_move_start_run)
                {
                    self.monsterinfo.currentmove = soldier_move_run;
                }
                else
                {
                    self.monsterinfo.currentmove = soldier_move_start_run;
                }

                return true;
            }
        }

        static EntPainAdapter soldier_pain = new AnonymousEntPainAdapter();
        private sealed class AnonymousEntPainAdapter : EntPainAdapter
		{
			
            public override string GetID()
            {
                return "soldier_pain";
            }

            public override void Pain(edict_t self, edict_t other, float kick, int damage)
            {
                float r;
                int n;
                if (self.health < (self.max_health / 2))
                    self.s.skinnum |= 1;
                if (GameBase.level.time < self.pain_debounce_time)
                {
                    if ((self.velocity[2] > 100) && ((self.monsterinfo.currentmove == soldier_move_pain1) || (self.monsterinfo.currentmove == soldier_move_pain2) || (self.monsterinfo.currentmove == soldier_move_pain3)))
                        self.monsterinfo.currentmove = soldier_move_pain4;
                    return;
                }

                self.pain_debounce_time = GameBase.level.time + 3;
                n = self.s.skinnum | 1;
                if (n == 1)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain_light, 1, Defines.ATTN_NORM, 0);
                else if (n == 3)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_pain_ss, 1, Defines.ATTN_NORM, 0);
                if (self.velocity[2] > 100)
                {
                    self.monsterinfo.currentmove = soldier_move_pain4;
                    return;
                }

                if (GameBase.skill.value == 3)
                    return;
                r = Lib.Random();
                if (r < 0.33)
                    self.monsterinfo.currentmove = soldier_move_pain1;
                else if (r < 0.66)
                    self.monsterinfo.currentmove = soldier_move_pain2;
                else
                    self.monsterinfo.currentmove = soldier_move_pain3;
            }
        }

        static EntThinkAdapter soldier_duck_up = new AnonymousEntThinkAdapter20();
        private sealed class AnonymousEntThinkAdapter20 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_duck_up";
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

        static EntInteractAdapter soldier_sight = new AnonymousEntInteractAdapter();
        private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{
			
            public override string GetID()
            {
                return "soldier_sight";
            }

            public override bool Interact(edict_t self, edict_t other)
            {
                if (Lib.Random() < 0.5)
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight1, 1, Defines.ATTN_NORM, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_VOICE, sound_sight2, 1, Defines.ATTN_NORM, 0);
                if ((GameBase.skill.value > 0) && (GameUtil.Range(self, self.enemy) >= Defines.RANGE_MID))
                {
                    if (Lib.Random() > 0.5)
                        self.monsterinfo.currentmove = soldier_move_attack6;
                }

                return true;
            }
        }

        static EntThinkAdapter SP_monster_soldier_x = new AnonymousEntThinkAdapter21();
        private sealed class AnonymousEntThinkAdapter21 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "SP_monster_soldier_x";
            }

            public override bool Think(edict_t self)
            {
                self.s.modelindex = GameBase.gi.Modelindex("models/monsters/soldier/tris.md2");
                self.monsterinfo.scale = MODEL_SCALE;
                Math3D.VectorSet(self.mins, -16, -16, -24);
                Math3D.VectorSet(self.maxs, 16, 16, 32);
                self.movetype = Defines.MOVETYPE_STEP;
                self.solid = Defines.SOLID_BBOX;
                sound_idle = GameBase.gi.Soundindex("soldier/solidle1.wav");
                sound_sight1 = GameBase.gi.Soundindex("soldier/solsght1.wav");
                sound_sight2 = GameBase.gi.Soundindex("soldier/solsrch1.wav");
                sound_cock = GameBase.gi.Soundindex("infantry/infatck3.wav");
                self.mass = 100;
                self.pain = soldier_pain;
                self.die = soldier_die;
                self.monsterinfo.stand = soldier_stand;
                self.monsterinfo.walk = soldier_walk;
                self.monsterinfo.run = soldier_run;
                self.monsterinfo.dodge = soldier_dodge;
                self.monsterinfo.attack = soldier_attack;
                self.monsterinfo.melee = null;
                self.monsterinfo.sight = soldier_sight;
                GameBase.gi.Linkentity(self);
                self.monsterinfo.stand.Think(self);
                GameAI.walkmonster_start.Think(self);
                return true;
            }
        }

        public static EntThinkAdapter SP_monster_soldier_light = new AnonymousEntThinkAdapter22();
        private sealed class AnonymousEntThinkAdapter22 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "SP_monster_soldier_light";
            }

            public override bool Think(edict_t self)
            {
                if (GameBase.deathmatch.value != 0)
                {
                    GameUtil.G_FreeEdict(self);
                    return true;
                }

                SP_monster_soldier_x.Think(self);
                sound_pain_light = GameBase.gi.Soundindex("soldier/solpain2.wav");
                sound_death_light = GameBase.gi.Soundindex("soldier/soldeth2.wav");
                GameBase.gi.Modelindex("models/objects/laser/tris.md2");
                GameBase.gi.Soundindex("misc/lasfly.wav");
                GameBase.gi.Soundindex("soldier/solatck2.wav");
                self.s.skinnum = 0;
                self.health = 20;
                self.gib_health = -30;
                return true;
            }
        }

        public static EntThinkAdapter SP_monster_soldier = new AnonymousEntThinkAdapter23();
        private sealed class AnonymousEntThinkAdapter23 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "SP_monster_soldier";
            }

            public override bool Think(edict_t self)
            {
                Com.DPrintf("Spawning a soldier at " + self.s.origin[0] + " " + self.s.origin[1] + " " + self.s.origin[2] + " " + "\\n");
                if (GameBase.deathmatch.value != 0)
                {
                    GameUtil.G_FreeEdict(self);
                    return true;
                }

                SP_monster_soldier_x.Think(self);
                sound_pain = GameBase.gi.Soundindex("soldier/solpain1.wav");
                sound_death = GameBase.gi.Soundindex("soldier/soldeth1.wav");
                GameBase.gi.Soundindex("soldier/solatck1.wav");
                self.s.skinnum = 2;
                self.health = 30;
                self.gib_health = -30;
                return true;
            }
        }

        public static EntThinkAdapter SP_monster_soldier_ss = new AnonymousEntThinkAdapter24();
        private sealed class AnonymousEntThinkAdapter24 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "SP_monster_soldier_ss";
            }

            public override bool Think(edict_t self)
            {
                if (GameBase.deathmatch.value != 0)
                {
                    GameUtil.G_FreeEdict(self);
                    return true;
                }

                SP_monster_soldier_x.Think(self);
                sound_pain_ss = GameBase.gi.Soundindex("soldier/solpain3.wav");
                sound_death_ss = GameBase.gi.Soundindex("soldier/soldeth3.wav");
                GameBase.gi.Soundindex("soldier/solatck3.wav");
                self.s.skinnum = 4;
                self.health = 40;
                self.gib_health = -30;
                return true;
            }
        }

        static void Soldier_fire(edict_t self, int flash_number)
        {
            float[] start = new float[]{0, 0, 0};
            float[] forward = new float[]{0, 0, 0}, right = new float[]{0, 0, 0}, up = new float[]{0, 0, 0};
            float[] aim = new float[]{0, 0, 0};
            float[] dir = new float[]{0, 0, 0};
            float[] end = new float[]{0, 0, 0};
            float r, u;
            int flash_index;
            if (self.s.skinnum < 2)
                flash_index = blaster_flash[flash_number];
            else if (self.s.skinnum < 4)
                flash_index = shotgun_flash[flash_number];
            else
                flash_index = machinegun_flash[flash_number];
            Math3D.AngleVectors(self.s.angles, forward, right, null);
            Math3D.G_ProjectSource(self.s.origin, M_Flash.monster_flash_offset[flash_index], forward, right, start);
            if (flash_number == 5 || flash_number == 6)
            {
                Math3D.VectorCopy(forward, aim);
            }
            else
            {
                Math3D.VectorCopy(self.enemy.s.origin, end);
                end[2] += self.enemy.viewheight;
                Math3D.VectorSubtract(end, start, aim);
                Math3D.Vectoangles(aim, dir);
                Math3D.AngleVectors(dir, forward, right, up);
                r = Lib.Crandom() * 1000;
                u = Lib.Crandom() * 500;
                Math3D.VectorMA(start, 8192, forward, end);
                Math3D.VectorMA(end, r, right, end);
                Math3D.VectorMA(end, u, up, end);
                Math3D.VectorSubtract(end, start, aim);
                Math3D.VectorNormalize(aim);
            }

            if (self.s.skinnum <= 1)
            {
                Monster.Monster_fire_blaster(self, start, aim, 5, 600, flash_index, Defines.EF_BLASTER);
            }
            else if (self.s.skinnum <= 3)
            {
                Monster.Monster_fire_shotgun(self, start, aim, 2, 1, Defines.DEFAULT_SHOTGUN_HSPREAD, Defines.DEFAULT_SHOTGUN_VSPREAD, Defines.DEFAULT_SHOTGUN_COUNT, flash_index);
            }
            else
            {
                if (0 == (self.monsterinfo.aiflags & Defines.AI_HOLD_FRAME))
                    self.monsterinfo.pausetime = GameBase.level.time + (3 + Lib.Rand() % 8) * Defines.FRAMETIME;
                Monster.Monster_fire_bullet(self, start, aim, 2, 4, Defines.DEFAULT_BULLET_HSPREAD, Defines.DEFAULT_BULLET_VSPREAD, flash_index);
                if (GameBase.level.time >= self.monsterinfo.pausetime)
                    self.monsterinfo.aiflags &= ~Defines.AI_HOLD_FRAME;
                else
                    self.monsterinfo.aiflags |= Defines.AI_HOLD_FRAME;
            }
        }

        static EntThinkAdapter soldier_cock = new AnonymousEntThinkAdapter25();
        private sealed class AnonymousEntThinkAdapter25 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_cock";
            }

            public override bool Think(edict_t self)
            {
                if (self.s.frame == FRAME_stand322)
                    GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_cock, 1, Defines.ATTN_IDLE, 0);
                else
                    GameBase.gi.Sound(self, Defines.CHAN_WEAPON, sound_cock, 1, Defines.ATTN_NORM, 0);
                return true;
            }
        }

        static mframe_t[] soldier_frames_stand1 = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, soldier_idle), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t soldier_move_stand1 = new mmove_t(FRAME_stand101, FRAME_stand130, soldier_frames_stand1, soldier_stand);
        static mframe_t[] soldier_frames_stand3 = new mframe_t[]{new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, soldier_cock), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null), new mframe_t(GameAI.ai_stand, 0, null)};
        static mmove_t soldier_move_stand3 = new mmove_t(FRAME_stand301, FRAME_stand339, soldier_frames_stand3, soldier_stand);
        static mframe_t[] soldier_frames_walk1 = new mframe_t[]{new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 2, null), new mframe_t(GameAI.ai_walk, 1, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, -1, soldier_walk1_random), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null), new mframe_t(GameAI.ai_walk, 0, null)};
        static mmove_t soldier_move_walk1 = new mmove_t(FRAME_walk101, FRAME_walk133, soldier_frames_walk1, null);
        static mframe_t[] soldier_frames_walk2 = new mframe_t[]{new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 4, null), new mframe_t(GameAI.ai_walk, 9, null), new mframe_t(GameAI.ai_walk, 8, null), new mframe_t(GameAI.ai_walk, 5, null), new mframe_t(GameAI.ai_walk, 1, null), new mframe_t(GameAI.ai_walk, 3, null), new mframe_t(GameAI.ai_walk, 7, null), new mframe_t(GameAI.ai_walk, 6, null), new mframe_t(GameAI.ai_walk, 7, null)};
        static mmove_t soldier_move_walk2 = new mmove_t(FRAME_walk209, FRAME_walk218, soldier_frames_walk2, null);
        static mframe_t[] soldier_frames_start_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 7, null), new mframe_t(GameAI.ai_run, 5, null)};
        static mmove_t soldier_move_start_run = new mmove_t(FRAME_run01, FRAME_run02, soldier_frames_start_run, soldier_run);
        static mframe_t[] soldier_frames_run = new mframe_t[]{new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 11, null), new mframe_t(GameAI.ai_run, 11, null), new mframe_t(GameAI.ai_run, 16, null), new mframe_t(GameAI.ai_run, 10, null), new mframe_t(GameAI.ai_run, 15, null)};
        static mmove_t soldier_move_run = new mmove_t(FRAME_run03, FRAME_run08, soldier_frames_run, null);
        static EntThinkAdapter soldier_duck_hold = new AnonymousEntThinkAdapter26();
        private sealed class AnonymousEntThinkAdapter26 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_duck_hold";
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

        static mframe_t[] soldier_frames_pain1 = new mframe_t[]{new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t soldier_move_pain1 = new mmove_t(FRAME_pain101, FRAME_pain105, soldier_frames_pain1, soldier_run);
        static mframe_t[] soldier_frames_pain2 = new mframe_t[]{new mframe_t(GameAI.ai_move, -13, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 2, null)};
        static mmove_t soldier_move_pain2 = new mmove_t(FRAME_pain201, FRAME_pain207, soldier_frames_pain2, soldier_run);
        static mframe_t[] soldier_frames_pain3 = new mframe_t[]{new mframe_t(GameAI.ai_move, -8, null), new mframe_t(GameAI.ai_move, 10, null), new mframe_t(GameAI.ai_move, -4, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -3, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 2, null)};
        static mmove_t soldier_move_pain3 = new mmove_t(FRAME_pain301, FRAME_pain318, soldier_frames_pain3, soldier_run);
        static mframe_t[] soldier_frames_pain4 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -10, null), new mframe_t(GameAI.ai_move, -6, null), new mframe_t(GameAI.ai_move, 8, null), new mframe_t(GameAI.ai_move, 4, null), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 5, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, -1, null), new mframe_t(GameAI.ai_move, 3, null), new mframe_t(GameAI.ai_move, 2, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t soldier_move_pain4 = new mmove_t(FRAME_pain401, FRAME_pain417, soldier_frames_pain4, soldier_run);
        static int[] blaster_flash = new[]{Defines.MZ2_SOLDIER_BLASTER_1, Defines.MZ2_SOLDIER_BLASTER_2, Defines.MZ2_SOLDIER_BLASTER_3, Defines.MZ2_SOLDIER_BLASTER_4, Defines.MZ2_SOLDIER_BLASTER_5, Defines.MZ2_SOLDIER_BLASTER_6, Defines.MZ2_SOLDIER_BLASTER_7, Defines.MZ2_SOLDIER_BLASTER_8};
        static int[] shotgun_flash = new[]{Defines.MZ2_SOLDIER_SHOTGUN_1, Defines.MZ2_SOLDIER_SHOTGUN_2, Defines.MZ2_SOLDIER_SHOTGUN_3, Defines.MZ2_SOLDIER_SHOTGUN_4, Defines.MZ2_SOLDIER_SHOTGUN_5, Defines.MZ2_SOLDIER_SHOTGUN_6, Defines.MZ2_SOLDIER_SHOTGUN_7, Defines.MZ2_SOLDIER_SHOTGUN_8};
        static int[] machinegun_flash = new[]{Defines.MZ2_SOLDIER_MACHINEGUN_1, Defines.MZ2_SOLDIER_MACHINEGUN_2, Defines.MZ2_SOLDIER_MACHINEGUN_3, Defines.MZ2_SOLDIER_MACHINEGUN_4, Defines.MZ2_SOLDIER_MACHINEGUN_5, Defines.MZ2_SOLDIER_MACHINEGUN_6, Defines.MZ2_SOLDIER_MACHINEGUN_7, Defines.MZ2_SOLDIER_MACHINEGUN_8};
        static mframe_t[] soldier_frames_attack1 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_fire1), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_attack1_refire1), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_cock), new mframe_t(GameAI.ai_charge, 0, soldier_attack1_refire2), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t soldier_move_attack1 = new mmove_t(FRAME_attak101, FRAME_attak112, soldier_frames_attack1, soldier_run);
        static mframe_t[] soldier_frames_attack2 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_fire2), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_attack2_refire1), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_cock), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_attack2_refire2), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t soldier_move_attack2 = new mmove_t(FRAME_attak201, FRAME_attak218, soldier_frames_attack2, soldier_run);
        static mframe_t[] soldier_frames_attack3 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_fire3), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_attack3_refire), new mframe_t(GameAI.ai_charge, 0, soldier_duck_up), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t soldier_move_attack3 = new mmove_t(FRAME_attak301, FRAME_attak309, soldier_frames_attack3, soldier_run);
        static mframe_t[] soldier_frames_attack4 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, soldier_fire4), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null), new mframe_t(GameAI.ai_charge, 0, null)};
        static mmove_t soldier_move_attack4 = new mmove_t(FRAME_attak401, FRAME_attak406, soldier_frames_attack4, soldier_run);
        static mframe_t[] soldier_frames_attack6 = new mframe_t[]{new mframe_t(GameAI.ai_charge, 10, null), new mframe_t(GameAI.ai_charge, 4, null), new mframe_t(GameAI.ai_charge, 12, null), new mframe_t(GameAI.ai_charge, 11, soldier_fire8), new mframe_t(GameAI.ai_charge, 13, null), new mframe_t(GameAI.ai_charge, 18, null), new mframe_t(GameAI.ai_charge, 15, null), new mframe_t(GameAI.ai_charge, 14, null), new mframe_t(GameAI.ai_charge, 11, null), new mframe_t(GameAI.ai_charge, 8, null), new mframe_t(GameAI.ai_charge, 11, null), new mframe_t(GameAI.ai_charge, 12, null), new mframe_t(GameAI.ai_charge, 12, null), new mframe_t(GameAI.ai_charge, 17, soldier_attack6_refire)};
        static mmove_t soldier_move_attack6 = new mmove_t(FRAME_runs01, FRAME_runs14, soldier_frames_attack6, soldier_run);
        static mframe_t[] soldier_frames_duck = new mframe_t[]{new mframe_t(GameAI.ai_move, 5, soldier_duck_down), new mframe_t(GameAI.ai_move, -1, soldier_duck_hold), new mframe_t(GameAI.ai_move, 1, null), new mframe_t(GameAI.ai_move, 0, soldier_duck_up), new mframe_t(GameAI.ai_move, 5, null)};
        static mmove_t soldier_move_duck = new mmove_t(FRAME_duck01, FRAME_duck05, soldier_frames_duck, soldier_run);
        static mframe_t[] soldier_frames_death1 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, -10, null), new mframe_t(GameAI.ai_move, -10, null), new mframe_t(GameAI.ai_move, -10, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, soldier_fire6), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, soldier_fire7), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t soldier_move_death1 = new mmove_t(FRAME_death101, FRAME_death136, soldier_frames_death1, soldier_dead);
        static mframe_t[] soldier_frames_death2 = new mframe_t[]{new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t soldier_move_death2 = new mmove_t(FRAME_death201, FRAME_death235, soldier_frames_death2, soldier_dead);
        static mframe_t[] soldier_frames_death3 = new mframe_t[]{new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t soldier_move_death3 = new mmove_t(FRAME_death301, FRAME_death345, soldier_frames_death3, soldier_dead);
        static mframe_t[] soldier_frames_death4 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t soldier_move_death4 = new mmove_t(FRAME_death401, FRAME_death453, soldier_frames_death4, soldier_dead);
        static mframe_t[] soldier_frames_death5 = new mframe_t[]{new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, -5, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t soldier_move_death5 = new mmove_t(FRAME_death501, FRAME_death524, soldier_frames_death5, soldier_dead);
        static mframe_t[] soldier_frames_death6 = new mframe_t[]{new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null), new mframe_t(GameAI.ai_move, 0, null)};
        static mmove_t soldier_move_death6 = new mmove_t(FRAME_death601, FRAME_death610, soldier_frames_death6, soldier_dead);
        static EntThinkAdapter soldier_attack = new AnonymousEntThinkAdapter27();
        private sealed class AnonymousEntThinkAdapter27 : EntThinkAdapter
		{
			
            public override string GetID()
            {
                return "soldier_attack";
            }

            public override bool Think(edict_t self)
            {
                if (self.s.skinnum < 4)
                {
                    if (Lib.Random() < 0.5)
                        self.monsterinfo.currentmove = soldier_move_attack1;
                    else
                        self.monsterinfo.currentmove = soldier_move_attack2;
                }
                else
                {
                    self.monsterinfo.currentmove = soldier_move_attack4;
                }

                return true;
            }
        }

        static EntDodgeAdapter soldier_dodge = new AnonymousEntDodgeAdapter();
        private sealed class AnonymousEntDodgeAdapter : EntDodgeAdapter
		{
			
            public override string GetID()
            {
                return "soldier_dodge";
            }

            public override void Dodge(edict_t self, edict_t attacker, float eta)
            {
                float r;
                r = Lib.Random();
                if (r > 0.25)
                    return;
                if (self.enemy == null)
                    self.enemy = attacker;
                if (GameBase.skill.value == 0)
                {
                    self.monsterinfo.currentmove = soldier_move_duck;
                    return;
                }

                self.monsterinfo.pausetime = GameBase.level.time + eta + 0.3F;
                r = Lib.Random();
                if (GameBase.skill.value == 1)
                {
                    if (r > 0.33)
                        self.monsterinfo.currentmove = soldier_move_duck;
                    else
                        self.monsterinfo.currentmove = soldier_move_attack3;
                    return;
                }

                if (GameBase.skill.value >= 2)
                {
                    if (r > 0.66)
                        self.monsterinfo.currentmove = soldier_move_duck;
                    else
                        self.monsterinfo.currentmove = soldier_move_attack3;
                    return;
                }

                self.monsterinfo.currentmove = soldier_move_attack3;
            }
        }
    }
}
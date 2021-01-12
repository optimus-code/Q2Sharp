using Q2Sharp.Game.Monsters;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class PlayerView
    {
        public static edict_t current_player;
        public static gclient_t current_client;
        public static float[] forward = new float[]{0, 0, 0};
        public static float[] right = new float[]{0, 0, 0};
        public static float[] up = new float[]{0, 0, 0};
        public static float SV_CalcRoll(float[] angles, float[] velocity)
        {
            float sign;
            float side;
            float value;
            side = Math3D.DotProduct(velocity, right);
            sign = side < 0 ? -1 : 1;
            side = Math.Abs(side);
            value = GameBase.sv_rollangle.value;
            if (side < GameBase.sv_rollspeed.value)
                side = side * value / GameBase.sv_rollspeed.value;
            else
                side = value;
            return side * sign;
        }

        public static void P_DamageFeedback(edict_t player)
        {
            gclient_t client;
            float side;
            float realcount, count, kick;
            float[] v = new float[]{0, 0, 0};
            int r, l;
            float[] power_color = new[]{0F, 1F, 0F};
            float[] acolor = new[]{1F, 1F, 1F};
            float[] bcolor = new[]{1F, 0F, 0F};
            client = player.client;
            client.ps.stats[Defines.STAT_FLASHES] = 0;
            if (client.damage_blood != 0)
                client.ps.stats[Defines.STAT_FLASHES] |= 1;
            if (client.damage_armor != 0 && 0 == (player.flags & Defines.FL_GODMODE) && (client.invincible_framenum <= GameBase.level.framenum))
                client.ps.stats[Defines.STAT_FLASHES] |= 2;
            count = (client.damage_blood + client.damage_armor + client.damage_parmor);
            if (count == 0)
                return;
            if ((client.anim_priority < Defines.ANIM_PAIN) & (player.s.modelindex == 255))
            {
                client.anim_priority = Defines.ANIM_PAIN;
                if ((client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED) != 0)
                {
                    player.s.frame = M_Player.FRAME_crpain1 - 1;
                    client.anim_end = M_Player.FRAME_crpain4;
                }
                else
                {
                    xxxi = (xxxi + 1) % 3;
                    switch ( xxxi )
                    {
                        case 0:
                            player.s.frame = M_Player.FRAME_pain101 - 1;
                            client.anim_end = M_Player.FRAME_pain104;
                            break;
                        case 1:
                            player.s.frame = M_Player.FRAME_pain201 - 1;
                            client.anim_end = M_Player.FRAME_pain204;
                            break;
                        case 2:
                            player.s.frame = M_Player.FRAME_pain301 - 1;
                            client.anim_end = M_Player.FRAME_pain304;
                            break;
                    }
                }
            }

            realcount = count;
            if (count < 10)
                count = 10;
            if ((GameBase.level.time > player.pain_debounce_time) && 0 == (player.flags & Defines.FL_GODMODE) && (client.invincible_framenum <= GameBase.level.framenum))
            {
                r = 1 + (Lib.Rand() & 1);
                player.pain_debounce_time = GameBase.level.time + 0.7F;
                if (player.health < 25)
                    l = 25;
                else if (player.health < 50)
                    l = 50;
                else if (player.health < 75)
                    l = 75;
                else
                    l = 100;
                GameBase.gi.Sound(player, Defines.CHAN_VOICE, GameBase.gi.Soundindex("*pain" + l + "_" + r + ".wav"), 1, Defines.ATTN_NORM, 0);
            }

            if (client.damage_alpha < 0)
                client.damage_alpha = 0;
            client.damage_alpha += count * 0.01F;
            if (client.damage_alpha < 0.2F)
                client.damage_alpha = 0.2F;
            if (client.damage_alpha > 0.6F)
                client.damage_alpha = 0.6F;
            Math3D.VectorClear(v);
            if (client.damage_parmor != 0)
                Math3D.VectorMA(v, (float)client.damage_parmor / realcount, power_color, v);
            if (client.damage_armor != 0)
                Math3D.VectorMA(v, (float)client.damage_armor / realcount, acolor, v);
            if (client.damage_blood != 0)
                Math3D.VectorMA(v, (float)client.damage_blood / realcount, bcolor, v);
            Math3D.VectorCopy(v, client.damage_blend);
            kick = Math.Abs(client.damage_knockback);
            if (kick != 0 && player.health > 0)
            {
                kick = kick * 100 / player.health;
                if (kick < count * 0.5)
                    kick = count * 0.5F;
                if (kick > 50)
                    kick = 50;
                Math3D.VectorSubtract(client.damage_from, player.s.origin, v);
                Math3D.VectorNormalize(v);
                side = Math3D.DotProduct(v, right);
                client.v_dmg_roll = kick * side * 0.3F;
                side = -Math3D.DotProduct(v, forward);
                client.v_dmg_pitch = kick * side * 0.3F;
                client.v_dmg_time = GameBase.level.time + Defines.DAMAGE_TIME;
            }

            client.damage_blood = 0;
            client.damage_armor = 0;
            client.damage_parmor = 0;
            client.damage_knockback = 0;
        }

        public static void SV_CalcViewOffset(edict_t ent)
        {
            float[] angles = new float[]{0, 0, 0};
            float bob;
            float ratio;
            float delta;
            float[] v = new float[]{0, 0, 0};
            angles = ent.client.ps.kick_angles;
            if (ent.deadflag != 0)
            {
                Math3D.VectorClear(angles);
                ent.client.ps.viewangles[Defines.ROLL] = 40;
                ent.client.ps.viewangles[Defines.PITCH] = -15;
                ent.client.ps.viewangles[Defines.YAW] = ent.client.killer_yaw;
            }
            else
            {
                Math3D.VectorCopy(ent.client.kick_angles, angles);
                ratio = (ent.client.v_dmg_time - GameBase.level.time) / Defines.DAMAGE_TIME;
                if (ratio < 0)
                {
                    ratio = 0;
                    ent.client.v_dmg_pitch = 0;
                    ent.client.v_dmg_roll = 0;
                }

                angles[Defines.PITCH] += ratio * ent.client.v_dmg_pitch;
                angles[Defines.ROLL] += ratio * ent.client.v_dmg_roll;
                ratio = (ent.client.fall_time - GameBase.level.time) / Defines.FALL_TIME;
                if (ratio < 0)
                    ratio = 0;
                angles[Defines.PITCH] += ratio * ent.client.fall_value;
                delta = Math3D.DotProduct(ent.velocity, forward);
                angles[Defines.PITCH] += delta * GameBase.run_pitch.value;
                delta = Math3D.DotProduct(ent.velocity, right);
                angles[Defines.ROLL] += delta * GameBase.run_roll.value;
                delta = bobfracsin * GameBase.bob_pitch.value * xyspeed;
                if ((ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED) != 0)
                    delta *= 6;
                angles[Defines.PITCH] += delta;
                delta = bobfracsin * GameBase.bob_roll.value * xyspeed;
                if ((ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED) != 0)
                    delta *= 6;
                if ((bobcycle & 1) != 0)
                    delta = -delta;
                angles[Defines.ROLL] += delta;
            }

            Math3D.VectorClear(v);
            v[2] += ent.viewheight;
            ratio = (ent.client.fall_time - GameBase.level.time) / Defines.FALL_TIME;
            if (ratio < 0)
                ratio = 0;
            v[2] -= ratio * ent.client.fall_value * 0.4f;
            bob = bobfracsin * xyspeed * GameBase.bob_up.value;
            if (bob > 6)
                bob = 6;
            v[2] += bob;
            Math3D.VectorAdd(v, ent.client.kick_origin, v);
            if (v[0] < -14)
                v[0] = -14;
            else if (v[0] > 14)
                v[0] = 14;
            if (v[1] < -14)
                v[1] = -14;
            else if (v[1] > 14)
                v[1] = 14;
            if (v[2] < -22)
                v[2] = -22;
            else if (v[2] > 30)
                v[2] = 30;
            Math3D.VectorCopy(v, ent.client.ps.viewoffset);
        }

        public static void SV_CalcGunOffset(edict_t ent)
        {
            int i;
            float delta;
            ent.client.ps.gunangles[Defines.ROLL] = xyspeed * bobfracsin * 0.005F;
            ent.client.ps.gunangles[Defines.YAW] = xyspeed * bobfracsin * 0.01F;
            if ((bobcycle & 1) != 0)
            {
                ent.client.ps.gunangles[Defines.ROLL] = -ent.client.ps.gunangles[Defines.ROLL];
                ent.client.ps.gunangles[Defines.YAW] = -ent.client.ps.gunangles[Defines.YAW];
            }

            ent.client.ps.gunangles[Defines.PITCH] = xyspeed * bobfracsin * 0.005F;
            for (i = 0; i < 3; i++)
            {
                delta = ent.client.oldviewangles[i] - ent.client.ps.viewangles[i];
                if (delta > 180)
                    delta -= 360;
                if (delta < -180)
                    delta += 360;
                if (delta > 45)
                    delta = 45;
                if (delta < -45)
                    delta = -45;
                if (i == Defines.YAW)
                    ent.client.ps.gunangles[Defines.ROLL] += 0.1f * delta;
                ent.client.ps.gunangles[i] += 0.2f * delta;
            }

            Math3D.VectorClear(ent.client.ps.gunoffset);
            for (i = 0; i < 3; i++)
            {
                ent.client.ps.gunoffset[i] += forward[i] * (GameBase.gun_y.value);
                ent.client.ps.gunoffset[i] += right[i] * GameBase.gun_x.value;
                ent.client.ps.gunoffset[i] += up[i] * (-GameBase.gun_z.value);
            }
        }

        public static void SV_AddBlend(float r, float g, float b, float a, float[] v_blend)
        {
            float a2, a3;
            if (a <= 0)
                return;
            a2 = v_blend[3] + (1 - v_blend[3]) * a;
            a3 = v_blend[3] / a2;
            v_blend[0] = v_blend[0] * a3 + r * (1 - a3);
            v_blend[1] = v_blend[1] * a3 + g * (1 - a3);
            v_blend[2] = v_blend[2] * a3 + b * (1 - a3);
            v_blend[3] = a2;
        }

        public static void SV_CalcBlend(edict_t ent)
        {
            int contents;
            float[] vieworg = new float[]{0, 0, 0};
            int remaining;
            ent.client.ps.blend[0] = ent.client.ps.blend[1] = ent.client.ps.blend[2] = ent.client.ps.blend[3] = 0;
            Math3D.VectorAdd(ent.s.origin, ent.client.ps.viewoffset, vieworg);
            contents = GameBase.gi.pointcontents.Pointcontents(vieworg);
            if ((contents & (Defines.CONTENTS_LAVA | Defines.CONTENTS_SLIME | Defines.CONTENTS_WATER)) != 0)
                ent.client.ps.rdflags |= Defines.RDF_UNDERWATER;
            else
                ent.client.ps.rdflags &= ~Defines.RDF_UNDERWATER;
            if ((contents & (Defines.CONTENTS_SOLID | Defines.CONTENTS_LAVA)) != 0)
                SV_AddBlend(1F, 0.3F, 0F, 0.6F, ent.client.ps.blend);
            else if ((contents & Defines.CONTENTS_SLIME) != 0)
                SV_AddBlend(0F, 0.1F, 0.05F, 0.6F, ent.client.ps.blend);
            else if ((contents & Defines.CONTENTS_WATER) != 0)
                SV_AddBlend(0.5F, 0.3F, 0.2F, 0.4F, ent.client.ps.blend);
            if (ent.client.quad_framenum > GameBase.level.framenum)
            {
                remaining = (int)(ent.client.quad_framenum - GameBase.level.framenum);
                if (remaining == 30)
                    GameBase.gi.Sound(ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex("items/damage2.wav"), 1, Defines.ATTN_NORM, 0);
                if (remaining > 30 || (remaining & 4) != 0)
                    SV_AddBlend(0, 0, 1, 0.08F, ent.client.ps.blend);
            }
            else if (ent.client.invincible_framenum > GameBase.level.framenum)
            {
                remaining = (int)ent.client.invincible_framenum - GameBase.level.framenum;
                if (remaining == 30)
                    GameBase.gi.Sound(ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex("items/protect2.wav"), 1, Defines.ATTN_NORM, 0);
                if (remaining > 30 || (remaining & 4) != 0)
                    SV_AddBlend(1, 1, 0, 0.08F, ent.client.ps.blend);
            }
            else if (ent.client.enviro_framenum > GameBase.level.framenum)
            {
                remaining = (int)ent.client.enviro_framenum - GameBase.level.framenum;
                if (remaining == 30)
                    GameBase.gi.Sound(ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex("items/airout.wav"), 1, Defines.ATTN_NORM, 0);
                if (remaining > 30 || (remaining & 4) != 0)
                    SV_AddBlend(0, 1, 0, 0.08F, ent.client.ps.blend);
            }
            else if (ent.client.breather_framenum > GameBase.level.framenum)
            {
                remaining = (int)ent.client.breather_framenum - GameBase.level.framenum;
                if (remaining == 30)
                    GameBase.gi.Sound(ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex("items/airout.wav"), 1, Defines.ATTN_NORM, 0);
                if (remaining > 30 || (remaining & 4) != 0)
                    SV_AddBlend(0.4F, 1, 0.4F, 0.04F, ent.client.ps.blend);
            }

            if (ent.client.damage_alpha > 0)
                SV_AddBlend(ent.client.damage_blend[0], ent.client.damage_blend[1], ent.client.damage_blend[2], ent.client.damage_alpha, ent.client.ps.blend);
            if (ent.client.bonus_alpha > 0)
                SV_AddBlend(0.85F, 0.7F, 0.3F, ent.client.bonus_alpha, ent.client.ps.blend);
            ent.client.damage_alpha -= 0.06f;
            if (ent.client.damage_alpha < 0)
                ent.client.damage_alpha = 0;
            ent.client.bonus_alpha -= 0.1f;
            if (ent.client.bonus_alpha < 0)
                ent.client.bonus_alpha = 0;
        }

        public static void P_FallingDamage(edict_t ent)
        {
            float delta;
            int damage;
            float[] dir = new float[]{0, 0, 0};
            if (ent.s.modelindex != 255)
                return;
            if (ent.movetype == Defines.MOVETYPE_NOCLIP)
                return;
            if ((ent.client.oldvelocity[2] < 0) && (ent.velocity[2] > ent.client.oldvelocity[2]) && (null == ent.groundentity))
            {
                delta = ent.client.oldvelocity[2];
            }
            else
            {
                if (ent.groundentity == null)
                    return;
                delta = ent.velocity[2] - ent.client.oldvelocity[2];
            }

            delta = delta * delta * 0.0001F;
            if (ent.waterlevel == 3)
                return;
            if (ent.waterlevel == 2)
                delta *= 0.25f;
            if (ent.waterlevel == 1)
                delta *= 0.5f;
            if (delta < 1)
                return;
            if (delta < 15)
            {
                ent.s.event_renamed = Defines.EV_FOOTSTEP;
                return;
            }

            ent.client.fall_value = delta * 0.5F;
            if (ent.client.fall_value > 40)
                ent.client.fall_value = 40;
            ent.client.fall_time = GameBase.level.time + Defines.FALL_TIME;
            if (delta > 30)
            {
                if (ent.health > 0)
                {
                    if (delta >= 55)
                        ent.s.event_renamed = Defines.EV_FALLFAR;
                    else
                        ent.s.event_renamed = Defines.EV_FALL;
                }

                ent.pain_debounce_time = GameBase.level.time;
                damage = (int)((delta - 30) / 2);
                if (damage < 1)
                    damage = 1;
                Math3D.VectorSet(dir, 0, 0, 1);
                if (GameBase.deathmatch.value == 0 || 0 == ((int)GameBase.dmflags.value & Defines.DF_NO_FALLING))
                    GameCombat.T_Damage(ent, GameBase.g_edicts[0], GameBase.g_edicts[0], dir, ent.s.origin, Globals.vec3_origin, damage, 0, 0, Defines.MOD_FALLING);
            }
            else
            {
                ent.s.event_renamed = Defines.EV_FALLSHORT;
                return;
            }
        }

        public static void P_WorldEffects()
        {
            bool breather;
            bool envirosuit;
            int waterlevel, old_waterlevel;
            if (current_player.movetype == Defines.MOVETYPE_NOCLIP)
            {
                current_player.air_finished = GameBase.level.time + 12;
                return;
            }

            waterlevel = current_player.waterlevel;
            old_waterlevel = current_client.old_waterlevel;
            current_client.old_waterlevel = waterlevel;
            breather = current_client.breather_framenum > GameBase.level.framenum;
            envirosuit = current_client.enviro_framenum > GameBase.level.framenum;
            if (old_waterlevel == 0 && waterlevel != 0)
            {
                PlayerWeapon.PlayerNoise(current_player, current_player.s.origin, Defines.PNOISE_SELF);
                if ((current_player.watertype & Defines.CONTENTS_LAVA) != 0)
                    GameBase.gi.Sound(current_player, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/lava_in.wav"), 1, Defines.ATTN_NORM, 0);
                else if ((current_player.watertype & Defines.CONTENTS_SLIME) != 0)
                    GameBase.gi.Sound(current_player, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/watr_in.wav"), 1, Defines.ATTN_NORM, 0);
                else if ((current_player.watertype & Defines.CONTENTS_WATER) != 0)
                    GameBase.gi.Sound(current_player, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/watr_in.wav"), 1, Defines.ATTN_NORM, 0);
                current_player.flags |= Defines.FL_INWATER;
                current_player.damage_debounce_time = GameBase.level.time - 1;
            }

            if (old_waterlevel != 0 && waterlevel == 0)
            {
                PlayerWeapon.PlayerNoise(current_player, current_player.s.origin, Defines.PNOISE_SELF);
                GameBase.gi.Sound(current_player, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/watr_out.wav"), 1, Defines.ATTN_NORM, 0);
                current_player.flags &= ~Defines.FL_INWATER;
            }

            if (old_waterlevel != 3 && waterlevel == 3)
            {
                GameBase.gi.Sound(current_player, Defines.CHAN_BODY, GameBase.gi.Soundindex("player/watr_un.wav"), 1, Defines.ATTN_NORM, 0);
            }

            if (old_waterlevel == 3 && waterlevel != 3)
            {
                if (current_player.air_finished < GameBase.level.time)
                {
                    GameBase.gi.Sound(current_player, Defines.CHAN_VOICE, GameBase.gi.Soundindex("player/gasp1.wav"), 1, Defines.ATTN_NORM, 0);
                    PlayerWeapon.PlayerNoise(current_player, current_player.s.origin, Defines.PNOISE_SELF);
                }
                else if (current_player.air_finished < GameBase.level.time + 11)
                {
                    GameBase.gi.Sound(current_player, Defines.CHAN_VOICE, GameBase.gi.Soundindex("player/gasp2.wav"), 1, Defines.ATTN_NORM, 0);
                }
            }

            if (waterlevel == 3)
            {
                if (breather || envirosuit)
                {
                    current_player.air_finished = GameBase.level.time + 10;
                    if (((int)(current_client.breather_framenum - GameBase.level.framenum) % 25) == 0)
                    {
                        if (current_client.breather_sound == 0)
                            GameBase.gi.Sound(current_player, Defines.CHAN_AUTO, GameBase.gi.Soundindex("player/u_breath1.wav"), 1, Defines.ATTN_NORM, 0);
                        else
                            GameBase.gi.Sound(current_player, Defines.CHAN_AUTO, GameBase.gi.Soundindex("player/u_breath2.wav"), 1, Defines.ATTN_NORM, 0);
                        current_client.breather_sound ^= 1;
                        PlayerWeapon.PlayerNoise(current_player, current_player.s.origin, Defines.PNOISE_SELF);
                    }
                }

                if (current_player.air_finished < GameBase.level.time)
                {
                    if (current_player.client.next_drown_time < GameBase.level.time && current_player.health > 0)
                    {
                        current_player.client.next_drown_time = GameBase.level.time + 1;
                        current_player.dmg += 2;
                        if (current_player.dmg > 15)
                            current_player.dmg = 15;
                        if (current_player.health <= current_player.dmg)
                            GameBase.gi.Sound(current_player, Defines.CHAN_VOICE, GameBase.gi.Soundindex("player/drown1.wav"), 1, Defines.ATTN_NORM, 0);
                        else if ((Lib.Rand() & 1) != 0)
                            GameBase.gi.Sound(current_player, Defines.CHAN_VOICE, GameBase.gi.Soundindex("*gurp1.wav"), 1, Defines.ATTN_NORM, 0);
                        else
                            GameBase.gi.Sound(current_player, Defines.CHAN_VOICE, GameBase.gi.Soundindex("*gurp2.wav"), 1, Defines.ATTN_NORM, 0);
                        current_player.pain_debounce_time = GameBase.level.time;
                        GameCombat.T_Damage(current_player, GameBase.g_edicts[0], GameBase.g_edicts[0], Globals.vec3_origin, current_player.s.origin, Globals.vec3_origin, current_player.dmg, 0, Defines.DAMAGE_NO_ARMOR, Defines.MOD_WATER);
                    }
                }
            }
            else
            {
                current_player.air_finished = GameBase.level.time + 12;
                current_player.dmg = 2;
            }

            if (waterlevel != 0 && 0 != (current_player.watertype & (Defines.CONTENTS_LAVA | Defines.CONTENTS_SLIME)))
            {
                if ((current_player.watertype & Defines.CONTENTS_LAVA) != 0)
                {
                    if (current_player.health > 0 && current_player.pain_debounce_time <= GameBase.level.time && current_client.invincible_framenum < GameBase.level.framenum)
                    {
                        if ((Lib.Rand() & 1) != 0)
                            GameBase.gi.Sound(current_player, Defines.CHAN_VOICE, GameBase.gi.Soundindex("player/burn1.wav"), 1, Defines.ATTN_NORM, 0);
                        else
                            GameBase.gi.Sound(current_player, Defines.CHAN_VOICE, GameBase.gi.Soundindex("player/burn2.wav"), 1, Defines.ATTN_NORM, 0);
                        current_player.pain_debounce_time = GameBase.level.time + 1;
                    }

                    if (envirosuit)
                        GameCombat.T_Damage(current_player, GameBase.g_edicts[0], GameBase.g_edicts[0], Globals.vec3_origin, current_player.s.origin, Globals.vec3_origin, 1 * waterlevel, 0, 0, Defines.MOD_LAVA);
                    else
                        GameCombat.T_Damage(current_player, GameBase.g_edicts[0], GameBase.g_edicts[0], Globals.vec3_origin, current_player.s.origin, Globals.vec3_origin, 3 * waterlevel, 0, 0, Defines.MOD_LAVA);
                }

                if ((current_player.watertype & Defines.CONTENTS_SLIME) != 0)
                {
                    if (!envirosuit)
                    {
                        GameCombat.T_Damage(current_player, GameBase.g_edicts[0], GameBase.g_edicts[0], Globals.vec3_origin, current_player.s.origin, Globals.vec3_origin, 1 * waterlevel, 0, 0, Defines.MOD_SLIME);
                    }
                }
            }
        }

        public static void G_SetClientEffects(edict_t ent)
        {
            int pa_type;
            int remaining;
            ent.s.effects = 0;
            ent.s.renderfx = 0;
            if (ent.health <= 0 || GameBase.level.intermissiontime != 0)
                return;
            if (ent.powerarmor_time > GameBase.level.time)
            {
                pa_type = GameItems.PowerArmorType(ent);
                if (pa_type == Defines.POWER_ARMOR_SCREEN)
                {
                    ent.s.effects |= Defines.EF_POWERSCREEN;
                }
                else if (pa_type == Defines.POWER_ARMOR_SHIELD)
                {
                    ent.s.effects |= Defines.EF_COLOR_SHELL;
                    ent.s.renderfx |= Defines.RF_SHELL_GREEN;
                }
            }

            if (ent.client.quad_framenum > GameBase.level.framenum)
            {
                remaining = (int)ent.client.quad_framenum - GameBase.level.framenum;
                if (remaining > 30 || 0 != (remaining & 4))
                    ent.s.effects |= Defines.EF_QUAD;
            }

            if (ent.client.invincible_framenum > GameBase.level.framenum)
            {
                remaining = (int)ent.client.invincible_framenum - GameBase.level.framenum;
                if (remaining > 30 || 0 != (remaining & 4))
                    ent.s.effects |= Defines.EF_PENT;
            }

            if ((ent.flags & Defines.FL_GODMODE) != 0)
            {
                ent.s.effects |= Defines.EF_COLOR_SHELL;
                ent.s.renderfx |= (Defines.RF_SHELL_RED | Defines.RF_SHELL_GREEN | Defines.RF_SHELL_BLUE);
            }
        }

        public static void G_SetClientEvent(edict_t ent)
        {
            if (ent.s.event_renamed != 0)
                return;
            if (ent.groundentity != null && xyspeed > 225)
            {
                if ((int)(current_client.bobtime + bobmove) != bobcycle)
                    ent.s.event_renamed = Defines.EV_FOOTSTEP;
            }
        }

        public static void G_SetClientSound(edict_t ent)
        {
            string weap;
            if (ent.client.pers.game_helpchanged != GameBase.game.helpchanged)
            {
                ent.client.pers.game_helpchanged = GameBase.game.helpchanged;
                ent.client.pers.helpchanged = 1;
            }

            if (ent.client.pers.helpchanged != 0 && ent.client.pers.helpchanged <= 3 && 0 == (GameBase.level.framenum & 63))
            {
                ent.client.pers.helpchanged++;
                GameBase.gi.Sound(ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex("misc/pc_up.wav"), 1, Defines.ATTN_STATIC, 0);
            }

            if (ent.client.pers.weapon != null)
                weap = ent.client.pers.weapon.classname;
            else
                weap = "";
            if (ent.waterlevel != 0 && 0 != (ent.watertype & (Defines.CONTENTS_LAVA | Defines.CONTENTS_SLIME)))
                ent.s.sound = GameBase.snd_fry;
            else if (Lib.Strcmp(weap, "weapon_railgun") == 0)
                ent.s.sound = GameBase.gi.Soundindex("weapons/rg_hum.wav");
            else if (Lib.Strcmp(weap, "weapon_bfg") == 0)
                ent.s.sound = GameBase.gi.Soundindex("weapons/bfg_hum.wav");
            else if (ent.client.weapon_sound != 0)
                ent.s.sound = ent.client.weapon_sound;
            else
                ent.s.sound = 0;
        }

        public static void G_SetClientFrame(edict_t ent)
        {
            gclient_t client;
            bool duck, run;
            if (ent.s.modelindex != 255)
                return;
            client = ent.client;
            if ((client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED) != 0)
                duck = true;
            else
                duck = false;
            if (xyspeed != 0)
                run = true;
            else
                run = false;
            bool skip = false;
            if (duck != client.anim_duck && client.anim_priority < Defines.ANIM_DEATH)
                skip = true;
            if (run != client.anim_run && client.anim_priority == Defines.ANIM_BASIC)
                skip = true;
            if (null == ent.groundentity && client.anim_priority <= Defines.ANIM_WAVE)
                skip = true;
            if (!skip)
            {
                if (client.anim_priority == Defines.ANIM_REVERSE)
                {
                    if (ent.s.frame > client.anim_end)
                    {
                        ent.s.frame--;
                        return;
                    }
                }
                else if (ent.s.frame < client.anim_end)
                {
                    ent.s.frame++;
                    return;
                }

                if (client.anim_priority == Defines.ANIM_DEATH)
                    return;
                if (client.anim_priority == Defines.ANIM_JUMP)
                {
                    if (null == ent.groundentity)
                        return;
                    ent.client.anim_priority = Defines.ANIM_WAVE;
                    ent.s.frame = M_Player.FRAME_jump3;
                    ent.client.anim_end = M_Player.FRAME_jump6;
                    return;
                }
            }

            client.anim_priority = Defines.ANIM_BASIC;
            client.anim_duck = duck;
            client.anim_run = run;
            if (null == ent.groundentity)
            {
                client.anim_priority = Defines.ANIM_JUMP;
                if (ent.s.frame != M_Player.FRAME_jump2)
                    ent.s.frame = M_Player.FRAME_jump1;
                client.anim_end = M_Player.FRAME_jump2;
            }
            else if (run)
            {
                if (duck)
                {
                    ent.s.frame = M_Player.FRAME_crwalk1;
                    client.anim_end = M_Player.FRAME_crwalk6;
                }
                else
                {
                    ent.s.frame = M_Player.FRAME_run1;
                    client.anim_end = M_Player.FRAME_run6;
                }
            }
            else
            {
                if (duck)
                {
                    ent.s.frame = M_Player.FRAME_crstnd01;
                    client.anim_end = M_Player.FRAME_crstnd19;
                }
                else
                {
                    ent.s.frame = M_Player.FRAME_stand01;
                    client.anim_end = M_Player.FRAME_stand40;
                }
            }
        }

        public static void ClientEndServerFrame(edict_t ent)
        {
            float bobtime;
            int i;
            current_player = ent;
            current_client = ent.client;
            for (i = 0; i < 3; i++)
            {
                current_client.ps.pmove.origin[i] = (short)(ent.s.origin[i] * 8);
                current_client.ps.pmove.velocity[i] = (short)(ent.velocity[i] * 8);
            }

            if (GameBase.level.intermissiontime != 0)
            {
                current_client.ps.blend[3] = 0;
                current_client.ps.fov = 90;
                PlayerHud.G_SetStats(ent);
                return;
            }

            Math3D.AngleVectors(ent.client.v_angle, forward, right, up);
            P_WorldEffects();
            if (ent.client.v_angle[Defines.PITCH] > 180)
                ent.s.angles[Defines.PITCH] = (-360 + ent.client.v_angle[Defines.PITCH]) / 3;
            else
                ent.s.angles[Defines.PITCH] = ent.client.v_angle[Defines.PITCH] / 3;
            ent.s.angles[Defines.YAW] = ent.client.v_angle[Defines.YAW];
            ent.s.angles[Defines.ROLL] = 0;
            ent.s.angles[Defines.ROLL] = SV_CalcRoll(ent.s.angles, ent.velocity) * 4;
            xyspeed = (float)Math.Sqrt(ent.velocity[0] * ent.velocity[0] + ent.velocity[1] * ent.velocity[1]);
            if (xyspeed < 5)
            {
                bobmove = 0;
                current_client.bobtime = 0;
            }
            else if (ent.groundentity != null)
            {
                if (xyspeed > 210)
                    bobmove = 0.25F;
                else if (xyspeed > 100)
                    bobmove = 0.125F;
                else
                    bobmove = 0.0625F;
            }

            bobtime = (current_client.bobtime += bobmove);
            if ((current_client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED) != 0)
                bobtime *= 4;
            bobcycle = (int)bobtime;
            bobfracsin = (float)Math.Abs(Math.Sin(bobtime * Math.PI));
            P_FallingDamage(ent);
            P_DamageFeedback(ent);
            SV_CalcViewOffset(ent);
            SV_CalcGunOffset(ent);
            SV_CalcBlend(ent);
            if (ent.client.resp.spectator)
                PlayerHud.G_SetSpectatorStats(ent);
            else
                PlayerHud.G_SetStats(ent);
            PlayerHud.G_CheckChaseStats(ent);
            G_SetClientEvent(ent);
            G_SetClientEffects(ent);
            G_SetClientSound(ent);
            G_SetClientFrame(ent);
            Math3D.VectorCopy(ent.velocity, ent.client.oldvelocity);
            Math3D.VectorCopy(ent.client.ps.viewangles, ent.client.oldviewangles);
            Math3D.VectorClear(ent.client.kick_origin);
            Math3D.VectorClear(ent.client.kick_angles);
            if (ent.client.showscores && 0 == (GameBase.level.framenum & 31))
            {
                PlayerHud.DeathmatchScoreboardMessage(ent, ent.enemy);
                GameBase.gi.Unicast(ent, false);
            }
        }

        public static float xyspeed;
        public static float bobmove;
        public static int bobcycle;
        public static float bobfracsin;
        private static int xxxi = 0;
    }
}
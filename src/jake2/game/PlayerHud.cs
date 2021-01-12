using J2N.Text;
using Jake2.Qcommon;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class PlayerHud
    {
        public static void MoveClientToIntermission(edict_t ent)
        {
            if (GameBase.deathmatch.value != 0 || GameBase.coop.value != 0)
                ent.client.showscores = true;
            Math3D.VectorCopy(GameBase.level.intermission_origin, ent.s.origin);
            ent.client.ps.pmove.origin[0] = (short)(GameBase.level.intermission_origin[0] * 8);
            ent.client.ps.pmove.origin[1] = (short)(GameBase.level.intermission_origin[1] * 8);
            ent.client.ps.pmove.origin[2] = (short)(GameBase.level.intermission_origin[2] * 8);
            Math3D.VectorCopy(GameBase.level.intermission_angle, ent.client.ps.viewangles);
            ent.client.ps.pmove.pm_type = Defines.PM_FREEZE;
            ent.client.ps.gunindex = 0;
            ent.client.ps.blend[3] = 0;
            ent.client.ps.rdflags &= ~Defines.RDF_UNDERWATER;
            ent.client.quad_framenum = 0;
            ent.client.invincible_framenum = 0;
            ent.client.breather_framenum = 0;
            ent.client.enviro_framenum = 0;
            ent.client.grenade_blew_up = false;
            ent.client.grenade_time = 0;
            ent.viewheight = 0;
            ent.s.modelindex = 0;
            ent.s.modelindex2 = 0;
            ent.s.modelindex3 = 0;
            ent.s.modelindex = 0;
            ent.s.effects = 0;
            ent.s.sound = 0;
            ent.solid = Defines.SOLID_NOT;
            if (GameBase.deathmatch.value != 0 || GameBase.coop.value != 0)
            {
                DeathmatchScoreboardMessage(ent, null);
                GameBase.gi.Unicast(ent, true);
            }
        }

        public static void BeginIntermission(edict_t targ)
        {
            int i, n;
            edict_t ent, client;
            if (GameBase.level.intermissiontime != 0)
                return;
            GameBase.game.autosaved = false;
            for (i = 0; i < GameBase.maxclients.value; i++)
            {
                client = GameBase.g_edicts[1 + i];
                if (!client.inuse)
                    continue;
                if (client.health <= 0)
                    PlayerClient.Respawn(client);
            }

            GameBase.level.intermissiontime = GameBase.level.time;
            GameBase.level.changemap = targ.map;
            if (GameBase.level.changemap.IndexOf('*') > -1)
            {
                if (GameBase.coop.value != 0)
                {
                    for (i = 0; i < GameBase.maxclients.value; i++)
                    {
                        client = GameBase.g_edicts[1 + i];
                        if (!client.inuse)
                            continue;
                        for (n = 1; n < GameItemList.itemlist.Length; n++)
                        {
                            if (GameItemList.itemlist[n] != null)
                                if ((GameItemList.itemlist[n].flags & Defines.IT_KEY) != 0)
                                    client.client.pers.inventory[n] = 0;
                        }
                    }
                }
            }
            else
            {
                if (0 == GameBase.deathmatch.value)
                {
                    GameBase.level.exitintermission = true;
                    return;
                }
            }

            GameBase.level.exitintermission = false;
            ent = GameBase.G_FindEdict(null, GameBase.findByClass, "info_player_intermission");
            if (ent == null)
            {
                ent = GameBase.G_FindEdict(null, GameBase.findByClass, "info_player_start");
                if (ent == null)
                    ent = GameBase.G_FindEdict(null, GameBase.findByClass, "info_player_deathmatch");
            }
            else
            {
                i = Lib.Rand() & 3;
                EdictIterator es = null;
                while (i-- > 0)
                {
                    es = GameBase.G_Find(es, GameBase.findByClass, "info_player_intermission");
                    if (es == null)
                        continue;
                    ent = es.o;
                }
            }

            Math3D.VectorCopy(ent.s.origin, GameBase.level.intermission_origin);
            Math3D.VectorCopy(ent.s.angles, GameBase.level.intermission_angle);
            for (i = 0; i < GameBase.maxclients.value; i++)
            {
                client = GameBase.g_edicts[1 + i];
                if (!client.inuse)
                    continue;
                MoveClientToIntermission(client);
            }
        }

        public static void DeathmatchScoreboardMessage(edict_t ent, edict_t killer)
        {
            StringBuffer string_renamed = new StringBuffer(1400);
            int stringlength;
            int i, j, k;
            int[] sorted = new int[Defines.MAX_CLIENTS];
            int[] sortedscores = new int[Defines.MAX_CLIENTS];
            int score, total;
            int picnum;
            int x, y;
            gclient_t cl;
            edict_t cl_ent;
            string tag;
            total = 0;
            for (i = 0; i < GameBase.game.maxclients; i++)
            {
                cl_ent = GameBase.g_edicts[1 + i];
                if (!cl_ent.inuse || GameBase.game.clients[i].resp.spectator)
                    continue;
                score = GameBase.game.clients[i].resp.score;
                for (j = 0; j < total; j++)
                {
                    if (score > sortedscores[j])
                        break;
                }

                for (k = total; k > j; k--)
                {
                    sorted[k] = sorted[k - 1];
                    sortedscores[k] = sortedscores[k - 1];
                }

                sorted[j] = i;
                sortedscores[j] = score;
                total++;
            }

            if (total > 12)
                total = 12;
            for (i = 0; i < total; i++)
            {
                cl = GameBase.game.clients[sorted[i]];
                cl_ent = GameBase.g_edicts[1 + sorted[i]];
                picnum = GameBase.gi.Imageindex("i_fixme");
                x = (i >= 6) ? 160 : 0;
                y = 32 + 32 * (i % 6);
                if (cl_ent == ent)
                    tag = "tag1";
                else if (cl_ent == killer)
                    tag = "tag2";
                else
                    tag = null;
                if (tag != null)
                {
                    string_renamed.Append("xv ").Append(x + 32).Append(" yv ").Append(y).Append(" picn ").Append(tag);
                }

                string_renamed.Append(" client ").Append(x).Append(" ").Append(y).Append(" ").Append(sorted[i]).Append(" ").Append(cl.resp.score).Append(" ").Append(cl.ping).Append(" ").Append((GameBase.level.framenum - cl.resp.enterframe) / 600);
            }

            GameBase.gi.WriteByte(Defines.svc_layout);
            GameBase.gi.WriteString(string_renamed.ToString());
        }

        public static void DeathmatchScoreboard(edict_t ent)
        {
            DeathmatchScoreboardMessage(ent, ent.enemy);
            GameBase.gi.Unicast(ent, true);
        }

        public static void Cmd_Score_f(edict_t ent)
        {
            ent.client.showinventory = false;
            ent.client.showhelp = false;
            if (0 == GameBase.deathmatch.value && 0 == GameBase.coop.value)
                return;
            if (ent.client.showscores)
            {
                ent.client.showscores = false;
                return;
            }

            ent.client.showscores = true;
            DeathmatchScoreboard(ent);
        }

        public static void G_SetStats(edict_t ent)
        {
            gitem_t item;
            int index, cells = 0;
            int power_armor_type;
            ent.client.ps.stats[Defines.STAT_HEALTH_ICON] = (short)GameBase.level.pic_health;
            ent.client.ps.stats[Defines.STAT_HEALTH] = (short)ent.health;
            if (0 == ent.client.ammo_index)
            {
                ent.client.ps.stats[Defines.STAT_AMMO_ICON] = 0;
                ent.client.ps.stats[Defines.STAT_AMMO] = 0;
            }
            else
            {
                item = GameItemList.itemlist[ent.client.ammo_index];
                ent.client.ps.stats[Defines.STAT_AMMO_ICON] = (short)GameBase.gi.Imageindex(item.icon);
                ent.client.ps.stats[Defines.STAT_AMMO] = (short)ent.client.pers.inventory[ent.client.ammo_index];
            }

            power_armor_type = GameItems.PowerArmorType(ent);
            if (power_armor_type != 0)
            {
                cells = ent.client.pers.inventory[GameItems.ITEM_INDEX(GameItems.FindItem("cells"))];
                if (cells == 0)
                {
                    ent.flags &= ~Defines.FL_POWER_ARMOR;
                    GameBase.gi.Sound(ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex("misc/power2.wav"), 1, Defines.ATTN_NORM, 0);
                    power_armor_type = 0;
                }
            }

            index = GameItems.ArmorIndex(ent);
            if (power_armor_type != 0 && (0 == index || 0 != (GameBase.level.framenum & 8)))
            {
                ent.client.ps.stats[Defines.STAT_ARMOR_ICON] = (short)GameBase.gi.Imageindex("i_powershield");
                ent.client.ps.stats[Defines.STAT_ARMOR] = (short)cells;
            }
            else if (index != 0)
            {
                item = GameItems.GetItemByIndex(index);
                ent.client.ps.stats[Defines.STAT_ARMOR_ICON] = (short)GameBase.gi.Imageindex(item.icon);
                ent.client.ps.stats[Defines.STAT_ARMOR] = (short)ent.client.pers.inventory[index];
            }
            else
            {
                ent.client.ps.stats[Defines.STAT_ARMOR_ICON] = 0;
                ent.client.ps.stats[Defines.STAT_ARMOR] = 0;
            }

            if (GameBase.level.time > ent.client.pickup_msg_time)
            {
                ent.client.ps.stats[Defines.STAT_PICKUP_ICON] = 0;
                ent.client.ps.stats[Defines.STAT_PICKUP_STRING] = 0;
            }

            if (ent.client.quad_framenum > GameBase.level.framenum)
            {
                ent.client.ps.stats[Defines.STAT_TIMER_ICON] = (short)GameBase.gi.Imageindex("p_quad");
                ent.client.ps.stats[Defines.STAT_TIMER] = (short)((ent.client.quad_framenum - GameBase.level.framenum) / 10);
            }
            else if (ent.client.invincible_framenum > GameBase.level.framenum)
            {
                ent.client.ps.stats[Defines.STAT_TIMER_ICON] = (short)GameBase.gi.Imageindex("p_invulnerability");
                ent.client.ps.stats[Defines.STAT_TIMER] = (short)((ent.client.invincible_framenum - GameBase.level.framenum) / 10);
            }
            else if (ent.client.enviro_framenum > GameBase.level.framenum)
            {
                ent.client.ps.stats[Defines.STAT_TIMER_ICON] = (short)GameBase.gi.Imageindex("p_envirosuit");
                ent.client.ps.stats[Defines.STAT_TIMER] = (short)((ent.client.enviro_framenum - GameBase.level.framenum) / 10);
            }
            else if (ent.client.breather_framenum > GameBase.level.framenum)
            {
                ent.client.ps.stats[Defines.STAT_TIMER_ICON] = (short)GameBase.gi.Imageindex("p_rebreather");
                ent.client.ps.stats[Defines.STAT_TIMER] = (short)((ent.client.breather_framenum - GameBase.level.framenum) / 10);
            }
            else
            {
                ent.client.ps.stats[Defines.STAT_TIMER_ICON] = 0;
                ent.client.ps.stats[Defines.STAT_TIMER] = 0;
            }

            if (ent.client.pers.selected_item <= 0)
                ent.client.ps.stats[Defines.STAT_SELECTED_ICON] = 0;
            else
                ent.client.ps.stats[Defines.STAT_SELECTED_ICON] = (short)GameBase.gi.Imageindex(GameItemList.itemlist[ent.client.pers.selected_item].icon);
            ent.client.ps.stats[Defines.STAT_SELECTED_ITEM] = (short)ent.client.pers.selected_item;
            ent.client.ps.stats[Defines.STAT_LAYOUTS] = 0;
            if (GameBase.deathmatch.value != 0)
            {
                if (ent.client.pers.health <= 0 || GameBase.level.intermissiontime != 0 || ent.client.showscores)
                    ent.client.ps.stats[Defines.STAT_LAYOUTS] |= 1;
                if (ent.client.showinventory && ent.client.pers.health > 0)
                    ent.client.ps.stats[Defines.STAT_LAYOUTS] |= 2;
            }
            else
            {
                if (ent.client.showscores || ent.client.showhelp)
                    ent.client.ps.stats[Defines.STAT_LAYOUTS] |= 1;
                if (ent.client.showinventory && ent.client.pers.health > 0)
                    ent.client.ps.stats[Defines.STAT_LAYOUTS] |= 2;
            }

            ent.client.ps.stats[Defines.STAT_FRAGS] = (short)ent.client.resp.score;
            if (ent.client.pers.helpchanged != 0 && (GameBase.level.framenum & 8) != 0)
                ent.client.ps.stats[Defines.STAT_HELPICON] = (short)GameBase.gi.Imageindex("i_help");
            else if ((ent.client.pers.hand == Defines.CENTER_HANDED || ent.client.ps.fov > 91) && ent.client.pers.weapon != null)
                ent.client.ps.stats[Defines.STAT_HELPICON] = (short)GameBase.gi.Imageindex(ent.client.pers.weapon.icon);
            else
                ent.client.ps.stats[Defines.STAT_HELPICON] = 0;
            ent.client.ps.stats[Defines.STAT_SPECTATOR] = 0;
        }

        public static void G_CheckChaseStats(edict_t ent)
        {
            int i;
            gclient_t cl;
            for (i = 1; i <= GameBase.maxclients.value; i++)
            {
                cl = GameBase.g_edicts[i].client;
                if (!GameBase.g_edicts[i].inuse || cl.chase_target != ent)
                    continue;
                System.Array.Copy(ent.client.ps.stats, 0, cl.ps.stats, 0, Defines.MAX_STATS);
                G_SetSpectatorStats(GameBase.g_edicts[i]);
            }
        }

        public static void G_SetSpectatorStats(edict_t ent)
        {
            gclient_t cl = ent.client;
            if (null == cl.chase_target)
                G_SetStats(ent);
            cl.ps.stats[Defines.STAT_SPECTATOR] = 1;
            cl.ps.stats[Defines.STAT_LAYOUTS] = 0;
            if (cl.pers.health <= 0 || GameBase.level.intermissiontime != 0 || cl.showscores)
                cl.ps.stats[Defines.STAT_LAYOUTS] |= 1;
            if (cl.showinventory && cl.pers.health > 0)
                cl.ps.stats[Defines.STAT_LAYOUTS] |= 2;
            if (cl.chase_target != null && cl.chase_target.inuse)
                cl.ps.stats[Defines.STAT_CHASE] = (short)(Defines.CS_PLAYERSKINS + cl.chase_target.index - 1);
            else
                cl.ps.stats[Defines.STAT_CHASE] = 0;
        }

        public static void HelpComputer(edict_t ent)
        {
            StringBuffer sb = new StringBuffer(256);
            string sk;
            if (GameBase.skill.value == 0)
                sk = "easy";
            else if (GameBase.skill.value == 1)
                sk = "medium";
            else if (GameBase.skill.value == 2)
                sk = "hard";
            else
                sk = "hard+";
            sb.Append("xv 32 yv 8 picn help ");
            sb.Append("xv 202 yv 12 string2 \\\"").Append(sk).Append("\\\" ");
            sb.Append("xv 0 yv 24 cstring2 \\\"").Append(GameBase.level.level_name).Append("\\\" ");
            sb.Append("xv 0 yv 54 cstring2 \\\"").Append(GameBase.game.helpmessage1).Append("\\\" ");
            sb.Append("xv 0 yv 110 cstring2 \\\"").Append(GameBase.game.helpmessage2).Append("\\\" ");
            sb.Append("xv 50 yv 164 string2 \\\" kills     goals    secrets\\\" ");
            sb.Append("xv 50 yv 172 string2 \\\"");
            sb.Append(Com.Sprintf("%3i/%3i     %i/%i       %i/%i\\\" ", GameBase.level.killed_monsters, GameBase.level.total_monsters, GameBase.level.found_goals, GameBase.level.total_goals, GameBase.level.found_secrets, GameBase.level.total_secrets));
            GameBase.gi.WriteByte(Defines.svc_layout);
            GameBase.gi.WriteString(sb.ToString());
            GameBase.gi.Unicast(ent, true);
        }
    }
}
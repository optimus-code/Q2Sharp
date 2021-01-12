using J2N.Text;
using Jake2.Client;
using Jake2.Qcommon;
using Jake2.Server;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Jake2.Game.pmove_t;

namespace Jake2.Game
{
    public class GameBase
    {
        public static cplane_t dummyplane = new cplane_t();
        public static game_locals_t game = new game_locals_t();
        public static level_locals_t level = new level_locals_t();
        public static game_import_t gi = new game_import_t();
        public static spawn_temp_t st = new spawn_temp_t();
        public static int sm_meat_index;
        public static int snd_fry;
        public static int meansOfDeath;
        public static int num_edicts;
        public static edict_t[] g_edicts = new edict_t[Defines.MAX_EDICTS];
        static GameBase()
        {
            for (int n = 0; n < Defines.MAX_EDICTS; n++)
                g_edicts[n] = new edict_t(n);

            for ( int n = 0; n < Defines.MAX_EDICTS; n++ )
                pushed[n] = new pushed_t();
        }

        public static cvar_t deathmatch = new cvar_t();
        public static cvar_t coop = new cvar_t();
        public static cvar_t dmflags = new cvar_t();
        public static cvar_t skill;
        public static cvar_t fraglimit = new cvar_t();
        public static cvar_t timelimit = new cvar_t();
        public static cvar_t password = new cvar_t();
        public static cvar_t spectator_password = new cvar_t();
        public static cvar_t needpass = new cvar_t();
        public static cvar_t maxclients = new cvar_t();
        public static cvar_t maxspectators = new cvar_t();
        public static cvar_t maxentities = new cvar_t();
        public static cvar_t g_select_empty = new cvar_t();
        public static cvar_t filterban = new cvar_t();
        public static cvar_t sv_maxvelocity = new cvar_t();
        public static cvar_t sv_gravity = new cvar_t();
        public static cvar_t sv_rollspeed = new cvar_t();
        public static cvar_t sv_rollangle = new cvar_t();
        public static cvar_t gun_x = new cvar_t();
        public static cvar_t gun_y = new cvar_t();
        public static cvar_t gun_z = new cvar_t();
        public static cvar_t run_pitch = new cvar_t();
        public static cvar_t run_roll = new cvar_t();
        public static cvar_t bob_up = new cvar_t();
        public static cvar_t bob_pitch = new cvar_t();
        public static cvar_t bob_roll = new cvar_t();
        public static cvar_t sv_cheats = new cvar_t();
        public static cvar_t flood_msgs = new cvar_t();
        public static cvar_t flood_persecond = new cvar_t();
        public static cvar_t flood_waitdelay = new cvar_t();
        public static cvar_t sv_maplist = new cvar_t();
        public static readonly float STOP_EPSILON = 0.1F;
        public static int ClipVelocity(float[] in_renamed, float[] normal, float[] out_renamed, float overbounce)
        {
            float backoff;
            float change;
            int i, blocked;
            blocked = 0;
            if (normal[2] > 0)
                blocked |= 1;
            if (normal[2] == 0F)
                blocked |= 2;
            backoff = Math3D.DotProduct(in_renamed, normal) * overbounce;
            for (i = 0; i < 3; i++)
            {
                change = normal[i] * backoff;
                out_renamed[i] = in_renamed[i] - change;
                if (out_renamed[i] > -STOP_EPSILON && out_renamed[i] < STOP_EPSILON)
                    out_renamed[i] = 0;
            }

            return blocked;
        }

        public static EdictIterator G_Find(EdictIterator from, EdictFindFilter eff, string s)
        {
            if (from == null)
                from = new EdictIterator(0);
            else
                from.i++;
            for (; from.i < num_edicts; from.i++)
            {
                from.o = g_edicts[from.i];
                if (from.o.classname == null)
                {
                    Com.Printf("edict with classname = null" + from.o.index);
                }

                if (!from.o.inuse)
                    continue;
                if (eff.Matches(from.o, s))
                    return from;
            }

            return null;
        }

        public static edict_t G_FindEdict(EdictIterator from, EdictFindFilter eff, string s)
        {
            EdictIterator ei = G_Find(from, eff, s);
            if (ei == null)
                return null;
            else
                return ei.o;
        }

        public static EdictIterator Findradius(EdictIterator from, float[] org, float rad)
        {
            float[] eorg = new[]{0f, 0f, 0f};
            int j;
            if (from == null)
                from = new EdictIterator(0);
            else
                from.i++;
            for (; from.i < num_edicts; from.i++)
            {
                from.o = g_edicts[from.i];
                if (!from.o.inuse)
                    continue;
                if (from.o.solid == Defines.SOLID_NOT)
                    continue;
                for (j = 0; j < 3; j++)
                    eorg[j] = org[j] - (from.o.s.origin[j] + (from.o.mins[j] + from.o.maxs[j]) * 0.5F);
                if (Math3D.VectorLength(eorg) > rad)
                    continue;
                return from;
            }

            return null;
        }

        public static int MAXCHOICES = 8;
        public static edict_t G_PickTarget(string targetname)
        {
            int num_choices = 0;
            edict_t[] choice = new edict_t[MAXCHOICES];
            if (targetname == null)
            {
                gi.Dprintf("G_PickTarget called with null targetname\\n");
                return null;
            }

            EdictIterator es = null;
            while ((es = G_Find(es, findByTarget, targetname)) != null)
            {
                choice[num_choices++] = es.o;
                if (num_choices == MAXCHOICES)
                    break;
            }

            if (num_choices == 0)
            {
                gi.Dprintf("G_PickTarget: target " + targetname + " not found\\n");
                return null;
            }

            return choice[Lib.Rand() % num_choices];
        }

        public static float[] VEC_UP = new float[]{0, -1, 0};
        public static float[] MOVEDIR_UP = new float[]{0, 0, 1};
        public static float[] VEC_DOWN = new float[]{0, -2, 0};
        public static float[] MOVEDIR_DOWN = new float[]{0, 0, -1};
        public static void G_SetMovedir(float[] angles, float[] movedir)
        {
            if (Math3D.VectorEquals(angles, VEC_UP))
            {
                Math3D.VectorCopy(MOVEDIR_UP, movedir);
            }
            else if (Math3D.VectorEquals(angles, VEC_DOWN))
            {
                Math3D.VectorCopy(MOVEDIR_DOWN, movedir);
            }
            else
            {
                Math3D.AngleVectors(angles, movedir, null, null);
            }

            Math3D.VectorClear(angles);
        }

        public static string G_CopyString(string in_renamed)
        {
            return new string (in_renamed);
        }

        static edict_t[] touch = new edict_t[Defines.MAX_EDICTS];
        public static void G_TouchTriggers(edict_t ent)
        {
            int i, num;
            edict_t hit;
            if ((ent.client != null || (ent.svflags & Defines.SVF_MONSTER) != 0) && (ent.health <= 0))
                return;
            num = gi.BoxEdicts(ent.absmin, ent.absmax, touch, Defines.MAX_EDICTS, Defines.AREA_TRIGGERS);
            for (i = 0; i < num; i++)
            {
                hit = touch[i];
                if (!hit.inuse)
                    continue;
                if (hit.touch == null)
                    continue;
                hit.touch.Touch(hit, ent, dummyplane, null);
            }
        }

        public static pushed_t[] pushed = new pushed_t[Defines.MAX_EDICTS];

        public static int pushed_p;
        public static edict_t obstacle;
        public static int c_yes, c_no;
        public static int STEPSIZE = 18;
        public static void G_RunEntity(edict_t ent)
        {
            if (ent.prethink != null)
                ent.prethink.Think(ent);
            switch ( (int)ent.movetype )
            {
                case Defines.MOVETYPE_PUSH:
                case Defines.MOVETYPE_STOP:
                    SV.SV_Physics_Pusher(ent);
                    break;
                case Defines.MOVETYPE_NONE:
                    SV.SV_Physics_None(ent);
                    break;
                case Defines.MOVETYPE_NOCLIP:
                    SV.SV_Physics_Noclip(ent);
                    break;
                case Defines.MOVETYPE_STEP:
                    SV.SV_Physics_Step(ent);
                    break;
                case Defines.MOVETYPE_TOSS:
                case Defines.MOVETYPE_BOUNCE:
                case Defines.MOVETYPE_FLY:
                case Defines.MOVETYPE_FLYMISSILE:
                    SV.SV_Physics_Toss(ent);
                    break;
                default:
                    gi.Error("SV_Physics: bad movetype " + (int)ent.movetype);
                    break;
            }
        }

        public static void ClearBounds(float[] mins, float[] maxs)
        {
            mins[0] = mins[1] = mins[2] = 99999;
            maxs[0] = maxs[1] = maxs[2] = -99999;
        }

        public static void AddPointToBounds(float[] v, float[] mins, float[] maxs)
        {
            int i;
            float val;
            for (i = 0; i < 3; i++)
            {
                val = v[i];
                if (val < mins[i])
                    mins[i] = val;
                if (val > maxs[i])
                    maxs[i] = val;
            }
        }

        public static EdictFindFilter findByTarget = new AnonymousEdictFindFilter();
        private sealed class AnonymousEdictFindFilter : EdictFindFilter
        {
            public override bool Matches(edict_t e, string s)
            {
                if (e.targetname == null)
                    return false;
                return e.targetname.EqualsIgnoreCase(s);
            }
        }

        public static EdictFindFilter findByClass = new AnonymousEdictFindFilter1();
        private sealed class AnonymousEdictFindFilter1 : EdictFindFilter
        {
            public override bool Matches(edict_t e, string s)
            {
                return e.classname.EqualsIgnoreCase(s);
            }
        }

        public static void ShutdownGame()
        {
            gi.Dprintf("==== ShutdownGame ====\\n");
        }

        public static void ClientEndServerFrames()
        {
            int i;
            edict_t ent;
            for (i = 0; i < maxclients.value; i++)
            {
                ent = g_edicts[1 + i];
                if (!ent.inuse || null == ent.client)
                    continue;
                PlayerView.ClientEndServerFrame(ent);
            }
        }

        public static edict_t CreateTargetChangeLevel(string map)
        {
            edict_t ent;
            ent = GameUtil.G_Spawn();
            ent.classname = "target_changelevel";
            level.nextmap = map;
            ent.map = level.nextmap;
            return ent;
        }

        public static void EndDMLevel()
        {
            edict_t ent;
            string s, t, f;
            string seps = " ,\\n\\r";
            if (((int)dmflags.value & Defines.DF_SAME_LEVEL) != 0)
            {
                PlayerHud.BeginIntermission(CreateTargetChangeLevel(level.mapname));
                return;
            }

            if (sv_maplist.string_renamed.Length > 0)
            {
                s = sv_maplist.string_renamed;
                f = null;
                StringTokenizer tk = new StringTokenizer(s, seps);
                while (tk.RemainingTokens > 0)
                {
                    tk.MoveNext();
                    t = tk.Current;
                    if (f == null)
                        f = t;
                    if (t.EqualsIgnoreCase(level.mapname))
                    {
                        if ( tk.RemainingTokens == 0 )
                        {
                            if ( f == null )
                                PlayerHud.BeginIntermission( CreateTargetChangeLevel( level.mapname ) );
                            else
                                PlayerHud.BeginIntermission( CreateTargetChangeLevel( f ) );
                        }
                        else
                        {
                            tk.MoveNext();
                            PlayerHud.BeginIntermission( CreateTargetChangeLevel( tk.Current ) );
                        }
                        return;
                    }
                }
            }

            if (level.nextmap.Length > 0)
                PlayerHud.BeginIntermission(CreateTargetChangeLevel(level.nextmap));
            else
            {
                EdictIterator edit = null;
                edit = G_Find(edit, findByClass, "target_changelevel");
                if (edit == null)
                {
                    PlayerHud.BeginIntermission(CreateTargetChangeLevel(level.mapname));
                    return;
                }

                ent = edit.o;
                PlayerHud.BeginIntermission(ent);
            }
        }

        public static void CheckNeedPass()
        {
            int need;
            if (password.modified || spectator_password.modified)
            {
                password.modified = spectator_password.modified = false;
                need = 0;
                if ((password.string_renamed.Length > 0) && 0 != Lib.Q_stricmp(password.string_renamed, "none"))
                    need |= 1;
                if ((spectator_password.string_renamed.Length > 0) && 0 != Lib.Q_stricmp(spectator_password.string_renamed, "none"))
                    need |= 2;
                gi.Cvar_set("needpass", "" + need);
            }
        }

        public static void CheckDMRules()
        {
            int i;
            gclient_t cl;
            if (level.intermissiontime != 0)
                return;
            if (0 == deathmatch.value)
                return;
            if (timelimit.value != 0)
            {
                if (level.time >= timelimit.value * 60)
                {
                    gi.Bprintf(Defines.PRINT_HIGH, "Timelimit hit.\\n");
                    EndDMLevel();
                    return;
                }
            }

            if (fraglimit.value != 0)
            {
                for (i = 0; i < maxclients.value; i++)
                {
                    cl = game.clients[i];
                    if (!g_edicts[i + 1].inuse)
                        continue;
                    if (cl.resp.score >= fraglimit.value)
                    {
                        gi.Bprintf(Defines.PRINT_HIGH, "Fraglimit hit.\\n");
                        EndDMLevel();
                        return;
                    }
                }
            }
        }

        public static void ExitLevel()
        {
            int i;
            edict_t ent;
            string command = "gamemap \\\"" + level.changemap + "\\\"\\n";
            gi.AddCommandString(command);
            level.changemap = null;
            level.exitintermission = false;
            level.intermissiontime = 0;
            ClientEndServerFrames();
            for (i = 0; i < maxclients.value; i++)
            {
                ent = g_edicts[1 + i];
                if (!ent.inuse)
                    continue;
                if (ent.health > ent.client.pers.max_health)
                    ent.health = ent.client.pers.max_health;
            }
        }

        public static void G_RunFrame()
        {
            int i;
            edict_t ent;
            level.framenum++;
            level.time = level.framenum * Defines.FRAMETIME;
            GameAI.AI_SetSightClient();
            if (level.exitintermission)
            {
                ExitLevel();
                return;
            }

            for (i = 0; i < num_edicts; i++)
            {
                ent = g_edicts[i];
                if (!ent.inuse)
                    continue;
                level.current_entity = ent;
                Math3D.VectorCopy(ent.s.origin, ent.s.old_origin);
                if ((ent.groundentity != null) && (ent.groundentity.linkcount != ent.groundentity_linkcount))
                {
                    ent.groundentity = null;
                    if (0 == (ent.flags & (Defines.FL_SWIM | Defines.FL_FLY)) && (ent.svflags & Defines.SVF_MONSTER) != 0)
                    {
                        M.M_CheckGround(ent);
                    }
                }

                if (i > 0 && i <= maxclients.value)
                {
                    PlayerClient.ClientBeginServerFrame(ent);
                    continue;
                }

                G_RunEntity(ent);
            }

            CheckDMRules();
            CheckNeedPass();
            ClientEndServerFrames();
        }

        public static void GetGameApi(game_import_t imp)
        {
            gi = imp;
            gi.pointcontents = new AnonymousPointContentsAdapter();
        }

        private sealed class AnonymousPointContentsAdapter : PointContentsAdapter
        {
            public override int Pointcontents(float[] o)
            {
                return SV_WORLD.SV_PointContents(o);
            }
        }
    }
}
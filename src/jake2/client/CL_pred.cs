using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Q2Sharp.Game.pmove_t;

namespace Q2Sharp.Client
{
    public class CL_pred
    {
        public static void CheckPredictionError()
        {
            int frame;
            int[] delta = new int[3];
            int i;
            int len;
            if (Globals.cl_predict.value == 0F || (Globals.cl.frame.playerstate.pmove.pm_flags & pmove_t.PMF_NO_PREDICTION) != 0)
                return;
            frame = Globals.cls.netchan.incoming_acknowledged;
            frame &= (Defines.CMD_BACKUP - 1);
            Math3D.VectorSubtract(Globals.cl.frame.playerstate.pmove.origin, Globals.cl.predicted_origins[frame], delta);
            len = Math.Abs(delta[0]) + Math.Abs(delta[1]) + Math.Abs(delta[2]);
            if (len > 640)
            {
                Math3D.VectorClear(Globals.cl.prediction_error);
            }
            else
            {
                if (Globals.cl_showmiss.value != 0F && (delta[0] != 0 || delta[1] != 0 || delta[2] != 0))
                    Com.Printf("prediction miss on " + Globals.cl.frame.serverframe + ": " + (delta[0] + delta[1] + delta[2]) + "\\n");
                Math3D.VectorCopy(Globals.cl.frame.playerstate.pmove.origin, Globals.cl.predicted_origins[frame]);
                for (i = 0; i < 3; i++)
                    Globals.cl.prediction_error[i] = delta[i] * 0.125F;
            }
        }

        static void ClipMoveToEntities(float[] start, float[] mins, float[] maxs, float[] end, trace_t tr)
        {
            int i, x, zd, zu;
            trace_t trace;
            int headnode;
            float[] angles;
            entity_state_t ent;
            int num;
            cmodel_t cmodel;
            float[] bmins = new float[3];
            float[] bmaxs = new float[3];
            for (i = 0; i < Globals.cl.frame.num_entities; i++)
            {
                num = (Globals.cl.frame.parse_entities + i) & (Defines.MAX_PARSE_ENTITIES - 1);
                ent = Globals.cl_parse_entities[num];
                if (ent.solid == 0)
                    continue;
                if (ent.number == Globals.cl.playernum + 1)
                    continue;
                if (ent.solid == 31)
                {
                    cmodel = Globals.cl.model_clip[ent.modelindex];
                    if (cmodel == null)
                        continue;
                    headnode = cmodel.headnode;
                    angles = ent.angles;
                }
                else
                {
                    x = 8 * (ent.solid & 31);
                    zd = 8 * ((ent.solid >> 5) & 31);
                    zu = 8 * ((ent.solid >> 10) & 63) - 32;
                    bmins[0] = bmins[1] = -x;
                    bmaxs[0] = bmaxs[1] = x;
                    bmins[2] = -zd;
                    bmaxs[2] = zu;
                    headnode = CM.HeadnodeForBox(bmins, bmaxs);
                    angles = Globals.vec3_origin;
                }

                if (tr.allsolid)
                    return;
                trace = CM.TransformedBoxTrace(start, end, mins, maxs, headnode, Defines.MASK_PLAYERSOLID, ent.origin, angles);
                if (trace.allsolid || trace.startsolid || trace.fraction < tr.fraction)
                {
                    trace.ent = ent.surrounding_ent;
                    if (tr.startsolid)
                    {
                        tr.Set(trace);
                        tr.startsolid = true;
                    }
                    else
                        tr.Set(trace);
                }
                else if (trace.startsolid)
                    tr.startsolid = true;
            }
        }

        public static edict_t DUMMY_ENT = new edict_t(-1);
        static trace_t PMTrace(float[] start, float[] mins, float[] maxs, float[] end)
        {
            trace_t t;
            t = CM.BoxTrace(start, end, mins, maxs, 0, Defines.MASK_PLAYERSOLID);
            if (t.fraction < 1F)
            {
                t.ent = DUMMY_ENT;
            }

            ClipMoveToEntities(start, mins, maxs, end, t);
            return t;
        }

        static int PMpointcontents(float[] point)
        {
            int i;
            entity_state_t ent;
            int num;
            cmodel_t cmodel;
            int contents;
            contents = CM.PointContents(point, 0);
            for (i = 0; i < Globals.cl.frame.num_entities; i++)
            {
                num = (Globals.cl.frame.parse_entities + i) & (Defines.MAX_PARSE_ENTITIES - 1);
                ent = Globals.cl_parse_entities[num];
                if (ent.solid != 31)
                    continue;
                cmodel = Globals.cl.model_clip[ent.modelindex];
                if (cmodel == null)
                    continue;
                contents |= CM.TransformedPointContents(point, cmodel.headnode, ent.origin, ent.angles);
            }

            return contents;
        }

        public static void PredictMovement()
        {
            if (Globals.cls.state != Defines.ca_active)
                return;
            if (Globals.cl_paused.value != 0F)
                return;
            if (Globals.cl_predict.value == 0F || (Globals.cl.frame.playerstate.pmove.pm_flags & pmove_t.PMF_NO_PREDICTION) != 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Globals.cl.predicted_angles[i] = Globals.cl.viewangles[i] + Math3D.SHORT2ANGLE(Globals.cl.frame.playerstate.pmove.delta_angles[i]);
                }

                return;
            }

            int ack = Globals.cls.netchan.incoming_acknowledged;
            int current = Globals.cls.netchan.outgoing_sequence;
            if (current - ack >= Defines.CMD_BACKUP)
            {
                if (Globals.cl_showmiss.value != 0F)
                    Com.Printf("exceeded CMD_BACKUP\\n");
                return;
            }

            pmove_t pm = new pmove_t();
            pm.trace = new AnonymousTraceAdapter();
            pm.pointcontents = new AnonymousPointContentsAdapter();
            PMove.pm_airaccelerate = Lib.Atof(Globals.cl.configstrings[Defines.CS_AIRACCEL]);
            pm.s.Set(Globals.cl.frame.playerstate.pmove);
            int frame = 0;
            usercmd_t cmd;
            while (++ack < current)
            {
                frame = ack & (Defines.CMD_BACKUP - 1);
                cmd = Globals.cl.cmds[frame];
                pm.cmd.Set(cmd);
                PMove.Pmove(pm);
                Math3D.VectorCopy(pm.s.origin, Globals.cl.predicted_origins[frame]);
            }

            int oldframe = (ack - 2) & (Defines.CMD_BACKUP - 1);
            int oldz = Globals.cl.predicted_origins[oldframe][2];
            int step = pm.s.origin[2] - oldz;
            if (step > 63 && step < 160 && (pm.s.pm_flags & pmove_t.PMF_ON_GROUND) != 0)
            {
                Globals.cl.predicted_step = step * 0.125F;
                Globals.cl.predicted_step_time = (int)(Globals.cls.realtime - Globals.cls.frametime * 500);
            }

            Globals.cl.predicted_origin[0] = pm.s.origin[0] * 0.125F;
            Globals.cl.predicted_origin[1] = pm.s.origin[1] * 0.125F;
            Globals.cl.predicted_origin[2] = pm.s.origin[2] * 0.125F;
            Math3D.VectorCopy(pm.viewangles, Globals.cl.predicted_angles);
        }

        private sealed class AnonymousTraceAdapter : TraceAdapter
        {
            public override trace_t Trace(float[] start, float[] mins, float[] maxs, float[] end)
            {
                return PMTrace(start, mins, maxs, end);
            }
        }

        private sealed class AnonymousPointContentsAdapter : PointContentsAdapter
        {
            public override int Pointcontents(float[] point)
            {
                return PMpointcontents(point);
            }
        }
    }
}
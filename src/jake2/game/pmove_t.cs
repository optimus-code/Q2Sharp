using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class pmove_t
    {
        public class TraceAdapter
        {
            public virtual trace_t Trace(float[] start, float[] mins, float[] maxs, float[] end)
            {
                return null;
            }
        }

        public class PointContentsAdapter
        {
            public virtual int Pointcontents(float[] point)
            {
                return 0;
            }
        }

        public pmove_state_t s = new pmove_state_t();
        public usercmd_t cmd = new usercmd_t();
        public bool snapinitial;
        public int numtouch;
        public edict_t[] touchents = new edict_t[Defines.MAXTOUCH];
        public float[] viewangles = new float[]{0, 0, 0};
        public float viewheight;
        public float[] mins = new float[]{0, 0, 0}, maxs = new float[]{0, 0, 0};
        public edict_t groundentity;
        public int watertype;
        public int waterlevel;
        public TraceAdapter trace;
        public PointContentsAdapter pointcontents;
        public static readonly int PMF_DUCKED = 1;
        public static readonly int PMF_JUMP_HELD = 2;
        public static readonly int PMF_ON_GROUND = 4;
        public static readonly int PMF_TIME_WATERJUMP = 8;
        public static readonly int PMF_TIME_LAND = 16;
        public static readonly int PMF_TIME_TELEPORT = 32;
        public static readonly int PMF_NO_PREDICTION = 64;
        public virtual void Clear()
        {
            groundentity = null;
            waterlevel = watertype = 0;
            trace = null;
            pointcontents = null;
            Math3D.VectorClear(mins);
            Math3D.VectorClear(maxs);
            viewheight = 0;
            Math3D.VectorClear(viewangles);
            touchents.Fill( null );
            numtouch = 0;
            snapinitial = false;
            cmd.Clear();
            s.Clear();
        }
    }
}
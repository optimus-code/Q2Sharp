using Q2Sharp.Game;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Server
{
    public class moveclip_t
    {
        public float[] boxmins = new float[]{0, 0, 0}, boxmaxs = new float[]{0, 0, 0};
        public float[] mins, maxs;
        public float[] mins2 = new float[]{0, 0, 0}, maxs2 = new float[]{0, 0, 0};
        public float[] start, end;
        public trace_t trace = new trace_t();
        public edict_t passedict;
        public int contentmask;
        public virtual void Clear()
        {
            Math3D.VectorClear(boxmins);
            Math3D.VectorClear(boxmaxs);
            Math3D.VectorClear(mins);
            Math3D.VectorClear(maxs);
            Math3D.VectorClear(mins2);
            Math3D.VectorClear(maxs2);
            start = end = null;
            trace.Clear();
            passedict = null;
            contentmask = 0;
        }
    }
}
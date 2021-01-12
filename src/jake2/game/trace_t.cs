using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class trace_t
    {
        public bool allsolid;
        public bool startsolid;
        public float fraction;
        public float[] endpos = new float[]{0, 0, 0};
        public cplane_t plane = new cplane_t();
        public csurface_t surface;
        public int contents;
        public edict_t ent;
        public virtual void Set(trace_t from)
        {
            allsolid = from.allsolid;
            startsolid = from.allsolid;
            fraction = from.fraction;
            Math3D.VectorCopy(from.endpos, endpos);
            plane.Set(from.plane);
            surface = from.surface;
            contents = from.contents;
            ent = from.ent;
        }

        public virtual void Clear()
        {
            allsolid = false;
            startsolid = false;
            fraction = 0;
            Math3D.VectorClear(endpos);
            plane.Clear();
            surface = null;
            contents = 0;
            ent = null;
        }
    }
}
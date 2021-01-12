using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class cplane_t
    {
        public float[] normal = new float[3];
        public float dist;
        public byte type;
        public byte signbits;
        public byte[] pad = new byte[]{0, 0};
        public virtual void Set(cplane_t c)
        {
            Math3D.Set(normal, c.normal);
            dist = c.dist;
            type = c.type;
            signbits = c.signbits;
            pad[0] = c.pad[0];
            pad[1] = c.pad[1];
        }

        public virtual void Clear()
        {
            Math3D.VectorClear(normal);
            dist = 0;
            type = 0;
            signbits = 0;
            pad[0] = 0;
            pad[1] = 0;
        }
    }
}
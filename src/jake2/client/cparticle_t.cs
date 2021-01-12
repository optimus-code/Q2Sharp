using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class cparticle_t
    {
        public cparticle_t next;
        public float time;
        public float[] org = new float[]{0, 0, 0};
        public float[] vel = new float[]{0, 0, 0};
        public float[] accel = new float[]{0, 0, 0};
        public float color;
        public float alpha;
        public float alphavel;
    }
}
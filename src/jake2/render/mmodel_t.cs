using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public class mmodel_t
    {
        public float[] mins = new float[]{0, 0, 0};
        public float[] maxs = new float[]{0, 0, 0};
        public float[] origin = new float[]{0, 0, 0};
        public float radius;
        public int headnode;
        public int visleafs;
        public int firstface, numfaces;
    }
}
using Jake2.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render
{
    public class mnode_t
    {
        public int contents;
        public int visframe;
        public float[] mins = new float[3];
        public float[] maxs = new float[3];
        public mnode_t parent;
        public cplane_t plane;
        public mnode_t[] children = new mnode_t[2];
        public int firstsurface;
        public int numsurfaces;
    }
}
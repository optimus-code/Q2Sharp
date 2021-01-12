using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class refdef_t
    {
        public int x, y, width, height;
        public float fov_x, fov_y;
        public float[] vieworg = new float[]{0, 0, 0};
        public float[] viewangles = new float[]{0, 0, 0};
        public float[] blend = new float[]{0, 0, 0, 0};
        public float time;
        public int rdflags;
        public byte[] areabits;
        public lightstyle_t[] lightstyles;
        public int num_entities;
        public entity_t[] entities;
        public int num_dlights;
        public dlight_t[] dlights;
        public int num_particles;
    }
}
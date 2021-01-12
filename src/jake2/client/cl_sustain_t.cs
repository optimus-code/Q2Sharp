using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class cl_sustain_t
    {
        public abstract class ThinkAdapter
        {
            public abstract void Think(cl_sustain_t self);
        }

        public int id;
        public int type;
        public int endtime;
        public int nextthink;
        public int thinkinterval;
        public float[] org = new float[3];
        public float[] dir = new float[3];
        public int color;
        public int count;
        public int magnitude;
        public ThinkAdapter think;
        public virtual void Clear()
        {
            org[0] = org[1] = org[2] = dir[0] = dir[1] = dir[2] = id = type = endtime = nextthink = thinkinterval = color = count = magnitude = 0;
            think = null;
        }
    }
}
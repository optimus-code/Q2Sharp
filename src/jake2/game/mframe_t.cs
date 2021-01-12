using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class mframe_t
    {
        public mframe_t(AIAdapter ai, float dist, EntThinkAdapter think)
        {
            this.ai = ai;
            this.dist = dist;
            this.think = think;
        }

        public mframe_t()
        {
        }

        public AIAdapter ai;
        public float dist;
        public EntThinkAdapter think;
        public virtual void Write(QuakeFile f)
        {
            f.WriteAdapter(ai);
            f.Write( dist);
            f.WriteAdapter(think);
        }

        public virtual void Read(QuakeFile f)
        {
            ai = (AIAdapter)f.ReadAdapter();
            dist = f.ReadSingle();
            think = (EntThinkAdapter)f.ReadAdapter();
        }
    }
}
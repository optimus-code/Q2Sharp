using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class mmove_t
    {
        public mmove_t(int firstframe, int lastframe, mframe_t[] frame, EntThinkAdapter endfunc)
        {
            this.firstframe = firstframe;
            this.lastframe = lastframe;
            this.frame = frame;
            this.endfunc = endfunc;
        }

        public mmove_t()
        {
        }

        public int firstframe;
        public int lastframe;
        public mframe_t[] frame;
        public EntThinkAdapter endfunc;
        public virtual void Write(QuakeFile f)
        {
            f.Write( firstframe);
            f.Write( lastframe);
            if (frame == null)
                f.Write( -1);
            else
            {
                f.Write( frame.Length);
                for (int n = 0; n < frame.Length; n++)
                    frame[n].Write(f);
            }

            f.WriteAdapter(endfunc);
        }

        public virtual void Read(QuakeFile f)
        {
            firstframe = f.ReadInt32();
            lastframe = f.ReadInt32();
            int len = f.ReadInt32();
            frame = new mframe_t[len];
            for (int n = 0; n < len; n++)
            {
                frame[n] = new mframe_t();
                frame[n].Read(f);
            }

            endfunc = (EntThinkAdapter)f.ReadAdapter();
        }
    }
}
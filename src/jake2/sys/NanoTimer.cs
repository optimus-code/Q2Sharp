using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Sys
{
    public class NanoTimer : Timer
    {
        private long base_renamed;
        public NanoTimer( )
        {
            base_renamed = System.NanoTime();
        }

        public override long CurrentTimeMillis()
        {
            long time = System.NanoTime();
            long delta = time - base_renamed;
            if (delta < 0)
            {
                delta += Long.MAX_VALUE + 1;
            }

            return (long)(delta * 1E-06);
        }
    }
}
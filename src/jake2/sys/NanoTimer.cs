using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Q2Sharp.Sys
{
    public class NanoTimer : Timer
    {
        private long base_renamed;
        public NanoTimer( )
        {
            base_renamed = 1000000000 * Stopwatch.GetTimestamp() / Stopwatch.Frequency;
        }

        public override long CurrentTimeMillis()
        {
            long time = 1000000000 * Stopwatch.GetTimestamp() / Stopwatch.Frequency;
            long delta = time - base_renamed;
            if (delta < 0)
            {
                delta += unchecked(long.MaxValue + 1);
            }

            return (long)(delta * 1E-06);
        }
    }
}
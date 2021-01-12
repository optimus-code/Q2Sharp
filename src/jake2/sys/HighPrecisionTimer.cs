using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Jake2.Sys
{
    public class HighPrecisionTimer : Timer
    {
        private Stopwatch perf = new Stopwatch();
        public HighPrecisionTimer( )
        {
            perf.Start();
        }

        public override long CurrentTimeMillis()
        {
            return perf.ElapsedMilliseconds;
        }
    }
}
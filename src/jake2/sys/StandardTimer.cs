using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Sys
{
    public class StandardTimer : Timer
    {
        private long base_renamed;
        public StandardTimer( )
        {
            base_renamed = System.CurrentTimeMillis();
        }

        public override long CurrentTimeMillis()
        {
            long time = System.CurrentTimeMillis();
            long delta = time - base_renamed;
            if (delta < 0)
            {
                delta += long.MaxValue + 1;
            }

            return delta;
        }
    }
}
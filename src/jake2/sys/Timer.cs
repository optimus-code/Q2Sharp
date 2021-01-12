using Q2Sharp.Qcommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Sys
{
    public abstract class Timer
    {
        public abstract long CurrentTimeMillis();
        static Timer t;
        static Timer()
        {
            try
            {
                t = new NanoTimer();
            }
            catch (Exception e)
            {
                try
                {
                    t = new HighPrecisionTimer();
                }
                catch ( Exception e1 )
                {
                    t = new StandardTimer();
                }
            }

            Com.Println("using " + t.GetType().Name);
        }

        public static int Milliseconds()
        {
            return Globals.curtime = (int)(t.CurrentTimeMillis());
        }
    }
}
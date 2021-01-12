using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public abstract class glpoly_t
    {
        public static readonly int STRIDE = 7;
        public static readonly int BYTE_STRIDE = 7 * Lib.SIZEOF_FLOAT;
        public static readonly int MAX_VERTICES = 64;
        public glpoly_t next;
        public glpoly_t chain;
        public int numverts;
        public int flags;
        public int pos = 0;
        protected glpoly_t()
        {
        }

        public abstract float X(int index);
        public abstract void X(int index, float value);
        public abstract float Y(int index);
        public abstract void Y(int index, float value);
        public abstract float Z(int index);
        public abstract void Z(int index, float value);
        public abstract float S1(int index);
        public abstract void S1(int index, float value);
        public abstract float T1(int index);
        public abstract void T1(int index, float value);
        public abstract float S2(int index);
        public abstract void S2(int index, float value);
        public abstract float T2(int index);
        public abstract void T2(int index, float value);
        public abstract void BeginScrolling(float s1);
        public abstract void EndScrolling();
    }
}
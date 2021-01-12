using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public sealed class console_t
    {
        public bool initialized;
        public byte[] text = new byte[Defines.CON_TEXTSIZE];
        public int current;
        public int x;
        public int display;
        public int ormask;
        public int linewidth;
        public int totallines;
        public float cursorspeed;
        public int vislines;
        public float[] times = new float[Defines.NUM_CON_TIMES];
    }
}
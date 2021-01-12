using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Qcommon
{
    public class miptex_t
    {
        public string name = "";
        public int width, height;
        public int[] offsets = new int[Defines.MIPLEVELS];
        public string animframe = "";
        public int flags;
        public int contents;
        public int value;
    }
}
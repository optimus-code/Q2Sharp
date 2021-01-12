using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public class mvertex_t
    {
        public static readonly int DISK_SIZE = 3 * Defines.SIZE_OF_FLOAT;
        public static readonly int MEM_SIZE = 3 * Defines.SIZE_OF_FLOAT;
        public float[] position = new float[]{0, 0, 0};
        public mvertex_t(ByteBuffer b)
        {
            position[0] = b.GetSingle();
            position[1] = b.GetSingle();
            position[2] = b.GetSingle();
        }
    }
}
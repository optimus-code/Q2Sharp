using J2N.IO;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class particle_t
    {
        private static ByteBuffer colorByteArray = Lib.NewByteBuffer(Defines.MAX_PARTICLES * Lib.SIZEOF_INT, ByteOrder.LittleEndian);
        public static SingleBuffer vertexArray = Lib.NewSingleBuffer(Defines.MAX_PARTICLES * 3);
        public static int[] colorTable = new int[256];
        public static Int32Buffer colorArray = colorByteArray.AsInt32Buffer();
        public static void SetColorPalette(int[] palette)
        {
            for (int i = 0; i < 256; i++)
            {
                colorTable[i] = palette[i] & 0x00FFFFFF;
            }
        }

        public static ByteBuffer GetColorAsByteBuffer()
        {
            return colorByteArray;
        }
    }
}
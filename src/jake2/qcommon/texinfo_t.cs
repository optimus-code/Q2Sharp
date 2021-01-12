using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Qcommon
{
    public class texinfo_t
    {
        public texinfo_t(byte[] cmod_base, int o, int len): this(ByteBuffer.Wrap(cmod_base, o, len).SetOrder(ByteOrder.LittleEndian))
        {
        }

        public texinfo_t(ByteBuffer bb)
        {
            byte[] str = new byte[32];
            vecs[0] = new float[] {bb.GetSingle(), bb.GetSingle(), bb.GetSingle(), bb.GetSingle()};
            vecs[1] = new float[] {bb.GetSingle(), bb.GetSingle(), bb.GetSingle(), bb.GetSingle()};
            flags = bb.GetInt32();
            value = bb.GetInt32();
            bb.Get(str);
            texture = Encoding.ASCII.GetString( str );
            nexttexinfo = bb.GetInt32();
        }

        public static readonly int SIZE = 32 + 4 + 4 + 32 + 4;
        public float[][] vecs = new float[][]{ new float[]{0, 0, 0, 0}, new float[]{0, 0, 0, 0}};
        public int flags;
        public int value;
        public string texture = "";
        public int nexttexinfo;
    }
}
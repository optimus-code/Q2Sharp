using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public abstract class EndianHandler
    {
        private static readonly int mask = 0xFF;
        public abstract float BigFloat(float f);
        public abstract short BigShort(short s);
        public abstract int BigLong(int i);
        public abstract float LittleFloat(float f);
        public abstract short LittleShort(short s);
        public abstract int LittleLong(int i);
        public static float SwapFloat(float f)
        {
            int i = BitConverter.ToInt32(BitConverter.GetBytes(f));
            i = SwapInt(i);
            f = BitConverter.ToSingle(BitConverter.GetBytes(i));
            return f;
        }

        public static int SwapInt(int i)
        {
            int a = i & mask;
            i >>= 8;
            a <<= 24;
            int b = i & mask;
            i >>= 8;
            b <<= 16;
            int c = i & mask;
            i >>= 8;
            c <<= 8;
            return i | c | b | a;
        }

        public static short SwapShort(short s)
        {
            int a = s & mask;
            a <<= 8;
            int b = (s >> 8) & mask;
            return (short)(b | a);
        }
    }
}
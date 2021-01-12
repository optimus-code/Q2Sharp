using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Util
{
    public sealed class Vec3Cache
    {
        private static readonly float[][] cache = Lib.CreateJaggedArray<float[][]>(64,3);
        private static int index = 0;
        private static int max = 0;
        public static float[] Get()
        {
            return cache[index++];
        }

        public static void Release()
        {
            index--;
        }

        public static void Release(int count)
        {
            index -= count;
        }

        public static void Debug()
        {
            System.Diagnostics.Debug.WriteLine("Vec3Cache: max. " + (max + 1) + " vectors used.");
        }
    }
}
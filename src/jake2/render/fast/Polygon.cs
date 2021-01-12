using J2N.IO;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render.Fast
{
    public sealed class Polygon : glpoly_t
    {
        private static readonly int MAX_POLYS = 20000;
        private static readonly int MAX_BUFFER_VERTICES = 120000;
        private static float[] s1_old = new float[MAX_VERTICES];
        private static SingleBuffer buffer = Lib.NewSingleBuffer(MAX_BUFFER_VERTICES * STRIDE);
        private static int bufferIndex = 0;
        private static int polyCount = 0;
        private static Polygon[] polyCache = new Polygon[MAX_POLYS];
        static Polygon()
        {
            for (int i = 0; i < polyCache.Length; i++)
            {
                polyCache[i] = new Polygon();
            }
        }

        public static glpoly_t Create(int numverts)
        {
            Polygon poly = polyCache[polyCount++];
            poly.Clear();
            poly.numverts = numverts;
            poly.pos = bufferIndex;
            bufferIndex += numverts;
            return poly;
        }

        public static void Reset()
        {
            polyCount = 0;
            bufferIndex = 0;
        }

        public static SingleBuffer GetInterleavedBuffer()
        {
            return (SingleBuffer)buffer.Rewind();
        }

        private Polygon()
        {
        }

        private void Clear()
        {
            next = null;
            chain = null;
            numverts = 0;
            flags = 0;
        }

        public override float S1(int index)
        {
            return buffer.Get((index + pos) * STRIDE);
        }

        public override void S1(int index, float value)
        {
            buffer.Put((index + pos) * STRIDE, value);
        }

        public override float T1(int index)
        {
            return buffer.Get((index + pos) * STRIDE + 1);
        }

        public override void T1(int index, float value)
        {
            buffer.Put((index + pos) * STRIDE + 1, value);
        }

        public override float X(int index)
        {
            return buffer.Get((index + pos) * STRIDE + 2);
        }

        public override void X(int index, float value)
        {
            buffer.Put((index + pos) * STRIDE + 2, value);
        }

        public override float Y(int index)
        {
            return buffer.Get((index + pos) * STRIDE + 3);
        }

        public override void Y(int index, float value)
        {
            buffer.Put((index + pos) * STRIDE + 3, value);
        }

        public override float Z(int index)
        {
            return buffer.Get((index + pos) * STRIDE + 4);
        }

        public override void Z(int index, float value)
        {
            buffer.Put((index + pos) * STRIDE + 4, value);
        }

        public override float S2(int index)
        {
            return buffer.Get((index + pos) * STRIDE + 5);
        }

        public override void S2(int index, float value)
        {
            buffer.Put((index + pos) * STRIDE + 5, value);
        }

        public override float T2(int index)
        {
            return buffer.Get((index + pos) * STRIDE + 6);
        }

        public override void T2(int index, float value)
        {
            buffer.Put((index + pos) * STRIDE + 6, value);
        }

        public override void BeginScrolling(float scroll)
        {
            int index = pos * STRIDE;
            for (int i = 0; i < numverts; i++, index += STRIDE)
            {
                scroll += s1_old[i] = buffer.Get(index);
                buffer.Put(index, scroll);
            }
        }

        public override void EndScrolling()
        {
            int index = pos * STRIDE;
            for (int i = 0; i < numverts; i++, index += STRIDE)
            {
                buffer.Put(index, s1_old[i]);
            }
        }
    }
}
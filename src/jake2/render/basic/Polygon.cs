using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render.Basic
{
    public sealed class Polygon : glpoly_t
    {
        private static readonly int MAXPOLYS = 20000;
        private static readonly int MAX_BUFFER_VERTICES = 120000;
        private static float[] buffer = new float[MAX_BUFFER_VERTICES * STRIDE];
        private static int bufferIndex = 0;
        private static int polyCount = 0;
        private static Polygon[] polyCache = new Polygon[MAXPOLYS];
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

        public override float X(int index)
        {
            return buffer[Offset(index)];
        }

        public override void X(int index, float value)
        {
            buffer[Offset(index)] = value;
        }

        public override float Y(int index)
        {
            return buffer[Offset(index) + 1];
        }

        public override void Y(int index, float value)
        {
            buffer[Offset(index) + 1] = value;
        }

        public override float Z(int index)
        {
            return buffer[Offset(index) + 2];
        }

        public override void Z(int index, float value)
        {
            buffer[Offset(index) + 2] = value;
        }

        public override float S1(int index)
        {
            return buffer[Offset(index) + 3];
        }

        public override void S1(int index, float value)
        {
            buffer[Offset(index) + 3] = value;
        }

        public override float T1(int index)
        {
            return buffer[Offset(index) + 4];
        }

        public override void T1(int index, float value)
        {
            buffer[Offset(index) + 4] = value;
        }

        public override float S2(int index)
        {
            return buffer[Offset(index) + 5];
        }

        public override void S2(int index, float value)
        {
            buffer[Offset(index) + 5] = value;
        }

        public override float T2(int index)
        {
            return buffer[Offset(index) + 6];
        }

        public override void T2(int index, float value)
        {
            buffer[Offset(index) + 6] = value;
        }

        public override void BeginScrolling(float value)
        {
        }

        public override void EndScrolling()
        {
        }

        private int Offset(int index)
        {
            return (index + pos) * STRIDE;
        }
    }
}
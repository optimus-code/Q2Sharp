using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Q2Sharp.Render.Fast
{
    public abstract class Warp : Model
    {
        public static readonly float[] SIN = new[]{0F, 0.19633F, 0.392541F, 0.588517F, 0.784137F, 0.979285F, 1.17384F, 1.3677F, 1.56072F, 1.75281F, 1.94384F, 2.1337F, 2.32228F, 2.50945F, 2.69512F, 2.87916F, 3.06147F, 3.24193F, 3.42044F, 3.59689F, 3.77117F, 3.94319F, 4.11282F, 4.27998F, 4.44456F, 4.60647F, 4.76559F, 4.92185F, 5.07515F, 5.22538F, 5.37247F, 5.51632F, 5.65685F, 5.79398F, 5.92761F, 6.05767F, 6.18408F, 6.30677F, 6.42566F, 6.54068F, 6.65176F, 6.75883F, 6.86183F, 6.9607F, 7.05537F, 7.14579F, 7.23191F, 7.31368F, 7.39104F, 7.46394F, 7.53235F, 7.59623F, 7.65552F, 7.71021F, 7.76025F, 7.80562F, 7.84628F, 7.88222F, 7.91341F, 7.93984F, 7.96148F, 7.97832F, 7.99036F, 7.99759F, 8F, 7.99759F, 7.99036F, 7.97832F, 7.96148F, 7.93984F, 7.91341F, 7.88222F, 7.84628F, 7.80562F, 7.76025F, 7.71021F, 7.65552F, 7.59623F, 7.53235F, 7.46394F, 7.39104F, 7.31368F, 7.23191F, 7.14579F, 7.05537F, 6.9607F, 6.86183F, 6.75883F, 6.65176F, 6.54068F, 6.42566F, 6.30677F, 6.18408F, 6.05767F, 5.92761F, 5.79398F, 5.65685F, 5.51632F, 5.37247F, 5.22538F, 5.07515F, 4.92185F, 4.76559F, 4.60647F, 4.44456F, 4.27998F, 4.11282F, 3.94319F, 3.77117F, 3.59689F, 3.42044F, 3.24193F, 3.06147F, 2.87916F, 2.69512F, 2.50945F, 2.32228F, 2.1337F, 1.94384F, 1.75281F, 1.56072F, 1.3677F, 1.17384F, 0.979285F, 0.784137F, 0.588517F, 0.392541F, 0.19633F, 9.79717E-16F, -0.19633F, -0.392541F, -0.588517F, -0.784137F, -0.979285F, -1.17384F, -1.3677F, -1.56072F, -1.75281F, -1.94384F, -2.1337F, -2.32228F, -2.50945F, -2.69512F, -2.87916F, -3.06147F, -3.24193F, -3.42044F, -3.59689F, -3.77117F, -3.94319F, -4.11282F, -4.27998F, -4.44456F, -4.60647F, -4.76559F, -4.92185F, -5.07515F, -5.22538F, -5.37247F, -5.51632F, -5.65685F, -5.79398F, -5.92761F, -6.05767F, -6.18408F, -6.30677F, -6.42566F, -6.54068F, -6.65176F, -6.75883F, -6.86183F, -6.9607F, -7.05537F, -7.14579F, -7.23191F, -7.31368F, -7.39104F, -7.46394F, -7.53235F, -7.59623F, -7.65552F, -7.71021F, -7.76025F, -7.80562F, -7.84628F, -7.88222F, -7.91341F, -7.93984F, -7.96148F, -7.97832F, -7.99036F, -7.99759F, -8F, -7.99759F, -7.99036F, -7.97832F, -7.96148F, -7.93984F, -7.91341F, -7.88222F, -7.84628F, -7.80562F, -7.76025F, -7.71021F, -7.65552F, -7.59623F, -7.53235F, -7.46394F, -7.39104F, -7.31368F, -7.23191F, -7.14579F, -7.05537F, -6.9607F, -6.86183F, -6.75883F, -6.65176F, -6.54068F, -6.42566F, -6.30677F, -6.18408F, -6.05767F, -5.92761F, -5.79398F, -5.65685F, -5.51632F, -5.37247F, -5.22538F, -5.07515F, -4.92185F, -4.76559F, -4.60647F, -4.44456F, -4.27998F, -4.11282F, -3.94319F, -3.77117F, -3.59689F, -3.42044F, -3.24193F, -3.06147F, -2.87916F, -2.69512F, -2.50945F, -2.32228F, -2.1337F, -1.94384F, -1.75281F, -1.56072F, -1.3677F, -1.17384F, -0.979285F, -0.784137F, -0.588517F, -0.392541F, -0.19633F};
        string skyname;
        float skyrotate;
        float[] skyaxis = new float[]{0, 0, 0};
        image_t[] sky_images = new image_t[6];
        msurface_t warpface;
        static readonly int SUBDIVIDE_SIZE = 64;
        public virtual void BoundPoly(int numverts, float[][] verts, float[] mins, float[] maxs)
        {
            mins[0] = mins[1] = mins[2] = 9999;
            maxs[0] = maxs[1] = maxs[2] = -9999;
            int j;
            float[] v;
            for (int i = 0; i < numverts; i++)
            {
                v = verts[i];
                for (j = 0; j < 3; j++)
                {
                    if (v[j] < mins[j])
                        mins[j] = v[j];
                    if (v[j] > maxs[j])
                        maxs[j] = v[j];
                }
            }
        }

        public virtual void SubdividePolygon(int numverts, float[][] verts)
        {
            int i, j, k;
            float m;
            float[][] front = Lib.CreateJaggedArray<float[][]>(64, 3);
            float[][] back = Lib.CreateJaggedArray<float[][]>( 64, 3 );
            int f, b;
            float[] dist = new float[64];
            float frac;
            if (numverts > 60)
                Com.Error(Defines.ERR_DROP, "numverts = " + numverts);
            float[] mins = Vec3Cache.Get();
            float[] maxs = Vec3Cache.Get();
            BoundPoly(numverts, verts, mins, maxs);
            float[] v;
            for (i = 0; i < 3; i++)
            {
                m = (mins[i] + maxs[i]) * 0.5F;
                m = SUBDIVIDE_SIZE * (float)Math.Floor(m / SUBDIVIDE_SIZE + 0.5F);
                if (maxs[i] - m < 8)
                    continue;
                if (m - mins[i] < 8)
                    continue;
                for (j = 0; j < numverts; j++)
                {
                    dist[j] = verts[j][i] - m;
                }

                dist[j] = dist[0];
                Math3D.VectorCopy(verts[0], verts[numverts]);
                f = b = 0;
                for (j = 0; j < numverts; j++)
                {
                    v = verts[j];
                    if (dist[j] >= 0)
                    {
                        Math3D.VectorCopy(v, front[f]);
                        f++;
                    }

                    if (dist[j] <= 0)
                    {
                        Math3D.VectorCopy(v, back[b]);
                        b++;
                    }

                    if (dist[j] == 0 || dist[j + 1] == 0)
                        continue;
                    if ((dist[j] > 0) != (dist[j + 1] > 0))
                    {
                        frac = dist[j] / (dist[j] - dist[j + 1]);
                        for (k = 0; k < 3; k++)
                            front[f][k] = back[b][k] = v[k] + frac * (verts[j + 1][k] - v[k]);
                        f++;
                        b++;
                    }
                }

                SubdividePolygon(f, front);
                SubdividePolygon(b, back);
                Vec3Cache.Release(2);
                return;
            }

            Vec3Cache.Release(2);
            glpoly_t poly = Polygon.Create(numverts + 2);
            poly.next = warpface.polys;
            warpface.polys = poly;
            float[] total = Vec3Cache.Get();
            Math3D.VectorClear(total);
            float total_s = 0;
            float total_t = 0;
            float s, t;
            for (i = 0; i < numverts; i++)
            {
                poly.X(i + 1, verts[i][0]);
                poly.Y(i + 1, verts[i][1]);
                poly.Z(i + 1, verts[i][2]);
                s = Math3D.DotProduct(verts[i], warpface.texinfo.vecs[0]);
                t = Math3D.DotProduct(verts[i], warpface.texinfo.vecs[1]);
                total_s += s;
                total_t += t;
                Math3D.VectorAdd(total, verts[i], total);
                poly.S1(i + 1, s);
                poly.T1(i + 1, t);
            }

            float scale = 1F / numverts;
            poly.X(0, total[0] * scale);
            poly.Y(0, total[1] * scale);
            poly.Z(0, total[2] * scale);
            poly.S1(0, total_s * scale);
            poly.T1(0, total_t * scale);
            poly.X(i + 1, poly.X(1));
            poly.Y(i + 1, poly.Y(1));
            poly.Z(i + 1, poly.Z(1));
            poly.S1(i + 1, poly.S1(1));
            poly.T1(i + 1, poly.T1(1));
            poly.S2(i + 1, poly.S2(1));
            poly.T2(i + 1, poly.T2(1));
            Vec3Cache.Release();
        }

        private readonly float[][] tmpVerts = Lib.CreateJaggedArray<float[][]>( 64, 3 );
        public override void GL_SubdivideSurface(msurface_t fa)
        {
            float[][] verts = tmpVerts;
            float[] vec;
            warpface = fa;
            int numverts = 0;
            for (int i = 0; i < fa.numedges; i++)
            {
                int lindex = loadmodel.surfedges[fa.firstedge + i];
                if (lindex > 0)
                    vec = loadmodel.vertexes[loadmodel.edges[lindex].v[0]].position;
                else
                    vec = loadmodel.vertexes[loadmodel.edges[-lindex].v[1]].position;
                Math3D.VectorCopy(vec, verts[numverts]);
                numverts++;
            }

            SubdividePolygon(numverts, verts);
        }

        static readonly float TURBSCALE = (float)(256F / (2 * Math.PI));
        public override void EmitWaterPolys(msurface_t fa)
        {
            float rdt = r_newrefdef.time;
            float scroll;
            if ((fa.texinfo.flags & Defines.SURF_FLOWING) != 0)
                scroll = -64 * ((r_newrefdef.time * 0.5F) - (int)(r_newrefdef.time * 0.5F));
            else
                scroll = 0;
            int i;
            float s, t, os, ot;
            glpoly_t p, bp;
            for (bp = fa.polys; bp != null; bp = bp.next)
            {
                p = bp;
                GL.Begin(PrimitiveType.TriangleFan);
                for (i = 0; i < p.numverts; i++)
                {
                    os = p.S1(i);
                    ot = p.T1(i);
                    s = os + Warp.SIN[(int)((ot * 0.125F + r_newrefdef.time) * TURBSCALE) & 255];
                    s += scroll;
                    s *= (1F / 64);
                    t = ot + Warp.SIN[(int)((os * 0.125F + rdt) * TURBSCALE) & 255];
                    t *= (1F / 64);
                    GL.TexCoord2(s, t);
                    GL.Vertex3(p.X(i), p.Y(i), p.Z(i));
                }

                GL.End();
            }
        }

        float[][] skyclip = new float[][]{new float[]{1, 1, 0}, new float[]{1, -1, 0}, new float[]{0, -1, 1}, new float[]{0, 1, 1}, new float[]{1, 0, 1}, new float[]{-1, 0, 1}};
        int c_sky;
        int[][] st_to_vec = new[]{new[]{3, -1, 2}, new[]{-3, 1, 2}, new[]{1, 3, 2}, new[]{-1, -3, 2}, new[]{-2, -1, 3}, new[]{2, -1, -3}};
        int[][] vec_to_st = new[]{new[]{-2, 3, 1}, new[]{2, 3, -1}, new[]{1, 3, 2}, new[]{-1, 3, -2}, new[]{-2, -1, 3}, new[]{-2, 1, -3}};
        float[][] skymins = Lib.CreateJaggedArray<float[][]>(2, 6);
        float[][] skymaxs = Lib.CreateJaggedArray<float[][]>(2, 6);
        float sky_min, sky_max;
        private readonly float[] v = new float[]{0, 0, 0};
        private readonly float[] av = new float[]{0, 0, 0};
        public virtual void DrawSkyPolygon(int nump, float[][] vecs)
        {
            c_sky++;
            Math3D.VectorCopy(Globals.vec3_origin, v);
            int i, axis;
            for (i = 0; i < nump; i++)
            {
                Math3D.VectorAdd(vecs[i], v, v);
            }

            av[0] = Math.Abs(v[0]);
            av[1] = Math.Abs(v[1]);
            av[2] = Math.Abs(v[2]);
            if (av[0] > av[1] && av[0] > av[2])
            {
                if (v[0] < 0)
                    axis = 1;
                else
                    axis = 0;
            }
            else if (av[1] > av[2] && av[1] > av[0])
            {
                if (v[1] < 0)
                    axis = 3;
                else
                    axis = 2;
            }
            else
            {
                if (v[2] < 0)
                    axis = 5;
                else
                    axis = 4;
            }

            float s, t, dv;
            int j;
            for (i = 0; i < nump; i++)
            {
                j = vec_to_st[axis][2];
                if (j > 0)
                    dv = vecs[i][j - 1];
                else
                    dv = -vecs[i][-j - 1];
                if (dv < 0.001F)
                    continue;
                j = vec_to_st[axis][0];
                if (j < 0)
                    s = -vecs[i][-j - 1] / dv;
                else
                    s = vecs[i][j - 1] / dv;
                j = vec_to_st[axis][1];
                if (j < 0)
                    t = -vecs[i][-j - 1] / dv;
                else
                    t = vecs[i][j - 1] / dv;
                if (s < skymins[0][axis])
                    skymins[0][axis] = s;
                if (t < skymins[1][axis])
                    skymins[1][axis] = t;
                if (s > skymaxs[0][axis])
                    skymaxs[0][axis] = s;
                if (t > skymaxs[1][axis])
                    skymaxs[1][axis] = t;
            }
        }

        public const float ON_EPSILON = 0.1F;
        public const int MAX_CLIP_VERTS = 64;
        public const int SIDE_BACK = 1;
        public const int SIDE_FRONT = 0;
        public const int SIDE_ON = 2;
        public float[] dists = new float[MAX_CLIP_VERTS];
        public int[] sides = new int[MAX_CLIP_VERTS];
        public float[][][][] newv = Lib.CreateJaggedArray<float[][][][]>(6, 2, MAX_CLIP_VERTS, 3);
        public virtual void ClipSkyPolygon(int nump, float[][] vecs, int stage)
        {
            if (nump > MAX_CLIP_VERTS - 2)
                Com.Error(Defines.ERR_DROP, "ClipSkyPolygon: MAX_CLIP_VERTS");
            if (stage == 6)
            {
                DrawSkyPolygon(nump, vecs);
                return;
            }

            bool front = false;
            bool back = false;
            float[] norm = skyclip[stage];
            int i;
            float d;
            for (i = 0; i < nump; i++)
            {
                d = Math3D.DotProduct(vecs[i], norm);
                if (d > ON_EPSILON)
                {
                    front = true;
                    sides[i] = SIDE_FRONT;
                }
                else if (d < -ON_EPSILON)
                {
                    back = true;
                    sides[i] = SIDE_BACK;
                }
                else
                    sides[i] = SIDE_ON;
                dists[i] = d;
            }

            if (!front || !back)
            {
                ClipSkyPolygon(nump, vecs, stage + 1);
                return;
            }

            sides[i] = sides[0];
            dists[i] = dists[0];
            Math3D.VectorCopy(vecs[0], vecs[i]);
            int newc0 = 0;
            int newc1 = 0;
            float[] v;
            float e;
            int j;
            for (i = 0; i < nump; i++)
            {
                v = vecs[i];
                switch (sides[i])

                {
                    case SIDE_FRONT:
                        Math3D.VectorCopy(v, newv[stage][0][newc0]);
                        newc0++;
                        break;
                    case SIDE_BACK:
                        Math3D.VectorCopy(v, newv[stage][1][newc1]);
                        newc1++;
                        break;
                    case SIDE_ON:
                        Math3D.VectorCopy(v, newv[stage][0][newc0]);
                        newc0++;
                        Math3D.VectorCopy(v, newv[stage][1][newc1]);
                        newc1++;
                        break;
                }

                if (sides[i] == SIDE_ON || sides[i + 1] == SIDE_ON || sides[i + 1] == sides[i])
                    continue;
                d = dists[i] / (dists[i] - dists[i + 1]);
                for (j = 0; j < 3; j++)
                {
                    e = v[j] + d * (vecs[i + 1][j] - v[j]);
                    newv[stage][0][newc0][j] = e;
                    newv[stage][1][newc1][j] = e;
                }

                newc0++;
                newc1++;
            }

            ClipSkyPolygon(newc0, newv[stage][0], stage + 1);
            ClipSkyPolygon(newc1, newv[stage][1], stage + 1);
        }

        float[][] verts = Lib.CreateJaggedArray<float[][]>(MAX_CLIP_VERTS, 3);
        public override void R_AddSkySurface(msurface_t fa)
        {
            for (glpoly_t p = fa.polys; p != null; p = p.next)
            {
                for (int i = 0; i < p.numverts; i++)
                {
                    verts[i][0] = p.X(i) - r_origin[0];
                    verts[i][1] = p.Y(i) - r_origin[1];
                    verts[i][2] = p.Z(i) - r_origin[2];
                }

                ClipSkyPolygon(p.numverts, verts, 0);
            }
        }

        public override void R_ClearSkyBox()
        {
            float[] skymins0 = skymins[0];
            float[] skymins1 = skymins[1];
            float[] skymaxs0 = skymaxs[0];
            float[] skymaxs1 = skymaxs[1];
            for (int i = 0; i < 6; i++)
            {
                skymins0[i] = skymins1[i] = 9999;
                skymaxs0[i] = skymaxs1[i] = -9999;
            }
        }

        private readonly float[] v1 = new float[]{0, 0, 0};
        private readonly float[] b = new float[]{0, 0, 0};
        public virtual void MakeSkyVec(float s, float t, int axis)
        {
            b[0] = s * 2300;
            b[1] = t * 2300;
            b[2] = 2300;
            int j, k;
            for (j = 0; j < 3; j++)
            {
                k = st_to_vec[axis][j];
                if (k < 0)
                    v1[j] = -b[-k - 1];
                else
                    v1[j] = b[k - 1];
            }

            s = (s + 1) * 0.5F;
            t = (t + 1) * 0.5F;
            if (s < sky_min)
                s = sky_min;
            else if (s > sky_max)
                s = sky_max;
            if (t < sky_min)
                t = sky_min;
            else if (t > sky_max)
                t = sky_max;
            t = 1F - t;
            GL.TexCoord2(s, t);
            GL.Vertex3(v1[0], v1[1], v1[2]);
        }

        int[] skytexorder = new[]{0, 2, 1, 3, 4, 5};
        public override void R_DrawSkyBox()
        {
            int i;
            if (skyrotate != 0)
            {
                for (i = 0; i < 6; i++)
                    if (skymins[0][i] < skymaxs[0][i] && skymins[1][i] < skymaxs[1][i])
                        break;
                if (i == 6)
                    return;
            }

            GL.PushMatrix();
            GL.Translate(r_origin[0], r_origin[1], r_origin[2]);
            GL.Rotate(r_newrefdef.time * skyrotate, skyaxis[0], skyaxis[1], skyaxis[2]);
            for (i = 0; i < 6; i++)
            {
                if (skyrotate != 0)
                {
                    skymins[0][i] = -1;
                    skymins[1][i] = -1;
                    skymaxs[0][i] = 1;
                    skymaxs[1][i] = 1;
                }

                if (skymins[0][i] >= skymaxs[0][i] || skymins[1][i] >= skymaxs[1][i])
                    continue;
                GL_Bind(sky_images[skytexorder[i]].texnum);
                GL.Begin(PrimitiveType.Quads);
                MakeSkyVec(skymins[0][i], skymins[1][i], i);
                MakeSkyVec(skymins[0][i], skymaxs[1][i], i);
                MakeSkyVec(skymaxs[0][i], skymaxs[1][i], i);
                MakeSkyVec(skymaxs[0][i], skymins[1][i], i);
                GL.End();
            }

            GL.PopMatrix();
        }

        String[] suf = new[]{"rt", "bk", "lf", "ft", "up", "dn"};
        public override void R_SetSky(string name, float rotate, float[] axis)
        {
            string pathname;
            skyname = name;
            skyrotate = rotate;
            Math3D.VectorCopy(axis, skyaxis);
            for (int i = 0; i < 6; i++)
            {
                if (gl_skymip.value != 0 || skyrotate != 0)
                    gl_picmip.value++;
                if (qglColorTableEXT && gl_ext_palettedtexture.value != 0)
                {
                    pathname = "env/" + skyname + suf[i] + ".pcx";
                }
                else
                {
                    pathname = "env/" + skyname + suf[i] + ".tga";
                }

                sky_images[i] = GL_FindImage(pathname, it_sky);
                if (sky_images[i] == null)
                    sky_images[i] = r_notexture;
                if (gl_skymip.value != 0 || skyrotate != 0)
                {
                    gl_picmip.value--;
                    sky_min = 1F / 256;
                    sky_max = 255F / 256;
                }
                else
                {
                    sky_min = 1F / 512;
                    sky_max = 511F / 512;
                }
            }
        }
    }
}
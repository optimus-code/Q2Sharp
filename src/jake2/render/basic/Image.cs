using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using J2N.IO;
using Jake2.Client;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using OpenTK.Graphics.OpenGL;
using Q2Sharp.util;
using static Jake2.Qcommon.qfiles;

namespace Jake2.Render.Basic
{
    public abstract class Image : Main
    {
        public image_t draw_chars;
        public image_t[] gltextures = new image_t[MAX_GLTEXTURES];
        public int numgltextures;
        public int base_textureid;
        public byte[] intensitytable = new byte[256];
        public byte[] gammatable = new byte[256];
        public cvar_t intensity;
        public PixelInternalFormat gl_solid_format = PixelInternalFormat.Three;
        public PixelInternalFormat gl_alpha_format = PixelInternalFormat.Four;
        public PixelInternalFormat gl_tex_solid_format = PixelInternalFormat.Three;
        public PixelInternalFormat gl_tex_alpha_format = PixelInternalFormat.Four;
        public int gl_filter_min = ( int ) All.LinearMipmapNearest;
        public int gl_filter_max = ( int ) All.Linear;
        public Image( )
        {
            for (int i = 0; i < gltextures.Length; i++)
            {
                gltextures[i] = new image_t(i);
            }

            numgltextures = 0;
        }

        public override void GL_SetTexturePalette(int[] palette)
        {
            if (qglColorTableEXT && gl_ext_palettedtexture.value != 0F)
            {
                ByteBuffer temptable = Lib.NewByteBuffer(768);
                for (int i = 0; i < 256; i++)
                {
                    temptable.Put(i * 3 + 0, (byte)((palette[i] >> 0) & 0xff));
                    temptable.Put(i * 3 + 1, (byte)((palette[i] >> 8) & 0xff));
                    temptable.Put(i * 3 + 2, (byte)((palette[i] >> 16) & 0xff));
                }

                new Pinnable( temptable, ( ptr ) =>
                {
                    // TODO - will probably error
                    GL.ColorTable( ( ColorTableTarget ) All.SharedTexturePaletteExt, InternalFormat.Rgb, 256, PixelFormat.Rgb, PixelType.UnsignedByte, ptr ); ;
                } );
            }
        }

        public virtual void GL_EnableMultitexture(bool enable)
        {
            if (!qglSelectTextureSGIS && !qglActiveTextureARB)
                return;
            if (enable)
            {
                GL_SelectTexture(TextureUnit.Texture1);
                GL.Enable(EnableCap.Texture2D);
                GL_TexEnv((int)All.Replace);
            }
            else
            {
                GL_SelectTexture(TextureUnit.Texture1);
                GL.Disable(EnableCap.Texture2D);
                GL_TexEnv((int)All.Replace);
            }

            GL_SelectTexture(TextureUnit.Texture0);
            GL_TexEnv((int)All.Replace);
        }

        public virtual void GL_SelectTexture(TextureUnit texture)
        {
            int tmu;
            if (!qglSelectTextureSGIS && !qglActiveTextureARB)
                return;
            if (texture == TextureUnit.Texture0)
            {
                tmu = 0;
            }
            else
            {
                tmu = 1;
            }

            if (tmu == gl_state.currenttmu)
            {
                return;
            }

            gl_state.currenttmu = tmu;
            if (qglSelectTextureSGIS)
            {
                GL.ActiveTexture(texture);
            }
            else if (qglActiveTextureARB)
            {
                GL.ActiveTexture(texture);
                GL.ClientActiveTexture(texture);
            }
        }

        int[] lastmodes = new[]{-1, -1};
        public override void GL_TexEnv(int mode)
        {
            if (mode != lastmodes[gl_state.currenttmu])
            {
                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, mode);
                lastmodes[gl_state.currenttmu] = mode;
            }
        }

        public override void GL_Bind(int texnum)
        {
            if ((gl_nobind.value != 0) && (draw_chars != null))
            {
                texnum = draw_chars.texnum;
            }

            if (gl_state.currenttextures[gl_state.currenttmu] == texnum)
                return;
            gl_state.currenttextures[gl_state.currenttmu] = texnum;
            GL.BindTexture(TextureTarget.Texture2D, texnum);
        }

        public virtual void GL_MBind(TextureUnit target, int texnum)
        {
            GL_SelectTexture(target);
            if (target == TextureUnit.Texture0)
            {
                if (gl_state.currenttextures[0] == texnum)
                    return;
            }
            else
            {
                if (gl_state.currenttextures[1] == texnum)
                    return;
            }

            GL_Bind(texnum);
        }

        public class glmode_t
        {
            public string name;
            public int minimize, maximize;
            public glmode_t(string name, int minimize, int maximze)
            {
                this.name = name;
                this.minimize = minimize;
                this.maximize = maximze;
            }
        }

        static readonly glmode_t[] modes = new glmode_t[]{new glmode_t("GL_NEAREST", (int)All.Nearest, (int)All.Nearest), new glmode_t("GL_LINEAR", (int)All.Linear, (int)All.Linear), new glmode_t("GL_NEAREST_MIPMAP_NEAREST", (int)All.NearestMipmapNearest, (int)All.Nearest), new glmode_t("GL_LINEAR_MIPMAP_NEAREST", (int)All.LinearMipmapNearest, (int)All.Linear), new glmode_t("GL_NEAREST_MIPMAP_LINEAR", (int)All.NearestMipmapLinear, (int)All.Nearest), new glmode_t("GL_LINEAR_MIPMAP_LINEAR", (int)All.LinearMipmapLinear, (int)All.Linear)};
        static readonly int NUM_GL_MODES = modes.Length;
        public class gltmode_t
        {
            public string name;
            public int mode;
            public gltmode_t( string name, int mode)
            {
                this.name = name;
                this.mode = mode;
            }
        }

        static readonly gltmode_t[] gl_alpha_modes = new[]{new gltmode_t("default", 4), new gltmode_t("GL_RGBA", (int)All.Rgba), new gltmode_t("GL_RGBA8", (int)All.Rgba8), new gltmode_t("GL_RGB5_A1", (int)All.Rgb5A1), new gltmode_t("GL_RGBA4", (int)All.Rgba4), new gltmode_t("GL_RGBA2", (int)All.Rgba2)};
        static readonly int NUM_GL_ALPHA_MODES = gl_alpha_modes.Length;
        static readonly gltmode_t[] gl_solid_modes = new[]{new gltmode_t("default", 3), new gltmode_t("GL_RGB", (int)All.Rgb), new gltmode_t("GL_RGB8", (int)All.Rgb8), new gltmode_t("GL_RGB5", (int)All.Rgb5), new gltmode_t("GL_RGB4", (int)All.Rgb4), new gltmode_t("GL_R3_G3_B2", (int)All.R3G3B2)};
        static readonly int NUM_GL_SOLID_MODES = gl_solid_modes.Length;
        public override void GL_TextureMode(string string_renamed)
        {
            int i;
            for (i = 0; i < NUM_GL_MODES; i++)
            {
                if (modes[i].name.EqualsIgnoreCase(string_renamed))
                    break;
            }

            if (i == NUM_GL_MODES)
            {
                VID.Printf(Defines.PRINT_ALL, "bad filter name: [" + string_renamed + "]\\n");
                return;
            }

            gl_filter_min = modes[i].minimize;
            gl_filter_max = modes[i].maximize;
            image_t glt;
            for (i = 0; i < numgltextures; i++)
            {
                glt = gltextures[i];
                if (glt.type != it_pic && glt.type != it_sky)
                {
                    GL_Bind(glt.texnum);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_min);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max);
                }
            }
        }

        public override void GL_TextureAlphaMode(string string_renamed)
        {
            int i;
            for (i = 0; i < NUM_GL_ALPHA_MODES; i++)
            {
                if (gl_alpha_modes[i].name.EqualsIgnoreCase(string_renamed))
                    break;
            }

            if (i == NUM_GL_ALPHA_MODES)
            {
                VID.Printf(Defines.PRINT_ALL, "bad alpha texture mode name: [" + string_renamed + "]\\n");
                return;
            }

            gl_tex_alpha_format = (PixelInternalFormat)gl_alpha_modes[i].mode;
        }

        public override void GL_TextureSolidMode(string string_renamed)
        {
            int i;
            for (i = 0; i < NUM_GL_SOLID_MODES; i++)
            {
                if (gl_solid_modes[i].name.EqualsIgnoreCase(string_renamed))
                    break;
            }

            if (i == NUM_GL_SOLID_MODES)
            {
                VID.Printf(Defines.PRINT_ALL, "bad solid texture mode name: [" + string_renamed + "]\\n");
                return;
            }

            gl_tex_solid_format = ( PixelInternalFormat ) gl_solid_modes[i].mode;
        }

        public override void GL_ImageList_f()
        {
            image_t image;
            int texels;
            String[] palstrings = new[]{"RGB", "PAL"};
            VID.Printf(Defines.PRINT_ALL, "------------------\\n");
            texels = 0;
            for (int i = 0; i < numgltextures; i++)
            {
                image = gltextures[i];
                if (image.texnum <= 0)
                    continue;
                texels += image.upload_width * image.upload_height;
                switch (image.type)

                {
                    case it_skin:
                        VID.Printf(Defines.PRINT_ALL, "M");
                        break;
                    case it_sprite:
                        VID.Printf(Defines.PRINT_ALL, "S");
                        break;
                    case it_wall:
                        VID.Printf(Defines.PRINT_ALL, "W");
                        break;
                    case it_pic:
                        VID.Printf(Defines.PRINT_ALL, "P");
                        break;
                    default:
                        VID.Printf(Defines.PRINT_ALL, " ");
                        break;
                }

                VID.Printf(Defines.PRINT_ALL, " %3i %3i %s: %s\\n", image.upload_width, image.upload_height, palstrings[(image.paletted) ? 1 : 0], image.name);
            }

            VID.Printf(Defines.PRINT_ALL, "Total texel count (not counting mipmaps): " + texels + '\\');
        }

        static readonly int MAX_SCRAPS = 1;
        static readonly int BLOCK_WIDTH = 256;
        static readonly int BLOCK_HEIGHT = 256;
        public int[][] scrap_allocated = Lib.CreateJaggedArray<int[][]>(MAX_SCRAPS, BLOCK_WIDTH);
        public byte[][] scrap_texels = Lib.CreateJaggedArray<byte[][]>( MAX_SCRAPS, BLOCK_WIDTH * BLOCK_HEIGHT );
        public bool scrap_dirty;
        public class pos_t
        {
            public int x, y;
            public pos_t( int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public virtual int Scrap_AllocBlock(int w, int h, pos_t pos)
        {
            int i, j;
            int best, best2;
            int texnum;
            for (texnum = 0; texnum < MAX_SCRAPS; texnum++)
            {
                best = BLOCK_HEIGHT;
                for (i = 0; i < BLOCK_WIDTH - w; i++)
                {
                    best2 = 0;
                    for (j = 0; j < w; j++)
                    {
                        if (scrap_allocated[texnum][i + j] >= best)
                            break;
                        if (scrap_allocated[texnum][i + j] > best2)
                            best2 = scrap_allocated[texnum][i + j];
                    }

                    if (j == w)
                    {
                        pos.x = i;
                        pos.y = best = best2;
                    }
                }

                if (best + h > BLOCK_HEIGHT)
                    continue;
                for (i = 0; i < w; i++)
                    scrap_allocated[texnum][pos.x + i] = best + h;
                return texnum;
            }

            return -1;
        }

        int scrap_uploads = 0;
        public virtual void Scrap_Upload()
        {
            scrap_uploads++;
            GL_Bind(TEXNUM_SCRAPS);
            GL_Upload8(scrap_texels[0], BLOCK_WIDTH, BLOCK_HEIGHT, false, false);
            scrap_dirty = false;
        }

        public virtual byte[] LoadPCX(string filename, byte[][] palette, out Size dim)
        {
            dim = new Size();
            qfiles.pcx_t pcx;
            byte[] raw = FS.LoadFile(filename);
            if (raw == null)
            {
                VID.Printf(Defines.PRINT_DEVELOPER, "Bad pcx file " + filename + '\\');
                return null;
            }

            pcx = new pcx_t(raw);
            if (pcx.manufacturer != 0x0a || pcx.version != 5 || pcx.encoding != 1 || pcx.bits_per_pixel != 8 || pcx.xmax >= 640 || pcx.ymax >= 480)
            {
                VID.Printf(Defines.PRINT_ALL, "Bad pcx file " + filename + '\\');
                return null;
            }

            int width = pcx.xmax - pcx.xmin + 1;
            int height = pcx.ymax - pcx.ymin + 1;
            byte[] pix = new byte[width * height];
            if (palette != null)
            {
                palette[0] = new byte[768];
                System.Array.Copy(raw, raw.Length - 768, palette[0], 0, 768);
            }

            dim.Width = width;
            dim.Height = height;

            int count = 0;
            byte dataByte = 0;
            int runLength = 0;
            int x, y;
            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width;)
                {
                    dataByte = pcx.data.Get();
                    if ((dataByte & 0xC0) == 0xC0)
                    {
                        runLength = dataByte & 0x3F;
                        dataByte = pcx.data.Get();
                        while (runLength-- > 0)
                        {
                            pix[count++] = dataByte;
                            x++;
                        }
                    }
                    else
                    {
                        pix[count++] = dataByte;
                        x++;
                    }
                }
            }

            return pix;
        }

        public virtual byte[] LoadTGA(string name, out Size dim )
        {
            int columns, rows, numPixels;
            int pixbuf;
            int row, column;
            byte[] raw;
            ByteBuffer buf_p;
            dim = new Size();
            qfiles.tga_t targa_header;
            byte[] pic = null;
            raw = FS.LoadFile(name);
            if (raw == null)
            {
                VID.Printf(Defines.PRINT_DEVELOPER, "Bad tga file " + name + '\\');
                return null;
            }

            targa_header = new tga_t(raw);
            if (targa_header.image_type != 2 && targa_header.image_type != 10)
                Com.Error(Defines.ERR_DROP, "LoadTGA: Only type 2 and 10 targa RGB images supported\\n");
            if (targa_header.colormap_type != 0 || (targa_header.pixel_size != 32 && targa_header.pixel_size != 24))
                Com.Error(Defines.ERR_DROP, "LoadTGA: Only 32 or 24 bit images supported (no colormaps)\\n");
            columns = targa_header.width;
            rows = targa_header.height;
            numPixels = columns * rows;

            dim.Width = columns;
            dim.Height = rows;

            pic = new byte[numPixels * 4];
            if (targa_header.id_length != 0)
                targa_header.data.Position = targa_header.id_length;
            buf_p = targa_header.data;
            byte red, green, blue, alphabyte;
            red = green = blue = alphabyte = 0;
            int packetHeader, packetSize, j;
            if (targa_header.image_type == 2)
            {
                for (row = rows - 1; row >= 0; row--)
                {
                    pixbuf = row * columns * 4;
                    for (column = 0; column < columns; column++)
                    {
                        switch (targa_header.pixel_size)

                        {
                            case 24:
                                blue = buf_p.Get();
                                green = buf_p.Get();
                                red = buf_p.Get();
                                pic[pixbuf++] = red;
                                pic[pixbuf++] = green;
                                pic[pixbuf++] = blue;
                                pic[pixbuf++] = (byte)255;
                                break;
                            case 32:
                                blue = buf_p.Get();
                                green = buf_p.Get();
                                red = buf_p.Get();
                                alphabyte = buf_p.Get();
                                pic[pixbuf++] = red;
                                pic[pixbuf++] = green;
                                pic[pixbuf++] = blue;
                                pic[pixbuf++] = alphabyte;
                                break;
                        }
                    }
                }
            }
            else if (targa_header.image_type == 10)
            {
                for (row = rows - 1; row >= 0; row--)
                {
                    pixbuf = row * columns * 4;
                    try
                    {
                        for (column = 0; column < columns;)
                        {
                            packetHeader = buf_p.Get() & 0xFF;
                            packetSize = 1 + (packetHeader & 0x7f);
                            if ((packetHeader & 0x80) != 0)
                            {
                                switch (targa_header.pixel_size)

                                {
                                    case 24:
                                        blue = buf_p.Get();
                                        green = buf_p.Get();
                                        red = buf_p.Get();
                                        alphabyte = (byte)255;
                                        break;
                                    case 32:
                                        blue = buf_p.Get();
                                        green = buf_p.Get();
                                        red = buf_p.Get();
                                        alphabyte = buf_p.Get();
                                        break;
                                }

                                for (j = 0; j < packetSize; j++)
                                {
                                    pic[pixbuf++] = red;
                                    pic[pixbuf++] = green;
                                    pic[pixbuf++] = blue;
                                    pic[pixbuf++] = alphabyte;
                                    column++;
                                    if (column == columns)
                                    {
                                        column = 0;
                                        if (row > 0)
                                            row--;
                                        else
                                            throw new longjmpException();
                                        pixbuf = row * columns * 4;
                                    }
                                }
                            }
                            else
                            {
                                for (j = 0; j < packetSize; j++)
                                {
                                    switch (targa_header.pixel_size)

                                    {
                                        case 24:
                                            blue = buf_p.Get();
                                            green = buf_p.Get();
                                            red = buf_p.Get();
                                            pic[pixbuf++] = red;
                                            pic[pixbuf++] = green;
                                            pic[pixbuf++] = blue;
                                            pic[pixbuf++] = (byte)255;
                                            break;
                                        case 32:
                                            blue = buf_p.Get();
                                            green = buf_p.Get();
                                            red = buf_p.Get();
                                            alphabyte = buf_p.Get();
                                            pic[pixbuf++] = red;
                                            pic[pixbuf++] = green;
                                            pic[pixbuf++] = blue;
                                            pic[pixbuf++] = alphabyte;
                                            break;
                                    }

                                    column++;
                                    if (column == columns)
                                    {
                                        column = 0;
                                        if (row > 0)
                                            row--;
                                        else
                                            throw new longjmpException();
                                        pixbuf = row * columns * 4;
                                    }
                                }
                            }
                        }
                    }
                    catch (longjmpException e)
                    {
                    }
                }
            }

            return pic;
        }

        public class floodfill_t
        {
            public short x, y;
        }

        static readonly int FLOODFILL_FIFO_SIZE = 0x1000;
        static readonly int FLOODFILL_FIFO_MASK = FLOODFILL_FIFO_SIZE - 1;
        static floodfill_t[] fifo = new floodfill_t[FLOODFILL_FIFO_SIZE];
        static Image()
        {
            for (int j = 0; j < fifo.Length; j++)
            {
                fifo[j] = new floodfill_t();
            }
        }

        public virtual void R_FloodFillSkin(byte[] skin, int skinwidth, int skinheight)
        {
            int fillcolor = skin[0] & 0xff;
            int inpt = 0, outpt = 0;
            int filledcolor = -1;
            int i;
            if (filledcolor == -1)
            {
                filledcolor = 0;
                for (i = 0; i < 256; ++i)
                    if (d_8to24table[i] == 0xFF000000)
                    {
                        filledcolor = i;
                        break;
                    }
            }

            if ((fillcolor == filledcolor) || (fillcolor == 255))
            {
                return;
            }

            fifo[inpt].x = 0;
            fifo[inpt].y = 0;
            inpt = (inpt + 1) & FLOODFILL_FIFO_MASK;
            while (outpt != inpt)
            {
                int x = fifo[outpt].x;
                int y = fifo[outpt].y;
                int fdc = filledcolor;
                int pos = x + skinwidth * y;
                outpt = (outpt + 1) & FLOODFILL_FIFO_MASK;
                int off, dx, dy;
                if (x > 0)
                {
                    off = -1;
                    dx = -1;
                    dy = 0;
                    if (skin[pos + off] == (byte)fillcolor)
                    {
                        skin[pos + off] = (byte)255;
                        fifo[inpt].x = (short)(x + dx);
                        fifo[inpt].y = (short)(y + dy);
                        inpt = (inpt + 1) & FLOODFILL_FIFO_MASK;
                    }
                    else if (skin[pos + off] != (byte)255)
                        fdc = skin[pos + off] & 0xff;
                }

                if (x < skinwidth - 1)
                {
                    off = 1;
                    dx = 1;
                    dy = 0;
                    if (skin[pos + off] == (byte)fillcolor)
                    {
                        skin[pos + off] = (byte)255;
                        fifo[inpt].x = (short)(x + dx);
                        fifo[inpt].y = (short)(y + dy);
                        inpt = (inpt + 1) & FLOODFILL_FIFO_MASK;
                    }
                    else if (skin[pos + off] != (byte)255)
                        fdc = skin[pos + off] & 0xff;
                }

                if (y > 0)
                {
                    off = -skinwidth;
                    dx = 0;
                    dy = -1;
                    if (skin[pos + off] == (byte)fillcolor)
                    {
                        skin[pos + off] = (byte)255;
                        fifo[inpt].x = (short)(x + dx);
                        fifo[inpt].y = (short)(y + dy);
                        inpt = (inpt + 1) & FLOODFILL_FIFO_MASK;
                    }
                    else if (skin[pos + off] != (byte)255)
                        fdc = skin[pos + off] & 0xff;
                }

                if (y < skinheight - 1)
                {
                    off = skinwidth;
                    dx = 0;
                    dy = 1;
                    if (skin[pos + off] == (byte)fillcolor)
                    {
                        skin[pos + off] = (byte)255;
                        fifo[inpt].x = (short)(x + dx);
                        fifo[inpt].y = (short)(y + dy);
                        inpt = (inpt + 1) & FLOODFILL_FIFO_MASK;
                    }
                    else if (skin[pos + off] != (byte)255)
                        fdc = skin[pos + off] & 0xff;
                }

                skin[x + skinwidth * y] = (byte)fdc;
            }
        }

        public virtual void GL_ResampleTexture(int[] in_renamed, int inwidth, int inheight, int[] out_renamed, int outwidth, int outheight)
        {
            BufferedImage image = new BufferedImage(inwidth, inheight, BufferedImage.TYPE_INT_ARGB);
            image.SetRGB(0, 0, inwidth, inheight, in_renamed, 0, inwidth);
            AffineTransformOp op = new AffineTransformOp(AffineTransform.GetScaleInstance(outwidth * 1 / inwidth, outheight * 1 / inheight), AffineTransformOp.TYPE_NEAREST_NEIGHBOR);
            BufferedImage tmp = op.Filter(image, null);
            tmp.GetRGB(0, 0, outwidth, outheight, out_renamed, 0, outwidth);
        }

        public virtual void GL_LightScaleTexture(int[] in_renamed, int inwidth, int inheight, bool only_gamma)
        {
            if (only_gamma)
            {
                int i, c;
                int r, g, b, color;
                c = inwidth * inheight;
                for (i = 0; i < c; i++)
                {
                    color = in_renamed[i];
                    r = (color >> 0) & 0xFF;
                    g = (color >> 8) & 0xFF;
                    b = (color >> 16) & 0xFF;
                    r = gammatable[r] & 0xFF;
                    g = gammatable[g] & 0xFF;
                    b = gammatable[b] & 0xFF;
                    in_renamed[i] = ( Int32 ) ( ( r << 0) | (g << 8) | (b << 16) | (color & 0xFF000000) );
                }
            }
            else
            {
                int i, c;
                int r, g, b, color;
                c = inwidth * inheight;
                for (i = 0; i < c; i++)
                {
                    color = in_renamed[i];
                    r = (color >> 0) & 0xFF;
                    g = (color >> 8) & 0xFF;
                    b = (color >> 16) & 0xFF;
                    r = gammatable[intensitytable[r] & 0xFF] & 0xFF;
                    g = gammatable[intensitytable[g] & 0xFF] & 0xFF;
                    b = gammatable[intensitytable[b] & 0xFF] & 0xFF;
                    in_renamed[i] = ( Int32 ) ( ( r << 0) | (g << 8) | (b << 16) | (color & 0xFF000000) );
                }
            }
        }

        public virtual void GL_MipMap(int[] in_renamed, int width, int height)
        {
            int i, j;
            int[] out_renamed;
            out_renamed = in_renamed;
            int inIndex = 0;
            int outIndex = 0;
            int r, g, b, a;
            int p1, p2, p3, p4;
            for (i = 0; i < height; i += 2, inIndex += width)
            {
                for (j = 0; j < width; j += 2, outIndex += 1, inIndex += 2)
                {
                    p1 = in_renamed[inIndex + 0];
                    p2 = in_renamed[inIndex + 1];
                    p3 = in_renamed[inIndex + width + 0];
                    p4 = in_renamed[inIndex + width + 1];
                    r = (((p1 >> 0) & 0xFF) + ((p2 >> 0) & 0xFF) + ((p3 >> 0) & 0xFF) + ((p4 >> 0) & 0xFF)) >> 2;
                    g = (((p1 >> 8) & 0xFF) + ((p2 >> 8) & 0xFF) + ((p3 >> 8) & 0xFF) + ((p4 >> 8) & 0xFF)) >> 2;
                    b = (((p1 >> 16) & 0xFF) + ((p2 >> 16) & 0xFF) + ((p3 >> 16) & 0xFF) + ((p4 >> 16) & 0xFF)) >> 2;
                    a = (((p1 >> 24) & 0xFF) + ((p2 >> 24) & 0xFF) + ((p3 >> 24) & 0xFF) + ((p4 >> 24) & 0xFF)) >> 2;
                    out_renamed[outIndex] = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                }
            }
        }

        public virtual void GL_BuildPalettedTexture(ByteBuffer paletted_texture, int[] scaled, int scaled_width, int scaled_height)
        {
            int r, g, b, c;
            int size = scaled_width * scaled_height;
            for (int i = 0; i < size; i++)
            {
                r = (scaled[i] >> 3) & 31;
                g = (scaled[i] >> 10) & 63;
                b = (scaled[i] >> 19) & 31;
                c = r | (g << 5) | (b << 11);
                paletted_texture.Put(i, gl_state.d_16to8table[c]);
            }
        }

        int upload_width, upload_height;
        bool uploaded_paletted;
        int[] scaled = new int[256 * 256];
        ByteBuffer paletted_texture = Lib.NewByteBuffer(256 * 256);
        Int32Buffer tex = Lib.NewInt32Buffer(512 * 256, ByteOrder.LittleEndian);
        public virtual bool GL_Upload32(int[] data, int width, int height, bool mipmap)
        {
            int samples;
            int scaled_width = 0, scaled_height = 0;
            int i, c;
            int comp;
            scaled.Fill(0);
            paletted_texture.Clear();
            for (int j = 0; j < 256 * 256; j++)
                paletted_texture.Put(j, (byte)0);
            uploaded_paletted = false;
            if (gl_round_down.value > 0F && scaled_width > width && mipmap)
                scaled_width >>= 1;
            if (gl_round_down.value > 0F && scaled_height > height && mipmap)
                scaled_height >>= 1;
            if (mipmap)
            {
                scaled_width >>= (int)gl_picmip.value;
                scaled_height >>= (int)gl_picmip.value;
            }

            if (scaled_width > 256)
                scaled_width = 256;
            if (scaled_height > 256)
                scaled_height = 256;
            if (scaled_width < 1)
                scaled_width = 1;
            if (scaled_height < 1)
                scaled_height = 1;
            upload_width = scaled_width;
            upload_height = scaled_height;
            if (scaled_width * scaled_height > 256 * 256)
                Com.Error(Defines.ERR_DROP, "GL_Upload32: too big");
            c = width * height;
            samples = ( int ) gl_solid_format;
            for (i = 0; i < c; i++)
            {
                if ((data[i] & 0xff000000) != 0xff000000)
                {
                    samples = (int )gl_alpha_format;
                    break;
                }
            }

            if (samples == ( int ) gl_solid_format )
                comp = ( int ) gl_tex_solid_format;
            else if (samples == ( int ) gl_alpha_format )
                comp = ( int ) gl_tex_alpha_format;
            else
            {
                VID.Printf(Defines.PRINT_ALL, "Unknown number of texture components " + samples + '\\');
                comp = samples;
            }

            try
            {
                if (scaled_width == width && scaled_height == height)
                {
                    if (!mipmap)
                    {                       
                        if ( qglColorTableEXT && gl_ext_palettedtexture.value != 0F && samples == ( int ) gl_solid_format )
                        {
                            uploaded_paletted = true;
                            GL_BuildPalettedTexture( paletted_texture, data, scaled_width, scaled_height );
                            var handle = GCHandle.Alloc( paletted_texture, GCHandleType.Pinned );
                            try
                            {
                                var ptr = handle.AddrOfPinnedObject();
                                var addr = ptr.ToInt64();
                                GL.TexImage2D( TextureTarget.Texture2D, 0, (PixelInternalFormat)GL_COLOR_INDEX8_EXT, scaled_width, scaled_height, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, new IntPtr( addr ) );
                            }
                            finally
                            {
                                handle.Free();
                            }
                        }
                        else
                        {
                            tex.Clear();
                            tex.Put( data, 0, scaled_width * scaled_height ).Flip();
                            var handle = GCHandle.Alloc( tex, GCHandleType.Pinned );
                            try
                            {
                                var ptr = handle.AddrOfPinnedObject();
                                var addr = ptr.ToInt64();
                                GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) comp, scaled_width, scaled_height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, new IntPtr( addr ) );
                            }
                            finally
                            {
                                handle.Free();
                            }

                            throw new longjmpException();
                        }
                    }

                    System.Array.Copy(data, 0, scaled, 0, width * height);
                }
                else
                    GL_ResampleTexture(data, width, height, scaled, scaled_width, scaled_height);
                GL_LightScaleTexture(scaled, scaled_width, scaled_height, !mipmap);
                if (qglColorTableEXT && gl_ext_palettedtexture.value != 0F && (samples == (int)gl_solid_format))
                {
                    uploaded_paletted = true;
                    GL_BuildPalettedTexture(paletted_texture, scaled, scaled_width, scaled_height);
                    var handle = GCHandle.Alloc( paletted_texture, GCHandleType.Pinned );
                    try
                    {
                        var ptr = handle.AddrOfPinnedObject();
                        var addr = ptr.ToInt64();
                        GL.TexImage2D(TextureTarget.Texture2D, 0, ( PixelInternalFormat ) GL_COLOR_INDEX8_EXT, scaled_width, scaled_height, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, new IntPtr( addr ));
                    }
                    finally
                    {
                        handle.Free();
                    }
                }
                else
                {
                    tex.Clear();
                    tex.Put(scaled, 0, scaled_width * scaled_height).Flip();
                    var handle = GCHandle.Alloc( tex, GCHandleType.Pinned );
                    try
                    {
                        var ptr = handle.AddrOfPinnedObject();
                        var addr = ptr.ToInt64();
                        GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)comp, scaled_width, scaled_height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, new IntPtr( addr ));
                    }
                    finally
                    {
                        handle.Free();
                    }
                }

                if (mipmap)
                {
                    int miplevel;
                    miplevel = 0;
                    while (scaled_width > 1 || scaled_height > 1)
                    {
                        GL_MipMap(scaled, scaled_width, scaled_height);
                        scaled_width >>= 1;
                        scaled_height >>= 1;
                        if (scaled_width < 1)
                            scaled_width = 1;
                        if (scaled_height < 1)
                            scaled_height = 1;
                        miplevel++;
                        if (qglColorTableEXT && gl_ext_palettedtexture.value != 0F && samples == (int)gl_solid_format)
                        {
                            uploaded_paletted = true;
                            GL_BuildPalettedTexture(paletted_texture, scaled, scaled_width, scaled_height);
                            var handle = GCHandle.Alloc( paletted_texture, GCHandleType.Pinned );
                            try
                            {
                                var ptr = handle.AddrOfPinnedObject();
                                var addr = ptr.ToInt64();
                                GL.TexImage2D(TextureTarget.Texture2D, miplevel, ( PixelInternalFormat ) GL_COLOR_INDEX8_EXT, scaled_width, scaled_height, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, new IntPtr( addr ) );
                            }
                            finally
                            {
                                handle.Free();
                            }
                        }
                        else
                        {
                            tex.Clear();
                            tex.Put(scaled, 0, scaled_width * scaled_height).Flip();
                            var handle = GCHandle.Alloc( tex, GCHandleType.Pinned );
                            try
                            {
                                var ptr = handle.AddrOfPinnedObject();
                                var addr = ptr.ToInt64();
                                GL.TexImage2D(TextureTarget.Texture2D, miplevel, (PixelInternalFormat)comp, scaled_width, scaled_height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, new IntPtr( addr ));
                            }
                            finally
                            {
                                handle.Free();
                            }
                        }
                    }
                }
            }
            catch (longjmpException e)
            {
            }

            if (mipmap)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_min);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_max);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max);
            }

            return (samples == (int)gl_alpha_format);
        }

        int[] trans = new int[512 * 256];
        public virtual bool GL_Upload8(byte[] data, int width, int height, bool mipmap, bool is_sky)
        {
            trans.Fill( 0 );
            int s = width * height;
            if (s > trans.Length)
                Com.Error(Defines.ERR_DROP, "GL_Upload8: too large");
            if (qglColorTableEXT && gl_ext_palettedtexture.value != 0F && is_sky)
            {
                var handle = GCHandle.Alloc( data, GCHandleType.Pinned );
                try
                {
                    var ptr = handle.AddrOfPinnedObject();
                    var addr = ptr.ToInt64();
                    GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) GL_COLOR_INDEX8_EXT, width, height, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, new IntPtr( addr ) );
                    GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_max );
                    GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max );
                }
                finally
                {
                    handle.Free();
                }
                return false;
            }
            else
            {
                int p;
                for (int i = 0; i < s; i++)
                {
                    p = data[i] & 0xff;
                    trans[i] = d_8to24table[p];
                    if (p == 255)
                    {
                        if (i > width && (data[i - width] & 0xff) != 255)
                            p = data[i - width] & 0xff;
                        else if (i < s - width && (data[i + width] & 0xff) != 255)
                            p = data[i + width] & 0xff;
                        else if (i > 0 && (data[i - 1] & 0xff) != 255)
                            p = data[i - 1] & 0xff;
                        else if (i < s - 1 && (data[i + 1] & 0xff) != 255)
                            p = data[i + 1] & 0xff;
                        else
                            p = 0;
                        trans[i] = d_8to24table[p] & 0x00FFFFFF;
                    }
                }

                return GL_Upload32(trans, width, height, mipmap);
            }
        }

        public virtual image_t GL_LoadPic(string name, byte[] pic, int width, int height, int type, int bits)
        {
            image_t image;
            int i;
            for (i = 0; i < numgltextures; i++)
            {
                image = gltextures[i];
                if (image.texnum == 0)
                    break;
            }

            if (i == numgltextures)
            {
                if (numgltextures == MAX_GLTEXTURES)
                    Com.Error(Defines.ERR_DROP, "MAX_GLTEXTURES");
                numgltextures++;
            }

            image = gltextures[i];
            if (name.Length > Defines.MAX_QPATH)
                Com.Error(Defines.ERR_DROP, "Draw_LoadPic: \\\"" + name + "\\\" is too long");
            image.name = name;
            image.registration_sequence = registration_sequence;
            image.width = width;
            image.height = height;
            image.type = type;
            if (type == it_skin && bits == 8)
                R_FloodFillSkin(pic, width, height);
            if (image.type == it_pic && bits == 8 && image.width < 64 && image.height < 64)
            {
                pos_t pos = new pos_t(0, 0);
                int j, k;
                int texnum = Scrap_AllocBlock(image.width, image.height, pos);
                if (texnum == -1)
                {
                    image.scrap = false;
                    image.texnum = TEXNUM_IMAGES + image.GetId();
                    GL_Bind(image.texnum);
                    if (bits == 8)
                    {
                        image.has_alpha = GL_Upload8(pic, width, height, (image.type != it_pic && image.type != it_sky), image.type == it_sky);
                    }
                    else
                    {
                        int[] tmp = new int[pic.Length / 4];
                        for (i = 0; i < tmp.Length; i++)
                        {
                            tmp[i] = ((pic[4 * i + 0] & 0xFF) << 0);
                            tmp[i] |= ((pic[4 * i + 1] & 0xFF) << 8);
                            tmp[i] |= ((pic[4 * i + 2] & 0xFF) << 16);
                            tmp[i] |= ((pic[4 * i + 3] & 0xFF) << 24);
                        }

                        image.has_alpha = GL_Upload32(tmp, width, height, (image.type != it_pic && image.type != it_sky));
                    }

                    image.upload_width = upload_width;
                    image.upload_height = upload_height;
                    image.paletted = uploaded_paletted;
                    image.sl = 0;
                    image.sh = 1;
                    image.tl = 0;
                    image.th = 1;
                    return image;
                }

                scrap_dirty = true;
                k = 0;
                for (i = 0; i < image.height; i++)
                    for (j = 0; j < image.width; j++, k++)
                        scrap_texels[texnum][(pos.y + i) * BLOCK_WIDTH + pos.x + j] = pic[k];
                image.texnum = TEXNUM_SCRAPS + texnum;
                image.scrap = true;
                image.has_alpha = true;
                image.sl = (pos.x + 0.01F) / (float)BLOCK_WIDTH;
                image.sh = (pos.x + image.width - 0.01F) / (float)BLOCK_WIDTH;
                image.tl = (pos.y + 0.01F) / (float)BLOCK_WIDTH;
                image.th = (pos.y + image.height - 0.01F) / (float)BLOCK_WIDTH;
            }
            else
            {
                image.scrap = false;
                image.texnum = TEXNUM_IMAGES + image.GetId();
                GL_Bind(image.texnum);
                if (bits == 8)
                {
                    image.has_alpha = GL_Upload8(pic, width, height, (image.type != it_pic && image.type != it_sky), image.type == it_sky);
                }
                else
                {
                    int[] tmp = new int[pic.Length / 4];
                    for (i = 0; i < tmp.Length; i++)
                    {
                        tmp[i] = ((pic[4 * i + 0] & 0xFF) << 0);
                        tmp[i] |= ((pic[4 * i + 1] & 0xFF) << 8);
                        tmp[i] |= ((pic[4 * i + 2] & 0xFF) << 16);
                        tmp[i] |= ((pic[4 * i + 3] & 0xFF) << 24);
                    }

                    image.has_alpha = GL_Upload32(tmp, width, height, (image.type != it_pic && image.type != it_sky));
                }

                image.upload_width = upload_width;
                image.upload_height = upload_height;
                image.paletted = uploaded_paletted;
                image.sl = 0;
                image.sh = 1;
                image.tl = 0;
                image.th = 1;
            }

            return image;
        }

        public virtual image_t GL_LoadWal(string name)
        {
            image_t image = null;
            byte[] raw = FS.LoadFile(name);
            if (raw == null)
            {
                VID.Printf(Defines.PRINT_ALL, "GL_FindImage: can't load " + name + '\\');
                return r_notexture;
            }

            qfiles.miptex_t mt = new qfiles.miptex_t(raw);
            byte[] pix = new byte[mt.width * mt.height];
            System.Array.Copy(raw, mt.offsets[0], pix, 0, pix.Length);
            image = GL_LoadPic(name, pix, mt.width, mt.height, it_wall, 8);
            return image;
        }

        public virtual image_t GL_FindImage(string name, int type)
        {
            image_t image = null;
            name = name.ToLower();
            int index = name.IndexOf('\\');
            if (index != -1)
                name = name.Substring(0, index);
            if (name == null || name.Length < 5)
                return null;
            for (int i = 0; i < numgltextures; i++)
            {
                image = gltextures[i];
                if (name.Equals(image.name))
                {
                    image.registration_sequence = registration_sequence;
                    return image;
                }
            }

            byte[] pic = null;
            Size dim = new Size();
            if (name.EndsWith(".pcx"))
            {
                pic = LoadPCX(name, null, out dim);
                if (pic == null)
                    return null;
                image = GL_LoadPic(name, pic, dim.Width, dim.Height, type, 8);
            }
            else if (name.EndsWith(".wal"))
            {
                image = GL_LoadWal(name);
            }
            else if (name.EndsWith(".tga"))
            {
                pic = LoadTGA(name, out dim);
                if (pic == null)
                    return null;
                image = GL_LoadPic(name, pic, dim.Width, dim.Height, type, 32);
            }
            else
                return null;
            return image;
        }

        public override image_t R_RegisterSkin(string name)
        {
            return GL_FindImage(name, it_skin);
        }

        private Int32Buffer texnum = Lib.NewInt32Buffer(1);
        public virtual void GL_FreeUnusedImages()
        {
            r_notexture.registration_sequence = registration_sequence;
            r_particletexture.registration_sequence = registration_sequence;
            image_t image = null;
            for (int i = 0; i < numgltextures; i++)
            {
                image = gltextures[i];
                if (image.registration_sequence == registration_sequence)
                    continue;
                if (image.registration_sequence == 0)
                    continue;
                if (image.type == it_pic)
                    continue;
                texnum.Put(0, image.texnum);
                GL.DeleteTextures(1,texnum.Array);
                image.Clear();
            }
        }

        protected override void Draw_GetPalette()
        {
            int r, g, b;
            byte[][] palette = Lib.CreateJaggedArray<byte[][]>( 1, 1 );
            LoadPCX("pics/colormap.pcx", palette, out var dummy );
            if (palette[0] == null || palette[0].Length != 768)
                Com.Error(Defines.ERR_FATAL, "Couldn't load pics/colormap.pcx");
            byte[] pal = palette[0];
            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                r = pal[j++] & 0xFF;
                g = pal[j++] & 0xFF;
                b = pal[j++] & 0xFF;
                d_8to24table[i] = (255 << 24) | (b << 16) | (g << 8) | (r << 0);
            }

            d_8to24table[255] &= 0x00FFFFFF;
            particle_t.SetColorPalette(d_8to24table);
        }

        public override void GL_InitImages()
        {
            int i, j;
            float g = vid_gamma.value;
            registration_sequence = 1;
            intensity = Cvar.Get("intensity", "2", 0);
            if (intensity.value <= 1)
                Cvar.Set("intensity", "1");
            gl_state.inverse_intensity = 1 / intensity.value;
            Draw_GetPalette();
            if (qglColorTableEXT)
            {
                gl_state.d_16to8table = FS.LoadFile("pics/16to8.dat");
                if (gl_state.d_16to8table == null)
                    Com.Error(Defines.ERR_FATAL, "Couldn't load pics/16to8.pcx");
            }

            if ((gl_config.renderer & (GL_RENDERER_VOODOO | GL_RENDERER_VOODOO2)) != 0)
            {
                g = 1F;
            }

            for (i = 0; i < 256; i++)
            {
                if (g == 1F)
                {
                    gammatable[i] = (byte)i;
                }
                else
                {
                    int inf = (int)(255F * Math.Pow((i + 0.5) / 255.5, g) + 0.5);
                    if (inf < 0)
                        inf = 0;
                    if (inf > 255)
                        inf = 255;
                    gammatable[i] = (byte)inf;
                }
            }

            for (i = 0; i < 256; i++)
            {
                j = (int)(i * intensity.value);
                if (j > 255)
                    j = 255;
                intensitytable[i] = (byte)j;
            }
        }

        public override void GL_ShutdownImages()
        {
            image_t image;
            for (int i = 0; i < numgltextures; i++)
            {
                image = gltextures[i];
                if (image.registration_sequence == 0)
                    continue;
                texnum.Put(0, image.texnum);
                GL.DeleteTextures(1, texnum.Array);
                image.Clear();
            }
        }
    }
}
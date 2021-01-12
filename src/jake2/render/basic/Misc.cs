using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Q2Sharp.Util;
using J2N.Text;
using System.IO;
using J2N.IO;
using System.Drawing;

namespace Q2Sharp.Render.Basic
{
    public sealed class Misc : Mesh
    {
        byte[][] dottexture = new byte[][]{new byte[]{0, 0, 0, 0, 0, 0, 0, 0}, new byte[]{0, 0, 1, 1, 0, 0, 0, 0}, new byte[]{0, 1, 1, 1, 1, 0, 0, 0}, new byte[]{0, 1, 1, 1, 1, 0, 0, 0}, new byte[]{0, 0, 1, 1, 0, 0, 0, 0}, new byte[]{0, 0, 0, 0, 0, 0, 0, 0}, new byte[]{0, 0, 0, 0, 0, 0, 0, 0}, new byte[]{0, 0, 0, 0, 0, 0, 0, 0}};
        public override void R_InitParticleTexture()
        {
            int x, y;
            byte[] data = new byte[8 * 8 * 4];
            for (x = 0; x < 8; x++)
            {
                for (y = 0; y < 8; y++)
                {
                    data[y * 32 + x * 4 + 0] = (byte)255;
                    data[y * 32 + x * 4 + 1] = (byte)255;
                    data[y * 32 + x * 4 + 2] = (byte)255;
                    data[y * 32 + x * 4 + 3] = (byte)(dottexture[x][y] * 255);
                }
            }

            r_particletexture = GL_LoadPic("***particle***", data, 8, 8, it_sprite, 32);
            for (x = 0; x < 8; x++)
            {
                for (y = 0; y < 8; y++)
                {
                    data[y * 32 + x * 4 + 0] = (byte)(dottexture[x & 3][y & 3] * 255);
                    data[y * 32 + x * 4 + 1] = 0;
                    data[y * 32 + x * 4 + 2] = 0;
                    data[y * 32 + x * 4 + 3] = (byte)255;
                }
            }

            r_notexture = GL_LoadPic("***r_notexture***", data, 8, 8, it_wall, 32);
        }

        private static readonly int TGA_HEADER_SIZE = 18;
        public override void GL_ScreenShot_f()
        {
            StringBuffer sb = new StringBuffer(FS.Gamedir() + "/scrshot/jake00.tga");
            FS.CreatePath(sb.ToString());
            FileInfo file = new FileInfo(sb.ToString());
            int i = 0;
            int offset = sb.Length - 6;
            while (file.Exists && i++ < 100)
            {
                sb[offset] = (char)((i / 10) + '0');
                sb[offset + 1] = (char)((i % 10) + '0');
                file = new FileInfo(sb.ToString());
            }

            if (i == 100)
            {
                VID.Printf(Defines.PRINT_ALL, "Clean up your screenshots\\n");
                return;
            }

            try
            {
                FileStream out_renamed = File.OpenWrite( file.FullName );
                FileChannel ch = out_renamed.GetChannel();
                int fileLength = TGA_HEADER_SIZE + vid.GetWidth() * vid.GetHeight() * 3;
                out_renamed.SetLength(fileLength);
                MappedByteBuffer image = ch.Map(FileChannel.MapMode.READ_WRITE, 0, fileLength);
                image.Put(0, (byte)0).Put(1, (byte)0);
                image.Put(2, (byte)2);
                image.Put(12, (byte)(vid.GetWidth() & 0xFF));
                image.Put(13, (byte)(vid.GetWidth() >> 8));
                image.Put(14, (byte)(vid.GetHeight() & 0xFF));
                image.Put(15, (byte)(vid.GetHeight() >> 8));
                image.Put(16, (byte)24);
                image.Position(TGA_HEADER_SIZE);
                ByteBuffer rgb = image.Slice();
                if (vid.GetWidth() % 4 != 0)
                {
                    GL.PixelStore(PixelStoreParameter.PackAlignment, 1);
                }

                if (gl_config.GetOpenGLVersion() >= 1.2F)
                {
                    Byte[] pixels = new Byte[vid.GetWidth() * vid.GetHeight() * 3];
                    GL.ReadPixels(0, 0, vid.GetWidth(), vid.GetHeight(), PixelFormat.Bgr, PixelType.UnsignedByte, pixels);
                    rgb = ByteBuffer.Allocate( pixels.Length );
                    rgb.Put(pixels);
                }
                else
                {
                    Byte[] pixels = new Byte[vid.GetWidth() * vid.GetHeight() * 3];
                    GL.ReadPixels(0, 0, vid.GetWidth(), vid.GetHeight(), PixelFormat.Rgb, PixelType.UnsignedByte, pixels);
                    rgb = ByteBuffer.Allocate( pixels.Length );
                    rgb.Put( pixels );
                    byte tmp;
                    for (i = TGA_HEADER_SIZE; i < fileLength; i += 3)
                    {
                        tmp = image.Get(i);
                        image.Put(i, image.Get(i + 2));
                        image.Put(i + 2, tmp);
                    }
                }

                GL.PixelStore(PixelStoreParameter.PackAlignment, 4);
                ch.Close();
            }
            catch (IOException e)
            {
                VID.Printf(Defines.PRINT_ALL, e.ToString() + '\\');
            }

            VID.Printf(Defines.PRINT_ALL, "Wrote " + file + '\\');
        }

        public override void GL_Strings_f()
        {
            VID.Printf(Defines.PRINT_ALL, "GL_VENDOR: " + gl_config.vendor_string + '\\');
            VID.Printf(Defines.PRINT_ALL, "GL_RENDERER: " + gl_config.renderer_string + '\\');
            VID.Printf(Defines.PRINT_ALL, "GL_VERSION: " + gl_config.version_string + '\\');
            VID.Printf(Defines.PRINT_ALL, "GL_EXTENSIONS: " + gl_config.extensions_string + '\\');
        }

        public override void GL_SetDefaultState()
        {
            GL.ClearColor(1F, 0F, 0.5F, 0.5F);
            GL.CullFace(CullFaceMode.Front);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.666F);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);
            GL.Color4(1, 1, 1, 1);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);            
            GL.ShadeModel(ShadingModel.Flat);
            GL_TextureMode(gl_texturemode.string_renamed);
            GL_TextureAlphaMode(gl_texturealphamode.string_renamed);
            GL_TextureSolidMode(gl_texturesolidmode.string_renamed);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_min);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);            
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL_TexEnv((int)All.Replace);
            if (qglPointParameterfEXT)
            {
                SingleBuffer attenuations = Lib.NewSingleBuffer(4);
                attenuations.Put(0, gl_particle_att_a.value);
                attenuations.Put(1, gl_particle_att_b.value);
                attenuations.Put(2, gl_particle_att_c.value);
                GL.Enable(EnableCap.PointSmooth);                      
                GL.PointParameter(PointParameterName.PointSizeMin, gl_particle_min_size.value);
                GL.PointParameter(PointParameterName.PointSizeMax, gl_particle_max_size.value);
                GL.PointParameter(PointParameterName.PointDistanceAttenuation, attenuations.Array);
            }

            if (qglColorTableEXT && gl_ext_palettedtexture.value != 0F)
            {
                GL.Enable( EnableCap.SharedTexturePaletteExt );
                GL_SetTexturePalette(d_8to24table);
            }

            GL_UpdateSwapInterval();
        }

        public override void GL_UpdateSwapInterval()
        {
            if (gl_swapinterval.modified)
            {
                gl_swapinterval.modified = false;
                if (!gl_state.stereo_enabled)
                {
                    gl.SetSwapInterval((int)gl_swapinterval.value);
                }
            }
        }

		public override void Draw_GetPicSize( out Size dim, String name )
		{
			throw new NotImplementedException();
		}
	}
}
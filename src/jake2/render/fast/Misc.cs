using Jake2.Client;
using Jake2.Qcommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Jake2.Util;
using J2N.Text;
using J2N.IO;
using System.IO;
using System.Drawing;

namespace Jake2.Render.Fast
{
    public sealed class Misc : Mesh
    {
        byte[][] dottexture = new byte[][]{ new byte[]{0, 0, 0, 0, 0, 0, 0, 0}, new byte[]{0, 0, 1, 1, 0, 0, 0, 0}, new byte[]{0, 1, 1, 1, 1, 0, 0, 0}, new byte[]{0, 1, 1, 1, 1, 0, 0, 0}, new byte[]{0, 0, 1, 1, 0, 0, 0, 0}, new byte[]{0, 0, 0, 0, 0, 0, 0, 0}, new byte[]{0, 0, 0, 0, 0, 0, 0, 0}, new byte[]{0, 0, 0, 0, 0, 0, 0, 0}};
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

        public override void GL_ScreenShot_f()
        {
            var screnshotIndex = Directory.GetFiles( FS.Gamedir() + "/scrshot/", "jake*.tga" )
                .Select( s => Path.GetFileNameWithoutExtension( s ).Replace( "jake", "" ) )
                .Max( s => int.Parse( s ) ) + 1;

            FileStream file = File.OpenWrite( FS.Gamedir() + $"/scrshot/jake{screnshotIndex:00}.tga" );          

            try
            {
                using ( var bmp = new Bitmap( vid.GetWidth(), vid.GetHeight() ) )
                {
                    var data = bmp.LockBits( new Rectangle( 0, 0, vid.GetWidth(), vid.GetHeight() ), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb );

                    GL.ReadPixels( 0, 0, vid.GetWidth(), vid.GetHeight(), PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0 );

                    bmp.UnlockBits( data );

                    bmp.RotateFlip( RotateFlipType.RotateNoneFlipY );

                    var encoder = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().First( c => c.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid );
                    var encParams = new System.Drawing.Imaging.EncoderParameters() { Param = new[] { new System.Drawing.Imaging.EncoderParameter( System.Drawing.Imaging.Encoder.Quality, 100L ) } };

                    bmp.Save( file, encoder, encParams );
                }
                file.Dispose();
            }
            catch (Exception e)
            {
                VID.Printf(Defines.PRINT_ALL, e.Message + '\\');
            }

            VID.Printf(Defines.PRINT_ALL, "Wrote " + file.Name + '\\');
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
                SingleBuffer att_buffer = Lib.NewSingleBuffer(4);
                att_buffer.Put(0, gl_particle_att_a.value);
                att_buffer.Put(1, gl_particle_att_b.value);
                att_buffer.Put(2, gl_particle_att_c.value);
                GL.Enable(EnableCap.PointSmooth);
                GL.PointParameter(PointParameterName.PointSizeMin, gl_particle_min_size.value);
                GL.PointParameter(PointParameterName.PointSizeMax, gl_particle_max_size.value);
                GL.PointParameter(PointParameterName.PointDistanceAttenuation, att_buffer.Array);
            }

            if (qglColorTableEXT && gl_ext_palettedtexture.value != 0F)
            {
                GL.Enable(EnableCap.SharedTexturePaletteExt);
                GL_SetTexturePalette(d_8to24table);
            }

            GL_UpdateSwapInterval();
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.ClientActiveTexture(TextureUnit.Texture0);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
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
    }
}
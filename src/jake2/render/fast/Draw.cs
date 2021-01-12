using J2N.IO;
using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using OpenTK.Graphics.OpenGL;
using Q2Sharp.util;
using System;
using System.Drawing;

namespace Q2Sharp.Render.Fast
{
	public abstract class Draw : Image
	{
		public override void Draw_InitLocal( )
		{
			draw_chars = GL_FindImage( "pics/conchars.pcx", it_pic );
			GL_Bind( draw_chars.texnum );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( Int32 ) All.Nearest );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( Int32 ) All.Nearest );
		}

		public override void Draw_Char( Int32 x, Int32 y, Int32 num )
		{
			num &= 255;
			if ( ( num & 127 ) == 32 )
				return;
			if ( y <= -8 )
				return;
			var row = num >> 4;
			var col = num & 15;
			var frow = row * 0.0625F;
			var fcol = col * 0.0625F;
			var size = 0.0625F;
			GL_Bind( draw_chars.texnum );
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( fcol, frow );
			GL.Vertex2( x, y );
			GL.TexCoord2( fcol + size, frow );
			GL.Vertex2( x + 8, y );
			GL.TexCoord2( fcol + size, frow + size );
			GL.Vertex2( x + 8, y + 8 );
			GL.TexCoord2( fcol, frow + size );
			GL.Vertex2( x, y + 8 );
			GL.End();
		}

		public override image_t Draw_FindPic( String name )
		{
			if ( !name.StartsWith( "/" ) && !name.StartsWith( "\\\\" ) )
			{
				return GL_FindImage( name, it_pic );
			}
			else
			{
				return GL_FindImage( name.Substring( 1 ), it_pic );
			}
		}

		public override void Draw_GetPicSize( out Size dim, String pic )
		{
			image_t image = Draw_FindPic( pic );
			dim = new Size();
			dim.Width = ( image != null ) ? image.width : -1;
			dim.Height = ( image != null ) ? image.height : -1;
		}

		public override void Draw_StretchPic( Int32 x, Int32 y, Int32 w, Int32 h, String pic )
		{
			image_t image;
			image = Draw_FindPic( pic );
			if ( image == null )
			{
				VID.Printf( Defines.PRINT_ALL, "Can't find pic: " + pic + '\\' );
				return;
			}

			if ( scrap_dirty )
				Scrap_Upload();
			if ( ( ( gl_config.renderer == GL_RENDERER_MCD ) || ( ( gl_config.renderer & GL_RENDERER_RENDITION ) != 0 ) ) && !image.has_alpha )
				GL.Disable( EnableCap.AlphaTest );
			GL_Bind( image.texnum );
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( image.sl, image.tl );
			GL.Vertex2( x, y );
			GL.TexCoord2( image.sh, image.tl );
			GL.Vertex2( x + w, y );
			GL.TexCoord2( image.sh, image.th );
			GL.Vertex2( x + w, y + h );
			GL.TexCoord2( image.sl, image.th );
			GL.Vertex2( x, y + h );
			GL.End();
			if ( ( ( gl_config.renderer == GL_RENDERER_MCD ) || ( ( gl_config.renderer & GL_RENDERER_RENDITION ) != 0 ) ) && !image.has_alpha )
				GL.Enable( EnableCap.AlphaTest );
		}

		public override void Draw_Pic( Int32 x, Int32 y, String pic )
		{
			image_t image;
			image = Draw_FindPic( pic );
			if ( image == null )
			{
				VID.Printf( Defines.PRINT_ALL, "Can't find pic: " + pic + '\\' );
				return;
			}

			if ( scrap_dirty )
				Scrap_Upload();
			if ( ( ( gl_config.renderer == GL_RENDERER_MCD ) || ( ( gl_config.renderer & GL_RENDERER_RENDITION ) != 0 ) ) && !image.has_alpha )
				GL.Disable( EnableCap.AlphaTest );
			GL_Bind( image.texnum );
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( image.sl, image.tl );
			GL.Vertex2( x, y );
			GL.TexCoord2( image.sh, image.tl );
			GL.Vertex2( x + image.width, y );
			GL.TexCoord2( image.sh, image.th );
			GL.Vertex2( x + image.width, y + image.height );
			GL.TexCoord2( image.sl, image.th );
			GL.Vertex2( x, y + image.height );
			GL.End();
			if ( ( ( gl_config.renderer == GL_RENDERER_MCD ) || ( ( gl_config.renderer & GL_RENDERER_RENDITION ) != 0 ) ) && !image.has_alpha )
				GL.Enable( EnableCap.AlphaTest );
		}

		public override void Draw_TileClear( Int32 x, Int32 y, Int32 w, Int32 h, String pic )
		{
			image_t image;
			image = Draw_FindPic( pic );
			if ( image == null )
			{
				VID.Printf( Defines.PRINT_ALL, "Can't find pic: " + pic + '\\' );
				return;
			}

			if ( ( ( gl_config.renderer == GL_RENDERER_MCD ) || ( ( gl_config.renderer & GL_RENDERER_RENDITION ) != 0 ) ) && !image.has_alpha )
				GL.Disable( EnableCap.AlphaTest );
			GL_Bind( image.texnum );
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( x / 64F, y / 64F );
			GL.Vertex2( x, y );
			GL.TexCoord2( ( x + w ) / 64F, y / 64F );
			GL.Vertex2( x + w, y );
			GL.TexCoord2( ( x + w ) / 64F, ( y + h ) / 64F );
			GL.Vertex2( x + w, y + h );
			GL.TexCoord2( x / 64F, ( y + h ) / 64F );
			GL.Vertex2( x, y + h );
			GL.End();
			if ( ( ( gl_config.renderer == GL_RENDERER_MCD ) || ( ( gl_config.renderer & GL_RENDERER_RENDITION ) != 0 ) ) && !image.has_alpha )
				GL.Enable( EnableCap.AlphaTest );
		}

		public override void Draw_Fill( Int32 x, Int32 y, Int32 w, Int32 h, Int32 colorIndex )
		{
			if ( colorIndex > 255 )
				Com.Error( Defines.ERR_FATAL, "Draw_Fill: bad color" );
			GL.Disable( EnableCap.Texture2D );
			var color = d_8to24table[colorIndex];
			GL.Color3( ( Byte ) ( ( color >> 0 ) & 0xff ), ( Byte ) ( ( color >> 8 ) & 0xff ), ( Byte ) ( ( color >> 16 ) & 0xff ) );
			GL.Begin( PrimitiveType.Quads );
			GL.Vertex2( x, y );
			GL.Vertex2( x + w, y );
			GL.Vertex2( x + w, y + h );
			GL.Vertex2( x, y + h );
			GL.End();
			GL.Color3( 1, 1, 1 );
			GL.Enable( EnableCap.Texture2D );
		}

		public override void Draw_FadeScreen( )
		{
			GL.Enable( EnableCap.Blend );
			GL.Disable( EnableCap.Texture2D );
			GL.Color4( 0, 0, 0, 0.8F );
			GL.Begin( PrimitiveType.Quads );
			GL.Vertex2( 0, 0 );
			GL.Vertex2( vid.GetWidth(), 0 );
			GL.Vertex2( vid.GetWidth(), vid.GetHeight() );
			GL.Vertex2( 0, vid.GetHeight() );
			GL.End();
			GL.Color4( 1, 1, 1, 1 );
			GL.Enable( EnableCap.Texture2D );
			GL.Disable( EnableCap.Blend );
		}

		Int32Buffer image32 = Lib.NewInt32Buffer( 256 * 256 );
		ByteBuffer image8 = Lib.NewByteBuffer( 256 * 256 );
		public override void Draw_StretchRaw( Int32 x, Int32 y, Int32 w, Int32 h, Int32 cols, Int32 rows, Byte[] data )
		{
			Int32 i, j, trows;
			Int32 sourceIndex;
			Int32 frac, fracstep;
			Single hscale;
			Int32 row;
			Single t;
			GL_Bind( 0 );
			if ( rows <= 256 )
			{
				hscale = 1;
				trows = rows;
			}
			else
			{
				hscale = rows / 256F;
				trows = 256;
			}

			t = rows * hscale / 256;
			if ( !qglColorTableEXT )
			{
				image32.Clear();
				var destIndex = 0;
				for ( i = 0; i < trows; i++ )
				{
					row = ( Int32 ) ( i * hscale );
					if ( row > rows )
						break;
					sourceIndex = cols * row;
					destIndex = i * 256;
					fracstep = cols * 0x10000 / 256;
					frac = fracstep >> 1;
					for ( j = 0; j < 256; j++ )
					{
						image32.Put( destIndex + j, r_rawpalette[data[sourceIndex + ( frac >> 16 )] & 0xff] );
						frac += fracstep;
					}
				}

				new Pinnable( image32, ( ptr ) =>
				{
					GL.TexImage2D( TextureTarget.Texture2D, 0, gl_tex_solid_format, 256, 256, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr );
				} );
			}
			else
			{
				image8.Clear();
				var destIndex = 0;
				for ( i = 0; i < trows; i++ )
				{
					row = ( Int32 ) ( i * hscale );
					if ( row > rows )
						break;
					sourceIndex = cols * row;
					destIndex = i * 256;
					fracstep = cols * 0x10000 / 256;
					frac = fracstep >> 1;
					for ( j = 0; j < 256; j++ )
					{
						image8.Put( destIndex + j, data[sourceIndex + ( frac >> 16 )] );
						frac += fracstep;
					}
				}

				new Pinnable( image8, ( ptr ) =>
				{
					GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) GL_COLOR_INDEX8_EXT, 256, 256, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, ptr );
				} );
			}

			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( Int32 ) All.Linear );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( Int32 ) All.Linear );
			if ( ( gl_config.renderer == GL_RENDERER_MCD ) || ( ( gl_config.renderer & GL_RENDERER_RENDITION ) != 0 ) )
				GL.Disable( EnableCap.AlphaTest );
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( 0, 0 );
			GL.Vertex2( x, y );
			GL.TexCoord2( 1, 0 );
			GL.Vertex2( x + w, y );
			GL.TexCoord2( 1, t );
			GL.Vertex2( x + w, y + h );
			GL.TexCoord2( 0, t );
			GL.Vertex2( x, y + h );
			GL.End();
			if ( ( gl_config.renderer == GL_RENDERER_MCD ) || ( ( gl_config.renderer & GL_RENDERER_RENDITION ) != 0 ) )
				GL.Enable( EnableCap.AlphaTest );
		}
	}
}
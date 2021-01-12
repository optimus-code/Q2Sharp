using J2N.IO;
using Jake2.Client;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using OpenTK.Graphics.OpenGL;
using Q2Sharp.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using static Jake2.Qcommon.qfiles;

namespace Jake2.Render.Fast
{
	public abstract class Image : Main
	{
		public image_t draw_chars;
		public image_t[] gltextures = new image_t[MAX_GLTEXTURES];
		public Int32 numgltextures;
		public Int32 base_textureid;
		public Byte[] intensitytable = new Byte[256];
		public Byte[] gammatable = new Byte[256];
		public cvar_t intensity;
		public PixelInternalFormat gl_solid_format = PixelInternalFormat.Three;
		public PixelInternalFormat gl_alpha_format = PixelInternalFormat.Four;
		public PixelInternalFormat gl_tex_solid_format = PixelInternalFormat.Three;
		public PixelInternalFormat gl_tex_alpha_format = PixelInternalFormat.Four;
		public Int32 gl_filter_min = ( Int32 ) All.LinearMipmapNearest;
		public Int32 gl_filter_max = ( Int32 ) All.Linear;
		public Image( )
		{
			for ( var i = 0; i < gltextures.Length; i++ )
			{
				gltextures[i] = new image_t( i );
			}

			numgltextures = 0;
		}

		public override void GL_SetTexturePalette( Int32[] palette )
		{
			Int32 i;
			if ( qglColorTableEXT && gl_ext_palettedtexture.value != 0F )
			{
				ByteBuffer temptable = Lib.NewByteBuffer( 768 );
				for ( i = 0; i < 256; i++ )
				{
					temptable.Put( i * 3 + 0, ( Byte ) ( ( palette[i] >> 0 ) & 0xff ) );
					temptable.Put( i * 3 + 1, ( Byte ) ( ( palette[i] >> 8 ) & 0xff ) );
					temptable.Put( i * 3 + 2, ( Byte ) ( ( palette[i] >> 16 ) & 0xff ) );
				}

				// TODO - Does this even work/?
				new Pinnable( temptable, ( ptr =>
				{
					GL.ColorTable( ( ColorTableTarget ) All.SharedTexturePaletteExt, InternalFormat.Rgb, 256, PixelFormat.Rgb, PixelType.UnsignedByte, ptr );
				} ) );
			}
		}

		public virtual void GL_EnableMultitexture( System.Boolean enable )
		{
			if ( enable )
			{
				GL_SelectTexture( TextureUnit.Texture1 );
				GL.Enable( EnableCap.Texture2D );
				GL_TexEnv( ( Int32 ) All.Replace );
			}
			else
			{
				GL_SelectTexture( TextureUnit.Texture1 );
				GL.Disable( EnableCap.Texture2D );
				GL_TexEnv( ( Int32 ) All.Replace );
			}

			GL_SelectTexture( TextureUnit.Texture0 );
			GL_TexEnv( ( Int32 ) All.Replace );
		}

		public virtual void GL_SelectTexture( TextureUnit texture )
		{
			var tmu = ( texture == TextureUnit.Texture0 ) ? 0 : 1;
			if ( tmu != gl_state.currenttmu )
			{
				gl_state.currenttmu = tmu;
				GL.ActiveTexture( texture );
				GL.ClientActiveTexture( texture );
			}
		}

		Int32[] lastmodes = new[] { -1, -1 };
		public override void GL_TexEnv( Int32 mode )
		{
			if ( mode != lastmodes[gl_state.currenttmu] )
			{
				GL.TexEnv( TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, mode );
				lastmodes[gl_state.currenttmu] = mode;
			}
		}

		public override void GL_Bind( Int32 texnum )
		{
			if ( ( gl_nobind.value != 0 ) && ( draw_chars != null ) )
			{
				texnum = draw_chars.texnum;
			}

			if ( gl_state.currenttextures[gl_state.currenttmu] == texnum )
				return;
			gl_state.currenttextures[gl_state.currenttmu] = texnum;
			GL.BindTexture( TextureTarget.Texture2D, texnum );
		}

		public virtual void GL_MBind( TextureUnit target, Int32 texnum )
		{
			GL_SelectTexture( target );
			if ( target == TextureUnit.Texture0 )
			{
				if ( gl_state.currenttextures[0] == texnum )
					return;
			}
			else
			{
				if ( gl_state.currenttextures[1] == texnum )
					return;
			}

			GL_Bind( texnum );
		}

		public class glmode_t
		{
			public String name;
			public Int32 minimize, maximize;
			public glmode_t( String name, Int32 minimize, Int32 maximze )
			{
				this.name = name;
				this.minimize = minimize;
				this.maximize = maximze;
			}
		}

		static readonly glmode_t[] modes = new[] { new glmode_t( "GL_NEAREST", ( Int32 ) All.Nearest, ( Int32 ) All.Nearest ), new glmode_t( "GL_LINEAR", ( Int32 ) All.Linear, ( Int32 ) All.Linear ), new glmode_t( "GL_NEAREST_MIPMAP_NEAREST", ( Int32 ) All.NearestMipmapNearest, ( Int32 ) All.Nearest ), new glmode_t( "GL_LINEAR_MIPMAP_NEAREST", ( Int32 ) All.LinearMipmapNearest, ( Int32 ) All.Linear ), new glmode_t( "GL_NEAREST_MIPMAP_LINEAR", ( Int32 ) All.NearestMipmapLinear, ( Int32 ) All.Nearest ), new glmode_t( "GL_LINEAR_MIPMAP_LINEAR", ( Int32 ) All.LinearMipmapLinear, ( Int32 ) All.Linear ) };
		static readonly Int32 NUM_GL_MODES = modes.Length;
		public class gltmode_t
		{
			public String name;
			public Int32 mode;
			public gltmode_t( String name, Int32 mode )
			{
				this.name = name;
				this.mode = mode;
			}
		}

		static readonly gltmode_t[] gl_alpha_modes = new[] { new gltmode_t( "default", 4 ), new gltmode_t( "GL_RGBA", ( Int32 ) All.Rgba ), new gltmode_t( "GL_RGBA8", ( Int32 ) All.Rgba8 ), new gltmode_t( "GL_RGB5_A1", ( Int32 ) All.Rgb5A1 ), new gltmode_t( "GL_RGBA4", ( Int32 ) All.Rgba4 ), new gltmode_t( "GL_RGBA2", ( Int32 ) All.Rgba2 ) };
		static readonly Int32 NUM_GL_ALPHA_MODES = gl_alpha_modes.Length;
		static readonly gltmode_t[] gl_solid_modes = new[] { new gltmode_t( "default", 3 ), new gltmode_t( "GL_RGB", ( Int32 ) All.Rgb ), new gltmode_t( "GL_RGB8", ( Int32 ) All.Rgb8 ), new gltmode_t( "GL_RGB5", ( Int32 ) All.Rgb5 ), new gltmode_t( "GL_RGB4", ( Int32 ) All.Rgb4 ), new gltmode_t( "GL_R3_G3_B2", ( Int32 ) All.R3G3B2 ) };
		static readonly Int32 NUM_GL_SOLID_MODES = gl_solid_modes.Length;
		public override void GL_TextureMode( String string_renamed )
		{
			Int32 i;
			for ( i = 0; i < NUM_GL_MODES; i++ )
			{
				if ( modes[i].name.EqualsIgnoreCase( string_renamed ) )
					break;
			}

			if ( i == NUM_GL_MODES )
			{
				VID.Printf( Defines.PRINT_ALL, "bad filter name: [" + string_renamed + "]\\n" );
				return;
			}

			gl_filter_min = modes[i].minimize;
			gl_filter_max = modes[i].maximize;
			image_t glt;
			for ( i = 0; i < numgltextures; i++ )
			{
				glt = gltextures[i];
				if ( glt.type != it_pic && glt.type != it_sky )
				{
					GL_Bind( glt.texnum );
					GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_min );
					GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max );
				}
			}
		}

		public override void GL_TextureAlphaMode( String string_renamed )
		{
			Int32 i;
			for ( i = 0; i < NUM_GL_ALPHA_MODES; i++ )
			{
				if ( gl_alpha_modes[i].name.EqualsIgnoreCase( string_renamed ) )
					break;
			}

			if ( i == NUM_GL_ALPHA_MODES )
			{
				VID.Printf( Defines.PRINT_ALL, "bad alpha texture mode name: [" + string_renamed + "]\\n" );
				return;
			}

			gl_tex_alpha_format = ( PixelInternalFormat ) gl_alpha_modes[i].mode;
		}

		public override void GL_TextureSolidMode( String string_renamed )
		{
			Int32 i;
			for ( i = 0; i < NUM_GL_SOLID_MODES; i++ )
			{
				if ( gl_solid_modes[i].name.EqualsIgnoreCase( string_renamed ) )
					break;
			}

			if ( i == NUM_GL_SOLID_MODES )
			{
				VID.Printf( Defines.PRINT_ALL, "bad solid texture mode name: [" + string_renamed + "]\\n" );
				return;
			}

			gl_tex_solid_format = ( PixelInternalFormat ) gl_solid_modes[i].mode;
		}

		public override void GL_ImageList_f( )
		{
			image_t image;
			Int32 texels;
			String[] palstrings = new[] { "RGB", "PAL" };
			VID.Printf( Defines.PRINT_ALL, "------------------\\n" );
			texels = 0;
			for ( var i = 0; i < numgltextures; i++ )
			{
				image = gltextures[i];
				if ( image.texnum <= 0 )
					continue;
				texels += image.upload_width * image.upload_height;
				switch ( image.type )

				{
					case it_skin:
						VID.Printf( Defines.PRINT_ALL, "M" );
						break;
					case it_sprite:
						VID.Printf( Defines.PRINT_ALL, "S" );
						break;
					case it_wall:
						VID.Printf( Defines.PRINT_ALL, "W" );
						break;
					case it_pic:
						VID.Printf( Defines.PRINT_ALL, "P" );
						break;
					default:
						VID.Printf( Defines.PRINT_ALL, " " );
						break;
				}

				VID.Printf( Defines.PRINT_ALL, " %3i %3i %s: %s\\n", image.upload_width, image.upload_height, palstrings[( image.paletted ) ? 1 : 0], image.name );
			}

			VID.Printf( Defines.PRINT_ALL, "Total texel count (not counting mipmaps): " + texels + '\\' );
		}

		public const Int32 MAX_SCRAPS = 1;
		public const Int32 BLOCK_WIDTH = 256;
		public const Int32 BLOCK_HEIGHT = 256;
		public Int32[][] scrap_allocated = Lib.CreateJaggedArray<Int32[][]>( MAX_SCRAPS, BLOCK_WIDTH );
		public Byte[][] scrap_texels = Lib.CreateJaggedArray<Byte[][]>( MAX_SCRAPS, BLOCK_WIDTH * BLOCK_HEIGHT );
		public System.Boolean scrap_dirty;
		public class pos_t
		{
			public Int32 x, y;
			public pos_t( Int32 x, Int32 y )
			{
				this.x = x;
				this.y = y;
			}
		}

		public virtual Int32 Scrap_AllocBlock( Int32 w, Int32 h, pos_t pos )
		{
			Int32 i, j;
			Int32 best, best2;
			Int32 texnum;
			for ( texnum = 0; texnum < MAX_SCRAPS; texnum++ )
			{
				best = BLOCK_HEIGHT;
				for ( i = 0; i < BLOCK_WIDTH - w; i++ )
				{
					best2 = 0;
					for ( j = 0; j < w; j++ )
					{
						if ( scrap_allocated[texnum][i + j] >= best )
							break;
						if ( scrap_allocated[texnum][i + j] > best2 )
							best2 = scrap_allocated[texnum][i + j];
					}

					if ( j == w )
					{
						pos.x = i;
						pos.y = best = best2;
					}
				}

				if ( best + h > BLOCK_HEIGHT )
					continue;
				for ( i = 0; i < w; i++ )
					scrap_allocated[texnum][pos.x + i] = best + h;
				return texnum;
			}

			return -1;
		}

		Int32 scrap_uploads = 0;
		public virtual void Scrap_Upload( )
		{
			scrap_uploads++;
			GL_Bind( TEXNUM_SCRAPS );
			GL_Upload8( scrap_texels[0], BLOCK_WIDTH, BLOCK_HEIGHT, false, false );
			scrap_dirty = false;
		}

		public virtual Byte[] LoadPCX( String filename, Byte[][] palette, out Size dim )
		{
			dim = new Size();
			qfiles.pcx_t pcx;
			Byte[] raw = FS.LoadFile( filename );
			if ( raw == null )
			{
				VID.Printf( Defines.PRINT_DEVELOPER, "Bad pcx file " + filename + '\\' );
				return null;
			}

			pcx = new pcx_t( raw );
			if ( pcx.manufacturer != 0x0a || pcx.version != 5 || pcx.encoding != 1 || pcx.bits_per_pixel != 8 || pcx.xmax >= 640 || pcx.ymax >= 480 )
			{
				VID.Printf( Defines.PRINT_ALL, "Bad pcx file " + filename + '\\' );
				return null;
			}

			var width = pcx.xmax - pcx.xmin + 1;
			var height = pcx.ymax - pcx.ymin + 1;
			Byte[] pix = new Byte[width * height];
			if ( palette != null )
			{
				palette[0] = new Byte[768];
				System.Array.Copy( raw, raw.Length - 768, palette[0], 0, 768 );
			}

			dim.Width = width;
			dim.Height = height;

			var count = 0;
			Byte dataByte = 0;
			var runLength = 0;
			Int32 x, y;
			for ( y = 0; y < height; y++ )
			{
				for ( x = 0; x < width; )
				{
					dataByte = pcx.data.Get();
					if ( ( dataByte & 0xC0 ) == 0xC0 )
					{
						runLength = dataByte & 0x3F;
						dataByte = pcx.data.Get();
						while ( runLength-- > 0 )
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

		private Exception gotoBreakOut = new Exception( "Go to break out" );
		private Exception gotoDone = new Exception( "Go to break done" );
		public virtual Byte[] LoadTGA( String name, Size dim )
		{
			Int32 columns, rows, numPixels;
			Int32 pixbuf;
			Int32 row, column;
			Byte[] raw;
			ByteBuffer buf_p;
			qfiles.tga_t targa_header;
			Byte[] pic = null;
			raw = FS.LoadFile( name );
			if ( raw == null )
			{
				VID.Printf( Defines.PRINT_DEVELOPER, "Bad tga file " + name + '\\' );
				return null;
			}

			targa_header = new tga_t( raw );
			if ( targa_header.image_type != 2 && targa_header.image_type != 10 )
				Com.Error( Defines.ERR_DROP, "LoadTGA: Only type 2 and 10 targa RGB images supported\\n" );
			if ( targa_header.colormap_type != 0 || ( targa_header.pixel_size != 32 && targa_header.pixel_size != 24 ) )
				Com.Error( Defines.ERR_DROP, "LoadTGA: Only 32 or 24 bit images supported (no colormaps)\\n" );
			columns = targa_header.width;
			rows = targa_header.height;
			numPixels = columns * rows;
			if ( dim != Size.Empty )
			{
				dim.Width = columns;
				dim.Height = rows;
			}

			pic = new Byte[numPixels * 4];
			if ( targa_header.id_length != 0 )
				targa_header.data.Position = targa_header.id_length;
			buf_p = targa_header.data;
			Byte red, green, blue, alphabyte;
			red = green = blue = alphabyte = 0;
			Int32 packetHeader, packetSize, j;
			if ( targa_header.image_type == 2 )
			{
				for ( row = rows - 1; row >= 0; row-- )
				{
					pixbuf = row * columns * 4;
					for ( column = 0; column < columns; column++ )
					{
						switch ( targa_header.pixel_size )

						{
							case 24:
								blue = buf_p.Get();
								green = buf_p.Get();
								red = buf_p.Get();
								pic[pixbuf++] = red;
								pic[pixbuf++] = green;
								pic[pixbuf++] = blue;
								pic[pixbuf++] = ( Byte ) 255;
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
			else if ( targa_header.image_type == 10 )
			{
				for ( row = rows - 1; row >= 0; row-- )
				{
					pixbuf = row * columns * 4;
					try
					{
						for ( column = 0; column < columns; )
						{
							packetHeader = buf_p.Get() & 0xFF;
							packetSize = 1 + ( packetHeader & 0x7f );
							if ( ( packetHeader & 0x80 ) != 0 )
							{
								switch ( targa_header.pixel_size )

								{
									case 24:
										blue = buf_p.Get();
										green = buf_p.Get();
										red = buf_p.Get();
										alphabyte = ( Byte ) 255;
										break;
									case 32:
										blue = buf_p.Get();
										green = buf_p.Get();
										red = buf_p.Get();
										alphabyte = buf_p.Get();
										break;
								}

								for ( j = 0; j < packetSize; j++ )
								{
									pic[pixbuf++] = red;
									pic[pixbuf++] = green;
									pic[pixbuf++] = blue;
									pic[pixbuf++] = alphabyte;
									column++;
									if ( column == columns )
									{
										column = 0;
										if ( row > 0 )
											row--;
										else
											throw gotoBreakOut;
										pixbuf = row * columns * 4;
									}
								}
							}
							else
							{
								for ( j = 0; j < packetSize; j++ )
								{
									switch ( targa_header.pixel_size )

									{
										case 24:
											blue = buf_p.Get();
											green = buf_p.Get();
											red = buf_p.Get();
											pic[pixbuf++] = red;
											pic[pixbuf++] = green;
											pic[pixbuf++] = blue;
											pic[pixbuf++] = ( Byte ) 255;
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
									if ( column == columns )
									{
										column = 0;
										if ( row > 0 )
											row--;
										else
											throw gotoBreakOut;
										pixbuf = row * columns * 4;
									}
								}
							}
						}
					}
					catch ( Exception e )
					{
					}
				}
			}

			return pic;
		}

		public class floodfill_t
		{
			public Int16 x, y;
		}

		public const Int32 FLOODFILL_FIFO_SIZE = 0x1000;
		public const Int32 FLOODFILL_FIFO_MASK = FLOODFILL_FIFO_SIZE - 1;
		public static floodfill_t[] fifo = new floodfill_t[FLOODFILL_FIFO_SIZE];
		static Image( )
		{
			for ( var j = 0; j < fifo.Length; j++ )
			{
				fifo[j] = new floodfill_t();
			}
		}

		public virtual void R_FloodFillSkin( Byte[] skin, Int32 skinwidth, Int32 skinheight )
		{
			var fillcolor = skin[0] & 0xff;
			Int32 inpt = 0, outpt = 0;
			var filledcolor = -1;
			Int32 i;
			if ( filledcolor == -1 )
			{
				filledcolor = 0;
				for ( i = 0; i < 256; ++i )
					if ( d_8to24table[i] == 0xFF000000 )
					{
						filledcolor = i;
						break;
					}
			}

			if ( ( fillcolor == filledcolor ) || ( fillcolor == 255 ) )
			{
				return;
			}

			fifo[inpt].x = 0;
			fifo[inpt].y = 0;
			inpt = ( inpt + 1 ) & FLOODFILL_FIFO_MASK;
			while ( outpt != inpt )
			{
				Int32 x = fifo[outpt].x;
				Int32 y = fifo[outpt].y;
				var fdc = filledcolor;
				var pos = x + skinwidth * y;
				outpt = ( outpt + 1 ) & FLOODFILL_FIFO_MASK;
				Int32 off, dx, dy;
				if ( x > 0 )
				{
					off = -1;
					dx = -1;
					dy = 0;
					if ( skin[pos + off] == ( Byte ) fillcolor )
					{
						skin[pos + off] = ( Byte ) 255;
						fifo[inpt].x = ( Int16 ) ( x + dx );
						fifo[inpt].y = ( Int16 ) ( y + dy );
						inpt = ( inpt + 1 ) & FLOODFILL_FIFO_MASK;
					}
					else if ( skin[pos + off] != ( Byte ) 255 )
						fdc = skin[pos + off] & 0xff;
				}

				if ( x < skinwidth - 1 )
				{
					off = 1;
					dx = 1;
					dy = 0;
					if ( skin[pos + off] == ( Byte ) fillcolor )
					{
						skin[pos + off] = ( Byte ) 255;
						fifo[inpt].x = ( Int16 ) ( x + dx );
						fifo[inpt].y = ( Int16 ) ( y + dy );
						inpt = ( inpt + 1 ) & FLOODFILL_FIFO_MASK;
					}
					else if ( skin[pos + off] != ( Byte ) 255 )
						fdc = skin[pos + off] & 0xff;
				}

				if ( y > 0 )
				{
					off = -skinwidth;
					dx = 0;
					dy = -1;
					if ( skin[pos + off] == ( Byte ) fillcolor )
					{
						skin[pos + off] = ( Byte ) 255;
						fifo[inpt].x = ( Int16 ) ( x + dx );
						fifo[inpt].y = ( Int16 ) ( y + dy );
						inpt = ( inpt + 1 ) & FLOODFILL_FIFO_MASK;
					}
					else if ( skin[pos + off] != ( Byte ) 255 )
						fdc = skin[pos + off] & 0xff;
				}

				if ( y < skinheight - 1 )
				{
					off = skinwidth;
					dx = 0;
					dy = 1;
					if ( skin[pos + off] == ( Byte ) fillcolor )
					{
						skin[pos + off] = ( Byte ) 255;
						fifo[inpt].x = ( Int16 ) ( x + dx );
						fifo[inpt].y = ( Int16 ) ( y + dy );
						inpt = ( inpt + 1 ) & FLOODFILL_FIFO_MASK;
					}
					else if ( skin[pos + off] != ( Byte ) 255 )
						fdc = skin[pos + off] & 0xff;
				}

				skin[x + skinwidth * y] = ( Byte ) fdc;
			}
		}

		readonly Int32[] p1 = new Int32[1024];
		readonly Int32[] p2 = new Int32[1024];
		public virtual void GL_ResampleTexture( Int32[] in_renamed, Int32 inwidth, Int32 inheight, Int32[] out_renamed, Int32 outwidth, Int32 outheight )
		{
			p1.Fill( 0 );
			p2.Fill( 0 );
			var fracstep = ( inwidth * 0x10000 ) / outwidth;
			Int32 i, j;
			var frac = fracstep >> 2;
			for ( i = 0; i < outwidth; i++ )
			{
				p1[i] = frac >> 16;
				frac += fracstep;
			}

			frac = 3 * ( fracstep >> 2 );
			for ( i = 0; i < outwidth; i++ )
			{
				p2[i] = frac >> 16;
				frac += fracstep;
			}

			var outp = 0;
			Int32 r, g, b, a;
			Int32 inrow, inrow2;
			Int32 pix1, pix2, pix3, pix4;
			for ( i = 0; i < outheight; i++ )
			{
				inrow = inwidth * ( Int32 ) ( ( i + 0.25F ) * inheight / outheight );
				inrow2 = inwidth * ( Int32 ) ( ( i + 0.75F ) * inheight / outheight );
				frac = fracstep >> 1;
				for ( j = 0; j < outwidth; j++ )
				{
					pix1 = in_renamed[inrow + p1[j]];
					pix2 = in_renamed[inrow + p2[j]];
					pix3 = in_renamed[inrow2 + p1[j]];
					pix4 = in_renamed[inrow2 + p2[j]];
					r = ( ( ( pix1 >> 0 ) & 0xFF ) + ( ( pix2 >> 0 ) & 0xFF ) + ( ( pix3 >> 0 ) & 0xFF ) + ( ( pix4 >> 0 ) & 0xFF ) ) >> 2;
					g = ( ( ( pix1 >> 8 ) & 0xFF ) + ( ( pix2 >> 8 ) & 0xFF ) + ( ( pix3 >> 8 ) & 0xFF ) + ( ( pix4 >> 8 ) & 0xFF ) ) >> 2;
					b = ( ( ( pix1 >> 16 ) & 0xFF ) + ( ( pix2 >> 16 ) & 0xFF ) + ( ( pix3 >> 16 ) & 0xFF ) + ( ( pix4 >> 16 ) & 0xFF ) ) >> 2;
					a = ( ( ( pix1 >> 24 ) & 0xFF ) + ( ( pix2 >> 24 ) & 0xFF ) + ( ( pix3 >> 24 ) & 0xFF ) + ( ( pix4 >> 24 ) & 0xFF ) ) >> 2;
					out_renamed[outp++] = ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | r;
				}
			}
		}

		public virtual void GL_LightScaleTexture( Int32[] in_renamed, Int32 inwidth, Int32 inheight, System.Boolean only_gamma )
		{
			if ( only_gamma )
			{
				Int32 i, c;
				Int32 r, g, b, color;
				c = inwidth * inheight;
				for ( i = 0; i < c; i++ )
				{
					color = in_renamed[i];
					r = ( color >> 0 ) & 0xFF;
					g = ( color >> 8 ) & 0xFF;
					b = ( color >> 16 ) & 0xFF;
					r = gammatable[r] & 0xFF;
					g = gammatable[g] & 0xFF;
					b = gammatable[b] & 0xFF;
					in_renamed[i] = ( Int32 ) ( ( r << 0 ) | ( g << 8 ) | ( b << 16 ) | ( color & 0xFF000000 ) );
				}
			}
			else
			{
				Int32 i, c;
				Int32 r, g, b, color;
				c = inwidth * inheight;
				for ( i = 0; i < c; i++ )
				{
					color = in_renamed[i];
					r = ( color >> 0 ) & 0xFF;
					g = ( color >> 8 ) & 0xFF;
					b = ( color >> 16 ) & 0xFF;
					r = gammatable[intensitytable[r] & 0xFF] & 0xFF;
					g = gammatable[intensitytable[g] & 0xFF] & 0xFF;
					b = gammatable[intensitytable[b] & 0xFF] & 0xFF;
					in_renamed[i] = ( Int32 ) ( ( r << 0 ) | ( g << 8 ) | ( b << 16 ) | ( color & 0xFF000000 ) );
				}
			}
		}

		public virtual void GL_MipMap( Int32[] in_renamed, Int32 width, Int32 height )
		{
			Int32 i, j;
			Int32[] out_renamed;
			out_renamed = in_renamed;
			var inIndex = 0;
			var outIndex = 0;
			Int32 r, g, b, a;
			Int32 p1, p2, p3, p4;
			for ( i = 0; i < height; i += 2, inIndex += width )
			{
				for ( j = 0; j < width; j += 2, outIndex += 1, inIndex += 2 )
				{
					p1 = in_renamed[inIndex + 0];
					p2 = in_renamed[inIndex + 1];
					p3 = in_renamed[inIndex + width + 0];
					p4 = in_renamed[inIndex + width + 1];
					r = ( ( ( p1 >> 0 ) & 0xFF ) + ( ( p2 >> 0 ) & 0xFF ) + ( ( p3 >> 0 ) & 0xFF ) + ( ( p4 >> 0 ) & 0xFF ) ) >> 2;
					g = ( ( ( p1 >> 8 ) & 0xFF ) + ( ( p2 >> 8 ) & 0xFF ) + ( ( p3 >> 8 ) & 0xFF ) + ( ( p4 >> 8 ) & 0xFF ) ) >> 2;
					b = ( ( ( p1 >> 16 ) & 0xFF ) + ( ( p2 >> 16 ) & 0xFF ) + ( ( p3 >> 16 ) & 0xFF ) + ( ( p4 >> 16 ) & 0xFF ) ) >> 2;
					a = ( ( ( p1 >> 24 ) & 0xFF ) + ( ( p2 >> 24 ) & 0xFF ) + ( ( p3 >> 24 ) & 0xFF ) + ( ( p4 >> 24 ) & 0xFF ) ) >> 2;
					out_renamed[outIndex] = ( r << 0 ) | ( g << 8 ) | ( b << 16 ) | ( a << 24 );
				}
			}
		}

		public virtual void GL_BuildPalettedTexture( ByteBuffer paletted_texture, Int32[] scaled, Int32 scaled_width, Int32 scaled_height )
		{
			Int32 r, g, b, c;
			var size = scaled_width * scaled_height;
			for ( var i = 0; i < size; i++ )
			{
				r = ( scaled[i] >> 3 ) & 31;
				g = ( scaled[i] >> 10 ) & 63;
				b = ( scaled[i] >> 19 ) & 31;
				c = r | ( g << 5 ) | ( b << 11 );
				paletted_texture.Put( i, gl_state.d_16to8table[c] );
			}
		}

		Int32 upload_width, upload_height;
		System.Boolean uploaded_paletted;
		Int32[] scaled = new Int32[256 * 256];
		ByteBuffer paletted_texture = Lib.NewByteBuffer( 256 * 256 );
		Int32Buffer tex = Lib.NewInt32Buffer( 512 * 256, ByteOrder.LittleEndian );
		public virtual System.Boolean GL_Upload32( Int32[] data, Int32 width, Int32 height, System.Boolean mipmap )
		{
			Int32 samples;
			Int32 scaled_width = 0, scaled_height = 0;
			Int32 i, c;
			Int32 comp;
			scaled.Fill( 0 );
			paletted_texture.Clear();
			for ( var j = 0; j < 256 * 256; j++ )
				paletted_texture.Put( j, ( Byte ) 0 );
			uploaded_paletted = false;
			if ( gl_round_down.value > 0F && scaled_width > width && mipmap )
				scaled_width >>= 1;
			if ( gl_round_down.value > 0F && scaled_height > height && mipmap )
				scaled_height >>= 1;
			if ( mipmap )
			{
				scaled_width >>= ( Int32 ) gl_picmip.value;
				scaled_height >>= ( Int32 ) gl_picmip.value;
			}

			if ( scaled_width > 256 )
				scaled_width = 256;
			if ( scaled_height > 256 )
				scaled_height = 256;
			if ( scaled_width < 1 )
				scaled_width = 1;
			if ( scaled_height < 1 )
				scaled_height = 1;
			upload_width = scaled_width;
			upload_height = scaled_height;
			if ( scaled_width * scaled_height > 256 * 256 )
				Com.Error( Defines.ERR_DROP, "GL_Upload32: too big" );
			c = width * height;
			samples = ( Int32 ) gl_solid_format;
			for ( i = 0; i < c; i++ )
			{
				if ( ( data[i] & 0xff000000 ) != 0xff000000 )
				{
					samples = ( Int32 ) gl_alpha_format;
					break;
				}
			}

			if ( samples == ( Int32 ) gl_solid_format )
				comp = ( Int32 ) gl_tex_solid_format;
			else if ( samples == ( Int32 ) gl_alpha_format )
				comp = ( Int32 ) gl_tex_alpha_format;
			else
			{
				VID.Printf( Defines.PRINT_ALL, "Unknown number of texture components " + samples + '\\' );
				comp = samples;
			}

			try
			{
				if ( scaled_width == width && scaled_height == height )
				{
					if ( !mipmap )
					{
						if ( qglColorTableEXT && gl_ext_palettedtexture.value != 0F && samples == ( Int32 ) gl_solid_format )
						{
							uploaded_paletted = true;
							GL_BuildPalettedTexture( paletted_texture, data, scaled_width, scaled_height );
							new Pinnable( paletted_texture, ( ptr ) =>
							{
								GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) GL_COLOR_INDEX8_EXT, scaled_width, scaled_height, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, ptr );
							} );
						}
						else
						{
							tex.Rewind();
							tex.Put( data );
							tex.Rewind();
							new Pinnable( tex, ( ptr ) =>
							{
								GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) comp, scaled_width, scaled_height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr );
							} );
						}

						throw gotoDone;
					}

					System.Array.Copy( data, 0, scaled, 0, width * height );
				}
				else
					GL_ResampleTexture( data, width, height, scaled, scaled_width, scaled_height );
				GL_LightScaleTexture( scaled, scaled_width, scaled_height, !mipmap );
				if ( qglColorTableEXT && gl_ext_palettedtexture.value != 0F && ( samples == ( Int32 ) gl_solid_format ) )
				{
					uploaded_paletted = true;
					GL_BuildPalettedTexture( paletted_texture, scaled, scaled_width, scaled_height );
					new Pinnable( paletted_texture, ( ptr ) =>
					{
						GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) GL_COLOR_INDEX8_EXT, scaled_width, scaled_height, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, ptr );
					} );
				}
				else
				{
					tex.Rewind();
					tex.Put( scaled );
					tex.Rewind();
					new Pinnable( tex, ( ptr ) =>
					{
						GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) comp, scaled_width, scaled_height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr );
					} );
				}

				if ( mipmap )
				{
					Int32 miplevel;
					miplevel = 0;
					while ( scaled_width > 1 || scaled_height > 1 )
					{
						GL_MipMap( scaled, scaled_width, scaled_height );
						scaled_width >>= 1;
						scaled_height >>= 1;
						if ( scaled_width < 1 )
							scaled_width = 1;
						if ( scaled_height < 1 )
							scaled_height = 1;
						miplevel++;
						if ( qglColorTableEXT && gl_ext_palettedtexture.value != 0F && samples == ( Int32 ) gl_solid_format )
						{
							uploaded_paletted = true;
							GL_BuildPalettedTexture( paletted_texture, scaled, scaled_width, scaled_height );
							new Pinnable( paletted_texture, ( ptr ) =>
							{
								GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) GL_COLOR_INDEX8_EXT, scaled_width, scaled_height, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, ptr );
							} );
						}
						else
						{
							tex.Rewind();
							tex.Put( scaled );
							tex.Rewind();
							new Pinnable( tex, ( ptr ) =>
							{
								GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) comp, scaled_width, scaled_height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr );
							} );
						}
					}
				}
			}
			catch ( Exception e )
			{
			}

			if ( mipmap )
			{
				GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_min );
				GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max );
			}
			else
			{
				GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_max );
				GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max );
			}

			return ( samples == ( Int32 ) gl_alpha_format );
		}

		Int32[] trans = new Int32[512 * 256];
		public virtual System.Boolean GL_Upload8( Byte[] data, Int32 width, Int32 height, System.Boolean mipmap, System.Boolean is_sky )
		{
			trans.Fill( 0 );
			var s = width * height;
			if ( s > trans.Length )
				Com.Error( Defines.ERR_DROP, "GL_Upload8: too large" );
			if ( qglColorTableEXT && gl_ext_palettedtexture.value != 0F && is_sky )
			{
				new Pinnable( data, ( ptr ) =>
				{
					GL.TexImage2D( TextureTarget.Texture2D, 0, ( PixelInternalFormat ) GL_COLOR_INDEX8_EXT, width, height, 0, PixelFormat.ColorIndex, PixelType.UnsignedByte, ptr );
				} );
				GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, gl_filter_max );
				GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, gl_filter_max );
				return false;
			}
			else
			{
				Int32 p;
				for ( var i = 0; i < s; i++ )
				{
					p = data[i] & 0xff;
					trans[i] = d_8to24table[p];
					if ( p == 255 )
					{
						if ( i > width && ( data[i - width] & 0xff ) != 255 )
							p = data[i - width] & 0xff;
						else if ( i < s - width && ( data[i + width] & 0xff ) != 255 )
							p = data[i + width] & 0xff;
						else if ( i > 0 && ( data[i - 1] & 0xff ) != 255 )
							p = data[i - 1] & 0xff;
						else if ( i < s - 1 && ( data[i + 1] & 0xff ) != 255 )
							p = data[i + 1] & 0xff;
						else
							p = 0;
						trans[i] = d_8to24table[p] & 0x00FFFFFF;
					}
				}

				return GL_Upload32( trans, width, height, mipmap );
			}
		}

		public virtual image_t GL_LoadPic( String name, Byte[] pic, Int32 width, Int32 height, Int32 type, Int32 bits )
		{
			image_t image;
			Int32 i;
			for ( i = 0; i < numgltextures; i++ )
			{
				image = gltextures[i];
				if ( image.texnum == 0 )
					break;
			}

			if ( i == numgltextures )
			{
				if ( numgltextures == MAX_GLTEXTURES )
					Com.Error( Defines.ERR_DROP, "MAX_GLTEXTURES" );
				numgltextures++;
			}

			image = gltextures[i];
			if ( name.Length > Defines.MAX_QPATH )
				Com.Error( Defines.ERR_DROP, "Draw_LoadPic: \\\"" + name + "\\\" is too long" );
			image.name = name;
			image.registration_sequence = registration_sequence;
			image.width = width;
			image.height = height;
			image.type = type;
			if ( type == it_skin && bits == 8 )
				R_FloodFillSkin( pic, width, height );
			if ( image.type == it_pic && bits == 8 && image.width < 64 && image.height < 64 )
			{
				pos_t pos = new pos_t( 0, 0 );
				Int32 j, k;
				var texnum = Scrap_AllocBlock( image.width, image.height, pos );
				if ( texnum == -1 )
				{
					image.scrap = false;
					image.texnum = TEXNUM_IMAGES + image.GetId();
					GL_Bind( image.texnum );
					if ( bits == 8 )
					{
						image.has_alpha = GL_Upload8( pic, width, height, ( image.type != it_pic && image.type != it_sky ), image.type == it_sky );
					}
					else
					{
						Int32[] tmp = new Int32[pic.Length / 4];
						for ( i = 0; i < tmp.Length; i++ )
						{
							tmp[i] = ( ( pic[4 * i + 0] & 0xFF ) << 0 );
							tmp[i] |= ( ( pic[4 * i + 1] & 0xFF ) << 8 );
							tmp[i] |= ( ( pic[4 * i + 2] & 0xFF ) << 16 );
							tmp[i] |= ( ( pic[4 * i + 3] & 0xFF ) << 24 );
						}

						image.has_alpha = GL_Upload32( tmp, width, height, ( image.type != it_pic && image.type != it_sky ) );
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
				for ( i = 0; i < image.height; i++ )
					for ( j = 0; j < image.width; j++, k++ )
						scrap_texels[texnum][( pos.y + i ) * BLOCK_WIDTH + pos.x + j] = pic[k];
				image.texnum = TEXNUM_SCRAPS + texnum;
				image.scrap = true;
				image.has_alpha = true;
				image.sl = ( pos.x + 0.01F ) / ( Single ) BLOCK_WIDTH;
				image.sh = ( pos.x + image.width - 0.01F ) / ( Single ) BLOCK_WIDTH;
				image.tl = ( pos.y + 0.01F ) / ( Single ) BLOCK_WIDTH;
				image.th = ( pos.y + image.height - 0.01F ) / ( Single ) BLOCK_WIDTH;
			}
			else
			{
				image.scrap = false;
				image.texnum = TEXNUM_IMAGES + image.GetId();
				GL_Bind( image.texnum );
				if ( bits == 8 )
				{
					image.has_alpha = GL_Upload8( pic, width, height, ( image.type != it_pic && image.type != it_sky ), image.type == it_sky );
				}
				else
				{
					Int32[] tmp = new Int32[pic.Length / 4];
					for ( i = 0; i < tmp.Length; i++ )
					{
						tmp[i] = ( ( pic[4 * i + 0] & 0xFF ) << 0 );
						tmp[i] |= ( ( pic[4 * i + 1] & 0xFF ) << 8 );
						tmp[i] |= ( ( pic[4 * i + 2] & 0xFF ) << 16 );
						tmp[i] |= ( ( pic[4 * i + 3] & 0xFF ) << 24 );
					}

					image.has_alpha = GL_Upload32( tmp, width, height, ( image.type != it_pic && image.type != it_sky ) );
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

		public virtual image_t GL_LoadWal( String name )
		{
			image_t image = null;
			Byte[] raw = FS.LoadFile( name );
			if ( raw == null )
			{
				VID.Printf( Defines.PRINT_ALL, "GL_FindImage: can't load " + name + '\\' );
				return r_notexture;
			}

			qfiles.miptex_t mt = new qfiles.miptex_t( raw );
			Byte[] pix = new Byte[mt.width * mt.height];
			System.Array.Copy( raw, mt.offsets[0], pix, 0, pix.Length );
			image = GL_LoadPic( name, pix, mt.width, mt.height, it_wall, 8 );
			return image;
		}

		Dictionary<String, image_t> imageCache = new Dictionary<String, image_t>( MAX_GLTEXTURES );
		public virtual image_t GL_FindImage( String name, Int32 type )
		{
			if ( name == null || name.Length < 1 )
				return null;

			image_t image = null;

			if ( imageCache.ContainsKey( name ) )
			{
				image = imageCache[name];

				if ( image != null )
				{
					image.registration_sequence = registration_sequence;
					return image;
				}
				else // Shouldn't happen but delete null entries
					imageCache.Remove( name );
			}

			image = null;
			Byte[] pic = null;
			Size dim = new Size();
			if ( name.EndsWith( ".pcx" ) )
			{
				pic = LoadPCX( name, null, out dim );
				if ( pic == null )
					return null;
				image = GL_LoadPic( name, pic, dim.Width, dim.Height, type, 8 );
			}
			else if ( name.EndsWith( ".wal" ) )
			{
				image = GL_LoadWal( name );
			}
			else if ( name.EndsWith( ".tga" ) )
			{
				pic = LoadTGA( name, dim );
				if ( pic == null )
					return null;
				image = GL_LoadPic( name, pic, dim.Width, dim.Height, type, 32 );
			}
			else
			{
				pic = LoadPCX( "pics/" + name + ".pcx", null, out dim );
				if ( pic == null )
					return null;
				image = GL_LoadPic( name, pic, dim.Width, dim.Height, type, 8 );
			}

			imageCache.Add( image.name, image );
			return image;
		}

		public override image_t R_RegisterSkin( String name )
		{
			return GL_FindImage( name, it_skin );
		}

		Int32Buffer texnumBuffer = Lib.NewInt32Buffer( 1 );
		public virtual void GL_FreeUnusedImages( )
		{
			r_notexture.registration_sequence = registration_sequence;
			r_particletexture.registration_sequence = registration_sequence;
			image_t image = null;
			for ( var i = 0; i < numgltextures; i++ )
			{
				image = gltextures[i];
				if ( image.registration_sequence == registration_sequence )
					continue;
				if ( image.registration_sequence == 0 )
					continue;
				if ( image.type == it_pic )
					continue;
				texnumBuffer.Clear();
				texnumBuffer.Put( 0, image.texnum );
				GL.DeleteTextures( texnumBuffer.Array.Length, texnumBuffer.Array );
				imageCache.Remove( image.name );
				image.Clear();
			}
		}

		protected override void Draw_GetPalette( )
		{
			Int32 r, g, b;
			Byte[][] palette = Lib.CreateJaggedArray<Byte[][]>( 1, 1 );
			LoadPCX( "pics/colormap.pcx", palette, out var dummy );
			if ( palette[0] == null || palette[0].Length != 768 )
				Com.Error( Defines.ERR_FATAL, "Couldn't load pics/colormap.pcx" );
			Byte[] pal = palette[0];
			var j = 0;
			for ( var i = 0; i < 256; i++ )
			{
				r = pal[j++] & 0xFF;
				g = pal[j++] & 0xFF;
				b = pal[j++] & 0xFF;
				d_8to24table[i] = ( 255 << 24 ) | ( b << 16 ) | ( g << 8 ) | ( r << 0 );
			}

			d_8to24table[255] &= 0x00FFFFFF;
			particle_t.SetColorPalette( d_8to24table );
		}

		public override void GL_InitImages( )
		{
			Int32 i, j;
			var g = vid_gamma.value;
			registration_sequence = 1;
			intensity = Cvar.Get( "intensity", "2", 0 );
			if ( intensity.value <= 1 )
				Cvar.Set( "intensity", "1" );
			gl_state.inverse_intensity = 1 / intensity.value;
			Draw_GetPalette();
			if ( qglColorTableEXT )
			{
				gl_state.d_16to8table = FS.LoadFile( "pics/16to8.dat" );
				if ( gl_state.d_16to8table == null )
					Com.Error( Defines.ERR_FATAL, "Couldn't load pics/16to8.pcx" );
			}

			if ( ( gl_config.renderer & ( GL_RENDERER_VOODOO | GL_RENDERER_VOODOO2 ) ) != 0 )
			{
				g = 1F;
			}

			for ( i = 0; i < 256; i++ )
			{
				if ( g == 1F )
				{
					gammatable[i] = ( Byte ) i;
				}
				else
				{
					var inf = ( Int32 ) ( 255F * Math.Pow( ( i + 0.5 ) / 255.5, g ) + 0.5 );
					if ( inf < 0 )
						inf = 0;
					if ( inf > 255 )
						inf = 255;
					gammatable[i] = ( Byte ) inf;
				}
			}

			for ( i = 0; i < 256; i++ )
			{
				j = ( Int32 ) ( i * intensity.value );
				if ( j > 255 )
					j = 255;
				intensitytable[i] = ( Byte ) j;
			}
		}

		public override void GL_ShutdownImages( )
		{
			image_t image;
			for ( var i = 0; i < numgltextures; i++ )
			{
				image = gltextures[i];
				if ( image.registration_sequence == 0 )
					continue;
				texnumBuffer.Clear();
				texnumBuffer.Put( 0, image.texnum );
				GL.DeleteTextures( 1, texnumBuffer.Array );
				imageCache.Remove( image.name );
				image.Clear();
			}
		}
	}
}
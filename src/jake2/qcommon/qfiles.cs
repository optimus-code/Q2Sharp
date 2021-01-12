using J2N.IO;
using Jake2.Util;
using System;
using System.Text;

namespace Jake2.Qcommon
{
	public class qfiles
	{
		public class pcx_t
		{
			static readonly Int32 PALETTE_SIZE = 48;
			static readonly Int32 FILLER_SIZE = 58;
			public Byte manufacturer;
			public Byte version;
			public Byte encoding;
			public Byte bits_per_pixel;
			public Int32 xmin, ymin, xmax, ymax;
			public Int32 hres, vres;
			public Byte[] palette;
			public Byte reserved;
			public Byte color_planes;
			public Int32 bytes_per_line;
			public Int32 palette_type;
			public Byte[] filler;
			public ByteBuffer data;
			public pcx_t( Byte[] dataBytes ) : this( ByteBuffer.Wrap( dataBytes ) )
			{
			}

			public pcx_t( ByteBuffer b )
			{
				b.Order = ByteOrder.LittleEndian;
				manufacturer = b.Get();
				version = b.Get();
				encoding = b.Get();
				bits_per_pixel = b.Get();
				xmin = b.GetInt16() & 0xffff;
				ymin = b.GetInt16() & 0xffff;
				xmax = b.GetInt16() & 0xffff;
				ymax = b.GetInt16() & 0xffff;
				hres = b.GetInt16() & 0xffff;
				vres = b.GetInt16() & 0xffff;
				b.Get( palette = new Byte[PALETTE_SIZE] );
				reserved = b.Get();
				color_planes = b.Get();
				bytes_per_line = b.GetInt16() & 0xffff;
				palette_type = b.GetInt16() & 0xffff;
				b.Get( filler = new Byte[FILLER_SIZE] );
				data = b.Slice();
			}
		}

		public class tga_t
		{
			public Int32 id_length, colormap_type, image_type;
			public Int32 colormap_index, colormap_length;
			public Int32 colormap_size;
			public Int32 x_origin, y_origin, width, height;
			public Int32 pixel_size, attributes;
			public ByteBuffer data;
			public tga_t( Byte[] dataBytes ) : this( ByteBuffer.Wrap( dataBytes ) )
			{
			}

			public tga_t( ByteBuffer b )
			{
				b.Order = ByteOrder.LittleEndian;
				id_length = b.Get() & 0xFF;
				colormap_type = b.Get() & 0xFF;
				image_type = b.Get() & 0xFF;
				colormap_index = b.GetInt16() & 0xFFFF;
				colormap_length = b.GetInt16() & 0xFFFF;
				colormap_size = b.Get() & 0xFF;
				x_origin = b.GetInt16() & 0xFFFF;
				y_origin = b.GetInt16() & 0xFFFF;
				width = b.GetInt16() & 0xFFFF;
				height = b.GetInt16() & 0xFFFF;
				pixel_size = b.Get() & 0xFF;
				attributes = b.Get() & 0xFF;
				data = b.Slice();
			}
		}

		public const Int32 IDALIASHEADER = ( ( '2' << 24 ) + ( 'P' << 16 ) + ( 'D' << 8 ) + 'I' );
		public const Int32 ALIAS_VERSION = 8;
		public const Int32 MAX_TRIANGLES = 4096;
		public const Int32 MAX_VERTS = 2048;
		public const Int32 MAX_FRAMES = 512;
		public const Int32 MAX_MD2SKINS = 32;
		public const Int32 MAX_SKINNAME = 64;
		public class dstvert_t
		{
			public Int16 s;
			public Int16 t;
			public dstvert_t( ByteBuffer b )
			{
				s = b.GetInt16();
				t = b.GetInt16();
			}
		}

		public class dtriangle_t
		{
			public Int16[] index_xyz = new Int16[] { 0, 0, 0 };
			public Int16[] index_st = new Int16[] { 0, 0, 0 };
			public dtriangle_t( ByteBuffer b )
			{
				index_xyz[0] = b.GetInt16();
				index_xyz[1] = b.GetInt16();
				index_xyz[2] = b.GetInt16();
				index_st[0] = b.GetInt16();
				index_st[1] = b.GetInt16();
				index_st[2] = b.GetInt16();
			}
		}

		public static readonly Int32 DTRIVERTX_V0 = 0;
		public static readonly Int32 DTRIVERTX_V1 = 1;
		public static readonly Int32 DTRIVERTX_V2 = 2;
		public static readonly Int32 DTRIVERTX_LNI = 3;
		public static readonly Int32 DTRIVERTX_SIZE = 4;
		public class daliasframe_t
		{
			public Single[] scale = new Single[] { 0, 0, 0 };
			public Single[] translate = new Single[] { 0, 0, 0 };
			public String name;
			public Int32[] verts;
			public daliasframe_t( ByteBuffer b )
			{
				scale[0] = b.GetSingle();
				scale[1] = b.GetSingle();
				scale[2] = b.GetSingle();
				translate[0] = b.GetSingle();
				translate[1] = b.GetSingle();
				translate[2] = b.GetSingle();
				Byte[] nameBuf = new Byte[16];
				b.Get( nameBuf );
				name = Encoding.ASCII.GetString( nameBuf ).Trim();
			}
		}

		public class dmdl_t
		{
			public Int32 ident;
			public Int32 version;
			public Int32 skinwidth;
			public Int32 skinheight;
			public Int32 framesize;
			public Int32 num_skins;
			public Int32 num_xyz;
			public Int32 num_st;
			public Int32 num_tris;
			public Int32 num_glcmds;
			public Int32 num_frames;
			public Int32 ofs_skins;
			public Int32 ofs_st;
			public Int32 ofs_tris;
			public Int32 ofs_frames;
			public Int32 ofs_glcmds;
			public Int32 ofs_end;
			public String[] skinNames;
			public dstvert_t[] stVerts;
			public dtriangle_t[] triAngles;
			public Int32[] glCmds;
			public daliasframe_t[] aliasFrames;
			public dmdl_t( ByteBuffer b )
			{
				ident = b.GetInt32();
				version = b.GetInt32();
				skinwidth = b.GetInt32();
				skinheight = b.GetInt32();
				framesize = b.GetInt32();
				num_skins = b.GetInt32();
				num_xyz = b.GetInt32();
				num_st = b.GetInt32();
				num_tris = b.GetInt32();
				num_glcmds = b.GetInt32();
				num_frames = b.GetInt32();
				ofs_skins = b.GetInt32();
				ofs_st = b.GetInt32();
				ofs_tris = b.GetInt32();
				ofs_frames = b.GetInt32();
				ofs_glcmds = b.GetInt32();
				ofs_end = b.GetInt32();
			}

			public SingleBuffer textureCoordBuf = null;
			public Int32Buffer vertexIndexBuf = null;
			public Int32[] counts = null;
			public Int32Buffer[] indexElements = null;
		}

		public const Int32 IDSPRITEHEADER = ( ( '2' << 24 ) + ( 'S' << 16 ) + ( 'D' << 8 ) + 'I' );
		public const Int32 SPRITE_VERSION = 2;
		public class dsprframe_t
		{
			public Int32 width, height;
			public Int32 origin_x, origin_y;
			public String name;
			public dsprframe_t( ByteBuffer b )
			{
				width = b.GetInt32();
				height = b.GetInt32();
				origin_x = b.GetInt32();
				origin_y = b.GetInt32();
				Byte[] nameBuf = new Byte[MAX_SKINNAME];
				b.Get( nameBuf );
				name = Encoding.ASCII.GetString( nameBuf ).Trim();
			}
		}

		public class dsprite_t
		{
			public Int32 ident;
			public Int32 version;
			public Int32 numframes;
			public dsprframe_t[] frames;
			public dsprite_t( ByteBuffer b )
			{
				ident = b.GetInt32();
				version = b.GetInt32();
				numframes = b.GetInt32();
				frames = new dsprframe_t[numframes];
				for ( var i = 0; i < numframes; i++ )
				{
					frames[i] = new dsprframe_t( b );
				}
			}
		}

		public class miptex_t
		{
			static readonly Int32 MIPLEVELS = 4;
			static readonly Int32 NAME_SIZE = 32;
			public String name;
			public Int32 width, height;
			public Int32[] offsets = new Int32[MIPLEVELS];
			public String animname;
			public Int32 flags;
			public Int32 contents;
			public Int32 value;
			public miptex_t( Byte[] dataBytes ) : this( ByteBuffer.Wrap( dataBytes ) )
			{
			}

			public miptex_t( ByteBuffer b )
			{
				b.Order = ByteOrder.LittleEndian;
				Byte[] nameBuf = new Byte[NAME_SIZE];
				b.Get( nameBuf );
				name = Encoding.ASCII.GetString( nameBuf ).Trim();
				width = b.GetInt32();
				height = b.GetInt32();
				offsets[0] = b.GetInt32();
				offsets[1] = b.GetInt32();
				offsets[2] = b.GetInt32();
				offsets[3] = b.GetInt32();
				b.Get( nameBuf );
				animname = Encoding.ASCII.GetString( nameBuf ).Trim();
				flags = b.GetInt32();
				contents = b.GetInt32();
				value = b.GetInt32();
			}
		}

		public const Int32 IDBSPHEADER = ( ( 'P' << 24 ) + ( 'S' << 16 ) + ( 'B' << 8 ) + 'I' );
		public class dheader_t
		{
			public dheader_t( ByteBuffer bb )
			{
				bb.Order = ByteOrder.LittleEndian;
				this.ident = bb.GetInt32();
				this.version = bb.GetInt32();
				for ( var n = 0; n < Defines.HEADER_LUMPS; n++ )
					lumps[n] = new lump_t( bb.GetInt32(), bb.GetInt32() );
			}

			public Int32 ident;
			public Int32 version;
			public lump_t[] lumps = new lump_t[Defines.HEADER_LUMPS];
		}

		public class dmodel_t
		{
			public dmodel_t( ByteBuffer bb )
			{
				bb.Order = ByteOrder.LittleEndian;
				for ( var j = 0; j < 3; j++ )
					mins[j] = bb.GetSingle();
				for ( var j = 0; j < 3; j++ )
					maxs[j] = bb.GetSingle();
				for ( var j = 0; j < 3; j++ )
					origin[j] = bb.GetSingle();
				headnode = bb.GetInt32();
				firstface = bb.GetInt32();
				numfaces = bb.GetInt32();
			}

			public Single[] mins = new Single[] { 0, 0, 0 };
			public Single[] maxs = new Single[] { 0, 0, 0 };
			public Single[] origin = new Single[] { 0, 0, 0 };
			public Int32 headnode;
			public Int32 firstface, numfaces;
			public static Int32 SIZE = 3 * 4 + 3 * 4 + 3 * 4 + 4 + 8;
		}

		public class dvertex_t
		{
			public static readonly Int32 SIZE = 3 * 4;
			public Single[] point = new Single[] { 0, 0, 0 };
			public dvertex_t( ByteBuffer b )
			{
				point[0] = b.GetSingle();
				point[1] = b.GetSingle();
				point[2] = b.GetSingle();
			}
		}

		public class dplane_t
		{
			public dplane_t( ByteBuffer bb )
			{
				bb.Order = ByteOrder.LittleEndian;
				normal[0] = ( bb.GetSingle() );
				normal[1] = ( bb.GetSingle() );
				normal[2] = ( bb.GetSingle() );
				dist = ( bb.GetSingle() );
				type = ( bb.GetInt32() );
			}

			public Single[] normal = new Single[] { 0, 0, 0 };
			public Single dist;
			public Int32 type;
			public static readonly Int32 SIZE = 3 * 4 + 4 + 4;
		}

		public class dnode_t
		{
			public dnode_t( ByteBuffer bb )
			{
				bb.Order = ByteOrder.LittleEndian;
				planenum = bb.GetInt32();
				children[0] = bb.GetInt32();
				children[1] = bb.GetInt32();
				for ( var j = 0; j < 3; j++ )
					mins[j] = bb.GetInt16();
				for ( var j = 0; j < 3; j++ )
					maxs[j] = bb.GetInt16();
				firstface = bb.GetInt16() & 0xffff;
				numfaces = bb.GetInt16() & 0xffff;
			}

			public Int32 planenum;
			public Int32[] children = new Int32[] { 0, 0 };
			public Int16[] mins = new Int16[] { 0, 0, 0 };
			public Int16[] maxs = new Int16[] { 0, 0, 0 };
			public Int32 firstface;
			public Int32 numfaces;
			public static Int32 SIZE = 4 + 8 + 6 + 6 + 2 + 2;
		}

		public class dedge_t
		{
			Int32[] v = new[] { 0, 0 };
		}

		public class dface_t
		{
			public static readonly Int32 SIZE = 4 * Defines.SIZE_OF_SHORT + 2 * Defines.SIZE_OF_INT + Defines.MAXLIGHTMAPS;
			public Int32 planenum;
			public Int16 side;
			public Int32 firstedge;
			public Int16 numedges;
			public Int16 texinfo;
			public Byte[] styles = new Byte[Defines.MAXLIGHTMAPS];
			public Int32 lightofs;
			public dface_t( ByteBuffer b )
			{
				planenum = b.GetInt16() & 0xFFFF;
				side = b.GetInt16();
				firstedge = b.GetInt32();
				numedges = b.GetInt16();
				texinfo = b.GetInt16();
				b.Get( styles );
				lightofs = b.GetInt32();
			}
		}

		public class dleaf_t
		{
			public dleaf_t( Byte[] cmod_base, Int32 i, Int32 j )
			{
				var bb = ByteBuffer.Wrap( cmod_base, i, j );
				bb.Order = ByteOrder.LittleEndian;
				Setup( bb );
			}

			public dleaf_t( ByteBuffer bb )
			{
				Setup( bb );
			}

			private void Setup( ByteBuffer bb )
			{
				contents = bb.GetInt32();
				cluster = bb.GetInt16();
				area = bb.GetInt16();
				mins[0] = bb.GetInt16();
				mins[1] = bb.GetInt16();
				mins[2] = bb.GetInt16();
				maxs[0] = bb.GetInt16();
				maxs[1] = bb.GetInt16();
				maxs[2] = bb.GetInt16();
				firstleafface = bb.GetInt16() & 0xffff;
				numleaffaces = bb.GetInt16() & 0xffff;
				firstleafbrush = bb.GetInt16() & 0xffff;
				numleafbrushes = bb.GetInt16() & 0xffff;
			}

			public static readonly Int32 SIZE = 4 + 8 * 2 + 4 * 2;
			public Int32 contents;
			public Int16 cluster;
			public Int16 area;
			public Int16[] mins = new Int16[] { 0, 0, 0 };
			public Int16[] maxs = new Int16[] { 0, 0, 0 };
			public Int32 firstleafface;
			public Int32 numleaffaces;
			public Int32 firstleafbrush;
			public Int32 numleafbrushes;
		}

		public class dbrushside_t
		{
			public dbrushside_t( ByteBuffer bb )
			{
				bb.Order = ByteOrder.LittleEndian;
				planenum = bb.GetInt16() & 0xffff;
				texinfo = bb.GetInt16();
			}

			public Int32 planenum;
			public Int16 texinfo;
			public static Int32 SIZE = 4;
		}

		public class dbrush_t
		{
			public dbrush_t( ByteBuffer bb )
			{
				bb.Order = ByteOrder.LittleEndian;
				firstside = bb.GetInt32();
				numsides = bb.GetInt32();
				contents = bb.GetInt32();
			}

			public static Int32 SIZE = 3 * 4;
			public Int32 firstside;
			public Int32 numsides;
			public Int32 contents;
		}

		public class dvis_t
		{
			public dvis_t( ByteBuffer bb )
			{
				numclusters = bb.GetInt32();
				bitofs = Lib.CreateJaggedArray<Int32[][]>( numclusters, 2 );
				for ( var i = 0; i < numclusters; i++ )
				{
					bitofs[i][0] = bb.GetInt32();
					bitofs[i][1] = bb.GetInt32();
				}
			}

			public Int32 numclusters;
			public Int32[][] bitofs = Lib.CreateJaggedArray<Int32[][]>( 8, 2 );
		}

		public class dareaportal_t
		{
			public dareaportal_t( )
			{
			}

			public dareaportal_t( ByteBuffer bb )
			{
				bb.Order = ByteOrder.LittleEndian;
				portalnum = bb.GetInt32();
				otherarea = bb.GetInt32();
			}

			public Int32 portalnum;
			public Int32 otherarea;
			public static Int32 SIZE = 8;
		}

		public class darea_t
		{
			public darea_t( ByteBuffer bb )
			{
				bb.Order = ByteOrder.LittleEndian;
				numareaportals = bb.GetInt32();
				firstareaportal = bb.GetInt32();
			}

			public Int32 numareaportals;
			public Int32 firstareaportal;
			public static Int32 SIZE = 8;
		}
	}
}
using J2N.IO;
using Q2Sharp.Game;
using Q2Sharp.Util;
using System;
using System.Text;
using static Q2Sharp.Qcommon.qfiles;

namespace Q2Sharp.Qcommon
{
	public class CM
	{
		public class cnode_t
		{
			public cplane_t plane;
			public Int32[] children = new[] { 0, 0 };
		}

		public class cbrushside_t
		{
			public cplane_t plane;
			public mapsurface_t surface;
		}

		public class cleaf_t
		{
			public Int32 contents;
			public Int32 cluster;
			public Int32 area;
			public Int16 firstleafbrush;
			public Int16 numleafbrushes;
		}

		public class cbrush_t
		{
			public Int32 contents;
			public Int32 numsides;
			public Int32 firstbrushside;
			public Int32 checkcount;
		}

		public class carea_t
		{
			public Int32 numareaportals;
			public Int32 firstareaportal;
			public Int32 floodnum;
			public Int32 floodvalid;
		}

		static Int32 checkcount;
		static String map_name = "";
		static Int32 numbrushsides;
		static cbrushside_t[] map_brushsides = new cbrushside_t[Defines.MAX_MAP_BRUSHSIDES];

		public static Int32 numtexinfo;
		public static mapsurface_t[] map_surfaces = new mapsurface_t[Defines.MAX_MAP_TEXINFO];

		static Int32 numplanes;
		static cplane_t[] map_planes = new cplane_t[Defines.MAX_MAP_PLANES + 6];

		static Int32 numnodes;
		static cnode_t[] map_nodes = new cnode_t[Defines.MAX_MAP_NODES + 6];

		static Int32 numleafs = 1;
		static cleaf_t[] map_leafs = new cleaf_t[Defines.MAX_MAP_LEAFS];

		static Int32 emptyleaf, solidleaf;
		static Int32 numleafbrushes;
		public static Int32[] map_leafbrushes = new Int32[Defines.MAX_MAP_LEAFBRUSHES];
		public static Int32 numcmodels;
		public static cmodel_t[] map_cmodels = new cmodel_t[Defines.MAX_MAP_MODELS];

		public static Int32 numbrushes;
		public static cbrush_t[] map_brushes = new cbrush_t[Defines.MAX_MAP_BRUSHES];

		public static Int32 numvisibility;
		public static Byte[] map_visibility = new Byte[Defines.MAX_MAP_VISIBILITY];
		public static qfiles.dvis_t map_vis = new dvis_t( ByteBuffer.Wrap( map_visibility ) );
		public static Int32 numentitychars;
		public static String map_entitystring;
		public static Int32 numareas = 1;
		public static carea_t[] map_areas = new carea_t[Defines.MAX_MAP_AREAS];

		public static Int32 numareaportals;
		public static qfiles.dareaportal_t[] map_areaportals = new qfiles.dareaportal_t[Defines.MAX_MAP_AREAPORTALS];

		static CM( )
		{
			for ( var n = 0; n < Defines.MAX_MAP_BRUSHSIDES; n++ )
				map_brushsides[n] = new cbrushside_t();

			for ( var n = 0; n < Defines.MAX_MAP_TEXINFO; n++ )
				map_surfaces[n] = new mapsurface_t();

			for ( var n = 0; n < Defines.MAX_MAP_PLANES + 6; n++ )
				map_planes[n] = new cplane_t();

			for ( var n = 0; n < Defines.MAX_MAP_NODES + 6; n++ )
				map_nodes[n] = new cnode_t();

			for ( var n = 0; n < Defines.MAX_MAP_LEAFS; n++ )
				map_leafs[n] = new cleaf_t();

			for ( var n = 0; n < Defines.MAX_MAP_MODELS; n++ )
				map_cmodels[n] = new cmodel_t();

			for ( var n = 0; n < Defines.MAX_MAP_BRUSHES; n++ )
				map_brushes[n] = new cbrush_t();

			for ( var n = 0; n < Defines.MAX_MAP_AREAS; n++ )
				map_areas[n] = new carea_t();

			for ( var n = 0; n < Defines.MAX_MAP_AREAPORTALS; n++ )
				map_areaportals[n] = new dareaportal_t();
		}

		public static Int32 numclusters = 1;
		public static mapsurface_t nullsurface = new mapsurface_t();
		public static Int32 floodvalid;
		public static Boolean[] portalopen = new Boolean[Defines.MAX_MAP_AREAPORTALS];
		public static cvar_t map_noareas;
		public static Byte[] cmod_base;
		public static Int32 checksum;
		public static Int32 last_checksum;
		public static cmodel_t CM_LoadMap( String name, Boolean clientload, Int32[] checksum )
		{
			Com.DPrintf( "CM_LoadMap(" + name + ")...\\n" );
			Byte[] buf;
			qfiles.dheader_t header;
			Int32 length;
			map_noareas = Cvar.Get( "map_noareas", "0", 0 );
			if ( map_name.Equals( name ) && ( clientload || 0 == Cvar.VariableValue( "flushmap" ) ) )
			{
				checksum[0] = last_checksum;
				if ( !clientload )
				{
					portalopen.Fill( false );
					FloodAreaConnections();
				}

				return map_cmodels[0];
			}

			numnodes = 0;
			numleafs = 0;
			numcmodels = 0;
			numvisibility = 0;
			numentitychars = 0;
			map_entitystring = "";
			map_name = "";
			if ( name == null || name.Length == 0 )
			{
				numleafs = 1;
				numclusters = 1;
				numareas = 1;
				checksum[0] = 0;
				return map_cmodels[0];
			}

			buf = FS.LoadFile( name );
			if ( buf == null )
				Com.Error( Defines.ERR_DROP, "Couldn't load " + name );
			length = buf.Length;
			ByteBuffer bbuf = ByteBuffer.Wrap( buf );
			last_checksum = MD4.Com_BlockChecksum( buf, length );
			checksum[0] = last_checksum;
			header = new dheader_t( bbuf.Slice() );
			if ( header.version != Defines.BSPVERSION )
				Com.Error( Defines.ERR_DROP, "CMod_LoadBrushModel: " + name + " has wrong version number (" + header.version + " should be " + Defines.BSPVERSION + ")" );
			cmod_base = buf;
			CMod_LoadSurfaces( header.lumps[Defines.LUMP_TEXINFO] );
			CMod_LoadLeafs( header.lumps[Defines.LUMP_LEAFS] );
			CMod_LoadLeafBrushes( header.lumps[Defines.LUMP_LEAFBRUSHES] );
			CMod_LoadPlanes( header.lumps[Defines.LUMP_PLANES] );
			CMod_LoadBrushes( header.lumps[Defines.LUMP_BRUSHES] );
			CMod_LoadBrushSides( header.lumps[Defines.LUMP_BRUSHSIDES] );
			CMod_LoadSubmodels( header.lumps[Defines.LUMP_MODELS] );
			CMod_LoadNodes( header.lumps[Defines.LUMP_NODES] );
			CMod_LoadAreas( header.lumps[Defines.LUMP_AREAS] );
			CMod_LoadAreaPortals( header.lumps[Defines.LUMP_AREAPORTALS] );
			CMod_LoadVisibility( header.lumps[Defines.LUMP_VISIBILITY] );
			CMod_LoadEntityString( header.lumps[Defines.LUMP_ENTITIES] );
			FS.FreeFile( buf );
			CM_InitBoxHull();
			portalopen.Fill( false );
			FloodAreaConnections();
			map_name = name;
			return map_cmodels[0];
		}

		public static void CMod_LoadSubmodels( lump_t l )
		{
			Com.DPrintf( "CMod_LoadSubmodels()\\n" );
			qfiles.dmodel_t in_renamed;
			cmodel_t out_renamed;
			Int32 i, j, count;
			if ( ( l.filelen % qfiles.dmodel_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "CMod_LoadBmodel: funny lump size" );
			count = l.filelen / qfiles.dmodel_t.SIZE;
			if ( count < 1 )
				Com.Error( Defines.ERR_DROP, "Map with no models" );
			if ( count > Defines.MAX_MAP_MODELS )
				Com.Error( Defines.ERR_DROP, "Map has too many models" );
			Com.DPrintf( " numcmodels=" + count + "\\n" );
			numcmodels = count;
			if ( debugloadmap )
			{
				Com.DPrintf( "submodles(headnode, <origin>, <mins>, <maxs>)\\n" );
			}

			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dmodel_t( ByteBuffer.Wrap( cmod_base, i * qfiles.dmodel_t.SIZE + l.fileofs, qfiles.dmodel_t.SIZE ) );
				out_renamed = map_cmodels[i];
				for ( j = 0; j < 3; j++ )
				{
					out_renamed.mins[j] = in_renamed.mins[j] - 1;
					out_renamed.maxs[j] = in_renamed.maxs[j] + 1;
					out_renamed.origin[j] = in_renamed.origin[j];
				}

				out_renamed.headnode = in_renamed.headnode;
				if ( debugloadmap )
				{
					Com.DPrintf( "|%6i|%8.2f|%8.2f|%8.2f|  %8.2f|%8.2f|%8.2f|   %8.2f|%8.2f|%8.2f|\\n", out_renamed.headnode, out_renamed.origin[0], out_renamed.origin[1], out_renamed.origin[2], out_renamed.mins[0], out_renamed.mins[1], out_renamed.mins[2], out_renamed.maxs[0], out_renamed.maxs[1], out_renamed.maxs[2] );
				}
			}
		}

		static Boolean debugloadmap = false;
		public static void CMod_LoadSurfaces( lump_t l )
		{
			Com.DPrintf( "CMod_LoadSurfaces()\\n" );
			texinfo_t in_renamed;
			mapsurface_t out_renamed;
			Int32 i, count;
			if ( ( l.filelen % texinfo_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size" );
			count = l.filelen / texinfo_t.SIZE;
			if ( count < 1 )
				Com.Error( Defines.ERR_DROP, "Map with no surfaces" );
			if ( count > Defines.MAX_MAP_TEXINFO )
				Com.Error( Defines.ERR_DROP, "Map has too many surfaces" );
			numtexinfo = count;
			Com.DPrintf( " numtexinfo=" + count + "\\n" );
			if ( debugloadmap )
				Com.DPrintf( "surfaces:\\n" );
			for ( i = 0; i < count; i++ )
			{
				out_renamed = map_surfaces[i] = new mapsurface_t();
				in_renamed = new texinfo_t( cmod_base, l.fileofs + i * texinfo_t.SIZE, texinfo_t.SIZE );
				out_renamed.c.name = in_renamed.texture;
				out_renamed.rname = in_renamed.texture;
				out_renamed.c.flags = in_renamed.flags;
				out_renamed.c.value = in_renamed.value;
				if ( debugloadmap )
				{
					Com.DPrintf( "|%20s|%20s|%6i|%6i|\\n", out_renamed.c.name, out_renamed.rname, out_renamed.c.value, out_renamed.c.flags );
				}
			}
		}

		public static void CMod_LoadNodes( lump_t l )
		{
			Com.DPrintf( "CMod_LoadNodes()\\n" );
			qfiles.dnode_t in_renamed;
			Int32 child;
			cnode_t out_renamed;
			Int32 i, j, count;
			if ( ( l.filelen % qfiles.dnode_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size:" + l.fileofs + "," + qfiles.dnode_t.SIZE );
			count = l.filelen / qfiles.dnode_t.SIZE;
			if ( count < 1 )
				Com.Error( Defines.ERR_DROP, "Map has no nodes" );
			if ( count > Defines.MAX_MAP_NODES )
				Com.Error( Defines.ERR_DROP, "Map has too many nodes" );
			numnodes = count;
			Com.DPrintf( " numnodes=" + count + "\\n" );
			if ( debugloadmap )
			{
				Com.DPrintf( "nodes(planenum, child[0], child[1])\\n" );
			}

			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dnode_t( ByteBuffer.Wrap( cmod_base, qfiles.dnode_t.SIZE * i + l.fileofs, qfiles.dnode_t.SIZE ) );
				out_renamed = map_nodes[i];
				out_renamed.plane = map_planes[in_renamed.planenum];
				for ( j = 0; j < 2; j++ )
				{
					child = in_renamed.children[j];
					out_renamed.children[j] = child;
				}

				if ( debugloadmap )
				{
					Com.DPrintf( "|%6i| %6i| %6i|\\n", in_renamed.planenum, out_renamed.children[0], out_renamed.children[1] );
				}
			}
		}

		public static void CMod_LoadBrushes( lump_t l )
		{
			Com.DPrintf( "CMod_LoadBrushes()\\n" );
			qfiles.dbrush_t in_renamed;
			cbrush_t out_renamed;
			Int32 i, count;
			if ( ( l.filelen % qfiles.dbrush_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size" );
			count = l.filelen / qfiles.dbrush_t.SIZE;
			if ( count > Defines.MAX_MAP_BRUSHES )
				Com.Error( Defines.ERR_DROP, "Map has too many brushes" );
			numbrushes = count;
			Com.DPrintf( " numbrushes=" + count + "\\n" );
			if ( debugloadmap )
			{
				Com.DPrintf( "brushes:(firstbrushside, numsides, contents)\\n" );
			}

			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dbrush_t( ByteBuffer.Wrap( cmod_base, i * qfiles.dbrush_t.SIZE + l.fileofs, qfiles.dbrush_t.SIZE ) );
				out_renamed = map_brushes[i];
				out_renamed.firstbrushside = in_renamed.firstside;
				out_renamed.numsides = in_renamed.numsides;
				out_renamed.contents = in_renamed.contents;
				if ( debugloadmap )
				{
					Com.DPrintf( "| %6i| %6i| %8X|\\n", out_renamed.firstbrushside, out_renamed.numsides, out_renamed.contents );
				}
			}
		}

		public static void CMod_LoadLeafs( lump_t l )
		{
			Com.DPrintf( "CMod_LoadLeafs()\\n" );
			Int32 i;
			cleaf_t out_renamed;
			qfiles.dleaf_t in_renamed;
			Int32 count;
			if ( ( l.filelen % qfiles.dleaf_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size" );
			count = l.filelen / qfiles.dleaf_t.SIZE;
			if ( count < 1 )
				Com.Error( Defines.ERR_DROP, "Map with no leafs" );
			if ( count > Defines.MAX_MAP_PLANES )
				Com.Error( Defines.ERR_DROP, "Map has too many planes" );
			Com.DPrintf( " numleafes=" + count + "\\n" );
			numleafs = count;
			numclusters = 0;
			if ( debugloadmap )
				Com.DPrintf( "cleaf-list:(contents, cluster, area, firstleafbrush, numleafbrushes)\\n" );
			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dleaf_t( cmod_base, i * qfiles.dleaf_t.SIZE + l.fileofs, qfiles.dleaf_t.SIZE );
				out_renamed = map_leafs[i];
				out_renamed.contents = in_renamed.contents;
				out_renamed.cluster = in_renamed.cluster;
				out_renamed.area = in_renamed.area;
				out_renamed.firstleafbrush = ( Int16 ) in_renamed.firstleafbrush;
				out_renamed.numleafbrushes = ( Int16 ) in_renamed.numleafbrushes;
				if ( out_renamed.cluster >= numclusters )
					numclusters = out_renamed.cluster + 1;
				if ( debugloadmap )
				{
					Com.DPrintf( "|%8x|%6i|%6i|%6i|\\n", out_renamed.contents, out_renamed.cluster, out_renamed.area, out_renamed.firstleafbrush, out_renamed.numleafbrushes );
				}
			}

			Com.DPrintf( " numclusters=" + numclusters + "\\n" );
			if ( map_leafs[0].contents != Defines.CONTENTS_SOLID )
				Com.Error( Defines.ERR_DROP, "Map leaf 0 is not CONTENTS_SOLID" );
			solidleaf = 0;
			emptyleaf = -1;
			for ( i = 1; i < numleafs; i++ )
			{
				if ( map_leafs[i].contents == 0 )
				{
					emptyleaf = i;
					break;
				}
			}

			if ( emptyleaf == -1 )
				Com.Error( Defines.ERR_DROP, "Map does not have an empty leaf" );
		}

		public static void CMod_LoadPlanes( lump_t l )
		{
			Com.DPrintf( "CMod_LoadPlanes()\\n" );
			Int32 i, j;
			cplane_t out_renamed;
			qfiles.dplane_t in_renamed;
			Int32 count;
			Int32 bits;
			if ( ( l.filelen % qfiles.dplane_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size" );
			count = l.filelen / qfiles.dplane_t.SIZE;
			if ( count < 1 )
				Com.Error( Defines.ERR_DROP, "Map with no planes" );
			if ( count > Defines.MAX_MAP_PLANES )
				Com.Error( Defines.ERR_DROP, "Map has too many planes" );
			Com.DPrintf( " numplanes=" + count + "\\n" );
			numplanes = count;
			if ( debugloadmap )
			{
				Com.DPrintf( "cplanes(normal[0],normal[1],normal[2], dist, type, signbits)\\n" );
			}

			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dplane_t( ByteBuffer.Wrap( cmod_base, i * qfiles.dplane_t.SIZE + l.fileofs, qfiles.dplane_t.SIZE ) );
				out_renamed = map_planes[i];
				bits = 0;
				for ( j = 0; j < 3; j++ )
				{
					out_renamed.normal[j] = in_renamed.normal[j];
					if ( out_renamed.normal[j] < 0 )
						bits |= 1 << j;
				}

				out_renamed.dist = in_renamed.dist;
				out_renamed.type = ( Byte ) in_renamed.type;
				out_renamed.signbits = ( Byte ) bits;
				if ( debugloadmap )
				{
					Com.DPrintf( "|%6.2f|%6.2f|%6.2f| %10.2f|%3i| %1i|\\n", out_renamed.normal[0], out_renamed.normal[1], out_renamed.normal[2], out_renamed.dist, out_renamed.type, out_renamed.signbits );
				}
			}
		}

		public static void CMod_LoadLeafBrushes( lump_t l )
		{
			Com.DPrintf( "CMod_LoadLeafBrushes()\\n" );
			if ( ( l.filelen % 2 ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size" );
			var count = l.filelen / 2;
			Com.DPrintf( " numbrushes=" + count + "\\n" );
			if ( count < 1 )
				Com.Error( Defines.ERR_DROP, "Map with no planes" );
			if ( count > Defines.MAX_MAP_LEAFBRUSHES )
				Com.Error( Defines.ERR_DROP, "Map has too many leafbrushes" );
			Int32[] out_renamed = map_leafbrushes;
			numleafbrushes = count;
			ByteBuffer bb = ByteBuffer.Wrap( cmod_base, l.fileofs, count * 2 );
			bb.Order = ByteOrder.LittleEndian;
			if ( debugloadmap )
			{
				Com.DPrintf( "map_brushes:\\n" );
			}

			for ( var i = 0; i < count; i++ )
			{
				out_renamed[i] = bb.GetInt16();
				if ( debugloadmap )
				{
					Com.DPrintf( "|%6i|%6i|\\n", i, out_renamed[i] );
				}
			}
		}

		public static void CMod_LoadBrushSides( lump_t l )
		{
			Com.DPrintf( "CMod_LoadBrushSides()\\n" );
			Int32 i, j;
			cbrushside_t out_renamed;
			qfiles.dbrushside_t in_renamed;
			Int32 count;
			Int32 num;
			if ( ( l.filelen % qfiles.dbrushside_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size" );
			count = l.filelen / qfiles.dbrushside_t.SIZE;
			if ( count > Defines.MAX_MAP_BRUSHSIDES )
				Com.Error( Defines.ERR_DROP, "Map has too many planes" );
			numbrushsides = count;
			Com.DPrintf( " numbrushsides=" + count + "\\n" );
			if ( debugloadmap )
			{
				Com.DPrintf( "brushside(planenum, surfacenum):\\n" );
			}

			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dbrushside_t( ByteBuffer.Wrap( cmod_base, i * qfiles.dbrushside_t.SIZE + l.fileofs, qfiles.dbrushside_t.SIZE ) );
				out_renamed = map_brushsides[i];
				num = in_renamed.planenum;
				out_renamed.plane = map_planes[num];
				j = in_renamed.texinfo;
				if ( j >= numtexinfo )
					Com.Error( Defines.ERR_DROP, "Bad brushside texinfo" );
				if ( j == -1 )
					out_renamed.surface = new mapsurface_t();
				else
					out_renamed.surface = map_surfaces[j];
				if ( debugloadmap )
				{
					Com.DPrintf( "| %6i| %6i|\\n", num, j );
				}
			}
		}

		public static void CMod_LoadAreas( lump_t l )
		{
			Com.DPrintf( "CMod_LoadAreas()\\n" );
			Int32 i;
			carea_t out_renamed;
			qfiles.darea_t in_renamed;
			Int32 count;
			if ( ( l.filelen % qfiles.darea_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size" );
			count = l.filelen / qfiles.darea_t.SIZE;
			if ( count > Defines.MAX_MAP_AREAS )
				Com.Error( Defines.ERR_DROP, "Map has too many areas" );
			Com.DPrintf( " numareas=" + count + "\\n" );
			numareas = count;
			if ( debugloadmap )
			{
				Com.DPrintf( "areas(numportals, firstportal)\\n" );
			}

			for ( i = 0; i < count; i++ )
			{
				in_renamed = new darea_t( ByteBuffer.Wrap( cmod_base, i * qfiles.darea_t.SIZE + l.fileofs, qfiles.darea_t.SIZE ) );
				out_renamed = map_areas[i];
				out_renamed.numareaportals = in_renamed.numareaportals;
				out_renamed.firstareaportal = in_renamed.firstareaportal;
				out_renamed.floodvalid = 0;
				out_renamed.floodnum = 0;
				if ( debugloadmap )
				{
					Com.DPrintf( "| %6i| %6i|\\n", out_renamed.numareaportals, out_renamed.firstareaportal );
				}
			}
		}

		public static void CMod_LoadAreaPortals( lump_t l )
		{
			Com.DPrintf( "CMod_LoadAreaPortals()\\n" );
			Int32 i;
			qfiles.dareaportal_t out_renamed;
			qfiles.dareaportal_t in_renamed;
			Int32 count;
			if ( ( l.filelen % qfiles.dareaportal_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size" );
			count = l.filelen / qfiles.dareaportal_t.SIZE;
			if ( count > Defines.MAX_MAP_AREAS )
				Com.Error( Defines.ERR_DROP, "Map has too many areas" );
			numareaportals = count;
			Com.DPrintf( " numareaportals=" + count + "\\n" );
			if ( debugloadmap )
			{
				Com.DPrintf( "areaportals(portalnum, otherarea)\\n" );
			}

			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dareaportal_t( ByteBuffer.Wrap( cmod_base, i * qfiles.dareaportal_t.SIZE + l.fileofs, qfiles.dareaportal_t.SIZE ) );
				out_renamed = map_areaportals[i];
				out_renamed.portalnum = in_renamed.portalnum;
				out_renamed.otherarea = in_renamed.otherarea;
				if ( debugloadmap )
				{
					Com.DPrintf( "|%6i|%6i|\\n", out_renamed.portalnum, out_renamed.otherarea );
				}
			}
		}

		public static void CMod_LoadVisibility( lump_t l )
		{
			Com.DPrintf( "CMod_LoadVisibility()\\n" );
			numvisibility = l.filelen;
			Com.DPrintf( " numvisibility=" + numvisibility + "\\n" );
			if ( l.filelen > Defines.MAX_MAP_VISIBILITY )
				Com.Error( Defines.ERR_DROP, "Map has too large visibility lump" );
			System.Array.Copy( cmod_base, l.fileofs, map_visibility, 0, l.filelen );
			ByteBuffer bb = ByteBuffer.Wrap( map_visibility, 0, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			map_vis = new dvis_t( bb );
		}

		public static void CMod_LoadEntityString( lump_t l )
		{
			Com.DPrintf( "CMod_LoadEntityString()\\n" );
			numentitychars = l.filelen;
			if ( l.filelen > Defines.MAX_MAP_ENTSTRING )
				Com.Error( Defines.ERR_DROP, "Map has too large entity lump" );
			var x = 0;
			map_entitystring = Encoding.ASCII.GetString( cmod_base, l.fileofs, x ).Trim();
			Com.Dprintln( "entitystring=" + map_entitystring.Length + " bytes, [" + map_entitystring.Substring( 0, Math.Min( map_entitystring.Length, 15 ) ) + "...]" );
		}

		public static cmodel_t InlineModel( String name )
		{
			if ( name == null || name[0] != '*' )
				Com.Error( Defines.ERR_DROP, "CM_InlineModel: bad name" );
			var num = Lib.Atoi( name.Substring( 1 ) );
			if ( num < 1 || num >= numcmodels )
				Com.Error( Defines.ERR_DROP, "CM_InlineModel: bad number" );
			return map_cmodels[num];
		}

		public static Int32 CM_NumClusters( )
		{
			return numclusters;
		}

		public static Int32 CM_NumInlineModels( )
		{
			return numcmodels;
		}

		public static String CM_EntityString( )
		{
			return map_entitystring;
		}

		public static Int32 CM_LeafContents( Int32 leafnum )
		{
			if ( leafnum < 0 || leafnum >= numleafs )
				Com.Error( Defines.ERR_DROP, "CM_LeafContents: bad number" );
			return map_leafs[leafnum].contents;
		}

		public static Int32 CM_LeafCluster( Int32 leafnum )
		{
			if ( leafnum < 0 || leafnum >= numleafs )
				Com.Error( Defines.ERR_DROP, "CM_LeafCluster: bad number" );
			return map_leafs[leafnum].cluster;
		}

		public static Int32 CM_LeafArea( Int32 leafnum )
		{
			if ( leafnum < 0 || leafnum >= numleafs )
				Com.Error( Defines.ERR_DROP, "CM_LeafArea: bad number" );
			return map_leafs[leafnum].area;
		}

		static cplane_t[] box_planes;
		static Int32 box_headnode;
		static cbrush_t box_brush;
		static cleaf_t box_leaf;
		public static void CM_InitBoxHull( )
		{
			box_headnode = numnodes;
			box_planes = new cplane_t[] { map_planes[numplanes], map_planes[numplanes + 1], map_planes[numplanes + 2], map_planes[numplanes + 3], map_planes[numplanes + 4], map_planes[numplanes + 5], map_planes[numplanes + 6], map_planes[numplanes + 7], map_planes[numplanes + 8], map_planes[numplanes + 9], map_planes[numplanes + 10], map_planes[numplanes + 11], map_planes[numplanes + 12] };
			if ( numnodes + 6 > Defines.MAX_MAP_NODES || numbrushes + 1 > Defines.MAX_MAP_BRUSHES || numleafbrushes + 1 > Defines.MAX_MAP_LEAFBRUSHES || numbrushsides + 6 > Defines.MAX_MAP_BRUSHSIDES || numplanes + 12 > Defines.MAX_MAP_PLANES )
				Com.Error( Defines.ERR_DROP, "Not enough room for box tree" );
			box_brush = map_brushes[numbrushes];
			box_brush.numsides = 6;
			box_brush.firstbrushside = numbrushsides;
			box_brush.contents = Defines.CONTENTS_MONSTER;
			box_leaf = map_leafs[numleafs];
			box_leaf.contents = Defines.CONTENTS_MONSTER;
			box_leaf.firstleafbrush = ( Int16 ) numleafbrushes;
			box_leaf.numleafbrushes = 1;
			map_leafbrushes[numleafbrushes] = numbrushes;
			Int32 side;
			cnode_t c;
			cplane_t p;
			cbrushside_t s;
			for ( var i = 0; i < 6; i++ )
			{
				side = i & 1;
				s = map_brushsides[numbrushsides + i];
				s.plane = map_planes[( numplanes + i * 2 + side )];
				s.surface = nullsurface;
				c = map_nodes[box_headnode + i];
				c.plane = map_planes[( numplanes + i * 2 )];
				c.children[side] = -1 - emptyleaf;
				if ( i != 5 )
					c.children[side ^ 1] = box_headnode + i + 1;
				else
					c.children[side ^ 1] = -1 - numleafs;
				p = box_planes[i * 2];
				p.type = ( Byte ) ( i >> 1 );
				p.signbits = 0;
				Math3D.VectorClear( p.normal );
				p.normal[i >> 1] = 1;
				p = box_planes[i * 2 + 1];
				p.type = ( Byte ) ( 3 + ( i >> 1 ) );
				p.signbits = 0;
				Math3D.VectorClear( p.normal );
				p.normal[i >> 1] = -1;
			}
		}

		public static Int32 HeadnodeForBox( Single[] mins, Single[] maxs )
		{
			box_planes[0].dist = maxs[0];
			box_planes[1].dist = -maxs[0];
			box_planes[2].dist = mins[0];
			box_planes[3].dist = -mins[0];
			box_planes[4].dist = maxs[1];
			box_planes[5].dist = -maxs[1];
			box_planes[6].dist = mins[1];
			box_planes[7].dist = -mins[1];
			box_planes[8].dist = maxs[2];
			box_planes[9].dist = -maxs[2];
			box_planes[10].dist = mins[2];
			box_planes[11].dist = -mins[2];
			return box_headnode;
		}

		private static Int32 CM_PointLeafnum_r( Single[] p, Int32 num )
		{
			Single d;
			cnode_t node;
			cplane_t plane;
			while ( num >= 0 )
			{
				node = map_nodes[num];
				plane = node.plane;
				if ( plane.type < 3 )
					d = p[plane.type] - plane.dist;
				else
					d = Math3D.DotProduct( plane.normal, p ) - plane.dist;
				if ( d < 0 )
					num = node.children[1];
				else
					num = node.children[0];
			}

			Globals.c_pointcontents++;
			return -1 - num;
		}

		public static Int32 CM_PointLeafnum( Single[] p )
		{
			if ( numplanes == 0 )
				return 0;
			return CM_PointLeafnum_r( p, 0 );
		}

		private static Int32 leaf_count, leaf_maxcount;
		private static Int32[] leaf_list;
		private static Single[] leaf_mins, leaf_maxs;
		private static Int32 leaf_topnode;
		private static void CM_BoxLeafnums_r( Int32 nodenum )
		{
			cplane_t plane;
			cnode_t node;
			Int32 s;
			while ( true )
			{
				if ( nodenum < 0 )
				{
					if ( leaf_count >= leaf_maxcount )
					{
						Com.DPrintf( "CM_BoxLeafnums_r: overflow\\n" );
						return;
					}

					leaf_list[leaf_count++] = -1 - nodenum;
					return;
				}

				node = map_nodes[nodenum];
				plane = node.plane;
				s = Math3D.BoxOnPlaneSide( leaf_mins, leaf_maxs, plane );
				if ( s == 1 )
					nodenum = node.children[0];
				else if ( s == 2 )
					nodenum = node.children[1];
				else
				{
					if ( leaf_topnode == -1 )
						leaf_topnode = nodenum;
					CM_BoxLeafnums_r( node.children[0] );
					nodenum = node.children[1];
				}
			}
		}

		private static Int32 CM_BoxLeafnums_headnode( Single[] mins, Single[] maxs, Int32[] list, Int32 listsize, Int32 headnode, Int32[] topnode )
		{
			leaf_list = list;
			leaf_count = 0;
			leaf_maxcount = listsize;
			leaf_mins = mins;
			leaf_maxs = maxs;
			leaf_topnode = -1;
			CM_BoxLeafnums_r( headnode );
			if ( topnode != null )
				topnode[0] = leaf_topnode;
			return leaf_count;
		}

		public static Int32 CM_BoxLeafnums( Single[] mins, Single[] maxs, Int32[] list, Int32 listsize, Int32[] topnode )
		{
			return CM_BoxLeafnums_headnode( mins, maxs, list, listsize, map_cmodels[0].headnode, topnode );
		}

		public static Int32 PointContents( Single[] p, Int32 headnode )
		{
			Int32 l;
			if ( numnodes == 0 )
				return 0;
			l = CM_PointLeafnum_r( p, headnode );
			return map_leafs[l].contents;
		}

		public static Int32 TransformedPointContents( Single[] p, Int32 headnode, Single[] origin, Single[] angles )
		{
			Single[] p_l = new Single[] { 0, 0, 0 };
			Single[] temp = new Single[] { 0, 0, 0 };
			Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 }, up = new Single[] { 0, 0, 0 };
			Int32 l;
			Math3D.VectorSubtract( p, origin, p_l );
			if ( headnode != box_headnode && ( angles[0] != 0 || angles[1] != 0 || angles[2] != 0 ) )
			{
				Math3D.AngleVectors( angles, forward, right, up );
				Math3D.VectorCopy( p_l, temp );
				p_l[0] = Math3D.DotProduct( temp, forward );
				p_l[1] = -Math3D.DotProduct( temp, right );
				p_l[2] = Math3D.DotProduct( temp, up );
			}

			l = CM_PointLeafnum_r( p_l, headnode );
			return map_leafs[l].contents;
		}

		private static readonly Single DIST_EPSILON = 0.03125F;
		private static Single[] trace_start = new Single[] { 0, 0, 0 }, trace_end = new Single[] { 0, 0, 0 };
		private static Single[] trace_mins = new Single[] { 0, 0, 0 }, trace_maxs = new Single[] { 0, 0, 0 };
		private static Single[] trace_extents = new Single[] { 0, 0, 0 };
		private static trace_t trace_trace = new trace_t();
		private static Int32 trace_contents;
		private static Boolean trace_ispoint;
		public static void CM_ClipBoxToBrush( Single[] mins, Single[] maxs, Single[] p1, Single[] p2, trace_t trace, cbrush_t brush )
		{
			Int32 i, j;
			cplane_t plane, clipplane;
			Single dist;
			Single enterfrac, leavefrac;
			Single[] ofs = new Single[] { 0, 0, 0 };
			Single d1, d2;
			Boolean getout, startout;
			Single f;
			cbrushside_t side, leadside;
			enterfrac = -1;
			leavefrac = 1;
			clipplane = null;
			if ( brush.numsides == 0 )
				return;
			Globals.c_brush_traces++;
			getout = false;
			startout = false;
			leadside = null;
			for ( i = 0; i < brush.numsides; i++ )
			{
				side = map_brushsides[brush.firstbrushside + i];
				plane = side.plane;
				if ( !trace_ispoint )
				{
					for ( j = 0; j < 3; j++ )
					{
						if ( plane.normal[j] < 0 )
							ofs[j] = maxs[j];
						else
							ofs[j] = mins[j];
					}

					dist = Math3D.DotProduct( ofs, plane.normal );
					dist = plane.dist - dist;
				}
				else
				{
					dist = plane.dist;
				}

				d1 = Math3D.DotProduct( p1, plane.normal ) - dist;
				d2 = Math3D.DotProduct( p2, plane.normal ) - dist;
				if ( d2 > 0 )
					getout = true;
				if ( d1 > 0 )
					startout = true;
				if ( d1 > 0 && d2 >= d1 )
					return;
				if ( d1 <= 0 && d2 <= 0 )
					continue;
				if ( d1 > d2 )
				{
					f = ( d1 - DIST_EPSILON ) / ( d1 - d2 );
					if ( f > enterfrac )
					{
						enterfrac = f;
						clipplane = plane;
						leadside = side;
					}
				}
				else
				{
					f = ( d1 + DIST_EPSILON ) / ( d1 - d2 );
					if ( f < leavefrac )
						leavefrac = f;
				}
			}

			if ( !startout )
			{
				trace.startsolid = true;
				if ( !getout )
					trace.allsolid = true;
				return;
			}

			if ( enterfrac < leavefrac )
			{
				if ( enterfrac > -1 && enterfrac < trace.fraction )
				{
					if ( enterfrac < 0 )
						enterfrac = 0;
					trace.fraction = enterfrac;
					trace.plane.Set( clipplane );
					trace.surface = leadside.surface.c;
					trace.contents = brush.contents;
				}
			}
		}

		public static void CM_TestBoxInBrush( Single[] mins, Single[] maxs, Single[] p1, trace_t trace, cbrush_t brush )
		{
			Int32 i, j;
			cplane_t plane;
			Single dist;
			Single[] ofs = new Single[] { 0, 0, 0 };
			Single d1;
			cbrushside_t side;
			if ( brush.numsides == 0 )
				return;
			for ( i = 0; i < brush.numsides; i++ )
			{
				side = map_brushsides[brush.firstbrushside + i];
				plane = side.plane;
				for ( j = 0; j < 3; j++ )
				{
					if ( plane.normal[j] < 0 )
						ofs[j] = maxs[j];
					else
						ofs[j] = mins[j];
				}

				dist = Math3D.DotProduct( ofs, plane.normal );
				dist = plane.dist - dist;
				d1 = Math3D.DotProduct( p1, plane.normal ) - dist;
				if ( d1 > 0 )
					return;
			}

			trace.startsolid = trace.allsolid = true;
			trace.fraction = 0;
			trace.contents = brush.contents;
		}

		public static void CM_TraceToLeaf( Int32 leafnum )
		{
			Int32 k;
			Int32 brushnum;
			cleaf_t leaf;
			cbrush_t b;
			leaf = map_leafs[leafnum];
			if ( 0 == ( leaf.contents & trace_contents ) )
				return;
			for ( k = 0; k < leaf.numleafbrushes; k++ )
			{
				brushnum = map_leafbrushes[leaf.firstleafbrush + k];
				b = map_brushes[brushnum];
				if ( b.checkcount == checkcount )
					continue;
				b.checkcount = checkcount;
				if ( 0 == ( b.contents & trace_contents ) )
					continue;
				CM_ClipBoxToBrush( trace_mins, trace_maxs, trace_start, trace_end, trace_trace, b );
				if ( 0 == trace_trace.fraction )
					return;
			}
		}

		public static void CM_TestInLeaf( Int32 leafnum )
		{
			Int32 k;
			Int32 brushnum;
			cleaf_t leaf;
			cbrush_t b;
			leaf = map_leafs[leafnum];
			if ( 0 == ( leaf.contents & trace_contents ) )
				return;
			for ( k = 0; k < leaf.numleafbrushes; k++ )
			{
				brushnum = map_leafbrushes[leaf.firstleafbrush + k];
				b = map_brushes[brushnum];
				if ( b.checkcount == checkcount )
					continue;
				b.checkcount = checkcount;
				if ( 0 == ( b.contents & trace_contents ) )
					continue;
				CM_TestBoxInBrush( trace_mins, trace_maxs, trace_start, trace_trace, b );
				if ( 0 == trace_trace.fraction )
					return;
			}
		}

		public static void CM_RecursiveHullCheck( Int32 num, Single p1f, Single p2f, Single[] p1, Single[] p2 )
		{
			cnode_t node;
			cplane_t plane;
			Single t1, t2, offset;
			Single frac, frac2;
			Single idist;
			Int32 i;
			Int32 side;
			Single midf;
			if ( trace_trace.fraction <= p1f )
				return;
			if ( num < 0 )
			{
				CM_TraceToLeaf( -1 - num );
				return;
			}

			node = map_nodes[num];
			plane = node.plane;
			if ( plane.type < 3 )
			{
				t1 = p1[plane.type] - plane.dist;
				t2 = p2[plane.type] - plane.dist;
				offset = trace_extents[plane.type];
			}
			else
			{
				t1 = Math3D.DotProduct( plane.normal, p1 ) - plane.dist;
				t2 = Math3D.DotProduct( plane.normal, p2 ) - plane.dist;
				if ( trace_ispoint )
					offset = 0;
				else
					offset = Math.Abs( trace_extents[0] * plane.normal[0] ) + Math.Abs( trace_extents[1] * plane.normal[1] ) + Math.Abs( trace_extents[2] * plane.normal[2] );
			}

			if ( t1 >= offset && t2 >= offset )
			{
				CM_RecursiveHullCheck( node.children[0], p1f, p2f, p1, p2 );
				return;
			}

			if ( t1 < -offset && t2 < -offset )
			{
				CM_RecursiveHullCheck( node.children[1], p1f, p2f, p1, p2 );
				return;
			}

			if ( t1 < t2 )
			{
				idist = 1F / ( t1 - t2 );
				side = 1;
				frac2 = ( t1 + offset + DIST_EPSILON ) * idist;
				frac = ( t1 - offset + DIST_EPSILON ) * idist;
			}
			else if ( t1 > t2 )
			{
				idist = 1F / ( t1 - t2 );
				side = 0;
				frac2 = ( t1 - offset - DIST_EPSILON ) * idist;
				frac = ( t1 + offset + DIST_EPSILON ) * idist;
			}
			else
			{
				side = 0;
				frac = 1;
				frac2 = 0;
			}

			if ( frac < 0 )
				frac = 0;
			if ( frac > 1 )
				frac = 1;
			midf = p1f + ( p2f - p1f ) * frac;
			Single[] mid = Vec3Cache.Get();
			for ( i = 0; i < 3; i++ )
				mid[i] = p1[i] + frac * ( p2[i] - p1[i] );
			CM_RecursiveHullCheck( node.children[side], p1f, midf, p1, mid );
			if ( frac2 < 0 )
				frac2 = 0;
			if ( frac2 > 1 )
				frac2 = 1;
			midf = p1f + ( p2f - p1f ) * frac2;
			for ( i = 0; i < 3; i++ )
				mid[i] = p1[i] + frac2 * ( p2[i] - p1[i] );
			CM_RecursiveHullCheck( node.children[side ^ 1], midf, p2f, mid, p2 );
			Vec3Cache.Release();
		}

		public static trace_t BoxTrace( Single[] start, Single[] end, Single[] mins, Single[] maxs, Int32 headnode, Int32 brushmask )
		{
			checkcount++;
			Globals.c_traces++;
			trace_trace = new trace_t();
			trace_trace.fraction = 1;
			trace_trace.surface = nullsurface.c;
			if ( numnodes == 0 )
			{
				return trace_trace;
			}

			trace_contents = brushmask;
			Math3D.VectorCopy( start, trace_start );
			Math3D.VectorCopy( end, trace_end );
			Math3D.VectorCopy( mins, trace_mins );
			Math3D.VectorCopy( maxs, trace_maxs );
			if ( start[0] == end[0] && start[1] == end[1] && start[2] == end[2] )
			{
				Int32[] leafs = new Int32[1024];
				Int32 i, numleafs;
				Single[] c1 = new Single[] { 0, 0, 0 }, c2 = new Single[] { 0, 0, 0 };
				var topnode = 0;
				Math3D.VectorAdd( start, mins, c1 );
				Math3D.VectorAdd( start, maxs, c2 );
				for ( i = 0; i < 3; i++ )
				{
					c1[i] -= 1;
					c2[i] += 1;
				}

				Int32[] tn = new[] { topnode };
				numleafs = CM_BoxLeafnums_headnode( c1, c2, leafs, 1024, headnode, tn );
				topnode = tn[0];
				for ( i = 0; i < numleafs; i++ )
				{
					CM_TestInLeaf( leafs[i] );
					if ( trace_trace.allsolid )
						break;
				}

				Math3D.VectorCopy( start, trace_trace.endpos );
				return trace_trace;
			}

			if ( mins[0] == 0 && mins[1] == 0 && mins[2] == 0 && maxs[0] == 0 && maxs[1] == 0 && maxs[2] == 0 )
			{
				trace_ispoint = true;
				Math3D.VectorClear( trace_extents );
			}
			else
			{
				trace_ispoint = false;
				trace_extents[0] = -mins[0] > maxs[0] ? -mins[0] : maxs[0];
				trace_extents[1] = -mins[1] > maxs[1] ? -mins[1] : maxs[1];
				trace_extents[2] = -mins[2] > maxs[2] ? -mins[2] : maxs[2];
			}

			CM_RecursiveHullCheck( headnode, 0, 1, start, end );
			if ( trace_trace.fraction == 1 )
			{
				Math3D.VectorCopy( end, trace_trace.endpos );
			}
			else
			{
				for ( var i = 0; i < 3; i++ )
					trace_trace.endpos[i] = start[i] + trace_trace.fraction * ( end[i] - start[i] );
			}

			return trace_trace;
		}

		public static trace_t TransformedBoxTrace( Single[] start, Single[] end, Single[] mins, Single[] maxs, Int32 headnode, Int32 brushmask, Single[] origin, Single[] angles )
		{
			trace_t trace;
			Single[] start_l = new Single[] { 0, 0, 0 }, end_l = new Single[] { 0, 0, 0 };
			Single[] a = new Single[] { 0, 0, 0 };
			Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 }, up = new Single[] { 0, 0, 0 };
			Single[] temp = new Single[] { 0, 0, 0 };
			Boolean rotated;
			Math3D.VectorSubtract( start, origin, start_l );
			Math3D.VectorSubtract( end, origin, end_l );
			if ( headnode != box_headnode && ( angles[0] != 0 || angles[1] != 0 || angles[2] != 0 ) )
				rotated = true;
			else
				rotated = false;
			if ( rotated )
			{
				Math3D.AngleVectors( angles, forward, right, up );
				Math3D.VectorCopy( start_l, temp );
				start_l[0] = Math3D.DotProduct( temp, forward );
				start_l[1] = -Math3D.DotProduct( temp, right );
				start_l[2] = Math3D.DotProduct( temp, up );
				Math3D.VectorCopy( end_l, temp );
				end_l[0] = Math3D.DotProduct( temp, forward );
				end_l[1] = -Math3D.DotProduct( temp, right );
				end_l[2] = Math3D.DotProduct( temp, up );
			}

			trace = BoxTrace( start_l, end_l, mins, maxs, headnode, brushmask );
			if ( rotated && trace.fraction != 1 )
			{
				Math3D.VectorNegate( angles, a );
				Math3D.AngleVectors( a, forward, right, up );
				Math3D.VectorCopy( trace.plane.normal, temp );
				trace.plane.normal[0] = Math3D.DotProduct( temp, forward );
				trace.plane.normal[1] = -Math3D.DotProduct( temp, right );
				trace.plane.normal[2] = Math3D.DotProduct( temp, up );
			}

			trace.endpos[0] = start[0] + trace.fraction * ( end[0] - start[0] );
			trace.endpos[1] = start[1] + trace.fraction * ( end[1] - start[1] );
			trace.endpos[2] = start[2] + trace.fraction * ( end[2] - start[2] );
			return trace;
		}

		public static void CM_DecompressVis( Byte[] in_renamed, Int32 offset, Byte[] out_renamed )
		{
			Int32 c;
			Int32 row;
			row = ( numclusters + 7 ) >> 3;
			var outp = 0;
			var inp = offset;
			if ( in_renamed == null || numvisibility == 0 )
			{
				while ( row != 0 )
				{
					out_renamed[outp++] = ( Byte ) 0xFF;
					row--;
				}

				return;
			}

			do
			{
				if ( in_renamed[inp] != 0 )
				{
					out_renamed[outp++] = in_renamed[inp++];
					continue;
				}

				c = in_renamed[inp + 1] & 0xFF;
				inp += 2;
				if ( outp + c > row )
				{
					c = row - ( outp );
					Com.DPrintf( "warning: Vis decompression overrun\\n" );
				}

				while ( c != 0 )
				{
					out_renamed[outp++] = 0;
					c--;
				}
			}
			while ( outp < row );
		}

		public static Byte[] pvsrow = new Byte[Defines.MAX_MAP_LEAFS / 8];
		public static Byte[] phsrow = new Byte[Defines.MAX_MAP_LEAFS / 8];
		public static Byte[] CM_ClusterPVS( Int32 cluster )
		{
			if ( cluster == -1 )
				Lib.Fill( pvsrow, 0, ( numclusters + 7 ) >> 3, ( Byte ) 0 );
			else
				CM_DecompressVis( map_visibility, map_vis.bitofs[cluster][Defines.DVIS_PVS], pvsrow );
			return pvsrow;
		}

		public static Byte[] CM_ClusterPHS( Int32 cluster )
		{
			if ( cluster == -1 )
				Lib.Fill( phsrow, 0, ( numclusters + 7 ) >> 3, ( Byte ) 0 );
			else
				CM_DecompressVis( map_visibility, map_vis.bitofs[cluster][Defines.DVIS_PHS], phsrow );
			return phsrow;
		}

		public static void FloodArea_r( carea_t area, Int32 floodnum )
		{
			Int32 i;
			qfiles.dareaportal_t p;
			if ( area.floodvalid == floodvalid )
			{
				if ( area.floodnum == floodnum )
					return;
				Com.Error( Defines.ERR_DROP, "FloodArea_r: reflooded" );
			}

			area.floodnum = floodnum;
			area.floodvalid = floodvalid;
			for ( i = 0; i < area.numareaportals; i++ )
			{
				p = map_areaportals[area.firstareaportal + i];
				if ( portalopen[p.portalnum] )
					FloodArea_r( map_areas[p.otherarea], floodnum );
			}
		}

		public static void FloodAreaConnections( )
		{
			Com.DPrintf( "FloodAreaConnections...\\n" );
			Int32 i;
			carea_t area;
			Int32 floodnum;
			floodvalid++;
			floodnum = 0;
			for ( i = 1; i < numareas; i++ )
			{
				area = map_areas[i];
				if ( area.floodvalid == floodvalid )
					continue;
				floodnum++;
				FloodArea_r( area, floodnum );
			}
		}

		public static void CM_SetAreaPortalState( Int32 portalnum, Boolean open )
		{
			if ( portalnum > numareaportals )
				Com.Error( Defines.ERR_DROP, "areaportal > numareaportals" );
			portalopen[portalnum] = open;
			FloodAreaConnections();
		}

		public static Boolean CM_AreasConnected( Int32 area1, Int32 area2 )
		{
			if ( map_noareas.value != 0 )
				return true;
			if ( area1 > numareas || area2 > numareas )
				Com.Error( Defines.ERR_DROP, "area > numareas" );
			if ( map_areas[area1].floodnum == map_areas[area2].floodnum )
				return true;
			return false;
		}

		public static Int32 CM_WriteAreaBits( Byte[] buffer, Int32 area )
		{
			Int32 i;
			Int32 floodnum;
			Int32 bytes;
			bytes = ( numareas + 7 ) >> 3;
			if ( map_noareas.value != 0 )
			{
				Lib.Fill( buffer, 0, bytes, ( Byte ) 255 );
			}
			else
			{
				Lib.Fill( buffer, 0, bytes, ( Byte ) 0 );
				floodnum = map_areas[area].floodnum;
				for ( i = 0; i < numareas; i++ )
				{
					if ( map_areas[i].floodnum == floodnum || area == 0 )
						buffer[i >> 3] = ( Byte ) ( buffer[i >> 3] | 1 << ( i & 7 ) );
				}
			}

			return bytes;
		}

		public static void CM_WritePortalState( QuakeFile os )
		{
			try
			{
				for ( var n = 0; n < portalopen.Length; n++ )
					if ( portalopen[n] )
						os.Write( 1 );
					else
						os.Write( 0 );
			}
			catch ( Exception e )
			{
				Com.Printf( "ERROR:" + e );
				e.PrintStackTrace();
			}
		}

		public static void CM_ReadPortalState( QuakeFile f )
		{
			var len = portalopen.Length * 4;
			Byte[] buf = f.ReadBytes( len );
			ByteBuffer bb = ByteBuffer.Wrap( buf );
			Int32Buffer ib = bb.AsInt32Buffer();
			for ( var n = 0; n < portalopen.Length; n++ )
				portalopen[n] = ib.Get() != 0;
			FloodAreaConnections();
		}

		public static Boolean CM_HeadnodeVisible( Int32 nodenum, Byte[] visbits )
		{
			if ( nodenum < 0 )
			{
				var leafnum = -1 - nodenum;
				var cluster = map_leafs[leafnum].cluster;
				if ( cluster == -1 )
					return false;
				if ( 0 != ( visbits[cluster >> 3] & ( 1 << ( cluster & 7 ) ) ) )
					return true;
				return false;
			}

			cnode_t node = map_nodes[nodenum];
			if ( CM_HeadnodeVisible( node.children[0], visbits ) )
				return true;
			return CM_HeadnodeVisible( node.children[1], visbits );
		}
	}
}
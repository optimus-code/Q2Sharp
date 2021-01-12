using J2N.IO;
using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Text;
using static Q2Sharp.Qcommon.qfiles;

namespace Q2Sharp.Render.Basic
{
	public abstract class Model : Surf
	{
		public model_t loadmodel;
		public Int32 modfilelen;
		public Byte[] mod_novis = new Byte[Defines.MAX_MAP_LEAFS / 8];
		public static readonly Int32 MAX_MOD_KNOWN = 512;
		public model_t[] mod_known = new model_t[MAX_MOD_KNOWN];
		public Int32 mod_numknown;
		public model_t[] mod_inline = new model_t[MAX_MOD_KNOWN];
		public abstract void GL_SubdivideSurface( msurface_t surface );
		public override mleaf_t Mod_PointInLeaf( Single[] p, model_t model )
		{
			mnode_t node;
			Single d;
			cplane_t plane;
			if ( model == null || model.nodes == null )
				Com.Error( Defines.ERR_DROP, "Mod_PointInLeaf: bad model" );
			node = model.nodes[0];
			while ( true )
			{
				if ( node.contents != -1 )
					return ( mleaf_t ) node;
				plane = node.plane;
				d = Math3D.DotProduct( p, plane.normal ) - plane.dist;
				if ( d > 0 )
					node = node.children[0];
				else
					node = node.children[1];
			}
		}

		Byte[] decompressed = new Byte[Defines.MAX_MAP_LEAFS / 8];
		Byte[] model_visibility = new Byte[Defines.MAX_MAP_VISIBILITY];
		public virtual Byte[] Mod_DecompressVis( Byte[] in_renamed, Int32 offset, model_t model )
		{
			Int32 c;
			Byte[] out_renamed;
			Int32 outp, inp;
			Int32 row;
			row = ( model.vis.numclusters + 7 ) >> 3;
			out_renamed = decompressed;
			outp = 0;
			inp = offset;
			if ( in_renamed == null )
			{
				while ( row != 0 )
				{
					out_renamed[outp++] = ( Byte ) 0xFF;
					row--;
				}

				return decompressed;
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
				while ( c != 0 )
				{
					out_renamed[outp++] = 0;
					c--;
				}
			}
			while ( outp < row );
			return decompressed;
		}

		public override Byte[] Mod_ClusterPVS( Int32 cluster, model_t model )
		{
			if ( cluster == -1 || model.vis == null )
				return mod_novis;
			return Mod_DecompressVis( model_visibility, model.vis.bitofs[cluster][Defines.DVIS_PVS], model );
		}

		public override void Mod_Modellist_f( )
		{
			Int32 i;
			model_t mod;
			Int32 total;
			total = 0;
			VID.Printf( Defines.PRINT_ALL, "Loaded models:\\n" );
			for ( i = 0; i < mod_numknown; i++ )
			{
				mod = mod_known[i];
				if ( mod.name.Length == 0 )
					continue;
				VID.Printf( Defines.PRINT_ALL, "%8i : %s\\n", mod.extradatasize, mod.name );
				total += mod.extradatasize;
			}

			VID.Printf( Defines.PRINT_ALL, "Total resident: " + total + '\\' );
		}

		public override void Mod_Init( )
		{
			for ( var i = 0; i < MAX_MOD_KNOWN; i++ )
			{
				mod_known[i] = new model_t();
			}

			Lib.Fill( mod_novis, ( Byte ) 0xff );
		}

		Byte[] fileBuffer;
		public virtual model_t Mod_ForName( String name, Boolean crash )
		{
			model_t mod = null;
			Int32 i;
			if ( name == null || name.Length == 0 )
				Com.Error( Defines.ERR_DROP, "Mod_ForName: NULL name" );
			if ( name[0] == '*' )
			{
				i = Int32.Parse( name.Substring( 1 ) );
				if ( i < 1 || r_worldmodel == null || i >= r_worldmodel.numsubmodels )
					Com.Error( Defines.ERR_DROP, "bad inline model number" );
				return mod_inline[i];
			}

			for ( i = 0; i < mod_numknown; i++ )
			{
				mod = mod_known[i];
				if ( mod.name.Length == 0 )
					continue;
				if ( mod.name.Equals( name ) )
					return mod;
			}

			for ( i = 0; i < mod_numknown; i++ )
			{
				mod = mod_known[i];
				if ( mod.name.Length == 0 )
					break;
			}

			if ( i == mod_numknown )
			{
				if ( mod_numknown == MAX_MOD_KNOWN )
					Com.Error( Defines.ERR_DROP, "mod_numknown == MAX_MOD_KNOWN" );
				mod_numknown++;
				mod = mod_known[i];
			}

			mod.name = name;
			fileBuffer = FS.LoadFile( name );
			if ( fileBuffer == null )
			{
				if ( crash )
					Com.Error( Defines.ERR_DROP, "Mod_NumForName: " + mod.name + " not found" );
				mod.name = "";
				return null;
			}

			modfilelen = fileBuffer.Length;
			loadmodel = mod;
			ByteBuffer bb = ByteBuffer.Wrap( fileBuffer );
			bb.Order = ByteOrder.LittleEndian;
			bb.Mark();
			var ident = bb.GetInt32();
			bb.Reset();
			switch ( ident )

			{
				case qfiles.IDALIASHEADER:
					Mod_LoadAliasModel( mod, bb );
					break;
				case qfiles.IDSPRITEHEADER:
					Mod_LoadSpriteModel( mod, bb );
					break;
				case qfiles.IDBSPHEADER:
					Mod_LoadBrushModel( mod, bb );
					break;
				default:
					Com.Error( Defines.ERR_DROP, "Mod_NumForName: unknown fileid for " + mod.name );
					break;
			}

			this.fileBuffer = null;
			return mod;
		}

		Byte[] mod_base;
		public virtual void Mod_LoadLighting( lump_t l )
		{
			if ( l.filelen == 0 )
			{
				loadmodel.lightdata = null;
				return;
			}

			loadmodel.lightdata = new Byte[l.filelen];
			System.Array.Copy( mod_base, l.fileofs, loadmodel.lightdata, 0, l.filelen );
		}

		public virtual void Mod_LoadVisibility( lump_t l )
		{
			if ( l.filelen == 0 )
			{
				loadmodel.vis = null;
				return;
			}

			System.Array.Copy( mod_base, l.fileofs, model_visibility, 0, l.filelen );
			ByteBuffer bb = ByteBuffer.Wrap( model_visibility, 0, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			loadmodel.vis = new dvis_t( bb );
		}

		public virtual void Mod_LoadVertexes( lump_t l )
		{
			mvertex_t[] vertexes;
			Int32 i, count;
			if ( ( l.filelen % mvertex_t.DISK_SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / mvertex_t.DISK_SIZE;
			vertexes = new mvertex_t[count];
			loadmodel.vertexes = vertexes;
			loadmodel.numvertexes = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
			{
				vertexes[i] = new mvertex_t( bb );
			}
		}

		public virtual Single RadiusFromBounds( Single[] mins, Single[] maxs )
		{
			Single[] corner = new Single[] { 0, 0, 0 };
			for ( var i = 0; i < 3; i++ )
			{
				corner[i] = Math.Abs( mins[i] ) > Math.Abs( maxs[i] ) ? Math.Abs( mins[i] ) : Math.Abs( maxs[i] );
			}

			return Math3D.VectorLength( corner );
		}

		public virtual void Mod_LoadSubmodels( lump_t l )
		{
			qfiles.dmodel_t in_renamed;
			mmodel_t[] out_renamed;
			Int32 i, j, count;
			if ( ( l.filelen % qfiles.dmodel_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / qfiles.dmodel_t.SIZE;
			out_renamed = new mmodel_t[count];
			loadmodel.submodels = out_renamed;
			loadmodel.numsubmodels = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dmodel_t( bb );
				out_renamed[i] = new mmodel_t();
				for ( j = 0; j < 3; j++ )
				{
					out_renamed[i].mins[j] = in_renamed.mins[j] - 1;
					out_renamed[i].maxs[j] = in_renamed.maxs[j] + 1;
					out_renamed[i].origin[j] = in_renamed.origin[j];
				}

				out_renamed[i].radius = RadiusFromBounds( out_renamed[i].mins, out_renamed[i].maxs );
				out_renamed[i].headnode = in_renamed.headnode;
				out_renamed[i].firstface = in_renamed.firstface;
				out_renamed[i].numfaces = in_renamed.numfaces;
			}
		}

		public virtual void Mod_LoadEdges( lump_t l )
		{
			medge_t[] edges;
			Int32 i, count;
			if ( ( l.filelen % medge_t.DISK_SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / medge_t.DISK_SIZE;
			edges = new medge_t[count + 1];
			loadmodel.edges = edges;
			loadmodel.numedges = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
			{
				edges[i] = new medge_t( bb );
			}
		}

		public virtual void Mod_LoadTexinfo( lump_t l )
		{
			texinfo_t in_renamed;
			mtexinfo_t[] out_renamed;
			mtexinfo_t step;
			Int32 i, count;
			Int32 next;
			String name;
			if ( ( l.filelen % texinfo_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / texinfo_t.SIZE;
			out_renamed = new mtexinfo_t[count];
			for ( i = 0; i < count; i++ )
			{
				out_renamed[i] = new mtexinfo_t();
			}

			loadmodel.texinfo = out_renamed;
			loadmodel.numtexinfo = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
			{
				in_renamed = new texinfo_t( bb );
				out_renamed[i].vecs = in_renamed.vecs;
				out_renamed[i].flags = in_renamed.flags;
				next = in_renamed.nexttexinfo;
				if ( next > 0 )
					out_renamed[i].next = loadmodel.texinfo[next];
				else
					out_renamed[i].next = null;
				name = "textures/" + in_renamed.texture + ".wal";
				out_renamed[i].image = GL_FindImage( name, it_wall );
				if ( out_renamed[i].image == null )
				{
					VID.Printf( Defines.PRINT_ALL, "Couldn't load " + name + '\\' );
					out_renamed[i].image = r_notexture;
				}
			}

			for ( i = 0; i < count; i++ )
			{
				out_renamed[i].numframes = 1;
				for ( step = out_renamed[i].next; ( step != null ) && ( step != out_renamed[i] ); step = step.next )
					out_renamed[i].numframes++;
			}
		}

		public virtual void CalcSurfaceExtents( msurface_t s )
		{
			Single[] mins = new Single[] { 0, 0 };
			Single[] maxs = new Single[] { 0, 0 };
			Single val;
			Int32 j, e;
			mvertex_t v;
			Int32[] bmins = new[] { 0, 0 };
			Int32[] bmaxs = new[] { 0, 0 };
			mins[0] = mins[1] = 999999;
			maxs[0] = maxs[1] = -99999;
			mtexinfo_t tex = s.texinfo;
			for ( var i = 0; i < s.numedges; i++ )
			{
				e = loadmodel.surfedges[s.firstedge + i];
				if ( e >= 0 )
					v = loadmodel.vertexes[loadmodel.edges[e].v[0]];
				else
					v = loadmodel.vertexes[loadmodel.edges[-e].v[1]];
				for ( j = 0; j < 2; j++ )
				{
					val = v.position[0] * tex.vecs[j][0] + v.position[1] * tex.vecs[j][1] + v.position[2] * tex.vecs[j][2] + tex.vecs[j][3];
					if ( val < mins[j] )
						mins[j] = val;
					if ( val > maxs[j] )
						maxs[j] = val;
				}
			}

			for ( var i = 0; i < 2; i++ )
			{
				bmins[i] = ( Int32 ) Math.Floor( mins[i] / 16 );
				bmaxs[i] = ( Int32 ) Math.Ceiling( maxs[i] / 16 );
				s.texturemins[i] = ( Int16 ) ( bmins[i] * 16 );
				s.extents[i] = ( Int16 ) ( ( bmaxs[i] - bmins[i] ) * 16 );
			}
		}

		public virtual void Mod_LoadFaces( lump_t l )
		{
			qfiles.dface_t in_renamed;
			msurface_t[] out_renamed;
			Int32 i, count, surfnum;
			Int32 planenum, side;
			Int32 ti;
			if ( ( l.filelen % qfiles.dface_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / qfiles.dface_t.SIZE;
			out_renamed = new msurface_t[count];
			loadmodel.surfaces = out_renamed;
			loadmodel.numsurfaces = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			currentmodel = loadmodel;
			GL_BeginBuildingLightmaps( loadmodel );
			for ( surfnum = 0; surfnum < count; surfnum++ )
			{
				in_renamed = new dface_t( bb );
				out_renamed[surfnum] = new msurface_t();
				out_renamed[surfnum].firstedge = in_renamed.firstedge;
				out_renamed[surfnum].numedges = in_renamed.numedges;
				out_renamed[surfnum].flags = 0;
				out_renamed[surfnum].polys = null;
				planenum = in_renamed.planenum;
				side = in_renamed.side;
				if ( side != 0 )
					out_renamed[surfnum].flags |= Defines.SURF_PLANEBACK;
				out_renamed[surfnum].plane = loadmodel.planes[planenum];
				ti = in_renamed.texinfo;
				if ( ti < 0 || ti >= loadmodel.numtexinfo )
					Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: bad texinfo number" );
				out_renamed[surfnum].texinfo = loadmodel.texinfo[ti];
				CalcSurfaceExtents( out_renamed[surfnum] );
				for ( i = 0; i < Defines.MAXLIGHTMAPS; i++ )
					out_renamed[surfnum].styles[i] = in_renamed.styles[i];
				i = in_renamed.lightofs;
				if ( i == -1 )
					out_renamed[surfnum].samples = null;
				else
				{
					ByteBuffer pointer = ByteBuffer.Wrap( loadmodel.lightdata );
					pointer.Position = i;
					pointer = pointer.Slice();
					pointer.Mark();
					out_renamed[surfnum].samples = pointer;
				}

				if ( ( out_renamed[surfnum].texinfo.flags & Defines.SURF_WARP ) != 0 )
				{
					out_renamed[surfnum].flags |= Defines.SURF_DRAWTURB;
					for ( i = 0; i < 2; i++ )
					{
						out_renamed[surfnum].extents[i] = 16384;
						out_renamed[surfnum].texturemins[i] = -8192;
					}

					GL_SubdivideSurface( out_renamed[surfnum] );
				}

				if ( ( out_renamed[surfnum].texinfo.flags & ( Defines.SURF_SKY | Defines.SURF_TRANS33 | Defines.SURF_TRANS66 | Defines.SURF_WARP ) ) == 0 )
					GL_CreateSurfaceLightmap( out_renamed[surfnum] );
				if ( ( out_renamed[surfnum].texinfo.flags & Defines.SURF_WARP ) == 0 )
					GL_BuildPolygonFromSurface( out_renamed[surfnum] );
			}

			GL_EndBuildingLightmaps();
		}

		public virtual void Mod_SetParent( mnode_t node, mnode_t parent )
		{
			node.parent = parent;
			if ( node.contents != -1 )
				return;
			Mod_SetParent( node.children[0], node );
			Mod_SetParent( node.children[1], node );
		}

		public virtual void Mod_LoadNodes( lump_t l )
		{
			Int32 i, j, count, p;
			qfiles.dnode_t in_renamed;
			mnode_t[] out_renamed;
			if ( ( l.filelen % qfiles.dnode_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / qfiles.dnode_t.SIZE;
			out_renamed = new mnode_t[count];
			loadmodel.nodes = out_renamed;
			loadmodel.numnodes = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
				out_renamed[i] = new mnode_t();
			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dnode_t( bb );
				for ( j = 0; j < 3; j++ )
				{
					out_renamed[i].mins[j] = in_renamed.mins[j];
					out_renamed[i].maxs[j] = in_renamed.maxs[j];
				}

				p = in_renamed.planenum;
				out_renamed[i].plane = loadmodel.planes[p];
				out_renamed[i].firstsurface = in_renamed.firstface;
				out_renamed[i].numsurfaces = in_renamed.numfaces;
				out_renamed[i].contents = -1;
				for ( j = 0; j < 2; j++ )
				{
					p = in_renamed.children[j];
					if ( p >= 0 )
						out_renamed[i].children[j] = loadmodel.nodes[p];
					else
						out_renamed[i].children[j] = loadmodel.leafs[-1 - p];
				}
			}

			Mod_SetParent( loadmodel.nodes[0], null );
		}

		public virtual void Mod_LoadLeafs( lump_t l )
		{
			qfiles.dleaf_t in_renamed;
			mleaf_t[] out_renamed;
			Int32 i, j, count;
			if ( ( l.filelen % qfiles.dleaf_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / qfiles.dleaf_t.SIZE;
			out_renamed = new mleaf_t[count];
			loadmodel.leafs = out_renamed;
			loadmodel.numleafs = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
			{
				in_renamed = new dleaf_t( bb );
				out_renamed[i] = new mleaf_t();
				for ( j = 0; j < 3; j++ )
				{
					out_renamed[i].mins[j] = in_renamed.mins[j];
					out_renamed[i].maxs[j] = in_renamed.maxs[j];
				}

				out_renamed[i].contents = in_renamed.contents;
				out_renamed[i].cluster = in_renamed.cluster;
				out_renamed[i].area = in_renamed.area;
				out_renamed[i].SetMarkSurface( in_renamed.firstleafface, loadmodel.marksurfaces );
				out_renamed[i].nummarksurfaces = in_renamed.numleaffaces;
			}
		}

		public virtual void Mod_LoadMarksurfaces( lump_t l )
		{
			Int32 i, j, count;
			msurface_t[] out_renamed;
			if ( ( l.filelen % Defines.SIZE_OF_SHORT ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / Defines.SIZE_OF_SHORT;
			out_renamed = new msurface_t[count];
			loadmodel.marksurfaces = out_renamed;
			loadmodel.nummarksurfaces = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
			{
				j = bb.GetInt16();
				if ( j < 0 || j >= loadmodel.numsurfaces )
					Com.Error( Defines.ERR_DROP, "Mod_ParseMarksurfaces: bad surface number" );
				out_renamed[i] = loadmodel.surfaces[j];
			}
		}

		public virtual void Mod_LoadSurfedges( lump_t l )
		{
			Int32 i, count;
			Int32[] offsets;
			if ( ( l.filelen % Defines.SIZE_OF_INT ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / Defines.SIZE_OF_INT;
			if ( count < 1 || count >= Defines.MAX_MAP_SURFEDGES )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: bad surfedges count in " + loadmodel.name + ": " + count );
			offsets = new Int32[count];
			loadmodel.surfedges = offsets;
			loadmodel.numsurfedges = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
				offsets[i] = bb.GetInt32();
		}

		public virtual void Mod_LoadPlanes( lump_t l )
		{
			Int32 i, j;
			cplane_t[] out_renamed;
			qfiles.dplane_t in_renamed;
			Int32 count;
			Int32 bits;
			if ( ( l.filelen % qfiles.dplane_t.SIZE ) != 0 )
				Com.Error( Defines.ERR_DROP, "MOD_LoadBmodel: funny lump size in " + loadmodel.name );
			count = l.filelen / qfiles.dplane_t.SIZE;
			out_renamed = new cplane_t[count * 2];
			loadmodel.planes = out_renamed;
			loadmodel.numplanes = count;
			ByteBuffer bb = ByteBuffer.Wrap( mod_base, l.fileofs, l.filelen );
			bb.Order = ByteOrder.LittleEndian;
			for ( i = 0; i < count; i++ )
			{
				bits = 0;
				in_renamed = new dplane_t( bb );
				out_renamed[i] = new cplane_t();
				for ( j = 0; j < 3; j++ )
				{
					out_renamed[i].normal[j] = in_renamed.normal[j];
					if ( out_renamed[i].normal[j] < 0 )
						bits |= ( 1 << j );
				}

				out_renamed[i].dist = in_renamed.dist;
				out_renamed[i].type = ( Byte ) in_renamed.type;
				out_renamed[i].signbits = ( Byte ) bits;
			}
		}

		public virtual void Mod_LoadBrushModel( model_t mod, ByteBuffer buffer )
		{
			Int32 i;
			qfiles.dheader_t header;
			mmodel_t bm;
			loadmodel.type = mod_brush;
			if ( loadmodel != mod_known[0] )
				Com.Error( Defines.ERR_DROP, "Loaded a brush model after the world" );
			header = new dheader_t( buffer );
			i = header.version;
			if ( i != Defines.BSPVERSION )
				Com.Error( Defines.ERR_DROP, "Mod_LoadBrushModel: " + mod.name + " has wrong version number (" + i + " should be " + Defines.BSPVERSION + ")" );
			mod_base = fileBuffer;
			Mod_LoadVertexes( header.lumps[Defines.LUMP_VERTEXES] );
			Mod_LoadEdges( header.lumps[Defines.LUMP_EDGES] );
			Mod_LoadSurfedges( header.lumps[Defines.LUMP_SURFEDGES] );
			Mod_LoadLighting( header.lumps[Defines.LUMP_LIGHTING] );
			Mod_LoadPlanes( header.lumps[Defines.LUMP_PLANES] );
			Mod_LoadTexinfo( header.lumps[Defines.LUMP_TEXINFO] );
			Mod_LoadFaces( header.lumps[Defines.LUMP_FACES] );
			Mod_LoadMarksurfaces( header.lumps[Defines.LUMP_LEAFFACES] );
			Mod_LoadVisibility( header.lumps[Defines.LUMP_VISIBILITY] );
			Mod_LoadLeafs( header.lumps[Defines.LUMP_LEAFS] );
			Mod_LoadNodes( header.lumps[Defines.LUMP_NODES] );
			Mod_LoadSubmodels( header.lumps[Defines.LUMP_MODELS] );
			mod.numframes = 2;
			model_t starmod;
			for ( i = 0; i < mod.numsubmodels; i++ )
			{
				bm = mod.submodels[i];
				starmod = mod_inline[i] = loadmodel.Copy();
				starmod.firstmodelsurface = bm.firstface;
				starmod.nummodelsurfaces = bm.numfaces;
				starmod.firstnode = bm.headnode;
				if ( starmod.firstnode >= loadmodel.numnodes )
					Com.Error( Defines.ERR_DROP, "Inline model " + i + " has bad firstnode" );
				Math3D.VectorCopy( bm.maxs, starmod.maxs );
				Math3D.VectorCopy( bm.mins, starmod.mins );
				starmod.radius = bm.radius;
				if ( i == 0 )
					loadmodel = starmod.Copy();
				starmod.numleafs = bm.visleafs;
			}
		}

		public virtual void Mod_LoadAliasModel( model_t mod, ByteBuffer buffer )
		{
			Int32 i;
			qfiles.dmdl_t pheader;
			qfiles.dstvert_t[] poutst;
			qfiles.dtriangle_t[] pouttri;
			qfiles.daliasframe_t[] poutframe;
			Int32[] poutcmd;
			pheader = new dmdl_t( buffer );
			if ( pheader.version != qfiles.ALIAS_VERSION )
				Com.Error( Defines.ERR_DROP, "%s has wrong version number (%i should be %i)", mod.name, pheader.version, qfiles.ALIAS_VERSION );
			if ( pheader.skinheight > MAX_LBM_HEIGHT )
				Com.Error( Defines.ERR_DROP, "model " + mod.name + " has a skin taller than " + MAX_LBM_HEIGHT );
			if ( pheader.num_xyz <= 0 )
				Com.Error( Defines.ERR_DROP, "model " + mod.name + " has no vertices" );
			if ( pheader.num_xyz > qfiles.MAX_VERTS )
				Com.Error( Defines.ERR_DROP, "model " + mod.name + " has too many vertices" );
			if ( pheader.num_st <= 0 )
				Com.Error( Defines.ERR_DROP, "model " + mod.name + " has no st vertices" );
			if ( pheader.num_tris <= 0 )
				Com.Error( Defines.ERR_DROP, "model " + mod.name + " has no triangles" );
			if ( pheader.num_frames <= 0 )
				Com.Error( Defines.ERR_DROP, "model " + mod.name + " has no frames" );
			poutst = new qfiles.dstvert_t[pheader.num_st];
			buffer.Position = pheader.ofs_st;
			for ( i = 0; i < pheader.num_st; i++ )
			{
				poutst[i] = new dstvert_t( buffer );
			}

			pouttri = new qfiles.dtriangle_t[pheader.num_tris];
			buffer.Position = pheader.ofs_tris;
			for ( i = 0; i < pheader.num_tris; i++ )
			{
				pouttri[i] = new dtriangle_t( buffer );
			}

			poutframe = new qfiles.daliasframe_t[pheader.num_frames];
			buffer.Position = pheader.ofs_frames;
			for ( i = 0; i < pheader.num_frames; i++ )
			{
				poutframe[i] = new daliasframe_t( buffer );
				poutframe[i].verts = new Int32[pheader.num_xyz];
				for ( var k = 0; k < pheader.num_xyz; k++ )
				{
					poutframe[i].verts[k] = buffer.GetInt32();
				}
			}

			mod.type = mod_alias;
			poutcmd = new Int32[pheader.num_glcmds];
			buffer.Position = pheader.ofs_glcmds;
			for ( i = 0; i < pheader.num_glcmds; i++ )
				poutcmd[i] = buffer.GetInt32();
			String[] skinNames = new String[pheader.num_skins];
			Byte[] nameBuf = new Byte[qfiles.MAX_SKINNAME];
			buffer.Position = pheader.ofs_skins;
			for ( i = 0; i < pheader.num_skins; i++ )
			{
				buffer.Get( nameBuf );
				skinNames[i] = Encoding.ASCII.GetString( nameBuf ).Trim();
				mod.skins[i] = GL_FindImage( skinNames[i], it_skin );
			}

			pheader.skinNames = skinNames;
			pheader.stVerts = poutst;
			pheader.triAngles = pouttri;
			pheader.glCmds = poutcmd;
			pheader.aliasFrames = poutframe;
			mod.extradata = pheader;
			mod.mins[0] = -32;
			mod.mins[1] = -32;
			mod.mins[2] = -32;
			mod.maxs[0] = 32;
			mod.maxs[1] = 32;
			mod.maxs[2] = 32;
		}

		public virtual void Mod_LoadSpriteModel( model_t mod, ByteBuffer buffer )
		{
			qfiles.dsprite_t sprout = new dsprite_t( buffer );
			if ( sprout.version != qfiles.SPRITE_VERSION )
				Com.Error( Defines.ERR_DROP, "%s has wrong version number (%i should be %i)", mod.name, sprout.version, qfiles.SPRITE_VERSION );
			if ( sprout.numframes > qfiles.MAX_MD2SKINS )
				Com.Error( Defines.ERR_DROP, "%s has too many frames (%i > %i)", mod.name, sprout.numframes, qfiles.MAX_MD2SKINS );
			for ( var i = 0; i < sprout.numframes; i++ )
			{
				mod.skins[i] = GL_FindImage( sprout.frames[i].name, it_sprite );
			}

			mod.type = mod_sprite;
			mod.extradata = sprout;
		}

		public override void R_BeginRegistration( String model )
		{
			cvar_t flushmap;
			Polygon.Reset();
			registration_sequence++;
			r_oldviewcluster = -1;
			var fullname = "maps/" + model + ".bsp";
			flushmap = Cvar.Get( "flushmap", "0", 0 );
			if ( !mod_known[0].name.Equals( fullname ) || flushmap.value != 0F )
				Mod_Free( mod_known[0] );
			r_worldmodel = Mod_ForName( fullname, true );
			r_viewcluster = -1;
		}

		public override model_t R_RegisterModel( String name )
		{
			model_t mod = null;
			Int32 i;
			qfiles.dsprite_t sprout;
			qfiles.dmdl_t pheader;
			mod = Mod_ForName( name, false );
			if ( mod != null )
			{
				mod.registration_sequence = registration_sequence;
				if ( mod.type == mod_sprite )
				{
					sprout = ( qfiles.dsprite_t ) mod.extradata;
					for ( i = 0; i < sprout.numframes; i++ )
						mod.skins[i] = GL_FindImage( sprout.frames[i].name, it_sprite );
				}
				else if ( mod.type == mod_alias )
				{
					pheader = ( qfiles.dmdl_t ) mod.extradata;
					for ( i = 0; i < pheader.num_skins; i++ )
						mod.skins[i] = GL_FindImage( pheader.skinNames[i], it_skin );
					mod.numframes = pheader.num_frames;
				}
				else if ( mod.type == mod_brush )
				{
					for ( i = 0; i < mod.numtexinfo; i++ )
						mod.texinfo[i].image.registration_sequence = registration_sequence;
				}
			}

			return mod;
		}

		public override void R_EndRegistration( )
		{
			model_t mod;
			for ( var i = 0; i < mod_numknown; i++ )
			{
				mod = mod_known[i];
				if ( mod.name.Length == 0 )
					continue;
				if ( mod.registration_sequence != registration_sequence )
				{
					Mod_Free( mod );
				}
			}

			GL_FreeUnusedImages();
		}

		public virtual void Mod_Free( model_t mod )
		{
			mod.Clear();
		}

		public override void Mod_FreeAll( )
		{
			for ( var i = 0; i < mod_numknown; i++ )
			{
				if ( mod_known[i].extradata != null )
					Mod_Free( mod_known[i] );
			}
		}
	}
}
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using System;

namespace Jake2.Render
{
	public class model_t : ICloneable
	{
		public String name = "";
		public Int32 registration_sequence;
		public Int32 type;
		public Int32 numframes;
		public Int32 flags;
		public Single[] mins = new Single[] { 0, 0, 0 }, maxs = new Single[] { 0, 0, 0 };
		public Single radius;
		public Boolean clipbox;
		public Single[] clipmins = new Single[] { 0, 0, 0 }, clipmaxs = new Single[] { 0, 0, 0 };
		public Int32 firstmodelsurface, nummodelsurfaces;
		public Int32 lightmap;
		public Int32 numsubmodels;
		public mmodel_t[] submodels;
		public Int32 numplanes;
		public cplane_t[] planes;
		public Int32 numleafs;
		public mleaf_t[] leafs;
		public Int32 numvertexes;
		public mvertex_t[] vertexes;
		public Int32 numedges;
		public medge_t[] edges;
		public Int32 numnodes;
		public Int32 firstnode;
		public mnode_t[] nodes;
		public Int32 numtexinfo;
		public mtexinfo_t[] texinfo;
		public Int32 numsurfaces;
		public msurface_t[] surfaces;
		public Int32 numsurfedges;
		public Int32[] surfedges;
		public Int32 nummarksurfaces;
		public msurface_t[] marksurfaces;
		public qfiles.dvis_t vis;
		public Byte[] lightdata;
		public image_t[] skins = new image_t[Defines.MAX_MD2SKINS];
		public Int32 extradatasize;
		public Object extradata;

		public virtual void Clear( )
		{
			name = "";
			registration_sequence = 0;
			type = 0;
			numframes = 0;
			flags = 0;
			Math3D.VectorClear( mins );
			Math3D.VectorClear( maxs );
			radius = 0;
			clipbox = false;
			Math3D.VectorClear( clipmins );
			Math3D.VectorClear( clipmaxs );
			firstmodelsurface = nummodelsurfaces = 0;
			lightmap = 0;
			numsubmodels = 0;
			submodels = null;
			numplanes = 0;
			planes = null;
			numleafs = 0;
			leafs = null;
			numvertexes = 0;
			vertexes = null;
			numedges = 0;
			edges = null;
			numnodes = 0;
			firstnode = 0;
			nodes = null;
			numtexinfo = 0;
			texinfo = null;
			numsurfaces = 0;
			surfaces = null;
			numsurfedges = 0;
			surfedges = null;
			nummarksurfaces = 0;
			marksurfaces = null;
			vis = null;
			lightdata = null;
			skins.Fill( null );
			extradatasize = 0;
			extradata = null;
		}

		public virtual void Set( model_t src )
		{
			name = src.name;
			registration_sequence = src.registration_sequence;
			type = src.type;
			numframes = src.numframes;
			flags = src.flags;
			mins = src.mins;
			maxs = src.maxs;
			radius = src.radius;
			clipbox = src.clipbox;
			clipmins = src.clipmins;
			clipmaxs = src.clipmaxs;
			firstmodelsurface = src.firstmodelsurface;
			nummodelsurfaces = src.nummodelsurfaces;
			lightmap = src.lightmap;
			numsubmodels = src.numsubmodels;
			submodels = src.submodels;
			numplanes = src.numplanes;
			planes = src.planes;
			numleafs = src.numleafs;
			leafs = src.leafs;
			numvertexes = src.numvertexes;
			vertexes = src.vertexes;
			numedges = src.numedges;
			edges = src.edges;
			numnodes = src.numnodes;
			firstnode = src.firstnode;
			nodes = src.nodes;
			numtexinfo = src.numtexinfo;
			texinfo = src.texinfo;
			numsurfaces = src.numsurfaces;
			surfaces = src.surfaces;
			numsurfedges = src.numsurfedges;
			surfedges = src.surfedges;
			nummarksurfaces = src.nummarksurfaces;
			marksurfaces = src.marksurfaces;
			vis = src.vis;
			lightdata = src.lightdata;
			skins = src.skins;
			extradatasize = src.extradatasize;
			extradata = src.extradata;
		}

		public Object Clone( )
		{
			var clone = new model_t();
			clone.Set( this );
			return clone;
		}

		public virtual model_t Copy( )
		{
			var theClone = ( model_t ) Clone();
			theClone.mins = Lib.Clone( this.mins );
			theClone.maxs = Lib.Clone( this.maxs );
			theClone.clipmins = Lib.Clone( this.clipmins );
			theClone.clipmaxs = Lib.Clone( this.clipmaxs );

			return theClone;
		}
	}
}
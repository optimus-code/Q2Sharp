using J2N.IO;
using Q2Sharp.Game;
using System;

namespace Q2Sharp.Render
{
	public class msurface_t
	{
		public Int32 visframe;
		public cplane_t plane;
		public Int32 flags;
		public Int32 firstedge;
		public Int32 numedges;
		public Int16[] texturemins = new Int16[] { 0, 0 };
		public Int16[] extents = new Int16[] { 0, 0 };
		public Int32 light_s, light_t;
		public Int32 dlight_s, dlight_t;
		public glpoly_t polys;
		public msurface_t texturechain;
		public msurface_t lightmapchain;
		public mtexinfo_t texinfo = new mtexinfo_t();
		public Int32 dlightframe;
		public Int32 dlightbits;
		public Int32 lightmaptexturenum;
		public Byte[] styles = new Byte[Defines.MAXLIGHTMAPS];
		public Single[] cached_light = new Single[Defines.MAXLIGHTMAPS];
		public ByteBuffer samples;
		public virtual void Clear( )
		{
			visframe = 0;
			plane.Clear();
			flags = 0;
			firstedge = 0;
			numedges = 0;
			texturemins[0] = texturemins[1] = -1;
			extents[0] = extents[1] = 0;
			light_s = light_t = 0;
			dlight_s = dlight_t = 0;
			polys = null;
			texturechain = null;
			lightmapchain = null;
			texinfo.Clear();
			dlightframe = 0;
			dlightbits = 0;
			lightmaptexturenum = 0;
			for ( var i = 0; i < styles.Length; i++ )
			{
				styles[i] = 0;
			}

			for ( var i = 0; i < cached_light.Length; i++ )
			{
				cached_light[i] = 0;
			}

			if ( samples != null )
				samples.Clear();
		}
	}
}
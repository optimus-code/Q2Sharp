using J2N.IO;
using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using OpenTK.Graphics.OpenGL;
using System;

namespace Q2Sharp.Render.Fast
{
	public abstract class Light : Warp
	{
		Int32 r_dlightframecount;
		static readonly Int32 DLIGHT_CUTOFF = 64;
		private readonly Single[] v = new Single[] { 0, 0, 0 };
		public virtual void R_RenderDlight( dlight_t light )
		{
			var rad = light.intensity * 0.35F;
			Math3D.VectorSubtract( light.origin, r_origin, v );
			GL.Begin( PrimitiveType.TriangleFan );
			GL.Color3( light.color[0] * 0.2F, light.color[1] * 0.2F, light.color[2] * 0.2F );
			Int32 i;
			for ( i = 0; i < 3; i++ )
				v[i] = light.origin[i] - vpn[i] * rad;
			GL.Vertex3( v[0], v[1], v[2] );
			GL.Color3( 0, 0, 0 );
			Int32 j;
			Single a;
			for ( i = 16; i >= 0; i-- )
			{
				a = ( Single ) ( i / 16F * Math.PI * 2 );
				for ( j = 0; j < 3; j++ )
					v[j] = ( Single ) ( light.origin[j] + vright[j] * Math.Cos( a ) * rad + vup[j] * Math.Sin( a ) * rad );
				GL.Vertex3( v[0], v[1], v[2] );
			}

			GL.End();
		}

		public override void R_RenderDlights( )
		{
			if ( gl_flashblend.value == 0 )
				return;
			r_dlightframecount = r_framecount + 1;
			GL.DepthMask( false );
			GL.Disable( EnableCap.Texture2D );
			GL.ShadeModel( ShadingModel.Smooth );
			GL.Enable( EnableCap.Blend );
			GL.BlendFunc( BlendingFactor.One, BlendingFactor.One );
			for ( var i = 0; i < r_newrefdef.num_dlights; i++ )
			{
				R_RenderDlight( r_newrefdef.dlights[i] );
			}

			GL.Color3( 1, 1, 1 );
			GL.Disable( EnableCap.Blend );
			GL.Enable( EnableCap.Texture2D );
			GL.BlendFunc( BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha );
			GL.DepthMask( true );
		}

		public override void R_MarkLights( dlight_t light, Int32 bit, mnode_t node )
		{
			if ( node.contents != -1 )
				return;
			cplane_t splitplane = node.plane;
			var dist = Math3D.DotProduct( light.origin, splitplane.normal ) - splitplane.dist;
			if ( dist > light.intensity - DLIGHT_CUTOFF )
			{
				R_MarkLights( light, bit, node.children[0] );
				return;
			}

			if ( dist < -light.intensity + DLIGHT_CUTOFF )
			{
				R_MarkLights( light, bit, node.children[1] );
				return;
			}

			msurface_t surf;
			Int32 sidebit;
			for ( var i = 0; i < node.numsurfaces; i++ )
			{
				surf = r_worldmodel.surfaces[node.firstsurface + i];
				dist = Math3D.DotProduct( light.origin, surf.plane.normal ) - surf.plane.dist;
				sidebit = ( dist >= 0 ) ? 0 : Defines.SURF_PLANEBACK;
				if ( ( surf.flags & Defines.SURF_PLANEBACK ) != sidebit )
					continue;
				if ( surf.dlightframe != r_dlightframecount )
				{
					surf.dlightbits = 0;
					surf.dlightframe = r_dlightframecount;
				}

				surf.dlightbits |= bit;
			}

			R_MarkLights( light, bit, node.children[0] );
			R_MarkLights( light, bit, node.children[1] );
		}

		public override void R_PushDlights( )
		{
			if ( gl_flashblend.value != 0 )
				return;
			r_dlightframecount = r_framecount + 1;
			dlight_t l;
			for ( var i = 0; i < r_newrefdef.num_dlights; i++ )
			{
				l = r_newrefdef.dlights[i];
				R_MarkLights( l, 1 << i, r_worldmodel.nodes[0] );
			}
		}

		public Single[] pointcolor = new Single[] { 0, 0, 0 };
		public cplane_t lightplane;
		public Single[] lightspot = new Single[] { 0, 0, 0 };
		public virtual Int32 RecursiveLightPoint( mnode_t node, Single[] start, Single[] end )
		{
			if ( node.contents != -1 )
				return -1;
			cplane_t plane = node.plane;
			var front = Math3D.DotProduct( start, plane.normal ) - plane.dist;
			var back = Math3D.DotProduct( end, plane.normal ) - plane.dist;
			var side = ( front < 0 );
			var sideIndex = ( side ) ? 1 : 0;
			if ( ( back < 0 ) == side )
				return RecursiveLightPoint( node.children[sideIndex], start, end );
			var frac = front / ( front - back );
			Single[] mid = Vec3Cache.Get();
			mid[0] = start[0] + ( end[0] - start[0] ) * frac;
			mid[1] = start[1] + ( end[1] - start[1] ) * frac;
			mid[2] = start[2] + ( end[2] - start[2] ) * frac;
			var r = RecursiveLightPoint( node.children[sideIndex], start, mid );
			if ( r >= 0 )
			{
				Vec3Cache.Release();
				return r;
			}

			if ( ( back < 0 ) == side )
			{
				Vec3Cache.Release();
				return -1;
			}

			Math3D.VectorCopy( mid, lightspot );
			lightplane = plane;
			var surfIndex = node.firstsurface;
			msurface_t surf;
			Int32 s, t, ds, dt;
			mtexinfo_t tex;
			ByteBuffer lightmap;
			Int32 maps;
			for ( var i = 0; i < node.numsurfaces; i++, surfIndex++ )
			{
				surf = r_worldmodel.surfaces[surfIndex];
				if ( ( surf.flags & ( Defines.SURF_DRAWTURB | Defines.SURF_DRAWSKY ) ) != 0 )
					continue;
				tex = surf.texinfo;
				s = ( Int32 ) ( Math3D.DotProduct( mid, tex.vecs[0] ) + tex.vecs[0][3] );
				t = ( Int32 ) ( Math3D.DotProduct( mid, tex.vecs[1] ) + tex.vecs[1][3] );
				if ( s < surf.texturemins[0] || t < surf.texturemins[1] )
					continue;
				ds = s - surf.texturemins[0];
				dt = t - surf.texturemins[1];
				if ( ds > surf.extents[0] || dt > surf.extents[1] )
					continue;
				if ( surf.samples == null )
					return 0;
				ds >>= 4;
				dt >>= 4;
				lightmap = surf.samples;
				var lightmapIndex = 0;
				Math3D.VectorCopy( Globals.vec3_origin, pointcolor );
				if ( lightmap != null )
				{
					Single[] rgb;
					lightmapIndex += 3 * ( dt * ( ( surf.extents[0] >> 4 ) + 1 ) + ds );
					Single scale0, scale1, scale2;
					for ( maps = 0; maps < Defines.MAXLIGHTMAPS && surf.styles[maps] != ( Byte ) 255; maps++ )
					{
						rgb = r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb;
						scale0 = gl_modulate.value * rgb[0];
						scale1 = gl_modulate.value * rgb[1];
						scale2 = gl_modulate.value * rgb[2];
						pointcolor[0] += ( lightmap.Get( lightmapIndex + 0 ) & 0xFF ) * scale0 * ( 1F / 255 );
						pointcolor[1] += ( lightmap.Get( lightmapIndex + 1 ) & 0xFF ) * scale1 * ( 1F / 255 );
						pointcolor[2] += ( lightmap.Get( lightmapIndex + 2 ) & 0xFF ) * scale2 * ( 1F / 255 );
						lightmapIndex += 3 * ( ( surf.extents[0] >> 4 ) + 1 ) * ( ( surf.extents[1] >> 4 ) + 1 );
					}
				}

				Vec3Cache.Release();
				return 1;
			}

			r = RecursiveLightPoint( node.children[1 - sideIndex], mid, end );
			Vec3Cache.Release();
			return r;
		}

		private readonly Single[] end = new Single[] { 0, 0, 0 };
		public override void R_LightPoint( Single[] p, Single[] color )
		{
			if ( r_worldmodel.lightdata == null )
			{
				color[0] = color[1] = color[2] = 1F;
				return;
			}

			end[0] = p[0];
			end[1] = p[1];
			end[2] = p[2] - 2048;
			Single r = RecursiveLightPoint( r_worldmodel.nodes[0], p, end );
			if ( r == -1 )
			{
				Math3D.VectorCopy( Globals.vec3_origin, color );
			}
			else
			{
				Math3D.VectorCopy( pointcolor, color );
			}

			dlight_t dl;
			Single add;
			for ( var lnum = 0; lnum < r_newrefdef.num_dlights; lnum++ )
			{
				dl = r_newrefdef.dlights[lnum];
				Math3D.VectorSubtract( currententity.origin, dl.origin, end );
				add = dl.intensity - Math3D.VectorLength( end );
				add *= ( 1F / 256 );
				if ( add > 0 )
				{
					Math3D.VectorMA( color, add, dl.color, color );
				}
			}

			Math3D.VectorScale( color, gl_modulate.value, color );
		}

		Single[] s_blocklights = new Single[34 * 34 * 3];
		private readonly Single[] impact = new Single[] { 0, 0, 0 };
		public virtual void R_AddDynamicLights( msurface_t surf )
		{
			Int32 sd, td;
			Single fdist, frad, fminlight;
			Int32 s, t;
			dlight_t dl;
			Single[] pfBL;
			Single fsacc, ftacc;
			var smax = ( surf.extents[0] >> 4 ) + 1;
			var tmax = ( surf.extents[1] >> 4 ) + 1;
			mtexinfo_t tex = surf.texinfo;
			Single local0, local1;
			for ( var lnum = 0; lnum < r_newrefdef.num_dlights; lnum++ )
			{
				if ( ( surf.dlightbits & ( 1 << lnum ) ) == 0 )
					continue;
				dl = r_newrefdef.dlights[lnum];
				frad = dl.intensity;
				fdist = Math3D.DotProduct( dl.origin, surf.plane.normal ) - surf.plane.dist;
				frad -= Math.Abs( fdist );
				fminlight = DLIGHT_CUTOFF;
				if ( frad < fminlight )
					continue;
				fminlight = frad - fminlight;
				for ( var i = 0; i < 3; i++ )
				{
					impact[i] = dl.origin[i] - surf.plane.normal[i] * fdist;
				}

				local0 = Math3D.DotProduct( impact, tex.vecs[0] ) + tex.vecs[0][3] - surf.texturemins[0];
				local1 = Math3D.DotProduct( impact, tex.vecs[1] ) + tex.vecs[1][3] - surf.texturemins[1];
				pfBL = s_blocklights;
				var pfBLindex = 0;
				for ( t = 0, ftacc = 0; t < tmax; t++, ftacc += 16 )
				{
					td = ( Int32 ) ( local1 - ftacc );
					if ( td < 0 )
						td = -td;
					for ( s = 0, fsacc = 0; s < smax; s++, fsacc += 16, pfBLindex += 3 )
					{
						sd = ( Int32 ) ( local0 - fsacc );
						if ( sd < 0 )
							sd = -sd;
						if ( sd > td )
							fdist = sd + ( td >> 1 );
						else
							fdist = td + ( sd >> 1 );
						if ( fdist < fminlight )
						{
							pfBL[pfBLindex + 0] += ( frad - fdist ) * dl.color[0];
							pfBL[pfBLindex + 1] += ( frad - fdist ) * dl.color[1];
							pfBL[pfBLindex + 2] += ( frad - fdist ) * dl.color[2];
						}
					}
				}
			}
		}

		public override void R_SetCacheState( msurface_t surf )
		{
			for ( var maps = 0; maps < Defines.MAXLIGHTMAPS && surf.styles[maps] != ( Byte ) 255; maps++ )
			{
				surf.cached_light[maps] = r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].white;
			}
		}

		private Exception gotoStore = new Exception( "Go to store" );
		public override void R_BuildLightMap( msurface_t surf, Int32Buffer dest, Int32 stride )
		{
			Int32 r, g, b, a, max;
			Int32 i, j;
			var nummaps = 0;
			var blp = 0;
			Single[] bl;
			if ( ( surf.texinfo.flags & ( Defines.SURF_SKY | Defines.SURF_TRANS33 | Defines.SURF_TRANS66 | Defines.SURF_WARP ) ) != 0 )
				Com.Error( Defines.ERR_DROP, "R_BuildLightMap called for non-lit surface" );
			var smax = ( surf.extents[0] >> 4 ) + 1;
			var tmax = ( surf.extents[1] >> 4 ) + 1;
			var size = smax * tmax;
			if ( size > ( ( s_blocklights.Length * Defines.SIZE_OF_FLOAT ) >> 4 ) )
				Com.Error( Defines.ERR_DROP, "Bad s_blocklights size" );
			try
			{
				if ( surf.samples == null )
				{
					for ( i = 0; i < size * 3; i++ )
						s_blocklights[i] = 255;
					throw gotoStore;
				}

				ByteBuffer lightmap = surf.samples;
				var lightmapIndex = 0;
				Single scale0;
				Single scale1;
				Single scale2;
				if ( nummaps == 1 )
				{
					Int32 maps;
					for ( maps = 0; maps < Defines.MAXLIGHTMAPS && surf.styles[maps] != ( Byte ) 255; maps++ )
					{
						bl = s_blocklights;
						blp = 0;
						scale0 = gl_modulate.value * r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb[0];
						scale1 = gl_modulate.value * r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb[1];
						scale2 = gl_modulate.value * r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb[2];
						if ( scale0 == 1F && scale1 == 1F && scale2 == 1F )
						{
							for ( i = 0; i < size; i++ )
							{
								bl[blp++] = lightmap.Get( lightmapIndex++ ) & 0xFF;
								bl[blp++] = lightmap.Get( lightmapIndex++ ) & 0xFF;
								bl[blp++] = lightmap.Get( lightmapIndex++ ) & 0xFF;
							}
						}
						else
						{
							for ( i = 0; i < size; i++ )
							{
								bl[blp++] = ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale0;
								bl[blp++] = ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale1;
								bl[blp++] = ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale2;
							}
						}
					}
				}
				else
				{
					Int32 maps;
					Lib.Fill( s_blocklights, 0, size * 3, 0F );
					for ( maps = 0; maps < Defines.MAXLIGHTMAPS && surf.styles[maps] != ( Byte ) 255; maps++ )
					{
						bl = s_blocklights;
						blp = 0;
						scale0 = gl_modulate.value * r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb[0];
						scale1 = gl_modulate.value * r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb[1];
						scale2 = gl_modulate.value * r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb[2];
						if ( scale0 == 1F && scale1 == 1F && scale2 == 1F )
						{
							for ( i = 0; i < size; i++ )
							{
								bl[blp++] += lightmap.Get( lightmapIndex++ ) & 0xFF;
								bl[blp++] += lightmap.Get( lightmapIndex++ ) & 0xFF;
								bl[blp++] += lightmap.Get( lightmapIndex++ ) & 0xFF;
							}
						}
						else
						{
							for ( i = 0; i < size; i++ )
							{
								bl[blp++] += ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale0;
								bl[blp++] += ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale1;
								bl[blp++] += ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale2;
							}
						}
					}
				}

				if ( surf.dlightframe == r_framecount )
					R_AddDynamicLights( surf );
			}
			catch ( Exception store )
			{
			}

			stride -= smax;
			bl = s_blocklights;
			blp = 0;
			Int32 monolightmap = gl_monolightmap.string_renamed[0];
			var destp = 0;
			if ( monolightmap == '0' )
			{
				for ( i = 0; i < tmax; i++, destp += stride )
				{
					for ( j = 0; j < smax; j++ )
					{
						r = ( Int32 ) bl[blp++];
						g = ( Int32 ) bl[blp++];
						b = ( Int32 ) bl[blp++];
						if ( r < 0 )
							r = 0;
						if ( g < 0 )
							g = 0;
						if ( b < 0 )
							b = 0;
						if ( r > g )
							max = r;
						else
							max = g;
						if ( b > max )
							max = b;
						a = max;
						if ( max > 255 )
						{
							var t = 255F / max;
							r = ( Int32 ) ( r * t );
							g = ( Int32 ) ( g * t );
							b = ( Int32 ) ( b * t );
							a = ( Int32 ) ( a * t );
						}

						dest.Put( destp++, ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | r );
					}
				}
			}
			else
			{
				for ( i = 0; i < tmax; i++, destp += stride )
				{
					for ( j = 0; j < smax; j++ )
					{
						r = ( Int32 ) bl[blp++];
						g = ( Int32 ) bl[blp++];
						b = ( Int32 ) bl[blp++];
						if ( r < 0 )
							r = 0;
						if ( g < 0 )
							g = 0;
						if ( b < 0 )
							b = 0;
						if ( r > g )
							max = r;
						else
							max = g;
						if ( b > max )
							max = b;
						a = max;
						if ( max > 255 )
						{
							var t = 255F / max;
							r = ( Int32 ) ( r * t );
							g = ( Int32 ) ( g * t );
							b = ( Int32 ) ( b * t );
							a = ( Int32 ) ( a * t );
						}

						switch ( monolightmap )

						{
							case 'L':
							case 'I':
								r = a;
								g = b = 0;
								break;
							case 'C':
								a = 255 - ( ( r + g + b ) / 3 );
								var af = a / 255F;
								r = ( Int32 ) ( r * af );
								g = ( Int32 ) ( g * af );
								b = ( Int32 ) ( b * af );
								break;
							case 'A':
							default:
								r = g = b = 0;
								a = 255 - a;
								break;
						}

						dest.Put( destp++, ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | r );
					}
				}
			}
		}
	}
}
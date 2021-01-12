using J2N.IO;
using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using OpenTK.Graphics.OpenGL;
using System;

namespace Q2Sharp.Render.Basic
{
	public abstract class Light : Warp
	{
		Int32 r_dlightframecount;
		static readonly Int32 DLIGHT_CUTOFF = 64;
		public virtual void R_RenderDlight( dlight_t light )
		{
			Int32 i, j;
			Single a;
			Single[] v = new Single[] { 0, 0, 0 };
			Single rad;
			rad = light.intensity * 0.35F;
			Math3D.VectorSubtract( light.origin, r_origin, v );
			GL.Begin( PrimitiveType.TriangleFan );
			GL.Color3( light.color[0] * 0.2F, light.color[1] * 0.2F, light.color[2] * 0.2F );
			for ( i = 0; i < 3; i++ )
				v[i] = light.origin[i] - vpn[i] * rad;
			GL.Vertex3( v[0], v[1], v[2] );
			GL.Color3( 0, 0, 0 );
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
			Int32 i;
			dlight_t l;
			if ( gl_flashblend.value == 0 )
				return;
			r_dlightframecount = r_framecount + 1;
			GL.DepthMask( false );
			GL.Disable( EnableCap.Texture2D );
			GL.ShadeModel( ShadingModel.Smooth );
			GL.Enable( EnableCap.Blend );
			GL.BlendFunc( BlendingFactor.One, BlendingFactor.One );
			for ( i = 0; i < r_newrefdef.num_dlights; i++ )
			{
				l = r_newrefdef.dlights[i];
				R_RenderDlight( l );
			}

			GL.Color3( 1, 1, 1 );
			GL.Disable( EnableCap.Blend );
			GL.Enable( EnableCap.Texture2D );
			GL.BlendFunc( BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha );
			GL.DepthMask( true );
		}

		public override void R_MarkLights( dlight_t light, Int32 bit, mnode_t node )
		{
			cplane_t splitplane;
			Single dist;
			msurface_t surf;
			Int32 i;
			Int32 sidebit;
			if ( node.contents != -1 )
				return;
			splitplane = node.plane;
			dist = Math3D.DotProduct( light.origin, splitplane.normal ) - splitplane.dist;
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

			for ( i = 0; i < node.numsurfaces; i++ )
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
			Int32 i;
			dlight_t l;
			if ( gl_flashblend.value != 0 )
				return;
			r_dlightframecount = r_framecount + 1;
			for ( i = 0; i < r_newrefdef.num_dlights; i++ )
			{
				l = r_newrefdef.dlights[i];
				R_MarkLights( l, 1 << i, r_worldmodel.nodes[0] );
			}
		}

		Single[] pointcolor = new Single[] { 0, 0, 0 };
		cplane_t lightplane;
		public Single[] lightspot = new Single[] { 0, 0, 0 };
		public virtual Int32 RecursiveLightPoint( mnode_t node, Single[] start, Single[] end )
		{
			if ( node.contents != -1 )
				return -1;
			msurface_t surf;
			Int32 s, t, ds, dt;
			Int32 i;
			mtexinfo_t tex;
			ByteBuffer lightmap;
			Int32 maps;
			Single[] mid = new Single[] { 0, 0, 0 };
			cplane_t plane = node.plane;
			var front = Math3D.DotProduct( start, plane.normal ) - plane.dist;
			var back = Math3D.DotProduct( end, plane.normal ) - plane.dist;
			var side = ( front < 0 );
			var sideIndex = ( side ) ? 1 : 0;
			if ( ( back < 0 ) == side )
				return RecursiveLightPoint( node.children[sideIndex], start, end );
			var frac = front / ( front - back );
			mid[0] = start[0] + ( end[0] - start[0] ) * frac;
			mid[1] = start[1] + ( end[1] - start[1] ) * frac;
			mid[2] = start[2] + ( end[2] - start[2] ) * frac;
			var r = RecursiveLightPoint( node.children[sideIndex], start, mid );
			if ( r >= 0 )
				return r;
			if ( ( back < 0 ) == side )
				return -1;
			Math3D.VectorCopy( mid, lightspot );
			lightplane = plane;
			var surfIndex = node.firstsurface;
			Single[] scale = new Single[] { 0, 0, 0 };
			for ( i = 0; i < node.numsurfaces; i++, surfIndex++ )
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
					for ( maps = 0; maps < Defines.MAXLIGHTMAPS && surf.styles[maps] != ( Byte ) 255; maps++ )
					{
						rgb = r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb;
						scale[0] = gl_modulate.value * rgb[0];
						scale[1] = gl_modulate.value * rgb[1];
						scale[2] = gl_modulate.value * rgb[2];
						pointcolor[0] += ( lightmap.Get( lightmapIndex + 0 ) & 0xFF ) * scale[0] * ( 1F / 255 );
						pointcolor[1] += ( lightmap.Get( lightmapIndex + 1 ) & 0xFF ) * scale[1] * ( 1F / 255 );
						pointcolor[2] += ( lightmap.Get( lightmapIndex + 2 ) & 0xFF ) * scale[2] * ( 1F / 255 );
						lightmapIndex += 3 * ( ( surf.extents[0] >> 4 ) + 1 ) * ( ( surf.extents[1] >> 4 ) + 1 );
					}
				}

				return 1;
			}

			return RecursiveLightPoint( node.children[1 - sideIndex], mid, end );
		}

		public override void R_LightPoint( Single[] p, Single[] color )
		{
			Single[] end = new Single[] { 0, 0, 0 };
			dlight_t dl;
			Single add;
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
		public virtual void R_AddDynamicLights( msurface_t surf )
		{
			Int32 lnum;
			Int32 sd, td;
			Single fdist, frad, fminlight;
			Single[] impact = new Single[] { 0, 0, 0 };
			Single[] local = new Single[] { 0, 0, 0 };
			Int32 s, t;
			Int32 i;
			Int32 smax, tmax;
			mtexinfo_t tex;
			dlight_t dl;
			Single[] pfBL;
			Single fsacc, ftacc;
			smax = ( surf.extents[0] >> 4 ) + 1;
			tmax = ( surf.extents[1] >> 4 ) + 1;
			tex = surf.texinfo;
			for ( lnum = 0; lnum < r_newrefdef.num_dlights; lnum++ )
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
				for ( i = 0; i < 3; i++ )
				{
					impact[i] = dl.origin[i] - surf.plane.normal[i] * fdist;
				}

				local[0] = Math3D.DotProduct( impact, tex.vecs[0] ) + tex.vecs[0][3] - surf.texturemins[0];
				local[1] = Math3D.DotProduct( impact, tex.vecs[1] ) + tex.vecs[1][3] - surf.texturemins[1];
				pfBL = s_blocklights;
				var pfBLindex = 0;
				for ( t = 0, ftacc = 0; t < tmax; t++, ftacc += 16 )
				{
					td = ( Int32 ) ( local[1] - ftacc );
					if ( td < 0 )
						td = -td;
					for ( s = 0, fsacc = 0; s < smax; s++, fsacc += 16, pfBLindex += 3 )
					{
						sd = ( Int32 ) ( local[0] - fsacc );
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
			Int32 maps;
			for ( maps = 0; maps < Defines.MAXLIGHTMAPS && surf.styles[maps] != ( Byte ) 255; maps++ )
			{
				surf.cached_light[maps] = r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].white;
			}
		}

		public override void R_BuildLightMap( msurface_t surf, Int32Buffer dest, Int32 stride )
		{
			Int32 smax, tmax;
			Int32 r, g, b, a, max;
			Int32 i, j, size;
			ByteBuffer lightmap;
			Single[] scale = new Single[] { 0, 0, 0, 0 };
			var nummaps = 0;
			Single[] bl;
			var blp = 0;
			Int32 monolightmap;
			if ( ( surf.texinfo.flags & ( Defines.SURF_SKY | Defines.SURF_TRANS33 | Defines.SURF_TRANS66 | Defines.SURF_WARP ) ) != 0 )
				Com.Error( Defines.ERR_DROP, "R_BuildLightMap called for non-lit surface" );
			smax = ( surf.extents[0] >> 4 ) + 1;
			tmax = ( surf.extents[1] >> 4 ) + 1;
			size = smax * tmax;
			if ( size > ( ( s_blocklights.Length * Defines.SIZE_OF_FLOAT ) >> 4 ) )
				Com.Error( Defines.ERR_DROP, "Bad s_blocklights size" );
			try
			{
				if ( surf.samples == null )
				{
					Int32 maps;
					for ( i = 0; i < size * 3; i++ )
						s_blocklights[i] = 255;
					for ( maps = 0; maps < Defines.MAXLIGHTMAPS && surf.styles[maps] != ( Byte ) 255; maps++ )
					{
					}

					throw new longjmpException();
				}

				lightmap = surf.samples;
				var lightmapIndex = 0;
				if ( nummaps == 1 )
				{
					Int32 maps;
					for ( maps = 0; maps < Defines.MAXLIGHTMAPS && surf.styles[maps] != ( Byte ) 255; maps++ )
					{
						bl = s_blocklights;
						blp = 0;
						for ( i = 0; i < 3; i++ )
							scale[i] = gl_modulate.value * r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb[i];
						if ( scale[0] == 1F && scale[1] == 1F && scale[2] == 1F )
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
								bl[blp++] = ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale[0];
								bl[blp++] = ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale[1];
								bl[blp++] = ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale[2];
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
						for ( i = 0; i < 3; i++ )
							scale[i] = gl_modulate.value * r_newrefdef.lightstyles[surf.styles[maps] & 0xFF].rgb[i];
						if ( scale[0] == 1F && scale[1] == 1F && scale[2] == 1F )
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
								bl[blp++] += ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale[0];
								bl[blp++] += ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale[1];
								bl[blp++] += ( lightmap.Get( lightmapIndex++ ) & 0xFF ) * scale[2];
							}
						}
					}
				}

				if ( surf.dlightframe == r_framecount )
					R_AddDynamicLights( surf );
			}
			catch ( longjmpException store )
			{
			}

			stride -= smax;
			bl = s_blocklights;
			blp = 0;
			monolightmap = gl_monolightmap.string_renamed[0];
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

						r &= 0xFF;
						g &= 0xFF;
						b &= 0xFF;
						a &= 0xFF;
						dest.Put( destp++, ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | ( r << 0 ) );
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
								r *= ( Int32 ) ( a / 255F );
								g *= ( Int32 ) ( a / 255F );
								b *= ( Int32 ) ( a / 255F );
								break;
							case 'A':
							default:
								r = g = b = 0;
								a = 255 - a;
								break;
						}

						r &= 0xFF;
						g &= 0xFF;
						b &= 0xFF;
						a &= 0xFF;
						dest.Put( destp++, ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | ( r << 0 ) );
					}
				}
			}
		}
	}
}
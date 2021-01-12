using J2N.IO;
using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using OpenTK.Graphics.OpenGL;
using Q2Sharp.util;
using System;

namespace Q2Sharp.Render.Fast
{
	public abstract class Surf : Draw
	{
		public Single[] modelorg = new Single[] { 0, 0, 0 };
		public msurface_t r_alpha_surfaces;
		public const Int32 DYNAMIC_LIGHT_WIDTH = 128;
		public const Int32 DYNAMIC_LIGHT_HEIGHT = 128;
		public const Int32 LIGHTMAP_BYTES = 4;
		public new const Int32 BLOCK_WIDTH = 128;
		public new const Int32 BLOCK_HEIGHT = 128;
		public const Int32 MAX_LIGHTMAPS = 128;
		public static readonly PixelFormat GL_LIGHTMAP_FORMAT = PixelFormat.Rgba;
		public class gllightmapstate_t
		{
			public PixelInternalFormat internal_format;
			public Int32 current_lightmap_texture;
			public msurface_t[] lightmap_surfaces = new msurface_t[MAX_LIGHTMAPS];
			public Int32[] allocated = new Int32[BLOCK_WIDTH];
			public Int32Buffer lightmap_buffer = Lib.NewInt32Buffer( BLOCK_WIDTH * BLOCK_HEIGHT, ByteOrder.LittleEndian );
			public gllightmapstate_t( )
			{
				for ( var i = 0; i < MAX_LIGHTMAPS; i++ )
					lightmap_surfaces[i] = new msurface_t();
			}

			public virtual void ClearLightmapSurfaces( )
			{
				for ( var i = 0; i < MAX_LIGHTMAPS; i++ )
					lightmap_surfaces[i] = new msurface_t();
			}
		}

		gllightmapstate_t gl_lms = new gllightmapstate_t();
		public abstract Byte[] Mod_ClusterPVS( Int32 cluster, model_t model );
		public abstract void R_DrawSkyBox( );
		public abstract void R_AddSkySurface( msurface_t surface );
		public abstract void R_ClearSkyBox( );
		public abstract void EmitWaterPolys( msurface_t fa );
		public abstract void R_MarkLights( dlight_t light, Int32 bit, mnode_t node );
		public abstract void R_SetCacheState( msurface_t surf );
		public abstract void R_BuildLightMap( msurface_t surf, Int32Buffer dest, Int32 stride );
		public virtual image_t R_TextureAnimation( mtexinfo_t tex )
		{
			if ( tex.next == null )
				return tex.image;
			var c = currententity.frame % tex.numframes;
			while ( c != 0 )
			{
				tex = tex.next;
				c--;
			}

			return tex.image;
		}

		public virtual void DrawGLPoly( glpoly_t p )
		{
			GL.DrawArrays( PrimitiveType.Polygon, p.pos, p.numverts );
		}

		public virtual void DrawGLFlowingPoly( glpoly_t p )
		{
			var scroll = -64 * ( ( r_newrefdef.time / 40F ) - ( Int32 ) ( r_newrefdef.time / 40F ) );
			if ( scroll == 0F )
				scroll = -64F;
			p.BeginScrolling( scroll );
			GL.DrawArrays( PrimitiveType.Polygon, p.pos, p.numverts );
			p.EndScrolling();
		}

		public virtual void R_DrawTriangleOutlines( )
		{
			if ( gl_showtris.value == 0 )
				return;
			GL.Disable( EnableCap.Texture2D );
			GL.Disable( EnableCap.DepthTest );
			GL.Color4( 1, 1, 1, 1 );
			msurface_t surf;
			glpoly_t p;
			Int32 j;
			for ( var i = 0; i < MAX_LIGHTMAPS; i++ )
			{
				for ( surf = gl_lms.lightmap_surfaces[i]; surf != null; surf = surf.lightmapchain )
				{
					for ( p = surf.polys; p != null; p = p.chain )
					{
						for ( j = 2; j < p.numverts; j++ )
						{
							GL.Begin( PrimitiveType.LineStrip );
							GL.Vertex3( p.X( 0 ), p.Y( 0 ), p.Z( 0 ) );
							GL.Vertex3( p.X( j - 1 ), p.Y( j - 1 ), p.Z( j - 1 ) );
							GL.Vertex3( p.X( j ), p.Y( j ), p.Z( j ) );
							GL.Vertex3( p.X( 0 ), p.Y( 0 ), p.Z( 0 ) );
							GL.End();
						}
					}
				}
			}

			GL.Enable( EnableCap.DepthTest );
			GL.Enable( EnableCap.Texture2D );
		}

		private readonly Int32Buffer temp2 = Lib.NewInt32Buffer( 34 * 34, ByteOrder.LittleEndian );
		public virtual void R_RenderBrushPoly( msurface_t fa )
		{
			c_brush_polys++;
			image_t image = R_TextureAnimation( fa.texinfo );
			if ( ( fa.flags & Defines.SURF_DRAWTURB ) != 0 )
			{
				GL_Bind( image.texnum );
				GL_TexEnv( ( Int32 ) All.Modulate );
				GL.Color4( gl_state.inverse_intensity, gl_state.inverse_intensity, gl_state.inverse_intensity, 1F );
				EmitWaterPolys( fa );
				GL_TexEnv( ( Int32 ) All.Replace );
				return;
			}
			else
			{
				GL_Bind( image.texnum );
				GL_TexEnv( ( Int32 ) All.Replace );
			}

			if ( ( fa.texinfo.flags & Defines.SURF_FLOWING ) != 0 )
				DrawGLFlowingPoly( fa.polys );
			else
				DrawGLPoly( fa.polys );
			var gotoDynamic = false;
			Int32 maps;
			for ( maps = 0; maps < Defines.MAXLIGHTMAPS && fa.styles[maps] != ( Byte ) 255; maps++ )
			{
				if ( r_newrefdef.lightstyles[fa.styles[maps] & 0xFF].white != fa.cached_light[maps] )
				{
					gotoDynamic = true;
					break;
				}
			}

			if ( maps == 4 )
				maps--;
			var is_dynamic = false;
			if ( gotoDynamic || ( fa.dlightframe == r_framecount ) )
			{
				if ( gl_dynamic.value != 0 )
				{
					if ( ( fa.texinfo.flags & ( Defines.SURF_SKY | Defines.SURF_TRANS33 | Defines.SURF_TRANS66 | Defines.SURF_WARP ) ) == 0 )
					{
						is_dynamic = true;
					}
				}
			}

			if ( is_dynamic )
			{
				if ( ( ( fa.styles[maps] & 0xFF ) >= 32 || fa.styles[maps] == 0 ) && ( fa.dlightframe != r_framecount ) )
				{
					Int32 smax, tmax;
					smax = ( fa.extents[0] >> 4 ) + 1;
					tmax = ( fa.extents[1] >> 4 ) + 1;
					R_BuildLightMap( fa, temp2, smax );
					R_SetCacheState( fa );
					GL_Bind( gl_state.lightmap_textures + fa.lightmaptexturenum );
					new Pinnable( temp2, ( ptr ) =>
					{
						GL.TexSubImage2D( TextureTarget.Texture2D, 0, fa.light_s, fa.light_t, smax, tmax, GL_LIGHTMAP_FORMAT, PixelType.UnsignedByte, ptr );
					} );
					fa.lightmapchain = gl_lms.lightmap_surfaces[fa.lightmaptexturenum];
					gl_lms.lightmap_surfaces[fa.lightmaptexturenum] = fa;
				}
				else
				{
					fa.lightmapchain = gl_lms.lightmap_surfaces[0];
					gl_lms.lightmap_surfaces[0] = fa;
				}
			}
			else
			{
				fa.lightmapchain = gl_lms.lightmap_surfaces[fa.lightmaptexturenum];
				gl_lms.lightmap_surfaces[fa.lightmaptexturenum] = fa;
			}
		}

		public override void R_DrawAlphaSurfaces( )
		{
			r_world_matrix.Clear();
			GL.LoadMatrix( r_world_matrix.Array );
			GL.Enable( EnableCap.Blend );
			GL_TexEnv( ( Int32 ) All.Modulate );
			var intens = gl_state.inverse_intensity;
			GL.InterleavedArrays( InterleavedArrayFormat.T2fV3f, glpoly_t.BYTE_STRIDE, globalPolygonInterleavedBuf.Array );
			for ( msurface_t s = r_alpha_surfaces; s != null; s = s.texturechain )
			{
				GL_Bind( s.texinfo.image.texnum );
				c_brush_polys++;
				if ( ( s.texinfo.flags & Defines.SURF_TRANS33 ) != 0 )
					GL.Color4( intens, intens, intens, 0.33F );
				else if ( ( s.texinfo.flags & Defines.SURF_TRANS66 ) != 0 )
					GL.Color4( intens, intens, intens, 0.66F );
				else
					GL.Color4( intens, intens, intens, 1 );
				if ( ( s.flags & Defines.SURF_DRAWTURB ) != 0 )
					EmitWaterPolys( s );
				else if ( ( s.texinfo.flags & Defines.SURF_FLOWING ) != 0 )
					DrawGLFlowingPoly( s.polys );
				else
					DrawGLPoly( s.polys );
			}

			GL_TexEnv( ( Int32 ) All.Replace );
			GL.Color4( 1, 1, 1, 1 );
			GL.Disable( EnableCap.Blend );
			r_alpha_surfaces = null;
		}

		public virtual void DrawTextureChains( )
		{
			c_visible_textures = 0;
			msurface_t s;
			image_t image;
			Int32 i;
			for ( i = 0; i < numgltextures; i++ )
			{
				image = gltextures[i];
				if ( image.registration_sequence == 0 )
					continue;
				if ( image.texturechain == null )
					continue;
				c_visible_textures++;
				for ( s = image.texturechain; s != null; s = s.texturechain )
				{
					if ( ( s.flags & Defines.SURF_DRAWTURB ) == 0 )
						R_RenderBrushPoly( s );
				}
			}

			GL_EnableMultitexture( false );
			for ( i = 0; i < numgltextures; i++ )
			{
				image = gltextures[i];
				if ( image.registration_sequence == 0 )
					continue;
				s = image.texturechain;
				if ( s == null )
					continue;
				for ( ; s != null; s = s.texturechain )
				{
					if ( ( s.flags & Defines.SURF_DRAWTURB ) != 0 )
						R_RenderBrushPoly( s );
				}

				image.texturechain = null;
			}

			GL_TexEnv( ( Int32 ) All.Replace );
		}

		private readonly Int32Buffer temp = Lib.NewInt32Buffer( 128 * 128, ByteOrder.LittleEndian );
		public virtual void GL_RenderLightmappedPoly( msurface_t surf )
		{
			var gotoDynamic = false;
			Int32 map;
			for ( map = 0; map < Defines.MAXLIGHTMAPS && ( surf.styles[map] != ( Byte ) 255 ); map++ )
			{
				if ( r_newrefdef.lightstyles[surf.styles[map] & 0xFF].white != surf.cached_light[map] )
				{
					gotoDynamic = true;
					break;
				}
			}

			if ( map == 4 )
				map--;
			var is_dynamic = false;
			if ( gotoDynamic || ( surf.dlightframe == r_framecount ) )
			{
				if ( gl_dynamic.value != 0 )
				{
					if ( ( surf.texinfo.flags & ( Defines.SURF_SKY | Defines.SURF_TRANS33 | Defines.SURF_TRANS66 | Defines.SURF_WARP ) ) == 0 )
					{
						is_dynamic = true;
					}
				}
			}

			glpoly_t p;
			image_t image = R_TextureAnimation( surf.texinfo );
			var lmtex = surf.lightmaptexturenum;
			if ( is_dynamic )
			{
				Int32 smax, tmax;
				if ( ( ( surf.styles[map] & 0xFF ) >= 32 || surf.styles[map] == 0 ) && ( surf.dlightframe != r_framecount ) )
				{
					smax = ( surf.extents[0] >> 4 ) + 1;
					tmax = ( surf.extents[1] >> 4 ) + 1;
					R_BuildLightMap( surf, temp, smax );
					R_SetCacheState( surf );
					GL_MBind( TextureUnit.Texture1, gl_state.lightmap_textures + surf.lightmaptexturenum );
					lmtex = surf.lightmaptexturenum;
					new Pinnable( temp, ( ptr ) =>
					{
						GL.TexSubImage2D( TextureTarget.Texture2D, 0, surf.light_s, surf.light_t, smax, tmax, GL_LIGHTMAP_FORMAT, PixelType.UnsignedByte, ptr );
					} );
				}
				else
				{
					smax = ( surf.extents[0] >> 4 ) + 1;
					tmax = ( surf.extents[1] >> 4 ) + 1;
					R_BuildLightMap( surf, temp, smax );
					GL_MBind( TextureUnit.Texture1, gl_state.lightmap_textures + 0 );
					lmtex = 0;
					new Pinnable( temp, ( ptr ) =>
					{
						GL.TexSubImage2D( TextureTarget.Texture2D, 0, surf.light_s, surf.light_t, smax, tmax, GL_LIGHTMAP_FORMAT, PixelType.UnsignedByte, ptr );
					} );
				}

				c_brush_polys++;
				GL_MBind( TextureUnit.Texture0, image.texnum );
				GL_MBind( TextureUnit.Texture1, gl_state.lightmap_textures + lmtex );
				if ( ( surf.texinfo.flags & Defines.SURF_FLOWING ) != 0 )
				{
					Single scroll;
					scroll = -64 * ( ( r_newrefdef.time / 40F ) - ( Int32 ) ( r_newrefdef.time / 40F ) );
					if ( scroll == 0F )
						scroll = -64F;
					for ( p = surf.polys; p != null; p = p.chain )
					{
						p.BeginScrolling( scroll );
						GL.DrawArrays( PrimitiveType.Polygon, p.pos, p.numverts );
						p.EndScrolling();
					}
				}
				else
				{
					for ( p = surf.polys; p != null; p = p.chain )
					{
						GL.DrawArrays( PrimitiveType.Polygon, p.pos, p.numverts );
					}
				}
			}
			else
			{
				c_brush_polys++;
				GL_MBind( TextureUnit.Texture0, image.texnum );
				GL_MBind( TextureUnit.Texture1, gl_state.lightmap_textures + lmtex );
				if ( ( surf.texinfo.flags & Defines.SURF_FLOWING ) != 0 )
				{
					Single scroll;
					scroll = -64 * ( ( r_newrefdef.time / 40F ) - ( Int32 ) ( r_newrefdef.time / 40F ) );
					if ( scroll == 0 )
						scroll = -64F;
					for ( p = surf.polys; p != null; p = p.chain )
					{
						p.BeginScrolling( scroll );
						GL.DrawArrays( PrimitiveType.Polygon, p.pos, p.numverts );
						p.EndScrolling();
					}
				}
				else
				{
					for ( p = surf.polys; p != null; p = p.chain )
					{
						GL.DrawArrays( PrimitiveType.Polygon, p.pos, p.numverts );
					}
				}
			}
		}

		public virtual void R_DrawInlineBModel( )
		{
			if ( gl_flashblend.value == 0 )
			{
				dlight_t lt;
				for ( var k = 0; k < r_newrefdef.num_dlights; k++ )
				{
					lt = r_newrefdef.dlights[k];
					R_MarkLights( lt, 1 << k, currentmodel.nodes[currentmodel.firstnode] );
				}
			}

			var psurfp = currentmodel.firstmodelsurface;
			msurface_t[] surfaces = currentmodel.surfaces;
			if ( ( currententity.flags & Defines.RF_TRANSLUCENT ) != 0 )
			{
				GL.Enable( EnableCap.Blend );
				GL.Color4( 1, 1, 1, 0.25F );
				GL_TexEnv( ( Int32 ) All.Modulate );
			}

			msurface_t psurf;
			cplane_t pplane;
			Single dot;
			for ( var i = 0; i < currentmodel.nummodelsurfaces; i++ )
			{
				psurf = surfaces[psurfp++];
				pplane = psurf.plane;
				dot = Math3D.DotProduct( modelorg, pplane.normal ) - pplane.dist;
				if ( ( ( psurf.flags & Defines.SURF_PLANEBACK ) != 0 && ( dot < -BACKFACE_EPSILON ) ) || ( ( psurf.flags & Defines.SURF_PLANEBACK ) == 0 && ( dot > BACKFACE_EPSILON ) ) )
				{
					if ( ( psurf.texinfo.flags & ( Defines.SURF_TRANS33 | Defines.SURF_TRANS66 ) ) != 0 )
					{
						psurf.texturechain = r_alpha_surfaces;
						r_alpha_surfaces = psurf;
					}
					else if ( ( psurf.flags & Defines.SURF_DRAWTURB ) == 0 )
					{
						GL_RenderLightmappedPoly( psurf );
					}
					else
					{
						GL_EnableMultitexture( false );
						R_RenderBrushPoly( psurf );
						GL_EnableMultitexture( true );
					}
				}
			}

			if ( ( currententity.flags & Defines.RF_TRANSLUCENT ) != 0 )
			{
				GL.Disable( EnableCap.Blend );
				GL.Color4( 1, 1, 1, 1 );
				GL_TexEnv( ( Int32 ) All.Replace );
			}
		}

		private readonly Single[] mins = new Single[] { 0, 0, 0 };
		private readonly Single[] maxs = new Single[] { 0, 0, 0 };
		private readonly Single[] org = new Single[] { 0, 0, 0 };
		private readonly Single[] forward = new Single[] { 0, 0, 0 };
		private readonly Single[] right = new Single[] { 0, 0, 0 };
		private readonly Single[] up = new Single[] { 0, 0, 0 };
		public override void R_DrawBrushModel( entity_t e )
		{
			if ( currentmodel.nummodelsurfaces == 0 )
				return;
			currententity = e;
			gl_state.currenttextures[0] = gl_state.currenttextures[1] = -1;
			System.Boolean rotated;
			if ( e.angles[0] != 0 || e.angles[1] != 0 || e.angles[2] != 0 )
			{
				rotated = true;
				for ( var i = 0; i < 3; i++ )
				{
					mins[i] = e.origin[i] - currentmodel.radius;
					maxs[i] = e.origin[i] + currentmodel.radius;
				}
			}
			else
			{
				rotated = false;
				Math3D.VectorAdd( e.origin, currentmodel.mins, mins );
				Math3D.VectorAdd( e.origin, currentmodel.maxs, maxs );
			}

			if ( R_CullBox( mins, maxs ) )
				return;
			GL.Color3( 1, 1, 1 );
			Math3D.VectorSubtract( r_newrefdef.vieworg, e.origin, modelorg );
			if ( rotated )
			{
				Math3D.VectorCopy( modelorg, org );
				Math3D.AngleVectors( e.angles, forward, right, up );
				modelorg[0] = Math3D.DotProduct( org, forward );
				modelorg[1] = -Math3D.DotProduct( org, right );
				modelorg[2] = Math3D.DotProduct( org, up );
			}

			GL.PushMatrix();
			e.angles[0] = -e.angles[0];
			e.angles[2] = -e.angles[2];
			R_RotateForEntity( e );
			e.angles[0] = -e.angles[0];
			e.angles[2] = -e.angles[2];
			GL_EnableMultitexture( true );
			GL_SelectTexture( TextureUnit.Texture0 );
			GL_TexEnv( ( Int32 ) All.Replace );
			GL.InterleavedArrays( InterleavedArrayFormat.T2fV3f, glpoly_t.BYTE_STRIDE, globalPolygonInterleavedBuf.Array );
			GL_SelectTexture( TextureUnit.Texture1 );
			GL_TexEnv( ( Int32 ) All.Modulate );

			new Pinnable( globalPolygonTexCoord1Buf, ( ptr ) =>
			{
				GL.TexCoordPointer( 2, TexCoordPointerType.Float, glpoly_t.BYTE_STRIDE, ptr );
			} );
			GL.EnableClientState( ArrayCap.TextureCoordArray );
			R_DrawInlineBModel();
			GL.ClientActiveTexture( TextureUnit.Texture1 );
			GL.DisableClientState( ArrayCap.TextureCoordArray );
			GL_EnableMultitexture( false );
			GL.PopMatrix();
		}

		public virtual void R_RecursiveWorldNode( mnode_t node )
		{
			if ( node.contents == Defines.CONTENTS_SOLID )
				return;
			if ( node.visframe != r_visframecount )
				return;
			if ( R_CullBox( node.mins, node.maxs ) )
				return;
			Int32 c;
			msurface_t mark;
			if ( node.contents != -1 )
			{
				mleaf_t pleaf = ( mleaf_t ) node;
				if ( r_newrefdef.areabits != null )
				{
					if ( ( ( r_newrefdef.areabits[pleaf.area >> 3] & 0xFF ) & ( 1 << ( pleaf.area & 7 ) ) ) == 0 )
						return;
				}

				var markp = 0;
				mark = pleaf.GetMarkSurface( markp );
				c = pleaf.nummarksurfaces;
				if ( c != 0 )
				{
					do
					{
						mark.visframe = r_framecount;
						mark = pleaf.GetMarkSurface( ++markp );
					}
					while ( --c != 0 );
				}

				return;
			}

			cplane_t plane = node.plane;
			Single dot;
			switch ( plane.type )

			{
				case Defines.PLANE_X:
					dot = modelorg[0] - plane.dist;
					break;
				case Defines.PLANE_Y:
					dot = modelorg[1] - plane.dist;
					break;
				case Defines.PLANE_Z:
					dot = modelorg[2] - plane.dist;
					break;
				default:
					dot = Math3D.DotProduct( modelorg, plane.normal ) - plane.dist;
					break;
			}

			Int32 side, sidebit;
			if ( dot >= 0F )
			{
				side = 0;
				sidebit = 0;
			}
			else
			{
				side = 1;
				sidebit = Defines.SURF_PLANEBACK;
			}

			R_RecursiveWorldNode( node.children[side] );
			msurface_t surf;
			image_t image;
			for ( c = 0; c < node.numsurfaces; c++ )
			{
				surf = r_worldmodel.surfaces[node.firstsurface + c];
				if ( surf.visframe != r_framecount )
					continue;
				if ( ( surf.flags & Defines.SURF_PLANEBACK ) != sidebit )
					continue;
				if ( ( surf.texinfo.flags & Defines.SURF_SKY ) != 0 )
				{
					R_AddSkySurface( surf );
				}
				else if ( ( surf.texinfo.flags & ( Defines.SURF_TRANS33 | Defines.SURF_TRANS66 ) ) != 0 )
				{
					surf.texturechain = r_alpha_surfaces;
					r_alpha_surfaces = surf;
				}
				else
				{
					if ( ( surf.flags & Defines.SURF_DRAWTURB ) == 0 )
					{
						GL_RenderLightmappedPoly( surf );
					}
					else
					{
						image = R_TextureAnimation( surf.texinfo );
						surf.texturechain = image.texturechain;
						image.texturechain = surf;
					}
				}
			}

			R_RecursiveWorldNode( node.children[1 - side] );
		}

		private readonly entity_t worldEntity = new entity_t();
		public override void R_DrawWorld( )
		{
			if ( r_drawworld.value == 0 )
				return;
			if ( ( r_newrefdef.rdflags & Defines.RDF_NOWORLDMODEL ) != 0 )
				return;
			currentmodel = r_worldmodel;
			Math3D.VectorCopy( r_newrefdef.vieworg, modelorg );
			entity_t ent = worldEntity;
			ent.Clear();
			ent.frame = ( Int32 ) ( r_newrefdef.time * 2 );
			currententity = ent;
			gl_state.currenttextures[0] = gl_state.currenttextures[1] = -1;
			GL.Color3( 1, 1, 1 );
			R_ClearSkyBox();
			GL_EnableMultitexture( true );
			GL_SelectTexture( TextureUnit.Texture0 );
			GL_TexEnv( ( Int32 ) All.Replace );
			GL.InterleavedArrays( InterleavedArrayFormat.T2fV3f, glpoly_t.BYTE_STRIDE, globalPolygonInterleavedBuf.Array );
			GL_SelectTexture( TextureUnit.Texture1 );
			new Pinnable( globalPolygonTexCoord1Buf, ( ptr ) =>
			{
				GL.TexCoordPointer( 2, TexCoordPointerType.Float, glpoly_t.BYTE_STRIDE, ptr );
			} );
			GL.EnableClientState( ArrayCap.TextureCoordArray );
			if ( gl_lightmap.value != 0 )
				GL_TexEnv( ( Int32 ) All.Replace );
			else
				GL_TexEnv( ( Int32 ) All.Modulate );
			R_RecursiveWorldNode( r_worldmodel.nodes[0] );
			GL.ClientActiveTexture( TextureUnit.Texture1 );
			GL.DisableClientState( ArrayCap.TextureCoordArray );
			GL_EnableMultitexture( false );
			DrawTextureChains();
			R_DrawSkyBox();
			R_DrawTriangleOutlines();
		}

		readonly Byte[] fatvis = new Byte[Defines.MAX_MAP_LEAFS / 8];
		public override void R_MarkLeaves( )
		{
			if ( r_oldviewcluster == r_viewcluster && r_oldviewcluster2 == r_viewcluster2 && r_novis.value == 0 && r_viewcluster != -1 )
				return;
			if ( gl_lockpvs.value != 0 )
				return;
			r_visframecount++;
			r_oldviewcluster = r_viewcluster;
			r_oldviewcluster2 = r_viewcluster2;
			Int32 i;
			if ( r_novis.value != 0 || r_viewcluster == -1 || r_worldmodel.vis == null )
			{
				for ( i = 0; i < r_worldmodel.numleafs; i++ )
					r_worldmodel.leafs[i].visframe = r_visframecount;
				for ( i = 0; i < r_worldmodel.numnodes; i++ )
					r_worldmodel.nodes[i].visframe = r_visframecount;
				return;
			}

			Byte[] vis = Mod_ClusterPVS( r_viewcluster, r_worldmodel );
			Int32 c;
			if ( r_viewcluster2 != r_viewcluster )
			{
				System.Array.Copy( vis, 0, fatvis, 0, ( r_worldmodel.numleafs + 7 ) >> 3 );
				vis = Mod_ClusterPVS( r_viewcluster2, r_worldmodel );
				c = ( r_worldmodel.numleafs + 31 ) >> 5;
				c <<= 2;
				for ( var k = 0; k < c; k += 4 )
				{
					fatvis[k] |= vis[k];
					fatvis[k + 1] |= vis[k + 1];
					fatvis[k + 2] |= vis[k + 2];
					fatvis[k + 3] |= vis[k + 3];
				}

				vis = fatvis;
			}

			mnode_t node;
			mleaf_t leaf;
			Int32 cluster;
			for ( i = 0; i < r_worldmodel.numleafs; i++ )
			{
				leaf = r_worldmodel.leafs[i];
				cluster = leaf.cluster;
				if ( cluster == -1 )
					continue;
				if ( ( ( vis[cluster >> 3] & 0xFF ) & ( 1 << ( cluster & 7 ) ) ) != 0 )
				{
					node = ( mnode_t ) leaf;
					do
					{
						if ( node.visframe == r_visframecount )
							break;
						node.visframe = r_visframecount;
						node = node.parent;
					}
					while ( node != null );
				}
			}
		}

		public virtual void LM_InitBlock( )
		{
			gl_lms.allocated.Fill( 0 );
		}

		public virtual void LM_UploadBlock( System.Boolean dynamic )
		{
			var texture = ( dynamic ) ? 0 : gl_lms.current_lightmap_texture;
			GL_Bind( gl_state.lightmap_textures + texture );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( Int32 ) All.Linear );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( Int32 ) All.Linear );
			gl_lms.lightmap_buffer.Rewind();

			new Pinnable( gl_lms.lightmap_buffer, ( ptr ) =>
			{
				if ( dynamic )
				{
					var height = 0;
					for ( var i = 0; i < BLOCK_WIDTH; i++ )
					{
						if ( gl_lms.allocated[i] > height )
							height = gl_lms.allocated[i];
					}

					GL.TexSubImage2D( TextureTarget.Texture2D, 0, 0, 0, BLOCK_WIDTH, height, GL_LIGHTMAP_FORMAT, PixelType.UnsignedByte, ptr );
				}
				else
				{
					GL.TexImage2D( TextureTarget.Texture2D, 0, gl_lms.internal_format, BLOCK_WIDTH, BLOCK_HEIGHT, 0, GL_LIGHTMAP_FORMAT, PixelType.UnsignedByte, ptr );
					if ( ++gl_lms.current_lightmap_texture == MAX_LIGHTMAPS )
						Com.Error( Defines.ERR_DROP, "LM_UploadBlock() - MAX_LIGHTMAPS exceeded\\n" );
				}
			} );
		}

		public virtual System.Boolean LM_AllocBlock( Int32 w, Int32 h, pos_t pos )
		{
			var best = BLOCK_HEIGHT;
			var x = pos.x;
			Int32 best2;
			Int32 i, j;
			for ( i = 0; i < BLOCK_WIDTH - w; i++ )
			{
				best2 = 0;
				for ( j = 0; j < w; j++ )
				{
					if ( gl_lms.allocated[i + j] >= best )
						break;
					if ( gl_lms.allocated[i + j] > best2 )
						best2 = gl_lms.allocated[i + j];
				}

				if ( j == w )
				{
					pos.x = x = i;
					pos.y = best = best2;
				}
			}

			if ( best + h > BLOCK_HEIGHT )
				return false;
			for ( i = 0; i < w; i++ )
				gl_lms.allocated[x + i] = best + h;
			return true;
		}

		public virtual void GL_BuildPolygonFromSurface( msurface_t fa )
		{
			medge_t[] pedges = currentmodel.edges;
			var lnumverts = fa.numedges;
			glpoly_t poly = Polygon.Create( lnumverts );
			poly.next = fa.polys;
			poly.flags = fa.flags;
			fa.polys = poly;
			Int32 lindex;
			Single[] vec;
			medge_t r_pedge;
			Single s, t;
			for ( var i = 0; i < lnumverts; i++ )
			{
				lindex = currentmodel.surfedges[fa.firstedge + i];
				if ( lindex > 0 )
				{
					r_pedge = pedges[lindex];
					vec = currentmodel.vertexes[r_pedge.v[0]].position;
				}
				else
				{
					r_pedge = pedges[-lindex];
					vec = currentmodel.vertexes[r_pedge.v[1]].position;
				}

				s = Math3D.DotProduct( vec, fa.texinfo.vecs[0] ) + fa.texinfo.vecs[0][3];
				s /= fa.texinfo.image.width;
				t = Math3D.DotProduct( vec, fa.texinfo.vecs[1] ) + fa.texinfo.vecs[1][3];
				t /= fa.texinfo.image.height;
				poly.X( i, vec[0] );
				poly.Y( i, vec[1] );
				poly.Z( i, vec[2] );
				poly.S1( i, s );
				poly.T1( i, t );
				s = Math3D.DotProduct( vec, fa.texinfo.vecs[0] ) + fa.texinfo.vecs[0][3];
				s -= fa.texturemins[0];
				s += fa.light_s * 16;
				s += 8;
				s /= BLOCK_WIDTH * 16;
				t = Math3D.DotProduct( vec, fa.texinfo.vecs[1] ) + fa.texinfo.vecs[1][3];
				t -= fa.texturemins[1];
				t += fa.light_t * 16;
				t += 8;
				t /= BLOCK_HEIGHT * 16;
				poly.S2( i, s );
				poly.T2( i, t );
			}
		}

		public virtual void GL_CreateSurfaceLightmap( msurface_t surf )
		{
			if ( ( surf.flags & ( Defines.SURF_DRAWSKY | Defines.SURF_DRAWTURB ) ) != 0 )
				return;
			var smax = ( surf.extents[0] >> 4 ) + 1;
			var tmax = ( surf.extents[1] >> 4 ) + 1;
			pos_t lightPos = new pos_t( surf.light_s, surf.light_t );
			if ( !LM_AllocBlock( smax, tmax, lightPos ) )
			{
				LM_UploadBlock( false );
				LM_InitBlock();
				lightPos = new pos_t( surf.light_s, surf.light_t );
				if ( !LM_AllocBlock( smax, tmax, lightPos ) )
				{
					Com.Error( Defines.ERR_FATAL, "Consecutive calls to LM_AllocBlock(" + smax + "," + tmax + ") failed\\n" );
				}
			}

			surf.light_s = lightPos.x;
			surf.light_t = lightPos.y;
			surf.lightmaptexturenum = gl_lms.current_lightmap_texture;
			Int32Buffer base_renamed = gl_lms.lightmap_buffer;
			base_renamed.Position = surf.light_t * BLOCK_WIDTH + surf.light_s;
			R_SetCacheState( surf );
			R_BuildLightMap( surf, base_renamed.Slice(), BLOCK_WIDTH );
		}

		lightstyle_t[] lightstyles;
		private readonly Int32Buffer dummy = Lib.NewInt32Buffer( 128 * 128 );
		public virtual void GL_BeginBuildingLightmaps( model_t m )
		{
			Int32 i;
			if ( lightstyles == null )
			{
				lightstyles = new lightstyle_t[Defines.MAX_LIGHTSTYLES];
				for ( i = 0; i < lightstyles.Length; i++ )
				{
					lightstyles[i] = new lightstyle_t();
				}
			}

			gl_lms.allocated.Fill( 0 );
			r_framecount = 1;
			GL_EnableMultitexture( true );
			GL_SelectTexture( TextureUnit.Texture1 );
			for ( i = 0; i < Defines.MAX_LIGHTSTYLES; i++ )
			{
				lightstyles[i].rgb[0] = 1;
				lightstyles[i].rgb[1] = 1;
				lightstyles[i].rgb[2] = 1;
				lightstyles[i].white = 3;
			}

			r_newrefdef.lightstyles = lightstyles;
			if ( gl_state.lightmap_textures == 0 )
			{
				gl_state.lightmap_textures = TEXNUM_LIGHTMAPS;
			}

			gl_lms.current_lightmap_texture = 1;
			var format = gl_monolightmap.string_renamed.ToUpper()[0];
			if ( format == 'A' )
			{
				gl_lms.internal_format = gl_tex_alpha_format;
			}
			else if ( format == 'C' )
			{
				gl_lms.internal_format = gl_tex_alpha_format;
			}
			else if ( format == 'I' )
			{
				gl_lms.internal_format = PixelInternalFormat.Intensity8;
			}
			else if ( format == 'L' )
			{
				gl_lms.internal_format = PixelInternalFormat.Luminance8;
			}
			else
			{
				gl_lms.internal_format = gl_tex_solid_format;
			}

			GL_Bind( gl_state.lightmap_textures + 0 );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( Int32 ) All.Linear );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( Int32 ) All.Linear );
			new Pinnable( dummy, ( ptr ) =>
			{
				GL.TexImage2D( TextureTarget.Texture2D, 0, gl_lms.internal_format, BLOCK_WIDTH, BLOCK_HEIGHT, 0, GL_LIGHTMAP_FORMAT, PixelType.UnsignedByte, ptr );
			} );
		}

		public virtual void GL_EndBuildingLightmaps( )
		{
			LM_UploadBlock( false );
			GL_EnableMultitexture( false );
		}

		static SingleBuffer globalPolygonInterleavedBuf = Polygon.GetInterleavedBuffer();
		static SingleBuffer globalPolygonTexCoord1Buf = null;
		static Surf( )
		{
			globalPolygonInterleavedBuf.Position = glpoly_t.STRIDE - 2;
			globalPolygonTexCoord1Buf = globalPolygonInterleavedBuf.Slice();
			globalPolygonInterleavedBuf.Position = 0;
		}
	}
}
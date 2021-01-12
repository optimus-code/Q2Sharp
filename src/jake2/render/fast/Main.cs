using J2N.IO;
using Jake2.Client;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Jake2.Render.Fast
{
	public abstract class Main : Base
	{
		public static Int32[] d_8to24table = new Int32[256];
		public Int32 c_visible_lightmaps;
		public Int32 c_visible_textures;
		public Int32 registration_sequence;
		public System.Boolean qglColorTableEXT = false;
		public System.Boolean qglActiveTextureARB = false;
		public System.Boolean qglPointParameterfEXT = false;
		public System.Boolean qglLockArraysEXT = false;
		public System.Boolean qwglSwapIntervalEXT = false;
		protected abstract void Draw_GetPalette( );
		public abstract void GL_ImageList_f( );
		public abstract void GL_SetTexturePalette( Int32[] palette );
		public abstract void GL_Strings_f( );
		public abstract void Mod_Modellist_f( );
		public abstract mleaf_t Mod_PointInLeaf( Single[] point, model_t model );
		public abstract void GL_SetDefaultState( );
		public abstract void GL_InitImages( );
		public abstract void Mod_Init( );
		public abstract void R_InitParticleTexture( );
		public abstract void R_DrawAliasModel( entity_t e );
		public abstract void R_DrawBrushModel( entity_t e );
		public abstract void Draw_InitLocal( );
		public abstract void R_LightPoint( Single[] p, Single[] color );
		public abstract void R_PushDlights( );
		public abstract void R_MarkLeaves( );
		public abstract void R_DrawWorld( );
		public abstract void R_RenderDlights( );
		public abstract void R_DrawAlphaSurfaces( );
		public abstract void Mod_FreeAll( );
		public abstract void GL_ShutdownImages( );
		public abstract void GL_Bind( Int32 texnum );
		public abstract void GL_TexEnv( Int32 mode );
		public abstract void GL_TextureMode( String string_renamed );
		public abstract void GL_TextureAlphaMode( String string_renamed );
		public abstract void GL_TextureSolidMode( String string_renamed );
		public abstract void GL_UpdateSwapInterval( );
		Int32 TEXTURE0 = ( Int32 ) TextureUnit.Texture0;
		Int32 TEXTURE1 = ( Int32 ) TextureUnit.Texture1;
		public model_t r_worldmodel;
		public Single gldepthmin, gldepthmax;
		public glconfig_t gl_config = new glconfig_t();
		public glstate_t gl_state = new glstate_t();
		public image_t r_notexture;
		public image_t r_particletexture;
		public entity_t currententity;
		public model_t currentmodel;
		public cplane_t[] frustum = new[] { new cplane_t(), new cplane_t(), new cplane_t(), new cplane_t() };
		public Int32 r_visframecount;
		public Int32 r_framecount;
		public Int32 c_brush_polys, c_alias_polys;
		public Single[] v_blend = new Single[] { 0, 0, 0, 0 };
		public Single[] vup = new Single[] { 0, 0, 0 };
		public Single[] vpn = new Single[] { 0, 0, 0 };
		public Single[] vright = new Single[] { 0, 0, 0 };
		public Single[] r_origin = new Single[] { 0, 0, 0 };
		public SingleBuffer r_world_matrix = Lib.NewSingleBuffer( 16 );
		public Single[] r_base_world_matrix = new Single[16];
		public refdef_t r_newrefdef = new refdef_t();
		public Int32 r_viewcluster, r_viewcluster2, r_oldviewcluster, r_oldviewcluster2;
		public cvar_t r_norefresh;
		public cvar_t r_drawentities;
		public cvar_t r_drawworld;
		public cvar_t r_speeds;
		public cvar_t r_fullbright;
		public cvar_t r_novis;
		public cvar_t r_nocull;
		public cvar_t r_lerpmodels;
		public cvar_t r_lefthand;
		public cvar_t r_lightlevel;
		public cvar_t gl_nosubimage;
		public cvar_t gl_allow_software;
		public cvar_t gl_vertex_arrays;
		public cvar_t gl_particle_min_size;
		public cvar_t gl_particle_max_size;
		public cvar_t gl_particle_size;
		public cvar_t gl_particle_att_a;
		public cvar_t gl_particle_att_b;
		public cvar_t gl_particle_att_c;
		public cvar_t gl_ext_swapinterval;
		public cvar_t gl_ext_palettedtexture;
		public cvar_t gl_ext_multitexture;
		public cvar_t gl_ext_pointparameters;
		public cvar_t gl_ext_compiled_vertex_array;
		public cvar_t gl_log;
		public cvar_t gl_bitdepth;
		public cvar_t gl_drawbuffer;
		public cvar_t gl_driver;
		public cvar_t gl_lightmap;
		public cvar_t gl_shadows;
		public cvar_t gl_mode;
		public cvar_t gl_dynamic;
		public cvar_t gl_monolightmap;
		public cvar_t gl_modulate;
		public cvar_t gl_nobind;
		public cvar_t gl_round_down;
		public cvar_t gl_picmip;
		public cvar_t gl_skymip;
		public cvar_t gl_showtris;
		public cvar_t gl_ztrick;
		public cvar_t gl_finish;
		public cvar_t gl_clear;
		public cvar_t gl_cull;
		public cvar_t gl_polyblend;
		public cvar_t gl_flashblend;
		public cvar_t gl_playermip;
		public cvar_t gl_saturatelighting;
		public cvar_t gl_swapinterval;
		public cvar_t gl_texturemode;
		public cvar_t gl_texturealphamode;
		public cvar_t gl_texturesolidmode;
		public cvar_t gl_lockpvs;
		public cvar_t gl_3dlabs_broken;
		public cvar_t vid_gamma;
		public cvar_t vid_ref;
		public System.Boolean R_CullBox( Single[] mins, Single[] maxs )
		{
			if ( r_nocull.value != 0 )
				return false;
			for ( var i = 0; i < 4; i++ )
			{
				if ( Math3D.BoxOnPlaneSide( mins, maxs, frustum[i] ) == 2 )
					return true;
			}

			return false;
		}

		public void R_RotateForEntity( entity_t e )
		{
			GL.Translate( e.origin[0], e.origin[1], e.origin[2] );
			GL.Rotate( e.angles[1], 0, 0, 1 );
			GL.Rotate( -e.angles[0], 0, 1, 0 );
			GL.Rotate( -e.angles[2], 1, 0, 0 );
		}

		private readonly Single[] point = new Single[] { 0, 0, 0 };
		public virtual void R_DrawSpriteModel( entity_t e )
		{
			var alpha = 1F;
			qfiles.dsprframe_t frame;
			qfiles.dsprite_t psprite;
			psprite = ( qfiles.dsprite_t ) currentmodel.extradata;
			e.frame %= psprite.numframes;
			frame = psprite.frames[e.frame];
			if ( ( e.flags & Defines.RF_TRANSLUCENT ) != 0 )
				alpha = e.alpha;
			if ( alpha != 1F )
				GL.Enable( EnableCap.Blend );
			GL.Color4( 1, 1, 1, alpha );
			GL_Bind( currentmodel.skins[e.frame].texnum );
			GL_TexEnv( ( Int32 ) All.Modulate );
			if ( alpha == 1 )
				GL.Enable( EnableCap.AlphaTest );
			else
				GL.Disable( EnableCap.AlphaTest );
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( 0, 1 );
			Math3D.VectorMA( e.origin, -frame.origin_y, vup, point );
			Math3D.VectorMA( point, -frame.origin_x, vright, point );
			GL.Vertex3( point[0], point[1], point[2] );
			GL.TexCoord2( 0, 0 );
			Math3D.VectorMA( e.origin, frame.height - frame.origin_y, vup, point );
			Math3D.VectorMA( point, -frame.origin_x, vright, point );
			GL.Vertex3( point[0], point[1], point[2] );
			GL.TexCoord2( 1, 0 );
			Math3D.VectorMA( e.origin, frame.height - frame.origin_y, vup, point );
			Math3D.VectorMA( point, frame.width - frame.origin_x, vright, point );
			GL.Vertex3( point[0], point[1], point[2] );
			GL.TexCoord2( 1, 1 );
			Math3D.VectorMA( e.origin, -frame.origin_y, vup, point );
			Math3D.VectorMA( point, frame.width - frame.origin_x, vright, point );
			GL.Vertex3( point[0], point[1], point[2] );
			GL.End();
			GL.Disable( EnableCap.AlphaTest );
			GL_TexEnv( ( Int32 ) All.Replace );
			if ( alpha != 1F )
				GL.Disable( EnableCap.Blend );
			GL.Color4( 1, 1, 1, 1 );
		}

		private readonly Single[] shadelight = new Single[] { 0, 0, 0 };
		public virtual void R_DrawNullModel( )
		{
			if ( ( currententity.flags & Defines.RF_FULLBRIGHT ) != 0 )
			{
				shadelight[0] = shadelight[1] = shadelight[2] = 0F;
				shadelight[2] = 0.8F;
			}
			else
			{
				R_LightPoint( currententity.origin, shadelight );
			}

			GL.PushMatrix();
			R_RotateForEntity( currententity );
			GL.Disable( EnableCap.Texture2D );
			GL.Color3( shadelight[0], shadelight[1], shadelight[2] );
			GL.Begin( PrimitiveType.TriangleFan );
			GL.Vertex3( 0, 0, -16 );
			Int32 i;
			for ( i = 0; i <= 4; i++ )
			{
				GL.Vertex3( ( Single ) ( 16F * Math.Cos( i * Math.PI / 2 ) ), ( Single ) ( 16F * Math.Sin( i * Math.PI / 2 ) ), 0F );
			}

			GL.End();
			GL.Begin( PrimitiveType.TriangleFan );
			GL.Vertex3( 0, 0, 16 );
			for ( i = 4; i >= 0; i-- )
			{
				GL.Vertex3( ( Single ) ( 16F * Math.Cos( i * Math.PI / 2 ) ), ( Single ) ( 16F * Math.Sin( i * Math.PI / 2 ) ), 0F );
			}

			GL.End();
			GL.Color3( 1, 1, 1 );
			GL.PopMatrix();
			GL.Enable( EnableCap.Texture2D );
		}

		public virtual void R_DrawEntitiesOnList( )
		{
			if ( r_drawentities.value == 0F )
				return;
			Int32 i;
			for ( i = 0; i < r_newrefdef.num_entities; i++ )
			{
				currententity = r_newrefdef.entities[i];
				if ( ( currententity.flags & Defines.RF_TRANSLUCENT ) != 0 )
					continue;
				if ( ( currententity.flags & Defines.RF_BEAM ) != 0 )
				{
					R_DrawBeam( currententity );
				}
				else
				{
					currentmodel = currententity.model;
					if ( currentmodel == null )
					{
						R_DrawNullModel();
						continue;
					}

					switch ( currentmodel.type )

					{
						case mod_alias:
							R_DrawAliasModel( currententity );
							break;
						case mod_brush:
							R_DrawBrushModel( currententity );
							break;
						case mod_sprite:
							R_DrawSpriteModel( currententity );
							break;
						default:
							Com.Error( Defines.ERR_DROP, "Bad modeltype" );
							break;
					}
				}
			}

			GL.DepthMask( false );
			for ( i = 0; i < r_newrefdef.num_entities; i++ )
			{
				currententity = r_newrefdef.entities[i];
				if ( ( currententity.flags & Defines.RF_TRANSLUCENT ) == 0 )
					continue;
				if ( ( currententity.flags & Defines.RF_BEAM ) != 0 )
				{
					R_DrawBeam( currententity );
				}
				else
				{
					currentmodel = currententity.model;
					if ( currentmodel == null )
					{
						R_DrawNullModel();
						continue;
					}

					switch ( currentmodel.type )

					{
						case mod_alias:
							R_DrawAliasModel( currententity );
							break;
						case mod_brush:
							R_DrawBrushModel( currententity );
							break;
						case mod_sprite:
							R_DrawSpriteModel( currententity );
							break;
						default:
							Com.Error( Defines.ERR_DROP, "Bad modeltype" );
							break;
					}
				}
			}

			GL.DepthMask( true );
		}

		private readonly Single[] up = new Single[] { 0, 0, 0 };
		private readonly Single[] right = new Single[] { 0, 0, 0 };
		public virtual void GL_DrawParticles( Int32 num_particles )
		{
			Single origin_x, origin_y, origin_z;
			Math3D.VectorScale( vup, 1.5F, up );
			Math3D.VectorScale( vright, 1.5F, right );
			GL_Bind( r_particletexture.texnum );
			GL.DepthMask( false );
			GL.Enable( EnableCap.Blend );
			GL_TexEnv( ( Int32 ) All.Modulate );
			GL.Begin( PrimitiveType.Triangles );
			SingleBuffer sourceVertices = particle_t.vertexArray;
			Int32Buffer sourceColors = particle_t.colorArray;
			Single scale;
			Int32 color;
			for ( Int32 j = 0, i = 0; i < num_particles; i++ )
			{
				origin_x = sourceVertices.Get( j++ );
				origin_y = sourceVertices.Get( j++ );
				origin_z = sourceVertices.Get( j++ );
				scale = ( origin_x - r_origin[0] ) * vpn[0] + ( origin_y - r_origin[1] ) * vpn[1] + ( origin_z - r_origin[2] ) * vpn[2];
				scale = ( scale < 20 ) ? 1 : 1 + scale * 0.004F;
				color = sourceColors.Get( i );
				GL.Color4( ( Byte ) ( ( color ) & 0xFF ), ( Byte ) ( ( color >> 8 ) & 0xFF ), ( Byte ) ( ( color >> 16 ) & 0xFF ), ( Byte ) ( ( color >> 24 ) ) );
				GL.TexCoord2( 0.0625F, 0.0625F );
				GL.Vertex3( origin_x, origin_y, origin_z );
				GL.TexCoord2( 1.0625F, 0.0625F );
				GL.Vertex3( origin_x + up[0] * scale, origin_y + up[1] * scale, origin_z + up[2] * scale );
				GL.TexCoord2( 0.0625F, 1.0625F );
				GL.Vertex3( origin_x + right[0] * scale, origin_y + right[1] * scale, origin_z + right[2] * scale );
			}

			GL.End();
			GL.Disable( EnableCap.Blend );
			GL.Color4( 1, 1, 1, 1 );
			GL.DepthMask( true );
			GL_TexEnv( ( Int32 ) All.Replace );
		}

		public virtual void R_DrawParticles( )
		{
			if ( gl_ext_pointparameters.value != 0F && qglPointParameterfEXT )
			{
				GL.VertexPointer( 3, VertexPointerType.Float, 0, particle_t.vertexArray.Array );
				GL.EnableClientState( ArrayCap.ColorArray );
				GL.ColorPointer( 4, ColorPointerType.Byte, 0, particle_t.GetColorAsByteBuffer().Array );
				GL.DepthMask( false );
				GL.Enable( EnableCap.Blend );
				GL.Disable( EnableCap.Texture2D );
				GL.PointSize( gl_particle_size.value );
				GL.DrawArrays( PrimitiveType.Points, 0, r_newrefdef.num_particles );
				GL.DisableClientState( ArrayCap.ColorArray );
				GL.Disable( EnableCap.Blend );
				GL.Color4( 1F, 1F, 1F, 1F );
				GL.DepthMask( true );
				GL.Enable( EnableCap.Texture2D );
			}
			else
			{
				GL_DrawParticles( r_newrefdef.num_particles );
			}
		}

		public virtual void R_PolyBlend( )
		{
			if ( gl_polyblend.value == 0F )
				return;
			if ( v_blend[3] == 0F )
				return;
			GL.Disable( EnableCap.AlphaTest );
			GL.Enable( EnableCap.Blend );
			GL.Disable( EnableCap.DepthTest );
			GL.Disable( EnableCap.Texture2D );
			GL.LoadIdentity();
			GL.Rotate( -90, 1, 0, 0 );
			GL.Rotate( 90, 0, 0, 1 );
			GL.Color4( v_blend[0], v_blend[1], v_blend[2], v_blend[3] );
			GL.Begin( PrimitiveType.Quads );
			GL.Vertex3( 10, 100, 100 );
			GL.Vertex3( 10, -100, 100 );
			GL.Vertex3( 10, -100, -100 );
			GL.Vertex3( 10, 100, -100 );
			GL.End();
			GL.Disable( EnableCap.Blend );
			GL.Enable( EnableCap.Texture2D );
			GL.Enable( EnableCap.AlphaTest );
			GL.Color4( 1, 1, 1, 1 );
		}

		public virtual Int32 SignbitsForPlane( cplane_t out_renamed )
		{
			var bits = 0;
			for ( var j = 0; j < 3; j++ )
			{
				if ( out_renamed.normal[j] < 0 )
					bits |= ( 1 << j );
			}

			return bits;
		}

		public virtual void R_SetFrustum( )
		{
			Math3D.RotatePointAroundVector( frustum[0].normal, vup, vpn, -( 90F - r_newrefdef.fov_x / 2F ) );
			Math3D.RotatePointAroundVector( frustum[1].normal, vup, vpn, 90F - r_newrefdef.fov_x / 2F );
			Math3D.RotatePointAroundVector( frustum[2].normal, vright, vpn, 90F - r_newrefdef.fov_y / 2F );
			Math3D.RotatePointAroundVector( frustum[3].normal, vright, vpn, -( 90F - r_newrefdef.fov_y / 2F ) );
			for ( var i = 0; i < 4; i++ )
			{
				frustum[i].type = Defines.PLANE_ANYZ;
				frustum[i].dist = Math3D.DotProduct( r_origin, frustum[i].normal );
				frustum[i].signbits = ( Byte ) SignbitsForPlane( frustum[i] );
			}
		}

		private readonly Single[] temp = new Single[] { 0, 0, 0 };
		public virtual void R_SetupFrame( )
		{
			r_framecount++;
			Math3D.VectorCopy( r_newrefdef.vieworg, r_origin );
			Math3D.AngleVectors( r_newrefdef.viewangles, vpn, vright, vup );
			mleaf_t leaf;
			if ( ( r_newrefdef.rdflags & Defines.RDF_NOWORLDMODEL ) == 0 )
			{
				r_oldviewcluster = r_viewcluster;
				r_oldviewcluster2 = r_viewcluster2;
				leaf = Mod_PointInLeaf( r_origin, r_worldmodel );
				r_viewcluster = r_viewcluster2 = leaf.cluster;
				if ( leaf.contents == 0 )
				{
					Math3D.VectorCopy( r_origin, temp );
					temp[2] -= 16;
					leaf = Mod_PointInLeaf( temp, r_worldmodel );
					if ( ( leaf.contents & Defines.CONTENTS_SOLID ) == 0 && ( leaf.cluster != r_viewcluster2 ) )
						r_viewcluster2 = leaf.cluster;
				}
				else
				{
					Math3D.VectorCopy( r_origin, temp );
					temp[2] += 16;
					leaf = Mod_PointInLeaf( temp, r_worldmodel );
					if ( ( leaf.contents & Defines.CONTENTS_SOLID ) == 0 && ( leaf.cluster != r_viewcluster2 ) )
						r_viewcluster2 = leaf.cluster;
				}
			}

			for ( var i = 0; i < 4; i++ )
				v_blend[i] = r_newrefdef.blend[i];
			c_brush_polys = 0;
			c_alias_polys = 0;
			if ( ( r_newrefdef.rdflags & Defines.RDF_NOWORLDMODEL ) != 0 )
			{
				GL.Enable( EnableCap.ScissorTest );
				GL.ClearColor( 0.3F, 0.3F, 0.3F, 1F );
				GL.Scissor( r_newrefdef.x, vid.GetHeight() - r_newrefdef.height - r_newrefdef.y, r_newrefdef.width, r_newrefdef.height );
				GL.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
				GL.ClearColor( 1F, 0F, 0.5F, 0.5F );
				GL.Disable( EnableCap.ScissorTest );
			}
		}

		public virtual void MYgluPerspective( Double fovy, Double aspect, Double zNear, Double zFar )
		{
			var ymax = zNear * Math.Tan( fovy * Math.PI / 360 );
			var ymin = -ymax;
			var xmin = ymin * aspect;
			var xmax = ymax * aspect;
			xmin += -( 2 * gl_state.camera_separation ) / zNear;
			xmax += -( 2 * gl_state.camera_separation ) / zNear;
			GL.Frustum( xmin, xmax, ymin, ymax, zNear, zFar );
		}

		public virtual void R_SetupGL( )
		{
			var x = r_newrefdef.x;
			var x2 = r_newrefdef.x + r_newrefdef.width;
			var y = vid.GetHeight() - r_newrefdef.y;
			var y2 = vid.GetHeight() - ( r_newrefdef.y + r_newrefdef.height );
			var w = x2 - x;
			var h = y - y2;
			GL.Viewport( x, y2, w, h );
			var screenaspect = ( Single ) r_newrefdef.width / r_newrefdef.height;
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			MYgluPerspective( r_newrefdef.fov_y, screenaspect, 4, 4096 );
			GL.CullFace( CullFaceMode.Front );
			GL.MatrixMode( MatrixMode.Modelview );
			GL.LoadIdentity();
			GL.Rotate( -90, 1, 0, 0 );
			GL.Rotate( 90, 0, 0, 1 );
			GL.Rotate( -r_newrefdef.viewangles[2], 1, 0, 0 );
			GL.Rotate( -r_newrefdef.viewangles[0], 0, 1, 0 );
			GL.Rotate( -r_newrefdef.viewangles[1], 0, 0, 1 );
			GL.Translate( -r_newrefdef.vieworg[0], -r_newrefdef.vieworg[1], -r_newrefdef.vieworg[2] );
			GL.GetFloat( GetPName.ModelviewMatrix, r_world_matrix.Array );
			r_world_matrix.Clear();
			if ( gl_cull.value != 0F )
				GL.Enable( EnableCap.CullFace );
			else
				GL.Disable( EnableCap.CullFace );
			GL.Disable( EnableCap.Blend );
			GL.Disable( EnableCap.AlphaTest );
			GL.Enable( EnableCap.DepthTest );
		}

		Int32 trickframe = 0;
		public virtual void R_Clear( )
		{
			if ( gl_ztrick.value != 0F )
			{
				if ( gl_clear.value != 0F )
				{
					GL.Clear( ClearBufferMask.ColorBufferBit );
				}

				trickframe++;
				if ( ( trickframe & 1 ) != 0 )
				{
					gldepthmin = 0;
					gldepthmax = 0.49999F;
					GL.DepthFunc( DepthFunction.Lequal );
				}
				else
				{
					gldepthmin = 1;
					gldepthmax = 0.5F;
					GL.DepthFunc( DepthFunction.Gequal );
				}
			}
			else
			{
				if ( gl_clear.value != 0F )
					GL.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
				else
					GL.Clear( ClearBufferMask.DepthBufferBit );
				gldepthmin = 0;
				gldepthmax = 1;
				GL.DepthFunc( DepthFunction.Lequal );
			}

			GL.DepthRange( gldepthmin, gldepthmax );
		}

		public virtual void R_Flash( )
		{
			R_PolyBlend();
		}

		public virtual void R_RenderView( refdef_t fd )
		{
			if ( r_norefresh.value != 0F )
				return;
			r_newrefdef = fd;
			if ( r_newrefdef == null )
			{
				Com.Error( Defines.ERR_DROP, "R_RenderView: refdef_t fd is null" );
			}

			if ( r_worldmodel == null && ( r_newrefdef.rdflags & Defines.RDF_NOWORLDMODEL ) == 0 )
				Com.Error( Defines.ERR_DROP, "R_RenderView: NULL worldmodel" );
			if ( r_speeds.value != 0F )
			{
				c_brush_polys = 0;
				c_alias_polys = 0;
			}

			R_PushDlights();
			if ( gl_finish.value != 0F )
				GL.Finish();
			R_SetupFrame();
			R_SetFrustum();
			R_SetupGL();
			R_MarkLeaves();
			R_DrawWorld();
			R_DrawEntitiesOnList();
			R_RenderDlights();
			R_DrawParticles();
			R_DrawAlphaSurfaces();
			R_Flash();
			if ( r_speeds.value != 0F )
			{
				VID.Printf( Defines.PRINT_ALL, "%4i wpoly %4i epoly %i tex %i lmaps\\n", c_brush_polys, c_alias_polys, c_visible_textures, c_visible_lightmaps );
			}
		}

		public virtual void R_SetGL2D( )
		{
			GL.Viewport( 0, 0, vid.GetWidth(), vid.GetHeight() );
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			GL.Ortho( 0, vid.GetWidth(), vid.GetHeight(), 0, -99999, 99999 );
			GL.MatrixMode( MatrixMode.Modelview );
			GL.LoadIdentity();
			GL.Disable( EnableCap.DepthTest );
			GL.Disable( EnableCap.CullFace );
			GL.Disable( EnableCap.Blend );
			GL.Enable( EnableCap.AlphaTest );
			GL.Color4( 1, 1, 1, 1 );
		}

		private readonly Single[] light = new Single[] { 0, 0, 0 };
		public virtual void R_SetLightLevel( )
		{
			if ( ( r_newrefdef.rdflags & Defines.RDF_NOWORLDMODEL ) != 0 )
				return;
			R_LightPoint( r_newrefdef.vieworg, light );
			if ( light[0] > light[1] )
			{
				if ( light[0] > light[2] )
					r_lightlevel.value = 150 * light[0];
				else
					r_lightlevel.value = 150 * light[2];
			}
			else
			{
				if ( light[1] > light[2] )
					r_lightlevel.value = 150 * light[1];
				else
					r_lightlevel.value = 150 * light[2];
			}
		}

		public override void R_RenderFrame( refdef_t fd )
		{
			R_RenderView( fd );
			R_SetLightLevel();
			R_SetGL2D();
		}

		protected virtual void R_Register( )
		{
			r_lefthand = Cvar.Get( "hand", "0", Defines.CVAR_USERINFO | Defines.CVAR_ARCHIVE );
			r_norefresh = Cvar.Get( "r_norefresh", "0", 0 );
			r_fullbright = Cvar.Get( "r_fullbright", "0", 0 );
			r_drawentities = Cvar.Get( "r_drawentities", "1", 0 );
			r_drawworld = Cvar.Get( "r_drawworld", "1", 0 );
			r_novis = Cvar.Get( "r_novis", "0", 0 );
			r_nocull = Cvar.Get( "r_nocull", "0", 0 );
			r_lerpmodels = Cvar.Get( "r_lerpmodels", "1", 0 );
			r_speeds = Cvar.Get( "r_speeds", "0", 0 );
			r_lightlevel = Cvar.Get( "r_lightlevel", "1", 0 );
			gl_nosubimage = Cvar.Get( "gl_nosubimage", "0", 0 );
			gl_allow_software = Cvar.Get( "gl_allow_software", "0", 0 );
			gl_particle_min_size = Cvar.Get( "gl_particle_min_size", "2", Defines.CVAR_ARCHIVE );
			gl_particle_max_size = Cvar.Get( "gl_particle_max_size", "40", Defines.CVAR_ARCHIVE );
			gl_particle_size = Cvar.Get( "gl_particle_size", "40", Defines.CVAR_ARCHIVE );
			gl_particle_att_a = Cvar.Get( "gl_particle_att_a", "0.01", Defines.CVAR_ARCHIVE );
			gl_particle_att_b = Cvar.Get( "gl_particle_att_b", "0.0", Defines.CVAR_ARCHIVE );
			gl_particle_att_c = Cvar.Get( "gl_particle_att_c", "0.01", Defines.CVAR_ARCHIVE );
			gl_modulate = Cvar.Get( "gl_modulate", "1.5", Defines.CVAR_ARCHIVE );
			gl_log = Cvar.Get( "gl_log", "0", 0 );
			gl_bitdepth = Cvar.Get( "gl_bitdepth", "0", 0 );
			gl_mode = Cvar.Get( "gl_mode", "3", Defines.CVAR_ARCHIVE );
			gl_lightmap = Cvar.Get( "gl_lightmap", "0", 0 );
			gl_shadows = Cvar.Get( "gl_shadows", "0", Defines.CVAR_ARCHIVE );
			gl_dynamic = Cvar.Get( "gl_dynamic", "1", 0 );
			gl_nobind = Cvar.Get( "gl_nobind", "0", 0 );
			gl_round_down = Cvar.Get( "gl_round_down", "1", 0 );
			gl_picmip = Cvar.Get( "gl_picmip", "0", 0 );
			gl_skymip = Cvar.Get( "gl_skymip", "0", 0 );
			gl_showtris = Cvar.Get( "gl_showtris", "0", 0 );
			gl_ztrick = Cvar.Get( "gl_ztrick", "0", 0 );
			gl_finish = Cvar.Get( "gl_finish", "0", Defines.CVAR_ARCHIVE );
			gl_clear = Cvar.Get( "gl_clear", "0", 0 );
			gl_cull = Cvar.Get( "gl_cull", "1", 0 );
			gl_polyblend = Cvar.Get( "gl_polyblend", "1", 0 );
			gl_flashblend = Cvar.Get( "gl_flashblend", "0", 0 );
			gl_playermip = Cvar.Get( "gl_playermip", "0", 0 );
			gl_monolightmap = Cvar.Get( "gl_monolightmap", "0", 0 );
			gl_driver = Cvar.Get( "gl_driver", "opengl32", Defines.CVAR_ARCHIVE );
			gl_texturemode = Cvar.Get( "gl_texturemode", "(int)All.Linear_MIPMAP_NEAREST", Defines.CVAR_ARCHIVE );
			gl_texturealphamode = Cvar.Get( "gl_texturealphamode", "default", Defines.CVAR_ARCHIVE );
			gl_texturesolidmode = Cvar.Get( "gl_texturesolidmode", "default", Defines.CVAR_ARCHIVE );
			gl_lockpvs = Cvar.Get( "gl_lockpvs", "0", 0 );
			gl_vertex_arrays = Cvar.Get( "gl_vertex_arrays", "1", Defines.CVAR_ARCHIVE );
			gl_ext_swapinterval = Cvar.Get( "gl_ext_swapinterval", "1", Defines.CVAR_ARCHIVE );
			gl_ext_palettedtexture = Cvar.Get( "gl_ext_palettedtexture", "0", Defines.CVAR_ARCHIVE );
			gl_ext_multitexture = Cvar.Get( "gl_ext_multitexture", "1", Defines.CVAR_ARCHIVE );
			gl_ext_pointparameters = Cvar.Get( "gl_ext_pointparameters", "1", Defines.CVAR_ARCHIVE );
			gl_ext_compiled_vertex_array = Cvar.Get( "gl_ext_compiled_vertex_array", "1", Defines.CVAR_ARCHIVE );
			gl_drawbuffer = Cvar.Get( "gl_drawbuffer", "GL_BACK", 0 );
			gl_swapinterval = Cvar.Get( "gl_swapinterval", "0", Defines.CVAR_ARCHIVE );
			gl_saturatelighting = Cvar.Get( "gl_saturatelighting", "0", 0 );
			gl_3dlabs_broken = Cvar.Get( "gl_3dlabs_broken", "1", Defines.CVAR_ARCHIVE );
			vid_fullscreen = Cvar.Get( "vid_fullscreen", "0", Defines.CVAR_ARCHIVE );
			vid_gamma = Cvar.Get( "vid_gamma", "1.0", Defines.CVAR_ARCHIVE );
			vid_ref = Cvar.Get( "vid_ref", "lwjgl", Defines.CVAR_ARCHIVE );
			Cmd.AddCommand( "imagelist", new Anonymousxcommand_t( this ) );
			Cmd.AddCommand( "screenshot", new Anonymousxcommand_t1( this ) );
			Cmd.AddCommand( "modellist", new Anonymousxcommand_t2( this ) );
			Cmd.AddCommand( "gl_strings", new Anonymousxcommand_t3( this ) );
		}

		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public Anonymousxcommand_t( Main parent )
			{
				this.parent = parent;
			}

			private readonly Main parent;
			public override void Execute( )
			{
				parent.GL_ImageList_f();
			}
		}

		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public Anonymousxcommand_t1( Main parent )
			{
				this.parent = parent;
			}

			private readonly Main parent;
			public override void Execute( )
			{
				parent.glImpl.Screenshot();
			}
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public Anonymousxcommand_t2( Main parent )
			{
				this.parent = parent;
			}

			private readonly Main parent;
			public override void Execute( )
			{
				parent.Mod_Modellist_f();
			}
		}

		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public Anonymousxcommand_t3( Main parent )
			{
				this.parent = parent;
			}

			private readonly Main parent;
			public override void Execute( )
			{
				parent.GL_Strings_f();
			}
		}

		protected virtual System.Boolean R_SetMode( )
		{
			var fullscreen = ( vid_fullscreen.value > 0F );
			vid_fullscreen.modified = false;
			gl_mode.modified = false;
			Size dim = new Size( vid.GetWidth(), vid.GetHeight() );
			Int32 err;
			if ( ( err = glImpl.SetMode( dim, ( Int32 ) gl_mode.value, fullscreen ) ) == rserr_ok )
			{
				gl_state.prev_mode = ( Int32 ) gl_mode.value;
			}
			else
			{
				if ( err == rserr_invalid_fullscreen )
				{
					Cvar.SetValue( "vid_fullscreen", 0 );
					vid_fullscreen.modified = false;
					VID.Printf( Defines.PRINT_ALL, "ref_gl::R_SetMode() - fullscreen unavailable in this mode\\n" );
					if ( ( err = glImpl.SetMode( dim, ( Int32 ) gl_mode.value, false ) ) == rserr_ok )
						return true;
				}
				else if ( err == rserr_invalid_mode )
				{
					Cvar.SetValue( "gl_mode", gl_state.prev_mode );
					gl_mode.modified = false;
					VID.Printf( Defines.PRINT_ALL, "ref_gl::R_SetMode() - invalid mode\\n" );
				}

				if ( ( err = glImpl.SetMode( dim, gl_state.prev_mode, false ) ) != rserr_ok )
				{
					VID.Printf( Defines.PRINT_ALL, "ref_gl::R_SetMode() - could not revert to safe mode\\n" );
					return false;
				}
			}

			return true;
		}

		Single[] r_turbsin = new Single[256];
		protected virtual System.Boolean R_Init( )
		{
			return R_Init( 0, 0 );
		}

		public override System.Boolean R_Init( Int32 vid_xpos, Int32 vid_ypos )
		{
			for ( var j = 0; j < 256; j++ )
			{
				r_turbsin[j] = Warp.SIN[j] * 0.5F;
			}

			VID.Printf( Defines.PRINT_ALL, "ref_gl version: " + REF_VERSION + '\\' );
			Draw_GetPalette();
			R_Register();
			gl_state.prev_mode = 3;
			if ( !R_SetMode() )
			{
				VID.Printf( Defines.PRINT_ALL, "ref_gl::R_Init() - could not R_SetMode()\\n" );
				return false;
			}

			return true;
		}

		public override System.Boolean R_Init2( )
		{
			VID.MenuInit();
			gl_config.vendor_string = GL.GetString( StringName.Vendor );
			VID.Printf( Defines.PRINT_ALL, "GL_VENDOR: " + gl_config.vendor_string + '\\' );
			gl_config.renderer_string = GL.GetString( StringName.Renderer );
			VID.Printf( Defines.PRINT_ALL, "GL_RENDERER: " + gl_config.renderer_string + '\\' );
			gl_config.version_string = GL.GetString( StringName.Version );
			VID.Printf( Defines.PRINT_ALL, "GL_VERSION: " + gl_config.version_string + '\\' );
			gl_config.extensions_string = GL.GetString( StringName.Extensions );
			VID.Printf( Defines.PRINT_ALL, "GL_EXTENSIONS: " + gl_config.extensions_string + '\\' );
			gl_config.ParseOpenGLVersion();
			var renderer_buffer = gl_config.renderer_string.ToLower();
			var vendor_buffer = gl_config.vendor_string.ToLower();
			if ( renderer_buffer.IndexOf( "voodoo" ) >= 0 )
			{
				if ( renderer_buffer.IndexOf( "rush" ) < 0 )
					gl_config.renderer = GL_RENDERER_VOODOO;
				else
					gl_config.renderer = GL_RENDERER_VOODOO_RUSH;
			}
			else if ( vendor_buffer.IndexOf( "sgi" ) >= 0 )
				gl_config.renderer = GL_RENDERER_SGI;
			else if ( renderer_buffer.IndexOf( "permedia" ) >= 0 )
				gl_config.renderer = GL_RENDERER_PERMEDIA2;
			else if ( renderer_buffer.IndexOf( "glint" ) >= 0 )
				gl_config.renderer = GL_RENDERER_GLINT_MX;
			else if ( renderer_buffer.IndexOf( "glzicd" ) >= 0 )
				gl_config.renderer = GL_RENDERER_REALIZM;
			else if ( renderer_buffer.IndexOf( "gdi" ) >= 0 )
				gl_config.renderer = GL_RENDERER_MCD;
			else if ( renderer_buffer.IndexOf( "pcx2" ) >= 0 )
				gl_config.renderer = GL_RENDERER_PCX2;
			else if ( renderer_buffer.IndexOf( "verite" ) >= 0 )
				gl_config.renderer = GL_RENDERER_RENDITION;
			else
				gl_config.renderer = unchecked(( Int32 ) GL_RENDERER_OTHER);
			var monolightmap = gl_monolightmap.string_renamed.ToUpper();
			if ( monolightmap.Length < 2 || monolightmap[1] != 'F' )
			{
				if ( gl_config.renderer == GL_RENDERER_PERMEDIA2 )
				{
					Cvar.Set( "gl_monolightmap", "A" );
					VID.Printf( Defines.PRINT_ALL, "...using gl_monolightmap 'a'\\n" );
				}
				else if ( ( gl_config.renderer & GL_RENDERER_POWERVR ) != 0 )
				{
					Cvar.Set( "gl_monolightmap", "0" );
				}
				else
				{
					Cvar.Set( "gl_monolightmap", "0" );
				}
			}

			if ( ( gl_config.renderer & GL_RENDERER_POWERVR ) != 0 )
			{
				Cvar.Set( "scr_drawall", "1" );
			}
			else
			{
				Cvar.Set( "scr_drawall", "0" );
			}

			if ( gl_config.renderer == GL_RENDERER_MCD )
			{
				Cvar.SetValue( "gl_finish", 1 );
			}

			if ( ( gl_config.renderer & GL_RENDERER_3DLABS ) != 0 )
			{
				if ( gl_3dlabs_broken.value != 0F )
					gl_config.allow_cds = false;
				else
					gl_config.allow_cds = true;
			}
			else
			{
				gl_config.allow_cds = true;
			}

			if ( gl_config.allow_cds )
				VID.Printf( Defines.PRINT_ALL, "...allowing CDS\\n" );
			else
				VID.Printf( Defines.PRINT_ALL, "...disabling CDS\\n" );
			if ( gl_config.extensions_string.IndexOf( "GL_EXT_compiled_vertex_array" ) >= 0 || gl_config.extensions_string.IndexOf( "GL_SGI_compiled_vertex_array" ) >= 0 )
			{
				VID.Printf( Defines.PRINT_ALL, "...enabling GL_EXT_compiled_vertex_array\\n" );
				if ( gl_ext_compiled_vertex_array.value != 0F )
					qglLockArraysEXT = true;
				else
					qglLockArraysEXT = false;
			}
			else
			{
				VID.Printf( Defines.PRINT_ALL, "...GL_EXT_compiled_vertex_array not found\\n" );
				qglLockArraysEXT = false;
			}

			if ( gl_config.extensions_string.IndexOf( "WGL_EXT_swap_control" ) >= 0 )
			{
				qwglSwapIntervalEXT = true;
				VID.Printf( Defines.PRINT_ALL, "...enabling WGL_EXT_swap_control\\n" );
			}
			else
			{
				qwglSwapIntervalEXT = false;
				VID.Printf( Defines.PRINT_ALL, "...WGL_EXT_swap_control not found\\n" );
			}

			if ( gl_config.extensions_string.IndexOf( "GL_EXT_point_parameters" ) >= 0 )
			{
				if ( gl_ext_pointparameters.value != 0F )
				{
					qglPointParameterfEXT = true;
					VID.Printf( Defines.PRINT_ALL, "...using GL_EXT_point_parameters\\n" );
				}
				else
				{
					VID.Printf( Defines.PRINT_ALL, "...ignoring GL_EXT_point_parameters\\n" );
				}
			}
			else
			{
				VID.Printf( Defines.PRINT_ALL, "...GL_EXT_point_parameters not found\\n" );
			}

			if ( !qglColorTableEXT && gl_config.extensions_string.IndexOf( "GL_EXT_paletted_texture" ) >= 0 && gl_config.extensions_string.IndexOf( "GL_EXT_shared_texture_palette" ) >= 0 )
			{
				if ( gl_ext_palettedtexture.value != 0F )
				{
					VID.Printf( Defines.PRINT_ALL, "...using GL_EXT_shared_texture_palette\\n" );
					qglColorTableEXT = false;
				}
				else
				{
					VID.Printf( Defines.PRINT_ALL, "...ignoring GL_EXT_shared_texture_palette\\n" );
					qglColorTableEXT = false;
				}
			}
			else
			{
				VID.Printf( Defines.PRINT_ALL, "...GL_EXT_shared_texture_palette not found\\n" );
			}

			if ( gl_config.extensions_string.IndexOf( "GL_ARB_multitexture" ) >= 0 )
			{
				try
				{
					GL.ClientActiveTexture( TextureUnit.Texture0 );
					VID.Printf( Defines.PRINT_ALL, "...using GL_ARB_multitexture\\n" );
					qglActiveTextureARB = true;
					TEXTURE0 = ( Int32 ) TextureUnit.Texture0;
					TEXTURE1 = ( Int32 ) TextureUnit.Texture1;
				}
				catch ( Exception e )
				{
					qglActiveTextureARB = false;
				}
			}
			else
			{
				qglActiveTextureARB = false;
				VID.Printf( Defines.PRINT_ALL, "...GL_ARB_multitexture not found\\n" );
			}

			if ( !( qglActiveTextureARB ) )
			{
				VID.Printf( Defines.PRINT_ALL, "Missing multi-texturing!\\n" );
				return false;
			}

			GL_SetDefaultState();
			GL_InitImages();
			Mod_Init();
			R_InitParticleTexture();
			Draw_InitLocal();
			ErrorCode err = GL.GetError();
			if ( err != ErrorCode.NoError )
				VID.Printf( Defines.PRINT_ALL, "GL.GetError() = 0x%x\\n\\t%s\\n", err, "" + GL.GetString( err ) );
			glImpl.EndFrame();
			return true;
		}

		public override void R_Shutdown( )
		{
			Cmd.RemoveCommand( "modellist" );
			Cmd.RemoveCommand( "screenshot" );
			Cmd.RemoveCommand( "imagelist" );
			Cmd.RemoveCommand( "gl_strings" );
			Mod_FreeAll();
			GL_ShutdownImages();
			glImpl.Shutdown();
		}

		public override void R_BeginFrame( Single camera_separation )
		{
			vid.Update();
			gl_state.camera_separation = camera_separation;
			if ( gl_mode.modified || vid_fullscreen.modified )
			{
				cvar_t ref_renamed;
				ref_renamed = Cvar.Get( "vid_ref", "lwjgl", 0 );
				ref_renamed.modified = true;
			}

			if ( gl_log.modified )
			{
				glImpl.EnableLogging( ( gl_log.value != 0F ) );
				gl_log.modified = false;
			}

			if ( gl_log.value != 0F )
			{
				glImpl.LogNewFrame();
			}

			if ( vid_gamma.modified )
			{
				vid_gamma.modified = false;
				if ( ( gl_config.renderer & GL_RENDERER_VOODOO ) != 0 )
				{
					VID.Printf( Defines.PRINT_DEVELOPER, "gamma anpassung fuer VOODOO nicht gesetzt" );
				}
			}

			glImpl.BeginFrame( camera_separation );
			GL.Viewport( 0, 0, vid.GetWidth(), vid.GetHeight() );
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			GL.Ortho( 0, vid.GetWidth(), vid.GetHeight(), 0, -99999, 99999 );
			GL.MatrixMode( MatrixMode.Modelview );
			GL.LoadIdentity();
			GL.Disable( EnableCap.DepthTest );
			GL.Disable( EnableCap.CullFace );
			GL.Disable( EnableCap.Blend );
			GL.Enable( EnableCap.AlphaTest );
			GL.Color4( 1, 1, 1, 1 );
			if ( gl_drawbuffer.modified )
			{
				gl_drawbuffer.modified = false;
				if ( gl_state.camera_separation == 0 || !gl_state.stereo_enabled )
				{
					if ( gl_drawbuffer.string_renamed.EqualsIgnoreCase( "GL_FRONT" ) )
						GL.DrawBuffer( DrawBufferMode.Front );
					else
						GL.DrawBuffer( DrawBufferMode.Back );
				}
			}

			if ( gl_texturemode.modified )
			{
				GL_TextureMode( gl_texturemode.string_renamed );
				gl_texturemode.modified = false;
			}

			if ( gl_texturealphamode.modified )
			{
				GL_TextureAlphaMode( gl_texturealphamode.string_renamed );
				gl_texturealphamode.modified = false;
			}

			if ( gl_texturesolidmode.modified )
			{
				GL_TextureSolidMode( gl_texturesolidmode.string_renamed );
				gl_texturesolidmode.modified = false;
			}

			GL_UpdateSwapInterval();
			R_Clear();
		}

		public Int32[] r_rawpalette = new Int32[256];
		public override void R_SetPalette( Byte[] palette )
		{
			Int32 i;
			var color = 0;
			if ( palette != null )
			{
				var j = 0;
				for ( i = 0; i < 256; i++ )
				{
					color = ( palette[j++] & 0xFF ) << 0;
					color |= ( palette[j++] & 0xFF ) << 8;
					color |= ( palette[j++] & 0xFF ) << 16;
					color |= unchecked(( Int32 ) 0xFF000000);
					r_rawpalette[i] = color;
				}
			}
			else
			{
				for ( i = 0; i < 256; i++ )
				{
					r_rawpalette[i] = unchecked(( Int32 ) ( d_8to24table[i] | 0xff000000 ));
				}
			}

			GL_SetTexturePalette( r_rawpalette );
			GL.ClearColor( 0, 0, 0, 0 );
			GL.Clear( ClearBufferMask.ColorBufferBit );
			GL.ClearColor( 1F, 0F, 0.5F, 0.5F );
		}

		static readonly Int32 NUM_BEAM_SEGS = 6;
		Single[][] start_points = Lib.CreateJaggedArray<Single[][]>( NUM_BEAM_SEGS, 3 );
		Single[][] end_points = Lib.CreateJaggedArray<Single[][]>( NUM_BEAM_SEGS, 3 );
		private readonly Single[] perpvec = new Single[] { 0, 0, 0 };
		private readonly Single[] direction = new Single[] { 0, 0, 0 };
		private readonly Single[] normalized_direction = new Single[] { 0, 0, 0 };
		private readonly Single[] oldorigin = new Single[] { 0, 0, 0 };
		private readonly Single[] origin = new Single[] { 0, 0, 0 };
		public virtual void R_DrawBeam( entity_t e )
		{
			oldorigin[0] = e.oldorigin[0];
			oldorigin[1] = e.oldorigin[1];
			oldorigin[2] = e.oldorigin[2];
			origin[0] = e.origin[0];
			origin[1] = e.origin[1];
			origin[2] = e.origin[2];
			normalized_direction[0] = direction[0] = oldorigin[0] - origin[0];
			normalized_direction[1] = direction[1] = oldorigin[1] - origin[1];
			normalized_direction[2] = direction[2] = oldorigin[2] - origin[2];
			if ( Math3D.VectorNormalize( normalized_direction ) == 0F )
				return;
			Math3D.PerpendicularVector( perpvec, normalized_direction );
			Math3D.VectorScale( perpvec, e.frame / 2, perpvec );
			for ( var i = 0; i < 6; i++ )
			{
				Math3D.RotatePointAroundVector( start_points[i], normalized_direction, perpvec, ( 360F / NUM_BEAM_SEGS ) * i );
				Math3D.VectorAdd( start_points[i], origin, start_points[i] );
				Math3D.VectorAdd( start_points[i], direction, end_points[i] );
			}

			GL.Disable( EnableCap.Texture2D );
			GL.Enable( EnableCap.Blend );
			GL.DepthMask( false );
			Single r = ( d_8to24table[e.skinnum & 0xFF] ) & 0xFF;
			Single g = ( d_8to24table[e.skinnum & 0xFF] >> 8 ) & 0xFF;
			Single b = ( d_8to24table[e.skinnum & 0xFF] >> 16 ) & 0xFF;
			r *= 1 / 255F;
			g *= 1 / 255F;
			b *= 1 / 255F;
			GL.Color4( r, g, b, e.alpha );
			GL.Begin( PrimitiveType.TriangleStrip );
			Single[] v;
			for ( var i = 0; i < NUM_BEAM_SEGS; i++ )
			{
				v = start_points[i];
				GL.Vertex3( v[0], v[1], v[2] );
				v = end_points[i];
				GL.Vertex3( v[0], v[1], v[2] );
				v = start_points[( i + 1 ) % NUM_BEAM_SEGS];
				GL.Vertex3( v[0], v[1], v[2] );
				v = end_points[( i + 1 ) % NUM_BEAM_SEGS];
				GL.Vertex3( v[0], v[1], v[2] );
			}

			GL.End();
			GL.Enable( EnableCap.Texture2D );
			GL.Disable( EnableCap.Blend );
			GL.DepthMask( true );
		}
	}
}
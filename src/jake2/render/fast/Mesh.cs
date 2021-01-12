using J2N.IO;
using Jake2.Client;
using Jake2.Qcommon;
using Jake2.Util;
using OpenTK.Graphics.OpenGL;
using Q2Sharp.util;
using System;

namespace Jake2.Render.Fast
{
	public abstract class Mesh : Light
	{
		static readonly Int32 NUMVERTEXNORMALS = 162;
		Single[][] r_avertexnormals = Anorms.VERTEXNORMALS;
		Single[] shadevector = new Single[] { 0, 0, 0 };
		Single[] shadelight = new Single[] { 0, 0, 0 };
		static readonly Int32 SHADEDOT_QUANT = 16;
		Single[][] r_avertexnormal_dots = Anorms.VERTEXNORMAL_DOTS;
		Single[] shadedots;

		public Mesh( )
		{
			shadedots = r_avertexnormal_dots[0];
		}

		public virtual void GL_LerpVerts( Int32 nverts, Int32[] ov, Int32[] v, Single[] move, Single[] frontv, Single[] backv )
		{
			SingleBuffer lerp = vertexArrayBuf;
			lerp.Limit = ( nverts << 2 ) - nverts;
			Int32 ovv, vv;
			if ( ( currententity.flags & ( Defines.RF_SHELL_RED | Defines.RF_SHELL_GREEN | Defines.RF_SHELL_BLUE | Defines.RF_SHELL_DOUBLE | Defines.RF_SHELL_HALF_DAM ) ) != 0 )
			{
				Single[] normal;
				var j = 0;
				for ( var i = 0; i < nverts; i++ )
				{
					vv = v[i];
					normal = r_avertexnormals[( vv >> 24 ) & 0xFF];
					ovv = ov[i];
					lerp.Put( j, move[0] + ( ovv & 0xFF ) * backv[0] + ( vv & 0xFF ) * frontv[0] + normal[0] * Defines.POWERSUIT_SCALE );
					lerp.Put( j + 1, move[1] + ( ( ovv >> 8 ) & 0xFF ) * backv[1] + ( ( vv >> 8 ) & 0xFF ) * frontv[1] + normal[1] * Defines.POWERSUIT_SCALE );
					lerp.Put( j + 2, move[2] + ( ( ovv >> 16 ) & 0xFF ) * backv[2] + ( ( vv >> 16 ) & 0xFF ) * frontv[2] + normal[2] * Defines.POWERSUIT_SCALE );
					j += 3;
				}
			}
			else
			{
				var j = 0;
				for ( var i = 0; i < nverts; i++ )
				{
					ovv = ov[i];
					vv = v[i];
					lerp.Put( j, move[0] + ( ovv & 0xFF ) * backv[0] + ( vv & 0xFF ) * frontv[0] );
					lerp.Put( j + 1, move[1] + ( ( ovv >> 8 ) & 0xFF ) * backv[1] + ( ( vv >> 8 ) & 0xFF ) * frontv[1] );
					lerp.Put( j + 2, move[2] + ( ( ovv >> 16 ) & 0xFF ) * backv[2] + ( ( vv >> 16 ) & 0xFF ) * frontv[2] );
					j += 3;
				}
			}
		}

		private readonly SingleBuffer colorArrayBuf = Lib.NewSingleBuffer( qfiles.MAX_VERTS * 4 );
		private readonly SingleBuffer vertexArrayBuf = Lib.NewSingleBuffer( qfiles.MAX_VERTS * 3 );
		private readonly SingleBuffer textureArrayBuf = Lib.NewSingleBuffer( qfiles.MAX_VERTS * 2 );
		System.Boolean isFilled = false;
		Single[] tmpVec = new Single[] { 0, 0, 0 };
		Single[][] vectors = new Single[][] { new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 } };
		private readonly Single[] move = new Single[] { 0, 0, 0 };
		private readonly Single[] frontv = new Single[] { 0, 0, 0 };
		private readonly Single[] backv = new Single[] { 0, 0, 0 };
		public virtual void GL_DrawAliasFrameLerp( qfiles.dmdl_t paliashdr, Single backlerp )
		{
			qfiles.daliasframe_t frame = paliashdr.aliasFrames[currententity.frame];
			Int32[] verts = frame.verts;
			qfiles.daliasframe_t oldframe = paliashdr.aliasFrames[currententity.oldframe];
			Int32[] ov = oldframe.verts;
			Single alpha;
			Int32 size;
			if ( ( currententity.flags & Defines.RF_TRANSLUCENT ) != 0 )
				alpha = currententity.alpha;
			else
				alpha = 1F;
			if ( ( currententity.flags & ( Defines.RF_SHELL_RED | Defines.RF_SHELL_GREEN | Defines.RF_SHELL_BLUE | Defines.RF_SHELL_DOUBLE | Defines.RF_SHELL_HALF_DAM ) ) != 0 )
				GL.Disable( EnableCap.Texture2D );
			var frontlerp = 1F - backlerp;
			Math3D.VectorSubtract( currententity.oldorigin, currententity.origin, frontv );
			Math3D.AngleVectors( currententity.angles, vectors[0], vectors[1], vectors[2] );
			move[0] = Math3D.DotProduct( frontv, vectors[0] );
			move[1] = -Math3D.DotProduct( frontv, vectors[1] );
			move[2] = Math3D.DotProduct( frontv, vectors[2] );
			Math3D.VectorAdd( move, oldframe.translate, move );
			for ( var i = 0; i < 3; i++ )
			{
				move[i] = backlerp * move[i] + frontlerp * frame.translate[i];
				frontv[i] = frontlerp * frame.scale[i];
				backv[i] = backlerp * oldframe.scale[i];
			}

			GL_LerpVerts( paliashdr.num_xyz, ov, verts, move, frontv, backv );
			new Pinnable( vertexArrayBuf.Array, ( ptr ) =>
			{
				GL.VertexPointer( 3, VertexPointerType.Float, 0, ptr );
			} );
			if ( ( currententity.flags & ( Defines.RF_SHELL_RED | Defines.RF_SHELL_GREEN | Defines.RF_SHELL_BLUE | Defines.RF_SHELL_DOUBLE | Defines.RF_SHELL_HALF_DAM ) ) != 0 )
			{
				GL.Color4( shadelight[0], shadelight[1], shadelight[2], alpha );
			}
			else
			{
				GL.EnableClientState( ArrayCap.ColorArray );
				new Pinnable( colorArrayBuf.Array, ( ptr ) =>
				{
					GL.ColorPointer( 4, ColorPointerType.Float, 0, ptr );
				} );
				SingleBuffer color = colorArrayBuf;
				Single l;
				size = paliashdr.num_xyz;
				var j = 0;
				for ( var i = 0; i < size; i++ )
				{
					l = shadedots[( verts[i] >> 24 ) & 0xFF];
					color.Put( j, l * shadelight[0] );
					color.Put( j + 1, l * shadelight[1] );
					color.Put( j + 2, l * shadelight[2] );
					color.Put( j + 3, alpha );
					j += 4;
				}
			}

			GL.ClientActiveTexture( TextureUnit.Texture0 );
			new Pinnable( textureArrayBuf.Array, ( ptr ) =>
			{
				GL.TexCoordPointer( 2, TexCoordPointerType.Float, 0, ptr );
			} );
			var pos = 0;
			Int32[] counts = paliashdr.counts;
			Int32Buffer srcIndexBuf = null;
			SingleBuffer dstTextureCoords = textureArrayBuf;
			SingleBuffer srcTextureCoords = paliashdr.textureCoordBuf;
			var dstIndex = 0;
			var srcIndex = 0;
			Int32 count;
			Int32 mode;
			size = counts.Length;
			for ( var j = 0; j < size; j++ )
			{
				count = counts[j];
				if ( count == 0 )
					break;
				srcIndexBuf = paliashdr.indexElements[j];
				mode = ( Int32 ) PrimitiveType.TriangleStrip;
				if ( count < 0 )
				{
					mode = ( Int32 ) PrimitiveType.TriangleFan;
					count = -count;
				}

				srcIndex = pos << 1;
				srcIndex--;
				for ( var k = 0; k < count; k++ )
				{
					dstIndex = srcIndexBuf.Get( k ) << 1;
					dstTextureCoords.Put( dstIndex, srcTextureCoords.Get( ++srcIndex ) );
					dstTextureCoords.Put( ++dstIndex, srcTextureCoords.Get( ++srcIndex ) );
				}
				new Pinnable( srcIndexBuf.Array, ( ptr ) =>
				{
					GL.DrawElements( ( PrimitiveType ) mode, 1, DrawElementsType.UnsignedInt, ptr );
				} );
				pos += count;
			}

			if ( ( currententity.flags & ( Defines.RF_SHELL_RED | Defines.RF_SHELL_GREEN | Defines.RF_SHELL_BLUE | Defines.RF_SHELL_DOUBLE | Defines.RF_SHELL_HALF_DAM ) ) != 0 )
				GL.Enable( EnableCap.Texture2D );
			GL.DisableClientState( ArrayCap.ColorArray );
		}

		private readonly Single[] point = new Single[] { 0, 0, 0 };
		public virtual void GL_DrawAliasShadow( qfiles.dmdl_t paliashdr, Int32 posenum )
		{
			var lheight = currententity.origin[2] - lightspot[2];
			Int32[] order = paliashdr.glCmds;
			var height = -lheight + 1F;
			var orderIndex = 0;
			var index = 0;
			Int32 count;
			while ( true )
			{
				count = order[orderIndex++];
				if ( count == 0 )
					break;
				if ( count < 0 )
				{
					count = -count;
					GL.Begin( PrimitiveType.TriangleFan );
				}
				else
					GL.Begin( PrimitiveType.TriangleStrip );
				do
				{
					index = order[orderIndex + 2] * 3;
					point[0] = vertexArrayBuf.Get( index );
					point[1] = vertexArrayBuf.Get( index + 1 );
					point[2] = vertexArrayBuf.Get( index + 2 );
					point[0] -= shadevector[0] * ( point[2] + lheight );
					point[1] -= shadevector[1] * ( point[2] + lheight );
					point[2] = height;
					GL.Vertex3( point[0], point[1], point[2] );
					orderIndex += 3;
				}
				while ( --count != 0 );
				GL.End();
			}
		}

		private readonly Single[] mins = new Single[] { 0, 0, 0 };
		private readonly Single[] maxs = new Single[] { 0, 0, 0 };
		public virtual System.Boolean R_CullAliasModel( entity_t e )
		{
			qfiles.dmdl_t paliashdr = ( qfiles.dmdl_t ) currentmodel.extradata;
			if ( ( e.frame >= paliashdr.num_frames ) || ( e.frame < 0 ) )
			{
				VID.Printf( Defines.PRINT_ALL, "R_CullAliasModel " + currentmodel.name + ": no such frame " + e.frame + '\\' );
				e.frame = 0;
			}

			if ( ( e.oldframe >= paliashdr.num_frames ) || ( e.oldframe < 0 ) )
			{
				VID.Printf( Defines.PRINT_ALL, "R_CullAliasModel " + currentmodel.name + ": no such oldframe " + e.oldframe + '\\' );
				e.oldframe = 0;
			}

			qfiles.daliasframe_t pframe = paliashdr.aliasFrames[e.frame];
			qfiles.daliasframe_t poldframe = paliashdr.aliasFrames[e.oldframe];
			if ( pframe == poldframe )
			{
				for ( var i = 0; i < 3; i++ )
				{
					mins[i] = pframe.translate[i];
					maxs[i] = mins[i] + pframe.scale[i] * 255;
				}
			}
			else
			{
				Single thismaxs, oldmaxs;
				for ( var i = 0; i < 3; i++ )
				{
					thismaxs = pframe.translate[i] + pframe.scale[i] * 255;
					oldmaxs = poldframe.translate[i] + poldframe.scale[i] * 255;
					if ( pframe.translate[i] < poldframe.translate[i] )
						mins[i] = pframe.translate[i];
					else
						mins[i] = poldframe.translate[i];
					if ( thismaxs > oldmaxs )
						maxs[i] = thismaxs;
					else
						maxs[i] = oldmaxs;
				}
			}

			Single[] tmp;
			for ( var i = 0; i < 8; i++ )
			{
				tmp = bbox[i];
				if ( ( i & 1 ) != 0 )
					tmp[0] = mins[0];
				else
					tmp[0] = maxs[0];
				if ( ( i & 2 ) != 0 )
					tmp[1] = mins[1];
				else
					tmp[1] = maxs[1];
				if ( ( i & 4 ) != 0 )
					tmp[2] = mins[2];
				else
					tmp[2] = maxs[2];
			}

			tmp = mins;
			Math3D.VectorCopy( e.angles, tmp );
			tmp[YAW] = -tmp[YAW];
			Math3D.AngleVectors( tmp, vectors[0], vectors[1], vectors[2] );
			for ( var i = 0; i < 8; i++ )
			{
				Math3D.VectorCopy( bbox[i], tmp );
				bbox[i][0] = Math3D.DotProduct( vectors[0], tmp );
				bbox[i][1] = -Math3D.DotProduct( vectors[1], tmp );
				bbox[i][2] = Math3D.DotProduct( vectors[2], tmp );
				Math3D.VectorAdd( e.origin, bbox[i], bbox[i] );
			}

			Int32 f, mask;
			var aggregatemask = ~0;
			for ( var p = 0; p < 8; p++ )
			{
				mask = 0;
				for ( f = 0; f < 4; f++ )
				{
					var dp = Math3D.DotProduct( frustum[f].normal, bbox[p] );
					if ( ( dp - frustum[f].dist ) < 0 )
					{
						mask |= ( 1 << f );
					}
				}

				aggregatemask &= mask;
			}

			if ( aggregatemask != 0 )
			{
				return true;
			}

			return false;
		}

		Single[][] bbox = new Single[][] { new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 }, new Single[] { 0, 0, 0 } };
		public override void R_DrawAliasModel( entity_t e )
		{
			if ( ( e.flags & Defines.RF_WEAPONMODEL ) == 0 )
			{
				if ( R_CullAliasModel( e ) )
					return;
			}

			if ( ( e.flags & Defines.RF_WEAPONMODEL ) != 0 )
			{
				if ( r_lefthand.value == 2F )
					return;
			}

			qfiles.dmdl_t paliashdr = ( qfiles.dmdl_t ) currentmodel.extradata;
			Int32 i;
			if ( ( currententity.flags & ( Defines.RF_SHELL_HALF_DAM | Defines.RF_SHELL_GREEN | Defines.RF_SHELL_RED | Defines.RF_SHELL_BLUE | Defines.RF_SHELL_DOUBLE ) ) != 0 )
			{
				Math3D.VectorClear( shadelight );
				if ( ( currententity.flags & Defines.RF_SHELL_HALF_DAM ) != 0 )
				{
					shadelight[0] = 0.56F;
					shadelight[1] = 0.59F;
					shadelight[2] = 0.45F;
				}

				if ( ( currententity.flags & Defines.RF_SHELL_DOUBLE ) != 0 )
				{
					shadelight[0] = 0.9F;
					shadelight[1] = 0.7F;
				}

				if ( ( currententity.flags & Defines.RF_SHELL_RED ) != 0 )
					shadelight[0] = 1F;
				if ( ( currententity.flags & Defines.RF_SHELL_GREEN ) != 0 )
					shadelight[1] = 1F;
				if ( ( currententity.flags & Defines.RF_SHELL_BLUE ) != 0 )
					shadelight[2] = 1F;
			}
			else if ( ( currententity.flags & Defines.RF_FULLBRIGHT ) != 0 )
			{
				for ( i = 0; i < 3; i++ )
					shadelight[i] = 1F;
			}
			else
			{
				R_LightPoint( currententity.origin, shadelight );
				if ( ( currententity.flags & Defines.RF_WEAPONMODEL ) != 0 )
				{
					if ( shadelight[0] > shadelight[1] )
					{
						if ( shadelight[0] > shadelight[2] )
							r_lightlevel.value = 150 * shadelight[0];
						else
							r_lightlevel.value = 150 * shadelight[2];
					}
					else
					{
						if ( shadelight[1] > shadelight[2] )
							r_lightlevel.value = 150 * shadelight[1];
						else
							r_lightlevel.value = 150 * shadelight[2];
					}
				}

				if ( gl_monolightmap.string_renamed[0] != '0' )
				{
					var s = shadelight[0];
					if ( s < shadelight[1] )
						s = shadelight[1];
					if ( s < shadelight[2] )
						s = shadelight[2];
					shadelight[0] = s;
					shadelight[1] = s;
					shadelight[2] = s;
				}
			}

			if ( ( currententity.flags & Defines.RF_MINLIGHT ) != 0 )
			{
				for ( i = 0; i < 3; i++ )
					if ( shadelight[i] > 0.1F )
						break;
				if ( i == 3 )
				{
					shadelight[0] = 0.1F;
					shadelight[1] = 0.1F;
					shadelight[2] = 0.1F;
				}
			}

			if ( ( currententity.flags & Defines.RF_GLOW ) != 0 )
			{
				Single scale;
				Single min;
				scale = ( Single ) ( 0.1F * Math.Sin( r_newrefdef.time * 7 ) );
				for ( i = 0; i < 3; i++ )
				{
					min = shadelight[i] * 0.8F;
					shadelight[i] += scale;
					if ( shadelight[i] < min )
						shadelight[i] = min;
				}
			}

			if ( ( r_newrefdef.rdflags & Defines.RDF_IRGOGGLES ) != 0 && ( currententity.flags & Defines.RF_IR_VISIBLE ) != 0 )
			{
				shadelight[0] = 1F;
				shadelight[1] = 0F;
				shadelight[2] = 0F;
			}

			shadedots = r_avertexnormal_dots[( ( Int32 ) ( currententity.angles[1] * ( SHADEDOT_QUANT / 360 ) ) ) & ( SHADEDOT_QUANT - 1 )];
			var an = ( Single ) ( currententity.angles[1] / 180 * Math.PI );
			shadevector[0] = ( Single ) Math.Cos( -an );
			shadevector[1] = ( Single ) Math.Sin( -an );
			shadevector[2] = 1;
			Math3D.VectorNormalize( shadevector );
			c_alias_polys += paliashdr.num_tris;
			if ( ( currententity.flags & Defines.RF_DEPTHHACK ) != 0 )
				GL.DepthRange( gldepthmin, gldepthmin + 0.3 * ( gldepthmax - gldepthmin ) );
			if ( ( currententity.flags & Defines.RF_WEAPONMODEL ) != 0 && ( r_lefthand.value == 1F ) )
			{
				GL.MatrixMode( MatrixMode.Projection );
				GL.PushMatrix();
				GL.LoadIdentity();
				GL.Scale( -1, 1, 1 );
				MYgluPerspective( r_newrefdef.fov_y, ( Single ) r_newrefdef.width / r_newrefdef.height, 4, 4096 );
				GL.MatrixMode( MatrixMode.Modelview );
				GL.CullFace( CullFaceMode.Back );
			}

			GL.PushMatrix();
			e.angles[PITCH] = -e.angles[PITCH];
			R_RotateForEntity( e );
			e.angles[PITCH] = -e.angles[PITCH];
			image_t skin;
			if ( currententity.skin != null )
				skin = currententity.skin;
			else
			{
				if ( currententity.skinnum >= qfiles.MAX_MD2SKINS )
					skin = currentmodel.skins[0];
				else
				{
					skin = currentmodel.skins[currententity.skinnum];
					if ( skin == null )
						skin = currentmodel.skins[0];
				}
			}

			if ( skin == null )
				skin = r_notexture;
			GL_Bind( skin.texnum );
			GL.ShadeModel( ShadingModel.Smooth );
			GL_TexEnv( ( Int32 ) All.Modulate );
			if ( ( currententity.flags & Defines.RF_TRANSLUCENT ) != 0 )
			{
				GL.Enable( EnableCap.Blend );
			}

			if ( ( currententity.frame >= paliashdr.num_frames ) || ( currententity.frame < 0 ) )
			{
				VID.Printf( Defines.PRINT_ALL, "R_DrawAliasModel " + currentmodel.name + ": no such frame " + currententity.frame + '\\' );
				currententity.frame = 0;
				currententity.oldframe = 0;
			}

			if ( ( currententity.oldframe >= paliashdr.num_frames ) || ( currententity.oldframe < 0 ) )
			{
				VID.Printf( Defines.PRINT_ALL, "R_DrawAliasModel " + currentmodel.name + ": no such oldframe " + currententity.oldframe + '\\' );
				currententity.frame = 0;
				currententity.oldframe = 0;
			}

			if ( r_lerpmodels.value == 0F )
				currententity.backlerp = 0;
			GL_DrawAliasFrameLerp( paliashdr, currententity.backlerp );
			GL_TexEnv( ( Int32 ) All.Replace );
			GL.ShadeModel( ShadingModel.Flat );
			GL.PopMatrix();
			if ( ( currententity.flags & Defines.RF_WEAPONMODEL ) != 0 && ( r_lefthand.value == 1F ) )
			{
				GL.MatrixMode( MatrixMode.Projection );
				GL.PopMatrix();
				GL.MatrixMode( MatrixMode.Modelview );
				GL.CullFace( CullFaceMode.Front );
			}

			if ( ( currententity.flags & Defines.RF_TRANSLUCENT ) != 0 )
			{
				GL.Disable( EnableCap.Blend );
			}

			if ( ( currententity.flags & Defines.RF_DEPTHHACK ) != 0 )
				GL.DepthRange( gldepthmin, gldepthmax );
			if ( gl_shadows.value != 0F && ( currententity.flags & ( Defines.RF_TRANSLUCENT | Defines.RF_WEAPONMODEL ) ) == 0 )
			{
				GL.PushMatrix();
				R_RotateForEntity( e );
				GL.Disable( EnableCap.Texture2D );
				GL.Enable( EnableCap.Blend );
				GL.Color4( 0, 0, 0, 0.5F );
				GL_DrawAliasShadow( paliashdr, currententity.frame );
				GL.Enable( EnableCap.Texture2D );
				GL.Disable( EnableCap.Blend );
				GL.PopMatrix();
			}

			GL.Color4( 1, 1, 1, 1 );
		}
	}
}
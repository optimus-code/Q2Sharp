using Jake2.Game;
using Jake2.Server;
using Jake2.Util;
using System;

namespace Jake2.Qcommon
{
	public class PMove
	{
		public class pml_t
		{
			public Single[] origin = new Single[] { 0, 0, 0 };
			public Single[] velocity = new Single[] { 0, 0, 0 };
			public Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 }, up = new Single[] { 0, 0, 0 };
			public Single frametime;
			public csurface_t groundsurface;
			public Int32 groundcontents;
			public Single[] previous_origin = new Single[] { 0, 0, 0 };
			public Boolean ladder;
		}

		public static pmove_t pm;
		public static pml_t pml = new pml_t();
		public static Single pm_stopspeed = 100;
		public static Single pm_maxspeed = 300;
		public static Single pm_duckspeed = 100;
		public static Single pm_accelerate = 10;
		public static Single pm_airaccelerate = 0;
		public static Single pm_wateraccelerate = 10;
		public static Single pm_friction = 6;
		public static Single pm_waterfriction = 1;
		public static Single pm_waterspeed = 400;
		public static Int32[] jitterbits = new[] { 0, 4, 1, 2, 3, 5, 6, 7 };
		public static Int32[] offset = new[] { 0, -1, 1 };
		public static void PM_ClipVelocity( Single[] in_renamed, Single[] normal, Single[] out_renamed, Single overbounce )
		{
			Single backoff;
			Single change;
			Int32 i;
			backoff = Math3D.DotProduct( in_renamed, normal ) * overbounce;
			for ( i = 0; i < 3; i++ )
			{
				change = normal[i] * backoff;
				out_renamed[i] = in_renamed[i] - change;
				if ( out_renamed[i] > -Defines.MOVE_STOP_EPSILON && out_renamed[i] < Defines.MOVE_STOP_EPSILON )
					out_renamed[i] = 0;
			}
		}

		static Single[][] planes = Lib.CreateJaggedArray<Single[][]>( SV.MAX_CLIP_PLANES, 3 );
		public static void PM_StepSlideMove_( )
		{
			Int32 bumpcount, numbumps;
			Single[] dir = new Single[] { 0, 0, 0 };
			Single d;
			Int32 numplanes;
			Single[] primal_velocity = new Single[] { 0, 0, 0 };
			Int32 i, j;
			trace_t trace;
			Single[] end = new Single[] { 0, 0, 0 };
			Single time_left;
			numbumps = 4;
			Math3D.VectorCopy( pml.velocity, primal_velocity );
			numplanes = 0;
			time_left = pml.frametime;
			for ( bumpcount = 0; bumpcount < numbumps; bumpcount++ )
			{
				for ( i = 0; i < 3; i++ )
					end[i] = pml.origin[i] + time_left * pml.velocity[i];
				trace = pm.trace.Trace( pml.origin, pm.mins, pm.maxs, end );
				if ( trace.allsolid )
				{
					pml.velocity[2] = 0;
					return;
				}

				if ( trace.fraction > 0 )
				{
					Math3D.VectorCopy( trace.endpos, pml.origin );
					numplanes = 0;
				}

				if ( trace.fraction == 1 )
					break;
				if ( pm.numtouch < Defines.MAXTOUCH && trace.ent != null )
				{
					pm.touchents[pm.numtouch] = trace.ent;
					pm.numtouch++;
				}

				time_left -= time_left * trace.fraction;
				if ( numplanes >= SV.MAX_CLIP_PLANES )
				{
					Math3D.VectorCopy( Globals.vec3_origin, pml.velocity );
					break;
				}

				Math3D.VectorCopy( trace.plane.normal, planes[numplanes] );
				numplanes++;
				for ( i = 0; i < numplanes; i++ )
				{
					PM_ClipVelocity( pml.velocity, planes[i], pml.velocity, 1.01F );
					for ( j = 0; j < numplanes; j++ )
						if ( j != i )
						{
							if ( Math3D.DotProduct( pml.velocity, planes[j] ) < 0 )
								break;
						}

					if ( j == numplanes )
						break;
				}

				if ( i != numplanes )
				{
				}
				else
				{
					if ( numplanes != 2 )
					{
						Math3D.VectorCopy( Globals.vec3_origin, pml.velocity );
						break;
					}

					Math3D.CrossProduct( planes[0], planes[1], dir );
					d = Math3D.DotProduct( dir, pml.velocity );
					Math3D.VectorScale( dir, d, pml.velocity );
				}

				if ( Math3D.DotProduct( pml.velocity, primal_velocity ) <= 0 )
				{
					Math3D.VectorCopy( Globals.vec3_origin, pml.velocity );
					break;
				}
			}

			if ( pm.s.pm_time != 0 )
			{
				Math3D.VectorCopy( primal_velocity, pml.velocity );
			}
		}

		public static void PM_StepSlideMove( )
		{
			Single[] start_o = new Single[] { 0, 0, 0 }, start_v = new Single[] { 0, 0, 0 };
			Single[] down_o = new Single[] { 0, 0, 0 }, down_v = new Single[] { 0, 0, 0 };
			trace_t trace;
			Single down_dist, up_dist;
			Single[] up = new Single[] { 0, 0, 0 }, down = new Single[] { 0, 0, 0 };
			Math3D.VectorCopy( pml.origin, start_o );
			Math3D.VectorCopy( pml.velocity, start_v );
			PM_StepSlideMove_();
			Math3D.VectorCopy( pml.origin, down_o );
			Math3D.VectorCopy( pml.velocity, down_v );
			Math3D.VectorCopy( start_o, up );
			up[2] += Defines.STEPSIZE;
			trace = pm.trace.Trace( up, pm.mins, pm.maxs, up );
			if ( trace.allsolid )
				return;
			Math3D.VectorCopy( up, pml.origin );
			Math3D.VectorCopy( start_v, pml.velocity );
			PM_StepSlideMove_();
			Math3D.VectorCopy( pml.origin, down );
			down[2] -= Defines.STEPSIZE;
			trace = pm.trace.Trace( pml.origin, pm.mins, pm.maxs, down );
			if ( !trace.allsolid )
			{
				Math3D.VectorCopy( trace.endpos, pml.origin );
			}

			Math3D.VectorCopy( pml.origin, up );
			down_dist = ( down_o[0] - start_o[0] ) * ( down_o[0] - start_o[0] ) + ( down_o[1] - start_o[1] ) * ( down_o[1] - start_o[1] );
			up_dist = ( up[0] - start_o[0] ) * ( up[0] - start_o[0] ) + ( up[1] - start_o[1] ) * ( up[1] - start_o[1] );
			if ( down_dist > up_dist || trace.plane.normal[2] < Defines.MIN_STEP_NORMAL )
			{
				Math3D.VectorCopy( down_o, pml.origin );
				Math3D.VectorCopy( down_v, pml.velocity );
				return;
			}

			pml.velocity[2] = down_v[2];
		}

		public static void PM_Friction( )
		{
			Single[] vel;
			Single speed, newspeed, control;
			Single friction;
			Single drop;
			vel = pml.velocity;
			speed = ( Single ) ( Math.Sqrt( vel[0] * vel[0] + vel[1] * vel[1] + vel[2] * vel[2] ) );
			if ( speed < 1 )
			{
				vel[0] = 0;
				vel[1] = 0;
				return;
			}

			drop = 0;
			if ( ( pm.groundentity != null && pml.groundsurface != null && 0 == ( pml.groundsurface.flags & Defines.SURF_SLICK ) ) || ( pml.ladder ) )
			{
				friction = pm_friction;
				control = speed < pm_stopspeed ? pm_stopspeed : speed;
				drop += control * friction * pml.frametime;
			}

			if ( pm.waterlevel != 0 && !pml.ladder )
				drop += speed * pm_waterfriction * pm.waterlevel * pml.frametime;
			newspeed = speed - drop;
			if ( newspeed < 0 )
			{
				newspeed = 0;
			}

			newspeed /= speed;
			vel[0] = vel[0] * newspeed;
			vel[1] = vel[1] * newspeed;
			vel[2] = vel[2] * newspeed;
		}

		public static void PM_Accelerate( Single[] wishdir, Single wishspeed, Single accel )
		{
			Int32 i;
			Single addspeed, accelspeed, currentspeed;
			currentspeed = Math3D.DotProduct( pml.velocity, wishdir );
			addspeed = wishspeed - currentspeed;
			if ( addspeed <= 0 )
				return;
			accelspeed = accel * pml.frametime * wishspeed;
			if ( accelspeed > addspeed )
				accelspeed = addspeed;
			for ( i = 0; i < 3; i++ )
				pml.velocity[i] += accelspeed * wishdir[i];
		}

		public static void PM_AirAccelerate( Single[] wishdir, Single wishspeed, Single accel )
		{
			Int32 i;
			Single addspeed, accelspeed, currentspeed, wishspd = wishspeed;
			if ( wishspd > 30 )
				wishspd = 30;
			currentspeed = Math3D.DotProduct( pml.velocity, wishdir );
			addspeed = wishspd - currentspeed;
			if ( addspeed <= 0 )
				return;
			accelspeed = accel * wishspeed * pml.frametime;
			if ( accelspeed > addspeed )
				accelspeed = addspeed;
			for ( i = 0; i < 3; i++ )
				pml.velocity[i] += accelspeed * wishdir[i];
		}

		public static void PM_AddCurrents( Single[] wishvel )
		{
			Single[] v = new Single[] { 0, 0, 0 };
			Single s;
			if ( pml.ladder && Math.Abs( pml.velocity[2] ) <= 200 )
			{
				if ( ( pm.viewangles[Defines.PITCH] <= -15 ) && ( pm.cmd.forwardmove > 0 ) )
					wishvel[2] = 200;
				else if ( ( pm.viewangles[Defines.PITCH] >= 15 ) && ( pm.cmd.forwardmove > 0 ) )
					wishvel[2] = -200;
				else if ( pm.cmd.upmove > 0 )
					wishvel[2] = 200;
				else if ( pm.cmd.upmove < 0 )
					wishvel[2] = -200;
				else
					wishvel[2] = 0;
				if ( wishvel[0] < -25 )
					wishvel[0] = -25;
				else if ( wishvel[0] > 25 )
					wishvel[0] = 25;
				if ( wishvel[1] < -25 )
					wishvel[1] = -25;
				else if ( wishvel[1] > 25 )
					wishvel[1] = 25;
			}

			if ( ( pm.watertype & Defines.MASK_CURRENT ) != 0 )
			{
				Math3D.VectorClear( v );
				if ( ( pm.watertype & Defines.CONTENTS_CURRENT_0 ) != 0 )
					v[0] += 1;
				if ( ( pm.watertype & Defines.CONTENTS_CURRENT_90 ) != 0 )
					v[1] += 1;
				if ( ( pm.watertype & Defines.CONTENTS_CURRENT_180 ) != 0 )
					v[0] -= 1;
				if ( ( pm.watertype & Defines.CONTENTS_CURRENT_270 ) != 0 )
					v[1] -= 1;
				if ( ( pm.watertype & Defines.CONTENTS_CURRENT_UP ) != 0 )
					v[2] += 1;
				if ( ( pm.watertype & Defines.CONTENTS_CURRENT_DOWN ) != 0 )
					v[2] -= 1;
				s = pm_waterspeed;
				if ( ( pm.waterlevel == 1 ) && ( pm.groundentity != null ) )
					s /= 2;
				Math3D.VectorMA( wishvel, s, v, wishvel );
			}

			if ( pm.groundentity != null )
			{
				Math3D.VectorClear( v );
				if ( ( pml.groundcontents & Defines.CONTENTS_CURRENT_0 ) != 0 )
					v[0] += 1;
				if ( ( pml.groundcontents & Defines.CONTENTS_CURRENT_90 ) != 0 )
					v[1] += 1;
				if ( ( pml.groundcontents & Defines.CONTENTS_CURRENT_180 ) != 0 )
					v[0] -= 1;
				if ( ( pml.groundcontents & Defines.CONTENTS_CURRENT_270 ) != 0 )
					v[1] -= 1;
				if ( ( pml.groundcontents & Defines.CONTENTS_CURRENT_UP ) != 0 )
					v[2] += 1;
				if ( ( pml.groundcontents & Defines.CONTENTS_CURRENT_DOWN ) != 0 )
					v[2] -= 1;
				Math3D.VectorMA( wishvel, 100, v, wishvel );
			}
		}

		public static void PM_WaterMove( )
		{
			Int32 i;
			Single[] wishvel = new Single[] { 0, 0, 0 };
			Single wishspeed;
			Single[] wishdir = new Single[] { 0, 0, 0 };
			for ( i = 0; i < 3; i++ )
				wishvel[i] = pml.forward[i] * pm.cmd.forwardmove + pml.right[i] * pm.cmd.sidemove;
			if ( 0 == pm.cmd.forwardmove && 0 == pm.cmd.sidemove && 0 == pm.cmd.upmove )
				wishvel[2] -= 60;
			else
				wishvel[2] += pm.cmd.upmove;
			PM_AddCurrents( wishvel );
			Math3D.VectorCopy( wishvel, wishdir );
			wishspeed = Math3D.VectorNormalize( wishdir );
			if ( wishspeed > pm_maxspeed )
			{
				Math3D.VectorScale( wishvel, pm_maxspeed / wishspeed, wishvel );
				wishspeed = pm_maxspeed;
			}

			wishspeed *= 0.5f;
			PM_Accelerate( wishdir, wishspeed, pm_wateraccelerate );
			PM_StepSlideMove();
		}

		public static void PM_AirMove( )
		{
			Single[] wishvel = new Single[] { 0, 0, 0 };
			Single fmove, smove;
			Single[] wishdir = new Single[] { 0, 0, 0 };
			Single wishspeed;
			Single maxspeed;
			fmove = pm.cmd.forwardmove;
			smove = pm.cmd.sidemove;
			wishvel[0] = pml.forward[0] * fmove + pml.right[0] * smove;
			wishvel[1] = pml.forward[1] * fmove + pml.right[1] * smove;
			wishvel[2] = 0;
			PM_AddCurrents( wishvel );
			Math3D.VectorCopy( wishvel, wishdir );
			wishspeed = Math3D.VectorNormalize( wishdir );
			maxspeed = ( pm.s.pm_flags & pmove_t.PMF_DUCKED ) != 0 ? pm_duckspeed : pm_maxspeed;
			if ( wishspeed > maxspeed )
			{
				Math3D.VectorScale( wishvel, maxspeed / wishspeed, wishvel );
				wishspeed = maxspeed;
			}

			if ( pml.ladder )
			{
				PM_Accelerate( wishdir, wishspeed, pm_accelerate );
				if ( 0 == wishvel[2] )
				{
					if ( pml.velocity[2] > 0 )
					{
						pml.velocity[2] -= pm.s.gravity * pml.frametime;
						if ( pml.velocity[2] < 0 )
							pml.velocity[2] = 0;
					}
					else
					{
						pml.velocity[2] += pm.s.gravity * pml.frametime;
						if ( pml.velocity[2] > 0 )
							pml.velocity[2] = 0;
					}
				}

				PM_StepSlideMove();
			}
			else if ( pm.groundentity != null )
			{
				pml.velocity[2] = 0;
				PM_Accelerate( wishdir, wishspeed, pm_accelerate );
				if ( pm.s.gravity > 0 )
					pml.velocity[2] = 0;
				else
					pml.velocity[2] -= pm.s.gravity * pml.frametime;
				if ( 0 == pml.velocity[0] && 0 == pml.velocity[1] )
					return;
				PM_StepSlideMove();
			}
			else
			{
				if ( pm_airaccelerate != 0 )
					PM_AirAccelerate( wishdir, wishspeed, pm_accelerate );
				else
					PM_Accelerate( wishdir, wishspeed, 1 );
				pml.velocity[2] -= pm.s.gravity * pml.frametime;
				PM_StepSlideMove();
			}
		}

		public static void PM_CatagorizePosition( )
		{
			Single[] point = new Single[] { 0, 0, 0 };
			Int32 cont;
			trace_t trace;
			Int32 sample1;
			Int32 sample2;
			point[0] = pml.origin[0];
			point[1] = pml.origin[1];
			point[2] = pml.origin[2] - 0.25F;
			if ( pml.velocity[2] > 180 )
			{
				pm.s.pm_flags &= ( Byte ) ~pmove_t.PMF_ON_GROUND;
				pm.groundentity = null;
			}
			else
			{
				trace = pm.trace.Trace( pml.origin, pm.mins, pm.maxs, point );
				pml.groundsurface = trace.surface;
				pml.groundcontents = trace.contents;
				if ( null == trace.ent || ( trace.plane.normal[2] < 0.7 && !trace.startsolid ) )
				{
					pm.groundentity = null;
					pm.s.pm_flags &= ( Byte ) ~pmove_t.PMF_ON_GROUND;
				}
				else
				{
					pm.groundentity = trace.ent;
					if ( ( pm.s.pm_flags & pmove_t.PMF_TIME_WATERJUMP ) != 0 )
					{
						pm.s.pm_flags &= ( Byte ) ~( pmove_t.PMF_TIME_WATERJUMP | pmove_t.PMF_TIME_LAND | pmove_t.PMF_TIME_TELEPORT );
						pm.s.pm_time = 0;
					}

					if ( 0 == ( pm.s.pm_flags & pmove_t.PMF_ON_GROUND ) )
					{
						pm.s.pm_flags |= ( Byte ) pmove_t.PMF_ON_GROUND;
						if ( pml.velocity[2] < -200 )
						{
							pm.s.pm_flags |= ( Byte ) pmove_t.PMF_TIME_LAND;
							if ( pml.velocity[2] < -400 )
								pm.s.pm_time = 25;
							else
								pm.s.pm_time = 18;
						}
					}
				}

				if ( pm.numtouch < Defines.MAXTOUCH && trace.ent != null )
				{
					pm.touchents[pm.numtouch] = trace.ent;
					pm.numtouch++;
				}
			}

			pm.waterlevel = 0;
			pm.watertype = 0;
			sample2 = ( Int32 ) ( pm.viewheight - pm.mins[2] );
			sample1 = sample2 / 2;
			point[2] = pml.origin[2] + pm.mins[2] + 1;
			cont = pm.pointcontents.Pointcontents( point );
			if ( ( cont & Defines.MASK_WATER ) != 0 )
			{
				pm.watertype = cont;
				pm.waterlevel = 1;
				point[2] = pml.origin[2] + pm.mins[2] + sample1;
				cont = pm.pointcontents.Pointcontents( point );
				if ( ( cont & Defines.MASK_WATER ) != 0 )
				{
					pm.waterlevel = 2;
					point[2] = pml.origin[2] + pm.mins[2] + sample2;
					cont = pm.pointcontents.Pointcontents( point );
					if ( ( cont & Defines.MASK_WATER ) != 0 )
						pm.waterlevel = 3;
				}
			}
		}

		public static void PM_CheckJump( )
		{
			if ( ( pm.s.pm_flags & pmove_t.PMF_TIME_LAND ) != 0 )
			{
				return;
			}

			if ( pm.cmd.upmove < 10 )
			{
				pm.s.pm_flags &= ( Byte ) ~pmove_t.PMF_JUMP_HELD;
				return;
			}

			if ( ( pm.s.pm_flags & pmove_t.PMF_JUMP_HELD ) != 0 )
				return;
			if ( pm.s.pm_type == Defines.PM_DEAD )
				return;
			if ( pm.waterlevel >= 2 )
			{
				pm.groundentity = null;
				if ( pml.velocity[2] <= -300 )
					return;
				if ( pm.watertype == Defines.CONTENTS_WATER )
					pml.velocity[2] = 100;
				else if ( pm.watertype == Defines.CONTENTS_SLIME )
					pml.velocity[2] = 80;
				else
					pml.velocity[2] = 50;
				return;
			}

			if ( pm.groundentity == null )
				return;
			pm.s.pm_flags |= ( Byte ) pmove_t.PMF_JUMP_HELD;
			pm.groundentity = null;
			pml.velocity[2] += 270;
			if ( pml.velocity[2] < 270 )
				pml.velocity[2] = 270;
		}

		public static void PM_CheckSpecialMovement( )
		{
			Single[] spot = new Single[] { 0, 0, 0 };
			Int32 cont;
			Single[] flatforward = new Single[] { 0, 0, 0 };
			trace_t trace;
			if ( pm.s.pm_time != 0 )
				return;
			pml.ladder = false;
			flatforward[0] = pml.forward[0];
			flatforward[1] = pml.forward[1];
			flatforward[2] = 0;
			Math3D.VectorNormalize( flatforward );
			Math3D.VectorMA( pml.origin, 1, flatforward, spot );
			trace = pm.trace.Trace( pml.origin, pm.mins, pm.maxs, spot );
			if ( ( trace.fraction < 1 ) && ( trace.contents & Defines.CONTENTS_LADDER ) != 0 )
				pml.ladder = true;
			if ( pm.waterlevel != 2 )
				return;
			Math3D.VectorMA( pml.origin, 30, flatforward, spot );
			spot[2] += 4;
			cont = pm.pointcontents.Pointcontents( spot );
			if ( 0 == ( cont & Defines.CONTENTS_SOLID ) )
				return;
			spot[2] += 16;
			cont = pm.pointcontents.Pointcontents( spot );
			if ( cont != 0 )
				return;
			Math3D.VectorScale( flatforward, 50, pml.velocity );
			pml.velocity[2] = 350;
			pm.s.pm_flags |= ( Byte ) pmove_t.PMF_TIME_WATERJUMP;
			pm.s.pm_time = 0;
		}

		public static void PM_FlyMove( Boolean doclip )
		{
			Single speed, drop, friction, control, newspeed;
			Single currentspeed, addspeed, accelspeed;
			Int32 i;
			Single[] wishvel = new Single[] { 0, 0, 0 };
			Single fmove, smove;
			Single[] wishdir = new Single[] { 0, 0, 0 };
			Single wishspeed;
			Single[] end = new Single[] { 0, 0, 0 };
			trace_t trace;
			pm.viewheight = 22;
			speed = Math3D.VectorLength( pml.velocity );
			if ( speed < 1 )
			{
				Math3D.VectorCopy( Globals.vec3_origin, pml.velocity );
			}
			else
			{
				drop = 0;
				friction = pm_friction * 1.5F;
				control = speed < pm_stopspeed ? pm_stopspeed : speed;
				drop += control * friction * pml.frametime;
				newspeed = speed - drop;
				if ( newspeed < 0 )
					newspeed = 0;
				newspeed /= speed;
				Math3D.VectorScale( pml.velocity, newspeed, pml.velocity );
			}

			fmove = pm.cmd.forwardmove;
			smove = pm.cmd.sidemove;
			Math3D.VectorNormalize( pml.forward );
			Math3D.VectorNormalize( pml.right );
			for ( i = 0; i < 3; i++ )
				wishvel[i] = pml.forward[i] * fmove + pml.right[i] * smove;
			wishvel[2] += pm.cmd.upmove;
			Math3D.VectorCopy( wishvel, wishdir );
			wishspeed = Math3D.VectorNormalize( wishdir );
			if ( wishspeed > pm_maxspeed )
			{
				Math3D.VectorScale( wishvel, pm_maxspeed / wishspeed, wishvel );
				wishspeed = pm_maxspeed;
			}

			currentspeed = Math3D.DotProduct( pml.velocity, wishdir );
			addspeed = wishspeed - currentspeed;
			if ( addspeed <= 0 )
				return;
			accelspeed = pm_accelerate * pml.frametime * wishspeed;
			if ( accelspeed > addspeed )
				accelspeed = addspeed;
			for ( i = 0; i < 3; i++ )
				pml.velocity[i] += accelspeed * wishdir[i];
			if ( doclip )
			{
				for ( i = 0; i < 3; i++ )
					end[i] = pml.origin[i] + pml.frametime * pml.velocity[i];
				trace = pm.trace.Trace( pml.origin, pm.mins, pm.maxs, end );
				Math3D.VectorCopy( trace.endpos, pml.origin );
			}
			else
			{
				Math3D.VectorMA( pml.origin, pml.frametime, pml.velocity, pml.origin );
			}
		}

		public static void PM_CheckDuck( )
		{
			trace_t trace;
			pm.mins[0] = -16;
			pm.mins[1] = -16;
			pm.maxs[0] = 16;
			pm.maxs[1] = 16;
			if ( pm.s.pm_type == Defines.PM_GIB )
			{
				pm.mins[2] = 0;
				pm.maxs[2] = 16;
				pm.viewheight = 8;
				return;
			}

			pm.mins[2] = -24;
			if ( pm.s.pm_type == Defines.PM_DEAD )
			{
				pm.s.pm_flags |= ( Byte ) pmove_t.PMF_DUCKED;
			}
			else if ( pm.cmd.upmove < 0 && ( pm.s.pm_flags & pmove_t.PMF_ON_GROUND ) != 0 )
			{
				pm.s.pm_flags |= ( Byte ) pmove_t.PMF_DUCKED;
			}
			else
			{
				if ( ( pm.s.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
				{
					pm.maxs[2] = 32;
					trace = pm.trace.Trace( pml.origin, pm.mins, pm.maxs, pml.origin );
					if ( !trace.allsolid )
						pm.s.pm_flags &= ( Byte ) ~pmove_t.PMF_DUCKED;
				}
			}

			if ( ( pm.s.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
			{
				pm.maxs[2] = 4;
				pm.viewheight = -2;
			}
			else
			{
				pm.maxs[2] = 32;
				pm.viewheight = 22;
			}
		}

		public static void PM_DeadMove( )
		{
			Single forward;
			if ( null == pm.groundentity )
				return;
			forward = Math3D.VectorLength( pml.velocity );
			forward -= 20;
			if ( forward <= 0 )
			{
				Math3D.VectorClear( pml.velocity );
			}
			else
			{
				Math3D.VectorNormalize( pml.velocity );
				Math3D.VectorScale( pml.velocity, forward, pml.velocity );
			}
		}

		public static Boolean PM_GoodPosition( )
		{
			trace_t trace;
			Single[] origin = new Single[] { 0, 0, 0 }, end = new Single[] { 0, 0, 0 };
			Int32 i;
			if ( pm.s.pm_type == Defines.PM_SPECTATOR )
				return true;
			for ( i = 0; i < 3; i++ )
				origin[i] = end[i] = pm.s.origin[i] * 0.125F;
			trace = pm.trace.Trace( origin, pm.mins, pm.maxs, end );
			return !trace.allsolid;
		}

		public static void PM_SnapPosition( )
		{
			Int32[] sign = new[] { 0, 0, 0 };
			Int32 i, j, bits;
			Int16[] base_renamed = new Int16[] { 0, 0, 0 };
			for ( i = 0; i < 3; i++ )
				pm.s.velocity[i] = ( Int16 ) ( pml.velocity[i] * 8 );
			for ( i = 0; i < 3; i++ )
			{
				if ( pml.origin[i] >= 0 )
					sign[i] = 1;
				else
					sign[i] = -1;
				pm.s.origin[i] = ( Int16 ) ( pml.origin[i] * 8 );
				if ( pm.s.origin[i] * 0.125 == pml.origin[i] )
					sign[i] = 0;
			}

			Math3D.VectorCopy( pm.s.origin, base_renamed );
			for ( j = 0; j < 8; j++ )
			{
				bits = jitterbits[j];
				Math3D.VectorCopy( base_renamed, pm.s.origin );
				for ( i = 0; i < 3; i++ )
					if ( ( bits & ( 1 << i ) ) != 0 )
						pm.s.origin[i] += ( Int16 ) sign[i];
				if ( PM_GoodPosition() )
					return;
			}

			Math3D.VectorCopy( pml.previous_origin, pm.s.origin );
		}

		public static void PM_InitialSnapPosition( )
		{
			Int32 x, y, z;
			Int16[] base_renamed = new Int16[] { 0, 0, 0 };
			Math3D.VectorCopy( pm.s.origin, base_renamed );
			for ( z = 0; z < 3; z++ )
			{
				pm.s.origin[2] = ( Int16 ) ( base_renamed[2] + offset[z] );
				for ( y = 0; y < 3; y++ )
				{
					pm.s.origin[1] = ( Int16 ) ( base_renamed[1] + offset[y] );
					for ( x = 0; x < 3; x++ )
					{
						pm.s.origin[0] = ( Int16 ) ( base_renamed[0] + offset[x] );
						if ( PM_GoodPosition() )
						{
							pml.origin[0] = pm.s.origin[0] * 0.125F;
							pml.origin[1] = pm.s.origin[1] * 0.125F;
							pml.origin[2] = pm.s.origin[2] * 0.125F;
							Math3D.VectorCopy( pm.s.origin, pml.previous_origin );
							return;
						}
					}
				}
			}

			Com.DPrintf( "Bad InitialSnapPosition\\n" );
		}

		public static void PM_ClampAngles( )
		{
			Int16 temp;
			Int32 i;
			if ( ( pm.s.pm_flags & pmove_t.PMF_TIME_TELEPORT ) != 0 )
			{
				pm.viewangles[Defines.YAW] = Math3D.SHORT2ANGLE( pm.cmd.angles[Defines.YAW] + pm.s.delta_angles[Defines.YAW] );
				pm.viewangles[Defines.PITCH] = 0;
				pm.viewangles[Defines.ROLL] = 0;
			}
			else
			{
				for ( i = 0; i < 3; i++ )
				{
					temp = ( Int16 ) ( pm.cmd.angles[i] + pm.s.delta_angles[i] );
					pm.viewangles[i] = Math3D.SHORT2ANGLE( temp );
				}

				if ( pm.viewangles[Defines.PITCH] > 89 && pm.viewangles[Defines.PITCH] < 180 )
					pm.viewangles[Defines.PITCH] = 89;
				else if ( pm.viewangles[Defines.PITCH] < 271 && pm.viewangles[Defines.PITCH] >= 180 )
					pm.viewangles[Defines.PITCH] = 271;
			}

			Math3D.AngleVectors( pm.viewangles, pml.forward, pml.right, pml.up );
		}

		public static void Pmove( pmove_t pmove )
		{
			pm = pmove;
			pm.numtouch = 0;
			Math3D.VectorClear( pm.viewangles );
			pm.viewheight = 0;
			pm.groundentity = null;
			pm.watertype = 0;
			pm.waterlevel = 0;
			pml.groundsurface = null;
			pml.groundcontents = 0;
			pml.origin[0] = pm.s.origin[0] * 0.125F;
			pml.origin[1] = pm.s.origin[1] * 0.125F;
			pml.origin[2] = pm.s.origin[2] * 0.125F;
			pml.velocity[0] = pm.s.velocity[0] * 0.125F;
			pml.velocity[1] = pm.s.velocity[1] * 0.125F;
			pml.velocity[2] = pm.s.velocity[2] * 0.125F;
			Math3D.VectorCopy( pm.s.origin, pml.previous_origin );
			pml.frametime = ( pm.cmd.msec & 0xFF ) * 0.001F;
			PM_ClampAngles();
			if ( pm.s.pm_type == Defines.PM_SPECTATOR )
			{
				PM_FlyMove( false );
				PM_SnapPosition();
				return;
			}

			if ( pm.s.pm_type >= Defines.PM_DEAD )
			{
				pm.cmd.forwardmove = 0;
				pm.cmd.sidemove = 0;
				pm.cmd.upmove = 0;
			}

			if ( pm.s.pm_type == Defines.PM_FREEZE )
				return;
			PM_CheckDuck();
			if ( pm.snapinitial )
				PM_InitialSnapPosition();
			PM_CatagorizePosition();
			if ( pm.s.pm_type == Defines.PM_DEAD )
				PM_DeadMove();
			PM_CheckSpecialMovement();
			if ( pm.s.pm_time != 0 )
			{
				Int32 msec;
				msec = pm.cmd.msec >> 3;
				if ( msec == 0 )
					msec = 1;
				if ( msec >= ( pm.s.pm_time & 0xFF ) )
				{
					pm.s.pm_flags &= ( Byte ) ~( pmove_t.PMF_TIME_WATERJUMP | pmove_t.PMF_TIME_LAND | pmove_t.PMF_TIME_TELEPORT );
					pm.s.pm_time = 0;
				}
				else
					pm.s.pm_time = ( Byte ) ( ( pm.s.pm_time & 0xFF ) - msec );
			}

			if ( ( pm.s.pm_flags & pmove_t.PMF_TIME_TELEPORT ) != 0 )
			{
			}
			else if ( ( pm.s.pm_flags & pmove_t.PMF_TIME_WATERJUMP ) != 0 )
			{
				pml.velocity[2] -= pm.s.gravity * pml.frametime;
				if ( pml.velocity[2] < 0 )
				{
					pm.s.pm_flags &= ( Byte ) ~( pmove_t.PMF_TIME_WATERJUMP | pmove_t.PMF_TIME_LAND | pmove_t.PMF_TIME_TELEPORT );
					pm.s.pm_time = 0;
				}

				PM_StepSlideMove();
			}
			else
			{
				PM_CheckJump();
				PM_Friction();
				if ( pm.waterlevel >= 2 )
					PM_WaterMove();
				else
				{
					Single[] angles = new Single[] { 0, 0, 0 };
					Math3D.VectorCopy( pm.viewangles, angles );
					if ( angles[Defines.PITCH] > 180 )
						angles[Defines.PITCH] = angles[Defines.PITCH] - 360;
					angles[Defines.PITCH] /= 3;
					Math3D.AngleVectors( angles, pml.forward, pml.right, pml.up );
					PM_AirMove();
				}
			}

			PM_CatagorizePosition();
			PM_SnapPosition();
		}
	}
}
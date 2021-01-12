using Jake2.Client;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using System;

namespace Jake2.Server
{
	public sealed class SV
	{
		public static edict_t[] SV_TestEntityPosition( edict_t ent )
		{
			trace_t trace;
			Int32 mask;
			if ( ent.clipmask != 0 )
				mask = ent.clipmask;
			else
				mask = Defines.MASK_SOLID;
			trace = GameBase.gi.Trace( ent.s.origin, ent.mins, ent.maxs, ent.s.origin, ent, mask );
			if ( trace.startsolid )
				return GameBase.g_edicts;
			return null;
		}

		public static void SV_CheckVelocity( edict_t ent )
		{
			Int32 i;
			for ( i = 0; i < 3; i++ )
			{
				if ( ent.velocity[i] > GameBase.sv_maxvelocity.value )
					ent.velocity[i] = GameBase.sv_maxvelocity.value;
				else if ( ent.velocity[i] < -GameBase.sv_maxvelocity.value )
					ent.velocity[i] = -GameBase.sv_maxvelocity.value;
			}
		}

		public static Boolean SV_RunThink( edict_t ent )
		{
			Single thinktime;
			thinktime = ent.nextthink;
			if ( thinktime <= 0 )
				return true;
			if ( thinktime > GameBase.level.time + 0.001 )
				return true;
			ent.nextthink = 0;
			if ( ent.think == null )
				Com.Error( Defines.ERR_FATAL, "NULL ent.think" );
			ent.think.Think( ent );
			return false;
		}

		public static void SV_Impact( edict_t e1, trace_t trace )
		{
			edict_t e2;
			e2 = trace.ent;
			if ( e1.touch != null && e1.solid != Defines.SOLID_NOT )
				e1.touch.Touch( e1, e2, trace.plane, trace.surface );
			if ( e2.touch != null && e2.solid != Defines.SOLID_NOT )
				e2.touch.Touch( e2, e1, GameBase.dummyplane, null );
		}

		public static readonly Int32 MAX_CLIP_PLANES = 5;
		public static Int32 SV_FlyMove( edict_t ent, Single time, Int32 mask )
		{
			edict_t hit;
			Int32 bumpcount, numbumps;
			Single[] dir = new[] { 0F, 0F, 0F };
			Single d;
			Int32 numplanes;
			Single[][] planes = Lib.CreateJaggedArray<Single[][]>( MAX_CLIP_PLANES, 3 );
			Single[] primal_velocity = new[] { 0F, 0F, 0F };
			Single[] original_velocity = new[] { 0F, 0F, 0F };
			Single[] new_velocity = new[] { 0F, 0F, 0F };
			Int32 i, j;
			trace_t trace;
			Single[] end = new[] { 0F, 0F, 0F };
			Single time_left;
			Int32 blocked;
			numbumps = 4;
			blocked = 0;
			Math3D.VectorCopy( ent.velocity, original_velocity );
			Math3D.VectorCopy( ent.velocity, primal_velocity );
			numplanes = 0;
			time_left = time;
			ent.groundentity = null;
			for ( bumpcount = 0; bumpcount < numbumps; bumpcount++ )
			{
				for ( i = 0; i < 3; i++ )
					end[i] = ent.s.origin[i] + time_left * ent.velocity[i];
				trace = GameBase.gi.Trace( ent.s.origin, ent.mins, ent.maxs, end, ent, mask );
				if ( trace.allsolid )
				{
					Math3D.VectorCopy( Globals.vec3_origin, ent.velocity );
					return 3;
				}

				if ( trace.fraction > 0 )
				{
					Math3D.VectorCopy( trace.endpos, ent.s.origin );
					Math3D.VectorCopy( ent.velocity, original_velocity );
					numplanes = 0;
				}

				if ( trace.fraction == 1 )
					break;
				hit = trace.ent;
				if ( trace.plane.normal[2] > 0.7 )
				{
					blocked |= 1;
					if ( hit.solid == Defines.SOLID_BSP )
					{
						ent.groundentity = hit;
						ent.groundentity_linkcount = hit.linkcount;
					}
				}

				if ( trace.plane.normal[2] == 0F )
				{
					blocked |= 2;
				}

				SV_Impact( ent, trace );
				if ( !ent.inuse )
					break;
				time_left -= time_left * trace.fraction;
				if ( numplanes >= MAX_CLIP_PLANES )
				{
					Math3D.VectorCopy( Globals.vec3_origin, ent.velocity );
					return 3;
				}

				Math3D.VectorCopy( trace.plane.normal, planes[numplanes] );
				numplanes++;
				for ( i = 0; i < numplanes; i++ )
				{
					GameBase.ClipVelocity( original_velocity, planes[i], new_velocity, 1 );
					for ( j = 0; j < numplanes; j++ )
						if ( ( j != i ) && !Math3D.VectorEquals( planes[i], planes[j] ) )
						{
							if ( Math3D.DotProduct( new_velocity, planes[j] ) < 0 )
								break;
						}

					if ( j == numplanes )
						break;
				}

				if ( i != numplanes )
				{
					Math3D.VectorCopy( new_velocity, ent.velocity );
				}
				else
				{
					if ( numplanes != 2 )
					{
						Math3D.VectorCopy( Globals.vec3_origin, ent.velocity );
						return 7;
					}

					Math3D.CrossProduct( planes[0], planes[1], dir );
					d = Math3D.DotProduct( dir, ent.velocity );
					Math3D.VectorScale( dir, d, ent.velocity );
				}

				if ( Math3D.DotProduct( ent.velocity, primal_velocity ) <= 0 )
				{
					Math3D.VectorCopy( Globals.vec3_origin, ent.velocity );
					return blocked;
				}
			}

			return blocked;
		}

		public static void SV_AddGravity( edict_t ent )
		{
			ent.velocity[2] -= ent.gravity * GameBase.sv_gravity.value * Defines.FRAMETIME;
		}

		public static trace_t SV_PushEntity( edict_t ent, Single[] push )
		{
			trace_t trace;
			Single[] start = new Single[] { 0, 0, 0 };
			Single[] end = new Single[] { 0, 0, 0 };
			Int32 mask;
			Math3D.VectorCopy( ent.s.origin, start );
			Math3D.VectorAdd( start, push, end );
			var retry = false;
			do
			{
				if ( ent.clipmask != 0 )
					mask = ent.clipmask;
				else
					mask = Defines.MASK_SOLID;
				trace = GameBase.gi.Trace( start, ent.mins, ent.maxs, end, ent, mask );
				Math3D.VectorCopy( trace.endpos, ent.s.origin );
				GameBase.gi.Linkentity( ent );
				retry = false;
				if ( trace.fraction != 1 )
				{
					SV_Impact( ent, trace );
					if ( !trace.ent.inuse && ent.inuse )
					{
						Math3D.VectorCopy( start, ent.s.origin );
						GameBase.gi.Linkentity( ent );
						retry = true;
					}
				}
			}
			while ( retry );
			if ( ent.inuse )
				GameBase.G_TouchTriggers( ent );
			return trace;
		}

		public static Boolean SV_Push( edict_t pusher, Single[] move, Single[] amove )
		{
			Int32 i, e;
			edict_t[] block;
			edict_t check;
			Single[] mins = new Single[] { 0, 0, 0 };
			Single[] maxs = new Single[] { 0, 0, 0 };
			pushed_t p;
			Single[] org = new Single[] { 0, 0, 0 };
			Single[] org2 = new Single[] { 0, 0, 0 };
			Single[] move2 = new Single[] { 0, 0, 0 };
			Single[] forward = new Single[] { 0, 0, 0 };
			Single[] right = new Single[] { 0, 0, 0 };
			Single[] up = new Single[] { 0, 0, 0 };
			for ( i = 0; i < 3; i++ )
			{
				Single temp;
				temp = move[i] * 8F;
				if ( temp > 0 )
					temp += 0.5f;
				else
					temp -= 0.5f;
				move[i] = 0.125F * ( Int32 ) temp;
			}

			for ( i = 0; i < 3; i++ )
			{
				mins[i] = pusher.absmin[i] + move[i];
				maxs[i] = pusher.absmax[i] + move[i];
			}

			Math3D.VectorSubtract( Globals.vec3_origin, amove, org );
			Math3D.AngleVectors( org, forward, right, up );
			GameBase.pushed[GameBase.pushed_p].ent = pusher;
			Math3D.VectorCopy( pusher.s.origin, GameBase.pushed[GameBase.pushed_p].origin );
			Math3D.VectorCopy( pusher.s.angles, GameBase.pushed[GameBase.pushed_p].angles );
			if ( pusher.client != null )
				GameBase.pushed[GameBase.pushed_p].deltayaw = pusher.client.ps.pmove.delta_angles[Defines.YAW];
			GameBase.pushed_p++;
			Math3D.VectorAdd( pusher.s.origin, move, pusher.s.origin );
			Math3D.VectorAdd( pusher.s.angles, amove, pusher.s.angles );
			GameBase.gi.Linkentity( pusher );
			for ( e = 1; e < GameBase.num_edicts; e++ )
			{
				check = GameBase.g_edicts[e];
				if ( !check.inuse )
					continue;
				if ( check.movetype == Defines.MOVETYPE_PUSH || check.movetype == Defines.MOVETYPE_STOP || check.movetype == Defines.MOVETYPE_NONE || check.movetype == Defines.MOVETYPE_NOCLIP )
					continue;
				if ( check.area.prev == null )
					continue;
				if ( check.groundentity != pusher )
				{
					if ( check.absmin[0] >= maxs[0] || check.absmin[1] >= maxs[1] || check.absmin[2] >= maxs[2] || check.absmax[0] <= mins[0] || check.absmax[1] <= mins[1] || check.absmax[2] <= mins[2] )
						continue;
					if ( SV_TestEntityPosition( check ) == null )
						continue;
				}

				if ( ( pusher.movetype == Defines.MOVETYPE_PUSH ) || ( check.groundentity == pusher ) )
				{
					GameBase.pushed[GameBase.pushed_p].ent = check;
					Math3D.VectorCopy( check.s.origin, GameBase.pushed[GameBase.pushed_p].origin );
					Math3D.VectorCopy( check.s.angles, GameBase.pushed[GameBase.pushed_p].angles );
					GameBase.pushed_p++;
					Math3D.VectorAdd( check.s.origin, move, check.s.origin );
					if ( check.client != null )
					{
						check.client.ps.pmove.delta_angles[Defines.YAW] = ( Int16 ) ( check.client.ps.pmove.delta_angles[Defines.YAW] + amove[Defines.YAW] );
					}

					Math3D.VectorSubtract( check.s.origin, pusher.s.origin, org );
					org2[0] = Math3D.DotProduct( org, forward );
					org2[1] = -Math3D.DotProduct( org, right );
					org2[2] = Math3D.DotProduct( org, up );
					Math3D.VectorSubtract( org2, org, move2 );
					Math3D.VectorAdd( check.s.origin, move2, check.s.origin );
					if ( check.groundentity != pusher )
						check.groundentity = null;
					block = SV_TestEntityPosition( check );
					if ( block == null )
					{
						GameBase.gi.Linkentity( check );
						continue;
					}

					Math3D.VectorSubtract( check.s.origin, move, check.s.origin );
					block = SV_TestEntityPosition( check );
					if ( block == null )
					{
						GameBase.pushed_p--;
						continue;
					}
				}

				GameBase.obstacle = check;
				for ( var ip = GameBase.pushed_p - 1; ip >= 0; ip-- )
				{
					p = GameBase.pushed[ip];
					Math3D.VectorCopy( p.origin, p.ent.s.origin );
					Math3D.VectorCopy( p.angles, p.ent.s.angles );
					if ( p.ent.client != null )
					{
						p.ent.client.ps.pmove.delta_angles[Defines.YAW] = ( Int16 ) p.deltayaw;
					}

					GameBase.gi.Linkentity( p.ent );
				}

				return false;
			}

			for ( var ip = GameBase.pushed_p - 1; ip >= 0; ip-- )
				GameBase.G_TouchTriggers( GameBase.pushed[ip].ent );
			return true;
		}

		public static void SV_Physics_Pusher( edict_t ent )
		{
			Single[] move = new Single[] { 0, 0, 0 };
			Single[] amove = new Single[] { 0, 0, 0 };
			edict_t part, mv;
			if ( ( ent.flags & Defines.FL_TEAMSLAVE ) != 0 )
				return;
			GameBase.pushed_p = 0;
			for ( part = ent; part != null; part = part.teamchain )
			{
				if ( part.velocity[0] != 0 || part.velocity[1] != 0 || part.velocity[2] != 0 || part.avelocity[0] != 0 || part.avelocity[1] != 0 || part.avelocity[2] != 0 )
				{
					Math3D.VectorScale( part.velocity, Defines.FRAMETIME, move );
					Math3D.VectorScale( part.avelocity, Defines.FRAMETIME, amove );
					if ( !SV_Push( part, move, amove ) )
						break;
				}
			}

			if ( GameBase.pushed_p > Defines.MAX_EDICTS )
				SV_GAME.PF_error( Defines.ERR_FATAL, "pushed_p > &pushed[MAX_EDICTS], memory corrupted" );
			if ( part != null )
			{
				for ( mv = ent; mv != null; mv = mv.teamchain )
				{
					if ( mv.nextthink > 0 )
						mv.nextthink += Defines.FRAMETIME;
				}

				if ( part.blocked != null )
					part.blocked.Blocked( part, GameBase.obstacle );
			}
			else
			{
				for ( part = ent; part != null; part = part.teamchain )
				{
					SV_RunThink( part );
				}
			}
		}

		public static void SV_Physics_None( edict_t ent )
		{
			SV_RunThink( ent );
		}

		public static void SV_Physics_Noclip( edict_t ent )
		{
			if ( !SV_RunThink( ent ) )
				return;
			Math3D.VectorMA( ent.s.angles, Defines.FRAMETIME, ent.avelocity, ent.s.angles );
			Math3D.VectorMA( ent.s.origin, Defines.FRAMETIME, ent.velocity, ent.s.origin );
			GameBase.gi.Linkentity( ent );
		}

		public static void SV_Physics_Toss( edict_t ent )
		{
			trace_t trace;
			Single[] move = new Single[] { 0, 0, 0 };
			Single backoff;
			edict_t slave;
			Boolean wasinwater;
			Boolean isinwater;
			Single[] old_origin = new Single[] { 0, 0, 0 };
			SV_RunThink( ent );
			if ( ( ent.flags & Defines.FL_TEAMSLAVE ) != 0 )
				return;
			if ( ent.velocity[2] > 0 )
				ent.groundentity = null;
			if ( ent.groundentity != null )
				if ( !ent.groundentity.inuse )
					ent.groundentity = null;
			if ( ent.groundentity != null )
				return;
			Math3D.VectorCopy( ent.s.origin, old_origin );
			SV_CheckVelocity( ent );
			if ( ent.movetype != Defines.MOVETYPE_FLY && ent.movetype != Defines.MOVETYPE_FLYMISSILE )
				SV_AddGravity( ent );
			Math3D.VectorMA( ent.s.angles, Defines.FRAMETIME, ent.avelocity, ent.s.angles );
			Math3D.VectorScale( ent.velocity, Defines.FRAMETIME, move );
			trace = SV_PushEntity( ent, move );
			if ( !ent.inuse )
				return;
			if ( trace.fraction < 1 )
			{
				if ( ent.movetype == Defines.MOVETYPE_BOUNCE )
					backoff = 1.5F;
				else
					backoff = 1;
				GameBase.ClipVelocity( ent.velocity, trace.plane.normal, ent.velocity, backoff );
				if ( trace.plane.normal[2] > 0.7 )
				{
					if ( ent.velocity[2] < 60 || ent.movetype != Defines.MOVETYPE_BOUNCE )
					{
						ent.groundentity = trace.ent;
						ent.groundentity_linkcount = trace.ent.linkcount;
						Math3D.VectorCopy( Globals.vec3_origin, ent.velocity );
						Math3D.VectorCopy( Globals.vec3_origin, ent.avelocity );
					}
				}
			}

			wasinwater = ( ent.watertype & Defines.MASK_WATER ) != 0;
			ent.watertype = GameBase.gi.pointcontents.Pointcontents( ent.s.origin );
			isinwater = ( ent.watertype & Defines.MASK_WATER ) != 0;
			if ( isinwater )
				ent.waterlevel = 1;
			else
				ent.waterlevel = 0;
			if ( !wasinwater && isinwater )
				GameBase.gi.Positioned_sound( old_origin, ent, Defines.CHAN_AUTO, GameBase.gi.Soundindex( "misc/h2ohit1.wav" ), 1, 1, 0 );
			else if ( wasinwater && !isinwater )
				GameBase.gi.Positioned_sound( ent.s.origin, ent, Defines.CHAN_AUTO, GameBase.gi.Soundindex( "misc/h2ohit1.wav" ), 1, 1, 0 );
			for ( slave = ent.teamchain; slave != null; slave = slave.teamchain )
			{
				Math3D.VectorCopy( ent.s.origin, slave.s.origin );
				GameBase.gi.Linkentity( slave );
			}
		}

		public static void SV_AddRotationalFriction( edict_t ent )
		{
			Int32 n;
			Single adjustment;
			Math3D.VectorMA( ent.s.angles, Defines.FRAMETIME, ent.avelocity, ent.s.angles );
			adjustment = Defines.FRAMETIME * Defines.sv_stopspeed * Defines.sv_friction;
			for ( n = 0; n < 3; n++ )
			{
				if ( ent.avelocity[n] > 0 )
				{
					ent.avelocity[n] -= adjustment;
					if ( ent.avelocity[n] < 0 )
						ent.avelocity[n] = 0;
				}
				else
				{
					ent.avelocity[n] += adjustment;
					if ( ent.avelocity[n] > 0 )
						ent.avelocity[n] = 0;
				}
			}
		}

		public static void SV_Physics_Step( edict_t ent )
		{
			Boolean wasonground;
			var hitsound = false;
			Single[] vel;
			Single speed, newspeed, control;
			Single friction;
			edict_t groundentity;
			Int32 mask;
			if ( ent.groundentity == null )
				M.M_CheckGround( ent );
			groundentity = ent.groundentity;
			SV_CheckVelocity( ent );
			if ( groundentity != null )
				wasonground = true;
			else
				wasonground = false;
			if ( ent.avelocity[0] != 0 || ent.avelocity[1] != 0 || ent.avelocity[2] != 0 )
				SV_AddRotationalFriction( ent );
			if ( !wasonground )
				if ( 0 == ( ent.flags & Defines.FL_FLY ) )
					if ( !( ( ent.flags & Defines.FL_SWIM ) != 0 && ( ent.waterlevel > 2 ) ) )
					{
						if ( ent.velocity[2] < GameBase.sv_gravity.value * -0.1 )
							hitsound = true;
						if ( ent.waterlevel == 0 )
							SV_AddGravity( ent );
					}

			if ( ( ent.flags & Defines.FL_FLY ) != 0 && ( ent.velocity[2] != 0 ) )
			{
				speed = Math.Abs( ent.velocity[2] );
				control = speed < Defines.sv_stopspeed ? Defines.sv_stopspeed : speed;
				friction = Defines.sv_friction / 3;
				newspeed = speed - ( Defines.FRAMETIME * control * friction );
				if ( newspeed < 0 )
					newspeed = 0;
				newspeed /= speed;
				ent.velocity[2] *= newspeed;
			}

			if ( ( ent.flags & Defines.FL_SWIM ) != 0 && ( ent.velocity[2] != 0 ) )
			{
				speed = Math.Abs( ent.velocity[2] );
				control = speed < Defines.sv_stopspeed ? Defines.sv_stopspeed : speed;
				newspeed = speed - ( Defines.FRAMETIME * control * Defines.sv_waterfriction * ent.waterlevel );
				if ( newspeed < 0 )
					newspeed = 0;
				newspeed /= speed;
				ent.velocity[2] *= newspeed;
			}

			if ( ent.velocity[2] != 0 || ent.velocity[1] != 0 || ent.velocity[0] != 0 )
			{
				if ( ( wasonground ) || 0 != ( ent.flags & ( Defines.FL_SWIM | Defines.FL_FLY ) ) )
					if ( !( ent.health <= 0 && !M.M_CheckBottom( ent ) ) )
					{
						vel = ent.velocity;
						speed = ( Single ) Math.Sqrt( vel[0] * vel[0] + vel[1] * vel[1] );
						if ( speed != 0 )
						{
							friction = Defines.sv_friction;
							control = speed < Defines.sv_stopspeed ? Defines.sv_stopspeed : speed;
							newspeed = speed - Defines.FRAMETIME * control * friction;
							if ( newspeed < 0 )
								newspeed = 0;
							newspeed /= speed;
							vel[0] *= newspeed;
							vel[1] *= newspeed;
						}
					}

				if ( ( ent.svflags & Defines.SVF_MONSTER ) != 0 )
					mask = Defines.MASK_MONSTERSOLID;
				else
					mask = Defines.MASK_SOLID;
				SV_FlyMove( ent, Defines.FRAMETIME, mask );
				GameBase.gi.Linkentity( ent );
				GameBase.G_TouchTriggers( ent );
				if ( !ent.inuse )
					return;
				if ( ent.groundentity != null )
					if ( !wasonground )
						if ( hitsound )
							GameBase.gi.Sound( ent, 0, GameBase.gi.Soundindex( "world/land.wav" ), 1, 1, 0 );
			}

			SV_RunThink( ent );
		}

		public static Boolean SV_movestep( edict_t ent, Single[] move, Boolean relink )
		{
			Single dz;
			Single[] oldorg = new Single[] { 0, 0, 0 };
			Single[] neworg = new Single[] { 0, 0, 0 };
			Single[] end = new Single[] { 0, 0, 0 };
			trace_t trace = null;
			Int32 i;
			Single stepsize;
			Single[] test = new Single[] { 0, 0, 0 };
			Int32 contents;
			Math3D.VectorCopy( ent.s.origin, oldorg );
			Math3D.VectorAdd( ent.s.origin, move, neworg );
			if ( ( ent.flags & ( Defines.FL_SWIM | Defines.FL_FLY ) ) != 0 )
			{
				for ( i = 0; i < 2; i++ )
				{
					Math3D.VectorAdd( ent.s.origin, move, neworg );
					if ( i == 0 && ent.enemy != null )
					{
						if ( ent.goalentity == null )
							ent.goalentity = ent.enemy;
						dz = ent.s.origin[2] - ent.goalentity.s.origin[2];
						if ( ent.goalentity.client != null )
						{
							if ( dz > 40 )
								neworg[2] -= 8;
							if ( !( ( ent.flags & Defines.FL_SWIM ) != 0 && ( ent.waterlevel < 2 ) ) )
								if ( dz < 30 )
									neworg[2] += 8;
						}
						else
						{
							if ( dz > 8 )
								neworg[2] -= 8;
							else if ( dz > 0 )
								neworg[2] -= dz;
							else if ( dz < -8 )
								neworg[2] += 8;
							else
								neworg[2] += dz;
						}
					}

					trace = GameBase.gi.Trace( ent.s.origin, ent.mins, ent.maxs, neworg, ent, Defines.MASK_MONSTERSOLID );
					if ( ( ent.flags & Defines.FL_FLY ) != 0 )
					{
						if ( ent.waterlevel == 0 )
						{
							test[0] = trace.endpos[0];
							test[1] = trace.endpos[1];
							test[2] = trace.endpos[2] + ent.mins[2] + 1;
							contents = GameBase.gi.pointcontents.Pointcontents( test );
							if ( ( contents & Defines.MASK_WATER ) != 0 )
								return false;
						}
					}

					if ( ( ent.flags & Defines.FL_SWIM ) != 0 )
					{
						if ( ent.waterlevel < 2 )
						{
							test[0] = trace.endpos[0];
							test[1] = trace.endpos[1];
							test[2] = trace.endpos[2] + ent.mins[2] + 1;
							contents = GameBase.gi.pointcontents.Pointcontents( test );
							if ( ( contents & Defines.MASK_WATER ) == 0 )
								return false;
						}
					}

					if ( trace.fraction == 1 )
					{
						Math3D.VectorCopy( trace.endpos, ent.s.origin );
						if ( relink )
						{
							GameBase.gi.Linkentity( ent );
							GameBase.G_TouchTriggers( ent );
						}

						return true;
					}

					if ( ent.enemy == null )
						break;
				}

				return false;
			}

			if ( ( ent.monsterinfo.aiflags & Defines.AI_NOSTEP ) == 0 )
				stepsize = GameBase.STEPSIZE;
			else
				stepsize = 1;
			neworg[2] += stepsize;
			Math3D.VectorCopy( neworg, end );
			end[2] -= stepsize * 2;
			trace = GameBase.gi.Trace( neworg, ent.mins, ent.maxs, end, ent, Defines.MASK_MONSTERSOLID );
			if ( trace.allsolid )
				return false;
			if ( trace.startsolid )
			{
				neworg[2] -= stepsize;
				trace = GameBase.gi.Trace( neworg, ent.mins, ent.maxs, end, ent, Defines.MASK_MONSTERSOLID );
				if ( trace.allsolid || trace.startsolid )
					return false;
			}

			if ( ent.waterlevel == 0 )
			{
				test[0] = trace.endpos[0];
				test[1] = trace.endpos[1];
				test[2] = trace.endpos[2] + ent.mins[2] + 1;
				contents = GameBase.gi.pointcontents.Pointcontents( test );
				if ( ( contents & Defines.MASK_WATER ) != 0 )
					return false;
			}

			if ( trace.fraction == 1 )
			{
				if ( ( ent.flags & Defines.FL_PARTIALGROUND ) != 0 )
				{
					Math3D.VectorAdd( ent.s.origin, move, ent.s.origin );
					if ( relink )
					{
						GameBase.gi.Linkentity( ent );
						GameBase.G_TouchTriggers( ent );
					}

					ent.groundentity = null;
					return true;
				}

				return false;
			}

			Math3D.VectorCopy( trace.endpos, ent.s.origin );
			if ( !M.M_CheckBottom( ent ) )
			{
				if ( ( ent.flags & Defines.FL_PARTIALGROUND ) != 0 )
				{
					if ( relink )
					{
						GameBase.gi.Linkentity( ent );
						GameBase.G_TouchTriggers( ent );
					}

					return true;
				}

				Math3D.VectorCopy( oldorg, ent.s.origin );
				return false;
			}

			if ( ( ent.flags & Defines.FL_PARTIALGROUND ) != 0 )
			{
				ent.flags &= ~Defines.FL_PARTIALGROUND;
			}

			ent.groundentity = trace.ent;
			ent.groundentity_linkcount = trace.ent.linkcount;
			if ( relink )
			{
				GameBase.gi.Linkentity( ent );
				GameBase.G_TouchTriggers( ent );
			}

			return true;
		}

		public static Boolean SV_StepDirection( edict_t ent, Single yaw, Single dist )
		{
			Single[] move = new Single[] { 0, 0, 0 };
			Single[] oldorigin = new Single[] { 0, 0, 0 };
			Single delta;
			ent.ideal_yaw = yaw;
			M.M_ChangeYaw( ent );
			yaw = ( Single ) ( yaw * Math.PI * 2 / 360 );
			move[0] = ( Single ) Math.Cos( yaw ) * dist;
			move[1] = ( Single ) Math.Sin( yaw ) * dist;
			move[2] = 0;
			Math3D.VectorCopy( ent.s.origin, oldorigin );
			if ( SV_movestep( ent, move, false ) )
			{
				delta = ent.s.angles[Defines.YAW] - ent.ideal_yaw;
				if ( delta > 45 && delta < 315 )
				{
					Math3D.VectorCopy( oldorigin, ent.s.origin );
				}

				GameBase.gi.Linkentity( ent );
				GameBase.G_TouchTriggers( ent );
				return true;
			}

			GameBase.gi.Linkentity( ent );
			GameBase.G_TouchTriggers( ent );
			return false;
		}

		public static void SV_FixCheckBottom( edict_t ent )
		{
			ent.flags |= Defines.FL_PARTIALGROUND;
		}

		public static void SV_NewChaseDir( edict_t actor, edict_t enemy, Single dist )
		{
			Single deltax, deltay;
			Single[] d = new Single[] { 0, 0, 0 };
			Single tdir, olddir, turnaround;
			if ( enemy == null )
			{
				Com.DPrintf( "SV_NewChaseDir without enemy!\\n" );
				return;
			}

			olddir = Math3D.Anglemod( ( Int32 ) ( actor.ideal_yaw / 45 ) * 45 );
			turnaround = Math3D.Anglemod( olddir - 180 );
			deltax = enemy.s.origin[0] - actor.s.origin[0];
			deltay = enemy.s.origin[1] - actor.s.origin[1];
			if ( deltax > 10 )
				d[1] = 0;
			else if ( deltax < -10 )
				d[1] = 180;
			else
				d[1] = DI_NODIR;
			if ( deltay < -10 )
				d[2] = 270;
			else if ( deltay > 10 )
				d[2] = 90;
			else
				d[2] = DI_NODIR;
			if ( d[1] != DI_NODIR && d[2] != DI_NODIR )
			{
				if ( d[1] == 0 )
					tdir = d[2] == 90 ? 45 : 315;
				else
					tdir = d[2] == 90 ? 135 : 215;
				if ( tdir != turnaround && SV_StepDirection( actor, tdir, dist ) )
					return;
			}

			if ( ( ( Lib.Rand() & 3 ) & 1 ) != 0 || Math.Abs( deltay ) > Math.Abs( deltax ) )
			{
				tdir = d[1];
				d[1] = d[2];
				d[2] = tdir;
			}

			if ( d[1] != DI_NODIR && d[1] != turnaround && SV_StepDirection( actor, d[1], dist ) )
				return;
			if ( d[2] != DI_NODIR && d[2] != turnaround && SV_StepDirection( actor, d[2], dist ) )
				return;
			if ( olddir != DI_NODIR && SV_StepDirection( actor, olddir, dist ) )
				return;
			if ( ( Lib.Rand() & 1 ) != 0 )
			{
				for ( tdir = 0; tdir <= 315; tdir += 45 )
					if ( tdir != turnaround && SV_StepDirection( actor, tdir, dist ) )
						return;
			}
			else
			{
				for ( tdir = 315; tdir >= 0; tdir -= 45 )
					if ( tdir != turnaround && SV_StepDirection( actor, tdir, dist ) )
						return;
			}

			if ( turnaround != DI_NODIR && SV_StepDirection( actor, turnaround, dist ) )
				return;
			actor.ideal_yaw = olddir;
			if ( !M.M_CheckBottom( actor ) )
				SV_FixCheckBottom( actor );
		}

		public static Boolean SV_CloseEnough( edict_t ent, edict_t goal, Single dist )
		{
			Int32 i;
			for ( i = 0; i < 3; i++ )
			{
				if ( goal.absmin[i] > ent.absmax[i] + dist )
					return false;
				if ( goal.absmax[i] < ent.absmin[i] - dist )
					return false;
			}

			return true;
		}

		public static Int32 DI_NODIR = -1;
	}
}
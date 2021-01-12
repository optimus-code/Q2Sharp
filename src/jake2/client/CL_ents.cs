using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;

namespace Q2Sharp.Client
{
	public class CL_ents
	{
		static Int32[] bitcounts = new Int32[32];
		static Int32[] bfg_lightramp = new[] { 300, 400, 600, 300, 150, 75 };
		public static Int32 ParseEntityBits( Int32[] bits )
		{
			Int32 b, total;
			Int32 i;
			Int32 number;
			total = MSG.ReadByte( Globals.net_message );
			if ( ( total & Defines.U_MOREBITS1 ) != 0 )
			{
				b = MSG.ReadByte( Globals.net_message );
				total |= b << 8;
			}

			if ( ( total & Defines.U_MOREBITS2 ) != 0 )
			{
				b = MSG.ReadByte( Globals.net_message );
				total |= b << 16;
			}

			if ( ( total & Defines.U_MOREBITS3 ) != 0 )
			{
				b = MSG.ReadByte( Globals.net_message );
				total |= b << 24;
			}

			for ( i = 0; i < 32; i++ )
				if ( ( total & ( 1 << i ) ) != 0 )
					bitcounts[i]++;
			if ( ( total & Defines.U_NUMBER16 ) != 0 )
				number = MSG.ReadShort( Globals.net_message );
			else
				number = MSG.ReadByte( Globals.net_message );
			bits[0] = total;
			return number;
		}

		public static void ParseDelta( entity_state_t from, entity_state_t to, Int32 number, Int32 bits )
		{
			to.Set( from );
			Math3D.VectorCopy( from.origin, to.old_origin );
			to.number = number;
			if ( ( bits & Defines.U_MODEL ) != 0 )
				to.modelindex = MSG.ReadByte( Globals.net_message );
			if ( ( bits & Defines.U_MODEL2 ) != 0 )
				to.modelindex2 = MSG.ReadByte( Globals.net_message );
			if ( ( bits & Defines.U_MODEL3 ) != 0 )
				to.modelindex3 = MSG.ReadByte( Globals.net_message );
			if ( ( bits & Defines.U_MODEL4 ) != 0 )
				to.modelindex4 = MSG.ReadByte( Globals.net_message );
			if ( ( bits & Defines.U_FRAME8 ) != 0 )
				to.frame = MSG.ReadByte( Globals.net_message );
			if ( ( bits & Defines.U_FRAME16 ) != 0 )
				to.frame = MSG.ReadShort( Globals.net_message );
			if ( ( bits & Defines.U_SKIN8 ) != 0 && ( bits & Defines.U_SKIN16 ) != 0 )
				to.skinnum = MSG.ReadLong( Globals.net_message );
			else if ( ( bits & Defines.U_SKIN8 ) != 0 )
				to.skinnum = MSG.ReadByte( Globals.net_message );
			else if ( ( bits & Defines.U_SKIN16 ) != 0 )
				to.skinnum = MSG.ReadShort( Globals.net_message );
			if ( ( bits & ( Defines.U_EFFECTS8 | Defines.U_EFFECTS16 ) ) == ( Defines.U_EFFECTS8 | Defines.U_EFFECTS16 ) )
				to.effects = MSG.ReadLong( Globals.net_message );
			else if ( ( bits & Defines.U_EFFECTS8 ) != 0 )
				to.effects = MSG.ReadByte( Globals.net_message );
			else if ( ( bits & Defines.U_EFFECTS16 ) != 0 )
				to.effects = MSG.ReadShort( Globals.net_message );
			if ( ( bits & ( Defines.U_RENDERFX8 | Defines.U_RENDERFX16 ) ) == ( Defines.U_RENDERFX8 | Defines.U_RENDERFX16 ) )
				to.renderfx = MSG.ReadLong( Globals.net_message );
			else if ( ( bits & Defines.U_RENDERFX8 ) != 0 )
				to.renderfx = MSG.ReadByte( Globals.net_message );
			else if ( ( bits & Defines.U_RENDERFX16 ) != 0 )
				to.renderfx = MSG.ReadShort( Globals.net_message );
			if ( ( bits & Defines.U_ORIGIN1 ) != 0 )
				to.origin[0] = MSG.ReadCoord( Globals.net_message );
			if ( ( bits & Defines.U_ORIGIN2 ) != 0 )
				to.origin[1] = MSG.ReadCoord( Globals.net_message );
			if ( ( bits & Defines.U_ORIGIN3 ) != 0 )
				to.origin[2] = MSG.ReadCoord( Globals.net_message );
			if ( ( bits & Defines.U_ANGLE1 ) != 0 )
				to.angles[0] = MSG.ReadAngle( Globals.net_message );
			if ( ( bits & Defines.U_ANGLE2 ) != 0 )
				to.angles[1] = MSG.ReadAngle( Globals.net_message );
			if ( ( bits & Defines.U_ANGLE3 ) != 0 )
				to.angles[2] = MSG.ReadAngle( Globals.net_message );
			if ( ( bits & Defines.U_OLDORIGIN ) != 0 )
				MSG.ReadPos( Globals.net_message, to.old_origin );
			if ( ( bits & Defines.U_SOUND ) != 0 )
				to.sound = MSG.ReadByte( Globals.net_message );
			if ( ( bits & Defines.U_EVENT ) != 0 )
				to.event_renamed = MSG.ReadByte( Globals.net_message );
			else
				to.event_renamed = 0;
			if ( ( bits & Defines.U_SOLID ) != 0 )
				to.solid = MSG.ReadShort( Globals.net_message );
		}

		public static void DeltaEntity( frame_t frame, Int32 newnum, entity_state_t old, Int32 bits )
		{
			centity_t ent;
			entity_state_t state;
			ent = Globals.cl_entities[newnum];
			state = Globals.cl_parse_entities[Globals.cl.parse_entities & ( Defines.MAX_PARSE_ENTITIES - 1 )];
			Globals.cl.parse_entities++;
			frame.num_entities++;
			ParseDelta( old, state, newnum, bits );
			if ( state.modelindex != ent.current.modelindex || state.modelindex2 != ent.current.modelindex2 || state.modelindex3 != ent.current.modelindex3 || state.modelindex4 != ent.current.modelindex4 || Math.Abs( state.origin[0] - ent.current.origin[0] ) > 512 || Math.Abs( state.origin[1] - ent.current.origin[1] ) > 512 || Math.Abs( state.origin[2] - ent.current.origin[2] ) > 512 || state.event_renamed == Defines.EV_PLAYER_TELEPORT || state.event_renamed == Defines.EV_OTHER_TELEPORT )
			{
				ent.serverframe = -99;
			}

			if ( ent.serverframe != Globals.cl.frame.serverframe - 1 )
			{
				ent.trailcount = 1024;
				ent.prev.Set( state );
				if ( state.event_renamed == Defines.EV_OTHER_TELEPORT )
				{
					Math3D.VectorCopy( state.origin, ent.prev.origin );
					Math3D.VectorCopy( state.origin, ent.lerp_origin );
				}
				else
				{
					Math3D.VectorCopy( state.old_origin, ent.prev.origin );
					Math3D.VectorCopy( state.old_origin, ent.lerp_origin );
				}
			}
			else
			{
				ent.prev.Set( ent.current );
			}

			ent.serverframe = Globals.cl.frame.serverframe;
			ent.current.Set( state );
		}

		private static readonly Int32[] iw = new[] { 0 };
		public static void ParsePacketEntities( frame_t oldframe, frame_t newframe )
		{
			Int32 newnum;
			var bits = 0;
			entity_state_t oldstate = null;
			Int32 oldnum;
			newframe.parse_entities = Globals.cl.parse_entities;
			newframe.num_entities = 0;
			var oldindex = 0;
			if ( oldframe == null )
				oldnum = 99999;
			else
			{
				oldstate = Globals.cl_parse_entities[( oldframe.parse_entities + oldindex ) & ( Defines.MAX_PARSE_ENTITIES - 1 )];
				oldnum = oldstate.number;
			}

			while ( true )
			{
				iw[0] = bits;
				newnum = ParseEntityBits( iw );
				bits = iw[0];
				if ( newnum >= Defines.MAX_EDICTS )
					Com.Error( Defines.ERR_DROP, "CL_ParsePacketEntities: bad number:" + newnum );
				if ( Globals.net_message.readcount > Globals.net_message.cursize )
					Com.Error( Defines.ERR_DROP, "CL_ParsePacketEntities: end of message" );
				if ( 0 == newnum )
					break;
				while ( oldnum < newnum )
				{
					if ( Globals.cl_shownet.value == 3 )
						Com.Printf( "   unchanged: " + oldnum + "\\n" );
					DeltaEntity( newframe, oldnum, oldstate, 0 );
					oldindex++;
					if ( oldindex >= oldframe.num_entities )
						oldnum = 99999;
					else
					{
						oldstate = Globals.cl_parse_entities[( oldframe.parse_entities + oldindex ) & ( Defines.MAX_PARSE_ENTITIES - 1 )];
						oldnum = oldstate.number;
					}
				}

				if ( ( bits & Defines.U_REMOVE ) != 0 )
				{
					if ( Globals.cl_shownet.value == 3 )
						Com.Printf( "   remove: " + newnum + "\\n" );
					if ( oldnum != newnum )
						Com.Printf( "U_REMOVE: oldnum != newnum\\n" );
					oldindex++;
					if ( oldindex >= oldframe.num_entities )
						oldnum = 99999;
					else
					{
						oldstate = Globals.cl_parse_entities[( oldframe.parse_entities + oldindex ) & ( Defines.MAX_PARSE_ENTITIES - 1 )];
						oldnum = oldstate.number;
					}

					continue;
				}

				if ( oldnum == newnum )
				{
					if ( Globals.cl_shownet.value == 3 )
						Com.Printf( "   delta: " + newnum + "\\n" );
					DeltaEntity( newframe, newnum, oldstate, bits );
					oldindex++;
					if ( oldindex >= oldframe.num_entities )
						oldnum = 99999;
					else
					{
						oldstate = Globals.cl_parse_entities[( oldframe.parse_entities + oldindex ) & ( Defines.MAX_PARSE_ENTITIES - 1 )];
						oldnum = oldstate.number;
					}

					continue;
				}

				if ( oldnum > newnum )
				{
					if ( Globals.cl_shownet.value == 3 )
						Com.Printf( "   baseline: " + newnum + "\\n" );
					DeltaEntity( newframe, newnum, Globals.cl_entities[newnum].baseline, bits );
					continue;
				}
			}

			while ( oldnum != 99999 )
			{
				if ( Globals.cl_shownet.value == 3 )
					Com.Printf( "   unchanged: " + oldnum + "\\n" );
				DeltaEntity( newframe, oldnum, oldstate, 0 );
				oldindex++;
				if ( oldindex >= oldframe.num_entities )
					oldnum = 99999;
				else
				{
					oldstate = Globals.cl_parse_entities[( oldframe.parse_entities + oldindex ) & ( Defines.MAX_PARSE_ENTITIES - 1 )];
					oldnum = oldstate.number;
				}
			}
		}

		public static void ParsePlayerstate( frame_t oldframe, frame_t newframe )
		{
			Int32 flags;
			player_state_t state;
			Int32 i;
			Int32 statbits;
			state = newframe.playerstate;
			if ( oldframe != null )
				state.Set( oldframe.playerstate );
			else
				state.Clear();
			flags = MSG.ReadShort( Globals.net_message );
			if ( ( flags & Defines.PS_M_TYPE ) != 0 )
				state.pmove.pm_type = MSG.ReadByte( Globals.net_message );
			if ( ( flags & Defines.PS_M_ORIGIN ) != 0 )
			{
				state.pmove.origin[0] = MSG.ReadShort( Globals.net_message );
				state.pmove.origin[1] = MSG.ReadShort( Globals.net_message );
				state.pmove.origin[2] = MSG.ReadShort( Globals.net_message );
			}

			if ( ( flags & Defines.PS_M_VELOCITY ) != 0 )
			{
				state.pmove.velocity[0] = MSG.ReadShort( Globals.net_message );
				state.pmove.velocity[1] = MSG.ReadShort( Globals.net_message );
				state.pmove.velocity[2] = MSG.ReadShort( Globals.net_message );
			}

			if ( ( flags & Defines.PS_M_TIME ) != 0 )
			{
				state.pmove.pm_time = ( Byte ) MSG.ReadByte( Globals.net_message );
			}

			if ( ( flags & Defines.PS_M_FLAGS ) != 0 )
				state.pmove.pm_flags = ( Byte ) MSG.ReadByte( Globals.net_message );
			if ( ( flags & Defines.PS_M_GRAVITY ) != 0 )
				state.pmove.gravity = MSG.ReadShort( Globals.net_message );
			if ( ( flags & Defines.PS_M_DELTA_ANGLES ) != 0 )
			{
				state.pmove.delta_angles[0] = MSG.ReadShort( Globals.net_message );
				state.pmove.delta_angles[1] = MSG.ReadShort( Globals.net_message );
				state.pmove.delta_angles[2] = MSG.ReadShort( Globals.net_message );
			}

			if ( Globals.cl.attractloop )
				state.pmove.pm_type = Defines.PM_FREEZE;
			if ( ( flags & Defines.PS_VIEWOFFSET ) != 0 )
			{
				state.viewoffset[0] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.viewoffset[1] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.viewoffset[2] = MSG.ReadChar( Globals.net_message ) * 0.25F;
			}

			if ( ( flags & Defines.PS_VIEWANGLES ) != 0 )
			{
				state.viewangles[0] = MSG.ReadAngle16( Globals.net_message );
				state.viewangles[1] = MSG.ReadAngle16( Globals.net_message );
				state.viewangles[2] = MSG.ReadAngle16( Globals.net_message );
			}

			if ( ( flags & Defines.PS_KICKANGLES ) != 0 )
			{
				state.kick_angles[0] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.kick_angles[1] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.kick_angles[2] = MSG.ReadChar( Globals.net_message ) * 0.25F;
			}

			if ( ( flags & Defines.PS_WEAPONINDEX ) != 0 )
			{
				state.gunindex = MSG.ReadByte( Globals.net_message );
			}

			if ( ( flags & Defines.PS_WEAPONFRAME ) != 0 )
			{
				state.gunframe = MSG.ReadByte( Globals.net_message );
				state.gunoffset[0] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.gunoffset[1] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.gunoffset[2] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.gunangles[0] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.gunangles[1] = MSG.ReadChar( Globals.net_message ) * 0.25F;
				state.gunangles[2] = MSG.ReadChar( Globals.net_message ) * 0.25F;
			}

			if ( ( flags & Defines.PS_BLEND ) != 0 )
			{
				state.blend[0] = MSG.ReadByte( Globals.net_message ) / 255F;
				state.blend[1] = MSG.ReadByte( Globals.net_message ) / 255F;
				state.blend[2] = MSG.ReadByte( Globals.net_message ) / 255F;
				state.blend[3] = MSG.ReadByte( Globals.net_message ) / 255F;
			}

			if ( ( flags & Defines.PS_FOV ) != 0 )
				state.fov = MSG.ReadByte( Globals.net_message );
			if ( ( flags & Defines.PS_RDFLAGS ) != 0 )
				state.rdflags = MSG.ReadByte( Globals.net_message );
			statbits = MSG.ReadLong( Globals.net_message );
			for ( i = 0; i < Defines.MAX_STATS; i++ )
				if ( ( statbits & ( 1 << i ) ) != 0 )
					state.stats[i] = MSG.ReadShort( Globals.net_message );
		}

		public static void FireEntityEvents( frame_t frame )
		{
			entity_state_t s1;
			Int32 pnum, num;
			for ( pnum = 0; pnum < frame.num_entities; pnum++ )
			{
				num = ( frame.parse_entities + pnum ) & ( Defines.MAX_PARSE_ENTITIES - 1 );
				s1 = Globals.cl_parse_entities[num];
				if ( s1.event_renamed != 0 )
					CL_fx.EntityEvent( s1 );
				if ( ( s1.effects & Defines.EF_TELEPORTER ) != 0 )
					CL_fx.TeleporterParticles( s1 );
			}
		}

		public static void ParseFrame( )
		{
			Int32 cmd;
			Int32 len;
			frame_t old;
			Globals.cl.frame.Reset();
			Globals.cl.frame.serverframe = MSG.ReadLong( Globals.net_message );
			Globals.cl.frame.deltaframe = MSG.ReadLong( Globals.net_message );
			Globals.cl.frame.servertime = Globals.cl.frame.serverframe * 100;
			if ( Globals.cls.serverProtocol != 26 )
				Globals.cl.surpressCount = MSG.ReadByte( Globals.net_message );
			if ( Globals.cl_shownet.value == 3 )
				Com.Printf( "   frame:" + Globals.cl.frame.serverframe + "  delta:" + Globals.cl.frame.deltaframe + "\\n" );
			if ( Globals.cl.frame.deltaframe <= 0 )
			{
				Globals.cl.frame.valid = true;
				old = null;
				Globals.cls.demowaiting = false;
			}
			else
			{
				old = Globals.cl.frames[Globals.cl.frame.deltaframe & Defines.UPDATE_MASK];
				if ( !old.valid )
				{
					Com.Printf( "Delta from invalid frame (not supposed to happen!).\\n" );
				}

				if ( old.serverframe != Globals.cl.frame.deltaframe )
				{
					Com.Printf( "Delta frame too old.\\n" );
				}
				else if ( Globals.cl.parse_entities - old.parse_entities > Defines.MAX_PARSE_ENTITIES - 128 )
				{
					Com.Printf( "Delta parse_entities too old.\\n" );
				}
				else
					Globals.cl.frame.valid = true;
			}

			if ( Globals.cl.time > Globals.cl.frame.servertime )
				Globals.cl.time = Globals.cl.frame.servertime;
			else if ( Globals.cl.time < Globals.cl.frame.servertime - 100 )
				Globals.cl.time = Globals.cl.frame.servertime - 100;
			len = MSG.ReadByte( Globals.net_message );
			MSG.ReadData( Globals.net_message, Globals.cl.frame.areabits, len );
			cmd = MSG.ReadByte( Globals.net_message );
			CL_parse.SHOWNET( CL_parse.svc_strings[cmd] );
			if ( cmd != Defines.svc_playerinfo )
				Com.Error( Defines.ERR_DROP, "CL_ParseFrame: not playerinfo" );
			ParsePlayerstate( old, Globals.cl.frame );
			cmd = MSG.ReadByte( Globals.net_message );
			CL_parse.SHOWNET( CL_parse.svc_strings[cmd] );
			if ( cmd != Defines.svc_packetentities )
				Com.Error( Defines.ERR_DROP, "CL_ParseFrame: not packetentities" );
			ParsePacketEntities( old, Globals.cl.frame );
			Globals.cl.frames[Globals.cl.frame.serverframe & Defines.UPDATE_MASK].Set( Globals.cl.frame );
			if ( Globals.cl.frame.valid )
			{
				if ( Globals.cls.state != Defines.ca_active )
				{
					Globals.cls.state = Defines.ca_active;
					Globals.cl.force_refdef = true;
					Globals.cl.predicted_origin[0] = Globals.cl.frame.playerstate.pmove.origin[0] * 0.125F;
					Globals.cl.predicted_origin[1] = Globals.cl.frame.playerstate.pmove.origin[1] * 0.125F;
					Globals.cl.predicted_origin[2] = Globals.cl.frame.playerstate.pmove.origin[2] * 0.125F;
					Math3D.VectorCopy( Globals.cl.frame.playerstate.viewangles, Globals.cl.predicted_angles );
					if ( Globals.cls.disable_servercount != Globals.cl.servercount && Globals.cl.refresh_prepped )
						SCR.EndLoadingPlaque();
				}

				Globals.cl.sound_prepped = true;
				FireEntityEvents( Globals.cl.frame );
				CL_pred.CheckPredictionError();
			}
		}

		private static readonly entity_t ent = new entity_t();
		static void AddPacketEntities( frame_t frame )
		{
			entity_state_t s1;
			Single autorotate;
			Int32 i;
			Int32 pnum;
			centity_t cent;
			Int32 autoanim;
			clientinfo_t ci;
			Int32 effects, renderfx;
			autorotate = Math3D.Anglemod( Globals.cl.time / 10 );
			autoanim = 2 * Globals.cl.time / 1000;
			ent.Clear();
			for ( pnum = 0; pnum < frame.num_entities; pnum++ )
			{
				s1 = Globals.cl_parse_entities[( frame.parse_entities + pnum ) & ( Defines.MAX_PARSE_ENTITIES - 1 )];
				cent = Globals.cl_entities[s1.number];
				effects = s1.effects;
				renderfx = s1.renderfx;
				if ( ( effects & Defines.EF_ANIM01 ) != 0 )
					ent.frame = autoanim & 1;
				else if ( ( effects & Defines.EF_ANIM23 ) != 0 )
					ent.frame = 2 + ( autoanim & 1 );
				else if ( ( effects & Defines.EF_ANIM_ALL ) != 0 )
					ent.frame = autoanim;
				else if ( ( effects & Defines.EF_ANIM_ALLFAST ) != 0 )
					ent.frame = Globals.cl.time / 100;
				else
					ent.frame = s1.frame;
				if ( ( effects & Defines.EF_PENT ) != 0 )
				{
					effects &= ~Defines.EF_PENT;
					effects |= Defines.EF_COLOR_SHELL;
					renderfx |= Defines.RF_SHELL_RED;
				}

				if ( ( effects & Defines.EF_QUAD ) != 0 )
				{
					effects &= ~Defines.EF_QUAD;
					effects |= Defines.EF_COLOR_SHELL;
					renderfx |= Defines.RF_SHELL_BLUE;
				}

				if ( ( effects & Defines.EF_DOUBLE ) != 0 )
				{
					effects &= ~Defines.EF_DOUBLE;
					effects |= Defines.EF_COLOR_SHELL;
					renderfx |= Defines.RF_SHELL_DOUBLE;
				}

				if ( ( effects & Defines.EF_HALF_DAMAGE ) != 0 )
				{
					effects &= ~Defines.EF_HALF_DAMAGE;
					effects |= Defines.EF_COLOR_SHELL;
					renderfx |= Defines.RF_SHELL_HALF_DAM;
				}

				ent.oldframe = cent.prev.frame;
				ent.backlerp = 1F - Globals.cl.lerpfrac;
				if ( ( renderfx & ( Defines.RF_FRAMELERP | Defines.RF_BEAM ) ) != 0 )
				{
					Math3D.VectorCopy( cent.current.origin, ent.origin );
					Math3D.VectorCopy( cent.current.old_origin, ent.oldorigin );
				}
				else
				{
					for ( i = 0; i < 3; i++ )
					{
						ent.origin[i] = ent.oldorigin[i] = cent.prev.origin[i] + Globals.cl.lerpfrac * ( cent.current.origin[i] - cent.prev.origin[i] );
					}
				}

				if ( ( renderfx & Defines.RF_BEAM ) != 0 )
				{
					ent.alpha = 0.3F;
					ent.skinnum = ( Int32 ) ( s1.skinnum >> ( ( Globals.rnd.Next( 4 ) ) * 8 ) ) & 0xff;
					//Math.Random();
					ent.model = null;
				}
				else
				{
					if ( s1.modelindex == 255 )
					{
						ent.skinnum = 0;
						ci = Globals.cl.clientinfo[s1.skinnum & 0xff];
						ent.skin = ci.skin;
						ent.model = ci.model;
						if ( null == ent.skin || null == ent.model )
						{
							ent.skin = Globals.cl.baseclientinfo.skin;
							ent.model = Globals.cl.baseclientinfo.model;
						}

						if ( ( renderfx & Defines.RF_USE_DISGUISE ) != 0 )
						{
							if ( ent.skin.name.StartsWith( "players/male" ) )
							{
								ent.skin = Globals.re.RegisterSkin( "players/male/disguise.pcx" );
								ent.model = Globals.re.RegisterModel( "players/male/tris.md2" );
							}
							else if ( ent.skin.name.StartsWith( "players/female" ) )
							{
								ent.skin = Globals.re.RegisterSkin( "players/female/disguise.pcx" );
								ent.model = Globals.re.RegisterModel( "players/female/tris.md2" );
							}
							else if ( ent.skin.name.StartsWith( "players/cyborg" ) )
							{
								ent.skin = Globals.re.RegisterSkin( "players/cyborg/disguise.pcx" );
								ent.model = Globals.re.RegisterModel( "players/cyborg/tris.md2" );
							}
						}
					}
					else
					{
						ent.skinnum = ( Int32 ) s1.skinnum;
						ent.skin = null;
						ent.model = Globals.cl.model_draw[s1.modelindex];
					}
				}

				if ( renderfx == Defines.RF_TRANSLUCENT )
					ent.alpha = 0.7F;
				if ( ( effects & Defines.EF_COLOR_SHELL ) != 0 )
					ent.flags = 0;
				else
					ent.flags = renderfx;
				if ( ( effects & Defines.EF_ROTATE ) != 0 )
				{
					ent.angles[0] = 0;
					ent.angles[1] = autorotate;
					ent.angles[2] = 0;
				}
				else if ( ( effects & Defines.EF_SPINNINGLIGHTS ) != 0 )
				{
					ent.angles[0] = 0;
					ent.angles[1] = Math3D.Anglemod( Globals.cl.time / 2 ) + s1.angles[1];
					ent.angles[2] = 180;
					{
						Single[] forward = new Single[] { 0, 0, 0 };
						Single[] start = new Single[] { 0, 0, 0 };
						Math3D.AngleVectors( ent.angles, forward, null, null );
						Math3D.VectorMA( ent.origin, 64, forward, start );
						V.AddLight( start, 100, 1, 0, 0 );
					}
				}
				else
				{
					Single a1, a2;
					for ( i = 0; i < 3; i++ )
					{
						a1 = cent.current.angles[i];
						a2 = cent.prev.angles[i];
						ent.angles[i] = Math3D.LerpAngle( a2, a1, Globals.cl.lerpfrac );
					}
				}

				if ( s1.number == Globals.cl.playernum + 1 )
				{
					ent.flags |= Defines.RF_VIEWERMODEL;
					if ( ( effects & Defines.EF_FLAG1 ) != 0 )
						V.AddLight( ent.origin, 225, 1F, 0.1F, 0.1F );
					else if ( ( effects & Defines.EF_FLAG2 ) != 0 )
						V.AddLight( ent.origin, 225, 0.1F, 0.1F, 1F );
					else if ( ( effects & Defines.EF_TAGTRAIL ) != 0 )
						V.AddLight( ent.origin, 225, 1F, 1F, 0F );
					else if ( ( effects & Defines.EF_TRACKERTRAIL ) != 0 )
						V.AddLight( ent.origin, 225, -1F, -1F, -1F );
					continue;
				}

				if ( s1.modelindex == 0 )
					continue;
				if ( ( effects & Defines.EF_BFG ) != 0 )
				{
					ent.flags |= Defines.RF_TRANSLUCENT;
					ent.alpha = 0.3F;
				}

				if ( ( effects & Defines.EF_PLASMA ) != 0 )
				{
					ent.flags |= Defines.RF_TRANSLUCENT;
					ent.alpha = 0.6F;
				}

				if ( ( effects & Defines.EF_SPHERETRANS ) != 0 )
				{
					ent.flags |= Defines.RF_TRANSLUCENT;
					if ( ( effects & Defines.EF_TRACKERTRAIL ) != 0 )
						ent.alpha = 0.6F;
					else
						ent.alpha = 0.3F;
				}

				V.AddEntity( ent );
				if ( ( effects & Defines.EF_COLOR_SHELL ) != 0 )
				{
					if ( ( renderfx & Defines.RF_SHELL_HALF_DAM ) != 0 )
					{
						if ( FS.Developer_searchpath( 2 ) == 2 )
						{
							if ( ( renderfx & ( Defines.RF_SHELL_RED | Defines.RF_SHELL_BLUE | Defines.RF_SHELL_DOUBLE ) ) != 0 )
								renderfx &= ~Defines.RF_SHELL_HALF_DAM;
						}
					}

					if ( ( renderfx & Defines.RF_SHELL_DOUBLE ) != 0 )
					{
						if ( FS.Developer_searchpath( 2 ) == 2 )
						{
							if ( ( renderfx & ( Defines.RF_SHELL_RED | Defines.RF_SHELL_BLUE | Defines.RF_SHELL_GREEN ) ) != 0 )
								renderfx &= ~Defines.RF_SHELL_DOUBLE;
							if ( ( renderfx & Defines.RF_SHELL_RED ) != 0 )
								renderfx |= Defines.RF_SHELL_BLUE;
							else if ( ( renderfx & Defines.RF_SHELL_BLUE ) != 0 )
								if ( ( renderfx & Defines.RF_SHELL_GREEN ) != 0 )
									renderfx &= ~Defines.RF_SHELL_BLUE;
								else
									renderfx |= Defines.RF_SHELL_GREEN;
						}
					}

					ent.flags = renderfx | Defines.RF_TRANSLUCENT;
					ent.alpha = 0.3F;
					V.AddEntity( ent );
				}

				ent.skin = null;
				ent.skinnum = 0;
				ent.flags = 0;
				ent.alpha = 0;
				if ( s1.modelindex2 != 0 )
				{
					if ( s1.modelindex2 == 255 )
					{
						ci = Globals.cl.clientinfo[s1.skinnum & 0xff];
						i = ( Int32 ) ( s1.skinnum >> 8 ); // These casts probably break something TODO
						if ( 0 == Globals.cl_vwep.value || i > Defines.MAX_CLIENTWEAPONMODELS - 1 )
							i = 0;
						ent.model = ci.weaponmodel[i];
						if ( null == ent.model )
						{
							if ( i != 0 )
								ent.model = ci.weaponmodel[0];
							if ( null == ent.model )
								ent.model = Globals.cl.baseclientinfo.weaponmodel[0];
						}
					}
					else
						ent.model = Globals.cl.model_draw[s1.modelindex2];
					if ( Globals.cl.configstrings[Defines.CS_MODELS + ( s1.modelindex2 )].EqualsIgnoreCase( "models/items/shell/tris.md2" ) )
					{
						ent.alpha = 0.32F;
						ent.flags = Defines.RF_TRANSLUCENT;
					}

					V.AddEntity( ent );
					ent.flags = 0;
					ent.alpha = 0;
				}

				if ( s1.modelindex3 != 0 )
				{
					ent.model = Globals.cl.model_draw[s1.modelindex3];
					V.AddEntity( ent );
				}

				if ( s1.modelindex4 != 0 )
				{
					ent.model = Globals.cl.model_draw[s1.modelindex4];
					V.AddEntity( ent );
				}

				if ( ( effects & Defines.EF_POWERSCREEN ) != 0 )
				{
					ent.model = CL_tent.cl_mod_powerscreen;
					ent.oldframe = 0;
					ent.frame = 0;
					ent.flags |= ( Defines.RF_TRANSLUCENT | Defines.RF_SHELL_GREEN );
					ent.alpha = 0.3F;
					V.AddEntity( ent );
				}

				if ( ( effects & ~Defines.EF_ROTATE ) != 0 )
				{
					if ( ( effects & Defines.EF_ROCKET ) != 0 )
					{
						CL_fx.RocketTrail( cent.lerp_origin, ent.origin, cent );
						V.AddLight( ent.origin, 200, 1, 1, 0 );
					}
					else if ( ( effects & Defines.EF_BLASTER ) != 0 )
					{
						if ( ( effects & Defines.EF_TRACKER ) != 0 )
						{
							CL_newfx.BlasterTrail2( cent.lerp_origin, ent.origin );
							V.AddLight( ent.origin, 200, 0, 1, 0 );
						}
						else
						{
							CL_fx.BlasterTrail( cent.lerp_origin, ent.origin );
							V.AddLight( ent.origin, 200, 1, 1, 0 );
						}
					}
					else if ( ( effects & Defines.EF_HYPERBLASTER ) != 0 )
					{
						if ( ( effects & Defines.EF_TRACKER ) != 0 )
							V.AddLight( ent.origin, 200, 0, 1, 0 );
						else
							V.AddLight( ent.origin, 200, 1, 1, 0 );
					}
					else if ( ( effects & Defines.EF_GIB ) != 0 )
					{
						CL_fx.DiminishingTrail( cent.lerp_origin, ent.origin, cent, effects );
					}
					else if ( ( effects & Defines.EF_GRENADE ) != 0 )
					{
						CL_fx.DiminishingTrail( cent.lerp_origin, ent.origin, cent, effects );
					}
					else if ( ( effects & Defines.EF_FLIES ) != 0 )
					{
						CL_fx.FlyEffect( cent, ent.origin );
					}
					else if ( ( effects & Defines.EF_BFG ) != 0 )
					{
						if ( ( effects & Defines.EF_ANIM_ALLFAST ) != 0 )
						{
							CL_fx.BfgParticles( ent );
							i = 200;
						}
						else
						{
							i = bfg_lightramp[s1.frame];
						}

						V.AddLight( ent.origin, i, 0, 1, 0 );
					}
					else if ( ( effects & Defines.EF_TRAP ) != 0 )
					{
						ent.origin[2] += 32;
						CL_fx.TrapParticles( ent );
						i = ( Globals.rnd.Next( 100 ) ) + 100;
						V.AddLight( ent.origin, i, 1, 0.8F, 0.1F );
					}
					else if ( ( effects & Defines.EF_FLAG1 ) != 0 )
					{
						CL_fx.FlagTrail( cent.lerp_origin, ent.origin, 242 );
						V.AddLight( ent.origin, 225, 1, 0.1F, 0.1F );
					}
					else if ( ( effects & Defines.EF_FLAG2 ) != 0 )
					{
						CL_fx.FlagTrail( cent.lerp_origin, ent.origin, 115 );
						V.AddLight( ent.origin, 225, 0.1F, 0.1F, 1 );
					}
					else if ( ( effects & Defines.EF_TAGTRAIL ) != 0 )
					{
						CL_newfx.TagTrail( cent.lerp_origin, ent.origin, 220 );
						V.AddLight( ent.origin, 225, 1F, 1F, 0F );
					}
					else if ( ( effects & Defines.EF_TRACKERTRAIL ) != 0 )
					{
						if ( ( effects & Defines.EF_TRACKER ) != 0 )
						{
							Single intensity;
							intensity = ( Single ) ( 50 + ( 500 * ( Math.Sin( Globals.cl.time / 500 ) + 1 ) ) );
							if ( Globals.vidref_val == Defines.VIDREF_GL )
								V.AddLight( ent.origin, intensity, -1F, -1F, -1F );
							else
								V.AddLight( ent.origin, -1F * intensity, 1F, 1F, 1F );
						}
						else
						{
							CL_newfx.Tracker_Shell( cent.lerp_origin );
							V.AddLight( ent.origin, 155, -1F, -1F, -1F );
						}
					}
					else if ( ( effects & Defines.EF_TRACKER ) != 0 )
					{
						CL_newfx.TrackerTrail( cent.lerp_origin, ent.origin, 0 );
						if ( Globals.vidref_val == Defines.VIDREF_GL )
							V.AddLight( ent.origin, 200, -1, -1, -1 );
						else
							V.AddLight( ent.origin, -200, 1, 1, 1 );
					}
					else if ( ( effects & Defines.EF_GREENGIB ) != 0 )
					{
						CL_fx.DiminishingTrail( cent.lerp_origin, ent.origin, cent, effects );
					}
					else if ( ( effects & Defines.EF_IONRIPPER ) != 0 )
					{
						CL_fx.IonripperTrail( cent.lerp_origin, ent.origin );
						V.AddLight( ent.origin, 100, 1, 0.5F, 0.5F );
					}
					else if ( ( effects & Defines.EF_BLUEHYPERBLASTER ) != 0 )
					{
						V.AddLight( ent.origin, 200, 0, 0, 1 );
					}
					else if ( ( effects & Defines.EF_PLASMA ) != 0 )
					{
						if ( ( effects & Defines.EF_ANIM_ALLFAST ) != 0 )
						{
							CL_fx.BlasterTrail( cent.lerp_origin, ent.origin );
						}

						V.AddLight( ent.origin, 130, 1, 0.5F, 0.5F );
					}
				}

				Math3D.VectorCopy( ent.origin, cent.lerp_origin );
			}
		}

		private static readonly entity_t gun = new entity_t();
		static void AddViewWeapon( player_state_t ps, player_state_t ops )
		{
			Int32 i;
			if ( 0 == Globals.cl_gun.value )
				return;
			if ( ps.fov > 90 )
				return;
			gun.Clear();
			if ( Globals.gun_model != null )
				gun.model = Globals.gun_model;
			else
				gun.model = Globals.cl.model_draw[ps.gunindex];
			if ( gun.model == null )
				return;
			for ( i = 0; i < 3; i++ )
			{
				gun.origin[i] = Globals.cl.refdef.vieworg[i] + ops.gunoffset[i] + Globals.cl.lerpfrac * ( ps.gunoffset[i] - ops.gunoffset[i] );
				gun.angles[i] = Globals.cl.refdef.viewangles[i] + Math3D.LerpAngle( ops.gunangles[i], ps.gunangles[i], Globals.cl.lerpfrac );
			}

			if ( Globals.gun_frame != 0 )
			{
				gun.frame = Globals.gun_frame;
				gun.oldframe = Globals.gun_frame;
			}
			else
			{
				gun.frame = ps.gunframe;
				if ( gun.frame == 0 )
					gun.oldframe = 0;
				else
					gun.oldframe = ops.gunframe;
			}

			gun.flags = Defines.RF_MINLIGHT | Defines.RF_DEPTHHACK | Defines.RF_WEAPONMODEL;
			gun.backlerp = 1F - Globals.cl.lerpfrac;
			Math3D.VectorCopy( gun.origin, gun.oldorigin );
			V.AddEntity( gun );
		}

		static void CalcViewValues( )
		{
			Int32 i;
			Single lerp, backlerp;
			frame_t oldframe;
			player_state_t ps, ops;
			ps = Globals.cl.frame.playerstate;
			i = ( Globals.cl.frame.serverframe - 1 ) & Defines.UPDATE_MASK;
			oldframe = Globals.cl.frames[i];
			if ( oldframe.serverframe != Globals.cl.frame.serverframe - 1 || !oldframe.valid )
				oldframe = Globals.cl.frame;
			ops = oldframe.playerstate;
			if ( Math.Abs( ops.pmove.origin[0] - ps.pmove.origin[0] ) > 256 * 8 || Math.Abs( ops.pmove.origin[1] - ps.pmove.origin[1] ) > 256 * 8 || Math.Abs( ops.pmove.origin[2] - ps.pmove.origin[2] ) > 256 * 8 )
				ops = ps;
			lerp = Globals.cl.lerpfrac;
			if ( ( Globals.cl_predict.value != 0 ) && 0 == ( Globals.cl.frame.playerstate.pmove.pm_flags & pmove_t.PMF_NO_PREDICTION ) )
			{
				Int32 delta;
				backlerp = 1F - lerp;
				for ( i = 0; i < 3; i++ )
				{
					Globals.cl.refdef.vieworg[i] = Globals.cl.predicted_origin[i] + ops.viewoffset[i] + Globals.cl.lerpfrac * ( ps.viewoffset[i] - ops.viewoffset[i] ) - backlerp * Globals.cl.prediction_error[i];
				}

				delta = ( Int32 ) ( Globals.cls.realtime - Globals.cl.predicted_step_time );
				if ( delta < 100 )
					Globals.cl.refdef.vieworg[2] -= Globals.cl.predicted_step * ( 100 - delta ) * 0.01f;
			}
			else
			{
				for ( i = 0; i < 3; i++ )
					Globals.cl.refdef.vieworg[i] = ops.pmove.origin[i] * 0.125F + ops.viewoffset[i] + lerp * ( ps.pmove.origin[i] * 0.125F + ps.viewoffset[i] - ( ops.pmove.origin[i] * 0.125F + ops.viewoffset[i] ) );
			}

			if ( Globals.cl.frame.playerstate.pmove.pm_type < Defines.PM_DEAD )
			{
				for ( i = 0; i < 3; i++ )
					Globals.cl.refdef.viewangles[i] = Globals.cl.predicted_angles[i];
			}
			else
			{
				for ( i = 0; i < 3; i++ )
					Globals.cl.refdef.viewangles[i] = Math3D.LerpAngle( ops.viewangles[i], ps.viewangles[i], lerp );
			}

			for ( i = 0; i < 3; i++ )
				Globals.cl.refdef.viewangles[i] += Math3D.LerpAngle( ops.kick_angles[i], ps.kick_angles[i], lerp );
			Math3D.AngleVectors( Globals.cl.refdef.viewangles, Globals.cl.v_forward, Globals.cl.v_right, Globals.cl.v_up );
			Globals.cl.refdef.fov_x = ops.fov + lerp * ( ps.fov - ops.fov );
			for ( i = 0; i < 4; i++ )
				Globals.cl.refdef.blend[i] = ps.blend[i];
			AddViewWeapon( ps, ops );
		}

		public static void AddEntities( )
		{
			if ( Globals.cls.state != Defines.ca_active )
				return;
			if ( Globals.cl.time > Globals.cl.frame.servertime )
			{
				if ( Globals.cl_showclamp.value != 0 )
					Com.Printf( "high clamp " + ( Globals.cl.time - Globals.cl.frame.servertime ) + "\\n" );
				Globals.cl.time = Globals.cl.frame.servertime;
				Globals.cl.lerpfrac = 1F;
			}
			else if ( Globals.cl.time < Globals.cl.frame.servertime - 100 )
			{
				if ( Globals.cl_showclamp.value != 0 )
					Com.Printf( "low clamp " + ( Globals.cl.frame.servertime - 100 - Globals.cl.time ) + "\\n" );
				Globals.cl.time = Globals.cl.frame.servertime - 100;
				Globals.cl.lerpfrac = 0;
			}
			else
				Globals.cl.lerpfrac = 1F - ( Globals.cl.frame.servertime - Globals.cl.time ) * 0.01F;
			if ( Globals.cl_timedemo.value != 0 )
				Globals.cl.lerpfrac = 1F;
			CalcViewValues();
			AddPacketEntities( Globals.cl.frame );
			CL_tent.AddTEnts();
			CL_fx.AddParticles();
			CL_fx.AddDLights();
			CL_fx.AddLightStyles();
		}

		public static void GetEntitySoundOrigin( Int32 ent, Single[] org )
		{
			centity_t old;
			if ( ent < 0 || ent >= Defines.MAX_EDICTS )
				Com.Error( Defines.ERR_DROP, "CL_GetEntitySoundOrigin: bad ent" );
			old = Globals.cl_entities[ent];
			Math3D.VectorCopy( old.lerp_origin, org );
		}
	}
}
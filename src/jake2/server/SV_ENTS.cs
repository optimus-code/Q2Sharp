using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using System;

namespace Jake2.Server
{
	public class SV_ENTS
	{
		public static Byte[] fatpvs = new Byte[65536 / 8];
		static void SV_EmitPacketEntities( client_frame_t from, client_frame_t to, sizebuf_t msg )
		{
			entity_state_t oldent = null, newent = null;
			Int32 oldindex, newindex;
			Int32 oldnum, newnum;
			Int32 from_num_entities;
			Int32 bits;
			MSG.WriteByte( msg, Defines.svc_packetentities );
			if ( from == null )
				from_num_entities = 0;
			else
				from_num_entities = from.num_entities;
			newindex = 0;
			oldindex = 0;
			while ( newindex < to.num_entities || oldindex < from_num_entities )
			{
				if ( newindex >= to.num_entities )
					newnum = 9999;
				else
				{
					newent = SV_INIT.svs.client_entities[( to.first_entity + newindex ) % SV_INIT.svs.num_client_entities];
					newnum = newent.number;
				}

				if ( oldindex >= from_num_entities )
					oldnum = 9999;
				else
				{
					oldent = SV_INIT.svs.client_entities[( from.first_entity + oldindex ) % SV_INIT.svs.num_client_entities];
					oldnum = oldent.number;
				}

				if ( newnum == oldnum )
				{
					MSG.WriteDeltaEntity( oldent, newent, msg, false, newent.number <= SV_MAIN.maxclients.value );
					oldindex++;
					newindex++;
					continue;
				}

				if ( newnum < oldnum )
				{
					MSG.WriteDeltaEntity( SV_INIT.sv.baselines[newnum], newent, msg, true, true );
					newindex++;
					continue;
				}

				if ( newnum > oldnum )
				{
					bits = Defines.U_REMOVE;
					if ( oldnum >= 256 )
						bits |= Defines.U_NUMBER16 | Defines.U_MOREBITS1;
					MSG.WriteByte( msg, bits & 255 );
					if ( ( bits & 0x0000ff00 ) != 0 )
						MSG.WriteByte( msg, ( bits >> 8 ) & 255 );
					if ( ( bits & Defines.U_NUMBER16 ) != 0 )
						MSG.WriteShort( msg, oldnum );
					else
						MSG.WriteByte( msg, oldnum );
					oldindex++;
					continue;
				}
			}

			MSG.WriteShort( msg, 0 );
		}

		static void SV_WritePlayerstateToClient( client_frame_t from, client_frame_t to, sizebuf_t msg )
		{
			player_state_t ps, ops;
			player_state_t dummy;
			ps = to.ps;
			if ( from == null )
			{
				dummy = new player_state_t();
				ops = dummy;
			}
			else
			{
				ops = from.ps;
			}

			var pflags = 0;
			if ( ps.pmove.pm_type != ops.pmove.pm_type )
				pflags |= Defines.PS_M_TYPE;
			if ( ps.pmove.origin[0] != ops.pmove.origin[0] || ps.pmove.origin[1] != ops.pmove.origin[1] || ps.pmove.origin[2] != ops.pmove.origin[2] )
				pflags |= Defines.PS_M_ORIGIN;
			if ( ps.pmove.velocity[0] != ops.pmove.velocity[0] || ps.pmove.velocity[1] != ops.pmove.velocity[1] || ps.pmove.velocity[2] != ops.pmove.velocity[2] )
				pflags |= Defines.PS_M_VELOCITY;
			if ( ps.pmove.pm_time != ops.pmove.pm_time )
				pflags |= Defines.PS_M_TIME;
			if ( ps.pmove.pm_flags != ops.pmove.pm_flags )
				pflags |= Defines.PS_M_FLAGS;
			if ( ps.pmove.gravity != ops.pmove.gravity )
				pflags |= Defines.PS_M_GRAVITY;
			if ( ps.pmove.delta_angles[0] != ops.pmove.delta_angles[0] || ps.pmove.delta_angles[1] != ops.pmove.delta_angles[1] || ps.pmove.delta_angles[2] != ops.pmove.delta_angles[2] )
				pflags |= Defines.PS_M_DELTA_ANGLES;
			if ( ps.viewoffset[0] != ops.viewoffset[0] || ps.viewoffset[1] != ops.viewoffset[1] || ps.viewoffset[2] != ops.viewoffset[2] )
				pflags |= Defines.PS_VIEWOFFSET;
			if ( ps.viewangles[0] != ops.viewangles[0] || ps.viewangles[1] != ops.viewangles[1] || ps.viewangles[2] != ops.viewangles[2] )
				pflags |= Defines.PS_VIEWANGLES;
			if ( ps.kick_angles[0] != ops.kick_angles[0] || ps.kick_angles[1] != ops.kick_angles[1] || ps.kick_angles[2] != ops.kick_angles[2] )
				pflags |= Defines.PS_KICKANGLES;
			if ( ps.blend[0] != ops.blend[0] || ps.blend[1] != ops.blend[1] || ps.blend[2] != ops.blend[2] || ps.blend[3] != ops.blend[3] )
				pflags |= Defines.PS_BLEND;
			if ( ps.fov != ops.fov )
				pflags |= Defines.PS_FOV;
			if ( ps.rdflags != ops.rdflags )
				pflags |= Defines.PS_RDFLAGS;
			if ( ps.gunframe != ops.gunframe )
				pflags |= Defines.PS_WEAPONFRAME;
			pflags |= Defines.PS_WEAPONINDEX;
			MSG.WriteByte( msg, Defines.svc_playerinfo );
			MSG.WriteShort( msg, pflags );
			if ( ( pflags & Defines.PS_M_TYPE ) != 0 )
				MSG.WriteByte( msg, ps.pmove.pm_type );
			if ( ( pflags & Defines.PS_M_ORIGIN ) != 0 )
			{
				MSG.WriteShort( msg, ps.pmove.origin[0] );
				MSG.WriteShort( msg, ps.pmove.origin[1] );
				MSG.WriteShort( msg, ps.pmove.origin[2] );
			}

			if ( ( pflags & Defines.PS_M_VELOCITY ) != 0 )
			{
				MSG.WriteShort( msg, ps.pmove.velocity[0] );
				MSG.WriteShort( msg, ps.pmove.velocity[1] );
				MSG.WriteShort( msg, ps.pmove.velocity[2] );
			}

			if ( ( pflags & Defines.PS_M_TIME ) != 0 )
				MSG.WriteByte( msg, ps.pmove.pm_time );
			if ( ( pflags & Defines.PS_M_FLAGS ) != 0 )
				MSG.WriteByte( msg, ps.pmove.pm_flags );
			if ( ( pflags & Defines.PS_M_GRAVITY ) != 0 )
				MSG.WriteShort( msg, ps.pmove.gravity );
			if ( ( pflags & Defines.PS_M_DELTA_ANGLES ) != 0 )
			{
				MSG.WriteShort( msg, ps.pmove.delta_angles[0] );
				MSG.WriteShort( msg, ps.pmove.delta_angles[1] );
				MSG.WriteShort( msg, ps.pmove.delta_angles[2] );
			}

			if ( ( pflags & Defines.PS_VIEWOFFSET ) != 0 )
			{
				MSG.WriteChar( msg, ps.viewoffset[0] * 4 );
				MSG.WriteChar( msg, ps.viewoffset[1] * 4 );
				MSG.WriteChar( msg, ps.viewoffset[2] * 4 );
			}

			if ( ( pflags & Defines.PS_VIEWANGLES ) != 0 )
			{
				MSG.WriteAngle16( msg, ps.viewangles[0] );
				MSG.WriteAngle16( msg, ps.viewangles[1] );
				MSG.WriteAngle16( msg, ps.viewangles[2] );
			}

			if ( ( pflags & Defines.PS_KICKANGLES ) != 0 )
			{
				MSG.WriteChar( msg, ps.kick_angles[0] * 4 );
				MSG.WriteChar( msg, ps.kick_angles[1] * 4 );
				MSG.WriteChar( msg, ps.kick_angles[2] * 4 );
			}

			if ( ( pflags & Defines.PS_WEAPONINDEX ) != 0 )
			{
				MSG.WriteByte( msg, ps.gunindex );
			}

			if ( ( pflags & Defines.PS_WEAPONFRAME ) != 0 )
			{
				MSG.WriteByte( msg, ps.gunframe );
				MSG.WriteChar( msg, ps.gunoffset[0] * 4 );
				MSG.WriteChar( msg, ps.gunoffset[1] * 4 );
				MSG.WriteChar( msg, ps.gunoffset[2] * 4 );
				MSG.WriteChar( msg, ps.gunangles[0] * 4 );
				MSG.WriteChar( msg, ps.gunangles[1] * 4 );
				MSG.WriteChar( msg, ps.gunangles[2] * 4 );
			}

			if ( ( pflags & Defines.PS_BLEND ) != 0 )
			{
				MSG.WriteByte( msg, ps.blend[0] * 255 );
				MSG.WriteByte( msg, ps.blend[1] * 255 );
				MSG.WriteByte( msg, ps.blend[2] * 255 );
				MSG.WriteByte( msg, ps.blend[3] * 255 );
			}

			if ( ( pflags & Defines.PS_FOV ) != 0 )
				MSG.WriteByte( msg, ps.fov );
			if ( ( pflags & Defines.PS_RDFLAGS ) != 0 )
				MSG.WriteByte( msg, ps.rdflags );
			var statbits = 0;
			for ( var i = 0; i < Defines.MAX_STATS; i++ )
				if ( ps.stats[i] != ops.stats[i] )
					statbits |= 1 << i;
			MSG.WriteLong( msg, statbits );
			for ( var i = 0; i < Defines.MAX_STATS; i++ )
				if ( ( statbits & ( 1 << i ) ) != 0 )
					MSG.WriteShort( msg, ps.stats[i] );
		}

		public static void SV_WriteFrameToClient( client_t client, sizebuf_t msg )
		{
			client_frame_t frame, oldframe;
			Int32 lastframe;
			frame = client.frames[SV_INIT.sv.framenum & Defines.UPDATE_MASK];
			if ( client.lastframe <= 0 )
			{
				oldframe = null;
				lastframe = -1;
			}
			else if ( SV_INIT.sv.framenum - client.lastframe >= ( Defines.UPDATE_BACKUP - 3 ) )
			{
				oldframe = null;
				lastframe = -1;
			}
			else
			{
				oldframe = client.frames[client.lastframe & Defines.UPDATE_MASK];
				lastframe = client.lastframe;
			}

			MSG.WriteByte( msg, Defines.svc_frame );
			MSG.WriteLong( msg, SV_INIT.sv.framenum );
			MSG.WriteLong( msg, lastframe );
			MSG.WriteByte( msg, client.surpressCount );
			client.surpressCount = 0;
			MSG.WriteByte( msg, frame.areabytes );
			SZ.Write( msg, frame.areabits, frame.areabytes );
			SV_WritePlayerstateToClient( oldframe, frame, msg );
			SV_EmitPacketEntities( oldframe, frame, msg );
		}

		public static void SV_FatPVS( Single[] org )
		{
			Int32[] leafs = new Int32[64];
			Int32 i, j, count;
			Int32 longs;
			Byte[] src;
			Single[] mins = new Single[] { 0, 0, 0 }, maxs = new Single[] { 0, 0, 0 };
			for ( i = 0; i < 3; i++ )
			{
				mins[i] = org[i] - 8;
				maxs[i] = org[i] + 8;
			}

			count = CM.CM_BoxLeafnums( mins, maxs, leafs, 64, null );
			if ( count < 1 )
				Com.Error( Defines.ERR_FATAL, "SV_FatPVS: count < 1" );
			longs = ( CM.CM_NumClusters() + 31 ) >> 5;
			for ( i = 0; i < count; i++ )
				leafs[i] = CM.CM_LeafCluster( leafs[i] );
			System.Array.Copy( CM.CM_ClusterPVS( leafs[0] ), 0, SV_ENTS.fatpvs, 0, longs << 2 );
			for ( i = 1; i < count; i++ )
			{
				for ( j = 0; j < i; j++ )
					if ( leafs[i] == leafs[j] )
						break;
				if ( j != i )
					continue;
				src = CM.CM_ClusterPVS( leafs[i] );
				var k = 0;
				for ( j = 0; j < longs; j++ )
				{
					SV_ENTS.fatpvs[k] |= src[k++];
					SV_ENTS.fatpvs[k] |= src[k++];
					SV_ENTS.fatpvs[k] |= src[k++];
					SV_ENTS.fatpvs[k] |= src[k++];
				}
			}
		}

		public static void SV_BuildClientFrame( client_t client )
		{
			Int32 e, i;
			Single[] org = new Single[] { 0, 0, 0 };
			edict_t ent;
			edict_t clent;
			client_frame_t frame;
			entity_state_t state;
			Int32 l;
			Int32 clientarea, clientcluster;
			Int32 leafnum;
			Int32 c_fullsend;
			Byte[] clientphs;
			Byte[] bitvector;
			clent = client.edict;
			if ( clent.client == null )
				return;
			frame = client.frames[SV_INIT.sv.framenum & Defines.UPDATE_MASK];
			frame.senttime = SV_INIT.svs.realtime;
			for ( i = 0; i < 3; i++ )
				org[i] = clent.client.ps.pmove.origin[i] * 0.125F + clent.client.ps.viewoffset[i];
			leafnum = CM.CM_PointLeafnum( org );
			clientarea = CM.CM_LeafArea( leafnum );
			clientcluster = CM.CM_LeafCluster( leafnum );
			frame.areabytes = CM.CM_WriteAreaBits( frame.areabits, clientarea );
			frame.ps.Set( clent.client.ps );
			SV_FatPVS( org );
			clientphs = CM.CM_ClusterPHS( clientcluster );
			frame.num_entities = 0;
			frame.first_entity = SV_INIT.svs.next_client_entities;
			c_fullsend = 0;
			for ( e = 1; e < GameBase.num_edicts; e++ )
			{
				ent = GameBase.g_edicts[e];
				if ( ( ent.svflags & Defines.SVF_NOCLIENT ) != 0 )
					continue;
				if ( 0 == ent.s.modelindex && 0 == ent.s.effects && 0 == ent.s.sound && 0 == ent.s.event_renamed )
					continue;
				if ( ent != clent )
				{
					if ( !CM.CM_AreasConnected( clientarea, ent.areanum ) )
					{
						if ( 0 == ent.areanum2 || !CM.CM_AreasConnected( clientarea, ent.areanum2 ) )
							continue;
					}

					if ( ( ent.s.renderfx & Defines.RF_BEAM ) != 0 )
					{
						l = ent.clusternums[0];
						if ( 0 == ( clientphs[l >> 3] & ( 1 << ( l & 7 ) ) ) )
							continue;
					}
					else
					{
						if ( ent.s.sound == 0 )
						{
							bitvector = SV_ENTS.fatpvs;
						}
						else
							bitvector = SV_ENTS.fatpvs;
						if ( ent.num_clusters == -1 )
						{
							if ( !CM.CM_HeadnodeVisible( ent.headnode, bitvector ) )
								continue;
							c_fullsend++;
						}
						else
						{
							for ( i = 0; i < ent.num_clusters; i++ )
							{
								l = ent.clusternums[i];
								if ( ( bitvector[l >> 3] & ( 1 << ( l & 7 ) ) ) != 0 )
									break;
							}

							if ( i == ent.num_clusters )
								continue;
						}

						if ( ent.s.modelindex == 0 )
						{
							Single[] delta = new Single[] { 0, 0, 0 };
							Single len;
							Math3D.VectorSubtract( org, ent.s.origin, delta );
							len = Math3D.VectorLength( delta );
							if ( len > 400 )
								continue;
						}
					}
				}

				var ix = SV_INIT.svs.next_client_entities % SV_INIT.svs.num_client_entities;
				state = SV_INIT.svs.client_entities[ix];
				if ( ent.s.number != e )
				{
					Com.DPrintf( "FIXING ENT.S.NUMBER!!!\\n" );
					ent.s.number = e;
				}

				SV_INIT.svs.client_entities[ix].Set( ent.s );
				if ( ent.owner == client.edict )
					state.solid = 0;
				SV_INIT.svs.next_client_entities++;
				frame.num_entities++;
			}
		}

		private static readonly Byte[] buf_data = new Byte[32768];
		public static void SV_RecordDemoMessage( )
		{
			if ( SV_INIT.svs.demofile == null )
				return;
			entity_state_t nostate = new entity_state_t( null );
			sizebuf_t buf = new sizebuf_t();
			SZ.Init( buf, buf_data, buf_data.Length );
			MSG.WriteByte( buf, Defines.svc_frame );
			MSG.WriteLong( buf, SV_INIT.sv.framenum );
			MSG.WriteByte( buf, Defines.svc_packetentities );
			var e = 1;
			edict_t ent = GameBase.g_edicts[e];
			while ( e < GameBase.num_edicts )
			{
				if ( ent.inuse && ent.s.number != 0 && ( ent.s.modelindex != 0 || ent.s.effects != 0 || ent.s.sound != 0 || ent.s.event_renamed != 0 ) && 0 == ( ent.svflags & Defines.SVF_NOCLIENT ) )
					MSG.WriteDeltaEntity( nostate, ent.s, buf, false, true );
				e++;
				ent = GameBase.g_edicts[e];
			}

			MSG.WriteShort( buf, 0 );
			SZ.Write( buf, SV_INIT.svs.demo_multicast.data, SV_INIT.svs.demo_multicast.cursize );
			SZ.Clear( SV_INIT.svs.demo_multicast );
			var len = EndianHandler.SwapInt( buf.cursize );
			try
			{
				SV_INIT.svs.demofile.Write( len );
				SV_INIT.svs.demofile.Write( buf.data, 0, buf.cursize );
			}
			catch ( Exception e1 )
			{
				Com.Printf( "Error writing demo file:" + e );
			}
		}
	}
}
using J2N.Text;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using System;
using static Q2Sharp.Qcommon.Com;

namespace Q2Sharp.Server
{
	public class SV_MAIN
	{
		public static netadr_t[] master_adr = new netadr_t[Defines.MAX_MASTERS];
		static SV_MAIN( )
		{
			for ( var i = 0; i < Defines.MAX_MASTERS; i++ )
			{
				master_adr[i] = new netadr_t();
			}
		}

		public static client_t sv_client;
		public static cvar_t sv_paused;
		public static cvar_t sv_timedemo;
		public static cvar_t sv_enforcetime;
		public static cvar_t timeout;
		public static cvar_t zombietime;
		public static cvar_t rcon_password;
		public static cvar_t allow_download;
		public static cvar_t allow_download_players;
		public static cvar_t allow_download_models;
		public static cvar_t allow_download_sounds;
		public static cvar_t allow_download_maps;
		public static cvar_t sv_airaccelerate;
		public static cvar_t sv_noreload;
		public static cvar_t maxclients;
		public static cvar_t sv_showclamp;
		public static cvar_t hostname;
		public static cvar_t public_server;
		public static cvar_t sv_reconnect_limit;
		public static readonly Int32 HEARTBEAT_SECONDS = 300;
		public static void SV_DropClient( client_t drop )
		{
			MSG.WriteByte( drop.netchan.message, Defines.svc_disconnect );
			if ( drop.state == Defines.cs_spawned )
			{
				PlayerClient.ClientDisconnect( drop.edict );
			}

			if ( drop.download != null )
			{
				FS.FreeFile( drop.download );
				drop.download = null;
			}

			drop.state = Defines.cs_zombie;
			drop.name = "";
		}

		public static String SV_StatusString( )
		{
			String player;
			var status = "";
			Int32 i;
			client_t cl;
			Int32 statusLength;
			Int32 playerLength;
			status = Cvar.Serverinfo() + "\\n";
			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( cl.state == Defines.cs_connected || cl.state == Defines.cs_spawned )
				{
					player = "" + cl.edict.client.ps.stats[Defines.STAT_FRAGS] + " " + cl.ping + "\\\"" + cl.name + "\\\"\\n";
					playerLength = player.Length;
					statusLength = status.Length;
					if ( statusLength + playerLength >= 1024 )
						break;
					status += player;
				}
			}

			return status;
		}

		public static void SVC_Status( )
		{
			Netchan.OutOfBandPrint( Defines.NS_SERVER, Globals.net_from, "print\\n" + SV_StatusString() );
		}

		public static void SVC_Ack( )
		{
			Com.Printf( "Ping acknowledge from " + NET.AdrToString( Globals.net_from ) + "\\n" );
		}

		public static void SVC_Info( )
		{
			String string_renamed;
			Int32 i, count;
			Int32 version;
			if ( SV_MAIN.maxclients.value == 1 )
				return;
			version = Lib.Atoi( Cmd.Argv( 1 ) );
			if ( version != Defines.PROTOCOL_VERSION )
				string_renamed = SV_MAIN.hostname.string_renamed + ": wrong version\\n";
			else
			{
				count = 0;
				for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
					if ( SV_INIT.svs.clients[i].state >= Defines.cs_connected )
						count++;
				string_renamed = SV_MAIN.hostname.string_renamed + " " + SV_INIT.sv.name + " " + count + "/" + ( Int32 ) SV_MAIN.maxclients.value + "\\n";
			}

			Netchan.OutOfBandPrint( Defines.NS_SERVER, Globals.net_from, "info\\n" + string_renamed );
		}

		public static void SVC_Ping( )
		{
			Netchan.OutOfBandPrint( Defines.NS_SERVER, Globals.net_from, "ack" );
		}

		public static void SVC_GetChallenge( )
		{
			Int32 i;
			Int32 oldest;
			Int32 oldestTime;
			oldest = 0;
			oldestTime = 0x7fffffff;
			for ( i = 0; i < Defines.MAX_CHALLENGES; i++ )
			{
				if ( NET.CompareBaseAdr( Globals.net_from, SV_INIT.svs.challenges[i].adr ) )
					break;
				if ( SV_INIT.svs.challenges[i].time < oldestTime )
				{
					oldestTime = SV_INIT.svs.challenges[i].time;
					oldest = i;
				}
			}

			if ( i == Defines.MAX_CHALLENGES )
			{
				SV_INIT.svs.challenges[oldest].challenge = Lib.Rand() & 0x7fff;
				SV_INIT.svs.challenges[oldest].adr = Globals.net_from;
				SV_INIT.svs.challenges[oldest].time = ( Int32 ) Globals.curtime;
				i = oldest;
			}

			Netchan.OutOfBandPrint( Defines.NS_SERVER, Globals.net_from, "challenge " + SV_INIT.svs.challenges[i].challenge );
		}

		public static void SVC_DirectConnect( )
		{
			String userinfo;
			netadr_t adr;
			Int32 i;
			client_t cl;
			Int32 version;
			Int32 qport;
			adr = Globals.net_from;
			Com.DPrintf( "SVC_DirectConnect ()\\n" );
			version = Lib.Atoi( Cmd.Argv( 1 ) );
			if ( version != Defines.PROTOCOL_VERSION )
			{
				Netchan.OutOfBandPrint( Defines.NS_SERVER, adr, "print\\nServer is version " + Globals.VERSION + "\\n" );
				Com.DPrintf( "    rejected connect from version " + version + "\\n" );
				return;
			}

			qport = Lib.Atoi( Cmd.Argv( 2 ) );
			var challenge = Lib.Atoi( Cmd.Argv( 3 ) );
			userinfo = Cmd.Argv( 4 );
			userinfo = Info.Info_SetValueForKey( userinfo, "ip", NET.AdrToString( Globals.net_from ) );
			if ( SV_INIT.sv.attractloop )
			{
				if ( !NET.IsLocalAddress( adr ) )
				{
					Com.Printf( "Remote connect in attract loop.  Ignored.\\n" );
					Netchan.OutOfBandPrint( Defines.NS_SERVER, adr, "print\\nConnection refused.\\n" );
					return;
				}
			}

			if ( !NET.IsLocalAddress( adr ) )
			{
				for ( i = 0; i < Defines.MAX_CHALLENGES; i++ )
				{
					if ( NET.CompareBaseAdr( Globals.net_from, SV_INIT.svs.challenges[i].adr ) )
					{
						if ( challenge == SV_INIT.svs.challenges[i].challenge )
							break;
						Netchan.OutOfBandPrint( Defines.NS_SERVER, adr, "print\\nBad challenge.\\n" );
						return;
					}
				}

				if ( i == Defines.MAX_CHALLENGES )
				{
					Netchan.OutOfBandPrint( Defines.NS_SERVER, adr, "print\\nNo challenge for address.\\n" );
					return;
				}
			}

			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( cl.state == Defines.cs_free )
					continue;
				if ( NET.CompareBaseAdr( adr, cl.netchan.remote_address ) && ( cl.netchan.qport == qport || adr.port == cl.netchan.remote_address.port ) )
				{
					if ( !NET.IsLocalAddress( adr ) && ( SV_INIT.svs.realtime - cl.lastconnect ) < ( ( Int32 ) SV_MAIN.sv_reconnect_limit.value * 1000 ) )
					{
						Com.DPrintf( NET.AdrToString( adr ) + ":reconnect rejected : too soon\\n" );
						return;
					}

					Com.Printf( NET.AdrToString( adr ) + ":reconnect\\n" );
					Gotnewcl( i, challenge, userinfo, adr, qport );
					return;
				}
			}

			var index = -1;
			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( cl.state == Defines.cs_free )
				{
					index = i;
					break;
				}
			}

			if ( index == -1 )
			{
				Netchan.OutOfBandPrint( Defines.NS_SERVER, adr, "print\\nServer is full.\\n" );
				Com.DPrintf( "Rejected a connection.\\n" );
				return;
			}

			Gotnewcl( index, challenge, userinfo, adr, qport );
		}

		public static void Gotnewcl( Int32 i, Int32 challenge, String userinfo, netadr_t adr, Int32 qport )
		{
			SV_MAIN.sv_client = SV_INIT.svs.clients[i];
			var edictnum = i + 1;
			edict_t ent = GameBase.g_edicts[edictnum];
			SV_INIT.svs.clients[i].edict = ent;
			SV_INIT.svs.clients[i].challenge = challenge;
			if ( !( PlayerClient.ClientConnect( ent, userinfo ) ) )
			{
				if ( Info.Info_ValueForKey( userinfo, "rejmsg" ) != null )
					Netchan.OutOfBandPrint( Defines.NS_SERVER, adr, "print\\n" + Info.Info_ValueForKey( userinfo, "rejmsg" ) + "\\nConnection refused.\\n" );
				else
					Netchan.OutOfBandPrint( Defines.NS_SERVER, adr, "print\\nConnection refused.\\n" );
				Com.DPrintf( "Game rejected a connection.\\n" );
				return;
			}

			SV_INIT.svs.clients[i].userinfo = userinfo;
			SV_UserinfoChanged( SV_INIT.svs.clients[i] );
			Netchan.OutOfBandPrint( Defines.NS_SERVER, adr, "client_connect" );
			Netchan.Setup( Defines.NS_SERVER, SV_INIT.svs.clients[i].netchan, adr, qport );
			SV_INIT.svs.clients[i].state = Defines.cs_connected;
			SZ.Init( SV_INIT.svs.clients[i].datagram, SV_INIT.svs.clients[i].datagram_buf, SV_INIT.svs.clients[i].datagram_buf.Length );
			SV_INIT.svs.clients[i].datagram.allowoverflow = true;
			SV_INIT.svs.clients[i].lastmessage = SV_INIT.svs.realtime;
			SV_INIT.svs.clients[i].lastconnect = SV_INIT.svs.realtime;
			Com.DPrintf( "new client added.\\n" );
		}

		public static Int32 Rcon_Validate( )
		{
			if ( 0 == SV_MAIN.rcon_password.string_renamed.Length )
				return 0;
			if ( 0 != Lib.Strcmp( Cmd.Argv( 1 ), SV_MAIN.rcon_password.string_renamed ) )
				return 0;
			return 1;
		}

		public static void SVC_RemoteCommand( )
		{
			Int32 i;
			String remaining;
			i = Rcon_Validate();
			var msg = Lib.CtoJava( Globals.net_message.data, 4, 1024 );
			if ( i == 0 )
				Com.Printf( "Bad rcon from " + NET.AdrToString( Globals.net_from ) + ":\\n" + msg + "\\n" );
			else
				Com.Printf( "Rcon from " + NET.AdrToString( Globals.net_from ) + ":\\n" + msg + "\\n" );
			Com.BeginRedirect( Defines.RD_PACKET, SV_SEND.sv_outputbuf, Defines.SV_OUTPUTBUF_LENGTH, new AnonymousRD_Flusher() );
			if ( 0 == Rcon_Validate() )
			{
				Com.Printf( "Bad rcon_password.\\n" );
			}
			else
			{
				remaining = "";
				for ( i = 2; i < Cmd.Argc(); i++ )
				{
					remaining += Cmd.Argv( i );
					remaining += " ";
				}

				Cmd.ExecuteString( remaining );
			}

			Com.EndRedirect();
		}

		private sealed class AnonymousRD_Flusher : RD_Flusher
		{
			public override void Rd_flush( Int32 target, StringBuffer buffer )
			{
				SV_SEND.SV_FlushRedirect( target, Lib.StringToBytes( buffer.ToString() ) );
			}
		}

		public static void SV_ConnectionlessPacket( )
		{
			String s;
			String c;
			MSG.BeginReading( Globals.net_message );
			MSG.ReadLong( Globals.net_message );
			s = MSG.ReadStringLine( Globals.net_message );
			Cmd.TokenizeString( s.ToCharArray(), false );
			c = Cmd.Argv( 0 );
			if ( 0 == Lib.Strcmp( c, "ping" ) )
				SVC_Ping();
			else if ( 0 == Lib.Strcmp( c, "ack" ) )
				SVC_Ack();
			else if ( 0 == Lib.Strcmp( c, "status" ) )
				SVC_Status();
			else if ( 0 == Lib.Strcmp( c, "info" ) )
				SVC_Info();
			else if ( 0 == Lib.Strcmp( c, "getchallenge" ) )
				SVC_GetChallenge();
			else if ( 0 == Lib.Strcmp( c, "connect" ) )
				SVC_DirectConnect();
			else if ( 0 == Lib.Strcmp( c, "rcon" ) )
				SVC_RemoteCommand();
			else
			{
				Com.Printf( "bad connectionless packet from " + NET.AdrToString( Globals.net_from ) + "\\n" );
				Com.Printf( "[" + s + "]\\n" );
				Com.Printf( "" + Lib.HexDump( Globals.net_message.data, 128, false ) );
			}
		}

		public static void SV_CalcPings( )
		{
			Int32 i, j;
			client_t cl;
			Int32 total, count;
			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( cl.state != Defines.cs_spawned )
					continue;
				total = 0;
				count = 0;
				for ( j = 0; j < Defines.LATENCY_COUNTS; j++ )
				{
					if ( cl.frame_latency[j] > 0 )
					{
						count++;
						total += cl.frame_latency[j];
					}
				}

				if ( 0 == count )
					cl.ping = 0;
				else
					cl.ping = total / count;
				cl.edict.client.ping = cl.ping;
			}
		}

		public static void SV_GiveMsec( )
		{
			Int32 i;
			client_t cl;
			if ( ( SV_INIT.sv.framenum & 15 ) != 0 )
				return;
			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( cl.state == Defines.cs_free )
					continue;
				cl.commandMsec = 1800;
			}
		}

		public static void SV_ReadPackets( )
		{
			Int32 i;
			client_t cl;
			var qport = 0;
			while ( NET.GetPacket( Defines.NS_SERVER, Globals.net_from, Globals.net_message ) )
			{
				if ( ( Globals.net_message.data[0] == -1 ) && ( Globals.net_message.data[1] == -1 ) && ( Globals.net_message.data[2] == -1 ) && ( Globals.net_message.data[3] == -1 ) )
				{
					SV_ConnectionlessPacket();
					continue;
				}

				MSG.BeginReading( Globals.net_message );
				MSG.ReadLong( Globals.net_message );
				MSG.ReadLong( Globals.net_message );
				qport = MSG.ReadShort( Globals.net_message ) & 0xffff;
				for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
				{
					cl = SV_INIT.svs.clients[i];
					if ( cl.state == Defines.cs_free )
						continue;
					if ( !NET.CompareBaseAdr( Globals.net_from, cl.netchan.remote_address ) )
						continue;
					if ( cl.netchan.qport != qport )
						continue;
					if ( cl.netchan.remote_address.port != Globals.net_from.port )
					{
						Com.Printf( "SV_ReadPackets: fixing up a translated port\\n" );
						cl.netchan.remote_address.port = Globals.net_from.port;
					}

					if ( Netchan.Process( cl.netchan, Globals.net_message ) )
					{
						if ( cl.state != Defines.cs_zombie )
						{
							cl.lastmessage = SV_INIT.svs.realtime;
							SV_USER.SV_ExecuteClientMessage( cl );
						}
					}

					break;
				}

				if ( i != SV_MAIN.maxclients.value )
					continue;
			}
		}

		public static void SV_CheckTimeouts( )
		{
			Int32 i;
			client_t cl;
			Int32 droppoint;
			Int32 zombiepoint;
			droppoint = ( Int32 ) ( SV_INIT.svs.realtime - 1000 * SV_MAIN.timeout.value );
			zombiepoint = ( Int32 ) ( SV_INIT.svs.realtime - 1000 * SV_MAIN.zombietime.value );
			for ( i = 0; i < SV_MAIN.maxclients.value; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( cl.lastmessage > SV_INIT.svs.realtime )
					cl.lastmessage = SV_INIT.svs.realtime;
				if ( cl.state == Defines.cs_zombie && cl.lastmessage < zombiepoint )
				{
					cl.state = Defines.cs_free;
					continue;
				}

				if ( ( cl.state == Defines.cs_connected || cl.state == Defines.cs_spawned ) && cl.lastmessage < droppoint )
				{
					SV_SEND.SV_BroadcastPrintf( Defines.PRINT_HIGH, cl.name + " timed out\\n" );
					SV_DropClient( cl );
					cl.state = Defines.cs_free;
				}
			}
		}

		public static void SV_PrepWorldFrame( )
		{
			edict_t ent;
			Int32 i;
			for ( i = 0; i < GameBase.num_edicts; i++ )
			{
				ent = GameBase.g_edicts[i];
				ent.s.event_renamed = 0;
			}
		}

		public static void SV_RunGameFrame( )
		{
			if ( Globals.host_speeds.value != 0 )
				Globals.time_before_game = Timer.Milliseconds();
			SV_INIT.sv.framenum++;
			SV_INIT.sv.time = SV_INIT.sv.framenum * 100;
			if ( 0 == SV_MAIN.sv_paused.value || SV_MAIN.maxclients.value > 1 )
			{
				GameBase.G_RunFrame();
				if ( SV_INIT.sv.time < SV_INIT.svs.realtime )
				{
					if ( SV_MAIN.sv_showclamp.value != 0 )
						Com.Printf( "sv highclamp\\n" );
					SV_INIT.svs.realtime = SV_INIT.sv.time;
				}
			}

			if ( Globals.host_speeds.value != 0 )
				Globals.time_after_game = Timer.Milliseconds();
		}

		public static void SV_Frame( Int64 msec )
		{
			Globals.time_before_game = Globals.time_after_game = 0;
			if ( !SV_INIT.svs.initialized )
				return;
			SV_INIT.svs.realtime += ( Int32 ) msec;
			Lib.Rand();
			SV_CheckTimeouts();
			SV_ReadPackets();
			if ( 0 == SV_MAIN.sv_timedemo.value && SV_INIT.svs.realtime < SV_INIT.sv.time )
			{
				if ( SV_INIT.sv.time - SV_INIT.svs.realtime > 100 )
				{
					if ( SV_MAIN.sv_showclamp.value != 0 )
						Com.Printf( "sv lowclamp\\n" );
					SV_INIT.svs.realtime = SV_INIT.sv.time - 100;
				}

				NET.Sleep( SV_INIT.sv.time - SV_INIT.svs.realtime );
				return;
			}

			SV_CalcPings();
			SV_GiveMsec();
			SV_RunGameFrame();
			SV_SEND.SV_SendClientMessages();
			SV_ENTS.SV_RecordDemoMessage();
			Master_Heartbeat();
			SV_PrepWorldFrame();
		}

		public static void Master_Heartbeat( )
		{
			String string_renamed;
			Int32 i;
			if ( Globals.dedicated == null || 0 == Globals.dedicated.value )
				return;
			if ( null == SV_MAIN.public_server || 0 == SV_MAIN.public_server.value )
				return;
			if ( SV_INIT.svs.last_heartbeat > SV_INIT.svs.realtime )
				SV_INIT.svs.last_heartbeat = SV_INIT.svs.realtime;
			if ( SV_INIT.svs.realtime - SV_INIT.svs.last_heartbeat < SV_MAIN.HEARTBEAT_SECONDS * 1000 )
				return;
			SV_INIT.svs.last_heartbeat = SV_INIT.svs.realtime;
			string_renamed = SV_StatusString();
			for ( i = 0; i < Defines.MAX_MASTERS; i++ )
				if ( SV_MAIN.master_adr[i].port != 0 )
				{
					Com.Printf( "Sending heartbeat to " + NET.AdrToString( SV_MAIN.master_adr[i] ) + "\\n" );
					Netchan.OutOfBandPrint( Defines.NS_SERVER, SV_MAIN.master_adr[i], "heartbeat\\n" + string_renamed );
				}
		}

		public static void Master_Shutdown( )
		{
			Int32 i;
			if ( null == Globals.dedicated || 0 == Globals.dedicated.value )
				return;
			if ( null == SV_MAIN.public_server || 0 == SV_MAIN.public_server.value )
				return;
			for ( i = 0; i < Defines.MAX_MASTERS; i++ )
				if ( SV_MAIN.master_adr[i].port != 0 )
				{
					if ( i > 0 )
						Com.Printf( "Sending heartbeat to " + NET.AdrToString( SV_MAIN.master_adr[i] ) + "\\n" );
					Netchan.OutOfBandPrint( Defines.NS_SERVER, SV_MAIN.master_adr[i], "shutdown" );
				}
		}

		public static void SV_UserinfoChanged( client_t cl )
		{
			String val;
			Int32 i;
			PlayerClient.ClientUserinfoChanged( cl.edict, cl.userinfo );
			cl.name = Info.Info_ValueForKey( cl.userinfo, "name" );
			val = Info.Info_ValueForKey( cl.userinfo, "rate" );
			if ( val.Length > 0 )
			{
				i = Lib.Atoi( val );
				cl.rate = i;
				if ( cl.rate < 100 )
					cl.rate = 100;
				if ( cl.rate > 15000 )
					cl.rate = 15000;
			}
			else
				cl.rate = 5000;
			val = Info.Info_ValueForKey( cl.userinfo, "msg" );
			if ( val.Length > 0 )
			{
				cl.messagelevel = Lib.Atoi( val );
			}
		}

		public static void SV_Init( )
		{
			SV_CCMDS.SV_InitOperatorCommands();
			SV_MAIN.rcon_password = Cvar.Get( "rcon_password", "", 0 );
			Cvar.Get( "skill", "1", 0 );
			Cvar.Get( "deathmatch", "0", Defines.CVAR_LATCH );
			Cvar.Get( "coop", "0", Defines.CVAR_LATCH );
			Cvar.Get( "dmflags", "" + Defines.DF_INSTANT_ITEMS, Defines.CVAR_SERVERINFO );
			Cvar.Get( "fraglimit", "0", Defines.CVAR_SERVERINFO );
			Cvar.Get( "timelimit", "0", Defines.CVAR_SERVERINFO );
			Cvar.Get( "cheats", "0", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			Cvar.Get( "protocol", "" + Defines.PROTOCOL_VERSION, Defines.CVAR_SERVERINFO | Defines.CVAR_NOSET );
			SV_MAIN.maxclients = Cvar.Get( "maxclients", "1", Defines.CVAR_SERVERINFO | Defines.CVAR_LATCH );
			SV_MAIN.hostname = Cvar.Get( "hostname", "noname", Defines.CVAR_SERVERINFO | Defines.CVAR_ARCHIVE );
			SV_MAIN.timeout = Cvar.Get( "timeout", "125", 0 );
			SV_MAIN.zombietime = Cvar.Get( "zombietime", "2", 0 );
			SV_MAIN.sv_showclamp = Cvar.Get( "showclamp", "0", 0 );
			SV_MAIN.sv_paused = Cvar.Get( "paused", "0", 0 );
			SV_MAIN.sv_timedemo = Cvar.Get( "timedemo", "0", 0 );
			SV_MAIN.sv_enforcetime = Cvar.Get( "sv_enforcetime", "0", 0 );
			SV_MAIN.allow_download = Cvar.Get( "allow_download", "1", Defines.CVAR_ARCHIVE );
			SV_MAIN.allow_download_players = Cvar.Get( "allow_download_players", "0", Defines.CVAR_ARCHIVE );
			SV_MAIN.allow_download_models = Cvar.Get( "allow_download_models", "1", Defines.CVAR_ARCHIVE );
			SV_MAIN.allow_download_sounds = Cvar.Get( "allow_download_sounds", "1", Defines.CVAR_ARCHIVE );
			SV_MAIN.allow_download_maps = Cvar.Get( "allow_download_maps", "1", Defines.CVAR_ARCHIVE );
			SV_MAIN.sv_noreload = Cvar.Get( "sv_noreload", "0", 0 );
			SV_MAIN.sv_airaccelerate = Cvar.Get( "sv_airaccelerate", "0", Defines.CVAR_LATCH );
			SV_MAIN.public_server = Cvar.Get( "public", "0", 0 );
			SV_MAIN.sv_reconnect_limit = Cvar.Get( "sv_reconnect_limit", "3", Defines.CVAR_ARCHIVE );
			SZ.Init( Globals.net_message, Globals.net_message_buffer, Globals.net_message_buffer.Length );
		}

		public static void SV_FinalMessage( String message, Boolean reconnect )
		{
			Int32 i;
			client_t cl;
			SZ.Clear( Globals.net_message );
			MSG.WriteByte( Globals.net_message, Defines.svc_print );
			MSG.WriteByte( Globals.net_message, Defines.PRINT_HIGH );
			MSG.WriteString( Globals.net_message, message );
			if ( reconnect )
				MSG.WriteByte( Globals.net_message, Defines.svc_reconnect );
			else
				MSG.WriteByte( Globals.net_message, Defines.svc_disconnect );
			for ( i = 0; i < SV_INIT.svs.clients.Length; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( cl.state >= Defines.cs_connected )
					Netchan.Transmit( cl.netchan, Globals.net_message.cursize, Globals.net_message.data );
			}

			for ( i = 0; i < SV_INIT.svs.clients.Length; i++ )
			{
				cl = SV_INIT.svs.clients[i];
				if ( cl.state >= Defines.cs_connected )
					Netchan.Transmit( cl.netchan, Globals.net_message.cursize, Globals.net_message.data );
			}
		}

		public static void SV_Shutdown( String finalmsg, Boolean reconnect )
		{
			if ( SV_INIT.svs.clients != null )
				SV_FinalMessage( finalmsg, reconnect );
			Master_Shutdown();
			SV_GAME.SV_ShutdownGameProgs();
			if ( SV_INIT.sv.demofile != null )
				try
				{
					SV_INIT.sv.demofile.Close();
				}
				catch ( Exception e )
				{
					e.PrintStackTrace();
				}

			SV_INIT.sv = new server_t();
			Globals.server_state = SV_INIT.sv.state;
			if ( SV_INIT.svs.demofile != null )
				try
				{
					SV_INIT.svs.demofile.Close();
				}
				catch ( Exception e1 )
				{
					e1.PrintStackTrace();
				}

			SV_INIT.svs = new server_static_t();
		}
	}
}
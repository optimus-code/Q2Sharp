using J2N.IO;
using J2N.Text;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Server;
using Jake2.Sound;
using Jake2.Sys;
using Jake2.Util;
using System;
using System.IO;
using System.Threading;
using static Jake2.Qcommon.qfiles;
using Timer = Jake2.Sys.Timer;

namespace Jake2.Client
{
	public sealed class CL
	{
		static Int32 precache_check;
		static Int32 precache_spawncount;
		static Int32 precache_tex;
		static Int32 precache_model_skin;
		static Byte[] precache_model;
		public static readonly Int32 PLAYER_MULT = 5;
		public class cheatvar_t
		{
			public String name;
			public String value;
			public cvar_t var;
		}

		public static String[][] cheatvarsinfo = new String[][] { new[] { "timescale", "1" }, new[] { "timedemo", "0" }, new[] { "r_drawworld", "1" }, new[] { "cl_testlights", "0" }, new[] { "r_fullbright", "0" }, new[] { "r_drawflat", "0" }, new[] { "paused", "0" }, new[] { "fixedtime", "0" }, new[] { "sw_draworder", "0" }, new[] { "gl_lightmap", "0" }, new[] { "gl_saturatelighting", "0" }, new String[] { null, null } };
		public static cheatvar_t[] cheatvars;
		static CL( )
		{
			cheatvars = new cheatvar_t[cheatvarsinfo.Length];
			for ( var n = 0; n < cheatvarsinfo.Length; n++ )
			{
				cheatvars[n] = new cheatvar_t();
				cheatvars[n].name = cheatvarsinfo[n][0];
				cheatvars[n].value = cheatvarsinfo[n][1];
			}
		}

		static Int32 numcheatvars;
		static xcommand_t Stop_f = new Anonymousxcommand_t();
		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				try
				{
					Int32 len;
					if ( !Globals.cls.demorecording )
					{
						Com.Printf( "Not recording a demo.\\n" );
						return;
					}

					len = -1;
					Globals.cls.demofile.Write( EndianHandler.SwapInt( len ) );
					Globals.cls.demofile.Close();
					Globals.cls.demofile = null;
					Globals.cls.demorecording = false;
					Com.Printf( "Stopped demo.\\n" );
				}
				catch ( IOException e )
				{
				}
			}
		}

		static entity_state_t nullstate = new entity_state_t( null );
		static xcommand_t Record_f = new Anonymousxcommand_t1();
		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				try
				{
					String name;
					Byte[] buf_data = new Byte[Defines.MAX_MSGLEN];
					sizebuf_t buf = new sizebuf_t();
					Int32 i;
					entity_state_t ent;
					if ( Cmd.Argc() != 2 )
					{
						Com.Printf( "record <demoname>\\n" );
						return;
					}

					if ( Globals.cls.demorecording )
					{
						Com.Printf( "Already recording.\\n" );
						return;
					}

					if ( Globals.cls.state != Defines.ca_active )
					{
						Com.Printf( "You must be in a level to record.\\n" );
						return;
					}

					name = FS.Gamedir() + "/demos/" + Cmd.Argv( 1 ) + ".dm2";
					Com.Printf( "recording to " + name + ".\\n" );
					FS.CreatePath( name );
					Globals.cls.demofile = new QuakeFile( name, FileAccess.ReadWrite );
					if ( Globals.cls.demofile == null )
					{
						Com.Printf( "ERROR: couldn't open.\\n" );
						return;
					}

					Globals.cls.demorecording = true;
					Globals.cls.demowaiting = true;
					SZ.Init( buf, buf_data, Defines.MAX_MSGLEN );
					MSG.WriteByte( buf, Defines.svc_serverdata );
					MSG.WriteInt( buf, Defines.PROTOCOL_VERSION );
					MSG.WriteInt( buf, 0x10000 + Globals.cl.servercount );
					MSG.WriteByte( buf, 1 );
					MSG.WriteString( buf, Globals.cl.gamedir );
					MSG.WriteShort( buf, Globals.cl.playernum );
					MSG.WriteString( buf, Globals.cl.configstrings[Defines.CS_NAME] );
					for ( i = 0; i < Defines.MAX_CONFIGSTRINGS; i++ )
					{
						if ( Globals.cl.configstrings[i].Length > 0 )
						{
							if ( buf.cursize + Globals.cl.configstrings[i].Length + 32 > buf.maxsize )
							{
								Globals.cls.demofile.Write( EndianHandler.SwapInt( buf.cursize ) );
								Globals.cls.demofile.Write( buf.data, 0, buf.cursize );
								buf.cursize = 0;
							}

							MSG.WriteByte( buf, Defines.svc_configstring );
							MSG.WriteShort( buf, i );
							MSG.WriteString( buf, Globals.cl.configstrings[i] );
						}
					}

					nullstate.Clear();
					for ( i = 0; i < Defines.MAX_EDICTS; i++ )
					{
						ent = Globals.cl_entities[i].baseline;
						if ( ent.modelindex == 0 )
							continue;
						if ( buf.cursize + 64 > buf.maxsize )
						{
							Globals.cls.demofile.Write( EndianHandler.SwapInt( buf.cursize ) );
							Globals.cls.demofile.Write( buf.data, 0, buf.cursize );
							buf.cursize = 0;
						}

						MSG.WriteByte( buf, Defines.svc_spawnbaseline );
						MSG.WriteDeltaEntity( nullstate, Globals.cl_entities[i].baseline, buf, true, true );
					}

					MSG.WriteByte( buf, Defines.svc_stufftext );
					MSG.WriteString( buf, "precache\\n" );
					Globals.cls.demofile.Write( EndianHandler.SwapInt( buf.cursize ) );
					Globals.cls.demofile.Write( buf.data, 0, buf.cursize );
				}
				catch ( IOException e )
				{
				}
			}
		}

		static xcommand_t ForwardToServer_f = new Anonymousxcommand_t2();
		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				if ( Globals.cls.state != Defines.ca_connected && Globals.cls.state != Defines.ca_active )
				{
					Com.Printf( "Can't \\\"" + Cmd.Argv( 0 ) + "\\\", not connected\\n" );
					return;
				}

				if ( Cmd.Argc() > 1 )
				{
					MSG.WriteByte( Globals.cls.netchan.message, Defines.clc_stringcmd );
					SZ.Print( Globals.cls.netchan.message, Cmd.Args() );
				}
			}
		}

		static xcommand_t Pause_f = new Anonymousxcommand_t3();
		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public override void Execute( )
			{
				if ( Cvar.VariableValue( "maxclients" ) > 1 || Globals.server_state == 0 )
				{
					Cvar.SetValue( "paused", 0 );
					return;
				}

				Cvar.SetValue( "paused", Globals.cl_paused.value );
			}
		}

		public static xcommand_t Quit_f = new Anonymousxcommand_t4();
		private sealed class Anonymousxcommand_t4 : xcommand_t
		{
			public override void Execute( )
			{
				Disconnect();
				Com.Quit();
			}
		}

		static xcommand_t Connect_f = new Anonymousxcommand_t5();
		private sealed class Anonymousxcommand_t5 : xcommand_t
		{
			public override void Execute( )
			{
				String server;
				if ( Cmd.Argc() != 2 )
				{
					Com.Printf( "usage: connect <server>\\n" );
					return;
				}

				if ( Globals.server_state != 0 )
				{
					SV_MAIN.SV_Shutdown( "Server quit\\n", false );
				}
				else
				{
					Disconnect();
				}

				server = Cmd.Argv( 1 );
				NET.Config( true );
				Disconnect();
				Globals.cls.state = Defines.ca_connecting;
				Globals.cls.servername = server;
				Globals.cls.connect_time = -99999;
			}
		}

		static xcommand_t Rcon_f = new Anonymousxcommand_t6();
		private sealed class Anonymousxcommand_t6 : xcommand_t
		{
			public override void Execute( )
			{
				if ( Globals.rcon_client_password.string_renamed.Length == 0 )
				{
					Com.Printf( "You must set 'rcon_password' before\\nissuing an rcon command.\\n" );
					return;
				}

				StringBuffer message = new StringBuffer( 1024 );
				message.Append( 'ÿ' );
				message.Append( 'ÿ' );
				message.Append( 'ÿ' );
				message.Append( 'ÿ' );
				NET.Config( true );
				message.Append( "rcon " );
				message.Append( Globals.rcon_client_password.string_renamed );
				message.Append( " " );
				for ( var i = 1; i < Cmd.Argc(); i++ )
				{
					message.Append( Cmd.Argv( i ) );
					message.Append( " " );
				}

				netadr_t to = new netadr_t();
				if ( Globals.cls.state >= Defines.ca_connected )
					to = Globals.cls.netchan.remote_address;
				else
				{
					if ( Globals.rcon_address.string_renamed.Length == 0 )
					{
						Com.Printf( "You must either be connected,\\nor set the 'rcon_address' cvar\\nto issue rcon commands\\n" );
						return;
					}

					NET.StringToAdr( Globals.rcon_address.string_renamed, to );
					if ( to.port == 0 )
						to.port = Defines.PORT_SERVER;
				}

				message.Append( '\\' );
				var b = message.ToString();
				NET.SendPacket( Defines.NS_CLIENT, b.Length, Lib.StringToBytes( b ), to );
			}
		}

		static xcommand_t Disconnect_f = new Anonymousxcommand_t7();
		private sealed class Anonymousxcommand_t7 : xcommand_t
		{
			public override void Execute( )
			{
				Com.Error( Defines.ERR_DROP, "Disconnected from server" );
			}
		}

		static xcommand_t Changing_f = new Anonymousxcommand_t8();
		private sealed class Anonymousxcommand_t8 : xcommand_t
		{
			public override void Execute( )
			{
				if ( Globals.cls.download != null )
					return;
				SCR.BeginLoadingPlaque();
				Globals.cls.state = Defines.ca_connected;
				Com.Printf( "\\nChanging map...\\n" );
			}
		}

		static xcommand_t Reconnect_f = new Anonymousxcommand_t9();
		private sealed class Anonymousxcommand_t9 : xcommand_t
		{
			public override void Execute( )
			{
				if ( Globals.cls.download != null )
					return;
				S.StopAllSounds();
				if ( Globals.cls.state == Defines.ca_connected )
				{
					Com.Printf( "reconnecting...\\n" );
					Globals.cls.state = Defines.ca_connected;
					MSG.WriteChar( Globals.cls.netchan.message, Defines.clc_stringcmd );
					MSG.WriteString( Globals.cls.netchan.message, "new" );
					return;
				}

				if ( Globals.cls.servername != null )
				{
					if ( Globals.cls.state >= Defines.ca_connected )
					{
						Disconnect();
						Globals.cls.connect_time = Globals.cls.realtime - 1500;
					}
					else
						Globals.cls.connect_time = -99999;
					Globals.cls.state = Defines.ca_connecting;
					Com.Printf( "reconnecting...\\n" );
				}
			}
		}

		public static xcommand_t PingServers_f = new Anonymousxcommand_t10();
		private sealed class Anonymousxcommand_t10 : xcommand_t
		{
			public override void Execute( )
			{
				Int32 i;
				netadr_t adr = new netadr_t();
				String name;
				String adrstring;
				cvar_t noudp;
				cvar_t noipx;
				NET.Config( true );
				Com.Printf( "pinging broadcast...\\n" );
				noudp = Cvar.Get( "noudp", "0", Defines.CVAR_NOSET );
				if ( noudp.value == 0F )
				{
					adr.type = Defines.NA_BROADCAST;
					adr.port = Defines.PORT_SERVER;
					Netchan.OutOfBandPrint( Defines.NS_CLIENT, adr, "info " + Defines.PROTOCOL_VERSION );
				}

				noipx = Cvar.Get( "noipx", "1", Defines.CVAR_NOSET );
				if ( noipx.value == 0F )
				{
					adr.type = Defines.NA_BROADCAST_IPX;
					adr.port = Defines.PORT_SERVER;
					Netchan.OutOfBandPrint( Defines.NS_CLIENT, adr, "info " + Defines.PROTOCOL_VERSION );
				}

				for ( i = 0; i < 16; i++ )
				{
					name = "adr" + i;
					adrstring = Cvar.VariableString( name );
					if ( adrstring == null || adrstring.Length == 0 )
						continue;
					Com.Printf( "pinging " + adrstring + "...\\n" );
					if ( !NET.StringToAdr( adrstring, adr ) )
					{
						Com.Printf( "Bad address: " + adrstring + "\\n" );
						continue;
					}

					if ( adr.port == 0 )
						adr.port = Defines.PORT_SERVER;
					Netchan.OutOfBandPrint( Defines.NS_CLIENT, adr, "info " + Defines.PROTOCOL_VERSION );
				}
			}
		}

		static xcommand_t Skins_f = new Anonymousxcommand_t11();
		private sealed class Anonymousxcommand_t11 : xcommand_t
		{
			public override void Execute( )
			{
				Int32 i;
				for ( i = 0; i < Defines.MAX_CLIENTS; i++ )
				{
					if ( Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i] == null )
						continue;
					Com.Printf( "client " + i + ": " + Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i] + "\\n" );
					SCR.UpdateScreen();
					CoreSys.SendKeyEvents();
					CL_parse.ParseClientinfo( i );
				}
			}
		}

		static xcommand_t Userinfo_f = new Anonymousxcommand_t12();
		private sealed class Anonymousxcommand_t12 : xcommand_t
		{
			public override void Execute( )
			{
				Com.Printf( "User info settings:\\n" );
				Info.Print( Cvar.Userinfo() );
			}
		}

		public static xcommand_t Snd_Restart_f = new Anonymousxcommand_t13();
		private sealed class Anonymousxcommand_t13 : xcommand_t
		{
			public override void Execute( )
			{
				S.Shutdown();
				S.Init();
				CL_parse.RegisterSounds();
			}
		}

		public static readonly Int32 ENV_CNT = ( Defines.CS_PLAYERSKINS + Defines.MAX_CLIENTS * CL.PLAYER_MULT );
		public static readonly Int32 TEXTURE_CNT = ( ENV_CNT + 13 );
		static String[] env_suf = new[] { "rt", "bk", "lf", "ft", "up", "dn" };
		static xcommand_t Precache_f = new Anonymousxcommand_t14();
		private sealed class Anonymousxcommand_t14 : xcommand_t
		{
			public override void Execute( )
			{
				if ( Cmd.Argc() < 2 )
				{
					Int32[] iw = new[] { 0 };
					CM.CM_LoadMap( Globals.cl.configstrings[Defines.CS_MODELS + 1], true, iw );
					CL_parse.RegisterSounds();
					CL_view.PrepRefresh();
					return;
				}

				CL.precache_check = Defines.CS_MODELS;
				CL.precache_spawncount = Lib.Atoi( Cmd.Argv( 1 ) );
				CL.precache_model = null;
				CL.precache_model_skin = 0;
				RequestNextDownload();
			}
		}

		private static Int32 extratime;
		static Boolean isdown = false;
		public static void WriteDemoMessage( )
		{
			Int32 swlen;
			swlen = Globals.net_message.cursize - 8;
			try
			{
				Globals.cls.demofile.Write( EndianHandler.SwapInt( swlen ) );
				Globals.cls.demofile.Write( Globals.net_message.data, 8, swlen );
			}
			catch ( IOException e )
			{
			}
		}

		static void SendConnectPacket( )
		{
			netadr_t adr = new netadr_t();
			Int32 port;
			if ( !NET.StringToAdr( Globals.cls.servername, adr ) )
			{
				Com.Printf( "Bad server address\\n" );
				Globals.cls.connect_time = 0;
				return;
			}

			if ( adr.port == 0 )
				adr.port = Defines.PORT_SERVER;
			port = ( Int32 ) Cvar.VariableValue( "qport" );
			Globals.userinfo_modified = false;
			Netchan.OutOfBandPrint( Defines.NS_CLIENT, adr, "connect " + Defines.PROTOCOL_VERSION + " " + port + " " + Globals.cls.challenge + " \\\"" + Cvar.Userinfo() + "\\\"\\n" );
		}

		static void CheckForResend( )
		{
			if ( Globals.cls.state == Defines.ca_disconnected && Globals.server_state != 0 )
			{
				Globals.cls.state = Defines.ca_connecting;
				Globals.cls.servername = "localhost";
				SendConnectPacket();
				return;
			}

			if ( Globals.cls.state != Defines.ca_connecting )
				return;
			if ( Globals.cls.realtime - Globals.cls.connect_time < 3000 )
				return;
			netadr_t adr = new netadr_t();
			if ( !NET.StringToAdr( Globals.cls.servername, adr ) )
			{
				Com.Printf( "Bad server address\\n" );
				Globals.cls.state = Defines.ca_disconnected;
				return;
			}

			if ( adr.port == 0 )
				adr.port = Defines.PORT_SERVER;
			Globals.cls.connect_time = Globals.cls.realtime;
			Com.Printf( "Connecting to " + Globals.cls.servername + "...\\n" );
			Netchan.OutOfBandPrint( Defines.NS_CLIENT, adr, "getchallenge\\n" );
		}

		public static void ClearState( )
		{
			S.StopAllSounds();
			CL_fx.ClearEffects();
			CL_tent.ClearTEnts();
			Globals.cl = new client_state_t();
			for ( var i = 0; i < Globals.cl_entities.Length; i++ )
			{
				Globals.cl_entities[i] = new centity_t();
			}

			SZ.Clear( Globals.cls.netchan.message );
		}

		static void Disconnect( )
		{
			String fin;
			if ( Globals.cls.state == Defines.ca_disconnected )
				return;
			if ( Globals.cl_timedemo != null && Globals.cl_timedemo.value != 0F )
			{
				Int32 time;
				time = ( Int32 ) ( Timer.Milliseconds() - Globals.cl.timedemo_start );
				if ( time > 0 )
					Com.Printf( "%i frames, %3.1f seconds: %3.1f fps\\n", Globals.cl.timedemo_frames, time / 1000, Globals.cl.timedemo_frames * 1000 / time );
			}

			Math3D.VectorClear( Globals.cl.refdef.blend );
			Globals.re.CinematicSetPalette( null );
			Menu.ForceMenuOff();
			Globals.cls.connect_time = 0;
			SCR.StopCinematic();
			if ( Globals.cls.demorecording )
				Stop_f.Execute();
			fin = ( Char ) Defines.clc_stringcmd + "disconnect";
			Netchan.Transmit( Globals.cls.netchan, fin.Length, Lib.StringToBytes( fin ) );
			Netchan.Transmit( Globals.cls.netchan, fin.Length, Lib.StringToBytes( fin ) );
			Netchan.Transmit( Globals.cls.netchan, fin.Length, Lib.StringToBytes( fin ) );
			ClearState();
			if ( Globals.cls.download != null )
			{
				Globals.cls.download.Dispose();
				Globals.cls.download = null;
			}

			Globals.cls.state = Defines.ca_disconnected;
		}

		static void ParseStatusMessage( )
		{
			String s;
			s = MSG.ReadString( Globals.net_message );
			Com.Printf( s + "\\n" );
			Menu.AddToServerList( Globals.net_from, s );
		}

		static void ConnectionlessPacket( )
		{
			String s;
			String c;
			MSG.BeginReading( Globals.net_message );
			MSG.ReadLong( Globals.net_message );
			s = MSG.ReadStringLine( Globals.net_message );
			Cmd.TokenizeString( s.ToCharArray(), false );
			c = Cmd.Argv( 0 );
			Com.Println( Globals.net_from.ToString() + ": " + c );
			if ( c.Equals( "client_connect" ) )
			{
				if ( Globals.cls.state == Defines.ca_connected )
				{
					Com.Printf( "Dup connect received.  Ignored.\\n" );
					return;
				}

				Netchan.Setup( Defines.NS_CLIENT, Globals.cls.netchan, Globals.net_from, Globals.cls.quakePort );
				MSG.WriteChar( Globals.cls.netchan.message, Defines.clc_stringcmd );
				MSG.WriteString( Globals.cls.netchan.message, "new" );
				Globals.cls.state = Defines.ca_connected;
				return;
			}

			if ( c.Equals( "info" ) )
			{
				ParseStatusMessage();
				return;
			}

			if ( c.Equals( "cmd" ) )
			{
				if ( !NET.IsLocalAddress( Globals.net_from ) )
				{
					Com.Printf( "Command packet from remote host.  Ignored.\\n" );
					return;
				}

				s = MSG.ReadString( Globals.net_message );
				Cbuf.AddText( s );
				Cbuf.AddText( "\\n" );
				return;
			}

			if ( c.Equals( "print" ) )
			{
				s = MSG.ReadString( Globals.net_message );
				if ( s.Length > 0 )
					Com.Printf( s );
				return;
			}

			if ( c.Equals( "ping" ) )
			{
				Netchan.OutOfBandPrint( Defines.NS_CLIENT, Globals.net_from, "ack" );
				return;
			}

			if ( c.Equals( "challenge" ) )
			{
				Globals.cls.challenge = Lib.Atoi( Cmd.Argv( 1 ) );
				SendConnectPacket();
				return;
			}

			if ( c.Equals( "echo" ) )
			{
				Netchan.OutOfBandPrint( Defines.NS_CLIENT, Globals.net_from, Cmd.Argv( 1 ) );
				return;
			}

			Com.Printf( "Unknown command.\\n" );
		}

		static void ReadPackets( )
		{
			while ( NET.GetPacket( Defines.NS_CLIENT, Globals.net_from, Globals.net_message ) )
			{
				if ( Globals.net_message.data[0] == -1 && Globals.net_message.data[1] == -1 && Globals.net_message.data[2] == -1 && Globals.net_message.data[3] == -1 )
				{
					ConnectionlessPacket();
					continue;
				}

				if ( Globals.cls.state == Defines.ca_disconnected || Globals.cls.state == Defines.ca_connecting )
					continue;
				if ( Globals.net_message.cursize < 8 )
				{
					Com.Printf( NET.AdrToString( Globals.net_from ) + ": Runt packet\\n" );
					continue;
				}

				if ( !NET.CompareAdr( Globals.net_from, Globals.cls.netchan.remote_address ) )
				{
					Com.DPrintf( NET.AdrToString( Globals.net_from ) + ":sequenced packet without connection\\n" );
					continue;
				}

				if ( !Netchan.Process( Globals.cls.netchan, Globals.net_message ) )
					continue;
				CL_parse.ParseServerMessage();
			}

			if ( Globals.cls.state >= Defines.ca_connected && Globals.cls.realtime - Globals.cls.netchan.last_received > Globals.cl_timeout.value * 1000 )
			{
				if ( ++Globals.cl.timeoutcount > 5 )
				{
					Com.Printf( "\\nServer connection timed out.\\n" );
					Disconnect();
					return;
				}
			}
			else
				Globals.cl.timeoutcount = 0;
		}

		public static void FixUpGender( )
		{
			String sk;
			if ( Globals.gender_auto.value != 0F )
			{
				if ( Globals.gender.modified )
				{
					Globals.gender.modified = false;
					return;
				}

				sk = Globals.skin.string_renamed;
				if ( sk.StartsWith( "male" ) || sk.StartsWith( "cyborg" ) )
					Cvar.Set( "gender", "male" );
				else if ( sk.StartsWith( "female" ) || sk.StartsWith( "crackhor" ) )
					Cvar.Set( "gender", "female" );
				else
					Cvar.Set( "gender", "none" );
				Globals.gender.modified = false;
			}
		}

		public static void RequestNextDownload( )
		{
			var map_checksum = 0;
			String fn;
			qfiles.dmdl_t pheader;
			if ( Globals.cls.state != Defines.ca_connected )
				return;
			if ( SV_MAIN.allow_download.value == 0 && CL.precache_check < ENV_CNT )
				CL.precache_check = ENV_CNT;
			if ( CL.precache_check == Defines.CS_MODELS )
			{
				CL.precache_check = Defines.CS_MODELS + 2;
				if ( SV_MAIN.allow_download_maps.value != 0 )
					if ( !CL_parse.CheckOrDownloadFile( Globals.cl.configstrings[Defines.CS_MODELS + 1] ) )
						return;
			}

			if ( CL.precache_check >= Defines.CS_MODELS && CL.precache_check < Defines.CS_MODELS + Defines.MAX_MODELS )
			{
				if ( SV_MAIN.allow_download_models.value != 0 )
				{
					while ( CL.precache_check < Defines.CS_MODELS + Defines.MAX_MODELS && Globals.cl.configstrings[CL.precache_check].Length > 0 )
					{
						if ( Globals.cl.configstrings[CL.precache_check][0] == '*' || Globals.cl.configstrings[CL.precache_check][0] == '#' )
						{
							CL.precache_check++;
							continue;
						}

						if ( CL.precache_model_skin == 0 )
						{
							if ( !CL_parse.CheckOrDownloadFile( Globals.cl.configstrings[CL.precache_check] ) )
							{
								CL.precache_model_skin = 1;
								return;
							}

							CL.precache_model_skin = 1;
						}

						if ( CL.precache_model == null )
						{
							CL.precache_model = FS.LoadFile( Globals.cl.configstrings[CL.precache_check] );
							if ( CL.precache_model == null )
							{
								CL.precache_model_skin = 0;
								CL.precache_check++;
								continue;
							}

							ByteBuffer bb = ByteBuffer.Wrap( CL.precache_model );
							bb.Order = ByteOrder.LittleEndian;
							var header = bb.GetInt32();
							if ( header != qfiles.IDALIASHEADER )
							{
								FS.FreeFile( CL.precache_model );
								CL.precache_model = null;
								CL.precache_model_skin = 0;
								CL.precache_check++;
								continue;
							}

							var pm = ByteBuffer.Wrap( CL.precache_model );
							pm.Order = ByteOrder.LittleEndian;
							pheader = new dmdl_t( pm );
							if ( pheader.version != Defines.ALIAS_VERSION )
							{
								CL.precache_check++;
								CL.precache_model_skin = 0;
								continue;
							}
						}
						var pm2 = ByteBuffer.Wrap( CL.precache_model );
						pm2.Order = ByteOrder.LittleEndian;
						pheader = new dmdl_t( pm2 );
						var num_skins = pheader.num_skins;
						while ( CL.precache_model_skin - 1 < num_skins )
						{
							var name = Lib.CtoJava( CL.precache_model, pheader.ofs_skins + ( CL.precache_model_skin - 1 ) * Defines.MAX_SKINNAME, Defines.MAX_SKINNAME * num_skins );
							if ( !CL_parse.CheckOrDownloadFile( name ) )
							{
								CL.precache_model_skin++;
								return;
							}

							CL.precache_model_skin++;
						}

						if ( CL.precache_model != null )
						{
							FS.FreeFile( CL.precache_model );
							CL.precache_model = null;
						}

						CL.precache_model_skin = 0;
						CL.precache_check++;
					}
				}

				CL.precache_check = Defines.CS_SOUNDS;
			}

			if ( CL.precache_check >= Defines.CS_SOUNDS && CL.precache_check < Defines.CS_SOUNDS + Defines.MAX_SOUNDS )
			{
				if ( SV_MAIN.allow_download_sounds.value != 0 )
				{
					if ( CL.precache_check == Defines.CS_SOUNDS )
						CL.precache_check++;
					while ( CL.precache_check < Defines.CS_SOUNDS + Defines.MAX_SOUNDS && Globals.cl.configstrings[CL.precache_check].Length > 0 )
					{
						if ( Globals.cl.configstrings[CL.precache_check][0] == '*' )
						{
							CL.precache_check++;
							continue;
						}

						fn = "sound/" + Globals.cl.configstrings[CL.precache_check++];
						if ( !CL_parse.CheckOrDownloadFile( fn ) )
							return;
					}
				}

				CL.precache_check = Defines.CS_IMAGES;
			}

			if ( CL.precache_check >= Defines.CS_IMAGES && CL.precache_check < Defines.CS_IMAGES + Defines.MAX_IMAGES )
			{
				if ( CL.precache_check == Defines.CS_IMAGES )
					CL.precache_check++;
				while ( CL.precache_check < Defines.CS_IMAGES + Defines.MAX_IMAGES && Globals.cl.configstrings[CL.precache_check].Length > 0 )
				{
					fn = "pics/" + Globals.cl.configstrings[CL.precache_check++] + ".pcx";
					if ( !CL_parse.CheckOrDownloadFile( fn ) )
						return;
				}

				CL.precache_check = Defines.CS_PLAYERSKINS;
			}

			if ( CL.precache_check >= Defines.CS_PLAYERSKINS && CL.precache_check < Defines.CS_PLAYERSKINS + Defines.MAX_CLIENTS * CL.PLAYER_MULT )
			{
				if ( SV_MAIN.allow_download_players.value != 0 )
				{
					while ( CL.precache_check < Defines.CS_PLAYERSKINS + Defines.MAX_CLIENTS * CL.PLAYER_MULT )
					{
						Int32 i, n;
						String model, skin;
						i = ( CL.precache_check - Defines.CS_PLAYERSKINS ) / CL.PLAYER_MULT;
						n = ( CL.precache_check - Defines.CS_PLAYERSKINS ) % CL.PLAYER_MULT;
						if ( Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i].Length == 0 )
						{
							CL.precache_check = Defines.CS_PLAYERSKINS + ( i + 1 ) * CL.PLAYER_MULT;
							continue;
						}

						var pos = Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i].IndexOf( '\\' );
						if ( pos != -1 )
							pos++;
						else
							pos = 0;
						var pos2 = Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i].IndexOf( '\\', pos );
						if ( pos2 == -1 )
							pos2 = Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i].IndexOf( '/', pos );
						model = Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i].Substring( pos, pos2 );
						skin = Globals.cl.configstrings[Defines.CS_PLAYERSKINS + i].Substring( pos2 + 1 );
						switch ( n )
						{
							case 0:
								fn = "players/" + model + "/tris.md2";
								if ( !CL_parse.CheckOrDownloadFile( fn ) )
								{
									CL.precache_check = Defines.CS_PLAYERSKINS + i * CL.PLAYER_MULT + 1;
									return;
								}


								n++;
								break;
							case 1:
								fn = "players/" + model + "/weapon.md2";
								if ( !CL_parse.CheckOrDownloadFile( fn ) )
								{
									CL.precache_check = Defines.CS_PLAYERSKINS + i * CL.PLAYER_MULT + 2;
									return;
								}

								n++;
								break;
							case 2:
								fn = "players/" + model + "/weapon.pcx";
								if ( !CL_parse.CheckOrDownloadFile( fn ) )
								{
									CL.precache_check = Defines.CS_PLAYERSKINS + i * CL.PLAYER_MULT + 3;
									return;
								}

								n++;
								break;
							case 3:
								fn = "players/" + model + "/" + skin + ".pcx";
								if ( !CL_parse.CheckOrDownloadFile( fn ) )
								{
									CL.precache_check = Defines.CS_PLAYERSKINS + i * CL.PLAYER_MULT + 4;
									return;
								}

								n++;
								break;
							case 4:
								fn = "players/" + model + "/" + skin + "_i.pcx";
								if ( !CL_parse.CheckOrDownloadFile( fn ) )
								{
									CL.precache_check = Defines.CS_PLAYERSKINS + i * CL.PLAYER_MULT + 5;
									return;
								}

								CL.precache_check = Defines.CS_PLAYERSKINS + ( i + 1 ) * CL.PLAYER_MULT;
								break;
						}
					}
				}

				CL.precache_check = ENV_CNT;
			}

			if ( CL.precache_check == ENV_CNT )
			{
				CL.precache_check = ENV_CNT + 1;
				Int32[] iw = new[] { map_checksum };
				CM.CM_LoadMap( Globals.cl.configstrings[Defines.CS_MODELS + 1], true, iw );
				map_checksum = iw[0];
				if ( ( map_checksum ^ Lib.Atoi( Globals.cl.configstrings[Defines.CS_MAPCHECKSUM] ) ) != 0 )
				{
					Com.Error( Defines.ERR_DROP, "Local map version differs from server: " + map_checksum + " != '" + Globals.cl.configstrings[Defines.CS_MAPCHECKSUM] + "'\\n" );
					return;
				}
			}

			if ( CL.precache_check > ENV_CNT && CL.precache_check < TEXTURE_CNT )
			{
				if ( SV_MAIN.allow_download.value != 0 && SV_MAIN.allow_download_maps.value != 0 )
				{
					while ( CL.precache_check < TEXTURE_CNT )
					{
						var n = CL.precache_check++ - ENV_CNT - 1;
						if ( ( n & 1 ) != 0 )
							fn = "env/" + Globals.cl.configstrings[Defines.CS_SKY] + env_suf[n / 2] + ".pcx";
						else
							fn = "env/" + Globals.cl.configstrings[Defines.CS_SKY] + env_suf[n / 2] + ".tga";
						if ( !CL_parse.CheckOrDownloadFile( fn ) )
							return;
					}
				}

				CL.precache_check = TEXTURE_CNT;
			}

			if ( CL.precache_check == TEXTURE_CNT )
			{
				CL.precache_check = TEXTURE_CNT + 1;
				CL.precache_tex = 0;
			}

			if ( CL.precache_check == TEXTURE_CNT + 1 )
			{
				if ( SV_MAIN.allow_download.value != 0 && SV_MAIN.allow_download_maps.value != 0 )
				{
					while ( CL.precache_tex < CM.numtexinfo )
					{
						fn = "textures/" + CM.map_surfaces[CL.precache_tex++].rname + ".wal";
						if ( !CL_parse.CheckOrDownloadFile( fn ) )
							return;
					}
				}

				CL.precache_check = TEXTURE_CNT + 999;
			}

			CL_parse.RegisterSounds();
			CL_view.PrepRefresh();
			MSG.WriteByte( Globals.cls.netchan.message, Defines.clc_stringcmd );
			MSG.WriteString( Globals.cls.netchan.message, "begin " + CL.precache_spawncount + "\\n" );
		}

		public static void InitLocal( )
		{
			Globals.cls.state = Defines.ca_disconnected;
			Globals.cls.realtime = Timer.Milliseconds();
			CL_input.InitInput();
			Cvar.Get( "adr0", "", Defines.CVAR_ARCHIVE );
			Cvar.Get( "adr1", "", Defines.CVAR_ARCHIVE );
			Cvar.Get( "adr2", "", Defines.CVAR_ARCHIVE );
			Cvar.Get( "adr3", "", Defines.CVAR_ARCHIVE );
			Cvar.Get( "adr4", "", Defines.CVAR_ARCHIVE );
			Cvar.Get( "adr5", "", Defines.CVAR_ARCHIVE );
			Cvar.Get( "adr6", "", Defines.CVAR_ARCHIVE );
			Cvar.Get( "adr7", "", Defines.CVAR_ARCHIVE );
			Cvar.Get( "adr8", "", Defines.CVAR_ARCHIVE );
			Globals.cl_stereo_separation = Cvar.Get( "cl_stereo_separation", "0.4", Defines.CVAR_ARCHIVE );
			Globals.cl_stereo = Cvar.Get( "cl_stereo", "0", 0 );
			Globals.cl_add_blend = Cvar.Get( "cl_blend", "1", 0 );
			Globals.cl_add_lights = Cvar.Get( "cl_lights", "1", 0 );
			Globals.cl_add_particles = Cvar.Get( "cl_particles", "1", 0 );
			Globals.cl_add_entities = Cvar.Get( "cl_entities", "1", 0 );
			Globals.cl_gun = Cvar.Get( "cl_gun", "1", 0 );
			Globals.cl_footsteps = Cvar.Get( "cl_footsteps", "1", 0 );
			Globals.cl_noskins = Cvar.Get( "cl_noskins", "0", 0 );
			Globals.cl_autoskins = Cvar.Get( "cl_autoskins", "0", 0 );
			Globals.cl_predict = Cvar.Get( "cl_predict", "1", 0 );
			Globals.cl_maxfps = Cvar.Get( "cl_maxfps", "90", 0 );
			Globals.cl_upspeed = Cvar.Get( "cl_upspeed", "200", 0 );
			Globals.cl_forwardspeed = Cvar.Get( "cl_forwardspeed", "200", 0 );
			Globals.cl_sidespeed = Cvar.Get( "cl_sidespeed", "200", 0 );
			Globals.cl_yawspeed = Cvar.Get( "cl_yawspeed", "140", 0 );
			Globals.cl_pitchspeed = Cvar.Get( "cl_pitchspeed", "150", 0 );
			Globals.cl_anglespeedkey = Cvar.Get( "cl_anglespeedkey", "1.5", 0 );
			Globals.cl_run = Cvar.Get( "cl_run", "0", Defines.CVAR_ARCHIVE );
			Globals.lookspring = Cvar.Get( "lookspring", "0", Defines.CVAR_ARCHIVE );
			Globals.lookstrafe = Cvar.Get( "lookstrafe", "0", Defines.CVAR_ARCHIVE );
			Globals.sensitivity = Cvar.Get( "sensitivity", "3", Defines.CVAR_ARCHIVE );
			Globals.m_pitch = Cvar.Get( "m_pitch", "0.022", Defines.CVAR_ARCHIVE );
			Globals.m_yaw = Cvar.Get( "m_yaw", "0.022", 0 );
			Globals.m_forward = Cvar.Get( "m_forward", "1", 0 );
			Globals.m_side = Cvar.Get( "m_side", "1", 0 );
			Globals.cl_shownet = Cvar.Get( "cl_shownet", "0", 0 );
			Globals.cl_showmiss = Cvar.Get( "cl_showmiss", "0", 0 );
			Globals.cl_showclamp = Cvar.Get( "showclamp", "0", 0 );
			Globals.cl_timeout = Cvar.Get( "cl_timeout", "120", 0 );
			Globals.cl_paused = Cvar.Get( "paused", "0", 0 );
			Globals.cl_timedemo = Cvar.Get( "timedemo", "0", 0 );
			Globals.rcon_client_password = Cvar.Get( "rcon_password", "", 0 );
			Globals.rcon_address = Cvar.Get( "rcon_address", "", 0 );
			Globals.cl_lightlevel = Cvar.Get( "r_lightlevel", "0", 0 );
			Globals.info_password = Cvar.Get( "password", "", Defines.CVAR_USERINFO );
			Globals.info_spectator = Cvar.Get( "spectator", "0", Defines.CVAR_USERINFO );
			Globals.name = Cvar.Get( "name", "unnamed", Defines.CVAR_USERINFO | Defines.CVAR_ARCHIVE );
			Globals.skin = Cvar.Get( "skin", "male/grunt", Defines.CVAR_USERINFO | Defines.CVAR_ARCHIVE );
			Globals.rate = Cvar.Get( "rate", "25000", Defines.CVAR_USERINFO | Defines.CVAR_ARCHIVE );
			Globals.msg = Cvar.Get( "msg", "1", Defines.CVAR_USERINFO | Defines.CVAR_ARCHIVE );
			Globals.hand = Cvar.Get( "hand", "0", Defines.CVAR_USERINFO | Defines.CVAR_ARCHIVE );
			Globals.fov = Cvar.Get( "fov", "90", Defines.CVAR_USERINFO | Defines.CVAR_ARCHIVE );
			Globals.gender = Cvar.Get( "gender", "male", Defines.CVAR_USERINFO | Defines.CVAR_ARCHIVE );
			Globals.gender_auto = Cvar.Get( "gender_auto", "1", Defines.CVAR_ARCHIVE );
			Globals.gender.modified = false;
			Globals.cl_vwep = Cvar.Get( "cl_vwep", "1", Defines.CVAR_ARCHIVE );
			Cmd.AddCommand( "cmd", ForwardToServer_f );
			Cmd.AddCommand( "pause", Pause_f );
			Cmd.AddCommand( "pingservers", PingServers_f );
			Cmd.AddCommand( "skins", Skins_f );
			Cmd.AddCommand( "userinfo", Userinfo_f );
			Cmd.AddCommand( "snd_restart", Snd_Restart_f );
			Cmd.AddCommand( "changing", Changing_f );
			Cmd.AddCommand( "disconnect", Disconnect_f );
			Cmd.AddCommand( "record", Record_f );
			Cmd.AddCommand( "stop", Stop_f );
			Cmd.AddCommand( "quit", Quit_f );
			Cmd.AddCommand( "connect", Connect_f );
			Cmd.AddCommand( "reconnect", Reconnect_f );
			Cmd.AddCommand( "rcon", Rcon_f );
			Cmd.AddCommand( "precache", Precache_f );
			Cmd.AddCommand( "download", CL_parse.Download_f );
			Cmd.AddCommand( "wave", null );
			Cmd.AddCommand( "inven", null );
			Cmd.AddCommand( "kill", null );
			Cmd.AddCommand( "use", null );
			Cmd.AddCommand( "drop", null );
			Cmd.AddCommand( "say", null );
			Cmd.AddCommand( "say_team", null );
			Cmd.AddCommand( "info", null );
			Cmd.AddCommand( "prog", null );
			Cmd.AddCommand( "give", null );
			Cmd.AddCommand( "god", null );
			Cmd.AddCommand( "notarget", null );
			Cmd.AddCommand( "noclip", null );
			Cmd.AddCommand( "invuse", null );
			Cmd.AddCommand( "invprev", null );
			Cmd.AddCommand( "invnext", null );
			Cmd.AddCommand( "invdrop", null );
			Cmd.AddCommand( "weapnext", null );
			Cmd.AddCommand( "weapprev", null );
		}

		public static void WriteConfiguration( )
		{
			QuakeFile f;
			String path;
			path = FS.Gamedir() + "/config.cfg";
			f = new QuakeFile( path, FileAccess.ReadWrite );
			if ( f == null )
			{
				Com.Printf( "Couldn't write config.cfg.\\n" );
				return;
			}

			try
			{
				f.Seek( 0 );
				//f.SetLength(0);
			}
			catch ( IOException e1 )
			{
			}

			try
			{
				f.Write( "// generated by quake, do not modify\\n" );
			}
			catch ( IOException e )
			{
			}

			Key.WriteBindings( f );
			f.Dispose();
			Cvar.WriteVariables( path );
		}

		public static void FixCvarCheats( )
		{
			Int32 i;
			CL.cheatvar_t var;
			if ( "1".Equals( Globals.cl.configstrings[Defines.CS_MAXCLIENTS] ) || 0 == Globals.cl.configstrings[Defines.CS_MAXCLIENTS].Length )
				return;
			if ( 0 == CL.numcheatvars )
			{
				while ( CL.cheatvars[CL.numcheatvars].name != null )
				{
					CL.cheatvars[CL.numcheatvars].var = Cvar.Get( CL.cheatvars[CL.numcheatvars].name, CL.cheatvars[CL.numcheatvars].value, 0 );
					CL.numcheatvars++;
				}
			}

			for ( i = 0; i < CL.numcheatvars; i++ )
			{
				var = CL.cheatvars[i];
				if ( !var.var.string_renamed.Equals( var.value ) )
				{
					Cvar.Set( var.name, var.value );
				}
			}
		}

		public static void SendCommand( )
		{
			CoreSys.SendKeyEvents();
			IN.Commands();
			Cbuf.Execute();
			FixCvarCheats();
			CL_input.SendCmd();
			CheckForResend();
		}

		public static void Frame( Int32 msec )
		{
			if ( Globals.dedicated.value != 0 )
				return;
			extratime += msec;
			if ( Globals.cl_timedemo.value == 0F )
			{
				if ( Globals.cls.state == Defines.ca_connected && extratime < 100 )
				{
					return;
				}

				if ( extratime < 1000 / Globals.cl_maxfps.value )
				{
					return;
				}
			}

			IN.Frame();
			Globals.cls.frametime = extratime / 1000F;
			Globals.cl.time += extratime;
			Globals.cls.realtime = Globals.curtime;
			extratime = 0;
			if ( Globals.cls.frametime > ( 1F / 5 ) )
				Globals.cls.frametime = ( 1F / 5 );
			if ( msec > 5000 )
				Globals.cls.netchan.last_received = Timer.Milliseconds();
			ReadPackets();
			SendCommand();
			CL_pred.PredictMovement();
			VID.CheckChanges();
			if ( !Globals.cl.refresh_prepped && Globals.cls.state == Defines.ca_active )
			{
				CL_view.PrepRefresh();
				if ( Globals.cl.cinematictime == 0 )
					GC.Collect();
				//System.Gc();
			}

			SCR.UpdateScreen();
			S.Update( Globals.cl.refdef.vieworg, Globals.cl.v_forward, Globals.cl.v_right, Globals.cl.v_up );
			CL_fx.RunDLights();
			CL_fx.RunLightStyles();
			SCR.RunCinematic();
			SCR.RunConsole();
			Globals.cls.framecount++;
			if ( Globals.cls.state != Defines.ca_active || Globals.cls.key_dest != Defines.key_game )
			{
				try
				{
					Thread.Sleep( 20 );
				}
				catch ( Exception e )
				{
				}
			}
		}

		public static void Shutdown( )
		{
			if ( isdown )
			{
				System.Diagnostics.Debug.Write( "recursive shutdown\\n" );
				return;
			}

			isdown = true;
			WriteConfiguration();
			S.Shutdown();
			IN.Shutdown();
			VID.Shutdown();
		}

		public static void Init( )
		{
			if ( Globals.dedicated.value != 0F )
				return;
			Con.Init();
			S.Init();
			VID.Init();
			V.Init();
			Globals.net_message.data = Globals.net_message_buffer;
			Globals.net_message.maxsize = Globals.net_message_buffer.Length;
			Menu.Init();
			SCR.Init();
			InitLocal();
			IN.Init();
			FS.ExecAutoexec();
			Cbuf.Execute();
		}

		public static void Drop( )
		{
			if ( Globals.cls.state == Defines.ca_uninitialized )
				return;
			if ( Globals.cls.state == Defines.ca_disconnected )
				return;
			Disconnect();
			if ( Globals.cls.disable_servercount != -1 )
				SCR.EndLoadingPlaque();
		}
	}
}
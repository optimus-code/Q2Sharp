using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using Jake2Sharp.util;
using System;
using System.IO;

namespace Jake2.Server
{
	public class SV_USER
	{
		public static edict_t sv_player;
		public class ucmd_t
		{
			public ucmd_t( String n, Runnable r )
			{
				name = n;
				this.r = r;
			}

			public String name;
			public Runnable r;
		}

		static ucmd_t u1 = new ucmd_t( "new", new AnonymousRunnable() );
		private sealed class AnonymousRunnable : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_New_f();
			}
		}

		static ucmd_t[] ucmds = new[] { new ucmd_t( "new", new AnonymousRunnable1() ), new ucmd_t( "configstrings", new AnonymousRunnable2() ), new ucmd_t( "baselines", new AnonymousRunnable3() ), new ucmd_t( "begin", new AnonymousRunnable4() ), new ucmd_t( "nextserver", new AnonymousRunnable5() ), new ucmd_t( "disconnect", new AnonymousRunnable6() ), new ucmd_t( "info", new AnonymousRunnable7() ), new ucmd_t( "download", new AnonymousRunnable8() ), new ucmd_t( "nextdl", new AnonymousRunnable9() ) };
		private sealed class AnonymousRunnable1 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_New_f();
			}
		}

		private sealed class AnonymousRunnable2 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_Configstrings_f();
			}
		}

		private sealed class AnonymousRunnable3 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_Baselines_f();
			}
		}

		private sealed class AnonymousRunnable4 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_Begin_f();
			}
		}

		private sealed class AnonymousRunnable5 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_Nextserver_f();
			}
		}

		private sealed class AnonymousRunnable6 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_Disconnect_f();
			}
		}

		private sealed class AnonymousRunnable7 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_ShowServerinfo_f();
			}
		}

		private sealed class AnonymousRunnable8 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_BeginDownload_f();
			}
		}

		private sealed class AnonymousRunnable9 : Runnable
		{
			protected override void Execute( )
			{
				SV_USER.SV_NextDownload_f();
			}
		}

		public static readonly Int32 MAX_STRINGCMDS = 8;
		public static void SV_BeginDemoserver( )
		{
			String name;
			name = "demos/" + SV_INIT.sv.name;
			try
			{
				SV_INIT.sv.demofile = new QuakeFile( name, FileAccess.ReadWrite );
			}
			catch ( Exception e )
			{
				Com.Error( Defines.ERR_DROP, "Couldn't open " + name + "\\n" );
			}

			if ( SV_INIT.sv.demofile == null )
				Com.Error( Defines.ERR_DROP, "Couldn't open " + name + "\\n" );
		}

		public static void SV_New_f( )
		{
			String gamedir;
			Int32 playernum;
			edict_t ent;
			Com.DPrintf( "New() from " + SV_MAIN.sv_client.name + "\\n" );
			if ( SV_MAIN.sv_client.state != Defines.cs_connected )
			{
				Com.Printf( "New not valid -- already spawned\\n" );
				return;
			}

			if ( SV_INIT.sv.state == Defines.ss_demo )
			{
				SV_BeginDemoserver();
				return;
			}

			gamedir = Cvar.VariableString( "gamedir" );
			MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_serverdata );
			MSG.WriteInt( SV_MAIN.sv_client.netchan.message, Defines.PROTOCOL_VERSION );
			MSG.WriteLong( SV_MAIN.sv_client.netchan.message, SV_INIT.svs.spawncount );
			MSG.WriteByte( SV_MAIN.sv_client.netchan.message, SV_INIT.sv.attractloop ? 1 : 0 );
			MSG.WriteString( SV_MAIN.sv_client.netchan.message, gamedir );
			if ( SV_INIT.sv.state == Defines.ss_cinematic || SV_INIT.sv.state == Defines.ss_pic )
				playernum = -1;
			else
				playernum = SV_MAIN.sv_client.serverindex;
			MSG.WriteShort( SV_MAIN.sv_client.netchan.message, playernum );
			MSG.WriteString( SV_MAIN.sv_client.netchan.message, SV_INIT.sv.configstrings[Defines.CS_NAME] );
			if ( SV_INIT.sv.state == Defines.ss_game )
			{
				ent = GameBase.g_edicts[playernum + 1];
				ent.s.number = playernum + 1;
				SV_MAIN.sv_client.edict = ent;
				SV_MAIN.sv_client.lastcmd = new usercmd_t();
				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_stufftext );
				MSG.WriteString( SV_MAIN.sv_client.netchan.message, "cmd configstrings " + SV_INIT.svs.spawncount + " 0\\n" );
			}
		}

		public static void SV_Configstrings_f( )
		{
			Int32 start;
			Com.DPrintf( "Configstrings() from " + SV_MAIN.sv_client.name + "\\n" );
			if ( SV_MAIN.sv_client.state != Defines.cs_connected )
			{
				Com.Printf( "configstrings not valid -- already spawned\\n" );
				return;
			}

			if ( Lib.Atoi( Cmd.Argv( 1 ) ) != SV_INIT.svs.spawncount )
			{
				Com.Printf( "SV_Configstrings_f from different level\\n" );
				SV_New_f();
				return;
			}

			start = Lib.Atoi( Cmd.Argv( 2 ) );
			while ( SV_MAIN.sv_client.netchan.message.cursize < Defines.MAX_MSGLEN / 2 && start < Defines.MAX_CONFIGSTRINGS )
			{
				if ( SV_INIT.sv.configstrings[start] != null && SV_INIT.sv.configstrings[start].Length != 0 )
				{
					MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_configstring );
					MSG.WriteShort( SV_MAIN.sv_client.netchan.message, start );
					MSG.WriteString( SV_MAIN.sv_client.netchan.message, SV_INIT.sv.configstrings[start] );
				}

				start++;
			}

			if ( start == Defines.MAX_CONFIGSTRINGS )
			{
				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_stufftext );
				MSG.WriteString( SV_MAIN.sv_client.netchan.message, "cmd baselines " + SV_INIT.svs.spawncount + " 0\\n" );
			}
			else
			{
				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_stufftext );
				MSG.WriteString( SV_MAIN.sv_client.netchan.message, "cmd configstrings " + SV_INIT.svs.spawncount + " " + start + "\\n" );
			}
		}

		public static void SV_Baselines_f( )
		{
			Int32 start;
			entity_state_t nullstate;
			entity_state_t base_renamed;
			Com.DPrintf( "Baselines() from " + SV_MAIN.sv_client.name + "\\n" );
			if ( SV_MAIN.sv_client.state != Defines.cs_connected )
			{
				Com.Printf( "baselines not valid -- already spawned\\n" );
				return;
			}

			if ( Lib.Atoi( Cmd.Argv( 1 ) ) != SV_INIT.svs.spawncount )
			{
				Com.Printf( "SV_Baselines_f from different level\\n" );
				SV_New_f();
				return;
			}

			start = Lib.Atoi( Cmd.Argv( 2 ) );
			nullstate = new entity_state_t( null );
			while ( SV_MAIN.sv_client.netchan.message.cursize < Defines.MAX_MSGLEN / 2 && start < Defines.MAX_EDICTS )
			{
				base_renamed = SV_INIT.sv.baselines[start];
				if ( base_renamed.modelindex != 0 || base_renamed.sound != 0 || base_renamed.effects != 0 )
				{
					MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_spawnbaseline );
					MSG.WriteDeltaEntity( nullstate, base_renamed, SV_MAIN.sv_client.netchan.message, true, true );
				}

				start++;
			}

			if ( start == Defines.MAX_EDICTS )
			{
				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_stufftext );
				MSG.WriteString( SV_MAIN.sv_client.netchan.message, "precache " + SV_INIT.svs.spawncount + "\\n" );
			}
			else
			{
				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_stufftext );
				MSG.WriteString( SV_MAIN.sv_client.netchan.message, "cmd baselines " + SV_INIT.svs.spawncount + " " + start + "\\n" );
			}
		}

		public static void SV_Begin_f( )
		{
			Com.DPrintf( "Begin() from " + SV_MAIN.sv_client.name + "\\n" );
			if ( Lib.Atoi( Cmd.Argv( 1 ) ) != SV_INIT.svs.spawncount )
			{
				Com.Printf( "SV_Begin_f from different level\\n" );
				SV_New_f();
				return;
			}

			SV_MAIN.sv_client.state = Defines.cs_spawned;
			PlayerClient.ClientBegin( SV_USER.sv_player );
			Cbuf.InsertFromDefer();
		}

		public static void SV_NextDownload_f( )
		{
			Int32 r;
			Int32 percent;
			Int32 size;
			if ( SV_MAIN.sv_client.download == null )
				return;
			r = SV_MAIN.sv_client.downloadsize - SV_MAIN.sv_client.downloadcount;
			if ( r > 1024 )
				r = 1024;
			MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_download );
			MSG.WriteShort( SV_MAIN.sv_client.netchan.message, r );
			SV_MAIN.sv_client.downloadcount += r;
			size = SV_MAIN.sv_client.downloadsize;
			if ( size == 0 )
				size = 1;
			percent = SV_MAIN.sv_client.downloadcount * 100 / size;
			MSG.WriteByte( SV_MAIN.sv_client.netchan.message, percent );
			SZ.Write( SV_MAIN.sv_client.netchan.message, SV_MAIN.sv_client.download, SV_MAIN.sv_client.downloadcount - r, r );
			if ( SV_MAIN.sv_client.downloadcount != SV_MAIN.sv_client.downloadsize )
				return;
			FS.FreeFile( SV_MAIN.sv_client.download );
			SV_MAIN.sv_client.download = null;
		}

		public static void SV_BeginDownload_f( )
		{
			String name;
			var offset = 0;
			name = Cmd.Argv( 1 );
			if ( Cmd.Argc() > 2 )
				offset = Lib.Atoi( Cmd.Argv( 2 ) );
			if ( name.IndexOf( ".." ) != -1 || SV_MAIN.allow_download.value == 0 || name[0] == '.' || name[0] == '/' || ( name.StartsWith( "players/" ) && 0 == SV_MAIN.allow_download_players.value ) || ( name.StartsWith( "models/" ) && 0 == SV_MAIN.allow_download_models.value ) || ( name.StartsWith( "sound/" ) && 0 == SV_MAIN.allow_download_sounds.value ) || ( name.StartsWith( "maps/" ) && 0 == SV_MAIN.allow_download_maps.value ) || name.IndexOf( '/' ) == -1 )
			{
				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_download );
				MSG.WriteShort( SV_MAIN.sv_client.netchan.message, -1 );
				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, 0 );
				return;
			}

			if ( SV_MAIN.sv_client.download != null )
				FS.FreeFile( SV_MAIN.sv_client.download );
			SV_MAIN.sv_client.download = FS.LoadFile( name );
			if ( SV_MAIN.sv_client.download == null )
			{
				return;
			}

			SV_MAIN.sv_client.downloadsize = SV_MAIN.sv_client.download.Length;
			SV_MAIN.sv_client.downloadcount = offset;
			if ( offset > SV_MAIN.sv_client.downloadsize )
				SV_MAIN.sv_client.downloadcount = SV_MAIN.sv_client.downloadsize;
			if ( SV_MAIN.sv_client.download == null || ( name.StartsWith( "maps/" ) && FS.file_from_pak != 0 ) )
			{
				Com.DPrintf( "Couldn't download " + name + " to " + SV_MAIN.sv_client.name + "\\n" );
				if ( SV_MAIN.sv_client.download != null )
				{
					FS.FreeFile( SV_MAIN.sv_client.download );
					SV_MAIN.sv_client.download = null;
				}

				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, Defines.svc_download );
				MSG.WriteShort( SV_MAIN.sv_client.netchan.message, -1 );
				MSG.WriteByte( SV_MAIN.sv_client.netchan.message, 0 );
				return;
			}

			SV_NextDownload_f();
			Com.DPrintf( "Downloading " + name + " to " + SV_MAIN.sv_client.name + "\\n" );
		}

		public static void SV_Disconnect_f( )
		{
			SV_MAIN.SV_DropClient( SV_MAIN.sv_client );
		}

		public static void SV_ShowServerinfo_f( )
		{
			Info.Print( Cvar.Serverinfo() );
		}

		public static void SV_Nextserver( )
		{
			String v;
			if ( SV_INIT.sv.state == Defines.ss_game || ( SV_INIT.sv.state == Defines.ss_pic && 0 == Cvar.VariableValue( "coop" ) ) )
				return;
			SV_INIT.svs.spawncount++;
			v = Cvar.VariableString( "nextserver" );
			if ( v.Length == 0 )
				Cbuf.AddText( "killserver\\n" );
			else
			{
				Cbuf.AddText( v );
				Cbuf.AddText( "\\n" );
			}

			Cvar.Set( "nextserver", "" );
		}

		public static void SV_Nextserver_f( )
		{
			if ( Lib.Atoi( Cmd.Argv( 1 ) ) != SV_INIT.svs.spawncount )
			{
				Com.DPrintf( "Nextserver() from wrong level, from " + SV_MAIN.sv_client.name + "\\n" );
				return;
			}

			Com.DPrintf( "Nextserver() from " + SV_MAIN.sv_client.name + "\\n" );
			SV_Nextserver();
		}

		public static void SV_ExecuteUserCommand( String s )
		{
			Com.Dprintln( "SV_ExecuteUserCommand:" + s );
			SV_USER.ucmd_t u = null;
			Cmd.TokenizeString( s.ToCharArray(), true );
			SV_USER.sv_player = SV_MAIN.sv_client.edict;
			var i = 0;
			for ( ; i < SV_USER.ucmds.Length; i++ )
			{
				u = SV_USER.ucmds[i];
				if ( Cmd.Argv( 0 ).Equals( u.name ) )
				{
					u.r.Run();
					break;
				}
			}

			if ( i == SV_USER.ucmds.Length && SV_INIT.sv.state == Defines.ss_game )
				Cmd.ClientCommand( SV_USER.sv_player );
		}

		public static void SV_ClientThink( client_t cl, usercmd_t cmd )
		{
			cl.commandMsec -= cmd.msec & 0xFF;
			if ( cl.commandMsec < 0 && SV_MAIN.sv_enforcetime.value != 0 )
			{
				Com.DPrintf( "commandMsec underflow from " + cl.name + "\\n" );
				return;
			}

			PlayerClient.ClientThink( cl.edict, cmd );
		}

		public static void SV_ExecuteClientMessage( client_t cl )
		{
			Int32 c;
			String s;
			usercmd_t nullcmd = new usercmd_t();
			usercmd_t oldest = new usercmd_t(), oldcmd = new usercmd_t(), newcmd = new usercmd_t();
			Int32 net_drop;
			Int32 stringCmdCount;
			Int32 checksum, calculatedChecksum;
			Int32 checksumIndex;
			Boolean move_issued;
			Int32 lastframe;
			SV_MAIN.sv_client = cl;
			SV_USER.sv_player = SV_MAIN.sv_client.edict;
			move_issued = false;
			stringCmdCount = 0;
			while ( true )
			{
				if ( Globals.net_message.readcount > Globals.net_message.cursize )
				{
					Com.Printf( "SV_ReadClientMessage: bad read:\\n" );
					Com.Printf( Lib.HexDump( Globals.net_message.data, 32, false ) );
					SV_MAIN.SV_DropClient( cl );
					return;
				}

				c = MSG.ReadByte( Globals.net_message );
				if ( c == -1 )
					break;
				switch ( c )

				{
					default:
						Com.Printf( "SV_ReadClientMessage: unknown command char\\n" );
						SV_MAIN.SV_DropClient( cl );
						return;
					case Defines.clc_nop:
						break;
					case Defines.clc_userinfo:
						cl.userinfo = MSG.ReadString( Globals.net_message );
						SV_MAIN.SV_UserinfoChanged( cl );
						break;
					case Defines.clc_move:
						if ( move_issued )
							return;
						move_issued = true;
						checksumIndex = Globals.net_message.readcount;
						checksum = MSG.ReadByte( Globals.net_message );
						lastframe = MSG.ReadLong( Globals.net_message );
						if ( lastframe != cl.lastframe )
						{
							cl.lastframe = lastframe;
							if ( cl.lastframe > 0 )
							{
								cl.frame_latency[cl.lastframe & ( Defines.LATENCY_COUNTS - 1 )] = SV_INIT.svs.realtime - cl.frames[cl.lastframe & Defines.UPDATE_MASK].senttime;
							}
						}

						nullcmd = new usercmd_t();
						MSG.ReadDeltaUsercmd( Globals.net_message, nullcmd, oldest );
						MSG.ReadDeltaUsercmd( Globals.net_message, oldest, oldcmd );
						MSG.ReadDeltaUsercmd( Globals.net_message, oldcmd, newcmd );
						if ( cl.state != Defines.cs_spawned )
						{
							cl.lastframe = -1;
							break;
						}

						calculatedChecksum = Com.BlockSequenceCRCByte( Globals.net_message.data, checksumIndex + 1, Globals.net_message.readcount - checksumIndex - 1, cl.netchan.incoming_sequence );
						if ( ( calculatedChecksum & 0xff ) != checksum )
						{
							Com.DPrintf( "Failed command checksum for " + cl.name + " (" + calculatedChecksum + " != " + checksum + ")/" + cl.netchan.incoming_sequence + "\\n" );
							return;
						}

						if ( 0 == SV_MAIN.sv_paused.value )
						{
							net_drop = cl.netchan.dropped;
							if ( net_drop < 20 )
							{
								while ( net_drop > 2 )
								{
									SV_ClientThink( cl, cl.lastcmd );
									net_drop--;
								}

								if ( net_drop > 1 )
									SV_ClientThink( cl, oldest );
								if ( net_drop > 0 )
									SV_ClientThink( cl, oldcmd );
							}

							SV_ClientThink( cl, newcmd );
						}

						cl.lastcmd.Set( newcmd );
						break;
					case Defines.clc_stringcmd:
						s = MSG.ReadString( Globals.net_message );
						if ( ++stringCmdCount < SV_USER.MAX_STRINGCMDS )
							SV_ExecuteUserCommand( s );
						if ( cl.state == Defines.cs_zombie )
							return;
						break;
				}
			}
		}
	}
}
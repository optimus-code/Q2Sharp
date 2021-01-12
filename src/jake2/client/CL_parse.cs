using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Render;
using Jake2.Sound;
using Jake2.Sys;
using Jake2.Util;
using System;
using System.IO;

namespace Jake2.Client
{
	public class CL_parse
	{
		public static String[] svc_strings = new[] { "svc_bad", "svc_muzzleflash", "svc_muzzlflash2", "svc_temp_entity", "svc_layout", "svc_inventory", "svc_nop", "svc_disconnect", "svc_reconnect", "svc_sound", "svc_print", "svc_stufftext", "svc_serverdata", "svc_configstring", "svc_spawnbaseline", "svc_centerprint", "svc_download", "svc_playerinfo", "svc_packetentities", "svc_deltapacketentities", "svc_frame" };
		public static String DownloadFileName( String fn )
		{
			return FS.Gamedir() + "/" + fn;
		}

		public static Boolean CheckOrDownloadFile( String filename )
		{
			if ( filename.IndexOf( ".." ) != -1 )
			{
				Com.Printf( "Refusing to download a path with ..\\n" );
				return true;
			}

			if ( FS.FileLength( filename ) > 0 )
			{
				return true;
			}

			Globals.cls.downloadname = filename;
			Globals.cls.downloadtempname = Com.StripExtension( Globals.cls.downloadname );
			Globals.cls.downloadtempname += ".tmp";
			var name = DownloadFileName( Globals.cls.downloadtempname );
			QuakeFile fp = new QuakeFile( name, FileAccess.ReadWrite );
			if ( fp != null )
			{
				Int64 len = 0;
				try
				{
					len = fp.Length;
				}
				catch ( IOException e )
				{
				}

				Globals.cls.download = fp;
				Com.Printf( "Resuming " + Globals.cls.downloadname + "\\n" );
				MSG.WriteByte( Globals.cls.netchan.message, Defines.clc_stringcmd );
				MSG.WriteString( Globals.cls.netchan.message, "download " + Globals.cls.downloadname + " " + len );
			}
			else
			{
				Com.Printf( "Downloading " + Globals.cls.downloadname + "\\n" );
				MSG.WriteByte( Globals.cls.netchan.message, Defines.clc_stringcmd );
				MSG.WriteString( Globals.cls.netchan.message, "download " + Globals.cls.downloadname );
			}

			Globals.cls.downloadnumber++;
			return false;
		}

		public static xcommand_t Download_f = new Anonymousxcommand_t();
		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				String filename;
				if ( Cmd.Argc() != 2 )
				{
					Com.Printf( "Usage: download <filename>\\n" );
					return;
				}

				filename = Cmd.Argv( 1 );
				if ( filename.IndexOf( ".." ) != -1 )
				{
					Com.Printf( "Refusing to download a path with ..\\n" );
					return;
				}

				if ( FS.LoadFile( filename ) != null )
				{
					Com.Printf( "File already exists.\\n" );
					return;
				}

				Globals.cls.downloadname = filename;
				Com.Printf( "Downloading " + Globals.cls.downloadname + "\\n" );
				Globals.cls.downloadtempname = Com.StripExtension( Globals.cls.downloadname );
				Globals.cls.downloadtempname += ".tmp";
				MSG.WriteByte( Globals.cls.netchan.message, Defines.clc_stringcmd );
				MSG.WriteString( Globals.cls.netchan.message, "download " + Globals.cls.downloadname );
				Globals.cls.downloadnumber++;
			}
		}

		public static void RegisterSounds( )
		{
			S.BeginRegistration();
			CL_tent.RegisterTEntSounds();
			for ( var i = 1; i < Defines.MAX_SOUNDS; i++ )
			{
				if ( Globals.cl.configstrings[Defines.CS_SOUNDS + i] == null || Globals.cl.configstrings[Defines.CS_SOUNDS + i].Equals( "" ) || Globals.cl.configstrings[Defines.CS_SOUNDS + i].Equals( "\\0" ) )
					break;
				Globals.cl.sound_precache[i] = S.RegisterSound( Globals.cl.configstrings[Defines.CS_SOUNDS + i] );
				CoreSys.SendKeyEvents();
			}

			S.EndRegistration();
		}

		public static void ParseDownload( )
		{
			Int32 size = MSG.ReadShort( Globals.net_message );
			var percent = MSG.ReadByte( Globals.net_message );
			if ( size == -1 )
			{
				Com.Printf( "Server does not have this file.\\n" );
				if ( Globals.cls.download != null )
				{
					try
					{
						Globals.cls.download.Close();
					}
					catch ( IOException e )
					{
					}

					Globals.cls.download = null;
				}

				CL.RequestNextDownload();
				return;
			}

			if ( Globals.cls.download == null )
			{
				var name = DownloadFileName( Globals.cls.downloadtempname ).ToLower();
				FS.CreatePath( name );
				Globals.cls.download = new QuakeFile( name, FileAccess.ReadWrite );
				if ( Globals.cls.download == null )
				{
					Globals.net_message.readcount += size;
					Com.Printf( "Failed to open " + Globals.cls.downloadtempname + "\\n" );
					CL.RequestNextDownload();
					return;
				}
			}

			try
			{
				Globals.cls.download.Write( Globals.net_message.data, Globals.net_message.readcount, size );
			}
			catch ( Exception e )
			{
			}

			Globals.net_message.readcount += size;
			if ( percent != 100 )
			{
				Globals.cls.downloadpercent = percent;
				MSG.WriteByte( Globals.cls.netchan.message, Defines.clc_stringcmd );
				SZ.Print( Globals.cls.netchan.message, "nextdl" );
			}
			else
			{
				try
				{
					Globals.cls.download.Close();
				}
				catch ( IOException e )
				{
				}

				var oldn = DownloadFileName( Globals.cls.downloadtempname );
				var newn = DownloadFileName( Globals.cls.downloadname );
				var r = Lib.Rename( oldn, newn );
				if ( r != 0 )
					Com.Printf( "failed to rename.\\n" );
				Globals.cls.download = null;
				Globals.cls.downloadpercent = 0;
				CL.RequestNextDownload();
			}
		}

		public static void ParseServerData( )
		{
			Com.DPrintf( "ParseServerData():Serverdata packet received.\\n" );
			CL.ClearState();
			Globals.cls.state = Defines.ca_connected;
			var i = MSG.ReadLong( Globals.net_message );
			Globals.cls.serverProtocol = i;
			if ( Globals.server_state != 0 && Defines.PROTOCOL_VERSION == 34 )
			{
			}
			else if ( i != Defines.PROTOCOL_VERSION )
				Com.Error( Defines.ERR_DROP, "Server returned version " + i + ", not " + Defines.PROTOCOL_VERSION );
			Globals.cl.servercount = MSG.ReadLong( Globals.net_message );
			Globals.cl.attractloop = MSG.ReadByte( Globals.net_message ) != 0;
			var str = MSG.ReadString( Globals.net_message );
			Globals.cl.gamedir = str;
			Com.Dprintln( "gamedir=" + str );
			if ( str.Length > 0 && ( FS.fs_gamedirvar.string_renamed == null || FS.fs_gamedirvar.string_renamed.Length == 0 || FS.fs_gamedirvar.string_renamed.Equals( str ) ) || ( str.Length == 0 && ( FS.fs_gamedirvar.string_renamed != null || FS.fs_gamedirvar.string_renamed.Length == 0 ) ) )
				Cvar.Set( "game", str );
			Globals.cl.playernum = MSG.ReadShort( Globals.net_message );
			Com.Dprintln( "numplayers=" + Globals.cl.playernum );
			str = MSG.ReadString( Globals.net_message );
			Com.Dprintln( "levelname=" + str );
			if ( Globals.cl.playernum == -1 )
			{
				SCR.PlayCinematic( str );
			}
			else
			{
				Com.Printf( "Levelname:" + str + "\\n" );
				Globals.cl.refresh_prepped = false;
			}
		}

		public static void ParseBaseline( )
		{
			entity_state_t nullstate = new entity_state_t( null );
			Int32[] bits = new[] { 0 };
			var newnum = CL_ents.ParseEntityBits( bits );
			entity_state_t es = Globals.cl_entities[newnum].baseline;
			CL_ents.ParseDelta( nullstate, es, newnum, bits[0] );
		}

		public static void LoadClientinfo( clientinfo_t ci, String s )
		{
			String model_name, skin_name, model_filename, skin_filename, weapon_filename;
			ci.cinfo = s;
			ci.name = s;
			var t = s.IndexOf( '\\' );
			if ( t != -1 )
			{
				ci.name = s.Substring( 0, t );
				s = s.Substring( t + 1, s.Length );
			}

			if ( Globals.cl_noskins.value != 0 || s.Length == 0 )
			{
				model_filename = ( "players/male/tris.md2" );
				weapon_filename = ( "players/male/weapon.md2" );
				skin_filename = ( "players/male/grunt.pcx" );
				ci.iconname = ( "/players/male/grunt_i.pcx" );
				ci.model = Globals.re.RegisterModel( model_filename );
				ci.weaponmodel = new model_t[Defines.MAX_CLIENTWEAPONMODELS];
				ci.weaponmodel[0] = Globals.re.RegisterModel( weapon_filename );
				ci.skin = Globals.re.RegisterSkin( skin_filename );
				ci.icon = Globals.re.RegisterPic( ci.iconname );
			}
			else
			{
				var pos = s.IndexOf( '/' );
				if ( pos == -1 )
					pos = s.IndexOf( '/' );
				if ( pos == -1 )
				{
					pos = 0;
					Com.Error( Defines.ERR_FATAL, "Invalid model name:" + s );
				}

				model_name = s.Substring( 0, pos );
				skin_name = s.Substring( pos + 1, s.Length );
				model_filename = "players/" + model_name + "/tris.md2";
				ci.model = Globals.re.RegisterModel( model_filename );
				if ( ci.model == null )
				{
					model_name = "male";
					model_filename = "players/male/tris.md2";
					ci.model = Globals.re.RegisterModel( model_filename );
				}

				skin_filename = "players/" + model_name + "/" + skin_name + ".pcx";
				ci.skin = Globals.re.RegisterSkin( skin_filename );
				if ( ci.skin == null && !model_name.EqualsIgnoreCase( "male" ) )
				{
					model_name = "male";
					model_filename = "players/male/tris.md2";
					ci.model = Globals.re.RegisterModel( model_filename );
					skin_filename = "players/" + model_name + "/" + skin_name + ".pcx";
					ci.skin = Globals.re.RegisterSkin( skin_filename );
				}

				if ( ci.skin == null )
				{
					skin_filename = "players/" + model_name + "/grunt.pcx";
					ci.skin = Globals.re.RegisterSkin( skin_filename );
				}

				for ( var i = 0; i < CL_view.num_cl_weaponmodels; i++ )
				{
					weapon_filename = "players/" + model_name + "/" + CL_view.cl_weaponmodels[i];
					ci.weaponmodel[i] = Globals.re.RegisterModel( weapon_filename );
					if ( null == ci.weaponmodel[i] && model_name.Equals( "cyborg" ) )
					{
						weapon_filename = "players/male/" + CL_view.cl_weaponmodels[i];
						ci.weaponmodel[i] = Globals.re.RegisterModel( weapon_filename );
					}

					if ( 0 == Globals.cl_vwep.value )
						break;
				}

				ci.iconname = "/players/" + model_name + "/" + skin_name + "_i.pcx";
				ci.icon = Globals.re.RegisterPic( ci.iconname );
			}

			if ( ci.skin == null || ci.icon == null || ci.model == null || ci.weaponmodel[0] == null )
			{
				ci.skin = null;
				ci.icon = null;
				ci.model = null;
				ci.weaponmodel[0] = null;
				return;
			}
		}

		public static void ParseClientinfo( Int32 player )
		{
			var s = Globals.cl.configstrings[player + Defines.CS_PLAYERSKINS];
			clientinfo_t ci = Globals.cl.clientinfo[player];
			LoadClientinfo( ci, s );
		}

		public static void ParseConfigString( )
		{
			Int32 i = MSG.ReadShort( Globals.net_message );
			if ( i < 0 || i >= Defines.MAX_CONFIGSTRINGS )
				Com.Error( Defines.ERR_DROP, "configstring > MAX_CONFIGSTRINGS" );
			var s = MSG.ReadString( Globals.net_message );
			var olds = Globals.cl.configstrings[i];
			Globals.cl.configstrings[i] = s;
			if ( i >= Defines.CS_LIGHTS && i < Defines.CS_LIGHTS + Defines.MAX_LIGHTSTYLES )
			{
				CL_fx.SetLightstyle( i - Defines.CS_LIGHTS );
			}
			else if ( i >= Defines.CS_MODELS && i < Defines.CS_MODELS + Defines.MAX_MODELS )
			{
				if ( Globals.cl.refresh_prepped )
				{
					Globals.cl.model_draw[i - Defines.CS_MODELS] = Globals.re.RegisterModel( Globals.cl.configstrings[i] );
					if ( Globals.cl.configstrings[i].StartsWith( "*" ) )
						Globals.cl.model_clip[i - Defines.CS_MODELS] = CM.InlineModel( Globals.cl.configstrings[i] );
					else
						Globals.cl.model_clip[i - Defines.CS_MODELS] = null;
				}
			}
			else if ( i >= Defines.CS_SOUNDS && i < Defines.CS_SOUNDS + Defines.MAX_MODELS )
			{
				if ( Globals.cl.refresh_prepped )
					Globals.cl.sound_precache[i - Defines.CS_SOUNDS] = S.RegisterSound( Globals.cl.configstrings[i] );
			}
			else if ( i >= Defines.CS_IMAGES && i < Defines.CS_IMAGES + Defines.MAX_MODELS )
			{
				if ( Globals.cl.refresh_prepped )
					Globals.cl.image_precache[i - Defines.CS_IMAGES] = Globals.re.RegisterPic( Globals.cl.configstrings[i] );
			}
			else if ( i >= Defines.CS_PLAYERSKINS && i < Defines.CS_PLAYERSKINS + Defines.MAX_CLIENTS )
			{
				if ( Globals.cl.refresh_prepped && !olds.Equals( s ) )
					ParseClientinfo( i - Defines.CS_PLAYERSKINS );
			}
		}

		private static readonly Single[] pos_v = new Single[] { 0, 0, 0 };
		public static void ParseStartSoundPacket( )
		{
			var flags = MSG.ReadByte( Globals.net_message );
			var sound_num = MSG.ReadByte( Globals.net_message );
			Single volume;
			if ( ( flags & Defines.SND_VOLUME ) != 0 )
				volume = MSG.ReadByte( Globals.net_message ) / 255F;
			else
				volume = Defines.DEFAULT_SOUND_PACKET_VOLUME;
			Single attenuation;
			if ( ( flags & Defines.SND_ATTENUATION ) != 0 )
				attenuation = MSG.ReadByte( Globals.net_message ) / 64F;
			else
				attenuation = Defines.DEFAULT_SOUND_PACKET_ATTENUATION;
			Single ofs;
			if ( ( flags & Defines.SND_OFFSET ) != 0 )
				ofs = MSG.ReadByte( Globals.net_message ) / 1000F;
			else
				ofs = 0;
			Int32 channel;
			Int32 ent;
			if ( ( flags & Defines.SND_ENT ) != 0 )
			{
				channel = MSG.ReadShort( Globals.net_message );
				ent = channel >> 3;
				if ( ent > Defines.MAX_EDICTS )
					Com.Error( Defines.ERR_DROP, "CL_ParseStartSoundPacket: ent = " + ent );
				channel &= 7;
			}
			else
			{
				ent = 0;
				channel = 0;
			}

			Single[] pos;
			if ( ( flags & Defines.SND_POS ) != 0 )
			{
				MSG.ReadPos( Globals.net_message, pos_v );
				pos = pos_v;
			}
			else
				pos = null;
			if ( null == Globals.cl.sound_precache[sound_num] )
				return;
			S.StartSound( pos, ent, channel, Globals.cl.sound_precache[sound_num], volume, attenuation, ofs );
		}

		public static void SHOWNET( String s )
		{
			if ( Globals.cl_shownet.value >= 2 )
				Com.Printf( Globals.net_message.readcount - 1 + ":" + s + "\\n" );
		}

		public static void ParseServerMessage( )
		{
			while ( true )
			{
				if ( Globals.net_message.readcount > Globals.net_message.cursize )
				{
					Com.Error( Defines.ERR_FATAL, "CL_ParseServerMessage: Bad server message:" );
					break;
				}

				var cmd = MSG.ReadByte( Globals.net_message );
				if ( cmd == -1 )
				{
					SHOWNET( "END OF MESSAGE" );
					break;
				}

				if ( Globals.cl_shownet.value >= 2 )
				{
					if ( null == svc_strings[cmd] )
						Com.Printf( Globals.net_message.readcount - 1 + ":BAD CMD " + cmd + "\\n" );
					else
						SHOWNET( svc_strings[cmd] );
				}

				switch ( cmd )

				{
					default:
						Com.Error( Defines.ERR_DROP, "CL_ParseServerMessage: Illegible server message\\n" );
						break;
					case Defines.svc_nop:
						break;
					case Defines.svc_disconnect:
						Com.Error( Defines.ERR_DISCONNECT, "Server disconnected\\n" );
						break;
					case Defines.svc_reconnect:
						Com.Printf( "Server disconnected, reconnecting\\n" );
						if ( Globals.cls.download != null )
						{
							try
							{
								Globals.cls.download.Close();
							}
							catch ( IOException e )
							{
							}

							Globals.cls.download = null;
						}

						Globals.cls.state = Defines.ca_connecting;
						Globals.cls.connect_time = -99999;
						break;
					case Defines.svc_print:
						var i = MSG.ReadByte( Globals.net_message );
						if ( i == Defines.PRINT_CHAT )
						{
							S.StartLocalSound( "misc/talk.wav" );
							Globals.con.ormask = 128;
						}

						Com.Printf( MSG.ReadString( Globals.net_message ) );
						Globals.con.ormask = 0;
						break;
					case Defines.svc_centerprint:
						SCR.CenterPrint( MSG.ReadString( Globals.net_message ) );
						break;
					case Defines.svc_stufftext:
						var s = MSG.ReadString( Globals.net_message );
						Com.DPrintf( "stufftext: " + s + "\\n" );
						Cbuf.AddText( s );
						break;
					case Defines.svc_serverdata:
						Cbuf.Execute();
						ParseServerData();
						break;
					case Defines.svc_configstring:
						ParseConfigString();
						break;
					case Defines.svc_sound:
						ParseStartSoundPacket();
						break;
					case Defines.svc_spawnbaseline:
						ParseBaseline();
						break;
					case Defines.svc_temp_entity:
						CL_tent.ParseTEnt();
						break;
					case Defines.svc_muzzleflash:
						CL_fx.ParseMuzzleFlash();
						break;
					case Defines.svc_muzzleflash2:
						CL_fx.ParseMuzzleFlash2();
						break;
					case Defines.svc_download:
						ParseDownload();
						break;
					case Defines.svc_frame:
						CL_ents.ParseFrame();
						break;
					case Defines.svc_inventory:
						CL_inv.ParseInventory();
						break;
					case Defines.svc_layout:
						Globals.cl.layout = MSG.ReadString( Globals.net_message );
						break;
					case Defines.svc_playerinfo:
					case Defines.svc_packetentities:
					case Defines.svc_deltapacketentities:
						Com.Error( Defines.ERR_DROP, "Out of place frame data" );
						break;
				}
			}

			CL_view.AddNetgraph();
			if ( Globals.cls.demorecording && !Globals.cls.demowaiting )
				CL.WriteDemoMessage();
		}
	}
}
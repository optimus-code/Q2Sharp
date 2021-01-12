using J2N.IO;
using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.IO;

namespace Q2Sharp.Sound.Jsound
{
	public class SND_DMA : SND_MIX
	{
		static readonly Int32 SOUND_FULLVOLUME = 80;
		static readonly Single SOUND_LOOPATTENUATE = 0.003F;
		static Int32 s_registration_sequence;
		public static Boolean sound_started = false;
		static Single[] listener_origin = new Single[] { 0, 0, 0 };
		static Single[] listener_forward = new Single[] { 0, 0, 0 };
		static Single[] listener_right = new Single[] { 0, 0, 0 };
		static Single[] listener_up = new Single[] { 0, 0, 0 };
		static Boolean s_registering;
		static Int32 soundtime;
		static readonly Int32 MAX_SFX = ( MAX_SOUNDS * 2 );
		static sfx_t[] known_sfx = new sfx_t[MAX_SFX];
		static Int32 num_sfx;
		static readonly Int32 MAX_PLAYSOUNDS = 128;
		static playsound_t[] s_playsounds = new playsound_t[MAX_PLAYSOUNDS];

		static SND_DMA( )
		{
			for ( var i = 0; i < known_sfx.Length; i++ )
				known_sfx[i] = new sfx_t();

			for ( var i = 0; i < MAX_PLAYSOUNDS; i++ )
			{
				s_playsounds[i] = new playsound_t();
			}
		}

		public static playsound_t s_freeplays = new playsound_t();
		public static Int32 s_beginofs;
		public static cvar_t s_testsound;
		public static cvar_t s_loadas8bit;
		public static cvar_t s_khz;
		public static cvar_t s_show;
		public static cvar_t s_mixahead;
		public static cvar_t s_primary;
		static void SoundInfo_f( )
		{
			if ( !sound_started )
			{
				Com.Printf( "sound system not started\\n" );
				return;
			}

			Com.Printf( "%5d stereo\\n", dma.channels - 1 );
			Com.Printf( "%5d samples\\n", dma.samples );
			Com.Printf( "%5d samplebits\\n", dma.samplebits );
			Com.Printf( "%5d submission_chunk\\n", dma.submission_chunk );
			Com.Printf( "%5d speed\\n", dma.speed );
		}

		public static void Init( )
		{
			cvar_t cv;
			Com.Printf( "\\n------- sound initialization -------\\n" );
			cv = Cvar.Get( "s_initsound", "0", 0 );
			if ( cv.value == 0F )
				Com.Printf( "not initializing.\\n" );
			else
			{
				s_volume = Cvar.Get( "s_volume", "0.7", CVAR_ARCHIVE );
				s_khz = Cvar.Get( "s_khz", "11", CVAR_ARCHIVE );
				s_loadas8bit = Cvar.Get( "s_loadas8bit", "1", CVAR_ARCHIVE );
				s_mixahead = Cvar.Get( "s_mixahead", "0.2", CVAR_ARCHIVE );
				s_show = Cvar.Get( "s_show", "0", 0 );
				s_testsound = Cvar.Get( "s_testsound", "0", 0 );
				s_primary = Cvar.Get( "s_primary", "0", CVAR_ARCHIVE );
				Cmd.AddCommand( "play", new Anonymousxcommand_t() );
				Cmd.AddCommand( "stopsound", new Anonymousxcommand_t1() );
				Cmd.AddCommand( "soundlist", new Anonymousxcommand_t2() );
				Cmd.AddCommand( "soundinfo", new Anonymousxcommand_t3() );
				if ( !SNDDMA_Init() )
					return;
				InitScaletable();
				sound_started = true;
				num_sfx = 0;
				soundtime = 0;
				paintedtime = 0;
				Com.Printf( "sound sampling rate: " + dma.speed + "\\n" );
				StopAllSounds();
			}

			Com.Printf( "------------------------------------\\n" );
		}

		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				Play();
			}
		}

		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				StopAllSounds();
			}
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				SoundList();
			}
		}

		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public override void Execute( )
			{
				SoundInfo_f();
			}
		}

		public static void Shutdown( )
		{
			Int32 i;
			sfx_t[] sfx;
			if ( !sound_started )
				return;
			SNDDMA_Shutdown();
			sound_started = false;
			Cmd.RemoveCommand( "play" );
			Cmd.RemoveCommand( "stopsound" );
			Cmd.RemoveCommand( "soundlist" );
			Cmd.RemoveCommand( "soundinfo" );
			for ( i = 0, sfx = known_sfx; i < num_sfx; i++ )
			{
				if ( sfx[i].name == null )
					continue;
				sfx[i].Clear();
			}

			num_sfx = 0;
		}

		static sfx_t FindName( String name, Boolean create )
		{
			Int32 i;
			sfx_t sfx = null;
			if ( name == null )
				Com.Error( ERR_FATAL, "S_FindName: NULL\\n" );
			if ( name.Length == 0 )
				Com.Error( ERR_FATAL, "S_FindName: empty name\\n" );
			if ( name.Length >= MAX_QPATH )
				Com.Error( ERR_FATAL, "Sound name too long: " + name );
			for ( i = 0; i < num_sfx; i++ )
				if ( name.Equals( known_sfx[i].name ) )
				{
					return known_sfx[i];
				}

			if ( !create )
				return null;
			for ( i = 0; i < num_sfx; i++ )
				if ( known_sfx[i].name == null )
					break;
			if ( i == num_sfx )
			{
				if ( num_sfx == MAX_SFX )
					Com.Error( ERR_FATAL, "S_FindName: out of sfx_t" );
				num_sfx++;
			}

			sfx = known_sfx[i];
			sfx.Clear();
			sfx.name = name;
			sfx.registration_sequence = s_registration_sequence;
			return sfx;
		}

		static sfx_t AliasName( String aliasname, String truename )
		{
			sfx_t sfx = null;
			Int32 i;
			for ( i = 0; i < num_sfx; i++ )
				if ( known_sfx[i].name == null )
					break;
			if ( i == num_sfx )
			{
				if ( num_sfx == MAX_SFX )
					Com.Error( ERR_FATAL, "S_FindName: out of sfx_t" );
				num_sfx++;
			}

			sfx = known_sfx[i];
			sfx.name = aliasname;
			sfx.registration_sequence = s_registration_sequence;
			sfx.truename = truename;
			return sfx;
		}

		public static void BeginRegistration( )
		{
			s_registration_sequence++;
			s_registering = true;
		}

		public static sfx_t RegisterSound( String name )
		{
			sfx_t sfx = null;
			if ( !sound_started )
				return null;
			sfx = FindName( name, true );
			sfx.registration_sequence = s_registration_sequence;
			if ( !s_registering )
				WaveLoader.LoadSound( sfx );
			return sfx;
		}

		public static void EndRegistration( )
		{
			Int32 i;
			sfx_t sfx;
			Int32 size;
			for ( i = 0; i < num_sfx; i++ )
			{
				sfx = known_sfx[i];
				if ( sfx.name == null )
					continue;
				if ( sfx.registration_sequence != s_registration_sequence )
				{
					sfx.Clear();
				}
				else
				{
				}
			}

			for ( i = 0; i < num_sfx; i++ )
			{
				sfx = known_sfx[i];
				if ( sfx.name == null )
					continue;
				WaveLoader.LoadSound( sfx );
			}

			s_registering = false;
		}

		static channel_t PickChannel( Int32 entnum, Int32 entchannel )
		{
			Int32 ch_idx;
			Int32 first_to_die;
			Int32 life_left;
			channel_t ch;
			if ( entchannel < 0 )
				Com.Error( ERR_DROP, "S_PickChannel: entchannel<0" );
			first_to_die = -1;
			life_left = 0x7fffffff;
			for ( ch_idx = 0; ch_idx < MAX_CHANNELS; ch_idx++ )
			{
				if ( entchannel != 0 && channels[ch_idx].entnum == entnum && channels[ch_idx].entchannel == entchannel )
				{
					first_to_die = ch_idx;
					break;
				}

				if ( ( channels[ch_idx].entnum == cl.playernum + 1 ) && ( entnum != cl.playernum + 1 ) && channels[ch_idx].sfx != null )
					continue;
				if ( channels[ch_idx].end - paintedtime < life_left )
				{
					life_left = channels[ch_idx].end - paintedtime;
					first_to_die = ch_idx;
				}
			}

			if ( first_to_die == -1 )
				return null;
			ch = channels[first_to_die];
			ch.Clear();
			return ch;
		}

		static void SpatializeOrigin( Single[] origin, Single master_vol, Single dist_mult, channel_t ch )
		{
			Single dot;
			Single dist;
			Single lscale, rscale, scale;
			Single[] source_vec = new Single[] { 0, 0, 0 };
			if ( cls.state != ca_active )
			{
				ch.leftvol = ch.rightvol = 255;
				return;
			}

			Math3D.VectorSubtract( origin, listener_origin, source_vec );
			dist = Math3D.VectorNormalize( source_vec );
			dist -= SOUND_FULLVOLUME;
			if ( dist < 0 )
				dist = 0;
			dist *= dist_mult;
			dot = Math3D.DotProduct( listener_right, source_vec );
			if ( dma.channels == 1 || dist_mult == 0F )
			{
				rscale = 1F;
				lscale = 1F;
			}
			else
			{
				rscale = 0.5F * ( 1F + dot );
				lscale = 0.5F * ( 1F - dot );
			}

			scale = ( 1F - dist ) * rscale;
			ch.rightvol = ( Int32 ) ( master_vol * scale );
			if ( ch.rightvol < 0 )
				ch.rightvol = 0;
			scale = ( 1F - dist ) * lscale;
			ch.leftvol = ( Int32 ) ( master_vol * scale );
			if ( ch.leftvol < 0 )
				ch.leftvol = 0;
		}

		static void Spatialize( channel_t ch )
		{
			Single[] origin = new Single[] { 0, 0, 0 };
			if ( ch.entnum == cl.playernum + 1 )
			{
				ch.leftvol = ch.master_vol;
				ch.rightvol = ch.master_vol;
				return;
			}

			if ( ch.fixed_origin )
			{
				Math3D.VectorCopy( ch.origin, origin );
			}
			else
				CL_ents.GetEntitySoundOrigin( ch.entnum, origin );
			SpatializeOrigin( origin, ( Single ) ch.master_vol, ch.dist_mult, ch );
		}

		static playsound_t AllocPlaysound( )
		{
			playsound_t ps;
			ps = s_freeplays.next;
			if ( ps == s_freeplays )
				return null;
			ps.prev.next = ps.next;
			ps.next.prev = ps.prev;
			return ps;
		}

		static void FreePlaysound( playsound_t ps )
		{
			ps.prev.next = ps.next;
			ps.next.prev = ps.prev;
			ps.next = s_freeplays.next;
			s_freeplays.next.prev = ps;
			ps.prev = s_freeplays;
			s_freeplays.next = ps;
		}

		public static void IssuePlaysound( playsound_t ps )
		{
			channel_t ch;
			sfxcache_t sc;
			if ( s_show.value != 0F )
				Com.Printf( "Issue " + ps.begin + "\\n" );
			ch = PickChannel( ps.entnum, ps.entchannel );
			if ( ch == null )
			{
				FreePlaysound( ps );
				return;
			}

			if ( ps.attenuation == ATTN_STATIC )
				ch.dist_mult = ps.attenuation * 0.001F;
			else
				ch.dist_mult = ps.attenuation * 0.0005F;
			ch.master_vol = ( Int32 ) ps.volume;
			ch.entnum = ps.entnum;
			ch.entchannel = ps.entchannel;
			ch.sfx = ps.sfx;
			Math3D.VectorCopy( ps.origin, ch.origin );
			ch.fixed_origin = ps.fixed_origin;
			Spatialize( ch );
			ch.pos = 0;
			sc = WaveLoader.LoadSound( ch.sfx );
			ch.end = paintedtime + sc.length;
			FreePlaysound( ps );
		}

		static sfx_t RegisterSexedSound( entity_state_t ent, String base_renamed )
		{
			sfx_t sfx = null;
			var model = "male";
			var n = CS_PLAYERSKINS + ent.number - 1;
			if ( cl.configstrings[n] != null )
			{
				var p = cl.configstrings[n].IndexOf( '\\' );
				if ( p >= 0 )
				{
					p++;
					model = cl.configstrings[n].Substring( p );
					p = model.IndexOf( '/' );
					if ( p > 0 )
						model = model.Substring( 0, p - 1 );
				}
			}

			if ( model == null || model.Length == 0 )
				model = "male";
			var sexedFilename = "#players/" + model + "/" + base_renamed.Substring( 1 );
			sfx = FindName( sexedFilename, false );
			if ( sfx == null )
			{
				FileStream f = null;
				try
				{
					f = FS.FOpenFile( sexedFilename.Substring( 1 ) );
				}
				catch ( Exception e )
				{
				}

				if ( f != null )
				{
					try
					{
						FS.FCloseFile( f );
					}
					catch ( Exception e1 )
					{
					}

					sfx = RegisterSound( sexedFilename );
				}
				else
				{
					var maleFilename = "player/male/" + base_renamed.Substring( 1 );
					sfx = AliasName( sexedFilename, maleFilename );
				}
			}

			return sfx;
		}

		public static void StartSound( Single[] origin, Int32 entnum, Int32 entchannel, sfx_t sfx, Single fvol, Single attenuation, Single timeofs )
		{
			if ( !sound_started )
				return;
			if ( sfx == null )
				return;
			if ( sfx.name[0] == '*' )
				sfx = RegisterSexedSound( cl_entities[entnum].current, sfx.name );
			sfxcache_t sc = WaveLoader.LoadSound( sfx );
			if ( sc == null )
				return;
			var vol = ( Int32 ) ( fvol * 255 );
			playsound_t ps = AllocPlaysound();
			if ( ps == null )
				return;
			if ( origin != null )
			{
				Math3D.VectorCopy( origin, ps.origin );
				ps.fixed_origin = true;
			}
			else
				ps.fixed_origin = false;
			ps.entnum = entnum;
			ps.entchannel = entchannel;
			ps.attenuation = attenuation;
			ps.volume = vol;
			ps.sfx = sfx;
			var start = ( Int32 ) ( cl.frame.servertime * 0.001F * dma.speed + s_beginofs );
			if ( start < paintedtime )
			{
				start = paintedtime;
				s_beginofs = ( Int32 ) ( start - ( cl.frame.servertime * 0.001F * dma.speed ) );
			}
			else if ( start > paintedtime + 0.3F * dma.speed )
			{
				start = ( Int32 ) ( paintedtime + 0.1F * dma.speed );
				s_beginofs = ( Int32 ) ( start - ( cl.frame.servertime * 0.001F * dma.speed ) );
			}
			else
			{
				s_beginofs -= 10;
			}

			if ( timeofs == 0F )
				ps.begin = paintedtime;
			else
				ps.begin = ( Int64 ) ( start + timeofs * dma.speed );
			playsound_t sort = new playsound_t();
			ps.next = sort;
			ps.prev = sort.prev;
			ps.next.prev = ps;
			ps.prev.next = ps;
		}

		public static void StartLocalSound( String sound )
		{
			sfx_t sfx;
			if ( !sound_started )
				return;
			sfx = RegisterSound( sound );
			if ( sfx == null )
			{
				Com.Printf( "S_StartLocalSound: can't cache " + sound + "\\n" );
				return;
			}

			StartSound( null, cl.playernum + 1, 0, sfx, 1, 1, 0 );
		}

		static void ClearBuffer( )
		{
			Int32 clear;
			if ( !sound_started )
				return;
			s_rawend = 0;
			if ( dma.samplebits == 8 )
				clear = 0x80;
			else
				clear = 0;
			SNDDMA_BeginPainting();
			if ( dma.buffer != null )
				SNDDMA_Submit();
		}

		public static void StopAllSounds( )
		{
			Int32 i;
			if ( !sound_started )
				return;
			s_freeplays.next = s_freeplays.prev = s_freeplays;
			s_pendingplays.next = s_pendingplays.prev = s_pendingplays;
			for ( i = 0; i < MAX_PLAYSOUNDS; i++ )
			{
				s_playsounds[i].Clear();
				s_playsounds[i].prev = s_freeplays;
				s_playsounds[i].next = s_freeplays.next;
				s_playsounds[i].prev.next = s_playsounds[i];
				s_playsounds[i].next.prev = s_playsounds[i];
			}

			for ( i = 0; i < MAX_CHANNELS; i++ )
				channels[i].Clear();
			ClearBuffer();
		}

		static void AddLoopSounds( )
		{
			Int32 i, j;
			Int32[] sounds = new Int32[Defines.MAX_EDICTS];
			Int32 left, right, left_total, right_total;
			channel_t ch;
			sfx_t sfx;
			sfxcache_t sc;
			Int32 num;
			entity_state_t ent;
			if ( cl_paused.value != 0F )
				return;
			if ( cls.state != ca_active )
				return;
			if ( !cl.sound_prepped )
				return;
			for ( i = 0; i < cl.frame.num_entities; i++ )
			{
				num = ( cl.frame.parse_entities + i ) & ( MAX_PARSE_ENTITIES - 1 );
				ent = cl_parse_entities[num];
				sounds[i] = ent.sound;
			}

			for ( i = 0; i < cl.frame.num_entities; i++ )
			{
				if ( sounds[i] == 0 )
					continue;
				sfx = cl.sound_precache[sounds[i]];
				if ( sfx == null )
					continue;
				sc = sfx.cache;
				if ( sc == null )
					continue;
				num = ( cl.frame.parse_entities + i ) & ( MAX_PARSE_ENTITIES - 1 );
				ent = cl_parse_entities[num];
				channel_t tch = new channel_t();
				SpatializeOrigin( ent.origin, 255F, SOUND_LOOPATTENUATE, tch );
				left_total = tch.leftvol;
				right_total = tch.rightvol;
				for ( j = i + 1; j < cl.frame.num_entities; j++ )
				{
					if ( sounds[j] != sounds[i] )
						continue;
					sounds[j] = 0;
					num = ( cl.frame.parse_entities + j ) & ( MAX_PARSE_ENTITIES - 1 );
					ent = cl_parse_entities[num];
					SpatializeOrigin( ent.origin, 255F, SOUND_LOOPATTENUATE, tch );
					left_total += tch.leftvol;
					right_total += tch.rightvol;
				}

				if ( left_total == 0 && right_total == 0 )
					continue;
				ch = PickChannel( 0, 0 );
				if ( ch == null )
					return;
				if ( left_total > 255 )
					left_total = 255;
				if ( right_total > 255 )
					right_total = 255;
				ch.leftvol = left_total;
				ch.rightvol = right_total;
				ch.autosound = true;
				ch.sfx = sfx;
				ch.pos = paintedtime % sc.length;
				ch.end = paintedtime + sc.length - ch.pos;
			}
		}

		public static void RawSamples( Int32 samples, Int32 rate, Int32 width, Int32 channels, ByteBuffer data )
		{
			Int32 i;
			Int32 src, dst;
			Single scale;
			if ( !sound_started )
				return;
			if ( s_rawend < paintedtime )
				s_rawend = paintedtime;
			scale = ( Single ) rate / dma.speed;
			if ( channels == 2 && width == 2 )
			{
				if ( scale == 1 )
				{
				}
				else
				{
					for ( i = 0; ; i++ )
					{
					}
				}
			}
			else if ( channels == 1 && width == 2 )
			{
				for ( i = 0; ; i++ )
				{
				}
			}
			else if ( channels == 2 && width == 1 )
			{
				for ( i = 0; ; i++ )
				{
				}
			}
			else if ( channels == 1 && width == 1 )
			{
				for ( i = 0; ; i++ )
				{
				}
			}
		}

		public static void Update( Single[] origin, Single[] forward, Single[] right, Single[] up )
		{
			if ( !sound_started )
				return;
			if ( cls.disable_screen != 0F )
			{
				ClearBuffer();
				return;
			}

			if ( s_volume.modified )
				InitScaletable();
			Math3D.VectorCopy( origin, listener_origin );
			Math3D.VectorCopy( forward, listener_forward );
			Math3D.VectorCopy( right, listener_right );
			Math3D.VectorCopy( up, listener_up );
			channel_t combine = null;
			channel_t ch;
			for ( var i = 0; i < MAX_CHANNELS; i++ )
			{
				ch = channels[i];
				if ( ch.sfx == null )
					continue;
				if ( ch.autosound )
				{
					ch.Clear();
					continue;
				}

				Spatialize( ch );
				if ( ch.leftvol == 0 && ch.rightvol == 0 )
				{
					ch.Clear();
					continue;
				}
			}

			AddLoopSounds();
			if ( s_show.value != 0F )
			{
				var total = 0;
				for ( var i = 0; i < MAX_CHANNELS; i++ )
				{
					ch = channels[i];
					if ( ch.sfx != null && ( ch.leftvol != 0 || ch.rightvol != 0 ) )
					{
						Com.Printf( ch.leftvol + " " + ch.rightvol + " " + ch.sfx.name + "\\n" );
						total++;
					}
				}
			}

			Update_();
		}

		static Int32 buffers = 0;
		static Int32 oldsamplepos = 0;
		static void GetSoundtime( )
		{
			Int32 samplepos;
			Int32 fullsamples;
			fullsamples = dma.samples / dma.channels;
			samplepos = SNDDMA_GetDMAPos();
			if ( samplepos < oldsamplepos )
			{
				buffers++;
				if ( paintedtime > 0x40000000 )
				{
					buffers = 0;
					paintedtime = fullsamples;
					StopAllSounds();
				}
			}

			oldsamplepos = samplepos;
			soundtime = buffers * fullsamples + samplepos / dma.channels;
		}

		static void Update_( )
		{
			Int32 endtime;
			Int32 samps;
			if ( !sound_started )
				return;
			SNDDMA_BeginPainting();
			if ( dma.buffer == null )
				return;
			GetSoundtime();
			if ( paintedtime < soundtime )
			{
				Com.DPrintf( "S_Update_ : overflow\\n" );
				paintedtime = soundtime;
			}

			endtime = ( Int32 ) ( soundtime + s_mixahead.value * dma.speed );
			endtime = ( endtime + dma.submission_chunk - 1 ) & ~( dma.submission_chunk - 1 );
			samps = dma.samples >> ( dma.channels - 1 );
			if ( endtime - soundtime > samps )
				endtime = soundtime + samps;
			PaintChannels( endtime );
			SNDDMA_Submit();
		}

		static void Play( )
		{
			Int32 i;
			String name;
			sfx_t sfx;
			i = 1;
			while ( i < Cmd.Argc() )
			{
				name = new String( Cmd.Argv( i ) );
				if ( name.IndexOf( '.' ) == -1 )
					name += ".wav";
				sfx = RegisterSound( name );
				StartSound( null, cl.playernum + 1, 0, sfx, 1F, 1F, 0F );
				i++;
			}
		}

		static void SoundList( )
		{
			Int32 i;
			sfx_t sfx;
			sfxcache_t sc;
			Int32 size, total;
			total = 0;
			for ( i = 0; i < num_sfx; i++ )
			{
				sfx = known_sfx[i];
				if ( sfx.registration_sequence == 0 )
					continue;
				sc = sfx.cache;
				if ( sc != null )
				{
					size = sc.length * sc.width * ( sc.stereo + 1 );
					total += size;
					if ( sc.loopstart >= 0 )
						Com.Printf( "L" );
					else
						Com.Printf( " " );
					Com.Printf( "(%2db) %6i : %s\\n", sc.width * 8, size, sfx.name );
				}
				else
				{
					if ( sfx.name[0] == '*' )
						Com.Printf( "  placeholder : " + sfx.name + "\\n" );
					else
						Com.Printf( "  not loaded  : " + sfx.name + "\\n" );
				}
			}

			Com.Printf( "Total resident: " + total + "\\n" );
		}
	}
}
using J2N.IO;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using OpenTK.Audio.OpenAL;
using Q2Sharp.util;
using System;

namespace Q2Sharp.Sound.Joal
{
	public sealed class JOALSoundImpl : ISound
	{
		public const Int32 MAX_SFX = Defines.MAX_SOUNDS * 2;
		public const Int32 STREAM_QUEUE = 8;

		static sfx_t[] known_sfx = new sfx_t[MAX_SFX];

		static JOALSoundImpl( )
		{
			S.Register( new JOALSoundImpl() );
			for ( var i = 0; i < known_sfx.Length; i++ )
				known_sfx[i] = new sfx_t();
		}

		static AL al;
		static ALC alc;
		//static EAX eax;
		cvar_t s_volume;
		private Int32[] buffers = new Int32[MAX_SFX + STREAM_QUEUE];
		private Boolean initialized;
		private JOALSoundImpl( )
		{
			this.initialized = false;
			var sliced = sfxDataBuffer.Slice();
			sliced.Order = ByteOrder.BigEndian;
			streamBuffer = sliced.AsInt16Buffer();
		}

		public Boolean Init( )
		{
			try
			{
				if ( !initialized )
				{
					//ALut.AlutInit();
					initialized = true;
				}
				// Do we need this?
				//al = ALFactory.GetAL();
				//alc = ALFactory.GetALC();
				CheckError();
				InitOpenALExtensions();
			}
			catch ( Exception e )
			{
				Com.Printf( e.Message + '\\' );
				return false;
			}

			s_volume = Cvar.Get( "s_volume", "0.7", Defines.CVAR_ARCHIVE );
			AL.GenBuffers( buffers.Length, buffers );
			var count = Channel.Init( buffers );
			Com.Printf( "... using " + count + " channels\\n" );
			AL.DistanceModel( ALDistanceModel.InverseDistanceClamped );
			Cmd.AddCommand( "play", new Anonymousxcommand_t( this ) );
			Cmd.AddCommand( "stopsound", new Anonymousxcommand_t1( this ) );
			Cmd.AddCommand( "soundlist", new Anonymousxcommand_t2( this ) );
			Cmd.AddCommand( "soundinfo", new Anonymousxcommand_t3( this ) );
			num_sfx = 0;
			Com.Println( "sound sampling rate: 44100Hz" );
			StopAllSounds();
			Com.Println( "------------------------------------" );
			return true;
		}

		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public Anonymousxcommand_t( JOALSoundImpl parent )
			{
				this.parent = parent;
			}

			private readonly JOALSoundImpl parent;
			public override void Execute( )
			{
				parent.Play();
			}
		}

		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public Anonymousxcommand_t1( JOALSoundImpl parent )
			{
				this.parent = parent;
			}

			private readonly JOALSoundImpl parent;
			public override void Execute( )
			{
				parent.StopAllSounds();
			}
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public Anonymousxcommand_t2( JOALSoundImpl parent )
			{
				this.parent = parent;
			}

			private readonly JOALSoundImpl parent;
			public override void Execute( )
			{
				parent.SoundList();
			}
		}

		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public Anonymousxcommand_t3( JOALSoundImpl parent )
			{
				this.parent = parent;
			}

			private readonly JOALSoundImpl parent;
			public override void Execute( )
			{
				parent.SoundInfo_f();
			}
		}

		private void InitOpenALExtensions( )
		{
#if EAX
            if (AL.IsExtensionPresent("EAX2.0"))
            {
                try
                {
                    eax = EAXFactory.GetEAX();
                    Com.Println("... using EAX2.0");
                }
                catch (Exception e)
                {
                    Com.Println(e.Message);
                    Com.Println("... EAX2.0 not initialized");
                    eax = null;
                }
            }
            else
            {
                Com.Println("... EAX2.0 not found");
                eax = null;
            }
#endif
		}

		private ByteBuffer sfxDataBuffer = Lib.NewByteBuffer( 2 * 1024 * 1024 );
		private void InitBuffer( Byte[] samples, Int32 bufferId, Int32 freq )
		{
			ByteBuffer data = sfxDataBuffer.Slice();
			data.Put( samples ).Flip();
			new Pinnable( data, ( ptr ) =>
			{
				AL.BufferData( buffers[bufferId], ALFormat.Mono16, ptr, data.Limit, freq );
			} );
		}

		private void CheckError( )
		{
			Com.DPrintf( "AL Error: " + AlErrorString() + '\\' );
		}

		private String AlErrorString( )
		{
			ALError error;
			var message = "";
			if ( ( error = AL.GetError() ) != ALError.NoError )
			{
				switch ( error )

				{
					case ALError.InvalidOperation:
						message = "invalid operation";
						break;
					case ALError.InvalidValue:
						message = "invalid value";
						break;
					case ALError.InvalidEnum:
						message = "invalid enum";
						break;
					case ALError.InvalidName:
						message = "invalid name";
						break;
					default:
						message = "" + error;
						break;
				}
			}

			return message;
		}

		public void Shutdown( )
		{
			StopAllSounds();
			Channel.Shutdown();
			AL.DeleteBuffers( buffers );
			Cmd.RemoveCommand( "play" );
			Cmd.RemoveCommand( "stopsound" );
			Cmd.RemoveCommand( "soundlist" );
			Cmd.RemoveCommand( "soundinfo" );
			for ( var i = 0; i < num_sfx; i++ )
			{
				if ( known_sfx[i].name == null )
					continue;
				known_sfx[i].Clear();
			}

			num_sfx = 0;
		}

		public void StartSound( Single[] origin, Int32 entnum, Int32 entchannel, sfx_t sfx, Single fvol, Single attenuation, Single timeofs )
		{
			if ( sfx == null )
				return;
			if ( sfx.name[0] == '*' )
				sfx = RegisterSexedSound( Globals.cl_entities[entnum].current, sfx.name );
			if ( LoadSound( sfx ) == null )
				return;
			if ( attenuation != Defines.ATTN_STATIC )
				attenuation *= 0.5F;
			PlaySound.Allocate( origin, entnum, entchannel, buffers[sfx.bufferId], fvol, attenuation, timeofs );
		}

		private Single[] listenerOrigin = new Single[] { 0, 0, 0 };
		private Single[] listenerOrientation = new Single[] { 0, 0, 0, 0, 0, 0 };
		private Int32Buffer eaxEnv = Lib.NewInt32Buffer( 1 );
		private Int32 currentEnv = -1;
		private Boolean changeEnv = true;
		public void Update( Single[] origin, Single[] forward, Single[] right, Single[] up )
		{
			Channel.ConvertVector( origin, listenerOrigin );
			AL.Listener( ALListener3f.Position, listenerOrigin[0], listenerOrigin[1], listenerOrigin[2] );
			Channel.ConvertOrientation( forward, up, listenerOrientation );
			AL.Listener( ALListenerfv.Orientation, listenerOrientation );
			AL.Listener( ALListenerf.Gain, s_volume.value );

#if EAX
            if (eax != null)
            {
                if (currentEnv == -1)
                {
                    eaxEnv.Put(0, EAX.EAX_ENVIRONMENT_UNDERWATER);
                    eax.EAXSet(EAX.LISTENER, EAX.DSPROPERTY_EAXLISTENER_ENVIRONMENT | EAX.DSPROPERTY_EAXLISTENER_DEFERRED, 0, eaxEnv, 4);
                    changeEnv = true;
                }

                if ((GameBase.gi.pointcontents.Pointcontents(origin) & Defines.MASK_WATER) != 0)
                {
                    changeEnv = currentEnv != EAX.EAX_ENVIRONMENT_UNDERWATER;
                    currentEnv = EAX.EAX_ENVIRONMENT_UNDERWATER;
                }
                else
                {
                    changeEnv = currentEnv != EAX.EAX_ENVIRONMENT_GENERIC;
                    currentEnv = EAX.EAX_ENVIRONMENT_GENERIC;
                }

                if (changeEnv)
                {
                    eaxEnv.Put(0, currentEnv);
                    eax.EAXSet(EAX.LISTENER, EAX.DSPROPERTY_EAXLISTENER_ENVIRONMENT | EAX.DSPROPERTY_EAXLISTENER_DEFERRED, 0, eaxEnv, 4);
                }
            }
#endif
			Channel.AddLoopSounds();
			Channel.AddPlaySounds();
			Channel.PlayAllSounds( listenerOrigin );
		}

		public void StopAllSounds( )
		{
			AL.Listener( ALListenerf.Gain, 0 );
			PlaySound.Reset();
			Channel.Reset();
		}

		public String GetName( )
		{
			return "joal";
		}

		Int32 s_registration_sequence;
		Boolean s_registering;
		public void BeginRegistration( )
		{
			s_registration_sequence++;
			s_registering = true;
		}

		public sfx_t RegisterSound( String name )
		{
			sfx_t sfx = FindName( name, true );
			sfx.registration_sequence = s_registration_sequence;
			if ( !s_registering )
				LoadSound( sfx );
			return sfx;
		}

		public void EndRegistration( )
		{
			Int32 i;
			sfx_t sfx;
			for ( i = 0; i < num_sfx; i++ )
			{
				sfx = known_sfx[i];
				if ( sfx.name == null )
					continue;
				if ( sfx.registration_sequence != s_registration_sequence )
				{
					sfx.Clear();
				}
			}

			for ( i = 0; i < num_sfx; i++ )
			{
				sfx = known_sfx[i];
				if ( sfx.name == null )
					continue;
				LoadSound( sfx );
			}

			s_registering = false;
		}

		sfx_t RegisterSexedSound( entity_state_t ent, String base_renamed )
		{
			sfx_t sfx = null;
			String model = null;
			var n = Globals.CS_PLAYERSKINS + ent.number - 1;
			if ( Globals.cl.configstrings[n] != null )
			{
				var p = Globals.cl.configstrings[n].IndexOf( '\\' );
				if ( p >= 0 )
				{
					p++;
					model = Globals.cl.configstrings[n].Substring( p );
					p = model.IndexOf( '/' );
					if ( p > 0 )
						model = model.Substring( 0, p );
				}
			}

			if ( model == null || model.Length == 0 )
				model = "male";
			var sexedFilename = "#players/" + model + "/" + base_renamed.Substring( 1 );
			sfx = FindName( sexedFilename, false );
			if ( sfx != null )
				return sfx;
			if ( FS.FileLength( sexedFilename.Substring( 1 ) ) > 0 )
			{
				return RegisterSound( sexedFilename );
			}

			if ( model.EqualsIgnoreCase( "female" ) )
			{
				var femaleFilename = "player/female/" + base_renamed.Substring( 1 );
				if ( FS.FileLength( "sound/" + femaleFilename ) > 0 )
					return AliasName( sexedFilename, femaleFilename );
			}

			var maleFilename = "player/male/" + base_renamed.Substring( 1 );
			return AliasName( sexedFilename, maleFilename );
		}

		static Int32 num_sfx;
		sfx_t FindName( String name, Boolean create )
		{
			Int32 i;
			sfx_t sfx = null;
			if ( name == null )
				Com.Error( Defines.ERR_FATAL, "S_FindName: NULL\\n" );
			if ( name.Length == 0 )
				Com.Error( Defines.ERR_FATAL, "S_FindName: empty name\\n" );
			if ( name.Length >= Defines.MAX_QPATH )
				Com.Error( Defines.ERR_FATAL, "Sound name too long: " + name );
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
					Com.Error( Defines.ERR_FATAL, "S_FindName: out of sfx_t" );
				num_sfx++;
			}

			sfx = known_sfx[i];
			sfx.Clear();
			sfx.name = name;
			sfx.registration_sequence = s_registration_sequence;
			sfx.bufferId = i;
			return sfx;
		}

		sfx_t AliasName( String aliasname, String truename )
		{
			sfx_t sfx = null;
			String s;
			Int32 i;
			s = new String( truename );
			for ( i = 0; i < num_sfx; i++ )
				if ( known_sfx[i].name == null )
					break;
			if ( i == num_sfx )
			{
				if ( num_sfx == MAX_SFX )
					Com.Error( Defines.ERR_FATAL, "S_FindName: out of sfx_t" );
				num_sfx++;
			}

			sfx = known_sfx[i];
			sfx.Clear();
			sfx.name = new String( aliasname );
			sfx.registration_sequence = s_registration_sequence;
			sfx.truename = s;
			sfx.bufferId = i;
			return sfx;
		}

		public sfxcache_t LoadSound( sfx_t s )
		{
			if ( s.isCached )
				return s.cache;
			sfxcache_t sc = WaveLoader.LoadSound( s );
			if ( sc != null )
			{
				InitBuffer( sc.data, s.bufferId, sc.speed );
				s.isCached = true;
				s.cache.data = null;
			}

			return sc;
		}

		public void StartLocalSound( String sound )
		{
			sfx_t sfx = RegisterSound( sound );
			if ( sfx == null )
			{
				Com.Printf( "S_StartLocalSound: can't cache " + sound + "\\n" );
				return;
			}

			StartSound( null, Globals.cl.playernum + 1, 0, sfx, 1, 1, 0F );
		}

		private Int16Buffer streamBuffer;
		public void RawSamples( Int32 samples, Int32 rate, Int32 width, Int32 channels, ByteBuffer data )
		{
			ALFormat format;
			if ( channels == 2 )
			{
				format = ( width == 2 ) ? ALFormat.Stereo16 : ALFormat.Stereo8;
			}
			else
			{
				format = ( width == 2 ) ? ALFormat.Mono16 : ALFormat.Mono8;
			}

			if ( format == ALFormat.Mono8 )
			{
				Int16Buffer sampleData = streamBuffer;
				Int32 value;
				for ( var i = 0; i < samples; i++ )
				{
					value = ( data.Get( i ) & 0xFF ) - 128;
					sampleData.Put( i, ( Int16 ) value );
				}

				format = ALFormat.Mono16;
				width = 2;
				data = sfxDataBuffer.Slice();
			}

			Channel.UpdateStream( data, samples * channels * width, format, rate );
		}

		public void DisableStreaming( )
		{
			Channel.DisableStreaming();
		}

		void Play( )
		{
			var i = 1;
			String name;
			while ( i < Cmd.Argc() )
			{
				name = new String( Cmd.Argv( i ) );
				if ( name.IndexOf( '.' ) == -1 )
					name += ".wav";
				RegisterSound( name );
				StartLocalSound( name );
				i++;
			}
		}

		void SoundList( )
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

		void SoundInfo_f( )
		{
			Com.Printf( "%5d stereo\\n", 1 );
			Com.Printf( "%5d samples\\n", 22050 );
			Com.Printf( "%5d samplebits\\n", 16 );
			Com.Printf( "%5d speed\\n", 44100 );
		}
	}
}
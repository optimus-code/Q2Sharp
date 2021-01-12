using J2N.IO;
using Jake2.Game;
using Jake2.Qcommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jake2.Sound
{
	public class S
	{
		static ISound impl;
		static cvar_t s_impl;
		static List<ISound> drivers = new List<ISound>( 3 );
		static S( )
		{
			//try
			//{
			//	Class.ForName( "jake2.sound.DummyDriver" );
			//	UseDriver( "dummy" );
			//}
			//catch ( Exception e )
			//{
			//	Com.DPrintf( "could not init dummy sound driver class." );
			//}

			//try
			//{
			//	Class.ForName( "org.lwjgl.openal.AL" );
			//	Class.ForName( "jake2.sound.lwjgl.LWJGLSoundImpl" );
			//}
			//catch ( Exception e )
			//{
			//	Com.DPrintf( "could not init lwjgl sound driver class." );
			//}

			//try
			//{
			//	Class.ForName( "net.java.games.joal.AL" );
			//	Class.ForName( "jake2.sound.joal.JOALSoundImpl" );
			//}
			//catch ( Exception e )
			//{
			//	Com.DPrintf( "could not init joal sound driver class." );
			//}
		}

		public static void Register( ISound driver )
		{
			if ( driver == null )
			{
				throw new ArgumentException( "Sound implementation can't be null" );
			}

			if ( !drivers.Contains( driver ) )
			{
				drivers.Add( driver );
			}
		}

		public static void UseDriver( String driverName )
		{
			ISound driver = null;
			var count = drivers.Count;
			for ( var i = 0; i < count; i++ )
			{
				driver = ( ISound ) drivers[i];
				if ( driver.GetName().Equals( driverName ) )
				{
					impl = driver;
					return;
				}
			}

			impl = ( ISound ) drivers.Last();
		}

		public static void Init( )
		{
			Com.Printf( "\\n------- sound initialization -------\\n" );
			cvar_t cv = Cvar.Get( "s_initsound", "1", 0 );
			if ( cv.value == 0F )
			{
				Com.Printf( "not initializing.\\n" );
				UseDriver( "dummy" );
				return;
			}

			var defaultDriver = "dummy";
			if ( drivers.Count > 1 )
			{
				defaultDriver = ( ( ISound ) drivers.Last() ).GetName();
			}

			s_impl = Cvar.Get( "s_impl", defaultDriver, Defines.CVAR_ARCHIVE );
			UseDriver( s_impl.string_renamed );
			if ( impl.Init() )
			{
				Cvar.Set( "s_impl", impl.GetName() );
			}
			else
			{
				UseDriver( "dummy" );
			}

			Com.Printf( "\\n------- use sound driver \\\"" + impl.GetName() + "\\\" -------\\n" );
			StopAllSounds();
		}

		public static void Shutdown( )
		{
			impl.Shutdown();
		}

		public static void BeginRegistration( )
		{
			impl.BeginRegistration();
		}

		public static sfx_t RegisterSound( String sample )
		{
			return impl.RegisterSound( sample );
		}

		public static void EndRegistration( )
		{
			impl.EndRegistration();
		}

		public static void StartLocalSound( String sound )
		{
			impl.StartLocalSound( sound );
		}

		public static void StartSound( Single[] origin, Int32 entnum, Int32 entchannel, sfx_t sfx, Single fvol, Single attenuation, Single timeofs )
		{
			impl.StartSound( origin, entnum, entchannel, sfx, fvol, attenuation, timeofs );
		}

		public static void Update( Single[] origin, Single[] forward, Single[] right, Single[] up )
		{
			impl.Update( origin, forward, right, up );
		}

		public static void RawSamples( Int32 samples, Int32 rate, Int32 width, Int32 channels, ByteBuffer data )
		{
			impl.RawSamples( samples, rate, width, channels, data );
		}

		public static void DisableStreaming( )
		{
			impl.DisableStreaming();
		}

		public static void StopAllSounds( )
		{
			impl.StopAllSounds();
		}

		public static String GetDriverName( )
		{
			return impl.GetName();
		}

		public static String[] GetDriverNames( )
		{
			String[] names = new String[drivers.Count];
			for ( var i = 0; i < names.Length; i++ )
			{
				names[i] = ( ( ISound ) drivers[i] ).GetName();
			}

			return names;
		}

		public static Int32 GetDefaultSampleRate( )
		{
			return 44100;
		}
	}
}
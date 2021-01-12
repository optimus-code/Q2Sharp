using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Server;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;

namespace Q2Sharp.Qcommon
{
	public sealed class Qcom : Globals
	{
		public static readonly String BUILDSTRING = "C# " + GetTargetFramework();
		public static readonly String CPUSTRING = Environment.GetEnvironmentVariable( "PROCESSOR_ARCHITECTURE" );

		private static String GetTargetFramework( )
		{
			var targetFramework = "Unknown";
			var targetFrameworkAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( TargetFrameworkAttribute ), true );
			if ( targetFrameworkAttributes.Length > 0 )
			{
				var targetFrameworkAttribute = ( TargetFrameworkAttribute ) targetFrameworkAttributes.First();
				targetFramework = ( targetFrameworkAttribute.FrameworkDisplayName );
			}
			return targetFramework;
		}

		public static void Init( String[] args )
		{
			try
			{
				Com.InitArgv( args );
				Cbuf.Init();
				Cmd.Init();
				Cvar.Init();
				Key.Init();
				Cbuf.AddEarlyCommands( false );
				Cbuf.Execute();
				if ( Globals.dedicated.value != 1F )
					Program.Q2Dialog.SetStatus( "initializing filesystem..." );
				FS.InitFilesystem();
				if ( Globals.dedicated.value != 1F )
					Program.Q2Dialog.SetStatus( "loading config..." );
				Reconfigure( false );
				FS.SetCDDir();
				FS.MarkBaseSearchPaths();
				if ( Globals.dedicated.value != 1F )
					Program.Q2Dialog.TestQ2Data();
				Reconfigure( true );
				Cmd.AddCommand( "error", Com.Error_f );
				Globals.host_speeds = Cvar.Get( "host_speeds", "0", 0 );
				Globals.log_stats = Cvar.Get( "log_stats", "0", 0 );
				Globals.developer = Cvar.Get( "developer", "0", CVAR_ARCHIVE );
				Globals.timescale = Cvar.Get( "timescale", "0", 0 );
				Globals.fixedtime = Cvar.Get( "fixedtime", "0", 0 );
				Globals.logfile_active = Cvar.Get( "logfile", "0", 0 );
				Globals.showtrace = Cvar.Get( "showtrace", "0", 0 );
				Globals.dedicated = Cvar.Get( "dedicated", "0", CVAR_NOSET );
				var s = Com.Sprintf( "%4.2f %s %s %s", Globals.VERSION, CPUSTRING, Globals.__DATE__, BUILDSTRING );
				Cvar.Get( "version", s, CVAR_SERVERINFO | CVAR_NOSET );
				if ( Globals.dedicated.value != 1F )
					Program.Q2Dialog.SetStatus( "initializing network subsystem..." );
				NET.Init();
				Netchan.Netchan_Init();
				if ( Globals.dedicated.value != 1F )
					Program.Q2Dialog.SetStatus( "initializing server subsystem..." );
				SV_MAIN.SV_Init();
				if ( Globals.dedicated.value != 1F )
					Program.Q2Dialog.SetStatus( "initializing client subsystem..." );
				CL.Init();
				if ( !Cbuf.AddLateCommands() )
				{
					if ( Globals.dedicated.value == 0 )
						Cbuf.AddText( "d1\\n" );
					else
						Cbuf.AddText( "dedicated_start\\n" );
					Cbuf.Execute();
				}
				else
				{
					SCR.EndLoadingPlaque();
				}

				Com.Printf( "====== Quake2 Initialized ======\\n\\n" );
				CL.WriteConfiguration();
				if ( Globals.dedicated.value != 1F )
					Program.Q2Dialog.Dispose();
			}
			catch ( longjmpException e )
			{
				CoreSys.Error( "Error during initialization" );
			}
		}

		public static void Frame( Int32 msec )
		{
			try
			{
				if ( Globals.log_stats.modified )
				{
					Globals.log_stats.modified = false;
					if ( Globals.log_stats.value != 0F )
					{
						if ( Globals.log_stats_file != null )
						{
							try
							{
								Globals.log_stats_file.Close();
							}
							catch ( IOException e )
							{
							}

							Globals.log_stats_file = null;
						}

						try
						{
							Globals.log_stats_file = new QuakeFile( "stats.log", FileAccess.ReadWrite );
						}
						catch ( IOException e )
						{
							Globals.log_stats_file = null;
						}

						if ( Globals.log_stats_file != null )
						{
							try
							{
								var bytes = Encoding.ASCII.GetBytes( "entities,dlights,parts,frame time\\n" );
								Globals.log_stats_file.Write( bytes );
							}
							catch ( IOException e )
							{
							}
						}
					}
					else
					{
						if ( Globals.log_stats_file != null )
						{
							try
							{
								Globals.log_stats_file.Close();
							}
							catch ( IOException e )
							{
							}

							Globals.log_stats_file = null;
						}
					}
				}

				if ( Globals.fixedtime.value != 0F )
				{
					msec = ( Int32 ) Globals.fixedtime.value;
				}
				else if ( Globals.timescale.value != 0F )
				{
					msec = ( Int32 ) ( msec * Globals.timescale.value );
					if ( msec < 1 )
						msec = 1;
				}

				if ( Globals.showtrace.value != 0F )
				{
					Com.Printf( "%4i traces  %4i points\\n", Globals.c_traces, Globals.c_pointcontents );
					Globals.c_traces = 0;
					Globals.c_brush_traces = 0;
					Globals.c_pointcontents = 0;
				}

				Cbuf.Execute();
				var time_before = 0;
				var time_between = 0;
				var time_after = 0;
				if ( Globals.host_speeds.value != 0F )
					time_before = Timer.Milliseconds();
				Com.debugContext = "SV:";
				SV_MAIN.SV_Frame( msec );
				if ( Globals.host_speeds.value != 0F )
					time_between = Timer.Milliseconds();
				Com.debugContext = "CL:";
				CL.Frame( msec );
				if ( Globals.host_speeds.value != 0F )
				{
					time_after = Timer.Milliseconds();
					var all = time_after - time_before;
					var sv = time_between - time_before;
					var cl = time_after - time_between;
					var gm = Globals.time_after_game - Globals.time_before_game;
					var rf = Globals.time_after_ref - Globals.time_before_ref;
					sv -= gm;
					cl -= rf;
					Com.Printf( "all:%3i sv:%3i gm:%3i cl:%3i rf:%3i\\n", all, sv, gm, cl, rf );
				}
			}
			catch ( longjmpException e )
			{
				Com.DPrintf( "longjmp exception:" + e );
			}
		}

		static void Reconfigure( Boolean clear )
		{
			var dir = Cvar.Get( "cddir", "", CVAR_ARCHIVE ).string_renamed;
			Cbuf.AddText( "exec default.cfg\\n" );
			Cbuf.AddText( "bind MWHEELUP weapnext\\n" );
			Cbuf.AddText( "bind MWHEELDOWN weapprev\\n" );
			Cbuf.AddText( "bind w +forward\\n" );
			Cbuf.AddText( "bind s +back\\n" );
			Cbuf.AddText( "bind a +moveleft\\n" );
			Cbuf.AddText( "bind d +moveright\\n" );
			Cbuf.Execute();
			Cvar.Set( "vid_fullscreen", "0" );
			Cbuf.AddText( "exec config.cfg\\n" );
			Cbuf.AddEarlyCommands( clear );
			Cbuf.Execute();
			if ( !( "".Equals( dir ) ) )
				Cvar.Set( "cddir", dir );
		}
	}
}
using Jake2.Qcommon;
using Jake2.Sys;
using Q2Sharp.win;
using System;

namespace Jake2
{
	public sealed class Program
	{
		public static Q2DataDialog Q2Dialog;
		public static Action RunWindow;

		private static int oldtime;
		private static int newtime;
		private static int time;

		[STAThread]
		public static void Main( String[] args )
		{
			System.Windows.Forms.Application.SetHighDpiMode( System.Windows.Forms.HighDpiMode.SystemAware );
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault( false );

			bool dedicated = false;
			for ( int n = 0; n < args.Length; n++ )
			{
				if ( args[n].Equals( "+set" ) )
				{
					if ( n++ >= args.Length )
						break;
					if ( !args[n].Equals( "dedicated" ) )
						continue;
					if ( n++ >= args.Length )
						break;
					if ( args[n].Equals( "1" ) || args[n].Equals( "\\\"1\\\"" ) )
					{
						Com.Printf( "Starting in dedicated mode.\\n" );
						dedicated = true;
					}
				}
			}

			Globals.dedicated = Cvar.Get( "dedicated", "0", Qcom.CVAR_NOSET );
			if ( dedicated )
				Globals.dedicated.value = 1F;
			if ( Globals.dedicated.value != 1F )
			{
				Q2Dialog = new Q2DataDialog();
				Q2Dialog.ShowDialog();
			}

			int argc = ( args == null ) ? 1 : args.Length + 1;
			String[] c_args = new string[argc];
			c_args[0] = "Jake2";
			if ( argc > 1 )
			{
				System.Array.Copy( args, 0, c_args, 1, argc - 1 );
			}

			Qcom.Init( c_args );
			Globals.nostdout = Cvar.Get( "nostdout", "0", 0 );
			oldtime = Timer.Milliseconds();

			RunWindowless();
		}

		private static void RunWindowless()
		{
			while ( true )
			{
				Frame();

				// Execute the blocking Run command, i want it top level so it doesn't break engine flow
				if ( RunWindow != null )
				{
					RunWindow.Invoke();
					// Once Run has stopped blocking the window has closed, allow it to return to normal processing
					RunWindow = null;
				}
			}
		}

		public static void Frame( )
		{
			newtime = Timer.Milliseconds();
			time = newtime - oldtime;
			if ( time > 0 )
				Qcom.Frame( time );
			oldtime = newtime;
		}
	}
}
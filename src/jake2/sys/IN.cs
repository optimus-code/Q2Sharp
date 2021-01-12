using Jake2.Client;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using System;

namespace Jake2.Sys
{
	public sealed class IN : Globals
	{
		public static Boolean mouse_avail = true;
		public static Boolean mouse_active = false;
		public static Boolean ignorefirst = false;
		public static Int32 mouse_buttonstate;
		public static Int32 mouse_oldbuttonstate;
		public static Int32 old_mouse_x;
		public static Int32 old_mouse_y;
		public static Boolean mlooking;
		public static void ActivateMouse( )
		{
			if ( !mouse_avail )
				return;
			if ( !mouse_active )
			{
				KBD.mx = KBD.my = 0;
				Install_grabs();
				mouse_active = true;
			}
		}

		public static void DeactivateMouse( )
		{
			if ( mouse_active )
			{
				Uninstall_grabs();
				mouse_active = false;
			}
		}

		private static void Install_grabs( )
		{
			Globals.re.GetKeyboardHandler().InstallGrabs();
			ignorefirst = true;
		}

		private static void Uninstall_grabs( )
		{
			Globals.re.GetKeyboardHandler().UninstallGrabs();
		}

		public static void ToggleMouse( )
		{
			if ( mouse_avail )
			{
				mouse_avail = false;
				DeactivateMouse();
			}
			else
			{
				mouse_avail = true;
				ActivateMouse();
			}
		}

		public static void Init( )
		{
			in_mouse = Cvar.Get( "in_mouse", "1", CVAR_ARCHIVE );
			in_joystick = Cvar.Get( "in_joystick", "0", CVAR_ARCHIVE );
		}

		public static void Shutdown( )
		{
			mouse_avail = false;
		}

		public static void Real_IN_Init( )
		{
			Globals.m_filter = Cvar.Get( "m_filter", "0", 0 );
			Globals.in_mouse = Cvar.Get( "in_mouse", "1", CVAR_ARCHIVE );
			Globals.freelook = Cvar.Get( "freelook", "1", 0 );
			Globals.lookstrafe = Cvar.Get( "lookstrafe", "0", 0 );
			Globals.sensitivity = Cvar.Get( "sensitivity", "3", 0 );
			Globals.m_pitch = Cvar.Get( "m_pitch", "0.022", 0 );
			Globals.m_yaw = Cvar.Get( "m_yaw", "0.022", 0 );
			Globals.m_forward = Cvar.Get( "m_forward", "1", 0 );
			Globals.m_side = Cvar.Get( "m_side", "0.8", 0 );
			Cmd.AddCommand( "+mlook", new Anonymousxcommand_t() );
			Cmd.AddCommand( "-mlook", new Anonymousxcommand_t1() );
			Cmd.AddCommand( "force_centerview", new Anonymousxcommand_t2() );
			Cmd.AddCommand( "togglemouse", new Anonymousxcommand_t3() );
			IN.mouse_avail = true;
		}

		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				MLookDown();
			}
		}

		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				MLookUp();
			}
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				Force_CenterView_f();
			}
		}

		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public override void Execute( )
			{
				ToggleMouse();
			}
		}

		public static void Commands( )
		{
			Int32 i;
			if ( !IN.mouse_avail )
				return;
			KBD kbd = Globals.re.GetKeyboardHandler();
			for ( i = 0; i < 3; i++ )
			{
				if ( ( IN.mouse_buttonstate & ( 1 << i ) ) != 0 && ( IN.mouse_oldbuttonstate & ( 1 << i ) ) == 0 )
					kbd.Do_Key_Event( Key.K_MOUSE1 + i, true );
				if ( ( IN.mouse_buttonstate & ( 1 << i ) ) == 0 && ( IN.mouse_oldbuttonstate & ( 1 << i ) ) != 0 )
					kbd.Do_Key_Event( Key.K_MOUSE1 + i, false );
			}

			IN.mouse_oldbuttonstate = IN.mouse_buttonstate;
		}

		public static void Frame( )
		{
			if ( !cl.cinematicpalette_active && ( !cl.refresh_prepped || cls.key_dest == key_console || cls.key_dest == key_menu ) )
				DeactivateMouse();
			else
				ActivateMouse();
		}

		public static void CenterView( )
		{
			cl.viewangles[PITCH] = -Math3D.SHORT2ANGLE( cl.frame.playerstate.pmove.delta_angles[PITCH] );
		}

		public static void Move( usercmd_t cmd )
		{
			if ( !IN.mouse_avail )
				return;
			if ( Globals.m_filter.value != 0F )
			{
				KBD.mx = ( KBD.mx + IN.old_mouse_x ) / 2;
				KBD.my = ( KBD.my + IN.old_mouse_y ) / 2;
			}

			IN.old_mouse_x = KBD.mx;
			IN.old_mouse_y = KBD.my;
			KBD.mx = ( Int32 ) ( KBD.mx * Globals.sensitivity.value );
			KBD.my = ( Int32 ) ( KBD.my * Globals.sensitivity.value );
			if ( ( CL_input.in_strafe.state & 1 ) != 0 || ( ( Globals.lookstrafe.value != 0 ) && IN.mlooking ) )
			{
				cmd.sidemove += ( Int16 ) ( Globals.m_side.value * KBD.mx ); // TODO - Is this truncating
			}
			else
			{
				Globals.cl.viewangles[YAW] -= Globals.m_yaw.value * KBD.mx;
			}

			if ( ( IN.mlooking || Globals.freelook.value != 0F ) && ( CL_input.in_strafe.state & 1 ) == 0 )
			{
				Globals.cl.viewangles[PITCH] += Globals.m_pitch.value * KBD.my;
			}
			else
			{
				cmd.forwardmove -= ( Int16 ) ( Globals.m_forward.value * KBD.my ); // TODO - Is this truncating
			}

			KBD.mx = KBD.my = 0;
		}

		static void MLookDown( )
		{
			mlooking = true;
		}

		static void MLookUp( )
		{
			mlooking = false;
			CenterView();
		}

		static void Force_CenterView_f( )
		{
			Globals.cl.viewangles[PITCH] = 0;
		}
	}
}
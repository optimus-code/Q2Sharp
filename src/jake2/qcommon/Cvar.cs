using Q2Sharp.Game;
using Q2Sharp.Util;
using System;
using System.Collections;

namespace Q2Sharp.Qcommon
{
	public class Cvar : Globals
	{
		public static cvar_t Get( String var_name, String var_value, Int32 flags )
		{
			cvar_t var;
			if ( ( flags & ( CVAR_USERINFO | CVAR_SERVERINFO ) ) != 0 )
			{
				if ( !InfoValidate( var_name ) )
				{
					Com.Printf( "invalid info cvar name\\n" );
					return null;
				}
			}

			var = Cvar.FindVar( var_name );
			if ( var != null )
			{
				var.flags |= flags;
				return var;
			}

			if ( var_value == null )
				return null;
			if ( ( flags & ( CVAR_USERINFO | CVAR_SERVERINFO ) ) != 0 )
			{
				if ( !InfoValidate( var_value ) )
				{
					Com.Printf( "invalid info cvar value\\n" );
					return null;
				}
			}

			var = new cvar_t();
			var.name = new String( var_name );
			var.string_renamed = new String( var_value );
			var.modified = true;
			var.value = Lib.Atof( var.string_renamed );
			var.next = Globals.cvar_vars;
			Globals.cvar_vars = var;
			var.flags = flags;
			return var;
		}

		public static void Init( )
		{
			Cmd.AddCommand( "set", Set_f );
			Cmd.AddCommand( "cvarlist", List_f );
		}

		public static String VariableString( String var_name )
		{
			cvar_t var;
			var = FindVar( var_name );
			return ( var == null ) ? "" : var.string_renamed;
		}

		static cvar_t FindVar( String var_name )
		{
			cvar_t var;
			for ( var = Globals.cvar_vars; var != null; var = var.next )
			{
				if ( var_name.Equals( var.name ) )
					return var;
			}

			return null;
		}

		public static cvar_t FullSet( String var_name, String value, Int32 flags )
		{
			cvar_t var;
			var = Cvar.FindVar( var_name );
			if ( null == var )
			{
				return Cvar.Get( var_name, value, flags );
			}

			var.modified = true;
			if ( ( var.flags & CVAR_USERINFO ) != 0 )
				Globals.userinfo_modified = true;
			var.string_renamed = value;
			var.value = Lib.Atof( var.string_renamed );
			var.flags = flags;
			return var;
		}

		public static cvar_t Set( String var_name, String value )
		{
			return Set2( var_name, value, false );
		}

		public static cvar_t ForceSet( String var_name, String value )
		{
			return Cvar.Set2( var_name, value, true );
		}

		static cvar_t Set2( String var_name, String value, Boolean force )
		{
			cvar_t var = Cvar.FindVar( var_name );
			if ( var == null )
			{
				return Cvar.Get( var_name, value, 0 );
			}

			if ( ( var.flags & ( CVAR_USERINFO | CVAR_SERVERINFO ) ) != 0 )
			{
				if ( !InfoValidate( value ) )
				{
					Com.Printf( "invalid info cvar value\\n" );
					return var;
				}
			}

			if ( !force )
			{
				if ( ( var.flags & CVAR_NOSET ) != 0 )
				{
					Com.Printf( var_name + " is write protected.\\n" );
					return var;
				}

				if ( ( var.flags & CVAR_LATCH ) != 0 )
				{
					if ( var.latched_string != null )
					{
						if ( value.Equals( var.latched_string ) )
							return var;
						var.latched_string = null;
					}
					else
					{
						if ( value.Equals( var.string_renamed ) )
							return var;
					}

					if ( Globals.server_state != 0 )
					{
						Com.Printf( var_name + " will be changed for next game.\\n" );
						var.latched_string = value;
					}
					else
					{
						var.string_renamed = value;
						var.value = Lib.Atof( var.string_renamed );
						if ( var.name.Equals( "game" ) )
						{
							FS.SetGamedir( var.string_renamed );
							FS.ExecAutoexec();
						}
					}

					return var;
				}
			}
			else
			{
				if ( var.latched_string != null )
				{
					var.latched_string = null;
				}
			}

			if ( value.Equals( var.string_renamed ) )
				return var;
			var.modified = true;
			if ( ( var.flags & CVAR_USERINFO ) != 0 )
				Globals.userinfo_modified = true;
			var.string_renamed = value;
			try
			{
				var.value = Single.Parse( var.string_renamed );
			}
			catch ( Exception e )
			{
				var.value = 0F;
			}

			return var;
		}

		static xcommand_t Set_f = new Anonymousxcommand_t();
		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				Int32 c;
				Int32 flags;
				c = Cmd.Argc();
				if ( c != 3 && c != 4 )
				{
					Com.Printf( "usage: set <variable> <value> [u / s]\\n" );
					return;
				}

				if ( c == 4 )
				{
					if ( Cmd.Argv( 3 ).Equals( "u" ) )
						flags = CVAR_USERINFO;
					else if ( Cmd.Argv( 3 ).Equals( "s" ) )
						flags = CVAR_SERVERINFO;
					else
					{
						Com.Printf( "flags can only be 'u' or 's'\\n" );
						return;
					}

					Cvar.FullSet( Cmd.Argv( 1 ), Cmd.Argv( 2 ), flags );
				}
				else
					Cvar.Set( Cmd.Argv( 1 ), Cmd.Argv( 2 ) );
			}
		}

		static xcommand_t List_f = new Anonymousxcommand_t1();
		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				cvar_t var;
				Int32 i;
				i = 0;
				for ( var = Globals.cvar_vars; var != null; var = var.next, i++ )
				{
					if ( ( var.flags & CVAR_ARCHIVE ) != 0 )
						Com.Printf( "*" );
					else
						Com.Printf( " " );
					if ( ( var.flags & CVAR_USERINFO ) != 0 )
						Com.Printf( "U" );
					else
						Com.Printf( " " );
					if ( ( var.flags & CVAR_SERVERINFO ) != 0 )
						Com.Printf( "S" );
					else
						Com.Printf( " " );
					if ( ( var.flags & CVAR_NOSET ) != 0 )
						Com.Printf( "-" );
					else if ( ( var.flags & CVAR_LATCH ) != 0 )
						Com.Printf( "L" );
					else
						Com.Printf( " " );
					Com.Printf( " " + var.name + " \\\"" + var.string_renamed + "\\\"\\n" );
				}

				Com.Printf( i + " cvars\\n" );
			}
		}

		public static void SetValue( String var_name, Int32 value )
		{
			Cvar.Set( var_name, "" + value );
		}

		public static void SetValue( String var_name, Single value )
		{
			if ( value == ( Int32 ) value )
			{
				Cvar.Set( var_name, "" + ( Int32 ) value );
			}
			else
			{
				Cvar.Set( var_name, "" + value );
			}
		}

		public static Single VariableValue( String var_name )
		{
			cvar_t var = Cvar.FindVar( var_name );
			if ( var == null )
				return 0;
			return Lib.Atof( var.string_renamed );
		}

		public static Boolean Command( )
		{
			cvar_t v;
			v = Cvar.FindVar( Cmd.Argv( 0 ) );
			if ( v == null )
				return false;
			if ( Cmd.Argc() == 1 )
			{
				Com.Printf( "\\\"" + v.name + "\\\" is \\\"" + v.string_renamed + "\\\"\\n" );
				return true;
			}

			Cvar.Set( v.name, Cmd.Argv( 1 ) );
			return true;
		}

		public static String BitInfo( Int32 bit )
		{
			String info;
			cvar_t var;
			info = "";
			for ( var = Globals.cvar_vars; var != null; var = var.next )
			{
				if ( ( var.flags & bit ) != 0 )
					info = Info.Info_SetValueForKey( info, var.name, var.string_renamed );
			}

			return info;
		}

		public static String Serverinfo( )
		{
			return BitInfo( Defines.CVAR_SERVERINFO );
		}

		public static void GetLatchedVars( )
		{
			cvar_t var;
			for ( var = Globals.cvar_vars; var != null; var = var.next )
			{
				if ( var.latched_string == null || var.latched_string.Length == 0 )
					continue;
				var.string_renamed = var.latched_string;
				var.latched_string = null;
				var.value = Lib.Atof( var.string_renamed );
				if ( var.name.Equals( "game" ) )
				{
					FS.SetGamedir( var.string_renamed );
					FS.ExecAutoexec();
				}
			}
		}

		public static String Userinfo( )
		{
			return BitInfo( CVAR_USERINFO );
		}

		public static void WriteVariables( String path )
		{
			cvar_t var;
			QuakeFile f;
			String buffer;
			f = new QuakeFile( path, System.IO.FileAccess.ReadWrite );
			if ( f == null )
				return;
			try
			{
				f.Seek( f.Length );
			}
			catch ( Exception e1 )
			{
				f.Dispose();
				return;
			}

			for ( var = cvar_vars; var != null; var = var.next )
			{
				if ( ( var.flags & CVAR_ARCHIVE ) != 0 )
				{
					buffer = "set " + var.name + " \\\"" + var.string_renamed + "\\\"\\n";
					try
					{
						f.Write( buffer );
					}
					catch ( Exception e )
					{
					}
				}
			}

			f.Dispose();
		}

		public static ArrayList CompleteVariable( String partial )
		{
			ArrayList vars = new ArrayList();
			for ( cvar_t cvar = Globals.cvar_vars; cvar != null; cvar = cvar.next )
				if ( cvar.name.StartsWith( partial ) )
					vars.Add( cvar.name );
			return vars;
		}

		static Boolean InfoValidate( String s )
		{
			if ( s.IndexOf( "\\\\" ) != -1 )
				return false;
			if ( s.IndexOf( "\\\"" ) != -1 )
				return false;
			if ( s.IndexOf( ";" ) != -1 )
				return false;
			return true;
		}
	}
}
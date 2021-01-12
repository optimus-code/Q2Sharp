using J2N.IO;
using J2N.Text;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Sound;
using Jake2.Sys;
using Jake2.Util;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using static Jake2.Qcommon.qfiles;

namespace Jake2.Client
{
	public sealed class SCR : Globals
	{
		static String[][] sb_nums = new[] { new[] { "num_0", "num_1", "num_2", "num_3", "num_4", "num_5", "num_6", "num_7", "num_8", "num_9", "num_minus" }, new[] { "anum_0", "anum_1", "anum_2", "anum_3", "anum_4", "anum_5", "anum_6", "anum_7", "anum_8", "anum_9", "anum_minus" } };
		public static float scr_con_current;
		public static float scr_conlines;
		public static bool scr_initialized;
		public static int scr_draw_loading;
		public static cvar_t scr_viewsize;
		public static cvar_t scr_conspeed;
		public static cvar_t scr_centertime;
		public static cvar_t scr_showturtle;
		public static cvar_t scr_showpause;
		public static cvar_t scr_printspeed;
		public static cvar_t scr_netgraph;
		public static cvar_t scr_timegraph;
		public static cvar_t scr_debuggraph;
		public static cvar_t scr_graphheight;
		public static cvar_t scr_graphscale;
		public static cvar_t scr_graphshift;
		public static cvar_t scr_drawall;
		public static cvar_t fps = new cvar_t();
		static dirty_t scr_dirty = new dirty_t();
		static dirty_t[] scr_old_dirty = new[] { new dirty_t(), new dirty_t() };
		static string crosshair_pic;
		static int crosshair_width, crosshair_height;
		class dirty_t
		{
			public int x1;
			public int x2;
			public int y1;
			public int y2;
			public virtual void Set( dirty_t src )
			{
				x1 = src.x1;
				x2 = src.x2;
				y1 = src.y1;
				y2 = src.y2;
			}

			public virtual void Clear( )
			{
				x1 = x2 = y1 = y2 = 0;
			}
		}

		public class graphsamp_t
		{
			public float value;
			public int color;
		}

		static int current;
		static graphsamp_t[] values = new graphsamp_t[1024];
		static SCR( )
		{
			for ( int n = 0; n < 1024; n++ )
				values[n] = new graphsamp_t();
		}

		public static void DebugGraph( float value, int color )
		{
			values[current & 1023].value = value;
			values[current & 1023].color = color;
			current++;
		}

		static void DrawDebugGraph( )
		{
			int a, x, y, w, i, h;
			float v;
			int color;
			w = scr_vrect.width;
			x = scr_vrect.x;
			y = scr_vrect.y + scr_vrect.height;
			re.DrawFill( x, ( int ) ( y - scr_graphheight.value ), w, ( int ) scr_graphheight.value, 8 );
			for ( a = 0; a < w; a++ )
			{
				i = ( current - 1 - a + 1024 ) & 1023;
				v = values[i].value;
				color = values[i].color;
				v = v * scr_graphscale.value + scr_graphshift.value;
				if ( v < 0 )
					v += scr_graphheight.value * ( 1 + ( int ) ( -v / scr_graphheight.value ) );
				h = ( int ) v % ( int ) scr_graphheight.value;
				re.DrawFill( x + w - 1 - a, y - h, 1, h, color );
			}
		}

		static string scr_centerstring;
		static float scr_centertime_start;
		static float scr_centertime_off;
		static int scr_center_lines;
		static int scr_erase_center;
		public static void CenterPrint( string str )
		{
			int s;
			StringBuffer line = new StringBuffer( 64 );
			int i, j, l;
			scr_centerstring = str;
			scr_centertime_off = scr_centertime.value;
			scr_centertime_start = cl.time;
			scr_center_lines = 1;
			s = 0;
			while ( s < str.Length )
			{
				if ( str[ s ] == '\\' )
					scr_center_lines++;
				s++;
			}

			Com.Printf( "\\n\\n\\35\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\37\\n\\n" );
			s = 0;
			if ( str.Length != 0 )
			{
				do
				{
					for ( l = 0; l < 40 && ( l + s ) < str.Length; l++ )
						if ( str[ s + l ] == '\\' || str[ s + l ] == 0 )
							break;
					for ( i = 0; i < ( 40 - l ) / 2; i++ )
						line.Append( ' ' );
					for ( j = 0; j < l; j++ )
					{
						line.Append( str[ s + j ] );
					}

					line.Append( '\\' );
					Com.Printf( line.ToString() );
					while ( s < str.Length && str[ s ] != '\\' )
						s++;
					if ( s == str.Length )
						break;
					s++;
				}
				while ( true );
			}

			Com.Printf( "\\n\\n\\35\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\36\\37\\n\\n" );
			Con.ClearNotify();
		}

		static void DrawCenterString( )
		{
			string cs = scr_centerstring + "\\0";
			int start;
			int l;
			int j;
			int x, y;
			int remaining;
			if ( cs == null )
				return;
			if ( cs.Length == 0 )
				return;
			remaining = 9999;
			scr_erase_center = 0;
			start = 0;
			if ( scr_center_lines <= 4 )
				y = ( int ) ( viddef.GetHeight() * 0.35 );
			else
				y = 48;
			do
			{
				for ( l = 0; l < 40; l++ )
					if ( start + l == cs.Length - 1 || cs[ start + l ] == '\\' )
						break;
				x = ( viddef.GetWidth() - l * 8 ) / 2;
				SCR.AddDirtyPoint( x, y );
				for ( j = 0; j < l; j++, x += 8 )
				{
					re.DrawChar( x, y, cs[ start + j ] );
					if ( remaining == 0 )
						return;
					remaining--;
				}

				SCR.AddDirtyPoint( x, y + 8 );
				y += 8;
				while ( start < cs.Length && cs[ start ] != '\\' )
					start++;
				if ( start == cs.Length )
					break;
				start++;
			}
			while ( true );
		}

		static void CheckDrawCenterString( )
		{
			scr_centertime_off -= cls.frametime;
			if ( scr_centertime_off <= 0 )
				return;
			DrawCenterString();
		}

		static void CalcVrect( )
		{
			int size;
			if ( scr_viewsize.value < 40 )
				Cvar.Set( "viewsize", "40" );
			if ( scr_viewsize.value > 100 )
				Cvar.Set( "viewsize", "100" );
			size = ( int ) scr_viewsize.value;
			scr_vrect.width = viddef.GetWidth() * size / 100;
			scr_vrect.width &= ~7;
			scr_vrect.height = viddef.GetHeight() * size / 100;
			scr_vrect.height &= ~1;
			scr_vrect.x = ( viddef.GetWidth() - scr_vrect.width ) / 2;
			scr_vrect.y = ( viddef.GetHeight() - scr_vrect.height ) / 2;
		}

		static void SizeUp_f( )
		{
			Cvar.SetValue( "viewsize", scr_viewsize.value + 10 );
		}

		static void SizeDown_f( )
		{
			Cvar.SetValue( "viewsize", scr_viewsize.value - 10 );
		}

		static void Sky_f( )
		{
			float rotate;
			float[] axis = new float[] { 0, 0, 0 };
			if ( Cmd.Argc() < 2 )
			{
				Com.Printf( "Usage: sky <basename> <rotate> <axis x y z>\\n" );
				return;
			}

			if ( Cmd.Argc() > 2 )
				rotate = float.Parse( Cmd.Argv( 2 ) );
			else
				rotate = 0;
			if ( Cmd.Argc() == 6 )
			{
				axis[0] = float.Parse( Cmd.Argv( 3 ) );
				axis[1] = float.Parse( Cmd.Argv( 4 ) );
				axis[2] = float.Parse( Cmd.Argv( 5 ) );
			}
			else
			{
				axis[0] = 0;
				axis[1] = 0;
				axis[2] = 1;
			}

			re.SetSky( Cmd.Argv( 1 ), rotate, axis );
		}

		public static void Init( )
		{
			scr_viewsize = Cvar.Get( "viewsize", "100", CVAR_ARCHIVE );
			scr_conspeed = Cvar.Get( "scr_conspeed", "3", 0 );
			scr_showturtle = Cvar.Get( "scr_showturtle", "0", 0 );
			scr_showpause = Cvar.Get( "scr_showpause", "1", 0 );
			scr_centertime = Cvar.Get( "scr_centertime", "2.5", 0 );
			scr_printspeed = Cvar.Get( "scr_printspeed", "8", 0 );
			scr_netgraph = Cvar.Get( "netgraph", "1", 0 );
			scr_timegraph = Cvar.Get( "timegraph", "1", 0 );
			scr_debuggraph = Cvar.Get( "debuggraph", "1", 0 );
			scr_graphheight = Cvar.Get( "graphheight", "32", 0 );
			scr_graphscale = Cvar.Get( "graphscale", "1", 0 );
			scr_graphshift = Cvar.Get( "graphshift", "0", 0 );
			scr_drawall = Cvar.Get( "scr_drawall", "1", 0 );
			fps = Cvar.Get( "fps", "0", 0 );
			Cmd.AddCommand( "timerefresh", new Anonymousxcommand_t( ) );
			Cmd.AddCommand( "loading", new Anonymousxcommand_t1( ) );
			Cmd.AddCommand( "sizeup", new Anonymousxcommand_t2( ) );
			Cmd.AddCommand( "sizedown", new Anonymousxcommand_t3( ) );
			Cmd.AddCommand( "sky", new Anonymousxcommand_t4( ) );
			scr_initialized = true;
		}

		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				TimeRefresh_f();
			}
		}

		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				Loading_f();
			}
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				SizeUp_f();
			}
		}

		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public override void Execute( )
			{
				SizeDown_f();
			}
		}

		private sealed class Anonymousxcommand_t4 : xcommand_t
		{
			public override void Execute( )
			{
				Sky_f();
			}
		}

		static void DrawNet( )
		{
			if ( cls.netchan.outgoing_sequence - cls.netchan.incoming_acknowledged < CMD_BACKUP - 1 )
				return;
			re.DrawPic( scr_vrect.x + 64, scr_vrect.y, "net" );
		}

		static void DrawPause( )
		{
			if ( scr_showpause.value == 0 )
				return;
			if ( cl_paused.value == 0 )
				return;
			re.DrawGetPicSize( out var dim, "pause" );
			re.DrawPic( ( viddef.GetWidth() - dim.Width ) / 2, viddef.GetHeight() / 2 + 8, "pause" );
		}

		static void DrawLoading( )
		{
			if ( scr_draw_loading == 0 )
				return;
			scr_draw_loading = 0;
			re.DrawGetPicSize( out var dim, "loading" );
			re.DrawPic( ( viddef.GetWidth() - dim.Width ) / 2, ( viddef.GetHeight() - dim.Height ) / 2, "loading" );
		}

		public static void RunConsole( )
		{
			if ( cls.key_dest == key_console )
				scr_conlines = 0.5F;
			else
				scr_conlines = 0;
			if ( scr_conlines < scr_con_current )
			{
				scr_con_current -= scr_conspeed.value * cls.frametime;
				if ( scr_conlines > scr_con_current )
					scr_con_current = scr_conlines;
			}
			else if ( scr_conlines > scr_con_current )
			{
				scr_con_current += scr_conspeed.value * cls.frametime;
				if ( scr_conlines < scr_con_current )
					scr_con_current = scr_conlines;
			}
		}

		static void DrawConsole( )
		{
			Con.CheckResize();
			if ( cls.state == ca_disconnected || cls.state == ca_connecting )
			{
				Con.DrawConsole( 1F );
				return;
			}

			if ( cls.state != ca_active || !cl.refresh_prepped )
			{
				Con.DrawConsole( 0.5F );
				re.DrawFill( 0, viddef.GetHeight() / 2, viddef.GetWidth(), viddef.GetHeight() / 2, 0 );
				return;
			}

			if ( scr_con_current != 0 )
			{
				Con.DrawConsole( scr_con_current );
			}
			else
			{
				if ( cls.key_dest == key_game || cls.key_dest == key_message )
					Con.DrawNotify();
			}
		}

		public static void BeginLoadingPlaque( )
		{
			S.StopAllSounds();
			cl.sound_prepped = false;
			if ( cls.disable_screen != 0 )
				return;
			if ( developer.value != 0 )
				return;
			if ( cls.state == ca_disconnected )
				return;
			if ( cls.key_dest == key_console )
				return;
			if ( cl.cinematictime > 0 )
				scr_draw_loading = 2;
			else
				scr_draw_loading = 1;
			UpdateScreen();
			cls.disable_screen = Timer.Milliseconds();
			cls.disable_servercount = cl.servercount;
		}

		public static void EndLoadingPlaque( )
		{
			cls.disable_screen = 0;
			Con.ClearNotify();
		}

		static void Loading_f( )
		{
			BeginLoadingPlaque();
		}

		static void TimeRefresh_f( )
		{
			int i;
			int start, stop;
			float time;
			if ( cls.state != ca_active )
				return;
			start = Timer.Milliseconds();
			if ( Cmd.Argc() == 2 )
			{
				re.BeginFrame( 0 );
				for ( i = 0; i < 128; i++ )
				{
					cl.refdef.viewangles[1] = i / 128F * 360F;
					re.RenderFrame( cl.refdef );
				}

				re.EndFrame();
			}
			else
			{
				for ( i = 0; i < 128; i++ )
				{
					cl.refdef.viewangles[1] = i / 128F * 360F;
					re.BeginFrame( 0 );
					re.RenderFrame( cl.refdef );
					re.EndFrame();
				}
			}

			stop = Timer.Milliseconds();
			time = ( stop - start ) / 1000F;
			Com.Printf( "%f seconds (%f fps)\\n", time, 128F / time );
		}

		public static void DirtyScreen( )
		{
			AddDirtyPoint( 0, 0 );
			AddDirtyPoint( viddef.GetWidth() - 1, viddef.GetHeight() - 1 );
		}

		static dirty_t clear = new dirty_t();
		static void TileClear( )
		{
			int i;
			int top, bottom, left, right;
			clear.Clear();
			if ( scr_drawall.value != 0 )
				DirtyScreen();
			if ( scr_con_current == 1F )
				return;
			if ( scr_viewsize.value == 100 )
				return;
			if ( cl.cinematictime > 0 )
				return;
			clear.Set( scr_dirty );
			for ( i = 0; i < 2; i++ )
			{
				if ( scr_old_dirty[i].x1 < clear.x1 )
					clear.x1 = scr_old_dirty[i].x1;
				if ( scr_old_dirty[i].x2 > clear.x2 )
					clear.x2 = scr_old_dirty[i].x2;
				if ( scr_old_dirty[i].y1 < clear.y1 )
					clear.y1 = scr_old_dirty[i].y1;
				if ( scr_old_dirty[i].y2 > clear.y2 )
					clear.y2 = scr_old_dirty[i].y2;
			}

			scr_old_dirty[1].Set( scr_old_dirty[0] );
			scr_old_dirty[0].Set( scr_dirty );
			scr_dirty.x1 = 9999;
			scr_dirty.x2 = -9999;
			scr_dirty.y1 = 9999;
			scr_dirty.y2 = -9999;
			top = ( int ) ( scr_con_current * viddef.GetHeight() );
			if ( top >= clear.y1 )
				clear.y1 = top;
			if ( clear.y2 <= clear.y1 )
				return;
			top = scr_vrect.y;
			bottom = top + scr_vrect.height - 1;
			left = scr_vrect.x;
			right = left + scr_vrect.width - 1;
			if ( clear.y1 < top )
			{
				i = clear.y2 < top - 1 ? clear.y2 : top - 1;
				re.DrawTileClear( clear.x1, clear.y1, clear.x2 - clear.x1 + 1, i - clear.y1 + 1, "backtile" );
				clear.y1 = top;
			}

			if ( clear.y2 > bottom )
			{
				i = clear.y1 > bottom + 1 ? clear.y1 : bottom + 1;
				re.DrawTileClear( clear.x1, i, clear.x2 - clear.x1 + 1, clear.y2 - i + 1, "backtile" );
				clear.y2 = bottom;
			}

			if ( clear.x1 < left )
			{
				i = clear.x2 < left - 1 ? clear.x2 : left - 1;
				re.DrawTileClear( clear.x1, clear.y1, i - clear.x1 + 1, clear.y2 - clear.y1 + 1, "backtile" );
				clear.x1 = left;
			}

			if ( clear.x2 > right )
			{
				i = clear.x1 > right + 1 ? clear.x1 : right + 1;
				re.DrawTileClear( i, clear.y1, clear.x2 - i + 1, clear.y2 - clear.y1 + 1, "backtile" );
				clear.x2 = right;
			}
		}

		static readonly int STAT_MINUS = 10;
		static readonly int ICON_WIDTH = 24;
		static readonly int ICON_HEIGHT = 24;
		static readonly int CHAR_WIDTH = 16;
		static readonly int ICON_SPACE = 8;
		static void SizeHUDString( string string_renamed, out Size dim )
		{
			int lines, width, current;
			lines = 1;
			width = 0;
			current = 0;
			for ( int i = 0; i < string_renamed.Length; i++ )
			{
				if ( string_renamed[i] == '\\' )
				{
					lines++;
					current = 0;
				}
				else
				{
					current++;
					if ( current > width )
						width = current;
				}
			}

			dim = new Size();
			dim.Width = width * 8;
			dim.Height = lines * 8;
		}

		static void DrawHUDString( string string_renamed, int x, int y, int centerwidth, int xor )
		{
			int margin;
			StringBuffer line = new StringBuffer( 1024 );
			int i;
			margin = x;
			for ( int l = 0; l < string_renamed.Length; )
			{
				line = new StringBuffer( 1024 );
				while ( l < string_renamed.Length && string_renamed[l] != '\\' )
				{
					line.Append( string_renamed[l] );
					l++;
				}

				if ( centerwidth != 0 )
					x = margin + ( centerwidth - line.Length * 8 ) / 2;
				else
					x = margin;
				for ( i = 0; i < line.Length; i++ )
				{
					re.DrawChar( x, y, line[i] ^ xor );
					x += 8;
				}

				if ( l < string_renamed.Length )
				{
					l++;
					x = margin;
					y += 8;
				}
			}
		}

		static void DrawField( int x, int y, int color, int width, int value )
		{
			char ptr;
			string num;
			int l;
			int frame;
			if ( width < 1 )
				return;
			if ( width > 5 )
				width = 5;
			AddDirtyPoint( x, y );
			AddDirtyPoint( x + width * CHAR_WIDTH + 2, y + 23 );
			num = value.ToString();
			l = num.Length;
			if ( l > width )
				l = width;
			x += 2 + CHAR_WIDTH * ( width - l );
			ptr = num[0];
			for ( int i = 0; i < l; i++ )
			{
				ptr = num[i];
				if ( ptr == '-' )
					frame = STAT_MINUS;
				else
					frame = ptr - '0';
				re.DrawPic( x, y, sb_nums[color][frame] );
				x += CHAR_WIDTH;
			}
		}

		public static void TouchPics( )
		{
			int i, j;
			for ( i = 0; i < 2; i++ )
				for ( j = 0; j < 11; j++ )
					re.RegisterPic( sb_nums[i][j] );
			if ( crosshair.value != 0F )
			{
				if ( crosshair.value > 3F || crosshair.value < 0F )
					crosshair.value = 3F;
				crosshair_pic = "ch" + ( int ) crosshair.value;
				re.DrawGetPicSize( out var dim, crosshair_pic );
				crosshair_width = dim.Width;
				crosshair_height = dim.Height;
				if ( crosshair_width == 0 )
					crosshair_pic = "";
			}
		}

		private static LayoutParser layoutParser = new LayoutParser();
		static void ExecuteLayoutString( string s )
		{
			if ( cls.state != ca_active || !cl.refresh_prepped )
				return;
			if ( s == null || s.Length == 0 )
				return;
			int x = 0;
			int y = 0;
			int width = 3;
			int value;
			LayoutParser parser = layoutParser;
			parser.Init( s );
			while ( parser.HasNext() )
			{
				parser.Next();
				if ( parser.TokenEquals( "xl" ) )
				{
					parser.Next();
					x = parser.TokenAsInt();
					continue;
				}

				if ( parser.TokenEquals( "xr" ) )
				{
					parser.Next();
					x = viddef.GetWidth() + parser.TokenAsInt();
					continue;
				}

				if ( parser.TokenEquals( "xv" ) )
				{
					parser.Next();
					x = viddef.GetWidth() / 2 - 160 + parser.TokenAsInt();
					continue;
				}

				if ( parser.TokenEquals( "yt" ) )
				{
					parser.Next();
					y = parser.TokenAsInt();
					continue;
				}

				if ( parser.TokenEquals( "yb" ) )
				{
					parser.Next();
					y = viddef.GetHeight() + parser.TokenAsInt();
					continue;
				}

				if ( parser.TokenEquals( "yv" ) )
				{
					parser.Next();
					y = viddef.GetHeight() / 2 - 120 + parser.TokenAsInt();
					continue;
				}

				if ( parser.TokenEquals( "pic" ) )
				{
					parser.Next();
					value = cl.frame.playerstate.stats[parser.TokenAsInt()];
					if ( value >= MAX_IMAGES )
						Com.Error( ERR_DROP, "Pic >= MAX_IMAGES" );
					if ( cl.configstrings[CS_IMAGES + value] != null )
					{
						AddDirtyPoint( x, y );
						AddDirtyPoint( x + 23, y + 23 );
						re.DrawPic( x, y, cl.configstrings[CS_IMAGES + value] );
					}

					continue;
				}

				if ( parser.TokenEquals( "client" ) )
				{
					int score, ping, time;
					parser.Next();
					x = viddef.GetWidth() / 2 - 160 + parser.TokenAsInt();
					parser.Next();
					y = viddef.GetHeight() / 2 - 120 + parser.TokenAsInt();
					AddDirtyPoint( x, y );
					AddDirtyPoint( x + 159, y + 31 );
					parser.Next();
					value = parser.TokenAsInt();
					if ( value >= MAX_CLIENTS || value < 0 )
						Com.Error( ERR_DROP, "client >= MAX_CLIENTS" );
					clientinfo_t ci = cl.clientinfo[value];
					parser.Next();
					score = parser.TokenAsInt();
					parser.Next();
					ping = parser.TokenAsInt();
					parser.Next();
					time = parser.TokenAsInt();
					Con.DrawAltString( x + 32, y, ci.name );
					Con.DrawString( x + 32, y + 8, "Score: " );
					Con.DrawAltString( x + 32 + 7 * 8, y + 8, "" + score );
					Con.DrawString( x + 32, y + 16, "Ping:  " + ping );
					Con.DrawString( x + 32, y + 24, "Time:  " + time );
					if ( ci.icon == null )
						ci = cl.baseclientinfo;
					re.DrawPic( x, y, ci.iconname );
					continue;
				}

				if ( parser.TokenEquals( "ctf" ) )
				{
					int score, ping;
					parser.Next();
					x = viddef.GetWidth() / 2 - 160 + parser.TokenAsInt();
					parser.Next();
					y = viddef.GetHeight() / 2 - 120 + parser.TokenAsInt();
					AddDirtyPoint( x, y );
					AddDirtyPoint( x + 159, y + 31 );
					parser.Next();
					value = parser.TokenAsInt();
					if ( value >= MAX_CLIENTS || value < 0 )
						Com.Error( ERR_DROP, "client >= MAX_CLIENTS" );
					clientinfo_t ci = cl.clientinfo[value];
					parser.Next();
					score = parser.TokenAsInt();
					parser.Next();
					ping = parser.TokenAsInt();
					if ( ping > 999 )
						ping = 999;
					string block = Com.Sprintf( "%3d %3d %-12.12s", score, ping, ci.name );
					if ( value == cl.playernum )
						Con.DrawAltString( x, y, block );
					else
						Con.DrawString( x, y, block );
					continue;
				}

				if ( parser.TokenEquals( "picn" ) )
				{
					parser.Next();
					AddDirtyPoint( x, y );
					AddDirtyPoint( x + 23, y + 23 );
					re.DrawPic( x, y, parser.Token() );
					continue;
				}

				if ( parser.TokenEquals( "num" ) )
				{
					parser.Next();
					width = parser.TokenAsInt();
					parser.Next();
					value = cl.frame.playerstate.stats[parser.TokenAsInt()];
					DrawField( x, y, 0, width, value );
					continue;
				}

				if ( parser.TokenEquals( "hnum" ) )
				{
					int color;
					width = 3;
					value = cl.frame.playerstate.stats[STAT_HEALTH];
					if ( value > 25 )
						color = 0;
					else if ( value > 0 )
						color = ( cl.frame.serverframe >> 2 ) & 1;
					else
						color = 1;
					if ( ( cl.frame.playerstate.stats[STAT_FLASHES] & 1 ) != 0 )
						re.DrawPic( x, y, "field_3" );
					DrawField( x, y, color, width, value );
					continue;
				}

				if ( parser.TokenEquals( "anum" ) )
				{
					int color;
					width = 3;
					value = cl.frame.playerstate.stats[STAT_AMMO];
					if ( value > 5 )
						color = 0;
					else if ( value >= 0 )
						color = ( cl.frame.serverframe >> 2 ) & 1;
					else
						continue;
					if ( ( cl.frame.playerstate.stats[STAT_FLASHES] & 4 ) != 0 )
						re.DrawPic( x, y, "field_3" );
					DrawField( x, y, color, width, value );
					continue;
				}

				if ( parser.TokenEquals( "rnum" ) )
				{
					int color;
					width = 3;
					value = cl.frame.playerstate.stats[STAT_ARMOR];
					if ( value < 1 )
						continue;
					color = 0;
					if ( ( cl.frame.playerstate.stats[STAT_FLASHES] & 2 ) != 0 )
						re.DrawPic( x, y, "field_3" );
					DrawField( x, y, color, width, value );
					continue;
				}

				if ( parser.TokenEquals( "stat_string" ) )
				{
					parser.Next();
					int index = parser.TokenAsInt();
					if ( index < 0 || index >= MAX_CONFIGSTRINGS )
						Com.Error( ERR_DROP, "Bad stat_string index" );
					index = cl.frame.playerstate.stats[index];
					if ( index < 0 || index >= MAX_CONFIGSTRINGS )
						Com.Error( ERR_DROP, "Bad stat_string index" );
					Con.DrawString( x, y, cl.configstrings[index] );
					continue;
				}

				if ( parser.TokenEquals( "cstring" ) )
				{
					parser.Next();
					DrawHUDString( parser.Token(), x, y, 320, 0 );
					continue;
				}

				if ( parser.TokenEquals( "string" ) )
				{
					parser.Next();
					Con.DrawString( x, y, parser.Token() );
					continue;
				}

				if ( parser.TokenEquals( "cstring2" ) )
				{
					parser.Next();
					DrawHUDString( parser.Token(), x, y, 320, 0x80 );
					continue;
				}

				if ( parser.TokenEquals( "string2" ) )
				{
					parser.Next();
					Con.DrawAltString( x, y, parser.Token() );
					continue;
				}

				if ( parser.TokenEquals( "if" ) )
				{
					parser.Next();
					value = cl.frame.playerstate.stats[parser.TokenAsInt()];
					if ( value == 0 )
					{
						parser.Next();
						while ( parser.HasNext() && !( parser.TokenEquals( "endif" ) ) )
						{
							parser.Next();
						}
					}

					continue;
				}
			}
		}

		static void DrawStats( )
		{
			SCR.ExecuteLayoutString( cl.configstrings[CS_STATUSBAR] );
		}

		static new readonly int STAT_LAYOUTS = 13;
		static void DrawLayout( )
		{
			if ( cl.frame.playerstate.stats[STAT_LAYOUTS] != 0 )
				SCR.ExecuteLayoutString( cl.layout );
		}

		private static readonly float[] separation = new float[] { 0, 0 };
		static void UpdateScreen2( )
		{
			int numframes;
			int i;
			if ( cls.disable_screen != 0 )
			{
				if ( Timer.Milliseconds() - cls.disable_screen > 120000 )
				{
					cls.disable_screen = 0;
					Com.Printf( "Loading plaque timed out.\\n" );
				}

				return;
			}

			if ( !scr_initialized || !con.initialized )
				return;
			if ( cl_stereo_separation.value > 1 )
				Cvar.SetValue( "cl_stereo_separation", 1F );
			else if ( cl_stereo_separation.value < 0 )
				Cvar.SetValue( "cl_stereo_separation", 0F );
			if ( cl_stereo.value != 0 )
			{
				numframes = 2;
				separation[0] = -cl_stereo_separation.value / 2;
				separation[1] = cl_stereo_separation.value / 2;
			}
			else
			{
				separation[0] = 0;
				separation[1] = 0;
				numframes = 1;
			}

			for ( i = 0; i < numframes; i++ )
			{
				re.BeginFrame( separation[i] );
				if ( scr_draw_loading == 2 )
				{
					re.CinematicSetPalette( null );
					scr_draw_loading = 0;
					re.DrawGetPicSize( out var dim, "loading" );
					re.DrawPic( ( viddef.GetWidth() - dim.Width ) / 2, ( viddef.GetHeight() - dim.Height ) / 2, "loading" );
				}
				else if ( cl.cinematictime > 0 )
				{
					if ( cls.key_dest == key_menu )
					{
						if ( cl.cinematicpalette_active )
						{
							re.CinematicSetPalette( null );
							cl.cinematicpalette_active = false;
						}

						Menu.Draw();
					}
					else if ( cls.key_dest == key_console )
					{
						if ( cl.cinematicpalette_active )
						{
							re.CinematicSetPalette( null );
							cl.cinematicpalette_active = false;
						}

						DrawConsole();
					}
					else
					{
						DrawCinematic();
					}
				}
				else
				{
					if ( cl.cinematicpalette_active )
					{
						re.CinematicSetPalette( null );
						cl.cinematicpalette_active = false;
					}

					CalcVrect();
					TileClear();
					V.RenderView( separation[i] );
					DrawStats();
					if ( ( cl.frame.playerstate.stats[STAT_LAYOUTS] & 1 ) != 0 )
						DrawLayout();
					if ( ( cl.frame.playerstate.stats[STAT_LAYOUTS] & 2 ) != 0 )
						CL_inv.DrawInventory();
					DrawNet();
					CheckDrawCenterString();
					DrawFPS();
					DrawPause();
					DrawConsole();
					Menu.Draw();
					DrawLoading();
				}
			}

			Globals.re.EndFrame();
		}

		public static void DrawCrosshair( )
		{
			if ( crosshair.value == 0F )
				return;
			if ( crosshair.modified )
			{
				crosshair.modified = false;
				SCR.TouchPics();
			}

			if ( crosshair_pic.Length == 0 )
				return;
			re.DrawPic( scr_vrect.x + ( ( scr_vrect.width - crosshair_width ) >> 1 ), scr_vrect.y + ( ( scr_vrect.height - crosshair_height ) >> 1 ), crosshair_pic );
		}

		private static xcommand_t updateScreenCallback = new Anonymousxcommand_t5( );
		private sealed class Anonymousxcommand_t5 : xcommand_t
		{
			public override void Execute( )
			{
				UpdateScreen2();
			}
		}

		public static void UpdateScreen( )
		{
			Globals.re.UpdateScreen( updateScreenCallback );
		}

		public static void AddDirtyPoint( int x, int y )
		{
			if ( x < scr_dirty.x1 )
				scr_dirty.x1 = x;
			if ( x > scr_dirty.x2 )
				scr_dirty.x2 = x;
			if ( y < scr_dirty.y1 )
				scr_dirty.y1 = y;
			if ( y > scr_dirty.y2 )
				scr_dirty.y2 = y;
		}

		private static int lastframes = 0;
		private static int lasttime = 0;
		private static string fpsvalue = "";
		static void DrawFPS( )
		{
			if ( fps.value > 0F )
			{
				if ( fps.modified )
				{
					fps.modified = false;
					Cvar.SetValue( "cl_maxfps", 1000 );
				}

				int diff = cls.realtime - lasttime;
				if ( diff > ( int ) ( fps.value * 1000 ) )
				{
					fpsvalue = ( cls.framecount - lastframes ) * 100000 / diff / 100F + " fps";
					lastframes = cls.framecount;
					lasttime = cls.realtime;
				}

				int x = viddef.GetWidth() - 8 * fpsvalue.Length - 2;
				for ( int i = 0; i < fpsvalue.Length; i++ )
				{
					re.DrawChar( x, 2, fpsvalue[i] );
					x += 8;
				}
			}
			else if ( fps.modified )
			{
				fps.modified = false;
				Cvar.SetValue( "cl_maxfps", 90 );
			}
		}

		private class cinematics_t
		{
			public bool restart_sound;
			public int s_rate;
			public int s_width;
			public int s_channels;
			public int width;
			public int height;
			public byte[] pic;
			public byte[] pic_pending;
			public int[] hnodes1;
			public int[] numhnodes1 = new int[256];
			public int[] h_used = new int[512];
			public int[] h_count = new int[512];
		}

		private static cinematics_t cin = new cinematics_t();
		static int LoadPCX( string filename, byte[] palette, cinematics_t cin )
		{
			qfiles.pcx_t pcx;
			ByteBuffer raw = FS.LoadMappedFile( filename );
			if ( raw == null )
			{
				VID.Printf( Defines.PRINT_DEVELOPER, "Bad pcx file " + filename + '\\' );
				return 0;
			}

			pcx = new pcx_t( raw );
			if ( pcx.manufacturer != 0x0a || pcx.version != 5 || pcx.encoding != 1 || pcx.bits_per_pixel != 8 || pcx.xmax >= 640 || pcx.ymax >= 480 )
			{
				VID.Printf( Defines.PRINT_ALL, "Bad pcx file " + filename + '\\' );
				return 0;
			}

			int width = pcx.xmax - pcx.xmin + 1;
			int height = pcx.ymax - pcx.ymin + 1;
			byte[] pix = new byte[width * height];
			if ( palette != null )
			{
				raw.Position = raw.Limit - 768;
				raw.Get( palette );
			}

			if ( cin != null )
			{
				cin.pic = pix;
				cin.width = width;
				cin.height = height;
			}

			int count = 0;
			byte dataByte = 0;
			int runLength = 0;
			int x, y;
			int p = 0;
			for ( y = 0; y < height; y++ )
			{
				for ( x = 0; x < width; )
				{
					dataByte = pcx.data.Get( p++ );
					if ( ( dataByte & 0xC0 ) == 0xC0 )
					{
						runLength = dataByte & 0x3F;
						dataByte = pcx.data.Get( p++ );
						while ( runLength-- > 0 )
						{
							pix[count++] = dataByte;
							x++;
						}
					}
					else
					{
						pix[count++] = dataByte;
						x++;
					}
				}
			}

			return width * height;
		}

		public static void StopCinematic( )
		{
			if ( cin.restart_sound )
			{
				cl.cinematictime = 0;
				cin.pic = null;
				cin.pic_pending = null;
				if ( cl.cinematicpalette_active )
				{
					re.CinematicSetPalette( null );
					cl.cinematicpalette_active = false;
				}

				if ( cl.cinematic_file != null )
				{
					cl.cinematic_file = null;
				}

				if ( cin.hnodes1 != null )
				{
					cin.hnodes1 = null;
				}

				S.DisableStreaming();
				cin.restart_sound = false;
			}
		}

		public static void FinishCinematic( )
		{
			MSG.WriteByte( cls.netchan.message, clc_stringcmd );
			SZ.Print( cls.netchan.message, "nextserver " + cl.servercount + '\\' );
		}

		private static int SmallestNode1( int numhnodes )
		{
			int best = 99999999;
			int bestnode = -1;
			for ( int i = 0; i < numhnodes; i++ )
			{
				if ( cin.h_used[i] != 0 )
					continue;
				if ( cin.h_count[i] == 0 )
					continue;
				if ( cin.h_count[i] < best )
				{
					best = cin.h_count[i];
					bestnode = i;
				}
			}

			if ( bestnode == -1 )
				return -1;
			cin.h_used[bestnode] = 1;
			return bestnode;
		}

		private static void Huff1TableInit( )
		{
			int[] node;
			byte[] counts = new byte[256];
			int numhnodes;
			cin.hnodes1 = new int[256 * 256 * 2];
			cin.hnodes1.Fill( 0 );
			for ( int prev = 0; prev < 256; prev++ )
			{
				cin.h_count.Fill( 0 );
				cin.h_used.Fill( 0 );
				cl.cinematic_file.Get( counts );
				for ( int j = 0; j < 256; j++ )
					cin.h_count[j] = counts[j] & 0xFF;
				numhnodes = 256;
				int nodebase = 0 + prev * 256 * 2;
				int index = 0;
				node = cin.hnodes1;
				while ( numhnodes != 511 )
				{
					index = nodebase + ( numhnodes - 256 ) * 2;
					node[index] = SmallestNode1( numhnodes );
					if ( node[index] == -1 )
						break;
					node[index + 1] = SmallestNode1( numhnodes );
					if ( node[index + 1] == -1 )
						break;
					cin.h_count[numhnodes] = cin.h_count[node[index]] + cin.h_count[node[index + 1]];
					numhnodes++;
				}

				cin.numhnodes1[prev] = numhnodes - 1;
			}
		}

		private static byte[] Huff1Decompress( byte[] in_renamed, int size )
		{
			int count = ( in_renamed[0] & 0xFF ) | ( ( in_renamed[1] & 0xFF ) << 8 ) | ( ( in_renamed[2] & 0xFF ) << 16 ) | ( ( in_renamed[3] & 0xFF ) << 24 );
			int input = 4;
			byte[] out_renamed = new byte[count];
			int out_p = 0;
			int hnodesbase = -256 * 2;
			int index = hnodesbase;
			int[] hnodes = cin.hnodes1;
			int nodenum = cin.numhnodes1[0];
			int inbyte;
			while ( count != 0 )
			{
				inbyte = in_renamed[input++] & 0xFF;
				if ( nodenum < 256 )
				{
					index = hnodesbase + ( nodenum << 9 );
					out_renamed[out_p++] = ( byte ) nodenum;
					if ( --count == 0 )
						break;
					nodenum = cin.numhnodes1[nodenum];
				}

				nodenum = hnodes[index + nodenum * 2 + ( inbyte & 1 )];
				inbyte >>= 1;
				if ( nodenum < 256 )
				{
					index = hnodesbase + ( nodenum << 9 );
					out_renamed[out_p++] = ( byte ) nodenum;
					if ( --count == 0 )
						break;
					nodenum = cin.numhnodes1[nodenum];
				}

				nodenum = hnodes[index + nodenum * 2 + ( inbyte & 1 )];
				inbyte >>= 1;
				if ( nodenum < 256 )
				{
					index = hnodesbase + ( nodenum << 9 );
					out_renamed[out_p++] = ( byte ) nodenum;
					if ( --count == 0 )
						break;
					nodenum = cin.numhnodes1[nodenum];
				}

				nodenum = hnodes[index + nodenum * 2 + ( inbyte & 1 )];
				inbyte >>= 1;
				if ( nodenum < 256 )
				{
					index = hnodesbase + ( nodenum << 9 );
					out_renamed[out_p++] = ( byte ) nodenum;
					if ( --count == 0 )
						break;
					nodenum = cin.numhnodes1[nodenum];
				}

				nodenum = hnodes[index + nodenum * 2 + ( inbyte & 1 )];
				inbyte >>= 1;
				if ( nodenum < 256 )
				{
					index = hnodesbase + ( nodenum << 9 );
					out_renamed[out_p++] = ( byte ) nodenum;
					if ( --count == 0 )
						break;
					nodenum = cin.numhnodes1[nodenum];
				}

				nodenum = hnodes[index + nodenum * 2 + ( inbyte & 1 )];
				inbyte >>= 1;
				if ( nodenum < 256 )
				{
					index = hnodesbase + ( nodenum << 9 );
					out_renamed[out_p++] = ( byte ) nodenum;
					if ( --count == 0 )
						break;
					nodenum = cin.numhnodes1[nodenum];
				}

				nodenum = hnodes[index + nodenum * 2 + ( inbyte & 1 )];
				inbyte >>= 1;
				if ( nodenum < 256 )
				{
					index = hnodesbase + ( nodenum << 9 );
					out_renamed[out_p++] = ( byte ) nodenum;
					if ( --count == 0 )
						break;
					nodenum = cin.numhnodes1[nodenum];
				}

				nodenum = hnodes[index + nodenum * 2 + ( inbyte & 1 )];
				inbyte >>= 1;
				if ( nodenum < 256 )
				{
					index = hnodesbase + ( nodenum << 9 );
					out_renamed[out_p++] = ( byte ) nodenum;
					if ( --count == 0 )
						break;
					nodenum = cin.numhnodes1[nodenum];
				}

				nodenum = hnodes[index + nodenum * 2 + ( inbyte & 1 )];
				inbyte >>= 1;
			}

			if ( input != size && input != size + 1 )
			{
				Com.Printf( "Decompression overread by " + ( input - size ) );
			}

			return out_renamed;
		}

		private static byte[] compressed = new byte[0x20000];
		static byte[] ReadNextFrame( )
		{
			ByteBuffer file = cl.cinematic_file;
			int command = file.GetInt32();
			if ( command == 2 )
			{
				return null;
			}

			if ( command == 1 )
			{
				file.Get( cl.cinematicpalette );
				cl.cinematicpalette_active = false;
			}

			int size = file.GetInt32();
			if ( size > compressed.Length || size < 1 )
				Com.Error( ERR_DROP, "Bad compressed frame size:" + size );
			file.Get( compressed, 0, size );
			int start = cl.cinematicframe * cin.s_rate / 14;
			int end = ( cl.cinematicframe + 1 ) * cin.s_rate / 14;
			int count = end - start;
			S.RawSamples( count, cin.s_rate, cin.s_width, cin.s_channels, file.Slice() );
			file.Position = file.Position + count * cin.s_width * cin.s_channels;
			byte[] pic = Huff1Decompress( compressed, size );
			cl.cinematicframe++;
			return pic;
		}

		public static void RunCinematic( )
		{
			if ( cl.cinematictime <= 0 )
			{
				StopCinematic();
				return;
			}

			if ( cl.cinematicframe == -1 )
			{
				return;
			}

			if ( cls.key_dest != key_game )
			{
				cl.cinematictime = cls.realtime - cl.cinematicframe * 1000 / 14;
				return;
			}

			int frame = ( int ) ( ( cls.realtime - cl.cinematictime ) * 14F / 1000 );
			if ( frame <= cl.cinematicframe )
				return;
			if ( frame > cl.cinematicframe + 1 )
			{
				Com.Println( "Dropped frame: " + frame + " > " + ( cl.cinematicframe + 1 ) );
				cl.cinematictime = cls.realtime - cl.cinematicframe * 1000 / 14;
			}

			cin.pic = cin.pic_pending;
			cin.pic_pending = ReadNextFrame();
			if ( cin.pic_pending == null )
			{
				StopCinematic();
				FinishCinematic();
				cl.cinematictime = 1;
				BeginLoadingPlaque();
				cl.cinematictime = 0;
				return;
			}
		}

		static bool DrawCinematic( )
		{
			if ( cl.cinematictime <= 0 )
			{
				return false;
			}

			if ( cls.key_dest == key_menu )
			{
				Globals.re.CinematicSetPalette( null );
				cl.cinematicpalette_active = false;
				return true;
			}

			if ( !cl.cinematicpalette_active )
			{
				re.CinematicSetPalette( cl.cinematicpalette );
				cl.cinematicpalette_active = true;
			}

			if ( cin.pic == null )
				return true;
			Globals.re.DrawStretchRaw( 0, 0, viddef.GetWidth(), viddef.GetHeight(), cin.width, cin.height, cin.pic );
			return true;
		}

		public static void PlayCinematic( string arg )
		{
			cl.cinematicframe = 0;
			if ( arg.EndsWith( ".pcx" ) )
			{
				string cinematicName = "pics/" + arg;
				int size = LoadPCX( cinematicName, cl.cinematicpalette, cin );
				cl.cinematicframe = -1;
				cl.cinematictime = 1;
				EndLoadingPlaque();
				cls.state = ca_active;
				if ( size == 0 || cin.pic == null )
				{
					Com.Println( cinematicName + " not found." );
					cl.cinematictime = 0;
				}

				return;
			}

			string name = "video/" + arg;
			cl.cinematic_file = FS.LoadMappedFile( name );
			if ( cl.cinematic_file == null )
			{
				FinishCinematic();
				cl.cinematictime = 0;
				return;
			}

			EndLoadingPlaque();
			cls.state = ca_active;
			cl.cinematic_file.Order = ByteOrder.LittleEndian;
			ByteBuffer file = cl.cinematic_file;
			cin.width = file.GetInt32();
			cin.height = file.GetInt32();
			cin.s_rate = file.GetInt32();
			cin.s_width = file.GetInt32();
			cin.s_channels = file.GetInt32();
			Huff1TableInit();
			cin.restart_sound = true;
			cl.cinematicframe = 0;
			cin.pic = ReadNextFrame();
			cl.cinematictime = Timer.Milliseconds();
		}
	}
}
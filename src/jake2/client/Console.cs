using J2N.Text;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.IO;

namespace Q2Sharp.Client
{
	public sealed class Con : Globals
	{
		public static xcommand_t ToggleConsole_f = new Anonymousxcommand_t();
		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				SCR.EndLoadingPlaque();
				if ( Globals.cl.attractloop )
				{
					Cbuf.AddText( "killserver\\n" );
					return;
				}

				if ( Globals.cls.state == Defines.ca_disconnected )
				{
					Cbuf.AddText( "d1\\n" );
					return;
				}

				Key.ClearTyping();
				Con.ClearNotify();
				if ( Globals.cls.key_dest == Defines.key_console )
				{
					Menu.ForceMenuOff();
					Cvar.Set( "paused", "0" );
				}
				else
				{
					Menu.ForceMenuOff();
					Globals.cls.key_dest = Defines.key_console;
					if ( Cvar.VariableValue( "maxclients" ) == 1 && Globals.server_state != 0 )
						Cvar.Set( "paused", "1" );
				}
			}
		}

		public static xcommand_t Clear_f = new Anonymousxcommand_t1();
		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				Globals.con.text.Fill( ( Byte ) ' ' );
			}
		}

		public static xcommand_t Dump_f = new Anonymousxcommand_t2();
		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			private readonly Con parent;
			public override void Execute( )
			{
				Int32 l, x;
				Int32 line;
				FileStream f;
				Byte[] buffer = new Byte[1024];
				String name;
				if ( Cmd.Argc() != 2 )
				{
					Com.Printf( "usage: condump <filename>\\n" );
					return;
				}

				name = FS.Gamedir() + "/" + Cmd.Argv( 1 ) + ".txt";
				Com.Printf( "Dumped console text to " + name + ".\\n" );
				FS.CreatePath( name );
				f = Lib.Fopen( name, FileMode.OpenOrCreate, FileAccess.ReadWrite );
				if ( f == null )
				{
					Com.Printf( "ERROR: couldn't open.\\n" );
					return;
				}

				for ( l = con.current - con.totallines + 1; l <= con.current; l++ )
				{
					line = ( l % con.totallines ) * con.linewidth;
					for ( x = 0; x < con.linewidth; x++ )
						if ( con.text[line + x] != ' ' )
							break;
					if ( x != con.linewidth )
						break;
				}

				buffer[con.linewidth] = 0;
				for ( ; l <= con.current; l++ )
				{
					line = ( l % con.totallines ) * con.linewidth;
					System.Array.Copy( con.text, line, buffer, 0, con.linewidth );
					for ( x = con.linewidth - 1; x >= 0; x-- )
					{
						if ( buffer[x] == ' ' )
							buffer[x] = 0;
						else
							break;
					}

					for ( x = 0; buffer[x] != 0; x++ )
						buffer[x] &= 0x7f;
					buffer[x] = ( Byte ) '\\';
					try
					{
						f.Write( buffer, 0, x + 1 );
					}
					catch ( IOException e )
					{
					}
				}

				Lib.Fclose( f );
			}
		}

		public static void Init( )
		{
			Globals.con.linewidth = -1;
			CheckResize();
			Com.Printf( "Console initialized.\\n" );
			Globals.con_notifytime = Cvar.Get( "con_notifytime", "3", 0 );
			Cmd.AddCommand( "toggleconsole", ToggleConsole_f );
			Cmd.AddCommand( "togglechat", ToggleChat_f );
			Cmd.AddCommand( "messagemode", MessageMode_f );
			Cmd.AddCommand( "messagemode2", MessageMode2_f );
			Cmd.AddCommand( "clear", Clear_f );
			Cmd.AddCommand( "condump", Dump_f );
			Globals.con.initialized = true;
		}

		public static void CheckResize( )
		{
			var width = ( Globals.viddef.GetWidth() >> 3 ) - 2;
			if ( width > Defines.MAXCMDLINE )
				width = Defines.MAXCMDLINE;
			if ( width == Globals.con.linewidth )
				return;
			if ( width < 1 )
			{
				width = 38;
				Globals.con.linewidth = width;
				Globals.con.totallines = Defines.CON_TEXTSIZE / Globals.con.linewidth;
				Globals.con.text.Fill( ( Byte ) ' ' );
			}
			else
			{
				var oldwidth = Globals.con.linewidth;
				Globals.con.linewidth = width;
				var oldtotallines = Globals.con.totallines;
				Globals.con.totallines = Defines.CON_TEXTSIZE / Globals.con.linewidth;
				var numlines = oldtotallines;
				if ( Globals.con.totallines < numlines )
					numlines = Globals.con.totallines;
				var numchars = oldwidth;
				if ( Globals.con.linewidth < numchars )
					numchars = Globals.con.linewidth;
				Byte[] tbuf = new Byte[Defines.CON_TEXTSIZE];
				System.Array.Copy( Globals.con.text, 0, tbuf, 0, Defines.CON_TEXTSIZE );
				Globals.con.text.Fill( ( Byte ) ' ' );
				for ( var i = 0; i < numlines; i++ )
				{
					for ( var j = 0; j < numchars; j++ )
					{
						Globals.con.text[( Globals.con.totallines - 1 - i ) * Globals.con.linewidth + j] = tbuf[( ( Globals.con.current - i + oldtotallines ) % oldtotallines ) * oldwidth + j];
					}
				}

				Con.ClearNotify();
			}

			Globals.con.current = Globals.con.totallines - 1;
			Globals.con.display = Globals.con.current;
		}

		public static void ClearNotify( )
		{
			Int32 i;
			for ( i = 0; i < Defines.NUM_CON_TIMES; i++ )
				Globals.con.times[i] = 0;
		}

		public static void DrawString( Int32 x, Int32 y, String s )
		{
			for ( var i = 0; i < s.Length; i++ )
			{
				Globals.re.DrawChar( x, y, s[i] );
				x += 8;
			}
		}

		public static void DrawAltString( Int32 x, Int32 y, String s )
		{
			for ( var i = 0; i < s.Length; i++ )
			{
				Globals.re.DrawChar( x, y, s[i] ^ 0x80 );
				x += 8;
			}
		}

		static xcommand_t ToggleChat_f = new Anonymousxcommand_t3();
		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			public override void Execute( )
			{
				Key.ClearTyping();
				if ( cls.key_dest == key_console )
				{
					if ( cls.state == ca_active )
					{
						Menu.ForceMenuOff();
						cls.key_dest = key_game;
					}
				}
				else
					cls.key_dest = key_console;
				ClearNotify();
			}
		}

		static xcommand_t MessageMode_f = new Anonymousxcommand_t4();
		private sealed class Anonymousxcommand_t4 : xcommand_t
		{
			public override void Execute( )
			{
				chat_team = false;
				cls.key_dest = key_message;
			}
		}

		static xcommand_t MessageMode2_f = new Anonymousxcommand_t5();
		private sealed class Anonymousxcommand_t5 : xcommand_t
		{
			private readonly Con parent;
			public override void Execute( )
			{
				chat_team = true;
				cls.key_dest = key_message;
			}
		}

		static void Linefeed( )
		{
			Globals.con.x = 0;
			if ( Globals.con.display == Globals.con.current )
				Globals.con.display++;
			Globals.con.current++;
			var i = ( Globals.con.current % Globals.con.totallines ) * Globals.con.linewidth;
			var e = i + Globals.con.linewidth;
			while ( i++ < e )
				Globals.con.text[i] = ( Byte ) ' ';
		}

		private static Int32 cr;
		public static void Print( String txt )
		{
			Int32 y;
			Int32 c, l;
			Int32 mask;
			var txtpos = 0;
			if ( !con.initialized )
				return;
			if ( txt[0] == 1 || txt[0] == 2 )
			{
				mask = 128;
				txtpos++;
			}
			else
				mask = 0;
			while ( txtpos < txt.Length )
			{
				c = txt[txtpos];
				for ( l = 0; l < con.linewidth && l < ( txt.Length - txtpos ); l++ )
					if ( txt[l + txtpos] <= ' ' )
						break;
				if ( l != con.linewidth && ( con.x + l > con.linewidth ) )
					con.x = 0;
				txtpos++;
				if ( cr != 0 )
				{
					con.current--;
					cr = 0;
				}

				if ( con.x == 0 )
				{
					Con.Linefeed();
					if ( con.current >= 0 )
						con.times[con.current % NUM_CON_TIMES] = cls.realtime;
				}

				switch ( ( Char ) c )
				{
					case '\\':
						con.x = 0;
						cr = 1;
						break;
					default:
						y = con.current % con.totallines;
						con.text[y * con.linewidth + con.x] = ( Byte ) ( c | mask | con.ormask );
						con.x++;
						if ( con.x >= con.linewidth )
							con.x = 0;
						break;
				}
			}
		}

		static void CenteredPrint( String text )
		{
			var l = text.Length;
			l = ( con.linewidth - l ) / 2;
			if ( l < 0 )
				l = 0;
			StringBuffer sb = new StringBuffer( 1024 );
			for ( var i = 0; i < l; i++ )
				sb.Append( ' ' );
			sb.Append( text );
			sb.Append( '\\' );
			sb.Length = 1024;
			Con.Print( sb.ToString() );
		}

		static void DrawInput( )
		{
			Int32 i;
			Byte[] text;
			var start = 0;
			if ( cls.key_dest == key_menu )
				return;
			if ( cls.key_dest != key_console && cls.state == ca_active )
				return;
			text = key_lines[edit_line];
			text[key_linepos] = ( Byte ) ( 10 + ( ( Int32 ) ( cls.realtime >> 8 ) & 1 ) );
			for ( i = key_linepos + 1; i < con.linewidth; i++ )
				text[i] = ( Byte ) ' ';
			if ( key_linepos >= con.linewidth )
				start += 1 + key_linepos - con.linewidth;
			for ( i = 0; i < con.linewidth; i++ )
				re.DrawChar( ( i + 1 ) << 3, con.vislines - 22, text[i] );
			key_lines[edit_line][key_linepos] = 0;
		}

		public static void DrawNotify( )
		{
			Int32 x, v;
			Int32 text;
			Int32 i;
			Int32 time;
			String s;
			Int32 skip;
			v = 0;
			for ( i = con.current - NUM_CON_TIMES + 1; i <= con.current; i++ )
			{
				if ( i < 0 )
					continue;
				time = ( Int32 ) con.times[i % NUM_CON_TIMES];
				if ( time == 0 )
					continue;
				time = ( Int32 ) ( cls.realtime - time );
				if ( time > con_notifytime.value * 1000 )
					continue;
				text = ( i % con.totallines ) * con.linewidth;
				for ( x = 0; x < con.linewidth; x++ )
					re.DrawChar( ( x + 1 ) << 3, v, con.text[text + x] );
				v += 8;
			}

			if ( cls.key_dest == key_message )
			{
				if ( chat_team )
				{
					DrawString( 8, v, "say_team:" );
					skip = 11;
				}
				else
				{
					DrawString( 8, v, "say:" );
					skip = 5;
				}

				s = chat_buffer;
				if ( chat_bufferlen > ( viddef.GetWidth() >> 3 ) - ( skip + 1 ) )
					s = s.Substring( chat_bufferlen - ( ( viddef.GetWidth() >> 3 ) - ( skip + 1 ) ) );
				for ( x = 0; x < s.Length; x++ )
				{
					re.DrawChar( ( x + skip ) << 3, v, s[x] );
				}

				re.DrawChar( ( x + skip ) << 3, v, ( Int32 ) ( 10 + ( ( cls.realtime >> 8 ) & 1 ) ) );
				v += 8;
			}

			if ( v != 0 )
			{
				SCR.AddDirtyPoint( 0, 0 );
				SCR.AddDirtyPoint( viddef.GetWidth() - 1, v );
			}
		}

		public static void DrawConsole( Single frac )
		{
			var width = viddef.GetWidth();
			var height = viddef.GetHeight();
			var lines = ( Int32 ) ( height * frac );
			if ( lines <= 0 )
				return;
			if ( lines > height )
				lines = height;
			re.DrawStretchPic( 0, -height + lines, width, height, "conback" );
			SCR.AddDirtyPoint( 0, 0 );
			SCR.AddDirtyPoint( width - 1, lines - 1 );
			var version = Com.Sprintf( "v%4.2f", 1, VERSION );
			var x = 0;
			for ( x = 0; x < 5; x++ )
				re.DrawChar( width - 44 + x * 8, lines - 12, 128 + version[x] );
			con.vislines = lines;
			var rows = ( lines - 22 ) >> 3;
			var y = lines - 30;
			if ( con.display != con.current )
			{
				for ( x = 0; x < con.linewidth; x += 4 )
					re.DrawChar( ( x + 1 ) << 3, y, '^' );
				y -= 8;
				rows--;
			}

			Int32 i, j, n;
			x = 0;
			var row = con.display;
			for ( i = 0; i < rows; i++, y -= 8, row-- )
			{
				if ( row < 0 )
					break;
				if ( con.current - row >= con.totallines )
					break;
				var first = ( row % con.totallines ) * con.linewidth;
				for ( x = 0; x < con.linewidth; x++ )
					re.DrawChar( ( x + 1 ) << 3, y, con.text[x + first] );
			}

			if ( cls.download != null )
			{
				Int32 text;
				if ( ( text = cls.downloadname.LastIndexOf( '/' ) ) != 0 )
					text++;
				else
					text = 0;
				x = con.linewidth - ( ( con.linewidth * 7 ) / 40 );
				y = x - ( cls.downloadname.Length - text ) - 8;
				i = con.linewidth / 3;
				StringBuffer dlbar = new StringBuffer( 512 );
				if ( cls.downloadname.Length - text > i )
				{
					y = x - i - 11;
					var end = text + i - 1;
					dlbar.Append( cls.downloadname.Substring( text, end ) );
					dlbar.Append( "..." );
				}
				else
				{
					dlbar.Append( cls.downloadname.Substring( text ) );
				}

				dlbar.Append( ": " );
				dlbar.Append( ( Char ) 0x80 );
				if ( cls.downloadpercent == 0 )
					n = 0;
				else
					n = y * cls.downloadpercent / 100;
				for ( j = 0; j < y; j++ )
				{
					if ( j == n )
						dlbar.Append( ( Char ) 0x83 );
					else
						dlbar.Append( ( Char ) 0x81 );
				}

				dlbar.Append( ( Char ) 0x82 );
				dlbar.Append( ( cls.downloadpercent < 10 ) ? " 0" : " " );
				dlbar.Append( cls.downloadpercent ).Append( '%' );
				y = con.vislines - 12;
				for ( i = 0; i < dlbar.Length; i++ )
					re.DrawChar( ( i + 1 ) << 3, y, dlbar[i] );
			}

			DrawInput();
		}
	}
}
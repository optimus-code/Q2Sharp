using J2N.IO;
using J2N.Text;
using Q2Sharp.Game;
using Q2Sharp.Sys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Q2Sharp.Qcommon
{
	public sealed class FS : Globals
	{
		public class packfile_t
		{
			public static readonly Int32 SIZE = 64;
			public static readonly Int32 NAME_SIZE = 56;
			public String name;
			public Int32 filepos, filelen;
			public override String ToString( )
			{
				return name + " [ length: " + filelen + " pos: " + filepos + " ]";
			}
		}

		public class pack_t
		{
			public String filename;
			public FileStream handle;
			public ByteBuffer backbuffer;
			public Int32 numfiles;
			public Hashtable files;
		}

		public static String fs_gamedir;
		private static String fs_userdir;
		public static cvar_t fs_basedir;
		public static cvar_t fs_cddir;
		public static cvar_t fs_gamedirvar;
		public class filelink_t
		{
			public String from;
			public Int32 fromlength;
			public String to;
		}

		public static LinkedList<filelink_t> fs_links = new LinkedList<filelink_t>();
		public class searchpath_t
		{
			public String filename;
			public pack_t pack;
			public searchpath_t next;
		}

		public static searchpath_t fs_searchpaths;
		public static searchpath_t fs_base_searchpaths;
		public static void CreatePath( String path )
		{
			var index = path.LastIndexOf( '/' );
			if ( index > 0 )
			{
				try
				{
					var dirInfo = Directory.CreateDirectory( path );


					if ( !dirInfo.Exists )
						Com.Printf( "can't create path \\\"" + path + '"' + "\\n" );
				}
				catch ( Exception e )
				{
					Com.Printf( "can't create path \\\"" + path + '"' + " due to the following reason '" + e.ToString() + "'\\n" );
				}
			}
		}

		public static void FCloseFile( FileStream file )
		{
			file.Close();
		}

		public static void FCloseFile( Stream stream )
		{
			stream.Close();
		}

		public static Int32 FileLength( String filename )
		{
			searchpath_t search;
			String netpath;
			pack_t pak;
			file_from_pak = 0;
			foreach ( var link in fs_links )
			{
				if ( filename.RegionMatches( 0, link.from, 0, link.fromlength, StringComparison.InvariantCulture ) )
				{
					netpath = link.to + filename.Substring( link.fromlength );
					FileInfo file = new FileInfo( netpath );
					if ( file.Attributes == FileAttributes.ReadOnly && file.Attributes == FileAttributes.Normal )
					{
						Com.DPrintf( "link file: " + netpath + '\\' );
						return ( Int32 ) file.Length;
					}

					return -1;
				}
			}

			for ( search = fs_searchpaths; search != null; search = search.next )
			{
				if ( search.pack != null )
				{
					pak = search.pack;
					filename = filename.ToLower();
					packfile_t entry = ( packfile_t ) pak.files[filename];
					if ( entry != null )
					{
						file_from_pak = 1;
						Com.DPrintf( "PackFile: " + pak.filename + " : " + filename + '\\' );
						FileInfo file = new FileInfo( pak.filename );
						if ( file.Attributes != FileAttributes.ReadOnly && file.Attributes != FileAttributes.Normal )
						{
							Com.Error( Defines.ERR_FATAL, "Couldn't reopen " + pak.filename );
						}

						return entry.filelen;
					}
				}
				else
				{
					netpath = search.filename + '/' + filename;
					FileInfo file = new FileInfo( netpath );
					if ( file.Attributes != FileAttributes.ReadOnly && file.Attributes != FileAttributes.Normal )
						continue;
					Com.DPrintf( "FindFile: " + netpath + '\\' );
					return ( Int32 ) file.Length;
				}
			}

			Com.DPrintf( "FindFile: can't find " + filename + '\\' );
			return -1;
		}

		public static Int32 file_from_pak = 0;
		public static FileStream FOpenFile( String filename )
		{
			searchpath_t search;
			String netpath;
			pack_t pak;
			FileInfo file = null;
			file_from_pak = 0;
			foreach ( var link in fs_links )
			{
				if ( filename.RegionMatches( 0, link.from, 0, link.fromlength, StringComparison.InvariantCulture ) )
				{
					netpath = link.to + filename.Substring( link.fromlength );
					file = new FileInfo( netpath );
					if ( file.Attributes == FileAttributes.ReadOnly || file.Attributes == FileAttributes.Normal )
					{
						return File.OpenRead( file.FullName );
					}

					return null;
				}
			}

			for ( search = fs_searchpaths; search != null; search = search.next )
			{
				if ( search.pack != null )
				{
					pak = search.pack;
					filename = filename.ToLower();
					packfile_t entry = ( packfile_t ) pak.files[filename];
					if ( entry != null )
					{
						file_from_pak = 1;
						file = new FileInfo( pak.filename );
						if ( file.Attributes != FileAttributes.ReadOnly && file.Attributes != FileAttributes.Normal )
							Com.Error( Defines.ERR_FATAL, "Couldn't reopen " + pak.filename );
						if ( pak.handle == null || !pak.handle.CanRead )
						{
							pak.handle = File.OpenRead( pak.filename );
						}

						FileStream raf = File.OpenRead( file.FullName );
						raf.Position = entry.filepos;
						return raf;
					}
				}
				else
				{
					netpath = search.filename + '/' + filename;
					file = new FileInfo( netpath );
					if ( file.Attributes != FileAttributes.ReadOnly && file.Attributes != FileAttributes.Normal )
						continue;
					return File.OpenRead( file.FullName );
				}
			}

			return null;
		}

		public static readonly Int32 MAX_READ = 0x10000;
		public static void Read( Byte[] buffer, Int32 len, FileStream f )
		{
			var offset = 0;
			var read = 0;
			var remaining = len;
			Int32 block;
			while ( remaining != 0 )
			{
				block = Math.Min( remaining, MAX_READ );
				try
				{
					read = f.Read( buffer, offset, block );
				}
				catch ( IOException e )
				{
					Com.Error( Defines.ERR_FATAL, e.ToString() );
				}

				if ( read == 0 )
				{
					Com.Error( Defines.ERR_FATAL, "FS_Read: 0 bytes read" );
				}
				else if ( read == -1 )
				{
					Com.Error( Defines.ERR_FATAL, "FS_Read: -1 bytes read" );
				}

				remaining -= read;
				offset += read;
			}
		}

		public static Byte[] LoadFile( String path )
		{
			FileStream file;
			Byte[] buf = null;
			var len = 0;
			var index = path.IndexOf( '\\' );
			if ( index != -1 )
				path = path.Substring( 0, index );
			len = FileLength( path );
			if ( len < 1 )
				return null;
			try
			{
				file = FOpenFile( path );
				buf = new Byte[len];
				file.Read( buf, 0, len );
				file.Close();
			}
			catch ( IOException e )
			{
				Com.Error( Defines.ERR_FATAL, e.ToString() );
			}

			return buf;
		}

		public static ByteBuffer LoadMappedFile( String filename )
		{
			searchpath_t search;
			String netpath;
			pack_t pak;
			FileInfo file = null;
			var fileLength = 0;
			FileChannel channel = null;
			FileInputStream input = null;
			ByteBuffer buffer = null;
			file_from_pak = 0;
			try
			{
				foreach ( var link in fs_links )
				{
					if ( filename.RegionMatches( 0, link.from, 0, link.fromlength, StringComparison.InvariantCulture ) )
					{
						netpath = link.to + filename.Substring( link.fromlength );
						file = new FileInfo( netpath );
						if ( file.Attributes == FileAttributes.ReadOnly || file.Attributes == FileAttributes.Normal )
						{
							input = new FileInputStream( file );
							channel = input.GetChannel();
							fileLength = ( Int32 ) channel.Size();
							buffer = channel.Map( FileChannel.MapMode.READ_ONLY, 0, fileLength );
							input.Close();
							return buffer;
						}

						return null;
					}
				}

				for ( search = fs_searchpaths; search != null; search = search.next )
				{
					if ( search.pack != null )
					{
						pak = search.pack;
						filename = filename.ToLower();
						packfile_t entry = ( packfile_t ) pak.files[filename];
						if ( entry != null )
						{
							file_from_pak = 1;
							var fileInfo = new FileInfo( pak.filename );
							if ( !( fileInfo.Attributes == FileAttributes.ReadOnly || fileInfo.Attributes == FileAttributes.Normal ) )
								Com.Error( Defines.ERR_FATAL, "Couldn't reopen " + pak.filename );
							if ( pak.handle == null || !pak.handle.CanRead )
							{
								pak.handle = File.OpenRead( pak.filename );
							}

							if ( pak.backbuffer == null )
							{
								channel = pak.handle.GetChannel();
								pak.backbuffer = channel.Map( FileChannel.MapMode.READ_ONLY, 0, pak.handle.Length );
								channel.Close();
							}

							pak.backbuffer.Position = entry.filepos;
							buffer = pak.backbuffer.Slice();
							buffer.Limit = entry.filelen;
							return buffer;
						}
					}
					else
					{
						netpath = search.filename + '/' + filename;
						file = new FileInfo( netpath );
						if ( file.Attributes != FileAttributes.ReadOnly && file.Attributes != FileAttributes.Normal )
							continue;
						input = new FileInputStream( file );
						channel = input.GetChannel();
						fileLength = ( Int32 ) channel.Size();
						buffer = channel.Map( FileChannel.MapMode.READ_ONLY, 0, fileLength );
						input.Close();
						return buffer;
					}
				}
			}
			catch ( Exception e )
			{
			}

			try
			{
				if ( input != null )
					input.Close();
				else if ( channel != null && channel.IsOpen() )
					channel.Close();
			}
			catch ( IOException ioe )
			{
			}

			return null;
		}

		public static void FreeFile( Byte[] buffer )
		{
			buffer = null;
		}

		static readonly Int32 IDPAKHEADER = ( ( 'K' << 24 ) + ( 'C' << 16 ) + ( 'A' << 8 ) + 'P' );
		public class dpackheader_t
		{
			public Int32 ident;
			public Int32 dirofs;
			public Int32 dirlen;
		}

		static readonly Int32 MAX_FILES_IN_PACK = 4096;
		static Byte[] tmpText = new Byte[packfile_t.NAME_SIZE];
		static pack_t LoadPackFile( String packfile )
		{
			dpackheader_t header;
			Hashtable newfiles;
			FileStream file;
			var numpackfiles = 0;
			pack_t pack = null;
			try
			{
				file = File.OpenRead( packfile );
				FileChannel fc = file.GetChannel();
				ByteBuffer packhandle = fc.Map( FileChannel.MapMode.READ_ONLY, 0, file.Length );
				packhandle.Order = ByteOrder.LittleEndian;
				fc.Close();
				if ( packhandle == null || packhandle.Limit < 1 )
					return null;
				header = new dpackheader_t();
				header.ident = packhandle.GetInt32();
				header.dirofs = packhandle.GetInt32();
				header.dirlen = packhandle.GetInt32();
				if ( header.ident != IDPAKHEADER )
					Com.Error( Defines.ERR_FATAL, packfile + " is not a packfile" );
				numpackfiles = header.dirlen / packfile_t.SIZE;
				if ( numpackfiles > MAX_FILES_IN_PACK )
					Com.Error( Defines.ERR_FATAL, packfile + " has " + numpackfiles + " files" );
				newfiles = new Hashtable( numpackfiles );
				packhandle.Position = header.dirofs;
				packfile_t entry = null;
				for ( var i = 0; i < numpackfiles; i++ )
				{
					packhandle.Get( tmpText );
					entry = new packfile_t();
					entry.name = Encoding.ASCII.GetString( tmpText ).Trim();
					entry.filepos = packhandle.GetInt32();
					entry.filelen = packhandle.GetInt32();
					newfiles.Add( entry.name.ToLower(), entry );
				}
			}
			catch ( Exception e )
			{
				Com.DPrintf( e.Message + '\\' );
				return null;
			}

			pack = new pack_t();
			pack.filename = new String( packfile );
			pack.handle = file;
			pack.numfiles = numpackfiles;
			pack.files = newfiles;
			Com.Printf( "Added packfile " + packfile + " (" + numpackfiles + " files)\\n" );
			return pack;
		}

		static void AddGameDirectory( String dir )
		{
			Int32 i;
			searchpath_t search;
			pack_t pak;
			String pakfile;
			fs_gamedir = new String( dir );
			search = new searchpath_t();
			search.filename = new String( dir );
			if ( fs_searchpaths != null )
			{
				search.next = fs_searchpaths.next;
				fs_searchpaths.next = search;
			}
			else
			{
				fs_searchpaths = search;
			}

			for ( i = 0; i < 10; i++ )
			{
				pakfile = dir + "/pak" + i + ".pak";
				var fileInfo = new FileInfo( pakfile );
				if ( !( fileInfo.Attributes == FileAttributes.ReadOnly || fileInfo.Attributes == FileAttributes.Normal ) )
					continue;
				pak = LoadPackFile( pakfile );
				if ( pak == null )
					continue;
				search = new searchpath_t();
				search.pack = pak;
				search.filename = "";
				search.next = fs_searchpaths;
				fs_searchpaths = search;
			}
		}

		public static String Gamedir( )
		{
			return ( fs_userdir != null ) ? fs_userdir : Globals.BASEDIRNAME;
		}

		public static String BaseGamedir( )
		{
			return ( fs_gamedir != null ) ? fs_gamedir : Globals.BASEDIRNAME;
		}

		public static void ExecAutoexec( )
		{
			var dir = fs_userdir;
			String name;
			if ( dir != null && dir.Length > 0 )
			{
				name = dir + "/autoexec.cfg";
			}
			else
			{
				name = fs_basedir.string_renamed + '/' + Globals.BASEDIRNAME + "/autoexec.cfg";
			}

			var canthave = Defines.SFF_SUBDIR | Defines.SFF_HIDDEN | Defines.SFF_SYSTEM;
			if ( CoreSys.FindAll( name ) != null )
			{
				Cbuf.AddText( "exec autoexec.cfg\\n" );
			}
		}

		public static void SetGamedir( String dir )
		{
			searchpath_t next;
			if ( dir.IndexOf( ".." ) != -1 || dir.IndexOf( "/" ) != -1 || dir.IndexOf( "\\\\" ) != -1 || dir.IndexOf( ":" ) != -1 )
			{
				Com.Printf( "Gamedir should be a single filename, not a path\\n" );
				return;
			}

			while ( fs_searchpaths != fs_base_searchpaths )
			{
				if ( fs_searchpaths.pack != null )
				{
					try
					{
						fs_searchpaths.pack.handle.Close();
					}
					catch ( IOException e )
					{
						Com.DPrintf( e.Message + '\\' );
					}

					fs_searchpaths.pack.files.Clear();
					fs_searchpaths.pack.files = null;
					fs_searchpaths.pack = null;
				}

				next = fs_searchpaths.next;
				fs_searchpaths = null;
				fs_searchpaths = next;
			}

			if ( ( Globals.dedicated != null ) && ( Globals.dedicated.value == 0F ) )
				Cbuf.AddText( "vid_restart\\nsnd_restart\\n" );
			fs_gamedir = fs_basedir.string_renamed + '/' + dir;
			if ( dir.Equals( Globals.BASEDIRNAME ) || ( dir.Length == 0 ) )
			{
				Cvar.FullSet( "gamedir", "", CVAR_SERVERINFO | CVAR_NOSET );
				Cvar.FullSet( "game", "", CVAR_LATCH | CVAR_SERVERINFO );
			}
			else
			{
				Cvar.FullSet( "gamedir", dir, CVAR_SERVERINFO | CVAR_NOSET );
				if ( fs_cddir.string_renamed != null && fs_cddir.string_renamed.Length > 0 )
					AddGameDirectory( fs_cddir.string_renamed + '/' + dir );
				AddGameDirectory( fs_basedir.string_renamed + '/' + dir );
			}
		}

		public static void Link_f( )
		{
			if ( Cmd.Argc() != 3 )
			{
				Com.Printf( "USAGE: link <from> <to>\\n" );
				return;
			}

			foreach ( var entry in fs_links.ToArray() )
			{
				if ( entry.from.Equals( Cmd.Argv( 1 ) ) )
				{
					if ( Cmd.Argv( 2 ).Length < 1 )
					{
						fs_links.Remove( entry );
						return;
					}

					entry.to = new String( Cmd.Argv( 2 ) );
					return;
				}
			}

			if ( Cmd.Argv( 2 ).Length > 0 )
			{
				var entry = new filelink_t();
				entry.from = new String( Cmd.Argv( 1 ) );
				entry.fromlength = entry.from.Length;
				entry.to = new String( Cmd.Argv( 2 ) );
				fs_links.AddLast( entry );
			}
		}

		public static String[] ListFiles( String findname, Int32 musthave, Int32 canthave )
		{
			String[] list = null;
			String[] files = CoreSys.FindAll( findname/*, musthave, canthave*/);
			if ( files != null )
			{
				list = new String[files.Length];
				for ( var i = 0; i < files.Length; i++ )
				{
					list[i] = files[i];
				}
			}

			return list;
		}

		public static void Dir_f( )
		{
			String path = null;
			String findname = null;
			var wildcard = "*.*";
			String[] dirnames;
			if ( Cmd.Argc() != 1 )
			{
				wildcard = Cmd.Argv( 1 );
			}

			while ( ( path = NextPath( path ) ) != null )
			{
				var tmp = findname;
				findname = path + '/' + wildcard;
				if ( tmp != null )
					tmp.Replace( "\\\\\\\\", "/" );
				Com.Printf( "Directory of " + findname + '\\' );
				Com.Printf( "----\\n" );
				dirnames = ListFiles( findname, 0, 0 );
				if ( dirnames != null )
				{
					var index = 0;
					for ( var i = 0; i < dirnames.Length; i++ )
					{
						if ( ( index = dirnames[i].LastIndexOf( '/' ) ) > 0 )
						{
							Com.Printf( dirnames[i].Substring( index + 1, dirnames[i].Length ) + '\\' );
						}
						else
						{
							Com.Printf( dirnames[i] + '\\' );
						}
					}
				}

				Com.Printf( "\\n" );
			}
		}

		public static void Path_f( )
		{
			searchpath_t s;
			Com.Printf( "Current search path:\\n" );
			for ( s = fs_searchpaths; s != null; s = s.next )
			{
				if ( s == fs_base_searchpaths )
					Com.Printf( "----------\\n" );
				if ( s.pack != null )
					Com.Printf( s.pack.filename + " (" + s.pack.numfiles + " files)\\n" );
				else
					Com.Printf( s.filename + '\\' );
			}

			Com.Printf( "\\nLinks:\\n" );
			foreach ( var link in fs_links )
			{
				Com.Printf( link.from + " : " + link.to + '\\' );
			}
		}

		public static String NextPath( String prevpath )
		{
			searchpath_t s;
			String prev;
			if ( prevpath == null || prevpath.Length == 0 )
				return fs_gamedir;
			prev = fs_gamedir;
			for ( s = fs_searchpaths; s != null; s = s.next )
			{
				if ( s.pack != null )
					continue;
				if ( prevpath == prev )
					return s.filename;
				prev = s.filename;
			}

			return null;
		}

		public static void InitFilesystem( )
		{
			Cmd.AddCommand( "path", new Anonymousxcommand_t() );
			Cmd.AddCommand( "link", new Anonymousxcommand_t1() );
			Cmd.AddCommand( "dir", new Anonymousxcommand_t2() );
			fs_userdir = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ) + "/.jake2";
			FS.CreatePath( fs_userdir + "/" );
			FS.AddGameDirectory( fs_userdir );
			fs_basedir = Cvar.Get( "basedir", ".", CVAR_NOSET );
			SetCDDir();
			AddGameDirectory( fs_basedir.string_renamed + '/' + Globals.BASEDIRNAME );
			MarkBaseSearchPaths();
			fs_gamedirvar = Cvar.Get( "game", "", CVAR_LATCH | CVAR_SERVERINFO );
			if ( fs_gamedirvar.string_renamed.Length > 0 )
				SetGamedir( fs_gamedirvar.string_renamed );
		}

		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				Path_f();
			}
		}

		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				Link_f();
			}
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				Dir_f();
			}
		}

		public static void SetCDDir( )
		{
			fs_cddir = Cvar.Get( "cddir", "", CVAR_ARCHIVE );
			if ( fs_cddir.string_renamed.Length > 0 )
				AddGameDirectory( fs_cddir.string_renamed );
		}

		public static void MarkBaseSearchPaths( )
		{
			fs_base_searchpaths = fs_searchpaths;
		}

		public static Int32 Developer_searchpath( Int32 who )
		{
			searchpath_t s;
			for ( s = fs_searchpaths; s != null; s = s.next )
			{
				if ( s.filename.IndexOf( "xatrix" ) != -1 )
					return 1;
				if ( s.filename.IndexOf( "rogue" ) != -1 )
					return 2;
			}

			return 0;
		}
	}
}
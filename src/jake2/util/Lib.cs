using J2N.IO;
using J2N.Text;
using Jake2.Qcommon;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.IO;
using System.Text;

namespace Jake2.Util
{
	public static class Lib
	{
		public static String Vtos( Single[] v )
		{
			return ( Int32 ) v[0] + " " + ( Int32 ) v[1] + " " + ( Int32 ) v[2];
		}

		public static String Vtofs( Single[] v )
		{
			return v[0] + " " + v[1] + " " + v[2];
		}

		public static String Vtofsbeaty( Single[] v )
		{
			return Com.Sprintf( "%8.2f %8.2f %8.2f", v[0], v[1], v[2] );
		}

		public static Int16 Rand( )
		{
			return ( Int16 ) Globals.rnd.Next( Int16.MaxValue + 1 );
		}

		public static Single Crandom( )
		{
			return ( ( Single ) Globals.rnd.NextDouble() - 0.5F ) * 2F;
		}

		public static Single Random( )
		{
			return ( Single ) Globals.rnd.NextDouble();
		}

		public static Single Crand( )
		{
			return ( Single ) ( Globals.rnd.NextDouble() - 0.5F ) * 2F;
		}

		public static Int32 Strcmp( String in1, String in2 )
		{
			return in1.CompareTo( in2 );
		}

		public static Single Atof( String in_renamed )
		{
			if ( in_renamed == null || in_renamed.Length == 0 )
				return 0;
			try
			{
				return Single.Parse( in_renamed );
			}
			catch ( Exception e )
			{
				return 0;
			}
		}

		public static Int32 Q_stricmp( String in1, String in2 )
		{
			return String.Compare( in1, in2, true );
		}

		public static Int32 Atoi( String in_renamed )
		{
			try
			{
				return Int32.Parse( in_renamed );
			}
			catch ( Exception e )
			{
				try
				{
					return ( Int32 ) Double.Parse( in_renamed );
				}
				catch ( Exception e1 )
				{
					return 0;
				}
			}
		}

		public static Single[] Atov( String v )
		{
			Single[] res = new[] { 0f, 0f, 0f };
			String[] strres = v.Split( " " );
			for ( var n = 0; n < 3 && n < strres.Length; n++ )
			{
				res[n] = Atof( strres[n] );
			}

			return res;
		}

		public static Int32 Strlen( Char[] in_renamed )
		{
			for ( var i = 0; i < in_renamed.Length; i++ )
				if ( in_renamed[i] == 0 )
					return i;
			return in_renamed.Length;
		}

		public static Int32 Strlen( Byte[] in_renamed )
		{
			for ( var i = 0; i < in_renamed.Length; i++ )
				if ( in_renamed[i] == 0 )
					return i;
			return in_renamed.Length;
		}

		public static String Hexdumpfile( ByteBuffer bb, Int32 len )
		{
			ByteBuffer bb1 = bb.Slice();
			Byte[] buf = new Byte[len];
			bb1.Get( buf );
			return HexDump( buf, len, false );
		}

		public static Boolean EqualsIgnoreCase( this String text, String comparison )
		{
			return String.Equals( text, comparison, StringComparison.InvariantCultureIgnoreCase );
		}

		public static String HexDump( Byte[] data1, Int32 len, Boolean showAddress )
		{
			StringBuffer result = new StringBuffer();
			StringBuffer charfield = new StringBuffer();
			var i = 0;
			while ( i < len )
			{
				if ( ( i & 0xf ) == 0 )
				{
					if ( showAddress )
					{
						var address = i.ToString( "X4" );
						address = ( "0000".Substring( 0, 4 - address.Length ) + address ).ToUpper();
						result.Append( address + ": " );
					}
				}

				Int32 v = data1[i];
				result.Append( Hex2( v ) );
				result.Append( " " );
				charfield.Append( ReadableChar( v ) );
				i++;
				if ( ( i & 0xf ) == 0 )
				{
					result.Append( charfield );
					result.Append( "\\n" );
					charfield.Length = 0;
				}
				else if ( ( i & 0xf ) == 8 )
				{
					result.Append( " " );
				}
			}

			return result.ToString();
		}

		public static String Hex2( Int32 i )
		{
			var val = ( i & 0xff ).ToString( "X4" );
			return ( "00".Substring( 0, 2 - val.Length ) + val ).ToUpper();
		}

		public static Char ReadableChar( Int32 i )
		{
			if ( ( i < 0x20 ) || ( i > 0x7f ) )
				return '.';
			else
				return ( Char ) i;
		}

		public static void Printv( String in_renamed, Single[] arr )
		{
			for ( var n = 0; n < arr.Length; n++ )
			{
				Com.Println( in_renamed + "[" + n + "]: " + arr[n] );
			}
		}

		static readonly Byte[] nullfiller = new Byte[8192];
		public static void FwriteString( String s, Int32 len, FileStream f )
		{
			if ( s == null )
				return;
			var diff = len - s.Length;
			if ( diff > 0 )
			{
				f.Write( StringToBytes( s ) );
				f.Write( nullfiller, 0, diff );
			}
			else
				f.Write( StringToBytes( s ), 0, len );
		}

		public static FileStream Fopen( String name, FileMode mode, FileAccess access )
		{
			try
			{
				return File.Open( name, mode, access );
			}
			catch ( Exception e )
			{
				Com.DPrintf( "Could not open file:" + name );
				return null;
			}
		}

		public static void Fclose( FileStream f )
		{
			try
			{
				f.Close();
			}
			catch ( Exception e )
			{
			}
		}

		public static String FreadString( FileStream f, Int32 len )
		{
			Byte[] buffer = new Byte[len];
			FS.Read( buffer, len, f );
			return Lib.CtoJava( buffer );
		}

		public static String RightFrom( String in_renamed, Char c )
		{
			var pos = in_renamed.LastIndexOf( c );
			if ( pos == -1 )
				return "";
			else if ( pos < in_renamed.Length )
				return in_renamed.Substring( pos + 1, in_renamed.Length );
			return "";
		}

		public static String LeftFrom( String in_renamed, Char c )
		{
			var pos = in_renamed.LastIndexOf( c );
			if ( pos == -1 )
				return "";
			else if ( pos < in_renamed.Length )
				return in_renamed.Substring( 0, pos );
			return "";
		}

		public static Int32 Rename( String oldn, String newn )
		{
			try
			{
				File.Copy( oldn, newn );
				File.Delete( oldn );
				return 0;
			}
			catch ( Exception e )
			{
				return 1;
			}
		}

		public static Byte[] GetIntBytes( Int32 c )
		{
			Byte[] b = new Byte[4];
			b[0] = ( Byte ) ( ( c & 0xff ) );
			b[1] = ( Byte ) ( ( c >> 8 ) & 0xff );
			b[2] = ( Byte ) ( ( c >> 16 ) & 0xff );
			b[3] = ( Byte ) ( ( c >> 24 ) & 0xff );
			return b;
		}

		public static Int32 GetInt( Byte[] b )
		{
			return ( b[0] & 0xff ) | ( ( b[1] & 0xff ) << 8 ) | ( ( b[2] & 0xff ) << 16 ) | ( ( b[3] & 0xff ) << 24 );
		}

		public static Single[] Clone( Single[] in_renamed )
		{
			Single[] out_renamed = new Single[in_renamed.Length];
			if ( in_renamed.Length != 0 )
				System.Array.Copy( in_renamed, 0, out_renamed, 0, in_renamed.Length );
			return out_renamed;
		}

		public static Byte[] StringToBytes( String value )
		{
			return Encoding.ASCII.GetBytes( value );
		}

		public static String BytesToString( Byte[] value )
		{
			return Encoding.ASCII.GetString( value );
		}

		public static String CtoJava( String old )
		{
			var index = old.IndexOf( '\\' );
			if ( index == 0 )
				return "";
			return ( index > 0 ) ? old.Substring( 0, index ) : old;
		}

		public static String CtoJava( Byte[] old )
		{
			return CtoJava( old, 0, old.Length );
		}

		public static String CtoJava( Byte[] old, Int32 offset, Int32 maxLenght )
		{
			if ( old.Length == 0 || old[0] == 0 )
				return "";
			Int32 i;
			return Encoding.ASCII.GetString( old, offset, maxLenght );
		}

		public static readonly Int32 SIZEOF_FLOAT = 4;
		public static readonly Int32 SIZEOF_INT = 4;
		public static SingleBuffer NewSingleBuffer( Int32 numElements )
		{
			ByteBuffer bb = NewByteBuffer( numElements * SIZEOF_FLOAT );
			return bb.AsSingleBuffer();
		}

		public static SingleBuffer NewSingleBuffer( Int32 numElements, ByteOrder order )
		{
			ByteBuffer bb = NewByteBuffer( numElements * SIZEOF_FLOAT, order );
			return bb.AsSingleBuffer();
		}

		public static Int32Buffer NewInt32Buffer( Int32 numElements )
		{
			ByteBuffer bb = NewByteBuffer( numElements * SIZEOF_INT );
			return bb.AsInt32Buffer();
		}

		public static Int32Buffer NewInt32Buffer( Int32 numElements, ByteOrder order )
		{
			ByteBuffer bb = NewByteBuffer( numElements * SIZEOF_INT, order );
			return bb.AsInt32Buffer();
		}

		public static ByteBuffer NewByteBuffer( Int32 numElements )
		{
			ByteBuffer bb = ByteBuffer.Allocate( numElements );
			bb.SetOrder( ByteOrder.NativeOrder );
			return bb;
		}

		public static ByteBuffer NewByteBuffer( Int32 numElements, ByteOrder order )
		{
			ByteBuffer bb = ByteBuffer.Allocate( numElements );
			bb.SetOrder( order );
			return bb;
		}

		public static T CreateJaggedArray<T>( params Int32[] lengths )
		{
			return ( T ) InitializeJaggedArray( typeof( T ).GetElementType(), 0, lengths );
		}

		public static Object InitializeJaggedArray( Type type, Int32 index, Int32[] lengths )
		{
			Array array = Array.CreateInstance( type, lengths[index] );
			Type elementType = type.GetElementType();

			if ( elementType != null )
			{
				for ( var i = 0; i < lengths[index]; i++ )
				{
					array.SetValue(
						InitializeJaggedArray( elementType, index + 1, lengths ), i );
				}
			}

			return array;
		}

		public static void PrintStackTrace( this Exception e )
		{
			System.Console.WriteLine( e.ToString() );
		}

		public static Int32 GetBitDepth( this VideoMode v )
		{
			return v.RedBits + v.GreenBits + v.BlueBits;
		}

		public static Int32 Digit( Char value, Int32 radix )
		{
			if ( ( radix <= 0 ) || ( radix > 36 ) )
				return -1; // Or throw exception

			if ( radix <= 10 )
				if ( value >= '0' && value < '0' + radix )
					return value - '0';
				else
					return -1;
			else if ( value >= '0' && value <= '9' )
				return value - '0';
			else if ( value >= 'a' && value < 'a' + radix - 10 )
				return value - 'a' + 10;
			else if ( value >= 'A' && value < 'A' + radix - 10 )
				return value - 'A' + 10;

			return -1;
		}

		public static void Fill<T>( this T[] array, T value )
		{
			for ( var i = 0; i < array.Length; i++ )
				array[i] = value;
		}

		public static void Fill<T>( this T[] array, Int32 startIndex, Int32 count, T value )
		{
			for ( var i = startIndex; i < count; i++ )
				array[i] = value;
		}

	}
}
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using System;
using System.IO;

namespace Q2Sharp.Util
{
	public class QuakeFile : IDisposable
	{
		public FileStream Stream
		{
			get;
			private set;
		}

		private BinaryReader Input
		{
			get;
			set;
		}

		private BinaryWriter Output
		{
			get;
			set;
		}

		public Int64 Length
		{
			get
			{
				return Stream.Length;
			}
		}

		public QuakeFile( String filename, FileAccess access )
		{
			var isWriting = access == FileAccess.Write || access == FileAccess.ReadWrite;
			var isReading = access == FileAccess.Read || access == FileAccess.ReadWrite;

			Stream = File.Open( filename, isWriting ? FileMode.OpenOrCreate : FileMode.Open, access );

			if ( isReading )
				Input = new BinaryReader( Stream );

			if ( isWriting )
				Output = new BinaryWriter( Stream );
		}

		public virtual void Seek( Int64 offset )
		{
			if ( Input != null )
				Input.BaseStream.Seek( offset, SeekOrigin.Begin );
			if ( Output != null )
				Output.BaseStream.Seek( offset, SeekOrigin.Begin );
		}

		public virtual void Close( )
		{
			Input?.Close();
			Output?.Close();
			Stream.Close();
		}

		public virtual void Dispose( )
		{
			Input?.Dispose();
			Output?.Dispose();
			Stream.Dispose();
		}

		public virtual Boolean ReadBoolean( )
		{
			return Input.ReadBoolean();
		}

		public virtual Byte ReadByte( )
		{
			return Input.ReadByte();
		}

		public virtual Byte[] ReadBytes( Int32 count )
		{
			return Input.ReadBytes( count );
		}

		public virtual Char ReadChar( )
		{
			return Input.ReadChar();
		}

		public virtual Char[] ReadChars( Int32 count )
		{
			return Input.ReadChars( count );
		}

		public virtual Decimal ReadDecimal( )
		{
			return Input.ReadDecimal();
		}

		public virtual Double ReadDouble( )
		{
			return Input.ReadDouble();
		}

		public virtual Int16 ReadInt16( )
		{
			return Input.ReadInt16();
		}

		public virtual Int32 ReadInt32( )
		{
			return Input.ReadInt32();
		}

		public virtual Int64 ReadInt64( )
		{
			return Input.ReadInt16();
		}

		public virtual SByte ReadSByte( )
		{
			return Input.ReadSByte();
		}

		public virtual Single ReadSingle( )
		{
			return Input.ReadSingle();
		}

		public virtual String ReadString( )
		{
			return Input.ReadString();
		}

		public virtual UInt16 ReadUInt16( )
		{
			return Input.ReadUInt16();
		}

		public virtual UInt32 ReadUInt32( )
		{
			return Input.ReadUInt32();
		}

		public virtual UInt64 ReadUInt64( )
		{
			return Input.ReadUInt64();
		}

		public virtual void Write( UInt64 value )
		{
			Output.Write( value );
		}

		public virtual void Write( UInt32 value )
		{
			Output.Write( value );
		}

		public virtual void Write( UInt16 value )
		{
			Output.Write( value );
		}

		public virtual void Write( String value )
		{
			Output.Write( value );
		}

		public virtual void Write( Single value )
		{
			Output.Write( value );
		}

		public virtual void Write( SByte value )
		{
			Output.Write( value );
		}

		public virtual void Write( Int64 value )
		{
			Output.Write( value );
		}

		public virtual void Write( Int32 value )
		{
			Output.Write( value );
		}

		public virtual void Write( Double value )
		{
			Output.Write( value );
		}

		public virtual void Write( Decimal value )
		{
			Output.Write( value );
		}

		public virtual void Write( Char[] chars, Int32 index, Int32 count )
		{
			Output.Write( chars, index, count );
		}

		public virtual void Write( Char[] chars )
		{
			Output.Write( chars );
		}

		public virtual void Write( Byte[] buffer, Int32 index, Int32 count )
		{
			Output.Write( buffer, index, count );
		}

		public virtual void Write( Byte[] buffer )
		{
			Output.Write( buffer );
		}

		public virtual void Write( Byte value )
		{
			Output.Write( value );
		}

		public virtual void Write( Boolean value )
		{
			Output.Write( value );
		}

		public virtual void Write( Int16 value )
		{
			Output.Write( value );
		}

		public virtual void Write( Char ch )
		{
			Output.Write( ch );
		}

		public virtual void WriteVector( Single[] v )
		{
			for ( var n = 0; n < 3; n++ )
				Output.Write( v[n] );
		}

		public virtual Single[] ReadVector( )
		{
			Single[] res = new Single[] { 0, 0, 0 };
			for ( var n = 0; n < 3; n++ )
				res[n] = Input.ReadSingle();
			return res;
		}

		public virtual void WriteEdictRef( edict_t ent )
		{
			if ( ent == null )
				Output.Write( -1 );
			else
			{
				Output.Write( ent.s.number );
			}
		}

		public virtual edict_t ReadEdictRef( )
		{
			var i = Input.ReadInt32();
			if ( i < 0 )
				return null;
			if ( i > GameBase.g_edicts.Length )
			{
				Com.DPrintf( "jake2: illegal edict num:" + i + "\\n" );
				return null;
			}

			return GameBase.g_edicts[i];
		}

		public virtual void WriteAdapter( SuperAdapter a )
		{
			Output.Write( 3988 );
			if ( a == null )
				Write( ( String ) null );
			else
			{
				var str = a.GetID();
				if ( a == null )
				{
					Com.DPrintf( "writeAdapter: invalid Adapter id for " + a + "\\n" );
				}

				Write( str );
			}
		}

		public virtual SuperAdapter ReadAdapter( )
		{
			if ( Input.ReadInt32() != 3988 )
				Com.DPrintf( "wrong read position: readadapter 3988 \\n" );
			var id = ReadString();
			if ( id == null )
			{
				return null;
			}

			return SuperAdapter.GetFromID( id );
		}

		public virtual void WriteItem( gitem_t item )
		{
			if ( item == null )
				Output.Write( -1 );
			else
				Output.Write( item.index );
		}

		public virtual gitem_t ReadItem( )
		{
			var ndx = Input.ReadInt32();
			if ( ndx == -1 )
				return null;
			else
				return GameItemList.itemlist[ndx];
		}

		public virtual int Read( Byte[] buffer, Int32 index, Int32 count )
		{
			return Input.Read( buffer, index, count );
		}
	}
}
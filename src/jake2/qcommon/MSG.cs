using Q2Sharp.Game;
using Q2Sharp.Util;
using System;
using System.Text;

namespace Q2Sharp.Qcommon
{
	public class MSG : Globals
	{
		public static void WriteChar( sizebuf_t sb, Int32 c )
		{
			sb.data[SZ.GetSpace( sb, 1 )] = ( Byte ) ( c & 0xFF );
		}

		public static void WriteChar( sizebuf_t sb, Single c )
		{
			WriteChar( sb, ( Int32 ) c );
		}

		public static void WriteByte( sizebuf_t sb, Int32 c )
		{
			sb.data[SZ.GetSpace( sb, 1 )] = ( Byte ) ( c & 0xFF );
		}

		public static void WriteByte( sizebuf_t sb, Single c )
		{
			WriteByte( sb, ( Int32 ) c );
		}

		public static void WriteShort( sizebuf_t sb, Int32 c )
		{
			var i = SZ.GetSpace( sb, 2 );
			sb.data[i++] = ( Byte ) ( c & 0xff );
			sb.data[i] = ( Byte ) ( ( c >> 8 ) & 0xFF );
		}

		public static void WriteInt( sizebuf_t sb, Int32 c )
		{
			var i = SZ.GetSpace( sb, 4 );
			sb.data[i++] = ( Byte ) ( ( c & 0xff ) );
			sb.data[i++] = ( Byte ) ( ( c >> 8 ) & 0xff );
			sb.data[i++] = ( Byte ) ( ( c >> 16 ) & 0xff );
			sb.data[i++] = ( Byte ) ( ( c >> 24 ) & 0xff );
		}

		public static void WriteLong( sizebuf_t sb, Int32 c )
		{
			WriteInt( sb, c );
		}

		public static void WriteFloat( sizebuf_t sb, Single f )
		{
			WriteInt( sb, Convert.ToInt32( f ) );
		}

		public static void WriteString( sizebuf_t sb, String s )
		{
			var x = s;
			if ( s == null )
				x = "";
			SZ.Write( sb, Lib.StringToBytes( x ) );
			WriteByte( sb, 0 );
		}

		public static void WriteString( sizebuf_t sb, Byte[] s )
		{
			WriteString( sb, Encoding.ASCII.GetString( s ).Trim() );
		}

		public static void WriteCoord( sizebuf_t sb, Single f )
		{
			WriteShort( sb, ( Int32 ) ( f * 8 ) );
		}

		public static void WritePos( sizebuf_t sb, Single[] pos )
		{
			WriteShort( sb, ( Int32 ) ( pos[0] * 8 ) );
			WriteShort( sb, ( Int32 ) ( pos[1] * 8 ) );
			WriteShort( sb, ( Int32 ) ( pos[2] * 8 ) );
		}

		public static void WriteAngle( sizebuf_t sb, Single f )
		{
			WriteByte( sb, ( Int32 ) ( f * 256 / 360 ) & 255 );
		}

		public static void WriteAngle16( sizebuf_t sb, Single f )
		{
			WriteShort( sb, Math3D.ANGLE2SHORT( f ) );
		}

		public static void WriteDeltaUsercmd( sizebuf_t buf, usercmd_t from, usercmd_t cmd )
		{
			Int32 bits;
			bits = 0;
			if ( cmd.angles[0] != from.angles[0] )
				bits |= CM_ANGLE1;
			if ( cmd.angles[1] != from.angles[1] )
				bits |= CM_ANGLE2;
			if ( cmd.angles[2] != from.angles[2] )
				bits |= CM_ANGLE3;
			if ( cmd.forwardmove != from.forwardmove )
				bits |= CM_FORWARD;
			if ( cmd.sidemove != from.sidemove )
				bits |= CM_SIDE;
			if ( cmd.upmove != from.upmove )
				bits |= CM_UP;
			if ( cmd.buttons != from.buttons )
				bits |= CM_BUTTONS;
			if ( cmd.impulse != from.impulse )
				bits |= CM_IMPULSE;
			WriteByte( buf, bits );
			if ( ( bits & CM_ANGLE1 ) != 0 )
				WriteShort( buf, cmd.angles[0] );
			if ( ( bits & CM_ANGLE2 ) != 0 )
				WriteShort( buf, cmd.angles[1] );
			if ( ( bits & CM_ANGLE3 ) != 0 )
				WriteShort( buf, cmd.angles[2] );
			if ( ( bits & CM_FORWARD ) != 0 )
				WriteShort( buf, cmd.forwardmove );
			if ( ( bits & CM_SIDE ) != 0 )
				WriteShort( buf, cmd.sidemove );
			if ( ( bits & CM_UP ) != 0 )
				WriteShort( buf, cmd.upmove );
			if ( ( bits & CM_BUTTONS ) != 0 )
				WriteByte( buf, cmd.buttons );
			if ( ( bits & CM_IMPULSE ) != 0 )
				WriteByte( buf, cmd.impulse );
			WriteByte( buf, cmd.msec );
			WriteByte( buf, cmd.lightlevel );
		}

		public static void WriteDir( sizebuf_t sb, Single[] dir )
		{
			Int32 i, best;
			Single d, bestd;
			if ( dir == null )
			{
				WriteByte( sb, 0 );
				return;
			}

			bestd = 0;
			best = 0;
			for ( i = 0; i < NUMVERTEXNORMALS; i++ )
			{
				d = Math3D.DotProduct( dir, bytedirs[i] );
				if ( d > bestd )
				{
					bestd = d;
					best = i;
				}
			}

			WriteByte( sb, best );
		}

		public static void ReadDir( sizebuf_t sb, Single[] dir )
		{
			Int32 b;
			b = ReadByte( sb );
			if ( b >= NUMVERTEXNORMALS )
				Com.Error( ERR_DROP, "MSF_ReadDir: out of range" );
			Math3D.VectorCopy( bytedirs[b], dir );
		}

		public static void WriteDeltaEntity( entity_state_t from, entity_state_t to, sizebuf_t msg, Boolean force, Boolean newentity )
		{
			Int32 bits;
			if ( 0 == to.number )
				Com.Error( ERR_FATAL, "Unset entity number" );
			if ( to.number >= MAX_EDICTS )
				Com.Error( ERR_FATAL, "Entity number >= MAX_EDICTS" );
			bits = 0;
			if ( to.number >= 256 )
				bits |= U_NUMBER16;
			if ( to.origin[0] != from.origin[0] )
				bits |= U_ORIGIN1;
			if ( to.origin[1] != from.origin[1] )
				bits |= U_ORIGIN2;
			if ( to.origin[2] != from.origin[2] )
				bits |= U_ORIGIN3;
			if ( to.angles[0] != from.angles[0] )
				bits |= U_ANGLE1;
			if ( to.angles[1] != from.angles[1] )
				bits |= U_ANGLE2;
			if ( to.angles[2] != from.angles[2] )
				bits |= U_ANGLE3;
			if ( to.skinnum != from.skinnum )
			{
				if ( to.skinnum < 256 )
					bits |= U_SKIN8;
				else if ( to.skinnum < 0x10000 )
					bits |= U_SKIN16;
				else
					bits |= ( U_SKIN8 | U_SKIN16 );
			}

			if ( to.frame != from.frame )
			{
				if ( to.frame < 256 )
					bits |= U_FRAME8;
				else
					bits |= U_FRAME16;
			}

			if ( to.effects != from.effects )
			{
				if ( to.effects < 256 )
					bits |= U_EFFECTS8;
				else if ( to.effects < 0x8000 )
					bits |= U_EFFECTS16;
				else
					bits |= U_EFFECTS8 | U_EFFECTS16;
			}

			if ( to.renderfx != from.renderfx )
			{
				if ( to.renderfx < 256 )
					bits |= U_RENDERFX8;
				else if ( to.renderfx < 0x8000 )
					bits |= U_RENDERFX16;
				else
					bits |= U_RENDERFX8 | U_RENDERFX16;
			}

			if ( to.solid != from.solid )
				bits |= U_SOLID;
			if ( to.event_renamed != 0 )
				bits |= U_EVENT;
			if ( to.modelindex != from.modelindex )
				bits |= U_MODEL;
			if ( to.modelindex2 != from.modelindex2 )
				bits |= U_MODEL2;
			if ( to.modelindex3 != from.modelindex3 )
				bits |= U_MODEL3;
			if ( to.modelindex4 != from.modelindex4 )
				bits |= U_MODEL4;
			if ( to.sound != from.sound )
				bits |= U_SOUND;
			if ( newentity || ( to.renderfx & RF_BEAM ) != 0 )
				bits |= U_OLDORIGIN;
			if ( bits == 0 && !force )
				return;
			if ( ( bits & 0xff000000 ) != 0 )
				bits |= U_MOREBITS3 | U_MOREBITS2 | U_MOREBITS1;
			else if ( ( bits & 0x00ff0000 ) != 0 )
				bits |= U_MOREBITS2 | U_MOREBITS1;
			else if ( ( bits & 0x0000ff00 ) != 0 )
				bits |= U_MOREBITS1;
			WriteByte( msg, bits & 255 );
			if ( ( bits & 0xff000000 ) != 0 )
			{
				WriteByte( msg, ( bits >> 8 ) & 255 );
				WriteByte( msg, ( bits >> 16 ) & 255 );
				WriteByte( msg, ( bits >> 24 ) & 255 );
			}
			else if ( ( bits & 0x00ff0000 ) != 0 )
			{
				WriteByte( msg, ( bits >> 8 ) & 255 );
				WriteByte( msg, ( bits >> 16 ) & 255 );
			}
			else if ( ( bits & 0x0000ff00 ) != 0 )
			{
				WriteByte( msg, ( bits >> 8 ) & 255 );
			}

			if ( ( bits & U_NUMBER16 ) != 0 )
				WriteShort( msg, to.number );
			else
				WriteByte( msg, to.number );
			if ( ( bits & U_MODEL ) != 0 )
				WriteByte( msg, to.modelindex );
			if ( ( bits & U_MODEL2 ) != 0 )
				WriteByte( msg, to.modelindex2 );
			if ( ( bits & U_MODEL3 ) != 0 )
				WriteByte( msg, to.modelindex3 );
			if ( ( bits & U_MODEL4 ) != 0 )
				WriteByte( msg, to.modelindex4 );
			if ( ( bits & U_FRAME8 ) != 0 )
				WriteByte( msg, to.frame );
			if ( ( bits & U_FRAME16 ) != 0 )
				WriteShort( msg, to.frame );
			if ( ( bits & U_SKIN8 ) != 0 && ( bits & U_SKIN16 ) != 0 )
				WriteInt( msg, to.skinnum );
			else if ( ( bits & U_SKIN8 ) != 0 )
				WriteByte( msg, to.skinnum );
			else if ( ( bits & U_SKIN16 ) != 0 )
				WriteShort( msg, to.skinnum );
			if ( ( bits & ( U_EFFECTS8 | U_EFFECTS16 ) ) == ( U_EFFECTS8 | U_EFFECTS16 ) )
				WriteInt( msg, to.effects );
			else if ( ( bits & U_EFFECTS8 ) != 0 )
				WriteByte( msg, to.effects );
			else if ( ( bits & U_EFFECTS16 ) != 0 )
				WriteShort( msg, to.effects );
			if ( ( bits & ( U_RENDERFX8 | U_RENDERFX16 ) ) == ( U_RENDERFX8 | U_RENDERFX16 ) )
				WriteInt( msg, to.renderfx );
			else if ( ( bits & U_RENDERFX8 ) != 0 )
				WriteByte( msg, to.renderfx );
			else if ( ( bits & U_RENDERFX16 ) != 0 )
				WriteShort( msg, to.renderfx );
			if ( ( bits & U_ORIGIN1 ) != 0 )
				WriteCoord( msg, to.origin[0] );
			if ( ( bits & U_ORIGIN2 ) != 0 )
				WriteCoord( msg, to.origin[1] );
			if ( ( bits & U_ORIGIN3 ) != 0 )
				WriteCoord( msg, to.origin[2] );
			if ( ( bits & U_ANGLE1 ) != 0 )
				WriteAngle( msg, to.angles[0] );
			if ( ( bits & U_ANGLE2 ) != 0 )
				WriteAngle( msg, to.angles[1] );
			if ( ( bits & U_ANGLE3 ) != 0 )
				WriteAngle( msg, to.angles[2] );
			if ( ( bits & U_OLDORIGIN ) != 0 )
			{
				WriteCoord( msg, to.old_origin[0] );
				WriteCoord( msg, to.old_origin[1] );
				WriteCoord( msg, to.old_origin[2] );
			}

			if ( ( bits & U_SOUND ) != 0 )
				WriteByte( msg, to.sound );
			if ( ( bits & U_EVENT ) != 0 )
				WriteByte( msg, to.event_renamed );
			if ( ( bits & U_SOLID ) != 0 )
				WriteShort( msg, to.solid );
		}

		public static void BeginReading( sizebuf_t msg )
		{
			msg.readcount = 0;
		}

		public static Int32 ReadChar( sizebuf_t msg_read )
		{
			Int32 c;
			if ( msg_read.readcount + 1 > msg_read.cursize )
				c = -1;
			else
				c = msg_read.data[msg_read.readcount];
			msg_read.readcount++;
			return c;
		}

		public static Int32 ReadByte( sizebuf_t msg_read )
		{
			Int32 c;
			if ( msg_read.readcount + 1 > msg_read.cursize )
				c = -1;
			else
				c = msg_read.data[msg_read.readcount] & 0xff;
			msg_read.readcount++;
			return c;
		}

		public static Int16 ReadShort( sizebuf_t msg_read )
		{
			Int32 c;
			if ( msg_read.readcount + 2 > msg_read.cursize )
				c = -1;
			else
				c = ( Int16 ) ( ( msg_read.data[msg_read.readcount] & 0xff ) + ( msg_read.data[msg_read.readcount + 1] << 8 ) );
			msg_read.readcount += 2;
			return ( Int16 ) c;
		}

		public static Int32 ReadLong( sizebuf_t msg_read )
		{
			Int32 c;
			if ( msg_read.readcount + 4 > msg_read.cursize )
			{
				Com.Printf( "buffer underrun in ReadLong!" );
				c = -1;
			}
			else
				c = ( msg_read.data[msg_read.readcount] & 0xff ) | ( ( msg_read.data[msg_read.readcount + 1] & 0xff ) << 8 ) | ( ( msg_read.data[msg_read.readcount + 2] & 0xff ) << 16 ) | ( ( msg_read.data[msg_read.readcount + 3] & 0xff ) << 24 );
			msg_read.readcount += 4;
			return c;
		}

		public static Single ReadFloat( sizebuf_t msg_read )
		{
			var n = ReadLong( msg_read );
			return BitConverter.ToSingle( BitConverter.GetBytes( n ) );
		}

		public static Byte[] readbuf = new Byte[2048];
		public static String ReadString( sizebuf_t msg_read )
		{
			Byte c;
			var l = 0;
			do
			{
				c = ( Byte ) ReadByte( msg_read );
				if ( c == -1 || c == 0 )
					break;
				readbuf[l] = c;
				l++;
			}
			while ( l < 2047 );
			var ret = Encoding.ASCII.GetString( readbuf );
			return ret;
		}

		public static String ReadStringLine( sizebuf_t msg_read )
		{
			Int32 l;
			Byte c;
			l = 0;
			do
			{
				c = ( Byte ) ReadChar( msg_read );
				if ( c == -1 || c == 0 || c == 0x0a )
					break;
				readbuf[l] = c;
				l++;
			}
			while ( l < 2047 );
			var ret = Encoding.ASCII.GetString( readbuf ).Trim();
			Com.Dprintln( "MSG.ReadStringLine:[" + ret.Replace( '\\', '@' ) + "]" );
			return ret;
		}

		public static Single ReadCoord( sizebuf_t msg_read )
		{
			return ReadShort( msg_read ) * ( 1F / 8 );
		}

		public static void ReadPos( sizebuf_t msg_read, Single[] pos )
		{
			pos[0] = ReadShort( msg_read ) * ( 1F / 8 );
			pos[1] = ReadShort( msg_read ) * ( 1F / 8 );
			pos[2] = ReadShort( msg_read ) * ( 1F / 8 );
		}

		public static Single ReadAngle( sizebuf_t msg_read )
		{
			return ReadChar( msg_read ) * ( 360F / 256 );
		}

		public static Single ReadAngle16( sizebuf_t msg_read )
		{
			return Math3D.SHORT2ANGLE( ReadShort( msg_read ) );
		}

		public static void ReadDeltaUsercmd( sizebuf_t msg_read, usercmd_t from, usercmd_t move )
		{
			Int32 bits;
			move.Set( from );
			bits = ReadByte( msg_read );
			if ( ( bits & CM_ANGLE1 ) != 0 )
				move.angles[0] = ReadShort( msg_read );
			if ( ( bits & CM_ANGLE2 ) != 0 )
				move.angles[1] = ReadShort( msg_read );
			if ( ( bits & CM_ANGLE3 ) != 0 )
				move.angles[2] = ReadShort( msg_read );
			if ( ( bits & CM_FORWARD ) != 0 )
				move.forwardmove = ReadShort( msg_read );
			if ( ( bits & CM_SIDE ) != 0 )
				move.sidemove = ReadShort( msg_read );
			if ( ( bits & CM_UP ) != 0 )
				move.upmove = ReadShort( msg_read );
			if ( ( bits & CM_BUTTONS ) != 0 )
				move.buttons = ( Byte ) ReadByte( msg_read );
			if ( ( bits & CM_IMPULSE ) != 0 )
				move.impulse = ( Byte ) ReadByte( msg_read );
			move.msec = ( Byte ) ReadByte( msg_read );
			move.lightlevel = ( Byte ) ReadByte( msg_read );
		}

		public static void ReadData( sizebuf_t msg_read, Byte[] data, Int32 len )
		{
			for ( var i = 0; i < len; i++ )
				data[i] = ( Byte ) ReadByte( msg_read );
		}
	}
}
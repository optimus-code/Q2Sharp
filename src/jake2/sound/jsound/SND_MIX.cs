using J2N.IO;
using Q2Sharp.Game;
using Q2Sharp.Util;
using System;

namespace Q2Sharp.Sound.Jsound
{
	public class SND_MIX : SND_JAVA
	{
		public const Int32 MAX_CHANNELS = 32;
		public const Int32 MAX_RAW_SAMPLES = 8192;
		public class playsound_t
		{
			public playsound_t prev, next;
			public sfx_t sfx;
			public Single volume;
			public Single attenuation;
			public Int32 entnum;
			public Int32 entchannel;
			public Boolean fixed_origin;
			public Single[] origin = new Single[] { 0, 0, 0 };
			public Int64 begin;
			public virtual void Clear( )
			{
				prev = next = null;
				sfx = null;
				volume = attenuation = begin = entnum = entchannel = 0;
				fixed_origin = false;
				Math3D.VectorClear( origin );
			}
		}

		public class channel_t
		{
			public sfx_t sfx;
			public Int32 leftvol;
			public Int32 rightvol;
			public Int32 end;
			public Int32 pos;
			public Int32 looping;
			public Int32 entnum;
			public Int32 entchannel;
			public Single[] origin = new Single[] { 0, 0, 0 };
			public Single dist_mult;
			public Int32 master_vol;
			public Boolean fixed_origin;
			public Boolean autosound;
			public virtual void Clear( )
			{
				sfx = null;
				dist_mult = leftvol = rightvol = end = pos = looping = entnum = entchannel = master_vol = 0;
				Math3D.VectorClear( origin );
				fixed_origin = autosound = false;
			}
		}

		class portable_samplepair_t
		{
			Int32 left;
			Int32 right;
		}

		public static cvar_t s_volume;
		public static Int32 s_rawend;
		public static readonly Int32 PAInt32Buffer_SIZE = 2048;
		public static Int32Buffer paInt32Buffer = Int32Buffer.Allocate( PAInt32Buffer_SIZE * 2 );
		public static Int32[][] snd_scaletable = Lib.CreateJaggedArray<Int32[][]>( 32, 256 );
		static Int32Buffer snd_p;
		public static Int16Buffer snd_out;
		public static Int32 snd_linear_count;
		public static Int32 snd_vol;
		public static Int32 paintedtime;
		public static playsound_t s_pendingplays = new playsound_t();
		public static Int32Buffer s_rawsamples = Int32Buffer.Allocate( MAX_RAW_SAMPLES * 2 );
		public static channel_t[] channels = new channel_t[MAX_CHANNELS];
		static SND_MIX( )
		{
			for ( var i = 0; i < MAX_CHANNELS; i++ )
				channels[i] = new channel_t();
		}

		static void WriteLinearBlastStereo16( )
		{
			Int32 i;
			Int32 val;
			for ( i = 0; i < snd_linear_count; i += 2 )
			{
				val = snd_p.Get( i ) >> 8;
				if ( val > 0x7fff )
					snd_out.Put( i, unchecked(( Int16 ) 0x7fff) );
				else if ( val < unchecked(( Int16 ) 0x8000) )
					snd_out.Put( i, unchecked(( Int16 ) 0x8000) );
				else
					snd_out.Put( i, unchecked(( Int16 ) val) );
				val = snd_p.Get( i + 1 ) >> 8;
				if ( val > 0x7fff )
					snd_out.Put( i + 1, unchecked(( Int16 ) 0x7fff) );
				else if ( val < unchecked(( Int16 ) 0x8000) )
					snd_out.Put( i + 1, unchecked(( Int16 ) 0x8000) );
				else
					snd_out.Put( i + 1, unchecked(( Int16 ) val) );
			}
		}

		static void TransferStereo16( ByteBuffer pbuf, Int32 endtime )
		{
			Int32 lpos;
			Int32 lpaintedtime;
			snd_p = paInt32Buffer;
			lpaintedtime = paintedtime;
			while ( lpaintedtime < endtime )
			{
				lpos = lpaintedtime & ( ( dma.samples >> 1 ) - 1 );
				snd_out = pbuf.AsInt16Buffer();
				snd_out.Position = lpos << 1;
				snd_out = snd_out.Slice();
				snd_linear_count = ( dma.samples >> 1 ) - lpos;
				if ( lpaintedtime + snd_linear_count > endtime )
					snd_linear_count = endtime - lpaintedtime;
				snd_linear_count <<= 1;
				WriteLinearBlastStereo16();
				paInt32Buffer.Position = snd_linear_count;
				snd_p = paInt32Buffer.Slice();
				lpaintedtime += ( snd_linear_count >> 1 );
			}
		}

		static void TransferPaInt32Buffer( Int32 endtime )
		{
			Int32 out_idx;
			Int32 count;
			Int32 out_mask;
			Int32 p;
			Int32 step;
			Int32 val;
			ByteBuffer pbuf = ByteBuffer.Wrap( dma.buffer );
			pbuf.Order = ByteOrder.LittleEndian;
			if ( SND_DMA.s_testsound.value != 0F )
			{
				Int32 i;
				Int32 count2;
				count2 = ( endtime - paintedtime ) * 2;
				Int32 v;
				for ( i = 0; i < count2; i += 2 )
				{
					v = ( Int32 ) ( Math.Sin( ( paintedtime + i ) * 0.1 ) * 20000 * 256 );
					paInt32Buffer.Put( i, v );
					paInt32Buffer.Put( i + 1, v );
				}
			}

			if ( dma.samplebits == 16 && dma.channels == 2 )
			{
				TransferStereo16( pbuf, endtime );
			}
			else
			{
				p = 0;
				count = ( endtime - paintedtime ) * dma.channels;
				out_mask = dma.samples - 1;
				out_idx = paintedtime * dma.channels & out_mask;
				step = 3 - dma.channels;
				if ( dma.samplebits == 16 )
				{
					Int16Buffer out_renamed = pbuf.AsInt16Buffer();
					while ( count-- > 0 )
					{
						val = paInt32Buffer.Get( p ) >> 8;
						p += step;
						if ( val > 0x7fff )
							val = 0x7fff;
						else if ( val < unchecked(( Int16 ) 0x8000) )
							val = unchecked(( Int16 ) 0x8000);
						out_renamed.Put( out_idx, ( Int16 ) val );
						out_idx = ( out_idx + 1 ) & out_mask;
					}
				}
				else if ( dma.samplebits == 8 )
				{
					ByteBuffer out_renamed = pbuf;
					while ( count-- > 0 )
					{
						val = paInt32Buffer.Get( p ) >> 8;
						p += step;
						if ( val > 0x7fff )
							val = 0x7fff;
						else if ( val < unchecked(( Int16 ) 0x8000) )
							val = unchecked(( Int16 ) 0x8000);
						out_renamed.Put( out_idx, ( Byte ) ( val >> 8 ) );
						out_idx = ( out_idx + 1 ) & out_mask;
					}
				}
			}
		}

		public static void PaintChannels( Int32 endtime )
		{
			Int32 i;
			Int32 end;
			channel_t ch;
			sfxcache_t sc;
			Int32 ltime, count;
			playsound_t ps;
			snd_vol = ( Int32 ) ( s_volume.value * 256 );
			while ( paintedtime < endtime )
			{
				end = endtime;
				if ( endtime - paintedtime > PAInt32Buffer_SIZE )
					end = paintedtime + PAInt32Buffer_SIZE;
				while ( true )
				{
					ps = s_pendingplays.next;
					if ( ps == s_pendingplays )
						break;
					if ( ps.begin <= paintedtime )
					{
						SND_DMA.IssuePlaysound( ps );
						continue;
					}

					if ( ps.begin < end )
						end = ( Int32 ) ps.begin;
					break;
				}

				if ( s_rawend < paintedtime )
				{
					for ( i = 0; i < ( end - paintedtime ) * 2; i++ )
					{
						paInt32Buffer.Put( i, 0 );
					}
				}
				else
				{
					Int32 s;
					Int32 stop;
					stop = ( end < s_rawend ) ? end : s_rawend;
					for ( i = paintedtime; i < stop; i++ )
					{
						s = i & ( MAX_RAW_SAMPLES - 1 );
						paInt32Buffer.Put( ( i - paintedtime ) * 2, s_rawsamples.Get( 2 * s ) );
						paInt32Buffer.Put( ( i - paintedtime ) * 2 + 1, s_rawsamples.Get( 2 * s ) + 1 );
					}

					for ( ; i < end; i++ )
					{
						paInt32Buffer.Put( ( i - paintedtime ) * 2, 0 );
						paInt32Buffer.Put( ( i - paintedtime ) * 2 + 1, 0 );
					}
				}

				for ( i = 0; i < MAX_CHANNELS; i++ )
				{
					ch = channels[i];
					ltime = paintedtime;
					while ( ltime < end )
					{
						if ( ch.sfx == null || ( ch.leftvol == 0 && ch.rightvol == 0 ) )
							break;
						count = end - ltime;
						if ( ch.end - ltime < count )
							count = ch.end - ltime;
						sc = WaveLoader.LoadSound( ch.sfx );
						if ( sc == null )
							break;
						if ( count > 0 && ch.sfx != null )
						{
							if ( sc.width == 1 )
								PaintChannelFrom8( ch, sc, count, ltime - paintedtime );
							else
								PaintChannelFrom16( ch, sc, count, ltime - paintedtime );
							ltime += count;
						}

						if ( ltime >= ch.end )
						{
							if ( ch.autosound )
							{
								ch.pos = 0;
								ch.end = ltime + sc.length;
							}
							else if ( sc.loopstart >= 0 )
							{
								ch.pos = sc.loopstart;
								ch.end = ltime + sc.length - ch.pos;
							}
							else
							{
								ch.sfx = null;
							}
						}
					}
				}

				TransferPaInt32Buffer( end );
				paintedtime = end;
			}
		}

		public static void InitScaletable( )
		{
			Int32 i, j;
			Int32 scale;
			s_volume.modified = false;
			for ( i = 0; i < 32; i++ )
			{
				scale = ( Int32 ) ( i * 8 * 256 * s_volume.value );
				for ( j = 0; j < 256; j++ )
					snd_scaletable[i][j] = ( ( Byte ) j ) * scale;
			}
		}

		static void PaintChannelFrom8( channel_t ch, sfxcache_t sc, Int32 count, Int32 offset )
		{
			Int32 data;
			Int32[] lscale;
			Int32[] rscale;
			Int32 sfx;
			Int32 i;
			portable_samplepair_t samp;
			if ( ch.leftvol > 255 )
				ch.leftvol = 255;
			if ( ch.rightvol > 255 )
				ch.rightvol = 255;
			lscale = snd_scaletable[ch.leftvol >> 3];
			rscale = snd_scaletable[ch.rightvol >> 3];
			sfx = ch.pos;
			for ( i = 0; i < count; i++, offset++ )
			{
				var left = paInt32Buffer.Get( offset * 2 );
				var right = paInt32Buffer.Get( offset * 2 + 1 );
				data = sc.data[sfx + i];
				left += lscale[data];
				right += rscale[data];
				paInt32Buffer.Put( offset * 2, left );
				paInt32Buffer.Put( offset * 2 + 1, right );
			}

			ch.pos += count;
		}

		private static ByteBuffer bb;
		private static Int16Buffer sb;
		static void PaintChannelFrom16( channel_t ch, sfxcache_t sc, Int32 count, Int32 offset )
		{
			Int32 data;
			Int32 left, right;
			Int32 leftvol, rightvol;
			Int32 sfx;
			Int32 i;
			portable_samplepair_t samp;
			leftvol = ch.leftvol * snd_vol;
			rightvol = ch.rightvol * snd_vol;
			ByteBuffer bb = ByteBuffer.Wrap( sc.data );
			bb.Order = ByteOrder.LittleEndian;
			sb = bb.AsInt16Buffer();
			sfx = ch.pos;
			for ( i = 0; i < count; i++, offset++ )
			{
				left = paInt32Buffer.Get( offset * 2 );
				right = paInt32Buffer.Get( offset * 2 + 1 );
				data = sb.Get( sfx + i );
				left += ( data * leftvol ) >> 8;
				right += ( data * rightvol ) >> 8;
				paInt32Buffer.Put( offset * 2, left );
				paInt32Buffer.Put( offset * 2 + 1, right );
			}

			ch.pos += count;
		}
	}
}
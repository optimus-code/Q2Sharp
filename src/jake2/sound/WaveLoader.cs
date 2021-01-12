using Q2Sharp.Qcommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Sound
{
    public class WaveLoader
    {
        private static bool DONT_DO_A_RESAMPLING_FOR_JOAL_AND_LWJGL = true;
        private static int maxsamplebytes = 2048 * 1024;
        public static sfxcache_t LoadSound(sfx_t s)
        {
            if (s.name[0] == '*')
                return null;
            sfxcache_t sc = s.cache;
            if (sc != null)
                return sc;
            string name;
            if (s.truename != null)
                name = s.truename;
            else
                name = s.name;
            string namebuffer;
            if (name[0] == '#')
                namebuffer = name.Substring(1);
            else
                namebuffer = "sound/" + name;
            byte[] data = FS.LoadFile(namebuffer);
            if (data == null)
            {
                Com.DPrintf("Couldn't load " + namebuffer + "\\n");
                return null;
            }

            int size = data.Length;
            wavinfo_t info = GetWavinfo(s.name, data, size);
            if (info.channels != 1)
            {
                Com.Printf(s.name + " is a stereo sample - ignoring\\n");
                return null;
            }

            float stepscale;
            if (DONT_DO_A_RESAMPLING_FOR_JOAL_AND_LWJGL)
                stepscale = 1;
            else
                stepscale = (float)info.rate / S.GetDefaultSampleRate();
            int len = (int)(info.samples / stepscale);
            len = len * info.width * info.channels;
            if (len >= maxsamplebytes)
            {
                Com.Printf(s.name + " is too long: " + len + " bytes?! ignoring.\\n");
                return null;
            }

            sc = s.cache = new sfxcache_t(len);
            sc.length = info.samples;
            sc.loopstart = info.loopstart;
            sc.speed = info.rate;
            sc.width = info.width;
            sc.stereo = info.channels;
            ResampleSfx(s, sc.speed, sc.width, data, info.dataofs);
            data = null;
            return sc;
        }

        public static void ResampleSfx(sfx_t sfx, int inrate, int inwidth, byte[] data, int offset)
        {
            int outcount;
            int srcsample;
            int i;
            int sample, samplefrac, fracstep;
            sfxcache_t sc;
            sc = sfx.cache;
            if (sc == null)
                return;
            float stepscale;
            if (DONT_DO_A_RESAMPLING_FOR_JOAL_AND_LWJGL)
                stepscale = 1;
            else
                stepscale = (float)inrate / S.GetDefaultSampleRate();
            outcount = (int)(sc.length / stepscale);
            sc.length = outcount;
            if (sc.loopstart != -1)
                sc.loopstart = (int)(sc.loopstart / stepscale);
            if (DONT_DO_A_RESAMPLING_FOR_JOAL_AND_LWJGL == false)
                sc.speed = S.GetDefaultSampleRate();
            sc.width = inwidth;
            sc.stereo = 0;
            samplefrac = 0;
            fracstep = (int)(stepscale * 256);
            for (i = 0; i < outcount; i++)
            {
                srcsample = samplefrac >> 8;
                samplefrac += fracstep;
                if (inwidth == 2)
                {
                    sample = (data[offset + srcsample * 2] & 0xff) + (data[offset + srcsample * 2 + 1] << 8);
                }
                else
                {
                    sample = ((data[offset + srcsample] & 0xff) - 128) << 8;
                }

                if (sc.width == 2)
                {
                    if (Defines.LITTLE_ENDIAN)
                    {
                        sc.data[i * 2] = (byte)(sample & 0xff);
                        sc.data[i * 2 + 1] = (byte)((sample >> 8) & 0xff);
                    }
                    else
                    {
                        sc.data[i * 2] = (byte)((sample >> 8) & 0xff);
                        sc.data[i * 2 + 1] = (byte)(sample & 0xff);
                    }
                }
                else
                {
                    sc.data[i] = (byte)(sample >> 8);
                }
            }
        }

        static byte[] data_b;
        static int data_p;
        static int iff_end;
        static int last_chunk;
        static int iff_data;
        static int iff_chunk_len;
        static short GetLittleShort()
        {
            int val = 0;
            val = data_b[data_p] & 0xFF;
            data_p++;
            val |= ((data_b[data_p] & 0xFF) << 8);
            data_p++;
            return (short)val;
        }

        static int GetLittleLong()
        {
            int val = 0;
            val = data_b[data_p] & 0xFF;
            data_p++;
            val |= ((data_b[data_p] & 0xFF) << 8);
            data_p++;
            val |= ((data_b[data_p] & 0xFF) << 16);
            data_p++;
            val |= ((data_b[data_p] & 0xFF) << 24);
            data_p++;
            return val;
        }

        static void FindNextChunk(string name)
        {
            while (true)
            {
                data_p = last_chunk;
                if (data_p >= iff_end)
                {
                    data_p = 0;
                    return;
                }

                data_p += 4;
                iff_chunk_len = GetLittleLong();
                if (iff_chunk_len < 0)
                {
                    data_p = 0;
                    return;
                }

                if (iff_chunk_len > 1024 * 1024)
                {
                    Com.Println(" Warning: FindNextChunk: length is past the 1 meg sanity limit");
                }

                data_p -= 8;
                last_chunk = data_p + 8 + ((iff_chunk_len + 1) & ~1);
                string s = Encoding.ASCII.GetString(data_b, 0, 4); //new string (data_b, data_p, 4);
                if (s.Equals(name))
                    return;
            }
        }

        static void FindChunk(string name)
        {
            last_chunk = iff_data;
            FindNextChunk(name);
        }

        static wavinfo_t GetWavinfo(string name, byte[] wav, int wavlength)
        {
            wavinfo_t info = new wavinfo_t();
            int i;
            int format;
            int samples;
            if (wav == null)
                return info;
            iff_data = 0;
            iff_end = wavlength;
            data_b = wav;
            FindChunk("RIFF");
            string s = Encoding.ASCII.GetString( data_b, 8, 4 );//, data_p + 8, 4);
            if (!s.Equals("WAVE"))
            {
                Com.Printf("Missing RIFF/WAVE chunks\\n");
                return info;
            }

            iff_data = data_p + 12;
            FindChunk("fmt ");
            if (data_p == 0)
            {
                Com.Printf("Missing fmt chunk\\n");
                return info;
            }

            data_p += 8;
            format = GetLittleShort();
            if (format != 1)
            {
                Com.Printf("Microsoft PCM format only\\n");
                return info;
            }

            info.channels = GetLittleShort();
            info.rate = GetLittleLong();
            data_p += 4 + 2;
            info.width = GetLittleShort() / 8;
            FindChunk("cue ");
            if (data_p != 0)
            {
                data_p += 32;
                info.loopstart = GetLittleLong();
                FindNextChunk("LIST");
                if (data_p != 0)
                {
                    if (data_b.Length >= data_p + 32)
                    {
                        s = Encoding.ASCII.GetString( data_b, 28, 4 );//new string (data_b, data_p + 28, 4);
                        if (s.Equals("MARK"))
                        {
                            data_p += 24;
                            i = GetLittleLong();
                            info.samples = info.loopstart + i;
                        }
                    }
                }
            }
            else
                info.loopstart = -1;
            FindChunk("data");
            if (data_p == 0)
            {
                Com.Printf("Missing data chunk\\n");
                return info;
            }

            data_p += 4;
            samples = GetLittleLong() / info.width;
            if (info.samples != 0)
            {
                if (samples < info.samples)
                    Com.Error(Defines.ERR_DROP, "Sound " + name + " has a bad loop length");
            }
            else
            {
                info.samples = samples;
                if (info.loopstart > 0)
                    info.samples -= info.loopstart;
            }

            info.dataofs = data_p;
            return info;
        }

        public class wavinfo_t
        {
            public int rate;
            public int width;
            public int channels;
            public int loopstart;
            public int samples;
            public int dataofs;
        }
    }
}
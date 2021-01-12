using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SharpAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Q2Sharp.Sound.Jsound
{
    public class SND_JAVA : Globals
    {
        static bool snd_inited = false;
        static cvar_t sndbits;
        static cvar_t sndspeed;
        static cvar_t sndchannels;
        public class dma_t
        {
            public int channels;
            public int samples;
            public int submission_chunk;
            public int samplebits;
            public int speed;
            public byte[] buffer;
        }

        public static SND_DMA.dma_t dma = new dma_t();
        public class SoundThread : IDisposable
        {
            byte[] b;
            AudioEngine l;
            int pos = 0;
            bool running = false;
            private Thread currentThread;

            public SoundThread(byte[] buffer, AudioEngine line )
            {
                b = buffer;
                l = line;
            }

            public virtual void Start()
			{
                currentThread = new Thread( Run );
                currentThread.Name = "SoundStream";
                currentThread.Start();
            }

            public virtual void Run()
            {
                running = true;
                while (running)
                {
                    line.Write(b, pos, 512);
                    pos = (pos + 512) % b.Length;
                }
            }

            public virtual void StopLoop()
            {
                lock (this)
                {
                    running = false;
                }
            }

            public virtual int GetSamplePos()
            {
                return pos >> 1;
            }

			public virtual void Dispose( )
			{
                running = false;
                currentThread.Join();
            }
		}

        static SoundThread thread;
        static AudioEngine audioEngine;
        static AudioBuffer audioBuffer;
        static AudioSource audioSource;
        static AudioFormat format;
        public static bool SNDDMA_Init()
        {
            if (snd_inited)
                return true;
            if (sndbits == null)
            {
                sndbits = Cvar.Get("sndbits", "16", CVAR_ARCHIVE);
                sndspeed = Cvar.Get("sndspeed", "0", CVAR_ARCHIVE);
                sndchannels = Cvar.Get("sndchannels", "1", CVAR_ARCHIVE);
            }

            audioEngine = AudioEngine.CreateOpenAL();
            audioBuffer = audioEngine.CreateBuffer();
            audioSource = audioEngine.CreateSource();

            format = new AudioFormat();
            format.BitsPerSample = 16;
            format.Channels = 2;
            format.SampleRate = 22050;

            audioBuffer.BufferData( dma.buffer, format );
            audioSource.QueueBuffer( audioBuffer );

            dma.buffer = new byte[65536];
            dma.channels = format.Channels;
            dma.samplebits = format.BitsPerSample;
            dma.samples = dma.buffer.Length / format.BytesPerSample;
            dma.speed = (int)format.SampleRate;
            dma.submission_chunk = 1;

            thread = new SoundThread(dma.buffer, audioEngine);
            thread.Start();
            snd_inited = true;
            return true;
        }

        public static int SNDDMA_GetDMAPos()
        {
            return thread.GetSamplePos();
        }

        public static void SNDDMA_Shutdown()
        {
            thread.StopLoop();
            thread.Dispose();            
            audioEngine.Dispose();
            snd_inited = false;
        }

        public static void SNDDMA_Submit()
        {
        }

        public static void SNDDMA_BeginPainting()
        {
        }
    }

    public class TestStereoProvider : WaveProvider16
    {
        public TestStereoProvider( )
            : base( 22050, 2 )
        { }

        short current;

        public override int Read( short[] buffer, int offset, int sampleCount )
        {
            for ( int sample = 0; sample < sampleCount; sample += 2 )
            {
                buffer[offset + sample] = current;
                buffer[offset + sample + 1] = ( short ) ( 0 - current );
                current++;
            }
            return sampleCount;
        }
    }
}
using J2N.IO;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Sound.Lwjgl
{
    public sealed class LWJGLSoundImpl : ISound
    {
        public const int MAX_SFX = Defines.MAX_SOUNDS * 2;
        public const int STREAM_QUEUE = 8;
        static sfx_t[] known_sfx = new sfx_t[MAX_SFX];

        static LWJGLSoundImpl()
        {
            S.Register(new LWJGLSoundImpl());

            for ( int i = 0; i < known_sfx.Length; i++ )
                known_sfx[i] = new sfx_t();
        }

        private cvar_t s_volume;
        private Int32Buffer buffers = Lib.NewInt32Buffer(MAX_SFX + STREAM_QUEUE);
        private LWJGLSoundImpl()
        {
        }

        public bool Init()
        {
            try
            {
                InitOpenAL();
                CheckError();
            }
            catch (Exception e)
            {
                Com.DPrintf(e.Message + '\\');
                return false;
            }

            s_volume = Cvar.Get("s_volume", "0.7", Defines.CVAR_ARCHIVE);
            AL10.AlGenBuffers(buffers);
            int count = Channel.Init(buffers);
            Com.Printf("... using " + count + " channels\\n");
            AL10.AlDistanceModel(AL10.AL_INVERSE_DISTANCE_CLAMPED);
            Cmd.AddCommand("play", new Anonymousxcommand_t(this));
            Cmd.AddCommand("stopsound", new Anonymousxcommand_t1(this));
            Cmd.AddCommand("soundlist", new Anonymousxcommand_t2(this));
            Cmd.AddCommand("soundinfo", new Anonymousxcommand_t3(this));
            num_sfx = 0;
            Com.Printf("sound sampling rate: 44100Hz\\n");
            StopAllSounds();
            Com.Printf("------------------------------------\\n");
            return true;
        }

        private sealed class Anonymousxcommand_t : xcommand_t
        {
            public Anonymousxcommand_t(LWJGLSoundImpl parent)
            {
                this.parent = parent;
            }

            private readonly LWJGLSoundImpl parent;
            public override void Execute()
            {
                parent.Play();
            }
        }

        private sealed class Anonymousxcommand_t1 : xcommand_t
        {
            public Anonymousxcommand_t1(LWJGLSoundImpl parent)
            {
                this.parent = parent;
            }

            private readonly LWJGLSoundImpl parent;
            public override void Execute()
            {
                parent.StopAllSounds();
            }
        }

        private sealed class Anonymousxcommand_t2 : xcommand_t
        {
            public Anonymousxcommand_t2(LWJGLSoundImpl parent)
            {
                this.parent = parent;
            }

            private readonly LWJGLSoundImpl parent;
            public override void Execute()
            {
                parent.SoundList();
            }
        }

        private sealed class Anonymousxcommand_t3 : xcommand_t
        {
            public Anonymousxcommand_t3(LWJGLSoundImpl parent)
            {
                this.parent = parent;
            }

            private readonly LWJGLSoundImpl parent;
            public override void Execute()
            {
                parent.SoundInfo_f();
            }
        }

        private void InitOpenAL()
        {
            try
            {
                AL.Create();
            }
            catch (LWJGLException e)
            {
                throw new OpenALException(e);
            }

            string deviceName = null;
            string os = System.GetProperty("os.name");
            if (os.StartsWith("Windows"))
            {
                deviceName = "DirectSound3D";
            }

            string defaultSpecifier = ALC10.AlcGetString(AL.GetDevice(), ALC10.ALC_DEFAULT_DEVICE_SPECIFIER);
            Com.Printf(os + " using " + ((deviceName == null) ? defaultSpecifier : deviceName) + '\\');
            if (ALC10.AlcGetError(AL.GetDevice()) != ALC10.ALC_NO_ERROR)
            {
                Com.DPrintf("Error with SoundDevice");
            }
        }

        void ExitOpenAL()
        {
            AL.Destroy();
        }

        private ByteBuffer sfxDataBuffer = Lib.NewByteBuffer(2 * 1024 * 1024);
        private void InitBuffer(byte[] samples, int bufferId, int freq)
        {
            ByteBuffer data = sfxDataBuffer.Slice();
            data.Put(samples).Flip();
            AL10.AlBufferData(buffers.Get(bufferId), AL10.AL_FORMAT_MONO16, data, freq);
        }

        private void CheckError()
        {
            Com.DPrintf("AL Error: " + AlErrorString() + '\\');
        }

        private string AlErrorString()
        {
            int error;
            string message = "";
            if ((error = AL10.AlGetError()) != AL10.AL_NO_ERROR)
            {
                switch ( error )
                {
                    case AL10.AL_INVALID_OPERATION:
                        message = "invalid operation";
                        break;
                    case AL10.AL_INVALID_VALUE:
                        message = "invalid value";
                        break;
                    case AL10.AL_INVALID_ENUM:
                        message = "invalid enum";
                        break;
                    case AL10.AL_INVALID_NAME:
                        message = "invalid name";
                        break;
                    default:
                        message = "" + error;
                        break;
                }
            }

            return message;
        }

        public void Shutdown()
        {
            StopAllSounds();
            Channel.Shutdown();
            AL10.AlDeleteBuffers(buffers);
            ExitOpenAL();
            Cmd.RemoveCommand("play");
            Cmd.RemoveCommand("stopsound");
            Cmd.RemoveCommand("soundlist");
            Cmd.RemoveCommand("soundinfo");
            for (int i = 0; i < num_sfx; i++)
            {
                if (known_sfx[i].name == null)
                    continue;
                known_sfx[i].Clear();
            }

            num_sfx = 0;
        }

        public void StartSound(float[] origin, int entnum, int entchannel, sfx_t sfx, float fvol, float attenuation, float timeofs)
        {
            if (sfx == null)
                return;
            if (sfx.name[0] == '*')
                sfx = RegisterSexedSound(Globals.cl_entities[entnum].current, sfx.name);
            if (LoadSound(sfx) == null)
                return;
            if (attenuation != Defines.ATTN_STATIC)
                attenuation *= 0.5F;
            PlaySound.Allocate(origin, entnum, entchannel, buffers.Get(sfx.bufferId), fvol, attenuation, timeofs);
        }

        private SingleBuffer listenerOrigin = Lib.NewSingleBuffer(3);
        private SingleBuffer listenerOrientation = Lib.NewSingleBuffer(6);
        public void Update(float[] origin, float[] forward, float[] right, float[] up)
        {
            Channel.ConvertVector(origin, listenerOrigin);
            AL10.AlListener(AL10.AL_POSITION, listenerOrigin);
            Channel.ConvertOrientation(forward, up, listenerOrientation);
            AL10.AlListener(AL10.AL_ORIENTATION, listenerOrientation);
            AL10.AlListenerf(AL10.AL_GAIN, s_volume.value);
            Channel.AddLoopSounds();
            Channel.AddPlaySounds();
            Channel.PlayAllSounds(listenerOrigin);
        }

        public void StopAllSounds()
        {
            AL10.AlListenerf(AL10.AL_GAIN, 0);
            PlaySound.Reset();
            Channel.Reset();
        }

        public string GetName()
        {
            return "lwjgl";
        }

        int s_registration_sequence;
        bool s_registering;
        public void BeginRegistration()
        {
            s_registration_sequence++;
            s_registering = true;
        }

        public sfx_t RegisterSound(string name)
        {
            sfx_t sfx = FindName(name, true);
            sfx.registration_sequence = s_registration_sequence;
            if (!s_registering)
                LoadSound(sfx);
            return sfx;
        }

        public void EndRegistration()
        {
            int i;
            sfx_t sfx;
            for (i = 0; i < num_sfx; i++)
            {
                sfx = known_sfx[i];
                if (sfx.name == null)
                    continue;
                if (sfx.registration_sequence != s_registration_sequence)
                {
                    sfx.Clear();
                }
            }

            for (i = 0; i < num_sfx; i++)
            {
                sfx = known_sfx[i];
                if (sfx.name == null)
                    continue;
                LoadSound(sfx);
            }

            s_registering = false;
        }

        sfx_t RegisterSexedSound(entity_state_t ent, string base_renamed)
        {
            sfx_t sfx = null;
            string model = null;
            int n = Globals.CS_PLAYERSKINS + ent.number - 1;
            if (Globals.cl.configstrings[n] != null)
            {
                int p = Globals.cl.configstrings[n].IndexOf('\\');
                if (p >= 0)
                {
                    p++;
                    model = Globals.cl.configstrings[n].Substring(p);
                    p = model.IndexOf('/');
                    if (p > 0)
                        model = model.Substring(0, p);
                }
            }

            if (model == null || model.Length == 0)
                model = "male";
            string sexedFilename = "#players/" + model + "/" + base_renamed.Substring(1);
            sfx = FindName(sexedFilename, false);
            if (sfx != null)
                return sfx;
            if (FS.FileLength(sexedFilename.Substring(1)) > 0)
            {
                return RegisterSound(sexedFilename);
            }

            if (model.EqualsIgnoreCase("female"))
            {
                string femaleFilename = "player/female/" + base_renamed.Substring(1);
                if (FS.FileLength("sound/" + femaleFilename) > 0)
                    return AliasName(sexedFilename, femaleFilename);
            }

            string maleFilename = "player/male/" + base_renamed.Substring(1);
            return AliasName(sexedFilename, maleFilename);
        }

        static int num_sfx;
        sfx_t FindName(string name, bool create)
        {
            int i;
            sfx_t sfx = null;
            if (name == null)
                Com.Error(Defines.ERR_FATAL, "S_FindName: NULL\\n");
            if (name.Length == 0)
                Com.Error(Defines.ERR_FATAL, "S_FindName: empty name\\n");
            if (name.Length >= Defines.MAX_QPATH)
                Com.Error(Defines.ERR_FATAL, "Sound name too long: " + name);
            for (i = 0; i < num_sfx; i++)
                if (name.Equals(known_sfx[i].name))
                {
                    return known_sfx[i];
                }

            if (!create)
                return null;
            for (i = 0; i < num_sfx; i++)
                if (known_sfx[i].name == null)
                    break;
            if (i == num_sfx)
            {
                if (num_sfx == MAX_SFX)
                    Com.Error(Defines.ERR_FATAL, "S_FindName: out of sfx_t");
                num_sfx++;
            }

            sfx = known_sfx[i];
            sfx.Clear();
            sfx.name = name;
            sfx.registration_sequence = s_registration_sequence;
            sfx.bufferId = i;
            return sfx;
        }

        sfx_t AliasName(string aliasname, string truename)
        {
            sfx_t sfx = null;
            string s;
            int i;
            s = new string (truename);
            for (i = 0; i < num_sfx; i++)
                if (known_sfx[i].name == null)
                    break;
            if (i == num_sfx)
            {
                if (num_sfx == MAX_SFX)
                    Com.Error(Defines.ERR_FATAL, "S_FindName: out of sfx_t");
                num_sfx++;
            }

            sfx = known_sfx[i];
            sfx.Clear();
            sfx.name = new string (aliasname);
            sfx.registration_sequence = s_registration_sequence;
            sfx.truename = s;
            sfx.bufferId = i;
            return sfx;
        }

        public sfxcache_t LoadSound(sfx_t s)
        {
            if (s.isCached)
                return s.cache;
            sfxcache_t sc = WaveLoader.LoadSound(s);
            if (sc != null)
            {
                InitBuffer(sc.data, s.bufferId, sc.speed);
                s.isCached = true;
                s.cache.data = null;
            }

            return sc;
        }

        public void StartLocalSound(string sound)
        {
            sfx_t sfx;
            sfx = RegisterSound(sound);
            if (sfx == null)
            {
                Com.Printf("S_StartLocalSound: can't cache " + sound + "\\n");
                return;
            }

            StartSound(null, Globals.cl.playernum + 1, 0, sfx, 1, 1, 0);
        }

        private Int16Buffer streamBuffer = sfxDataBuffer.Slice().Order(ByteOrder.BigEndian).AsInt16Buffer();
        public void RawSamples(int samples, int rate, int width, int channels, ByteBuffer data)
        {
            int format;
            if (channels == 2)
            {
                format = (width == 2) ? AL10.AL_FORMAT_STEREO16 : AL10.AL_FORMAT_STEREO8;
            }
            else
            {
                format = (width == 2) ? AL10.AL_FORMAT_MONO16 : AL10.AL_FORMAT_MONO8;
            }

            if (format == AL10.AL_FORMAT_MONO8)
            {
                Int16Buffer sampleData = streamBuffer;
                int value;
                for (int i = 0; i < samples; i++)
                {
                    value = (data.Get(i) & 0xFF) - 128;
                    sampleData.Put(i, (short)value);
                }

                format = AL10.AL_FORMAT_MONO16;
                width = 2;
                data = sfxDataBuffer.Slice();
            }

            Channel.UpdateStream(data, samples * channels * width, format, rate);
        }

        public void DisableStreaming()
        {
            Channel.DisableStreaming();
        }

        void Play()
        {
            int i;
            string name;
            sfx_t sfx;
            i = 1;
            while (i < Cmd.Argc())
            {
                name = new string (Cmd.Argv(i));
                if (name.IndexOf('.') == -1)
                    name += ".wav";
                sfx = RegisterSound(name);
                StartSound(null, Globals.cl.playernum + 1, 0, sfx, 1F, 1F, 0F);
                i++;
            }
        }

        void SoundList()
        {
            int i;
            sfx_t sfx;
            sfxcache_t sc;
            int size, total;
            total = 0;
            for (i = 0; i < num_sfx; i++)
            {
                sfx = known_sfx[i];
                if (sfx.registration_sequence == 0)
                    continue;
                sc = sfx.cache;
                if (sc != null)
                {
                    size = sc.length * sc.width * (sc.stereo + 1);
                    total += size;
                    if (sc.loopstart >= 0)
                        Com.Printf("L");
                    else
                        Com.Printf(" ");
                    Com.Printf("(%2db) %6i : %s\\n", sc.width * 8, size, sfx.name);
                }
                else
                {
                    if (sfx.name[0] == '*')
                        Com.Printf("  placeholder : " + sfx.name + "\\n");
                    else
                        Com.Printf("  not loaded  : " + sfx.name + "\\n");
                }
            }

            Com.Printf("Total resident: " + total + "\\n");
        }

        void SoundInfo_f()
        {
            Com.Printf("%5d stereo\\n", 1);
            Com.Printf("%5d samples\\n", 22050);
            Com.Printf("%5d samplebits\\n", 16);
            Com.Printf("%5d speed\\n", 44100);
        }
    }
}
using J2N.IO;
using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Sound.Lwjgl
{
    public class Channel
    {
        public const int LISTENER = 0;
        public const int FIXED = 1;
        public const int DYNAMIC = 2;
        public const int MAX_CHANNELS = 32;
        private static readonly SingleBuffer NULLVECTOR = Lib.NewSingleBuffer(3);
        private static Channel[] channels = new Channel[MAX_CHANNELS];
        private static Int32Buffer sources = Lib.NewInt32Buffer(MAX_CHANNELS);
        private static Int32Buffer buffers;
        private static Hashtable looptable = new Hashtable(MAX_CHANNELS);
        private static int numChannels;
        private static bool streamingEnabled = false;
        private static int streamQueue = 0;
        private int type;
        private int entnum;
        private int entchannel;
        private int bufferId;
        private int sourceId;
        private float volume;
        private float rolloff;
        private float[] origin = new float[]{0, 0, 0};
        private bool autosound;
        private bool active;
        private bool modified;
        private bool bufferChanged;
        private bool volumeChanged;
        private Channel(int sourceId)
        {
            this.sourceId = sourceId;
            Clear();
            volumeChanged = false;
            volume = 1F;
        }

        private void Clear()
        {
            entnum = entchannel = bufferId = -1;
            bufferChanged = false;
            rolloff = 0;
            autosound = false;
            active = false;
            modified = false;
        }

        private static Int32Buffer tmp = Lib.NewInt32Buffer(1);
        public static int Init(Int32Buffer buffers)
        {
            Channel.buffers = buffers;
            int sourceId;
            numChannels = 0;
            for (int i = 0; i < MAX_CHANNELS; i++)
            {
                try
                {
                    AL10.AlGenSources(tmp);
                    sourceId = tmp.Get(0);
                    if (sourceId <= 0)
                        break;
                }
                catch (OpenALException e)
                {
                    break;
                }

                sources.Put(i, sourceId);
                channels[i] = new Channel(sourceId);
                numChannels++;
                AL10.AlSourcef(sourceId, AL10.AL_GAIN, 1F);
                AL10.AlSourcef(sourceId, AL10.AL_PITCH, 1F);
                AL10.AlSourcei(sourceId, AL10.AL_SOURCE_RELATIVE, AL10.AL_FALSE);
                AL10.AlSource(sourceId, AL10.AL_VELOCITY, NULLVECTOR);
                AL10.AlSourcei(sourceId, AL10.AL_LOOPING, AL10.AL_FALSE);
                AL10.AlSourcef(sourceId, AL10.AL_REFERENCE_DISTANCE, 200F);
                AL10.AlSourcef(sourceId, AL10.AL_MIN_GAIN, 0.0005F);
                AL10.AlSourcef(sourceId, AL10.AL_MAX_GAIN, 1F);
            }

            sources.Limit = numChannels;
            return numChannels;
        }

        public static void Reset()
        {
            for (int i = 0; i < numChannels; i++)
            {
                AL10.AlSourceStop(sources.Get(i));
                AL10.AlSourcei(sources.Get(i), AL10.AL_BUFFER, 0);
                channels[i].Clear();
            }
        }

        public static void Shutdown()
        {
            AL10.AlDeleteSources(sources);
            numChannels = 0;
        }

        static void EnableStreaming()
        {
            if (streamingEnabled)
                return;
            numChannels--;
            streamingEnabled = true;
            streamQueue = 0;
            int source = channels[numChannels].sourceId;
            AL10.AlSourcei(source, AL10.AL_SOURCE_RELATIVE, AL10.AL_TRUE);
            AL10.AlSourcef(source, AL10.AL_GAIN, 1F);
            channels[numChannels].volumeChanged = true;
            Com.DPrintf("streaming enabled\\n");
        }

        public static void DisableStreaming()
        {
            if (!streamingEnabled)
                return;
            UnqueueStreams();
            int source = channels[numChannels].sourceId;
            AL10.AlSourcei(source, AL10.AL_SOURCE_RELATIVE, AL10.AL_FALSE);
            numChannels++;
            streamingEnabled = false;
            Com.DPrintf("streaming disabled\\n");
        }

        static void UnqueueStreams()
        {
            if (!streamingEnabled)
                return;
            int source = channels[numChannels].sourceId;
            AL10.AlSourceStop(source);
            int count = AL10.AlGetSourcei(source, AL10.AL_BUFFERS_QUEUED);
            Com.DPrintf("unqueue " + count + " buffers\\n");
            while (count-- > 0)
            {
                AL10.AlSourceUnqueueBuffers(source, tmp);
            }

            streamQueue = 0;
        }

        public static void UpdateStream(ByteBuffer samples, int count, int format, int rate)
        {
            EnableStreaming();
            int source = channels[numChannels].sourceId;
            int processed = AL10.AlGetSourcei(source, AL10.AL_BUFFERS_PROCESSED);
            bool playing = (AL10.AlGetSourcei(source, AL10.AL_SOURCE_STATE) == AL10.AL_PLAYING);
            bool interupted = !playing && streamQueue > 2;
            Int32Buffer buffer = tmp;
            if (interupted)
            {
                UnqueueStreams();
                buffer.Put(0, buffers.Get(Sound.MAX_SFX + streamQueue++));
                Com.DPrintf("queue " + (streamQueue - 1) + '\\');
            }
            else if (processed < 2)
            {
                if (streamQueue >= Sound.STREAM_QUEUE)
                    return;
                buffer.Put(0, buffers.Get(Sound.MAX_SFX + streamQueue++));
                Com.DPrintf("queue " + (streamQueue - 1) + '\\');
            }
            else
            {
                AL10.AlSourceUnqueueBuffers(source, buffer);
            }

            samples.Position = 0;
            samples.Limit = count;
            AL10.AlBufferData(buffer.Get(0), format, samples, rate);
            AL10.AlSourceQueueBuffers(source, buffer);
            if (streamQueue > 1 && !playing)
            {
                Com.DPrintf("start sound\\n");
                AL10.AlSourcePlay(source);
            }
        }

        public static void AddPlaySounds()
        {
            while (Channel.Assign(PlaySound.NextPlayableSound()))
                ;
        }

        private static bool Assign(PlaySound ps)
        {
            if (ps == null)
                return false;
            Channel ch = null;
            int i;
            for (i = 0; i < numChannels; i++)
            {
                ch = channels[i];
                if (ps.entchannel != 0 && ch.entnum == ps.entnum && ch.entchannel == ps.entchannel)
                {
                    if (ch.bufferId != ps.bufferId)
                    {
                        AL10.AlSourceStop(ch.sourceId);
                    }

                    break;
                }

                if ((ch.entnum == Globals.cl.playernum + 1) && (ps.entnum != Globals.cl.playernum + 1) && ch.bufferId != -1)
                    continue;
                if (!ch.active)
                {
                    break;
                }
            }

            if (i == numChannels)
                return false;
            ch.type = ps.type;
            if (ps.type == Channel.FIXED)
                Math3D.VectorCopy(ps.origin, ch.origin);
            ch.entnum = ps.entnum;
            ch.entchannel = ps.entchannel;
            ch.bufferChanged = (ch.bufferId != ps.bufferId);
            ch.bufferId = ps.bufferId;
            ch.rolloff = ps.attenuation * 2;
            ch.volumeChanged = (ch.volume != ps.volume);
            ch.volume = ps.volume;
            ch.active = true;
            ch.modified = true;
            return true;
        }

        private static Channel PickForLoop(int bufferId, float attenuation)
        {
            Channel ch;
            for (int i = 0; i < numChannels; i++)
            {
                ch = channels[i];
                if (!ch.active)
                {
                    ch.entnum = 0;
                    ch.entchannel = 0;
                    ch.bufferChanged = (ch.bufferId != bufferId);
                    ch.bufferId = bufferId;
                    ch.volumeChanged = (ch.volume != 1F);
                    ch.volume = 1F;
                    ch.rolloff = attenuation * 2;
                    ch.active = true;
                    ch.modified = true;
                    return ch;
                }
            }

            return null;
        }

        private static SingleBuffer sourceOriginBuffer = Lib.NewSingleBuffer(3);
        private static float[] entityOrigin = new float[]{0, 0, 0};
        public static void PlayAllSounds(SingleBuffer listenerOrigin)
        {
            SingleBuffer sourceOrigin = sourceOriginBuffer;
            Channel ch;
            int sourceId;
            int state;
            for (int i = 0; i < numChannels; i++)
            {
                ch = channels[i];
                if (ch.active)
                {
                    sourceId = ch.sourceId;
                    switch ( ch.type )
                    {
                        case Channel.LISTENER:
                            sourceOrigin.Put(0, listenerOrigin.Get(0));
                            sourceOrigin.Put(1, listenerOrigin.Get(1));
                            sourceOrigin.Put(2, listenerOrigin.Get(2));
                            break;
                        case Channel.DYNAMIC:
                            CL_ents.GetEntitySoundOrigin(ch.entnum, entityOrigin);
                            ConvertVector(entityOrigin, sourceOrigin);
                            break;
                        case Channel.FIXED:
                            ConvertVector(ch.origin, sourceOrigin);
                            break;
                    }

                    if (ch.modified)
                    {
                        if (ch.bufferChanged)
                        {
                            try
                            {
                                AL10.AlSourcei(sourceId, AL10.AL_BUFFER, ch.bufferId);
                            }
                            catch (OpenALException e)
                            {
                                AL10.AlSourceStop(sourceId);
                                AL10.AlSourcei(sourceId, AL10.AL_BUFFER, ch.bufferId);
                            }
                        }

                        if (ch.volumeChanged)
                        {
                            AL10.AlSourcef(sourceId, AL10.AL_GAIN, ch.volume);
                        }

                        AL10.AlSourcef(sourceId, AL10.AL_ROLLOFF_FACTOR, ch.rolloff);
                        AL10.AlSource(sourceId, AL10.AL_POSITION, sourceOrigin);
                        AL10.AlSourcePlay(sourceId);
                        ch.modified = false;
                    }
                    else
                    {
                        state = AL10.AlGetSourcei(sourceId, AL10.AL_SOURCE_STATE);
                        if (state == AL10.AL_PLAYING)
                        {
                            AL10.AlSource(sourceId, AL10.AL_POSITION, sourceOrigin);
                        }
                        else
                        {
                            ch.Clear();
                        }
                    }

                    ch.autosound = false;
                }
            }
        }

        public static void AddLoopSounds()
        {
            if ((Globals.cl_paused.value != 0F) || (Globals.cls.state != Globals.ca_active) || !Globals.cl.sound_prepped)
            {
                RemoveUnusedLoopSounds();
                return;
            }

            Channel ch;
            sfx_t sfx;
            sfxcache_t sc;
            int num;
            entity_state_t ent;
            Object key;
            int sound = 0;
            for (int i = 0; i < Globals.cl.frame.num_entities; i++)
            {
                num = (Globals.cl.frame.parse_entities + i) & (Defines.MAX_PARSE_ENTITIES - 1);
                ent = Globals.cl_parse_entities[num];
                sound = ent.sound;
                if (sound == 0)
                    continue;
                key = ent.number;
                ch = (Channel)looptable[key];
                if (ch != null)
                {
                    ch.autosound = true;
                    Math3D.VectorCopy(ent.origin, ch.origin);
                    continue;
                }

                sfx = Globals.cl.sound_precache[sound];
                if (sfx == null)
                    continue;
                sc = sfx.cache;
                if (sc == null)
                    continue;
                ch = Channel.PickForLoop(buffers.Get(sfx.bufferId), 6);
                if (ch == null)
                    break;
                ch.type = FIXED;
                Math3D.VectorCopy(ent.origin, ch.origin);
                ch.autosound = true;
                looptable.Add(key, ch);
                AL10.AlSourcei(ch.sourceId, AL10.AL_LOOPING, AL10.AL_TRUE);
            }

            RemoveUnusedLoopSounds();
        }

        private static void RemoveUnusedLoopSounds()
        {
            Channel ch;
            for (Iterator iter = looptable.Values().iterator(); iter.HasNext();)
            {
                ch = (Channel)iter.Next();
                if (!ch.autosound)
                {
                    AL10.AlSourceStop(ch.sourceId);
                    AL10.AlSourcei(ch.sourceId, AL10.AL_LOOPING, AL10.AL_FALSE);
                    iter.Remove();
                    ch.Clear();
                }
            }
        }

        public static void ConvertVector(float[] from, SingleBuffer to)
        {
            to.Put(0, from[0]);
            to.Put(1, from[2]);
            to.Put(2, -from[1]);
        }

        public static void ConvertOrientation(float[] forward, float[] up, SingleBuffer orientation)
        {
            orientation.Put(0, forward[0]);
            orientation.Put(1, forward[2]);
            orientation.Put(2, -forward[1]);
            orientation.Put(3, up[0]);
            orientation.Put(4, up[2]);
            orientation.Put(5, -up[1]);
        }
    }
}
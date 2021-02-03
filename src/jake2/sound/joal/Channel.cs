using J2N.IO;
using Q2Sharp.Qcommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio.OpenAL;
using System.Collections;
using Q2Sharp.Util;
using Q2Sharp.Game;
using Q2Sharp.Client;

namespace Q2Sharp.Sound.Joal
{
    public class Channel
    {
        public const int LISTENER = 0;
        public const int FIXED = 1;
        public const int DYNAMIC = 2;
        public const int MAX_CHANNELS = 32;
        public static readonly float[] NULLVECTOR = new float[]{0, 0, 0};
        private static Channel[] channels = new Channel[MAX_CHANNELS];
        private static int[] sources = new int[MAX_CHANNELS];
        private static int[] buffers;
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
            this.volumeChanged = false;
            this.volume = 1F;
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
        
        private static int[] tmp = new int[1];
        public static int Init(int[] buffers)
        {
            Channel.buffers = buffers;
            int sourceId;
            numChannels = 0;
            for (int i = 0; i < MAX_CHANNELS; i++)
            {
                try
                {
                    AL.GenSources(1, tmp);
                    sourceId = tmp[0];
                    if (sourceId <= 0)
                        break;
                }
                catch (Exception e)
                {
                    break;
                }

                sources[i] = sourceId;
                channels[i] = new Channel(sourceId);
                numChannels++;
                AL.Source(sourceId, ALSourcef.Gain, 1F);
                AL.Source(sourceId, ALSourcef.Pitch, 1F);
                AL.Source(sourceId, ALSourceb.SourceRelative, false);
                AL.Source(sourceId, ALSource3f.Velocity, NULLVECTOR[0], NULLVECTOR[1], NULLVECTOR[2]);
                AL.Source(sourceId, ALSourceb.Looping, false);
                AL.Source(sourceId, ALSourcef.ReferenceDistance, 200F);
                AL.Source(sourceId, ALSourcef.MinGain, 0.0005F);
                AL.Source(sourceId, ALSourcef.MaxGain, 1F);
            }

            return numChannels;
        }

        public static void Reset()
        {
            for (int i = 0; i < numChannels; i++)
            {
                AL.SourceStop(sources[i]);
                AL.Source(sources[i], ALSourcei.Buffer, 0);
                channels[i].Clear();
            }
        }

        public static void Shutdown()
        {
            AL.DeleteSources(sources);
            numChannels = 0;
        }

        public static void EnableStreaming()
        {
            if (streamingEnabled)
                return;
            numChannels--;
            streamingEnabled = true;
            streamQueue = 0;
            int source = channels[numChannels].sourceId;
            AL.Source(source, ALSourceb.SourceRelative, true);
            AL.Source(source, ALSourcef.Gain, 1F);
            channels[numChannels].volumeChanged = true;
            Com.DPrintf("streaming enabled\\n");
        }

        public static void DisableStreaming()
        {
            if (!streamingEnabled)
                return;
            UnqueueStreams();
            int source = channels[numChannels].sourceId;
            AL.Source(source, ALSourceb.SourceRelative, false);
            numChannels++;
            streamingEnabled = false;
            Com.DPrintf("streaming disabled\\n");
        }

        static void UnqueueStreams()
        {
            if (!streamingEnabled)
                return;
            int source = channels[numChannels].sourceId;
            AL.SourceStop(source);
            int[] tmpCount = new int[] {0};
            AL.GetSource(source, ALGetSourcei.BuffersQueued, out tmpCount[0]);
            int count = tmpCount[0];
            Com.DPrintf("unqueue " + count + " buffers\\n");
            while (count-- > 0)
            {
                AL.SourceUnqueueBuffers(source, 1, tmp);
            }

            streamQueue = 0;
        }

        public static void UpdateStream(ByteBuffer samples, int count, ALFormat format, int rate)
        {
            EnableStreaming();
            int[] buffer = new int[] { 0 };
            int source = channels[numChannels].sourceId;
            int[] tmp = new int[] {0};
            AL.GetSource(source, ALGetSourcei.BuffersProcessed, out tmp[0]);
            int processed = tmp[0];
            AL.GetSource(source, ALGetSourcei.SourceState, out tmp[0]);
            int state = tmp[0];
            bool playing = (state == ( int ) ALSourceState.Playing);
            bool interupted = !playing && streamQueue > 2;
            if (interupted)
            {
                UnqueueStreams();
                buffer[0] = buffers[JOALSoundImpl.MAX_SFX + streamQueue++];
                Com.DPrintf("queue " + (streamQueue - 1) + '\\');
            }
            else if (processed < 2)
            {
                if (streamQueue >= JOALSoundImpl.STREAM_QUEUE)
                    return;
                buffer[0] = buffers[JOALSoundImpl.MAX_SFX + streamQueue++];
                Com.DPrintf("queue " + (streamQueue - 1) + '\\');
            }
            else
            {
                AL.SourceUnqueueBuffers(source, 1, buffer);
            }

            samples.Position = 0;
            samples.Limit = count;
            AL.BufferData(buffer[0], format, ref samples.Array[0], count, rate);
            AL.SourceQueueBuffers(source, 1, buffer);
            if (streamQueue > 1 && !playing)
            {
                Com.DPrintf("start sound\\n");
                AL.SourcePlay(source);
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
                        AL.SourceStop(ch.sourceId);
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
            ch.volumeChanged = (ch.volume != ps.volume);
            ch.volume = ps.volume;
            ch.rolloff = ps.attenuation * 2;
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
                    ch.volumeChanged = (ch.volume < 1F);
                    ch.volume = 1F;
                    ch.rolloff = attenuation * 2;
                    ch.active = true;
                    ch.modified = true;
                    return ch;
                }
            }

            return null;
        }

        private static float[] entityOrigin = new float[]{0, 0, 0};
        private static float[] sourceOrigin = new float[]{0, 0, 0};
        public static void PlayAllSounds(float[] listenerOrigin)
        {
            Channel ch;
            int sourceId;
            ALSourceState state;
            int[] tmp = new int[]{0};
            for (int i = 0; i < numChannels; i++)
            {
                ch = channels[i];
                if (ch.active)
                {
                    sourceId = ch.sourceId;
                    switch (ch.type)

                    {
                        case Channel.LISTENER:
                            Math3D.VectorCopy(listenerOrigin, sourceOrigin);
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
                                AL.Source(sourceId, ALSourcei.Buffer, ch.bufferId);
                            }
                            catch (Exception e)
                            {
                                AL.SourceStop(sourceId);
                                AL.Source(sourceId, ALSourcei.Buffer, ch.bufferId);
                            }
                        }

                        if (ch.volumeChanged)
                        {
                            AL.Source(sourceId, ALSourcef.Gain, ch.volume);
                        }

                        AL.Source(sourceId, ALSourcef.RolloffFactor, ch.rolloff);
                        AL.Source(sourceId, ALSource3f.Position, sourceOrigin[0], sourceOrigin[1], sourceOrigin[2]);
                        AL.SourcePlay(sourceId);
                        ch.modified = false;
                    }
                    else
                    {
                        AL.GetSource(sourceId, ALGetSourcei.SourceState, out tmp[0]);
                        state = (ALSourceState)tmp[0];
                        if (state == ALSourceState.Playing)
                        {
                            AL.Source(sourceId, ALSource3f.Position, sourceOrigin[0], sourceOrigin[1], sourceOrigin[2]);
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
                ch = (Channel)( looptable.ContainsKey(key) ? looptable[key] : null );
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
                ch = Channel.PickForLoop(buffers[sfx.bufferId], 6);
                if (ch == null)
                    break;
                ch.type = FIXED;
                Math3D.VectorCopy(ent.origin, ch.origin);
                ch.autosound = true;
                looptable.Add(key, ch);
                AL.Source(ch.sourceId, ALSourceb.Looping, true);
            }

            RemoveUnusedLoopSounds();
        }

        private static void RemoveUnusedLoopSounds()
        {
            Channel ch;
            var i = 0;
            var c = looptable.Count;
            var toRemove = new List<int>();
            for (var iter = looptable.Values.GetEnumerator(); i < c;)
            foreach ( int key in looptable.Keys )
            {
                ch = (Channel)looptable[key];
                if (!ch.autosound)
                {
                    AL.SourceStop(ch.sourceId);
                    AL.Source(ch.sourceId, ALSourceb.Looping, false);
                    toRemove.Add(key);
                    ch.Clear();
                }
                i++;
            }

            foreach ( var key in toRemove )
                looptable.Remove( key );
        }

        public static void ConvertVector(float[] from, float[] to)
        {
            to[0] = from[0];
            to[1] = from[2];
            to[2] = -from[1];
        }

        public static void ConvertOrientation(float[] forward, float[] up, float[] orientation)
        {
            orientation[0] = forward[0];
            orientation[1] = forward[2];
            orientation[2] = -forward[1];
            orientation[3] = up[0];
            orientation[4] = up[2];
            orientation[5] = -up[1];
        }
    }
}
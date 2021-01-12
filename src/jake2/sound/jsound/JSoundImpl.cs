using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Sound.Jsound
{
    public class JSoundImpl : ISound
    {
        static JSoundImpl()
        {
            S.Register(new JSoundImpl());
        }

        public virtual bool Init()
        {
            SND_DMA.Init();
            if (SND_DMA.sound_started)
                return true;
            return false;
        }

        public virtual void Shutdown()
        {
            SND_DMA.Shutdown();
        }

        public virtual void StartSound(float[] origin, int entnum, int entchannel, sfx_t sfx, float fvol, float attenuation, float timeofs)
        {
            SND_DMA.StartSound(origin, entnum, entchannel, sfx, fvol, attenuation, timeofs);
        }

        public virtual void StopAllSounds()
        {
            SND_DMA.StopAllSounds();
        }

        public virtual void Update(float[] origin, float[] forward, float[] right, float[] up)
        {
            SND_DMA.Update(origin, forward, right, up);
        }

        public virtual string GetName()
        {
            return "jsound";
        }

        public virtual void BeginRegistration()
        {
            SND_DMA.BeginRegistration();
        }

        public virtual sfx_t RegisterSound(string sample)
        {
            return SND_DMA.RegisterSound(sample);
        }

        public virtual void EndRegistration()
        {
            SND_DMA.EndRegistration();
        }

        public virtual void StartLocalSound(string sound)
        {
            SND_DMA.StartLocalSound(sound);
        }

        public virtual void RawSamples(int samples, int rate, int width, int channels, ByteBuffer data)
        {
            SND_DMA.RawSamples(samples, rate, width, channels, data);
        }

        public virtual void DisableStreaming()
        {
        }
    }
}
using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Sound
{
    public sealed class DummyDriver : ISound
    {
        static DummyDriver()
        {
            S.Register(new DummyDriver());
        }

        private DummyDriver()
        {
        }

        public bool Init()
        {
            return true;
        }

        public void Shutdown()
        {
        }

        public void BeginRegistration()
        {
        }

        public sfx_t RegisterSound(string sample)
        {
            return null;
        }

        public void EndRegistration()
        {
        }

        public void StartLocalSound(string sound)
        {
        }

        public void StartSound(float[] origin, int entnum, int entchannel, sfx_t sfx, float fvol, float attenuation, float timeofs)
        {
        }

        public void Update(float[] origin, float[] forward, float[] right, float[] up)
        {
        }

        public void RawSamples(int samples, int rate, int width, int channels, ByteBuffer data)
        {
        }

        public void DisableStreaming()
        {
        }

        public void StopAllSounds()
        {
        }

        public string GetName()
        {
            return "dummy";
        }
    }
}
using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Sound
{
    public interface ISound
    {
        string GetName();
        bool Init();
        void Shutdown();
        void BeginRegistration();
        sfx_t RegisterSound(string sample);
        void EndRegistration();
        void StartLocalSound(string sound);
        void StartSound(float[] origin, int entnum, int entchannel, sfx_t sfx, float fvol, float attenuation, float timeofs);
        void Update(float[] origin, float[] forward, float[] right, float[] up);
        void RawSamples(int samples, int rate, int width, int channels, ByteBuffer data);
        void DisableStreaming();
        void StopAllSounds();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Sound
{
    public class sfxcache_t
    {
        public int length;
        public int loopstart;
        public int speed;
        public int width;
        public int stereo;
        public byte[] data;
        public sfxcache_t(int size)
        {
            data = new byte[size];
        }
    }
}
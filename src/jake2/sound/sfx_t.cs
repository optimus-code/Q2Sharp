using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Sound
{
    public class sfx_t
    {
        public string name;
        public int registration_sequence;
        public sfxcache_t cache;
        public string truename;
        public int bufferId = -1;
        public bool isCached = false;
        public virtual void Clear()
        {
            name = truename = null;
            cache = null;
            registration_sequence = 0;
            bufferId = -1;
            isCached = false;
        }
    }
}
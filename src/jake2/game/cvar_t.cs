using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public sealed class cvar_t
    {
        public string name;
        public string string_renamed;
        public string latched_string;
        public int flags = 0;
        public bool modified = false;
        public float value = 0F;
        public cvar_t next = null;
    }
}
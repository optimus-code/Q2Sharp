using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public abstract class EntBlockedAdapter : SuperAdapter
    {
        public abstract void Blocked(edict_t self, edict_t other);
    }
}
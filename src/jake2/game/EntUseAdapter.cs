using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public abstract class EntUseAdapter : SuperAdapter
    {
        public abstract void Use(edict_t self, edict_t other, edict_t activator);
    }
}
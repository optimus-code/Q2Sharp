using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public abstract class EntThinkAdapter : SuperAdapter
    {
        public abstract bool Think(edict_t self);
    }
}
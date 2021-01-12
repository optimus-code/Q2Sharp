using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public abstract class AIAdapter : SuperAdapter
    {
        public abstract void Ai(edict_t self, float dist);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public abstract class EntPainAdapter : SuperAdapter
    {
        public abstract void Pain(edict_t self, edict_t other, float kick, int damage);
    }
}
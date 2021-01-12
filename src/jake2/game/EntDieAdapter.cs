using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public abstract class EntDieAdapter : SuperAdapter
    {
        public abstract void Die(edict_t self, edict_t inflictor, edict_t attacker, int damage, float[] point);
    }
}
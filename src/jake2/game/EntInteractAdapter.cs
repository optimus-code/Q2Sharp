using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public abstract class EntInteractAdapter : SuperAdapter
    {
        public abstract bool Interact(edict_t self, edict_t other);
    }
}
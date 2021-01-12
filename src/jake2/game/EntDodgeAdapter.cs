using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public abstract class EntDodgeAdapter : SuperAdapter
    {
        public abstract void Dodge(edict_t self, edict_t other, float eta);
    }
}
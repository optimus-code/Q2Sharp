using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public abstract class ItemDropAdapter : SuperAdapter
    {
        public virtual void Drop(edict_t ent, gitem_t item)
        {
        }
    }
}
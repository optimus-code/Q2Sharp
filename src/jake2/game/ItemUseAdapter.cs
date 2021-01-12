using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public abstract class ItemUseAdapter : SuperAdapter
    {
        public virtual void Use(edict_t ent, gitem_t item)
        {
        }
    }
}
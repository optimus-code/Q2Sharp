using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class EdictFindFilter
    {
        public virtual bool Matches(edict_t e, string s)
        {
            return false;
        }
    }
}
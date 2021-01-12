using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public abstract class EntTouchAdapter : SuperAdapter
    {
        public abstract void Touch(edict_t self, edict_t other, cplane_t plane, csurface_t surf);
    }
}
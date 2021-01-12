using Jake2.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Server
{
    public class areanode_t
    {
        public int axis;
        public float dist;
        public areanode_t[] children = new areanode_t[2];
        public link_t trigger_edicts;
        public link_t solid_edicts;

        public areanode_t()
        {
            trigger_edicts = new link_t( this );
            solid_edicts = new link_t( this );
        }
    }
}
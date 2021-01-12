using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class spawn_t
    {
        public spawn_t(string name, EntThinkAdapter spawn)
        {
            this.name = name;
            this.spawn = spawn;
        }

        public string name;
        public EntThinkAdapter spawn;
    }
}
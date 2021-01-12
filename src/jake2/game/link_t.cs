using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class link_t
    {
        public link_t(Object o)
        {
            this.o = o;
        }

        public link_t prev, next;
        public Object o;
    }
}
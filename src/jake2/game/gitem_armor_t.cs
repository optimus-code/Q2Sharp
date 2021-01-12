using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class gitem_armor_t
    {
        public gitem_armor_t(int base_count, int max_count, float normal_protection, float energy_protection, int armor)
        {
            this.base_count = base_count;
            this.max_count = max_count;
            this.normal_protection = normal_protection;
            this.energy_protection = energy_protection;
            this.armor = armor;
        }

        public int base_count;
        public int max_count;
        public float normal_protection;
        public float energy_protection;
        public int armor;
    }
}
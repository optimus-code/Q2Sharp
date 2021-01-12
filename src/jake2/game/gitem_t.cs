using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class gitem_t
    {
        private static int id = 0;
        public gitem_t(int xxx)
        {
            index = xxx;
        }

        public gitem_t(string classname, EntInteractAdapter pickup, ItemUseAdapter use, ItemDropAdapter drop, EntThinkAdapter weaponthink)
        {
        }

        public gitem_t(string classname, EntInteractAdapter pickup, ItemUseAdapter use, ItemDropAdapter drop, EntThinkAdapter weaponthink, string pickup_sound, string world_model, int world_model_flags, string view_model, string icon, string pickup_name, int count_width, int quantity, string ammo, int flags, int weapmodel, gitem_armor_t info, int tag, string precaches)
        {
            this.classname = classname;
            this.pickup = pickup;
            this.use = use;
            this.drop = drop;
            this.weaponthink = weaponthink;
            this.pickup_sound = pickup_sound;
            this.world_model = world_model;
            this.world_model_flags = world_model_flags;
            this.view_model = view_model;
            this.icon = icon;
            this.pickup_name = pickup_name;
            this.count_width = count_width;
            this.quantity = quantity;
            this.ammo = ammo;
            this.flags = flags;
            this.weapmodel = weapmodel;
            this.info = info;
            this.tag = tag;
            this.precaches = precaches;
            this.index = id++;
        }

        public string classname;
        public EntInteractAdapter pickup;
        public ItemUseAdapter use;
        public ItemDropAdapter drop;
        public EntThinkAdapter weaponthink;
        public string pickup_sound;
        public string world_model;
        public int world_model_flags;
        public string view_model;
        public string icon;
        public string pickup_name;
        public int count_width;
        public int quantity;
        public string ammo;
        public int flags;
        public int weapmodel;
        public gitem_armor_t info;
        public int tag;
        public string precaches;
        public int index;
    }
}
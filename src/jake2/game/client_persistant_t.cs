using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class client_persistant_t
    {
        public virtual void Set(client_persistant_t from)
        {
            userinfo = from.userinfo;
            netname = from.netname;
            hand = from.hand;
            connected = from.connected;
            health = from.health;
            max_health = from.max_health;
            savedFlags = from.savedFlags;
            selected_item = from.selected_item;
            System.Array.Copy(from.inventory, 0, inventory, 0, inventory.Length);
            max_bullets = from.max_bullets;
            max_shells = from.max_shells;
            max_rockets = from.max_rockets;
            max_grenades = from.max_grenades;
            max_cells = from.max_cells;
            max_slugs = from.max_slugs;
            weapon = from.weapon;
            lastweapon = from.lastweapon;
            power_cubes = from.power_cubes;
            score = from.score;
            game_helpchanged = from.game_helpchanged;
            helpchanged = from.helpchanged;
            spectator = from.spectator;
        }

        public string userinfo = "";
        public string netname = "";
        public int hand;
        public bool connected;
        public int health;
        public int max_health;
        public int savedFlags;
        public int selected_item;
        public int[] inventory = new int[Defines.MAX_ITEMS];
        public int max_bullets;
        public int max_shells;
        public int max_rockets;
        public int max_grenades;
        public int max_cells;
        public int max_slugs;
        public gitem_t weapon;
        public gitem_t lastweapon;
        public int power_cubes;
        public int score;
        public int game_helpchanged;
        public int helpchanged;
        public bool spectator;
        public virtual void Read(QuakeFile f)
        {
            userinfo = f.ReadString();
            netname = f.ReadString();
            hand = f.ReadInt32();
            connected = f.ReadInt32() != 0;
            health = f.ReadInt32();
            max_health = f.ReadInt32();
            savedFlags = f.ReadInt32();
            selected_item = f.ReadInt32();
            for (int n = 0; n < Defines.MAX_ITEMS; n++)
                inventory[n] = f.ReadInt32();
            max_bullets = f.ReadInt32();
            max_shells = f.ReadInt32();
            max_rockets = f.ReadInt32();
            max_grenades = f.ReadInt32();
            max_cells = f.ReadInt32();
            max_slugs = f.ReadInt32();
            weapon = f.ReadItem();
            lastweapon = f.ReadItem();
            power_cubes = f.ReadInt32();
            score = f.ReadInt32();
            game_helpchanged = f.ReadInt32();
            helpchanged = f.ReadInt32();
            spectator = f.ReadInt32() != 0;
        }

        public virtual void Write(QuakeFile f)
        {
            f.Write( userinfo);
            f.Write( netname);
            f.Write(hand);
            f.Write( connected ? 1 : 0);
            f.Write( health);
            f.Write( max_health);
            f.Write( savedFlags);
            f.Write( selected_item);
            for (int n = 0; n < Defines.MAX_ITEMS; n++)
                f.Write( inventory[n]);
            f.Write( max_bullets);
            f.Write( max_shells);
            f.Write( max_rockets);
            f.Write( max_grenades);
            f.Write( max_cells);
            f.Write( max_slugs);
            f.WriteItem(weapon);
            f.WriteItem(lastweapon);
            f.Write( power_cubes);
            f.Write( score);
            f.Write( game_helpchanged);
            f.Write( helpchanged);
            f.Write( spectator ? 1 : 0);
        }
    }
}
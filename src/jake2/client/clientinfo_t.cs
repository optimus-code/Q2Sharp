using Q2Sharp.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class clientinfo_t
    {
        public string name = "";
        public string cinfo = "";
        public image_t skin;
        public image_t icon;
        public string iconname = "";
        public model_t model;
        public model_t[] weaponmodel = new model_t[Defines.MAX_CLIENTWEAPONMODELS];
        public virtual void Set(clientinfo_t from)
        {
            name = from.name;
            cinfo = from.cinfo;
            skin = from.skin;
            icon = from.icon;
            iconname = from.iconname;
            model = from.model;
            System.Array.Copy(from.weaponmodel, 0, weaponmodel, 0, Defines.MAX_CLIENTWEAPONMODELS);
        }
    }
}
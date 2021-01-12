using Q2Sharp.Qcommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public abstract class SuperAdapter
    {
        public SuperAdapter()
        {
            Register(this, GetID());
        }

        private static void Register(SuperAdapter sa, string id)
        {
            adapters.Add(id, sa);
        }

        private static Hashtable adapters = new Hashtable();
        public static SuperAdapter GetFromID(string key)
        {
            SuperAdapter sa = (SuperAdapter)adapters[key];
            if (sa == null)
            {
                Com.DPrintf("SuperAdapter.getFromID():adapter not found->" + key + "\\n");
            }

            return sa;
        }

        public abstract string GetID();
    }
}
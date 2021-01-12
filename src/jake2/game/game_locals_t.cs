using Jake2.Qcommon;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class game_locals_t
    {
        public string helpmessage1 = "";
        public string helpmessage2 = "";
        public int helpchanged;
        public gclient_t[] clients = new gclient_t[Defines.MAX_CLIENTS];
        public string spawnpoint = "";
        public int maxclients;
        public int maxentities;
        public int serverflags;
        public int num_items;
        public bool autosaved;
        public virtual void Load(QuakeFile f)
        {
            string date = f.ReadString();
            helpmessage1 = f.ReadString();
            helpmessage2 = f.ReadString();
            helpchanged = f.ReadInt32();
            spawnpoint = f.ReadString();
            maxclients = f.ReadInt32();
            maxentities = f.ReadInt32();
            serverflags = f.ReadInt32();
            num_items = f.ReadInt32();
            autosaved = f.ReadInt32() != 0;
            if (f.ReadInt32() != 1928)
                Com.DPrintf("error in loading game_locals, 1928\\n");
        }

        public virtual void Write(QuakeFile f)
        {
            f.Write(DateTime.Now.ToString());
            f.Write( helpmessage1);
            f.Write( helpmessage2);
            f.Write( helpchanged);
            f.Write( spawnpoint);
            f.Write( maxclients);
            f.Write( maxentities);
            f.Write( serverflags);
            f.Write( num_items);
            f.Write( autosaved ? 1 : 0);
            f.Write( 1928);
        }
    }
}
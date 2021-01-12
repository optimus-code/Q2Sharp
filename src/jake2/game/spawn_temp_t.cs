using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class spawn_temp_t
    {
        public string sky = "";
        public float skyrotate;
        public float[] skyaxis = new float[]{0, 0, 0};
        public string nextmap = "";
        public int lip;
        public int distance;
        public int height;
        public string noise = "";
        public float pausetime;
        public string item = "";
        public string gravity = "";
        public float minyaw;
        public float maxyaw;
        public float minpitch;
        public float maxpitch;
        public virtual bool Set(string key, string value)
        {
            if (key.Equals("lip"))
            {
                lip = Lib.Atoi(value);
                return true;
            }

            if (key.Equals("distance"))
            {
                distance = Lib.Atoi(value);
                return true;
            }

            if (key.Equals("height"))
            {
                height = Lib.Atoi(value);
                return true;
            }

            if (key.Equals("noise"))
            {
                noise = GameSpawn.ED_NewString(value);
                return true;
            }

            if (key.Equals("pausetime"))
            {
                pausetime = Lib.Atof(value);
                return true;
            }

            if (key.Equals("item"))
            {
                item = GameSpawn.ED_NewString(value);
                return true;
            }

            if (key.Equals("gravity"))
            {
                gravity = GameSpawn.ED_NewString(value);
                return true;
            }

            if (key.Equals("sky"))
            {
                sky = GameSpawn.ED_NewString(value);
                return true;
            }

            if (key.Equals("skyrotate"))
            {
                skyrotate = Lib.Atof(value);
                return true;
            }

            if (key.Equals("skyaxis"))
            {
                skyaxis = Lib.Atov(value);
                return true;
            }

            if (key.Equals("minyaw"))
            {
                minyaw = Lib.Atof(value);
                return true;
            }

            if (key.Equals("maxyaw"))
            {
                maxyaw = Lib.Atof(value);
                return true;
            }

            if (key.Equals("minpitch"))
            {
                minpitch = Lib.Atof(value);
                return true;
            }

            if (key.Equals("maxpitch"))
            {
                maxpitch = Lib.Atof(value);
                return true;
            }

            if (key.Equals("nextmap"))
            {
                nextmap = GameSpawn.ED_NewString(value);
                return true;
            }

            return false;
        }
    }
}
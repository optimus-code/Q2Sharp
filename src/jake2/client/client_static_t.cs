using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class client_static_t
    {
        public int state;
        public int key_dest;
        public int framecount;
        public int realtime;
        public float frametime;
        public float disable_screen;
        public int disable_servercount;
        public string servername = "";
        public float connect_time;
        public int quakePort;
        public netchan_t netchan = new netchan_t();
        public int serverProtocol;
        public int challenge;
        public QuakeFile download;
        public string downloadtempname = "";
        public string downloadname = "";
        public int downloadnumber;
        public int downloadtype;
        public int downloadpercent;
        public bool demorecording;
        public bool demowaiting;
        public QuakeFile demofile;
    }
}
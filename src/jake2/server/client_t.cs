using Jake2.Game;
using Jake2.Qcommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Server
{
    public class client_t
    {
        public client_t()
        {
            for (int n = 0; n < Defines.UPDATE_BACKUP; n++)
            {
                frames[n] = new client_frame_t();
            }
        }

        public static readonly int LATENCY_COUNTS = 16;
        public static readonly int RATE_MESSAGES = 10;
        public int state;
        public string userinfo = "";
        public int lastframe;
        public usercmd_t lastcmd = new usercmd_t();
        public int commandMsec;
        public int[] frame_latency = new int[LATENCY_COUNTS];
        public int ping;
        public int[] message_size = new int[RATE_MESSAGES];
        public int rate;
        public int surpressCount;
        public edict_t edict;
        public string name = "";
        public int messagelevel;
        public sizebuf_t datagram = new sizebuf_t();
        public byte[] datagram_buf = new byte[Defines.MAX_MSGLEN];
        public client_frame_t[] frames = new client_frame_t[Defines.UPDATE_BACKUP];
        public byte[] download;
        public int downloadsize;
        public int downloadcount;
        public int lastmessage;
        public int lastconnect;
        public int challenge;
        public netchan_t netchan = new netchan_t();
        public int serverindex;
    }
}
using Jake2.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Server
{
    public class client_frame_t
    {
        public int areabytes;
        public byte[] areabits = new byte[Defines.MAX_MAP_AREAS / 8];
        public player_state_t ps = new player_state_t();
        public int num_entities;
        public int first_entity;
        public int senttime;
    }
}
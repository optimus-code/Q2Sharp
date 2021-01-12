using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;

namespace Q2Sharp.Server
{
	public class server_static_t
	{
		public server_static_t( )
		{
			for ( var n = 0; n < Defines.MAX_CHALLENGES; n++ )
			{
				challenges[n] = new challenge_t();
			}
		}

		public Boolean initialized;
		public Int32 realtime;
		public String mapcmd = "";
		public Int32 spawncount;
		public client_t[] clients;
		public Int32 num_client_entities;
		public Int32 next_client_entities;
		public entity_state_t[] client_entities;
		public Int32 last_heartbeat;
		public challenge_t[] challenges = new challenge_t[Defines.MAX_CHALLENGES];
		public QuakeFile demofile;
		public sizebuf_t demo_multicast = new sizebuf_t();
		public Byte[] demo_multicast_buf = new Byte[Defines.MAX_MSGLEN];
	}
}
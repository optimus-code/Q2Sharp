using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using System;

namespace Jake2.Server
{
	public class server_t
	{
		public server_t( )
		{
			models = new cmodel_t[Defines.MAX_MODELS];
			for ( var n = 0; n < Defines.MAX_MODELS; n++ )
				models[n] = new cmodel_t();
			for ( var n = 0; n < Defines.MAX_EDICTS; n++ )
				baselines[n] = new entity_state_t( null );
		}

		public Int32 state;
		public Boolean attractloop;
		public Boolean loadgame;
		public Int32 time;
		public Int32 framenum;
		public String name = "";
		public cmodel_t[] models;
		public String[] configstrings = new String[Defines.MAX_CONFIGSTRINGS];
		public entity_state_t[] baselines = new entity_state_t[Defines.MAX_EDICTS];
		public sizebuf_t multicast = new sizebuf_t();
		public Byte[] multicast_buf = new Byte[Defines.MAX_MSGLEN];
		public QuakeFile demofile;
		public Boolean timedemo;
	}
}
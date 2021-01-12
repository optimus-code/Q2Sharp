using Jake2.Game;
using Jake2.Util;
using System;

namespace Jake2.Client
{
	public class frame_t
	{
		public static readonly Int32 MAX_MAP_AREAS = 256;
		public Boolean valid;
		public Int32 serverframe;
		public Int32 servertime;
		public Int32 deltaframe;
		public Byte[] areabits = new Byte[MAX_MAP_AREAS / 8];
		public player_state_t playerstate = new player_state_t();
		public Int32 num_entities;
		public Int32 parse_entities;
		public virtual void Set( frame_t from )
		{
			valid = from.valid;
			serverframe = from.serverframe;
			deltaframe = from.deltaframe;
			num_entities = from.num_entities;
			parse_entities = from.parse_entities;
			System.Array.Copy( from.areabits, 0, areabits, 0, areabits.Length );
			playerstate.Set( from.playerstate );
		}

		public virtual void Reset( )
		{
			valid = false;
			serverframe = servertime = deltaframe = 0;
			areabits.Fill( ( Byte ) 0 );
			playerstate.Clear();
			num_entities = parse_entities = 0;
		}
	}
}
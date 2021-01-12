using Q2Sharp.Util;
using System;

namespace Q2Sharp.Game
{
	public class monsterinfo_t
	{
		public mmove_t currentmove;
		public Int32 aiflags;
		public Int32 nextframe;
		public Single scale;
		public EntThinkAdapter stand;
		public EntThinkAdapter idle;
		public EntThinkAdapter search;
		public EntThinkAdapter walk;
		public EntThinkAdapter run;
		public EntDodgeAdapter dodge;
		public EntThinkAdapter attack;
		public EntThinkAdapter melee;
		public EntInteractAdapter sight;
		public EntThinkAdapter checkattack;
		public Single pausetime;
		public Single attack_finished;
		public Single[] saved_goal = new Single[] { 0, 0, 0 };
		public Single search_time;
		public Single trail_time;
		public Single[] last_sighting = new Single[] { 0, 0, 0 };
		public Int32 attack_state;
		public Int32 lefty;
		public Single idle_time;
		public Int32 linkcount;
		public Int32 power_armor_type;
		public Int32 power_armor_power;
		public virtual void Write( QuakeFile f )
		{
			f.Write( currentmove != null );
			if ( currentmove != null )
				currentmove.Write( f );
			f.Write( aiflags );
			f.Write( nextframe );
			f.Write( scale );
			f.WriteAdapter( stand );
			f.WriteAdapter( idle );
			f.WriteAdapter( search );
			f.WriteAdapter( walk );
			f.WriteAdapter( run );
			f.WriteAdapter( dodge );
			f.WriteAdapter( attack );
			f.WriteAdapter( melee );
			f.WriteAdapter( sight );
			f.WriteAdapter( checkattack );
			f.Write( pausetime );
			f.Write( attack_finished );
			f.WriteVector( saved_goal );
			f.Write( search_time );
			f.Write( trail_time );
			f.WriteVector( last_sighting );
			f.Write( attack_state );
			f.Write( lefty );
			f.Write( idle_time );
			f.Write( linkcount );
			f.Write( power_armor_power );
			f.Write( power_armor_type );
		}

		public virtual void Read( QuakeFile f )
		{
			if ( f.ReadBoolean() )
			{
				currentmove = new mmove_t();
				currentmove.Read( f );
			}
			else
				currentmove = null;
			aiflags = f.ReadInt32();
			nextframe = f.ReadInt32();
			scale = f.ReadSingle();
			stand = ( EntThinkAdapter ) f.ReadAdapter();
			idle = ( EntThinkAdapter ) f.ReadAdapter();
			search = ( EntThinkAdapter ) f.ReadAdapter();
			walk = ( EntThinkAdapter ) f.ReadAdapter();
			run = ( EntThinkAdapter ) f.ReadAdapter();
			dodge = ( EntDodgeAdapter ) f.ReadAdapter();
			attack = ( EntThinkAdapter ) f.ReadAdapter();
			melee = ( EntThinkAdapter ) f.ReadAdapter();
			sight = ( EntInteractAdapter ) f.ReadAdapter();
			checkattack = ( EntThinkAdapter ) f.ReadAdapter();
			pausetime = f.ReadSingle();
			attack_finished = f.ReadSingle();
			saved_goal = f.ReadVector();
			search_time = f.ReadSingle();
			trail_time = f.ReadSingle();
			last_sighting = f.ReadVector();
			attack_state = f.ReadInt32();
			lefty = f.ReadInt32();
			idle_time = f.ReadSingle();
			linkcount = f.ReadInt32();
			power_armor_power = f.ReadInt32();
			power_armor_type = f.ReadInt32();
		}
	}
}
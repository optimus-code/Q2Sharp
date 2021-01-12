using Q2Sharp.Util;
using System;

namespace Q2Sharp.Game
{
	public class level_locals_t
	{
		public Int32 framenum;
		public Single time;
		public String level_name = "";
		public String mapname = "";
		public String nextmap = "";
		public Single intermissiontime;
		public String changemap;
		public Boolean exitintermission;
		public Single[] intermission_origin = new Single[] { 0, 0, 0 };
		public Single[] intermission_angle = new Single[] { 0, 0, 0 };
		public edict_t sight_client;
		public edict_t sight_entity;
		public Int32 sight_entity_framenum;
		public edict_t sound_entity;
		public Int32 sound_entity_framenum;
		public edict_t sound2_entity;
		public Int32 sound2_entity_framenum;
		public Int32 pic_health;
		public Int32 total_secrets;
		public Int32 found_secrets;
		public Int32 total_goals;
		public Int32 found_goals;
		public Int32 total_monsters;
		public Int32 killed_monsters;
		public edict_t current_entity;
		public Int32 body_que;
		public Int32 power_cubes;
		public virtual void Write( QuakeFile f )
		{
			f.Write( framenum );
			f.Write( time );
			f.Write( level_name );
			f.Write( mapname );
			f.Write( nextmap );
			f.Write( intermissiontime );
			f.Write( changemap );
			f.Write( exitintermission );
			f.WriteVector( intermission_origin );
			f.WriteVector( intermission_angle );
			f.WriteEdictRef( sight_client );
			f.WriteEdictRef( sight_entity );
			f.Write( sight_entity_framenum );
			f.WriteEdictRef( sound_entity );
			f.Write( sound_entity_framenum );
			f.WriteEdictRef( sound2_entity );
			f.Write( sound2_entity_framenum );
			f.Write( pic_health );
			f.Write( total_secrets );
			f.Write( found_secrets );
			f.Write( total_goals );
			f.Write( found_goals );
			f.Write( total_monsters );
			f.Write( killed_monsters );
			f.WriteEdictRef( current_entity );
			f.Write( body_que );
			f.Write( power_cubes );
			f.Write( 4711 );
		}

		public virtual void Read( QuakeFile f )
		{
			framenum = f.ReadInt32();
			time = f.ReadSingle();
			level_name = f.ReadString();
			mapname = f.ReadString();
			nextmap = f.ReadString();
			intermissiontime = f.ReadSingle();
			changemap = f.ReadString();
			exitintermission = f.ReadBoolean();
			intermission_origin = f.ReadVector();
			intermission_angle = f.ReadVector();
			sight_client = f.ReadEdictRef();
			sight_entity = f.ReadEdictRef();
			sight_entity_framenum = f.ReadInt32();
			sound_entity = f.ReadEdictRef();
			sound_entity_framenum = f.ReadInt32();
			sound2_entity = f.ReadEdictRef();
			sound2_entity_framenum = f.ReadInt32();
			pic_health = f.ReadInt32();
			total_secrets = f.ReadInt32();
			found_secrets = f.ReadInt32();
			total_goals = f.ReadInt32();
			found_goals = f.ReadInt32();
			total_monsters = f.ReadInt32();
			killed_monsters = f.ReadInt32();
			current_entity = f.ReadEdictRef();
			body_que = f.ReadInt32();
			power_cubes = f.ReadInt32();
			if ( f.ReadInt32() != 4711 )
				System.Diagnostics.Debug.WriteLine( "error in reading level_locals." );
		}
	}
}
using Q2Sharp.Util;

namespace Q2Sharp.Game
{
	public class moveinfo_t
	{
		public float[] start_origin = new float[] { 0, 0, 0 };
		public float[] start_angles = new float[] { 0, 0, 0 };
		public float[] end_origin = new float[] { 0, 0, 0 };
		public float[] end_angles = new float[] { 0, 0, 0 };
		public int sound_start;
		public int sound_middle;
		public int sound_end;
		public float accel;
		public float speed;
		public float decel;
		public float distance;
		public float wait;
		public int state;
		public float[] dir = new float[] { 0, 0, 0 };
		public float current_speed;
		public float move_speed;
		public float next_speed;
		public float remaining_distance;
		public float decel_distance;
		public EntThinkAdapter endfunc;
		public virtual void Write( QuakeFile f )
		{
			f.WriteVector( start_origin );
			f.WriteVector( start_angles );
			f.WriteVector( end_origin );
			f.WriteVector( end_angles );
			f.Write( sound_start );
			f.Write( sound_middle );
			f.Write( sound_end );
			f.Write( accel );
			f.Write( speed );
			f.Write( decel );
			f.Write( distance );
			f.Write( wait );
			f.Write( state );
			f.WriteVector( dir );
			f.Write( current_speed );
			f.Write( move_speed );
			f.Write( next_speed );
			f.Write( remaining_distance );
			f.Write( decel_distance );
			f.WriteAdapter( endfunc );
		}

		public virtual void Read( QuakeFile f )
		{
			start_origin = f.ReadVector();
			start_angles = f.ReadVector();
			end_origin = f.ReadVector();
			end_angles = f.ReadVector();
			sound_start = f.ReadInt32();
			sound_middle = f.ReadInt32();
			sound_end = f.ReadInt32();
			accel = f.ReadSingle();
			speed = f.ReadSingle();
			decel = f.ReadSingle();
			distance = f.ReadSingle();
			wait = f.ReadSingle();
			state = f.ReadInt32();
			dir = f.ReadVector();
			current_speed = f.ReadSingle();
			move_speed = f.ReadSingle();
			next_speed = f.ReadSingle();
			remaining_distance = f.ReadSingle();
			decel_distance = f.ReadSingle();
			endfunc = ( EntThinkAdapter ) f.ReadAdapter();
		}
	}
}
using Jake2.Util;
using System;

namespace Jake2.Game
{
	public class entity_state_t : ICloneable
	{
		public entity_state_t( edict_t ent )
		{
			this.surrounding_ent = ent;
			if ( ent != null )
				number = ent.index;
		}

		public Int32 number = 0;
		public edict_t surrounding_ent = null;
		public Single[] origin = new Single[] { 0, 0, 0 };
		public Single[] angles = new Single[] { 0, 0, 0 };
		public Single[] old_origin = new Single[] { 0, 0, 0 };
		public Int32 modelindex;
		public Int32 modelindex2, modelindex3, modelindex4;
		public Int32 frame;
		public Int32 skinnum;
		public Int32 effects;
		public Int32 renderfx;
		public Int32 solid;
		public Int32 sound;
		public Int32 event_renamed;
		public virtual void Write( QuakeFile f )
		{
			f.WriteEdictRef( surrounding_ent );
			f.WriteVector( origin );
			f.WriteVector( angles );
			f.WriteVector( old_origin );
			f.Write( modelindex );
			f.Write( modelindex2 );
			f.Write( modelindex3 );
			f.Write( modelindex4 );
			f.Write( frame );
			f.Write( skinnum );
			f.Write( effects );
			f.Write( renderfx );
			f.Write( solid );
			f.Write( sound );
			f.Write( event_renamed );
		}

		public virtual void Read( QuakeFile f )
		{
			surrounding_ent = f.ReadEdictRef();
			origin = f.ReadVector();
			angles = f.ReadVector();
			old_origin = f.ReadVector();
			modelindex = f.ReadInt32();
			modelindex2 = f.ReadInt32();
			modelindex3 = f.ReadInt32();
			modelindex4 = f.ReadInt32();
			frame = f.ReadInt32();
			skinnum = f.ReadInt32();
			effects = f.ReadInt32();
			renderfx = f.ReadInt32();
			solid = f.ReadInt32();
			sound = f.ReadInt32();
			event_renamed = f.ReadInt32();
		}

		public virtual Object Clone( )
		{
			entity_state_t out_renamed = new entity_state_t( this.surrounding_ent );
			out_renamed.Set( this );
			return out_renamed;
		}

		public virtual entity_state_t GetClone( )
		{
			return ( entity_state_t ) Clone();
		}

		public virtual void Set( entity_state_t from )
		{
			number = from.number;
			Math3D.VectorCopy( from.origin, origin );
			Math3D.VectorCopy( from.angles, angles );
			Math3D.VectorCopy( from.old_origin, old_origin );
			modelindex = from.modelindex;
			modelindex2 = from.modelindex2;
			modelindex3 = from.modelindex3;
			modelindex4 = from.modelindex4;
			frame = from.frame;
			skinnum = from.skinnum;
			effects = from.effects;
			renderfx = from.renderfx;
			solid = from.solid;
			sound = from.sound;
			event_renamed = from.event_renamed;
		}

		public virtual void Clear( )
		{
			number = 0;
			surrounding_ent = null;
			Math3D.VectorClear( origin );
			Math3D.VectorClear( angles );
			Math3D.VectorClear( old_origin );
			modelindex = 0;
			modelindex2 = modelindex3 = modelindex4 = 0;
			frame = 0;
			skinnum = 0;
			effects = 0;
			renderfx = 0;
			solid = 0;
			sound = 0;
			event_renamed = 0;
		}
	}
}
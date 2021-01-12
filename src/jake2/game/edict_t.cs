using Jake2.Qcommon;
using Jake2.Util;
using System;

namespace Jake2.Game
{
	public class edict_t
	{
		public edict_t( Int32 i )
		{
			s.number = i;
			index = i;

			area = new link_t( this );
			s = new entity_state_t( this );
		}

		public virtual void Cleararealinks( )
		{
			area = new link_t( this );
		}

		public entity_state_t s;
		public Boolean inuse;
		public Int32 linkcount;
		public link_t area;
		public Int32 num_clusters;
		public Int32[] clusternums = new Int32[Defines.MAX_ENT_CLUSTERS];
		public Int32 headnode;
		public Int32 areanum, areanum2;
		public Int32 svflags;
		public Single[] mins = new Single[] { 0, 0, 0 };
		public Single[] maxs = new Single[] { 0, 0, 0 };
		public Single[] absmin = new Single[] { 0, 0, 0 };
		public Single[] absmax = new Single[] { 0, 0, 0 };
		public Single[] size = new Single[] { 0, 0, 0 };
		public Int32 solid;
		public Int32 clipmask;
		public Int32 movetype;
		public Int32 flags;
		public String model = null;
		public Single freetime;
		public String message = null;
		public String classname = "";
		public Int32 spawnflags;
		public Single timestamp;
		public Single angle;
		public String target = null;
		public String targetname = null;
		public String killtarget = null;
		public String team = null;
		public String pathtarget = null;
		public String deathtarget = null;
		public String combattarget = null;
		public edict_t target_ent = null;
		public Single speed, accel, decel;
		public Single[] movedir = new Single[] { 0, 0, 0 };
		public Single[] pos1 = new Single[] { 0, 0, 0 };
		public Single[] pos2 = new Single[] { 0, 0, 0 };
		public Single[] velocity = new Single[] { 0, 0, 0 };
		public Single[] avelocity = new Single[] { 0, 0, 0 };
		public Int32 mass;
		public Single air_finished;
		public Single gravity;
		public edict_t goalentity = null;
		public edict_t movetarget = null;
		public Single yaw_speed;
		public Single ideal_yaw;
		public Single nextthink;
		public EntThinkAdapter prethink = null;
		public EntThinkAdapter think = null;
		public EntBlockedAdapter blocked = null;
		public EntTouchAdapter touch = null;
		public EntUseAdapter use = null;
		public EntPainAdapter pain = null;
		public EntDieAdapter die = null;
		public Single touch_debounce_time;
		public Single pain_debounce_time;
		public Single damage_debounce_time;
		public Single fly_sound_debounce_time;
		public Single last_move_time;
		public Int32 health;
		public Int32 max_health;
		public Int32 gib_health;
		public Int32 deadflag;
		public Int32 show_hostile;
		public Single powerarmor_time;
		public String map = null;
		public Int32 viewheight;
		public Int32 takedamage;
		public Int32 dmg;
		public Int32 radius_dmg;
		public Single dmg_radius;
		public Int32 sounds;
		public Int32 count;
		public edict_t chain = null;
		public edict_t enemy = null;
		public edict_t oldenemy = null;
		public edict_t activator = null;
		public edict_t groundentity = null;
		public Int32 groundentity_linkcount;
		public edict_t teamchain = null;
		public edict_t teammaster = null;
		public edict_t mynoise = null;
		public edict_t mynoise2 = null;
		public Int32 noise_index;
		public Int32 noise_index2;
		public Single volume;
		public Single attenuation;
		public Single wait;
		public Single delay;
		public Single random;
		public Single teleport_time;
		public Int32 watertype;
		public Int32 waterlevel;
		public Single[] move_origin = new Single[] { 0, 0, 0 };
		public Single[] move_angles = new Single[] { 0, 0, 0 };
		public Int32 light_level;
		public Int32 style;
		public gitem_t item;
		public moveinfo_t moveinfo = new moveinfo_t();
		public monsterinfo_t monsterinfo = new monsterinfo_t();
		public gclient_t client;
		public edict_t owner;
		public Int32 index;
		public virtual Boolean SetField( String key, String value )
		{
			if ( key.Equals( "classname" ) )
			{
				classname = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "model" ) )
			{
				model = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "spawnflags" ) )
			{
				spawnflags = Lib.Atoi( value );
				return true;
			}

			if ( key.Equals( "speed" ) )
			{
				speed = Lib.Atof( value );
				return true;
			}

			if ( key.Equals( "accel" ) )
			{
				accel = Lib.Atof( value );
				return true;
			}

			if ( key.Equals( "decel" ) )
			{
				decel = Lib.Atof( value );
				return true;
			}

			if ( key.Equals( "target" ) )
			{
				target = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "targetname" ) )
			{
				targetname = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "pathtarget" ) )
			{
				pathtarget = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "deathtarget" ) )
			{
				deathtarget = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "killtarget" ) )
			{
				killtarget = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "combattarget" ) )
			{
				combattarget = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "message" ) )
			{
				message = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "team" ) )
			{
				team = GameSpawn.ED_NewString( value );
				Com.Dprintln( "Monster Team:" + team );
				return true;
			}

			if ( key.Equals( "wait" ) )
			{
				wait = Lib.Atof( value );
				return true;
			}

			if ( key.Equals( "delay" ) )
			{
				delay = Lib.Atof( value );
				return true;
			}

			if ( key.Equals( "random" ) )
			{
				random = Lib.Atof( value );
				return true;
			}

			if ( key.Equals( "move_origin" ) )
			{
				move_origin = Lib.Atov( value );
				return true;
			}

			if ( key.Equals( "move_angles" ) )
			{
				move_angles = Lib.Atov( value );
				return true;
			}

			if ( key.Equals( "style" ) )
			{
				style = Lib.Atoi( value );
				return true;
			}

			if ( key.Equals( "count" ) )
			{
				count = Lib.Atoi( value );
				return true;
			}

			if ( key.Equals( "health" ) )
			{
				health = Lib.Atoi( value );
				return true;
			}

			if ( key.Equals( "sounds" ) )
			{
				sounds = Lib.Atoi( value );
				return true;
			}

			if ( key.Equals( "light" ) )
			{
				return true;
			}

			if ( key.Equals( "dmg" ) )
			{
				dmg = Lib.Atoi( value );
				return true;
			}

			if ( key.Equals( "mass" ) )
			{
				mass = Lib.Atoi( value );
				return true;
			}

			if ( key.Equals( "volume" ) )
			{
				volume = Lib.Atof( value );
				return true;
			}

			if ( key.Equals( "attenuation" ) )
			{
				attenuation = Lib.Atof( value );
				return true;
			}

			if ( key.Equals( "map" ) )
			{
				map = GameSpawn.ED_NewString( value );
				return true;
			}

			if ( key.Equals( "origin" ) )
			{
				s.origin = Lib.Atov( value );
				return true;
			}

			if ( key.Equals( "angles" ) )
			{
				s.angles = Lib.Atov( value );
				return true;
			}

			if ( key.Equals( "angle" ) )
			{
				s.angles = new Single[] { 0, Lib.Atof( value ), 0 };
				return true;
			}

			if ( key.Equals( "item" ) )
			{
				GameBase.gi.Error( "ent.set(\\\"item\\\") called." );
				return true;
			}

			return false;
		}

		public virtual void Write( QuakeFile f )
		{
			s.Write( f );
			f.Write( inuse );
			f.Write( linkcount );
			f.Write( num_clusters );
			f.Write( 9999 );
			if ( clusternums == null )
				f.Write( -1 );
			else
			{
				f.Write( Defines.MAX_ENT_CLUSTERS );
				for ( var n = 0; n < Defines.MAX_ENT_CLUSTERS; n++ )
					f.Write( clusternums[n] );
			}

			f.Write( headnode );
			f.Write( areanum );
			f.Write( areanum2 );
			f.Write( svflags );
			f.WriteVector( mins );
			f.WriteVector( maxs );
			f.WriteVector( absmin );
			f.WriteVector( absmax );
			f.WriteVector( size );
			f.Write( solid );
			f.Write( clipmask );
			f.Write( movetype );
			f.Write( flags );
			f.Write( model );
			f.Write( freetime );
			f.Write( message );
			f.Write( classname );
			f.Write( spawnflags );
			f.Write( timestamp );
			f.Write( angle );
			f.Write( target );
			f.Write( targetname );
			f.Write( killtarget );
			f.Write( team );
			f.Write( pathtarget );
			f.Write( deathtarget );
			f.Write( combattarget );
			f.WriteEdictRef( target_ent );
			f.Write( speed );
			f.Write( accel );
			f.Write( decel );
			f.WriteVector( movedir );
			f.WriteVector( pos1 );
			f.WriteVector( pos2 );
			f.WriteVector( velocity );
			f.WriteVector( avelocity );
			f.Write( mass );
			f.Write( air_finished );
			f.Write( gravity );
			f.WriteEdictRef( goalentity );
			f.WriteEdictRef( movetarget );
			f.Write( yaw_speed );
			f.Write( ideal_yaw );
			f.Write( nextthink );
			f.WriteAdapter( prethink );
			f.WriteAdapter( think );
			f.WriteAdapter( blocked );
			f.WriteAdapter( touch );
			f.WriteAdapter( use );
			f.WriteAdapter( pain );
			f.WriteAdapter( die );
			f.Write( touch_debounce_time );
			f.Write( pain_debounce_time );
			f.Write( damage_debounce_time );
			f.Write( fly_sound_debounce_time );
			f.Write( last_move_time );
			f.Write( health );
			f.Write( max_health );
			f.Write( gib_health );
			f.Write( deadflag );
			f.Write( show_hostile );
			f.Write( powerarmor_time );
			f.Write( map );
			f.Write( viewheight );
			f.Write( takedamage );
			f.Write( dmg );
			f.Write( radius_dmg );
			f.Write( dmg_radius );
			f.Write( sounds );
			f.Write( count );
			f.WriteEdictRef( chain );
			f.WriteEdictRef( enemy );
			f.WriteEdictRef( oldenemy );
			f.WriteEdictRef( activator );
			f.WriteEdictRef( groundentity );
			f.Write( groundentity_linkcount );
			f.WriteEdictRef( teamchain );
			f.WriteEdictRef( teammaster );
			f.WriteEdictRef( mynoise );
			f.WriteEdictRef( mynoise2 );
			f.Write( noise_index );
			f.Write( noise_index2 );
			f.Write( volume );
			f.Write( attenuation );
			f.Write( wait );
			f.Write( delay );
			f.Write( random );
			f.Write( teleport_time );
			f.Write( watertype );
			f.Write( waterlevel );
			f.WriteVector( move_origin );
			f.WriteVector( move_angles );
			f.Write( light_level );
			f.Write( style );
			f.WriteItem( item );
			moveinfo.Write( f );
			monsterinfo.Write( f );
			if ( client == null )
				f.Write( -1 );
			else
				f.Write( client.index );
			f.WriteEdictRef( owner );
			f.Write( 9876 );
		}

		public virtual void Read( QuakeFile f )
		{
			s.Read( f );
			inuse = f.ReadBoolean();
			linkcount = f.ReadInt32();
			num_clusters = f.ReadInt32();
			if ( f.ReadInt32() != 9999 )
				new Exception( "wrong read pos!" ).PrintStackTrace();
			var len = f.ReadInt32();
			if ( len == -1 )
				clusternums = null;
			else
			{
				clusternums = new Int32[Defines.MAX_ENT_CLUSTERS];
				for ( var n = 0; n < Defines.MAX_ENT_CLUSTERS; n++ )
					clusternums[n] = f.ReadInt32();
			}

			headnode = f.ReadInt32();
			areanum = f.ReadInt32();
			areanum2 = f.ReadInt32();
			svflags = f.ReadInt32();
			mins = f.ReadVector();
			maxs = f.ReadVector();
			absmin = f.ReadVector();
			absmax = f.ReadVector();
			size = f.ReadVector();
			solid = f.ReadInt32();
			clipmask = f.ReadInt32();
			movetype = f.ReadInt32();
			flags = f.ReadInt32();
			model = f.ReadString();
			freetime = f.ReadSingle();
			message = f.ReadString();
			classname = f.ReadString();
			spawnflags = f.ReadInt32();
			timestamp = f.ReadSingle();
			angle = f.ReadSingle();
			target = f.ReadString();
			targetname = f.ReadString();
			killtarget = f.ReadString();
			team = f.ReadString();
			pathtarget = f.ReadString();
			deathtarget = f.ReadString();
			combattarget = f.ReadString();
			target_ent = f.ReadEdictRef();
			speed = f.ReadSingle();
			accel = f.ReadSingle();
			decel = f.ReadSingle();
			movedir = f.ReadVector();
			pos1 = f.ReadVector();
			pos2 = f.ReadVector();
			velocity = f.ReadVector();
			avelocity = f.ReadVector();
			mass = f.ReadInt32();
			air_finished = f.ReadSingle();
			gravity = f.ReadSingle();
			goalentity = f.ReadEdictRef();
			movetarget = f.ReadEdictRef();
			yaw_speed = f.ReadSingle();
			ideal_yaw = f.ReadSingle();
			nextthink = f.ReadSingle();
			prethink = ( EntThinkAdapter ) f.ReadAdapter();
			think = ( EntThinkAdapter ) f.ReadAdapter();
			blocked = ( EntBlockedAdapter ) f.ReadAdapter();
			touch = ( EntTouchAdapter ) f.ReadAdapter();
			use = ( EntUseAdapter ) f.ReadAdapter();
			pain = ( EntPainAdapter ) f.ReadAdapter();
			die = ( EntDieAdapter ) f.ReadAdapter();
			touch_debounce_time = f.ReadSingle();
			pain_debounce_time = f.ReadSingle();
			damage_debounce_time = f.ReadSingle();
			fly_sound_debounce_time = f.ReadSingle();
			last_move_time = f.ReadSingle();
			health = f.ReadInt32();
			max_health = f.ReadInt32();
			gib_health = f.ReadInt32();
			deadflag = f.ReadInt32();
			show_hostile = f.ReadInt32();
			powerarmor_time = f.ReadSingle();
			map = f.ReadString();
			viewheight = f.ReadInt32();
			takedamage = f.ReadInt32();
			dmg = f.ReadInt32();
			radius_dmg = f.ReadInt32();
			dmg_radius = f.ReadSingle();
			sounds = f.ReadInt32();
			count = f.ReadInt32();
			chain = f.ReadEdictRef();
			enemy = f.ReadEdictRef();
			oldenemy = f.ReadEdictRef();
			activator = f.ReadEdictRef();
			groundentity = f.ReadEdictRef();
			groundentity_linkcount = f.ReadInt32();
			teamchain = f.ReadEdictRef();
			teammaster = f.ReadEdictRef();
			mynoise = f.ReadEdictRef();
			mynoise2 = f.ReadEdictRef();
			noise_index = f.ReadInt32();
			noise_index2 = f.ReadInt32();
			volume = f.ReadSingle();
			attenuation = f.ReadSingle();
			wait = f.ReadSingle();
			delay = f.ReadSingle();
			random = f.ReadSingle();
			teleport_time = f.ReadSingle();
			watertype = f.ReadInt32();
			waterlevel = f.ReadInt32();
			move_origin = f.ReadVector();
			move_angles = f.ReadVector();
			light_level = f.ReadInt32();
			style = f.ReadInt32();
			item = f.ReadItem();
			moveinfo.Read( f );
			monsterinfo.Read( f );
			var ndx = f.ReadInt32();
			if ( ndx == -1 )
				client = null;
			else
				client = GameBase.game.clients[ndx];
			owner = f.ReadEdictRef();
			if ( f.ReadInt32() != 9876 )
				System.Diagnostics.Debug.WriteLine( "ent load check failed for num " + index );
		}
	}
}
using Q2Sharp.Util;
using System;

namespace Q2Sharp.Game
{
	public class GameTarget
	{
		public static void SP_target_temp_entity( edict_t ent )
		{
			ent.use = Use_Target_Tent;
		}

		public static void SP_target_speaker( edict_t ent )
		{
			String buffer;
			if ( GameBase.st.noise == null )
			{
				GameBase.gi.Dprintf( "target_speaker with no noise set at " + Lib.Vtos( ent.s.origin ) + "\\n" );
				return;
			}

			if ( GameBase.st.noise.IndexOf( ".wav" ) < 0 )
				buffer = "" + GameBase.st.noise + ".wav";
			else
				buffer = GameBase.st.noise;
			ent.noise_index = GameBase.gi.Soundindex( buffer );
			if ( ent.volume == 0 )
				ent.volume = 1F;
			if ( ent.attenuation == 0 )
				ent.attenuation = 1F;
			else if ( ent.attenuation == -1 )
				ent.attenuation = 0;
			if ( ( ent.spawnflags & 1 ) != 0 )
				ent.s.sound = ent.noise_index;
			ent.use = Use_Target_Speaker;
			GameBase.gi.Linkentity( ent );
		}

		public static void SP_target_help( edict_t ent )
		{
			if ( GameBase.deathmatch.value != 0 )
			{
				GameUtil.G_FreeEdict( ent );
				return;
			}

			if ( ent.message == null )
			{
				GameBase.gi.Dprintf( ent.classname + " with no message at " + Lib.Vtos( ent.s.origin ) + "\\n" );
				GameUtil.G_FreeEdict( ent );
				return;
			}

			ent.use = Use_Target_Help;
		}

		public static void SP_target_secret( edict_t ent )
		{
			if ( GameBase.deathmatch.value != 0 )
			{
				GameUtil.G_FreeEdict( ent );
				return;
			}

			ent.use = use_target_secret;
			if ( GameBase.st.noise == null )
				GameBase.st.noise = "misc/secret.wav";
			ent.noise_index = GameBase.gi.Soundindex( GameBase.st.noise );
			ent.svflags = Defines.SVF_NOCLIENT;
			GameBase.level.total_secrets++;
			if ( 0 == Lib.Q_stricmp( GameBase.level.mapname, "mine3" ) && ent.s.origin[0] == 280 && ent.s.origin[1] == -2048 && ent.s.origin[2] == -624 )
				ent.message = "You have found a secret area.";
		}

		public static void SP_target_goal( edict_t ent )
		{
			if ( GameBase.deathmatch.value != 0 )
			{
				GameUtil.G_FreeEdict( ent );
				return;
			}

			ent.use = use_target_goal;
			if ( GameBase.st.noise == null )
				GameBase.st.noise = "misc/secret.wav";
			ent.noise_index = GameBase.gi.Soundindex( GameBase.st.noise );
			ent.svflags = Defines.SVF_NOCLIENT;
			GameBase.level.total_goals++;
		}

		public static void SP_target_explosion( edict_t ent )
		{
			ent.use = use_target_explosion;
			ent.svflags = Defines.SVF_NOCLIENT;
		}

		public static void SP_target_changelevel( edict_t ent )
		{
			if ( ent.map == null )
			{
				GameBase.gi.Dprintf( "target_changelevel with no map at " + Lib.Vtos( ent.s.origin ) + "\\n" );
				GameUtil.G_FreeEdict( ent );
				return;
			}

			if ( ( Lib.Q_stricmp( GameBase.level.mapname, "fact1" ) == 0 ) && ( Lib.Q_stricmp( ent.map, "fact3" ) == 0 ) )
				ent.map = "fact3$secret1";
			ent.use = use_target_changelevel;
			ent.svflags = Defines.SVF_NOCLIENT;
		}

		public static void SP_target_splash( edict_t self )
		{
			self.use = use_target_splash;
			GameBase.G_SetMovedir( self.s.angles, self.movedir );
			if ( 0 == self.count )
				self.count = 32;
			self.svflags = Defines.SVF_NOCLIENT;
		}

		public static void SP_target_spawner( edict_t self )
		{
			self.use = use_target_spawner;
			self.svflags = Defines.SVF_NOCLIENT;
			if ( self.speed != 0 )
			{
				GameBase.G_SetMovedir( self.s.angles, self.movedir );
				Math3D.VectorScale( self.movedir, self.speed, self.movedir );
			}
		}

		public static void SP_target_blaster( edict_t self )
		{
			self.use = use_target_blaster;
			GameBase.G_SetMovedir( self.s.angles, self.movedir );
			self.noise_index = GameBase.gi.Soundindex( "weapons/laser2.wav" );
			if ( 0 == self.dmg )
				self.dmg = 15;
			if ( 0 == self.speed )
				self.speed = 1000;
			self.svflags = Defines.SVF_NOCLIENT;
		}

		public static void SP_target_crosslevel_trigger( edict_t self )
		{
			self.svflags = Defines.SVF_NOCLIENT;
			self.use = trigger_crosslevel_trigger_use;
		}

		public static void SP_target_crosslevel_target( edict_t self )
		{
			if ( 0 == self.delay )
				self.delay = 1;
			self.svflags = Defines.SVF_NOCLIENT;
			self.think = target_crosslevel_target_think;
			self.nextthink = GameBase.level.time + self.delay;
		}

		public static void Target_laser_on( edict_t self )
		{
			if ( null == self.activator )
				self.activator = self;
			self.spawnflags = ( Int32 ) ( self.spawnflags | 0x80000001 );
			self.svflags &= ~Defines.SVF_NOCLIENT;
			target_laser_think.Think( self );
		}

		public static void Target_laser_off( edict_t self )
		{
			self.spawnflags &= ~1;
			self.svflags |= Defines.SVF_NOCLIENT;
			self.nextthink = 0;
		}

		public static void SP_target_laser( edict_t self )
		{
			self.think = target_laser_start;
			self.nextthink = GameBase.level.time + 1;
		}

		public static void SP_target_lightramp( edict_t self )
		{
			if ( self.message == null || self.message.Length != 2 || self.message[0] < 'a' || self.message[0] > 'z' || self.message[1] < 'a' || self.message[1] > 'z' || self.message[0] == self.message[1] )
			{
				GameBase.gi.Dprintf( "target_lightramp has bad ramp (" + self.message + ") at " + Lib.Vtos( self.s.origin ) + "\\n" );
				GameUtil.G_FreeEdict( self );
				return;
			}

			if ( GameBase.deathmatch.value != 0 )
			{
				GameUtil.G_FreeEdict( self );
				return;
			}

			if ( self.target == null )
			{
				GameBase.gi.Dprintf( self.classname + " with no target at " + Lib.Vtos( self.s.origin ) + "\\n" );
				GameUtil.G_FreeEdict( self );
				return;
			}

			self.svflags |= Defines.SVF_NOCLIENT;
			self.use = target_lightramp_use;
			self.think = target_lightramp_think;
			self.movedir[0] = self.message[0] - 'a';
			self.movedir[1] = self.message[1] - 'a';
			self.movedir[2] = ( self.movedir[1] - self.movedir[0] ) / ( self.speed / Defines.FRAMETIME );
		}

		public static void SP_target_earthquake( edict_t self )
		{
			if ( null == self.targetname )
				GameBase.gi.Dprintf( "untargeted " + self.classname + " at " + Lib.Vtos( self.s.origin ) + "\\n" );
			if ( 0 == self.count )
				self.count = 5;
			if ( 0 == self.speed )
				self.speed = 200;
			self.svflags |= Defines.SVF_NOCLIENT;
			self.think = target_earthquake_think;
			self.use = target_earthquake_use;
			self.noise_index = GameBase.gi.Soundindex( "world/quake.wav" );
		}

		public static EntUseAdapter Use_Target_Tent = new AnonymousEntUseAdapter();
		private sealed class AnonymousEntUseAdapter : EntUseAdapter
		{

			public override String GetID( )
			{
				return "Use_Target_Tent";
			}

			public override void Use( edict_t ent, edict_t other, edict_t activator )
			{
				GameBase.gi.WriteByte( Defines.svc_temp_entity );
				GameBase.gi.WriteByte( ent.style );
				GameBase.gi.WritePosition( ent.s.origin );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
			}
		}

		public static EntUseAdapter Use_Target_Speaker = new AnonymousEntUseAdapter1();
		private sealed class AnonymousEntUseAdapter1 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "Use_Target_Speaker";
			}

			public override void Use( edict_t ent, edict_t other, edict_t activator )
			{
				Int32 chan;
				if ( ( ent.spawnflags & 3 ) != 0 )
				{
					if ( ent.s.sound != 0 )
						ent.s.sound = 0;
					else
						ent.s.sound = ent.noise_index;
				}
				else
				{
					if ( ( ent.spawnflags & 4 ) != 0 )
						chan = Defines.CHAN_VOICE | Defines.CHAN_RELIABLE;
					else
						chan = Defines.CHAN_VOICE;
					GameBase.gi.Positioned_sound( ent.s.origin, ent, chan, ent.noise_index, ent.volume, ent.attenuation, 0 );
				}
			}
		}

		public static EntUseAdapter Use_Target_Help = new AnonymousEntUseAdapter2();
		private sealed class AnonymousEntUseAdapter2 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "Use_Target_Help";
			}

			public override void Use( edict_t ent, edict_t other, edict_t activator )
			{
				if ( ( ent.spawnflags & 1 ) != 0 )
					GameBase.game.helpmessage1 = ent.message;
				else
					GameBase.game.helpmessage2 = ent.message;
				GameBase.game.helpchanged++;
			}
		}

		static EntUseAdapter use_target_secret = new AnonymousEntUseAdapter3();
		private sealed class AnonymousEntUseAdapter3 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "use_target_secret";
			}

			public override void Use( edict_t ent, edict_t other, edict_t activator )
			{
				GameBase.gi.Sound( ent, Defines.CHAN_VOICE, ent.noise_index, 1, Defines.ATTN_NORM, 0 );
				GameBase.level.found_secrets++;
				GameUtil.G_UseTargets( ent, activator );
				GameUtil.G_FreeEdict( ent );
			}
		}

		static EntUseAdapter use_target_goal = new AnonymousEntUseAdapter4();
		private sealed class AnonymousEntUseAdapter4 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "use_target_goal";
			}

			public override void Use( edict_t ent, edict_t other, edict_t activator )
			{
				GameBase.gi.Sound( ent, Defines.CHAN_VOICE, ent.noise_index, 1, Defines.ATTN_NORM, 0 );
				GameBase.level.found_goals++;
				if ( GameBase.level.found_goals == GameBase.level.total_goals )
					GameBase.gi.Configstring( Defines.CS_CDTRACK, "0" );
				GameUtil.G_UseTargets( ent, activator );
				GameUtil.G_FreeEdict( ent );
			}
		}

		static EntThinkAdapter target_explosion_explode = new AnonymousEntThinkAdapter();
		private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "target_explosion_explode";
			}

			public override Boolean Think( edict_t self )
			{
				Single save;
				GameBase.gi.WriteByte( Defines.svc_temp_entity );
				GameBase.gi.WriteByte( Defines.TE_EXPLOSION1 );
				GameBase.gi.WritePosition( self.s.origin );
				GameBase.gi.Multicast( self.s.origin, Defines.MULTICAST_PHS );
				GameCombat.T_RadiusDamage( self, self.activator, self.dmg, null, self.dmg + 40, Defines.MOD_EXPLOSIVE );
				save = self.delay;
				self.delay = 0;
				GameUtil.G_UseTargets( self, self.activator );
				self.delay = save;
				return true;
			}
		}

		static EntUseAdapter use_target_explosion = new AnonymousEntUseAdapter5();
		private sealed class AnonymousEntUseAdapter5 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "use_target_explosion";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				self.activator = activator;
				if ( 0 == self.delay )
				{
					target_explosion_explode.Think( self );
					return;
				}

				self.think = target_explosion_explode;
				self.nextthink = GameBase.level.time + self.delay;
			}
		}

		static EntUseAdapter use_target_changelevel = new AnonymousEntUseAdapter6();
		private sealed class AnonymousEntUseAdapter6 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "use_target_changelevel";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				if ( GameBase.level.intermissiontime != 0 )
					return;
				if ( 0 == GameBase.deathmatch.value && 0 == GameBase.coop.value )
				{
					if ( GameBase.g_edicts[1].health <= 0 )
						return;
				}

				if ( GameBase.deathmatch.value != 0 && 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_ALLOW_EXIT ) && other != GameBase.g_edicts[0] )
				{
					GameCombat.T_Damage( other, self, self, Globals.vec3_origin, other.s.origin, Globals.vec3_origin, 10 * other.max_health, 1000, 0, Defines.MOD_EXIT );
					return;
				}

				if ( GameBase.deathmatch.value != 0 )
				{
					if ( activator != null && activator.client != null )
						GameBase.gi.Bprintf( Defines.PRINT_HIGH, activator.client.pers.netname + " exited the level.\\n" );
				}

				if ( self.map.IndexOf( '*' ) > -1 )
					GameBase.game.serverflags &= ~( Defines.SFL_CROSS_TRIGGER_MASK );
				PlayerHud.BeginIntermission( self );
			}
		}

		static EntUseAdapter use_target_splash = new AnonymousEntUseAdapter7();
		private sealed class AnonymousEntUseAdapter7 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "use_target_splash";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				GameBase.gi.WriteByte( Defines.svc_temp_entity );
				GameBase.gi.WriteByte( Defines.TE_SPLASH );
				GameBase.gi.WriteByte( self.count );
				GameBase.gi.WritePosition( self.s.origin );
				GameBase.gi.WriteDir( self.movedir );
				GameBase.gi.WriteByte( self.sounds );
				GameBase.gi.Multicast( self.s.origin, Defines.MULTICAST_PVS );
				if ( self.dmg != 0 )
					GameCombat.T_RadiusDamage( self, activator, self.dmg, null, self.dmg + 40, Defines.MOD_SPLASH );
			}
		}

		static EntUseAdapter use_target_spawner = new AnonymousEntUseAdapter8();
		private sealed class AnonymousEntUseAdapter8 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "use_target_spawner";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				edict_t ent;
				ent = GameUtil.G_Spawn();
				ent.classname = self.target;
				Math3D.VectorCopy( self.s.origin, ent.s.origin );
				Math3D.VectorCopy( self.s.angles, ent.s.angles );
				GameSpawn.ED_CallSpawn( ent );
				GameBase.gi.Unlinkentity( ent );
				GameUtil.KillBox( ent );
				GameBase.gi.Linkentity( ent );
				if ( self.speed != 0 )
					Math3D.VectorCopy( self.movedir, ent.velocity );
			}
		}

		public static EntUseAdapter use_target_blaster = new AnonymousEntUseAdapter9();
		private sealed class AnonymousEntUseAdapter9 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "use_target_blaster";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				Int32 effect;
				if ( ( self.spawnflags & 2 ) != 0 )
					effect = 0;
				else if ( ( self.spawnflags & 1 ) != 0 )
					effect = Defines.EF_HYPERBLASTER;
				else
					effect = Defines.EF_BLASTER;
				GameWeapon.Fire_blaster( self, self.s.origin, self.movedir, self.dmg, ( Int32 ) self.speed, Defines.EF_BLASTER, Defines.MOD_TARGET_BLASTER != 0 );
				GameBase.gi.Sound( self, Defines.CHAN_VOICE, self.noise_index, 1, Defines.ATTN_NORM, 0 );
			}
		}

		public static EntUseAdapter trigger_crosslevel_trigger_use = new AnonymousEntUseAdapter10();
		private sealed class AnonymousEntUseAdapter10 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "trigger_crosslevel_trigger_use";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				GameBase.game.serverflags |= self.spawnflags;
				GameUtil.G_FreeEdict( self );
			}
		}

		static EntThinkAdapter target_crosslevel_target_think = new AnonymousEntThinkAdapter1();
		private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "target_crosslevel_target_think";
			}

			public override Boolean Think( edict_t self )
			{
				if ( self.spawnflags == ( GameBase.game.serverflags & Defines.SFL_CROSS_TRIGGER_MASK & self.spawnflags ) )
				{
					GameUtil.G_UseTargets( self, self );
					GameUtil.G_FreeEdict( self );
				}

				return true;
			}
		}

		public static EntThinkAdapter target_laser_think = new AnonymousEntThinkAdapter2();
		private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "target_laser_think";
			}

			public override Boolean Think( edict_t self )
			{
				edict_t ignore;
				Single[] start = new Single[] { 0, 0, 0 };
				Single[] end = new Single[] { 0, 0, 0 };
				trace_t tr;
				Single[] point = new Single[] { 0, 0, 0 };
				Single[] last_movedir = new Single[] { 0, 0, 0 };
				Int32 count;
				if ( ( self.spawnflags & 0x80000000 ) != 0 )
					count = 8;
				else
					count = 4;
				if ( self.enemy != null )
				{
					Math3D.VectorCopy( self.movedir, last_movedir );
					Math3D.VectorMA( self.enemy.absmin, 0.5F, self.enemy.size, point );
					Math3D.VectorSubtract( point, self.s.origin, self.movedir );
					Math3D.VectorNormalize( self.movedir );
					if ( !Math3D.VectorEquals( self.movedir, last_movedir ) )
						self.spawnflags = ( Int32 ) ( self.spawnflags | 0x80000000 );
				}

				ignore = self;
				Math3D.VectorCopy( self.s.origin, start );
				Math3D.VectorMA( start, 2048, self.movedir, end );
				while ( true )
				{
					tr = GameBase.gi.Trace( start, null, null, end, ignore, Defines.CONTENTS_SOLID | Defines.CONTENTS_MONSTER | Defines.CONTENTS_DEADMONSTER );
					if ( tr.ent == null )
						break;
					if ( ( tr.ent.takedamage != 0 ) && 0 == ( tr.ent.flags & Defines.FL_IMMUNE_LASER ) )
						GameCombat.T_Damage( tr.ent, self, self.activator, self.movedir, tr.endpos, Globals.vec3_origin, self.dmg, 1, Defines.DAMAGE_ENERGY, Defines.MOD_TARGET_LASER );
					if ( 0 == ( tr.ent.svflags & Defines.SVF_MONSTER ) && ( null == tr.ent.client ) )
					{
						if ( ( self.spawnflags & 0x80000000 ) != 0 )
						{
							self.spawnflags = ( Int32 ) ( self.spawnflags & ~0x80000000 );
							GameBase.gi.WriteByte( Defines.svc_temp_entity );
							GameBase.gi.WriteByte( Defines.TE_LASER_SPARKS );
							GameBase.gi.WriteByte( count );
							GameBase.gi.WritePosition( tr.endpos );
							GameBase.gi.WriteDir( tr.plane.normal );
							GameBase.gi.WriteByte( ( Int32 ) self.s.skinnum ); // This cast might break something
							GameBase.gi.Multicast( tr.endpos, Defines.MULTICAST_PVS );
						}

						break;
					}

					ignore = tr.ent;
					Math3D.VectorCopy( tr.endpos, start );
				}

				Math3D.VectorCopy( tr.endpos, self.s.old_origin );
				self.nextthink = GameBase.level.time + Defines.FRAMETIME;
				return true;
			}
		}

		public static EntUseAdapter target_laser_use = new AnonymousEntUseAdapter11();
		private sealed class AnonymousEntUseAdapter11 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "target_laser_use";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				self.activator = activator;
				if ( ( self.spawnflags & 1 ) != 0 )
					Target_laser_off( self );
				else
					Target_laser_on( self );
			}
		}

		static EntThinkAdapter target_laser_start = new AnonymousEntThinkAdapter3();
		private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "target_laser_start";
			}

			public override Boolean Think( edict_t self )
			{
				self.movetype = Defines.MOVETYPE_NONE;
				self.solid = Defines.SOLID_NOT;
				self.s.renderfx |= Defines.RF_BEAM | Defines.RF_TRANSLUCENT;
				self.s.modelindex = 1;
				if ( ( self.spawnflags & 64 ) != 0 )
					self.s.frame = 16;
				else
					self.s.frame = 4;
				if ( ( self.spawnflags & 2 ) != 0 )
					self.s.skinnum = unchecked(( Int32 ) 0xf2f2f0f0);
				else if ( ( self.spawnflags & 4 ) != 0 )
					self.s.skinnum = unchecked(( Int32 ) 0xd0d1d2d3);
				else if ( ( self.spawnflags & 8 ) != 0 )
					self.s.skinnum = unchecked(( Int32 ) 0xf3f3f1f1);
				else if ( ( self.spawnflags & 16 ) != 0 )
					self.s.skinnum = unchecked(( Int32 ) 0xdcdddedf);
				else if ( ( self.spawnflags & 32 ) != 0 )
					self.s.skinnum = unchecked(( Int32 ) 0xe0e1e2e3);
				if ( null == self.enemy )
				{
					if ( self.target != null )
					{
						EdictIterator edit = GameBase.G_Find( null, GameBase.findByTarget, self.target );
						if ( edit == null )
							GameBase.gi.Dprintf( self.classname + " at " + Lib.Vtos( self.s.origin ) + ": " + self.target + " is a bad target\\n" );
						self.enemy = edit.o;
					}
					else
					{
						GameBase.G_SetMovedir( self.s.angles, self.movedir );
					}
				}

				self.use = target_laser_use;
				self.think = target_laser_think;
				if ( 0 == self.dmg )
					self.dmg = 1;
				Math3D.VectorSet( self.mins, -8, -8, -8 );
				Math3D.VectorSet( self.maxs, 8, 8, 8 );
				GameBase.gi.Linkentity( self );
				if ( ( self.spawnflags & 1 ) != 0 )
					Target_laser_on( self );
				else
					Target_laser_off( self );
				return true;
			}
		}

		static EntThinkAdapter target_lightramp_think = new AnonymousEntThinkAdapter4();
		private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "target_lightramp_think";
			}

			public override Boolean Think( edict_t self )
			{
				Char[] tmp = new[] { ( Char ) ( 'a' + ( Int32 ) ( self.movedir[0] + ( GameBase.level.time - self.timestamp ) / Defines.FRAMETIME * self.movedir[2] ) ) };
				GameBase.gi.Configstring( Defines.CS_LIGHTS + self.enemy.style, new String( tmp ) );
				if ( ( GameBase.level.time - self.timestamp ) < self.speed )
				{
					self.nextthink = GameBase.level.time + Defines.FRAMETIME;
				}
				else if ( ( self.spawnflags & 1 ) != 0 )
				{
					Char temp;
					temp = ( Char ) self.movedir[0];
					self.movedir[0] = self.movedir[1];
					self.movedir[1] = temp;
					self.movedir[2] *= -1;
				}

				return true;
			}
		}

		static EntUseAdapter target_lightramp_use = new AnonymousEntUseAdapter12();
		private sealed class AnonymousEntUseAdapter12 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "target_lightramp_use";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				if ( self.enemy == null )
				{
					edict_t e;
					e = null;
					EdictIterator es = null;
					while ( true )
					{
						es = GameBase.G_Find( es, GameBase.findByTarget, self.target );
						if ( es == null )
							break;
						e = es.o;
						if ( Lib.Strcmp( e.classname, "light" ) != 0 )
						{
							GameBase.gi.Dprintf( self.classname + " at " + Lib.Vtos( self.s.origin ) );
							GameBase.gi.Dprintf( "target " + self.target + " (" + e.classname + " at " + Lib.Vtos( e.s.origin ) + ") is not a light\\n" );
						}
						else
						{
							self.enemy = e;
						}
					}

					if ( null == self.enemy )
					{
						GameBase.gi.Dprintf( self.classname + " target " + self.target + " not found at " + Lib.Vtos( self.s.origin ) + "\\n" );
						GameUtil.G_FreeEdict( self );
						return;
					}
				}

				self.timestamp = GameBase.level.time;
				target_lightramp_think.Think( self );
			}
		}

		static EntThinkAdapter target_earthquake_think = new AnonymousEntThinkAdapter5();
		private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "target_earthquake_think";
			}

			public override Boolean Think( edict_t self )
			{
				Int32 i;
				edict_t e;
				if ( self.last_move_time < GameBase.level.time )
				{
					GameBase.gi.Positioned_sound( self.s.origin, self, Defines.CHAN_AUTO, self.noise_index, 1F, Defines.ATTN_NONE, 0 );
					self.last_move_time = GameBase.level.time + 0.5F;
				}

				for ( i = 1; i < GameBase.num_edicts; i++ )
				{
					e = GameBase.g_edicts[i];
					if ( !e.inuse )
						continue;
					if ( null == e.client )
						continue;
					if ( null == e.groundentity )
						continue;
					e.groundentity = null;
					e.velocity[0] += Lib.Crandom() * 150;
					e.velocity[1] += Lib.Crandom() * 150;
					e.velocity[2] = self.speed * ( 100F / e.mass );
				}

				if ( GameBase.level.time < self.timestamp )
					self.nextthink = GameBase.level.time + Defines.FRAMETIME;
				return true;
			}
		}

		static EntUseAdapter target_earthquake_use = new AnonymousEntUseAdapter13();
		private sealed class AnonymousEntUseAdapter13 : EntUseAdapter
		{

			public override String GetID( )
			{
				return "target_earthquake_use";
			}

			public override void Use( edict_t self, edict_t other, edict_t activator )
			{
				self.timestamp = GameBase.level.time + self.count;
				self.nextthink = GameBase.level.time + Defines.FRAMETIME;
				self.activator = activator;
				self.last_move_time = 0;
			}
		}
	}
}
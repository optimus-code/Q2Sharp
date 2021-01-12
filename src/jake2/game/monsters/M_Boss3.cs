using Q2Sharp.Util;

namespace Q2Sharp.Game.Monsters
{
	public class M_Boss3
	{
		static EntUseAdapter Use_Boss3 = new AnonymousEntUseAdapter();
		private sealed class AnonymousEntUseAdapter : EntUseAdapter
		{

			public override string GetID( )
			{
				return "Use_Boss3";
			}

			public override void Use( edict_t ent, edict_t other, edict_t activator )
			{
				GameBase.gi.WriteByte( Defines.svc_temp_entity );
				GameBase.gi.WriteByte( Defines.TE_BOSSTPORT );
				GameBase.gi.WritePosition( ent.s.origin );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
				GameUtil.G_FreeEdict( ent );
			}
		}

		static EntThinkAdapter Think_Boss3Stand = new AnonymousEntThinkAdapter();
		private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{

			public override string GetID( )
			{
				return "Think_Boss3Stand";
			}

			public override bool Think( edict_t ent )
			{
				if ( ent.s.frame == M_Boss32.FRAME_stand260 )
					ent.s.frame = M_Boss32.FRAME_stand201;
				else
					ent.s.frame++;
				ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
				return true;
			}
		}

		public static void SP_monster_boss3_stand( edict_t self )
		{
			if ( GameBase.deathmatch.value != 0 )
			{
				GameUtil.G_FreeEdict( self );
				return;
			}

			self.movetype = Defines.MOVETYPE_STEP;
			self.solid = Defines.SOLID_BBOX;
			self.model = "models/monsters/boss3/rider/tris.md2";
			self.s.modelindex = GameBase.gi.Modelindex( self.model );
			self.s.frame = M_Boss32.FRAME_stand201;
			GameBase.gi.Soundindex( "misc/bigtele.wav" );
			Math3D.VectorSet( self.mins, -32, -32, 0 );
			Math3D.VectorSet( self.maxs, 32, 32, 90 );
			self.use = Use_Boss3;
			self.think = Think_Boss3Stand;
			self.nextthink = GameBase.level.time + Defines.FRAMETIME;
			GameBase.gi.Linkentity( self );
		}
	}
}
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Render;
using Jake2.Sound;
using Jake2.Util;
using System;
using static Jake2.Client.cl_sustain_t;

namespace Jake2.Client
{
	public class CL_tent
	{
		public class explosion_t
		{
			public Int32 type;
			public entity_t ent = new entity_t();
			public Int32 frames;
			public Single light;
			public Single[] lightcolor = new Single[3];
			public Single start;
			public Int32 baseframe;
			public virtual void Clear( )
			{
				lightcolor[0] = lightcolor[1] = lightcolor[2] = light = start = type = frames = baseframe = 0;
				ent.Clear();
			}
		}

		static readonly Int32 MAX_EXPLOSIONS = 32;
		static explosion_t[] cl_explosions = new explosion_t[MAX_EXPLOSIONS];
		static readonly Int32 MAX_BEAMS = 32;
		static beam_t[] cl_beams = new beam_t[MAX_BEAMS];
		static beam_t[] cl_playerbeams = new beam_t[MAX_BEAMS];
		static readonly Int32 MAX_LASERS = 32;
		static laser_t[] cl_lasers = new laser_t[MAX_LASERS];
		static readonly Int32 MAX_SUSTAINS = 32;
		static cl_sustain_t[] cl_sustains = new cl_sustain_t[MAX_SUSTAINS];
		public class beam_t
		{
			public Int32 entity;
			public Int32 dest_entity;
			public model_t model;
			public Int32 endtime;
			public Single[] offset = new Single[3];
			public Single[] start = new Single[3];
			public Single[] end = new Single[3];
			public virtual void Clear( )
			{
				offset[0] = offset[1] = offset[2] = start[0] = start[1] = start[2] = end[0] = end[1] = end[2] = entity = dest_entity = endtime = 0;
				model = null;
			}
		}

		static CL_tent( )
		{
			for ( var i = 0; i < cl_explosions.Length; i++ )
				cl_explosions[i] = new explosion_t();

			for ( var i = 0; i < cl_beams.Length; i++ )
				cl_beams[i] = new beam_t();
			for ( var i = 0; i < cl_playerbeams.Length; i++ )
				cl_playerbeams[i] = new beam_t();

			for ( var i = 0; i < cl_lasers.Length; i++ )
				cl_lasers[i] = new laser_t();

			for ( var i = 0; i < cl_sustains.Length; i++ )
				cl_sustains[i] = new cl_sustain_t();
		}

		public class laser_t
		{
			public entity_t ent = new entity_t();
			public Int32 endtime;
			public virtual void Clear( )
			{
				endtime = 0;
				ent.Clear();
			}
		}

		public const Int32 ex_free = 0;
		public const Int32 ex_explosion = 1;
		public const Int32 ex_misc = 2;
		public const Int32 ex_flash = 3;
		public const Int32 ex_mflash = 4;
		public const Int32 ex_poly = 5;
		public const Int32 ex_poly2 = 6;
		static sfx_t cl_sfx_ric1;
		static sfx_t cl_sfx_ric2;
		static sfx_t cl_sfx_ric3;
		static sfx_t cl_sfx_lashit;
		static sfx_t cl_sfx_spark5;
		static sfx_t cl_sfx_spark6;
		static sfx_t cl_sfx_spark7;
		static sfx_t cl_sfx_railg;
		static sfx_t cl_sfx_rockexp;
		static sfx_t cl_sfx_grenexp;
		static sfx_t cl_sfx_watrexp;
		static sfx_t cl_sfx_plasexp;
		public static sfx_t[] cl_sfx_footsteps = new sfx_t[4];
		static model_t cl_mod_explode;
		static model_t cl_mod_smoke;
		static model_t cl_mod_flash;
		static model_t cl_mod_parasite_segment;
		static model_t cl_mod_grapple_cable;
		static model_t cl_mod_parasite_tip;
		static model_t cl_mod_explo4;
		static model_t cl_mod_bfg_explo;
		public static model_t cl_mod_powerscreen;
		static model_t cl_mod_plasmaexplo;
		static sfx_t cl_sfx_lightning;
		static sfx_t cl_sfx_disrexp;
		static model_t cl_mod_lightning;
		static model_t cl_mod_heatbeam;
		static model_t cl_mod_monster_heatbeam;
		static model_t cl_mod_explo4_big;
		public static void RegisterTEntSounds( )
		{
			Int32 i;
			String name;
			cl_sfx_ric1 = S.RegisterSound( "world/ric1.wav" );
			cl_sfx_ric2 = S.RegisterSound( "world/ric2.wav" );
			cl_sfx_ric3 = S.RegisterSound( "world/ric3.wav" );
			cl_sfx_lashit = S.RegisterSound( "weapons/lashit.wav" );
			cl_sfx_spark5 = S.RegisterSound( "world/spark5.wav" );
			cl_sfx_spark6 = S.RegisterSound( "world/spark6.wav" );
			cl_sfx_spark7 = S.RegisterSound( "world/spark7.wav" );
			cl_sfx_railg = S.RegisterSound( "weapons/railgf1a.wav" );
			cl_sfx_rockexp = S.RegisterSound( "weapons/rocklx1a.wav" );
			cl_sfx_grenexp = S.RegisterSound( "weapons/grenlx1a.wav" );
			cl_sfx_watrexp = S.RegisterSound( "weapons/xpld_wat.wav" );
			S.RegisterSound( "player/land1.wav" );
			S.RegisterSound( "player/fall2.wav" );
			S.RegisterSound( "player/fall1.wav" );
			for ( i = 0; i < 4; i++ )
			{
				name = "player/step" + ( i + 1 ) + ".wav";
				cl_sfx_footsteps[i] = S.RegisterSound( name );
			}

			cl_sfx_lightning = S.RegisterSound( "weapons/tesla.wav" );
			cl_sfx_disrexp = S.RegisterSound( "weapons/disrupthit.wav" );
		}

		public static void RegisterTEntModels( )
		{
			cl_mod_explode = Globals.re.RegisterModel( "models/objects/explode/tris.md2" );
			cl_mod_smoke = Globals.re.RegisterModel( "models/objects/smoke/tris.md2" );
			cl_mod_flash = Globals.re.RegisterModel( "models/objects/flash/tris.md2" );
			cl_mod_parasite_segment = Globals.re.RegisterModel( "models/monsters/parasite/segment/tris.md2" );
			cl_mod_grapple_cable = Globals.re.RegisterModel( "models/ctf/segment/tris.md2" );
			cl_mod_parasite_tip = Globals.re.RegisterModel( "models/monsters/parasite/tip/tris.md2" );
			cl_mod_explo4 = Globals.re.RegisterModel( "models/objects/r_explode/tris.md2" );
			cl_mod_bfg_explo = Globals.re.RegisterModel( "sprites/s_bfg2.sp2" );
			cl_mod_powerscreen = Globals.re.RegisterModel( "models/items/armor/effect/tris.md2" );
			Globals.re.RegisterModel( "models/objects/laser/tris.md2" );
			Globals.re.RegisterModel( "models/objects/grenade2/tris.md2" );
			Globals.re.RegisterModel( "models/weapons/v_machn/tris.md2" );
			Globals.re.RegisterModel( "models/weapons/v_handgr/tris.md2" );
			Globals.re.RegisterModel( "models/weapons/v_shotg2/tris.md2" );
			Globals.re.RegisterModel( "models/objects/gibs/bone/tris.md2" );
			Globals.re.RegisterModel( "models/objects/gibs/sm_meat/tris.md2" );
			Globals.re.RegisterModel( "models/objects/gibs/bone2/tris.md2" );
			Globals.re.RegisterPic( "w_machinegun" );
			Globals.re.RegisterPic( "a_bullets" );
			Globals.re.RegisterPic( "i_health" );
			Globals.re.RegisterPic( "a_grenades" );
			cl_mod_explo4_big = Globals.re.RegisterModel( "models/objects/r_explode2/tris.md2" );
			cl_mod_lightning = Globals.re.RegisterModel( "models/proj/lightning/tris.md2" );
			cl_mod_heatbeam = Globals.re.RegisterModel( "models/proj/beam/tris.md2" );
			cl_mod_monster_heatbeam = Globals.re.RegisterModel( "models/proj/widowbeam/tris.md2" );
		}

		public static void ClearTEnts( )
		{
			for ( var i = 0; i < cl_beams.Length; i++ )
				cl_beams[i].Clear();
			for ( var i = 0; i < cl_explosions.Length; i++ )
				cl_explosions[i].Clear();
			for ( var i = 0; i < cl_lasers.Length; i++ )
				cl_lasers[i].Clear();
			for ( var i = 0; i < cl_playerbeams.Length; i++ )
				cl_playerbeams[i].Clear();
			for ( var i = 0; i < cl_sustains.Length; i++ )
				cl_sustains[i].Clear();
		}

		static explosion_t AllocExplosion( )
		{
			Int32 i;
			Int32 time;
			Int32 index;
			for ( i = 0; i < MAX_EXPLOSIONS; i++ )
			{
				if ( cl_explosions[i].type == ex_free )
				{
					cl_explosions[i].Clear();
					return cl_explosions[i];
				}
			}

			time = Globals.cl.time;
			index = 0;
			for ( i = 0; i < MAX_EXPLOSIONS; i++ )
				if ( cl_explosions[i].start < time )
				{
					time = ( Int32 ) cl_explosions[i].start;
					index = i;
				}

			cl_explosions[index].Clear();
			return cl_explosions[index];
		}

		public static void SmokeAndFlash( Single[] origin )
		{
			explosion_t ex;
			ex = AllocExplosion();
			Math3D.VectorCopy( origin, ex.ent.origin );
			ex.type = ex_misc;
			ex.frames = 4;
			ex.ent.flags = Defines.RF_TRANSLUCENT;
			ex.start = Globals.cl.frame.servertime - 100;
			ex.ent.model = cl_mod_smoke;
			ex = AllocExplosion();
			Math3D.VectorCopy( origin, ex.ent.origin );
			ex.type = ex_flash;
			ex.ent.flags = Defines.RF_FULLBRIGHT;
			ex.frames = 2;
			ex.start = Globals.cl.frame.servertime - 100;
			ex.ent.model = cl_mod_flash;
		}

		static Int32 ParseBeam( model_t model )
		{
			Int32 ent;
			Single[] start = new Single[3];
			Single[] end = new Single[3];
			beam_t[] b;
			Int32 i;
			ent = MSG.ReadShort( Globals.net_message );
			MSG.ReadPos( Globals.net_message, start );
			MSG.ReadPos( Globals.net_message, end );
			b = cl_beams;
			for ( i = 0; i < MAX_BEAMS; i++ )
				if ( b[i].entity == ent )
				{
					b[i].entity = ent;
					b[i].model = model;
					b[i].endtime = Globals.cl.time + 200;
					Math3D.VectorCopy( start, b[i].start );
					Math3D.VectorCopy( end, b[i].end );
					Math3D.VectorClear( b[i].offset );
					return ent;
				}

			b = cl_beams;
			for ( i = 0; i < MAX_BEAMS; i++ )
			{
				if ( b[i].model == null || b[i].endtime < Globals.cl.time )
				{
					b[i].entity = ent;
					b[i].model = model;
					b[i].endtime = Globals.cl.time + 200;
					Math3D.VectorCopy( start, b[i].start );
					Math3D.VectorCopy( end, b[i].end );
					Math3D.VectorClear( b[i].offset );
					return ent;
				}
			}

			Com.Printf( "beam list overflow!\\n" );
			return ent;
		}

		static Int32 ParseBeam2( model_t model )
		{
			Int32 ent;
			Single[] start = new Single[3];
			Single[] end = new Single[3];
			Single[] offset = new Single[3];
			beam_t[] b;
			Int32 i;
			ent = MSG.ReadShort( Globals.net_message );
			MSG.ReadPos( Globals.net_message, start );
			MSG.ReadPos( Globals.net_message, end );
			MSG.ReadPos( Globals.net_message, offset );
			b = cl_beams;
			for ( i = 0; i < MAX_BEAMS; i++ )
				if ( b[i].entity == ent )
				{
					b[i].entity = ent;
					b[i].model = model;
					b[i].endtime = Globals.cl.time + 200;
					Math3D.VectorCopy( start, b[i].start );
					Math3D.VectorCopy( end, b[i].end );
					Math3D.VectorCopy( offset, b[i].offset );
					return ent;
				}

			b = cl_beams;
			for ( i = 0; i < MAX_BEAMS; i++ )
			{
				if ( b[i].model == null || b[i].endtime < Globals.cl.time )
				{
					b[i].entity = ent;
					b[i].model = model;
					b[i].endtime = Globals.cl.time + 200;
					Math3D.VectorCopy( start, b[i].start );
					Math3D.VectorCopy( end, b[i].end );
					Math3D.VectorCopy( offset, b[i].offset );
					return ent;
				}
			}

			Com.Printf( "beam list overflow!\\n" );
			return ent;
		}

		static Int32 ParsePlayerBeam( model_t model )
		{
			Int32 ent;
			Single[] start = new Single[3];
			Single[] end = new Single[3];
			Single[] offset = new Single[3];
			beam_t[] b;
			Int32 i;
			ent = MSG.ReadShort( Globals.net_message );
			MSG.ReadPos( Globals.net_message, start );
			MSG.ReadPos( Globals.net_message, end );
			if ( model == cl_mod_heatbeam )
				Math3D.VectorSet( offset, 2, 7, -3 );
			else if ( model == cl_mod_monster_heatbeam )
			{
				model = cl_mod_heatbeam;
				Math3D.VectorSet( offset, 0, 0, 0 );
			}
			else
				MSG.ReadPos( Globals.net_message, offset );
			b = cl_playerbeams;
			for ( i = 0; i < MAX_BEAMS; i++ )
			{
				if ( b[i].entity == ent )
				{
					b[i].entity = ent;
					b[i].model = model;
					b[i].endtime = Globals.cl.time + 200;
					Math3D.VectorCopy( start, b[i].start );
					Math3D.VectorCopy( end, b[i].end );
					Math3D.VectorCopy( offset, b[i].offset );
					return ent;
				}
			}

			b = cl_playerbeams;
			for ( i = 0; i < MAX_BEAMS; i++ )
			{
				if ( b[i].model == null || b[i].endtime < Globals.cl.time )
				{
					b[i].entity = ent;
					b[i].model = model;
					b[i].endtime = Globals.cl.time + 100;
					Math3D.VectorCopy( start, b[i].start );
					Math3D.VectorCopy( end, b[i].end );
					Math3D.VectorCopy( offset, b[i].offset );
					return ent;
				}
			}

			Com.Printf( "beam list overflow!\\n" );
			return ent;
		}

		private static readonly Single[] start = new Single[3];
		private static readonly Single[] end = new Single[3];
		static Int32 ParseLightning( model_t model )
		{
			Int32 srcEnt, destEnt;
			beam_t[] b;
			Int32 i;
			srcEnt = MSG.ReadShort( Globals.net_message );
			destEnt = MSG.ReadShort( Globals.net_message );
			MSG.ReadPos( Globals.net_message, start );
			MSG.ReadPos( Globals.net_message, end );
			b = cl_beams;
			for ( i = 0; i < MAX_BEAMS; i++ )
				if ( b[i].entity == srcEnt && b[i].dest_entity == destEnt )
				{
					b[i].entity = srcEnt;
					b[i].dest_entity = destEnt;
					b[i].model = model;
					b[i].endtime = Globals.cl.time + 200;
					Math3D.VectorCopy( start, b[i].start );
					Math3D.VectorCopy( end, b[i].end );
					Math3D.VectorClear( b[i].offset );
					return srcEnt;
				}

			b = cl_beams;
			for ( i = 0; i < MAX_BEAMS; i++ )
			{
				if ( b[i].model == null || b[i].endtime < Globals.cl.time )
				{
					b[i].entity = srcEnt;
					b[i].dest_entity = destEnt;
					b[i].model = model;
					b[i].endtime = Globals.cl.time + 200;
					Math3D.VectorCopy( start, b[i].start );
					Math3D.VectorCopy( end, b[i].end );
					Math3D.VectorClear( b[i].offset );
					return srcEnt;
				}
			}

			Com.Printf( "beam list overflow!\\n" );
			return srcEnt;
		}

		static void ParseLaser( Int32 colors )
		{
			laser_t[] l;
			Int32 i;
			MSG.ReadPos( Globals.net_message, start );
			MSG.ReadPos( Globals.net_message, end );
			l = cl_lasers;
			for ( i = 0; i < MAX_LASERS; i++ )
			{
				if ( l[i].endtime < Globals.cl.time )
				{
					l[i].ent.flags = Defines.RF_TRANSLUCENT | Defines.RF_BEAM;
					Math3D.VectorCopy( start, l[i].ent.origin );
					Math3D.VectorCopy( end, l[i].ent.oldorigin );
					l[i].ent.alpha = 0.3F;
					l[i].ent.skinnum = ( colors >> ( ( Lib.Rand() % 4 ) * 8 ) ) & 0xff;
					l[i].ent.model = null;
					l[i].ent.frame = 4;
					l[i].endtime = Globals.cl.time + 100;
					return;
				}
			}
		}

		private static readonly Single[] pos = new Single[3];
		private static readonly Single[] dir = new Single[3];
		static void ParseSteam( )
		{
			Int32 id, i;
			Int32 r;
			Int32 cnt;
			Int32 color;
			Int32 magnitude;
			cl_sustain_t[] s;
			cl_sustain_t free_sustain;
			id = MSG.ReadShort( Globals.net_message );
			if ( id != -1 )
			{
				free_sustain = null;
				s = cl_sustains;
				for ( i = 0; i < MAX_SUSTAINS; i++ )
				{
					if ( s[i].id == 0 )
					{
						free_sustain = s[i];
						break;
					}
				}

				if ( free_sustain != null )
				{
					s[i].id = id;
					s[i].count = MSG.ReadByte( Globals.net_message );
					MSG.ReadPos( Globals.net_message, s[i].org );
					MSG.ReadDir( Globals.net_message, s[i].dir );
					r = MSG.ReadByte( Globals.net_message );
					s[i].color = r & 0xff;
					s[i].magnitude = MSG.ReadShort( Globals.net_message );
					s[i].endtime = Globals.cl.time + MSG.ReadLong( Globals.net_message );
					s[i].think = new AnonymousThinkAdapter();
					s[i].thinkinterval = 100;
					s[i].nextthink = Globals.cl.time;
				}
				else
				{
					cnt = MSG.ReadByte( Globals.net_message );
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					r = MSG.ReadByte( Globals.net_message );
					magnitude = MSG.ReadShort( Globals.net_message );
					magnitude = MSG.ReadLong( Globals.net_message );
				}
			}
			else
			{
				cnt = MSG.ReadByte( Globals.net_message );
				MSG.ReadPos( Globals.net_message, pos );
				MSG.ReadDir( Globals.net_message, dir );
				r = MSG.ReadByte( Globals.net_message );
				magnitude = MSG.ReadShort( Globals.net_message );
				color = r & 0xff;
				CL_newfx.ParticleSteamEffect( pos, dir, color, cnt, magnitude );
			}
		}

		private sealed class AnonymousThinkAdapter : ThinkAdapter
		{
			public override void Think( cl_sustain_t self )
			{
				CL_newfx.ParticleSteamEffect2( self );
			}
		}

		static void ParseWidow( )
		{
			Int32 id, i;
			cl_sustain_t[] s;
			cl_sustain_t free_sustain;
			id = MSG.ReadShort( Globals.net_message );
			free_sustain = null;
			s = cl_sustains;
			for ( i = 0; i < MAX_SUSTAINS; i++ )
			{
				if ( s[i].id == 0 )
				{
					free_sustain = s[i];
					break;
				}
			}

			if ( free_sustain != null )
			{
				s[i].id = id;
				MSG.ReadPos( Globals.net_message, s[i].org );
				s[i].endtime = Globals.cl.time + 2100;
				s[i].think = new AnonymousThinkAdapter1();
				s[i].thinkinterval = 1;
				s[i].nextthink = Globals.cl.time;
			}
			else
			{
				MSG.ReadPos( Globals.net_message, pos );
			}
		}

		private sealed class AnonymousThinkAdapter1 : ThinkAdapter
		{
			public override void Think( cl_sustain_t self )
			{
				CL_newfx.Widowbeamout( self );
			}
		}

		static void ParseNuke( )
		{
			Int32 i;
			cl_sustain_t[] s;
			cl_sustain_t free_sustain;
			free_sustain = null;
			s = cl_sustains;
			for ( i = 0; i < MAX_SUSTAINS; i++ )
			{
				if ( s[i].id == 0 )
				{
					free_sustain = s[i];
					break;
				}
			}

			if ( free_sustain != null )
			{
				s[i].id = 21000;
				MSG.ReadPos( Globals.net_message, s[i].org );
				s[i].endtime = Globals.cl.time + 1000;
				s[i].think = new AnonymousThinkAdapter2();
				s[i].thinkinterval = 1;
				s[i].nextthink = Globals.cl.time;
			}
			else
			{
				MSG.ReadPos( Globals.net_message, pos );
			}
		}

		private sealed class AnonymousThinkAdapter2 : ThinkAdapter
		{
			public override void Think( cl_sustain_t self )
			{
				CL_newfx.Nukeblast( self );
			}
		}

		static Int32[] splash_color = new[] { 0x00, 0xe0, 0xb0, 0x50, 0xd0, 0xe0, 0xe8 };
		private static readonly Single[] pos2 = new Single[] { 0, 0, 0 };
		public static void ParseTEnt( )
		{
			Int32 type;
			explosion_t ex;
			Int32 cnt;
			Int32 color;
			Int32 r;
			Int32 ent;
			Int32 magnitude;
			type = MSG.ReadByte( Globals.net_message );
			switch ( type )

			{
				case Defines.TE_BLOOD:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					CL_fx.ParticleEffect( pos, dir, 0xe8, 60 );
					break;
				case Defines.TE_GUNSHOT:
				case Defines.TE_SPARKS:
				case Defines.TE_BULLET_SPARKS:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					if ( type == Defines.TE_GUNSHOT )
						CL_fx.ParticleEffect( pos, dir, 0, 40 );
					else
						CL_fx.ParticleEffect( pos, dir, 0xe0, 6 );
					if ( type != Defines.TE_SPARKS )
					{
						SmokeAndFlash( pos );
						cnt = Lib.Rand() & 15;
						if ( cnt == 1 )
							S.StartSound( pos, 0, 0, cl_sfx_ric1, 1, Defines.ATTN_NORM, 0 );
						else if ( cnt == 2 )
							S.StartSound( pos, 0, 0, cl_sfx_ric2, 1, Defines.ATTN_NORM, 0 );
						else if ( cnt == 3 )
							S.StartSound( pos, 0, 0, cl_sfx_ric3, 1, Defines.ATTN_NORM, 0 );
					}

					break;
				case Defines.TE_SCREEN_SPARKS:
				case Defines.TE_SHIELD_SPARKS:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					if ( type == Defines.TE_SCREEN_SPARKS )
						CL_fx.ParticleEffect( pos, dir, 0xd0, 40 );
					else
						CL_fx.ParticleEffect( pos, dir, 0xb0, 40 );
					S.StartSound( pos, 0, 0, cl_sfx_lashit, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_SHOTGUN:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					CL_fx.ParticleEffect( pos, dir, 0, 20 );
					SmokeAndFlash( pos );
					break;
				case Defines.TE_SPLASH:
					cnt = MSG.ReadByte( Globals.net_message );
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					r = MSG.ReadByte( Globals.net_message );
					if ( r > 6 )
						color = 0x00;
					else
						color = splash_color[r];
					CL_fx.ParticleEffect( pos, dir, color, cnt );
					if ( r == Defines.SPLASH_SPARKS )
					{
						r = Lib.Rand() & 3;
						if ( r == 0 )
							S.StartSound( pos, 0, 0, cl_sfx_spark5, 1, Defines.ATTN_STATIC, 0 );
						else if ( r == 1 )
							S.StartSound( pos, 0, 0, cl_sfx_spark6, 1, Defines.ATTN_STATIC, 0 );
						else
							S.StartSound( pos, 0, 0, cl_sfx_spark7, 1, Defines.ATTN_STATIC, 0 );
					}

					break;
				case Defines.TE_LASER_SPARKS:
					cnt = MSG.ReadByte( Globals.net_message );
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					color = MSG.ReadByte( Globals.net_message );
					CL_fx.ParticleEffect2( pos, dir, color, cnt );
					break;
				case Defines.TE_BLUEHYPERBLASTER:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadPos( Globals.net_message, dir );
					CL_fx.BlasterParticles( pos, dir );
					break;
				case Defines.TE_BLASTER:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					CL_fx.BlasterParticles( pos, dir );
					ex = AllocExplosion();
					Math3D.VectorCopy( pos, ex.ent.origin );
					ex.ent.angles[0] = ( Single ) ( Math.Acos( dir[2] ) / Math.PI * 180 );
					if ( dir[0] != 0F )
						ex.ent.angles[1] = ( Single ) ( Math.Atan2( dir[1], dir[0] ) / Math.PI * 180 );
					else if ( dir[1] > 0 )
						ex.ent.angles[1] = 90;
					else if ( dir[1] < 0 )
						ex.ent.angles[1] = 270;
					else
						ex.ent.angles[1] = 0;
					ex.type = ex_misc;
					ex.ent.flags = Defines.RF_FULLBRIGHT | Defines.RF_TRANSLUCENT;
					ex.start = Globals.cl.frame.servertime - 100;
					ex.light = 150;
					ex.lightcolor[0] = 1;
					ex.lightcolor[1] = 1;
					ex.ent.model = cl_mod_explode;
					ex.frames = 4;
					S.StartSound( pos, 0, 0, cl_sfx_lashit, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_RAILTRAIL:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadPos( Globals.net_message, pos2 );
					CL_fx.RailTrail( pos, pos2 );
					S.StartSound( pos2, 0, 0, cl_sfx_railg, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_EXPLOSION2:
				case Defines.TE_GRENADE_EXPLOSION:
				case Defines.TE_GRENADE_EXPLOSION_WATER:
					MSG.ReadPos( Globals.net_message, pos );
					ex = AllocExplosion();
					Math3D.VectorCopy( pos, ex.ent.origin );
					ex.type = ex_poly;
					ex.ent.flags = Defines.RF_FULLBRIGHT;
					ex.start = Globals.cl.frame.servertime - 100;
					ex.light = 350;
					ex.lightcolor[0] = 1F;
					ex.lightcolor[1] = 0.5F;
					ex.lightcolor[2] = 0.5F;
					ex.ent.model = cl_mod_explo4;
					ex.frames = 19;
					ex.baseframe = 30;
					ex.ent.angles[1] = Lib.Rand() % 360;
					CL_fx.ExplosionParticles( pos );
					if ( type == Defines.TE_GRENADE_EXPLOSION_WATER )
						S.StartSound( pos, 0, 0, cl_sfx_watrexp, 1, Defines.ATTN_NORM, 0 );
					else
						S.StartSound( pos, 0, 0, cl_sfx_grenexp, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_PLASMA_EXPLOSION:
					MSG.ReadPos( Globals.net_message, pos );
					ex = AllocExplosion();
					Math3D.VectorCopy( pos, ex.ent.origin );
					ex.type = ex_poly;
					ex.ent.flags = Defines.RF_FULLBRIGHT;
					ex.start = Globals.cl.frame.servertime - 100;
					ex.light = 350;
					ex.lightcolor[0] = 1F;
					ex.lightcolor[1] = 0.5F;
					ex.lightcolor[2] = 0.5F;
					ex.ent.angles[1] = Lib.Rand() % 360;
					ex.ent.model = cl_mod_explo4;
					if ( Globals.rnd.NextDouble() < 0.5 )
						ex.baseframe = 15;
					ex.frames = 15;
					CL_fx.ExplosionParticles( pos );
					S.StartSound( pos, 0, 0, cl_sfx_rockexp, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_EXPLOSION1:
				case Defines.TE_EXPLOSION1_BIG:
				case Defines.TE_ROCKET_EXPLOSION:
				case Defines.TE_ROCKET_EXPLOSION_WATER:
				case Defines.TE_EXPLOSION1_NP:
					MSG.ReadPos( Globals.net_message, pos );
					ex = AllocExplosion();
					Math3D.VectorCopy( pos, ex.ent.origin );
					ex.type = ex_poly;
					ex.ent.flags = Defines.RF_FULLBRIGHT;
					ex.start = Globals.cl.frame.servertime - 100;
					ex.light = 350;
					ex.lightcolor[0] = 1F;
					ex.lightcolor[1] = 0.5F;
					ex.lightcolor[2] = 0.5F;
					ex.ent.angles[1] = Lib.Rand() % 360;
					if ( type != Defines.TE_EXPLOSION1_BIG )
						ex.ent.model = cl_mod_explo4;
					else
						ex.ent.model = cl_mod_explo4_big;
					if ( Globals.rnd.NextDouble() < 0.5 )
						ex.baseframe = 15;
					ex.frames = 15;
					if ( ( type != Defines.TE_EXPLOSION1_BIG ) && ( type != Defines.TE_EXPLOSION1_NP ) )
						CL_fx.ExplosionParticles( pos );
					if ( type == Defines.TE_ROCKET_EXPLOSION_WATER )
						S.StartSound( pos, 0, 0, cl_sfx_watrexp, 1, Defines.ATTN_NORM, 0 );
					else
						S.StartSound( pos, 0, 0, cl_sfx_rockexp, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_BFG_EXPLOSION:
					MSG.ReadPos( Globals.net_message, pos );
					ex = AllocExplosion();
					Math3D.VectorCopy( pos, ex.ent.origin );
					ex.type = ex_poly;
					ex.ent.flags = Defines.RF_FULLBRIGHT;
					ex.start = Globals.cl.frame.servertime - 100;
					ex.light = 350;
					ex.lightcolor[0] = 0F;
					ex.lightcolor[1] = 1F;
					ex.lightcolor[2] = 0F;
					ex.ent.model = cl_mod_bfg_explo;
					ex.ent.flags |= Defines.RF_TRANSLUCENT;
					ex.ent.alpha = 0.3F;
					ex.frames = 4;
					break;
				case Defines.TE_BFG_BIGEXPLOSION:
					MSG.ReadPos( Globals.net_message, pos );
					CL_fx.BFGExplosionParticles( pos );
					break;
				case Defines.TE_BFG_LASER:
					ParseLaser( unchecked(( Int32 ) 0xd0d1d2d3) );
					break;
				case Defines.TE_BUBBLETRAIL:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadPos( Globals.net_message, pos2 );
					CL_fx.BubbleTrail( pos, pos2 );
					break;
				case Defines.TE_PARASITE_ATTACK:
				case Defines.TE_MEDIC_CABLE_ATTACK:
					ent = ParseBeam( cl_mod_parasite_segment );
					break;
				case Defines.TE_BOSSTPORT:
					MSG.ReadPos( Globals.net_message, pos );
					CL_fx.BigTeleportParticles( pos );
					S.StartSound( pos, 0, 0, S.RegisterSound( "misc/bigtele.wav" ), 1, Defines.ATTN_NONE, 0 );
					break;
				case Defines.TE_GRAPPLE_CABLE:
					ent = ParseBeam2( cl_mod_grapple_cable );
					break;
				case Defines.TE_WELDING_SPARKS:
					cnt = MSG.ReadByte( Globals.net_message );
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					color = MSG.ReadByte( Globals.net_message );
					CL_fx.ParticleEffect2( pos, dir, color, cnt );
					ex = AllocExplosion();
					Math3D.VectorCopy( pos, ex.ent.origin );
					ex.type = ex_flash;
					ex.ent.flags = Defines.RF_BEAM;
					ex.start = Globals.cl.frame.servertime - 0.1F;
					ex.light = 100 + ( Lib.Rand() % 75 );
					ex.lightcolor[0] = 1F;
					ex.lightcolor[1] = 1F;
					ex.lightcolor[2] = 0.3F;
					ex.ent.model = cl_mod_flash;
					ex.frames = 2;
					break;
				case Defines.TE_GREENBLOOD:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					CL_fx.ParticleEffect2( pos, dir, 0xdf, 30 );
					break;
				case Defines.TE_TUNNEL_SPARKS:
					cnt = MSG.ReadByte( Globals.net_message );
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					color = MSG.ReadByte( Globals.net_message );
					CL_fx.ParticleEffect3( pos, dir, color, cnt );
					break;
				case Defines.TE_BLASTER2:
				case Defines.TE_FLECHETTE:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					if ( type == Defines.TE_BLASTER2 )
						CL_newfx.BlasterParticles2( pos, dir, 0xd0 );
					else
						CL_newfx.BlasterParticles2( pos, dir, 0x6f );
					ex = AllocExplosion();
					Math3D.VectorCopy( pos, ex.ent.origin );
					ex.ent.angles[0] = ( Single ) ( Math.Acos( dir[2] ) / Math.PI * 180 );
					if ( dir[0] != 0F )
						ex.ent.angles[1] = ( Single ) ( Math.Atan2( dir[1], dir[0] ) / Math.PI * 180 );
					else if ( dir[1] > 0 )
						ex.ent.angles[1] = 90;
					else if ( dir[1] < 0 )
						ex.ent.angles[1] = 270;
					else
						ex.ent.angles[1] = 0;
					ex.type = ex_misc;
					ex.ent.flags = Defines.RF_FULLBRIGHT | Defines.RF_TRANSLUCENT;
					if ( type == Defines.TE_BLASTER2 )
						ex.ent.skinnum = 1;
					else
						ex.ent.skinnum = 2;
					ex.start = Globals.cl.frame.servertime - 100;
					ex.light = 150;
					if ( type == Defines.TE_BLASTER2 )
						ex.lightcolor[1] = 1;
					else
					{
						ex.lightcolor[0] = 0.19F;
						ex.lightcolor[1] = 0.41F;
						ex.lightcolor[2] = 0.75F;
					}

					ex.ent.model = cl_mod_explode;
					ex.frames = 4;
					S.StartSound( pos, 0, 0, cl_sfx_lashit, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_LIGHTNING:
					ent = ParseLightning( cl_mod_lightning );
					S.StartSound( null, ent, Defines.CHAN_WEAPON, cl_sfx_lightning, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_DEBUGTRAIL:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadPos( Globals.net_message, pos2 );
					CL_newfx.DebugTrail( pos, pos2 );
					break;
				case Defines.TE_PLAIN_EXPLOSION:
					MSG.ReadPos( Globals.net_message, pos );
					ex = AllocExplosion();
					Math3D.VectorCopy( pos, ex.ent.origin );
					ex.type = ex_poly;
					ex.ent.flags = Defines.RF_FULLBRIGHT;
					ex.start = Globals.cl.frame.servertime - 100;
					ex.light = 350;
					ex.lightcolor[0] = 1F;
					ex.lightcolor[1] = 0.5F;
					ex.lightcolor[2] = 0.5F;
					ex.ent.angles[1] = Lib.Rand() % 360;
					ex.ent.model = cl_mod_explo4;
					if ( Globals.rnd.NextDouble() < 0.5 )
						ex.baseframe = 15;
					ex.frames = 15;
					if ( type == Defines.TE_ROCKET_EXPLOSION_WATER )
						S.StartSound( pos, 0, 0, cl_sfx_watrexp, 1, Defines.ATTN_NORM, 0 );
					else
						S.StartSound( pos, 0, 0, cl_sfx_rockexp, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_FLASHLIGHT:
					MSG.ReadPos( Globals.net_message, pos );
					ent = MSG.ReadShort( Globals.net_message );
					CL_newfx.Flashlight( ent, pos );
					break;
				case Defines.TE_FORCEWALL:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadPos( Globals.net_message, pos2 );
					color = MSG.ReadByte( Globals.net_message );
					CL_newfx.ForceWall( pos, pos2, color );
					break;
				case Defines.TE_HEATBEAM:
					ent = ParsePlayerBeam( cl_mod_heatbeam );
					break;
				case Defines.TE_MONSTER_HEATBEAM:
					ent = ParsePlayerBeam( cl_mod_monster_heatbeam );
					break;
				case Defines.TE_HEATBEAM_SPARKS:
					cnt = 50;
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					r = 8;
					magnitude = 60;
					color = r & 0xff;
					CL_newfx.ParticleSteamEffect( pos, dir, color, cnt, magnitude );
					S.StartSound( pos, 0, 0, cl_sfx_lashit, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_HEATBEAM_STEAM:
					cnt = 20;
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					color = 0xe0;
					magnitude = 60;
					CL_newfx.ParticleSteamEffect( pos, dir, color, cnt, magnitude );
					S.StartSound( pos, 0, 0, cl_sfx_lashit, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_STEAM:
					ParseSteam();
					break;
				case Defines.TE_BUBBLETRAIL2:
					cnt = 8;
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadPos( Globals.net_message, pos2 );
					CL_newfx.BubbleTrail2( pos, pos2, cnt );
					S.StartSound( pos, 0, 0, cl_sfx_lashit, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_MOREBLOOD:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					CL_fx.ParticleEffect( pos, dir, 0xe8, 250 );
					break;
				case Defines.TE_CHAINFIST_SMOKE:
					dir[0] = 0;
					dir[1] = 0;
					dir[2] = 1;
					MSG.ReadPos( Globals.net_message, pos );
					CL_newfx.ParticleSmokeEffect( pos, dir, 0, 20, 20 );
					break;
				case Defines.TE_ELECTRIC_SPARKS:
					MSG.ReadPos( Globals.net_message, pos );
					MSG.ReadDir( Globals.net_message, dir );
					CL_fx.ParticleEffect( pos, dir, 0x75, 40 );
					S.StartSound( pos, 0, 0, cl_sfx_lashit, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_TRACKER_EXPLOSION:
					MSG.ReadPos( Globals.net_message, pos );
					CL_newfx.ColorFlash( pos, 0, 150, -1, -1, -1 );
					CL_newfx.ColorExplosionParticles( pos, 0, 1 );
					S.StartSound( pos, 0, 0, cl_sfx_disrexp, 1, Defines.ATTN_NORM, 0 );
					break;
				case Defines.TE_TELEPORT_EFFECT:
				case Defines.TE_DBALL_GOAL:
					MSG.ReadPos( Globals.net_message, pos );
					CL_fx.TeleportParticles( pos );
					break;
				case Defines.TE_WIDOWBEAMOUT:
					ParseWidow();
					break;
				case Defines.TE_NUKEBLAST:
					ParseNuke();
					break;
				case Defines.TE_WIDOWSPLASH:
					MSG.ReadPos( Globals.net_message, pos );
					CL_newfx.WidowSplash( pos );
					break;
				default:
					Com.Error( Defines.ERR_DROP, "CL_ParseTEnt: bad type" );
					break;
			}
		}

		private static readonly entity_t ent = new entity_t();
		static void AddBeams( )
		{
			Int32 i, j;
			beam_t[] b;
			Single d;
			Single yaw, pitch;
			Single forward;
			Single len, steps;
			Single model_length;
			b = cl_beams;
			for ( i = 0; i < MAX_BEAMS; i++ )
			{
				if ( b[i].model == null || b[i].endtime < Globals.cl.time )
					continue;
				if ( b[i].entity == Globals.cl.playernum + 1 )
				{
					Math3D.VectorCopy( Globals.cl.refdef.vieworg, b[i].start );
					b[i].start[2] -= 22;
				}

				Math3D.VectorAdd( b[i].start, b[i].offset, org );
				Math3D.VectorSubtract( b[i].end, org, dist );
				if ( dist[1] == 0 && dist[0] == 0 )
				{
					yaw = 0;
					if ( dist[2] > 0 )
						pitch = 90;
					else
						pitch = 270;
				}
				else
				{
					if ( dist[0] != 0F )
						yaw = ( Single ) ( Math.Atan2( dist[1], dist[0] ) * 180 / Math.PI );
					else if ( dist[1] > 0 )
						yaw = 90;
					else
						yaw = 270;
					if ( yaw < 0 )
						yaw += 360;
					forward = ( Single ) Math.Sqrt( dist[0] * dist[0] + dist[1] * dist[1] );
					pitch = ( Single ) ( Math.Atan2( dist[2], forward ) * -180 / Math.PI );
					if ( pitch < 0 )
						pitch += 360;
				}

				d = Math3D.VectorNormalize( dist );
				ent.Clear();
				if ( b[i].model == cl_mod_lightning )
				{
					model_length = 35F;
					d -= 20;
				}
				else
				{
					model_length = 30F;
				}

				steps = ( Single ) Math.Ceiling( d / model_length );
				len = ( d - model_length ) / ( steps - 1 );
				if ( ( b[i].model == cl_mod_lightning ) && ( d <= model_length ) )
				{
					Math3D.VectorCopy( b[i].end, ent.origin );
					ent.model = b[i].model;
					ent.flags = Defines.RF_FULLBRIGHT;
					ent.angles[0] = pitch;
					ent.angles[1] = yaw;
					ent.angles[2] = Lib.Rand() % 360;
					V.AddEntity( ent );
					return;
				}

				while ( d > 0 )
				{
					Math3D.VectorCopy( org, ent.origin );
					ent.model = b[i].model;
					if ( b[i].model == cl_mod_lightning )
					{
						ent.flags = Defines.RF_FULLBRIGHT;
						ent.angles[0] = -pitch;
						ent.angles[1] = yaw + 180F;
						ent.angles[2] = Lib.Rand() % 360;
					}
					else
					{
						ent.angles[0] = pitch;
						ent.angles[1] = yaw;
						ent.angles[2] = Lib.Rand() % 360;
					}

					V.AddEntity( ent );
					for ( j = 0; j < 3; j++ )
						org[j] += dist[j] * len;
					d -= model_length;
				}
			}
		}

		private static readonly Single[] dist = new Single[3];
		private static readonly Single[] org = new Single[3];
		private static readonly Single[] f = new Single[3];
		private static readonly Single[] u = new Single[3];
		private static readonly Single[] r = new Single[3];
		static void AddPlayerBeams( )
		{
			Single d;
			Single yaw, pitch;
			Single forward;
			Single len, steps;
			var framenum = 0;
			Single model_length;
			Single hand_multiplier;
			frame_t oldframe;
			player_state_t ps, ops;
			if ( Globals.hand != null )
			{
				if ( Globals.hand.value == 2 )
					hand_multiplier = 0;
				else if ( Globals.hand.value == 1 )
					hand_multiplier = -1;
				else
					hand_multiplier = 1;
			}
			else
			{
				hand_multiplier = 1;
			}

			beam_t[] b = cl_playerbeams;
			for ( var i = 0; i < MAX_BEAMS; i++ )
			{
				if ( b[i].model == null || b[i].endtime < Globals.cl.time )
					continue;
				if ( cl_mod_heatbeam != null && ( b[i].model == cl_mod_heatbeam ) )
				{
					if ( b[i].entity == Globals.cl.playernum + 1 )
					{
						ps = Globals.cl.frame.playerstate;
						var j = ( Globals.cl.frame.serverframe - 1 ) & Defines.UPDATE_MASK;
						oldframe = Globals.cl.frames[j];
						if ( oldframe.serverframe != Globals.cl.frame.serverframe - 1 || !oldframe.valid )
							oldframe = Globals.cl.frame;
						ops = oldframe.playerstate;
						for ( j = 0; j < 3; j++ )
						{
							b[i].start[j] = Globals.cl.refdef.vieworg[j] + ops.gunoffset[j] + Globals.cl.lerpfrac * ( ps.gunoffset[j] - ops.gunoffset[j] );
						}

						Math3D.VectorMA( b[i].start, ( hand_multiplier * b[i].offset[0] ), Globals.cl.v_right, org );
						Math3D.VectorMA( org, b[i].offset[1], Globals.cl.v_forward, org );
						Math3D.VectorMA( org, b[i].offset[2], Globals.cl.v_up, org );
						if ( ( Globals.hand != null ) && ( Globals.hand.value == 2 ) )
						{
							Math3D.VectorMA( org, -1, Globals.cl.v_up, org );
						}

						Math3D.VectorCopy( Globals.cl.v_right, r );
						Math3D.VectorCopy( Globals.cl.v_forward, f );
						Math3D.VectorCopy( Globals.cl.v_up, u );
					}
					else
						Math3D.VectorCopy( b[i].start, org );
				}
				else
				{
					if ( b[i].entity == Globals.cl.playernum + 1 )
					{
						Math3D.VectorCopy( Globals.cl.refdef.vieworg, b[i].start );
						b[i].start[2] -= 22;
					}

					Math3D.VectorAdd( b[i].start, b[i].offset, org );
				}

				Math3D.VectorSubtract( b[i].end, org, dist );
				if ( cl_mod_heatbeam != null && ( b[i].model == cl_mod_heatbeam ) && ( b[i].entity == Globals.cl.playernum + 1 ) )
				{
					len = Math3D.VectorLength( dist );
					Math3D.VectorScale( f, len, dist );
					Math3D.VectorMA( dist, ( hand_multiplier * b[i].offset[0] ), r, dist );
					Math3D.VectorMA( dist, b[i].offset[1], f, dist );
					Math3D.VectorMA( dist, b[i].offset[2], u, dist );
					if ( ( Globals.hand != null ) && ( Globals.hand.value == 2 ) )
					{
						Math3D.VectorMA( org, -1, Globals.cl.v_up, org );
					}
				}

				if ( dist[1] == 0 && dist[0] == 0 )
				{
					yaw = 0;
					if ( dist[2] > 0 )
						pitch = 90;
					else
						pitch = 270;
				}
				else
				{
					if ( dist[0] != 0F )
						yaw = ( Single ) ( Math.Atan2( dist[1], dist[0] ) * 180 / Math.PI );
					else if ( dist[1] > 0 )
						yaw = 90;
					else
						yaw = 270;
					if ( yaw < 0 )
						yaw += 360;
					forward = ( Single ) Math.Sqrt( dist[0] * dist[0] + dist[1] * dist[1] );
					pitch = ( Single ) ( Math.Atan2( dist[2], forward ) * -180 / Math.PI );
					if ( pitch < 0 )
						pitch += 360;
				}

				if ( cl_mod_heatbeam != null && ( b[i].model == cl_mod_heatbeam ) )
				{
					if ( b[i].entity != Globals.cl.playernum + 1 )
					{
						framenum = 2;
						ent.angles[0] = -pitch;
						ent.angles[1] = yaw + 180F;
						ent.angles[2] = 0;
						Math3D.AngleVectors( ent.angles, f, r, u );
						if ( !Math3D.VectorEquals( b[i].offset, Globals.vec3_origin ) )
						{
							Math3D.VectorMA( org, -( b[i].offset[0] ) + 1, r, org );
							Math3D.VectorMA( org, -( b[i].offset[1] ), f, org );
							Math3D.VectorMA( org, -( b[i].offset[2] ) - 10, u, org );
						}
						else
						{
							CL_newfx.MonsterPlasma_Shell( b[i].start );
						}
					}
					else
					{
						framenum = 1;
					}
				}

				if ( ( cl_mod_heatbeam != null && ( b[i].model == cl_mod_heatbeam ) && ( b[i].entity == Globals.cl.playernum + 1 ) ) )
				{
					CL_newfx.Heatbeam( org, dist );
				}

				d = Math3D.VectorNormalize( dist );
				ent.Clear();
				if ( b[i].model == cl_mod_heatbeam )
				{
					model_length = 32F;
				}
				else if ( b[i].model == cl_mod_lightning )
				{
					model_length = 35F;
					d -= 20;
				}
				else
				{
					model_length = 30F;
				}

				steps = ( Single ) Math.Ceiling( d / model_length );
				len = ( d - model_length ) / ( steps - 1 );
				if ( ( b[i].model == cl_mod_lightning ) && ( d <= model_length ) )
				{
					Math3D.VectorCopy( b[i].end, ent.origin );
					ent.model = b[i].model;
					ent.flags = Defines.RF_FULLBRIGHT;
					ent.angles[0] = pitch;
					ent.angles[1] = yaw;
					ent.angles[2] = Lib.Rand() % 360;
					V.AddEntity( ent );
					return;
				}

				while ( d > 0 )
				{
					Math3D.VectorCopy( org, ent.origin );
					ent.model = b[i].model;
					if ( cl_mod_heatbeam != null && ( b[i].model == cl_mod_heatbeam ) )
					{
						ent.flags = Defines.RF_FULLBRIGHT;
						ent.angles[0] = -pitch;
						ent.angles[1] = yaw + 180F;
						ent.angles[2] = ( Globals.cl.time ) % 360;
						ent.frame = framenum;
					}
					else if ( b[i].model == cl_mod_lightning )
					{
						ent.flags = Defines.RF_FULLBRIGHT;
						ent.angles[0] = -pitch;
						ent.angles[1] = yaw + 180F;
						ent.angles[2] = Lib.Rand() % 360;
					}
					else
					{
						ent.angles[0] = pitch;
						ent.angles[1] = yaw;
						ent.angles[2] = Lib.Rand() % 360;
					}

					V.AddEntity( ent );
					for ( var j = 0; j < 3; j++ )
						org[j] += dist[j] * len;
					d -= model_length;
				}
			}
		}

		static void AddExplosions( )
		{
			entity_t ent;
			Int32 i;
			explosion_t[] ex;
			Single frac;
			Int32 f;
			ent = null;
			ex = cl_explosions;
			for ( i = 0; i < MAX_EXPLOSIONS; i++ )
			{
				if ( ex[i].type == ex_free )
					continue;
				frac = ( Globals.cl.time - ex[i].start ) / 100F;
				f = ( Int32 ) Math.Floor( frac );
				ent = ex[i].ent;
				switch ( ex[i].type )

				{
					case ex_mflash:
						if ( f >= ex[i].frames - 1 )
							ex[i].type = ex_free;
						break;
					case ex_misc:
						if ( f >= ex[i].frames - 1 )
						{
							ex[i].type = ex_free;
							break;
						}

						ent.alpha = 1F - frac / ( ex[i].frames - 1 );
						break;
					case ex_flash:
						if ( f >= 1 )
						{
							ex[i].type = ex_free;
							break;
						}

						ent.alpha = 1F;
						break;
					case ex_poly:
						if ( f >= ex[i].frames - 1 )
						{
							ex[i].type = ex_free;
							break;
						}

						ent.alpha = ( 16F - ( Single ) f ) / 16F;
						if ( f < 10 )
						{
							ent.skinnum = ( f >> 1 );
							if ( ent.skinnum < 0 )
								ent.skinnum = 0;
						}
						else
						{
							ent.flags |= Defines.RF_TRANSLUCENT;
							if ( f < 13 )
								ent.skinnum = 5;
							else
								ent.skinnum = 6;
						}

						break;
					case ex_poly2:
						if ( f >= ex[i].frames - 1 )
						{
							ex[i].type = ex_free;
							break;
						}

						ent.alpha = ( 5F - ( Single ) f ) / 5F;
						ent.skinnum = 0;
						ent.flags |= Defines.RF_TRANSLUCENT;
						break;
				}

				if ( ex[i].type == ex_free )
					continue;
				if ( ex[i].light != 0F )
				{
					V.AddLight( ent.origin, ex[i].light * ent.alpha, ex[i].lightcolor[0], ex[i].lightcolor[1], ex[i].lightcolor[2] );
				}

				Math3D.VectorCopy( ent.origin, ent.oldorigin );
				if ( f < 0 )
					f = 0;
				ent.frame = ex[i].baseframe + f + 1;
				ent.oldframe = ex[i].baseframe + f;
				ent.backlerp = 1F - Globals.cl.lerpfrac;
				V.AddEntity( ent );
			}
		}

		static void AddLasers( )
		{
			laser_t[] l;
			Int32 i;
			l = cl_lasers;
			for ( i = 0; i < MAX_LASERS; i++ )
			{
				if ( l[i].endtime >= Globals.cl.time )
					V.AddEntity( l[i].ent );
			}
		}

		static void ProcessSustain( )
		{
			cl_sustain_t[] s;
			Int32 i;
			s = cl_sustains;
			for ( i = 0; i < MAX_SUSTAINS; i++ )
			{
				if ( s[i].id != 0 )
					if ( ( s[i].endtime >= Globals.cl.time ) && ( Globals.cl.time >= s[i].nextthink ) )
					{
						s[i].think.Think( s[i] );
					}
					else if ( s[i].endtime < Globals.cl.time )
						s[i].id = 0;
			}
		}

		public static void AddTEnts( )
		{
			AddBeams();
			AddPlayerBeams();
			AddExplosions();
			AddLasers();
			ProcessSustain();
		}
	}
}
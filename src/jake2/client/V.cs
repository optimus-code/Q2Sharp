using J2N.IO;
using Q2Sharp.Game;
using Q2Sharp.Qcommon;
using Q2Sharp.Sys;
using Q2Sharp.Util;
using System;

namespace Q2Sharp.Client
{
	public sealed class V : Globals
	{
		static cvar_t cl_testblend;
		static cvar_t cl_testparticles;
		static cvar_t cl_testentities;
		static cvar_t cl_testlights;
		static cvar_t cl_stats;
		static Int32 r_numdlights;
		static dlight_t[] r_dlights = new dlight_t[MAX_DLIGHTS];
		static Int32 r_numentities;
		static entity_t[] r_entities = new entity_t[MAX_ENTITIES];
		static Int32 r_numparticles;
		static lightstyle_t[] r_lightstyles = new lightstyle_t[MAX_LIGHTSTYLES];
		static V( )
		{
			for ( var i = 0; i < r_dlights.Length; i++ )
				r_dlights[i] = new dlight_t();
			for ( var i = 0; i < r_entities.Length; i++ )
				r_entities[i] = new entity_t();
			for ( var i = 0; i < r_lightstyles.Length; i++ )
				r_lightstyles[i] = new lightstyle_t();
		}

		static void ClearScene( )
		{
			r_numdlights = 0;
			r_numentities = 0;
			r_numparticles = 0;
		}

		public static void AddEntity( entity_t ent )
		{
			if ( r_numentities >= MAX_ENTITIES )
				return;
			r_entities[r_numentities++].Set( ent );
		}

		public static void AddParticle( Single[] org, Int32 color, Single alpha )
		{
			if ( r_numparticles >= MAX_PARTICLES )
				return;
			var i = r_numparticles++;
			var c = particle_t.colorTable[color];
			c |= ( Int32 ) ( alpha * 255 ) << 24;
			particle_t.colorArray.Put( i, c );
			i *= 3;
			SingleBuffer vertexBuf = particle_t.vertexArray;
			vertexBuf.Put( i++, org[0] );
			vertexBuf.Put( i++, org[1] );
			vertexBuf.Put( i++, org[2] );
		}

		public static void AddLight( Single[] org, Single intensity, Single r, Single g, Single b )
		{
			dlight_t dl;
			if ( r_numdlights >= MAX_DLIGHTS )
				return;
			dl = r_dlights[r_numdlights++];
			Math3D.VectorCopy( org, dl.origin );
			dl.intensity = intensity;
			dl.color[0] = r;
			dl.color[1] = g;
			dl.color[2] = b;
		}

		public static void AddLightStyle( Int32 style, Single r, Single g, Single b )
		{
			lightstyle_t ls;
			if ( style < 0 || style > MAX_LIGHTSTYLES )
				Com.Error( ERR_DROP, "Bad light style " + style );
			ls = r_lightstyles[style];
			ls.white = r + g + b;
			ls.rgb[0] = r;
			ls.rgb[1] = g;
			ls.rgb[2] = b;
		}

		private static readonly Single[] origin = new Single[] { 0, 0, 0 };
		static void TestParticles( )
		{
			Int32 i, j;
			Single d, r, u;
			r_numparticles = 0;
			for ( i = 0; i < MAX_PARTICLES; i++ )
			{
				d = i * 0.25F;
				r = 4 * ( ( i & 7 ) - 3.5F );
				u = 4 * ( ( ( i >> 3 ) & 7 ) - 3.5F );
				for ( j = 0; j < 3; j++ )
					origin[j] = cl.refdef.vieworg[j] + cl.v_forward[j] * d + cl.v_right[j] * r + cl.v_up[j] * u;
				AddParticle( origin, 8, cl_testparticles.value );
			}
		}

		static void TestEntities( )
		{
			Int32 i, j;
			Single f, r;
			entity_t ent;
			r_numentities = 32;
			for ( i = 0; i < r_entities.Length; i++ )
				r_entities[i].Clear();
			for ( i = 0; i < r_numentities; i++ )
			{
				ent = r_entities[i];
				r = 64 * ( ( i % 4 ) - 1.5F );
				f = 64 * ( i / 4 ) + 128;
				for ( j = 0; j < 3; j++ )
					ent.origin[j] = cl.refdef.vieworg[j] + cl.v_forward[j] * f + cl.v_right[j] * r;
				ent.model = cl.baseclientinfo.model;
				ent.skin = cl.baseclientinfo.skin;
			}
		}

		static void TestLights( )
		{
			Int32 i, j;
			Single f, r;
			dlight_t dl;
			r_numdlights = 32;
			for ( i = 0; i < r_dlights.Length; i++ )
				r_dlights[i] = new dlight_t();
			for ( i = 0; i < r_numdlights; i++ )
			{
				dl = r_dlights[i];
				r = 64 * ( ( i % 4 ) - 1.5F );
				f = 64 * ( i / 4 ) + 128;
				for ( j = 0; j < 3; j++ )
					dl.origin[j] = cl.refdef.vieworg[j] + cl.v_forward[j] * f + cl.v_right[j] * r;
				dl.color[0] = ( ( i % 6 ) + 1 ) & 1;
				dl.color[1] = ( ( ( i % 6 ) + 1 ) & 2 ) >> 1;
				dl.color[2] = ( ( ( i % 6 ) + 1 ) & 4 ) >> 2;
				dl.intensity = 200;
			}
		}

		static xcommand_t Gun_Next_f = new Anonymousxcommand_t();
		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public override void Execute( )
			{
				gun_frame++;
				Com.Printf( "frame " + gun_frame + "\\n" );
			}
		}

		static xcommand_t Gun_Prev_f = new Anonymousxcommand_t1();
		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			public override void Execute( )
			{
				gun_frame--;
				if ( gun_frame < 0 )
					gun_frame = 0;
				Com.Printf( "frame " + gun_frame + "\\n" );
			}
		}

		static xcommand_t Gun_Model_f = new Anonymousxcommand_t2();
		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			public override void Execute( )
			{
				if ( Cmd.Argc() != 2 )
				{
					gun_model = null;
					return;
				}

				var name = "models/" + Cmd.Argv( 1 ) + "/tris.md2";
				gun_model = re.RegisterModel( name );
			}
		}

		public static void RenderView( Single stereo_separation )
		{
			if ( cls.state != ca_active )
				return;
			if ( !cl.refresh_prepped )
				return;
			if ( cl_timedemo.value != 0F )
			{
				if ( cl.timedemo_start == 0 )
					cl.timedemo_start = Timer.Milliseconds();
				cl.timedemo_frames++;
			}

			if ( cl.frame.valid && ( cl.force_refdef || cl_paused.value == 0F ) )
			{
				cl.force_refdef = false;
				V.ClearScene();
				CL_ents.AddEntities();
				if ( cl_testparticles.value != 0F )
					TestParticles();
				if ( cl_testentities.value != 0F )
					TestEntities();
				if ( cl_testlights.value != 0F )
					TestLights();
				if ( cl_testblend.value != 0F )
				{
					cl.refdef.blend[0] = 1F;
					cl.refdef.blend[1] = 0.5F;
					cl.refdef.blend[2] = 0.25F;
					cl.refdef.blend[3] = 0.5F;
				}

				if ( stereo_separation != 0 )
				{
					Single[] tmp = new Single[3];
					Math3D.VectorScale( cl.v_right, stereo_separation, tmp );
					Math3D.VectorAdd( cl.refdef.vieworg, tmp, cl.refdef.vieworg );
				}

				cl.refdef.vieworg[0] += 1 / 16;
				cl.refdef.vieworg[1] += 1 / 16;
				cl.refdef.vieworg[2] += 1 / 16;
				cl.refdef.x = scr_vrect.x;
				cl.refdef.y = scr_vrect.y;
				cl.refdef.width = scr_vrect.width;
				cl.refdef.height = scr_vrect.height;
				cl.refdef.fov_y = Math3D.CalcFov( cl.refdef.fov_x, cl.refdef.width, cl.refdef.height );
				cl.refdef.time = cl.time * 0.001F;
				cl.refdef.areabits = cl.frame.areabits;
				if ( cl_add_entities.value == 0F )
					r_numentities = 0;
				if ( cl_add_particles.value == 0F )
					r_numparticles = 0;
				if ( cl_add_lights.value == 0F )
					r_numdlights = 0;
				if ( cl_add_blend.value == 0 )
				{
					Math3D.VectorClear( cl.refdef.blend );
				}

				cl.refdef.num_entities = r_numentities;
				cl.refdef.entities = r_entities;
				cl.refdef.num_particles = r_numparticles;
				cl.refdef.num_dlights = r_numdlights;
				cl.refdef.dlights = r_dlights;
				cl.refdef.lightstyles = r_lightstyles;
				cl.refdef.rdflags = cl.frame.playerstate.rdflags;
			}

			re.RenderFrame( cl.refdef );
			if ( cl_stats.value != 0F )
				Com.Printf( "ent:%i  lt:%i  part:%i\\n", r_numentities, r_numdlights, r_numparticles );
			if ( log_stats.value != 0F && ( log_stats_file != null ) )
				try
				{
					log_stats_file.Write( r_numentities + "," + r_numdlights + "," + r_numparticles );
				}
				catch ( Exception e )
				{
				}

			SCR.AddDirtyPoint( scr_vrect.x, scr_vrect.y );
			SCR.AddDirtyPoint( scr_vrect.x + scr_vrect.width - 1, scr_vrect.y + scr_vrect.height - 1 );
			SCR.DrawCrosshair();
		}

		static xcommand_t Viewpos_f = new Anonymousxcommand_t3();
		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			private readonly V parent;
			public override void Execute( )
			{
				Com.Printf( "(%i %i %i) : %i\\n", ( Int32 ) cl.refdef.vieworg[0], ( Int32 ) cl.refdef.vieworg[1], ( Int32 ) cl.refdef.vieworg[2], ( Int32 ) cl.refdef.viewangles[YAW] );
			}
		}

		public static void Init( )
		{
			Cmd.AddCommand( "gun_next", Gun_Next_f );
			Cmd.AddCommand( "gun_prev", Gun_Prev_f );
			Cmd.AddCommand( "gun_model", Gun_Model_f );
			Cmd.AddCommand( "viewpos", Viewpos_f );
			crosshair = Cvar.Get( "crosshair", "0", CVAR_ARCHIVE );
			cl_testblend = Cvar.Get( "cl_testblend", "0", 0 );
			cl_testparticles = Cvar.Get( "cl_testparticles", "0", 0 );
			cl_testentities = Cvar.Get( "cl_testentities", "0", 0 );
			cl_testlights = Cvar.Get( "cl_testlights", "0", 0 );
			cl_stats = Cvar.Get( "cl_stats", "0", 0 );
		}
	}
}
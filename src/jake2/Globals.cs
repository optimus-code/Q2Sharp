using Jake2.Client;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Render;
using Jake2.Util;
using System;

namespace Jake2
{
	public class Globals : Defines
	{
		public static readonly String __DATE__ = "2021";
		public static readonly Single VERSION = 3.21F;
		public static readonly String BASEDIRNAME = "baseq2";
		public static Int32 curtime = 0;
		public static Boolean cmd_wait;
		public static Int32 alias_count;
		public static Int32 c_traces;
		public static Int32 c_brush_traces;
		public static Int32 c_pointcontents;
		public static Int32 server_state;
		public static cvar_t cl_add_blend;
		public static cvar_t cl_add_entities;
		public static cvar_t cl_add_lights;
		public static cvar_t cl_add_particles;
		public static cvar_t cl_anglespeedkey;
		public static cvar_t cl_autoskins;
		public static cvar_t cl_footsteps;
		public static cvar_t cl_forwardspeed;
		public static cvar_t cl_gun;
		public static cvar_t cl_maxfps;
		public static cvar_t cl_noskins;
		public static cvar_t cl_pitchspeed;
		public static cvar_t cl_predict;
		public static cvar_t cl_run;
		public static cvar_t cl_sidespeed;
		public static cvar_t cl_stereo;
		public static cvar_t cl_stereo_separation;
		public static cvar_t cl_timedemo = new cvar_t();
		public static cvar_t cl_timeout;
		public static cvar_t cl_upspeed;
		public static cvar_t cl_yawspeed;
		public static cvar_t dedicated;
		public static cvar_t developer;
		public static cvar_t fixedtime;
		public static cvar_t freelook;
		public static cvar_t host_speeds;
		public static cvar_t log_stats;
		public static cvar_t logfile_active;
		public static cvar_t lookspring;
		public static cvar_t lookstrafe;
		public static cvar_t nostdout;
		public static cvar_t sensitivity;
		public static cvar_t showtrace;
		public static cvar_t timescale;
		public static cvar_t in_mouse;
		public static cvar_t in_joystick;
		public static sizebuf_t net_message = new sizebuf_t();
		public static sizebuf_t cmd_text = new sizebuf_t();
		public static Byte[] defer_text_buf = new Byte[8192];
		public static Byte[] cmd_text_buf = new Byte[8192];
		public static cmdalias_t cmd_alias;
		public static Byte[] net_message_buffer = new Byte[MAX_MSGLEN];
		public static Int32 time_before_game;
		public static Int32 time_after_game;
		public static Int32 time_before_ref;
		public static Int32 time_after_ref;
		public static QuakeFile log_stats_file = null;
		public static cvar_t m_pitch;
		public static cvar_t m_yaw;
		public static cvar_t m_forward;
		public static cvar_t m_side;
		public static cvar_t cl_lightlevel;
		public static cvar_t info_password;
		public static cvar_t info_spectator;
		public static cvar_t name;
		public static cvar_t skin;
		public static cvar_t rate;
		public static cvar_t fov;
		public static cvar_t msg;
		public static cvar_t hand;
		public static cvar_t gender;
		public static cvar_t gender_auto;
		public static cvar_t cl_vwep;
		public static client_static_t cls = new client_static_t();
		public static client_state_t cl = new client_state_t();
		public static centity_t[] cl_entities = new centity_t[Defines.MAX_EDICTS];
		public static entity_state_t[] cl_parse_entities = new entity_state_t[Defines.MAX_PARSE_ENTITIES];
		static Globals( )
		{
			for ( var i = 0; i < cl_entities.Length; i++ )
			{
				cl_entities[i] = new centity_t();
			}

			for ( var i = 0; i < cl_parse_entities.Length; i++ )
			{
				cl_parse_entities[i] = new entity_state_t( null );
			}

			for ( var i = 0; i < key_lines.Length; i++ )
				key_lines[i] = new Byte[Defines.MAXCMDLINE];
		}

		public static cvar_t rcon_client_password;
		public static cvar_t rcon_address;
		public static cvar_t cl_shownet;
		public static cvar_t cl_showmiss;
		public static cvar_t cl_showclamp;
		public static cvar_t cl_paused;
		public static readonly Single[][] bytedirs = new Single[][] { new[] { -0.525731F, 0F, 0.850651F }, new[] { -0.442863F, 0.238856F, 0.864188F }, new[] { -0.295242F, 0F, 0.955423F }, new[] { -0.309017F, 0.5F, 0.809017F }, new[] { -0.16246F, 0.262866F, 0.951056F }, new[] { 0F, 0F, 1F }, new[] { 0F, 0.850651F, 0.525731F }, new[] { -0.147621F, 0.716567F, 0.681718F }, new[] { 0.147621F, 0.716567F, 0.681718F }, new[] { 0F, 0.525731F, 0.850651F }, new[] { 0.309017F, 0.5F, 0.809017F }, new[] { 0.525731F, 0F, 0.850651F }, new[] { 0.295242F, 0F, 0.955423F }, new[] { 0.442863F, 0.238856F, 0.864188F }, new[] { 0.16246F, 0.262866F, 0.951056F }, new[] { -0.681718F, 0.147621F, 0.716567F }, new[] { -0.809017F, 0.309017F, 0.5F }, new[] { -0.587785F, 0.425325F, 0.688191F }, new[] { -0.850651F, 0.525731F, 0F }, new[] { -0.864188F, 0.442863F, 0.238856F }, new[] { -0.716567F, 0.681718F, 0.147621F }, new[] { -0.688191F, 0.587785F, 0.425325F }, new[] { -0.5F, 0.809017F, 0.309017F }, new[] { -0.238856F, 0.864188F, 0.442863F }, new[] { -0.425325F, 0.688191F, 0.587785F }, new[] { -0.716567F, 0.681718F, -0.147621F }, new[] { -0.5F, 0.809017F, -0.309017F }, new[] { -0.525731F, 0.850651F, 0F }, new[] { 0F, 0.850651F, -0.525731F }, new[] { -0.238856F, 0.864188F, -0.442863F }, new[] { 0F, 0.955423F, -0.295242F }, new[] { -0.262866F, 0.951056F, -0.16246F }, new[] { 0F, 1F, 0F }, new[] { 0F, 0.955423F, 0.295242F }, new[] { -0.262866F, 0.951056F, 0.16246F }, new[] { 0.238856F, 0.864188F, 0.442863F }, new[] { 0.262866F, 0.951056F, 0.16246F }, new[] { 0.5F, 0.809017F, 0.309017F }, new[] { 0.238856F, 0.864188F, -0.442863F }, new[] { 0.262866F, 0.951056F, -0.16246F }, new[] { 0.5F, 0.809017F, -0.309017F }, new[] { 0.850651F, 0.525731F, 0F }, new[] { 0.716567F, 0.681718F, 0.147621F }, new[] { 0.716567F, 0.681718F, -0.147621F }, new[] { 0.525731F, 0.850651F, 0F }, new[] { 0.425325F, 0.688191F, 0.587785F }, new[] { 0.864188F, 0.442863F, 0.238856F }, new[] { 0.688191F, 0.587785F, 0.425325F }, new[] { 0.809017F, 0.309017F, 0.5F }, new[] { 0.681718F, 0.147621F, 0.716567F }, new[] { 0.587785F, 0.425325F, 0.688191F }, new[] { 0.955423F, 0.295242F, 0F }, new[] { 1F, 0F, 0F }, new[] { 0.951056F, 0.16246F, 0.262866F }, new[] { 0.850651F, -0.525731F, 0F }, new[] { 0.955423F, -0.295242F, 0F }, new[] { 0.864188F, -0.442863F, 0.238856F }, new[] { 0.951056F, -0.16246F, 0.262866F }, new[] { 0.809017F, -0.309017F, 0.5F }, new[] { 0.681718F, -0.147621F, 0.716567F }, new[] { 0.850651F, 0F, 0.525731F }, new[] { 0.864188F, 0.442863F, -0.238856F }, new[] { 0.809017F, 0.309017F, -0.5F }, new[] { 0.951056F, 0.16246F, -0.262866F }, new[] { 0.525731F, 0F, -0.850651F }, new[] { 0.681718F, 0.147621F, -0.716567F }, new[] { 0.681718F, -0.147621F, -0.716567F }, new[] { 0.850651F, 0F, -0.525731F }, new[] { 0.809017F, -0.309017F, -0.5F }, new[] { 0.864188F, -0.442863F, -0.238856F }, new[] { 0.951056F, -0.16246F, -0.262866F }, new[] { 0.147621F, 0.716567F, -0.681718F }, new[] { 0.309017F, 0.5F, -0.809017F }, new[] { 0.425325F, 0.688191F, -0.587785F }, new[] { 0.442863F, 0.238856F, -0.864188F }, new[] { 0.587785F, 0.425325F, -0.688191F }, new[] { 0.688191F, 0.587785F, -0.425325F }, new[] { -0.147621F, 0.716567F, -0.681718F }, new[] { -0.309017F, 0.5F, -0.809017F }, new[] { 0F, 0.525731F, -0.850651F }, new[] { -0.525731F, 0F, -0.850651F }, new[] { -0.442863F, 0.238856F, -0.864188F }, new[] { -0.295242F, 0F, -0.955423F }, new[] { -0.16246F, 0.262866F, -0.951056F }, new[] { 0F, 0F, -1F }, new[] { 0.295242F, 0F, -0.955423F }, new[] { 0.16246F, 0.262866F, -0.951056F }, new[] { -0.442863F, -0.238856F, -0.864188F }, new[] { -0.309017F, -0.5F, -0.809017F }, new[] { -0.16246F, -0.262866F, -0.951056F }, new[] { 0F, -0.850651F, -0.525731F }, new[] { -0.147621F, -0.716567F, -0.681718F }, new[] { 0.147621F, -0.716567F, -0.681718F }, new[] { 0F, -0.525731F, -0.850651F }, new[] { 0.309017F, -0.5F, -0.809017F }, new[] { 0.442863F, -0.238856F, -0.864188F }, new[] { 0.16246F, -0.262866F, -0.951056F }, new[] { 0.238856F, -0.864188F, -0.442863F }, new[] { 0.5F, -0.809017F, -0.309017F }, new[] { 0.425325F, -0.688191F, -0.587785F }, new[] { 0.716567F, -0.681718F, -0.147621F }, new[] { 0.688191F, -0.587785F, -0.425325F }, new[] { 0.587785F, -0.425325F, -0.688191F }, new[] { 0F, -0.955423F, -0.295242F }, new[] { 0F, -1F, 0F }, new[] { 0.262866F, -0.951056F, -0.16246F }, new[] { 0F, -0.850651F, 0.525731F }, new[] { 0F, -0.955423F, 0.295242F }, new[] { 0.238856F, -0.864188F, 0.442863F }, new[] { 0.262866F, -0.951056F, 0.16246F }, new[] { 0.5F, -0.809017F, 0.309017F }, new[] { 0.716567F, -0.681718F, 0.147621F }, new[] { 0.525731F, -0.850651F, 0F }, new[] { -0.238856F, -0.864188F, -0.442863F }, new[] { -0.5F, -0.809017F, -0.309017F }, new[] { -0.262866F, -0.951056F, -0.16246F }, new[] { -0.850651F, -0.525731F, 0F }, new[] { -0.716567F, -0.681718F, -0.147621F }, new[] { -0.716567F, -0.681718F, 0.147621F }, new[] { -0.525731F, -0.850651F, 0F }, new[] { -0.5F, -0.809017F, 0.309017F }, new[] { -0.238856F, -0.864188F, 0.442863F }, new[] { -0.262866F, -0.951056F, 0.16246F }, new[] { -0.864188F, -0.442863F, 0.238856F }, new[] { -0.809017F, -0.309017F, 0.5F }, new[] { -0.688191F, -0.587785F, 0.425325F }, new[] { -0.681718F, -0.147621F, 0.716567F }, new[] { -0.442863F, -0.238856F, 0.864188F }, new[] { -0.587785F, -0.425325F, 0.688191F }, new[] { -0.309017F, -0.5F, 0.809017F }, new[] { -0.147621F, -0.716567F, 0.681718F }, new[] { -0.425325F, -0.688191F, 0.587785F }, new[] { -0.16246F, -0.262866F, 0.951056F }, new[] { 0.442863F, -0.238856F, 0.864188F }, new[] { 0.16246F, -0.262866F, 0.951056F }, new[] { 0.309017F, -0.5F, 0.809017F }, new[] { 0.147621F, -0.716567F, 0.681718F }, new[] { 0F, -0.525731F, 0.850651F }, new[] { 0.425325F, -0.688191F, 0.587785F }, new[] { 0.587785F, -0.425325F, 0.688191F }, new[] { 0.688191F, -0.587785F, 0.425325F }, new[] { -0.955423F, 0.295242F, 0F }, new[] { -0.951056F, 0.16246F, 0.262866F }, new[] { -1F, 0F, 0F }, new[] { -0.850651F, 0F, 0.525731F }, new[] { -0.955423F, -0.295242F, 0F }, new[] { -0.951056F, -0.16246F, 0.262866F }, new[] { -0.864188F, 0.442863F, -0.238856F }, new[] { -0.951056F, 0.16246F, -0.262866F }, new[] { -0.809017F, 0.309017F, -0.5F }, new[] { -0.864188F, -0.442863F, -0.238856F }, new[] { -0.951056F, -0.16246F, -0.262866F }, new[] { -0.809017F, -0.309017F, -0.5F }, new[] { -0.681718F, 0.147621F, -0.716567F }, new[] { -0.681718F, -0.147621F, -0.716567F }, new[] { -0.850651F, 0F, -0.525731F }, new[] { -0.688191F, 0.587785F, -0.425325F }, new[] { -0.587785F, 0.425325F, -0.688191F }, new[] { -0.425325F, 0.688191F, -0.587785F }, new[] { -0.425325F, -0.688191F, -0.587785F }, new[] { -0.587785F, -0.425325F, -0.688191F }, new[] { -0.688191F, -0.587785F, -0.425325F } };
		public static Boolean userinfo_modified = false;
		public static cvar_t cvar_vars;
		public static readonly console_t con = new console_t();
		public static cvar_t con_notifytime;
		public static viddef_t viddef = new viddef_t();
		public static Irefexport_t re = new DummyRenderer();
		public static String[] keybindings = new String[256];
		public static Boolean[] keydown = new Boolean[256];
		public static Boolean chat_team = false;
		public static String chat_buffer = "";
		public static Byte[][] key_lines = Lib.CreateJaggedArray<Byte[][]>( 32, 1 );
		public static Int32 key_linepos;

		public static Int32 edit_line;
		public static cvar_t crosshair;
		public static vrect_t scr_vrect = new vrect_t();
		public static Int32 sys_frame_time;
		public static Int32 chat_bufferlen = 0;
		public static Int32 gun_frame;
		public static model_t gun_model;
		public static netadr_t net_from = new netadr_t();
		public static QuakeFile logfile = null;
		public static Single[] vec3_origin = new[] { 0F, 0F, 0F };
		public static cvar_t m_filter;
		public static Int32 vidref_val = VIDREF_GL;
		public static Random rnd = new Random();
		public static Boolean appletMode;
		public static Object applet;
	}
}
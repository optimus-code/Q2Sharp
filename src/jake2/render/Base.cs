using Q2Sharp.Client;
using Q2Sharp.Game;
using Q2Sharp.Render.Opengl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Q2Sharp.Render
{
    public abstract class Base : IRenderAPI
    {
        public const PixelFormat GL_COLOR_INDEX8_EXT = PixelFormat.ColorIndex;
        public const string REF_VERSION = "GL 0.01";
        public const int PITCH = 0;
        public const int YAW = 1;
        public const int ROLL = 2;
        public const int it_skin = 0;
        public const int it_sprite = 1;
        public const int it_wall = 2;
        public const int it_pic = 3;
        public const int it_sky = 4;
        public const int mod_bad = 0;
        public const int mod_brush = 1;
        public const int mod_sprite = 2;
        public const int mod_alias = 3;
        public const int TEXNUM_LIGHTMAPS = 1024;
        public const int TEXNUM_SCRAPS = 1152;
        public const int TEXNUM_IMAGES = 1153;
        public const int MAX_GLTEXTURES = 1024;
        public const int MAX_LBM_HEIGHT = 480;
        public const float BACKFACE_EPSILON = 0.01F;
        public const int GL_RENDERER_VOODOO = 0x00000001;
        public const int GL_RENDERER_VOODOO2 = 0x00000002;
        public const int GL_RENDERER_VOODOO_RUSH = 0x00000004;
        public const int GL_RENDERER_BANSHEE = 0x00000008;
        public const int GL_RENDERER_3DFX = 0x0000000F;
        public const int GL_RENDERER_PCX1 = 0x00000010;
        public const int GL_RENDERER_PCX2 = 0x00000020;
        public const int GL_RENDERER_PMX = 0x00000040;
        public const int GL_RENDERER_POWERVR = 0x00000070;
        public const int GL_RENDERER_PERMEDIA2 = 0x00000100;
        public const int GL_RENDERER_GLINT_MX = 0x00000200;
        public const int GL_RENDERER_GLINT_TX = 0x00000400;
        public const int GL_RENDERER_3DLABS_MISC = 0x00000800;
        public const int GL_RENDERER_3DLABS = 0x00000F00;
        public const int GL_RENDERER_REALIZM = 0x00001000;
        public const int GL_RENDERER_REALIZM2 = 0x00002000;
        public const int GL_RENDERER_INTERGRAPH = 0x00003000;
        public const int GL_RENDERER_3DPRO = 0x00004000;
        public const int GL_RENDERER_REAL3D = 0x00008000;
        public const int GL_RENDERER_RIVA128 = 0x00010000;
        public const int GL_RENDERER_DYPIC = 0x00020000;
        public const int GL_RENDERER_V1000 = 0x00040000;
        public const int GL_RENDERER_V2100 = 0x00080000;
        public const int GL_RENDERER_V2200 = 0x00100000;
        public const int GL_RENDERER_RENDITION = 0x001C0000;
        public const int GL_RENDERER_O2 = 0x00100000;
        public const int GL_RENDERER_IMPACT = 0x00200000;
        public const int GL_RENDERER_RE = 0x00400000;
        public const int GL_RENDERER_IR = 0x00800000;
        public const int GL_RENDERER_SGI = 0x00F00000;
        public const int GL_RENDERER_MCD = 0x01000000;
        public const uint GL_RENDERER_OTHER = 0x80000000;
        public const int rserr_ok = 0;
        public const int rserr_invalid_fullscreen = 1;
        public const int rserr_invalid_mode = 2;
        public const int rserr_unknown = 3;
        protected static readonly viddef_t vid = new viddef_t();
        protected cvar_t vid_fullscreen;
        protected IGLDriver glImpl;
        public virtual void SetGLDriver( IGLDriver driver )
        {
            glImpl = driver;
        }

        public static void SetVid(int width, int height)
        {
            lock (typeof(Base))
            {
                vid.SetSize(width, height);
            }
        }

		public abstract System.Boolean R_Init( Int32 vid_xpos, Int32 vid_ypos );
		public abstract System.Boolean R_Init2( );
		public abstract void R_Shutdown( );
		public abstract void R_BeginRegistration( String map );
		public abstract model_t R_RegisterModel( String name );
		public abstract image_t R_RegisterSkin( String name );
		public abstract image_t Draw_FindPic( String name );
		public abstract void R_SetSky( String name, Single rotate, Single[] axis );
		public abstract void R_EndRegistration( );
		public abstract void R_RenderFrame( refdef_t fd );
		public abstract void Draw_GetPicSize( out Size dim, String name );
		public abstract void Draw_Pic( Int32 x, Int32 y, String name );
		public abstract void Draw_StretchPic( Int32 x, Int32 y, Int32 w, Int32 h, String name );
		public abstract void Draw_Char( Int32 x, Int32 y, Int32 num );
		public abstract void Draw_TileClear( Int32 x, Int32 y, Int32 w, Int32 h, String name );
		public abstract void Draw_Fill( Int32 x, Int32 y, Int32 w, Int32 h, Int32 c );
		public abstract void Draw_FadeScreen( );
		public abstract void Draw_StretchRaw( Int32 x, Int32 y, Int32 w, Int32 h, Int32 cols, Int32 rows, Byte[] data );
		public abstract void R_SetPalette( Byte[] palette );
		public abstract void R_BeginFrame( Single camera_separation );
		public abstract void GL_ScreenShot_f( );
	}
}
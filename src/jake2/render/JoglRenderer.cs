using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using Q2Sharp.Render.Opengl;
using Q2Sharp.Sys;
using System;
using System.Drawing;

namespace Q2Sharp.Render
{
	sealed class JoglRenderer : JoglDriver, Irefexport_t, IRef
	{
		public static readonly String DRIVER_NAME = "jogl";
		private KBD kbd = new JOGLKBD();
		private IRenderAPI impl;
		static JoglRenderer( )
		{
			Renderer.Register( new JoglRenderer() );
		}

		private JoglRenderer( )
		{
		}

		public override Boolean Init( Int32 vid_xpos, Int32 vid_ypos )
		{
			impl.SetGLDriver( this );
			if ( !impl.R_Init( vid_xpos, vid_ypos ) )
				return false;
			UpdateScreen( new Anonymousxcommand_t( this ) );
			return post_init;
		}

		private sealed class Anonymousxcommand_t : xcommand_t
		{
			public Anonymousxcommand_t( JoglRenderer parent )
			{
				this.parent = parent;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.post_init = parent.impl.R_Init2();
			}
		}

		public override void Shutdown( )
		{
			impl.R_Shutdown();
		}

		public void BeginRegistration( String map )
		{
			if ( contextInUse )
			{
				impl.R_BeginRegistration( map );
			}
			else
			{
				UpdateScreen( new Anonymousxcommand_t1( this, map ) );
			}
		}

		private sealed class Anonymousxcommand_t1 : xcommand_t
		{
			String Map;

			public Anonymousxcommand_t1( JoglRenderer parent, String map )
			{
				this.parent = parent;
				this.Map = map;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.impl.R_BeginRegistration( Map );
			}
		}

		private model_t model = null;
		public model_t RegisterModel( String name )
		{
			if ( contextInUse )
			{
				return impl.R_RegisterModel( name );
			}
			else
			{
				UpdateScreen( new Anonymousxcommand_t2( this, name ) );
				return model;
			}
		}

		private sealed class Anonymousxcommand_t2 : xcommand_t
		{
			private String Name;
			public Anonymousxcommand_t2( JoglRenderer parent, String name )
			{
				this.parent = parent;
				this.Name = name;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.model = parent.impl.R_RegisterModel( Name );
			}
		}

		private image_t image = null;
		public image_t RegisterSkin( String name )
		{
			if ( contextInUse )
			{
				return impl.R_RegisterSkin( name );
			}
			else
			{
				UpdateScreen( new Anonymousxcommand_t3( this, name ) );
				return image;
			}
		}

		private sealed class Anonymousxcommand_t3 : xcommand_t
		{
			private String Name;
			public Anonymousxcommand_t3( JoglRenderer parent, String name )
			{
				this.parent = parent;
				this.Name = name;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.image = parent.impl.R_RegisterSkin( Name );
			}
		}

		public image_t RegisterPic( String name )
		{
			if ( contextInUse )
			{
				return impl.Draw_FindPic( name );
			}
			else
			{
				UpdateScreen( new Anonymousxcommand_t4( this, name ) );
				return image;
			}
		}

		private sealed class Anonymousxcommand_t4 : xcommand_t
		{
			private String Name;
			public Anonymousxcommand_t4( JoglRenderer parent, String name )
			{
				this.parent = parent;
				this.Name = name;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.image = parent.impl.Draw_FindPic( Name );
			}
		}

		public void SetSky( String name, Single rotate, Single[] axis )
		{
			if ( contextInUse )
			{
				impl.R_SetSky( name, rotate, axis );
			}
			else
			{
				UpdateScreen( new Anonymousxcommand_t5( this, name, rotate, axis ) );
			}
		}

		private sealed class Anonymousxcommand_t5 : xcommand_t
		{
			String Name;
			Single Rotate;
			Single[] Axis;

			public Anonymousxcommand_t5( JoglRenderer parent, String name, Single rotate, Single[] axis )
			{
				this.parent = parent;
				this.Name = name;
				this.Rotate = rotate;
				this.Axis = axis;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.impl.R_SetSky( Name, Rotate, Axis );
			}
		}

		public void EndRegistration( )
		{
			if ( contextInUse )
			{
				impl.R_EndRegistration();
			}
			else
			{
				UpdateScreen( new Anonymousxcommand_t6( this ) );
			}
		}

		private sealed class Anonymousxcommand_t6 : xcommand_t
		{
			public Anonymousxcommand_t6( JoglRenderer parent )
			{
				this.parent = parent;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.impl.R_EndRegistration();
			}
		}

		public void RenderFrame( refdef_t fd )
		{
			impl.R_RenderFrame( fd );
		}

		public void DrawGetPicSize( out Size dim, String name )
		{
			impl.Draw_GetPicSize( out dim, name );
		}

		public void DrawPic( Int32 x, Int32 y, String name )
		{
			impl.Draw_Pic( x, y, name );
		}

		public void DrawStretchPic( Int32 x, Int32 y, Int32 w, Int32 h, String name )
		{
			impl.Draw_StretchPic( x, y, w, h, name );
		}

		public void DrawChar( Int32 x, Int32 y, Int32 num )
		{
			if ( contextInUse )
			{
				impl.Draw_Char( x, y, num );
			}
			else
			{
				UpdateScreen( new Anonymousxcommand_t7( this, x, y, num ) );
			}
		}

		private sealed class Anonymousxcommand_t7 : xcommand_t
		{
			Int32 X;
			Int32 Y;
			Int32 Num;

			public Anonymousxcommand_t7( JoglRenderer parent, Int32 x, Int32 y, Int32 num )
			{
				this.parent = parent;
				this.X = x;
				this.Y = y;
				this.Num = num;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.impl.Draw_Char( X, Y, Num );
			}
		}

		public void DrawTileClear( Int32 x, Int32 y, Int32 w, Int32 h, String name )
		{
			impl.Draw_TileClear( x, y, w, h, name );
		}

		public void DrawFill( Int32 x, Int32 y, Int32 w, Int32 h, Int32 c )
		{
			impl.Draw_Fill( x, y, w, h, c );
		}

		public void DrawFadeScreen( )
		{
			impl.Draw_FadeScreen();
		}

		public void DrawStretchRaw( Int32 x, Int32 y, Int32 w, Int32 h, Int32 cols, Int32 rows, Byte[] data )
		{
			impl.Draw_StretchRaw( x, y, w, h, cols, rows, data );
		}

		public void CinematicSetPalette( Byte[] palette )
		{
			impl.R_SetPalette( palette );
		}

		public override void BeginFrame( Single camera_separation )
		{
			impl.R_BeginFrame( camera_separation );
		}

		public override void EndFrame( )
		{
			EndFrame();
		}

		public override void AppActivate( Boolean activate )
		{
			AppActivate( activate );
		}

		public override void Screenshot( )
		{
			if ( contextInUse )
			{
				impl.GL_ScreenShot_f();
			}
			else
			{
				UpdateScreen( new Anonymousxcommand_t8( this ) );
			}
		}

		private sealed class Anonymousxcommand_t8 : xcommand_t
		{
			public Anonymousxcommand_t8( JoglRenderer parent )
			{
				this.parent = parent;
			}

			private readonly JoglRenderer parent;
			public override void Execute( )
			{
				parent.impl.GL_ScreenShot_f();
			}
		}

		public Int32 ApiVersion( )
		{
			return Defines.API_VERSION;
		}

		public KBD GetKeyboardHandler( )
		{
			return kbd;
		}

		public String GetName( )
		{
			return DRIVER_NAME;
		}

		public override String ToString( )
		{
			return DRIVER_NAME;
		}

		public Irefexport_t GetRefAPI( IRenderAPI renderer )
		{
			this.impl = renderer;
			return this;
		}
	}
}
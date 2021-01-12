using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Concurrent;

namespace Q2Sharp.Sys
{
	public sealed class InputListener
	{
		private static ConcurrentQueue<Jake2InputEvent> eventQueue = new ConcurrentQueue<Jake2InputEvent>();
		static void AddEvent( Jake2InputEvent ev )
		{
			eventQueue.Enqueue( ev );
		}

		public static Jake2InputEvent NextEvent( )
		{
			eventQueue.TryDequeue( out var ev );
			return ev;
		}

		public void KeyPressed( KeyboardKeyEventArgs e )
		{
			if ( !( ( e.Modifiers & KeyModifiers.Alt ) != 0 ) )
			{
				AddEvent( new Jake2InputEvent( InputEvent.KeyPress, e, JOGLKBD.c ) );
			}
		}

		public void KeyReleased( KeyboardKeyEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.KeyRelease, e, JOGLKBD.c ) );
		}

		public void KeyTyped( TextInputEventArgs e )
		{
			//if ( !( ( e.Modifiers & KeyModifiers.Alt ) != 0 ) )
			//{
			AddEvent( new Jake2InputEvent( InputEvent.KeyPress, e, JOGLKBD.c ) );
			AddEvent( new Jake2InputEvent( InputEvent.KeyRelease, e, JOGLKBD.c ) );
			//}
		}

		public void MouseClicked( MouseButtonEventArgs e )
		{
		}

		public void MouseEntered( )
		{
		}

		public void MouseExited( )
		{
		}

		public void MousePressed( MouseButtonEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.ButtonPress, e, JOGLKBD.c ) );
		}

		public void MouseReleased( MouseButtonEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.ButtonRelease, e, JOGLKBD.c ) );
		}

		public void MouseDragged( MouseMoveEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.MotionNotify, e, JOGLKBD.c ) );
		}

		public void MouseMoved( MouseMoveEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.MotionNotify, e, JOGLKBD.c ) );
		}

		public void ComponentHidden( MinimizedEventArgs e )
		{
		}

		public void ComponentMoved( WindowPositionEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.ConfigureNotify, e, JOGLKBD.c ) );
		}

		public void ComponentResized( ResizeEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.ConfigureNotify, e, JOGLKBD.c ) );
		}

		public void ComponentShown( MaximizedEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.CreateNotify, e, JOGLKBD.c ) );
		}

		public void MouseWheelMoved( MouseWheelEventArgs e )
		{
			AddEvent( new Jake2InputEvent( InputEvent.WheelMoved, e, JOGLKBD.c ) );
		}
	}
}
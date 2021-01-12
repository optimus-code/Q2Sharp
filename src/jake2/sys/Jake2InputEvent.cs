using OpenTK.Windowing.Desktop;
using System;

namespace Q2Sharp.Sys
{
	public enum InputEvent : Int32
	{
		KeyPress = 0,
		KeyRelease = 1,
		MotionNotify = 2,
		ButtonPress = 3,
		ButtonRelease = 4,
		CreateNotify = 5,
		ConfigureNotify = 6,
		WheelMoved = 7
	}

	public class Jake2InputEvent
	{
		public InputEvent type;
		public Object ev;
		public GameWindow gameWindow;
		public Jake2InputEvent( InputEvent type, Object ev, GameWindow gw )
		{
			this.type = type;
			this.ev = ev;
			this.gameWindow = gw;
		}
	}
}
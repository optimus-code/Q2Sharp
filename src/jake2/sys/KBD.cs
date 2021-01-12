using System;

namespace Q2Sharp.Sys
{
	public abstract class KBD
	{
		public static Int32 win_x = 0;
		public static Int32 win_y = 0;
		public static Int32 mx = 0;
		public static Int32 my = 0;
		public abstract void Init( );
		public abstract void Update( );
		public abstract void Close( );
		public abstract void Do_Key_Event( Int32 key, Boolean down );
		public abstract void InstallGrabs( );
		public abstract void UninstallGrabs( );
	}
}
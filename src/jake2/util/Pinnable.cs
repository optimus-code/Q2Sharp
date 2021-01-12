using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Q2Sharp.util
{
	public class Pinnable
	{
		private GCHandle Handle
		{
			get;
			set;
		}

		public Pinnable( Object value, Action<IntPtr> onPinned )
		{
			try
			{
				Handle = GCHandle.Alloc( value, GCHandleType.Pinned );
				var pointer = Handle.AddrOfPinnedObject();
				onPinned( pointer );
			}
			finally
			{
				Handle.Free();
			}
		}
	}
}

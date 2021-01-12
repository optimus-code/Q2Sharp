using Jake2.Util;
using System;

namespace Jake2.Qcommon
{
	public sealed class sizebuf_t
	{
		public Boolean allowoverflow = false;
		public Boolean overflowed = false;
		public Byte[] data = null;
		public Int32 maxsize = 0;
		public Int32 cursize = 0;
		public Int32 readcount = 0;
		public void Clear( )
		{
			if ( data != null )
				Lib.Fill( data, ( Byte ) 0 );
			cursize = 0;
			overflowed = false;
		}
	}
}
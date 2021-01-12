using Q2Sharp.Util;
using System;

namespace Q2Sharp.Render
{
	public class mtexinfo_t
	{
		public Single[][] vecs = new Single[][] { new Single[] { 0, 0, 0, 0 }, new Single[] { 0, 0, 0, 0 } };
		public Int32 flags;
		public Int32 numframes;
		public mtexinfo_t next;
		public image_t image;
		public virtual void Clear( )
		{
			vecs[0].Fill( 0 );
			vecs[1].Fill( 0 );
			flags = 0;
			numframes = 0;
			next = null;
			image = null;
		}
	}
}
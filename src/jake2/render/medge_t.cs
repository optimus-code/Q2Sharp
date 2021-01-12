using J2N.IO;
using System;

namespace Q2Sharp.Render
{
	public class medge_t
	{
		public static readonly Int32 DISK_SIZE = 2 * Defines.SIZE_OF_SHORT;
		public static readonly Int32 MEM_SIZE = 3 * Defines.SIZE_OF_INT;
		public Int32[] v = new Int32[2];
		public Int32 cachededgeoffset;
		public medge_t( ByteBuffer b )
		{
			v[0] = b.GetInt16() & 0xFFFF;
			v[1] = b.GetInt16() & 0xFFFF;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Qcommon
{
    public class lump_t
    {
        public lump_t(int offset, int len)
        {
            this.fileofs = offset;
            this.filelen = len;
        }

        public int fileofs, filelen;
    }
}
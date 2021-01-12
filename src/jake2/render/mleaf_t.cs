using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render
{
    public class mleaf_t : mnode_t
    {
        public int cluster;
        public int area;
        public int nummarksurfaces;
        public int markIndex;
        public msurface_t[] markSurfaces;
        public virtual void SetMarkSurface(int markIndex, msurface_t[] markSurfaces)
        {
            this.markIndex = markIndex;
            this.markSurfaces = markSurfaces;
        }

        public virtual msurface_t GetMarkSurface(int index)
        {
            return (index < nummarksurfaces) ? markSurfaces[markIndex + index] : null;
        }
    }
}
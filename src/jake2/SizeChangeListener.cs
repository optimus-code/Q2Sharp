using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2
{
    public interface ISizeChangeListener
    {
        void SizeChanged(int width, int height);
    }
}
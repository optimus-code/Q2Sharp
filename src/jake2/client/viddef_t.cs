using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public class viddef_t
    {
        private int width;
        private int height;
        private int newWidth;
        private int newHeight;
        public virtual void SetSize(int width, int height)
        {
            lock (this)
            {
                newWidth = width;
                newHeight = height;
            }
        }

        public virtual void Update()
        {
            lock (this)
            {
                width = newWidth;
                height = newHeight;
            }
        }

        public virtual int GetWidth()
        {
            return width;
        }

        public virtual int GetHeight()
        {
            return height;
        }
    }
}
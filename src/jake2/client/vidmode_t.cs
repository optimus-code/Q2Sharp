using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Client
{
    public class vidmode_t
    {
        public string description;
        public int width, height;
        public int mode;
        public vidmode_t(string description, int width, int height, int mode)
        {
            this.description = description;
            this.width = width;
            this.height = height;
            this.mode = mode;
        }
    }
}
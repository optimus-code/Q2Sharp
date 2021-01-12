using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render
{
    public class glconfig_t
    {
        public int renderer;
        public string renderer_string;
        public string vendor_string;
        public string version_string;
        public string extensions_string;
        public bool allow_cds;
        private float version = 1.1F;
        public virtual void ParseOpenGLVersion()
        {
            try
            {
                version = float.Parse(version_string.Substring(0, 3));
            }
            catch (Exception e)
            {
                version = 1.1F;
            }
        }

        public virtual float GetOpenGLVersion()
        {
            return version;
        }
    }
}
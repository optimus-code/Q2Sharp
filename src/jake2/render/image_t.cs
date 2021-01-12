using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render
{
    public class image_t
    {
        public static readonly int MAX_NAME_SIZE = Defines.MAX_QPATH;
        private int id;
        public string name = "";
        public int type;
        public int width, height;
        public int upload_width, upload_height;
        public int registration_sequence;
        public msurface_t texturechain;
        public int texnum;
        public float sl, tl, sh, th;
        public bool scrap;
        public bool has_alpha;
        public bool paletted;
        public image_t(int id)
        {
            this.id = id;
        }

        public virtual void Clear()
        {
            name = "";
            type = 0;
            width = height = 0;
            upload_width = upload_height = 0;
            registration_sequence = 0;
            texturechain = null;
            texnum = 0;
            sl = tl = sh = th = 0;
            scrap = false;
            has_alpha = false;
            paletted = false;
        }

        public virtual int GetId()
        {
            return id;
        }

        public override string ToString()
        {
            return name + ":" + texnum;
        }
    }
}
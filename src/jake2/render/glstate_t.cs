using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public class glstate_t
    {
        public float inverse_intensity;
        public bool fullscreen;
        public int prev_mode;
        public byte[] d_16to8table;
        public int lightmap_textures;
        public int[] currenttextures = new[]{0, 0};
        public int currenttmu;
        public float camera_separation;
        public bool stereo_enabled;
        public byte[] originalRedGammaTable = new byte[256];
        public byte[] originalGreenGammaTable = new byte[256];
        public byte[] originalBlueGammaTable = new byte[256];
    }
}
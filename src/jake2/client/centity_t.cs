using Jake2.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public class centity_t
    {
        public entity_state_t baseline = new entity_state_t(null);
        public entity_state_t current = new entity_state_t(null);
        public entity_state_t prev = new entity_state_t(null);
        public int serverframe;
        public int trailcount;
        public float[] lerp_origin = new float[]{0, 0, 0};
        public int fly_stoptime;
    }
}
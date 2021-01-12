using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class usercmd_t : ICloneable
    {
        public byte msec;
        public byte buttons;
        public short[] angles = new short[3];
        public short forwardmove, sidemove, upmove;
        public byte impulse;
        public byte lightlevel;
        public virtual void Clear()
        {
            forwardmove = sidemove = upmove = msec = buttons = impulse = lightlevel = 0;
            angles[0] = angles[1] = angles[2] = 0;
        }

        public usercmd_t()
        {
        }

        public usercmd_t(usercmd_t from)
        {
            msec = from.msec;
            buttons = from.buttons;
            angles[0] = from.angles[0];
            angles[1] = from.angles[1];
            angles[2] = from.angles[2];
            forwardmove = from.forwardmove;
            sidemove = from.sidemove;
            upmove = from.upmove;
            impulse = from.impulse;
            lightlevel = from.lightlevel;
        }

        public virtual usercmd_t Set(usercmd_t from)
        {
            msec = from.msec;
            buttons = from.buttons;
            angles[0] = from.angles[0];
            angles[1] = from.angles[1];
            angles[2] = from.angles[2];
            forwardmove = from.forwardmove;
            sidemove = from.sidemove;
            upmove = from.upmove;
            impulse = from.impulse;
            lightlevel = from.lightlevel;
            return this;
        }

		public Object Clone( )
		{
            var clone = new usercmd_t();
            clone.Set( this );
            return clone;
		}
	}
}
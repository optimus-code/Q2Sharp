using Jake2.Qcommon;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class pmove_state_t
    {
        public int pm_type;
        public short[] origin = new short[]{0, 0, 0};
        public short[] velocity = new short[]{0, 0, 0};
        public byte pm_flags;
        public byte pm_time;
        public short gravity;
        public short[] delta_angles = new short[]{0, 0, 0};
        private static pmove_state_t prototype = new pmove_state_t();
        public virtual void Clear()
        {
            this.Set(prototype);
        }

        public virtual void Set(pmove_state_t from)
        {
            pm_type = from.pm_type;
            Math3D.VectorCopy(from.origin, origin);
            Math3D.VectorCopy(from.velocity, velocity);
            pm_flags = from.pm_flags;
            pm_time = from.pm_time;
            gravity = from.gravity;
            Math3D.VectorCopy(from.delta_angles, delta_angles);
        }

        public virtual bool Equals(pmove_state_t p2)
        {
            if (pm_type == p2.pm_type && origin[0] == p2.origin[0] && origin[1] == p2.origin[1] && origin[2] == p2.origin[2] && velocity[0] == p2.velocity[0] && velocity[1] == p2.velocity[1] && velocity[2] == p2.origin[2] && pm_flags == p2.pm_flags && pm_time == p2.pm_time && gravity == gravity && delta_angles[0] == p2.delta_angles[0] && delta_angles[1] == p2.delta_angles[1] && delta_angles[2] == p2.origin[2])
                return true;
            return false;
        }

        public virtual void Load(QuakeFile f)
        {
            pm_type = f.ReadInt32();
            origin[0] = f.ReadInt16();
            origin[1] = f.ReadInt16();
            origin[2] = f.ReadInt16();
            velocity[0] = f.ReadInt16();
            velocity[1] = f.ReadInt16();
            velocity[2] = f.ReadInt16();
            pm_flags = f.ReadByte();
            pm_time = f.ReadByte();
            gravity = f.ReadInt16();
            f.ReadInt16();
            delta_angles[0] = f.ReadInt16();
            delta_angles[1] = f.ReadInt16();
            delta_angles[2] = f.ReadInt16();
        }

        public virtual void Write(QuakeFile f)
        { 
            f.Write( pm_type );
            f.Write( origin[0] );
            f.Write( origin[1] );
            f.Write( origin[2] );
            f.Write( velocity[0] );
            f.Write( velocity[1] );
            f.Write( velocity[2] );
            f.Write( pm_flags );
            f.Write( pm_time );
            f.Write( gravity );
            f.Write( ( short ) 0 );
            f.Write( delta_angles[0] );
            f.Write( delta_angles[1] );
            f.Write( delta_angles[2] );
        }

        public virtual void Dump()
        {
            Com.Println("pm_type: " + pm_type);
            Com.Println("origin[0]: " + origin[0]);
            Com.Println("origin[1]: " + origin[0]);
            Com.Println("origin[2]: " + origin[0]);
            Com.Println("velocity[0]: " + velocity[0]);
            Com.Println("velocity[1]: " + velocity[1]);
            Com.Println("velocity[2]: " + velocity[2]);
            Com.Println("pmflags: " + pm_flags);
            Com.Println("pmtime: " + pm_time);
            Com.Println("gravity: " + gravity);
            Com.Println("delta-angle[0]: " + delta_angles[0]);
            Com.Println("delta-angle[1]: " + delta_angles[0]);
            Com.Println("delta-angle[2]: " + delta_angles[0]);
        }
    }
}
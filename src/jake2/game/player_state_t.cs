using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class player_state_t
    {
        public pmove_state_t pmove = new pmove_state_t();
        public float[] viewangles = new float[]{0, 0, 0};
        public float[] viewoffset = new float[]{0, 0, 0};
        public float[] kick_angles = new float[]{0, 0, 0};
        public float[] gunangles = new float[]{0, 0, 0};
        public float[] gunoffset = new float[]{0, 0, 0};
        public int gunindex;
        public int gunframe;
        public float[] blend = new float[4];
        public float fov;
        public int rdflags;
        public short[] stats = new short[Defines.MAX_STATS];
        private static player_state_t prototype = new player_state_t();
        public virtual void Clear()
        {
            this.Set(prototype);
        }

        public virtual player_state_t GetClone()
        {
            return new player_state_t().Set(this);
        }

        public virtual player_state_t Set(player_state_t from)
        {
            pmove.Set(from.pmove);
            Math3D.VectorCopy(from.viewangles, viewangles);
            Math3D.VectorCopy(from.viewoffset, viewoffset);
            Math3D.VectorCopy(from.kick_angles, kick_angles);
            Math3D.VectorCopy(from.gunangles, gunangles);
            Math3D.VectorCopy(from.gunoffset, gunoffset);
            gunindex = from.gunindex;
            gunframe = from.gunframe;
            blend[0] = from.blend[0];
            blend[1] = from.blend[1];
            blend[2] = from.blend[2];
            blend[3] = from.blend[3];
            fov = from.fov;
            rdflags = from.rdflags;
            System.Array.Copy(from.stats, 0, stats, 0, Defines.MAX_STATS);
            return this;
        }

        public virtual void Load(QuakeFile f)
        {
            pmove.Load(f);
            viewangles[0] = f.ReadSingle();
            viewangles[1] = f.ReadSingle();
            viewangles[2] = f.ReadSingle();
            viewoffset[0] = f.ReadSingle();
            viewoffset[1] = f.ReadSingle();
            viewoffset[2] = f.ReadSingle();
            kick_angles[0] = f.ReadSingle();
            kick_angles[1] = f.ReadSingle();
            kick_angles[2] = f.ReadSingle();
            gunangles[0] = f.ReadSingle();
            gunangles[1] = f.ReadSingle();
            gunangles[2] = f.ReadSingle();
            gunoffset[0] = f.ReadSingle();
            gunoffset[1] = f.ReadSingle();
            gunoffset[2] = f.ReadSingle();
            gunindex = f.ReadInt32();
            gunframe = f.ReadInt32();
            blend[0] = f.ReadSingle();
            blend[1] = f.ReadSingle();
            blend[2] = f.ReadSingle();
            blend[3] = f.ReadSingle();
            fov = f.ReadSingle();
            rdflags = f.ReadInt32();
            for (int n = 0; n < Defines.MAX_STATS; n++)
                stats[n] = f.ReadInt16();
        }

        public virtual void Write(QuakeFile f)
        {
            pmove.Write(f);
            f.Write( viewangles[0]);
            f.Write( viewangles[1]);
            f.Write( viewangles[2]);
            f.Write( viewoffset[0]);
            f.Write( viewoffset[1]);
            f.Write( viewoffset[2]);
            f.Write( kick_angles[0]);
            f.Write( kick_angles[1]);
            f.Write( kick_angles[2]);
            f.Write( gunangles[0]);
            f.Write( gunangles[1]);
            f.Write( gunangles[2]);
            f.Write( gunoffset[0]);
            f.Write( gunoffset[1]);
            f.Write( gunoffset[2]);
            f.Write( gunindex);
            f.Write( gunframe);
            f.Write( blend[0]);
            f.Write( blend[1]);
            f.Write( blend[2]);
            f.Write( blend[3]);
            f.Write( fov);
            f.Write( rdflags);
            for (int n = 0; n < Defines.MAX_STATS; n++)
                f.Write( stats[n]);
        }

        public virtual void Dump()
        {
            pmove.Dump();
            Lib.Printv("viewangles", viewangles);
            Lib.Printv("viewoffset", viewoffset);
            Lib.Printv("kick_angles", kick_angles);
            Lib.Printv("gunangles", gunangles);
            Lib.Printv("gunoffset", gunoffset);
            Com.Println("gunindex: " + gunindex);
            Com.Println("gunframe: " + gunframe);
            Lib.Printv("blend", blend);
            Com.Println("fov: " + fov);
            Com.Println("rdflags: " + rdflags);
            for (int n = 0; n < Defines.MAX_STATS; n++)
                System.Diagnostics.Debug.WriteLine("stats[" + n + "]: " + stats[n]);
        }
    }
}
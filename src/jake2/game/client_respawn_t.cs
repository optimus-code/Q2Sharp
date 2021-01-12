using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Game
{
    public class client_respawn_t
    {
        public client_persistant_t coop_respawn = new client_persistant_t();
        public int enterframe;
        public int score;
        public float[] cmd_angles = new[]{0f, 0f, 0f};
        public bool spectator;
        public virtual void Set(client_respawn_t from)
        {
            coop_respawn.Set(from.coop_respawn);
            enterframe = from.enterframe;
            score = from.score;
            Math3D.VectorCopy(from.cmd_angles, cmd_angles);
            spectator = from.spectator;
        }

        public virtual void Clear()
        {
            coop_respawn = new client_persistant_t();
            enterframe = 0;
            score = 0;
            Math3D.VectorClear(cmd_angles);
            spectator = false;
        }

        public virtual void Read(QuakeFile f)
        {
            coop_respawn.Read(f);
            enterframe = f.ReadInt32();
            score = f.ReadInt32();
            cmd_angles[0] = f.ReadSingle();
            cmd_angles[1] = f.ReadSingle();
            cmd_angles[2] = f.ReadSingle();
            spectator = f.ReadInt32() != 0;
        }

        public virtual void Write(QuakeFile f)
        {
            coop_respawn.Write(f);
            f.Write(enterframe);
            f.Write(score);
            f.Write(cmd_angles[0]);
            f.Write(cmd_angles[1]);
            f.Write(cmd_angles[2]);
            f.Write(spectator ? 1 : 0);
        }
    }
}
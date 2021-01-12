using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class PlayerTrail
    {
        static int TRAIL_LENGTH = 8;
        static edict_t[] trail = new edict_t[TRAIL_LENGTH];
        static int trail_head;
        static bool trail_active = false;
        static PlayerTrail()
        {
            for (int n = 0; n < TRAIL_LENGTH; n++)
                trail[n] = new edict_t(n);
        }

        static int NEXT(int n)
        {
            return (n + 1) % PlayerTrail.TRAIL_LENGTH;
        }

        static int PREV(int n)
        {
            return (n + PlayerTrail.TRAIL_LENGTH - 1) % PlayerTrail.TRAIL_LENGTH;
        }

        public static void Init()
        {
            if (GameBase.deathmatch.value != 0)
                return;
            for (int n = 0; n < PlayerTrail.TRAIL_LENGTH; n++)
            {
                PlayerTrail.trail[n] = GameUtil.G_Spawn();
                PlayerTrail.trail[n].classname = "player_trail";
            }

            trail_head = 0;
            trail_active = true;
        }

        public static void Add(float[] spot)
        {
            float[] temp = new float[]{0, 0, 0};
            if (!trail_active)
                return;
            Math3D.VectorCopy(spot, PlayerTrail.trail[trail_head].s.origin);
            PlayerTrail.trail[trail_head].timestamp = GameBase.level.time;
            Math3D.VectorSubtract(spot, PlayerTrail.trail[PREV(trail_head)].s.origin, temp);
            PlayerTrail.trail[trail_head].s.angles[1] = Math3D.Vectoyaw(temp);
            trail_head = NEXT(trail_head);
        }

        static void New(float[] spot)
        {
            if (!trail_active)
                return;
            Init();
            Add(spot);
        }

        public static edict_t PickFirst(edict_t self)
        {
            if (!trail_active)
                return null;
            int marker = trail_head;
            for (int n = PlayerTrail.TRAIL_LENGTH; n > 0; n--)
            {
                if (PlayerTrail.trail[marker].timestamp <= self.monsterinfo.trail_time)
                    marker = NEXT(marker);
                else
                    break;
            }

            if (GameUtil.Visible(self, PlayerTrail.trail[marker]))
            {
                return PlayerTrail.trail[marker];
            }

            if (GameUtil.Visible(self, PlayerTrail.trail[PREV(marker)]))
            {
                return PlayerTrail.trail[PREV(marker)];
            }

            return PlayerTrail.trail[marker];
        }

        public static edict_t PickNext(edict_t self)
        {
            int marker;
            int n;
            if (!trail_active)
                return null;
            for (marker = trail_head, n = PlayerTrail.TRAIL_LENGTH; n > 0; n--)
            {
                if (PlayerTrail.trail[marker].timestamp <= self.monsterinfo.trail_time)
                    marker = NEXT(marker);
                else
                    break;
            }

            return PlayerTrail.trail[marker];
        }

        public static edict_t LastSpot()
        {
            return PlayerTrail.trail[PREV(trail_head)];
        }
    }
}
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Sound.Joal
{
    public class PlaySound
    {
        static readonly int MAX_PLAYSOUNDS = 128;
        private static PlaySound freeList;
        private static PlaySound playableList;
        private static PlaySound[] backbuffer = new PlaySound[MAX_PLAYSOUNDS];
        static PlaySound()
        {
            for (int i = 0; i < backbuffer.Length; i++)
            {
                backbuffer[i] = new PlaySound();
            }

            freeList = new PlaySound();
            playableList = new PlaySound();
            Reset();
        }

        public int type;
        public int entnum;
        public int entchannel;
        public int bufferId;
        public float volume;
        public float attenuation;
        public float[] origin = new float[]{0, 0, 0};
        private long beginTime;
        private PlaySound prev, next;
        private PlaySound()
        {
            prev = next = null;
            this.Clear();
        }

        private void Clear()
        {
            type = bufferId = entnum = entchannel = -1;
            attenuation = beginTime = 0;
        }

        public static void Reset()
        {
            freeList.next = freeList.prev = freeList;
            playableList.next = playableList.prev = playableList;
            PlaySound ps;
            for (int i = 0; i < backbuffer.Length; i++)
            {
                ps = backbuffer[i];
                ps.Clear();
                ps.prev = freeList;
                ps.next = freeList.next;
                ps.prev.next = ps;
                ps.next.prev = ps;
            }
        }

        public static PlaySound NextPlayableSound()
        {
            PlaySound ps = null;
            while (true)
            {
                ps = playableList.next;
                if (ps == playableList || ps.beginTime > Globals.cl.time)
                    return null;
                PlaySound.Release(ps);
                return ps;
            }
        }

        private static PlaySound Get()
        {
            PlaySound ps = freeList.next;
            if (ps == freeList)
                return null;
            ps.prev.next = ps.next;
            ps.next.prev = ps.prev;
            return ps;
        }

        private static void Add(PlaySound ps)
        {
            PlaySound sort = playableList.next;
            ps.next = sort;
            ps.prev = sort.prev;
            ps.next.prev = ps;
            ps.prev.next = ps;
        }

        private static void Release(PlaySound ps)
        {
            ps.prev.next = ps.next;
            ps.next.prev = ps.prev;
            ps.next = freeList.next;
            freeList.next.prev = ps;
            ps.prev = freeList;
            freeList.next = ps;
        }

        public static void Allocate(float[] origin, int entnum, int entchannel, int bufferId, float volume, float attenuation, float timeoffset)
        {
            PlaySound ps = PlaySound.Get();
            if (ps != null)
            {
                if (entnum == Globals.cl.playernum + 1)
                {
                    ps.type = Channel.LISTENER;
                }
                else if (origin != null)
                {
                    ps.type = Channel.FIXED;
                    Math3D.VectorCopy(origin, ps.origin);
                }
                else
                {
                    ps.type = Channel.DYNAMIC;
                }

                ps.entnum = entnum;
                ps.entchannel = entchannel;
                ps.bufferId = bufferId;
                ps.volume = volume;
                ps.attenuation = attenuation;
                ps.beginTime = Globals.cl.time + (long)(timeoffset * 1000);
                PlaySound.Add(ps);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("PlaySounds out of Limit");
            }
        }
    }
}
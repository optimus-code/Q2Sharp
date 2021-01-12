using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Server
{
    public class SV_GAME
    {
        public static void PF_Unicast(edict_t ent, bool reliable)
        {
            int p;
            client_t client;
            if (ent == null)
                return;
            p = ent.index;
            if (p < 1 || p > SV_MAIN.maxclients.value)
                return;
            client = SV_INIT.svs.clients[p - 1];
            if (reliable)
                SZ.Write(client.netchan.message, SV_INIT.sv.multicast.data, SV_INIT.sv.multicast.cursize);
            else
                SZ.Write(client.datagram, SV_INIT.sv.multicast.data, SV_INIT.sv.multicast.cursize);
            SZ.Clear(SV_INIT.sv.multicast);
        }

        public static void PF_dprintf(string fmt)
        {
            Com.Printf(fmt);
        }

        public static void PF_cprintfhigh(edict_t ent, string fmt)
        {
            PF_cprintf(ent, Defines.PRINT_HIGH, fmt);
        }

        public static void PF_cprintf(edict_t ent, int level, string fmt)
        {
            int n = 0;
            if (ent != null)
            {
                n = ent.index;
                if (n < 1 || n > SV_MAIN.maxclients.value)
                    Com.Error(Defines.ERR_DROP, "cprintf to a non-client");
            }

            if (ent != null)
                SV_SEND.SV_ClientPrintf(SV_INIT.svs.clients[n - 1], level, fmt);
            else
                Com.Printf(fmt);
        }

        public static void PF_centerprintf(edict_t ent, string fmt)
        {
            int n;
            n = ent.index;
            if (n < 1 || n > SV_MAIN.maxclients.value)
                return;
            MSG.WriteByte(SV_INIT.sv.multicast, Defines.svc_centerprint);
            MSG.WriteString(SV_INIT.sv.multicast, fmt);
            PF_Unicast(ent, true);
        }

        public static void PF_error(string fmt)
        {
            Com.Error(Defines.ERR_DROP, "Game Error: " + fmt);
        }

        public static void PF_error(int level, string fmt)
        {
            Com.Error(level, fmt);
        }

        public static void PF_setmodel(edict_t ent, string name)
        {
            int i;
            cmodel_t mod;
            if (name == null)
                Com.Error(Defines.ERR_DROP, "PF_setmodel: NULL");
            i = SV_INIT.SV_ModelIndex(name);
            ent.s.modelindex = i;
            if (name.StartsWith("*"))
            {
                mod = CM.InlineModel(name);
                Math3D.VectorCopy(mod.mins, ent.mins);
                Math3D.VectorCopy(mod.maxs, ent.maxs);
                SV_WORLD.SV_LinkEdict(ent);
            }
        }

        public static void PF_Configstring(int index, string val)
        {
            if (index < 0 || index >= Defines.MAX_CONFIGSTRINGS)
                Com.Error(Defines.ERR_DROP, "configstring: bad index " + index + "\\n");
            if (val == null)
                val = "";
            SV_INIT.sv.configstrings[index] = val;
            if (SV_INIT.sv.state != Defines.ss_loading)
            {
                SZ.Clear(SV_INIT.sv.multicast);
                MSG.WriteChar(SV_INIT.sv.multicast, Defines.svc_configstring);
                MSG.WriteShort(SV_INIT.sv.multicast, index);
                MSG.WriteString(SV_INIT.sv.multicast, val);
                SV_SEND.SV_Multicast(Globals.vec3_origin, Defines.MULTICAST_ALL_R);
            }
        }

        public static void PF_WriteChar(int c)
        {
            MSG.WriteChar(SV_INIT.sv.multicast, c);
        }

        public static void PF_WriteByte(int c)
        {
            MSG.WriteByte(SV_INIT.sv.multicast, c);
        }

        public static void PF_WriteShort(int c)
        {
            MSG.WriteShort(SV_INIT.sv.multicast, c);
        }

        public static void PF_WriteLong(int c)
        {
            MSG.WriteLong(SV_INIT.sv.multicast, c);
        }

        public static void PF_WriteFloat(float f)
        {
            MSG.WriteFloat(SV_INIT.sv.multicast, f);
        }

        public static void PF_WriteString(string s)
        {
            MSG.WriteString(SV_INIT.sv.multicast, s);
        }

        public static void PF_WritePos(float[] pos)
        {
            MSG.WritePos(SV_INIT.sv.multicast, pos);
        }

        public static void PF_WriteDir(float[] dir)
        {
            MSG.WriteDir(SV_INIT.sv.multicast, dir);
        }

        public static void PF_WriteAngle(float f)
        {
            MSG.WriteAngle(SV_INIT.sv.multicast, f);
        }

        public static bool PF_inPVS(float[] p1, float[] p2)
        {
            int leafnum;
            int cluster;
            int area1, area2;
            byte[] mask;
            leafnum = CM.CM_PointLeafnum(p1);
            cluster = CM.CM_LeafCluster(leafnum);
            area1 = CM.CM_LeafArea(leafnum);
            mask = CM.CM_ClusterPVS(cluster);
            leafnum = CM.CM_PointLeafnum(p2);
            cluster = CM.CM_LeafCluster(leafnum);
            area2 = CM.CM_LeafArea(leafnum);
            if (cluster == -1)
                return false;
            if (mask != null && (0 == (mask[cluster >> 3] & (1 << (cluster & 7)))))
                return false;
            if (!CM.CM_AreasConnected(area1, area2))
                return false;
            return true;
        }

        public static bool PF_inPHS(float[] p1, float[] p2)
        {
            int leafnum;
            int cluster;
            int area1, area2;
            byte[] mask;
            leafnum = CM.CM_PointLeafnum(p1);
            cluster = CM.CM_LeafCluster(leafnum);
            area1 = CM.CM_LeafArea(leafnum);
            mask = CM.CM_ClusterPHS(cluster);
            leafnum = CM.CM_PointLeafnum(p2);
            cluster = CM.CM_LeafCluster(leafnum);
            area2 = CM.CM_LeafArea(leafnum);
            if (cluster == -1)
                return false;
            if (mask != null && (0 == (mask[cluster >> 3] & (1 << (cluster & 7)))))
                return false;
            if (!CM.CM_AreasConnected(area1, area2))
                return false;
            return true;
        }

        public static void PF_StartSound(edict_t entity, int channel, int sound_num, float volume, float attenuation, float timeofs)
        {
            if (null == entity)
                return;
            SV_SEND.SV_StartSound(null, entity, channel, sound_num, volume, attenuation, timeofs);
        }

        public static void SV_ShutdownGameProgs()
        {
            GameBase.ShutdownGame();
        }

        public static void SV_InitGameProgs()
        {
            SV_ShutdownGameProgs();
            game_import_t gimport = new game_import_t();
            GameBase.GetGameApi(gimport);
            GameSave.InitGame();
        }
    }
}
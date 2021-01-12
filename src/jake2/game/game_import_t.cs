using Jake2.Qcommon;
using Jake2.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Jake2.Game.pmove_t;

namespace Jake2.Game
{
    public class game_import_t
    {
        public virtual void Bprintf(int printlevel, string s)
        {
            SV_SEND.SV_BroadcastPrintf(printlevel, s);
        }

        public virtual void Dprintf(string s)
        {
            SV_GAME.PF_dprintf(s);
        }

        public virtual void Cprintf(edict_t ent, int printlevel, string s)
        {
            SV_GAME.PF_cprintf(ent, printlevel, s);
        }

        public virtual void Centerprintf(edict_t ent, string s)
        {
            SV_GAME.PF_centerprintf(ent, s);
        }

        public virtual void Sound(edict_t ent, int channel, int soundindex, float volume, float attenuation, float timeofs)
        {
            SV_GAME.PF_StartSound(ent, channel, soundindex, volume, attenuation, timeofs);
        }

        public virtual void Positioned_sound(float[] origin, edict_t ent, int channel, int soundinedex, float volume, float attenuation, float timeofs)
        {
            SV_SEND.SV_StartSound(origin, ent, channel, soundinedex, volume, attenuation, timeofs);
        }

        public virtual void Configstring(int num, string string_renamed)
        {
            SV_GAME.PF_Configstring(num, string_renamed);
        }

        public virtual void Error(string err)
        {
            Com.Error(Defines.ERR_FATAL, err);
        }

        public virtual void Error(int level, string err)
        {
            SV_GAME.PF_error(level, err);
        }

        public virtual int Modelindex(string name)
        {
            return SV_INIT.SV_ModelIndex(name);
        }

        public virtual int Soundindex(string name)
        {
            return SV_INIT.SV_SoundIndex(name);
        }

        public virtual int Imageindex(string name)
        {
            return SV_INIT.SV_ImageIndex(name);
        }

        public virtual void Setmodel(edict_t ent, string name)
        {
            SV_GAME.PF_setmodel(ent, name);
        }

        public virtual trace_t Trace(float[] start, float[] mins, float[] maxs, float[] end, edict_t passent, int contentmask)
        {
            return SV_WORLD.SV_Trace(start, mins, maxs, end, passent, contentmask);
        }

        public pmove_t.PointContentsAdapter pointcontents = new AnonymousPointContentsAdapter();
        private sealed class AnonymousPointContentsAdapter : PointContentsAdapter
        {
            public override int Pointcontents(float[] o)
            {
                return 0;
            }
        }

        public virtual bool InPHS(float[] p1, float[] p2)
        {
            return SV_GAME.PF_inPHS(p1, p2);
        }

        public virtual void SetAreaPortalState(int portalnum, bool open)
        {
            CM.CM_SetAreaPortalState(portalnum, open);
        }

        public virtual bool AreasConnected(int area1, int area2)
        {
            return CM.CM_AreasConnected(area1, area2);
        }

        public virtual void Linkentity(edict_t ent)
        {
            SV_WORLD.SV_LinkEdict(ent);
        }

        public virtual void Unlinkentity(edict_t ent)
        {
            SV_WORLD.SV_UnlinkEdict(ent);
        }

        public virtual int BoxEdicts(float[] mins, float[] maxs, edict_t[] list, int maxcount, int areatype)
        {
            return SV_WORLD.SV_AreaEdicts(mins, maxs, list, maxcount, areatype);
        }

        public virtual void Pmove(pmove_t pmove)
        {
            PMove.Pmove(pmove);
        }

        public virtual void Multicast(float[] origin, int to)
        {
            SV_SEND.SV_Multicast(origin, to);
        }

        public virtual void Unicast(edict_t ent, bool reliable)
        {
            SV_GAME.PF_Unicast(ent, reliable);
        }

        public virtual void WriteByte(int c)
        {
            SV_GAME.PF_WriteByte(c);
        }

        public virtual void WriteShort(int c)
        {
            SV_GAME.PF_WriteShort(c);
        }

        public virtual void WriteString(string s)
        {
            SV_GAME.PF_WriteString(s);
        }

        public virtual void WritePosition(float[] pos)
        {
            SV_GAME.PF_WritePos(pos);
        }

        public virtual void WriteDir(float[] pos)
        {
            SV_GAME.PF_WriteDir(pos);
        }

        public virtual cvar_t Cvar_f(string var_name, string value, int flags)
        {
            return Cvar.Get(var_name, value, flags);
        }

        public virtual cvar_t Cvar_set(string var_name, string value)
        {
            return Cvar.Set(var_name, value);
        }

        public virtual cvar_t Cvar_forceset(string var_name, string value)
        {
            return Cvar.ForceSet(var_name, value);
        }

        public virtual int Argc()
        {
            return Cmd.Argc();
        }

        public virtual string Argv(int n)
        {
            return Cmd.Argv(n);
        }

        public virtual string Args()
        {
            return Cmd.Args();
        }

        public virtual void AddCommandString(string text)
        {
            Cbuf.AddText(text);
        }
    }
}
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Sys;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public class CL_input
    {
        static long frame_msec;
        static long old_sys_frame_time;
        static cvar_t cl_nodelta;
        static kbutton_t in_klook = new kbutton_t();
        static kbutton_t in_left = new kbutton_t();
        static kbutton_t in_right = new kbutton_t();
        static kbutton_t in_forward = new kbutton_t();
        static kbutton_t in_back = new kbutton_t();
        static kbutton_t in_lookup = new kbutton_t();
        static kbutton_t in_lookdown = new kbutton_t();
        static kbutton_t in_moveleft = new kbutton_t();
        static kbutton_t in_moveright = new kbutton_t();
        public static kbutton_t in_strafe = new kbutton_t();
        static kbutton_t in_speed = new kbutton_t();
        static kbutton_t in_use = new kbutton_t();
        static kbutton_t in_attack = new kbutton_t();
        static kbutton_t in_up = new kbutton_t();
        static kbutton_t in_down = new kbutton_t();
        static int in_impulse;
        static void KeyDown(kbutton_t b)
        {
            int k;
            string c;
            c = Cmd.Argv(1);
            if (c.Length > 0)
                k = Lib.Atoi(c);
            else
                k = -1;
            if (k == b.down[0] || k == b.down[1])
                return;
            if (b.down[0] == 0)
                b.down[0] = k;
            else if (b.down[1] == 0)
                b.down[1] = k;
            else
            {
                Com.Printf("Three keys down for a button!\\n");
                return;
            }

            if ((b.state & 1) != 0)
                return;
            c = Cmd.Argv(2);
            b.downtime = Lib.Atoi(c);
            if (b.downtime == 0)
                b.downtime = Globals.sys_frame_time - 100;
            b.state |= 3;
        }

        static void KeyUp(kbutton_t b)
        {
            int k;
            string c;
            int uptime;
            c = Cmd.Argv(1);
            if (c.Length > 0)
                k = Lib.Atoi(c);
            else
            {
                b.down[0] = b.down[1] = 0;
                b.state = 4;
                return;
            }

            if (b.down[0] == k)
                b.down[0] = 0;
            else if (b.down[1] == k)
                b.down[1] = 0;
            else
                return;
            if (b.down[0] != 0 || b.down[1] != 0)
                return;
            if ((b.state & 1) == 0)
                return;
            c = Cmd.Argv(2);
            uptime = Lib.Atoi(c);
            if (uptime != 0)
                b.msec += uptime - b.downtime;
            else
                b.msec += 10;
            b.state &= ~1;
            b.state |= 4;
        }

        static void IN_KLookDown()
        {
            KeyDown(in_klook);
        }

        static void IN_KLookUp()
        {
            KeyUp(in_klook);
        }

        static void IN_UpDown()
        {
            KeyDown(in_up);
        }

        static void IN_UpUp()
        {
            KeyUp(in_up);
        }

        static void IN_DownDown()
        {
            KeyDown(in_down);
        }

        static void IN_DownUp()
        {
            KeyUp(in_down);
        }

        static void IN_LeftDown()
        {
            KeyDown(in_left);
        }

        static void IN_LeftUp()
        {
            KeyUp(in_left);
        }

        static void IN_RightDown()
        {
            KeyDown(in_right);
        }

        static void IN_RightUp()
        {
            KeyUp(in_right);
        }

        static void IN_ForwardDown()
        {
            KeyDown(in_forward);
        }

        static void IN_ForwardUp()
        {
            KeyUp(in_forward);
        }

        static void IN_BackDown()
        {
            KeyDown(in_back);
        }

        static void IN_BackUp()
        {
            KeyUp(in_back);
        }

        static void IN_LookupDown()
        {
            KeyDown(in_lookup);
        }

        static void IN_LookupUp()
        {
            KeyUp(in_lookup);
        }

        static void IN_LookdownDown()
        {
            KeyDown(in_lookdown);
        }

        static void IN_LookdownUp()
        {
            KeyUp(in_lookdown);
        }

        static void IN_MoveleftDown()
        {
            KeyDown(in_moveleft);
        }

        static void IN_MoveleftUp()
        {
            KeyUp(in_moveleft);
        }

        static void IN_MoverightDown()
        {
            KeyDown(in_moveright);
        }

        static void IN_MoverightUp()
        {
            KeyUp(in_moveright);
        }

        static void IN_SpeedDown()
        {
            KeyDown(in_speed);
        }

        static void IN_SpeedUp()
        {
            KeyUp(in_speed);
        }

        static void IN_StrafeDown()
        {
            KeyDown(in_strafe);
        }

        static void IN_StrafeUp()
        {
            KeyUp(in_strafe);
        }

        static void IN_AttackDown()
        {
            KeyDown(in_attack);
        }

        static void IN_AttackUp()
        {
            KeyUp(in_attack);
        }

        static void IN_UseDown()
        {
            KeyDown(in_use);
        }

        static void IN_UseUp()
        {
            KeyUp(in_use);
        }

        static void IN_Impulse()
        {
            in_impulse = Lib.Atoi(Cmd.Argv(1));
        }

        static float KeyState(kbutton_t key)
        {
            float val;
            long msec;
            key.state &= 1;
            msec = key.msec;
            key.msec = 0;
            if (key.state != 0)
            {
                msec += Globals.sys_frame_time - key.downtime;
                key.downtime = Globals.sys_frame_time;
            }

            val = (float)msec / frame_msec;
            if (val < 0)
                val = 0;
            if (val > 1)
                val = 1;
            return val;
        }

        static void AdjustAngles()
        {
            float speed;
            float up, down;
            if ((in_speed.state & 1) != 0)
                speed = Globals.cls.frametime * Globals.cl_anglespeedkey.value;
            else
                speed = Globals.cls.frametime;
            if ((in_strafe.state & 1) == 0)
            {
                Globals.cl.viewangles[Defines.YAW] -= speed * Globals.cl_yawspeed.value * KeyState(in_right);
                Globals.cl.viewangles[Defines.YAW] += speed * Globals.cl_yawspeed.value * KeyState(in_left);
            }

            if ((in_klook.state & 1) != 0)
            {
                Globals.cl.viewangles[Defines.PITCH] -= speed * Globals.cl_pitchspeed.value * KeyState(in_forward);
                Globals.cl.viewangles[Defines.PITCH] += speed * Globals.cl_pitchspeed.value * KeyState(in_back);
            }

            up = KeyState(in_lookup);
            down = KeyState(in_lookdown);
            Globals.cl.viewangles[Defines.PITCH] -= speed * Globals.cl_pitchspeed.value * up;
            Globals.cl.viewangles[Defines.PITCH] += speed * Globals.cl_pitchspeed.value * down;
        }

        static void BaseMove(usercmd_t cmd)
        {
            AdjustAngles();
            cmd.Clear();
            Math3D.VectorCopy(Globals.cl.viewangles, cmd.angles);
            if ((in_strafe.state & 1) != 0)
            {
                cmd.sidemove += (short)(Globals.cl_sidespeed.value * KeyState(in_right));
                cmd.sidemove -= (short)(Globals.cl_sidespeed.value * KeyState(in_left));
            }

            cmd.sidemove += (short)(Globals.cl_sidespeed.value * KeyState(in_moveright));
            cmd.sidemove -= (short)(Globals.cl_sidespeed.value * KeyState(in_moveleft));
            cmd.upmove += (short)(Globals.cl_upspeed.value * KeyState(in_up));
            cmd.upmove -= (short)(Globals.cl_upspeed.value * KeyState(in_down));
            if ((in_klook.state & 1) == 0)
            {
                cmd.forwardmove += (short)(Globals.cl_forwardspeed.value * KeyState(in_forward));
                cmd.forwardmove -= (short)(Globals.cl_forwardspeed.value * KeyState(in_back));
            }

            if (((in_speed.state & 1) ^ (int)(Globals.cl_run.value)) != 0)
            {
                cmd.forwardmove *= 2;
                cmd.sidemove *= 2;
                cmd.upmove *= 2;
            }
        }

        static void ClampPitch()
        {
            float pitch;
            pitch = Math3D.SHORT2ANGLE(Globals.cl.frame.playerstate.pmove.delta_angles[Defines.PITCH]);
            if (pitch > 180)
                pitch -= 360;
            if (Globals.cl.viewangles[Defines.PITCH] + pitch < -360)
                Globals.cl.viewangles[Defines.PITCH] += 360;
            if (Globals.cl.viewangles[Defines.PITCH] + pitch > 360)
                Globals.cl.viewangles[Defines.PITCH] -= 360;
            if (Globals.cl.viewangles[Defines.PITCH] + pitch > 89)
                Globals.cl.viewangles[Defines.PITCH] = 89 - pitch;
            if (Globals.cl.viewangles[Defines.PITCH] + pitch < -89)
                Globals.cl.viewangles[Defines.PITCH] = -89 - pitch;
        }

        static void FinishMove(usercmd_t cmd)
        {
            int ms;
            int i;
            if ((in_attack.state & 3) != 0)
                cmd.buttons |= Defines.BUTTON_ATTACK;
            in_attack.state &= ~2;
            if ((in_use.state & 3) != 0)
                cmd.buttons |= Defines.BUTTON_USE;
            in_use.state &= ~2;
            if (Key.anykeydown != 0 && Globals.cls.key_dest == Defines.key_game)
                cmd.buttons |= Defines.BUTTON_ANY;
            ms = (int)(Globals.cls.frametime * 1000);
            if (ms > 250)
                ms = 100;
            cmd.msec = (byte)ms;
            ClampPitch();
            for (i = 0; i < 3; i++)
                cmd.angles[i] = (short)Math3D.ANGLE2SHORT(Globals.cl.viewangles[i]);
            cmd.impulse = (byte)in_impulse;
            in_impulse = 0;
            cmd.lightlevel = (byte)Globals.cl_lightlevel.value;
        }

        static void CreateCmd(usercmd_t cmd)
        {
            frame_msec = Globals.sys_frame_time - old_sys_frame_time;
            if (frame_msec < 1)
                frame_msec = 1;
            if (frame_msec > 200)
                frame_msec = 200;
            BaseMove(cmd);
            IN.Move(cmd);
            FinishMove(cmd);
            old_sys_frame_time = Globals.sys_frame_time;
        }

        public static void InitInput()
        {
            Cmd.AddCommand("centerview", new Anonymousxcommand_t());
            Cmd.AddCommand("+moveup", new Anonymousxcommand_t1());
            Cmd.AddCommand("-moveup", new Anonymousxcommand_t2());
            Cmd.AddCommand("+movedown", new Anonymousxcommand_t3());
            Cmd.AddCommand("-movedown", new Anonymousxcommand_t4());
            Cmd.AddCommand("+left", new Anonymousxcommand_t5());
            Cmd.AddCommand("-left", new Anonymousxcommand_t6());
            Cmd.AddCommand("+right", new Anonymousxcommand_t7());
            Cmd.AddCommand("-right", new Anonymousxcommand_t8());
            Cmd.AddCommand("+forward", new Anonymousxcommand_t9());
            Cmd.AddCommand("-forward", new Anonymousxcommand_t10());
            Cmd.AddCommand("+back", new Anonymousxcommand_t11());
            Cmd.AddCommand("-back", new Anonymousxcommand_t12());
            Cmd.AddCommand("+lookup", new Anonymousxcommand_t13());
            Cmd.AddCommand("-lookup", new Anonymousxcommand_t14());
            Cmd.AddCommand("+lookdown", new Anonymousxcommand_t15());
            Cmd.AddCommand("-lookdown", new Anonymousxcommand_t16());
            Cmd.AddCommand("+strafe", new Anonymousxcommand_t17());
            Cmd.AddCommand("-strafe", new Anonymousxcommand_t18());
            Cmd.AddCommand("+moveleft", new Anonymousxcommand_t19());
            Cmd.AddCommand("-moveleft", new Anonymousxcommand_t20());
            Cmd.AddCommand("+moveright", new Anonymousxcommand_t21());
            Cmd.AddCommand("-moveright", new Anonymousxcommand_t22());
            Cmd.AddCommand("+speed", new Anonymousxcommand_t23());
            Cmd.AddCommand("-speed", new Anonymousxcommand_t24());
            Cmd.AddCommand("+attack", new Anonymousxcommand_t25());
            Cmd.AddCommand("-attack", new Anonymousxcommand_t26());
            Cmd.AddCommand("+use", new Anonymousxcommand_t27());
            Cmd.AddCommand("-use", new Anonymousxcommand_t28());
            Cmd.AddCommand("impulse", new Anonymousxcommand_t29());
            Cmd.AddCommand("+klook", new Anonymousxcommand_t30());
            Cmd.AddCommand("-klook", new Anonymousxcommand_t31());
            cl_nodelta = Cvar.Get("cl_nodelta", "0", 0);
        }

        private sealed class Anonymousxcommand_t : xcommand_t
        {
            public override void Execute()
            {
                IN.CenterView();
            }
        }

        private sealed class Anonymousxcommand_t1 : xcommand_t
        {
            public override void Execute()
            {
                IN_UpDown();
            }
        }

        private sealed class Anonymousxcommand_t2 : xcommand_t
        {
            public override void Execute()
            {
                IN_UpUp();
            }
        }

        private sealed class Anonymousxcommand_t3 : xcommand_t
        {
            public override void Execute()
            {
                IN_DownDown();
            }
        }

        private sealed class Anonymousxcommand_t4 : xcommand_t
        {
            public override void Execute()
            {
                IN_DownUp();
            }
        }

        private sealed class Anonymousxcommand_t5 : xcommand_t
        {
            public override void Execute()
            {
                IN_LeftDown();
            }
        }

        private sealed class Anonymousxcommand_t6 : xcommand_t
        {
            public override void Execute()
            {
                IN_LeftUp();
            }
        }

        private sealed class Anonymousxcommand_t7 : xcommand_t
        {
            public override void Execute()
            {
                IN_RightDown();
            }
        }

        private sealed class Anonymousxcommand_t8 : xcommand_t
        {
            public override void Execute()
            {
                IN_RightUp();
            }
        }

        private sealed class Anonymousxcommand_t9 : xcommand_t
        {
            public override void Execute()
            {
                IN_ForwardDown();
            }
        }

        private sealed class Anonymousxcommand_t10 : xcommand_t
        {
            public override void Execute()
            {
                IN_ForwardUp();
            }
        }

        private sealed class Anonymousxcommand_t11 : xcommand_t
        {
            public override void Execute()
            {
                IN_BackDown();
            }
        }

        private sealed class Anonymousxcommand_t12 : xcommand_t
        {
            public override void Execute()
            {
                IN_BackUp();
            }
        }

        private sealed class Anonymousxcommand_t13 : xcommand_t
        {
            public override void Execute()
            {
                IN_LookupDown();
            }
        }

        private sealed class Anonymousxcommand_t14 : xcommand_t
        {
            public override void Execute()
            {
                IN_LookupUp();
            }
        }

        private sealed class Anonymousxcommand_t15 : xcommand_t
        {
            public override void Execute()
            {
                IN_LookdownDown();
            }
        }

        private sealed class Anonymousxcommand_t16 : xcommand_t
        {
            public override void Execute()
            {
                IN_LookdownUp();
            }
        }

        private sealed class Anonymousxcommand_t17 : xcommand_t
        {
            public override void Execute()
            {
                IN_StrafeDown();
            }
        }

        private sealed class Anonymousxcommand_t18 : xcommand_t
        {
            public override void Execute()
            {
                IN_StrafeUp();
            }
        }

        private sealed class Anonymousxcommand_t19 : xcommand_t
        {
            public override void Execute()
            {
                IN_MoveleftDown();
            }
        }

        private sealed class Anonymousxcommand_t20 : xcommand_t
        {
            public override void Execute()
            {
                IN_MoveleftUp();
            }
        }

        private sealed class Anonymousxcommand_t21 : xcommand_t
        {
            public override void Execute()
            {
                IN_MoverightDown();
            }
        }

        private sealed class Anonymousxcommand_t22 : xcommand_t
        {
            public override void Execute()
            {
                IN_MoverightUp();
            }
        }

        private sealed class Anonymousxcommand_t23 : xcommand_t
        {
            public override void Execute()
            {
                IN_SpeedDown();
            }
        }

        private sealed class Anonymousxcommand_t24 : xcommand_t
        {
            public override void Execute()
            {
                IN_SpeedUp();
            }
        }

        private sealed class Anonymousxcommand_t25 : xcommand_t
        {
            public override void Execute()
            {
                IN_AttackDown();
            }
        }

        private sealed class Anonymousxcommand_t26 : xcommand_t
        {
            public override void Execute()
            {
                IN_AttackUp();
            }
        }

        private sealed class Anonymousxcommand_t27 : xcommand_t
        {
            public override void Execute()
            {
                IN_UseDown();
            }
        }

        private sealed class Anonymousxcommand_t28 : xcommand_t
        {
            public override void Execute()
            {
                IN_UseUp();
            }
        }

        private sealed class Anonymousxcommand_t29 : xcommand_t
        {
            public override void Execute()
            {
                IN_Impulse();
            }
        }

        private sealed class Anonymousxcommand_t30 : xcommand_t
        {
            public override void Execute()
            {
                IN_KLookDown();
            }
        }

        private sealed class Anonymousxcommand_t31 : xcommand_t
        {
            public override void Execute()
            {
                IN_KLookUp();
            }
        }

        private static readonly sizebuf_t buf = new sizebuf_t();
        private static readonly byte[] data = new byte[128];
        private static readonly usercmd_t nullcmd = new usercmd_t();
        public static void SendCmd()
        {
            int i;
            usercmd_t cmd, oldcmd;
            int checksumIndex;
            i = Globals.cls.netchan.outgoing_sequence & (Defines.CMD_BACKUP - 1);
            cmd = Globals.cl.cmds[i];
            Globals.cl.cmd_time[i] = (int)Globals.cls.realtime;
            CreateCmd(cmd);
            Globals.cl.cmd.Set(cmd);
            if (Globals.cls.state == Defines.ca_disconnected || Globals.cls.state == Defines.ca_connecting)
                return;
            if (Globals.cls.state == Defines.ca_connected)
            {
                if (Globals.cls.netchan.message.cursize != 0 || Globals.curtime - Globals.cls.netchan.last_sent > 1000)
                    Netchan.Transmit(Globals.cls.netchan, 0, new byte[0]);
                return;
            }

            if (Globals.userinfo_modified)
            {
                CL.FixUpGender();
                Globals.userinfo_modified = false;
                MSG.WriteByte(Globals.cls.netchan.message, Defines.clc_userinfo);
                MSG.WriteString(Globals.cls.netchan.message, Cvar.Userinfo());
            }

            SZ.Init(buf, data, data.Length);
            if (cmd.buttons != 0 && Globals.cl.cinematictime > 0 && !Globals.cl.attractloop && Globals.cls.realtime - Globals.cl.cinematictime > 1000)
            {
                SCR.FinishCinematic();
            }

            MSG.WriteByte(buf, Defines.clc_move);
            checksumIndex = buf.cursize;
            MSG.WriteByte(buf, 0);
            if (cl_nodelta.value != 0F || !Globals.cl.frame.valid || Globals.cls.demowaiting)
                MSG.WriteLong(buf, -1);
            else
                MSG.WriteLong(buf, Globals.cl.frame.serverframe);
            i = (Globals.cls.netchan.outgoing_sequence - 2) & (Defines.CMD_BACKUP - 1);
            cmd = Globals.cl.cmds[i];
            nullcmd.Clear();
            MSG.WriteDeltaUsercmd(buf, nullcmd, cmd);
            oldcmd = cmd;
            i = (Globals.cls.netchan.outgoing_sequence - 1) & (Defines.CMD_BACKUP - 1);
            cmd = Globals.cl.cmds[i];
            MSG.WriteDeltaUsercmd(buf, oldcmd, cmd);
            oldcmd = cmd;
            i = (Globals.cls.netchan.outgoing_sequence) & (Defines.CMD_BACKUP - 1);
            cmd = Globals.cl.cmds[i];
            MSG.WriteDeltaUsercmd(buf, oldcmd, cmd);
            buf.data[checksumIndex] = Com.BlockSequenceCRCByte(buf.data, checksumIndex + 1, buf.cursize - checksumIndex - 1, Globals.cls.netchan.outgoing_sequence);
            Netchan.Transmit(Globals.cls.netchan, buf.cursize, buf.data);
        }
    }
}
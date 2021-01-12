using Jake2.Game;
using Jake2.Server;
using Jake2.Sys;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Qcommon
{
    public sealed class Netchan : SV_MAIN
    {
        public static cvar_t showpackets;
        public static cvar_t showdrop;
        public static cvar_t qport;
        public static sizebuf_t net_message = new sizebuf_t();
        public static byte[] net_message_buffer = new byte[Defines.MAX_MSGLEN];
        public static void Netchan_Init()
        {
            long port;
            port = Timer.Milliseconds() & 0xffff;
            showpackets = Cvar.Get("showpackets", "0", 0);
            showdrop = Cvar.Get("showdrop", "0", 0);
            qport = Cvar.Get("qport", "" + port, Defines.CVAR_NOSET);
        }

        private static readonly byte[] send_buf = new byte[Defines.MAX_MSGLEN];
        private static readonly sizebuf_t send = new sizebuf_t();
        public static void Netchan_OutOfBand(int net_socket, netadr_t adr, int length, byte[] data)
        {
            SZ.Init(send, send_buf, Defines.MAX_MSGLEN);
            MSG.WriteInt(send, -1);
            SZ.Write(send, data, length);
            NET.SendPacket(net_socket, send.cursize, send.data, adr);
        }

        public static void OutOfBandPrint(int net_socket, netadr_t adr, string s)
        {
            Netchan_OutOfBand(net_socket, adr, s.Length, Lib.StringToBytes(s));
        }

        public static void Setup(int sock, netchan_t chan, netadr_t adr, int qport)
        {
            chan.Clear();
            chan.sock = sock;
            chan.remote_address.Set(adr);
            chan.qport = qport;
            chan.last_received = Globals.curtime;
            chan.incoming_sequence = 0;
            chan.outgoing_sequence = 1;
            SZ.Init(chan.message, chan.message_buf, chan.message_buf.Length);
            chan.message.allowoverflow = true;
        }

        public static bool Netchan_CanReliable(netchan_t chan)
        {
            if (chan.reliable_length != 0)
                return false;
            return true;
        }

        public static bool Netchan_NeedReliable(netchan_t chan)
        {
            bool send_reliable;
            send_reliable = false;
            if (chan.incoming_acknowledged > chan.last_reliable_sequence && chan.incoming_reliable_acknowledged != chan.reliable_sequence)
                send_reliable = true;
            if (0 == chan.reliable_length && chan.message.cursize != 0)
            {
                send_reliable = true;
            }

            return send_reliable;
        }

        public static void Transmit(netchan_t chan, int length, byte[] data)
        {
            int send_reliable;
            int w1, w2;
            if (chan.message.overflowed)
            {
                chan.fatal_error = true;
                Com.Printf(NET.AdrToString(chan.remote_address) + ":Outgoing message overflow\\n");
                return;
            }

            send_reliable = Netchan_NeedReliable(chan) ? 1 : 0;
            if (chan.reliable_length == 0 && chan.message.cursize != 0)
            {
                System.Array.Copy(chan.message_buf, 0, chan.reliable_buf, 0, chan.message.cursize);
                chan.reliable_length = chan.message.cursize;
                chan.message.cursize = 0;
                chan.reliable_sequence ^= 1;
            }

            SZ.Init(send, send_buf, send_buf.Length);
            w1 = (chan.outgoing_sequence & ~(1 << 31)) | (send_reliable << 31);
            w2 = (chan.incoming_sequence & ~(1 << 31)) | (chan.incoming_reliable_sequence << 31);
            chan.outgoing_sequence++;
            chan.last_sent = (int)Globals.curtime;
            MSG.WriteInt(send, w1);
            MSG.WriteInt(send, w2);
            if (chan.sock == Defines.NS_CLIENT)
                MSG.WriteShort(send, (int)qport.value);
            if (send_reliable != 0)
            {
                SZ.Write(send, chan.reliable_buf, chan.reliable_length);
                chan.last_reliable_sequence = chan.outgoing_sequence;
            }

            if (send.maxsize - send.cursize >= length)
                SZ.Write(send, data, length);
            else
                Com.Printf("Netchan_Transmit: dumped unreliable\\n");
            NET.SendPacket(chan.sock, send.cursize, send.data, chan.remote_address);
            if (showpackets.value != 0)
            {
                if (send_reliable != 0)
                    Com.Printf("send " + send.cursize + " : s=" + (chan.outgoing_sequence - 1) + " reliable=" + chan.reliable_sequence + " ack=" + chan.incoming_sequence + " rack=" + chan.incoming_reliable_sequence + "\\n");
                else
                    Com.Printf("send " + send.cursize + " : s=" + (chan.outgoing_sequence - 1) + " ack=" + chan.incoming_sequence + " rack=" + chan.incoming_reliable_sequence + "\\n");
            }
        }

        public static bool Process(netchan_t chan, sizebuf_t msg)
        {
            MSG.BeginReading(msg);
            int sequence = MSG.ReadLong(msg);
            int sequence_ack = MSG.ReadLong(msg);
            if (chan.sock == Defines.NS_SERVER)
                MSG.ReadShort(msg);
            int reliable_message = sequence >> 31;
            int reliable_ack = sequence_ack >> 31;
            sequence &= ~(1 << 31);
            sequence_ack &= ~(1 << 31);
            if (showpackets.value != 0)
            {
                if (reliable_message != 0)
                    Com.Printf("recv " + msg.cursize + " : s=" + sequence + " reliable=" + (chan.incoming_reliable_sequence ^ 1) + " ack=" + sequence_ack + " rack=" + reliable_ack + "\\n");
                else
                    Com.Printf("recv " + msg.cursize + " : s=" + sequence + " ack=" + sequence_ack + " rack=" + reliable_ack + "\\n");
            }

            if (sequence <= chan.incoming_sequence)
            {
                if (showdrop.value != 0)
                    Com.Printf(NET.AdrToString(chan.remote_address) + ":Out of order packet " + sequence + " at " + chan.incoming_sequence + "\\n");
                return false;
            }

            chan.dropped = sequence - (chan.incoming_sequence + 1);
            if (chan.dropped > 0)
            {
                if (showdrop.value != 0)
                    Com.Printf(NET.AdrToString(chan.remote_address) + ":Dropped " + chan.dropped + " packets at " + sequence + "\\n");
            }

            if (reliable_ack == chan.reliable_sequence)
                chan.reliable_length = 0;
            chan.incoming_sequence = sequence;
            chan.incoming_acknowledged = sequence_ack;
            chan.incoming_reliable_acknowledged = reliable_ack;
            if (reliable_message != 0)
            {
                chan.incoming_reliable_sequence ^= 1;
            }

            chan.last_received = (int)Globals.curtime;
            return true;
        }
    }
}
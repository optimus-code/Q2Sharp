using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Qcommon
{
    public class netchan_t
    {
        public bool fatal_error;
        public int sock;
        public int dropped;
        public int last_received;
        public int last_sent;
        public netadr_t remote_address = new netadr_t();
        public int qport;
        public int incoming_sequence;
        public int incoming_acknowledged;
        public int incoming_reliable_acknowledged;
        public int incoming_reliable_sequence;
        public int outgoing_sequence;
        public int reliable_sequence;
        public int last_reliable_sequence;
        public sizebuf_t message = new sizebuf_t();
        public byte[] message_buf = new byte[Defines.MAX_MSGLEN - 16];
        public int reliable_length;
        public byte[] reliable_buf = new byte[Defines.MAX_MSGLEN - 16];
        public virtual void Clear()
        {
            sock = dropped = last_received = last_sent = 0;
            remote_address = new netadr_t();
            qport = incoming_sequence = incoming_acknowledged = incoming_reliable_acknowledged = incoming_reliable_sequence = outgoing_sequence = reliable_sequence = last_reliable_sequence = 0;
            message = new sizebuf_t();
            message_buf = new byte[Defines.MAX_MSGLEN - 16];
            reliable_length = 0;
            reliable_buf = new byte[Defines.MAX_MSGLEN - 16];
        }
    }
}
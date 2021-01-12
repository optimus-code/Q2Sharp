using J2N.IO;
using J2N.Text;
using Jake2.Game;
using Jake2.Qcommon;
using Jake2.Util;
using Q2Sharp.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Jake2.Sys
{
    public sealed class NET
    {
        private static readonly int MAX_LOOPBACK = 4;
        private static netadr_t net_local_adr = new netadr_t();
        public class loopmsg_t
        {
            public byte[] data = new byte[Defines.MAX_MSGLEN];
            public int datalen;
        }

        public class loopback_t
        {
            public loopback_t()
            {
                msgs = new loopmsg_t[MAX_LOOPBACK];
                for (int n = 0; n < MAX_LOOPBACK; n++)
                {
                    msgs[n] = new loopmsg_t();
                }
            }

            public loopmsg_t[] msgs;
            public int get, send;
        }

        public static loopback_t[] loopbacks = new loopback_t[2];
        static NET()
        {
            loopbacks[0] = new loopback_t();
            loopbacks[1] = new loopback_t();
        }

        //private static DatagramChannel[] ip_channels = new[]{null, null};
        private static DatagramSocket[] ip_sockets = new DatagramSocket[]{null, null};
        public static bool CompareAdr(netadr_t a, netadr_t b)
        {
            return a.ip == b.ip;
        }

        public static bool CompareBaseAdr(netadr_t a, netadr_t b)
        {
            if (a.type != b.type)
                return false;
            if (a.type == Defines.NA_LOOPBACK)
                return true;
            if (a.type == Defines.NA_IP)
            {
                return a.ip == b.ip;
            }

            return false;
        }

        public static string AdrToString(netadr_t a)
        {
            return a.ip.ToString( );
        }

        public static string BaseAdrToString(netadr_t a)
        {
            return a.ip.ToString();
        }

        public static bool StringToAdr(string s, netadr_t a)
        {
            if (s.EqualsIgnoreCase("localhost") || s.EqualsIgnoreCase("loopback"))
            {
                a.Set(net_local_adr);
                return true;
            }

            try
            {
                String[] address = s.Split(":");
                a.ip = IPAddress.Parse( address[0] );
                a.type = Defines.NA_IP;
                if (address.Length == 2)
                    a.port = Lib.Atoi(address[1]);
                return true;
            }
            catch (Exception e)
            {
                Com.Println(e.Message);
                return false;
            }
        }

        public static bool IsLocalAddress(netadr_t adr)
        {
            return CompareAdr(adr, net_local_adr);
        }

        public static bool GetLoopPacket(int sock, netadr_t net_from, sizebuf_t net_message)
        {
            loopback_t loop = loopbacks[sock];
            if (loop.send - loop.get > MAX_LOOPBACK)
                loop.get = loop.send - MAX_LOOPBACK;
            if (loop.get >= loop.send)
                return false;
            int i = loop.get & (MAX_LOOPBACK - 1);
            loop.get++;
            System.Array.Copy(loop.msgs[i].data, 0, net_message.data, 0, loop.msgs[i].datalen);
            net_message.cursize = loop.msgs[i].datalen;
            net_from.Set(net_local_adr);
            return true;
        }

        public static void SendLoopPacket(int sock, int length, byte[] data, netadr_t to)
        {
            int i;
            loopback_t loop;
            loop = loopbacks[sock ^ 1];
            i = loop.send & (MAX_LOOPBACK - 1);
            loop.send++;
            System.Array.Copy(data, 0, loop.msgs[i].data, 0, length);
            loop.msgs[i].datalen = length;
        }

        public static bool GetPacket(int sock, netadr_t net_from, sizebuf_t net_message)
        {
            if (GetLoopPacket(sock, net_from, net_message))
            {
                return true;
            }

            if (ip_sockets[sock] == null)
                return false;
            try
            {
                ByteBuffer receiveBuffer = ByteBuffer.Wrap(net_message.data);
                InetSocketAddress srcSocket = (InetSocketAddress)ip_channels[sock].Receive(receiveBuffer);
                if (srcSocket == null)
                    return false;
                net_from.ip = srcSocket.GetAddress().GetAddress();
                net_from.port = srcSocket.GetPort();
                net_from.type = Defines.NA_IP;
                int packetLength = receiveBuffer.Position;
                if (packetLength > net_message.maxsize)
                {
                    Com.Println("Oversize packet from " + AdrToString(net_from));
                    return false;
                }

                net_message.cursize = packetLength;
                net_message.data[packetLength] = 0;
                return true;
            }
            catch (Exception e)
            {
                Com.DPrintf("NET_GetPacket: " + e + " from " + AdrToString(net_from) + "\\n");
                return false;
            }
        }

        public static void SendPacket(int sock, int length, byte[] data, netadr_t to)
        {
            if (to.type == Defines.NA_LOOPBACK)
            {
                SendLoopPacket(sock, length, data, to);
                return;
            }

            if (ip_sockets[sock] == null)
                return;
            if (to.type != Defines.NA_BROADCAST && to.type != Defines.NA_IP)
            {
                Com.Error(Defines.ERR_FATAL, "NET_SendPacket: bad address type");
                return;
            }

            try
            {
                IPEndPoint dstSocket = new IPEndPoint(to.GetInetAddress(), to.port);
                ip_channels[sock].Send(ByteBuffer.Wrap(data, 0, length), dstSocket);
            }
            catch (Exception e)
            {
                Com.Println("NET_SendPacket ERROR: " + e + " to " + AdrToString(to));
            }
        }

        public static void OpenIP()
        {
            cvar_t port, ip, clientport;
            port = Cvar.Get("port", "" + Defines.PORT_SERVER, Defines.CVAR_NOSET);
            ip = Cvar.Get("ip", "localhost", Defines.CVAR_NOSET);
            clientport = Cvar.Get("clientport", "" + Defines.PORT_CLIENT, Defines.CVAR_NOSET);
            if (ip_sockets[Defines.NS_SERVER] == null)
                ip_sockets[Defines.NS_SERVER] = Socket(Defines.NS_SERVER, ip.string_renamed, (int)port.value);
            if (ip_sockets[Defines.NS_CLIENT] == null)
                ip_sockets[Defines.NS_CLIENT] = Socket(Defines.NS_CLIENT, ip.string_renamed, (int)clientport.value);
            if (ip_sockets[Defines.NS_CLIENT] == null)
                ip_sockets[Defines.NS_CLIENT] = Socket(Defines.NS_CLIENT, ip.string_renamed, Defines.PORT_ANY);
        }

        public static void Config(bool multiplayer)
        {
            if (!multiplayer)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (ip_sockets[i] != null)
                    {
                        ip_sockets[i].Close();
                        ip_sockets[i] = null;
                    }
                }
            }
            else
            {
                OpenIP();
            }
        }

        public static void Init()
        {
        }

        public static DatagramSocket Socket(int sock, string ip, int port)
        {
            DatagramSocket newsocket = null;
            try
            {
                if (ip_channels[sock] == null || !ip_channels[sock].IsOpen())
                    ip_channels[sock] =  new DatagramSocket();// DatagramChannel.Open();
                if (ip == null || ip.Length == 0 || ip.Equals("localhost"))
                {
                    if (port == Defines.PORT_ANY)
                    {
                        newsocket = ip_channels[sock].Socket();
                        newsocket.Bind(0);
                    }
                    else
                    {
                        newsocket = ip_channels[sock].Socket();
                        newsocket.Bind(port);
                    }
                }
                else
                {
                    newsocket = ip_channels[sock].Socket();
                    newsocket.Bind( ip, port);
                }

                ip_channels[sock].ConfigureBlocking(false);
                newsocket.EnableBroadcast = true;
            }
            catch (Exception e)
            {
                Com.Println("Error: " + e.ToString());
                newsocket = null;
            }

            return newsocket;
        }

        public static void Shutdown()
        {
            Config(false);
        }

        public static void Sleep(int msec)
        {
            if (ip_sockets[Defines.NS_SERVER] == null || (Globals.dedicated != null && Globals.dedicated.value == 0))
                return;
            try
            {
                Thread.Sleep(msec);
            }
            catch (Exception e)
            {
            }
        }
    }
}
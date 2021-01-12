using J2N.IO;
using System.Net;
using System.Net.Sockets;

namespace Q2Sharp.util
{
	public class DatagramSocket
	{
		private UdpClient Socket
		{
			get;
			set;
		}

		public bool EnableBroadcast
		{
			get
			{
				return Socket.EnableBroadcast;
			}
			set
			{
				Socket.EnableBroadcast = value;
			}
		}

		private IPEndPoint EndPoint;

		public void Bind( int port )
		{
			Bind( "127.0.0.1", port );
		}

		public void Close( )
		{
			Socket.Close();
			Socket.Dispose();
		}

		public void Bind( string hostname, int port )
		{
			Socket = new UdpClient( hostname, port );
		}

		public ByteBuffer Receive( )
		{
			return ByteBuffer.Wrap( Socket.Receive( ref EndPoint ) );
		}

		public int Send( ByteBuffer buffer, IPEndPoint endPoint )
		{
			return Socket.Send( buffer.Array, buffer.Array.Length, endPoint );
		}

		private volatile bool _isDisposed;
		public void Dispose( )
		{
			_isDisposed = true;
			Socket.Dispose();
		}
	}
}

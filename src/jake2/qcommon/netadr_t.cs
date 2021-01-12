using Q2Sharp.Sys;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Q2Sharp.Qcommon
{
	public class netadr_t
	{
		public Int32 type;
		public Int32 port;
		public IPAddress ip;

		public netadr_t( )
		{
			type = Defines.NA_LOOPBACK;
			port = 0;

			if ( !System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() )
			{
				return;
			}

			var host = Dns.GetHostEntry( Dns.GetHostName() );

			ip = host
				.AddressList
				.FirstOrDefault( ip => ip.AddressFamily == AddressFamily.InterNetwork );
		}

		public virtual IPAddress GetInetAddress( )
		{
			switch ( type )

			{
				case Defines.NA_BROADCAST:
					return IPAddress.Parse( "255.255.255.255" );
				case Defines.NA_LOOPBACK:
					return IPAddress.Parse( "127.0.0.1" );
				case Defines.NA_IP:
					return ip;
				default:
					return null;
			}
		}

		public virtual void Set( netadr_t from )
		{
			type = from.type;
			port = from.port;
			ip = from.ip;
		}

		public override String ToString( )
		{
			return ( type == Defines.NA_LOOPBACK ) ? "loopback" : NET.AdrToString( this );
		}
	}
}
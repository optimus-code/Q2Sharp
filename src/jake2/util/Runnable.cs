using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jake2Sharp.util
{
	public abstract class Runnable : IDisposable
	{
		private Task Handle
		{
			get;
			set;
		}

		private CancellationTokenSource CancelToken
		{
			get;
			set;
		}

		protected abstract void Execute( );

		public Runnable()
		{
		}

		public void Run()
		{
			CancelToken = new CancellationTokenSource();

			Handle = Task.Run( ( ) =>
			{
				Execute();
			}, CancelToken.Token );
		}

		public void Dispose( )
		{
			if ( Handle != null )
			{
				if ( Handle.Status == TaskStatus.Running )
				{
					CancelToken.Cancel();
					Handle.ContinueWith( ( t ) => { Handle.Dispose(); } );
				}
				else
					Handle.Dispose();
			}
		}
	}
}

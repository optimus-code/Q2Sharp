using Jake2.Qcommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Q2Sharp.win
{
	public partial class Q2DataDialog : Form
	{
		private String BasePath
		{
			get;
			set;
		}

		public Q2DataDialog( )
		{
			InitializeComponent();
			BasePath = System.IO.Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
		}

		private void Q2DataDialog_Load( Object sender, EventArgs e )
		{
			Width = 416;
			Height = 364;
			SelectedPath.Text = BasePath + "\\baseq2";
			DownloadDestinationPath.Text = BasePath + "\\baseq2";
			LoadingPanel.Visible = true;
			NotFoundPanel.Visible = false;
			DownloadingPanel.Visible = false;
			DownloadDemoPanel.Visible = false;
		}

		private void ShowChooseDialog()
		{
			LoadingPanel.Visible = false;
			DownloadingPanel.Visible = false;
			DownloadDemoPanel.Visible = false;
			NotFoundPanel.Visible = false;
			ChooseInstallPanel.Visible = true;
		}

		private void ShowStatus( )
		{
			LoadingPanel.Visible = true;
			DownloadingPanel.Visible = false;
			DownloadDemoPanel.Visible = false;
			NotFoundPanel.Visible = false;
			ChooseInstallPanel.Visible = false;
		}
		
		private void ShowDownloadDemoPanel( )
		{
			DownloadDemoPanel.Visible = true;
			DownloadingPanel.Visible = false;
			LoadingPanel.Visible = false;
			NotFoundPanel.Visible = false;
			ChooseInstallPanel.Visible = false;
		}

		private void ShowDownloadingPanel( )
		{
			DownloadingPanel.Visible = true;
			LoadingPanel.Visible = false;
			DownloadDemoPanel.Visible = false;
			NotFoundPanel.Visible = false;
			ChooseInstallPanel.Visible = false;
		}

		private void ShowNotFoundPanel( )
		{
			LoadingPanel.Visible = false;
			DownloadingPanel.Visible = false;
			DownloadDemoPanel.Visible = false;
			NotFoundPanel.Visible = true;
			ChooseInstallPanel.Visible = false;
		}

		public void SetStatus( string text )
		{
			StatusText.Text = text;
		}

		public virtual void TestQ2Data( )
		{
			while ( FS.LoadFile( "pics/colormap.pcx" ) == null )
			{
				ShowNotFoundPanel();
				try
				{
					lock ( this )
					{
						Wait();
					}
				}
				catch ( Exception e )
				{
				}
			}

			ShowStatus();
			Update();
		}

		private void ChoosePathOkButton_Click( Object sender, EventArgs e )
		{
			var dir = SelectedPath.Text;
			if ( !string.IsNullOrEmpty( dir ) && System.IO.Directory.Exists( dir ) )
			{
				Cvar.Set( "cddir", dir );
				FS.SetCDDir();
			}
		}


	}
}


namespace Q2Sharp.win
{
	partial class Q2DataDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Q2DataDialog));
			this.StatusText = new System.Windows.Forms.Label();
			this.LoadingPanel = new System.Windows.Forms.Panel();
			this.ContainerPanel = new System.Windows.Forms.Panel();
			this.NotFoundPanel = new System.Windows.Forms.Panel();
			this.ChooseDownload = new System.Windows.Forms.RadioButton();
			this.ChooseExisting = new System.Windows.Forms.RadioButton();
			this.NoDataOkButton = new System.Windows.Forms.Button();
			this.NoDataCloseButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.ChooseInstallPanel = new System.Windows.Forms.Panel();
			this.ChoosePathButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.SelectedPath = new System.Windows.Forms.TextBox();
			this.ChoosePathOkButton = new System.Windows.Forms.Button();
			this.ChoosePathCloseButton = new System.Windows.Forms.Button();
			this.ChoosePathCancelButton = new System.Windows.Forms.Button();
			this.DownloadDemoPanel = new System.Windows.Forms.Panel();
			this.DownloadCancelButton = new System.Windows.Forms.Button();
			this.DownloadOkButton = new System.Windows.Forms.Button();
			this.DownloadCloseButton = new System.Windows.Forms.Button();
			this.DemoDownloadURL = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.DownloadDestinationPath = new System.Windows.Forms.TextBox();
			this.ChooseDownloadDirectoryButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.DownloadingPanel = new System.Windows.Forms.Panel();
			this.DownloadingStatusText = new System.Windows.Forms.Label();
			this.DownloadingCancel = new System.Windows.Forms.Button();
			this.DownloadingOk = new System.Windows.Forms.Button();
			this.DownloadingClose = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.LoadingPanel.SuspendLayout();
			this.ContainerPanel.SuspendLayout();
			this.NotFoundPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.ChooseInstallPanel.SuspendLayout();
			this.DownloadDemoPanel.SuspendLayout();
			this.DownloadingPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// StatusText
			// 
			this.StatusText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatusText.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.StatusText.Location = new System.Drawing.Point(0, 0);
			this.StatusText.Margin = new System.Windows.Forms.Padding(0);
			this.StatusText.Name = "StatusText";
			this.StatusText.Padding = new System.Windows.Forms.Padding(5);
			this.StatusText.Size = new System.Drawing.Size(400, 150);
			this.StatusText.TabIndex = 0;
			this.StatusText.Text = "initialising Q2Sharp...";
			this.StatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LoadingPanel
			// 
			this.LoadingPanel.Controls.Add(this.StatusText);
			this.LoadingPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.LoadingPanel.Location = new System.Drawing.Point(0, 888);
			this.LoadingPanel.Margin = new System.Windows.Forms.Padding(0);
			this.LoadingPanel.Name = "LoadingPanel";
			this.LoadingPanel.Size = new System.Drawing.Size(400, 150);
			this.LoadingPanel.TabIndex = 1;
			this.LoadingPanel.Visible = false;
			// 
			// ContainerPanel
			// 
			this.ContainerPanel.Controls.Add(this.DownloadingPanel);
			this.ContainerPanel.Controls.Add(this.DownloadDemoPanel);
			this.ContainerPanel.Controls.Add(this.ChooseInstallPanel);
			this.ContainerPanel.Controls.Add(this.NotFoundPanel);
			this.ContainerPanel.Controls.Add(this.pictureBox1);
			this.ContainerPanel.Controls.Add(this.LoadingPanel);
			this.ContainerPanel.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.ContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ContainerPanel.Location = new System.Drawing.Point(0, 0);
			this.ContainerPanel.Margin = new System.Windows.Forms.Padding(0);
			this.ContainerPanel.Name = "ContainerPanel";
			this.ContainerPanel.Size = new System.Drawing.Size(400, 1038);
			this.ContainerPanel.TabIndex = 3;
			// 
			// NotFoundPanel
			// 
			this.NotFoundPanel.Controls.Add(this.label7);
			this.NotFoundPanel.Controls.Add(this.ChooseDownload);
			this.NotFoundPanel.Controls.Add(this.ChooseExisting);
			this.NotFoundPanel.Controls.Add(this.NoDataOkButton);
			this.NotFoundPanel.Controls.Add(this.NoDataCloseButton);
			this.NotFoundPanel.Controls.Add(this.label2);
			this.NotFoundPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.NotFoundPanel.Location = new System.Drawing.Point(0, 723);
			this.NotFoundPanel.Name = "NotFoundPanel";
			this.NotFoundPanel.Padding = new System.Windows.Forms.Padding(10);
			this.NotFoundPanel.Size = new System.Drawing.Size(400, 165);
			this.NotFoundPanel.TabIndex = 2;
			// 
			// ChooseDownload
			// 
			this.ChooseDownload.AutoSize = true;
			this.ChooseDownload.BackColor = System.Drawing.Color.Transparent;
			this.ChooseDownload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.ChooseDownload.Location = new System.Drawing.Point(22, 91);
			this.ChooseDownload.Name = "ChooseDownload";
			this.ChooseDownload.Padding = new System.Windows.Forms.Padding(2);
			this.ChooseDownload.Size = new System.Drawing.Size(295, 23);
			this.ChooseDownload.TabIndex = 4;
			this.ChooseDownload.TabStop = true;
			this.ChooseDownload.Text = "Download and install Quake2 demo data (10MB)";
			this.ChooseDownload.UseVisualStyleBackColor = false;
			// 
			// ChooseExisting
			// 
			this.ChooseExisting.AutoSize = true;
			this.ChooseExisting.BackColor = System.Drawing.Color.Transparent;
			this.ChooseExisting.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.ChooseExisting.Location = new System.Drawing.Point(22, 59);
			this.ChooseExisting.Name = "ChooseExisting";
			this.ChooseExisting.Padding = new System.Windows.Forms.Padding(2);
			this.ChooseExisting.Size = new System.Drawing.Size(352, 23);
			this.ChooseExisting.TabIndex = 3;
			this.ChooseExisting.TabStop = true;
			this.ChooseExisting.Text = "Choose baseq2 directory from existing Quake2 installation";
			this.ChooseExisting.UseVisualStyleBackColor = false;
			// 
			// NoDataOkButton
			// 
			this.NoDataOkButton.Location = new System.Drawing.Point(303, 126);
			this.NoDataOkButton.Name = "NoDataOkButton";
			this.NoDataOkButton.Size = new System.Drawing.Size(85, 25);
			this.NoDataOkButton.TabIndex = 2;
			this.NoDataOkButton.Text = "OK";
			this.NoDataOkButton.UseVisualStyleBackColor = true;
			// 
			// NoDataCloseButton
			// 
			this.NoDataCloseButton.Location = new System.Drawing.Point(208, 126);
			this.NoDataCloseButton.Name = "NoDataCloseButton";
			this.NoDataCloseButton.Size = new System.Drawing.Size(85, 25);
			this.NoDataCloseButton.TabIndex = 1;
			this.NoDataCloseButton.Text = "Close";
			this.NoDataCloseButton.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.label2.Location = new System.Drawing.Point(12, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 15);
			this.label2.TabIndex = 0;
			this.label2.Text = "Failed to locate game data";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(400, 200);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// ChooseInstallPanel
			// 
			this.ChooseInstallPanel.Controls.Add(this.label6);
			this.ChooseInstallPanel.Controls.Add(this.label5);
			this.ChooseInstallPanel.Controls.Add(this.ChoosePathCancelButton);
			this.ChooseInstallPanel.Controls.Add(this.ChoosePathOkButton);
			this.ChooseInstallPanel.Controls.Add(this.ChoosePathCloseButton);
			this.ChooseInstallPanel.Controls.Add(this.SelectedPath);
			this.ChooseInstallPanel.Controls.Add(this.ChoosePathButton);
			this.ChooseInstallPanel.Controls.Add(this.label3);
			this.ChooseInstallPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ChooseInstallPanel.Location = new System.Drawing.Point(0, 558);
			this.ChooseInstallPanel.Margin = new System.Windows.Forms.Padding(0);
			this.ChooseInstallPanel.Name = "ChooseInstallPanel";
			this.ChooseInstallPanel.Padding = new System.Windows.Forms.Padding(10);
			this.ChooseInstallPanel.Size = new System.Drawing.Size(400, 165);
			this.ChooseInstallPanel.TabIndex = 4;
			this.ChooseInstallPanel.Visible = false;
			// 
			// ChoosePathButton
			// 
			this.ChoosePathButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.ChoosePathButton.Location = new System.Drawing.Point(339, 79);
			this.ChoosePathButton.Name = "ChoosePathButton";
			this.ChoosePathButton.Size = new System.Drawing.Size(48, 24);
			this.ChoosePathButton.TabIndex = 2;
			this.ChoosePathButton.Text = "...";
			this.ChoosePathButton.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label3.Location = new System.Drawing.Point(12, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(139, 15);
			this.label3.TabIndex = 1;
			this.label3.Text = "Quake 2 Install Location";
			// 
			// SelectedPath
			// 
			this.SelectedPath.Location = new System.Drawing.Point(15, 80);
			this.SelectedPath.Name = "SelectedPath";
			this.SelectedPath.Size = new System.Drawing.Size(313, 23);
			this.SelectedPath.TabIndex = 3;
			// 
			// ChoosePathOkButton
			// 
			this.ChoosePathOkButton.Location = new System.Drawing.Point(303, 124);
			this.ChoosePathOkButton.Name = "ChoosePathOkButton";
			this.ChoosePathOkButton.Size = new System.Drawing.Size(85, 25);
			this.ChoosePathOkButton.TabIndex = 6;
			this.ChoosePathOkButton.Text = "OK";
			this.ChoosePathOkButton.UseVisualStyleBackColor = true;
			this.ChoosePathOkButton.Click += new System.EventHandler(this.ChoosePathOkButton_Click);
			// 
			// ChoosePathCloseButton
			// 
			this.ChoosePathCloseButton.Location = new System.Drawing.Point(113, 124);
			this.ChoosePathCloseButton.Name = "ChoosePathCloseButton";
			this.ChoosePathCloseButton.Size = new System.Drawing.Size(85, 25);
			this.ChoosePathCloseButton.TabIndex = 5;
			this.ChoosePathCloseButton.Text = "Close";
			this.ChoosePathCloseButton.UseVisualStyleBackColor = true;
			// 
			// ChoosePathCancelButton
			// 
			this.ChoosePathCancelButton.Location = new System.Drawing.Point(208, 124);
			this.ChoosePathCancelButton.Name = "ChoosePathCancelButton";
			this.ChoosePathCancelButton.Size = new System.Drawing.Size(85, 25);
			this.ChoosePathCancelButton.TabIndex = 7;
			this.ChoosePathCancelButton.Text = "Cancel";
			this.ChoosePathCancelButton.UseVisualStyleBackColor = true;
			// 
			// DownloadDemoPanel
			// 
			this.DownloadDemoPanel.Controls.Add(this.DownloadDestinationPath);
			this.DownloadDemoPanel.Controls.Add(this.ChooseDownloadDirectoryButton);
			this.DownloadDemoPanel.Controls.Add(this.label4);
			this.DownloadDemoPanel.Controls.Add(this.DownloadCancelButton);
			this.DownloadDemoPanel.Controls.Add(this.DownloadOkButton);
			this.DownloadDemoPanel.Controls.Add(this.DownloadCloseButton);
			this.DownloadDemoPanel.Controls.Add(this.DemoDownloadURL);
			this.DownloadDemoPanel.Controls.Add(this.label1);
			this.DownloadDemoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.DownloadDemoPanel.Location = new System.Drawing.Point(0, 393);
			this.DownloadDemoPanel.Margin = new System.Windows.Forms.Padding(0);
			this.DownloadDemoPanel.Name = "DownloadDemoPanel";
			this.DownloadDemoPanel.Padding = new System.Windows.Forms.Padding(10);
			this.DownloadDemoPanel.Size = new System.Drawing.Size(400, 165);
			this.DownloadDemoPanel.TabIndex = 5;
			this.DownloadDemoPanel.Visible = false;
			// 
			// DownloadCancelButton
			// 
			this.DownloadCancelButton.Location = new System.Drawing.Point(208, 128);
			this.DownloadCancelButton.Name = "DownloadCancelButton";
			this.DownloadCancelButton.Size = new System.Drawing.Size(85, 25);
			this.DownloadCancelButton.TabIndex = 7;
			this.DownloadCancelButton.Text = "Cancel";
			this.DownloadCancelButton.UseVisualStyleBackColor = true;
			// 
			// DownloadOkButton
			// 
			this.DownloadOkButton.Location = new System.Drawing.Point(303, 128);
			this.DownloadOkButton.Name = "DownloadOkButton";
			this.DownloadOkButton.Size = new System.Drawing.Size(85, 25);
			this.DownloadOkButton.TabIndex = 6;
			this.DownloadOkButton.Text = "Download";
			this.DownloadOkButton.UseVisualStyleBackColor = true;
			// 
			// DownloadCloseButton
			// 
			this.DownloadCloseButton.Location = new System.Drawing.Point(113, 128);
			this.DownloadCloseButton.Name = "DownloadCloseButton";
			this.DownloadCloseButton.Size = new System.Drawing.Size(85, 25);
			this.DownloadCloseButton.TabIndex = 5;
			this.DownloadCloseButton.Text = "Close";
			this.DownloadCloseButton.UseVisualStyleBackColor = true;
			// 
			// DemoDownloadURL
			// 
			this.DemoDownloadURL.Location = new System.Drawing.Point(15, 40);
			this.DemoDownloadURL.Name = "DemoDownloadURL";
			this.DemoDownloadURL.Size = new System.Drawing.Size(372, 23);
			this.DemoDownloadURL.TabIndex = 3;
			this.DemoDownloadURL.Text = "https://archive.org/download/QuakeII_1020/q2_test.zip";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(89, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Download URL";
			// 
			// DownloadDestinationPath
			// 
			this.DownloadDestinationPath.Location = new System.Drawing.Point(15, 92);
			this.DownloadDestinationPath.Name = "DownloadDestinationPath";
			this.DownloadDestinationPath.Size = new System.Drawing.Size(313, 23);
			this.DownloadDestinationPath.TabIndex = 10;
			// 
			// ChooseDownloadDirectoryButton
			// 
			this.ChooseDownloadDirectoryButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.ChooseDownloadDirectoryButton.Location = new System.Drawing.Point(339, 92);
			this.ChooseDownloadDirectoryButton.Name = "ChooseDownloadDirectoryButton";
			this.ChooseDownloadDirectoryButton.Size = new System.Drawing.Size(48, 23);
			this.ChooseDownloadDirectoryButton.TabIndex = 9;
			this.ChooseDownloadDirectoryButton.Text = "...";
			this.ChooseDownloadDirectoryButton.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label4.Location = new System.Drawing.Point(12, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(90, 15);
			this.label4.TabIndex = 8;
			this.label4.Text = "Install Location";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.label5.Location = new System.Drawing.Point(12, 36);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(338, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "If you already have the game installed, find the directory that contains";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.label6.Location = new System.Drawing.Point(12, 54);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(90, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "the baseq2 folder.";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.label7.Location = new System.Drawing.Point(12, 35);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(332, 15);
			this.label7.TabIndex = 10;
			this.label7.Text = "No configuration and could not find baseq2 folder in exe path.";
			// 
			// DownloadingPanel
			// 
			this.DownloadingPanel.Controls.Add(this.progressBar1);
			this.DownloadingPanel.Controls.Add(this.DownloadingStatusText);
			this.DownloadingPanel.Controls.Add(this.DownloadingCancel);
			this.DownloadingPanel.Controls.Add(this.DownloadingOk);
			this.DownloadingPanel.Controls.Add(this.DownloadingClose);
			this.DownloadingPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.DownloadingPanel.Location = new System.Drawing.Point(0, 228);
			this.DownloadingPanel.Margin = new System.Windows.Forms.Padding(0);
			this.DownloadingPanel.Name = "DownloadingPanel";
			this.DownloadingPanel.Padding = new System.Windows.Forms.Padding(10);
			this.DownloadingPanel.Size = new System.Drawing.Size(400, 165);
			this.DownloadingPanel.TabIndex = 6;
			this.DownloadingPanel.Visible = false;
			// 
			// DownloadingStatusText
			// 
			this.DownloadingStatusText.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.DownloadingStatusText.Location = new System.Drawing.Point(12, 26);
			this.DownloadingStatusText.Name = "DownloadingStatusText";
			this.DownloadingStatusText.Size = new System.Drawing.Size(376, 40);
			this.DownloadingStatusText.TabIndex = 9;
			this.DownloadingStatusText.Text = "Downloading...0%";
			this.DownloadingStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DownloadingCancel
			// 
			this.DownloadingCancel.Location = new System.Drawing.Point(208, 124);
			this.DownloadingCancel.Name = "DownloadingCancel";
			this.DownloadingCancel.Size = new System.Drawing.Size(85, 25);
			this.DownloadingCancel.TabIndex = 7;
			this.DownloadingCancel.Text = "Cancel";
			this.DownloadingCancel.UseVisualStyleBackColor = true;
			// 
			// DownloadingOk
			// 
			this.DownloadingOk.Enabled = false;
			this.DownloadingOk.Location = new System.Drawing.Point(303, 124);
			this.DownloadingOk.Name = "DownloadingOk";
			this.DownloadingOk.Size = new System.Drawing.Size(85, 25);
			this.DownloadingOk.TabIndex = 6;
			this.DownloadingOk.Text = "OK";
			this.DownloadingOk.UseVisualStyleBackColor = true;
			// 
			// DownloadingClose
			// 
			this.DownloadingClose.Location = new System.Drawing.Point(113, 124);
			this.DownloadingClose.Name = "DownloadingClose";
			this.DownloadingClose.Size = new System.Drawing.Size(85, 25);
			this.DownloadingClose.TabIndex = 5;
			this.DownloadingClose.Text = "Close";
			this.DownloadingClose.UseVisualStyleBackColor = true;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(15, 69);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(373, 23);
			this.progressBar1.TabIndex = 10;
			// 
			// Q2DataDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(400, 1038);
			this.Controls.Add(this.ContainerPanel);
			this.MaximizeBox = false;
			this.Name = "Q2DataDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Q2Sharp";
			this.Load += new System.EventHandler(this.Q2DataDialog_Load);
			this.LoadingPanel.ResumeLayout(false);
			this.ContainerPanel.ResumeLayout(false);
			this.NotFoundPanel.ResumeLayout(false);
			this.NotFoundPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ChooseInstallPanel.ResumeLayout(false);
			this.ChooseInstallPanel.PerformLayout();
			this.DownloadDemoPanel.ResumeLayout(false);
			this.DownloadDemoPanel.PerformLayout();
			this.DownloadingPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label status;
		private System.Windows.Forms.Label StatusText;
		private System.Windows.Forms.Panel ChoosePanelhh;
		private System.Windows.Forms.Panel LoadingPanel;
		private System.Windows.Forms.Panel ContainerPanel;
		private System.Windows.Forms.Panel NotFoundPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button NoDataCloseButton;
		private System.Windows.Forms.Button NoDataOkButton;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.RadioButton ChooseExisting;
		private System.Windows.Forms.RadioButton ChooseDownload;
		private System.Windows.Forms.Panel ChooseInstallPanel;
		private System.Windows.Forms.Button ChoosePathButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox SelectedPath;
		private System.Windows.Forms.Button ChoosePathOkButton;
		private System.Windows.Forms.Button ChoosePathCloseButton;
		private System.Windows.Forms.Button ChoosePathCancelButton;
		private System.Windows.Forms.Panel DownloadDemoPanel;
		private System.Windows.Forms.Button DownloadCancelButton;
		private System.Windows.Forms.Button DownloadOkButton;
		private System.Windows.Forms.Button DownloadCloseButton;
		private System.Windows.Forms.TextBox DemoDownloadURL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox DownloadDestinationPath;
		private System.Windows.Forms.Button ChooseDownloadDirectoryButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Panel DownloadingPanel;
		private System.Windows.Forms.Label DownloadingStatusText;
		private System.Windows.Forms.Button DownloadingCancel;
		private System.Windows.Forms.Button DownloadingOk;
		private System.Windows.Forms.Button DownloadingClose;
		private System.Windows.Forms.ProgressBar progressBar1;
	}
}
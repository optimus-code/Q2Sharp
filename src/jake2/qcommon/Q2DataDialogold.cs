using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Q2Sharp.Qcommon
{
    public class Q2DataDialog : JDialog
    {
        static readonly string home = System.GetProperty("user.home");
        static readonly string sep = System.GetProperty("file.separator");
        public Q2DataDialog(): base()
        {
            InitComponents();
            VideoMode mode = GraphicsEnvironment.GetLocalGraphicsEnvironment().GetDefaultScreenDevice().GetDisplayMode();
            int x = (mode.GetWidth() - GetWidth()) / 2;
            int y = (mode.GetHeight() - GetHeight()) / 2;
            SetLocation(x, y);
            dir = home + sep + "Jake2" + sep + "baseq2";
            jTextField1.SetText(dir);
        }

        private void InitComponents()
        {
            JComponent.SetDefaultLocale(Locale.US);
            java.awt.GridBagConstraints gridBagConstraints;
            choosePanel = new JPanel();
            statusPanel = new JPanel();
            status = new JLabel("initializing Q2Sharp...");
            jTextField1 = new JTextField();
            changeButton = new JButton();
            cancelButton = new JButton();
            exitButton = new JButton();
            okButton = new JButton();
            SetDefaultCloseOperation(javax.swing.WindowConstants.DO_NOTHING_ON_CLOSE);
            SetTitle("Jake2 - Bytonic Software");
            SetResizable(false);
            AddWindowListener(new AnonymousWindowAdapter(this));
            choosePanel.SetLayout(new GridBagLayout());
            choosePanel.SetMaximumSize(new Size(400, 100));
            choosePanel.SetMinimumSize(new Size( 400, 100));
            choosePanel.SetPreferredSize(new Size( 400, 100));
            gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridy = 0;
            gridBagConstraints.gridwidth = 1;
            gridBagConstraints.insets = new Insets(5, 5, 5, 5);
            gridBagConstraints.weightx = 0;
            gridBagConstraints.anchor = GridBagConstraints.SOUTHWEST;
            choosePanel.Add(new JLabel("baseq2 directory"), gridBagConstraints);
            gridBagConstraints.gridx = 1;
            gridBagConstraints.gridy = 0;
            gridBagConstraints.gridwidth = 2;
            gridBagConstraints.fill = java.awt.GridBagConstraints.BOTH;
            gridBagConstraints.insets = new Insets(5, 2, 5, 2);
            gridBagConstraints.weightx = 1;
            choosePanel.Add(jTextField1, gridBagConstraints);
            changeButton.SetText("...");
            changeButton.AddActionListener(new AnonymousActionListener(this));
            gridBagConstraints.gridx = 3;
            gridBagConstraints.gridy = 0;
            gridBagConstraints.gridwidth = 1;
            gridBagConstraints.weightx = 0;
            gridBagConstraints.fill = java.awt.GridBagConstraints.NONE;
            gridBagConstraints.insets = new Insets(5, 2, 5, 5);
            gridBagConstraints.anchor = java.awt.GridBagConstraints.EAST;
            choosePanel.Add(changeButton, gridBagConstraints);
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridy = 1;
            gridBagConstraints.gridwidth = 4;
            gridBagConstraints.weightx = 0;
            gridBagConstraints.weighty = 1;
            gridBagConstraints.fill = java.awt.GridBagConstraints.VERTICAL;
            choosePanel.Add(new JPanel(), gridBagConstraints);
            cancelButton.SetText("Cancel");
            cancelButton.AddActionListener(new AnonymousActionListener1(this));
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridy = 2;
            gridBagConstraints.gridwidth = 4;
            gridBagConstraints.weighty = 0;
            gridBagConstraints.insets = new Insets(5, 5, 5, 5);
            gridBagConstraints.anchor = java.awt.GridBagConstraints.SOUTH;
            choosePanel.Add(cancelButton, gridBagConstraints);
            exitButton.SetText("Exit");
            exitButton.AddActionListener(new AnonymousActionListener2(this));
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridy = 2;
            gridBagConstraints.gridwidth = 1;
            gridBagConstraints.anchor = java.awt.GridBagConstraints.SOUTHWEST;
            choosePanel.Add(exitButton, gridBagConstraints);
            okButton.SetText("OK");
            okButton.AddActionListener(new AnonymousActionListener3(this));
            gridBagConstraints.gridx = 2;
            gridBagConstraints.gridy = 2;
            gridBagConstraints.gridwidth = 2;
            gridBagConstraints.anchor = java.awt.GridBagConstraints.SOUTHEAST;
            choosePanel.Add(okButton, gridBagConstraints);
            Jake2Canvas c = new Jake2Canvas();
            GetContentPane().Add(c, BorderLayout.CENTER);
            statusPanel.SetLayout(new GridBagLayout());
            statusPanel.SetMaximumSize(new Dimension(400, 100));
            statusPanel.SetMinimumSize(new Dimension(400, 100));
            statusPanel.SetPreferredSize(new Dimension(400, 100));
            gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridy = 0;
            gridBagConstraints.gridwidth = 1;
            gridBagConstraints.fill = java.awt.GridBagConstraints.HORIZONTAL;
            gridBagConstraints.insets = new Insets(10, 10, 10, 10);
            gridBagConstraints.weightx = 1;
            statusPanel.Add(status, gridBagConstraints);
            GetContentPane().Add(statusPanel, java.awt.BorderLayout.SOUTH);
            progressPanel = new ProgressPanel(this);
            installPanel = new InstallPanel(this);
            notFoundPanel = new NotFoundPanel(this);
            Pack();
        }

        private sealed class AnonymousWindowAdapter : WindowAdapter
        {
            public AnonymousWindowAdapter(Q2DataDialog parent)
            {
                this.parent = parent;
            }

            private readonly Q2DataDialog parent;
            public void WindowClosing(java.awt.event.WindowEvent  evt)
            {
                FormWindowClosing(evt);
            }
        }

        private sealed class AnonymousActionListener : ActionListener
        {
            public AnonymousActionListener(Q2DataDialog parent)
            {
                this.parent = parent;
            }

            private readonly Q2DataDialog parent;
            public void ActionPerformed(java.awt.event.ActionEvent  evt)
            {
                ChangeButtonActionPerformed(evt);
            }
        }

        private sealed class AnonymousActionListener1 : ActionListener
        {
            public AnonymousActionListener1(Q2DataDialog parent)
            {
                this.parent = parent;
            }

            private readonly Q2DataDialog parent;
            public void ActionPerformed(java.awt. event . ActionEvent  evt)
            {
                CancelButtonActionPerformed(evt);
            }
        }

        private sealed class AnonymousActionListener2 : ActionListener
        {
            public AnonymousActionListener2(Q2DataDialog parent)
            {
                this.parent = parent;
            }

            private readonly Q2DataDialog parent;
            public void ActionPerformed(java.awt. event . ActionEvent  evt)
            {
                ExitButtonActionPerformed(evt);
            }
        }

        private sealed class AnonymousActionListener3 : ActionListener
        {
            public AnonymousActionListener3(Q2DataDialog parent)
            {
                this.parent = parent;
            }

            private readonly Q2DataDialog parent;
            public void ActionPerformed(java.awt. event . ActionEvent  evt)
            {
                OkButtonActionPerformed(evt);
            }
        }

        private void CancelButtonActionPerformed(java.awt. event . ActionEvent  evt)
        {
            ShowNotFoundPanel();
        }

        private void ExitButtonActionPerformed(java.awt. event . ActionEvent  evt)
        {
            if (!Globals.appletMode)
            {
                System.Exit(1);
            }

            Dispose();
        }

        private void OkButtonActionPerformed(java.awt. event . ActionEvent  evt)
        {
            dir = jTextField1.GetText();
            if (dir != null)
            {
                Cvar.Set("cddir", dir);
                FS.SetCDDir();
            }

            lock (this)
            {
                NotifyAll();
            }
        }

        private void ChangeButtonActionPerformed(java.awt. event . ActionEvent  evt)
        {
            JFileChooser chooser = new JFileChooser();
            chooser.SetFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
            chooser.SetDialogType(JFileChooser.CUSTOM_DIALOG);
            chooser.SetMultiSelectionEnabled(false);
            chooser.SetDialogTitle("choose a valid baseq2 directory");
            chooser.ShowDialog(this, "OK");
            dir = null;
            try
            {
                dir = chooser.GetSelectedFile().GetCanonicalPath();
            }
            catch (Exception e)
            {
            }

            if (dir != null)
                jTextField1.SetText(dir);
            else
                dir = jTextField1.GetText();
        }

        private void FormWindowClosing(java.awt. event . WindowEvent  evt)
        {
            if (!Globals.appletMode)
            {
                System.Exit(1);
            }

            Dispose();
        }

        private javax.swing.JButton changeButton;
        private javax.swing.JButton exitButton;
        private javax.swing.JButton cancelButton;
        private javax.swing.JPanel choosePanel;
        private JPanel statusPanel;
        private ProgressPanel progressPanel;
        private InstallPanel installPanel;
        private NotFoundPanel notFoundPanel;
        private JLabel status;
        javax.swing.JTextField jTextField1;
        private javax.swing.JButton okButton;
        private string dir;
        virtual void ShowChooseDialog()
        {
            GetContentPane().Remove(statusPanel);
            GetContentPane().Remove(progressPanel);
            GetContentPane().Remove(installPanel);
            GetContentPane().Remove(notFoundPanel);
            GetContentPane().Add(choosePanel, BorderLayout.SOUTH);
            Validate();
            Repaint();
        }

        virtual void ShowStatus()
        {
            GetContentPane().Remove(choosePanel);
            GetContentPane().Remove(installPanel);
            GetContentPane().Add(statusPanel, BorderLayout.SOUTH);
            Validate();
            Repaint();
        }

        virtual void ShowProgressPanel()
        {
            GetContentPane().Remove(choosePanel);
            GetContentPane().Remove(installPanel);
            GetContentPane().Add(progressPanel, BorderLayout.SOUTH);
            Validate();
            Repaint();
        }

        virtual void ShowInstallPanel()
        {
            GetContentPane().Remove(choosePanel);
            GetContentPane().Remove(statusPanel);
            GetContentPane().Remove(notFoundPanel);
            GetContentPane().Add(installPanel, BorderLayout.SOUTH);
            Validate();
            Repaint();
        }

        virtual void ShowNotFoundPanel()
        {
            GetContentPane().Remove(choosePanel);
            GetContentPane().Remove(installPanel);
            GetContentPane().Remove(statusPanel);
            GetContentPane().Add(notFoundPanel, BorderLayout.SOUTH);
            Validate();
            Repaint();
        }

        virtual void SetStatus(string text)
        {
            status.SetText(text);
        }

        virtual void TestQ2Data()
        {
            while (FS.LoadFile("pics/colormap.pcx") == null)
            {
                ShowNotFoundPanel();
                try
                {
                    lock (this)
                    {
                        Wait();
                    }
                }
                catch (InterruptedException e)
                {
                }
            }

            ShowStatus();
            Repaint();
        }

        class Jake2Canvas : Canvas
        {
            private Image image;
            Jake2Canvas()
            {
                SetSize(400, 200);
                try
                {
                    image = ImageIO.Read(GetType().GetResource("/splash.png"));
                }
                catch (Exception e)
                {
                }
            }

            public virtual void Paint(Graphics g)
            {
                g.DrawImage(image, 0, 0, null);
            }
        }

        class NotFoundPanel : JPanel
        {
            private Q2DataDialog parent;
            private ButtonGroup selection;
            private JRadioButton dir;
            private JRadioButton install;
            private JButton exit;
            private JButton ok;
            private JLabel message;
            NotFoundPanel(Q2DataDialog d)
            {
                parent = d;
                InitComponents();
            }

            private void InitComponents()
            {
                GridBagConstraints constraints = new GridBagConstraints();
                SetLayout(new GridBagLayout());
                Size d = new Size( 400, 100);
                SetMinimumSize(d);
                SetMaximumSize(d);
                SetPreferredSize(d);
                message = new JLabel("Quake2 level data not found");
                message.SetForeground(Color.RED);
                constraints.gridx = 0;
                constraints.gridy = 0;
                constraints.gridwidth = 2;
                constraints.insets = new Insets(5, 5, 2, 5);
                constraints.anchor = GridBagConstraints.CENTER;
                Add(message, constraints);
                constraints.gridx = 1;
                constraints.gridy = 1;
                constraints.gridwidth = 2;
                constraints.weightx = 1;
                constraints.fill = GridBagConstraints.HORIZONTAL;
                constraints.insets = new Insets(0, 2, 0, 5);
                constraints.anchor = GridBagConstraints.WEST;
                JLabel label = new JLabel("select baseq2 directory from existing Quake2 installation");
                label.AddMouseListener(new AnonymousMouseAdapter(this));
                Add(label, constraints);
                constraints.gridx = 1;
                constraints.gridy = 2;
                label = new JLabel("download and install Quake2 demo data (38MB)");
                label.AddMouseListener(new AnonymousMouseAdapter1(this));
                Add(label, constraints);
                selection = new ButtonGroup();
                dir = new JRadioButton();
                install = new JRadioButton();
                selection.Add(dir);
                selection.Add(install);
                constraints.gridx = 0;
                constraints.gridy = 1;
                constraints.gridwidth = 1;
                constraints.weightx = 0;
                constraints.insets = new Insets(0, 5, 0, 2);
                constraints.fill = GridBagConstraints.NONE;
                constraints.anchor = GridBagConstraints.EAST;
                dir.SetSelected(true);
                Add(dir, constraints);
                constraints.gridx = 0;
                constraints.gridy = 2;
                Add(install, constraints);
                constraints.gridx = 0;
                constraints.gridy = 3;
                constraints.gridwidth = 2;
                constraints.weighty = 1;
                constraints.insets = new Insets(5, 5, 5, 5);
                constraints.fill = GridBagConstraints.NONE;
                constraints.anchor = GridBagConstraints.SOUTHWEST;
                exit = new JButton("Exit");
                exit.AddActionListener(new AnonymousActionListener4(this));
                Add(exit, constraints);
                constraints.gridx = 2;
                constraints.gridy = 3;
                constraints.gridwidth = 1;
                constraints.anchor = GridBagConstraints.SOUTHEAST;
                ok = new JButton("OK");
                ok.AddActionListener(new AnonymousActionListener5(this));
                Add(ok, constraints);
            }

            private sealed class AnonymousMouseAdapter : MouseAdapter
            {
                public AnonymousMouseAdapter(NotFoundPanel parent)
                {
                    this.parent = parent;
                }

                private readonly NotFoundPanel parent;
                public void MouseClicked(MouseEvent e)
                {
                    dir.SetSelected(true);
                }
            }

            private sealed class AnonymousMouseAdapter1 : MouseAdapter
            {
                public AnonymousMouseAdapter1(NotFoundPanel parent)
                {
                    this.parent = parent;
                }

                private readonly NotFoundPanel parent;
                public void MouseClicked(MouseEvent e)
                {
                    install.SetSelected(true);
                }
            }

            private sealed class AnonymousActionListener4 : ActionListener
            {
                public AnonymousActionListener4(NotFoundPanel parent)
                {
                    this.parent = parent;
                }

                private readonly NotFoundPanel parent;
                public void ActionPerformed(ActionEvent e)
                {
                    if (!Globals.appletMode)
                    {
                        System.Exit(0);
                    }
                }
            }

            private sealed class AnonymousActionListener5 : ActionListener
            {
                public AnonymousActionListener5(NotFoundPanel parent)
                {
                    this.parent = parent;
                }

                private readonly NotFoundPanel parent;
                public void ActionPerformed(ActionEvent e)
                {
                    Ok();
                }
            }

            private void Ok()
            {
                if (dir.IsSelected())
                {
                    parent.ShowChooseDialog();
                }
                else
                {
                    parent.ShowInstallPanel();
                }
            }
        }

        class InstallPanel : JPanel
        {
            private Vector mirrorNames = new Vector();
            private Vector mirrorLinks = new Vector();
            private Q2DataDialog parent;
            private JComboBox mirrorBox;
            private JTextField destDir;
            private JButton cancel;
            private JButton exit;
            private JButton install;
            private JButton choose;
            public InstallPanel(Q2DataDialog d)
            {
                InitComponents();
                string dir = Q2DataDialog.home + Q2DataDialog.sep + "Jake2";
                destDir.SetText(dir);
                InitMirrors();
                parent = d;
            }

            private void InitComponents()
            {
                GridBagConstraints constraints = new GridBagConstraints();
                SetLayout(new GridBagLayout());
                Dimension d = new Dimension(400, 100);
                SetMinimumSize(d);
                SetMaximumSize(d);
                SetPreferredSize(d);
                constraints.gridx = 0;
                constraints.gridy = 0;
                constraints.insets = new Insets(5, 5, 0, 5);
                constraints.anchor = GridBagConstraints.SOUTHWEST;
                Add(new JLabel("download mirror"), constraints);
                constraints.gridx = 0;
                constraints.gridy = 1;
                constraints.insets = new Insets(5, 5, 5, 5);
                Add(new JLabel("destination directory"), constraints);
                constraints.gridx = 1;
                constraints.gridy = 0;
                constraints.weightx = 1;
                constraints.gridwidth = 3;
                constraints.insets = new Insets(5, 5, 0, 5);
                constraints.fill = GridBagConstraints.HORIZONTAL;
                mirrorBox = new JComboBox();
                Add(mirrorBox, constraints);
                constraints.gridx = 1;
                constraints.gridy = 1;
                constraints.gridwidth = 2;
                constraints.fill = GridBagConstraints.BOTH;
                constraints.insets = new Insets(5, 5, 5, 5);
                destDir = new JTextField();
                Add(destDir, constraints);
                constraints.gridx = 3;
                constraints.gridy = 1;
                constraints.weightx = 0;
                constraints.gridwidth = 1;
                constraints.fill = GridBagConstraints.NONE;
                choose = new JButton("...");
                choose.AddActionListener(new AnonymousActionListener6(this));
                Add(choose, constraints);
                constraints.gridx = 0;
                constraints.gridy = 2;
                constraints.gridwidth = 1;
                constraints.weighty = 1;
                constraints.fill = GridBagConstraints.NONE;
                exit = new JButton("Exit");
                exit.AddActionListener(new AnonymousActionListener7(this));
                Add(exit, constraints);
                constraints.gridx = 0;
                constraints.gridy = 2;
                constraints.gridwidth = 4;
                constraints.anchor = GridBagConstraints.SOUTH;
                cancel = new JButton("Cancel");
                cancel.AddActionListener(new AnonymousActionListener8(this));
                Add(cancel, constraints);
                constraints.gridx = 2;
                constraints.gridy = 2;
                constraints.gridwidth = 2;
                constraints.anchor = GridBagConstraints.SOUTHEAST;
                install = new JButton("Install");
                install.AddActionListener(new AnonymousActionListener9(this));
                Add(install, constraints);
            }

            private sealed class AnonymousActionListener6 : ActionListener
            {
                public AnonymousActionListener6(InstallPanel parent)
                {
                    this.parent = parent;
                }

                private readonly InstallPanel parent;
                public void ActionPerformed(ActionEvent e)
                {
                    Choose();
                }
            }

            private sealed class AnonymousActionListener7 : ActionListener
            {
                public AnonymousActionListener7(InstallPanel parent)
                {
                    this.parent = parent;
                }

                private readonly InstallPanel parent;
                public void ActionPerformed(ActionEvent e)
                {
                    Exit();
                }
            }

            private sealed class AnonymousActionListener8 : ActionListener
            {
                public AnonymousActionListener8(InstallPanel parent)
                {
                    this.parent = parent;
                }

                private readonly InstallPanel parent;
                public void ActionPerformed(ActionEvent e)
                {
                    Cancel();
                }
            }

            private sealed class AnonymousActionListener9 : ActionListener
            {
                public AnonymousActionListener9(InstallPanel parent)
                {
                    this.parent = parent;
                }

                private readonly InstallPanel parent;
                public void ActionPerformed(ActionEvent e)
                {
                    Install();
                }
            }

            private void ReadMirrors()
            {
                InputStream in_renamed = GetType().GetResourceAsStream("/mirrors");
                BufferedReader r = new BufferedReader(new InputStreamReader(in_renamed));
                try
                {
                    while (true)
                    {
                        string name = r.ReadLine();
                        string value = r.ReadLine();
                        if (name == null || value == null)
                            break;
                        mirrorNames.Add(name);
                        mirrorLinks.Add(value);
                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                    try
                    {
                        r.Close();
                    }
                    catch (Exception e1)
                    {
                    }

                    try
                    {
                        in_renamed.Close();
                    }
                    catch (Exception e1)
                    {
                    }
                }
            }

            private void InitMirrors()
            {
                ReadMirrors();
                for (int i = 0; i < mirrorNames.Size(); i++)
                {
                    mirrorBox.AddItem(mirrorNames.Get(i));
                }

                int i = Globals.rnd.NextInt(mirrorNames.Size());
                mirrorBox.SetSelectedIndex(i);
            }

            private void Cancel()
            {
                parent.ShowNotFoundPanel();
            }

            private void Install()
            {
                parent.progressPanel.destDir = destDir.GetText();
                parent.progressPanel.mirror = (string)mirrorLinks.Get(mirrorBox.GetSelectedIndex());
                parent.ShowProgressPanel();
                new Thread(parent.progressPanel).Start();
            }

            private void Exit()
            {
                if (!Globals.appletMode)
                {
                    System.Exit(0);
                }
            }

            private void Choose()
            {
                JFileChooser chooser = new JFileChooser();
                chooser.SetFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
                chooser.SetDialogType(JFileChooser.CUSTOM_DIALOG);
                chooser.SetMultiSelectionEnabled(false);
                chooser.SetDialogTitle("choose destination directory");
                chooser.ShowDialog(this, "OK");
                string dir = null;
                try
                {
                    dir = chooser.GetSelectedFile().GetCanonicalPath();
                }
                catch (Exception e)
                {
                }

                if (dir != null)
                    destDir.SetText(dir);
            }
        }

        class ProgressPanel : JPanel, IRunnable
        {
            static byte[] buf = new byte[8192];
            string destDir;
            string mirror;
            JProgressBar progress = new JProgressBar();
            JLabel label = new JLabel("");
            JButton cancel = new JButton("Cancel");
            Q2DataDialog parent;
            bool running;
            public ProgressPanel(Q2DataDialog d)
            {
                InitComponents();
                parent = d;
            }

            virtual void InitComponents()
            {
                progress.SetMinimum(0);
                progress.SetMaximum(100);
                progress.SetStringPainted(true);
                SetLayout(new GridBagLayout());
                GridBagConstraints gridBagConstraints = new GridBagConstraints();
                gridBagConstraints = new GridBagConstraints();
                gridBagConstraints.gridx = 0;
                gridBagConstraints.gridy = 0;
                gridBagConstraints.gridwidth = 1;
                gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
                gridBagConstraints.insets = new Insets(5, 10, 5, 10);
                gridBagConstraints.weightx = 1;
                gridBagConstraints.anchor = GridBagConstraints.SOUTH;
                Add(label, gridBagConstraints);
                gridBagConstraints.gridy = 1;
                gridBagConstraints.anchor = GridBagConstraints.NORTH;
                Add(progress, gridBagConstraints);
                gridBagConstraints.gridy = 1;
                gridBagConstraints.anchor = GridBagConstraints.SOUTH;
                gridBagConstraints.fill = GridBagConstraints.NONE;
                gridBagConstraints.weighty = 1;
                gridBagConstraints.weightx = 0;
                cancel.AddActionListener(new AnonymousActionListener10(this));
                Add(cancel, gridBagConstraints);
                Dimension d = new Dimension(400, 100);
                SetMinimumSize(d);
                SetMaximumSize(d);
                SetPreferredSize(d);
            }

            private sealed class AnonymousActionListener10 : ActionListener
            {
                public AnonymousActionListener10(ProgressPanel parent)
                {
                    this.parent = parent;
                }

                private readonly ProgressPanel parent;
                public void ActionPerformed(ActionEvent e)
                {
                    Cancel();
                }
            }

            virtual void Cancel()
            {
                lock (this)
                {
                    running = false;
                }
            }

            public virtual void Run()
            {
                lock (this)
                {
                    running = true;
                }

                InputStream in_renamed = null;
                OutputStream out_renamed = null;
                File outFile = null;
                label.SetText("downloading...");
                File dir = null;
                try
                {
                    dir = new File(destDir);
                    dir.Mkdirs();
                }
                catch (Exception e)
                {
                }

                try
                {
                    if (!dir.IsDirectory() || !dir.CanWrite())
                    {
                        EndInstall("can't write to " + destDir);
                        return;
                    }
                }
                catch (Exception e)
                {
                    EndInstall(e.GetMessage());
                    return;
                }

                try
                {
                    URL url = new URL(mirror);
                    URLConnection conn = url.OpenConnection();
                    int length = conn.GetContentLength();
                    progress.SetMaximum(length / 1024);
                    progress.SetMinimum(0);
                    in_renamed = conn.GetInputStream();
                    outFile = File.CreateTempFile("Jake2Data", ".zip");
                    outFile.DeleteOnExit();
                    out_renamed = new FileOutputStream(outFile);
                    CopyStream(in_renamed, out_renamed);
                }
                catch (Exception e)
                {
                    EndInstall(e.GetMessage());
                    return;
                }
                finally
                {
                    try
                    {
                        in_renamed.Close();
                    }
                    catch (Exception e)
                    {
                    }

                    try
                    {
                        out_renamed.Close();
                    }
                    catch (Exception e)
                    {
                    }
                }

                try
                {
                    InstallData(outFile.GetCanonicalPath());
                }
                catch (Exception e)
                {
                    EndInstall(e.GetMessage());
                    return;
                }

                try
                {
                    if (outFile != null)
                        outFile.Delete();
                }
                catch (Exception e)
                {
                }

                EndInstall("installation successful");
            }

            virtual void InstallData(string filename)
            {
                InputStream in_renamed = null;
                OutputStream out_renamed = null;
                try
                {
                    ZipFile f = new ZipFile(filename);
                    Enumeration e = f.Entries();
                    while (e.HasMoreElements())
                    {
                        ZipEntry entry = (ZipEntry)e.NextElement();
                        string name = entry.GetName();
                        int i;
                        if ((i = name.IndexOf("/baseq2")) > -1 && name.IndexOf(".dll") == -1)
                        {
                            name = destDir + name.Substring(i);
                            File outFile = new File(name);
                            if (entry.IsDirectory())
                            {
                                outFile.Mkdirs();
                            }
                            else
                            {
                                label.SetText("installing " + outFile.GetName());
                                progress.SetMaximum((int)entry.GetSize() / 1024);
                                progress.SetValue(0);
                                outFile.GetParentFile().Mkdirs();
                                out_renamed = new FileOutputStream(outFile);
                                in_renamed = f.GetInputStream(entry);
                                CopyStream(in_renamed, out_renamed);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    try
                    {
                        in_renamed.Close();
                    }
                    catch (Exception e1)
                    {
                    }

                    try
                    {
                        out_renamed.Close();
                    }
                    catch (Exception e1)
                    {
                    }
                }
            }

            virtual void EndInstall(string message)
            {
                parent.notFoundPanel.message.SetText(message);
                parent.jTextField1.SetText(destDir + "/baseq2");
                parent.ShowChooseDialog();
                parent.OkButtonActionPerformed(null);
            }

            virtual void CopyStream(InputStream in_renamed, OutputStream out_renamed)
            {
                try
                {
                    int c = 0;
                    int l;
                    while ((l = in_renamed.Read(buf)) > 0)
                    {
                        if (!running)
                            throw new Exception("installation canceled");
                        out_renamed.Write(buf, 0, l);
                        c += l;
                        int k = c / 1024;
                        progress.SetValue(k);
                        progress.SetString(k + "/" + progress.GetMaximum() + " KB");
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    try
                    {
                        in_renamed.Close();
                    }
                    catch (Exception e)
                    {
                    }

                    try
                    {
                        out_renamed.Close();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
    }
}
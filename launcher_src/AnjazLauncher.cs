using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AnjazLauncher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LauncherForm());
        }
    }

    public class LauncherForm : Form
    {
        private Label statusLabel;
        private ProgressBar progressBar;
        private Label titleLabel;
        private Process bridgeProcess;
        private Process electronProcess;
        private System.Windows.Forms.Timer timer;
        private int step = 0;

        // Base paths
        private string projectRoot = @"D:\anjazclaw";
        private string electronExe;
        private string desktopAppPath;
        private string bridgeScript;
        private string pythonExe;

        public LauncherForm()
        {
            // Resolve paths
            desktopAppPath = Path.Combine(projectRoot, "desktop-app");
            electronExe = Path.Combine(desktopAppPath, "node_modules", "electron", "dist", "electron.exe");
            bridgeScript = Path.Combine(projectRoot, "bridge", "main.py");
            pythonExe = "python";

            // Window setup
            this.Text = "Anjazclaw + Hermes AI";
            this.Size = new Size(480, 260);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(15, 15, 26);
            this.ForeColor = Color.White;

            // Title
            titleLabel = new Label();
            titleLabel.Text = "⚡ Anjazclaw + Hermes Sovereign AI";
            titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(108, 99, 255);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(40, 30);
            this.Controls.Add(titleLabel);

            // Status
            statusLabel = new Label();
            statusLabel.Text = "جاري التحضير...";
            statusLabel.Font = new Font("Segoe UI", 11);
            statusLabel.ForeColor = Color.FromArgb(180, 180, 200);
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(40, 90);
            this.Controls.Add(statusLabel);

            // Progress bar
            progressBar = new ProgressBar();
            progressBar.Location = new Point(40, 130);
            progressBar.Size = new Size(390, 24);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Maximum = 100;
            this.Controls.Add(progressBar);

            // Version label
            var versionLabel = new Label();
            versionLabel.Text = "v1.0 — CLAW Intelligence";
            versionLabel.Font = new Font("Segoe UI", 8);
            versionLabel.ForeColor = Color.FromArgb(80, 80, 100);
            versionLabel.AutoSize = true;
            versionLabel.Location = new Point(40, 175);
            this.Controls.Add(versionLabel);

            // Start the launch sequence with a timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 800;
            timer.Tick += LaunchSequence;
            timer.Start();
        }

        private void LaunchSequence(object sender, EventArgs e)
        {
            step++;

            switch (step)
            {
                case 1:
                    statusLabel.Text = "🔍 التحقق من الملفات...";
                    progressBar.Value = 10;
                    break;

                case 2:
                    if (!File.Exists(electronExe))
                    {
                        statusLabel.Text = "❌ لم يتم العثور على Electron! يرجى تثبيت المكتبات أولاً.";
                        statusLabel.ForeColor = Color.FromArgb(255, 100, 100);
                        timer.Stop();
                        return;
                    }
                    statusLabel.Text = "✓ تم العثور على Electron";
                    progressBar.Value = 25;
                    break;

                case 3:
                    statusLabel.Text = "🚀 تشغيل محرك هيرمس (Bridge)...";
                    progressBar.Value = 40;
                    StartBridge();
                    break;

                case 4:
                    // Wait for bridge
                    statusLabel.Text = "⏳ انتظار جاهزية المحرك...";
                    progressBar.Value = 60;
                    break;

                case 5:
                    statusLabel.Text = "⏳ انتظار جاهزية المحرك...";
                    progressBar.Value = 70;
                    break;

                case 6:
                    statusLabel.Text = "🖥️ تشغيل واجهة Anjazclaw...";
                    progressBar.Value = 85;
                    StartElectron();
                    break;

                case 7:
                    statusLabel.Text = "✅ تم التشغيل بنجاح! جاري الإغلاق...";
                    statusLabel.ForeColor = Color.FromArgb(0, 200, 120);
                    progressBar.Value = 100;
                    break;

                case 8:
                    timer.Stop();
                    this.Close();
                    break;
            }
        }

        private void StartBridge()
        {
            try
            {
                if (!File.Exists(bridgeScript))
                {
                    // Bridge is optional — skip silently
                    return;
                }

                var psi = new ProcessStartInfo();
                psi.FileName = pythonExe;
                psi.Arguments = "\"" + bridgeScript + "\"";
                psi.WorkingDirectory = projectRoot;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;

                // Pass environment variable so bridge knows where Hermes is
                psi.EnvironmentVariables["HERMES_PATH"] = @"c:\Users\kaher.WAW707\hermes";

                bridgeProcess = Process.Start(psi);
            }
            catch (Exception ex)
            {
                // Bridge failure is non-fatal
                Debug.WriteLine("Bridge start failed: " + ex.Message);
            }
        }

        private void StartElectron()
        {
            try
            {
                var psi = new ProcessStartInfo();
                psi.FileName = electronExe;
                psi.Arguments = ".";
                psi.WorkingDirectory = desktopAppPath;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = false;

                electronProcess = Process.Start(psi);
            }
            catch (Exception ex)
            {
                statusLabel.Text = "❌ خطأ في تشغيل التطبيق: " + ex.Message;
                statusLabel.ForeColor = Color.FromArgb(255, 100, 100);
                timer.Stop();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (timer != null) timer.Stop();
            base.OnFormClosing(e);
        }
    }
}

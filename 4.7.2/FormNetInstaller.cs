using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace frmdnsflush
{
    public partial class FormNetInstaller : Form
    {
        // Offline installer fwlink (stable Microsoft redirect ~66 MB)
        private const string DownloadUrl =
            "https://go.microsoft.com/fwlink/p/?LinkId=780604";

        private string _installerPath;
        private WebClient _webClient;

        public bool InstallSucceeded { get; private set; }

        public FormNetInstaller()
        {
            InitializeComponent();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;
            btnCancel.Enabled = false;
            _installerPath = Path.Combine(Path.GetTempPath(), "NDP462-Setup.exe");

            lblStatus.Text = "Downloading .NET Framework 4.6.2...\r\nPlease wait, this may take a few minutes.";
            progressBar.Style = ProgressBarStyle.Continuous;

            _webClient = new WebClient();
            _webClient.DownloadProgressChanged += OnDownloadProgressChanged;
            _webClient.DownloadFileCompleted += OnDownloadCompleted;
            _webClient.DownloadFileAsync(new Uri(DownloadUrl), _installerPath);

            btnCancel.Enabled = true;
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            double mbReceived = e.BytesReceived / 1024.0 / 1024.0;
            double mbTotal = e.TotalBytesToReceive / 1024.0 / 1024.0;
            lblSize.Text = $"{mbReceived:F1} MB / {mbTotal:F1} MB  ({e.ProgressPercentage}%)";
        }

        private void OnDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _webClient.Dispose();
            _webClient = null;

            if (e.Cancelled)
            {
                TryDeleteInstaller();
                lblStatus.Text = "Download cancelled.";
                progressBar.Value = 0;
                lblSize.Text = "";
                btnInstall.Enabled = true;
                btnCancel.Enabled = true;
                return;
            }

            if (e.Error != null)
            {
                TryDeleteInstaller();
                MessageBox.Show("Download failed:\r\n" + e.Error.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Download failed. Please try again.";
                progressBar.Value = 0;
                lblSize.Text = "";
                btnInstall.Enabled = true;
                btnCancel.Enabled = true;
                return;
            }

            lblStatus.Text = "Installing .NET Framework 4.6.2...\r\nThis may take several minutes. Please do not close this window.";
            lblSize.Text = "";
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;

            var worker = new BackgroundWorker();
            worker.DoWork += RunInstaller;
            worker.RunWorkerCompleted += OnInstallCompleted;
            worker.RunWorkerAsync();
        }

        private void RunInstaller(object sender, DoWorkEventArgs e)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = _installerPath,
                    Arguments = "/q /norestart",   // silent, no reboot prompt
                    UseShellExecute = true,
                    Verb = "runas"                 // request UAC elevation
                };
                var proc = Process.Start(psi);
                proc.WaitForExit();
                e.Result = proc.ExitCode;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void OnInstallCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TryDeleteInstaller();
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Value = 100;

            if (e.Result is Exception ex)
            {
                MessageBox.Show("Installation failed:\r\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Installation failed.";
                progressBar.Value = 0;
                btnInstall.Enabled = true;
                btnCancel.Enabled = true;
                return;
            }

            int exitCode = (int)e.Result;
            // Exit codes: 0 = success, 3010 = success but reboot required, others = error
            if (exitCode == 0 || exitCode == 3010)
            {
                InstallSucceeded = true;
                lblStatus.Text = exitCode == 3010
                    ? "Installation complete! A reboot is required.\r\nPlease restart your computer."
                    : "Installation complete! Launching application...";

                if (exitCode == 3010)
                {
                    MessageBox.Show(
                        ".NET Framework 4.6.2 installed successfully.\r\n\r\nA system restart is required before the application can run.",
                        "Restart Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblStatus.Text = $"Installation failed with exit code {exitCode}.";
                MessageBox.Show($"Installation failed (exit code: {exitCode}).\r\nPlease install .NET Framework 4.6.2 manually.",
                    "Installation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnInstall.Enabled = true;
                btnCancel.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_webClient != null && _webClient.IsBusy)
            {
                _webClient.CancelAsync();
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_webClient != null && _webClient.IsBusy)
            {
                e.Cancel = true;
                MessageBox.Show("Download in progress. Click Cancel button to abort.",
                    "Please Wait", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            base.OnFormClosing(e);
        }

        private void TryDeleteInstaller()
        {
            try
            {
                if (File.Exists(_installerPath))
                    File.Delete(_installerPath);
            }
            catch { /* ignore */ }
        }
    }
}

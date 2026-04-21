using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Bootstrapper
{
    public class FormInstaller : Form
    {
        public string DownloadUrl { get; set; }
        /// <summary>If set, skip download and use this local file directly.</summary>
        public string LocalInstallerPath { get; set; }
        public bool RebootRequired { get; private set; }

        private Label lblTitle;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Label lblSize;
        private Button btnInstall;
        private Button btnCancel;

        private WebClient _webClient;
        private string _installerPath;

        public FormInstaller()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = ".NET Framework 4.6.2 Required";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(420, 200);

            lblTitle = new Label();
            lblTitle.Text = ".NET Framework 4.6.2 is required";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(10, 12);
            lblTitle.Size = new System.Drawing.Size(400, 28);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblStatus = new Label();
            lblStatus.Text = "This application requires .NET Framework 4.6.2.\r\nClick Install to download and install it automatically.";
            lblStatus.Location = new System.Drawing.Point(10, 48);
            lblStatus.Size = new System.Drawing.Size(400, 40);
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            progressBar = new ProgressBar();
            progressBar.Location = new System.Drawing.Point(10, 96);
            progressBar.Size = new System.Drawing.Size(400, 22);

            lblSize = new Label();
            lblSize.Location = new System.Drawing.Point(10, 122);
            lblSize.Size = new System.Drawing.Size(400, 18);
            lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblSize.Text = "";

            btnInstall = new Button();
            btnInstall.Text = "Install";
            btnInstall.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            btnInstall.Location = new System.Drawing.Point(220, 155);
            btnInstall.Size = new System.Drawing.Size(95, 30);
            btnInstall.Click += new EventHandler(btnInstall_Click);

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new System.Drawing.Point(325, 155);
            btnCancel.Size = new System.Drawing.Size(85, 30);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblStatus);
            this.Controls.Add(progressBar);
            this.Controls.Add(lblSize);
            this.Controls.Add(btnInstall);
            this.Controls.Add(btnCancel);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!string.IsNullOrEmpty(LocalInstallerPath))
            {
                lblStatus.Text = "Local installer found. Click Install to begin.";
                lblSize.Text = LocalInstallerPath;
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;
            btnCancel.Enabled = false;

            // Use local file directly — no download needed
            if (!string.IsNullOrEmpty(LocalInstallerPath) && File.Exists(LocalInstallerPath))
            {
                _installerPath = LocalInstallerPath;
                lblStatus.Text = "Installing .NET Framework 4.6.2...\r\nPlease wait, do not close this window.";
                lblSize.Text = "";
                progressBar.Style = ProgressBarStyle.Marquee;
                progressBar.MarqueeAnimationSpeed = 30;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += DoInstall;
                worker.RunWorkerCompleted += OnInstallDone;
                worker.RunWorkerAsync();
                return;
            }

            // Fallback: download from URL
            _installerPath = Path.Combine(Path.GetTempPath(), "NDP462-Setup.exe");
            lblStatus.Text = "Downloading .NET Framework 4.6.2...\r\nThis may take a few minutes (~66 MB).";

            _webClient = new WebClient();
            _webClient.DownloadProgressChanged += OnProgress;
            _webClient.DownloadFileCompleted += OnDownloadDone;
            _webClient.DownloadFileAsync(new Uri(DownloadUrl), _installerPath);
            btnCancel.Enabled = true;
        }

        private void OnProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            double recv = e.BytesReceived / 1048576.0;
            double total = e.TotalBytesToReceive / 1048576.0;
            lblSize.Text = string.Format("{0:F1} MB / {1:F1} MB  ({2}%)", recv, total, e.ProgressPercentage);
        }

        private void OnDownloadDone(object sender, AsyncCompletedEventArgs e)
        {
            _webClient.Dispose();
            _webClient = null;

            if (e.Cancelled)
            {
                TryDelete();
                ResetUI("Download cancelled.");
                return;
            }
            if (e.Error != null)
            {
                TryDelete();
                MessageBox.Show("Download error:\r\n" + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetUI("Download failed. Please try again.");
                return;
            }

            lblStatus.Text = "Installing... Please wait, do not close this window.";
            lblSize.Text = "";
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += DoInstall;
            worker.RunWorkerCompleted += OnInstallDone;
            worker.RunWorkerAsync();
        }

        private void DoInstall(object sender, DoWorkEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = _installerPath;
                psi.Arguments = "/q /norestart";
                psi.UseShellExecute = true;
                psi.Verb = "runas";
                Process p = Process.Start(psi);
                p.WaitForExit();
                e.Result = p.ExitCode;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void OnInstallDone(object sender, RunWorkerCompletedEventArgs e)
        {
            TryDelete();
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Value = 100;

            if (e.Result is Exception)
            {
                Exception ex = (Exception)e.Result;
                MessageBox.Show("Install error:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetUI("Installation failed.");
                return;
            }

            int code = (int)e.Result;
            if (code == 0 || code == 3010)
            {
                RebootRequired = (code == 3010);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Installation failed (exit code: " + code + ").\r\nPlease install .NET Framework 4.6.2 manually.",
                    "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetUI("Installation failed (code " + code + ").");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_webClient != null && _webClient.IsBusy)
                _webClient.CancelAsync();
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
                MessageBox.Show("Download in progress. Click Cancel to abort.", "Please Wait",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            base.OnFormClosing(e);
        }

        private void ResetUI(string msg)
        {
            lblStatus.Text = msg;
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Value = 0;
            lblSize.Text = "";
            btnInstall.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void TryDelete()
        {
            try { if (File.Exists(_installerPath)) File.Delete(_installerPath); }
            catch { }
        }
    }
}

using System.Diagnostics;
using System.Net.NetworkInformation;

namespace frmdnsflush
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            // Load Ko-fi button image from embedded resource
            var imgStream = typeof(Form1).Assembly.GetManifestResourceStream("frmdnsflush.Resources.images.png");
            if (imgStream != null) pictureBox1.Image = Image.FromStream(imgStream);
            // Detect Windows version
            lblos.Text = $"OS: {GetWindowsVersion()}";
        }

        private static string GetWindowsVersion()
        {
            var osVersion = Environment.OSVersion;
            var version = osVersion.Version;

            return version.Major switch
            {
                10 => version.Build >= 22000 ? "Windows 11" : "Windows 10",
                6 => version.Minor switch
                {
                    3 => "Windows 8.1",
                    2 => "Windows 8",
                    1 => "Windows 7",
                    0 => "Windows Vista",
                    _ => "Windows"
                },
                _ => $"Windows {version.Major}.{version.Minor}"
            };
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = $"DNS Flush - {DateTime.Now:hh:mm:ss tt}";
        }

        private async void btnflush_Click(object sender, EventArgs e)
        {
            btnflush.Enabled = false;
            btnflush.Text = "Running...";
            txtcmd.Clear();

            try
            {
                AppendOutput(await ExecuteCommandAsync("ipconfig", "/release"));
                AppendOutput(await ExecuteCommandAsync("ipconfig", "/renew"));
                AppendOutput(await ExecuteCommandAsync("ipconfig", "/flushdns"));
                AppendOutput("--- DNS Flush complete. Proceeding to Change DNS... ---");
            }
            catch (Exception ex)
            {
                AppendOutput($"Error: {ex.Message}");
            }
            finally
            {
                btnflush.Enabled = true;
                btnflush.Text = "Flush DNS";
            }

            await ChangeDnsAsync();
        }

        private void AppendOutput(string text)
        {
            txtcmd.AppendText(text + Environment.NewLine);
            txtcmd.ScrollToCaret();
        }

        private async void btnChangeDns_Click(object sender, EventArgs e)
        {
            await ChangeDnsAsync();
        }

        private async Task ChangeDnsAsync()
        {
            var confirm = MessageBox.Show(
                "Do you want to change DNS to:\n  • Primary:   1.1.1.1  (Cloudflare)\n  • Secondary: 8.8.8.8  (Google)",
                "Confirm DNS Change",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                AppendOutput("DNS change cancelled by user.");
                Application.Exit();
                return;
            }

            btnChangeDns.Enabled = false;
            btnChangeDns.Text = "Running...";

            try
            {
                string iface = GetActiveNetworkInterface();
                if (string.IsNullOrEmpty(iface))
                {
                    AppendOutput("Warning: No active network interface found.");
                    Application.Exit();
                    return;
                }

                AppendOutput($"Interface: {iface}");
                AppendOutput(await ExecuteCommandAsync("netsh", $"interface ip set dns name=\"{iface}\" static 1.1.1.1"));
                AppendOutput(await ExecuteCommandAsync("netsh", $"interface ip add dns name=\"{iface}\" 8.8.8.8 index=2"));
                AppendOutput(await ExecuteCommandAsync("ipconfig", "/flushdns"));
                AppendOutput("DNS changed successfully: Primary 1.1.1.1 | Secondary 8.8.8.8");
                await PingCheckAsync();
            }
            catch (Exception ex)
            {
                AppendOutput($"Error: {ex.Message}");
            }
            finally
            {
                btnChangeDns.Enabled = true;
                btnChangeDns.Text = "Change DNS";
            }

            Application.Exit();
        }

        private static readonly char[] SpaceSeparator = [' '];

        private async Task PingCheckAsync()
        {
            AppendOutput("--- Checking network connectivity... ---");
            string[] hosts = ["8.8.8.8", "1.1.1.1"];

            foreach (var host in hosts)
            {
                try
                {
                    using var ping = new Ping();
                    var reply = await Task.Run(() => ping.Send(host, 3000));
                    if (reply.Status == IPStatus.Success)
                        AppendOutput($"Ping {host}: OK ({reply.RoundtripTime} ms)");
                    else
                        AppendOutput($"Ping {host}: Failed ({reply.Status})");
                }
                catch (Exception ex)
                {
                    AppendOutput($"Ping {host}: Error - {ex.Message}");
                }
            }
        }

        private static string GetActiveNetworkInterface()
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = "interface show interface",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            foreach (var line in output.Split('\n'))
            {
                if (line.Contains("Connected"))
                {
                    var parts = line.Trim().Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 4)
                        return string.Join(" ", parts[3..]);
                }
            }

            return string.Empty;
        }

        private static Task<string> ExecuteCommandAsync(string command, string arguments)
        {
            return Task.Run(() =>
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                return string.IsNullOrWhiteSpace(error) ? output.Trim() : error.Trim();
            });
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "https://ko-fi.com/thanhnguyen150993"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

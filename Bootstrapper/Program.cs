using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Bootstrapper
{
    internal static class Program
    {
        private const int Net462Release = 394802;
        private const string InstallerFileName = "ndp462-kb3151800-x86-x64-allos-enu.exe";
        // Fallback download (fwlink) if local file not found
        private const string DownloadUrl = "https://go.microsoft.com/fwlink/p/?LinkId=780604";
        private const string MainExe = "DNS Flush and Renew (Release).exe";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Enable TLS 1.2 for Windows 7 / .NET 3.5 (raw value 3072 = Tls12)
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(48 | 192 | 768 | 3072);

            if (!IsNet462Installed())
            {
                string dir0 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string localInstaller = Path.Combine(dir0, InstallerFileName);

                using (FormInstaller frm = new FormInstaller())
                {
                    // Use local bundled file if available, else download
                    if (File.Exists(localInstaller))
                        frm.LocalInstallerPath = localInstaller;
                    else
                        frm.DownloadUrl = DownloadUrl;

                    if (frm.ShowDialog() != DialogResult.OK)
                        return;

                    if (frm.RebootRequired)
                    {
                        MessageBox.Show(
                            ".NET Framework 4.6.2 installed.\r\n\r\nPlease restart your computer, then run the application again.",
                            "Restart Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }

            // Launch main app from same folder as this bootstrapper
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string mainPath = Path.Combine(dir, MainExe);

            if (!File.Exists(mainPath))
            {
                MessageBox.Show("Cannot find:\r\n" + mainPath +
                    "\r\n\r\nMake sure both files are in the same folder.",
                    "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process.Start(mainPath);
        }

        internal static bool IsNet462Installed()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full");
                if (key == null) return false;
                object val = key.GetValue("Release");
                key.Close();
                return val != null && (int)val >= Net462Release;
            }
            catch
            {
                return false;
            }
        }
    }
}

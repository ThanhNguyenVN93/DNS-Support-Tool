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
        private const string DownloadUrl =
            "https://download.microsoft.com/download/F/9/4/F942F07D-F26F-4F30-B4E3-EBD54FABA377/NDP462-KB3151800-x86-x64-AllOS-ENU.exe";
        private const string MainExe = "DNS Flush and Renew (Release).exe";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!IsNet462Installed())
            {
                using (FormInstaller frm = new FormInstaller())
                {
                    frm.DownloadUrl = DownloadUrl;
                    if (frm.ShowDialog() != DialogResult.OK)
                        return;

                    // Reboot required
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

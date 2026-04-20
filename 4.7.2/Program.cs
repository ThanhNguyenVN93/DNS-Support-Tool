using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace frmdnsflush
{
    internal static class Program
    {
        // .NET 4.6.2 Release key = 394802 (or higher)
        private const int Net462ReleaseKey = 394802;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!IsNet462Installed())
            {
                using (var installer = new FormNetInstaller())
                {
                    if (installer.ShowDialog() != DialogResult.OK || !installer.InstallSucceeded)
                        return;
                }
            }

            Application.Run(new Form1());
        }

        private static bool IsNet462Installed()
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"))
                {
                    if (key == null) return false;
                    var release = key.GetValue("Release");
                    return release != null && (int)release >= Net462ReleaseKey;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

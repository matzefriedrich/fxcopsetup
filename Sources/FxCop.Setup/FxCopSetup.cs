// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FxCopSetup.cs" company="Matthias Friedrich">
//   Copyright © Matthias Friedrich 2009 - 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FxCop.Setup
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Win32;

    internal sealed class FxCopSetup
    {
        private const string MetricsPowerToolExeKey = @"3082010a0282010100e8af5ca2200df8287cbc057b7fadeeeb76ac28533f3adb407db38e33e6573fa551153454a5cfb48ba93fa837e12d50ed35164eef4d7adb137688b02cf0595ca9ebe1d72975e41b85279bf3f82d9e41362b0b40fbbe3bbab95c759316524bca33c537b0f3eb7ea8f541155c08651d2137f02cba220b10b1109d772285847c4fb91b90b0f5a3fe8bf40c9a4ea0f5c90a21e2aae3013647fd2f826a8103f5a935dc94579dfb4bd40e82db388f12fee3d67a748864e162c4252e2aae9d181f0e1eb6c2af24b40e50bcde1c935c49a679b5b6dbcef9707b280184b82a29cfbfa90505e1e00f714dfdad5c238329ebc7c54ac8e82784d37ec6430b950005b14f6571c50203010001";

        public static Task InstallCodeMetricsPowerTool(string sfxPackageFile, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(sfxPackageFile))
            {
                throw new ArgumentNullException("sfxPackageFile");
            }

            ThrowIfPackageIsInvalid(sfxPackageFile);

            if (cancellationToken == null)
            {
                throw new ArgumentNullException("cancellationToken");
            }

            string destinationFolder;
            if (ObtainFxCopDirectory(out destinationFolder) == false)
            {
                throw new FxCopSetupException("Failed to obtain the FxCop directory.");
            }

            return Task.Run(() =>
                {
                    string arguments = string.Format(CultureInfo.InvariantCulture, "/x:\"{0}\" /q", destinationFolder);
                    using (var sfxProcess = new Process())
                    {
                        sfxProcess.StartInfo = new ProcessStartInfo(sfxPackageFile, arguments)
                        {
                            WorkingDirectory = destinationFolder, 
                            Verb = "runas"
                        };
                        sfxProcess.Start();
                        sfxProcess.WaitForExit();
                    }
                }, cancellationToken);
        }

        public static IFileReference TryDownloadSfxPackage(string url)
        {
            bool success = false;

            const string MetricspowertoolExe = "MetricsPowerTool.exe";
            string localFile = Path.Combine(Path.GetTempPath(), MetricspowertoolExe);

            var syncObject = new ManualResetEventSlim();
            using (var client = new WebClient())
            {
                client.DownloadFileCompleted += (sender, e) =>
                    {
                        success = (e.Cancelled == false && e.Error == null);
                        syncObject.Set();
                    };

                client.DownloadFileAsync(new Uri(url, UriKind.Absolute), localFile);
                syncObject.Wait();
            }

            if (success)
            {
                return new TemporaryFileReference(localFile);
            }

            return null;
        }

        private static bool ObtainFxCopDirectory(out string installationFolder)
        {
            installationFolder = null;

            const string VisualStudioVersion = "11.0";
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            string subKeyName = string.Format(formatProvider, @"Software\Microsoft\VisualStudio\{0}_Config", VisualStudioVersion);

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(subKeyName))
            {
                if (key != null)
                {
                    var shellFolder = (string)key.GetValue("ShellFolder");
                    string staticAnalysisToolsPath = Path.Combine(shellFolder, @"Team Tools\Static Analysis Tools\FxCop");
                    if (Directory.Exists(staticAnalysisToolsPath))
                    {
                        installationFolder = staticAnalysisToolsPath;
                        return true;
                    }
                }
            }

            return false;
        }

        private static void ThrowIfPackageIsInvalid(string sfxCabinetFile)
        {
            string publicKey = SignatureUtil.GetPublicKeyFrom(sfxCabinetFile);

            if (MetricsPowerToolExeKey.Equals(publicKey) == false)
            {
                throw new FxCopSetupException("The specified self-extracting cabinet file is invalid.");
            }
        }
    }
}
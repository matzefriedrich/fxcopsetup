// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Matthias Friedrich">
//   Copyright © Matthias Friedrich 2009 - 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FxCop.Setup
{
    using System.Threading;

    internal class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        private static async void DownloadAndInstallCodeMetricsPowerTool11()
        {
            const string Url = @"http://download.microsoft.com/download/A/2/5/A25566FE-CF42-409B-BCCE-1F8DC1A81D8A/MetricsPowerTool.exe";
            using (IFileReference sfxCabinetTemporaryFile = FxCopSetup.TryDownloadSfxPackage(Url))
            {
                if (sfxCabinetTemporaryFile != null)
                {
                    CancellationToken cancellationToken = CancellationTokenSource.Token;
                    await FxCopSetup.InstallCodeMetricsPowerTool(sfxCabinetTemporaryFile.FullName, cancellationToken);
                }
            }
        }

        private static void Main()
        {
            DownloadAndInstallCodeMetricsPowerTool11();
        }
    }
}
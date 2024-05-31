// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Matthias Friedrich">
//   Copyright © Matthias Friedrich 2009 - 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FxCop.Setup
{
    using System.Threading;
    using System.Threading.Tasks;

    internal static class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new();

        private static async Task DownloadAndInstallCodeMetricsPowerTool11Async()
        {
            const string url = "http://download.microsoft.com/download/A/2/5/A25566FE-CF42-409B-BCCE-1F8DC1A81D8A/MetricsPowerTool.exe";
            using IFileReference? sfxCabinetTemporaryFile = FxCopSetup.TryDownloadSfxPackage(url);
            if (sfxCabinetTemporaryFile == null) return;

            CancellationToken cancellationToken = CancellationTokenSource.Token;
            await FxCopSetup.InstallCodeMetricsPowerToolAsync(sfxCabinetTemporaryFile.FullName, cancellationToken);
        }

        private static async Task Main()
        {
            await DownloadAndInstallCodeMetricsPowerTool11Async();
        }
    }
}
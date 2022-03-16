global using System;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;

namespace Highlighter
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.HighlighterString)]
    [ProvideOptionPage(typeof(OptionsProvider.HighlighterOptions), "Highlighter", "General", 0, 0, true)]
    [ProvideProfile(typeof(OptionsProvider.HighlighterOptions), "Highlighter", "General", 0, 0, true)]
    public sealed class HighlighterPackage : ToolkitPackage
    {
        public HighlighterPackage()
        {
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();
        }

    }
}
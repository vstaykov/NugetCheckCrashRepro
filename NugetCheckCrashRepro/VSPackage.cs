using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NugetCheckCrashRepro.Loggers;
using NugetCheckCrashRepro.Notifiers;
using NugetCheckCrashRepro.ProjectSearchers;

namespace NugetCheckCrashRepro
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad("{ADFC4E64-0397-11D1-9F4E-00A0C911004F}")]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(VSPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class VSPackage : Package
    {
        public const string PackageGuidString = "a25ccc09-0182-4010-8a92-1eb757bea47e";

        private const string NugetVisualStudioPackageId = "NuGet.VisualStudio";

        private readonly ILogger logger;
        private readonly NugetPackageBasedProjectSearcher nugetPackageBasedProjectSearcher;
        private DTE dte;
        private SolutionEventsNotifier solutionEventsNotifier;


        public VSPackage()
        {
            this.logger = new PopupWindowLogger();
            this.nugetPackageBasedProjectSearcher = new NugetPackageBasedProjectSearcher();
        }

        private DTE Dte
        {
            get
            {
                if (this.dte == null)
                {
                    this.dte = this.GetService(typeof(DTE)) as DTE;
                }

                return this.dte;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.SubscribeForEvents();
        }

        private void SubscribeForEvents()
        {
            this.solutionEventsNotifier = new SolutionEventsNotifier((IVsSolution)this.GetService(typeof(SVsSolution)));
            this.solutionEventsNotifier.AfterBackgroundSolutionLoadComplete += OnSolutionBackgroundLoadComplete;
            this.solutionEventsNotifier.AfterProjectLoadComplete += OnProjectLoadComplete;
            this.solutionEventsNotifier.Activate();
        }

        private void OnSolutionBackgroundLoadComplete(object sender, EventArgs e)
        {
            this.CheckSolutionHasProjectsWithSpecificPackageInstalled();
        }

        private void OnProjectLoadComplete(object sender, EventArgs e)
        {
            this.CheckSolutionHasProjectsWithSpecificPackageInstalled();
        }

        private void CheckSolutionHasProjectsWithSpecificPackageInstalled()
        {
            var hasProjectsWithNugetVisualStudioPackageInstalled = this.nugetPackageBasedProjectSearcher.CheckSolutionHasProjectWithSpecificPackage(this.Dte.Solution, NugetVisualStudioPackageId);

            if (hasProjectsWithNugetVisualStudioPackageInstalled)
            {
                var message = string.Format("Project with installed {0} package detected.", NugetVisualStudioPackageId);

                this.logger.LogInfo(message);
            }
        }
    }
}

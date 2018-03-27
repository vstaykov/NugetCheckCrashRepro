using System;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;
using NugetCheckCrashRepro.Loggers;

namespace NugetCheckCrashRepro.NuGet
{
    internal class NugetPackageServices : INugetPackageService
    {
        private readonly ILogger logger;
        private readonly IVsPackageInstallerServices packageInstallerServices;

        public NugetPackageServices()
            : this(new PopupWindowLogger())
        {
        }
        public NugetPackageServices(ILogger logger)
        {
            this.logger = logger;

            var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            this.packageInstallerServices = componentModel.GetService<IVsPackageInstallerServices>();
        }

        public NugetPackageServices(IVsPackageInstallerServices installerService)
        {
            this.packageInstallerServices = installerService;
        }

        public bool HasInstalledPackage(Project project, string packageName)
        {
            var hasInstalledPackage = false;

            try
            {
                var projectPackages = this.packageInstallerServices.GetInstalledPackages(project);
                var package = projectPackages.Where(x => x.Id.StartsWith(packageName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (package != null)
                {
                    hasInstalledPackage = true;
                }
            }
            catch (Exception e)
            {
                this.logger.LogException(e);
            }

            return hasInstalledPackage;
        }
    }
}

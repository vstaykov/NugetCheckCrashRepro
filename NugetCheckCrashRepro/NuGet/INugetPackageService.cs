using System;
using EnvDTE;

namespace NugetCheckCrashRepro.NuGet
{
    internal interface INugetPackageService
    {
        bool HasInstalledPackage(Project project, string packageName);
    }
}

using EnvDTE;
using NugetCheckCrashRepro.NuGet;
using System.Collections.Generic;

namespace NugetCheckCrashRepro.ProjectSearchers
{
    internal class NugetPackageBasedProjectSearcher
    {
        private const string SolutionFolderProjectKind = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";
        private readonly INugetPackageService nugetPackageService;

        public NugetPackageBasedProjectSearcher()
            : this(new NugetPackageServices())
        {
        }

        public NugetPackageBasedProjectSearcher(INugetPackageService nugetPackageService)
        {
            this.nugetPackageService = nugetPackageService;
        }

        public bool CheckSolutionHasProjectWithSpecificPackage(Solution solution, string package)
        {
            var projects = this.GetProjects(solution);
            var hasProjectWithSpecificPackage = false;

            foreach (Project project in projects)
            {
                hasProjectWithSpecificPackage = this.nugetPackageService.HasInstalledPackage(project, package);

                if (hasProjectWithSpecificPackage)
                {
                    break;
                }
            }

            return hasProjectWithSpecificPackage;
        }

        private IEnumerable<Project> GetProjects(Solution solution)
        {
            var projects = new List<Project>();

            foreach (Project project in solution.Projects)
            {
                if (project.Kind != SolutionFolderProjectKind)
                {
                    projects.Add(project);
                }
            }

            return projects;
        }
    }
}

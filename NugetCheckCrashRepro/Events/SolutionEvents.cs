using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace NugetCheckCrashRepro.Events
{
    internal class SolutionEvents : IVsSolutionEvents, IVsSolutionLoadEvents
    {
        public event EventHandler<EventArgs> AfterBackgroundSolutionLoadComplete;
        public event EventHandler<EventArgs> AfterProjectLoadComplete;

        public int OnAfterCloseSolution(object unkReserved)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy stubHierarchy, IVsHierarchy realHierarchy)
        {
            this.RaiseEvent(this.AfterProjectLoadComplete);
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnAfterOpenProject(IVsHierarchy hierarchy, int added)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object unkReserved, int newSolution)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy hierarchy, int removed)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object unkReserved)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy realHierarchy, IVsHierarchy stubHierarchy)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy hierarchy, int removing, ref int cancel)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object unkReserved, ref int cancel)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy realHierarchy, ref int cancel)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnAfterBackgroundSolutionLoadComplete()
        {
            this.RaiseEvent(this.AfterBackgroundSolutionLoadComplete);
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnAfterLoadProjectBatch(bool backgroundIdleBatch)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnBeforeBackgroundSolutionLoadBegins()
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnBeforeLoadProjectBatch(bool backgroundIdleBatch)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnBeforeOpenSolution(string solutionFilename)
        {
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int OnQueryBackgroundLoadProjectBatch(out bool shouldDelayLoadToNextIdle)
        {
            shouldDelayLoadToNextIdle = false;
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        private void RaiseEvent(EventHandler<EventArgs> eventHandler)
        {
            var copyEventHandler = eventHandler;
            if (copyEventHandler != null)
            {
                copyEventHandler(this, EventArgs.Empty);
            }
        }
    }
}

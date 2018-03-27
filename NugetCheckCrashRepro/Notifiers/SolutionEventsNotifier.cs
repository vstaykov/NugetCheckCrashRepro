using System;
using Microsoft.VisualStudio.Shell.Interop;
using SolutionEvents = NugetCheckCrashRepro.Events.SolutionEvents;

namespace NugetCheckCrashRepro.Notifiers
{
    internal class SolutionEventsNotifier
    {
        private uint solutionOpenedCookie;
        private IVsSolution solution;
        private SolutionEvents solutionEvents;
        private EventHandler<EventArgs> afterBackgroundSolutionLoadHandler;
        private EventHandler<EventArgs> afterProjectLoadHandler;

        public SolutionEventsNotifier(IVsSolution solution)
        {
            this.solution = solution;
            this.solutionEvents = new SolutionEvents();
            this.solutionEvents.AfterBackgroundSolutionLoadComplete += this.OnAfterBackgroundSolutionLoadComplete;
            this.solutionEvents.AfterProjectLoadComplete += this.OnAfterProjectLoadComplete;
        }

        public event EventHandler<EventArgs> AfterBackgroundSolutionLoadComplete
        {
            add
            {
                this.afterBackgroundSolutionLoadHandler = (EventHandler<EventArgs>)Delegate.Combine(this.afterBackgroundSolutionLoadHandler, value);
            }
            remove
            {
                this.afterBackgroundSolutionLoadHandler = (EventHandler<EventArgs>)Delegate.Remove(this.afterBackgroundSolutionLoadHandler, value);
            }
        }

        public event EventHandler<EventArgs> AfterProjectLoadComplete
        {
            add
            {
                this.afterProjectLoadHandler = (EventHandler<EventArgs>)Delegate.Combine(this.afterProjectLoadHandler, value);
            }
            remove
            {
                this.afterProjectLoadHandler = (EventHandler<EventArgs>)Delegate.Remove(this.afterProjectLoadHandler, value);
            }
        }

        public void Activate()
        {
            if (this.solution != null)
            {
                this.solution.AdviseSolutionEvents(this.solutionEvents, out this.solutionOpenedCookie);
            }
        }

        public void Deactivate()
        {
            if (this.solution != null)
            {
                this.solution.UnadviseSolutionEvents(this.solutionOpenedCookie);
            }
        }

        private void OnAfterBackgroundSolutionLoadComplete(object sender, EventArgs args)
        {
            if (this.afterBackgroundSolutionLoadHandler != null)
            {
                this.afterBackgroundSolutionLoadHandler(this, null);
            }
        }

        private void OnAfterProjectLoadComplete(object sender, EventArgs args)
        {
            if (this.afterProjectLoadHandler != null)
            {
                this.afterProjectLoadHandler(this, null);
            }
        }
    }
}

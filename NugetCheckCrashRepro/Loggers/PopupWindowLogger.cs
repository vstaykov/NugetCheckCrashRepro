using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NugetCheckCrashRepro.Loggers
{
    internal class PopupWindowLogger : ILogger
    {
        private const string InfoCaption = "INFO";

        public void LogInfo(string message)
        {
            MessageBox.Show(message, InfoCaption);
        }

        public void LogException(Exception exception)
        {
            var caption = string.Format("Exception of type {0} occured", exception.GetType());
            var message = string.Format("{1}{0}{0}{2}", Environment.NewLine, exception.Message, exception.StackTrace);

            MessageBox.Show(message);
        }
    }
}

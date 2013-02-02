using System;
using System.Diagnostics;
using AzureTraceListeners.Models;

namespace AzureTraceListeners.Listeners.Base
{
    public abstract class AzureTraceListener : TraceListener
    {
        protected string ApplicationName { get; set; }

        public override void Close()
        {
            Flush();
            base.Close();
        }

        #region Write/WriteLine
        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void Write(object o)
        {
            WriteLine(Convert.ToString(o));
        }

        public override void Write(object o, string category)
        {
            WriteLine(Convert.ToString(o), category);
        }

        public override void Write(string message, string category)
        {
            WriteLine(message, category);
        }

        public override void WriteLine(string message)
        {
            WriteLine(message, null);
        }

        public override void WriteLine(object o)
        {
            WriteLine(Convert.ToString(o), null);
        }

        public override void WriteLine(object o, string category)
        {
            base.WriteLine(Convert.ToString(o), category);
        }

        public override void WriteLine(string message, string category)
        {
            if (string.IsNullOrEmpty(message))
                return;

            try
            {
                var logMessage = new AzureTraceMessage(message, category, ApplicationName);
                SaveMessage(logMessage);
            }
            catch (Exception ex)
            {
                LogTraceException(ex);
            }
        }

        protected virtual void LogTraceException(Exception ex)
        {
            Console.WriteLine("An uncaught Trace.WriteLine() exception occured in AzureTraceListener: {0}", ex);
        }

        protected abstract void SaveMessage(AzureTraceMessage logMessage);

        #endregion
    }
}
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
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

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((Filter != null) && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
                return;
            var message = data.ToString();
            WriteLine(message, eventType.ToString());
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if ((Filter != null) && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
                return;

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                if (i != 0)
                {
                    stringBuilder.Append(", ");
                }
                if (data[i] != null)
                {
                    stringBuilder.Append(data[i]);
                }
            }

            WriteLine(stringBuilder.ToString(), eventType.ToString());
        }
        
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            WriteLine(message, eventType.ToString());
        }

        protected virtual void LogTraceException(Exception ex)
        {
            Console.WriteLine("An uncaught Trace.WriteLine() exception occured in AzureTraceListener: {0}", ex);
        }

        protected abstract void SaveMessage(AzureTraceMessage logMessage);

        #endregion
    }
}
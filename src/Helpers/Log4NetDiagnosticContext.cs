using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace TranslateOnlineDoc.Helpers
{
    public class Log4NetDiagnosticContext : IDisposable
    {
        private readonly IDisposable _context;

        public Log4NetDiagnosticContext(string context)
        {
            _context = LogicalThreadContext.Stacks["NDC"].Push(context);
        }

        public static Log4NetDiagnosticContext Create(string context)
        {
            return new Log4NetDiagnosticContext(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free any other managed objects here.
                _context?.Dispose();
            }

            // Free unmanaged objects here
        }

        ~Log4NetDiagnosticContext()
        {
            Dispose(false);
        }
    }
}

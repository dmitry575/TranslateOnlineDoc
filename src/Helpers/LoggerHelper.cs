using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateOnlineDoc.Helpers
{
    public static class LoggerHelper
    {
        /// <summary>
        /// Начать scope логирования
        /// </summary>
        /// <param name="log">Логгер</param>
        /// <param name="scope">Тело scope</param>
        /// <returns>IDisposable</returns>
        public static IDisposable StartScope(this log4net.ILog log, string scope)
        {
            return Log4NetDiagnosticContext.Create(scope);
        }
    }
}

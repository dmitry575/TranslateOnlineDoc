using System.ComponentModel;
using System.Threading;
using log4net;
using TranslateOnlineDoc.Common;
using TranslateOnlineDoc.Configs;

namespace TranslateOnlineDoc.Translates
{
    /// <summary>
    /// Job for wich do all work
    /// </summary>
    public class TranslateJobs
    {
        private readonly Configuration _config;
        private static ILog _logger = LogManager.GetLogger(typeof(TranslateJobs));
        private readonly CancellationToken _cancellationToken;

        public TranslateJobs(Configuration config, CancellationToken cancellationToken)
        {
            _config = config;
            _cancellationToken = cancellationToken;
        }

        public void Work()
        {
            var files = new Files(_config.DirSrc).GetList();

            if (files == null || files.Count <= 0)
            {
                _logger.Warn($"no files to path: {_config.DirSrc}");
                return;
            }
            new TranslateBackgroundHandler(_config,files, _cancellationToken).Work();
        }
    }
}

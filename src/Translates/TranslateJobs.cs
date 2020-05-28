using System.ComponentModel;
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

        public TranslateJobs(Configuration config)
        {
            _config = config;
        }

        public void Work()
        {
            var files = new Files(_config.DirSrc).GetList();

            if (files == null || files.Count <= 0)
            {
                _logger.Warn($"no files to path: {_config.DirSrc}");
                return;
            }
            new TranslateBackgroundHandler(_config,files).Work();
        }
    }
}

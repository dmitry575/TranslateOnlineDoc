using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using TranslateOnlineDoc.Configs;
using TranslateOnlineDoc.Helpers;

namespace TranslateOnlineDoc.Translates
{
    /// <summary>
    /// Handler of queue with files
    /// </summary>
    public class TranslateBackgroundHandler
    {
        private readonly Configuration _config;
        private readonly List<string> _files;
        private readonly ILog _logger = LogManager.GetLogger(typeof(TranslateBackgroundHandler));
        private const int MaxTasks = 1;
        private readonly CancellationToken _cancellationToken;


        public TranslateBackgroundHandler(Configuration config, List<string> files, CancellationToken cancellationToken)
        {
            _config = config;
            _cancellationToken = cancellationToken;
            _files = files;
        }

        /// <summary>
        /// Start Translate files in Task
        /// </summary>
        public void Work()
        {
            try
            {
                _logger.Info($"start translate {_files.Count} files");
                Parallel.ForEach<string>(_files, new ParallelOptions
                {
                    CancellationToken = _cancellationToken,
                    MaxDegreeOfParallelism = MaxTasks
                }, async (i) => await TranslateFile(i));

            }
            catch (Exception e)
            {
                _logger.Error($"translate files parallel failed: {e}");
            }
        }

        private async Task TranslateFile(string filename)
        {
            using (var t = new TranslateFile(filename, _config))
            {
                using (LogicalThreadContext.Stacks["NDC"].Push($"Filename: {filename}"))
                {
                    await t.Translate();
                }
            }
        }
    }
}

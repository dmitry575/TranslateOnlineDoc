using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using TranslateOnlineDoc.Configs;

namespace TranslateOnlineDoc.Translates
{
    /// <summary>
    /// Handler of queue with files
    /// </summary>
    public class TranslateBackgroundHandler
    {
        private readonly Configuration _config;
        private readonly Queue<string> _queue;
        private readonly List<Task> _tasks;
        private static ILog _logger = LogManager.GetLogger(typeof(TranslateBackgroundHandler));
        private const int MaxTasks = 3;

        public TranslateBackgroundHandler(Configuration config, List<string> files)
        {
            _config = config;
            _queue = new Queue<string>(files);
            _tasks = new List<Task>();
        }

        /// <summary>
        /// Start Translate files in Task
        /// </summary>
        public void Work()
        {
            while (_queue.Count>0)
            {
                var filename = _queue.Dequeue();
                if (_tasks.Count >= MaxTasks)
                {
                    var idx = Task.WaitAny(_tasks.ToArray());
                    _tasks.RemoveAt(idx);
                }
                
                _tasks.Add(Task.Run(() => new TranslateFile(filename, _config).Translate()));
                _logger.Info($"Added file to Task for file: {filename}");
            }

            Task.WaitAll(_tasks.ToArray());
            _tasks.Clear();

        }
    }
}

using System;
using System.Threading;
using log4net;
using log4net.Config;
using TranslateOnlineDoc.Configs;
using TranslateOnlineDoc.Translates;

[assembly: XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace TranslateOnlineDoc
{

    class Program
    {
        const string Version = "1.0.2";

        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));
        private static CancellationTokenSource _cancellationTokenSource;
        static void Main(string[] args)
        {

            PrintIntro();
            var config = new Configuration(args);

            _cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelHandler);

            try
            {
                new TranslateJobs(config, _cancellationTokenSource.Token).Work();
            }
            catch (Exception e)
            {
                _logger.Error($"invalid translates files: {e}");
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }

            _logger.Info("working finished...");
        }

        private static void CancelHandler(object sender, ConsoleCancelEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Print info about programm
        /// </summary>
        private static void PrintIntro()
        {
            _logger.Info($"Start program translate files with {string.Format(Configuration.UrlTranslate, "en")}");
            _logger.Info($"Version: {Version}");
            _logger.Info("");
            _logger.Info("Parameters for using:");
            _logger.Info("/from - from language translate, default value en");
            _logger.Info("/to - to language translate, default value: en");
            _logger.Info("/dir - full directories from witch get all files and try to translate");
            _logger.Info("/output - full directories where will be saving new files");
        }
    }
}

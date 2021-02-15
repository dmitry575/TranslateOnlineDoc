using System;
using System.Reflection;
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
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static CancellationTokenSource _cancellationTokenSource;
        static void Main(string[] args)
        {

            PrintIntro();
            var config = new Configuration(args);

            _cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += CancelHandler;

            try
            {
                new TranslateJobs(config, _cancellationTokenSource.Token).Work();
            }
            catch (Exception e)
            {
                Logger.Error($"invalid translates files: {e}");
            }
            finally
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                Logger.Info("working finished...");
            }

            Logger.Info("working finished...");
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
            Logger.Info($"Start program translate files with {string.Format(Configuration.UrlTranslate, "en")}");
            Logger.Info($"Version: {Assembly.GetExecutingAssembly().GetName().Version}");
            Logger.Info("");
            Logger.Info("Parameters for using:");
            Logger.Info("/from - from language translate, default value en");
            Logger.Info("/to - to language translate, default value: en");
            Logger.Info("/dir - full directories from witch get all files and try to translate");
            Logger.Info("/output - full directories where will be saving new files");
            Logger.Info("/timeout - how many seconds wait loading (page, button)");
            Logger.Info("/threads - counts of work treads, max 10");
        }
    }
}

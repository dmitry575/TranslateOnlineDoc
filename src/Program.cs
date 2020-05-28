using System;
using log4net;
using log4net.Config;
using TranslateOnlineDoc.Configs;
using TranslateOnlineDoc.Translates;

[assembly: XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace TranslateOnlineDoc
{

    class Program
    {
        const string Version = "1.0.0";

        private static ILog _logger = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {

            PrintIntro();
            var config = new Configuration(args);

            new TranslateJobs(config).Work();

            _logger.Info("working finished, press any key...");
            Console.Read();
        }
        
        /// <summary>
        /// Print info about programm
        /// </summary>
        private static void PrintIntro()
        {
            _logger.Info($"Start program translate files with {string.Format(Configuration.UrlTranslate,"en")}");
            _logger.Info($"Version: {Version}");
            _logger.Info("");
            _logger.Info("Parameters for using:");
            _logger.Info("/from - from language translate, for example: en");
            _logger.Info("/to - to language translate, for example: en");
            _logger.Info("/dir - full directories from witch get all files and try to translate");
            _logger.Info("/output - full directories where will be saving new files");
        }
    }
}

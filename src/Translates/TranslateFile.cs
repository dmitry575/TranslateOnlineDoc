using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TranslateOnlineDoc.Configs;
using TranslateOnlineDoc.Elements;

namespace TranslateOnlineDoc.Translates
{
    /// <summary>
    /// Class for translate one file
    /// open url, upload file, download new file
    /// </summary>
    public class TranslateFile : IDisposable
    {
        /// <summary>
        /// Minimal seconds for pause between action on form
        /// </summary>
        private const int MIN_PAUSE_SECONDS = 10;

        /// <summary>
        /// Maximal seconds for pause between action on form
        /// </summary>
        private const int MAX_PAUSE_SECONDS = 20;
        
        /// <summary>
        /// How many seconds need wait a load website
        /// </summary>
        private readonly int _maxSecondsWaiting;

        /// <summary>
        /// Filename fro translate
        /// </summary>
        private readonly string _filename;

        /// <summary>
        /// Configuration
        /// </summary>
        private readonly Configuration _config;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(TranslateFile));

        /// <summary>
        /// Selenium driver, use FireFox
        /// </summary>
        private FirefoxDriver _driver;

        /// <summary>
        /// Is disabled object or not
        /// </summary>
        private bool _isDisposable = false;

        /// <summary>
        /// Get random seconds for pause
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Translate file
        /// </summary>
        /// <param name="filename">Filename for translate</param>
        /// <param name="config">Configuration translate</param>
        public TranslateFile(string filename, Configuration config)
        {
            _filename = filename;
            _config = config;
            _maxSecondsWaiting = config.Timeout;
        }

        /// <summary>
        /// Translate the file and download to new path
        /// </summary>
        public async Task Translate()
        {
            // check may be file already translated
            if (FileTranslatedExists())
            {
                Logger.Info($"translated file already exists: {_filename}");
                return;
            }
            Logger.Info($"starting translate file: {_filename}");
            //FirefoxProfile profile = new FirefoxProfile(@"C:\Users\Dmitry\AppData\Local\Mozilla\Firefox\Profiles\t21lgdab.default-1456481888344");
            var options = GetOptions(_config.DirOutput);
           // options.Profile = profile;
            _driver = new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(_maxSecondsWaiting));
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_maxSecondsWaiting);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_maxSecondsWaiting);
            _driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMinutes(_maxSecondsWaiting);
            
            _driver.Manage().Window.Maximize();

            string url = _config.GetUrlTranslate();

            Logger.Info($"open url: {url}");
            try
            {
                _driver.Navigate().GoToUrl(url);
            }
            catch (WebDriverException e)
            {
                Logger.Error($"Open url failed: {e}");
            }

            try
            {
                new UploadFile(_driver, null, _filename).Action();
                Logger.Info($"set file: {_filename}");

                await Task.Delay(GetPause());

                new SelectedElement(_driver, "//select[@name='from']", _config.FromLang).Action();
                Logger.Info($"set lang from: {_config.FromLang}");

                new SelectedElement(_driver, "//select[@name='to']", _config.ToLang).Action();
                Logger.Info($"set lang to: {_config.ToLang}");

                
                new ButtonWaiteElement(_driver, "//input[@id='translation-button']").Action();
                Logger.Info($"click on button");

                await Task.Delay(GetPause());

                var downloadUrl = new DownloadElement(_driver, ".download-link", Path.GetFullPath(_config.DirOutput), _config.Timeout);

                downloadUrl.Action();
                Logger.Info($"downloaded url: {downloadUrl.UrlDownload}, file: {downloadUrl.FileDownload}");
            }
            catch (Exception e)
            {
                Logger.Info($"unexpected exception in transalte process: {e}");
            }
        }

        private TimeSpan GetPause()
        {
            return TimeSpan.FromSeconds(_random.Next(MIN_PAUSE_SECONDS, MAX_PAUSE_SECONDS));
        }

        /// <summary>
        /// Checking translated file
        /// </summary>
        private bool FileTranslatedExists()
        {
            var fInfo = new FileInfo(_filename);
            // result translate file.from.to.extenstion

            var fileResultName =
                fInfo.Name.Substring(0, fInfo.Name.Length - fInfo.Extension.Length)
                + "."
                + _config.FromLang
                + "."
                + _config.ToLang
                + fInfo.Extension;

            var fileName = Path.Combine(_config.DirOutput, fileResultName);

            return File.Exists(fileName);

        }

        /// <summary>
        /// Get options for FireFox
        /// </summary>
        /// <param name="dirOutput">Default directory for download files</param>
        private static FirefoxOptions GetOptions(string dirOutput)
        {
            var options = new FirefoxOptions();
            options.SetPreference("browser.download.folderList", 2);
            options.SetPreference("browser.download.manager.showWhenStarting", false);
            options.SetPreference("browser.download.dir", Path.GetFullPath(dirOutput));
            options.SetPreference("general.useragent.override", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:82.0) Gecko/20100101 Firefox/82.0");
            options.SetPreference("browser.helperApps.neverAsk.openFile",
                "text/csv,application/x-msexcel,application/excel,application/x-excel,application/vnd.ms-excel,image/png,image/jpeg,text/html,text/plain,application/msword,application/xml");
            options.SetPreference("browser.helperApps.neverAsk.saveToDisk",
                "text/csv,application/x-msexcel,application/excel,application/x-excel,application/vnd.ms-excel,image/png,image/jpeg,text/html,text/plain,application/msword,application/xml");
            //options.AddArgument("no-sandbox");
            return options;
        }

        public void Dispose()
        {
            _isDisposable = true;
            if (_driver != null)
            {
                _driver.Close();
                _driver.Quit();
                _driver.Dispose();
            }
        }

        ~TranslateFile()
        {
            if (!_isDisposable)
            {
                Dispose();
            }
        }
    }
}

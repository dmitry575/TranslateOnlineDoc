using System;
using System.IO;
using System.Threading;
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
        private readonly string _filename;
        private readonly Configuration _config;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TranslateFile));
        private FirefoxDriver _driver;
        private bool _isDisposable = false;

        public TranslateFile(string filename, Configuration config)
        {
            _filename = filename;
            _config = config;
        }

        /// <summary>
        /// Translate the file and download to new path
        /// </summary>
        public void Translate()
        {
            _driver = new FirefoxDriver(GetOptions(_config.DirOutput));
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);
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
                new SelectedElement(_driver, "//select[@name='from']", _config.FromLang).Action();
                Logger.Info($"set lang from: {_config.FromLang}");

                new SelectedElement(_driver, "//select[@name='to']", _config.ToLang).Action();
                Logger.Info($"set lang to: {_config.ToLang}");

                new UploadFile(_driver, null, _filename).Action();
                Logger.Info($"set file: {_filename}");

                new ButtonWaiteElement(_driver, "//input[@id='translation-button']").Action();
                Logger.Info($"click on button");

                var downloadUrl = new DownloadElement(_driver, ".download-link", Path.GetFullPath(_config.DirOutput));

                downloadUrl.Action();
                Logger.Info($"downloaded url: {downloadUrl.UrlDownload}, file: {downloadUrl.FileDownload}");
            }
            catch (Exception e)
            {
                Logger.Info($"unexpected exception in transalte process: {e}");
            }
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
            options.SetPreference("browser.helperApps.neverAsk.openFile",
                "text/csv,application/x-msexcel,application/excel,application/x-excel,application/vnd.ms-excel,image/png,image/jpeg,text/html,text/plain,application/msword,application/xml");
            options.SetPreference("browser.helperApps.neverAsk.saveToDisk",
                "text/csv,application/x-msexcel,application/excel,application/x-excel,application/vnd.ms-excel,image/png,image/jpeg,text/html,text/plain,application/msword,application/xml");
            return options;
        }

        public void Dispose()
        {
            _isDisposable = true;
            _driver.Close();
            _driver.Quit();
            _driver.Dispose();
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

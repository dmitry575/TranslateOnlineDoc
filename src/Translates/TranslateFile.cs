using System;
using System.IO;
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
    public class TranslateFile
    {
        private readonly string _filename;
        private readonly Configuration _config;
        private static ILog _logger = LogManager.GetLogger(typeof(TranslateFile));

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
            using (var driver = new FirefoxDriver(GetOptions(_config.DirOutput)))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);
                string url = _config.GetUrlTranslate();
                _logger.Info($"open url: {url}");
                try
                {
                    driver.Navigate().GoToUrl(url);
                }
                catch (WebDriverException e)
                {
                    _logger.Error($"Open url failed: {e}");
                }

                try
                {
                    new SelectedElement(driver, "//select[@name='from']", _config.FromLang).Action();
                    _logger.Info($"set lang from: {_config.FromLang}");

                    new SelectedElement(driver, "//select[@name='to']", _config.ToLang).Action();
                    _logger.Info($"set lang to: {_config.ToLang}");

                    new UploadFile(driver, null, _filename).Action();
                    _logger.Info($"set file: {_filename}");

                    new ButtonWaiteElement(driver, "//input[@id='translation-button']").Action();
                    _logger.Info($"click on button");

                    var downloadUrl = new DownloadElement(driver, ".download-link");

                    downloadUrl.Action();
                    _logger.Info($"downloaded url: {downloadUrl}");
                }
                catch (Exception e)
                {
                    _logger.Info($"unexpected exception in transalte process: {e}");
                }
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

    }
}

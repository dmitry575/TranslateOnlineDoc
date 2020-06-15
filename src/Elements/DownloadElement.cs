using System;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using TranslateOnlineDoc.Common;

namespace TranslateOnlineDoc.Elements
{
    public class DownloadElement : BaseElementAction
    {
        /// <summary>
        /// How many seconds need wait a download link
        /// </summary>
        private const int MaxSecondsWaiting = 60;

        /// <summary>
        /// Path where will be save files
        /// </summary>
        private readonly string _downloadPath;

        /// <summary>
        /// Url downloaded
        /// </summary>
        public string UrlDownload { get; private set; }

        /// <summary>
        /// Fullname file of doenloaded
        /// </summary>
        public string FileDownload { get; private set; }

        public DownloadElement(FirefoxDriver driver, string xpath, string downloadPath) : base(driver, xpath)
        {
            _downloadPath = downloadPath;
        }

        /// <summary>
        /// Download link
        /// </summary>
        public override void Action()
        {
            //waiting while translating
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(MaxSecondsWaiting));
            IWebElement blockDownload;
            try
            {
                blockDownload = wait.Until(d => d.FindElement(By.CssSelector(Xpath)));
            }
            catch (Exception e)
            {
                Logger.Error($"not found element in time: {Xpath}, {e.Message}");
            }

            blockDownload = Driver.FindElementByCssSelector(Xpath);

            if (blockDownload == null)
            {
                throw new ElementActionException($"not found element for download file by xpath: {Xpath}");
            }
            var linkElement = blockDownload.FindElements(By.TagName("a")).FirstOrDefault();
            if (linkElement == null)
            {
                throw new ElementActionException($"not found tag a, with download file url in xpath: {Xpath}");
            }

            UrlDownload = linkElement.GetAttribute("href");
            try
            {
                Driver.ScrollToCenter(linkElement);
            }
            catch (Exception e)
            {
                Logger.Warn($"windows scroll before download failed: {e}");
            }


            linkElement.Click();

            // check file downloaded, and rename
            FileDownload = GetFileName(linkElement);

        }

        /// <summary>
        /// Get full path of downloaded file
        /// </summary>
        /// <param name="linkElement">Url of downloaded file</param>
        private string GetFileName(IWebElement linkElement)
        {
            string href = linkElement.GetAttribute("href");
            if (string.IsNullOrEmpty(href))
            {
                Logger.Warn($"download link has not 'href' attribute");
                return string.Empty;
            }

            var fInfo = new FileInfo(href);
            return Path.Combine(_downloadPath, fInfo.Name);
        }
    }
}
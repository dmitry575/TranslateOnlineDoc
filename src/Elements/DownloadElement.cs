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
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(25));

            var blockDownload = wait.Until(d => d.FindElement(By.CssSelector(Xpath)));
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
            string src = linkElement.GetAttribute("href");
            if (string.IsNullOrEmpty(src))
            {
                Logger.Warn($"download link has not 'href' attribute");
                return string.Empty;
            }

            var fInfo = new FileInfo(src);
            return Path.Combine(_downloadPath, fInfo.Name);
        }
    }
}
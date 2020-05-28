using System;
using System.Linq;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace TranslateOnlineDoc.Elements
{
    public class DownloadElement : BaseElementAction
    {
        protected readonly ILog Logger = LogManager.GetLogger(typeof(DownloadElement));

        private readonly string _cssName;

        public string UrlDownload { get; private set; }

        public DownloadElement(FirefoxDriver driver, string xpath) : base(driver, xpath)
        {
            _cssName = xpath;
        }

        /// <summary>
        /// Download link
        /// </summary>
        public override void Action()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            try
            {
                Driver.ExecuteScript("window.scrollTo(0,500);");
            }
            catch (Exception e)
            {
                Logger.Warn($"windows scroll befor download failed: {e}");
            }

            var blockDownload = wait.Until(d => d.FindElement(By.CssSelector(_cssName)));
            if (blockDownload == null)
            {
                Logger.Warn("not found element for download file");
                return;
            }

            var linkElement = blockDownload.FindElements(By.TagName("a")).FirstOrDefault();
            if (linkElement == null)
            {
                Logger.Warn("not found tag a, with download file url");
                return;
            }

            UrlDownload = linkElement.GetAttribute("href");

            linkElement.Click();
        }
    }
}
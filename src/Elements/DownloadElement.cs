using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using TranslateOnlineDoc.Common;

namespace TranslateOnlineDoc.Elements
{
    public class DownloadElement : BaseElementAction
    {
        public string UrlDownload { get; private set; }

        public DownloadElement(FirefoxDriver driver, string xpath) : base(driver, xpath)
        {
        }

        /// <summary>
        /// Download link
        /// </summary>
        public override void Action()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

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
                Driver.ScrollTo(linkElement);
            }
            catch (Exception e)
            {
                Logger.Warn($"windows scroll before download failed: {e}");
            }


            linkElement.Click();
        }
    }
}
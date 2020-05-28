using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace TranslateOnlineDoc.Elements
{
    public class UploadFile : BaseElementAction
    {
        private readonly string _fileName;

        public UploadFile(FirefoxDriver driver, string xpath, string fileName) : base(driver, xpath)
        {
            _fileName = fileName;
        }

        public override void Action()
        {

            var select = Driver.FindElement(By.CssSelector("input[type=file]"));
            if (select == null)
            {
                Logger.Warn($"Not found element by xpath: {Xpath}, {nameof(SelectedElement)}");
                return;
            }

            if (!File.Exists(_fileName))
            {
                Logger.Warn($"Not file exists {_fileName}, {nameof(SelectedElement)}");
                return;
            }


            var fileInfo = new FileInfo(_fileName);
            select.SendKeys(fileInfo.FullName);
        }
    }
}

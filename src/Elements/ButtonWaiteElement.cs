using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace TranslateOnlineDoc.Elements
{
    public class ButtonWaiteElement : BaseElementAction
    {
        private int _tryClick = 0;
        private const int MaxClick = 3;
        public ButtonWaiteElement(FirefoxDriver driver, string xpath) : base(driver, xpath)
        {
        }

        public override void Action()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            var button = wait.Until(d => d.FindElement(By.XPath(Xpath)));

            if (button == null)
            {
                Logger.Warn($"Not found element by xpath: {Xpath}, {nameof(ButtonWaiteElement)}");
                return;
            }

            var display = button.GetCssValue("display");
            Logger.Info($"button display: {display}");
            if ((button.Displayed && display.Contains("inline") )|| _tryClick > MaxClick)
            {
                button.Click();
            }
            else
            {
                _tryClick++;
                Action();
            }
        }
    }
}

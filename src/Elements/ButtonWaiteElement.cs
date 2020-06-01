﻿using System;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using TranslateOnlineDoc.Common;

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
            wait.PollingInterval = TimeSpan.FromSeconds(20);
            var button = wait.Until(d => d.FindElement(By.XPath(Xpath)));

            if (button == null)
            {
                Logger.Warn($"Not found element by xpath: {Xpath}, {nameof(ButtonWaiteElement)}");
                return;
            }

            var display = button.GetCssValue("display");
            Logger.Info($"button display: {display}");

            if ((button.Displayed && display.Contains("inline")) || _tryClick > MaxClick)
            {
                // try to scroll to button element
                try
                {
                    Driver.ScrollTo(button);
                }
                catch (Exception e)
                {
                    Logger.Error($"failed scroll to button {Xpath}. {e}");
                }

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

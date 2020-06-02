using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using TranslateOnlineDoc.Common;

namespace TranslateOnlineDoc.Elements
{
    public class SelectedElement : BaseElementAction
    {
        private readonly string _value;

        public SelectedElement(FirefoxDriver driver, string xpath, string value) : base(driver, xpath)
        {
            _value = value;
        }
        public override void Action()
        {
            var select = Driver.FindElement(By.XPath(Xpath));

            if (select == null)
            {
                Logger.Warn($"Not found element by xpath: {Xpath}, {nameof(SelectedElement)}");
                return;
            }

            var selectElement = new SelectElement(select);

            try
            {
                Driver.ScrollToCenter(select);
            }
            catch (Exception e)
            {
                Logger.Error($"failed scroll to {Xpath}, value: {_value}. {e}");
            }

            try
            {
                selectElement.SelectByValue(_value);
            }
            catch (Exception e)
            {
                Logger.Error($"failed set value to {Xpath}, value: {_value}. {e}");
            }
        }
    }
}


using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace TranslateOnlineDoc.Common
{
  public  static class DriverHelper
    {
        /// <summary>
        /// Scroll to element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="element"></param>
        public static void ScrollTo(this FirefoxDriver driver, IWebElement element)
        {
            var actions = new Actions(driver);
            actions.MoveToElement(element);
            actions.Perform();
        }
    }
}

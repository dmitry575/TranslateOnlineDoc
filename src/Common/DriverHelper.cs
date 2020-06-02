using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace TranslateOnlineDoc.Common
{
    public static class DriverHelper
    {
        /// <summary>
        /// Scroll to element
        /// </summary>
        /// <param name="driver">Driver selenium</param>
        /// <param name="element">Element to witch need scrolling</param>
        public static void ScrollTo(this FirefoxDriver driver, IWebElement element)
        {
            var actions = new Actions(driver);
            actions.MoveByOffset(element.Location.X, element.Location.Y + 30);
            actions.Build().Perform();
        }

        /// <summary>
        /// Scroll to element and center of the browser
        /// </summary>
        /// <param name="driver">Driver selenium</param>
        /// <param name="element">Element to witch need scrolling</param>
        public static void ScrollToCenter(this FirefoxDriver driver, IWebElement element)
        {
            string scrollElementIntoMiddle = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                                             + "var elementTop = arguments[0].getBoundingClientRect().top;"
                                             + "window.scrollBy(0, elementTop-(viewPortHeight/2));";

            ((IJavaScriptExecutor)driver).ExecuteScript(scrollElementIntoMiddle, element);

        }
    }
}

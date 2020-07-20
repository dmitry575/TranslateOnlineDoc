using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium.Firefox;

namespace TranslateOnlineDoc.Elements
{
    public abstract class BaseElementAction : IElementAction
    {
        protected readonly FirefoxDriver Driver;
        protected readonly string Xpath;
        protected readonly ILog Logger = LogManager.GetLogger(typeof(BaseElementAction));


        protected BaseElementAction(FirefoxDriver driver, string xpath)
        {
            Driver = driver;
            Xpath = xpath;
        }
        public abstract void Action();
    }
}

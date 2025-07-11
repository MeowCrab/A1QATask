using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace SteamTestsUI.Pages
{
    internal class AppPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public AppPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement DownloadButton => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[.//span[text()='Download']]")));
        private IWebElement NeedSteamButton => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[.//h3[text()='No, I need Steam']]")));
        private IWebElement AppHeader => wait.Until(ExpectedConditions.ElementIsVisible(By.Id("appHubAppName")));

        public void ClickDownload()
        {
            DownloadButton.Click();
        }

        public void ClickNeedSteam()
        {
            NeedSteamButton.Click();
        }

        public string GetAppName()
        {
            return AppHeader.Text;
        }
    }
}

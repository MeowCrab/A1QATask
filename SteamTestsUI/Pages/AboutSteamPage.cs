using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SteamTestsUI.Pages
{
    internal class AboutSteamPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public AboutSteamPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement DownloadButton => wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[text()='Install Steam']")));
        private IWebElement OnlineUsersCounter => wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'gamers_online')]/..")));
        private IWebElement PlayingUsersCounter => wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'gamers_in_game')]/..")));

        public IWebElement GetDownloadButton()
        {
            return DownloadButton;
        }

        public int GetUsersOnline()
        {
            return GetPlayersCountFromCounter(OnlineUsersCounter);
        }

        public int GetUsersPlaying()
        {
            return GetPlayersCountFromCounter(PlayingUsersCounter);
        }

        private int GetPlayersCountFromCounter(IWebElement counterElement)
        {
            string counterText = GetElementOwnText(counterElement);
            counterText = counterText.Replace(",", "");
            Console.WriteLine(counterText);
            return int.TryParse(counterText, out var playersCount) ? playersCount : -1;
        }

        private string GetElementOwnText(IWebElement element)
        {
            return (string)((IJavaScriptExecutor)driver).ExecuteScript("""
            return jQuery(arguments[0]).contents().filter(function() {
                return this.nodeType == Node.TEXT_NODE;
            }).text().trim();
            """, element);
        }
    }
}

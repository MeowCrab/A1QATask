using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SteamTestsUI.Pages
{
    public class StorePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public StorePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement SearchBox => wait.Until(ExpectedConditions.ElementIsVisible(By.Id("store_nav_search_term")));
        private IList<IWebElement> SearchResults => driver.FindElements(By.CssSelector(".match"));

        public void SearchFor(string query)
        {
            SearchBox.SendKeys(query);
        }

        public void ClickSearchResult(int index)
        {
            wait.Until(d => SearchResults.Count > index);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", SearchResults[index]);
        }

        public string GetSearchResultName(int index)
        {
            wait.Until(d => SearchResults.Count > index);
            var matchNameElement = SearchResults[index].FindElement(By.CssSelector(".match_name"));
            return matchNameElement.Text;
        }
    }
}

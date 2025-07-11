using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using SteamTestsUI.Pages;

namespace SteamUITests.Steps
{
    [Binding]
    public class SteamNavigationSteps
    {
        private IWebDriver driver;
        private ScenarioContext _scenarioContext;

        public SteamNavigationSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I launch chrome with modes: (.*) and open the (.*) link")]
        public void GivenIOpenTheSteamStore(string modes, string url)
        {
            var options = new ChromeOptions();
            if (modes.Contains("incognito"))
            {
                options.AddArgument("--incognito");
            }
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
        }

        [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }

        [When(@"I search for a game ""(.*)""")]
        public void WhenISearchFor(string query)
        {
            var storePage = new StorePage(driver);
            storePage.SearchFor(query);
        }

        [When(@"I click on the search result #(.*)")]
        public void WhenIClickFirstResult(int index)
        {
            var storePage = new StorePage(driver);
            storePage.ClickSearchResult(index - 1);
        }

        [When(@"I click the ""Download"" button")]
        public void WhenIClickDownload()
        {
            var appPage = new AppPage(driver);
            appPage.ClickDownload();
        }

        [When(@"I click the ""No, I need Steam"" button")]
        public void WhenIClickNoINeedSteam()
        {
            var appPage = new AppPage(driver);
            appPage.ClickNeedSteam();
        }

        [Then(@"first search is ""(.*)"" and second is ""(.*)""")]
        public void ThenFirstAndSecondSearchesCorrect(string firstName, string secondName)
        {
            var storePage = new StorePage(driver);
            var firstResultName = storePage.GetSearchResultName(0);
            var secondResultName = storePage.GetSearchResultName(1);
            Assert.That(firstResultName.Contains(firstName), "First result doesn't match expected title");
            Assert.That(secondResultName.Contains(secondName), "Second result doesn't match expected title");
            _scenarioContext["selectedAppName"] = firstResultName; 
        }
        
        [Then(@"app page displaying the matching search name")]
        public void ThenAppPageDisplayingMatchingName()
        {
            var appPage = new AppPage(driver);
            var appName = appPage.GetAppName();
            Assert.That(appName, Is.EqualTo(_scenarioContext["selectedAppName"]), "App title on page does not match the selected search result");
        }

        [Then(@"I should be on the About Steam Page")]
        public void ThenIShouldBeOnAboutPage()
        {
            Assert.That(driver.Url.EndsWith("/about/"));
        }

        [Then(@"the ""Install Steam"" button is clickable")]
        public void ThenInstallSteamButtonIsClickable()
        {
            var aboutSteamPage = new AboutSteamPage(driver);
            var button = aboutSteamPage.GetDownloadButton();
            Assert.That(button.Displayed && button.Enabled, "Install Steam button is not clickable");
        }

        [Then(@"the number of users playing now is less than online")]
        public void ThenCompareGamersOnlineAndPlaying()
        {
            var aboutSteamPage = new AboutSteamPage(driver);
            var onlineNow = aboutSteamPage.GetUsersOnline(); 
            var playingNow = aboutSteamPage.GetUsersPlaying();

            Assert.That(onlineNow >= 0, "Could not parse players online");
            Assert.That(playingNow >= 0, "Could not parse players playing");
            Assert.That(playingNow < onlineNow, "Players playing now should be less than players online");
        }
    }
}
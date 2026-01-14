using Core;
using Microsoft.Playwright;
using UI.Tests.Hooks;

namespace UI.Tests.Pages
{
    internal class Homepage(UIHooks hooks)
    {
        private readonly IPage _page = hooks.Page;

        public ILocator HomepageLink => _page.Locator("a[href='/']");

        public ILocator ProjectsLink => _page.Locator("a[href='/projects']");

        public ILocator LinkedInLink => _page.Locator("a.btn:has(i.bi-linkedin)");

        public ILocator GithubLink => _page.Locator("a.btn:has(i.bi-github)");

        public ILocator DownloadButton => _page.GetByText("Download");

        internal async Task GoToAsync()
        {
            await _page.GotoAsync(TestConfig.BaseUrl);
        }

        internal async Task WaitForPageAsync()
        {
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        internal async Task<string> GetTitleAsync()
        {
            return await _page.TitleAsync();
        }
    }
}
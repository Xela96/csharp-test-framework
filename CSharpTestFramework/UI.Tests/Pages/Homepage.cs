using Core;
using Microsoft.Playwright;
using UI.Tests.Hooks;

namespace UI.Tests.Pages
{
    internal class Homepage(UIHooks hooks)
    {
        private readonly IPage _page = hooks.Page;

        public ILocator ProjectsLink => _page.Locator("a.nav-link[href='/projects']");

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

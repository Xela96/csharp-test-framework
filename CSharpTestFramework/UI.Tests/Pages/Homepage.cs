using Microsoft.Playwright;
using UI.Tests.StepDefinitions;

namespace UI.Tests.Pages
{
    public class Homepage(Hooks hooks)
    {
        private readonly IPage _page = hooks.Page;

        public ILocator ProjectsLink => _page.Locator("a.nav-link[href='/projects']");

        public async Task GoToAsync()
        {
            await _page.GotoAsync("https://dohertyalex.cc");
        }

        public async Task WaitForPageAsync()
        {
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        public async Task<string> GetTitleAsync()
        {
            return await _page.TitleAsync();
        }
    }
}

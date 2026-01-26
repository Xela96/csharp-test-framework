using Microsoft.Playwright;
using UI.Tests.Hooks;

namespace UI.Tests.Pages
{
    internal class ContactForm(UIHooks hooks)
    {
        private readonly IPage _page = hooks.Page;

        public ILocator NameInput => _page.GetByRole(AriaRole.Textbox, new() { Name = "name" });

        public ILocator EmailInput => _page.GetByRole(AriaRole.Textbox, new() { Name = "email" });

        public ILocator MessageInput => _page.GetByRole(AriaRole.Textbox, new() { Name = "message" });

        public ILocator SubmitButton => _page.Locator("input[value='Send Message']");
    }
}
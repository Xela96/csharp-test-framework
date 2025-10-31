# csharp-test-framework
Test framework in CSharp, demonstrating the structure for component testing, integration testing, API testing, data testing, performance testing, etc.

- NUnit test framework is used across solution for running tests and assertions.
- Reqnroll is used across the solution for BDD test scenarios.

## Project Breakdown
 * [Core](./CSharpTestFramework/Core)
    * Core functionality for the tests that can be used by any of the testing projects.
 * [UI.Tests](./CSharpTestFramework/UI.Tests)
    * UI tests are run using Playwright for GUI automation, and POM for components on each page of the web app.
 * [Data.Tests](./CSharpTestFramework/Data.Tests)
    * Data tests are run by connecting to Supabase through the Supabase Nuget package.


## Dependencies
### Nuget Packages
- DotNetEnv Version = 3.1.1
- Microsoft.Playwright.NUnit Version = 1.55.0
- NUnit Version = 4.4.0
- Reqnroll.NUnit Version = 3.2.0
- Supabase Version = 1.1.1
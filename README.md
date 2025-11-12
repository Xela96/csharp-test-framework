# csharp-test-framework
Test framework in CSharp, demonstrating the structure for component testing, integration testing, API testing, data testing, performance testing, etc.

- NUnit test framework is used across solution for running tests and assertions.
- Reqnroll is used across the solution for BDD test scenarios.

## Project Breakdown
 * [API.Tests](./CSharpTestFramework/API.Tests)
    * API tests using RestSharp to make calls and process responses.
 * [Core](./CSharpTestFramework/Core)
    * Core functionality for the tests that can be used by any of the testing projects.
 * [Data.Tests](./CSharpTestFramework/Data.Tests)
    * Data tests are run by connecting to Supabase through the Supabase Nuget package.
 * [Performance.Tests](./CSharpTestFramework/Performance.Tests)
    * Performance tests run Grafana K6 process and parse console output stream toget results to fit in with C# BDD structure. 
 * [UI.Tests](./CSharpTestFramework/UI.Tests)
    * UI tests are run using Playwright for GUI automation, and POM for components on each page of the web app.

## Dependencies
### Nuget Packages
- DotNetEnv Version = 3.1.1
- Microsoft.Playwright.NUnit Version = 1.55.0
- NUnit Version = 4.4.0
- Reqnroll.NUnit Version = 3.2.0
- Supabase Version = 1.1.1
- RestSharp Version = 112.1.0
- Serilog Version = 4.3.0
- Serilog.Sinks.File Version = 7.0.0
- Serilog.Sinks.Console Version = 1.1.1

## Setup
- From .\CSharpTestFramework:  
```dotnet build```
## Run Tests
- From .\CSharpTestFramework:  
```dotnet test [<project-name>] [--filter "TestCategory=<TestTag>"]```

Optional parameters: 
   * Project Name
   * Test filter

## Test Output
Log files for each test project is generated as an artifact following testing.
These can be found at:  
```test-logs/<project>/bin/Debug/net8.0/logs/<projectName>_<currentDateTime>.txt```

Similarly, screenshots are taken for the UI tests and can be found in the generated artifact at:   
```test-logs/<project>/bin/Debug/net8.0/screenshots/<test-name>.png```
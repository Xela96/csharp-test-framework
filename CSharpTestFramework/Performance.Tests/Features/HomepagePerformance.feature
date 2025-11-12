Feature: HomepagePerformance

As a sdet
I want to verify the homepage responds quickly under load
So that I can ensure the site remains performant for users

@mytag
Scenario: Verify homepage performance under load
	Given the performance test script homepage_performance.js
	When the load test is executed
	Then the appropriate status code should be returned
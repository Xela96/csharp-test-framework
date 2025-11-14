Feature: HomepageUI

@mytag
Scenario: Visit the homepage
	Given a user accessing the homepage
	When the page loads
	Then the page title is correct
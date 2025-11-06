Feature: HomepageUI

@mytag
Scenario: Visit the homepage
	Given a user accessing "https://dohertyalex.cc"
	When the page loads
	Then the page title is correct
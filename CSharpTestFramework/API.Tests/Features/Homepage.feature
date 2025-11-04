Feature: Homepage


@mytag
Scenario: Verify homepage status code is 200
	Given the homepage address "https://dohertyalex.cc"
	When the request is sent
	Then the response status should be Ok
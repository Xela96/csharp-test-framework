Feature: Homepage Content

As a site visitor
I want to see up to date content on the homepage
So that I can understand who the creator of the site is and what they offer

Background: 
	Given the homepage_content table is populated

@mytag
Scenario: Verify about me section content
	When the homepage is opened
	Then the about me content should be visible and matching the database
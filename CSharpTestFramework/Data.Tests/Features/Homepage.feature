Feature: Homepage Content

As a database consumer
I want to see valid data in the database
So that I can ensure usage of this database will not cause issues

Background: 
	Given the homepage_content table is populated

@mytag
Scenario: Verify about me section content
	When the about_me content is retrieved
	Then the text should match the expected value
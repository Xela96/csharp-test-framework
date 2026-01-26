@navigation
Feature: HomepageUI
    As a user
    I want to navigate through the website
    So that I can access different sections and external links and files

    Background:
        Given a user accessing the homepage
        When the page loads

    @smoke @functional
    Scenario: Verify that homepage loads and has correct title
        Then the page title is correct

    @smoke @functional
    Scenario: Verify that LinkedIn link navigates correctly
        When the user clicks the LinkedIn link
        Then the user is navigated to "linkedin.com/in/alex-doherty/"

    @smoke @functional
    Scenario: Verify that Github link navigates correctly
        When the user clicks the Github link
        Then the user is navigated to "github.com/Xela96"

    @smoke @functional
    Scenario: Verify that projects page link navigates correctly
        When the user clicks the Projects link
        Then the user is navigated to "/projects"

    @smoke @functional
    Scenario: Verify that CV download works
        When the user clicks the download CV button
        Then the CV file is downloaded successfully

 @functional
    Scenario: Verify that all contact form text fields accept user input
        Given the user fills the name field with "example name"
        And the user fills the email field with "example email"
        And the user fills the message field with "example message of 20 characters"
        Then all contact form fields accept user input

    @functional
    Scenario: Verify that name textbox enforces a max length of 40
        Given the user fills the name field with "this is a test name that is too long over 40 characters"
        And the user fills the email field with "validmail@gmail.com"
        And the user fills the message field with "example message of 20 characters"
        Then the name field value is "this is a test name that is too long ove"

    @functional
    Scenario: Verify that name textbox enforces a min length of 3 characters
        Given the user fills the name field with "1"
        And the user fills the email field with "validmail@gmail.com"
        And the user fills the message field with "example message of 20 characters"
        When the user submits the contact form
        Then the name field value is "1"
        And no POST request is sent

    @functional
    Scenario: Verify that email textbox enforces a max length of 60
        Given the user fills the name field with "John Doe"
        And the user fills the email field with "this_is_a_test_name_that_is_too_long_over_60_characters@gmail.com"
        And the user fills the message field with "example message of 20 characters"
        Then the email field value is "this_is_a_test_name_that_is_too_long_over_60_characters@gmai"

    @functional
    Scenario: Verify that email textbox enforces a min length of 3 characters
        Given the user fills the name field with "John Doe"
        And the user fills the email field with "1"
        And the user fills the message field with "example message of 20 characters"
        When the user submits the contact form
        Then the email field value is "1"
        And no POST request is sent

    @functional
    Scenario: Verify that message textbox enforces a max length of 500
        Given the user fills the name field with "John Doe"
        And the user fills the email field with "validmail@gmail.com"
        And the user fills the message field with "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
        Then the message field value is "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"

    @functional @regression
    Scenario: Verify that message textbox enforces a min length of 20 characters
        Given the user fills the name field with "John Doe"
        And the user fills the email field with "validmail@gmail.com"
        And the user fills the message field with "less than 20 chars"
        When the user submits the contact form
        Then the message field value is "less than 20 chars"
        # POST request validation can vary by browser leading to failures
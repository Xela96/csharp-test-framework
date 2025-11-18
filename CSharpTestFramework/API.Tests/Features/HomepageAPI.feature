Feature: HomepageAPI

@mytag
Scenario: Verify homepage status code is 200
	Given the homepage address
	When the GET request is sent
	Then the getResponse response status is 200

Scenario: Verify homepage content
	Given the homepage address
	When the GET request is sent
	Then the response body contains /download_cv
	Then the response body contains external links
	Then the response body contains projects page link

Scenario: Verify /download_cv status code is 200
	Given the homepage address with route "/download_cv"
	When the GET request is sent
	Then the getResponse response status is 200

Scenario: Verify /download_cv file content
	Given the homepage address with route "/download_cv"
	When the GET request is sent
	Then the response Content-Type is pdf
	Then the response filename is correct
	Then the file is not empty

Scenario: Verify /download_cv only accepts GET requests
	Given the homepage address with route "/download_cv"
	When the POST request is sent
	Then the postResponse response status is 405
	When the DELETE request is sent
	Then the deleteResponse response status is 405

@requiresCsrf
Scenario: Verify contact form with valid fields returns status code 200
	Given the homepage address
	When the POST request is sent with contact form body
	Then the postResponse response status is 200

# Failing, not implemented for this behaviour
@requiresCsrf
Scenario: Verify contact form with invalid fields returns status code 400
	Given the homepage address
	When the POST request is sent with contact form body:
	  |name       |email           |message
	  |Joe Bloggs |invalidmail |Valid me
	Then the postResponse response status is 400

@requiresCsrf @requiresLogin
Scenario: Verify logout requires valid authentication
	Given the homepage address with a logged in user
	When the GET request is sent with logout
	Then the response body contains logout link

# /logout (after being logged in) requires valid authentication
# returns status code X when logged out
# returns 200 when logged in
# POST /logout without CSRF returns 400 or 403
# POST /logout with CSRF and authenticated returns 200 or redirect
# 405 method not allowed for incorrect methods
# After calling /logout, test that:
#Session cookie is invalidated or replaced
#Session data no longer contains user info
#Auth-protected routes return 401/403 or redirect to login
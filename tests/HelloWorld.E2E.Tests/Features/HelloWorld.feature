Feature: Hello World page validation
  As a .NET developer learning BDD
  I want to validate a simple Hello World page in a real browser
  So that I understand how SpecFlow and Playwright work together

  @passing
  Scenario: Home page loads successfully
	Given the Hello World app is running
	When I open the home page
	Then the page should load successfully

  @passing
  Scenario: Hello World message is displayed
	Given the Hello World app is running
	When I open the home page
	Then I should see the "Hello World" message

  @passing
  Scenario: Page title is correct
	Given the Hello World app is running
	When I open the home page
	Then the page title should be "Hello World - SpecFlow + Playwright"

  @failing
  Scenario: Intentional failure to demonstrate debugging output
	Given the Hello World app is running
	When I open the home page
	Then the page title should be "Wrong Title For Demo"

  @passing
  Scenario: Home page button redirects to welcome page
    Given the Hello World app is running
    When I open the home page
    And I click the "Go to Welcome" button
    Then I should be redirected to "/welcome.html"
    And I should see the "Welcome" message

Feature: Navigate to About Steam Page

  Scenario: Search for FIFA and go to About Steam Page
    Given I launch chrome with modes: incognito and open the https://store.steampowered.com/ link
    
    When I search for a game "FIFA"
    Then first search is "EA SPORTS FC™ 25" and second is "FIFA 22"
    
    When I click on the search result #1
    Then app page displaying the matching search name

    When I click the "Download" button
    And I click the "No, I need Steam" button
    
    Then I should be on the About Steam Page
    And the "Install Steam" button is clickable
    And the number of users playing now is less than online
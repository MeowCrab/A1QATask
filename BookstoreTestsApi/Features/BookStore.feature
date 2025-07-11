Feature: BookStore API tests

  Scenario: Add and replace a book in the user's list
    Given I create and authorize random user
    When I get all books
    Then the response status is 200
    And book collection is not empty
    When I add the first book to the user's book list
    Then the response status is 201
    When I get the user by id
    Then the response status is 200
    And users book list contains 1 books
    And users book list contains 1th book from collection
    When I replace the 1th book in users collection with the 2th book from the list
    Then the response status is 200
    When I get the user by id
    Then the response status is 200
    And users book list contains 1 books
    And users book list contains 2th book from collection

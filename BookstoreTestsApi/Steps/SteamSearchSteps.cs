using Reqnroll;
using NUnit.Framework;
using BookstoreTestsApi.Helpers;
using BookstoreTestsApi.Models;
using RestSharp;
using System.Net;
using BookstoreTestsApi.DTOs;

namespace SteamUITests.Steps
{
    [Binding]
    public class BookSotreSteps
    {
        [Binding]
        public class BookStoreSteps
        {
            private readonly BookStoreApi _bookStoreApi;
            private readonly AccountApi _accountApi;
            private readonly ScenarioContext _scenarioContext;

            public BookStoreSteps(ScenarioContext scenarioContext)
            {
                var _client = new RestClient("https://bookstore.toolsqa.com");
                _bookStoreApi = new BookStoreApi(_client);
                _accountApi = new AccountApi(_client);
                _scenarioContext = scenarioContext;
            }

            [Given(@"I create and authorize random user")]
            public async Task GivenICreateAUserAndAuthorize()
            {
                var username = $"user{Guid.NewGuid().ToString().Substring(0, 8)}";
                var password = $"StrongPassword{Guid.NewGuid().ToString().Substring(0, 8)}!";
                _scenarioContext["user"] = await _accountApi.CreateUser(username, password);
                _scenarioContext["token"] = await _accountApi.GenerateToken(username, password);
            }

            [When(@"I get all books")]
            public async Task WhenIGetAllBooks()
            {
                _scenarioContext["previousResponse"] = await _bookStoreApi.GetAllBooks();
            }

            [Then(@"the response status is (.*)")]
            public void ThenResponseStatus(HttpStatusCode code)
            {
                var _previousResponse = _scenarioContext["previousResponse"] as RestResponse;
                Assert.That(_previousResponse.StatusCode == code,
                    $"Expected {code} status but was {_previousResponse.StatusCode}");
            }

            [Then(@"book collection is not empty")]
            public void ThenBookCollectionNotEmpty()
            {
                var allBooks = GetDataFromPreviousResponse<BooksResponse>().Books;
                Assert.That(allBooks != null, "Book collection does not exist");
                Assert.That(allBooks?.Count > 0, "Book collection is empty");
                _scenarioContext["allBooks"] = allBooks;
            }

            [When(@"I add the first book to the user's book list")]
            public async Task WhenIAddFirstBook()
            {
                var user = _scenarioContext["user"] as User;
                var allBooks = _scenarioContext["allBooks"] as List<Book>;
                var token = _scenarioContext["token"] as string;
                _scenarioContext["previousResponse"] = await _bookStoreApi.AddBookToUser(user.UserId, allBooks.First().Isbn, token);
            }

            [When(@"I get the user by id")]
            public async Task WhenIGetUserById()
            {
                var user = _scenarioContext["user"] as User;
                var token = _scenarioContext["token"] as string;
                _scenarioContext["previousResponse"] = await _accountApi.GetUserById(user.UserId, token);
            }

            [Then(@"users book list contains (.*) books")]
            public void ThenUserBookListContainsNBooks(int count)
            {
                var user = GetDataFromPreviousResponse<User>();
                Assert.That(user.Books.Count == count,
                    $"Expected {count} books in users collection but was {user.Books.Count}");
                _scenarioContext["user"] = user;
            }

            [Then(@"users book list contains (.*)th book from collection")]
            public void ThenUserBookListContainsNthBookFromCollection(int id)
            {
                var user = GetDataFromPreviousResponse<User>();
                var allBooks = _scenarioContext["allBooks"] as List<Book>;
                var etalonIsbn = allBooks[id - 1].Isbn;
                Assert.That(user.Books.Any(b => b.Isbn.Equals(etalonIsbn)),
                    "Expected book not found in a users collection");
                _scenarioContext["user"] = user;
            }

            [When(@"I replace the (.*)th book in users collection with the (.*)th book from the list")]
            public async Task WhenIReplaceBook(int userBookId, int newBookId)
            {
                var user = _scenarioContext["user"] as User;
                var allBooks = _scenarioContext["allBooks"] as List<Book>;
                var token = _scenarioContext["token"] as string;
                _scenarioContext["previousResponse"] = await _bookStoreApi.ReplaceBookForUser(
                    user.UserId,
                    user.Books[userBookId - 1].Isbn,
                    allBooks[newBookId - 1].Isbn,
                    token
                );
            }

            [Then(@"I delete the created user")]
            public async Task Dispose()
            {
                var user = _scenarioContext["user"] as User;
                var token = _scenarioContext["token"] as string;
                await _accountApi.DeleteUser(user.UserId, token);
            }

            private T GetDataFromPreviousResponse<T>()
            {
                var _previousResponse = _scenarioContext["_previousResponse"] as RestResponse<T>;
                if(_previousResponse.Data == null) throw new NullReferenceException("Response has no data in it");
                T data = _previousResponse.Data;
                return data;
            }
        }
    }
}
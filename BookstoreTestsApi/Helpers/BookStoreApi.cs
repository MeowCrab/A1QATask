using BookstoreTestsApi.DTOs;
using BookstoreTestsApi.Models;
using RestSharp;

namespace BookstoreTestsApi.Helpers
{
    public class BookStoreApi
    {
        private readonly RestClient _client;

        public BookStoreApi(RestClient client)
        {
            _client = client;
        }

        public async Task<RestResponse<BooksResponse>> GetAllBooks()
        {
            var request = new RestRequest("/BookStore/v1/Books", Method.Get);
            var response = await _client.ExecuteAsync<BooksResponse>(request);
            return response;
        }

        public async Task<RestResponse> AddBookToUser(string userId, string isbn, string token)
        {
            var request = new RestRequest("/BookStore/v1/Books", Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddJsonBody(new { userId, collectionOfIsbns = new[] { new { isbn } } });
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> ReplaceBookForUser(string userId, string oldIsbn, string newIsbn, string token)
        {
            var request = new RestRequest("/BookStore/v1/Books/{isbn}", Method.Put);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddUrlSegment("isbn", oldIsbn);
            request.AddJsonBody(new { userId, isbn = newIsbn });
            return await _client.ExecuteAsync(request);
        }
    }
}

using BookstoreTestsApi.DTOs;
using BookstoreTestsApi.Models;
using RestSharp;

namespace BookstoreTestsApi.Helpers
{
    public class AccountApi
    {
        private readonly RestClient _client;

        public AccountApi(RestClient client)
        {
            _client = client;
        }

        public async Task<User> CreateUser(string username, string password)
        {
            var request = new RestRequest("/Account/v1/User", Method.Post);
            request.AddJsonBody(new UserCreateRequest
            {
                UserName = username,
                Password = password
            });

            var response = await _client.ExecuteAsync<UserCreateResponse>(request);

            if (!response.IsSuccessful || response.Data == null)
            {
                throw new Exception($"Failed to create user: {response.StatusCode} - {response.Content}");
            }

            return new User
            {
                UserId = response.Data.UserID,
                Username = response.Data.Username,
                Books = new List<Book>()
            };
        }

        public async Task<string> GenerateToken(string username, string password)
        {
            var request = new RestRequest("/Account/v1/GenerateToken", Method.Post);
            request.AddJsonBody(new GenerateTokenRequest
            {
                UserName = username,
                Password = password
            });

            var response = await _client.ExecuteAsync<GenerateTokenResponse>(request);

            if (!response.IsSuccessful || response.Data == null || string.IsNullOrEmpty(response.Data.Token))
            {
                throw new Exception($"Failed to generate token: {response.StatusCode} - {response.Content}");
            }

            return response.Data.Token;
        }

        public async Task<RestResponse<User>> GetUserById(string userId, string token)
        {
            var request = new RestRequest($"/Account/v1/User/{userId}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token}");
            var response = await _client.ExecuteAsync<User>(request);
            return response;
        }

        public async Task DeleteUser(string userId, string token)
        {
            var request = new RestRequest($"/Account/v1/User/{userId}", Method.Delete);
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to delete user: {response.StatusCode} - {response.Content}");
            }
        }
    }
}

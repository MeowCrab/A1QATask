﻿namespace BookstoreTestsApi.DTOs
{
    public class GenerateTokenResponse
    {
        public string Token { get; set; }
        public string Expires { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
    }
}

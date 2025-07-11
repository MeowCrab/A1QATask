namespace BookstoreTestsApi.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public List<Book> Books { get; set; }
    }
}

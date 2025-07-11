using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookstoreTestsApi.Models;

namespace BookstoreTestsApi.DTOs
{
    public class BooksResponse
    {
        public List<Book> Books {  get; set; }
    }
}

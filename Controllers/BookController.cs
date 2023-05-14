using BookApp.Models;
using BookApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookApp.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public BookController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooksInfo()
        {
            string[] isbns = BookService.GetISBNsFromTextFile("ISBN_Input_File.txt");
            var books = new List<Book>();

            using (var client = new HttpClient())
            {
                foreach (string isbn in isbns)
                {
                    string dataRetrievalType;

                    if (!_cache.TryGetValue(isbn, out Book data))
                    {
                        var apiURL = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";

                        var response = await client.GetAsync(apiURL);

                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();

                            data = BookService.GetBookInfoFromJson(isbn, json);

                            _cache.Set(isbn, data, TimeSpan.FromMinutes(60));

                            dataRetrievalType = "Server";
                        }
                        else
                        {
                            throw new Exception($"Request for ISBN {isbn} failed with status code {response.StatusCode}.");
                        }
                    }
                    else
                    {
                        dataRetrievalType = "Cache";
                    }

                    var book = new Book
                    {
                        ISBN = isbn,
                        Title = data.Title,
                        Subtitle = data.Subtitle,
                        Authors = data.Authors,
                        Number_of_pages = data.Number_of_pages,
                        Publish_date = data.Publish_date,
                        DataRetrievalType = dataRetrievalType
                    };

                    books.Add(book);
                }

                BookService.WriteCsvFileToDisk(books, $@"C:\Users\{Environment.UserName}\Desktop\Books.csv");
            }

            if (books.Count > 0)
            {
                return Ok(books);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

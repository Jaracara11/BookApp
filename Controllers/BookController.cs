using BookApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BookApp.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Book>> GetBookInfo()
        {
            string filePath = Path.Combine("data", "ISBN_Input_File.txt");
            string isbn = System.IO.File.ReadAllText(filePath);

            string apiUrl = $"https://openlibrary.org/isbn/{isbn}";
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                    var data = await JsonSerializer.DeserializeAsync<Book>(stream);
                    var book = new Book
                    {
                        ISBN = data.ISBN,
                        Title = data.Title,
                        Subtitle = data.Subtitle,
                        AuthorName = data.AuthorName,
                        NumberOfPages = data.NumberOfPages,
                        PublishDate = data.PublishDate
                    };
                    return Ok(book);
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}

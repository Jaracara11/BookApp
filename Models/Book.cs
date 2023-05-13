using static BookApp.Shared.BookService;

namespace BookApp.Models
{
    public class Book
    {
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public List<Author>? Authors { get; set; }
        public int Number_of_pages { get; set; }
        public string Publish_date { get; set; } = string.Empty;
        public string DataRetrievalType { get; set; } = string.Empty;
    }
}

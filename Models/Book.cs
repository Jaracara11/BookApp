namespace BookApp.Models
{
    public class Book
    {
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int NumberOfPages { get; set; }
        public DateTime PublishDate { get; set; }
    }
}

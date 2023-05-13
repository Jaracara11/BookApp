using BookApp.Models;
using System.Text.Json;

namespace BookApp.Shared
{
    public static class BookService
    {
        public static string[] GetISBNsFromTextFile(string fileName)
        {
            string filePath = Path.Combine("data", fileName);
            string fileContent = File.ReadAllText(filePath);
            string[] isbns = fileContent.Replace("\r\n", "").Replace("\n", "").Replace(" ", "").Split(',');

            return isbns;
        }

        public static Book GetBookInfoFromJson(string isbn, string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var bookDictionary = JsonSerializer.Deserialize<Dictionary<string, Book>>(json, options);

            Book? book = bookDictionary?[$"ISBN:{isbn}"];

            if (book != null)
            {
                return book;
            }
            else
            {
                throw new Exception("Deserialized data is null.");
            }
        }

        public static void WriteCsvFileToDisk(List<Book> books, string savePath)
        {
            var rowNumber = 0;

            using var streamWriter = new StreamWriter(savePath, true);

            if (rowNumber == 0)
            {
                streamWriter.WriteLine("Row Number,Data Retrieval Type,ISBN,Title,Subtitle,Author Name(s),Number of Pages,Publish Date");
            }

            foreach (var book in books)
            {
                rowNumber++;

                var authors = book.Authors == null ? "" : string.Join(" - ", book.Authors.Select(a => a.Name));
                var formattedPublishDate = "\"" + book.Publish_date + "\"";

                streamWriter.WriteLine($"{rowNumber},{book.DataRetrievalType},{book.ISBN},{book.Title},{book.Subtitle},{authors},{book.Number_of_pages},{formattedPublishDate}");
            }
        }
    }
}

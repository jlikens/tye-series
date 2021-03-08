using System;

namespace api.books.Models
{
    public class Book
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public DateTime PublishDate{ get; set; }

        public Person Author { get; set; }

        public int PageCount { get; set; }

        public string Genre { get; set; }
    
        public int AuthorID { get; set; }
    }
}
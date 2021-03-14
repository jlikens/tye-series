using System;

namespace api.books.Resources
{
    public class BookResource
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public PersonResource Author { get; set; }

        public int PageCount { get; set; }

        public string Genre { get; set; }
    }
}
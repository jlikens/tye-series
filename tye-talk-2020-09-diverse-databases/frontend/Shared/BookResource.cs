using System;

namespace frontend.Shared
{
    public class BookResource
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public AuthorResource Author { get; set; }

        public int PageCount { get; set; }

        public string Genre { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace api.books.Resources
{
    public class BookResource
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public PersonResource Author { get; set; }

        public int PageCount { get; set; }

        public string Genre { get; set; }
    }
}
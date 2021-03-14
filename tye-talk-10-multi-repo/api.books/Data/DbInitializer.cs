using System;
using System.Linq;
using api.books.Models;

namespace api.books.Data
{
    public static class BookContextDbInitializer
    {
        public static void Initialize(BookContext context)
        {
            context.Database.EnsureCreated();

            // Look for any books.
            if (context.Books.Any())
            {
                return;   // DB has been seeded
            }

            var random = new Random();

            var authors = new Person[]
            {
                new Person { FirstMidName = "Kevin",   LastName = "Copeland" },
                new Person { FirstMidName = "Maritza", LastName = "Torres" },
                new Person { FirstMidName = "Kathy",   LastName = "Whitney" },
                new Person { FirstMidName = "Deegan",  LastName = "Byrd" },
                new Person { FirstMidName = "Braylon", LastName = "Deleon" }
            };
            context.People.AddRange(authors);
            context.SaveChanges();

            var books = new Book[]
            {
                new Book { Title = "The Fellowship of the Bracelet", AuthorId = authors.Single( i => i.LastName == "Copeland").Id, Genre = "Fantasy", PageCount = 342, PublishDate = new DateTime(1996, 12, 5) },
                new Book { Title = "The Three Towers", AuthorId = authors.Single( i => i.LastName == "Copeland").Id, Genre = "Fantasy", PageCount = 433, PublishDate = new DateTime(1997, 9, 22) },
                new Book { Title = "The Return of the Queen", AuthorId = authors.Single( i => i.LastName == "Copeland").Id, Genre = "Fantasy", PageCount = 641, PublishDate = new DateTime(1999, 5, 16) },
                new Book { Title = "Lincoln + Socrates", AuthorId = authors.Single( i => i.LastName == "Torres").Id, Genre = "Historical Fiction", PageCount = 250, PublishDate = new DateTime(2001, 4, 17) },
                new Book { Title = "Child Psychology for Dummies", AuthorId = authors.Single( i => i.LastName == "Whitney").Id, Genre = "Psychology", PageCount = 271, PublishDate = new DateTime(1975, 3, 22) },
                new Book { Title = "Laughs Etc.", AuthorId = authors.Single( i => i.LastName == "Byrd").Id, Genre = "Comedy", PageCount = 605, PublishDate = new DateTime(1820, 10, 21) },
                new Book { Title = "Laughs Etc. II: More Laughs", AuthorId = authors.Single( i => i.LastName == "Byrd").Id, Genre = "Comedy", PageCount = 474, PublishDate = new DateTime(1825, 11, 9) },
                new Book { Title = "Plato Discussions", AuthorId = authors.Single( i => i.LastName == "Deleon").Id, Genre = "Philosophy", PageCount = 488, PublishDate = new DateTime(2012, 5, 3) },
                new Book { Title = "Nietzshe on Plants", AuthorId = authors.Single( i => i.LastName == "Deleon").Id, Genre = "Philosophy", PageCount = 880, PublishDate = new DateTime(2015, 7, 12) },
                new Book { Title = "Camus Can Do", AuthorId = authors.Single( i => i.LastName == "Deleon").Id, Genre = "Philosophy", PageCount = 294, PublishDate = new DateTime(2021, 1, 1) },
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
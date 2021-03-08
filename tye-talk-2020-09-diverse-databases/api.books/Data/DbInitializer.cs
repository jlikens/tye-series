using System;
using System.Linq;
using api.books.Models;

namespace api.books.Data
{
    public static class SchoolContextDbInitializer
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
                new Book { Title = "The Fellowship of the Bracelet", AuthorID = authors.Single( i => i.LastName == "Copeland").ID, Genre = "Fantasy", PageCount = 342, PublishDate = new DateTime(1996, 12, 5) },
                new Book { Title = "The Three Towers", AuthorID = authors.Single( i => i.LastName == "Copeland").ID, Genre = "Fantasy", PageCount = 342, PublishDate = new DateTime(1997, 9, 22) },
                new Book { Title = "The Return of the Queen", AuthorID = authors.Single( i => i.LastName == "Copeland").ID, Genre = "Fantasy", PageCount = 342, PublishDate = new DateTime(1999, 5, 16) },
                new Book { Title = "Lincoln + Socrates", AuthorID = authors.Single( i => i.LastName == "Torres").ID, Genre = "Historical Fiction", PageCount = 342, PublishDate = new DateTime(2001, 4, 17) },
                new Book { Title = "Child Psychology for Dummies", AuthorID = authors.Single( i => i.LastName == "Whitney").ID, Genre = "Psychology", PageCount = 342, PublishDate = new DateTime(1975, 3, 22) },
                new Book { Title = "Laughs Etc.", AuthorID = authors.Single( i => i.LastName == "Byrd").ID, Genre = "Comedy", PageCount = 342, PublishDate = new DateTime(1820, 10, 21) },
                new Book { Title = "Laughs Etc. II: More Laughs", AuthorID = authors.Single( i => i.LastName == "Byrd").ID, Genre = "Comedy", PageCount = 342, PublishDate = new DateTime(1825, 11, 9) },
                new Book { Title = "Plato Discussions", AuthorID = authors.Single( i => i.LastName == "Deleon").ID, Genre = "Philosophy", PageCount = 342, PublishDate = new DateTime(2012, 5, 3) },
                new Book { Title = "Nietzshe on Plants", AuthorID = authors.Single( i => i.LastName == "Deleon").ID, Genre = "Philosophy", PageCount = 342, PublishDate = new DateTime(2015, 7, 12) },
                new Book { Title = "Camus Can Do", AuthorID = authors.Single( i => i.LastName == "Deleon").ID, Genre = "Philosophy", PageCount = 342, PublishDate = new DateTime(2021, 1, 1) },
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
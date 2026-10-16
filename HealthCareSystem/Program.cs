
using LibrarySystem.Data;
using LibrarySystem.Entities;
using Microsoft.EntityFrameworkCore;

using var context = new LibraryContext();

if (!context.Authors.Any())
{
    context.Authors.AddRange(
        new Author
        {
            Name = "Ahmed Khaled",
            BirthDate = new DateTime(1980, 5, 12)
        },
        new Author
        {
            Name = "Mona Samir",
            BirthDate = new DateTime(1975, 8, 20)
        }
    );

    context.SaveChanges();
}

if (!context.Books.Any())
{
    var author = context.Authors.First();

    context.Books.AddRange(
        new Book
        {
            Title = "Learn C#",
            ISBN = "978100000001",
            AuthorId = author.Id
        },
        new Book
        {
            Title = "Learn Entity Framework Core",
            ISBN = "978100000002",
            AuthorId = author.Id
        }
    );

    context.SaveChanges();
}

if (!context.Borrowers.Any())
{
    context.Borrowers.AddRange(
        new Borrower
        {
            Name = "Ali Hassan",
            MembershipDate = DateTime.Today.AddMonths(-3)
        },
        new Borrower
        {
            Name = "Sara Mohamed",
            MembershipDate = DateTime.Today.AddMonths(-1)
        }
    );

    context.SaveChanges();
}

if (!context.Loans.Any())
{
    var book = context.Books.First();
    var borrower = context.Borrowers.First();

    context.Loans.Add(new Loan
    {
        BookId = book.Id,
        BorrowedId = borrower.Id,
        LoanDate = DateTime.Today,
        ReturnDate = DateTime.Today.AddDays(14)
    });

    context.SaveChanges();
}

Console.WriteLine("=== Books and Authors ===");

var books = context.Books
    .Include(b => b.Author)
    .ToList();

foreach (var book in books)
{
    Console.WriteLine($"Book: {book.Title}");
    Console.WriteLine($"ISBN: {book.ISBN}");
    Console.WriteLine($"Author: {book.Author.Name}");
    Console.WriteLine("--------------------");
}

Console.WriteLine("\n=== Loans ===");

var loans = context.Loans
    .Include(l => l.Book)
    .Include(l => l.Borrower)
    .ToList();

foreach (var loan in loans)
{
    Console.WriteLine($"Book: {loan.Book.Title}");
    Console.WriteLine($"Borrower: {loan.Borrower.Name}");
    Console.WriteLine($"Loan Date: {loan.LoanDate:d}");
    Console.WriteLine($"Return Date: {loan.ReturnDate:d}");
    Console.WriteLine("--------------------");
}

using LibraryMS.DataBase;
using LibraryMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMS.Services
{
    public class BookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Add a new book
        public void AddBook(Book book)
        {
            if (book == null)
            {
                Console.WriteLine("Can't Add Empty Book ..");
                return;
            }
            var results = new List<ValidationResult>();
            var context = new ValidationContext(book);
            bool isValidDate = Validator.TryValidateObject(book, context, results, true);
            if (!isValidDate)
            {
                Console.WriteLine("Book data is not valid: " + string.Join("; ", results.Select(r => r.ErrorMessage)));
                return;
            }
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                Console.WriteLine("Book Added Successfully ..");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("Database Error: " + dbEx.InnerException?.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex Error: " + ex.Message);
            }
        }
        // Get All Books
        public void GetAllBooks()
        {
            var Categories = _context.Categories
                .Include(bo => bo.Books)
                .OrderByDescending(bo => bo.Books.Count)
                .ToList();
            foreach (var item in Categories)
            {
                if (item.Books.Count == 0)
                {
                    Console.WriteLine("There are not Books available in this Category");
                    continue;
                }
                Console.WriteLine($"The CategoryName : {item.Name}");
                foreach (var book in item.Books)
                {
                    Console.WriteLine($"Book : {book}");
                }
            }
        }
        // Search in Book using Id
        public Book? GetBookUsingId(int id)
        {
            var result = _context.Books.
                FirstOrDefault(book => book.Id == id);
            if (result == null)
            {
                Console.WriteLine("The Book with the Id is not valid");
                return null;
            }
            return result;
        }
        // Search in Book using Title
        public void BookUsingTitle(string title)
        {
            var results = _context.Books.
                Where(book => book.Title.ToLower() == (title.ToLower()))
                .ToList();
            if (results.Count == 0)
            {
                Console.WriteLine("No Books Found with the given Title");
                return;
            }
            Console.WriteLine($"The Book with the Title {title}");
            foreach (var book in results)
            {
                Console.WriteLine($"Book {book}");
            }
        }
        // Search in Book using Author
        public void BookUsingAuthor(string author)
        {
            var results = _context.Books.
                Where(book => book.Author.ToLower() == (author.ToLower()))
                .ToList();
            if (results.Count == 0)
            {
                Console.WriteLine("No Books Found with the given Author");
                return;
            }
            foreach (var book in results)
            {
                Console.WriteLine($"Book {book}");
            }
        }
        // Search in Book using CategoryId
        public void BookUsingCategoryId(int CategoryId)
        {
            var Category = _context.Categories
                .FirstOrDefault(cat => cat.Id == CategoryId);
            if (Category == null)
            {
                Console.WriteLine("This Id Is not Valid please Using a valid id..");
                return;
            }
            var results = _context.Books.Select(book => book)
                .Where(book => book.CategoryId == Category.Id)
                .ToList();
            if (results.Count == 0)
            {
                Console.WriteLine("There are not Book in this Category");
                return;
            }
            Console.WriteLine($"The Books in the Category {Category.Name} :");
            foreach (var book in results)
            {
                Console.WriteLine($"Book {book}");
            }
        }
        // Search in Book Using ISBN
        public void BookUsingISBN(string isbn)
        {
            var result = _context.Books.
                Where(book => book.ISBN == isbn)
                .OrderBy(b => b.publishedYear)
                .ToList();
            if (result == null)
            {
                Console.WriteLine("No Books Found with the given ISBN");
                return;
            }
            Console.WriteLine($"The Book with the ISBN {isbn} :");
            foreach (var book in result)
            {
                Console.WriteLine($"Book {book}");
            }
        }
        // Update Book 
        public void UpdateBook(Book book , int id)
        {
            var isExist = _context.Books.FirstOrDefault(bo => bo.Id == id);
            if (isExist == null)
            {
                Console.WriteLine($"There is no Book with the Id {id} please using a valid id");
                return;
            }
            var results = new List<ValidationResult>();
            var context = new ValidationContext(book);
            bool isValidDate = Validator.TryValidateObject(book, context, results, true);
            if (!isValidDate)
            {
                Console.WriteLine("Book data is not valid: " + string.Join("; ", results.Select(r => r.ErrorMessage)));
                return;
            }
            try
            {

                isExist.Author = book.Author;
                isExist.ISBN = book.ISBN;
                isExist.publishedYear = book.publishedYear;
                isExist.CategoryId = book.CategoryId;
                isExist.CopiesAvalible = book.CopiesAvalible;
                isExist.Title = book.Title;
                _context.SaveChanges();
                Console.WriteLine("Book updated successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message} ");
            }
        }
        //Delete Book Using Id 
        public void DeleteBookUsingId (int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                Console.WriteLine("The Book is Not Valid Using this Id Please Enter a Valid Id");
                return;
            }
            try
            {
                _context.Remove(book);
                _context.SaveChanges();
                Console.WriteLine("Book deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while deleting: " + ex.Message);
            }
        }
        // Check if the Book is available for borrowing
        public bool isValidForBorrowing (int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                Console.WriteLine($"Sorry Bro {book?.Title} not avalible now...");
                return false;
            }
            return true;
        }
        // Get available copies count
        public int AvalilableCopiesOfBook (int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                Console.WriteLine($"Sorry Bro {book?.Title} not avalible now...");
                return 0;
            }
            return book.CopiesAvalible;
        }
    }
}

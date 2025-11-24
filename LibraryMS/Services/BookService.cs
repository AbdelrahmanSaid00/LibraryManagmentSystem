using LibraryMS.DataBase;
using LibraryMS.Models;
using Microsoft.EntityFrameworkCore;
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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMS.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime publishedYear { get; set; }
        public int CopiesAvalible { get; set; }
        public int CategoryId { get; set; }
        public List<BorrowRecord> borrowRecords { get; set; }
        public Category category { get; set; }
    }
}

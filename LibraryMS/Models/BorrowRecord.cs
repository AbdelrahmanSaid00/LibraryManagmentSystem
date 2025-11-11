using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMS.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime BarrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Book book { get; set; }
        public Member member { get; set; }
    }
}

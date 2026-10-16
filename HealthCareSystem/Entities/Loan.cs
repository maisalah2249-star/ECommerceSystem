using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int BorrowedId { get; set; }
        public Borrower Borrower { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }


    }
}

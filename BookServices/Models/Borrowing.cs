using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookServices.Models
{
    public class Borrowing
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public bool Returned { get; set; }
    }
}

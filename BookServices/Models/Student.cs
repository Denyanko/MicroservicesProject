namespace BookServices.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Borrowing> Borrowings { get; set; }
    }
}

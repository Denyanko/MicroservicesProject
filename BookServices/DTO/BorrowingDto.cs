namespace BookServices.DTO
{
    public class BorrowingDto
    {
        public int Id { get; set; }
        public BookDto Book { get; set; }
        public StudentDto Student { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnedDate { get; set; }
    }
}

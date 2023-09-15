namespace BookServices.DTO
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public List<BookDto> Books { get; set; }
    }
}

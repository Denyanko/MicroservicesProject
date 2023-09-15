namespace BookServices.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public AuthorDto Author { get; set; }
        public List<GenreDto> Genres { get; set; }
    }
}

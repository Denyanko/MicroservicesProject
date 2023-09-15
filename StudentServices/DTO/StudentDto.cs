namespace StudentServices.DTO
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public ParentDto Parent { get; set; }
    }
}

namespace StudentServices.Model
{
    public class Parent
    {
        public int Id { get; set; }
        public string FatherName { get; set; }
        public string FatherAddress { get; set; }
        public string FatherPhone { get; set; }
        public string MotherName { get; set; }
        public string MotherAddress { get; set; }
        public string MotherPhone { get; set; }
        public Student Student { get; set; }
    }
}

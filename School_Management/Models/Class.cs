namespace School_Management.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Section { get; set; }

        public int TeacherId { get; set; }

        // navigation property
        public Teacher Teacher { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}

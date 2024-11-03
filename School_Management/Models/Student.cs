namespace School_Management.Models
{
    public class Student
    {
        public int Id { get; set; } // Primary Key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } // e.g., Male, Female, Other
        public string Address { get; set; }
        public string ContactNumber { get; set; }

        // Navigation property

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}

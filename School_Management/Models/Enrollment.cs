namespace School_Management.Models
{
    public class Enrollment
    {
        public int Id { get; set; } // Primary Key

        // Foreign Keys
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }

        // Properties
        public DateTime EnrollmentDate { get; set; }

        public Student? Student { get; set; }
        public Class? Class { get; set; }
        public Subject? Subject { get; set; }

    }
}

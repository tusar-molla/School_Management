namespace School_Management.Models
{
    public class Grade
    {
        public int Id { get; set; } // Primary Key

        // Foreign Keys
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        // Properties
        public string GradeLetter { get; set; }
        public decimal Score { get; set; }

        // Navigation Properties
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}

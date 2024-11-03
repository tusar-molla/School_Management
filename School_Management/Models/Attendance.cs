namespace School_Management.Models
{
    public class Attendance
    {
        public int Id { get; set; } // Primary Key

        // Foreign Keys
        public int StudentId { get; set; }
        public int ClassId { get; set; }

        // Properties
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        // Navigation Properties
        public Student Student { get; set; }
        public Class Class { get; set; }
    }
}

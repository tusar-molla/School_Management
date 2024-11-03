using System.Security.Claims;

namespace School_Management.Models
{
    public class Teacher
    {
        public int Id { get; set; } // Primary Key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Qualification { get; set; }
        public string SubjectSpecialization { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }

        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}

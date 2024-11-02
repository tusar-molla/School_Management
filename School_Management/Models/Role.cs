namespace School_Management.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }  // Examples: "Admin", "Teacher", "Student"

        // Navigation Property: a Role can have multiple Users
        public ICollection<User> Users { get; set; }
    }
}

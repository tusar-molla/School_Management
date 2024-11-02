namespace School_Management.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }      // User's login name
        public string PasswordHash { get; set; }  // Store hashed password for security
        public int RoleId { get; set; }           // Foreign key to Role

        // Navigation Property: each User has one Role
        public Role Role { get; set; }
    }
}

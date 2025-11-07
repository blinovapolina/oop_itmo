namespace UniversitySystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }

        public Teacher(int id, string firstName, string lastName, string email, string department)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Department = department;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} - {Department}";
        }
    }
}
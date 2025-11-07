namespace UniversitySystem.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Group { get; set; }

        public Student(int id, string firstName, string lastName, string email, string group)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Group = group;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Email}) — группа {Group}";
        }
    }
}
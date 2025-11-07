namespace UniversitySystem.Models
{
     public enum CourseType
    {
        Online,
        Offline
    }
    public abstract class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Student> Students { get; set; }
        public CourseType Type { get; protected set; }

        protected Course(int id, string name, string description, int credits)
        {
            Id = id;
            Name = name;
            Description = description;
            Credits = credits;
            Students = new List<Student>();
        }

        public void AssignTeacher(Teacher teacher)
        {
            Teacher = teacher;
        }

        public void AddStudent(Student student)
        {
            if (!Students.Any(person => person.Id == student.Id))
            {
                Students.Add(student);
            }
        }

        public void RemoveStudent(Student student)
        {
            var studentToRemove = Students.FirstOrDefault(person => person.Id == student.Id);
            if (studentToRemove != null)
            {
                Students.Remove(studentToRemove);
            }
        }

        public abstract string GetCourseDetails();

        public override string ToString()
        {
            return $"{Name} ({Type}) - {Teacher?.FirstName ?? "Не назначен преподаватель"} - Количество студентов: {Students.Count}";
        }
    }

    public class OfflineCourse : Course
    {
        public string Classroom { get; set; }
        public string Location { get; set; }
        public string Schedule { get; set; }
        public int HoldingCapacity { get; set; }

        public OfflineCourse(int id, string name, string description, int credits,
                            string classroom, string location, string schedule, int capacity = 30)
            : base(id, name, description, credits)
        {
            Classroom = classroom;
            Location = location;
            Schedule = schedule;
            HoldingCapacity = capacity;
            Type = CourseType.Offline;
        }

        public override string GetCourseDetails()
        {
            return $"{Name} (оффлайн-курс)\n" +
                    $"Преподаватель: {Teacher?.ToString() ?? "Не назначен"}\n" +
                    $"Аудитория: {Classroom}\n" +
                    $"Адрес: {Location}\n" +
                    $"Расписание: {Schedule}\n" +
                    $"Вместимость: {HoldingCapacity}\n" +
                    $"Учебные часы: {Credits}\n" +
                    $"Количество записанных студентов: {Students.Count}";
        }

        public bool HasAvailableSeats()
        {
            return Students.Count < HoldingCapacity;
        }

        public int AvailableSeatsCount()
        {
            return HoldingCapacity - Students.Count;
        }
    }

    public class OnlineCourse : Course
    {
        public string Platform { get; set; }
        public string MeetingLink { get; set; }

        public OnlineCourse(int id, string name, string description, int credits,
                           string platform, string meetingLink)
            : base(id, name, description, credits)
        {
            Platform = platform;
            MeetingLink = meetingLink;
            Type = CourseType.Online;
        }

        public override string GetCourseDetails()
        {
            return $"{Name} (онлайн-курс)\n" +
                    $"Преподаватель: {Teacher?.ToString() ?? "Не назначен"}\n" +
                    $"Платформа: {Platform}\n" +
                    $"Ссылка: {MeetingLink}\n" +
                    $"Учебные часы: {Credits}\n" +
                    $"Количество записанных студентов: {Students.Count}";
        }
    }
}
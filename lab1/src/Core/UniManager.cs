using UniversitySystem.Models;
using UniversitySystem.Interface;

namespace UniversitySystem.Core
{
    public class UniManager : InterfaceUniManager
    {
        private List<Course> _courses;
        private List<Student> _students;
        private List<Teacher> _teachers;

        public UniManager()
        {
            _courses = new List<Course>();
            _students = new List<Student>();
            _teachers = new List<Teacher>();
        }

        public int TotalTeachersCount => _teachers.Count;
        public int TotalStudentsCount => _students.Count;
        public int TotalCoursesCount => _courses.Count;
        public int OnlineCoursesCount => _courses.Count(item => item.Type == CourseType.Online);
        public int OfflineCoursesCount => _courses.Count(item => item.Type == CourseType.Offline);

        public void AddCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (_courses.Any(item => item.Id == course.Id))
                throw new InvalidOperationException($"Курс с таким ID {course.Id} уже существует. Попробуйте еще раз!");

            _courses.Add(course);
        }

        public Course GetCourse(int courseId)
        {
            var course = _courses.Find(item => item.Id == courseId);
            if (course == null)
            {
                throw new ArgumentException($"Курс с ID {courseId}, к сожалению, не найден");
            }
            return course;
        }

        public bool RemoveCourse(int courseId)
        {
            var course = _courses.Find(item => item.Id == courseId);
            return course != null && _courses.Remove(course);
        }

        public List<Course> GetAllCourses()
        {
            return _courses.ToList();
        }

        public void AssignTeacherToCourse(int courseId, Teacher teacher)
        {
            var course = GetCourse(courseId);

            if (course == null)
            {
                throw new ArgumentException($"Курс с ID {courseId}, к сожалению, не найден");
            }

            if (teacher == null)
                throw new ArgumentNullException(nameof(teacher));

            if (course.Teacher?.Id == teacher.Id)
            {
                throw new InvalidOperationException($"Преподаватель {teacher.FirstName} {teacher.LastName} уже назначен на курс '{course.Name}'");
            }

            if (!_teachers.Any(item => item.Id == teacher.Id))
            {
                _teachers.Add(teacher);
            }

            course.AssignTeacher(teacher);
        }

        public void AddStudentToCourse(int courseId, Student student)
        {
            var course = GetCourse(courseId);
            if (course != null)
            {
                if (student == null)
                    throw new ArgumentNullException(nameof(student));

                if (course.Students.Any(person => person.Id == student.Id))
                {
                    throw new InvalidOperationException($"Студент {student.FirstName} {student.LastName} уже записан на курс '{course.Name}'");
                }

                if (course is OfflineCourse offlineCourse)
                {
                    if (!offlineCourse.HasAvailableSeats())
                    {
                        throw new InvalidOperationException($"Курс '{course.Name}' уже полностью набран (вместимость {offlineCourse.HoldingCapacity} учеников)");
                    }
                }

                if (!_students.Any(person => person.Id == student.Id))
                {
                    _students.Add(student);
                }

                course.AddStudent(student);
                return;
            }
            throw new ArgumentException($"Курс с ID {courseId}, к сожалению, не найден");
        }

        public void RemoveStudentFromCourse(int courseId, Student student)
        {
            var course = GetCourse(courseId);
            if (course != null)
            {
                if (student == null)
                    throw new ArgumentNullException(nameof(student));

                course.RemoveStudent(student);
                return;
            }
            throw new ArgumentException($"Курс с таким ID {courseId}, к сожалению, не найден");
        }

        public List<Student> GetStudentsInCourse(int courseId)
        {
            var course = GetCourse(courseId);
            if (course == null)
            {
                return new List<Student>();
            }
            return course.Students;
        }

        public List<Course> GetOnlineCourses()
        {
            return _courses.FindAll(item => item.Type == CourseType.Online);
        }

        public List<Course> GetOfflineCourses()
        {
            return _courses.FindAll(item => item.Type == CourseType.Offline);
        }

        public List<Course> GetCoursesByTeacher(int teacherId)
        {
            return _courses.FindAll(item =>
                 item.Teacher != null && item.Teacher.Id == teacherId
             );
        }

        public void AddTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new ArgumentNullException(nameof(teacher));

            if (!_teachers.Any(item => item.Id == teacher.Id))
            {
                _teachers.Add(teacher);
            }
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (!_students.Any(s => s.Id == student.Id))
            {
                _students.Add(student);
            }
        }

        public List<Teacher> GetAllTeachers()
        {
            return new List<Teacher>(_teachers);
        }

        public List<Student> GetAllStudents()
        {
            return new List<Student>(_students);
        }
    }
}
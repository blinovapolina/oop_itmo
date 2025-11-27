using UniversitySystem.Models;
using UniversitySystem.Core;
using Xunit;

namespace UniversitySystem.Tests
{
    public class UniManagerTests
    {
        private UniManager _uniManager;

        public UniManagerTests()
        {
            _uniManager = new UniManager();
        }

        [Fact]
        public void AddCourse_ShouldAddCourseToSystem()
        {
            var course = new OnlineCourse(1, "Тестовый курс", "Описание", 30, "Платформа", "Ссылка");

            _uniManager.AddCourse(course);

            var addedCourse = _uniManager.GetCourse(1);
            Assert.NotNull(addedCourse);
            Assert.Equal("Тестовый курс", addedCourse.Name);
        }

        [Fact]
        public void AddCourse_WithDuplicateId_ShouldThrowException()
        {
            var course1 = new OnlineCourse(1, "Курс 1", "Описание", 30, "Платформа", "Ссылка");
            var course2 = new OnlineCourse(1, "Курс 2", "Описание", 40, "Платформа", "Ссылка");
            _uniManager.AddCourse(course1);

            Assert.Throws<InvalidOperationException>(() => _uniManager.AddCourse(course2));
        }

        [Fact]
        public void RemoveCourse_ExistingCourse_ShouldRemoveCourse()
        {
            var course = new OnlineCourse(1, "Тестовый курс", "Описание", 30, "Платформа", "Ссылка");
            _uniManager.AddCourse(course);

            var result = _uniManager.RemoveCourse(1);


            Assert.True(result);
            Assert.Throws<ArgumentException>(() => _uniManager.GetCourse(1));
        }

        [Fact]
        public void RemoveCourse_NonExistingCourse_ShouldReturnFalse()
        {
            var result = _uniManager.RemoveCourse(999);

            Assert.False(result);
        }

        [Fact]
        public void AddTeacher_ShouldAddTeacherToSystem()
        {
            var teacher = new Teacher(1, "Иван", "Иванченко", "ivan@university.com", "Компьютерные науки");

            _uniManager.AddTeacher(teacher);

            var teachers = _uniManager.GetAllTeachers();
            Assert.Single(teachers);
            Assert.Equal("Иван", teachers[0].FirstName);
        }

        [Fact]
        public void AddStudent_ShouldAddStudentToSystem()
        {
            var student = new Student(1, "Алиса", "Петрова", "alisa@university.com", "Группа1");

            _uniManager.AddStudent(student);

            var students = _uniManager.GetAllStudents();
            Assert.Single(students);
            Assert.Equal("Алиса", students[0].FirstName);
        }

        [Fact]
        public void AssignTeacherToCourse_ShouldAssignTeacher()
        {
            var course = new OnlineCourse(1, "Тестовый курс", "Описание", 30, "Платформа", "Ссылка");
            var teacher = new Teacher(1, "Иван", "Иванченко", "ivan@university.com", "Компьютерные науки");
            _uniManager.AddCourse(course);
            _uniManager.AddTeacher(teacher);

            _uniManager.AssignTeacherToCourse(1, teacher);

            var retrievedCourse = _uniManager.GetCourse(1);
            Assert.NotNull(retrievedCourse.Teacher);
            Assert.Equal("Иван", retrievedCourse.Teacher.FirstName);
        }

        [Fact]
        public void AssignTeacherToCourse_NonExistingCourse_ShouldThrowException()
        {
            var teacher = new Teacher(1, "Иван", "Иванченко", "ivan@university.com", "Компьютерные науки");
            _uniManager.AddTeacher(teacher);

            Assert.Throws<ArgumentException>(() => _uniManager.AssignTeacherToCourse(999, teacher));
        }

        [Fact]
        public void AddStudentToCourse_ShouldAddStudent()
        {
            var course = new OnlineCourse(1, "Тестовый курс", "Описание", 30, "Платформа", "Ссылка");
            var student = new Student(1, "Алиса", "Петрова", "alisa@university.com", "Группа1");
            _uniManager.AddCourse(course);
            _uniManager.AddStudent(student);

            _uniManager.AddStudentToCourse(1, student);

            var retrievedCourse = _uniManager.GetCourse(1);
            Assert.Single(retrievedCourse.Students);
            Assert.Equal("Алиса", retrievedCourse.Students[0].FirstName);
        }

        [Fact]
        public void AddStudentToCourse_StudentAlreadyEnrolled_ShouldThrowException()
        {
            var course = new OnlineCourse(1, "Тестовый курс", "Описание", 30, "Платформа", "Ссылка");
            var student = new Student(1, "Алиса", "Петрова", "alisa@university.com", "Группа1");
            _uniManager.AddCourse(course);
            _uniManager.AddStudent(student);
            _uniManager.AddStudentToCourse(1, student);

            Assert.Throws<InvalidOperationException>(() => _uniManager.AddStudentToCourse(1, student));
        }

        [Fact]
        public void AddStudentToCourse_OfflineCourseFull_ShouldThrowException()
        {
            var course = new OfflineCourse(1, "Офлайн курс", "Описание", 30, "Аудитория 101", "Здание А", "Пн 10:00", 1);
            var student1 = new Student(1, "Алиса", "Петрова", "alisa@university.com", "Группа1");
            var student2 = new Student(2, "Петр", "Сидоров", "petr@university.com", "Группа1");
            _uniManager.AddCourse(course);
            _uniManager.AddStudent(student1);
            _uniManager.AddStudent(student2);
            _uniManager.AddStudentToCourse(1, student1);

            Assert.Throws<InvalidOperationException>(() => _uniManager.AddStudentToCourse(1, student2));
        }

        [Fact]
        public void RemoveStudentFromCourse_ShouldRemoveStudent()
        {
            var course = new OnlineCourse(1, "Тестовый курс", "Описание", 30, "Платформа", "Ссылка");
            var student = new Student(1, "Алиса", "Петрова", "alisa@university.com", "Группа1");
            _uniManager.AddCourse(course);
            _uniManager.AddStudent(student);
            _uniManager.AddStudentToCourse(1, student);

            _uniManager.RemoveStudentFromCourse(1, student);

            var retrievedCourse = _uniManager.GetCourse(1);
            Assert.Empty(retrievedCourse.Students);
        }

        [Fact]
        public void GetOnlineCourses_ShouldReturnOnlyOnlineCourses()
        {
            var onlineCourse = new OnlineCourse(1, "Онлайн курс", "Описание", 30, "Платформа", "Ссылка");
            var offlineCourse = new OfflineCourse(2, "Офлайн курс", "Описание", 30, "Аудитория", "Местоположение", "Расписание", 30);
            _uniManager.AddCourse(onlineCourse);
            _uniManager.AddCourse(offlineCourse);

            var onlineCourses = _uniManager.GetOnlineCourses();

            Assert.Single(onlineCourses);
            Assert.All(onlineCourses, c => Assert.IsType<OnlineCourse>(c));
        }

        [Fact]
        public void GetOfflineCourses_ShouldReturnOnlyOfflineCourses()
        {
            var onlineCourse = new OnlineCourse(1, "Онлайн курс", "Описание", 30, "Платформа", "Ссылка");
            var offlineCourse = new OfflineCourse(2, "Офлайн курс", "Описание", 30, "Аудитория", "Местоположение", "Расписание", 30);
            _uniManager.AddCourse(onlineCourse);
            _uniManager.AddCourse(offlineCourse);

            var offlineCourses = _uniManager.GetOfflineCourses();

            Assert.Single(offlineCourses);
            Assert.All(offlineCourses, c => Assert.IsType<OfflineCourse>(c));
        }

        [Fact]
        public void GetCoursesByTeacher_ShouldReturnTeacherCourses()
        {
            var teacher = new Teacher(1, "Иван", "Иванченко", "ivan@university.com", "Компьютерные науки");
            var course1 = new OnlineCourse(1, "Курс 1", "Описание", 30, "Платформа", "Ссылка");
            var course2 = new OnlineCourse(2, "Курс 2", "Описание", 30, "Платформа", "Ссылка");
            _uniManager.AddTeacher(teacher);
            _uniManager.AddCourse(course1);
            _uniManager.AddCourse(course2);
            _uniManager.AssignTeacherToCourse(1, teacher);
            _uniManager.AssignTeacherToCourse(2, teacher);

            var teacherCourses = _uniManager.GetCoursesByTeacher(1);

            Assert.Equal(2, teacherCourses.Count);
        }

        [Fact]
        public void GetStudentsInCourse_ShouldReturnCourseStudents()
        {
            var course = new OnlineCourse(1, "Тестовый курс", "Описание", 30, "Платформа", "Ссылка");
            var student1 = new Student(1, "Алиса", "Петрова", "alisa@university.com", "Группа1");
            var student2 = new Student(2, "Петр", "Сидоров", "petr@university.com", "Группа1");
            _uniManager.AddCourse(course);
            _uniManager.AddStudent(student1);
            _uniManager.AddStudent(student2);
            _uniManager.AddStudentToCourse(1, student1);
            _uniManager.AddStudentToCourse(1, student2);

            var students = _uniManager.GetStudentsInCourse(1);

            Assert.Equal(2, students.Count);
        }
    }
}
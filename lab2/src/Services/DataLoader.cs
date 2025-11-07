using UniversitySystem.Models;
using UniversitySystem.Interface;
using System.Text.Json;

namespace UniversitySystem.Services
{
    public static class DataLoader
    {
        private static JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static void InitializeData(InterfaceUniManager manager)
        {
            try
            {
                LoadTeachers(manager);
                LoadStudents(manager);
                LoadCourses(manager);
                LoadAssignments(manager);
                LoadEnrollments(manager);

                Console.WriteLine("Данные успешно загружены!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                throw;
            }
        }

        private static void LoadTeachers(InterfaceUniManager manager)
        {
            var teachers = LoadFromJson<List<Teacher>>("Data/teachers.json");
            foreach (var teacher in teachers)
            {
                manager.AddTeacher(teacher);
            }
            Console.WriteLine($"Загружено преподавателей: {teachers.Count}");
        }

        private static void LoadStudents(InterfaceUniManager manager)
        {
            var students = LoadFromJson<List<Student>>("Data/students.json");
            foreach (var student in students)
            {
                manager.AddStudent(student);
            }
            Console.WriteLine($"Загружено студентов: {students.Count}");
        }

        private static void LoadCourses(InterfaceUniManager manager)
        {
            var onlineCourses = LoadFromJson<List<OnlineCourseData>>("Data/online_cources.json");
            var offlineCourses = LoadFromJson<List<OfflineCourseData>>("Data/offline_cources.json");

            int courseCount = 0;

            foreach (var courseData in onlineCourses)
            {
                var course = new OnlineCourse(
                    courseData.Id,
                    courseData.Name,
                    courseData.Description,
                    courseData.Credits,
                    courseData.Platform,
                    courseData.MeetingLink
                );
                manager.AddCourse(course);
                courseCount++;
            }

            foreach (var courseData in offlineCourses)
            {
                var course = new OfflineCourse(
                    courseData.Id,
                    courseData.Name,
                    courseData.Description,
                    courseData.Credits,
                    courseData.Classroom,
                    courseData.Location,
                    courseData.Schedule,
                    courseData.HoldingCapacity
                );
                manager.AddCourse(course);
                courseCount++;
            }

            Console.WriteLine($"Загружено курсов: {courseCount} ({onlineCourses.Count} онлайн, {offlineCourses.Count} офлайн)");
        }

        private static void LoadAssignments(InterfaceUniManager manager)
        {
            var assignments = LoadFromJson<List<Assignment>>("Data/assignments.json");
            int successCount = 0;

            foreach (var assignment in assignments)
            {
                var teacher = manager.GetAllTeachers().Find(t => t.Id == assignment.TeacherId);
                var course = manager.GetAllCourses().Find(c => c.Id == assignment.CourseId);
                
                if (teacher != null && course != null)
                {
                    try
                    {
                        manager.AssignTeacherToCourse(assignment.CourseId, teacher);
                        successCount++;
                        Console.WriteLine($"Преподаватель {teacher.FirstName} {teacher.LastName} назначен на курс {assignment.CourseId}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Не удалось назначить преподавателя {teacher.Id} на курс {assignment.CourseId}: {ex.Message}");
                    }
                }
                else
                {
                    if (teacher == null)
                        Console.WriteLine($"Преподаватель с ID {assignment.TeacherId} не найден для курса {assignment.CourseId}");
                    if (course == null)
                        Console.WriteLine($"Курс с ID {assignment.CourseId} не найден для назначения преподавателя {assignment.TeacherId}");
                }
            }

            Console.WriteLine($"Назначений преподавателей: {successCount}/{assignments.Count}");
        }

        private static void LoadEnrollments(InterfaceUniManager manager)
        {
            var enrollments = LoadFromJson<List<Enrollment>>("Data/enrollment.json");
            int totalEnrollments = 0;
            int successEnrollments = 0;

            foreach (var enrollment in enrollments)
            {
                var course = manager.GetAllCourses().Find(c => c.Id == enrollment.CourseId);
                if (course == null)
                {
                    Console.WriteLine($"Курс с ID {enrollment.CourseId} не найден для записи студентов");
                    continue;
                }


                totalEnrollments += enrollment.StudentIds.Count;

                foreach (var studentId in enrollment.StudentIds)
                {
                    var student = manager.GetAllStudents().Find(s => s.Id == studentId);
                    if (student != null)
                    {
                        try
                        {   
                            manager.AddStudentToCourse(enrollment.CourseId, student);
                            successEnrollments++;
                            var updatedCourse = manager.GetCourse(enrollment.CourseId);
                            Console.WriteLine($"✅ Студент записан. Теперь на курсе: {updatedCourse.Students.Count} студентов");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при записи студента {studentId} на курс {enrollment.CourseId}: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Студент с ID {studentId} не найден в системе");
                    }
                }
            }

            Console.WriteLine($"Итог: успешно записано {successEnrollments} из {totalEnrollments} студентов");
        }

        private static T LoadFromJson<T>(string filePath)
        {
            var possiblePaths = new[]
            {
                filePath,
                Path.Combine(Directory.GetCurrentDirectory(), filePath),
                Path.Combine("..", filePath),
                Path.Combine("src", filePath),
                Path.Combine(Directory.GetCurrentDirectory(), "src", filePath)
            };

            string? foundPath = null;
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    foundPath = path;
                    break;
                }
            }

            if (foundPath == null)
            {
                throw new FileNotFoundException($"Файл не найден: {filePath}");
            }

            try
            {
                var json = File.ReadAllText(foundPath);
                var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);

                if (result == null)
                {
                    throw new InvalidOperationException($"Не удалось загрузить данные из {foundPath}");
                }

                return result;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Ошибка формата JSON в файле {filePath}: {ex.Message}");
            }
        }

        public class OnlineCourseData
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int Credits { get; set; }
            public string Platform { get; set; } = string.Empty;
            public string MeetingLink { get; set; } = string.Empty;
        }

        public class OfflineCourseData
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int Credits { get; set; }
            public string Classroom { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string Schedule { get; set; } = string.Empty;
            public int HoldingCapacity { get; set; }
        }

        public class Assignment
        {
            public int CourseId { get; set; }
            public int TeacherId { get; set; }
        }

        public class Enrollment
        {
            public int CourseId { get; set; }
            public List<int> StudentIds { get; set; } = new List<int>();
        }
    }
}
using UniversitySystem.Models;


namespace UniversitySystem.Interface
{
    public interface InterfaceUniManager
    {
        Course GetCourse(int courseId);
        List<Course> GetAllCourses();
        void AddCourse(Course course);
        bool RemoveCourse(int courseId);

        void AssignTeacherToCourse(int courseId, Teacher teacher);
        void AddTeacher(Teacher teacher);
        List<Teacher> GetAllTeachers();

        void AddStudentToCourse(int courseId, Student student);
        void RemoveStudentFromCourse(int courseId, Student student);
        List<Student> GetStudentsInCourse(int courseId);
        void AddStudent(Student student);
        List<Student> GetAllStudents();

        List<Course> GetOnlineCourses();
        List<Course> GetOfflineCourses();
        List<Course> GetCoursesByTeacher(int teacherId);
    }
}
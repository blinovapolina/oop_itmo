using UniversitySystem.Models;
using UniversitySystem.Interface;

namespace UniversitySystem.Core
{
    public class MenuService
    {
        private InterfaceUniManager _uniManager;

        public MenuService(InterfaceUniManager uniManager)
        {
            _uniManager = uniManager;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                ShowMainMenu();
                var choice = Console.ReadLine();

                if (!ProcessMenuChoice(choice)) break;
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("1. Общая статистика");
            Console.WriteLine("2. Список всех курсов");
            Console.WriteLine("3. Преподаватели и их курсы");
            Console.WriteLine("4. Список всех студентов");
            Console.WriteLine("5. Студенты по курсам");
            Console.WriteLine("6. Управление данными");
            Console.WriteLine("0. Выход");
            Console.WriteLine("---------------------------------------");
            Console.Write("Выберите опцию: ");
        }

        private bool ProcessMenuChoice(string? choice)
        {
            if (string.IsNullOrEmpty(choice)) return true;

            if (choice == "1")
            {
                ShowStatistics();
            }
            else if (choice == "2")
            {
                ShowAllCourses();
            }
            else if (choice == "3")
            {
                ShowTeachersAndCourses();
            }
            else if (choice == "4")
            {
                ShowAllStudents();
            }
            else if (choice == "5")
            {
                ShowStudentsByCourse();
            }
            else if (choice == "6")
            {
                ShowDataManagementMenu();
            }
            else if (choice == "0")
            {
                Console.WriteLine("До свидания!");
                Console.Clear();
                return false;
            }
            else
            {
                Console.WriteLine("Неверный выбор!");
                WaitForContinue();
            }

            return true;
        }

        private void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void WaitForBackToMain()
        {
            Console.WriteLine("\nНажмите любую клавишу для возврата в главное меню...");
            Console.ReadKey();
        }

        private int GenerateCourseId()
        {
            var courses = _uniManager.GetAllCourses();
            if (courses.Count == 0)
                return 1;

            return courses.Max(c => c.Id) + 1;
        }

        private int GenerateTeacherId()
        {
            var teachers = _uniManager.GetAllTeachers();
            if (teachers.Count == 0)
                return 1;

            return teachers.Max(c => c.Id) + 1;
        }

        private int GenerateStudentId()
        {
            var students = _uniManager.GetAllStudents();
            if (students.Count == 0)
                return 1;

            return students.Max(c => c.Id) + 1;
        }

        private (string typeIcon, string capacityInfo) GetCourseDisplayInfo(Course course)
        {
            string typeIcon;
            if (course is OnlineCourse)
            {
                typeIcon = "[ОНЛАЙН]";
            }
            else
            {
                typeIcon = "[ОФЛАЙН]";
            }

            string capacityInfo;
            if (course is OfflineCourse offlineCourse)
            {
                capacityInfo = $"{offlineCourse.Students.Count}/{offlineCourse.HoldingCapacity}";
            }
            else
            {
                capacityInfo = $"{course.Students.Count}";
            }

            return (typeIcon, capacityInfo);
        }

        public void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ОБЩАЯ СТАТИСТИКА СИСТЕМЫ");
            Console.WriteLine("---------------------------------------");

            var manager = _uniManager as UniManager;
            if (manager != null)
            {
                Console.WriteLine($"Преподавателей: {manager.TotalTeachersCount}");
                Console.WriteLine($"Студентов: {manager.TotalStudentsCount}");
                Console.WriteLine($"Всего курсов: {manager.TotalCoursesCount}");
                Console.WriteLine($"Онлайн-курсов: {manager.OnlineCoursesCount}");
                Console.WriteLine($"Офлайн-курсов: {manager.OfflineCoursesCount}");
            }
            else
            {
                Console.WriteLine($"Преподавателей: {_uniManager.GetAllTeachers().Count}");
                Console.WriteLine($"Студентов: {_uniManager.GetAllStudents().Count}");
                Console.WriteLine($"Всего курсов: {_uniManager.GetAllCourses().Count}");
                Console.WriteLine($"Онлайн-курсов: {_uniManager.GetOnlineCourses().Count}");
                Console.WriteLine($"Офлайн-курсов: {_uniManager.GetOfflineCourses().Count}");
            }

            WaitForBackToMain();
        }

        public void ShowAllCourses()
        {
            Console.Clear();
            var allCourses = _uniManager.GetAllCourses();

            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ВСЕ КУРСЫ");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"Всего курсов: {allCourses.Count}\n");

            foreach (var course in allCourses.OrderBy(c => c.Id))
            {
                var (typeIcon, capacityInfo) = GetCourseDisplayInfo(course);

                Console.WriteLine($"{typeIcon} {course.Name} (ID: {course.Id})");
                Console.WriteLine($"   Описание: {course.Description}");
                Console.WriteLine($"   Преподаватель: {course.Teacher?.ToString() ?? "Не назначен"}");
                Console.WriteLine($"   Студентов: {capacityInfo}");
                Console.WriteLine($"   Часы: {course.Credits}");

                if (course is OnlineCourse online)
                {
                    Console.WriteLine($"   Платформа: {online.Platform}");
                    Console.WriteLine($"   Ссылка: {online.MeetingLink}");
                }
                else if (course is OfflineCourse offline)
                {
                    Console.WriteLine($"   Аудитория: {offline.Classroom}");
                    Console.WriteLine($"   Адрес: {offline.Location}");
                    Console.WriteLine($"   Расписание: {offline.Schedule}");
                    Console.WriteLine($"   Свободных мест: {offline.AvailableSeatsCount()}");
                }
                Console.WriteLine();
            }

            WaitForBackToMain();
        }

        public void ShowTeachersAndCourses()
        {
            Console.Clear();
            var teachers = _uniManager.GetAllTeachers();
            var courses = _uniManager.GetAllCourses();

            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ПРЕПОДАВАТЕЛИ И ИХ КУРСЫ");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"Всего преподавателей: {teachers.Count}\n");

            foreach (var teacher in teachers.OrderBy(t => t.Id))
            {
                var teacherCourses = courses.FindAll(c => c.Teacher != null && c.Teacher.Id == teacher.Id);

                Console.WriteLine($"{teacher.FirstName} {teacher.LastName} (ID: {teacher.Id})");
                Console.WriteLine($"   Email: {teacher.Email}");
                Console.WriteLine($"   Кафедра: {teacher.Department}");
                Console.WriteLine($"   Курсов: {teacherCourses.Count}");

                foreach (var course in teacherCourses)
                {
                    var (typeIcon, capacityInfo) = GetCourseDisplayInfo(course);

                    Console.WriteLine($"      {typeIcon} {course.Name} - {capacityInfo} студентов");
                }

                if (!teacherCourses.Any())
                {
                    Console.WriteLine("      Нет назначенных курсов");
                }
                Console.WriteLine();
            }

            WaitForBackToMain();
        }

        public void ShowAllStudents()
        {
            Console.Clear();
            var allStudents = _uniManager.GetAllStudents();

            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("СПИСОК ВСЕХ СТУДЕНТОВ");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"Всего студентов: {allStudents.Count}\n");

            if (allStudents.Any())
            {
                foreach (var student in allStudents.OrderBy(person => person.LastName).ThenBy(person => person.FirstName))
                {
                    var studentCourses = _uniManager.GetAllCourses().Count(c => c.Students.Any(s => s.Id == student.Id));

                    Console.WriteLine($"{student.FirstName} {student.LastName}");
                    Console.WriteLine($"   ID: {student.Id}");
                    Console.WriteLine($"   Email: {student.Email}");
                    Console.WriteLine($"   Группа: {student.Group}");
                    Console.WriteLine($"   Курсов: {studentCourses}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("В системе пока нет студентов");
            }

            WaitForBackToMain();
        }

        public void ShowStudentsByCourse()
        {
            Console.Clear();
            var allCourses = _uniManager.GetAllCourses();

            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("СТУДЕНТЫ ПО КУРСАМ");
            Console.WriteLine("---------------------------------------");

            foreach (var course in allCourses.OrderBy(c => c.Id))
            {
                var (typeIcon, capacityInfo) = GetCourseDisplayInfo(course);

                Console.WriteLine($"\n{typeIcon} {course.Name} (ID: {course.Id})");
                Console.WriteLine($"   Преподаватель: {course.Teacher?.ToString() ?? "Не назначен"}");
                Console.WriteLine($"   Студентов: {course.Students.Count}");

                if (course.Students.Any())
                {
                    foreach (var student in course.Students.OrderBy(s => s.LastName))
                    {
                        Console.WriteLine($"      {student.FirstName} {student.LastName}");
                        Console.WriteLine($"         Email: {student.Email}");
                        Console.WriteLine($"         Группа: {student.Group}");
                    }
                }
                else
                {
                    Console.WriteLine("      На курсе пока нет студентов");
                }
            }

            WaitForBackToMain();
        }

        private void ShowDataManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("УПРАВЛЕНИЕ ДАННЫМИ");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("1. Добавить курс");
                Console.WriteLine("2. Удалить курс");
                Console.WriteLine("3. Добавить преподавателя");
                Console.WriteLine("4. Добавить студента");
                Console.WriteLine("5. Назначить преподавателя на курс");
                Console.WriteLine("6. Записать студента на курс");
                Console.WriteLine("7. Убрать студента с курса");
                Console.WriteLine("8. Показать детальную информацию о курсе");
                Console.WriteLine("0. Назад в главное меню");
                Console.WriteLine("---------------------------------------");
                Console.Write("Выберите опцию: ");

                var choice = Console.ReadLine();

                if (choice == "0") break;

                ProcessDataManagementChoice(choice);
            }
        }

        private void ProcessDataManagementChoice(string? choice)
        {
            if (string.IsNullOrEmpty(choice)) return;

            if (choice == "1")
            {
                Console.Clear();
                AddCourse();
                WaitForContinue();
            }
            else if (choice == "2")
            {
                Console.Clear();
                RemoveCourse();
                WaitForContinue();
            }
            else if (choice == "3")
            {
                Console.Clear();
                AddTeacher();
                WaitForContinue();
            }
            else if (choice == "4")
            {
                Console.Clear();
                AddStudent();
                WaitForContinue();
            }
            else if (choice == "5")
            {
                Console.Clear();
                AssignTeacherToCourse();
                WaitForContinue();
            }
            else if (choice == "6")
            {
                Console.Clear();
                AddStudentToCourse();
                WaitForContinue();
            }
            else if (choice == "7")
            {
                Console.Clear();
                RemoveStudentFromCourse();
                WaitForContinue();
            }
            else if (choice == "8")
            {
                Console.Clear();
                ShowCourseDetails();
                WaitForContinue();
            }
            else
            {
                Console.WriteLine("Неверный выбор!");
                WaitForContinue();
            }
        }

        private void AddCourse()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ДОБАВЛЕНИЕ НОВОГО КУРСА");
            Console.WriteLine("---------------------------------------");

            Console.WriteLine("Выберите тип курса:");
            Console.WriteLine("1. Онлайн-курс");
            Console.WriteLine("2. Офлайн-курс");
            Console.Write("Ваш выбор: ");

            var typeChoice = Console.ReadLine();

            try
            {
                int id = GenerateCourseId();
                Console.WriteLine($"Автоматически сгенерирован ID курса: {id}");

                Console.Write("Название курса: ");
                var name = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Название курса не может быть пустым!");
                    return;
                }

                Console.Write("Описание: ");
                var description = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Количество часов: ");
                if (!int.TryParse(Console.ReadLine(), out int credits) || credits <= 0)
                {
                    Console.WriteLine("Некорректное количество часов!");
                    return;
                }

                Course course;

                if (typeChoice == "1")
                {
                    Console.Write("Платформа (Teams/Zoom/etc): ");
                    var platform = Console.ReadLine()?.Trim() ?? "";

                    Console.Write("Ссылка на встречу: ");
                    var meetingLink = Console.ReadLine()?.Trim() ?? "";

                    course = new OnlineCourse(id, name, description, credits, platform, meetingLink);
                }
                else if (typeChoice == "2")
                {
                    Console.Write("Аудитория: ");
                    var classroom = Console.ReadLine()?.Trim() ?? "";

                    Console.Write("Адрес: ");
                    var location = Console.ReadLine()?.Trim() ?? "";

                    Console.Write("Расписание: ");
                    var schedule = Console.ReadLine()?.Trim() ?? "";

                    Console.Write("Вместимость: ");
                    if (!int.TryParse(Console.ReadLine(), out int capacity) || capacity <= 0)
                    {
                        Console.WriteLine("Некорректная вместимость!");
                        return;
                    }

                    course = new OfflineCourse(id, name, description, credits, classroom, location, schedule, capacity);
                }
                else
                {
                    Console.WriteLine("Неверный выбор типа курса");
                    return;
                }

                _uniManager.AddCourse(course);
                Console.WriteLine("Курс успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void RemoveCourse()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("УДАЛЕНИЕ КУРСА");
            Console.WriteLine("---------------------------------------");

            var courses = _uniManager.GetAllCourses();
            Console.WriteLine("Доступные курсы:");

            foreach (var course in courses.OrderBy(c => c.Id))
            {
                var (typeIcon, capacityInfo) = GetCourseDisplayInfo(course);

                Console.WriteLine($"  {course.Id}. {typeIcon} {course.Name} - {course.Students.Count} студентов");
            }

            Console.Write("\nВыберите ID курса для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                var course = _uniManager.GetAllCourses().Find(c => c.Id == courseId);
                if (course != null)
                {
                    Console.WriteLine($"\nВы действительно хотите удалить курс '{course.Name}'? (y/n)");
                    Console.WriteLine($"На курсе записано студентов: {course.Students.Count}");
                    var confirmation = Console.ReadLine()?.ToLower();
                    if (confirmation == "y" || confirmation == "у")
                    {
                        try
                        {
                            var result = _uniManager.RemoveCourse(courseId);
                            if (result)
                                Console.WriteLine("Курс успешно удален!");
                            else
                                Console.WriteLine("Не удалось удалить курс");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено");
                    }
                }
                else
                {
                    Console.WriteLine("Курс с таким ID не найден");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID");
            }
        }

        private void AddTeacher()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ДОБАВЛЕНИЕ ПРЕПОДАВАТЕЛЯ");
            Console.WriteLine("---------------------------------------");

            try
            {
                int id = GenerateTeacherId();
                Console.WriteLine($"Автоматически сгенерирован ID преподавателя: {id}");

                Console.Write("Имя: ");
                var firstName = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(firstName))
                {
                    Console.WriteLine("Имя не может быть пустым!");
                    return;
                }

                Console.Write("Фамилия: ");
                var lastName = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(lastName))
                {
                    Console.WriteLine("Фамилия не может быть пустой!");
                    return;
                }

                Console.Write("Email: ");
                var email = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Кафедра: ");
                var department = Console.ReadLine()?.Trim() ?? "";

                var teacher = new Teacher(id, firstName, lastName, email, department);
                _uniManager.AddTeacher(teacher);

                Console.WriteLine("Преподаватель успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void AddStudent()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ДОБАВЛЕНИЕ СТУДЕНТА");
            Console.WriteLine("---------------------------------------");

            try
            {
                int id = GenerateStudentId();
                Console.WriteLine($"Автоматически сгенерирован ID студента: {id}");

                Console.Write("Имя: ");
                var firstName = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(firstName))
                {
                    Console.WriteLine("Имя не может быть пустым!");
                    return;
                }

                Console.Write("Фамилия: ");
                var lastName = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(lastName))
                {
                    Console.WriteLine("Фамилия не может быть пустой!");
                    return;
                }

                Console.Write("Email: ");
                var email = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Группа: ");
                var group = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(group))
                {
                    Console.WriteLine("Группа не может быть пустой!");
                    return;
                }

                var student = new Student(id, firstName, lastName, email, group);
                _uniManager.AddStudent(student);

                Console.WriteLine("Студент успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void AssignTeacherToCourse()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("\nНАЗНАЧЕНИЕ ПРЕПОДАВАТЕЛЯ НА КУРС");
            Console.WriteLine("---------------------------------------");

            var allCourses = _uniManager.GetAllCourses();
            Console.WriteLine("Доступные курсы:");
            foreach (var courseItem in allCourses.OrderBy(c => c.Id))
            {
                var (typeIcon, capacityInfo) = GetCourseDisplayInfo(courseItem);

                string teacherInfo = courseItem.Teacher != null
                    ? $" (Преподаватель: {courseItem.Teacher.FirstName} {courseItem.Teacher.LastName})"
                    : " (Преподаватель не назначен)";
                Console.WriteLine($"  {courseItem.Id}. {typeIcon} {courseItem.Name}{teacherInfo}");
            }

            Console.Write("\nВыберите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID курса");
                return;
            }

            var selectedCourse = _uniManager.GetAllCourses().Find(item => item.Id == courseId);
            if (selectedCourse == null)
            {
                Console.WriteLine("Курс не найден");
                return;
            }

            if (selectedCourse.Teacher != null)
            {
                Console.WriteLine($"\nНа курсе '{selectedCourse.Name}' уже назначен преподаватель: {selectedCourse.Teacher.FirstName} {selectedCourse.Teacher.LastName}");
                Console.Write("Хотите заменить преподавателя? (y/n): ");
                var replace = Console.ReadLine()?.ToLower();
                if (replace != "y" && replace != "у")
                {
                    Console.WriteLine("Назначение отменено");
                    return;
                }
            }

            var allTeachers = _uniManager.GetAllTeachers();
            Console.WriteLine("\nДоступные преподаватели:");
            foreach (var teacherItem in allTeachers.OrderBy(person => person.Id))
            {
                var teacherCourses = _uniManager.GetCoursesByTeacher(teacherItem.Id);
                Console.WriteLine($"  {teacherItem.Id}. {teacherItem.FirstName} {teacherItem.LastName} - {teacherItem.Department} (курсов: {teacherCourses.Count})");
            }

            Console.Write("\nВыберите ID преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int teacherId))
            {
                Console.WriteLine("Неверный формат ID преподавателя");
                return;
            }

            var selectedTeacher = _uniManager.GetAllTeachers().Find(t => t.Id == teacherId);
            if (selectedTeacher == null)
            {
                Console.WriteLine("Преподаватель не найден");
                return;
            }

            try
            {
                _uniManager.AssignTeacherToCourse(courseId, selectedTeacher);
                Console.WriteLine($"Преподаватель {selectedTeacher.FirstName} {selectedTeacher.LastName} назначен на курс '{selectedCourse.Name}'!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void AddStudentToCourse()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ЗАПИСЬ СТУДЕНТА НА КУРС");
            Console.WriteLine("---------------------------------------");

            var allCourses = _uniManager.GetAllCourses();
            Console.WriteLine("Доступные курсы:");
            foreach (var courseItem in allCourses.OrderBy(c => c.Id))
            {
                var (typeIcon, capacityInfo) = GetCourseDisplayInfo(courseItem);

                Console.WriteLine($"  {courseItem.Id}. {typeIcon} {courseItem.Name} {capacityInfo}");
            }

            Console.Write("\nВыберите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID курса");
                return;
            }

            int selectedCourseId = courseId;
            var selectedCourse = _uniManager.GetAllCourses().FirstOrDefault(c => c.Id == selectedCourseId);
            if (selectedCourse == null)
            {
                Console.WriteLine("Курс не найден");
                return;
            }

            var allStudents = _uniManager.GetAllStudents();
            var enrolledStudentIds = selectedCourse.Students.Select(s => s.Id).ToList();
            var availableStudents = allStudents.FindAll(s => !enrolledStudentIds.Any(id => id == s.Id));


            Console.WriteLine("\nСтуденты, еще не записанные на курс:");
            if (availableStudents.Any())
            {
                foreach (var studentItem in availableStudents.OrderBy(s => s.Id))
                {
                    var studentCoursesCount = _uniManager.GetAllCourses().Count(c => c.Students.Any(st => st.Id == studentItem.Id));
                    Console.WriteLine($"  {studentItem.Id}. {studentItem.FirstName} {studentItem.LastName} - {studentItem.Group} (курсов: {studentCoursesCount})");
                }
            }
            else
            {
                Console.WriteLine("  Все студенты уже записаны на этот курс");
                return;
            }

            Console.Write("\nВыберите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Неверный формат ID студента");
                return;
            }

            var selectedStudent = _uniManager.GetAllStudents().Find(s => s.Id == studentId);
            if (selectedStudent == null)
            {
                Console.WriteLine("Студент не найден");
                return;
            }


            if (enrolledStudentIds.Contains(studentId))
            {
                Console.WriteLine($"Студент {selectedStudent.FirstName} {selectedStudent.LastName} уже записан на курс '{selectedCourse.Name}'!");
                return;
            }

            if (selectedCourse is OfflineCourse offlineCourseObj && !offlineCourseObj.HasAvailableSeats())
            {
                Console.WriteLine($"В курсе '{selectedCourse.Name}' нет свободных мест!");
                return;
            }

            try
            {
                _uniManager.AddStudentToCourse(selectedCourseId, selectedStudent);
                Console.WriteLine($"Студент {selectedStudent.FirstName} {selectedStudent.LastName} записан на курс '{selectedCourse.Name}'!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void RemoveStudentFromCourse()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("УДАЛЕНИЕ СТУДЕНТА С КУРСА");
            Console.WriteLine("---------------------------------------");

            var courses = _uniManager.GetAllCourses();
            Console.WriteLine("Доступные курсы:");
            foreach (var course in courses.OrderBy(c => c.Id))
            {
                var (typeIcon, capacityInfo) = GetCourseDisplayInfo(course);

                Console.WriteLine($"  {course.Id}. {typeIcon} {course.Name} - {course.Students.Count} студентов");
            }

            Console.Write("\nВыберите ID курса: ");
            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                var course = _uniManager.GetAllCourses().Find(c => c.Id == courseId);
                if (course == null)
                {
                    Console.WriteLine("Курс не найден");
                    return;
                }

                Console.WriteLine($"\nСтуденты на курсе '{course.Name}':");
                if (course.Students.Any())
                {
                    foreach (var student in course.Students.OrderBy(s => s.Id))
                    {
                        Console.WriteLine($"  {student.Id}. {student.FirstName} {student.LastName} - {student.Group}");
                    }
                }
                else
                {
                    Console.WriteLine("На курсе нет студентов");
                    return;
                }

                Console.Write("\nВыберите ID студента для удаления: ");
                if (int.TryParse(Console.ReadLine(), out int studentId))
                {
                    var student = _uniManager.GetAllStudents().Find(s => s.Id == studentId);
                    if (student == null)
                    {
                        Console.WriteLine("Студент не найден");
                        return;
                    }

                    if (!course.Students.Any(s => s.Id == studentId))
                    {
                        Console.WriteLine($"Студент {student.FirstName} {student.LastName} не записан на курс '{course.Name}'!");
                        return;
                    }

                    Console.WriteLine($"\nВы действительно хотите удалить студента {student.FirstName} {student.LastName} с курса '{course.Name}'? (y/n)");
                    var confirmation = Console.ReadLine()?.ToLower();
                    if (confirmation == "y" || confirmation == "у")
                    {
                        try
                        {
                            _uniManager.RemoveStudentFromCourse(courseId, student);
                            Console.WriteLine($"Студент {student.FirstName} {student.LastName} успешно удален с курса '{course.Name}'!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный формат ID студента");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID курса");
            }
        }

        private void ShowCourseDetails()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ УНИВЕРСИТЕТА");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ДЕТАЛЬНАЯ ИНФОРМАЦИЯ О КУРСЕ");
            Console.WriteLine("---------------------------------------");

            var courses = _uniManager.GetAllCourses();
            Console.WriteLine("Доступные курсы:");
            foreach (var course in courses.OrderBy(c => c.Id))
            {
                var (typeIcon, capacityInfo) = GetCourseDisplayInfo(course);
                Console.WriteLine($"  {course.Id}. {typeIcon} {course.Name}");
            }

            Console.Write("\nВыберите ID курса: ");
            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                var course = _uniManager.GetAllCourses().Find(c => c.Id == courseId);
                if (course != null)
                {
                    Console.WriteLine("\n" + course.GetCourseDetails());
                }
                else
                {
                    Console.WriteLine("Курс с таким ID не найден");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID");
            }
        }
    }
}
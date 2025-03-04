using System.Windows;
using System.Windows.Input;
using Mindly.Administrator;
using Mindly.Director;
using Mindly.Models;
using Mindly.Student;
using Supabase.Gotrue;
using Supabase.Interfaces;

namespace Mindly
{
    public class SupabaseClient
    {
        private static Supabase.Client _client;
        private static bool _isInitialized;

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            const string supabaseUrl = "https://ixxnpvymvlrabjimteru.supabase.co";
            const string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Iml4eG5wdnltdmxyYWJqaW10ZXJ1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDA4MTgwOTYsImV4cCI6MjA1NjM5NDA5Nn0.LDAHtfypNRihLec9NAY-VgksPrL3-JWqLUTAUSd4sDo";

            if (string.IsNullOrWhiteSpace(supabaseUrl) || string.IsNullOrWhiteSpace(supabaseKey))
            {
                throw new InvalidOperationException("Supabase URL или API ключ не заданы.");
            }

            _client = new Supabase.Client(supabaseUrl, supabaseKey);
            await _client.InitializeAsync();

            _isInitialized = true;
        }

        public Supabase.Client GetClient()
        {
            if (!_isInitialized || _client == null)
            {
                throw new InvalidOperationException("Supabase клиент не инициализирован.");
            }

            return _client;
        }

        public async Task RegisterStudentWithCourseAsync(string username, string password, string firstName, string lastName, int roleId, int courseId, int teacher_id)
        {
            var client = App.SupabaseService.GetClient();

            try
            {
                // Шаг 1: Регистрация пользователя в таблице Users
                var user = new Users
                {
                    username = username,
                    password = password, // Пароль должен быть хэширован на стороне сервера
                    first_name = firstName,
                    last_name = lastName,
                    role_id = roleId
                };

                var insertResponse = await client
                    .From<Users>()
                    .Insert(user);

                if (insertResponse.ResponseMessage.IsSuccessStatusCode)
                {
                    var newUser = insertResponse.Models.FirstOrDefault();
                    if (newUser == null)
                    {
                        throw new Exception("Не удалось получить ID нового пользователя.");
                    }

                    // Шаг 2: Назначение курса студенту в таблице Assignments
                    var assignment = new Assignments
                    {
                        student_id = newUser.id,
                        course_id = courseId,
                        teacher_id = teacher_id
                    };

                    var assignmentResponse = await client
                        .From<Assignments>()
                        .Insert(assignment);

                    if (!assignmentResponse.ResponseMessage.IsSuccessStatusCode)
                    {
                        throw new Exception("Не удалось назначить курс студенту.");
                    }

                    MessageBox.Show("Студент успешно зарегистрирован и курс назначен.");
                }
                else
                {
                    throw new Exception("Не удалось зарегистрировать пользователя.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                throw; // Повторно выбрасываем исключение для обработки на уровне выше
            }
        }

        public async Task<List<Courses>> GetCoursesAsync()
        {
            var client = App.SupabaseService.GetClient();

            // Получаем все курсы из таблицы Courses
            var response = await client
                .From<Courses>()
                .Select("id, name")
            .Get();

            return response.Models?.ToList() ?? new List<Courses>();
        }

        public async Task LoginAsync(string username, string password)
        {
            var client = App.SupabaseService.GetClient();

            try
            {
                // Получаем информацию о пользователе из таблицы Users
                var userResponse = await client
                    .From<Users>()
                    .Select("id, role_id")
                    .Filter("username", Supabase.Postgrest.Constants.Operator.Equals, username)
                    .Single();

                if (userResponse == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Получаем роль пользователя
                var roleId = userResponse.role_id;

                CurrentUser.CurrentUserId = userResponse.id;

                // Открываем соответствующее окно в зависимости от роли
                OpenRoleBasedWindow(roleId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenRoleBasedWindow(int roleId)
        {
            Window window = null;

            switch (roleId)
            {
                case 1: // Студент
                    window = new StudentMainWindow();
                    break;

                case 2: // Учитель
                    //window = new TeacherMainWindow();
                    break;

                case 3: // Руководитель
                    window = new DirectorMainWindow();
                    break;

                case 4: // Администратор
                    window = new AdminMainWindow();
                    break;

                default:
                    MessageBox.Show("Роль пользователя не определена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            // Закрываем текущее окно (окно авторизации)
            Application.Current.MainWindow?.Close();

            // Открываем новое окно
            window.Show();
            Application.Current.MainWindow = window;
        }

        public async Task<List<Lessons>> GetStudentLessonsAsync(int studentId)
        {
            var client = App.SupabaseService.GetClient();

            try
            {
                // Шаг 1: Получаем список курсов студента
                var assignmentsResponse = await client
                    .From<Assignments>()
                    .Select("course_id")
                    .Filter("student_id", Supabase.Postgrest.Constants.Operator.Equals, studentId)
                    .Get();

                if (assignmentsResponse.Models == null || !assignmentsResponse.Models.Any())
                {
                    return new List<Lessons>(); // Если курсов нет, возвращаем пустой список
                }

                // Извлекаем ID курсов
                var courseIds = assignmentsResponse.Models
                    .Select(a => a.course_id)
                    .ToList();

                // Шаг 2: Получаем занятия для этих курсов
                var lessonsResponse = await client
                    .From<Lessons>()
                    .Select("id, title, description, date, course_id")
                    .Filter("course_id", Supabase.Postgrest.Constants.Operator.In, courseIds)
                    .Get();

                return lessonsResponse.Models?.ToList() ?? new List<Lessons>();
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine($"Ошибка при загрузке занятий: {ex.Message}");
                throw; // Повторно выбрасываем исключение для обработки на уровне выше
            }
        }
    }
}
using Mindly.Models;
using Supabase;
using Supabase.Gotrue;
using Supabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mindly.Director
{
    /// <summary>
    /// Логика взаимодействия для DirectorEditClasses.xaml
    /// </summary>
    public partial class DirectorEditClasses : Window
    {
        private ObservableCollection<Lessons> lessons;
        private Dictionary<string, int> _teachersCache = new Dictionary<string, int>();

        public DirectorEditClasses()
        {
            InitializeComponent();
            Loaded += DirectorEditClasses_Loaded;
        }

        private async Task LoadTeachersAsync()
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var response = await client
                    .From<Users>()
                    .Select("id, first_name, last_name")
                    .Filter("role_id", Supabase.Postgrest.Constants.Operator.Equals, 2)
                    .Get();

                _teachersCache = response.Models?
                    .ToDictionary(
                        t => $"{t.first_name} {t.last_name}",
                        t => t.id
                    ) ?? new Dictionary<string, int>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки преподавателей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обновите метод загрузки данных окна:
        private async void DirectorEditClasses_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeachersAsync(); // Загружаем преподавателей
            await LoadLessonsAsync();  // Загружаем занятия
        }

        private async Task LoadLessonsAsync()
        {
            try
            {
                var client = App.SupabaseService.GetClient();

                // Загрузка занятий
                var query = await client
                    .From<Lessons>()
                    .Select("id, title, teacher_id, date")
                    .Get();
                var teachersResponse = await client
                    .From<Users>()
                    .Select("id, first_name, last_name")
                    .Filter("role_id", Supabase.Postgrest.Constants.Operator.Equals, 2) // role_id=2 для учителей
                    .Get();

                var teachers = teachersResponse.Models?.ToDictionary(t => t.id, t => $"{t.first_name} {t.last_name}");

                // Формирование данных для DataGrid
                var lessons = query.Models?.Select(l => new LessonsViewModel
                {
                    Id = l.id,
                    Title = l.title,
                    Instructor = teachers?.GetValueOrDefault(l.teacher_id, "Неизвестно"),
                    Date = l.date
                }).ToList();

                classesGrid.ItemsSource = lessons;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetTeacherIdByName(string fullName)
        {
            // Проверяем, есть ли преподаватель в кэше
            if (_teachersCache.TryGetValue(fullName, out int teacherId))
            {
                return teacherId;
            }

            // Если не найден, выбрасываем исключение
            throw new KeyNotFoundException($"Преподаватель '{fullName}' не найден.");
        }

        private async void btnUpdateList_Click(object sender, RoutedEventArgs e)
        {
            LoadLessonsAsync();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            classesGrid.IsReadOnly = false;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var updatedLessons = classesGrid.ItemsSource as IEnumerable<dynamic>;

                foreach (var lesson in updatedLessons)
                {
                    // Получаем ID преподавателя по имени
                    int teacherId = GetTeacherIdByName(lesson.Instructor);

                    // Обновляем занятие
                    var updatedLesson = new LessonsViewModel
                    {
                        Id = lesson.Id,
                        Title = lesson.Title,
                        Date = lesson.Date,
                        teacher_id = teacherId
                    };

                    await client
                        .From<Lessons>()
                        .Where(l => l.id == updatedLesson.Id)
                        .Set(l => l.title, updatedLesson.Title)
                        .Set(l => l.date, updatedLesson.Date)
                        .Set(l => l.teacher_id, updatedLesson.teacher_id)
                        .Update();
                }

                MessageBox.Show("Изменения сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DirectorMainWindow dmw = new DirectorMainWindow();
            dmw.Show();
            this.Close();
        }
    }

    public class LessonsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Instructor { get; set; }
        public int teacher_id { get; set; }
    }
}
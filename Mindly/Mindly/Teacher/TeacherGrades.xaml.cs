using Mindly.Models;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
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

namespace Mindly.Teacher
{
    /// <summary>
    /// Логика взаимодействия для TeacherGrades.xaml
    /// </summary>
    public partial class TeacherGrades : Window
    {
        public TeacherGrades()
        {
            InitializeComponent();
            Loaded += TeacherMainWindow_Loaded;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherMainMenu tmm = new TeacherMainMenu();
            tmm.Show();
            this.Close();
        }

        private async void AssignGrade_Click(object sender, RoutedEventArgs e)
        {
            int studentId = (int)cbxStudent.SelectedValue; // ID студента
            int courseId = (int)cbxCourse.SelectedValue;  // ID курса
            int gradeValue = cbxGrade.SelectedIndex + 2; // Оценка

            // Выставляем оценку
            bool isSuccess = await App.SupabaseService.AddGradeAsync(studentId, courseId, gradeValue);

            if (isSuccess)
            {
                MessageBox.Show("Оценка успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не удалось добавить оценку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void TeacherMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadStudentsAsync();
            await LoadCoursesAsync();
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                // Получаем клиент Supabase
                var client = App.SupabaseService.GetClient();

                // Получаем всех пользователей с ролью "студент" (например, role_id = 1)
                var response = await client
                    .From<Users>()
                    .Select("id, username, first_name, last_name")
                    .Filter("role_id", Supabase.Postgrest.Constants.Operator.Equals, 1) // role_id = 1 для студентов
                    .Get();

                // Преобразуем данные в список для ComboBox
                var students = response.Models?.Select(u => new
                {
                    Id = u.id, // Используем Id как значение
                    FullName = $"{u.first_name} {u.last_name}" // Отображаем полное имя
                }).ToList();

                // Привязываем данные к ComboBox
                cbxStudent.ItemsSource = students;
                cbxStudent.DisplayMemberPath = "FullName"; // Отображаем полное имя
                cbxStudent.SelectedValuePath = "Id"; // Используем Id как значение
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке студентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                // Получаем клиент Supabase
                var client = App.SupabaseService.GetClient();

                // Получаем все курсы
                var response = await client
                    .From<Courses>()
                    .Select("id, name")
                    .Get();

                // Преобразуем данные в список для ComboBox
                var courses = response.Models?.Select(c => new
                {
                    Id = c.id, // Используем Id как значение
                    Name = c.name // Отображаем название курса
                }).ToList();

                // Привязываем данные к ComboBox
                cbxCourse.ItemsSource = courses;
                cbxCourse.DisplayMemberPath = "Name"; // Отображаем название курса
                cbxCourse.SelectedValuePath = "Id"; // Используем Id как значение
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке курсов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
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
            int studentId = (int)cbxStudent.SelectedValue;
            int courseId = (int)cbxCourse.SelectedValue;
            int gradeValue = cbxGrade.SelectedIndex + 2;

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
                var client = App.SupabaseService.GetClient();

                var response = await client
                    .From<Users>()
                    .Select("id, username, first_name, last_name")
                    .Filter("role_id", Supabase.Postgrest.Constants.Operator.Equals, 1)
                    .Get();

                var students = response.Models?.Select(u => new
                {
                    Id = u.id,
                    FullName = $"{u.first_name} {u.last_name}"
                }).ToList();

                cbxStudent.ItemsSource = students;
                cbxStudent.DisplayMemberPath = "FullName";
                cbxStudent.SelectedValuePath = "Id";
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
                var client = App.SupabaseService.GetClient();

                // Получаем все курсы
                var response = await client
                    .From<Courses>()
                    .Select("id, name")
                    .Get();

                var courses = response.Models?.Select(c => new
                {
                    Id = c.id,
                    Name = c.name
                }).ToList();

                cbxCourse.ItemsSource = courses;
                cbxCourse.DisplayMemberPath = "Name";
                cbxCourse.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке курсов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
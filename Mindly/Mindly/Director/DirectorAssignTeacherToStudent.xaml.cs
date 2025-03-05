using Mindly.Models;
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

namespace Mindly.Director
{
    /// <summary>
    /// Логика взаимодействия для DirectorAssignTeacherToStudent.xaml
    /// </summary>
    public partial class DirectorAssignTeacherToStudent : Window
    {
        public DirectorAssignTeacherToStudent()
        {
            InitializeComponent();
            Loaded += DirectorAssignTeacherToStudent_Loaded;
        }

        private async void DirectorAssignTeacherToStudent_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeachersAsync();
            await LoadStudentsAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DirectorMainWindow dmw = new DirectorMainWindow();
            dmw.Show();
            this.Close();
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                // Получаем клиент Supabase
                var client = App.SupabaseService.GetClient();

                var response = await client
                    .From<Users>()
                    .Select("id, first_name, last_name")
                    .Filter("role_id", Supabase.Postgrest.Constants.Operator.Equals, 1)
                    .Get();

                // Преобразуем данные в список для ComboBox
                var teachers = response.Models?.Select(u => new
                {
                    id = u.id, // Используем Id как значение
                    FullName = $"{u.first_name} {u.last_name}" // Отображаем полное имя
                }).ToList();

                // Привязываем данные к ComboBox
                cbStudent.ItemsSource = teachers;
                cbStudent.DisplayMemberPath = "FullName"; // Отображаем полное имя
                cbStudent.SelectedValuePath = "id"; // Используем Id как значение
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке учителей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadTeachersAsync()
        {
            try
            {
                // Получаем клиент Supabase
                var client = App.SupabaseService.GetClient();

                // Получаем всех пользователей с ролью "учитель" (например, role_id = 2)
                var response = await client
                    .From<Users>()
                    .Select("id, first_name, last_name")
                    .Filter("role_id", Supabase.Postgrest.Constants.Operator.Equals, 2) // role_id = 2 для учителей
                    .Get();

                // Преобразуем данные в список для ComboBox
                var teachers = response.Models?.Select(u => new
                {
                    id = u.id, // Используем Id как значение
                    FullName = $"{u.first_name} {u.last_name}" // Отображаем полное имя
                }).ToList();

                // Привязываем данные к ComboBox
                cbTeacher.ItemsSource = teachers;
                cbTeacher.DisplayMemberPath = "FullName"; // Отображаем полное имя
                cbTeacher.SelectedValuePath = "id"; // Используем Id как значение
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке учителей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выбранные значения из ComboBox
                var selectedTeacherId = cbTeacher.SelectedValue?.ToString();
                var selectedStudentId = cbStudent.SelectedValue?.ToString();

                if (string.IsNullOrEmpty(selectedTeacherId) || string.IsNullOrEmpty(selectedStudentId))
                {
                    MessageBox.Show("Выберите учителя и студента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var user = new Users
                {
                    id = int.Parse(selectedStudentId),
                    manager_id = int.Parse(selectedTeacherId) // role_id = 3 для руководителя
                };

                // Получаем клиент Supabase
                var client = App.SupabaseService.GetClient();

                // Обновляем manager_id у студента
                var response = await client
                    .From<Users>()
                    .Where(u => u.id == user.id)
                    .Set(u => u.manager_id, user.manager_id)
                    .Update();

                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    MessageBox.Show("Учитель успешно закреплен за студентом.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось закрепить учителя за студентом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
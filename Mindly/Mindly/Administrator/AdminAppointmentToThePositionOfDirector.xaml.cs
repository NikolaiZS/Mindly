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
using Mindly.Models;

namespace Mindly.Administrator
{
    /// <summary>
    /// Логика взаимодействия для AdminAppointmentToThePositionOfDirector.xaml
    /// </summary>
    public partial class AdminAppointmentToThePositionOfDirector : Window
    {
        public AdminAppointmentToThePositionOfDirector()
        {
            InitializeComponent();
            Loaded += AdminAppointmentToThePositionOfDirector_Loaded;
        }

        private async void AdminAppointmentToThePositionOfDirector_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeachersAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainWindow amw = new AdminMainWindow();
            amw.Show();
            this.Close();
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
                    Id = u.id, // Используем Id как значение
                    FullName = $"{u.first_name} {u.last_name}" // Отображаем полное имя
                }).ToList();

                // Привязываем данные к ComboBox
                cbUsers.ItemsSource = teachers;
                cbUsers.DisplayMemberPath = "FullName"; // Отображаем полное имя
                cbUsers.SelectedValuePath = "Id"; // Используем Id как значение
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке учителей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnAppointDirector_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedUserId = cbUsers.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(selectedUserId))
                {
                    MessageBox.Show("Выберите пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var client = App.SupabaseService.GetClient();

                var user = new Users
                {
                    id = int.Parse(selectedUserId),
                    role_id = 3
                };

                var response = await client
                    .From<Users>()
                    .Where(u => u.id == user.id)
                    .Set(u => u.role_id, user.role_id)
                    .Update();

                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    MessageBox.Show("Пользователь успешно назначен руководителем.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось назначить пользователя руководителем.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при назначении руководителя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
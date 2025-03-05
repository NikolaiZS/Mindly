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
    /// Логика взаимодействия для AdminResetPass.xaml
    /// </summary>
    public partial class AdminPasswordReset : Window
    {
        public AdminPasswordReset()
        {
            InitializeComponent();
            Loaded += AdminPasswordReset_Loaded;
        }

        private async void AdminPasswordReset_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadUsersAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainWindow amw = new AdminMainWindow();
            amw.Show();
            this.Close();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                // Получаем клиент Supabase
                var client = App.SupabaseService.GetClient();

                // Получаем всех пользователей
                var response = await client
                    .From<Users>()
                    .Select("id, first_name, last_name")
                    .Get();

                // Преобразуем данные в список для ComboBox
                var users = response.Models?.Select(u => new
                {
                    Id = u.id, // Используем Id как значение
                    FullName = $"{u.first_name} {u.last_name}" // Отображаем полное имя
                }).ToList();

                // Привязываем данные к ComboBox
                cbUsers.ItemsSource = users;
                cbUsers.DisplayMemberPath = "FullName"; // Отображаем полное имя
                cbUsers.SelectedValuePath = "Id"; // Используем Id как значение
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ResetPasswordAsync(int userId, string newPassword)
        {
            try
            {
                // Получаем клиент Supabase
                var client = App.SupabaseService.GetClient();

                // Обновляем пароль пользователя
                var response = await client
                    .From<Users>()
                    .Where(u => u.id == userId)
                    .Set(u => u.password, newPassword) // Предположим, что есть поле Password
                    .Update();

                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    MessageBox.Show("Пароль успешно сброшен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось сбросить пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сбросе пароля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выбранного пользователя
                var selectedUserId = cbUsers.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(selectedUserId))
                {
                    MessageBox.Show("Выберите пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Получаем новый пароль
                var newPassword = txtNewPassword.Password;
                if (string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("Введите новый пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Сбрасываем пароль
                await ResetPasswordAsync(int.Parse(selectedUserId), newPassword);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сбросе пароля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
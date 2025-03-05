using Supabase.Gotrue;
using System.Windows;
using Mindly.Models;

namespace Mindly.Administrator
{
    /// <summary>
    /// Логика взаимодействия для UserManagementWindow.xaml
    /// </summary>
    public partial class UserManagementWindow : Window
    {
        public UserManagementWindow()
        {
            InitializeComponent();
            Loaded += UserManagementWindow_Loaded;
        }

        private async void UserManagementWindow_Loaded(object sender, RoutedEventArgs e)
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

                var response = await client
                    .From<Users>()
                    .Select("id, username, password, first_name, last_name, role_id, manager_id, created_at, updated_at")
                    .Get();

                var users = response.Models?.Select(u => new UserViewModel
                {
                    Id = u.id,
                    FirstName = u.first_name,
                    LastName = u.last_name,
                    Username = u.username,
                    Role = GetRoleName(u.role_id), // Получаем название роли
                    Password = u.password,
                    ManagerId = u.manager_id,
                    CreatedAt = u.created_at,
                    UpdatedAt = u.updated_at
                }).ToList();

                usersGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case 1: return "Студент";
                case 2: return "Учитель";
                case 3: return "Руководитель";
                case 4: return "Администратор";
                default: return "Неизвестно";
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = App.SupabaseService.GetClient();

                var updatedUsers = usersGrid.ItemsSource as IEnumerable<UserViewModel>;
                if (updatedUsers == null) return;

                foreach (var user in updatedUsers)
                {
                    var updatedUser = new Users
                    {
                        id = user.Id,
                        first_name = user.FirstName,
                        last_name = user.LastName,
                        username = user.Username,
                        role_id = GetRoleId(user.Role)
                    };

                    var response = await client
                        .From<Users>()
                        .Where(u => u.id == updatedUser.id)
                        .Set(u => u.first_name, updatedUser.first_name)
                        .Set(u => u.last_name, updatedUser.last_name)
                        .Set(u => u.username, updatedUser.username)
                        .Set(u => u.role_id, updatedUser.role_id)
                        .Update();

                    if (!response.ResponseMessage.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Не удалось обновить пользователя {updatedUser.username}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetRoleId(string roleName)
        {
            switch (roleName)
            {
                case "Студент": return 1;
                case "Учитель": return 2;
                case "Руководитель": return 3;
                case "Администратор": return 4;
                default: return 0;
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = usersGrid.SelectedItem as UserViewModel;
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                var client = App.SupabaseService.GetClient();

                await client
                    .From<Users>()
                    .Where(u => u.id == selectedUser.Id)
                    .Delete();

                MessageBox.Show("Пользователь успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnUpdateList_Click(object sender, RoutedEventArgs e)
        {
            await LoadUsersAsync();
        }
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int ManagerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
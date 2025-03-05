using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Mindly.Administrator
{
    /// <summary>
    /// Логика взаимодействия для AdminNewUserReg.xaml
    /// </summary>
    public partial class AdminNewUserReg : Window
    {
        public AdminNewUserReg()
        {
            InitializeComponent();
            Loaded += AdminNewUserReg_Loaded;
        }

        private async void AdminNewUserReg_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCoursesAsync();
            await LoadRolesAsync();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainWindow amw = new AdminMainWindow();
            amw.Show();
            this.Close();
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                var roles = await App.SupabaseService.GetRolesAsync();

                cbxSelectRole.Items.Clear();

                foreach (var role in roles)
                {
                    cbxSelectRole.Items.Add(new ComboBoxItem
                    {
                        Content = role.name,
                        Tag = role.id
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                var courses = await App.SupabaseService.GetCoursesAsync();

                cbxSelectCourse.Items.Clear();

                foreach (var course in courses)
                {
                    cbxSelectCourse.Items.Add(new ComboBoxItem
                    {
                        Content = course.name, // Отображаемое имя
                        Tag = course.id       // ID курса (хранится в Tag)
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке курсов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var client = App.SupabaseService.GetClient();
            string firstname = txtFirstName.Text;
            string lastname = txtLastName.Text;
            string username = txtLogin.Text;
            string password = HashPassword(txtPassword.Password);
            int courseid = cbxSelectCourse.SelectedIndex;
            int roleid = cbxSelectRole.SelectedIndex + 1;
            await client.InitializeAsync();
            try
            {
                await App.SupabaseService.RegisterStudentWithCourseAsync(username, password, firstname, lastname, roleid, courseid + 1, 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }
    }
}
using System.Text;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace Mindly
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
            Loaded += RegistrationLoaded;
        }

        private async void RegistrationLoaded(object sender, RoutedEventArgs e)
        {
            await LoadCoursesAsync();
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
                        Content = course.name,
                        Tag = course.id
                    });
                }

                if (cbxSelectCourse.Items.Count > 0)
                {
                    cbxSelectCourse.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке курсов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var client = App.SupabaseService.GetClient();
            string firstname = txtFirstName.Text;
            string lastname = txtLastName.Text;
            string username = txtLogin.Text;
            string password = HashPassword(txtPassword.Password);
            int courseid = cbxSelectCourse.SelectedIndex;
            await client.InitializeAsync();
            try
            {
                await App.SupabaseService.RegisterStudentWithCourseAsync(username, password, firstname, lastname, 1, courseid + 1, 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }

        private void backToAuth_Click(object sender, RoutedEventArgs e)
        {
            Authorization auth = new Authorization();
            auth.Show();
            this.Close();
        }
    }
}
using System.Windows;

namespace Mindly.Administrator
{
    /// <summary>
    /// Логика взаимодействия для AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.CurrentUserId = -1;
            Authorization auth = new Authorization();
            auth.Show();
            this.Close();
        }

        private void EditUsersButton_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow umw = new UserManagementWindow();
            umw.Show();
            this.Close();
        }

        private void AssignManagersButton_Click(object sender, RoutedEventArgs e)
        {
            AdminAppointmentToThePositionOfDirector aattpod = new AdminAppointmentToThePositionOfDirector();
            aattpod.Show();
            this.Close();
        }

        private void RegisterNewUsersButton_Click(object sender, RoutedEventArgs e)
        {
            AdminNewUserReg anur = new AdminNewUserReg();
            anur.Show();
            this.Close();
        }

        private void ResetPasswordsButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPasswordReset apr = new AdminPasswordReset();
            apr.Show();
            this.Close();
        }
    }
}
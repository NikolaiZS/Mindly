using System.Windows;

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
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainWindow amw = new AdminMainWindow();
            amw.Show();
            this.Close();
        }
    }
}
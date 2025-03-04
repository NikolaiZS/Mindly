using Mindly.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mindly.Student
{
    /// <summary>
    /// Логика взаимодействия для StudentTheoryLessons.xaml
    /// </summary>
    public partial class StudentTheoryLessons : Window
    {
        private readonly Dictionary<DataGridCell, string> _cellContentMap = new Dictionary<DataGridCell, string>();

        private readonly int _studentId = CurrentUser.CurrentUserId;

        public StudentTheoryLessons()
        {
            InitializeComponent();
            Loaded += StudentMainWindow_Loaded;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StudentMainWindow smw = new StudentMainWindow();
            smw.Show();
            this.Close();
        }

        private async void StudentMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLessonsAsync();
        }

        private async Task LoadLessonsAsync()
        {
            try
            {
                // Получаем занятия для студента
                var lessons = await App.SupabaseService.GetStudentLessonsAsync(_studentId);

                // Отображаем занятия (например, в DataGrid)
                lessonsGrid.ItemsSource = lessons;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке занятий: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
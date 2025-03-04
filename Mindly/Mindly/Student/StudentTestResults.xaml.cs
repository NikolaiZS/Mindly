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

namespace Mindly.Student
{
    /// <summary>
    /// Логика взаимодействия для StudentTestResults.xaml
    /// </summary>
    public partial class StudentTestResults : Window
    {
        private readonly int _studentId = CurrentUser.CurrentUserId;

        public StudentTestResults()
        {
            InitializeComponent();
            Loaded += StudentMainWindow_Loaded;
        }

        private async void StudentMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTestResultsAsync();
        }

        private async Task LoadTestResultsAsync()
        {
            try
            {
                // Получаем результаты тестов для студента
                var testResults = await App.SupabaseService.GetTestResultsForStudentAsync(_studentId);

                // Привязываем данные к DataGrid
                testResultsGrid.ItemsSource = testResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке результатов тестов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StudentMainWindow smw = new StudentMainWindow();
            smw.Show();
            this.Close();
        }
    }

    public class TestResultViewModel
    {
        public string TestTitle { get; set; } // Название теста
        public DateTime UpdatedAt { get; set; } // Дата прохождения теста
        public int Score { get; set; } // Результат теста
    }
}
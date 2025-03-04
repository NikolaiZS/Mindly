using Mindly.Models;
using Supabase.Interfaces;
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
    /// Логика взаимодействия для StudentGrades.xaml
    /// </summary>
    public partial class StudentGrades : Window
    {
        private readonly int _studentId = CurrentUser.CurrentUserId;

        public StudentGrades()
        {
            InitializeComponent();
            Loaded += StudentMainWindow_Loaded;
        }

        private async void StudentMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadGradesAsync();
        }

        private async Task LoadGradesAsync()
        {
            try
            {
                // Получаем оценки для студента
                var grades = await App.SupabaseService.GetGradesForStudentAsync(_studentId);

                // Привязываем данные к DataGrid
                gradesGrid.ItemsSource = grades;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке оценок: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StudentMainWindow smw = new StudentMainWindow();
            smw.Show();
            this.Close();
        }

        public class GradeViewModel
        {
            public string Subject { get; set; } // Название курса
            public DateTime Date { get; set; }  // Дата оценки
            public int Grade { get; set; }      // Оценка
        }
    }
}
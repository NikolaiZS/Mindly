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

        private DataGridCell GetParentDataGridCell(DependencyObject element)
        {
            while (element != null && !(element is DataGridCell))
            {
                element = VisualTreeHelper.GetParent(element);
            }
            return element as DataGridCell;
        }

        private void DgLessons_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid == null) return;

            // Перебираем все ячейки в строке
            foreach (var column in dataGrid.Columns)
            {
                var cell = column.GetCellContent(e.Row)?.Parent as DataGridCell;
                if (cell != null)
                {
                    // Сохраняем содержимое ячейки в словаре
                    var content = (cell.Content as TextBlock)?.Text;
                    if (!string.IsNullOrEmpty(content))
                    {
                        _cellContentMap[cell] = content;
                    }
                }
            }
        }

        private void DgLessons_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("1");
            var dataGrid = sender as DataGrid;
            if (dataGrid == null) return;
            Debug.WriteLine("2");

            // Получаем элемент, по которому был произведен клик
            var element = e.OriginalSource as DependencyObject;
            if (element == null) return;
            Debug.WriteLine("3");

            // Ищем родительскую ячейку DataGrid
            var cell = GetParentDataGridCell(element);
            if (cell == null) return; // Если ячейка не найдена, выходим
            Debug.WriteLine("4");

            // Получаем содержимое ячейки из словаря
            if (_cellContentMap.TryGetValue(cell, out var content))
            {
                Debug.WriteLine("5");
                // Копируем содержимое в буфер обмена
                Clipboard.SetText(content);
                Debug.WriteLine("6");

                // Опционально: показываем уведомление
                MessageBox.Show($"Скопировано: {content}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
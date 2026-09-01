using Mindly.Models;
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

namespace Mindly.Teacher
{
    /// <summary>
    /// Логика взаимодействия для TeacherExam.xaml
    /// </summary>
    public partial class TeacherExam : Window
    {
        public TeacherExam()
        {
            InitializeComponent();
            Loaded += TeacherExam_Loaded;
        }

        private async void TeacherExam_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCoursesAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherMainMenu tmm = new TeacherMainMenu();
            tmm.Show();
            this.Close();
        }

        private async void AssignExam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedCourseId = cbxCourse.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(selectedCourseId))
                {
                    MessageBox.Show("Выберите курс.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var examName = txtExamName.Text;
                if (string.IsNullOrEmpty(examName))
                {
                    MessageBox.Show("Введите название экзамена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpExamDate.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату экзамена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var examDate = dpExamDate.SelectedDate.Value;

                var conferenceLink = txtConferenceLink.Text;
                if (string.IsNullOrEmpty(conferenceLink))
                {
                    MessageBox.Show("Введите ссылку на конференцию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var client = App.SupabaseService.GetClient();

                var exam = new Exams
                {
                    teacher_id = CurrentUser.CurrentUserId,
                    course_id = int.Parse(selectedCourseId),
                    title = examName,
                    date = examDate,
                    description = conferenceLink
                };

                var response = await client
                    .From<Exams>()
                    .Insert(exam);

                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    MessageBox.Show("Экзамен успешно назначен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Не удалось назначить экзамен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при назначении экзамена: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            cbxCourse.SelectedIndex = -1;
            txtExamName.Clear();
            dpExamDate.SelectedDate = null;
            txtConferenceLink.Clear();
        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                // Получаем клиент Supabase
                var client = App.SupabaseService.GetClient();

                // Получаем все курсы
                var response = await client
                    .From<Courses>()
                    .Select("id, name")
                    .Get();

                // Преобразуем данные в список для ComboBox
                var courses = response.Models?.Select(c => new
                {
                    Id = c.id, // Используем Id как значение
                    Name = c.name // Отображаем название курса
                }).ToList();

                // Привязываем данные к ComboBox
                cbxCourse.ItemsSource = courses;
                cbxCourse.DisplayMemberPath = "Name"; // Отображаем название курса
                cbxCourse.SelectedValuePath = "Id"; // Используем Id как значение
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке курсов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
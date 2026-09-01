using System;
using System.Collections;
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
    /// Логика взаимодействия для TeacherTestQuestions.xaml
    /// </summary>
    public partial class TeacherTestQuestions : Window
    {
        public TeacherTestQuestions()
        {
            InitializeComponent();
            LoadSampleQuestions();
        }

        private void LoadSampleQuestions()
        {
            var questions = new[]
            {
            new
            {
                QuestionText = "Вопрос 1",
                Type = "Множественный выбор"
            },
            new
            {
                QuestionText = "Вопрос 2",
                Type = "Множественный выбор"
            },
            new
            {
                QuestionText = "Вопрос 3",
                Type = "Множественный выбор"
            },
            new
            {
                QuestionText = "Вопрос 4",
                Type = "Множественный выбор"
            },
            new
            {
                QuestionText = "Вопрос 5",
                Type = "Множественный выбор"
            }
        };

            QuestionsList.ItemsSource = questions;
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            // Логика добавления нового вопроса
            var newQuestion = new
            {
                QuestionText = "Новый вопрос",
                Type = "Текстовый ответ"
            };

            var list = new ArrayList((ICollection)QuestionsList.ItemsSource);
            list.Add(newQuestion);
            QuestionsList.ItemsSource = list;
        }

        private void EditQuestion_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            dynamic question = button.DataContext;
            TeacherEditQuestion teq = new TeacherEditQuestion();
            teq.Show();
            this.Close();
        }

        private void DeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            dynamic question = button.DataContext;

            var list = new ArrayList((ICollection)QuestionsList.ItemsSource);
            list.Remove(question);
            QuestionsList.ItemsSource = list;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherTestMainMenu ttmm = new TeacherTestMainMenu();
            ttmm.Show();
            this.Close();
        }
    }
}
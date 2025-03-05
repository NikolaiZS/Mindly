using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для TeacherEditQuestion.xaml
    /// </summary>
    public partial class TeacherEditQuestion : Window
    {
        public ObservableCollection<Answer> Answers { get; } = new ObservableCollection<Answer>();

        public TeacherEditQuestion()
        {
            InitializeComponent();
            AnswersItemsControl.ItemsSource = Answers;
            AddDefaultAnswers();
        }

        private void AddDefaultAnswers()
        {
            for (int i = 0; i < 4; i++)
            {
                Answers.Add(new Answer { Text = $"Вариант {i + 1}" });
            }
        }

        public class Answer : INotifyPropertyChanged
        {
            private string _text;
            private bool _isCorrect;

            public string Text
            {
                get => _text;
                set { _text = value; OnPropertyChanged(); }
            }

            public bool IsCorrect
            {
                get => _isCorrect;
                set { _isCorrect = value; OnPropertyChanged(); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            Answers.Add(new Answer { Text = "Новый вариант" });
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Answers.Count(a => a.IsCorrect) == 0)
            {
                MessageBox.Show("Выберите хотя бы один правильный ответ!");
                return;
            }

            if (Answers.Any(a => string.IsNullOrWhiteSpace(a.Text)))
            {
                MessageBox.Show("Все варианты ответов должны быть заполнены!");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
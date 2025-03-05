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
    /// Логика взаимодействия для StudentTestWindow.xaml
    /// </summary>
    public partial class StudentTestWindow : Window
    {
        private List<Question> questions;
        private int currentQuestionIndex = 0;
        private int score = 0;
        private Dictionary<int, string> userAnswers = new Dictionary<int, string>();

        public StudentTestWindow()
        {
            InitializeComponent();
            InitializeTestData();
            LoadQuestion(currentQuestionIndex);
        }

        private void InitializeTestData()
        {
            questions = new List<Question>
        {
            new Question
            {
                Title = "Вопрос 1",
                Text = "Сколько будет 2 + 2?",
                Answers = new List<string> { "3", "4", "5", "6" },
                CorrectAnswer = "4"
            },
            new Question
            {
                Title = "Вопрос 2",
                Text = "Столица Франции?",
                Answers = new List<string> { "Лондон", "Берлин", "Париж", "Мадрид" },
                CorrectAnswer = "Париж"
            },
            new Question
            {
                Title = "Вопрос 3",
                Text = "Самый большой океан?",
                Answers = new List<string> { "Атлантический", "Индийский", "Северный Ледовитый", "Тихий" },
                CorrectAnswer = "Тихий"
            },
            new Question
            {
                Title = "Вопрос 4",
                Text = "Автор 'Преступления и наказания'?",
                Answers = new List<string> { "Толстой", "Достоевский", "Чехов", "Гоголь" },
                CorrectAnswer = "Достоевский"
            },
            new Question
            {
                Title = "Вопрос 5",
                Text = "Химический символ золота?",
                Answers = new List<string> { "Fe", "Cu", "Ag", "Au" },
                CorrectAnswer = "Au"
            }
        };
        }

        private void LoadQuestion(int index)
        {
            var question = questions[index];

            tbQuestionNumber.Text = $"Вопрос {index + 1} из {questions.Count}";
            tbQuestionTitle.Text = question.Title;
            tbQuestionText.Text = question.Text;

            btnAnswer1.Content = question.Answers[0];
            btnAnswer2.Content = question.Answers[1];
            btnAnswer3.Content = question.Answers[2];
            btnAnswer4.Content = question.Answers[3];
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = (Button)sender;
            userAnswers[currentQuestionIndex] = selectedButton.Content.ToString();

            if (currentQuestionIndex < questions.Count - 1)
            {
                currentQuestionIndex++;
                LoadQuestion(currentQuestionIndex);
            }
            else
            {
                CalculateScore();
                tbScore.Visibility = Visibility.Visible;
                buttonScoreExit.Visibility = Visibility.Visible;

                btnAnswer1.IsEnabled = false;
                btnAnswer2.IsEnabled = false;
                btnAnswer3.IsEnabled = false;
                btnAnswer4.IsEnabled = false;
            }
        }

        private void CalculateScore()
        {
            score = 0;
            foreach (var answer in userAnswers)
            {
                if (answer.Value == questions[answer.Key].CorrectAnswer)
                {
                    score++;
                }
            }
            tbScore.Text = $"Оценка: {score}/5";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }

    public class Question
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public List<string> Answers { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
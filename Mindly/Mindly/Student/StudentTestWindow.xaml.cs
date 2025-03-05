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
                Text = "Чему равно 15 + 7?",
                Answers = new List<string> { "21", "22", "23", "24" },
                CorrectAnswer = "22"
            },
            new Question
            {
                Title = "Вопрос 2",
                Text = "Чему равно 36 ÷ 6?",
                Answers = new List<string> { "5", "6", "7", "8" },
                CorrectAnswer = "6"
            },
            new Question
            {
                Title = "Вопрос 3",
                Text = "Чему равно 9 × 4?",
                Answers = new List<string> { "32", "34", "36", "38" },
                CorrectAnswer = "36"
            },
            new Question
            {
                Title = "Вопрос 4",
                Text = "Чему равно 50 - 19?",
                Answers = new List<string> { "29", "30", "31", "32" },
                CorrectAnswer = "31"
            },
            new Question
            {
                Title = "Вопрос 5",
                Text = "Какой остаток останется при делении 17 на 5?",
                Answers = new List<string> { "1", "2", "3", "5" },
                CorrectAnswer = "3"
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
            StudentMainWindow smw = new StudentMainWindow();
            smw.Show();
            this.Close();
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
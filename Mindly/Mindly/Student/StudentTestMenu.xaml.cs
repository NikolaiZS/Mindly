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
    /// Логика взаимодействия для StudentTestMenu.xaml
    /// </summary>
    public partial class StudentTestMenu : Window
    {
        public StudentTestMenu()
        {
            InitializeComponent();
            LoadTests();
        }

        private void LoadTests()
        {
            // Пример загрузки тестов
            var availableTests = new List<Test>
        {
            new Test
            {
                Title = "Основы математики",
                Description = "Тест по базовым математическим операциям",
                QuestionCount = 20,
                TimeLimit = 30
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            },
            new Test
            {
                Title = "История Древнего мира",
                Description = "Тестирование знаний по древней истории",
                QuestionCount = 15,
                TimeLimit = 25
            }
        };

            TestsItemsControl.ItemsSource = availableTests;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StudentMainWindow smw = new StudentMainWindow();
            smw.Show();
            this.Close(); //Я твою мать ногами пиздил за такие приколы
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
        }

        // Пример ну ты понял работай давай
        /*private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            var selectedTest = (sender as Button)?.DataContext as Test;
            if (selectedTest != null)
            {
                var testWindow = new StudentTestWindow(selectedTest);
                testWindow.ShowDialog();
            }
        }*/
    }

    // Модель данных
    public class Test
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int QuestionCount { get; set; }
        public int TimeLimit { get; set; }
        public bool IsCompleted { get; set; }
    }
}
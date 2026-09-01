using Mindly.Student;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для TeacherTestMainMenu.xaml
    /// </summary>
    public partial class TeacherTestMainMenu : Window
    {
        public TeacherTestMainMenu()
        {
            InitializeComponent();
            LoadTests();
        }

        private void LoadTests()
        {
            // Пример данных без использования классов
            var tests = new[]
            {
            new
            {
                Title = "Основы математики",
                Description = "Тест по базовым операциям",
                QuestionCount = 5
            },
        };

            TestsList.ItemsSource = tests;
        }

        private void CreateNewTest_Click(object sender, RoutedEventArgs e)
        {
            // Логика создания нового теста
            var newTest = new
            {
                Title = "Новый тест",
                Description = "Описание теста",
                QuestionCount = 0,
                TimeLimit = 0
            };

            var list = new ArrayList((ICollection)TestsList.ItemsSource);
            list.Add(newTest);
            TestsList.ItemsSource = list;
        }

        private void EditTest_Click(object sender, RoutedEventArgs e)
        {
            var testWindow = new TeacherTestQuestions();
            testWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherMainMenu mainMenu = new TeacherMainMenu();
            mainMenu.Show();
            this.Close();
        }
    }
}
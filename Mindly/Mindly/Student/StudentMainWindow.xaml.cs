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
    /// Логика взаимодействия для StudentMainWindow.xaml
    /// </summary>
    public partial class StudentMainWindow : Window
    {
        public StudentMainWindow()
        {
            InitializeComponent();
        }

        private void TestsButton_Click(object sender, RoutedEventArgs e)
        {
            StudentTestMenu stm = new StudentTestMenu();
            stm.Show();
            this.Close();
        }

        private void TheoryLessonsButton_Click(object sender, RoutedEventArgs e)
        {
            StudentTheoryLessons stl = new StudentTheoryLessons();
            stl.Show();
            this.Close();
        }

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            StudentGrades sg = new StudentGrades();
            sg.Show();
            this.Close();
        }

        private void TestResultsButton_Click(object sender, RoutedEventArgs e)
        {
            StudentTestResults str = new StudentTestResults();
            str.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
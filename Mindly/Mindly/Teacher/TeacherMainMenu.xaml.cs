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
    /// Логика взаимодействия для TeacherMainMenu.xaml
    /// </summary>
    public partial class TeacherMainMenu : Window
    {
        public TeacherMainMenu()
        {
            InitializeComponent();
        }

        private void AssignGradesButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherGrades tg = new TeacherGrades();
            tg.Show();
            this.Close();
        }

        private void AssignExamsButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherExam te = new TeacherExam();
            te.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
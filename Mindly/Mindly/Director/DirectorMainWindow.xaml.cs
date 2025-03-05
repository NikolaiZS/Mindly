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

namespace Mindly.Director
{
    /// <summary>
    /// Логика взаимодействия для DirectorMainWindow.xaml
    /// </summary>
    public partial class DirectorMainWindow : Window
    {
        public DirectorMainWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AssignTeacherToStudentButton_Click(object sender, RoutedEventArgs e)
        {
            DirectorAssignTeacherToStudent datts = new DirectorAssignTeacherToStudent();
            datts.Show();
            this.Close();
        }

        private void EditLessonsButton_Click(object sender, RoutedEventArgs e)
        {
            DirectorEditClasses dec = new DirectorEditClasses();
            dec.Show();
            this.Close();
        }
    }
}
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
    /// Логика взаимодействия для TeacherEditQuestion.xaml
    /// </summary>
    public partial class TeacherEditQuestion : Window
    {
        public TeacherEditQuestion()
        {
            InitializeComponent();
        }

        public TeacherEditQuestion(string title, string question) : this()
        {
            txtTitle.Text = title;
            txtQuestion.Text = question;
        }

        public string QuestionTitle => txtTitle.Text;
        public string QuestionText => txtQuestion.Text;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
               string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!");
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
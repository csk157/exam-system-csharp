using Classes.Model;
using Classes.Service;
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

namespace Desktop
{
    public partial class CreateEditExam : Window
    {
        private Exam exam;
        private bool create = true;
        private bool saveClosed = false;
        public CreateEditExam()
        {
            InitializeComponent();
            Title = "Create exam";
            Educations.ItemsSource = Service.Instance.Educations;
        }

        public CreateEditExam(Education e)
            : this()
        {
            exam = new Exam();
            exam.Education = e;
            DataContext = exam;
        }

        public CreateEditExam(Exam e) : this()
        {
            exam = e;
            Title = "Edit exam";
            create = false;
            DataContext = e;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!exam.IsValid())
                return;

            if (create)
                Service.Instance.Add(exam);
            else
                Service.Instance.Update(exam);

            saveClosed = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClosedWindow(object sender, EventArgs e)
        {
            if (!saveClosed && !create)
                Service.Instance.Reset(exam);
        }
    }
}

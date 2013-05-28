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
    /// <summary>
    /// Interaction logic for CreateEditStudent.xaml
    /// </summary>
    public partial class ShowStudent : Window
    {
        private Student student;
        public ShowStudent(Student s)
        {
            InitializeComponent();
            student = s;
            DataContext = s;
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            CreateEditStudent d = new CreateEditStudent(student);
            d.ShowDialog();
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            Service.Instance.Remove(student);
            Close();
        }
    }
}

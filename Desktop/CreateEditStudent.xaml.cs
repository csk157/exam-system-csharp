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
    public partial class CreateEditStudent : Window
    {
        private Student student;
        private bool create = true;
        private bool saveClosed = false;
        public CreateEditStudent()
        {
            InitializeComponent();
            Education.ItemsSource = Service.Instance.Educations;
            Binding b = BindingOperations.GetBinding(CPR, TextBox.TextProperty);
            if (b != null)
                b.ValidationRules.Add(new UniqueCprValidation());
        }

        public CreateEditStudent(Student s) : this()
        {
            student = s;
            create = false;
            DataContext = student;
        }

        public CreateEditStudent(Education e)
            : this()
        {
            student = new Student();
            student.Education = e;
            DataContext = student;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!student.IsValid())
                return;

            if (create && !Service.Instance.IsUniqueCPR(CPR.Text))
                return;

            if (create)
                Service.Instance.Add(student);
            else
                Service.Instance.Update(student);

            saveClosed = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClosedWindow(object sender, EventArgs e)
        {
            if(!saveClosed)
                Service.Instance.Reset(student);
        }
    }

    public class UniqueCprValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string v = (string)value;

            if (Service.Instance.IsUniqueCPR(v))
                return new ValidationResult(true, null);
            else
                return new ValidationResult(false, "Cpr is already registered");
        }
    }
}

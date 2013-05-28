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
    /// Interaction logic for CreateEditEducation.xaml
    /// </summary>
    public partial class CreateEditEducation : Window
    {
        private Education education;
        public CreateEditEducation()
        {
            InitializeComponent();
        }

        public CreateEditEducation(Education e)
            : this()
        {
            education = e;
            this.DataContext = education;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (education == null)
            { //Create
                Education ed = new Education(Title.Text);
                Service.Instance.Add(ed);
            }
            else
            {
                education.Update();
                Service.Instance.UpdateTable("Educations");
            }

            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

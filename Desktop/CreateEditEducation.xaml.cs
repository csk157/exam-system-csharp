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
    public partial class CreateEditEducation : Window
    {
        private bool create = true;
        private bool saveClosed = false;
        private Education education;
        public CreateEditEducation()
        {
            InitializeComponent();
            Title = "Create education";
            education = new Education();
            DataContext = education;
        }

        public CreateEditEducation(Education e)
            : this()
        {
            education = e;
            Title = "Edit education";
            this.DataContext = education;
            create = false;
            DataContext = education;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!education.IsValid())
                return;

            if (create)
                Service.Instance.Add(education);
            else
                Service.Instance.Update(education);

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
                Service.Instance.Reset(education);
        }
    }
}

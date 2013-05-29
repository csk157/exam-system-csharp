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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Classes.Dao;
using Classes.Service;
using Classes.Model;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Educations.ItemsSource = Service.Instance.Educations;
            ExamEducations.ItemsSource = Service.Instance.Educations;
            RequiringExemption.ItemsSource = Service.Instance.GetRequiringExemptionStudents();
        }

        private void OnEducationsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Educations.SelectedValue != null)
            {
                Education ed = (Education)Educations.SelectedValue;
                Students.ItemsSource = ed.Students;
                Exams.ItemsSource = ed.Exams;
                EditEducationButton.IsEnabled = true;
                DeleteEducationButton.IsEnabled = true;
                CreateStudentButton.IsEnabled = true;
                CreateExamButton.IsEnabled = true;
            }
            else
            {
                EditEducationButton.IsEnabled = false;
                DeleteEducationButton.IsEnabled = false;
                CreateStudentButton.IsEnabled = false;
                CreateExamButton.IsEnabled = false;
            }
        }

        private void CreateEducationClick(object sender, RoutedEventArgs e)
        {
            CreateEditEducation c = new CreateEditEducation();
            c.ShowDialog();
        }

        private void EditEducationClick(object sender, RoutedEventArgs e)
        {
            Education ed = (Education)Educations.SelectedValue;
            CreateEditEducation c = new CreateEditEducation(ed);
            c.ShowDialog();
        }

        private void DeleteEducationClick(object sender, RoutedEventArgs e)
        {
            Education ed = (Education)Educations.SelectedValue;
            Service.Instance.Remove(ed);
        }

        private void ShowStudentClick(object sender, RoutedEventArgs e)
        {
            Student st = (Student)Students.SelectedValue;
            ShowStudent d = new ShowStudent(st);
            d.ShowDialog();
        }

        private void StudentsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Students.SelectedValue != null)
            {
                Student st = (Student) Students.SelectedValue;
                Exam ex = (Exam) Exams.SelectedValue;
                ShowStudentButton.IsEnabled = true;
                EditStudentButton.IsEnabled = true;
                DeleteStudentButton.IsEnabled = true;

                if (Exams.SelectedValue != null && st.CanRegisterForExam(ex))
                    RegisterStudent.IsEnabled = true;
                else
                {
                    RegisterStudent.IsEnabled = false;
                    if (Exams.SelectedValue != null && st.NeedsExemption(ex) && st.CanGetExemption(ex))
                        GiveExceptionButton.IsEnabled = true;
                    else
                        GiveExceptionButton.IsEnabled = false;
                }
            }
            else
            {
                ShowStudentButton.IsEnabled = false;
                EditStudentButton.IsEnabled = false;
                DeleteStudentButton.IsEnabled = false;
                RegisterStudent.IsEnabled = false;
                GiveExceptionButton.IsEnabled = false;
            }
        }

        private void ExamsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Exams.SelectedValue != null)
            {
                Exam ex = (Exam)Exams.SelectedValue;
                Student st = (Student) Students.SelectedValue;
                EditExamButton.IsEnabled = true;
                DeleteExamButton.IsEnabled = true;

                if (Students.SelectedValue != null && st.CanRegisterForExam(ex))
                    RegisterStudent.IsEnabled = true;
                else
                {
                    RegisterStudent.IsEnabled = false;
                    if (Students.SelectedValue != null && st.NeedsExemption(ex) && st.CanGetExemption(ex))
                        GiveExceptionButton.IsEnabled = true;
                    else
                        GiveExceptionButton.IsEnabled = false;
                }
            }
            else
            {
                EditExamButton.IsEnabled = false;
                DeleteExamButton.IsEnabled = false;
                RegisterStudent.IsEnabled = false;
                GiveExceptionButton.IsEnabled = false;
            }
        }

        private void CreateStudentClick(object sender, RoutedEventArgs e)
        {
            Education ed = (Education)Educations.SelectedValue;
            CreateEditStudent d = new CreateEditStudent(ed);
            d.ShowDialog();
        }

        private void EditStudentClick(object sender, RoutedEventArgs e)
        {
            Student st = (Student)Students.SelectedValue;
            CreateEditStudent d = new CreateEditStudent(st);
            d.ShowDialog();
            Exams.ItemsSource = null;
            Exams.ItemsSource = ((Education)Educations.SelectedValue).Exams;
        }

        private void DeleteStudentClick(object sender, RoutedEventArgs e)
        {
            Student st = (Student)Students.SelectedValue;
            Service.Instance.Remove(st);
        }

        private void CreateExamClick(object sender, RoutedEventArgs e)
        {
            Education ed = (Education)Educations.SelectedValue;
            CreateEditExam d = new CreateEditExam(ed);
            d.ShowDialog();
        }

        private void EditExamClick(object sender, RoutedEventArgs e)
        {
            Exam ex = (Exam)Exams.SelectedValue;
            CreateEditExam d = new CreateEditExam(ex);
            d.ShowDialog();
            Exams.ItemsSource = null;
            Exams.ItemsSource = ((Education)Educations.SelectedValue).Exams;
        }

        private void DeleteExamClick(object sender, RoutedEventArgs e)
        {
            Exam ex = (Exam)Exams.SelectedValue;
            Service.Instance.Remove(ex);
        }

        private void RegisterStudentClick(object sender, RoutedEventArgs e)
        {
            Student st = (Student)Students.SelectedValue;
            Exam ex = (Exam)Exams.SelectedValue;

            Service.Instance.RegisterForExam(st, ex);
            RegisterStudent.IsEnabled = false;
            GiveExceptionButton.IsEnabled = false;
        }

        private void ExamEducationsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExamEducations.SelectedValue != null)
            {
                Education ed = (Education)ExamEducations.SelectedValue;
                ExamExams.ItemsSource = ed.Exams;
            }
            else
                ExamExams.ItemsSource = null;
        }

        private void ExamExamsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExamExams.SelectedValue != null)
                FillAttempts();
            else
            {
                ExamExams.ItemsSource = null;
                ExamAttempts.Children.Clear();
            }
        }

        private void FillAttempts()
        {
            if (ExamEducations.SelectedValue != null && ExamExams.SelectedValue != null)
            {
                Exam ex = (Exam) ExamExams.SelectedValue;
                ExamAttempts.Children.Clear();
                foreach (Attempt a in ex.NotExamined())
                {
                    StackPanel pan = new StackPanel();
                    pan.Margin = new Thickness(0, 0, 0, 10);
                    Label name = new Label();
                    name.Content = a.Student.Name;
                    name.Width = 150;

                    Label cpr = new Label();
                    cpr.Content = a.Student.CPR;
                    cpr.Width = 100;

                    TextBox grade = new TextBox();
                    grade.Width = 50;
                    grade.Name = "Attempt" + a.ID;
                    grade.LostFocus += CheckIsGradeFocusLost;
                    grade.CharacterCasing = CharacterCasing.Upper;

                    pan.Orientation = Orientation.Horizontal;
                    pan.Children.Add(name);
                    pan.Children.Add(cpr);
                    pan.Children.Add(grade);

                    ExamAttempts.Children.Add(pan);
                    if(FindName(grade.Name) != null)
                        UnregisterName(grade.Name);
                    RegisterName(grade.Name, grade);
                }
            }
        }

        private void CompleteExamClick(object sender, RoutedEventArgs e)
        {
            if (ExamEducations.SelectedValue != null && ExamExams.SelectedValue != null)
            {
                if (!IsControlCorrect())
                {
                    MessageBox.Show("Some fields do not have values or control numbers are incorrect, please do the check and try again");
                    return;
                }
                else
                    MessageBox.Show("Control numbers are correct");

                Exam ex = (Exam)ExamExams.SelectedValue;
                List<Model> toUpdate = new List<Model>();
                foreach (Attempt a in ex.NotExamined())
                {
                    TextBox tb = (TextBox)FindName("Attempt" + a.ID);
                    if (tb != null)
                    {
                        if(Attempt.allowedGrades.Contains(tb.Text)){
                            a.Grade = tb.Text;
                            toUpdate.Add(a);
                            UnregisterName(tb.Name);
                        }
                    }
                }
                Service.Instance.Update(toUpdate.ToArray<Model>());
            }

            FillAttempts();
            ClearExam();
            RefreshRequiringExemption();
        }


        /* if empty grade, assume not tested */
        private bool IsControlCorrect()
        {
            bool valid = true;
            Dictionary<string, int> gradesCount = new Dictionary<string, int>();

            foreach (string g in Attempt.allowedGrades)
                gradesCount.Add(g, 0);

            if (ExamEducations.SelectedValue != null && ExamExams.SelectedValue != null)
            {
                Exam ex = (Exam)ExamExams.SelectedValue;
                foreach (Attempt a in ex.NotExamined())
                {
                    TextBox tb = (TextBox)FindName("Attempt" + a.ID);
                    if (tb != null)
                    {
                        if (gradesCount.ContainsKey(tb.Text))
                            gradesCount[tb.Text] += 1;
                        else if (tb.Text == "")
                            continue; // assume not tested
                        else
                            valid = false; // should not contain something else
                    }
                    else
                        valid = false; // Text box not found

                    if (!valid) break;
                }

                int count;
                valid = valid && int.TryParse(ExamSY.Text, out count) && count == gradesCount["SY"];
                valid = valid && int.TryParse(ExamEJ.Text, out count) && count == gradesCount["EJ"];
                valid = valid && int.TryParse(ExamN3.Text, out count) && count == gradesCount["-3"];
                valid = valid && int.TryParse(Exam00.Text, out count) && count == gradesCount["00"];
                valid = valid && int.TryParse(Exam02.Text, out count) && count == gradesCount["02"];
                valid = valid && int.TryParse(Exam4.Text, out count) && count == gradesCount["4"];
                valid = valid && int.TryParse(Exam7.Text, out count) && count == gradesCount["7"];
                valid = valid && int.TryParse(Exam10.Text, out count) && count == gradesCount["10"];
                valid = valid && int.TryParse(Exam12.Text, out count) && count == gradesCount["12"];
            }
            else
                valid = false; // No exam selected

            return valid;
        }

        private void ClearExam()
        {
            ExamSY.Text = "";
            ExamEJ.Text = "";
            ExamN3.Text = "";
            Exam00.Text = "";
            Exam02.Text = "";
            Exam4.Text = "";
            Exam7.Text = "";
            Exam10.Text = "";
            Exam12.Text = "";

            if (ExamEducations.SelectedValue != null && ExamExams.SelectedValue != null)
            {
                Exam ex = (Exam)ExamExams.SelectedValue;
                foreach (Attempt a in ex.NotExamined())
                {
                    TextBox tb = (TextBox)FindName("Attempt" + a.ID);
                    if (tb != null)
                        tb.Text = "";
                }
            }
        }

        private void AllowOnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsNumber(Convert.ToChar(e.Text));
            base.OnPreviewTextInput(e);
        }

        private void CheckIsGradeFocusLost(object sender, RoutedEventArgs e)
        {
            if(sender is TextBox){
                TextBox tb = (TextBox)sender;

                if (Attempt.allowedGrades.Contains(tb.Text))
                    return;
                else
                    tb.Text = "";
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            ClearExam();
        }

        private void RequiringExemptionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequiringExemption.SelectedValue != null)
            {
                Student s = (Student)RequiringExemption.SelectedValue;
                RequiringExemptionExams.ItemsSource = s.GetExamsNeedingExemptions();
            }
            else
            {
                RequiringExemptionExams.ItemsSource = null;
                RequiringGiveExemptionButton.IsEnabled = false;
            }
        }

        private void RequiringExemptionExamSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequiringExemptionExams.SelectedValue != null)
            {
                Exam ex = (Exam)RequiringExemptionExams.SelectedValue;
                Student st = (Student)RequiringExemption.SelectedValue;
                PreviousAttempts.ItemsSource = ex.AttemptsBy((Student)RequiringExemption.SelectedValue);

                RequiringGiveExemptionButton.IsEnabled = st.CanGetExemption(ex);
            }
            else
            {
                PreviousAttempts.ItemsSource = null;
                RequiringGiveExemptionButton.IsEnabled = false;
            }
        }

        private void RequiringGiveExemptionClick(object sender, RoutedEventArgs e)
        {
            if (RequiringExemption.SelectedValue != null && RequiringExemptionExams.SelectedValue != null)
            {
                Student st = (Student)RequiringExemption.SelectedValue;
                Exam ex = (Exam)RequiringExemptionExams.SelectedValue;

                Service.Instance.RegisterForExam(st, ex);
                RefreshRequiringExemption();
            }
        }

        private void RefreshRequiringExemption()
        {
            RequiringGiveExemptionButton.IsEnabled = false;
            RequiringExemption.ItemsSource = Service.Instance.GetRequiringExemptionStudents();
            RequiringExemptionExams.ItemsSource = null;
        }
    }
}

using Classes.Model;
using Classes.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class RegisterForExam : System.Web.UI.Page
    {
        private Student student;
        protected void Page_Load(object sender, EventArgs e)
        {
            student = (Student)Session["user"];
            FillRegisterTable();
            Master.NavSelection = "Reg";
        }

        private void FillRegisterTable()
        {
            TableHeaderRow hr = new TableHeaderRow();
            TableHeaderCell thTitle = new TableHeaderCell();
            thTitle.Text = "Title";
            TableHeaderCell thStatus = new TableHeaderCell();
            thStatus.Text = "Status";
            TableHeaderCell thReg = new TableHeaderCell();
            thReg.CssClass = "short-col";
            hr.Cells.Add(thTitle);
            hr.Cells.Add(thStatus);
            hr.Cells.Add(thReg);

            RegisterExams.Rows.Add(hr);

            IEnumerable<Exam> needed = student.GetNeededExams();
            if (needed.Count() > 0)
            {
                foreach (Exam ex in needed)
                {
                    TableRow tr = new TableRow();
                    TableCell tdTitle = new TableCell();
                    tdTitle.Text = ex.Title;

                    TableCell tdStatus = new TableCell();
                    TableCell tdReg = new TableCell();
                    if (student.CanRegisterForExam(ex))
                    {
                        tdStatus.Text = "You can register for this exam";
                        Button btn = new Button();
                        btn.CommandArgument = ex.ID.ToString();
                        btn.Command += RegisterForExamClick;
                        btn.Text = "Register";
                        btn.CssClass = "btn";
                        tdReg.Controls.Add(btn);
                    }
                    else if (student.CanGetExemption(ex))
                        tdStatus.Text = "You need exemption in order to take an exam. Please contact administration";
                    else
                        tdStatus.Text = "You are out of tries for this exam.";

                    tr.Cells.Add(tdTitle);
                    tr.Cells.Add(tdStatus);
                    tr.Cells.Add(tdReg);
                    RegisterExams.Rows.Add(tr);
                }
            }
            else
            {
                RegContainer.Controls.Remove(RegisterExams);
                Label lbl = new Label();
                lbl.Text = "There are no exams, congrats!";
                RegContainer.Controls.Add(lbl);
            }
        }

        private void RegisterForExamClick(object sender, CommandEventArgs e)
        {
            int id;
            if (int.TryParse(e.CommandArgument.ToString(), out id))
            {
                Exam ex = Service.Instance.GetExamById(id);

                if (ex != null)
                {
                    if (student.CanRegisterForExam(ex))
                    {
                        Service.Instance.RegisterForExam(student, ex);
                        Response.Redirect("Index.aspx");
                    }
                }
                else
                    Session["flash"] = new string[] { "error", "Exam cannot be found" };
            }
            else
                Session["flash"] = new string[] { "error", "There was an error. Please try again." };

            Response.Redirect("RegisterForExam.aspx");
        }
    }
}
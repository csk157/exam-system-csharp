using Classes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Index : System.Web.UI.Page
    {
        private Student student;
        protected void Page_Load(object sender, EventArgs e)
        {
            student = (Student)Session["user"];
            EducationName.Text = student.Education.Title;
            FillExamsTable();
            FillAttemptsTable();
            Master.NavSelection = "Overview";
        }

        private void FillAttemptsTable()
        {
            TableHeaderRow head = new TableHeaderRow();
            TableHeaderCell titleHead = new TableHeaderCell();
            titleHead.Text = "Title";
            TableHeaderCell gradeHead = new TableHeaderCell();
            gradeHead.Text = "Grade";
            gradeHead.CssClass = "short-col center-text";
            head.Cells.Add(titleHead);
            head.Cells.Add(gradeHead);
            Attempts.Rows.Add(head);

            foreach (Attempt a in student.Attempts)
            {
                TableRow row = new TableRow();
                TableCell title = new TableCell();
                title.Text = a.Exam.Title;

                TableCell grade = new TableCell();
                grade.CssClass = "center-text";
                if (a.IsTaken() || a.Grade == "SY")
                {
                    grade.Text = a.Grade;
                    if (a.IsPassed())
                        row.CssClass = "success";
                    else if (a.Grade != "SY")
                        row.CssClass = "error";
                }
                else
                {
                    Label clock = new Label();
                    clock.CssClass = "icon-time";
                    clock.ToolTip = "Not taken yet";
                    grade.Controls.Add(clock);
                }

                row.Cells.Add(title);
                row.Cells.Add(grade);

                Attempts.Rows.Add(row);
            }
        }

        private void FillExamsTable()
        {
            TableHeaderRow head = new TableHeaderRow();
            TableHeaderCell titleHead = new TableHeaderCell();
            titleHead.Text = "Title";
            TableHeaderCell passedHead = new TableHeaderCell();
            passedHead.Text = "Passed";
            passedHead.CssClass = "short-col center-text";
            head.Cells.Add(titleHead);
            head.Cells.Add(passedHead);
            Exams.Rows.Add(head);

            foreach (Exam e in student.Education.Exams)
            {
                TableRow row = new TableRow();
                TableCell title = new TableCell();
                title.Text = e.Title; 

                TableCell passed = new TableCell();
                passed.CssClass = "center-text";
                Label tick = new Label();

                if (student.HasPassedExam(e))
                {
                    tick.CssClass = "icon-ok";
                    tick.ToolTip = "Passed";
                    row.CssClass = "success";
                }
                else if (student.HasFailedExam(e) && !student.HasRegisteredForExam(e))
                {
                    tick.CssClass = "icon-remove";
                    tick.ToolTip = "Failed";
                    row.CssClass = "error";
                }
                else
                {
                    tick.CssClass = "icon-time";
                    tick.ToolTip = "Not taken yet";
                }

                passed.Controls.Add(tick);

                row.Cells.Add(title);
                row.Cells.Add(passed);
                Exams.Rows.Add(row);
            }
        }

        
    }
}
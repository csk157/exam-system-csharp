using Classes.Model;
using Classes.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class EditUser : System.Web.UI.Page
    {
        private Student student;
        protected void Page_Load(object sender, EventArgs e)
        {
            student = (Student)Session["user"];
        }

        protected void SaveUser(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                if (!String.IsNullOrWhiteSpace(Password.Text))
                {
                    student.Password = Password.Text;
                    Service.Instance.Update(student);
                }

                Session["flash"] = new string[] { "success", "Profile updated"};
                Response.Redirect("Index.aspx");
            }
        }

        protected void PasswordValidate(object source, ServerValidateEventArgs args)
        {
            string pass = (string)args.Value;
            args.IsValid = String.IsNullOrEmpty(pass) || pass.Length >= 5;
        }

        protected void PasswordsMatchValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (string)args.Value == Password.Text;
        }
    }
}
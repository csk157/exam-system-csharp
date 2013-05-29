using Classes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        private Student student;
        public string NavSelection { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                RedirectToLogin();
            else
            {
                student = (Student)Session["user"];
                UserName.Text = student.Name;
            }

            ShowFlash();
            SelectNav();
        }

        private void SelectNav()
        {
            if (NavSelection != null)
            {
                switch (NavSelection)
                {
                    case "Overview":
                        NavOverview.Attributes["class"] = "active";
                        break;
                    case "Reg":
                        NavReg.Attributes["class"] = "active";
                        break;
                }
            }
        }

        private void RedirectToLogin()
        {
            Response.Redirect("Login.aspx");
        }

        private void ShowFlash()
        {
            if (Session["flash"] != null && Session["flash"] is string[])
            {
                string[] flash = (string[])Session["flash"];

                if (flash.Length == 2)
                {
                    Alert.Visible = true;
                    Alert.Attributes["class"] += " " + flash[0];
                    Alert.InnerText = flash[1];
                }
            }

            Session["flash"] = null;
        }

        protected void LogOut(object sender, EventArgs e)
        {
            Session["user"] = null;
            RedirectToLogin();
        }
    }
}
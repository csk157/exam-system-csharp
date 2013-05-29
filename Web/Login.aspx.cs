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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Alert.Visible = false;
            if (Session["user"] != null)
                RedirectToIndex();
        }

        private void ShowAlert(string s)
        {
            Label error = new Label();
            error.Text = s;
            Alert.Controls.Add(error);
            Alert.Visible = true;
        }

        private void RedirectToIndex()
        {
            Response.Redirect("index.aspx");
        }

        protected void LoginClick(object sender, EventArgs e)
        {
            if (CPR.Text != "" && Password.Text != "")
            {
                Student s = Service.Instance.Login(CPR.Text, Password.Text);

                if (s != null)
                {
                    Session["user"] = s;
                    RedirectToIndex();
                }
                else
                    ShowAlert("CPR or Password is incorrect");
            }
            else
            {
                ShowAlert("Please make sure that CPR and Password are not empty");
            }
        }

    }
}
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Business Academy Aarhus - Students - Login</title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/login.css" rel="stylesheet" />
</head>
<body>
    <form id="LoginForm" runat="server">
        <div class="container">
            <img src="img/logo.png" title="Business Academy Aarhus" id="logo" />
            <div class="content">
                <div class="alert" id="Alert" runat="server"></div>
                <div class="row">
                    <div class="login-form">
                        <h2>Login</h2>
                        <div class="clearfix">
                            <asp:TextBox ID="CPR" runat="server" placeholder="CPR"></asp:TextBox>
                        </div>
                        <div class="clearfix">
                            <asp:TextBox ID="Password" type="password" runat="server" placeholder="Password"></asp:TextBox>
                        </div>
                        <asp:Button ID="SubmitLogin" runat="server" Text="Login" CssClass="btn primary" OnClick="LoginClick" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="js/jquery-1.10.0.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="Web.EditUser" MasterPageFile="~/Layout.Master" %>
<%@ MasterType VirtualPath="~/Layout.Master" %>


<asp:Content ContentPlaceHolderID="TitleHolder" runat="server">Edit profile</asp:Content>
<asp:Content ID="EditUser" ContentPlaceHolderID="Content" runat="server">
    <h2>Edit your profile</h2>

    <h4>Change password</h4>
    <div>
        <asp:Label runat="server" AssociatedControlID="Password" Text="New password"></asp:Label>
        <asp:TextBox runat="server" type="password" ID="Password" Text="" CausesValidation="true"></asp:TextBox>
        <asp:CustomValidator ID="PasswordValidation" runat="server" CssClass="validation-error" EnableClientScript="false" 
            Display="Dynamic" 
            ErrorMessage="Should be left empty if you do not want to change password or contain at least 5 characters"
            OnServerValidate="PasswordValidate" ControlToValidate="Password" ValidateEmptyText="true"></asp:CustomValidator>
    </div>

    <div>
        <asp:Label runat="server" AssociatedControlID="RepeatPassword" Text="Repeat new password"></asp:Label>
        <asp:TextBox runat="server" type="password" ID="RepeatPassword"  CausesValidation="true"></asp:TextBox>
        <%-- Compare validator does not compare when field is left empty. Have to create custom validator --%>
        <asp:CustomValidator ID="PasswordsMatch" runat="server" CssClass="validation-error" EnableClientScript="false" 
            Display="Dynamic" ErrorMessage="Passwords should match" OnServerValidate="PasswordsMatchValidate" 
            ControlToValidate="RepeatPassword" ValidateEmptyText="true"></asp:CustomValidator>
    </div>

    <div>
        <asp:Button runat="server" Text="Save" CssClass="btn btn-primary" OnClick="SaveUser"></asp:Button>
    </div>
</asp:Content>


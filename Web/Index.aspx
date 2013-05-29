<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Index" MasterPageFile="~/Layout.Master" %>
<%@ MasterType VirtualPath="~/Layout.Master" %>

<asp:Content ContentPlaceHolderID="TitleHolder" runat="server">Overview</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <div class="span6">
        <h3><asp:Label ID="EducationName" runat="server" Text=""></asp:Label> exams</h3>
        <div>
            <asp:Table ID="Exams" CssClass="table table-bordered" runat="server"></asp:Table>
        </div>
    </div>
    <div class="span6">
        <h3>Exam attempts</h3>
        <div>
            <asp:Table ID="Attempts" CssClass="table table-bordered" runat="server"></asp:Table>
        </div>
    </div>
</asp:Content>
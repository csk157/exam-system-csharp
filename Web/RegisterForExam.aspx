<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterForExam.aspx.cs" Inherits="Web.RegisterForExam" MasterPageFile="~/Layout.Master" %>
<%@ MasterType VirtualPath="~/Layout.Master" %>

<asp:Content ContentPlaceHolderID="TitleHolder" runat="server">Register for exam</asp:Content>
<asp:Content ID="RegExamContent" ContentPlaceHolderID="Content" runat="server">
    <h3>Available exams</h3>
    <div runat="server" id="RegContainer">
        <asp:Table ID="RegisterExams" CssClass="table table-bordered" runat="server"></asp:Table>
    </div>
</asp:Content>

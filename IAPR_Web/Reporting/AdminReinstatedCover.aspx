<%@ Page Language="C#"  MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="AdminReinstatedCover.aspx.cs" Inherits="IAPR_Web.Reporting.AdminReinstatedCover" %>
<%@ Register src="../UserControls/Reporting/Financer/ReinstatedCover.ascx" tagname="ReinstatedCover" tagprefix="uc1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:ReinstatedCover ID="ReinstatedCover" runat="server" />
</asp:Content>
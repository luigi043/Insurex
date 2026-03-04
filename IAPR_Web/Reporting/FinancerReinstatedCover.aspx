<%@ Page Language="C#"  MasterPageFile="~/Financer.Master"   AutoEventWireup="true" CodeBehind="FinancerReinstatedCover.aspx.cs" Inherits="IAPR_Web.Reporting.FinancerReinstatedCover" %>

<%@ Register src="../UserControls/Reporting/Financer/ReinstatedCover.ascx" tagname="ReinstatedCover" tagprefix="uc1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:ReinstatedCover ID="ReinstatedCover" runat="server" />
</asp:Content>
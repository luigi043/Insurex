<%@ Page Language="C#" MasterPageFile="~/Financer.Master" AutoEventWireup="true" CodeBehind="FinancerAllAssets.aspx.cs" Inherits="IAPR_Web.Reporting.FinancerAllAssets" %>

<%@ Register src="../UserControls/Reporting/Financer/AllAssets.ascx" tagname="AllAssets" tagprefix="uc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:AllAssets ID="AllAssets" runat="server" />
</asp:Content>

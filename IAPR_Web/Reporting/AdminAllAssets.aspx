<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="AdminAllAssets.aspx.cs" Inherits="IAPR_Web.Reporting.AdminAllAssets" %>
<%@ Register src="../UserControls/Reporting/Financer/AllAssets.ascx" tagname="AllAssets" tagprefix="uc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:AllAssets ID="AllAssets" runat="server" />
</asp:Content>


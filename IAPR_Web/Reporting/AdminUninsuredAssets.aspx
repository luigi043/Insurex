<%@ Page Language="C#"  MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="AdminUninsuredAssets.aspx.cs" Inherits="IAPR_Web.Reporting.AdminUninsuredAssets" %>

<%@ Register src="../UserControls/Reporting/Financer/UninsuredAssets.ascx" tagname="UninsuredAssets" tagprefix="uc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:UninsuredAssets ID="UninsuredAssets1" runat="server" />
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/Insurer.Master" AutoEventWireup="true" CodeBehind="InsurerUninsuredAssets.aspx.cs" Inherits="IAPR_Web.Reporting.InsurerUninsuredAssets" %>

<%@ Register src="../UserControls/Reporting/Insurer/UninsuredAssets.ascx" tagname="UninsuredAssets" tagprefix="uc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:UninsuredAssets ID="UninsuredAssets1" runat="server" />
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/Financer.Master" AutoEventWireup="true" CodeBehind="EditPartnerUsers.aspx.cs" Inherits="IAPR_Web.Admin.EditPartnerUsers" %>

<%@ Register Src="../UserControls/Admin/EditPartnerUser.ascx" TagName="EditPartnerUserUC" TagPrefix="uc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:EditPartnerUserUC ID="EditPartnerUserUC" runat="server" />

</asp:Content>

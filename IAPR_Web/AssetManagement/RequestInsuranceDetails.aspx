<%@ Page Language="C#" MasterPageFile="~/Financer.Master" AutoEventWireup="true" CodeBehind="RequestInsuranceDetails.aspx.cs" Inherits="IAPR_Web.AssetManagement.RequestInsuranceDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Request Insurance details</h1>

        <asp:Panel ID="pnlSaveButtons" runat="server">
            <asp:Button CssClass="btn btn-primary" ID="btnSendRequest" ValidationGroup="vgPolicy" runat="server" Text="Save" OnClick="btnCreatePolicy_Click" />
        </asp:Panel>
    </div>
</asp:Content>

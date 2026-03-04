<%@ Page Language="C#"  MasterPageFile="~/Insurer.Master"  AutoEventWireup="true" CodeBehind="InsurerMonthlyReport.aspx.cs" Inherits="IAPR_Web.Reporting.InsurerMonthlyReport" %>

<%@ Register Src="../UserControls/Reporting/Insurer/NonPayment.ascx" TagName="NonPayment" TagPrefix="uc1" %>


<%@ Register Src="../UserControls/Reporting/Insurer/ChangeInsuranceCover.ascx" TagName="ChangeInsuranceCover" TagPrefix="uc2" %>


<%@ Register Src="../UserControls/Reporting/Insurer/ChangeInsuranceValue.ascx" TagName="ChangeInsuranceValue" TagPrefix="uc3" %>


<%@ Register Src="../UserControls/Reporting/Insurer/AssetRemoval.ascx" TagName="AssetRemoval" TagPrefix="uc4" %>
<%@ Register Src="../UserControls/Reporting/Insurer/Notifications.ascx" TagName="Notifications" TagPrefix="uc5" %>

<asp:content id="BodyContent" contentplaceholderid="MainContent" runat="server">
    <div class="container-fluid">
        <!-- Page Heading -->
        <h1 class="h3 mb-2 text-gray-800">Monthly event reporting</h1>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Select event type</label>
                <asp:DropDownList CssClass="form-control" ID="ddlReport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Insurance premium non-payment</asp:ListItem>
                    <asp:ListItem Value="2">Change of cover</asp:ListItem>
                    <asp:ListItem Value="3">Insurance value updates</asp:ListItem>
                    <asp:ListItem Value="5">Insurance policy cancelled</asp:ListItem>
                    <%--<asp:ListItem Value="6">Change of address</asp:ListItem>--%>
                    <asp:ListItem Value="7">Notifications</asp:ListItem>
                </asp:DropDownList>

            </div>
        </div>



        <asp:Panel ID="pnlNonpayment" runat="server" Visible="false">
            <uc1:NonPayment ID="NonPayment" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlChangeInsuranceCover" runat="server" Visible="false">
            <uc2:ChangeInsuranceCover ID="ChangeInsuranceCover" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlChangeInsuranceValue" runat="server" Visible="false">
            <uc3:ChangeInsuranceValue ID="ChangeInsuranceValue" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlAssetRemoval" runat="server" Visible="false">
            <uc4:AssetRemoval ID="AssetRemoval" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlCustomerNotifications" runat="server"  Visible="false">
            <uc5:Notifications ID="Notifications" runat="server" />
        </asp:Panel>
    </div>
</asp:content>

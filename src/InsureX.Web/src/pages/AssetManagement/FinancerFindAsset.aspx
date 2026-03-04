<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Financer.Master" CodeBehind="FinancerFindAsset.aspx.cs" Inherits="IAPR_Web.AssetManagement.FinancerFindAsset" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
         <h1 class="h3 mb-2 text-gray-800">Asset details</h1>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Asset Type</label>
                <asp:DropDownList CssClass="form-control" ID="ddlAsset_Type" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Finance agreement number</label>
                <asp:TextBox CssClass="form-control mb-2" ID="txtFinanceNumber" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="txtFinanceNumber" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
            </div>

        </div>

        <asp:Button CssClass="btn btn-primary" ID="btnFindAsset" ValidationGroup="vgPolicy" runat="server" Text="Continue" OnClick="btnFindAsset_Click" />


        <asp:Panel ID="pnlAllDetails" runat="server" Visible="false">

            <h6 class="m-0 font-weight-bold text-primary">Customer details</h6>
            <div class="form-row align-items-center">
                <div class="form-group col-md-12">
                    <div id="divCustomerDeatils" runat="server" class="detailsDiv">
                    </div>
                </div>
            </div>
            <h6 class="m-0 font-weight-bold text-primary">Customer address</h6>
            <div class="form-row align-items-center">
                <div class="form-group col-md-12">
                    <div id="divPhysicalAddress" runat="server" class="detailsDiv">
                    </div>
                </div>

                <div class="form-group col-md-12">
                    <div id="divPostalAddress" runat="server" class="detailsDiv">
                    </div>
                </div>
            </div>

            <h6 class="m-0 font-weight-bold text-primary">Policy details</h6>
            <div class="form-row align-items-center">
                <div class="form-group col-md-12">
                    <div id="divPolicyDetails" runat="server" class="detailsDiv">
                    </div>
                </div>
            </div>
            <h6 class="m-0 font-weight-bold text-primary">Asset details</h6>
            <div class="form-row align-items-center">
                <div class="form-group col-md-12">

                    <div id="divAssetDetails" runat="server" class="detailsDiv">
                    </div>
                </div>
            </div>

        </asp:Panel>
    </div>
</asp:Content>

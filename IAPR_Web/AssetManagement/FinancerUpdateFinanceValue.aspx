<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Financer.Master" CodeBehind="FinancerUpdateFinanceValue.aspx.cs" Inherits="IAPR_Web.AssetManagement.FinancerUpdateFinanceValue" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdAssetId" runat="server" />
    <asp:HiddenField ID="hdAssetType" runat="server" />
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Update finance value</h1>
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
        <asp:Panel ID="pnlChangeFinanceValueDetails" runat="server" Visible="false">
            <h6 class="m-0 font-weight-bold text-primary">Update finance value</h6>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">New finance value</label>
                    <div class="input-group mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text">R</div>
                        </div>
                        <asp:TextBox CssClass="form-control" ID="txtNewFinanceValue" runat="server" data-type="currency"></asp:TextBox>
                    </div>
                    
                    <asp:RequiredFieldValidator ControlToValidate="txtNewFinanceValue" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeFinanceValue_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Date of change</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtDateOfChangeFinanceValue" runat="server" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtDateOfChangeFinanceValue" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeFinanceValue_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator26" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <asp:RegularExpressionValidator ID="revDecimals" runat="server" ValidationGroup="vgChangeCover_details" ErrorMessage="Enter correct format value e.g. 100000,50" ControlToValidate="txtNewFinanceValue" ValidationExpression="^\d{1,3}(,\d{3})*(\.\d+)?$"></asp:RegularExpressionValidator>
                    </div>
            <div class="form-group col-md-6">
                    </div>
            </div>
            <asp:Button CssClass="btn btn-primary" ID="btnSaveChangeFinanceValue" ValidationGroup="vgChangeFinanceValue_details" runat="server" Text="Update cover" OnClick="btnSaveChangeFinanceValue_Click" />
            <asp:Button CssClass="btn btn-warning" ID="btnCancelChangeFinanceValue" runat="server" Text="Cancel" OnClick="btnCancelChangeFinanceValue_Click" />
        </asp:Panel>

    </div>
</asp:Content>


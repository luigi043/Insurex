<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminBillingUpdateCharge.aspx.cs" Inherits="IAPR_Web.Billing.AdminBillingUpdateCharge" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Update charge fee</h1>
        <asp:Panel ID="pnlStep1" runat="server">
            <div class="form-div-border">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Select charge type</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlCharge_Type" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlCharge_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgSelectCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>


                </div>

                <asp:Button CssClass="btn btn-primary" ID="btnFind_Charge" ValidationGroup="vgSelectCharge" runat="server" Text="Continue" OnClick="btnFind_Charge_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlStep2" runat="server" Visible="false">
            <div class="form-div-border">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-12">
                        <h6 class="m-0 font-weight-bold text-primary">Charge current details</h6>
                        <div id="divPartnerChargeDetails" runat="server" class="detailsDiv">
                        </div>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">New charge amount</label>
                        <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text">R</div>
                            </div>
                            <asp:TextBox CssClass="form-control" ID="txtChargeAmount" runat="server" data-type="currency"></asp:TextBox>
                        </div>
                        <asp:RegularExpressionValidator ID="revDecimals" runat="server" ValidationGroup="vgPolicy" ErrorMessage="Enter correct format value e.g. 100000,50" ControlToValidate="txtChargeAmount" ValidationExpression="^\d{1,3}(,\d{3})*(\.\d+)?$"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ControlToValidate="txtChargeAmount" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Start date</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtCharge_Start_Date" runat="server" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtCharge_Start_Date" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator25" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">End date</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtCharge_End_Date" runat="server" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtCharge_End_Date" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <asp:Button CssClass="btn btn-primary" ID="btnSaveUpdateCharge" ValidationGroup="vgAddCharge" runat="server" Text="Save" OnClick="btnSaveUpdateCharge_Click" />
                <asp:Button CssClass="btn btn-warning" ID="btnCancelUpdateCharge" runat="server" Text="Cancel" OnClick="btnCancelUpdateCharge_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminBillingNewCharge.aspx.cs" Inherits="IAPR_Web.Billing.AdminBillingNewCharge" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">New charge</h1>
        <div class="form-div-border">
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Charge title</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtChargeTitle" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtChargeTitle" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator7" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Charge full description</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtDescription" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtDescription" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
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
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Charge amount</label>
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
                <div class="form-group col-md-12">
                    <label class="txtFieldLabel">Is this a monthly charge?</label>
                    <asp:RadioButtonList ID="rblMonthlyCharge" class="rbListWrap" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ControlToValidate="rblMonthlyCharge" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator23" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Which partner type is the charge applicable to?</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlPartnerType" runat="server" AutoPostBack="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlPartnerType" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Which partner package is the charge applicable to?</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlPartnerPackage" runat="server" AutoPostBack="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlPartnerPackage" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddCharge" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                </div>
            </div>
            <asp:Button CssClass="btn btn-primary" ID="btnAddNewCharge" ValidationGroup="vgAddCharge" runat="server" Text="Add new charge" OnClick="btnAddNewCharge_Click" />

            <asp:Button CssClass="btn btn-warning" ID="btnCancelCreatePolicy" runat="server" Text="Cancel" />
        </div>

    </div>
</asp:Content>

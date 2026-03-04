<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NonPayment.ascx.cs" Inherits="IAPR_Web.UserControls.PolicyTransactions.NonPayment" %>

<div class="sectionContainer">
    <div class="row">
        <div class="col-md-12">
            <h2 class="fs-title text-center">Non-payment details</h2>
            <section class="col-md-offset-2">

                <div class="col-md-6">
                    <label class="txtFieldLabel">Affected period</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlAffectedPeriod" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlAffectedPeriod" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgNon_payment_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-6">
                    <label class="txtFieldLabel">Affected Year</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlAffectedYear" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlAffectedYear" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgNon_payment_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </section>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">

            <section class="col-md-offset-2">
                <div class="col-md-6">
                    <asp:Button CssClass="btn btn-primary" ID="btnSaveNonPaymnet" ValidationGroup="vgNon_payment_details" runat="server" Text="Update status" OnClick="btnSaveNonPaymnet_Click" />

                </div>
            </section>
        </div>
    </div>
</div>

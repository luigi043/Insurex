<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminViewPartnerInvoice.aspx.cs" Inherits="IAPR_Web.Billing.AdminViewPartnerInvoice" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Invoicing</h1>

        <asp:HiddenField ID="hdPartnerID" runat="server" />
        <asp:HiddenField ID="hdPartnerTypeId" runat="server" />
        <asp:Panel ID="pnlStep1" runat="server">
            <div class="form-div-border">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Select partner type</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlPartnerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPartnerType_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlPartnerType" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgInvoice" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Select partner</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlPartners" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlPartners" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgInvoice" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                    </div>


                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Select month</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlInvoiceMonth" runat="server" AutoPostBack="true"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlInvoiceMonth" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgInvoice" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Select year</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlInvoiceYear" runat="server" AutoPostBack="true"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlInvoiceYear" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgInvoice" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                    </div>
                </div>
                <asp:Button CssClass="btn btn-primary" ID="btnContinue" ValidationGroup="vgInvoice" runat="server" Text="View invoice" OnClick="btnContinue_Click" />
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlStep2" runat="server" Visible="false">
            <div class="form-div-border">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-3">
                        <label class="txtFieldLabel">Parner name</label>
                        <asp:Label CssClass="form-control" ID="lblPartnerName" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="form-group col-md-3">
                        <label class="txtFieldLabel">Parner type</label>
                        <asp:Label CssClass="form-control" ID="lblPartnerType" runat="server" Text=""></asp:Label>
                    </div>

                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-3">
                        <label class="txtFieldLabel">Invoice number</label>
                        <asp:Label CssClass="form-control" ID="lblInvoiceNumber" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="form-group col-md-3">
                        <label class="txtFieldLabel">Invoice status</label>
                        <asp:Label CssClass="form-control" ID="lblInvoiceStatus" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-3">
                        <label class="txtFieldLabel">Invoice month</label>
                        <asp:Label CssClass="form-control" ID="lblInvoicingMonth" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="form-group col-md-3">
                        <label class="txtFieldLabel">Invoice year</label>
                        <asp:Label CssClass="form-control" ID="lblInvoiceYear" runat="server" Text=""></asp:Label>
                    </div>
                </div>

                <div class="form-row align-items-center">
                    <div class="form-group col-md-3">
                        <label class="txtFieldLabel">Invoice total</label>
                        <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text">R</div>
                            </div>
                            <asp:Label CssClass="form-control" ID="lblInvoiceTotalFee" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="form-group col-md-3">
                        <label class="txtFieldLabel">Total number of items</label>
                        <asp:Label CssClass="form-control" ID="lblNumberOfTransactions" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="card shadow mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-bordered" id="dataTable1">

                                <asp:Repeater ID="rptTransactionTypeTotals" runat="server">
                                    <HeaderTemplate>
                                        <div class="table-responsive">
                                            <thead>
                                                <tr>

                                                    <th>
                                                        <asp:Label ID="Label1" runat="server" Text="Item description"></asp:Label>
                                                    </th>
                                                    <th>
                                                        <asp:Label ID="Label2" runat="server" Text="Number of items"></asp:Label>
                                                    </th>
                                                    <th>
                                                        <asp:Label ID="Label12" runat="server" Text="Charge/Fee"></asp:Label>
                                                    </th>


                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblTransactionType" Text='<%# Eval("TransactionChargeType") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblNumberOfTransactions" Text='<%# Eval("InvoiceransactionCount") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblTotalCharge" Text='<%# "R" + Eval("InvoiceTotalFee") %>' />

                                                </td>




                                            </tr>
                                        </tbody>
                                    </ItemTemplate>

                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </div>

                <asp:Button CssClass="btn btn-primary" ID="btnDownLoadTransactions" ValidationGroup="vgAddCharge" runat="server" Text="Download transactions" OnClick="btnDownLoadTransactions_Click" />
                <asp:Button CssClass="btn btn-warning" ID="btnCancelUpdateCharge" runat="server" Text="Cancel" OnClick="btnCancelUpdateCharge_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>

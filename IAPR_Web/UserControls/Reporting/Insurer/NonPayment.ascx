<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NonPayment.ascx.cs" Inherits="IAPR_Web.UserControls.Reporting.Insurer.NonPayment" %>




<h6 class="m-0 font-weight-bold text-primary">Policy non-payment report</h6>
<asp:Panel ID="pnlPartner" runat="server" Visible="false">
    <div class="form-row align-items-center">
        <div class="form-group col-md-6">
            <label class="txtFieldLabel">Select partner</label>
            <asp:DropDownList CssClass="form-control" ID="ddlPartner" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator ControlToValidate="ddlPartner" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgMonthlyReport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

        </div>
    </div>
</asp:Panel>

<div class="form-row align-items-center">
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Select month</label>
        <asp:DropDownList CssClass="form-control" ID="ddlPeriod" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ControlToValidate="ddlPeriod" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgMonthlyReport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Select year</label>
        <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ControlToValidate="ddlYear" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgMonthlyReport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

    </div>
</div>
<asp:Button CssClass="btn btn-primary" ID="btnShowMonthlyNonPayment" ValidationGroup="vgMonthlyReport" runat="server" Text="View" OnClick="btnShowMonthlyNonPayment_Click" />


<asp:Panel ID="pnlNonPaymnet" runat="server" Visible="false">
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Non-payment for the period
                <asp:Label ID="lblPeriod" runat="server" Text=""></asp:Label>
            </h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table table-bordered" id="dataTable">
                    <asp:Repeater ID="rptNonPayment" runat="server" OnItemCommand="rptNonPayment_ItemCommand">
                        <HeaderTemplate>

                            <thead>
                                <tr>

                                    <th>
                                        <asp:Label ID="lbl2" runat="server" Text="Finance Agreement Number"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="lbl3" runat="server" Text="Insurance Company"></asp:Label>
                                    </th>

                                    <th>
                                        <asp:Label ID="lbl4" runat="server" Text="Insurance Policy Number"></asp:Label>
                                    </th>



                                    <th>
                                        <asp:Label ID="lbl8" runat="server" Text="Asset Type"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="lbl9" runat="server" Text="Asset Sub-Type"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="lbl10" runat="server" Text="Asset Description"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="lbl7" runat="server" Text="Finance Value"></asp:Label>
                                        <%--<asp:LinkButton ID='lnkEmployeeid' Runat='server' CommandName="Finance Value" OnClick='SortItems'>Finance Value</asp:LinkButton>--%>
                                    </th>
                                    <th>
                                        <asp:Label ID="lbl11" runat="server" Text="Date Submitted"></asp:Label>
                                    </th>

                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tbody>
                                <tr>

                                    <td>
                                        <asp:Label runat="server" ID="lblv2" Text='<%# Eval("Finance Agrreement Number") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblv3" Text='<%# Eval("Insurer Company") %>' />

                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblv4" Text='<%# Eval("Insurance Policy Number") %>' />

                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblv8" Text='<%# Eval("Asset Type") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblv9" Text='<%# Eval("Asset Sub-Type") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblv10" Text='<%# Eval("Asset Description") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Label1" Text='<%#Eval("Finance Value","{0:C}") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblv11" Text='<%# Eval("Date Submitted") %>' />

                                    </td>
                                    <%--<td>
                                                            <asp:LinkButton ID="lnkView" CssClass="linkButtonClass" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") + ";" + DataBinder.Eval(Container.DataItem, "iVehicle_Asset_Id")%>' Text="Update asset cover"
                                                                CommandName="VehicleCoverUpdate" runat="server"></asp:LinkButton>
                                                        </td>--%>
                                </tr>
                            </tbody>
                        </ItemTemplate>

                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <asp:Button CssClass="btn btn-primary" ID="btnSendReport" ValidationGroup="vgMonthlyReport" Visible="false" runat="server" Text="Download Report" OnClick="btnSendReport_Click" />
</asp:Panel>


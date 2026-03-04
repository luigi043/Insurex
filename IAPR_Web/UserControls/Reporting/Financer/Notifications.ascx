<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Notifications.ascx.cs" Inherits="IAPR_Web.UserControls.Reporting.Financer.Notifications" %>

<asp:HiddenField ID="hdPartnerID" runat="server" />
<asp:HiddenField ID="hdAssetTypeFilter" runat="server" />

<div class="form-div-border">
    <h6 class="m-0 font-weight-bold text-primary">Notifications</h6>
    <asp:Panel ID="pnlPartner" runat="server" Visible="false">
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Select partner</label>
                <asp:DropDownList CssClass="form-control" ID="ddlPartner" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPartner_SelectedIndexChanged"></asp:DropDownList>
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
            <asp:RequiredFieldValidator ControlToValidate="ddlYear" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgMonthlyReport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

        </div>
    </div>
    <div class="form-row align-items-center">
        <div class="form-group col-md-1">
            <asp:Button CssClass="btn btn-primary" ID="btnShowCustomerNotifications" ValidationGroup="vgMonthlyReport" runat="server" Text="View" OnClick="btnShowCustomerNotifications_Click" />
        </div>
    </div>
</div>
<asp:Panel ID="pnlCustomerNotifications" runat="server" Visible="false">
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Customer notifications for
                <asp:Label ID="lblPeriod" runat="server" Text=""></asp:Label>
            </h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table table-bordered" id="dataTable">
                    <thead>
                        <tr>

                            <%-- <th>
                                    <asp:Label ID="Label4" runat="server" Text="Finance Agreement Number"></asp:Label>
                                </th>--%>
                            <th>
                                <asp:Label ID="Label3" runat="server" Text="Insurer"></asp:Label>
                            </th>

                            <th>
                                <asp:Label ID="Label14" runat="server" Text="Policy Number"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label15" runat="server" Text="Asset Type"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label16" runat="server" Text="Asset Detail"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label17" runat="server" Text="Finance Value"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label18" runat="server" Text="Notification Date"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label19" runat="server" Text="Notification Method"></asp:Label>

                            </th>
                            <th>
                                <asp:Label ID="Label20" runat="server" Text="Notification Reason"></asp:Label>
                            </th>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>

                            <%-- <th>
                                    <asp:Label ID="Label4" runat="server" Text="Finance Agreement Number"></asp:Label>
                                </th>--%>
                            <th>
                                <asp:Label ID="Label4" runat="server" Text="Insurer"></asp:Label>
                            </th>

                            <th>
                                <asp:Label ID="Label5" runat="server" Text="Policy Number"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label6" runat="server" Text="Asset Type"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label7" runat="server" Text="Asset Detail"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label8" runat="server" Text="Finance Value"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label9" runat="server" Text="Notification Date"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="Label10" runat="server" Text="Notification Method"></asp:Label>

                            </th>
                            <th>
                                <asp:Label ID="Label11" runat="server" Text="Notification Reason"></asp:Label>
                            </th>
                        </tr>
                    </tfoot>
                    <tbody>
                        <asp:Repeater ID="rptCustomerNotifications" runat="server">

                            <ItemTemplate>

                                <tr>

                                    <%-- <td>
                                            <asp:Label runat="server" ID="lblv2" Text='<%# Eval("vcFinance_Agrreement_Number") %>' />

                                        </td>--%>
                                    <td>
                                        <asp:Label runat="server" ID="lblv3" Text='<%# Eval("Insurer") %>' />

                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblv4" Text='<%# Eval("Policy Number") %>' />

                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblv8" Text='<%# Eval("Asset Type") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Label2" Text='<%# Eval("Asset Detail") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Label1" Text='<%# Eval("Finance Value","{0:C}") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblv11" Text='<%# Convert.ToDateTime(Eval("Notification Date")).ToString("dd/MM/yyyy")  %>' />


                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Label12" Text='<%# Eval("Notification Method") %>' />

                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Label13" Text='<%# Eval("Notification Reason") %>' />

                                    </td>
                                </tr>

                            </ItemTemplate>

                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <asp:Button CssClass="btn btn-primary" ID="btnSendReport" ValidationGroup="vgMonthlyReport" Visible="false" runat="server" Text="Download Report" OnClick="btnSendReport_Click" />
</asp:Panel>




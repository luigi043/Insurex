<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReinstatedCover.ascx.cs" Inherits="IAPR_Web.UserControls.Reporting.Financer.ReinstatedCover" %>

<div class="container-fluid">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="form-div-border">
                <h6 class="m-0 font-weight-bold text-primary">Policy premiums unpaid </h6>
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

            </div>
            <asp:Panel ID="pnlNonPaymnet" runat="server" Visible="false">
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">Re-instated cover
                <asp:Label ID="lblPeriod" runat="server" Text=""></asp:Label>
                        </h6>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-bordered" id="dataTable">
                                <thead>
                                    <tr>

                                        <th>
                                            <asp:Label ID="lbl2" runat="server" Text="Finance Agreement Number"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="lbl3" runat="server" Text="Insurer"></asp:Label>
                                        </th>

                                        <th>
                                            <asp:Label ID="lbl4" runat="server" Text="Policy Number"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="lbl8" runat="server" Text="Asset"></asp:Label>
                                        </th>

                                        <th>
                                            <asp:Label ID="lbl7" runat="server" Text="Finance Value"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label3" runat="server" Text="Insured Value"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="lbl11" runat="server" Text="Date Uninsured"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label11" runat="server" Text="Date Re-instated"></asp:Label>
                                        </th>
                                        <th>Days to re-instate</th>
                                    </tr>
                                </thead>
                                <%-- <tfoot>
                            <tr>

                                <th>
                                    <asp:Label ID="Label4" runat="server" Text="Finance Agreement Number"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="Label5" runat="server" Text="Insurer Company"></asp:Label>
                                </th>

                                <th>
                                    <asp:Label ID="Label6" runat="server" Text="Insurance Policy Number"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="Label7" runat="server" Text="Asset"></asp:Label>
                                </th>
                                
                                <th>
                                    <asp:Label ID="Label3" runat="server" Text="Date uninsured"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="Label8" runat="server" Text="Date reinstated"></asp:Label>
                                </th>
                                <th>Days to re-instate</th>
                            </tr>
                        </tfoot>--%>
                                <tbody>
                                    <asp:Repeater ID="rptUReinstatedCover" runat="server" OnItemCommand="rptUReinstatedCover_ItemCommand">

                                        <ItemTemplate>

                                            <tr>

                                                <td>
                                                    <asp:Label runat="server" ID="lblv2" Text='<%# Eval("vcFinance_Agrreement_Number") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblv3" Text='<%# Eval("vcInsurance_Company_Name") %>' />

                                                </td>

                                                <td>
                                                    <asp:Label runat="server" ID="lblv4" Text='<%# Eval("vcPolicy_Number") %>' />

                                                </td>

                                                <td>
                                                    <asp:Label runat="server" ID="lblv8" Text='<%# Eval("vcAsset_Type_Description") + ": " + Eval("vcAsset_SubType_Description") %>' />

                                                </td>
                                                <%--<td>
                                            <asp:Label runat="server" ID="Label2" Text='<%# Eval("vcAsset_SubType_Description") %>' />

                                        </td>--%>
                                                <td>
                                                    <asp:Label runat="server" ID="Label1" Text='<%# Eval("mAsset_Finance_Value","{0:C}") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label4" Text='<%# Eval("mAsset_Insured_Value","{0:C}") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblv11" Text='<%# Convert.ToDateTime(Eval("dtDate_since_Unisured")).ToString("dd/MM/yyyy")  %>' />


                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label2" Text='<%# Convert.ToDateTime(Eval("dtDate_Rectified")).ToString("dd/MM/yyyy")  %>' />


                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label9" Text='<%# Eval("iDays_Difference") %>' />

                                                </td>


                                            </tr>

                                        </ItemTemplate>

                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            </asp:Panel>
            <asp:Button CssClass="btn btn-primary" ID="btnSendReport" ValidationGroup="vgMonthlyReport" Visible="false" runat="server" Text="Download Report" OnClick="btnSendReport_Click" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSendReport" />
        </Triggers>
    </asp:UpdatePanel>
</div>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllAssets.ascx.cs" Inherits="IAPR_Web.UserControls.Reporting.Financer.AllAssets" %>

<asp:HiddenField ID="hdPartnerID" runat="server" />
<asp:HiddenField ID="hdAssetTypeFilter" runat="server" />
<div class="container-fluid">
    <!-- Page Heading -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h1 class="h3 mb-2 text-gray-800">Assets list</h1>
            <asp:Panel ID="pnlPartner" runat="server" Visible="false">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Select partner</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlPartner" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPartner_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlPartner" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgMonthlyReport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                    </div>
                </div>
            </asp:Panel>


            <asp:Panel ID="pnlAssetType" runat="server" Visible="false">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Select asset type</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlAsset_type" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAsset_type_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlAsset_type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgMonthlyReport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlUninsuredAssets" runat="server" Visible="false">
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">Assets list
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
                                            <asp:Label ID="lbl3" runat="server" Text="Insurer Company"></asp:Label>
                                        </th>

                                        <th>
                                            <asp:Label ID="lbl4" runat="server" Text="Insurance Policy Number"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="lbl8" runat="server" Text="Asset Type"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label3" runat="server" Text="Asset Sub-Type"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="lbl7" runat="server" Text="Finance Value"></asp:Label>
                                        </th>
                                        <%--<th>
                                    <asp:Label ID="lbl11" runat="server" Text="Date since uninsured"></asp:Label>
                                </th>--%>
                                        <th>View</th>
                                    </tr>
                                </thead>
                                <tfoot>
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
                                            <asp:Label ID="Label7" runat="server" Text="Asset Type"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label8" runat="server" Text="Asset Sub-Type"></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label9" runat="server" Text="Finance Value"></asp:Label>
                                        </th>
                                        <%-- <th>
                                    <asp:Label ID="Label10" runat="server" Text="Date since uninsured"></asp:Label>
                                </th>--%>
                                        <th>View</th>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <asp:Repeater ID="rptUninsuredAssets" runat="server" OnItemCommand="rptUninsuredAssets_ItemCommand">

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
                                                    <asp:Label runat="server" ID="lblv8" Text='<%# Eval("vcAsset_Type_Description") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label2" Text='<%# Eval("vcAsset_SubType_Description") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label1" Text='<%# Eval("mAsset_Finance_Value") %>' />

                                                </td>
                                                <%-- <td>
                                            <asp:Label runat="server" ID="lblv11" Text='<%# Convert.ToDateTime(Eval("dtDate_since_Unisured")).ToString("dd/MM/yyyy")  %>' />
                                            <%--Eval("dtDate_since_Unisured")

                                        </td>--%>
                                                <td><%--<a href="/Reporting/UninsuredAssets.aspx"><i class="fa fa-eye"></i></a>--%>
                                                    <asp:LinkButton ID="lnkView" CssClass="fa fa-eye" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iAsset_Id")+ ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id") %>' title="View policy and asset details"
                                                        CommandName="ViewAssetDetails" runat="server"></asp:LinkButton>

                                                </td>
                                            </tr>

                                        </ItemTemplate>

                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <asp:Button CssClass="btn btn-primary" ID="btnSendReport" ValidationGroup="vgMonthlyReport"  runat="server" Text="Download Report" OnClick="btnSendReport_Click" />
            </asp:Panel>

            <asp:Panel ID="pnlAllDetails" runat="server" Visible="false">
                <h6 class="m-0 font-weight-bold text-primary">Asset details</h6>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-12">

                        <div id="divAssetDetails" runat="server" class="detailsDiv">
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
                <h6 class="m-0 font-weight-bold text-primary">Policy holder details</h6>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-12">
                        <div id="divCustomerDeatils" runat="server" class="detailsDiv">
                        </div>
                    </div>
                </div>
                <h6 class="m-0 font-weight-bold text-primary">Policy holders address</h6>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-12">
                        <div id="divPhysicalAddress" runat="server" class="detailsDiv">
                        </div>
                    </div>
                </div>
                <h6 class="m-0 font-weight-bold text-primary">Policy holders postal address</h6>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-12">
                        <div id="divPostalAddress" runat="server" class="detailsDiv">
                        </div>
                    </div>
                </div>


                <asp:Button CssClass="btn btn-primary" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />

            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSendReport" />
        </Triggers>
    </asp:UpdatePanel>
</div>

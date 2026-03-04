<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetRemoval.ascx.cs" Inherits="IAPR_Web.UserControls.Reporting.Insurer.AssetRemoval" %>

<asp:Panel ID="pnlPartner" runat="server" Visible="false">
    <div class="form-row align-items-center">
        <div class="form-group col-md-6">
            <label class="txtFieldLabel">Select partner</label>
            <asp:DropDownList CssClass="form-control" ID="ddlPartner" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator ControlToValidate="ddlPartner" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeOfCover" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

        </div>
    </div>
</asp:Panel>

<div class="form-row align-items-center">
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Select month</label>
        <asp:DropDownList CssClass="form-control" ID="ddlPeriod" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ControlToValidate="ddlPeriod" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeOfCover" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Select year</label>
        <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ControlToValidate="ddlYear" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeOfCover" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

    </div>
</div>
<asp:Button CssClass="btn btn-primary" ID="btnShowAssetRemoval" ValidationGroup="vgChangeOfCover" runat="server" Text="View" OnClick="btnShowAssetRemoval_Click" />


<asp:Panel ID="pnlAssetRemoval" runat="server" Visible="false">
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Change of cover
                <asp:Label ID="lblPeriod" runat="server" Text=""></asp:Label>
            </h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table table-bordered" id="dataTable">
                    <asp:Repeater ID="rptAssetRemoval" runat="server" OnItemCommand="rptAssetRemoval_ItemCommand">
                        <HeaderTemplate>

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
                                        <asp:Label ID="lbl9" runat="server" Text="Asset Sub-Type"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="lbl10" runat="server" Text="Asset Description"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="lbl11" runat="server" Text="Date Submitted"></asp:Label>
                                    </th>
                                    <th>View</th>
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
                                        <asp:Label runat="server" ID="lblv11" Text='<%#  Convert.ToDateTime(Eval("dtDateOfRemoval")).ToString("dd/MM/yyyy") %>' />
                                        <%--Eval("dtDateOfRemoval", "{0:dd/MM/yy}")--%>

                                    </td>
                                    <td class="action-td text-center">
                                        <asp:LinkButton ID="ImageButton1" title="View asset details" runat="server" CommandName="ViewHistory" CssClass="fa fa-eye mr-2" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iAsset_ID") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")%>' />
                                    </td>
                                </tr>
                            </tbody>
                        </ItemTemplate>

                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <asp:Button CssClass="btn btn-primary" ID="btnSendReport" ValidationGroup="vgChangeOfCover" Visible="false" runat="server" Text="Download Report" OnClick="btnSendReport_Click" />

</asp:Panel>

<asp:Panel ID="pnlAllDetails" runat="server" Visible="false">
    <div class="panelSeparator"></div>
    <h6 class="m-0 font-weight-bold text-primary">Asset details</h6>
    <div class="form-row align-items-center">
        <div class="form-group col-md-12">

            <div id="divAssetDetails" runat="server" class="detailsDiv">
            </div>
        </div>
    </div>
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



    <asp:Button CssClass="btn btn-primary" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />

</asp:Panel>

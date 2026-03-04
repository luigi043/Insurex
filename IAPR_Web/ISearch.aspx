<%@ Page Language="C#" MasterPageFile="~/Insurer.Master" AutoEventWireup="true" CodeBehind="ISearch.aspx.cs" Inherits="IAPR_Web.ISearch" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <asp:Panel ID="pnlPolicyList" runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Search results
                </h6>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-bordered" id="dataTable">
                            <asp:Repeater ID="rptPolicies" runat="server" OnItemCommand="rptPolicies_ItemCommand" OnItemDataBound="rptPolicies_ItemDataBound">
                                <HeaderTemplate>
                                    <div class="table-responsive">
                                        <thead>
                                            <tr>
                                                <th>
                                                    <asp:Label ID="lblCover" runat="server" Text="Policy number"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="Label12" runat="server" Text="Policy type"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblUninsuredAssetshd" runat="server" Text="Policy status"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="Label2" runat="server" Text="Sum Finance value"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="Label1" runat="server" Text="Sum Insurance value"></asp:Label>
                                                </th>

                                                <th>
                                                    <asp:Label ID="lblNumberOfAssetsh" runat="server" Text="Number of assets"></asp:Label>
                                                </th>
                                                <th></th>
                                                <%-- <th>
                                                <asp:Label ID="lblTotalNumberOfAssetshd" runat="server" Text="Date since status"></asp:Label>
                                            </th>--%>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tbody>
                                        <tr>

                                            <td>
                                                <asp:Label runat="server" ID="lblPolicy_Number" Text='<%# Eval("Policy Number") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPolicy_Type_Description" Text='<%# Eval("vcPolicy_Type_Description") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPolicy_Status_Description" Text='<%# Eval("vcPolicy_Status_Description") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblSumFinance" Text="" />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblSumInsurance" Text="" />

                                            </td>

                                            <td>
                                                <asp:Label runat="server" ID="lblNumberOfAssets" Text="" />

                                            </td>

                                            <td>

                                                <asp:LinkButton ID="lnkView" CssClass="fa fa-eye" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") %>' title="View all assets"
                                                    CommandName="ViewPolicies" runat="server"></asp:LinkButton>


                                            </td>

                                        </tr>
                                    </tbody>
                                </ItemTemplate>

                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlAssetList" runat="server" Visible="false">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Policy assets
                    </h6>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-bordered" id="dataTable2">
                            <asp:Repeater ID="rptAssetList" runat="server">
                                <HeaderTemplate>

                                    <thead>
                                        <tr>

                                            <th>
                                                <asp:Label ID="Label1" runat="server" Text="Financer"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label ID="lblCover" runat="server" Text="Asset Cover"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label ID="Label12" runat="server" Text="Asset type"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label ID="lblVehicle_Asset_Type_Description" runat="server" Text="Vehicle sub-type"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label ID="Label8" runat="server" Text="Make & Model"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label ID="Label4" runat="server" Text="Identifier/Description"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label ID="Label5" runat="server" Text="Insurance value"></asp:Label></th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblFinancer_Name" Text='<%# Eval("vcFinancer_Name") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label2" Text='<%# Eval("vcAsset_Cover") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblAsset_Type_Description" Text='<%# Eval("vcAsset_Type_Description") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblVehicle_Asset_Type_Description" Text='<%# Eval("vcAsset_Sub_Type_Description") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblakeModel" Text='<%# Eval("vcMakeModel") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblVin_Number" Text='<%# Eval("vcIdentifier") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label3" Text='<%# Eval("Insurance_Value") %>' />

                                            </td>

                                           <%-- <td class="action-td text-center">
                                                <asp:LinkButton ID="ImageButton1" runat="server" CommandName="VehicleCoverUpdate" CssClass="fa fa-edit mr-2" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") 
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID")  
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Sub_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcIdentifier")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Finance_Value")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Insurance_Value")
                                                                                        %>' />

                                                </td>--%>
                                               
                                        </tr>
                                    </tbody>
                                </ItemTemplate>

                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
            <asp:Button CssClass="btn btn-primary" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </asp:Panel>
    </div>
</asp:Content>

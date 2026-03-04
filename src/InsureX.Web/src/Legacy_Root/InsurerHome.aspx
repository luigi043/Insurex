<%@ Page Language="C#" MasterPageFile="~/Insurer.Master" AutoEventWireup="true" CodeBehind="InsurerHome.aspx.cs" Inherits="IAPR_Web.InsurerHome" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="background-color: #eff1f9;">

        <asp:Panel ID="plInsurerTotals" runat="server">
            <div class="row">
                <div class="col-xl-3 col-md-6 mb-4">
                    <div id="divUninsured_Assest" runat="server" class="card border-left-danger shadow h-100 py-2 dashBackground">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        Uninsured assest
                       
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-dark-800">
                                        <asp:Label ID="lblUninsuredAssetsTotal" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6 mb-4">
                    <div id="divUninsured_Value" runat="server" class="card border-left-danger shadow h-100 py-2 dashBackground">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        Uninsured value
                       
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-dark-800">
                                        <asp:Label ID="lblUninsuredValue" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-primary shadow h-100 py-2 dashBackground">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        Total insured assets
                       
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-dark-800">
                                        <asp:Label ID="lblTotaAssets" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-primary shadow h-100 py-2 dashBackground">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        Total insurance value
                       
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-dark-800">
                                        <asp:Label ID="lblTotalFinanceValue" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <div id="divUninsured_Assets_Perc" runat="server" class="card border-left-danger shadow h-100 py-2 dashBackground">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        % Uninsured assets
                       
                                    </div>
                                    <div class="row no-gutters align-items-center">
                                        <div class="col-auto">
                                            <div
                                                class="h5 mb-0 font-weight-bold text-dark-800">
                                                <asp:Label ID="lblPercUninsuredAssets" runat="server" Text=""></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col">
                                            <div class="progress progress-sm mr-2">
                                                <div runat="server" id="divUninsuredAssetsPerc"
                                                    role="progressbar"
                                                    aria-valuenow="50"
                                                    aria-valuemin="0"
                                                    aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-6">
                    <div id="divUninsured_Value_Perc" runat="server" class="card border-left-danger shadow h-100 py-2 dashBackground">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        % Uninsured value
                       
                                    </div>
                                    <div class="row no-gutters align-items-center">
                                        <div class="col-auto">
                                            <div
                                                class="h5 mb-0 font-weight-bold text-dark-800">
                                                <asp:Label ID="lblPercUninsuredValue" runat="server" Text=""></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col">
                                            <div class="progress progress-sm mr-2">
                                                <div runat="server" id="divUninsuredValuePerc"
                                                    role="progressbar"
                                                    aria-valuenow="50"
                                                    aria-valuemin="0"
                                                    aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card shadow mb-4" style="display: none;">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Assets totals
                </h6>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-bordered" id="dataTable1">
                            <asp:Repeater ID="rptInsurerTotals" runat="server" OnItemDataBound="rptInsurerTotals_ItemDataBound">
                                <HeaderTemplate>
                                    <div class="table-responsive">
                                        <thead>
                                            <tr>
                                                <th>
                                                    <asp:Label ID="lblNumberUninsuresAssetsHd" runat="server" Text="Number of uninsured assets"></asp:Label>
                                                </th>


                                                <th>
                                                    <asp:Label ID="lblTotaUninsuredValueHd" runat="server" Text="Total uninsured value"></asp:Label>
                                                </th>

                                                <th>
                                                    <asp:Label ID="lblNumberInsurerAssetsHd" runat="server" Text="Total number of assets"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblTotaInsurerValueHd" runat="server" Text="Total insurance assets"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblPercNumberUninsuredAssetsHd" runat="server" Text="% Uninsured assets"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblPercTotalInsuranceValueAssets" runat="server" Text="% Uninsured value"></asp:Label>
                                                </th>
                                                <%-- <th></th>--%>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tbody>
                                        <tr>

                                            <td>
                                                <asp:Label runat="server" ID="lblNumberUninsuresAssets" Text='<%# Eval("iNumber_Of_Assets") %>' />

                                            </td>

                                            <td>
                                                <asp:Label ID="lblTotaUninsuredValue" runat="server" Text='<%# Eval("dcInsurance_Value") %>'></asp:Label>

                                            </td>
                                            <td>
                                                <asp:Label ID="lblNumberInsurerAssets" runat="server" Text=""></asp:Label>

                                            </td>
                                            <td>
                                                <asp:Label ID="lblTotaInsurerValue" runat="server" Text=""></asp:Label>

                                            </td>
                                            <td>
                                                <asp:Label ID="lblPercNumberUninsuredAssets" runat="server" Text="0"></asp:Label>

                                            </td>
                                            <td>
                                                <asp:Label ID="lblPercTotalInsuranceValueAsset" runat="server" Text="0"></asp:Label>

                                            </td>
                                            <%--<td>
                                                <a href="/Reporting/UninsuredAssets.aspx"><i class="fa fa-eye"></i></a>
                                                 <asp:LinkButton ID="lnkView" CssClass="tableButton editButton" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPartner_Id") %>' ToolTip="View assets"
                                                                                    CommandName="VehicleCoverUpdate" runat="server"></asp:LinkButton>

                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="VehicleCoverUpdate" CssClass="editButton" ToolTip="Change asset cover" ImageUrl="~/Images/Edit.png" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID")  
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Sub_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcIdentifier")
                                                                                        %>' />
                                                <asp:Button CssClass="tableButton editButton" ID="btnSavePolicyStatus" runat="server"  ToolTip="Change asset cover" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")%>' />

                                                <asp:LinkButton ID="lnkView" CssClass="tableButton editButton" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")%>' ToolTip="Change asset cover"
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

        </asp:Panel>

        <asp:Panel ID="pnlCurrentlyUninsured" runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Non-Active policies
                </h6>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-bordered" id="dataTable">
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
                                        <asp:Label ID="Label1" runat="server" Text="Sum Insurance value"></asp:Label>
                                    </th>
                                    <%-- <th>
                                        <asp:Label ID="Label2" runat="server" Text="Sum Finance value"></asp:Label>
                                    </th>--%>
                                    <th>
                                        <asp:Label ID="lblNumberOfAssetsh" runat="server" Text="Number of assets"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="lblTotalNumberOfAssetshd" runat="server" Text="Date since status"></asp:Label>
                                    </th>
                                    <th></th>

                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <th>
                                        <asp:Label ID="Label6" runat="server" Text="Policy number"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="Label7" runat="server" Text="Policy type"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="Label9" runat="server" Text="Policy status"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="Label10" runat="server" Text="Sum Insurance value"></asp:Label>
                                    </th>
                                    <%-- <th>
                                        <asp:Label ID="Label11" runat="server" Text="Sum Finance value"></asp:Label>
                                    </th>--%>
                                    <th>
                                        <asp:Label ID="Label13" runat="server" Text="Number of assets"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="Label14" runat="server" Text="Date since status"></asp:Label>
                                    </th>
                                    <th></th>

                                </tr>
                            </tfoot>
                            <tbody>
                                <asp:Repeater ID="rptCurrentlyUninsured" runat="server" OnItemCommand="rptCurrentlyUninsured_ItemCommand" OnItemDataBound="rptCurrentlyUninsured_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>

                                            <td>
                                                <asp:Label runat="server" ID="lblPolicy_Number" Text='<%# Eval("vcPolicy_Number") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPolicy_Type_Description" Text='<%# Eval("vcPolicy_Type_Description") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPolicy_Status_Description" Text='<%# Eval("vcPolicy_Status_Description") %>' />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblSumInsurance" Text="" />

                                            </td>
                                            <%--<td>
                                                <asp:Label runat="server" ID="lblSumFinance" Text="" />

                                            </td>--%>
                                            <td>
                                                <asp:Label runat="server" ID="lblNumberOfAssets" Text="" />

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblStatusUpdated" Text='<%# Convert.ToDateTime(Eval("dtStatusUpdated")).ToString("dd/MM/yyyy") %>' />

                                            </td>

                                            <td>
                                                <%--<a href="/Reporting/UninsuredAssets.aspx"><i class="fa fa-eye"></i></a>--%>
                                                <asp:LinkButton ID="lnkView" CssClass="fa fa-eye" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") %>' title="View policy assets"
                                                    CommandName="ViewPolicies" runat="server"></asp:LinkButton>

                                                <%--<asp:ImageButton ID="ImageButton1" runat="server" CommandName="VehicleCoverUpdate" CssClass="editButton" ToolTip="Change asset cover" ImageUrl="~/Images/Edit.png" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID")  
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Sub_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcIdentifier")
                                                                                        %>' />--%>
                                                <%--<asp:Button CssClass="tableButton editButton" ID="btnSavePolicyStatus" runat="server"  ToolTip="Change asset cover" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")%>' />--%>

                                                <%--<asp:LinkButton ID="lnkView" CssClass="tableButton editButton" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID") + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")%>' ToolTip="Change asset cover"
                                                                                    CommandName="VehicleCoverUpdate" runat="server"></asp:LinkButton>--%>
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
        <asp:Panel ID="pnlAssetList" runat="server" Visible="false">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Policy assets
                    </h6>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-bordered" id="dataTable1">

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
                                                <asp:Label ID="Label5" runat="server" Text="Insurance value"></asp:Label>
                                            </th>

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

                                            <%--<td class="action-td text-center">--%>
                                            <%--<asp:LinkButton ID="ImageButton1" runat="server" CommandName="VehicleCoverUpdate" CssClass="fa fa-edit mr-2" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") 
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID")  
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Sub_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcIdentifier")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Finance_Value")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Insurance_Value")
                                                                                        %>' />--%>


                                            <%--  <asp:LinkButton ID="LinkButton1" title="Change insurred value" runat="server" CommandName="ChangeInsuranceValue" CssClass="fa fa-dollar-sign mr-2" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") 
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


                                                    <asp:LinkButton ID="ImageButton2" runat="server" CommandName="RemoveAssetFromPolicy" CssClass="fa fa-trash mr-2" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") 
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

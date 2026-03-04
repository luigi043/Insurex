<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddAssetToPolicy.aspx.cs" Inherits="IAPR_Web.PolicyManagement.AddAssetToPolicy" %>

<%@ Register Src="../UserControls/AssetTypes/AddVehicleAsset.ascx" TagName="AddVehicleAsset" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/AssetTypes/AddBuilding-PropertyAsset.ascx" TagName="AddBuildingAsset" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/AssetTypes/AddWatercraft.ascx" TagName="AddWatercraftAsset" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/AssetTypes/AddAviationAsset.ascx" TagName="AddAviationAsset" TagPrefix="uc4" %>
<%@ Register Src="../UserControls/AssetTypes/AddStockAsset.ascx" TagName="AddStockAsset" TagPrefix="uc5" %>

<%@ Register Src="../UserControls/AssetTypes/AddAccountReceivableAsset.ascx" TagName="AddAccountReceivableAsset" TagPrefix="uc6" %>
<%@ Register Src="../UserControls/AssetTypes/AddMachineryAsset.ascx" TagName="AddMachineryAsset" TagPrefix="uc7" %>
<%@ Register Src="../UserControls/AssetTypes/AddPlantEquipmentAsset.ascx" TagName="AddPlantEquipmentAsset" TagPrefix="uc8" %>
<%@ Register Src="../UserControls/AssetTypes/AddBusinessInterruptionAsset.ascx" TagName="AddBusinessInterruptionAsset" TagPrefix="uc9" %>
<%@ Register Src="../UserControls/AssetTypes/AddKeymanInsuranceAsset.ascx" TagName="AddKeymanInsuranceAsset" TagPrefix="uc10" %>
<%@ Register Src="../UserControls/AssetTypes/AddElectronicEquipmentAsset.ascx" TagName="AddElectronicEquipmentAsset" TagPrefix="uc11" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdPolicyId" runat="server" />
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Add asset</h1>
        <div class="form-div-border">
            <asp:Panel ID="pnlPolicyDetails" runat="server">
                <h6 class="m-0 font-weight-bold text-primary">Policy details</h6>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Insurance provider</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlInsuranceCompanies" runat="server" data-live-search="true"></asp:DropDownList>
                        <asp:RequiredFieldValidator Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server" ControlToValidate="ddlInsuranceCompanies"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group col-md-6">
                        <label for="inputEmail4">Insurance policy number</label>
                        <asp:TextBox CssClass="form-control" ID="txtPolicy_Number" runat="server"></asp:TextBox>
                        <asp:Literal ID="litPolicyNumber" runat="server"></asp:Literal>
                        <asp:RequiredFieldValidator Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server" ControlToValidate="txtPolicy_Number"></asp:RequiredFieldValidator>
                    </div>


                </div>


                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Asset Type</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlAsset_Type" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>


                </div>

                <asp:Button CssClass="btn btn-primary" ID="btnFind_Policy" ValidationGroup="vgPolicy" runat="server" Text="Find policy" OnClick="btnFind_Policy_Click" />




            </asp:Panel>
        </div>

        <asp:Panel ID="pnlAddAsset" runat="server" Visible="false">
            <div class="form-div-border">
                <asp:Panel ID="pnlAddVehicleAsset" runat="server" Visible="false">
                    <uc1:AddVehicleAsset ID="ucAddVehicleAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddBuildingProperty" runat="server" Visible="false">
                    <uc2:AddBuildingAsset ID="ucAddBuildingAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddWatercraftAsset" runat="server" Visible="false">
                    <uc3:AddWatercraftAsset ID="ucAddWatercraftAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddAviationAsset" runat="server" Visible="false">
                    <uc4:AddAviationAsset ID="ucAddAviationAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddStockAsset" runat="server" Visible="false">
                    <uc5:AddStockAsset ID="ucAddStockAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddAccountReceivableAsset" runat="server" Visible="false">
                    <uc6:AddAccountReceivableAsset ID="ucAddAccountReceivableAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddMachineryAsset" runat="server" Visible="false">
                    <uc7:AddMachineryAsset ID="ucAddMachineryAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddPlantEquipmentAsset" runat="server" Visible="false">
                    <uc8:AddPlantEquipmentAsset ID="ucAddPlantEquipmentAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddBusinessInterruptionAsset" runat="server" Visible="false">
                    <uc9:AddBusinessInterruptionAsset ID="ucAddBusinessInterruptionAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddKeymanInsuranceAsset" runat="server" Visible="false">
                    <uc10:AddKeymanInsuranceAsset ID="ucAddKeymanInsuranceAsset" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddElectronicEquipmentAsset" runat="server" Visible="false">
                    <uc11:AddElectronicEquipmentAsset ID="ucAddElectronicEquipmentAsset" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlSaveButtons" runat="server">

                    <asp:Button CssClass="btn btn-primary" ID="btnAddAssetToPolicy" ValidationGroup="vgPolicy" runat="server" Text="Add asset" OnClick="btnAddAssetToPolicy_Click" />

                    <asp:Button CssClass="btn btn-warning" ID="btnCancelCreatePolicy" runat="server" Text="Cancel" OnClick="btnCancelCreatePolicy_Click" />

                </asp:Panel>
                <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="successMessage">
                    <div class="row">
                        <div class="col-md-12">
                            <section class="col-md-offset-2">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        Asset has beed added successfully                         
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>
                </asp:Panel>

            </div>
        </asp:Panel>


    </div>

</asp:Content>

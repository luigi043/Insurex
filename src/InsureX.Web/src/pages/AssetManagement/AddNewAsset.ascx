<%@ Control Language="C#" MasterPageFile="~/Financer.Master"  AutoEventWireup="true" CodeBehind="AddNewAsset.ascx.cs" Inherits="IAPR_Web.AssetManagement.AddNewAsset" %>

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
    <div class="container-fluid" id="grad1">
        <div class="row justify-content-center mt-0">
            <div class="col-11 col-sm-12 col-md-12 col-lg-12 text-center p-0 mt-3 mb-2">
                <div class="card px-0 pt-4 pb-0 mt-3 mb-3">
                    <div class="col-md-12 mx-0">
                        <section id="msform">
                            <div class="form-card">

                                <asp:Panel ID="pnlStep1" runat="server">
                                    <div class="sectionContainer">
                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Customer type</label>
                                                        <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlPolicy_Type" runat="server"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ControlToValidate="ddlPolicy_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Asset Type</label>
                                                        <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlAsset_Type" runat="server"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </section>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <asp:Button CssClass="saveButton" ID="btnContinue1" ValidationGroup="vgPolicy" runat="server" Text="Continue" OnClick="btnContinue1_Click" />

                                                    </div>
                                                </section>
                                            </div>
                                        </div>


                                    </div>
                                </asp:Panel>
                            </div>

                            <asp:Panel ID="pnlStep2" runat="server" Visible="false" Enabled="false">
                                <div class="sectionContainer">
                                    <asp:Panel ID="pnlPersonalDetails" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h2 class="fs-title text-center">Personal details</h2>
                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Identification type</label>
                                                        <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlIdentification_Type" runat="server"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ControlToValidate="ddlIdentification_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator6" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Identification number</label>
                                                        <asp:TextBox ID="txtIdentification_Number" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtIdentification_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator7" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </section>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Title</label>
                                                        <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlPerson_Title" runat="server"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ControlToValidate="ddlPerson_Title" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>

                                                </section>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">First name</label>
                                                        <asp:TextBox ID="txtFirst_Names" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtFirst_Names" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator9" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Surname</label>
                                                        <asp:TextBox ID="txtSurname" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtSurname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator10" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>

                                                </section>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Contact number</label>
                                                        <asp:TextBox ID="txtContact_Number" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtContact_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator11" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Alternative contact number</label>
                                                        <asp:TextBox ID="txtAlternative_Contact_Number" runat="server"></asp:TextBox>

                                                    </div>

                                                </section>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Email address</label>
                                                        <asp:TextBox ID="txtEmail_Address" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtEmail_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator12" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator3" ControlToValidate="txtEmail_Address" runat="server" ValidationGroup="vgAddPartnerUser" ValidationExpression="^[a-zA-Z0-9]+[_a-zA-Z0-9\.-]*[a-zA-Z0-9]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$" ErrorMessage="Please enter a valid email address"></asp:RegularExpressionValidator>
                                                    </div>

                                                </section>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                    <asp:Panel ID="pnlBusinessDetails" Visible="false" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h2 class="fs-title text-center">Business details</h2>
                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Business name</label>
                                                        <asp:TextBox ID="txtBusiness_Name" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Name" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator22" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Business registration number</label>
                                                        <asp:TextBox ID="txtBusiness_Registration_Number" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Registration_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator23" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </section>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Contact fullname</label>
                                                        <asp:TextBox ID="txtBusiness_Contact_Fullname" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Contact_Fullname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator24" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Contact number</label>
                                                        <asp:TextBox ID="txtBusiness_Contact_Number" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Contact_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator25" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </section>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Contact alternative number</label>
                                                        <asp:TextBox ID="txtBusiness_Contact_Alternative_Number" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Contact_Alternative_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator26" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Contact email address</label>
                                                        <asp:TextBox ID="txtBusiness_Email_Address" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Email_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator27" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </section>
                                            </div>
                                        </div>



                                    </asp:Panel>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h2 class="fs-title text-center">Physical address details</h2>
                                            <section class="col-md-offset-2">

                                                <div class="col-md-6">

                                                    <label class="txtFieldLabel">Building/Unit/House number</label>

                                                    <asp:TextBox ID="txtBuilding_Unit" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtBuilding_Unit" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator13" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Address line 1</label>


                                                    <asp:TextBox ID="txtAddress_Line_1" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtAddress_Line_1" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator14" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">

                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Address line 2</label>

                                                    <asp:TextBox ID="txtAddress_Line_2" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Suburb</label>

                                                    <asp:TextBox ID="txtSuburb" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtSuburb" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator15" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">

                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">City</label>

                                                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtCity" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator16" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Province</label>

                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlProvince" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlProvince" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator17" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">

                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Postal Code</label>

                                                    <asp:TextBox ID="txtPostal_Code" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtPostal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator18" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h2 class="fs-title text-center">Postal address details</h2>
                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Same as physical address</label>

                                                    <asp:CheckBox ID="chkPostalSameAsPhysical" AutoPostBack="true" runat="server" OnCheckedChanged="chkPostalSameAsPhysical_CheckedChanged" />

                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                    <asp:Panel ID="pnlPostalAddress" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">PO Box/Bag</label>

                                                        <asp:TextBox ID="txtPOBox_Bag" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtPOBox_Bag" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator19" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Post office</label>

                                                        <asp:TextBox ID="txtPost_Office_Name" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtPost_Office_Name" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator20" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </section>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">

                                                <section class="col-md-offset-2">

                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Postal code</label>

                                                        <asp:TextBox ID="txtPost_Postal_Code" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtPost_Postal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator21" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </section>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </div>
                                <div class="sectionContainer">


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


                                    <div class="row">
                                        <div class="col-md-12">

                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <asp:Button CssClass="saveButton" ID="btnCreatePolicy" ValidationGroup="vgPolicy" runat="server" Text="Add policy" OnClick="btnCreatePolicy_Click" />

                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>


                        </section>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
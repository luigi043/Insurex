<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewPolicy_Old.aspx.cs" Inherits="IAPR_WeCssClass="form-control js-example-placeholder-single"b.AddNewPolicy_Old" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" id="grad1">
        <div class="row justify-content-center mt-0">
            <div class="col-11 col-sm-12 col-md-12 col-lg-12 text-center p-0 mt-3 mb-2">
                <div class="card px-0 pt-4 pb-0 mt-3 mb-3">
                    <%-- <ul id="progressbar">
            <li class="active account" id="account" runat="server"><strong>Account</strong></li>
            <li id="deals" class="deal" runat="server"><strong>Deals</strong></li>
            <li id="personal" class="personal" runat="server"><strong>Personal</strong></li>
            <li id="address" class="address" runat="server"><strong>Address</strong></li>
            <li id="banking" class="banking" runat="server"><strong>Banking</strong></li>
        </ul>--%>
                     <asp:ListBox ID="lstEmployee" runat="server" SelectionMode="Multiple">
                                                        <asp:ListItem Text="Nikunj Satasiya" Value="1" />
                                                        <asp:ListItem Text="Dev Karathiya" Value="2" />
                                                        <asp:ListItem Text="Hiren Dobariya" Value="3" />
                                                        <asp:ListItem Text="Vivek Ghadiya" Value="4" />
                                                        <asp:ListItem Text="Pratik Pansuriya" Value="5" />
                                                    </asp:ListBox>
                    <div class="col-md-12 mx-0">
                        <section id="msform">
                            <div class="form-card">
                                <asp:Panel ID="pnlStep1" runat="server">
                                    <div class="row">

                                        <div class="col-md-12">
                                            <h2 class="fs-title text-center">Policy details</h2>

                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Select insurance company</label>
                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlInsuranceCompanies" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlInsuranceCompanies" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail1" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Select policy type</label>
                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlPolicy_Type" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlPolicy_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail1" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Policy number</label>
                                                    <asp:TextBox ID="txtPolicy_Number" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtPolicy_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail1" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Policy payment frequency</label>
                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlPolicy_Payment_Frequency" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlPolicy_Payment_Frequency" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail1" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">

                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <asp:Button  CssClass="saveButton" ID="btnContinue1" ValidationGroup="vgAddPolicyDetail1" runat="server" Text="Continuue" OnClick="btnContinue1_Click" />

                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlStep2" runat="server" Visible="false" Enabled="false">
                                    <asp:Panel ID="pnlPersonalDetails" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h2 class="fs-title text-center">Policy holder details</h2>
                                                <section class="col-md-offset-2">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Identification type</label>
                                                        <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlIdentification_Type" runat="server"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ControlToValidate="ddlIdentification_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator6" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Identification number</label>
                                                        <asp:TextBox ID="txtIdentification_Number" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtIdentification_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator7" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Title</label>
                                                        <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlPerson_Title" runat="server"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ControlToValidate="ddlPerson_Title" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">First_Name</label>
                                                        <asp:TextBox ID="txtFirst_Names" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtFirst_Names" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator9" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Surname</label>
                                                        <asp:TextBox ID="txtSurname" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtSurname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator10" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">


                                                        <label class="txtFieldLabel">Contact number</label>
                                                        <asp:TextBox ID="txtContact_Number" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtContact_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator11" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Alternative contact number</label>
                                                        <asp:TextBox ID="txtAlternative_Contact_Number" runat="server"></asp:TextBox>

                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Email address</label>
                                                        <asp:TextBox ID="txtEmail_Address" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtEmail_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator12" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </section>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlBusinessDetails" runat="server" Visible="false">
                                    </asp:Panel>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h2 class="fs-title text-center">Physical address details</h2>
                                            <section class="col-md-offset-2">

                                                <div class="col-md-6">

                                                    <label class="txtFieldLabel">Building/Unit or House number</label>

                                                    <asp:TextBox ID="txtBuilding_Unit" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtBuilding_Unit" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator13" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Address line 1</label>


                                                    <asp:TextBox ID="txtAddress_Line_1" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtAddress_Line_1" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator14" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Address line 2</label>

                                                    <asp:TextBox ID="txtAddress_Line_2" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Suburb</label>

                                                    <asp:TextBox ID="txtSuburb" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtSuburb" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator15" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">City</label>

                                                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtCity" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator16" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Province</label>

                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlProvince" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlProvince" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator17" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Postal Code</label>

                                                    <asp:TextBox ID="txtPostal_Code" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtPostal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator18" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
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
                                                <asp:Panel ID="pnlPostalAddress" runat="server">
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">PO Box/Bag</label>

                                                        <asp:TextBox ID="txtPOBox_Bag" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtPOBox_Bag" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator19" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Post office</label>

                                                        <asp:TextBox ID="txtPost_Office_Name" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtPost_Office_Name" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator20" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="txtFieldLabel">Postal code</label>

                                                        <asp:TextBox ID="txtPost_Postal_Code" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtPost_Postal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator21" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                    </div>
                                                </asp:Panel>
                                            </section>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h2 class="fs-title text-center">Asset details</h2>
                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Select asset cover type</label>
                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlAsset_Cover_Type" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Cover_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail1" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Financier</label>
                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlAsset_Financier" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Financier" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator22" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Finance_Agrreement_Number</label>
                                                    <asp:TextBox ID="txtFinance_Agrreement_Number" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtFinance_Agrreement_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator23" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Financing start date</label>

                                                    <asp:TextBox ID="txtFinance_Start_Date" runat="server" TextMode="Date"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtFinance_Start_Date" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator25" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Financing end date</label>

                                                    <asp:TextBox ID="txtFinance_End_Date" runat="server" TextMode="Date"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtFinance_End_Date" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator26" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Model_Year</label>
                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlModel_Year" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlModel_Year" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator24" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Vehicle type</label>

                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlVehicle_Asset_Type" runat="server"></asp:DropDownList>
                                                    <asp:Literal ID="litVehicle_Asset_Type" runat="server"></asp:Literal>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlVehicle_Asset_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator27" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">

                                                    <label class="txtFieldLabel">Vehicle licence type</label>

                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlVehicle_Asset_Licence_Type" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-6">

                                                    <label class="txtFieldLabel">Asset usage</label>

                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlAsset_Usage_Type" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Usage_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator28" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Asset condition</label>

                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlAsset_Condition" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Condition" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator29" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">


                                                    <label class="txtFieldLabel">Vehicle make</label>

                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlVehicle_Make" runat="server" OnSelectedIndexChanged="ddlVehicle_Make_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlVehicle_Make" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator30" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">

                                                    <label class="txtFieldLabel">Vehicle model</label>

                                                    <asp:DropDownList CssClass="form-control js-example-placeholder-single" ID="ddlVehicle_Model" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ControlToValidate="ddlVehicle_Model" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator31" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Vin number</label>

                                                    <asp:TextBox ID="txtVin_Number" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtVin_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator32" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Registration number</label>

                                                    <asp:TextBox ID="txtRegistration_Number" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtRegistration_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator33" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="txtFieldLabel">Vehicle color</label>

                                                    <asp:TextBox ID="txtVehicle_Color" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="txtVehicle_Color" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPolicyDetail2" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator34" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">

                                            <section class="col-md-offset-2">
                                                <div class="col-md-6">
                                                    <asp:Button  CssClass="saveButton" ID="btnCreatePolicy" ValidationGroup="vgAddPolicyDetail2" runat="server" Text="Create policy" OnClick="btnCreatePolicy_Click" />

                                                </div>
                                            </section>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </section>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

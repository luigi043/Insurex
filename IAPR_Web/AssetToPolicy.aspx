<%@ Page Language="C#" MasterPageFile="~/NonUser.Master" AutoEventWireup="true" CodeBehind="AssetToPolicy.aspx.cs" Inherits="IAPR_Web.AssetToPolicy" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">

        <asp:Panel ID="pnlExists" runat="server">
            <h1 class="h3 mb-2 text-gray-800">Insurance policy details</h1>
            <asp:Panel ID="pnlIntro" runat="server">
                <p>
                    You are requested by
                    <asp:Label ID="lblFinancer" runat="server" Text=""></asp:Label>
                    to provide insurance details for the below listed asset.  
                </p>
            </asp:Panel>
            <asp:Panel ID="pnlStep1" runat="server" Visible="false" Enabled="false">

                <asp:Panel ID="pnlPersonalDetails" runat="server" Enabled="false" Visible="false">
                    <h6 class="m-0 font-weight-bold text-primary">Personal details</h6>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Identification type</label>
                            <asp:DropDownList CssClass="form-control" ID="ddlIdentification_Type" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlIdentification_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator6" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Identification number</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtIdentification_Number" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtIdentification_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator7" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Title</label>
                            <asp:DropDownList CssClass="form-control" ID="ddlPerson_Title" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlPerson_Title" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>

                    </div>


                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">First name</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtFirst_Names" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtFirst_Names" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator9" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Surname</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtSurname" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtSurname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator10" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>


                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Contact number</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtContact_Number" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtContact_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator11" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Alternative contact number</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtAlternative_Contact_Number" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Email address</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtEmail_Address" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtEmail_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator12" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator3" ControlToValidate="txtEmail_Address" runat="server" ValidationGroup="vgAddPartnerUser" ValidationExpression="^[a-zA-Z0-9]+[_a-zA-Z0-9\.-]*[a-zA-Z0-9]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$" ErrorMessage="Please enter a valid email address"></asp:RegularExpressionValidator>
                        </div>

                    </div>

                </asp:Panel>
                <asp:Panel ID="pnlBusinessDetails" Visible="false" runat="server" Enabled="false">

                    <h6 class="m-0 font-weight-bold text-primary">Business details</h6>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Business name</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtBusiness_Name" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Name" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator22" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Business registration number</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtBusiness_Registration_Number" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Registration_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator23" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>


                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Contact fullname</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtBusiness_Contact_Fullname" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Contact_Fullname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator24" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Contact number</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtBusiness_Contact_Number" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Contact_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator25" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Contact alternative number</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtBusiness_Contact_Alternative_Number" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Contact_Alternative_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator26" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Contact email address</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtBusiness_Email_Address" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtBusiness_Email_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator27" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </asp:Panel>
                <h6 class="m-0 font-weight-bold text-primary">Physical address details</h6>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Building/Unit/House number</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtBuilding_Unit" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtBuilding_Unit" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator13" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Address line 1</label>


                        <asp:TextBox CssClass="form-control mb-2" ID="txtAddress_Line_1" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtAddress_Line_1" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator14" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Address line 2</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtAddress_Line_2" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Suburb</label>
                        <asp:TextBox CssClass="form-control mb-2" ID="txtSuburb" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtSuburb" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator15" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">City</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtCity" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtCity" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator16" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Province</label>

                        <asp:DropDownList CssClass="form-control" ID="ddlProvince" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlProvince" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator17" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Postal Code</label>
                        <asp:TextBox CssClass="form-control mb-2" ID="txtPostal_Code" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPostal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator18" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <h6 class="m-0 font-weight-bold text-primary">Postal address details</h6>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <div class="divCheckPadding">
                            <asp:CheckBox ID="chkPostalSameAsPhysical" TextAlign="Right" Text="Same as physical address" AutoPostBack="true" runat="server" />
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlPostalAddress" runat="server">
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">PO Box/Bag</label>

                            <asp:TextBox CssClass="form-control mb-2" ID="txtPOBox_Bag" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtPOBox_Bag" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator19" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Post office</label>

                            <asp:TextBox CssClass="form-control mb-2" ID="txtPost_Office_Name" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtPost_Office_Name" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator20" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Postal code</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtPost_Postal_Code" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtPost_Postal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator21" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlAssetDeatils" runat="server">
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-12">
                            <h6 class="m-0 font-weight-bold text-primary">Asset details</h6>
                            <div id="divAssetDeatils" runat="server" class="detailsDiv">
                            </div>
                        </div>
                    </div>
                </asp:Panel>

            </asp:Panel>
            <asp:Panel ID="pnlStep2" runat="server" Visible="false">
                <h6 class="m-0 font-weight-bold text-primary">Policy details</h6>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-12">
                        <div class="btn btn-warning btn-circle">
                            <i class="fas fa-exclamation-circle"></i>
                        </div>
                        Note that the insurance details you provide will have to be verified by the selected provider.
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Insurance provider</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlInsuranceCompanies" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlInsuranceCompanies" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Policy type</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlPolicy_Type" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlPolicy_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Policy number</label>
                        <asp:TextBox CssClass="form-control mb-2" ID="txtPolicy_Number" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPolicy_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <asp:Button CssClass="btn btn-primary" ID="btnCheckPolicy" ValidationGroup="vgPolicy" runat="server" Text="Continue" OnClick="btnCheckPolicy_Click" />
            </asp:Panel>
            <asp:Panel ID="pnlInsurance_Value" runat="server" Visible="false">
                <div class="form-row align-items-center">
                    <%-- <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Insurance value</label>
                        <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text">R</div>
                            </div>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtAsset_Insurance_Value" runat="server" data-type="currency"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ControlToValidate="txtAsset_Insurance_Value" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator28" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>--%>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Asset cover type</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlAsset_Cover_Type" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Cover_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>

            </asp:Panel>
            <asp:Panel ID="pnlNewPolicy" runat="server" Visible="false">
                <asp:Panel ID="pnlPolicyPaymentFrequency" runat="server">
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Premium frequency</label>
                            <asp:DropDownList CssClass="form-control" ID="ddlPolicy_Payment_Frequency" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlPolicy_Payment_Frequency" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Button CssClass="btn btn-primary" ID="btnSaveNewPolicy" ValidationGroup="vgPolicy" runat="server" Text="Save" OnClick="btnSaveNewPolicy_Click" />
            </asp:Panel>
            <asp:Panel ID="pnlExistingPolicy" runat="server" Visible="false">
                <asp:Button CssClass="btn btn-primary" ID="btnSaveExistingPolicy" ValidationGroup="vgPolicy" runat="server" Text="Update" OnClick="btnSaveExistingPolicy_Click" />

            </asp:Panel>
            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="successMessage">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-12">
                        <div class="btn btn-success btn-circle">
                            <i class="fas fa-check-circle"></i>
                        </div>
                        Thank you for updating your insurance information. Your selected insurance provider will verify the details and you will receive feedback.
                    </div>

                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
            <div class="form-row align-items-center">
                <div class="form-group col-md-12">
                    <a href="#" class="btn btn-warning btn-circle">
                        <i class="fas fa-exclamation-triangle"></i>
                    </a>Sorry, we cannot find your details
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

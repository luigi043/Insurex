<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPartner.ascx.cs" Inherits="IAPR_Web.UserControls.Admin.EditPartner" %>


<div class="container-fluid">

    <asp:HiddenField ID="hdPartnerID" runat="server" />
    <asp:HiddenField ID="hdPartnerTypeId" runat="server" />
    <asp:Panel ID="pnlStep1" runat="server">
        <div class="form-div-border">
            <div class="form-row align-items-center">
                <h6 class="m-0 font-weight-bold text-primary">Edit partner details</h6>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Select partner type</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlPartnerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPartnerType_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlPartnerType" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Select partner</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlPartners" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPartners_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlPartners" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlStep2" runat="server" Visible="false">
        <div class="panelSeparator"></div>
        <div class="form-div-border">
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Partner name</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtPartnerName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtPartnerName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Business registration number</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtBusinessNumber" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtBusinessNumber" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator2" ControlToValidate="txtBusinessNumber" runat="server" ValidationGroup="vgEditPartner" ValidationExpression="((19|20)[\d]{2}\/[\d]{6}\/[\d]{2})" ErrorMessage="Please enter a valid business number (2001/123456/99)"></asp:RegularExpressionValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Vat registration number</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtVatRegistrationNumber" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtVatRegistrationNumber" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator11" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator4" ControlToValidate="txtVatRegistrationNumber" runat="server" ValidationGroup="vgEditPartner" ValidationExpression="^4\d{9}$" ErrorMessage="Please enter a valid business number (4123456789)"></asp:RegularExpressionValidator>
                </div>
            </div>

            <asp:Panel ID="pnlAssetsFinanced" runat="server" Visible="false">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-12">
                        <h4 class="">Financed assets</h4>
                        <div class="form-check">
                            <%--<asp:CheckBoxList ID="chkAssetsFinanced" runat="server" CssClass="">
                    </asp:CheckBoxList>--%>
                            <asp:RadioButtonList class="rbListWrap" ID="rblPackages" RepeatDirection="Horizontal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblPackages_SelectedIndexChanged">
                                <asp:ListItem Value="1">Consumer</asp:ListItem>
                                <asp:ListItem Value="2">Commercial</asp:ListItem>
                                <asp:ListItem Value="3">Consumer and Commercial</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:CustomValidator runat="server" ID="cvmodulelist" ClientValidationFunction="ValidateModuleList" Display="Dynamic" ErrorMessage="">
                            </asp:CustomValidator>
                            <asp:RequiredFieldValidator ControlToValidate="rblPackages" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator12" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <div id="divPackageAssets" runat="server" class="detailsDiv">
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <div class="form-row align-items-center">
                <h6 class="m-0 font-weight-bold text-primary">Physical address details</h6>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Building/Unit or House number</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtBuilding_Unit" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtBuilding_Unit" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator13" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Address line 1</label>


                    <asp:TextBox CssClass="form-control mb-2" ID="txtAddress_Line_1" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtAddress_Line_1" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator14" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
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
                    <asp:RequiredFieldValidator ControlToValidate="txtSuburb" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator15" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>

            </div>

            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">City</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtCity" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtCity" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator16" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Province</label>

                    <asp:DropDownList CssClass="form-control" ID="ddlProvince" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlProvince" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator17" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>

            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Postal Code</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtPostal_Code" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtPostal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator18" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="form-div-border">
            <div class="form-row align-items-center">
                <h6 class="m-0 font-weight-bold text-primary">Postal address details</h6>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <div class="divCheckPadding">
                        <asp:CheckBox ID="chkPostalSameAsPhysical" AutoPostBack="true" TextAlign="Right" Text="Same as physical address" runat="server" OnCheckedChanged="chkPostalSameAsPhysical_CheckedChanged" />
                    </div>
                </div>
            </div>
            <asp:Panel ID="pnlPostalAddress" runat="server">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">PO Box/Bag</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtPOBox_Bag" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPOBox_Bag" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator19" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Post office</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtPost_Office_Name" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPost_Office_Name" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator20" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>

                </div>


                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Postal code</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtPost_Postal_Code" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPost_Postal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator21" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>

                </div>

            </asp:Panel>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Company contact number</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtCompanyContactNumber" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtCompanyContactNumber" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator10" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="form-div-border">

            <div class="form-row align-items-center">

                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">First name</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtFirst_Names" runat="server" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtFirst_Names" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator6" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Surname</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtSurname" runat="server" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtSurname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator7" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>

            </div>
            <div class="form-row align-items-center">


                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Email address (Will serve as username)</label>
                    <div class="input-group mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text">@</div>
                        </div>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtEmail_Address" runat="server" Enabled="false"></asp:TextBox>
                    </div>
                    <asp:Literal ID="litUserExists" runat="server"></asp:Literal>
                    <asp:RequiredFieldValidator ControlToValidate="txtEmail_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator3" ControlToValidate="txtEmail_Address" runat="server" ValidationGroup="vgEditPartnerUser" ValidationExpression="^[a-zA-Z0-9]+[_a-zA-Z0-9\.-]*[a-zA-Z0-9]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$" ErrorMessage="Please enter a valid email address"></asp:RegularExpressionValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Position</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtPosition_Title" runat="server" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtPosition_Title" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-row align-items-center">

                <div class="form-group">
                    <label class="txtFieldLabel">Contact number</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtContact_Number" runat="server" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtContact_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgEditPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator9" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ControlToValidate="txtContact_Number" Display="Dynamic" ValidationExpression="^(\d{10})$" ValidationGroup="vgEditPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator1" ErrorMessage="Please enter a valid contact number" runat="server"></asp:RegularExpressionValidator>
                </div>

            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-12">
                    <label class="txtFieldLabel">Will the user receive transactional notifications?</label>
                    <asp:RadioButtonList ID="rblNotifications" class="rbListWrap" RepeatDirection="Horizontal" runat="server" Enabled="false">
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>

            </div>
        </div>
        <asp:Panel ID="pnlSaveButtons" runat="server">

            <asp:Button CssClass="btn btn-primary" ID="btnAddPartner" ValidationGroup="vgEditPartner" runat="server" Text="Update" OnClick="btnAddPartner_Click" />


            <asp:Button CssClass="btn btn-warning" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />


        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="successMessage">
            <div class="row">
                <div class="col-md-12">
                    <section class="col-md-offset-2">
                        <div class="col-md-12">
                            <div class="form-group">
                                Partner has been updated successfully.
                           
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </asp:Panel>

    </asp:Panel>
</div>

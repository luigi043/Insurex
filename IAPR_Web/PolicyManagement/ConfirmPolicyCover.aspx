<%@ Page Language="C#" MasterPageFile="~/Insurer.Master" AutoEventWireup="true" CodeBehind="ConfirmPolicyCover.aspx.cs" Inherits="IAPR_Web.PolicyManagement.ConfirmPolicyCover" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdAlnmlId" runat="server" />
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Pending confirmation</h1>
        <asp:Panel ID="pnlPoliciesAwaitingConfirmation" runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Policies
                    </h6>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-bordered" id="dataTable">
                            <asp:Repeater ID="rptPoliciesAwaitingConfirmation" runat="server" OnItemCommand="rptPoliciesAwaitingConfirmation_ItemCommand">
                                <HeaderTemplate>

                                    <thead>
                                        <tr>

                                            <th data-class="expand">
                                                <asp:Label ID="Label1" runat="server" Text="Policy number"></asp:Label>
                                            </th>

                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblFinancer_Name" Text='<%# Eval("vcPolicy_Number") %>' />

                                            </td>
                                            <td>
                                                <asp:LinkButton ID="ImageButton1" runat="server" CommandName="ViewDeatils" CssClass="fa fa-edit mr-2" title="View details" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iAsset_Policy_Alignment_Id")%>' />

                                            </td>

                                        </tr>
                                    </tbody>
                                </ItemTemplate>

                            </asp:Repeater>
                        </table>
                    </div>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">
                                <asp:Label ID="lblNoPolicies" runat="server" Text="There are currently no policies to process" Visible="false"></asp:Label></label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlStep1" runat="server" Visible="false">

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
                        <label class="txtFieldLabel">Mobile number</label>
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
                        <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text">@</div>
                            </div>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtEmail_Address" runat="server"></asp:TextBox>
                        </div>
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
            <asp:Panel ID="pnlPhysicalAddress" runat="server" Enabled="false">
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
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <h6 class="m-0 font-weight-bold text-primary">Postal address details</h6>
                    </div>
                </div>

                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <div class="divCheckPadding">
                            <asp:CheckBox ID="chkPostalSameAsPhysical" TextAlign="Right" Text="Same as physical address" AutoPostBack="true" runat="server" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
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
                <h6 class="m-0 font-weight-bold text-primary">Asset details</h6>
                <div class="form-row align-items-center">
                    <div class="form-group">
                        <div id="divAssetDetails" runat="server" class="detailsDiv">
                        </div>
                    </div>
                </div>
                <h6 class="m-0 font-weight-bold text-primary">Policy details</h6>
                <div class="form-row align-items-center">
                    <div class="form-group">
                        <div id="divPolicyDetails" runat="server" class="detailsDiv">
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </asp:Panel>



        <asp:Panel ID="pnlStep2" runat="server" Visible="false">
            <h6 class="m-0 font-weight-bold text-primary">Required action</h6>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Action</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="1">Confirm</asp:ListItem>
                        <asp:ListItem Value="2">Reject</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlAction" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Insurance value</label>
                    <div class="input-group mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text">R</div>
                        </div>
                        <asp:TextBox CssClass="form-control mb-2" ID="txtAsset_Insurance_Value" runat="server" data-type="currency"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ControlToValidate="txtAsset_Insurance_Value" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator28" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <asp:Panel ID="pnlRejection" runat="server" Visible="false">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Rejection reason</label>
                        <asp:TextBox CssClass="form-control mb-2" ID="txtRejectionReason" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtRejectionReason" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </asp:Panel>
            <asp:Button CssClass="btn btn-primary" ID="btnConfirmPolicy" ValidationGroup="vgPolicy" runat="server" Text="Save" OnClick="btnConfirmPolicy_Click" />
            <asp:Button CssClass="btn btn-warning" ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </asp:Panel>

        <asp:Panel ID="pnlComplete" runat="server" Visible="false">
            <div class="form-row align-items-center">
                <div class="col-lg-6 mb-4">
                    <div class="card bg-success text-white shadow">
                        <div class="card-body">
                            Completed
                    
                        <div class="col-md-6">
                            <label class="txtFieldLabel"></label>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>
</asp:Content>

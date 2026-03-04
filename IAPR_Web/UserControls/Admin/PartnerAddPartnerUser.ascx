<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PartnerAddPartnerUser.ascx.cs" Inherits="IAPR_Web.UserControls.Admin.PartnerAddPartnerUser" %>


<div class="container-fluid">
    <h1 class="h3 mb-2 text-gray-800">Add user</h1>

    <div class="form-row align-items-center">
        <div class="form-group col-md-6">
            <label class="txtFieldLabel">First name</label>

            <asp:TextBox CssClass="form-control mb-2" ID="txtFirst_Names" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtFirst_Names" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator6" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group col-md-6">
            <label class="txtFieldLabel">Surname</label>

            <asp:TextBox CssClass="form-control mb-2" ID="txtSurname" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtSurname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator7" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-row align-items-center">
        <div class="form-group col-md-6">
            <label class="txtFieldLabel">Email address (Will serve as username)</label>
            <asp:TextBox CssClass="form-control mb-2" ID="txtEmail_Address" runat="server"></asp:TextBox>
            <asp:Literal ID="litUserExists" runat="server"></asp:Literal>
            <asp:RequiredFieldValidator ControlToValidate="txtEmail_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
           <%-- <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator3" ControlToValidate="txtEmail_Address" runat="server" ValidationGroup="vgAddPartnerUser" ValidationExpression="^[a-zA-Z0-9]+[_a-zA-Z0-9\.-]*[a-zA-Z0-9]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$" ErrorMessage="Please enter a valid email address"></asp:RegularExpressionValidator>--%>
        </div>


        <div class="form-group col-md-6">
            <label class="txtFieldLabel">Position</label>
            <asp:TextBox CssClass="form-control mb-2" ID="txtPosition_Title" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPosition_Title" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
        </div>

    </div>
    <div class="form-row align-items-center">
        <div class="form-group col-md-6">
            <label class="txtFieldLabel">Contact number</label>

            <asp:TextBox CssClass="form-control mb-2" ID="txtContact_Number" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtContact_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator9" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-row align-items-center">
        <div class="form-group col-md-12">
            <label class="txtFieldLabel">Will the user receive transactional notifications?</label>
            <asp:RadioButtonList class="rbListWrap" ID="rblNotifications" RepeatDirection="Horizontal" runat="server">
                <asp:ListItem>Yes</asp:ListItem>
                <asp:ListItem>No</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ControlToValidate="rblNotifications" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator23" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
        </div>
    </div>
    <asp:Panel ID="pnlSaveButtons" runat="server">
        <asp:Button CssClass="btn btn-primary" ID="btnAddPartnerUser" ValidationGroup="vgAddPartnerUser" runat="server" Text="Add" OnClick="btnAddPartnerUser_Click" />
        <asp:Button CssClass="btn btn-warning" ID="Button3" runat="server" Text="Cancel" OnClick="Button3_Click" />
    </asp:Panel>
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-12">
                <section class="col-md-offset-2">
                    <div class="col-md-12">
                        <div class="form-group">
                            Partner has been created successfully.
                           
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </asp:Panel>
</div>

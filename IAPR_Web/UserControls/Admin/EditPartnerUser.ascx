<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPartnerUser.ascx.cs" Inherits="IAPR_Web.UserControls.Admin.EditPartnerUser" %>

<asp:HiddenField ID="hdPartnerID" runat="server" />
<asp:HiddenField ID="hdPartnerTypeId" runat="server" />
<asp:HiddenField ID="hdUID" runat="server" />
<div class="container-fluid">
    <asp:Panel ID="pnlPartner" runat="server" Visible="false">
        <h1 class="h3 mb-2 text-gray-800">Edit user</h1>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Select partner</label>
                <asp:DropDownList CssClass="form-control" ID="ddlPartnerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPartnerType_SelectedIndexChanged"></asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="ddlPartnerType" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgMonthlyReport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

            </div>
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Select partner</label>
                <asp:DropDownList CssClass="form-control" ID="ddlPartners" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPartners_SelectedIndexChanged"></asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="ddlPartners" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgMonthlyReport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

            </div>

        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSelectUser" runat="server" Visible="false">
        <h6 class="m-0 font-weight-bold text-primary">Edit user</h6>
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Users
                </h6>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-bordered" id="dataTable">
                        <asp:Repeater ID="rptUserList" runat="server" OnItemCommand="rptUserList_ItemCommand">
                            <HeaderTemplate>

                                <thead>
                                    <tr>
                                        <th>
                                            <asp:Label ID="lbl1" runat="server" Text="Name"></asp:Label>
                                            <%--<asp:ImageButton ID="ImageButton1" runat="server" CommandName="Sort" CssClass="editButton" ToolTip="Change asset cover" ImageUrl="~/Images/Sort.png" CommandArgument="vcName" />
                                            --%>  </th>
                                        <th>
                                            <asp:Label ID="lbl2" runat="server" Text="Surname"></asp:Label>
                                            <%--<asp:ImageButton ID="ImageButton3" runat="server" CommandName="Sort" CssClass="editButton" ToolTip="Change asset cover" ImageUrl="~/Images/Sort.png" CommandArgument="vcSurname" />
                                            --%> </th>
                                        <th>
                                            <asp:Label ID="lbl4" runat="server" Text="eMail Address"></asp:Label>
                                            <%--<asp:ImageButton ID="ImageButton4" runat="server" CommandName="Sort" CssClass="editButton" ToolTip="Change asset cover" ImageUrl="~/Images/Sort.png" CommandArgument="vcUsername" />
                                            --%> </th>
                                        <th>
                                            <asp:Label ID="lbl3" runat="server" Text="Position Title"></asp:Label>
                                        </th>


                                        <th>
                                            <asp:Label ID="lbl8" runat="server" Text="Company"></asp:Label>
                                        </th>

                                        <th>
                                            <asp:Label ID="lbl7" runat="server" Text="User Status"></asp:Label>

                                        </th>
                                        <th>
                                            <asp:Label ID="lbl11" runat="server" Text="Receive Notifications"></asp:Label>

                                        </th>
                                        <th></th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblv1" Text='<%# Eval("vcName") %>' />

                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblv2" Text='<%# Eval("vcSurname") %>' />

                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblv4" Text='<%# Eval("vcUsername") %>' />

                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblv3" Text='<%# Eval("vcPosition_Title") %>' />

                                        </td>



                                        <td>
                                            <asp:Label runat="server" ID="lblv8" Text='<%# Eval("vcPartner_Type_Description") %>' />

                                        </td>

                                        <td>
                                            <asp:Label runat="server" ID="Label1" Text='<%# Eval("vcUser_Status_Description") %>' />

                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblv11" Text='<%# Eval("bUserReceiveNotifications") %>' />

                                        </td>
                                        <td class="action-td text-center">
                                            <asp:LinkButton ID="ImageButton1" runat="server" CommandName="EditUser" CssClass="fa fa-edit mr-2"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iUser_Id") + ";" 
                                                                                        + DataBinder.Eval(Container.DataItem, "iUser_Type_Id")  
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcName")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcSurname")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcUsername")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcPosition_Title")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcContactNumber")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "bUserReceiveNotifications")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iUser_Status_Id")
                                                                                        %>' />
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
    <asp:Panel ID="pnlEditUser" runat="server" Visible="false">
        <h6 class="m-0 font-weight-bold text-primary">Edit user</h6>
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
                <div class="input-group mb-2">
                    <div class="input-group-prepend">
                        <div class="input-group-text">@</div>
                    </div>


                    <asp:TextBox CssClass="form-control mb-2" ID="txtEmail_Address" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <asp:Literal ID="litUserExists" runat="server"></asp:Literal>
                <asp:RequiredFieldValidator ControlToValidate="txtEmail_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator3" ControlToValidate="txtEmail_Address" runat="server" ValidationGroup="vgAddPartnerUser" ValidationExpression="^[a-zA-Z0-9]+[_a-zA-Z0-9\.-]*[a-zA-Z0-9]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$" ErrorMessage="Please enter a valid email address"></asp:RegularExpressionValidator>
            </div>
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Position</label>

                <asp:TextBox CssClass="form-control mb-2" ID="txtPosition_Title" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="txtPosition_Title" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
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
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">User status</label>
                <asp:DropDownList CssClass="form-control" ID="ddlUserStatus" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="ddlUserStatus" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgAddPartnerUser" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
            </div>
        </div>
        <asp:Panel ID="pnlSaveButtons" runat="server">
            <asp:Button CssClass="btn btn-primary" ID="btnUpdateUser" ValidationGroup="vgAddPartnerUser" runat="server" Text="Update" OnClick="btnUpdateUser_Click" />
            <asp:Button CssClass="btn btn-warning" ID="Button3" runat="server" Text="Cancel" OnClick="Button3_Click" />


        </asp:Panel>
        <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="successMessage">
            <div class="row">
                <div class="col-md-12">
                    <section class="col-md-offset-2">
                        <div class="col-md-12">
                            <div class="form-group">
                                Partner has been updated.
                           
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>

</div>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/NonUser.Master" CodeBehind="ChangePassword.aspx.cs" Inherits="IAPR_Web.Account.ChangePassword" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container-fluid">
        <div>
            <h2 class="fs-title text-center">Change your password</h2>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">

                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="litFailureText" />
                        </p>
                    </asp:PlaceHolder>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <asp:Label runat="server" AssociatedControlID="txtPassword">New Password</asp:Label>

                    <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger" ErrorMessage="The password field is required." />
                    <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator3" ControlToValidate="txtPassword" runat="server" ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$" ErrorMessage="Your password must be minimum eight characters, at least one letter, one number and one special characters"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <asp:Label runat="server" AssociatedControlID="txtConfirmPassword">Confirm password</asp:Label>

                    <asp:TextBox runat="server" ID="txtConfirmPassword" TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmPassword" CssClass="text-danger" ErrorMessage="The password field is required." />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" CssClass="text-danger" ErrorMessage="Your password does not match" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"></asp:CompareValidator>
                </div>

                <%--<div class="form-group">
                            <div class="col-md-offset-2 col-md-10">
                                <div class="checkbox">
                                    <asp:CheckBox runat="server" ID="RememberMe" />
                                    <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                                </div>
                            </div>
                        </div>--%>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button ID="btnUpdatePassword" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdatePassword_Click" />
                    </div>
                </div>

                <%-- <p>
                        <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register</asp:HyperLink>
                        if you don't have a local account.
                    </p>--%>
            </div>



        </div>
    </div>
</asp:Content>

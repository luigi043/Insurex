<%@ Page Title="Log in" Language="C#" MasterPageFile="~/NonUser.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IAPR_Web.Account.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--  <div class="spinner-border text-primary"></div>--%>
    <!-- Outer Row -->
    <div class="row justify-content-center">
        <div class="col-xl-10 col-lg-12 col-md-9">
            <div class="o-hidden border-0 shadow-lg my-5">
                <div class="p-0">
                    <!-- Nested Row within Card Body -->
                    <div class="row">
                        <div class="col-lg-6 d-none d-lg-block bg-login-image"></div>
                        <div class="col-lg-6">
                            <div class="p-5">
                                <div class="text-center">
                                    <h1 class="h4 text-gray-900 mb-4">
                                    Welcome Back!</h6>
                                </div>
                                <div class="user">
                                    <div class="form-group">
                                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                            <p class="text-danger">
                                                <asp:Literal runat="server" ID="litFailureText" />
                                            </p>
                                        </asp:PlaceHolder>
                                        <div class="form-group">
                                            <asp:TextBox runat="server" ID="txtUserName" class="form-control form-control-user" Placeholder="Enter Email Address..." />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUserName"
                                                CssClass="text-danger" ErrorMessage="The user name field is required." />
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" class="form-control form-control-user" placeholder="Password" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger" ErrorMessage="The password field is required." />
                                        </div>
                                        <div class="form-group">
                                            <div class="custom-control custom-checkbox small">
                                                <input
                                                    type="checkbox"
                                                    class="custom-control-input"
                                                    id="customCheck" />
                                                <label class="custom-control-label" for="customCheck">
                                                    Remember Me</label>
                                            </div>
                                        </div>

                                        <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-primary btn-user btn-block" />

                                    </div>
                                </div>
                                <hr />
                                <div class="text-center">
                                    <a class="small" href="PasswordReminder.aspx">Forgot Password?</a>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

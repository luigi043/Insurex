<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/NonUser.Master" CodeBehind="PasswordReminder.aspx.cs" Inherits="IAPR_Web.Account.PasswordReminder" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <div class="container-fluid">

        <h1 class="h3 mb-2 text-gray-800">Password reminder</h1>

        <div class="form-row align-items-center">
            <div class="form-group col-md-12">
                Enter your registered email address below and a confirmation notification will be sent to you. After you confirm on the email, your password will be sent to your email address
            </div>
        </div>

        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Registered email address</label>
                <div class="input-group mb-2">
                    <div class="input-group-prepend">
                        <div class="input-group-text">@</div>
                    </div>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtEmail_Address" runat="server"></asp:TextBox>
                </div>
                <asp:Literal ID="litUserExists" runat="server"></asp:Literal>
                <asp:RequiredFieldValidator ControlToValidate="txtEmail_Address" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPasswordReminder" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator Display="Dynamic" CssClass="txtnamevalidation erroMessage" ID="RegularExpressionValidator3" ControlToValidate="txtEmail_Address" runat="server" ValidationGroup="vgPasswordReminder" ValidationExpression="^[a-zA-Z0-9]+[_a-zA-Z0-9\.-]*[a-zA-Z0-9]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$" ErrorMessage="Please enter a valid email address"></asp:RegularExpressionValidator>
            </div>
        </div>



        <asp:Button ID="btnPasswordReminder" runat="server" ValidationGroup="vgPasswordReminder" Text="Send" CssClass="btn btn-primary" OnClick="btnPasswordReminder_Click" />
        <asp:Button CssClass="btn btn-warning" ID="btnCancelChangeCover" runat="server" Text="Cancel" OnClick="btnCancelChangeCover_Click" />
    </div>
</asp:Content>

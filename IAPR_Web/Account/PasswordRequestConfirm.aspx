<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/NonUser.Master" CodeBehind="PasswordRequestConfirm.aspx.cs" Inherits="IAPR_Web.Account.PasswordRequestConfirm" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container-fluid">

        <h1 class="h3 mb-2 text-gray-800">Password reminder</h1>
        <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
            <div class="form-row align-items-center">
                <div class="form-group col-md-12">
                    Your password hass been successfully sent to your email address.                      
                </div>
            </div>
            <asp:Button ID="btnPasswordReminder" runat="server" ValidationGroup="vgPasswordReminder" Text="Log in" CssClass="btn btn-primary" OnClick="btnPasswordReminder_Click" />
        </asp:Panel>

        <asp:Panel ID="pnlFail" runat="server" Visible="false">
            <div class="form-row align-items-center">
                <div class="form-group col-md-12">
                    Request not found!
                </div>
            </div>
        </asp:Panel>
        
    </div>
</asp:Content>

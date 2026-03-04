<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/NonUser.Master" CodeBehind="Encrypt_Decrypt.aspx.cs" Inherits="IAPR_Web.Encrypt_Decrypt" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">General Encryption</h1>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Plain</label>
                <asp:TextBox CssClass="form-control mb-2" ID="txtPLainTextGen" runat="server"></asp:TextBox>
            </div>
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Encrypted</label>
                <asp:TextBox CssClass="form-control mb-2" ID="txtEncryptedGen" runat="server"></asp:TextBox>
            </div>

        </div>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <asp:Label runat="server" CssClass="txtFieldLabel" ID="lblEncryptedAnswerGen"></asp:Label>

            </div>
            <div class="form-group col-md-6">
                <asp:Label runat="server" CssClass="txtFieldLabel" ID="lblDecryptedAnswerGen"></asp:Label>

            </div>

        </div>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <asp:Button CssClass="btn btn-primary" ID="btnEncryptGen" runat="server" Text="Encrypt" OnClick="btnEncryptGen_Click" />

            </div>
            <div class="form-group col-md-6">
                <asp:Button CssClass="btn btn-primary" ID="btnDecryptGen" runat="server" Text="Decrypt" OnClick="btnDecryptGen_Click" />

            </div>
        </div>

        <h1 class="h3 mb-2 text-gray-800">Validation Encryption</h1>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Plain</label>
                <asp:TextBox CssClass="form-control mb-2" ID="txtPLainTextVal" runat="server"></asp:TextBox>
            </div>
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Encrypted</label>
                <asp:TextBox CssClass="form-control mb-2" ID="txtEncryptedVal" runat="server"></asp:TextBox>
            </div>

        </div>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">

                <asp:Label runat="server" CssClass="txtFieldLabel" ID="lblEncryptedAnswerVal"></asp:Label>

            </div>
            <div class="form-group col-md-6">
                <asp:Label runat="server" CssClass="txtFieldLabel" ID="lblDecryptedAnswerVal"></asp:Label>

            </div>

        </div>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <asp:Button CssClass="btn btn-primary" ID="Button1" runat="server" Text="Encrypt" OnClick="Button1_Click" />

            </div>
            <div class="form-group col-md-6">
                <asp:Button CssClass="btn btn-primary" ID="Button2" runat="server" Text="Decrypt" OnClick="Button2_Click" />

            </div>
        </div>
    </div>
</asp:Content>

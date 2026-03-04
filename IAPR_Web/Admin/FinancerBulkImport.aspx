<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FinancerBulkImport.aspx.cs" Inherits="IAPR_Web.Admin.FinancerBulkImport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Import lender assets</h1>


        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Lender</label>
                <asp:DropDownList CssClass="form-control" ID="ddlAsset_Financier" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAsset_Financier_SelectedIndexChanged"></asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Financier" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgUpload" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator22" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

            </div>
        </div>
        <div class="form-row align-items-center">
            <div class="col-md-6">
                <label class="txtFieldLabel">Select asset type</label>
                <asp:DropDownList CssClass="form-control" ID="ddlAsset_Type" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAsset_Type_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgUpload" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <label class="txtFieldLabel">Select input file </label>

                <ajaxToolkit:AsyncFileUpload ID="fuBankAssets" OnClientUploadComplete="afu_OnUploadComplete" OnClientUploadError="uploadError" runat="server" Width="400px" UploaderStyle="Modern" CompleteBackColor="White" UploadingBackColor="#CCFFFF" ThrobberID="Image1"
                    OnUploadedComplete="FileUploadComplete" class="custom-file-input" />
               <%-- <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/import_csv.png" />--%>



            </div>

        </div>
        <div class="form-row align-items-center">
            <div class="form-group col-md-12">
                <div class="divCheckPadding">
                    <asp:CheckBox ID="chkCorrectIputs" AutoPostBack="true" TextAlign="Right" Text="Confirm that the correct financer and input csv file have been selected" runat="server" OnCheckedChanged="chkCorrectIputs_CheckedChanged" />
                </div>
            </div>
        </div>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <asp:Button CssClass="btn btn-primary" ID="btnImportBankAssest" Enabled="false" ValidationGroup="vgUpload" runat="server" Text="Import" OnClick="btnImportBankAssest_Click" />

            </div>
        </div>
    </div>
</asp:Content>


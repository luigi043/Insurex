<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InsurerBulkImport.aspx.cs" Inherits="IAPR_Web.Admin.InsurerBulkImport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" id="grad1">
        <div class="col-md-12 mx-0">
            <section id="msform">
                <div class="form-card">
                    <div class="sectionContainer">
                        <div class="row">

                            <div class="col-md-12">
                                <h2>Import insurer policies</h2>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-12">

                                <div class="col-md-6">
                                    <label class="txtFieldLabel">Policy type</label>
                                    <asp:DropDownList CssClass="form-control" ID="ddlPolicy_Type" runat="server">
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="1">Personal</asp:ListItem>
                                        <asp:ListItem Value="2">Business</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddlPolicy_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgImport" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator22" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>


                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">

                                <section class="col-md-offset-2">
                                    <div class="col-md-6">
                                        <label class="txtFieldLabel">Select input file </label>
                                        <label class="file-upload">
                                            <ajaxToolkit:AsyncFileUpload ID="fupInsurerPolicies" OnClientUploadComplete="afu_OnUploadComplete" OnClientUploadError="uploadError" runat="server" Width="400px" UploaderStyle="Modern" CompleteBackColor="White" UploadingBackColor="#CCFFFF" ThrobberID="Image1"
                                                OnUploadedComplete="FileUploadComplete" BackColor="#54AB82" />
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/import_csv.png" />


                                        </label>
                                    </div>
                                </section>
                            </div>

                        </div>


                        <div class="row">
                            <div class="col-md-12">

                                <section class="col-md-offset-2">
                                    <div class="col-md-6">
                                        <asp:Button CssClass="btn btn-primary" ID="btnImportInsurerPolicies" ValidationGroup="vgImport" runat="server" Text="Import" OnClick="btnImportInsurerPolicies_Click" />

                                    </div>
                                </section>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

</asp:Content>


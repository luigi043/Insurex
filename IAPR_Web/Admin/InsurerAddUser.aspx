<%@ Page Language="C#" MasterPageFile="~/Insurer.Master" AutoEventWireup="true" CodeBehind="InsurerAddUser.aspx.cs" Inherits="IAPR_Web.Admin.InsurerAddUser" %>


<%@ Register Src="../UserControls/Admin/PartnerAddPartnerUser.ascx" TagName="PartnerAddPartnerUser" TagPrefix="uc1" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" id="grad1">
        <div class="col-md-12 mx-0">
            <section id="msform">
                <div class="form-card">
                    <div class="sectionContainer">
                        <uc1:PartnerAddPartnerUser ID="PartnerAddPartnerUser1" runat="server" />
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

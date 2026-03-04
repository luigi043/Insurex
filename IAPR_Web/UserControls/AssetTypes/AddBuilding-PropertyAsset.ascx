<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddBuilding-PropertyAsset.ascx.cs" Inherits="IAPR_Web.UserControls.AssetTypes.AddBuilding_PropertyAsset" %>

<h6 class="m-0 font-weight-bold text-primary">Building details</h6>
<div class="form-row align-items-center">
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Asset cover type</label>
        <asp:DropDownList CssClass="form-control" ID="ddlAsset_Cover_Type" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Cover_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
</div>

<div class="form-row align-items-center">
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Lender</label>
        <asp:DropDownList CssClass="form-control" ID="ddlAsset_Financier" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ControlToValidate="ddlAsset_Financier" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator22" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Finance agreement number</label>
        <asp:TextBox CssClass="form-control mb-2" ID="txtFinance_Agrreement_Number" runat="server"></asp:TextBox>
        <asp:Literal ID="litFinanceNumberExists" runat="server"></asp:Literal>
        <asp:RequiredFieldValidator ControlToValidate="txtFinance_Agrreement_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator23" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-row align-items-center">
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Finance value</label>
        <div class="input-group mb-2">
            <div class="input-group-prepend">
                <div class="input-group-text">R</div>
            </div>
            <asp:TextBox CssClass="form-control" ID="txtAsset_Finance_Value" runat="server" data-type="currency"></asp:TextBox>
        </div>
        <asp:RegularExpressionValidator ID="revDecimals" runat="server" ValidationGroup="vgPolicy" ErrorMessage="Enter correct format value e.g. 100000,50" ControlToValidate="txtAsset_Finance_Value" ValidationExpression="^\d{1,3}(,\d{3})*(\.\d+)?$"></asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator ControlToValidate="txtAsset_Finance_Value" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Insurance value</label>
        <div class="input-group mb-2">
            <div class="input-group-prepend">
                <div class="input-group-text">R</div>
            </div>
            <asp:TextBox CssClass="form-control" ID="txtAsset_Insurance_Value" runat="server" data-type="currency"></asp:TextBox>
        </div>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="vgPolicy" ErrorMessage="Enter correct format value e.g. 100000,50" ControlToValidate="txtAsset_Insurance_Value" ValidationExpression="^\d{1,3}(,\d{3})*(\.\d+)?$"></asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator ControlToValidate="txtAsset_Insurance_Value" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>



</div>
<div class="form-row align-items-center">
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Financing start date</label>

        <asp:TextBox CssClass="form-control mb-2" ID="txtFinance_Start_Date" runat="server" TextMode="Date"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtFinance_Start_Date" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator25" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Financing end date</label>

        <asp:TextBox CssClass="form-control mb-2" ID="txtFinance_End_Date" runat="server" TextMode="Date"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtFinance_End_Date" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator26" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="CompareValidator1" CssClass="txtnamevalidation erroMessage" ValidationGroup="vgPolicy" runat="server"
            ControlToValidate="txtFinance_Start_Date" ControlToCompare="txtFinance_End_Date" Operator="LessThan" Type="Date"
            ErrorMessage="Finance start date must be less than End date."></asp:CompareValidator>
    </div>


</div>
<div class="form-row align-items-center">

    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Property type</label>
        <asp:DropDownList CssClass="form-control" ID="ddlProperty_Asset_Type" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ControlToValidate="ddlProperty_Asset_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Stand/ERF/Potion number</label>

        <asp:TextBox CssClass="form-control mb-2" ID="txtStand_ERF_Number" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtStand_ERF_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-row align-items-center">

   <div class="form-group col-md-6">
        <label class="txtFieldLabel">Sectional title number</label>

        <asp:TextBox CssClass="form-control mb-2" ID="txtSectionalTitleNumber" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtSectionalTitleNumber" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator7" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Sectional title name</label>

        <asp:TextBox CssClass="form-control mb-2" ID="txtSectionalTitleName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtSectionalTitleName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator6" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddKeymanInsuranceAsset.ascx.cs" Inherits="IAPR_Web.UserControls.AssetTypes.AddKeymanInsuranceAsset" %>


<h6 class="m-0 font-weight-bold text-primary">Keyman insurance asset details</h6>
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
        <asp:RequiredFieldValidator ControlToValidate="txtAsset_Finance_Value" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator1" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
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
        <asp:RequiredFieldValidator ControlToValidate="txtAsset_Insurance_Value" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
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
        <label class="txtFieldLabel">Keyman Insurance type type</label>

        <asp:DropDownList CssClass="form-control" ID="ddlKeymanInsurance_Asset_Type" runat="server"></asp:DropDownList>
        <asp:Literal ID="litVehicle_Asset_Type" runat="server"></asp:Literal>
        <asp:RequiredFieldValidator ControlToValidate="ddlKeymanInsurance_Asset_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator27" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>

</div>

<div class="form-row align-items-center">
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Keyman name</label>

        <asp:TextBox CssClass="form-control mb-2" ID="txtKeyman_Name" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtKeyman_Name" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator32" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Keyman surname</label>

        <asp:TextBox CssClass="form-control mb-2" ID="txtKeyman_Surname" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtKeyman_Surname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator33" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>


</div>
<div class="form-row align-items-center">
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Identification type</label>
        <asp:DropDownList CssClass="form-control" ID="ddlIdentification_Type" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ControlToValidate="ddlIdentification_Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator6" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group col-md-6">
        <label class="txtFieldLabel">Identity number</label>

        <asp:TextBox CssClass="form-control mb-2" ID="txtKeyman_Identity_Number" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtKeyman_Identity_Number" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
    </div>
</div>

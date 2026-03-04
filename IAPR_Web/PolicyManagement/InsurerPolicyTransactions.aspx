<%@ Page Language="C#" MasterPageFile="~/Insurer.Master" AutoEventWireup="true" CodeBehind="InsurerPolicyTransactions.aspx.cs" Inherits="IAPR_Web.PolicyManagement.InsurerPolicyTransactions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdPolicyId" runat="server" />
    <asp:HiddenField ID="hdAssetId" runat="server" />
    <asp:HiddenField ID="hdAssetType" runat="server" />
    <asp:HiddenField ID="hdPartnerID" runat="server" />
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Policy transactions</h1>
        <asp:Panel ID="pnlSelectPolicy" runat="server">
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Insurance policy number</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtPolicy_Number" runat="server"></asp:TextBox>
                    <asp:Literal ID="litPolicy_Number" runat="server"></asp:Literal>
                    <asp:RequiredFieldValidator Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgFindPolicy" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator2" ErrorMessage="This field is required" runat="server" ControlToValidate="txtPolicy_Number"></asp:RequiredFieldValidator>
                </div>

            </div>
            <asp:Button CssClass="btn btn-primary" ID="btnFind_Policy" ValidationGroup="vgFindPolicy" runat="server" Text="Find policy" OnClick="btnFind_Policy_Click" />
        </asp:Panel>
        <asp:Panel ID="pnlSelectTransactionType" runat="server" Visible="false">
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Select transaction type</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlPolicy_Transaction_Types" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlPolicy_Transaction_Types_SelectedIndexChanged"></asp:DropDownList>

                </div>
            </div>

        </asp:Panel>
        <div class="panelSeparator"></div>
        <asp:Panel ID="pnlPremiumNonPayment" runat="server" Visible="false">
            <h4 class="m-0 font-weight-bold text-secondary">Premium non-payment details</h4>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Affected period</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlAffectedPeriod" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlAffectedPeriod" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgNon_payment_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator4" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Affected Year</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlAffectedYear" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlAffectedYear" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgNon_payment_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Date of non-payment</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtDateOfNonPayment" runat="server" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtDateOfNonPayment" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgNon_payment_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator10" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <asp:Button CssClass="btn btn-primary" ID="btnSaveNonPaymnet" ValidationGroup="vgNon_payment_details" runat="server" Text="Update status" OnClick="btnSaveNonPaymnet_Click" />
            <asp:Button CssClass="btn btn-warning" ID="btnCancelNonPayment" runat="server" Text="Cancel" OnClick="btnCancelNonPayment_Click" />


        </asp:Panel>
        <asp:Panel ID="pnlChangeCover" runat="server" Visible="false">
            <h4 class="m-0 font-weight-bold text-secondary">Edit asset cover</h4>

            <asp:Panel ID="pnlAssetList" runat="server">
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">Policy assets
                        </h6>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-bordered" id="dataTable">

                                <asp:Repeater ID="rptAssetList" runat="server" OnItemCommand="rptAssetList_ItemCommand" OnItemDataBound="rptAssetList_ItemDataBound">
                                    <HeaderTemplate>

                                        <thead>
                                            <tr>

                                                <th>
                                                    <asp:Label ID="Label1" runat="server" Text="Financer"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblCover" runat="server" Text="Asset Cover"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="Label12" runat="server" Text="Asset type"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblVehicle_Asset_Type_Description" runat="server" Text="Vehicle sub-type"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="Label8" runat="server" Text="Make & Model"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="Label4" runat="server" Text="Identifier/Description"></asp:Label>
                                                </th>
                                                <th>Edit</th>
                                                <th>Insurance value</th>
                                                <th>Remove</th>

                                            </tr>
                                        </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblFinancer_Name" Text='<%# Eval("vcFinancer_Name") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label2" Text='<%# Eval("vcAsset_Cover") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblAsset_Type_Description" Text='<%# Eval("vcAsset_Type_Description") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblVehicle_Asset_Type_Description" Text='<%# Eval("vcAsset_Sub_Type_Description") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblakeModel" Text='<%# Eval("vcMakeModel") %>' />

                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblVin_Number" Text='<%# Eval("vcIdentifier") %>' />

                                                </td>


                                                <td class="action-td text-center">
                                                    <asp:LinkButton ID="ImageButton1" title="Update asset cover" runat="server" CommandName="VehicleCoverUpdate" CssClass="fa fa-edit mr-2" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") 
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID")  
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Sub_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcIdentifier")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Finance_Value")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Insurance_Value")
                                                                                        %>' />

                                                </td>
                                                <td class="action-td text-center">
                                                    <asp:LinkButton ID="LinkButton1" title="Update insurance value" runat="server" CommandName="ChangeInsuranceValue" CssClass="fa fa-dollar-sign mr-2" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") 
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID")  
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Sub_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcIdentifier")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Finance_Value")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Insurance_Value")
                                                                                        %>' />
                                                </td>
                                                <td class="action-td text-center">

                                                    <asp:LinkButton ID="ImageButton2" title="Remove asset from policy" runat="server" CommandName="RemoveAssetFromPolicy" CssClass="fa fa-trash mr-2" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "iPolicy_Id") 
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_ID")  
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "iAsset_Type_Id")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcAsset_Sub_Type_Description")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcMakeModel")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "vcIdentifier")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Finance_Value")
                                                                                        + ";" + DataBinder.Eval(Container.DataItem, "Insurance_Value")
                                                                                        %>' />

                                                </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>

                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlUpdateForm" runat="server" Visible="false">

                <div class="form-row align-items-center">
                    <div class="form-group">
                        <h6 class="m-0 font-weight-bold text-primary">Asset details</h6>
                        <div id="divAssetDetails" runat="server" class="detailsDiv">
                        </div>
                    </div>
                </div>
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Current cover: </label>
                        <asp:Label ID="lblCurrentCover" runat="server" Text=""></asp:Label>

                    </div>
                </div>
                <asp:Panel ID="pnlChangeCoverDetails" runat="server" Visible="false">
                    <h6 class="m-0 font-weight-bold text-primary">Change asset cover</h6>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Select new Cover</label>
                            <asp:DropDownList CssClass="form-control" ID="ddlNewCover" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlNewCover" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeCover_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator7" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">New insurance value</label>
                            <div class="input-group mb-2">
                                <div class="input-group-prepend">
                                    <div class="input-group-text">R</div>
                                </div>
                                <asp:TextBox CssClass="form-control" ID="txtInsurance_Cover_Value" runat="server" data-type="currency"></asp:TextBox>
                            </div>


                        </div>
                    </div>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                        </div>
                        <div class="form-group col-md-6">
                            <asp:RegularExpressionValidator ID="revDecimals" runat="server" ValidationGroup="vgChangeCover_details" ErrorMessage="Enter correct format value e.g. 100000,50" ControlToValidate="txtInsurance_Cover_Value" ValidationExpression="^\d{1,3}(,\d{3})*(\.\d+)?$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ControlToValidate="txtInsurance_Cover_Value" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeCover_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator8" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Date of change</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtDateOfChangeCover" runat="server" TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtDateOfChangeCover" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeCover_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator26" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <asp:Button CssClass="btn btn-primary" ID="btnSaveChangeCover" ValidationGroup="vgChangeCover_details" runat="server" Text="Update cover" OnClick="btnSaveChangeCover_Click" />
                    <asp:Button CssClass="btn btn-warning" ID="btnCancelChangeCover" runat="server" Text="Cancel" OnClick="btnCancelChangeCover_Click" />
                </asp:Panel>
                <asp:Panel ID="pnlChangeInsuranceValue" runat="server" Visible="false">
                    <h6 class="m-0 font-weight-bold text-primary">Change insurance value</h6>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">New Insurance value</label>
                            <div class="input-group mb-2">
                                <div class="input-group-prepend">
                                    <div class="input-group-text">R</div>
                                </div>
                                <asp:TextBox CssClass="form-control" ID="txtNewInsuranceValue" runat="server" data-type="currency"></asp:TextBox>
                            </div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="vgChangeInsurance_Value" ErrorMessage="Enter correct format value e.g. 100000,50" ControlToValidate="txtNewInsuranceValue" ValidationExpression="^\d{1,3}(,\d{3})*(\.\d+)?$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ControlToValidate="txtNewInsuranceValue" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeInsurance_Value" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator9" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Date of change</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtChangeInsurance_Value_Date" runat="server" TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtChangeInsurance_Value_Date" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeInsurance_Value" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator11" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>



                    <asp:Button CssClass="btn btn-primary" ID="btnSaveChangeInsurance_Value" ValidationGroup="vgChangeInsurance_Value" runat="server" Text="Update" OnClick="btnSaveChangeInsurance_Value_Click" />
                    <asp:Button CssClass="btn btn-warning" ID="btnCancelChangeInsurance_Value" runat="server" Text="Cancel" OnClick="btnCancelChangeInsurance_Value_Click" />
                </asp:Panel>
                <asp:Panel ID="pnlRemoveAssetFromPolicy" runat="server" Visible="false">
                    <h6 class="m-0 font-weight-bold text-primary">Remove asset</h6>
                    <div class="form-row align-items-center">

                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Reason for removal</label>
                            <asp:DropDownList CssClass="form-control" ID="ddlRemovalReasons" runat="server">

                                <asp:ListItem Value=""></asp:ListItem>
                                <asp:ListItem Value="Customer request">Customer request due to affordability</asp:ListItem>
                                <asp:ListItem Value="Change of ownership">Customer request due to change of ownership</asp:ListItem>
                                <asp:ListItem Value="Change of ownership">Asset damaged beyond repair</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlRemovalReasons" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgNon_payment_details" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator12" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-row align-items-center">
                        <div class="form-group col-md-6">
                            <label class="txtFieldLabel">Date of removal</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtRemoveAsset_Date" runat="server" TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtRemoveAsset_Date" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeInsurance_Value" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator22" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>



                    <asp:Button CssClass="btn btn-primary" ID="btnRemoveAsset" ValidationGroup="vgChangeInsurance_Value" runat="server" Text="Update" OnClick="btnRemoveAsset_Click" />
                    <asp:Button CssClass="btn btn-warning" ID="btnCancelRemoveAsset" runat="server" Text="Cancel" OnClick="btnCancelRemoveAsset_Click" />
                </asp:Panel>
            </asp:Panel>
            <asp:Button CssClass="btn btn-warning" ID="btnCancelUpdate" runat="server" Text="Cancel" OnClick="btnCancelUpdate_Click" />
        </asp:Panel>
        <asp:Panel ID="pnlPolicyStatus" runat="server" Visible="false">
            <h4 class="m-0 font-weight-bold text-secondary">Policy status</h4>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Current status</label>
                    <asp:Label ID="lblCurrentStatus" runat="server" Text=""></asp:Label>

                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Select status</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlPolicyStatus" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlPolicyStatus" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgStatusUpdate" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator3" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Date of change</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtDateOfChageStatus" runat="server" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtDateOfChageStatus" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgStatusUpdate" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator6" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                </div>
            </div>
            <asp:Button CssClass="btn btn-primary" ID="btnSavePolicyStatus" ValidationGroup="vgStatusUpdate" runat="server" Text="Update status" OnClick="btnSavePolicyStatus_Click" />
            <asp:Button CssClass="btn btn-warning" ID="btnCancelPolicyStatus" runat="server" Text="Cancel" OnClick="btnCancelPolicyStatus_Click" />

        </asp:Panel>
        <asp:Panel ID="pnlResumeCover" runat="server" Visible="false">
            <h4 class="m-0 font-weight-bold text-secondary">Resume policy cover</h4>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">

                    <label class="txtFieldLabel">Confirm that all outstanding premium(s) are up to date and all cover has beeen reinstated. </label>
                </div>

            </div>
            <asp:Button CssClass="btn btn-primary" ID="btnConfirmCoverResume" ValidationGroup="vgNon_payment_details" runat="server" Text="Update status" OnClick="btnConfirmCoverResume_Click" />
            <asp:Button CssClass="btn btn-warning" ID="btnCancelResumeCover" runat="server" Text="Cancel" OnClick="btnCancelResumeCover_Click" />
        </asp:Panel>
        <asp:Panel ID="pnlChangeAddress" runat="server" Visible="false">
            <h6 class="m-0 font-weight-bold text-primary">Physical address details</h6>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Building/Unit/House number</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtBuilding_Unit" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtBuilding_Unit" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator13" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Address line 1</label>


                    <asp:TextBox CssClass="form-control mb-2" ID="txtAddress_Line_1" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtAddress_Line_1" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator14" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Address line 2</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtAddress_Line_2" runat="server"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Suburb</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtSuburb" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtSuburb" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator15" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">City</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtCity" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtCity" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator16" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Province</label>

                    <asp:DropDownList CssClass="form-control" ID="ddlProvince" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlProvince" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator17" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Postal Code</label>

                    <asp:TextBox CssClass="form-control mb-2" ID="txtPostal_Code" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtPostal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator18" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <h6 class="m-0 font-weight-bold text-primary">Postal address details</h6>
                </div>
            </div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <div class="divCheckPadding">
                        <asp:CheckBox ID="chkPostalSameAsPhysical" AutoPostBack="true" TextAlign="Right" Text="Same as physical address" runat="server" OnCheckedChanged="chkPostalSameAsPhysical_CheckedChanged" />
                    </div>
                </div>
            </div>
            <asp:Panel ID="pnlPostalAddress" runat="server">
                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">PO Box/Bag</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtPOBox_Bag" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPOBox_Bag" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator19" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Post office</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtPost_Office_Name" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPost_Office_Name" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator20" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>


                <div class="form-row align-items-center">
                    <div class="form-group col-md-6">
                        <label class="txtFieldLabel">Postal code</label>

                        <asp:TextBox CssClass="form-control mb-2" ID="txtPost_Postal_Code" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPost_Postal_Code" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgChangeAddress" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator21" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </asp:Panel>
            <asp:Button CssClass="btn btn-primary" ID="BtnUpdateAddress" ValidationGroup="vgNon_payment_details" runat="server" Text="Update address" OnClick="BtnUpdateAddress_Click" />
            <asp:Button CssClass="btn btn-warning" ID="BtnCancelUpdateAddress" runat="server" Text="Cancel" OnClick="BtnCancelUpdateAddress_Click" />
        </asp:Panel>



    </div>


</asp:Content>

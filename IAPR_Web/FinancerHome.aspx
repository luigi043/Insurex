<%@ Page Language="C#" MasterPageFile="~/Financer.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="FinancerHome.aspx.cs" Inherits="IAPR_Web.FinancerHome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function toastSuccess(msg, title) {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-bottom-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            // toastr['success'](msg, title);
            var d = Date();
            toastr.success(msg, title);
            return false;
        }
    </script>
    <script type="text/javascript">
        function toastError(msg, title) {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-bottom-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            // toastr['success'](msg, title);
            var d = Date();
            toastr.error(msg, title);
            return false;
        }
    </script>
    <script type="text/javascript">
        function toastWarning(msg, title) {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-bottom-left",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            // toastr['success'](msg, title);
            var d = Date();
            toastr.warning(msg, title);
            return false;
        }
    </script>
    <div class="container-fluid">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="sidebar-html-div">
                    <asp:Panel ID="pdfPanel" runat="server">
                        <div class="row chartBlock">

                            <div class="form-group col-md-6">
                                <div class="row text-center">
                                    <div class="col-xl-12 col-md-6 mb-4">
                                        <div class="h6 mb-0 font-weight-bold text-primary text-center" style="padding-top: 15px;">
                                            All Assets
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center">
                                    <div class="col-xl-6 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex  text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            Total Assets                       
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblAllAssetCount" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-6 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold mb-1">
                                                            Total Finance Value
                       
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblAllAssetTotal" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center">
                                    <div class="col-xl-6 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex  text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            Insured Assets                       
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblInsuredAssetCount" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-6 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold mb-1">
                                                            Insured Financed Value
                       
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblInsuredAssetTotal" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center">


                                    <div class="col-xl-6 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex-Shortfall text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold mb-1">
                                                            Uninsured Assets
                       
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblUninsuredTotalAssetCount" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-6 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex-Shortfall text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            Uninsured Financed Value                        
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblUninsuredTotalAssetTotal2" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row text-center" style="display: none;">

                                    <div class="col-xl-12 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex-Shortfall  text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            Total Uninsured Financed Value
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-white">
                                                            <asp:Label ID="lblUninsuredTotalAssetTotal" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>
                            <div class="form-group col-md-6">
                                <div class="form-row align-items-center">

                                    <div id="div1" runat="server" class="form-group col-md-12">
                                        <asp:Literal ID="chartInsuraceStatus" runat="server"></asp:Literal>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="row chartBlock">
                            <div class="form-group col-md-6">
                                <div class="row text-center">
                                    <div class="col-xl-12 col-md-6 mb-4">
                                        <div class="h6 mb-0 font-weight-bold text-primary text-center" style="padding-top: 15px;">
                                            Uninsured Financed Value
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center">
                                    <div class="col-xl-7 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex  text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            Unpaid Premiums
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblPremiumUnpaidAssetTotal" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-5 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold mb-1">
                                                            Percentage
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblPremiumUnpaidAssetTotalPercent" runat="server" Text=""></asp:Label>

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center">
                                    <div class="col-xl-7 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex  text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            No Insurance Details
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblNoInsuranceAssetTotal" runat="server" Text=""></asp:Label>

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-5 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold mb-1">
                                                            Percentage
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblNoInsuranceAssetTotalPercent" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center" style="display:none;">

                                    <div class="col-xl-12 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex-Shortfall  text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            Insurance Shortfall                        
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-white">
                                                            <asp:Label ID="lblInsuredShortFall" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="form-group col-md-6">
                                <div class="form-row align-items-center">

                                    <div id="divchartUninsuredAssetsCount" runat="server" class="form-group  col-md-12">
                                        <asp:Literal ID="chartUninsuredAssetsCount" runat="server"></asp:Literal>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="row chartBlock">
                            <div class="form-group col-md-6">
                                <div class="row text-center">
                                    <div class="col-xl-12 col-md-6 mb-4">
                                        <div class="h6 mb-0 font-weight-bold text-primary text-center" style="padding-top: 15px;">
                                            Insured Financed Value
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center">
                                    <div class="col-xl-7 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex  text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            Adequately Insured Value
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblAdequatelyInsuredTotal" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-5 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold mb-1">
                                                            Percentage
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblAdequatelyInsuredTotalPercent" runat="server" Text=""></asp:Label>

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center">
                                    <div class="col-xl-7 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex-Shortfall  text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold  mb-1">
                                                            Under Insured Value
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblUnderInsuredTotal" runat="server" Text=""></asp:Label>

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-5 col-md-6 mb-4">
                                        <div>
                                            <div class="card-body bg-gradient-Insurex-Shortfall text-light">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold mb-1">
                                                            Percentage
                                                        </div>
                                                        <div class="h5 mb-0 font-weight-bold  text-light">
                                                            <asp:Label ID="lblUnderInsuredTotalPercent" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-group col-md-6">
                                <div class="form-row align-items-center">

                                    <div id="div2" runat="server" class="form-group  col-md-12">
                                        <asp:Literal ID="chartInsuredAssets" runat="server"></asp:Literal>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="row chartBlock">

                            <div id="divchartUninsuredStatistics" runat="server" class="form-group col-md-12">
                                <asp:Literal ID="chartUninsuredStatistics" runat="server"></asp:Literal>


                            </div>
                        </div>
                        <div class="row chartBlock">

                            <div id="divchartNonPaymentReInstatedByMonthFinanceValue" runat="server" class="form-group col-md-12">
                                <asp:Literal ID="chartNonPaymentReInstatedByMonthFinanceValue" runat="server"></asp:Literal>
                            </div>


                            <div class="form-group col-md-12">
                                <div class="table-responsive">
                                    <table class="table table-bordered graphTable text-dark  font-weight-bold" id="dataTable1">
                                        <thead class="text-light">
                                            <tr>
                                                <th></th>
                                                <htmlobject id="tbHeaderMonthsFinanceValue" runat="server" />
                                            </tr>
                                        </thead>

                                        <tbody class="text-dark">
                                            <htmlobject id="tbNonPaymentReInstatedByMonthFinanceValue" runat="server" />
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="row chartBlock">

                            <div id="divchartNonPaymentReInstatedByMonthAssetCount" runat="server" class="form-group col-md-12">
                                <asp:Literal ID="chartNonPaymentReInstatedByMonthAssetCount" runat="server"></asp:Literal>
                            </div>

                            <div class="form-group col-md-12">
                                <div class="table-responsive">
                                    <table class="table table-bordered graphTable text-dark font-weight-bold" id="dataTable2">
                                        <thead class="text-light">
                                            <tr>
                                                <th></th>
                                                <htmlobject id="tbHeaderMonthsAssetCount" runat="server" />
                                            </tr>
                                        </thead>

                                        <tbody>

                                            <htmlobject id="tbNonPaymentReInstatedByMonthAssetCount" runat="server" />



                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="row chartBlock">
                            <div id="divchartUninsuredByInsurer" runat="server" class="form-group col-md-12">
                                <asp:Literal ID="chartUninsuredByInsurer" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="row chartBlock">
                            <div id="divchartCommunicationsByFinancer" runat="server" class="form-group col-md-12">
                                <asp:Literal ID="chartCommunicationsByFinancer" runat="server"></asp:Literal>
                            </div>
                        </div>





                    </asp:Panel>
                </div>
                <asp:Button CssClass="btn btn-primary" ID="btnSendReport" ValidationGroup="vgMonthlyReport" runat="server" Text="Download" OnClick="btnDownloadDashboard_Click" />

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSendReport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

</asp:Content>

<%--   <div style="display: none">
        <div class="row">
            <div class="form-group col-md-12">
                <div id="divUninsured_Assest" runat="server" class="card   h-100 py-2 dashBackground">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="h8 mb-0 font-weight-bold text-primary mb-1">
                                    Uninsured assets
                       
                                </div>

                                <table class="table tableBlackFont text-right table-borderless mb-1" style="margin-left: 0px;">
                                    <thead>
                                        <tr>
                                            <th scope="col"></th>
                                            <th scope="col">Number of Assets</th>
                                            <th scope="col" style="border-right: 1px; border-right-style: solid">Asset Percentage</th>
                                            <th scope="col">Financed Value</th>
                                            <th scope="col">Financed Value Percentage</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <th scope="row">Premium Unpaid</th>
                                            <td class="text-right">
                                                <asp:Label ID="lblPremiumUnpaidAssetCount" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right" style="border-right: 1px; border-right-style: solid">
                                                <asp:Label ID="lblPremiumUnpaidAssetCountPercent" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right">
                                                <asp:Label ID="lblPremiumUnpaidAssetTotal1" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right">
                                                <asp:Label ID="lblPremiumUnpaidAssetTotalPercent1" runat="server" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <th scope="row">No Insurance Details</th>
                                            <td class="text-right">
                                                <asp:Label ID="lblNoInsuranceAssetCount" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right" style="border-right: 1px; border-right-style: solid">
                                                <asp:Label ID="lblNoInsuranceAssetCountPercent" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right">
                                                <asp:Label ID="lblNoInsuranceAssetTotal1" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right">
                                                <asp:Label ID="lblNoInsuranceAssetTotalPercent1" runat="server" Text=""></asp:Label></td>
                                        </tr>
                                        <tr class="table-warning font-weight-bold">
                                            <th scope="row">Total</th>
                                            <td class="text-right text-danger">
                                                <asp:Label ID="lblUninsuredTotalAssetCount1" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right text-danger" style="border-right: 1px; border-right-style: solid; border-color: black;">
                                                <asp:Label ID="lblUninsuredTotalAssetCountPercent" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right text-danger">
                                                <asp:Label ID="lblUninsuredTotalAssetTotal1" runat="server" Text=""></asp:Label></td>
                                            <td class="text-right text-danger">
                                                <asp:Label ID="lblUninsuredTotalAssetTotalPercent1" runat="server" Text=""></asp:Label></td>
                                        </tr>
                                    </tbody>
                                </table>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-md-6">
                <div id="divInsured_Value" class="card    h-100 py-2 dashBackground">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="h5 mb-0 font-weight-bold text-primary mb-1">
                                    Insured Assets

                                </div>
                                <table class="table tableBlackFont text-right table-borderless font-weight-bold mb-1" style="margin-left: 0px;">

                                    <thead>
                                        <tr>

                                            <th scope="col">Number of Assets</th>

                                            <th scope="col">Insurance Value</th>
                                            <th scope="col">Insurance Shortfall</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>

                                            <td class="table-success text-right">
                                                <asp:Label ID="lblInsuredAssetCount1" runat="server" Text=""></asp:Label></td>
                                            <td class="table-success  text-right">
                                                <asp:Label ID="lblInsuredAssetTotal1" runat="server" Text=""></asp:Label></td>
                                            <td class="table-warning  text-right  text-danger">
                                                <asp:Label ID="lblInsuredShortFall1" runat="server" Text=""></asp:Label></td>
                                        </tr>

                                    </tbody>
                                </table>
                            </div>

                        </div>

                    </div>

                </div>

            </div>
            <div class="form-group col-md-6">
                <div id="divUninsured_Value" class="card    h-100 py-2 dashBackground">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="h5 mb-0 font-weight-bold text-primary mb-1">
                                    All assets

                                </div>
                                <table class="table tableBlackFont text-right table-borderless font-weight-bold mb-1">

                                    <thead>
                                        <tr>

                                            <th scope="col">Number of Assets</th>

                                            <th scope="col">Financed Value</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>

                                            <td>
                                                <asp:Label ID="lblAllAssetCount1" runat="server" Text=""></asp:Label></td>

                                            <td>
                                                <asp:Label ID="lblAllAssetTotal1" runat="server" Text=""></asp:Label></td>
                                        </tr>

                                    </tbody>
                                </table>


                            </div>

                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>--%>
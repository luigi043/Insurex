<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="IAPR_Web.AdminHome" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid" style="background-color: #eff1f9;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                

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
                        <div class="row text-center" style="display: none;">

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
                    <div id="divchartUninsuredByFinancer" runat="server" class="form-group col-md-12">
                        <asp:Literal ID="chartUninsuredByFinancer" runat="server"></asp:Literal>
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



                <asp:Button CssClass="btn btn-primary" ID="btnSendReport" ValidationGroup="vgMonthlyReport" runat="server" Text="Download" OnClick="btnDownloadDashboard_Click" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSendReport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--**************************OLD table and charts*****************************************--%>
    <script type="text/javascript">
        function ShowCurrentTime() {
            $.ajax({
                type: "POST",
                url: "/AdminHome.aspx/getAdminTable",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d + "fail");
                }
            });
        }
        function OnSuccess(response) {
            alert(response.d + "suc");
        }
        //Sys.WebForms.PageRequestManager.getInstance(),

        //$(document).ready(function () {
        //    $.ajax({
        //        type: "POST",
        //        async: true,
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "JSON",
        //        url: "http://localhost:1945/ServicePage.aspx/getAdminTable",
        //        data: "",
        //        success: function (response) {
        //            console.log(response.d);
        //            console.log("Hello World1 " + response.d);
        //            alert("Hello World1 " + response.d);
        //        },
        //        error: function (response) {
        //        }

        //    });

        //})
    </script>
</asp:Content>

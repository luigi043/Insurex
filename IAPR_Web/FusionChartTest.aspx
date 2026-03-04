<%@ Page Language="C#" MasterPageFile="~/NonUser.Master" AutoEventWireup="true" CodeBehind="FusionChartTest.aspx.cs" Inherits="IAPR_Web.FusionChartTest" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div id="chart-container">FusionCharts XT will load here!</div>

    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            <div class="row">
            <div class="col-xl-3 col-md-6 mb-4">
                <div id="divUninsured_Assest" runat="server" class="card border-left-danger shadow h-100 py-2 dashBackground">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                    Uninsured assets
                       
                                </div>
                                <div class="h5 mb-0 font-weight-bold text-dark-800">
                                    <asp:Label ID="lblUninsuredAssetsTotal" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <%-- <div class="col-auto">
                                <i class="fas fa-building fa-2x text-gray-300"></i>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-md-6 mb-4">
                <div id="divUninsured_Value" runat="server" class="card border-left-danger shadow h-100 py-2 dashBackground">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                    Uninsured value
                       
                                </div>
                                <div class="h5 mb-0 font-weight-bold text-dark-800">
                                    <asp:Label ID="lblUninsuredValue" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <%--<div class="col-auto">
                                <i class="fas fa-money-bill-alt fa-2x text-gray-300"></i>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-primary shadow h-100 py-2 dashBackground">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div
                                    class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                    Total assets
                       
                                </div>
                                <div class="h5 mb-0 font-weight-bold text-dark-800">
                                    <asp:Label ID="lblTotaAssets" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <%-- <div class="col-auto">
                                <i class="fas fa-car fa-2x text-gray-300"></i>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-primary shadow h-100 py-2 dashBackground">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div
                                    class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                    Total financed value
                       
                                </div>
                                <div class="h5 mb-0 font-weight-bold text-dark-800">
                                    <asp:Label ID="lblTotalFinanceValue" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <%--<div class="col-auto">
                                <i class="fas fa-money-bill-alt fa-2x text-gray-300"></i>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row align-items-center">
            <div class="form-group col-md-12">
                <asp:Literal ID="chartUninsuredByFinancer" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="form-row align-items-center">
            <div class="form-group col-md-6">
                <asp:Literal ID="chartNonPaymentReInstatedByMonth" runat="server"></asp:Literal>
            </div>
            <div class="form-group col-md-6">
                <asp:Literal ID="chartUninsuredvsUnconfirmed" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="form-row align-items-center">


            <div class="form-group col-md-6">
                <asp:Literal ID="ChartUninsuredAssetPercentage" runat="server"></asp:Literal>
            </div>
            <div class="form-group col-md-6">
                <asp:Literal ID="CommunicationsByFinancer" runat="server"></asp:Literal>
            </div>
        </div>
</asp:Content>
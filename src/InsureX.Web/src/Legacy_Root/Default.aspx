<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IAPR_Web._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <asp:DropDownList ID="ddlCountries" runat="server">
    </asp:DropDownList>
    <hr />
    <cc1:PieChart ID="PieChart1" runat="server" ChartHeight="300" ChartWidth="300"
        charttype="Column" ChartTitleColor="#0E426C">
         <PieChartValues>
        <cc1:PieChartValue Category="United States" Data="30" PieChartValueColor="#0E426C" />
        <cc1:PieChartValue Category="India" Data="5" PieChartValueColor="#D08AD9" />
        <cc1:PieChartValue Category="France" Data="8" PieChartValueColor="#B85B3E" />
        <cc1:PieChartValue Category="Germany" Data="9" PieChartValueColor="#FFC652" />
        <cc1:PieChartValue Category="United Kingdom" Data="22" PieChartValueColor="#6586A7" />
        <cc1:PieChartValue Category="Australia" Data="18" PieChartValueColor="#669900" />
        <cc1:PieChartValue Category="Russia" Data="8" PieChartValueColor="#4508A2" />

         </PieChartValues>
    </cc1:PieChart>

</asp:Content>

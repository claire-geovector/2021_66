<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductPage.aspx.cs" Inherits="Production_ProductPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

     <br/>產品詳情：<br/><br/>

    編號：
        <asp:TextBox ID="textProductNumber" runat="server" Width="190px" Height="25px" MaxLength="25" />
        <asp:TextBox ID="textProductId" runat="server" Enabled="false" Visible="false" /><br/>	

    品名：
        <asp:TextBox ID="textProductName" runat="server" Width="190px" Height="25px" MaxLength="50" /><br/>	

</asp:Content>


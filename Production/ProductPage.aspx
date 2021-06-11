<%@ Page Title="產品詳情" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductPage.aspx.cs" Inherits="Production_ProductPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

     <br/>產品詳情：<br/><br/>

    編號：
        <asp:TextBox ID="textProductNumber" runat="server" Width="190px" Height="25px" MaxLength="25" />
        <asp:TextBox ID="textOriginalProductNumber" runat="server" Enabled="false" Visible="false" /><br/>	
        <asp:TextBox ID="textProductId" runat="server" Enabled="false" Visible="false" /><br/>	

    品名：
        <asp:TextBox ID="textProductName" runat="server" Width="190px" Height="25px" MaxLength="50" /><br/><br/>

    安全庫存量：
        <asp:TextBox ID="textSafetyStockLevel" runat="server" Width="190px" Height="25px" MaxLength="5" />
    
    	<br/>	

            <div style="text-align:center;">
                <asp:Button ID="savedata" runat="server" Height="25px" Text="儲存" Width="66px" OnClick="savedata_Click" />
                <asp:Button ID="goback" runat="server" Height="25px" Text="回目錄" Width="66px" OnClick="goback_Click" />
            </div>

</asp:Content>


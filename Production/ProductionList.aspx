<%@ Page Title="產品目錄" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductionList.aspx.cs" Inherits="Production_ProductionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <br/>
    <div>
    <table border="0" style="min-width:300px;height:50px;">
        <tr style="height:50px;"><td>
            編號：<asp:TextBox ID="textSearchNumber" runat="server" Width="190px" Height="25px" MaxLength="25" />
            <td>
        </tr>
        <tr style="height:50px;"><td>
            品名：<asp:TextBox ID="textSearchName" runat="server" Width="190px" Height="25px" MaxLength="50" />
            <td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSearch" runat="server" Height="25px" Text="搜尋" Width="76px" OnClick="btnSearch_Click" />
                <asp:Button ID="btnAll" runat="server" Height="25px" Text="清空" Width="76px" OnClick="btnAll_Click" />
            <td>
        </tr>
    </table>
    </div>
    <hr />
    <br/>產品目錄：　
        <asp:Button ID="btnNewProduct" runat="server" Height="25px" Text="新增產品" Width="76px" OnClick="btnNewProduct_Click" />
    <br/><br/>

            <asp:GridView ID="gvProductionList" runat="server" CssClass="graytable user AutoNewLine" style="border-collapse:collapse; font-family:'微軟正黑體'"
                OnPageIndexChanging="gvProductionList_PageIndexChanging"
                AutoGenerateColumns="False" AllowSorting="True"
                EmptyDataText="沒有符合查詢條件的資料" EmptyDataRowStyle-BackColor ="White" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-BorderWidth="0"
                GridLines="None" DataKeyNames="ProductID" PageSize="20" AllowPaging="True" PagerStyle-CssClass="pgr" >
                
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="流水號" SortExpression="ProductID" InsertVisible="false" ReadOnly ="true" Visible ="false" />
                    <asp:BoundField DataField="ProductNumber" HeaderText="編號" SortExpression="ProductNumber" />
                    <asp:BoundField DataField="ProductName" HeaderText="品名" SortExpression="ProductName" />
                    <asp:BoundField DataField="SafetyStockLevel" HeaderText="安全庫存量" SortExpression="SafetyStockLevel" />
                    
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="BtnProductItem" runat="server" CommandArgument='<%# Eval("ProductID") %>' OnClick="BtnProductItem_Click" Text="詳情" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="BtnProductDel" runat="server" CommandArgument='<%# Eval("ProductID") %>' OnClick="BtnProductDel_Click" Text="刪除" OnClientClick="return confirm('確認刪除這筆資料?')" />
                        </ItemTemplate>
                    </asp:TemplateField>
            
                </Columns>
                <PagerSettings LastPageText="最後一頁" FirstPageText="第一頁" Mode="NumericFirstLast" NextPageText="下一頁" PreviousPageText="上一頁" />
            </asp:GridView>

</asp:Content>


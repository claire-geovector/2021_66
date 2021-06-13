using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Production_ProductionList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //產品清單資訊
            setProductionList();
        }
    }

    private void setProductionList()
    {
        DataTable OBJ_GV01 = (DataTable)ViewState["GV01"];
        DataTable DTGV01 = new DataTable();
        DTGV01.Columns.Add(new DataColumn("ProductID", typeof(string)));
        DTGV01.Columns.Add(new DataColumn("ProductName", typeof(string)));
        DTGV01.Columns.Add(new DataColumn("ProductNumber", typeof(string)));
        DTGV01.Columns.Add(new DataColumn("SafetyStockLevel", typeof(string)));
        
        ViewState["GV01"] = DTGV01;
        OBJ_GV01 = DTGV01;

        #region pageSearch
        string searchNumber = textSearchNumber.Text;
        string searchName = textSearchName.Text;
        string searchStr = "";
        if (searchNumber.Trim() != "")
        {
            if (searchStr == "") {
                searchStr = " Where ProductNumber like @ProductNumber ";
            } else
            {
                searchStr = " and ProductNumber like @ProductNumber ";
            }
        }
        if (searchName.Trim() != "")
        {
            if (searchStr == "")
            {
                searchStr = " Where Name like @Name ";
            }
            else
            {
                searchStr = " and Name like @Name ";
            }
        }
        #endregion

        string sqlStrProductionList = " select * FROM [AdventureWorks2016].[Production].[Product] ";
        sqlStrProductionList += searchStr;
        sqlStrProductionList += "order by ProductNumber;";

        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AdvWConnStr"];
        using (SqlConnection ProductConn = new SqlConnection(connectionString.ConnectionString))
        {
            ProductConn.Open();
            using (var cmd = ProductConn.CreateCommand())
            {
                cmd.CommandText = sqlStrProductionList;
                #region pageSearch
                if (searchNumber.Trim() != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@ProductNumber", "%"+searchNumber+"%"));
                }
                if (searchName.Trim() != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@Name", "%" + searchName + "%"));
                }
                #endregion
                cmd.ExecuteNonQuery();
                using (SqlDataReader readerAW = cmd.ExecuteReader())
                {
                    if (readerAW.HasRows)
                    {
                        while (readerAW.Read())
                        {
                            string tmpProductID = readerAW["ProductID"] + "";
                            string tmpName = readerAW["Name"] + "";
                            string tmpProductNumber = readerAW["ProductNumber"] + "";
                            string tmpSafetyStockLevel = readerAW["SafetyStockLevel"] + "";

                            DataRow dr01 = OBJ_GV01.NewRow();

                            dr01["ProductID"] = tmpProductID;
                            dr01["ProductName"] = tmpName;
                            dr01["ProductNumber"] = tmpProductNumber;
                            dr01["SafetyStockLevel"] = tmpSafetyStockLevel;

                            OBJ_GV01.Rows.Add(dr01);
                            ViewState["GV01"] = OBJ_GV01;
                        }
                    }
                    readerAW.Close();
                }
                cmd.Cancel();
            }
        }
        gvProductionList.DataSource = OBJ_GV01;
        gvProductionList.DataBind();
    }
    protected void BtnProductItem_Click(object sender, EventArgs e)
    {
        Button LButton = (Button)sender;
        string LINK = "ProductPage.aspx?ProductID=" + LButton.CommandArgument;

        Response.Redirect(LINK);
    }
    protected void BtnProductDel_Click(object sender, EventArgs e)
    {
        Button LButton = (Button)sender;
        string productId =  LButton.CommandArgument;

        string sqlDelProduct = " DELETE [AdventureWorks2016].[Production].[ProductInventory]  WHERE ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[ProductProductPhoto] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[BillOfMaterials] where ComponentID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[BillOfMaterials] where ProductAssemblyID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Purchasing].[ProductVendor] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Purchasing].[PurchaseOrderDetail] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[TransactionHistory] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[ProductCostHistory] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[ProductListPriceHistory] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[WorkOrderRouting] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[WorkOrder] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Sales].[SalesOrderDetail] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Sales].[SpecialOfferProduct] where ProductID=@ProductID; ";
        sqlDelProduct += " DELETE [AdventureWorks2016].[Production].[Product] where ProductID=@ProductID; ";
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AdvWConnStr"];
        using (SqlConnection AwConn = new SqlConnection(connectionString.ConnectionString))
        {
            AwConn.Open();
            using (var cmd = AwConn.CreateCommand())
            {
                cmd.CommandText = sqlDelProduct;
                cmd.Parameters.Add(new SqlParameter("@ProductID", productId));
                cmd.ExecuteNonQuery();
                cmd.Cancel();
            }
        }
        //Response.Redirect(Request.Url.ToString());
        Response.Write("<script>alert('資料已刪除');location=location;</script>");
    }

    protected void btnNewProduct_Click(object sender, EventArgs e)
    {
        string LINK = "ProductPage.aspx?ProductID=NewProduct";

        Response.Redirect(LINK);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        setProductionList();
    }

    protected void btnAll_Click(object sender, EventArgs e)
    {
        textSearchNumber.Text = "";
        textSearchName.Text = "";
        setProductionList();
    }

    protected void gvProductionList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductionList.PageIndex = e.NewPageIndex;

        gvProductionList.DataSource = ViewState["GV01"];
        gvProductionList.DataBind();
    }
}
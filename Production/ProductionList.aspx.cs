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

        string sqlStrProductionList = " select * FROM [AdventureWorks2016].[Production].[Product]; ";
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AdvWConnStr"];

        using (SqlConnection ProductConn = new SqlConnection(connectionString.ConnectionString))
        {
            ProductConn.Open();
            SqlDataReader readerItem;
            SqlCommand objCmdItem = new SqlCommand(sqlStrProductionList, ProductConn);
            readerItem = objCmdItem.ExecuteReader();

            while (readerItem.Read())
            {
                string tmpProductID = readerItem["ProductID"] + "";
                string tmpName = readerItem["Name"] + "";
                string tmpProductNumber = readerItem["ProductNumber"] + "";
                string tmpSafetyStockLevel = readerItem["SafetyStockLevel"] + "";

                DataRow dr01 = OBJ_GV01.NewRow();

                dr01["ProductID"] = tmpProductID;
                dr01["ProductName"] = tmpName;
                dr01["ProductNumber"] = tmpProductNumber;
                dr01["SafetyStockLevel"] = tmpSafetyStockLevel;

                OBJ_GV01.Rows.Add(dr01);
                ViewState["GV01"] = OBJ_GV01;

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

        string sqlDelProduct = " delete [AdventureWorks2016].[Production].[Product] where ProductID=@ProductID; ";
        //sqlDelProduct += " delete [AdventureWorks2016].[Production].[BillOfMaterials] where ProductID=@ProductID; ";
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
        Response.Redirect(Request.Url.ToString());
    }

    protected void btnNewProduct_Click(object sender, EventArgs e)
    {
        string LINK = "ProductPage.aspx?ProductID=NewProduct";

        Response.Redirect(LINK);
    }
}
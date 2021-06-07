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
            //產品清單資訊
            setProductionList();
    }

    private void setProductionList()
    {
        DataTable OBJ_GV01 = (DataTable)ViewState["GV01"];
        DataTable DTGV01 = new DataTable();
        DTGV01.Columns.Add(new DataColumn("ProductID", typeof(string)));
        DTGV01.Columns.Add(new DataColumn("ProductName", typeof(string)));
        DTGV01.Columns.Add(new DataColumn("ProductNumber", typeof(string)));
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

                DataRow dr01 = OBJ_GV01.NewRow();

                dr01["ProductID"] = tmpProductID;
                dr01["ProductName"] = tmpName;
                dr01["ProductNumber"] = tmpProductNumber;

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
}
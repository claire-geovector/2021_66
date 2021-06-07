using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Production_ProductPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string productId = Request["ProductID"] + "";

        if (!IsPostBack) {
            SetProductPage(productId);
        }
    }

    private void SetProductPage(string productId)
    {
        string sqlStrProductionInfo = " select * FROM [AdventureWorks2016].[Production].[Product] where ProductID=@ProductID; ";
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AdvWConnStr"];
        using (SqlConnection AWConn = new SqlConnection(connectionString.ConnectionString))
        {
            AWConn.Open();
            using (var cmd = AWConn.CreateCommand())
            {
                cmd.CommandText = sqlStrProductionInfo;
                #region.設定值
                cmd.Parameters.Add(new SqlParameter("@ProductID", productId));
                #endregion
                cmd.ExecuteNonQuery();

                using (SqlDataReader readerAW = cmd.ExecuteReader())
                {
                    if (readerAW.HasRows)
                        while (readerAW.Read())
                        {
                            textProductId.Text = readerAW["ProductID"] + "";
                            textProductNumber.Text= readerAW["ProductNumber"] +"";
                            textProductName.Text = readerAW["Name"] + "";    
                        }
                    readerAW.Close();
                }
                cmd.Cancel();
            }
        }
    }

    protected void goback_Click(object sender, EventArgs e)
    {
        string LINK = "ProductionList.aspx";
        Response.Redirect(LINK);
    }

    protected void savedata_Click(object sender, EventArgs e)
    {
        string pgProductID = textProductId.Text;
        string pgProductNumber = textProductNumber.Text;
        string pgProductName = textProductName.Text;
        string ShowMsg = string.Empty;

        if (pgProductNumber.Trim() =="") {
            ShowMsg = "編號為必填請輸入資料";
            Response.Write("<script>alert('" + ShowMsg + "');</script>");
            return;
        }
        if (pgProductName.Trim() == "")
        {
            ShowMsg = "品名為必填請輸入資料";
            Response.Write("<script>alert('" + ShowMsg + "');</script>");
            return;
        }
        if (ShowMsg == "") {

            string sqlDelProduct = " update [AdventureWorks2016].[Production].[Product] set ProductNumber=@ProductNumber,Name=@Name where ProductID=@ProductID; ";
            //sqlDelProduct += " delete [AdventureWorks2016].[Production].[BillOfMaterials] where ProductID=@ProductID; ";
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AdvWConnStr"];
            using (SqlConnection HeoConn = new SqlConnection(connectionString.ConnectionString))
            {
                HeoConn.Open();
                using (var cmd = HeoConn.CreateCommand())
                {
                    cmd.CommandText = sqlDelProduct;
                    cmd.Parameters.Add(new SqlParameter("@ProductID", pgProductID));
                    cmd.Parameters.Add(new SqlParameter("@ProductNumber", pgProductNumber));
                    cmd.Parameters.Add(new SqlParameter("@Name", pgProductName));
                    cmd.ExecuteNonQuery();
                    cmd.Cancel();
                }
            }
            ShowMsg = "資料已存檔";
            Response.Write("<script>alert('" + ShowMsg + "');</script>");
            return;
        }
    }
}
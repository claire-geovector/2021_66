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
        bool goList = true;
        if (productId == "NewProduct") {
            goList = false;
            textProductId.Text = "NewProduct";

        } else if(productId.Trim()!="")
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
                        {
                            goList = false;
                            while (readerAW.Read())
                            {
                                textProductId.Text = readerAW["ProductID"] + "";
                                textProductNumber.Text = readerAW["ProductNumber"] + "";
                                textOriginalProductNumber.Text = readerAW["ProductNumber"] + "";
                                textProductName.Text = readerAW["Name"] + "";
                                textSafetyStockLevel.Text = readerAW["SafetyStockLevel"] + "";
                            }
                        }
                        readerAW.Close();
                    }
                    cmd.Cancel();
                }
            }
        }
        if (goList) {
            Response.Redirect("ProductionList.aspx");
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
        string pgOriginalProductNumber = textOriginalProductNumber.Text;
        string pgProductName = textProductName.Text;
        string pgSafetyStockLevel = textSafetyStockLevel.Text;
        int intSafetyStockLevel=0;
        string ShowMsg = string.Empty;

        if (pgProductNumber.Trim() =="") {
            ShowMsg = "編號為必填請輸入資料";
            Response.Write("<script>alert('" + ShowMsg + "');</script>");
            return;
        }
        else if (pgProductNumber != pgOriginalProductNumber && chkHavProductNumber(pgProductNumber))
        {
            ShowMsg = "已有相同編號，請修正";
            Response.Write("<script>alert('" + ShowMsg + "');</script>");
            return;
        }
        if (pgProductName.Trim() == "")
        {
            ShowMsg = "品名為必填請輸入資料";
            Response.Write("<script>alert('" + ShowMsg + "');</script>");
            return;
        }
        if (pgSafetyStockLevel.Trim() == "")
        {
            ShowMsg = "安全庫存量為必填請輸入資料";
            Response.Write("<script>alert('" + ShowMsg + "');</script>");
            return;
        }
        else
        {
            intSafetyStockLevel = strToInt(pgSafetyStockLevel, 0, 30000);
            if (intSafetyStockLevel < 0)
            {
                ShowMsg = "請輸入正確安全庫存量 (0~30000)";
                Response.Write("<script>alert('" + ShowMsg + "');</script>");
                return;
            }
        }

        if (ShowMsg == "") {
            if (pgProductID == "NewProduct")
            {
                string sqlDelProduct = " insert into [AdventureWorks2016].[Production].[Product]  (ProductNumber,[Name],SafetyStockLevel,ReorderPoint,StandardCost,ListPrice,DaysToManufacture,SellStartDate) ";
                sqlDelProduct += "values (@ProductNumber,@Name,@SafetyStockLevel,750,0,0,0,getdate());  ";
                //sqlDelProduct += " delete [AdventureWorks2016].[Production].[BillOfMaterials] where ProductID=@ProductID; ";
                ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AdvWConnStr"];
                using (SqlConnection AWConn = new SqlConnection(connectionString.ConnectionString))
                {
                    AWConn.Open();
                    using (var cmd = AWConn.CreateCommand())
                    {
                        cmd.CommandText = sqlDelProduct;
                        cmd.Parameters.Add(new SqlParameter("@ProductNumber", pgProductNumber));
                        cmd.Parameters.Add(new SqlParameter("@Name", pgProductName));
                        cmd.Parameters.Add(new SqlParameter("@SafetyStockLevel", intSafetyStockLevel));
                        
                        cmd.ExecuteNonQuery();
                        cmd.Cancel();
                    }
                }
                ShowMsg = "資料已存檔";

            } else
            {
                if (chkHavProductId(pgProductID))
                {
                    string sqlDelProduct = " update [AdventureWorks2016].[Production].[Product] set ProductNumber=@ProductNumber,Name=@Name,SafetyStockLevel=@SafetyStockLevel where ProductID=@ProductID; ";
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
                            cmd.Parameters.Add(new SqlParameter("@SafetyStockLevel", intSafetyStockLevel));
                            cmd.ExecuteNonQuery();
                            cmd.Cancel();
                        }
                    }
                    ShowMsg = "資料已存檔";
                }
                else
                {
                    ShowMsg = "資料已被刪除，請確認資料狀態";
                }
            }
            Response.Write("<script>alert('" + ShowMsg + "');</script>");
            return;
        }
    }

    private int strToInt(string pgSafetyStockLevel, int miniValue, int maxValue)
    {
        int rValue=-1;

        try
        {
            rValue = Convert.ToInt32(pgSafetyStockLevel);
            if (rValue < miniValue || rValue > maxValue)
                rValue = -1;
        }
        catch { rValue = -1; }
        return rValue;
    }

    #region 檢查是產品ID是否存在
    private bool chkHavProductId(string pgProductID)
    {
        bool rValue = false;
        string sqlStrProductionInfo = " select * FROM [AdventureWorks2016].[Production].[Product] where ProductID=@ProductID; ";
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AdvWConnStr"];
        using (SqlConnection AWConn = new SqlConnection(connectionString.ConnectionString))
        {
            AWConn.Open();
            using (var cmd = AWConn.CreateCommand())
            {
                cmd.CommandText = sqlStrProductionInfo;
                #region.設定值
                cmd.Parameters.Add(new SqlParameter("@ProductID", pgProductID));
                #endregion
                cmd.ExecuteNonQuery();

                using (SqlDataReader readerAW = cmd.ExecuteReader())
                {
                    if (readerAW.HasRows)
                    {
                        rValue = true;
                    }
                    readerAW.Close();
                }
                cmd.Cancel();
            }
        }
        return rValue;
    }
    #endregion
    #region 檢查是產品編號是否存在
    private bool chkHavProductNumber(string pgProductNumber)
    {
        bool rValue = false;

        string sqlStrProductionInfo = " select * FROM [AdventureWorks2016].[Production].[Product] where ProductNumber=@ProductNumber; ";
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AdvWConnStr"];
        using (SqlConnection AWConn = new SqlConnection(connectionString.ConnectionString))
        {
            AWConn.Open();
            using (var cmd = AWConn.CreateCommand())
            {
                cmd.CommandText = sqlStrProductionInfo;
                #region.設定值
                cmd.Parameters.Add(new SqlParameter("@ProductNumber", pgProductNumber));
                #endregion
                cmd.ExecuteNonQuery();

                using (SqlDataReader readerAW = cmd.ExecuteReader())
                {
                    if (readerAW.HasRows)
                    {
                        rValue = true;
                    }
                    readerAW.Close();
                }
                cmd.Cancel();
            }
        }

        return rValue;
    }
    #endregion
}
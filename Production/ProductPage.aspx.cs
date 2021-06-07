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
}
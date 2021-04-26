using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using AdvantShop.CMS;
using AdvantShop.Core;
using AdvantShop.Diagnostics;

public partial class Admin_UserControls_ProductsStatistics : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try // Product info
        {
            using (var db = new SQLDataAccess())
            {
                SqlCommand cmd = db.cmd;
                cmd.CommandText = "SELECT COUNT(ProductID) AS COUNTID FROM [Catalog].[Product]";
                cmd.CommandType = CommandType.Text;
                db.cnOpen();
                lblCommonProductsCOUNT.Text = cmd.ExecuteScalar().ToString();

                //-------------------------------------------

                //.CommandText = "SELECT COUNT([Product].ID) AS COUNTID FROM [Catalog].[Product] inner join Catalog.ProductCategories on ProductCategories.ProductID=Product.ID  WHERE (Enabled = '1')"
                cmd.CommandText =
                    "Select count(ProductID) AS COUNTID from (SELECT distinct ([Product].ProductID)  FROM [Catalog].[Product] inner join Catalog.ProductCategories on ProductCategories.ProductID=Product.ProductID  WHERE Enabled = \'1\') as temp";
                cmd.CommandType = CommandType.Text;
                lblCommonEnableProductCOUNT.Text = cmd.ExecuteScalar().ToString();

                //-------------------------------------------

                cmd.CommandText = "SELECT COUNT(CategoryID) AS COUNTID FROM [Catalog].[Category] WHERE CategoryID<>\'0\'";
                cmd.CommandType = CommandType.Text;
                lblProductCategoryCOUNT.Text = cmd.ExecuteScalar().ToString();

                //-------------------------------------------

                cmd.CommandText = "SELECT COUNT(ReviewId) AS COUNTID FROM [CMS].[Review] WHERE Type = @Type";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Type", (int)EntityType.Product);
                lblProductPostCOUNT.Text = cmd.ExecuteScalar().ToString();
                db.cnClose();
            }
            //-------------------------------------------
        }
        catch (Exception ex)
        {
            //MsgErr(ex.Message + " atLoad Product info");
            lblHeadProduct.Text += " (False)";
            Debug.LogError(ex);
        }
    }
}
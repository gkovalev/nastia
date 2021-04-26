//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using Resources;
using System.Data;

public partial class Admin_UserControls_Settings_MailSettings : System.Web.UI.UserControl
{
    public string ErrMessage = string.Empty; 

    protected void Page_Load(object sender, EventArgs e)
    {
    
    }

    
    private void ExecuteScript(string script)
    {
        string strResult = "True";
        txtMessage.Text = script;
        try
        {

            AdvantShop.Core.SQLDataAccess.ExecuteNonQuery(script, CommandType.Text);
        }
        catch (Exception ex)
        {
            strResult = ex.Message;
        }

        if (strResult.Equals("True"))
        {
            MsgErr(Resource.Admin_CommonSettings_TestEmail_Success, true);
        }
        else
        {
            MsgErr(strResult, false);
        }

    }

    private void ExecuteStoredProcedure(string script)
    {
        string strResult = "True";
        txtMessage.Text = script;
        try
        {

            AdvantShop.Core.SQLDataAccess.ExecuteNonQuery(script, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            strResult = ex.Message;
        }

        if (strResult.Equals("True"))
        {
            MsgErr(Resource.Admin_CommonSettings_TestEmail_Success, true);
        }
        else
        {
            MsgErr(strResult, false);
        }

    }

    private void MsgErr(string strMessageText, bool isSucces)
    {
        const string strSuccesFormat = "<div class=\"label-box good\">{0} // at {1}</div>";
        const string strFailFormat = "<div class=\"label-box error\">{0} // at {1}</div>";

        Message.Visible = true;

        if (isSucces)
        {
            Message.Text = string.Format(strSuccesFormat, strMessageText, DateTime.Now.ToString());
        }
        else
        {
            Message.Text = string.Format(strFailFormat, strMessageText, DateTime.Now.ToString());
        }

    }
    protected void btnToZeroAllExceptDremel_Click(object sender, EventArgs e)
    {
        string script = "UPDATE     Catalog.Offer SET              Amount = 0 FROM         Catalog.Product INNER JOIN Catalog.Offer ON Catalog.Product.ProductId = Catalog.Offer.ProductID WHERE     (Catalog.Product.BrandID <> 47)";
        ExecuteScript(script); 
    }
    protected void btnAllToZero_Click(object sender, EventArgs e)
    {

        string script = "UPDATE     Catalog.Offer SET              Amount = 0 FROM         Catalog.Product INNER JOIN Catalog.Offer ON Catalog.Product.ProductId = Catalog.Offer.ProductID";
        ExecuteScript(script);  
    }
    protected void btnToZeroAllExceptDremelAndSpareParts_Click(object sender, EventArgs e)
    {
        string script = "UPDATE     Catalog.Offer SET              Amount = 0 FROM         Catalog.Product INNER JOIN Catalog.Offer ON Catalog.Product.ProductId = Catalog.Offer.ProductID WHERE   (Catalog.Product.BrandID <> 47 AND SubBrandId <> 20)";
        ExecuteScript(script); 
    }
    protected void btnToZeroDremelOnly_Click(object sender, EventArgs e)
    {
        string script = "UPDATE     Catalog.Offer SET              Amount = 0 FROM         Catalog.Product INNER JOIN Catalog.Offer ON Catalog.Product.ProductId = Catalog.Offer.ProductID WHERE     (Catalog.Product.BrandID = 47)";
        ExecuteScript(script); 
    }
    protected void btnUpdateYandexMarket_Click(object sender, EventArgs e)
    {
        string script = "/*This Script Update Catalog For Yandeks.Market. Only available products with price more than 600.000BYR and accesories only (Subbrandid 2 or 6) */ ";
        script += "DELETE FROM         Settings.ExportFeedSelectedProducts WHERE ModuleName like 'YandexMarket' INSERT INTO Settings.ExportFeedSelectedProducts SELECT     'YandexMarket' AS Expr1, Catalog.Offer.ProductID FROM         Catalog.Offer INNER JOIN  Catalog.Product ON Catalog.Offer.ProductID = Catalog.Product.ProductId WHERE     (Catalog.Offer.Amount > 0) AND (Catalog.Offer.Price > 600000) AND (Catalog.Product.SubBrandId IN (2,6)) "; 
        ExecuteScript(script); 
    }
    protected void btnUpdateCategSorting_Click(object sender, EventArgs e)
    {
        string script = "";
        script += "[Catalog].[sp_UpdateSortingInCategories]";
        ExecuteStoredProcedure(script); 
    }
}
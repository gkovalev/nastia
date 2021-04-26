using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;
using AdvantShop.Diagnostics;
using AdvantShop.Payment;
using AdvantShop.Shipping;

public partial class Admin_UserControls_OrderStatistics : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try // Orders info
        {
            //-------------------------------------------

            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "[Order].[sp_GetOrderAvg]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.cnOpen();
                using(SqlDataReader reader = db.cmd.ExecuteReader())
                while (reader.Read())
                {
                    if (SQLDataHelper.GetString(reader, "flag") == "ByDay")
                    {
                        lblAvOrders.Text = SQLDataHelper.GetString(reader, "Count") ?? "0";
                    }
                    else
                    {
                        lblAvItems.Text = SQLDataHelper.GetString(reader, "Count") ?? "0";
                    }
                }
                
                db.cmd.CommandText = "SELECT avg([Sum]) FROM [Order].[Order] WHERE [PaymentDate] is not null";
                db.cmd.CommandType = CommandType.Text;
                object value = db.cmd.ExecuteScalar();
                if (value != DBNull.Value)
                    lblAvCheck.Text = AdvantShop.Catalog.CatalogService.GetStringPrice((decimal)(value));
                else
                    lblAvCheck.Text = AdvantShop.Catalog.CatalogService.GetStringPrice(0);
                db.cmd.CommandText = "select top 1 [PaymentMethodID] from [Order].[Order] where [PaymentDate] is not null group by [PaymentMethodID] order by count(*) desc";
                value = db.cmd.ExecuteScalar();

                if (value != DBNull.Value && value != null)
                {
                    LblPopPayment.Text = PaymentService.GetPaymentMethod((int)value).Name;
                }

                db.cmd.CommandText = "select top 1 [ShippingMethodID] from [Order].[Order]  where [PaymentDate] is not null group by [ShippingMethodID] order by count(*) desc";
                value = db.cmd.ExecuteScalar();
                if (value != DBNull.Value && value != null)
                {
                    LblPopShipping.Text = ShippingMethodService.GetShippingMethod((int)value).Name;
                }


                //db.cmd.CommandText = "select (sum([Sum]) - SUM([SupplyTotal]) - sum([TaxCost])- sum([ShippingCost])) as 'Profit' , sum([Sum]) as 'Sum' from [Order].[Order] WHERE [PaymentDate] is not null";
                //db.cmd.CommandText = "select (1 -  (SUM([SupplyTotal]) /( sum([Sum]) - sum([TaxCost]) - sum([ShippingCost])))) *100 from [Order].[Order] WHERE [PaymentDate] is not null";
                db.cmd.CommandText = "select case when sum([Sum]) - sum([TaxCost]) - sum([ShippingCost])= 0 then 0 else (1 -  (SUM([SupplyTotal]) /( sum([Sum]) - sum([TaxCost]) - sum([ShippingCost])))) *100 end from [Order].[Order] WHERE [PaymentDate] is not null";
                //(sum([Sum] - sum([TaxCost]) - [Shipping] + [ExtraCharge])/([Sum] - [Tax] - [Shipping])*100 as 'Profitability'

                value = db.cmd.ExecuteScalar();
                if (value != DBNull.Value && value != null)
                {
                    LblProfitability.Text = Convert.ToDecimal(value).ToString("F2") + "%";
                }
                else
                {
                    LblProfitability.Text = @"0%";
                }


                db.cmd.CommandText = "select top 1 [Country] from [Order].[OrderContact] inner join [Order].[Order] on [Order].[ShippingContactID] = [OrderContact].[OrderContactID] WHERE [OrderID] in (select top 1 [OrderID] from [Order].[Order] where [PaymentDate] is not null ) group by [Country] order by count (*) desc";
                value = db.cmd.ExecuteScalar();
                if (value != DBNull.Value && value != null)
                    LblCountry.Text = (string)(value);
                db.cmd.CommandText = "select top 1 [Zone] from [Order].[OrderContact] inner join [Order].[Order] on [Order].[ShippingContactID] = [OrderContact].[OrderContactID] WHERE [OrderID] in (select top 1 [OrderID] from [Order].[Order] where [PaymentDate] is not null ) group by [Zone] order by count (*) desc";
                value = db.cmd.ExecuteScalar();
                if (value != DBNull.Value && value != null)
                    LblRegion.Text = (string)(value);

                db.cmd.CommandText = "[Order].[sp_GetUserPercent]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                decimal unreg = 200;
                value = db.cmd.ExecuteScalar();
                if (value != DBNull.Value)
                    unreg = Convert.ToDecimal(value);
                if (unreg == 200)
                {
                    LblReg.Text = @"0%";
                    LblUnreg.Text = @"0%";
                }
                else
                {
                    LblReg.Text = (100 - unreg).ToString("F0") + '%';
                    LblUnreg.Text = unreg.ToString("F0") + '%';
                }
                db.cnClose();
            }
        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + " atLoad Orders Info");
            Debug.LogError(ex);
        }
    }
    private static void MsgErr(string messageText)
    {
        //Parent.FindControl("Message").Visible = true;
        //((TextBox)Parent.FindControl("Message")).Text += "<br/>" + messageText + "<br/>";
    }
}
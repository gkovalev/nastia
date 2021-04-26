using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Diagnostics;

public partial class Admin_UserControls_CustomersStatistics : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try // Customers info
        {
            using (var db = new SQLDataAccess())
            {
                SqlCommand cmd = db.cmd;
                cmd.CommandText = "SELECT COUNT(Email) AS COUNTID FROM [Customers].[Customer]";
                cmd.CommandType = CommandType.Text;
                db.cnOpen();
                string userCount = cmd.ExecuteScalar().ToString();
                lblRegUserCOUNT.Text = string.IsNullOrEmpty(userCount) ? "0 -" : userCount + @" - ";

                // Today------------------------------------------

                cmd.CommandText = "SELECT COUNT(*) AS COUNTID FROM [Customers].[Customer] WHERE DATEADD(dd, 0, DATEDIFF(dd, 0, [RegistrationDateTime])) = DATEADD(dd, 0, DATEDIFF(dd, 0, Getdate()))";
                cmd.CommandType = CommandType.Text;
                userCount = cmd.ExecuteScalar().ToString();
                lblTodayRegUserCOUNT.Text = string.IsNullOrEmpty(userCount) ? "0 -" : userCount + @" - ";

                
                // News-------------------------------------------

                cmd.CommandText = "SELECT COUNT(ID) AS COUNTID FROM Subscribe WHERE (ActivateDate <> \'\')";
                cmd.CommandType = CommandType.Text;

                int i = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText =
                    "SELECT COUNT(CustomerID) AS COUNTID FROM Customers.Customer WHERE (Subscribed4News = \'True\')";
                cmd.CommandType = CommandType.Text;

                int j = Convert.ToInt32(cmd.ExecuteScalar());
                
                userCount = (i + j).ToString();
                lblSubscribeUserCOUNT.Text = string.IsNullOrEmpty(userCount) ? "0 -" : userCount + @" - ";
                // With orders------------------------------------------
                cmd.CommandText = "Select count(*) from  ( SELECT [CustomerID], Count(OrderID) as tt FROM [Order].[OrderCustomer] group by [CustomerID] having Count(OrderID)  > 1) as t";
                cmd.CommandType = CommandType.Text;
                userCount = cmd.ExecuteScalar().ToString();
                lblUsersWithOrder.Text = string.IsNullOrEmpty(userCount) ? "0 -" : userCount + @" - ";
                db.cnClose();
            }
            //-------------------------------------------
        }
        catch (Exception ex)
        {
            //MsgErr(ex.Message + " atLoad Customers Info");
            lblHeadCustomers.Text += @" (False)";
            Debug.LogError(ex);
        }
    }
}
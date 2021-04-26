//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using AdvantShop.Core;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AjaxControlToolkit;
using Debug = AdvantShop.Diagnostics.Debug;

public partial class UserControls_ThaiRating : UserControl
{
    private int _productId = -1;
    public int ProductId
    {
        get
        {
            return _productId;
        }
        set
        {
            _productId = value;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        LoadRaiting();
        if (ThaiRating.ReadOnly)
        {
            SetFadeOut(Resources.Resource.Client_Details_VotingDisabled);
        }
        //ViewState[UniqueID + "_productId"] = _productId;
        if (CustomerSession.CurrentCustomer.RegistredUser)
            try
            {
                var userId = CustomerSession.CurrentCustomer.Id ;
                if (SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_GetCOUNT_Ratio]", CommandType.StoredProcedure, new SqlParameter("@CustomerId", userId), new SqlParameter("@ProductID", ProductId)) >= 1)
                {
                    SetFadeOut(Resources.Resource.Client_Details_AlreadyVoted);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
    }

    private void SetFadeOut(string message)
    {
        thaiRatingAuthBlock.Visible = true;
        lblAuthRating.Text = message;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (!CustomerSession.CurrentCustomer.RegistredUser)
        {
            SetFadeOut(Resources.Resource.Client_ThaiRating);
        }
        else
        {
            thaiRatingAuthBlock.Visible = false;
        }
    }

    private void LoadRaiting()
    {
        if (_productId != -1)
        {
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "SELECT [Ratio],[EnabledRatio],[ShowRatio] FROM [Catalog].[Product] WHERE ProductID = @ProductID";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@ProductID", ProductId);

                    db.cnOpen();
                    var read = db.cmd.ExecuteReader();
                    read.Read();
                    if (read.HasRows)
                    {
                        ThaiRating.CurrentRating = SQLDataHelper.GetInt(read, "Ratio");

                        ThaiRating.Visible = SQLDataHelper.GetBoolean(read, "ShowRatio");
                        pnlRatio.Visible = ThaiRating.Visible;
                        ThaiRating.ReadOnly = !SQLDataHelper.GetBoolean(read, "EnabledRatio");
                        read.Close();

                        db.cmd.CommandText = "[Catalog].[sp_GetCOUNTRatioByProductID]";
                        db.cmd.CommandType = CommandType.StoredProcedure;
                        db.cmd.Parameters.Clear();
                        db.cmd.Parameters.AddWithValue("@ProductID", ProductId);

                        lblSummRating.Text = @"(" + SQLDataHelper.GetString(db.cmd.ExecuteScalar()) + @" " +
                                             Resources.Resource.Client_Details_Votes + @")";
                    }
                    db.cnClose();
                }

            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }

    protected void ThaiRating_Changed(object sender, RatingEventArgs e)
    {
        try
        {
            if (_productId == -1)
            {
                return;
            }

            if (!CustomerSession.CurrentCustomer.RegistredUser)
            {
                lblRatingInfo.Visible = true;
                lblRatingInfo.Text = Resources.Resource.Client_Details_VotesCanRegUsers;
                return;
            }

            var userId = CustomerSession.CurrentCustomer.Id;

            using (var db = new SQLDataAccess())
            {
                //--------------------------------------------------
                db.cmd.CommandText = "[Catalog].[sp_GetCOUNT_Ratio]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@CustomerId", userId);
                db.cmd.Parameters.AddWithValue("@ProductID", ProductId);

                db.cnOpen();
                if (SQLDataHelper.GetInt(db.cmd.ExecuteScalar()) >= 1)
                {
                    db.cnClose();
                    ThaiRating.ReadOnly = true;
                    lblRatingInfo.Visible = true;
                    return;
                }

                //--------------------------------------------------
                db.cmd.CommandText = "[Catalog].[sp_AddRatio]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@ProductID", ProductId);
                db.cmd.Parameters.AddWithValue("@ProductRatio", Convert.ToInt32(e.Value));
                db.cmd.Parameters.AddWithValue("@CustomerId", userId);

                db.cmd.ExecuteNonQuery();
                //--------------------------------------------------

                db.cmd.CommandText = "[Catalog].[sp_GetCOUNTRatioByProductID]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@ProductID", ProductId);

                lblSummRating.Text = @"(" + SQLDataHelper.GetString(db.cmd.ExecuteScalar()) + @" " + Resources.Resource.Client_Details_Votes + @")";

                //--------------------------------------------------

                db.cmd.CommandText = "[Catalog].[sp_GetAVGRatioByProductID]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@ProductID", ProductId);

                int intNewRatioValue = SQLDataHelper.GetInt(db.cmd.ExecuteScalar());
                ThaiRating.CurrentRating = intNewRatioValue;


                //--------------------------------------------------

                db.cmd.CommandText = "[Catalog].[sp_UpdateProductRatio]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@ProductID", ProductId);
                db.cmd.Parameters.AddWithValue("@Ratio", intNewRatioValue);

                db.cmd.ExecuteNonQuery();

                db.cnClose();
            }

            ThaiRating.ReadOnly = true;
            lblRatingInfo.Visible = true;
            lblRatingInfo.Text = Resources.Resource.Client_Details_YourVoiceTaken;

        }
        catch (Exception ex)
        {
            //Debug.LogError(ex, sender, e);
            Debug.LogError(ex);
        }
    }
}
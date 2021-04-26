//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.SEO;
using Resources;

public partial class SendRequestOnProduct : AdvantShopPage
{
    private Product _product;
    protected Product product
    {
        get
        {
            if (_product == null)
            {
                int productId = 0;
                if (int.TryParse(Request["productid"], out productId))
                {
                    _product = ProductService.GetProduct(productId);
                }
            }

            return _product;
        }
    }

    protected bool ProductCanOrderByRequest
    {
        get { return ((product != null) && (product.OrderByRequest) && (product.Offers[0].Amount <= 0)); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (product == null)
        {
            Error404();
        }

        if (!product.CanOrderByRequest)
        {
            lblMessage.Text = Resource.Client_OrderByRequest_CantBeOrdered;
            MultiView1.SetActiveView(ViewResult);
            return;
        }

        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_OrderByRequest)), string.Empty);

        if (!IsPostBack)
        {
            MultiView1.SetActiveView(ViewForm);
            if (CustomerSession.CurrentCustomer.RegistredUser)
            {
                txtName.Text = string.Format("{0} {1}", CustomerSession.CurrentCustomer.FirstName, CustomerSession.CurrentCustomer.LastName);
                txtEmail.Text = CustomerSession.CurrentCustomer.EMail;
                txtPhone.Text = CustomerSession.CurrentCustomer.Phone;
            }

        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        bool boolIsValidPast = true;
        boolIsValidPast &= IsValidText(txtName.Text);
        boolIsValidPast &= IsValidText(txtEmail.Text);
        boolIsValidPast &= IsValidText(txtPhone.Text);

        int quantity = 0;
        if (!Int32.TryParse(txtAmount.Text, out quantity) || (quantity < 1))
        {
            boolIsValidPast = false;
        }

        if (!ValidationHelper.IsValidEmail(txtEmail.Text.Trim()))
        {
            boolIsValidPast = false;
        }

        if (!CaptchaControl1.IsValid())
        {
            CaptchaControl1.TryNew();
            boolIsValidPast = false;
        }

        if (boolIsValidPast == false)
        {
            ShowMessage(Notify.NotifyType.Error, Resource.Client_Feedback_WrongData);
            return;
        }

        try
        {
            var orderByRequest = new OrderByRequest
                                     {
                                         ProductId = product.ID,
                                         ProductName = product.Name,
                                         ArtNo = product.ArtNo,
                                         Quantity = quantity,
                                         UserName = txtName.Text,
                                         Email = txtEmail.Text,
                                         Phone = txtPhone.Text,
                                         Comment = txtComment.Text,
                                         IsComplete = false,
                                         RequestDate = DateTime.Now
                                     };

            OrderByRequestService.AddOrderByRequest(orderByRequest);

            var clsParam = new ClsMailParamOnOrderByRequest
                               {
                                   OrderByRequestId = orderByRequest.OrderByRequestId.ToString(CultureInfo.InvariantCulture),
                                   ArtNo = product.ArtNo,
                                   ProductName = product.Name,
                                   Quantity = quantity.ToString(CultureInfo.InvariantCulture),
                                   UserName = txtName.Text,
                                   Email = txtEmail.Text,
                                   Phone = txtPhone.Text,
                                   Comment = txtComment.Text
                               };

            string message = SendMail.BuildMail(clsParam);

            SendMail.SendMailNow(txtEmail.Text, Resource.Client_OrderByRequest_PreOrder, message, true);
            SendMail.SendMailNow(SettingsMail.EmailForOrders, Resource.Client_OrderByRequest_PreOrder, message, true);

            lblMessage.Text = Resource.Client_Feedback_MessageSent;
            MultiView1.SetActiveView(ViewResult);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            ShowMessage(Notify.NotifyType.Error, Resource.Client_Feedback_MessageError);
            MultiView1.SetActiveView(ViewResult);
        }
    }

    private static bool IsValidText(String textBox)
    {
        return !string.IsNullOrEmpty(textBox.Trim());
    }
}
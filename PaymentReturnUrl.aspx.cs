using System;
using System.Web.UI;
using AdvantShop.Payment;

public partial class PaymentReturnUrl : Page
{
    protected int PaymentMethodID
    {
        get
        {
            int id;
            return int.TryParse(Request["paymentmethodid"], out id) ? id : 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (PaymentMethodID < 1) 
            return;
        
        var method = PaymentService.GetPaymentMethod(PaymentMethodID);
        if (method != null && (method.NotificationType & NotificationType.ReturnUrl) == NotificationType.ReturnUrl)
            lblResult.Text = method.ProcessResponse(Context);
        else
            lblResult.Text = NotificationMessahges.InvalidRequestData;
    }
}
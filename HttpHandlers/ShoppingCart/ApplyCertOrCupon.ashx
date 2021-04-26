<%@ WebHandler Language="C#" Class="ApplyCertOrCupon" %>

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Customers;

public class ApplyCertOrCupon : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        if (context.Request["code"].IsNotEmpty())
        {
            string code = context.Request["code"].Trim();
            CustomerGroup customerGroup = CustomerSession.CurrentCustomer.CustomerGroup;
            
            if (customerGroup.CustomerGroupId != CustomerGroupService.DefaultCustomerGroup)
                return;

            var cert = GiftCertificateService.GetCertificateByCode(code);
            var coupon = CouponService.GetCouponByCode(code);

            if (SettingsOrderConfirmation.EnableGiftCertificateService && cert != null && cert.Paid && !cert.Used && cert.Enable)
            {
                GiftCertificateService.AddCustomerCertificate(cert.CertificateId);
            }
            else if (coupon != null && (coupon.ExpirationDate == null || coupon.ExpirationDate > DateTime.Now) && (coupon.PossibleUses == 0 || coupon.PossibleUses > coupon.ActualUses) && coupon.Enabled)
            {
                CouponService.AddCustomerCoupon(coupon.CouponID);
            }
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
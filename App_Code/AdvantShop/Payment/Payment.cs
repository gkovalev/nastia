//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Diagnostics;

namespace AdvantShop.Payment
{
    public enum PaymentType
    {
        SberBank = 1, //check_sberbank.aspx
        Bill = 2, //check_bill.aspx 
        Cash = 3, //doing nothing
        MailRu = 4, //No tested
        WebMoney = 5, //not tested
        Robokassa = 6, //Resources
        YandexMoney = 7, //not tested
        AuthorizeNet = 8, //not tested
        GoogleCheckout = 9, //not tested
        eWAY = 10, //not tested
        Check = 11, //check_check.aspx
        PayPal = 12, //working
        TwoCheckout = 13, //not tested
        Assist = 14, //not tested
        ZPayment = 15, // admin page + test
        Platron = 16, // not tested
        Rbkmoney = 17, // not tested
        CyberPlat = 18,
        Moneybookers = 19, // not tested
        AmazonSimplePay = 20,
        ChronoPay = 21,
        PayOnline = 22, // tested, debuged 14.02.2013
        QIWI = 23,
        PSIGate = 24,
        PayPoint = 25,
        SagePay = 26,
        WorldPay = 27,
        CashOnDelivery = 28,
        PickPoint = 29,
        OnPay = 30, // tested
        WalletOneCheckout = 31,
        GiftCertificate = 32,
        MasterBank = 33,
        Interkassa = 34,
        LiqPay = 35,
        BillUa = 36,
        Kupivkredit = 37,
        YesCredit = 38
    }

    public enum ProcessType
    {
        None,
        FormPost,
        Javascript,
        PageRedirect
    }
    [Flags]
    public enum NotificationType
    {
        None = 0x0,
        ReturnUrl = 0x1,
        Handler = 0x2
    }
    [Flags]
    public enum UrlStatus
    {
        None = 0x0,
        ReturnUrl = 0x1,
        CancelUrl = 0x2,
        FailUrl = 0x4,
        NotificationUrl = 0x8
    }

    public static class NotificationMessahges
    {
        public static string InvalidRequestData
        {
            get { return Resources.Resource.Client_Payment_InvalidRequestData; }
        }

        public static string TestMode
        {
            get { return Resources.Resource.Client_Payment_TestModeEnabled; }
        }

        public static string Fail
        {
            get { return Resources.Resource.Client_Payment_OrderProcessingFailed; }
        }

        public static string LogError(Exception ex)
        {
            Debug.LogError(ex);
            return Resources.Resource.Client_Payment_ProcessingFailedWithException + ex.Message;
        }

        public static string SuccessfullPayment(string orderNumber)
        {
            return string.Format(Resources.Resource.Client_Payment_OrderSuccessfullyPaid, orderNumber);
        }
    }

    public enum PaperPaymentType
    {
        NonPaperMethod = 0,
        SberBank,
        Bill,
        Check,
        BillUa
    }

    public static class PaymentTypeExtension
    {
        public static Type GetClass(this PaymentType type)
        {
            switch (type)
            {
                case PaymentType.SberBank:
                    return typeof(SberBank); //check_sberbank.aspx
                case PaymentType.Bill:
                    return typeof(Bill); //check_bill.aspx 
                case PaymentType.Cash:
                    return typeof(Cash); //doing nothing
                case PaymentType.MailRu:
                    return typeof(MailRu); //No tested
                case PaymentType.WebMoney:
                    return typeof(WebMoney); //not tested
                case PaymentType.MasterBank:
                    return typeof(MasterBank); //not tested
                case PaymentType.Robokassa:
                    return typeof(Robokassa); //Resources
                case PaymentType.YandexMoney:
                    return typeof(YandexMoney); //not tested
                case PaymentType.AuthorizeNet:
                    return typeof(AuthorizeNet); //not tested
                case PaymentType.GoogleCheckout:
                    return typeof(GoogleCheckout); //not tested
                case PaymentType.eWAY:
                    return typeof(eWAY); //not tested
                case PaymentType.Check:
                    return typeof(Check); //check_check.aspx
                case PaymentType.PayPal:
                    return typeof(PayPal); //working
                case PaymentType.TwoCheckout:
                    return typeof(TwoCheckout); //not tested
                case PaymentType.Assist:
                    return typeof(Assist); //not tested
                case PaymentType.ZPayment:
                    return typeof(ZPayment); // admin page + test
                case PaymentType.Platron:
                    return typeof(Platron); // not tested
                case PaymentType.Rbkmoney:
                    return typeof(Rbkmoney); // not tested
                case PaymentType.CyberPlat:
                    return typeof(CyberPlat);
                case PaymentType.Moneybookers:
                    return typeof(Moneybookers); // not tested
                case PaymentType.AmazonSimplePay:
                    return typeof(AmazonSimplePay);
                case PaymentType.ChronoPay:
                    return typeof(ChronoPay);
                case PaymentType.PayOnline:
                    return typeof(PayOnline);
                case PaymentType.QIWI:
                    return typeof(Qiwi);
                case PaymentType.PSIGate:
                    return typeof(PSIGate);
                case PaymentType.PayPoint:
                    return typeof(PayPoint);
                case PaymentType.SagePay:
                    return typeof(SagePay);
                case PaymentType.WorldPay:
                    return typeof(WorldPay);
                case PaymentType.OnPay:
                    return typeof(OnPay);
                case PaymentType.WalletOneCheckout:
                    return typeof(WalletOneCheckout);
                case PaymentType.Interkassa:
                    return typeof(Interkassa);
                case PaymentType.LiqPay:
                    return typeof(LiqPay);
                case PaymentType.BillUa:
                    return typeof(BillUa);
                case PaymentType.Kupivkredit:
                    return typeof(Kupivkredit);
                case PaymentType.YesCredit:
                    return typeof(YesCredit);
                default:
                    return typeof(PaymentMethod);
            }
        }
    }
}
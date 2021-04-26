//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop.Mails
{
    public abstract class ClsMailParam
    {
        public abstract MailType Type { get; }

        public virtual string FormatString(string formatedStr)
        {
            return string.Empty;
        }
    }

    public class ClsMailParamOnRegistration : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnRegistration; } }

        public string ShopURL { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegDate { get; set; }
        public string Password { get; set; }
        public string Subsrcibe { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#FIRSTNAME#", FirstName);
            formatedStr = formatedStr.Replace("#LASTNAME#", LastName);
            formatedStr = formatedStr.Replace("#REGDATE#", RegDate);
            formatedStr = formatedStr.Replace("#PASSWORD#", Password);
            formatedStr = formatedStr.Replace("#SUBSRCIBE#", Subsrcibe);
            formatedStr = formatedStr.Replace("#SHOPURL#", ShopURL);
            return formatedStr;
        }
    }

    public class ClsMailParamOnPwdRepair : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnPwdRepair; } }

        public string RecoveryCode { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#EMAIL#", Email);
            formatedStr = formatedStr.Replace("#RECOVERYCODE#", RecoveryCode);
            formatedStr = formatedStr.Replace("#LINK#", Link);
            return formatedStr;
        }
    }

    public class ClsMailParamOnNewOrder : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnNewOrder; } }

        public string CustomerContacts { get; set; }
        public string ShippingMethod { get; set; }
        public string PaymentType { get; set; }
        public string OrderTable { get; set; }
        public string CurrentCurrencyCode { get; set; }
        public string TotalPrice { get; set; }
        public string Comments { get; set; }
        public string Email { get; set; }
        public string OrderID { get; set; }
        public string Number { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#CUSTOMERCONTACTS#", CustomerContacts);
            formatedStr = formatedStr.Replace("#SHIPPINGMETHOD#", ShippingMethod);
            formatedStr = formatedStr.Replace("#PAYMENTTYPE#", PaymentType);
            formatedStr = formatedStr.Replace("#ORDERTABLE#", OrderTable);
            formatedStr = formatedStr.Replace("#CURRENTCURRENCYCODE#", CurrentCurrencyCode);
            formatedStr = formatedStr.Replace("#TOTALPRICE#", TotalPrice);
            formatedStr = formatedStr.Replace("#COMMENTS#", Comments);
            formatedStr = formatedStr.Replace("#EMAIL#", Email);
            formatedStr = formatedStr.Replace("#ORDER_ID#", OrderID);
            formatedStr = formatedStr.Replace("#NUMBER#", Number);
            return formatedStr;
        }
    }

    public class ClsMailParamOnChangeOrderStatus : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnChangeOrderStatus; } }

        public string OrderID { get; set; }
        public string OrderStatus { get; set; }
        public string StatusComment { get; set; }

        public string Number { get; set; }
        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#ORDERID#", OrderID);
            formatedStr = formatedStr.Replace("#ORDERSTATUS#", OrderStatus);
            formatedStr = formatedStr.Replace("#STATUSCOMMENT#", StatusComment);
            formatedStr = formatedStr.Replace("#NUMBER#", Number);

            //Changed By Evgeni 
            if (OrderStatus == "Доставлен")
            {
                // formatedStr += "<br> Благодарим, Вас за недавнюю покупку в нашем магазине электроинструментов BOSCH www.instrument-opt.by. <br> Мы стараемся удовлетворить все потребности и желания наших клиентов. <br> <br> Мы будем Вам благодарны, если Вы расскажете о своих впечатлениях от заказа товара в нашем магазине тут (http://instrument-opt.by/module/storereviews). <br> Удачных Вам покупок в нашем интернет-магазине."; 
                formatedStr = "Добрый день.<br> Интернет-магазин instrument-opt.by благодарит Вас за покупку. <br>Надеемся, что Вы остались довольны своим новым приобретением.<br> <br>  Мы будем очень благодарны, если Вы оставите свое мнение о работе нашего магазина и купленном товаре. Вы поможете другим покупателям определиться с выбором, оставив свой отзыв тут: (http://instrument-opt.by/module/storereviews). <br> Нам очень важно Ваше мнение. <br> <br> С уважением,<br> Администрация магазина instrument-opt.by"; 

                /*Благодарим, Вас за недавнюю покупку в нашем магазине электроинструментов BOSCH www.instrument-opt.by.
Надеемся, что Вы остались довольны своим новым приобретением, и будем очень благодарны, если Вы оставите свое мнение о работе нашего магазина и купленном товаре. Вы поможете другим покупателям определиться с выбором, оставив свой отзыв тут: http://instrument-opt.by/module/storereviews
Нам очень важно Ваше мнение.*/
            }

            return formatedStr;
        }
    }


    public class ClsMailParamOnSendMessage : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnSendMessage; } }

        public string Name { get; set; }
        public string MessageText { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#MESSAGETEXT#", MessageText);
            formatedStr = formatedStr.Replace("#NAME#", Name);
            return formatedStr;
        }
    }

    public class ClsMailParamOnSubscribeDeactivate : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnSubscribeDeactivate; } }

        public string Link { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#LINK#", Link);
            return formatedStr;
        }
    }

    public class ClsMailParamOnSubscribeActivate : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnSubscribeActivate; } }

        public string Link { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#LINK#", Link);
            return formatedStr;
        }
    }

    public class ClsMailParamOnFeedback : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnFeedback; } }

        public string ShopUrl { get; set; }
        public string ShopName { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string SubjectMessage { get; set; }
        public string TextMessage { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#SHOPURL#", ShopUrl);
            formatedStr = formatedStr.Replace("#STORE_NAME#", ShopName);
            formatedStr = formatedStr.Replace("#USERNAME#", UserName);
            formatedStr = formatedStr.Replace("#USEREMAIL#", UserEmail);
            formatedStr = formatedStr.Replace("#USERPHONE#", UserPhone);
            formatedStr = formatedStr.Replace("#SUBJECTMESSAGE#", SubjectMessage);
            formatedStr = formatedStr.Replace("#TEXTMESSAGE#", TextMessage);
            return formatedStr;
        }
    }

    public class ClsMailParamOnProductDiscuss : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnProductDiscuss; } }

        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string ProductLink { get; set; }
        public string Author { get; set; }
        public string DateString { get; set; }
        public string Text { get; set; }
        public string DeleteLink { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", ProductName);
            formatedStr = formatedStr.Replace("#PRODUCTLINK#", ProductLink);
            formatedStr = formatedStr.Replace("#SKU#", SKU);
            formatedStr = formatedStr.Replace("#AUTHOR#", Author);
            formatedStr = formatedStr.Replace("#DATE#", DateString);
            formatedStr = formatedStr.Replace("#DELETELINK#", DeleteLink);
            formatedStr = formatedStr.Replace("#TEXT#", Text);
            return formatedStr;
        }
    }

    public class ClsMailParamOnQuestionAboutProduct : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnQuestionAboutProduct; } }

        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string ProductLink { get; set; }
        public string Author { get; set; }
        public string DateString { get; set; }
        public string Text { get; set; }
        public string UserMail { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#AUTHOR#", Author);
            formatedStr = formatedStr.Replace("#DATE#", DateString);
            formatedStr = formatedStr.Replace("#TEXT#", Text);
            formatedStr = formatedStr.Replace("#USERMAIL#", UserMail);
            formatedStr = formatedStr.Replace("#SKU#", SKU);
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", ProductName);
            formatedStr = formatedStr.Replace("#PRODUCTLINK#", ProductLink);
            return formatedStr;
        }
    }

    public class ClsMailParamOnSendFriend : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnSendFriend; } }

        public string To { get; set; }
        public string Url { get; set; }
        public string From { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#FROM#", From);
            formatedStr = formatedStr.Replace("#URL#", Url);
            formatedStr = formatedStr.Replace("#TO#", To);
            return formatedStr;
        }
    }

    public class ClsMailParamOnOrderByRequest : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnOrderByRequest; } }

        public string OrderByRequestId { get; set; }
        public string ArtNo { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#ORDERID#", OrderByRequestId);
            formatedStr = formatedStr.Replace("#ARTNO#", ArtNo);
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", ProductName);
            formatedStr = formatedStr.Replace("#QUANTITY#", Quantity);
            formatedStr = formatedStr.Replace("#USERNAME#", UserName);
            formatedStr = formatedStr.Replace("#EMAIL#", Email);
            formatedStr = formatedStr.Replace("#PHONE#", Phone);
            formatedStr = formatedStr.Replace("#COMMENT#", Comment);
            return formatedStr;
        }
    }

    public class ClsMailParamOnSendLinkByRequest : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnSendLinkByRequest; } }

        public string OrderByRequestId { get; set; }
        public string UserName { get; set; }
        public string ArtNo { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Code { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#NUMBER#", OrderByRequestId);
            formatedStr = formatedStr.Replace("#USERNAME#", UserName);
            formatedStr = formatedStr.Replace("#ARTNO#", ArtNo);
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", ProductName);
            formatedStr = formatedStr.Replace("#QUANTITY#", Quantity);
            formatedStr = formatedStr.Replace("#LINK#", SettingsMain.SiteUrl + "/orderproduct.aspx?code=" + Code);

            return formatedStr;
        }
    }

    public class ClsMailParamOnSendFailureByRequest : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnSendFailureByRequest; } }

        public string OrderByRequestId { get; set; }
        public string UserName { get; set; }
        public string ArtNo { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#NUMBER#", OrderByRequestId);
            formatedStr = formatedStr.Replace("#USERNAME#", UserName);
            formatedStr = formatedStr.Replace("#ARTNO#", ArtNo);
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", ProductName);
            formatedStr = formatedStr.Replace("#QUANTITY#", Quantity);

            return formatedStr;
        }
    }

    public class ClsMailParamOnSendCertificate : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnSendGiftCertificate; } }

        public string CertificateCode { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Sum { get; set; }
        public string Message { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#CODE#", CertificateCode);
            formatedStr = formatedStr.Replace("#FROMNAME#", FromName);
            formatedStr = formatedStr.Replace("#TONAME#", ToName);
            formatedStr = formatedStr.Replace("#LINK#", SettingsMain.SiteUrl);
            formatedStr = formatedStr.Replace("#SUM#", Sum);
            formatedStr = formatedStr.Replace("#MESSAGE#", Message);

            return formatedStr;
        }
    }

    public class ClsMailParamOnBuyInOneClick : ClsMailParam
    {
        public override MailType Type { get { return MailType.OnBuyInOneClick; } }

        public string Name { get; set; }
        public string Adres { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public string OrderTable { get; set; }

        public override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#NAME#", Name);
            formatedStr = formatedStr.Replace("#COMMENTS#", Comment);
            formatedStr = formatedStr.Replace("#PHONE#", Phone);
            formatedStr = formatedStr.Replace("#ORDERTABLE#", OrderTable);
            formatedStr = formatedStr.Replace("#EMAIL#", Email);
            formatedStr = formatedStr.Replace("#ADRES#", Adres);

            return formatedStr;
        }
    }
}
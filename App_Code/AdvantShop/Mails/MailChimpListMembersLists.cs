//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Mails
{
    public class MailChimpListMembersList
    {
        //Member email address date timestamp timestamp of their associated status date (subscribed, unsubscribed, cleaned, or updated) in GMT
        public string Email { get; set; }

        //For unsubscribes only - the reason collected for the unsubscribe. If populated, one of 'NORMAL','NOSIGNUP','INAPPROPRIATE','SPAM','OTHER'
        public string Reason;

        //For unsubscribes only - if the reason is OTHER, the text entered.
        public string ReasonText;
    }
}
namespace AdvantShop.Mails
{
    public class MailChimpAccount
    {
        public string username;	//The Account username
        public string user_id;	//The Account user unique id (for building some links)
        public bool is_trial;	//Whether the Account is in Trial mode (can only send campaigns to less than 100 emails)
        public bool is_approved; 	//Whether the Account has been approved for purchases
        public bool has_activated;	//Whether the Account has been activated
        public string timezone;	//The timezone for the Account - default is "US/Eastern"
        public string plan_type;	//Plan Type - "monthly", "payasyougo", or "free"
        public int plan_low;//only for Monthly plans - the lower tier for list size
        public int plan_high;	//only for Monthly plans - the upper tier for list size
        public string plan_start_date;//only for Monthly plans - the start date for a monthly plan
        public int emails_left;//only for Free and Pay-as-you-go plans emails credits left for the account
        public bool pending_monthly;	//Whether the account is finishing Pay As You Go credits before switching to a Monthly plan
        public string first_payment;	//date of first payment
        public string last_payment;	//date of most recent payment
        public int times_logged_in;//total number of times the account has been logged into via the web
        public string last_login;//date/time of last login via the web
        public string affiliate_link;	//Monkey Rewards link for our Affiliate program
    }
}
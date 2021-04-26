//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Mails
{
    public class MailChimpList
    {
        public string Id { get; set; }
        public int WebId { get; set; }
        public string Name { get; set; }
        public string DateCreated { get; set; }
        public bool EmailTypeOption { get; set; }
        public bool UseAwesomebar { get; set; }
        public string DefaultFromName { get; set; }
        public string DefaultFromEmail { get; set; }
        public string DefaultSubject { get; set; }
        public string DefaultLanguage { get; set; }
        public int ListRating { get; set; }
        public string SubscribeUrlShort { get; set; }
        public string SubscribeUrlLong { get; set; }
        public string BeamerAddress { get; set; }
        //public array	stats	various stats and counts for the list - many of these are cached for at least 5 minutes
        //int	member_count	The number of active members in the given list.
        //int	unsubscribe_count	The number of members who have unsubscribed from the given list.
        //int	cleaned_count	The number of members cleaned from the given list.
        //int	member_count_since_send	The number of active members in the given list since the last campaign was sent
        //int	unsubscribe_count_since_send	The number of members who have unsubscribed from the given list since the last campaign was sent
        //int	cleaned_count_since_send	The number of members cleaned from the given list since the last campaign was sent
        //int	campaign_count	The number of campaigns in any status that use this list
        //int	grouping_count	The number of Interest Groupings for this list
        //int	group_count	The number of Interest Groups (regardless of grouping) for this list
        //int	merge_var_count	The number of merge vars for this list (not including the required EMAIL one)
        //int	avg_sub_rate	the average number of subscribe per month for the list (empty value if we haven't calculated this yet)
        //int	avg_unsub_rate	the average number of unsubscribe per month for the list (empty value if we haven't calculated this yet)
        //int	target_sub_rate	the target subscription rate for the list to keep it growing (empty value if we haven't calculated this yet)
        //int	open_rate	the average open rate per campaign for the list (empty value if we haven't calculated this yet)
        //int	click_rate	the average click rate per campaign for the list (empty value if we haven't calculated this yet)
        //array	modules	Any list specific modules installed for this list (example is SocialPro)
    }
}
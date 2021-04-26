//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lucene.Net.Analysis.Snowball
{
    public class RuStemming
    {    
        const string CVower = "аеиоуыэю€";
        const string CPerfectiveground = "((ив|ивши|ившись|ыв|ывши|ывшись)|((?<=[а€])(в|вши|вшись)))$";
        const string CReflexive = "(с[€ь])$";
        const string CAdjective = "(ее|ие|ые|ых|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|еых|ую|юю|а€|€€|ою|ею)$";
        const string CParticiple = "((ивш|ывш|ующ)|((?<=[а€])(ем|нн|вш|ющ|щ)))$";
        const string CVerb = "((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ены|ить|ыть|ишь|ую|ю)|((?<=[а€])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$";
        const string CNoun = "(ки|ок|а|ев|ов|ие|ье|е|и€ми|€ми|ами|еи|ии|и|ией|ей|ой|ий|й|и|и€м|€м|ием|ем|ам|ом|о|у|ах|и€х|€х|ы|ь|ию|ью|ю|и€|ь€|€)$";
        const string CRvre = "^(.*?[аеиоуыэю€])(.*)$";
        const string CDerivational = "[^аеиоуыэю€][аеиоуыэю€]+[^аеиоуыэю€]+[аеиоуыэю€].*(?<=о)сть?$";

        public RuStemming() { }

        bool RegexReplace(ref string Original, string regx, string value)
        {
            string original = Original;
            Regex reg = new Regex(regx);
            Original = reg.Replace(Original, value);
            return (Original != original);
        }

        Match RegexMatch(string original, string regx)
        {
            Regex reg = new Regex(regx);
            return reg.Match(original);
        }

        MatchCollection RegexMatches(string original, string regx)
        {
            Regex reg = new Regex(regx, RegexOptions.Multiline);
            return reg.Matches(original);
        }

        public string Parse(string query)
        {
            Regex reg = new Regex(@"[ ,\.\?!=\&\*\+]");
            string[] words = reg.Split(query);
            IList<string> swords = new List<string>();

            for (int i = 0; i < words.Length; i++)
                if (!string.IsNullOrEmpty(words[i].Trim()))
                    swords.Add(Stem(words[i].Trim()));
            string[] swordsArray = new string[swords.Count];
            for (int i = 0; i < swordsArray.Length; i++)
                swordsArray[i] = swords[i];
            string result = string.Join("%", swordsArray);

            return string.Format("%{0}%", result);
        }

        public string Stem(string Word)
        {
            string word = Word.ToLower().Trim().Replace("Є", "е");
            string value = string.Empty;
            do
            {
                MatchCollection matches = RegexMatches(word, CRvre);
                if (matches.Count < 1)
                {
                    Match matchEnglishOrDigits = RegexMatch(word, "[a-z0-9]");
                    if (matchEnglishOrDigits.Success)
                        value = word;

                    break;
                }
                    
                
                string rv = matches[0].Value;

                // шаг 1
                if (!RegexReplace(ref rv, CPerfectiveground, string.Empty))
                {
                    RegexReplace(ref rv, CReflexive, string.Empty);

                    if (RegexReplace(ref rv, CAdjective, string.Empty))
                        RegexReplace(ref rv, CParticiple, string.Empty);
                    else
                        if (!RegexReplace(ref rv, CVerb, string.Empty))
                            RegexReplace(ref rv, CNoun, string.Empty);                    
                }

                // шаг 2
                RegexReplace(ref rv, "и$", string.Empty);

                // шаг 3
                Match match = RegexMatch(rv, CDerivational);
                if (match.Success)
                    RegexReplace(ref rv, "ость?$", string.Empty);

                // шаг 4
                if (!RegexReplace(ref rv, "ь$", string.Empty))
                {
                    RegexReplace(ref rv, "ейше?", string.Empty);
                    RegexReplace(ref rv, "нн$", "н");
                }

                value = rv;      
                
            } while (false);

            return value;
        }

    }
}

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace Snowball.Net.Lucene.Net.Analysis.Snowball
{
    public class RuAnalyzer : Analyzer
    {
        public RuAnalyzer() { }
		
        public override TokenStream TokenStream(string fieldName, TextReader reader)
		{
			TokenStream result = new StandardTokenizer(reader);
			result = new StandardFilter(result);
			result = new LowerCaseFilter(result);
            result = new RuSnowballFilter(result);
			return result;
		}
    }
}

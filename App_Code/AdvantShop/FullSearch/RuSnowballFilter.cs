//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;

namespace Snowball.Net.Lucene.Net.Analysis.Snowball
{
    public class RuSnowballFilter : TokenFilter
    {
        private RuStemming stemmer = new RuStemming();

        public RuSnowballFilter(TokenStream token) : base(token) { }

        public override Token Next()
        {
            Token token = input.Next();
            if (token == null)
                return null;
            string result = null;
            try
            {
                result = stemmer.Stem(token.TermText());
            }
            catch (Exception e)
            {
                throw new System.SystemException(e.Message, e);
            }
            Token newToken = new Token(result, token.StartOffset(), token.EndOffset(), token.Type());
            newToken.SetPositionIncrement(token.GetPositionIncrement());
            return newToken;
        }
    }
}

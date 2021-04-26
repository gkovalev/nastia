//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop
{

    public class DevTool
    {

        public static Int32 GetRandomInt(Random rnd, Int32 intMin, Int32 intMax)
        {
            return rnd.Next(intMin, intMax);
        }

    }

}
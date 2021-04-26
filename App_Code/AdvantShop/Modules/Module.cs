//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;

namespace AdvantShop.Modules
{
    [Serializable]
    public class Module
    {
        public int Id { get; set; }
        public string StringId { get; set; }
        public string Version { get; set; }
        public bool Active { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }

        /// <summary>
        /// version in our system
        /// </summary>
        public string CurrentVersion { get; set; }

        public bool IsInstall { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsLocalVersion { get; set; }
    }
}
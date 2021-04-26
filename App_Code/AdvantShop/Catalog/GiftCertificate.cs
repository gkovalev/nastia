//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Catalog
{
    public enum CertificatePostType
    {
        Mail = 0,
        Email = 1
    }

    public class GiftCertificate
    {

        public int CertificateId { get; set; }
        public string CertificateCode { get; set; }
        public string OrderNumber { get; set; }

        public string FromName { get; set; }
        public string ToName { get; set; }

        public decimal Sum { get; set; }

        public CertificatePostType Type { get; set; }

        public string CertificateMessage { get; set; }

        public bool Used { get; set; }
        public bool Paid { get; set; }
        public bool Enable { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }
        public string Zone { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }

        public DateTime CreationDate { get; set; }

        public string CurrencyCode { get; set; }
        public decimal CurrencyValue { get; set; }

        public string FromEmail { get; set; }

        public override int GetHashCode()
        {
            return CertificateCode.GetHashCode() ^ Sum.GetHashCode() ^ Used.GetHashCode()*123 ^ Paid.GetHashCode()*321 ^ Enable.GetHashCode()*323;
        }
    }
}
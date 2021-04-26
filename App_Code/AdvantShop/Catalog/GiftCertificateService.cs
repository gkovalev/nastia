//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AdvantShop.Core;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Orders;

namespace AdvantShop.Catalog
{
    public class GiftCertificateService
    {

        #region add, get, update, delete
        private static GiftCertificate GetFromReader(SqlDataReader reader)
        {
            return new GiftCertificate
                       {
                           CertificateId = SQLDataHelper.GetInt(reader, "CertificateID"),
                           CertificateCode = SQLDataHelper.GetString(reader, "CertificateCode"),
                           FromName = SQLDataHelper.GetString(reader, "FromName"),
                           ToName = SQLDataHelper.GetString(reader, "ToName"),
                           OrderNumber = SQLDataHelper.GetString(reader, "OrderNumber"),
                           Sum = SQLDataHelper.GetDecimal(reader, "Sum"),
                           Paid = SQLDataHelper.GetBoolean(reader, "Paid"),
                           Used = SQLDataHelper.GetBoolean(reader, "Used"),
                           Enable = SQLDataHelper.GetBoolean(reader, "Enable"),
                           Type = (CertificatePostType)(SQLDataHelper.GetInt(reader, "Type")),
                           CertificateMessage = SQLDataHelper.GetString(reader, "Message"),
                           Email = SQLDataHelper.GetString(reader, "Email"),
                           Country = SQLDataHelper.GetString(reader, "Country"),
                           Zone = SQLDataHelper.GetString(reader, "Zone"),
                           City = SQLDataHelper.GetString(reader, "City"),
                           Zip = SQLDataHelper.GetString(reader, "Zip"),
                           Address = SQLDataHelper.GetString(reader, "Address"),
                           CreationDate = SQLDataHelper.GetDateTime(reader, "CreationDate"),
                           CurrencyCode = SQLDataHelper.GetString(reader, "CurrencyCode"),
                           CurrencyValue = SQLDataHelper.GetDecimal(reader, "CurrencyValue"),
                           FromEmail = SQLDataHelper.GetString(reader, "FromEmail")
                       };
        }

        public static GiftCertificate GetCertificateByID(int certificateId)
        {
            var certificate = SQLDataAccess.ExecuteReadOne<GiftCertificate>("[Catalog].[sp_GetCertificateById]",
                                                                                        CommandType.StoredProcedure, GetFromReader,
                                                                                        new SqlParameter { ParameterName = "@CertificateID", Value = certificateId });
            return certificate;
        }

        public static GiftCertificate GetCertificateByCode(string certificateCode)
        {
            var certificate = SQLDataAccess.ExecuteReadOne<GiftCertificate>("[Catalog].[sp_GetCertificateByCode]",
                                                                                        CommandType.StoredProcedure, GetFromReader,
                                                                                        new SqlParameter { ParameterName = "@CertificateCode", Value = certificateCode });
            return certificate;
        }

        public static int AddCertificate(GiftCertificate certificate)
        {
            var id = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddCertificate]", CommandType.StoredProcedure,
                                                      new SqlParameter("@CertificateCode", certificate.CertificateCode),
                                                      new SqlParameter("@OrderNumber", String.IsNullOrEmpty(certificate.OrderNumber) ? (object)DBNull.Value : certificate.OrderNumber),
                                                      new SqlParameter("@FromName", certificate.FromName),
                                                      new SqlParameter("@ToName", certificate.ToName),
                                                      new SqlParameter("@Used", certificate.Used),
                                                      new SqlParameter("@Paid", certificate.Paid),
                                                      new SqlParameter("@Enable", certificate.Enable),
                                                      new SqlParameter("@Type", certificate.Type),
                                                      new SqlParameter("@Sum", certificate.Sum),
                                                      new SqlParameter("@Message", certificate.CertificateMessage),
                                                      new SqlParameter("@Email", String.IsNullOrEmpty(certificate.Email) ? (object)DBNull.Value : certificate.Email),
                                                      new SqlParameter("@Country", String.IsNullOrEmpty(certificate.Country) ? (object)DBNull.Value : certificate.Country),
                                                      new SqlParameter("@Zone", String.IsNullOrEmpty(certificate.Zone) ? (object)DBNull.Value : certificate.Zone),
                                                      new SqlParameter("@City", String.IsNullOrEmpty(certificate.City) ? (object)DBNull.Value : certificate.City),
                                                      new SqlParameter("@Zip", String.IsNullOrEmpty(certificate.Zip) ? (object)DBNull.Value : certificate.Zip),
                                                      new SqlParameter("@Address", String.IsNullOrEmpty(certificate.Address) ? (object)DBNull.Value : certificate.Address),
                                                      new SqlParameter("@CurrencyCode", certificate.CurrencyCode),
                                                      new SqlParameter("@CurrencyValue", certificate.CurrencyValue),
                                                      new SqlParameter("@FromEmail", certificate.FromEmail)
                );
            return id;
        }

        public static bool UpdateCertificateById(GiftCertificate certificate)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateCertificateById]", CommandType.StoredProcedure,
                new SqlParameter("@CertificateId", certificate.CertificateId),
                new SqlParameter("@CertificateCode", certificate.CertificateCode),
                new SqlParameter("@OrderNumber", certificate.OrderNumber),
                new SqlParameter("@FromName", certificate.FromName),
                new SqlParameter("@ToName", certificate.ToName),
                new SqlParameter("@Used", certificate.Used),
                new SqlParameter("@Paid", certificate.Paid),
                new SqlParameter("@Enable", certificate.Enable),
                new SqlParameter("@Type", certificate.Type),
                new SqlParameter("@Sum", certificate.Sum),
                new SqlParameter("@Message", certificate.CertificateMessage),
                new SqlParameter("@Email", String.IsNullOrEmpty(certificate.Email) ? (object)DBNull.Value : certificate.Email),
                new SqlParameter("@Country", String.IsNullOrEmpty(certificate.Country) ? (object)DBNull.Value : certificate.Country),
                new SqlParameter("@Zone", String.IsNullOrEmpty(certificate.Zone) ? (object)DBNull.Value : certificate.Zone),
                new SqlParameter("@City", String.IsNullOrEmpty(certificate.City) ? (object)DBNull.Value : certificate.City),
                new SqlParameter("@Zip", String.IsNullOrEmpty(certificate.Zip) ? (object)DBNull.Value : certificate.Zip),
                new SqlParameter("@Address", String.IsNullOrEmpty(certificate.Address) ? (object)DBNull.Value : certificate.Address),
                new SqlParameter("@CurrencyCode", certificate.CurrencyCode),
                new SqlParameter("@CurrencyValue", certificate.CurrencyValue),
                new SqlParameter("@FromEmail", certificate.FromEmail)
                );
            return true;
        }

        public static bool DeleteCertificateById(int certificateId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteCertificateById]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@CertificateId", Value = certificateId });
            return true;
        }
        #endregion

        #region CustomerSertificate
        public static GiftCertificate GetCustomerCertificate()
        {
            return GetCustomerCertificate(CustomerSession.CustomerId);
        }

        public static GiftCertificate GetCustomerCertificate(Guid customerId)
        {
            var certificate = SQLDataAccess.ExecuteReadOne<GiftCertificate>
                ("Select * From Catalog.Certificate Where CertificateID = (Select CertificateID From Customers.CustomerCertificate Where CustomerID = @CustomerID)",
                    CommandType.Text, GetFromReader,
                    new SqlParameter { ParameterName = "@CustomerID", Value = customerId });

            if (certificate != null)
            {
                if (certificate.Paid && !certificate.Used)
                    return certificate;

                DeleteCustomerCertificate(certificate.CertificateId);
                return null;
            }
            return null;
        }

        public static void DeleteCustomerCertificate(int certificateId)
        {
            DeleteCustomerCertificate(certificateId, CustomerSession.CustomerId);
        }
        public static void DeleteCustomerCertificate(int certificateId, Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery
                ("Delete From Customers.CustomerCertificate Where CertificateID = @CertificateID and CustomerID = @CustomerID",
                    CommandType.Text,
                    new SqlParameter { ParameterName = "@CustomerID", Value = customerId },
                    new SqlParameter { ParameterName = "@CertificateID", Value = certificateId });
        }


        public static void AddCustomerCertificate(int certificateId)
        {
            AddCustomerCertificate(certificateId, CustomerSession.CustomerId);
        }

        public static void AddCustomerCertificate(int certificateId, Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery
                ("INSERT INTO Customers.CustomerCertificate (CustomerID, CertificateID) VALUES (@CustomerID, @CertificateID)",
                 CommandType.Text,
                 new SqlParameter { ParameterName = "@CustomerID", Value = customerId },
                 new SqlParameter { ParameterName = "@CertificateID", Value = certificateId }
                );
        }

        #endregion

        public static string GenerateCertificateCode()
        {
            var code = String.Empty;
            while (String.IsNullOrEmpty(code) || IsExistCertificateCode(code) || CouponService.IsExistCouponCode(code))
            {
                code = @"C-" + Strings.GetRandomString(8);
            }
            return code;
        }

        public static bool IsExistCertificateCode(string code)
        {
            return Convert.ToInt32(SQLDataAccess.ExecuteScalar
                    ("Select COUNT(CertificateID) From Catalog.Certificate Where CertificateCode = @CertificateCode",
                    CommandType.Text,
                    new SqlParameter { ParameterName = "@CertificateCode", Value = code })) > 0;
        }

        public static List<GiftCertificate> GetCertificates()
        {
            List<GiftCertificate> certificates = SQLDataAccess.ExecuteReadList<GiftCertificate>("[Catalog].[sp_GetCertificates]", CommandType.StoredProcedure, GetFromReader);
            return certificates;
        }

        public static decimal GetCertificatePriceById(int id)
        {
            return SQLDataHelper.GetDecimal(SQLDataAccess.ExecuteScalar("Select Sum From Catalog.Certificate Where CertificateId = @CertificateId",
                                                                        CommandType.Text,
                                                                        new SqlParameter { ParameterName = "@CertificateId", Value = id }));
        }

        public static void PayCertificate(int orderId)
        {
            SQLDataAccess.ExecuteScalar("Update Catalog.Certificate SET [Paid] = 1 Where CertificateID in (Select EntityID From [Order].[OrderedCart] Where ItemType = 1 and OrderID = @OrderId)",
                                                                       CommandType.Text,
                                                                       new SqlParameter { ParameterName = "@OrderId", Value = orderId });

            foreach (var orderItem in OrderService.GetOrderItems(orderId).Where(item => item.ItemType == EnumItemType.Certificate))
            {
                GiftCertificate certificate;

                if ((certificate = GetCertificateByID(orderItem.EntityId)) != null)
                {
                    SendCertificateMails(certificate);
                }
            }

        }
        public static void SendCertificateMails(GiftCertificate certificate)
        {


            string htmlMessage = SendMail.BuildMail(new ClsMailParamOnSendCertificate
            {
                CertificateCode = certificate.CertificateCode,
                FromName = certificate.FromName,
                ToName = certificate.ToName,
                Message = certificate.CertificateMessage,
                Sum = CatalogService.GetStringPrice(certificate.Sum)
            });

            SendMail.SendMailNow(certificate.Email, Resources.Resource.Admin_GiftCertificate_Certificate,
                                 htmlMessage, true);
            SendMail.SendMailNow(certificate.FromEmail,
                                Resources.Resource.Admin_GiftCertificate_CertificateSend + " " + certificate.Email, htmlMessage,
                                 true);
        }
    }
}
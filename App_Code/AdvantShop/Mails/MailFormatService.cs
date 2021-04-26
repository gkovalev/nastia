//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Mails
{
    /// <summary>
    /// Summary description for MailFormatService
    /// </summary>
    public class MailFormatService
    {
        public static List<MailFormat> GetMailFormatList()
        {
            List<MailFormat> mailFormats = SQLDataAccess.ExecuteReadList<MailFormat>("SELECT * FROM [Settings].[MailFormat]",
                                                                         CommandType.Text,
                                                                         reader => new MailFormat
                                                                                       {
                                                                                           MailFormatID = SQLDataHelper.GetInt(reader, "MailFormatID"),
                                                                                           FormatName = SQLDataHelper.GetString(reader, "FormatName"),
                                                                                           FormatText = SQLDataHelper.GetString(reader, "FormatText"),
                                                                                           FormatType = (MailType)SQLDataHelper.GetInt(reader, "FormatType"),
                                                                                           SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                                                                                           Enable = SQLDataHelper.GetBoolean(reader, "Enable"),
                                                                                           AddDate = SQLDataHelper.GetDateTime(reader, "AddDate"),
                                                                                           ModifyDate = SQLDataHelper.GetDateTime(reader, "ModifyDate")
                                                                                       });
            return mailFormats;
        }

        public static MailFormat GetMailFormat(int mailFormatId)
        {
            var mailFormat = SQLDataAccess.ExecuteReadOne<MailFormat>("SELECT * FROM [Settings].[MailFormat] WHERE MailFormatID = @MailFormatID",
                                                                             CommandType.Text,
                                                                             reader => new MailFormat
                                                                                           {
                                                                                               MailFormatID = SQLDataHelper.GetInt(reader, "MailFormatID"),
                                                                                               FormatName = SQLDataHelper.GetString(reader, "FormatName"),
                                                                                               FormatText = SQLDataHelper.GetString(reader, "FormatText"),
                                                                                               FormatType = (MailType)SQLDataHelper.GetInt(reader, "FormatType"),
                                                                                               SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                                                                                               Enable = SQLDataHelper.GetBoolean(reader, "Enable"),
                                                                                               AddDate = SQLDataHelper.GetDateTime(reader, "AddDate"),
                                                                                               ModifyDate = SQLDataHelper.GetDateTime(reader, "ModifyDate")
                                                                                           }, new SqlParameter("@MailFormatID", mailFormatId));
            return mailFormat;
        }

        public static void UpdateMailFormat(MailFormat mailFormat)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Settings].[MailFormat] SET FormatName = @FormatName, FormatText = @FormatText, FormatType = @FormatType, SortOrder = @SortOrder, Enable = @Enable, ModifyDate = GETDATE() WHERE (MailFormatID = @MailFormatID)",
                                            CommandType.Text,
                                            new SqlParameter("@MailFormatID", mailFormat.MailFormatID),
                                            new SqlParameter("@FormatName", mailFormat.FormatName),
                                            new SqlParameter("@FormatText", mailFormat.FormatText),
                                            new SqlParameter("@FormatType", (int)mailFormat.FormatType),
                                            new SqlParameter("@SortOrder", mailFormat.SortOrder),
                                            new SqlParameter("@Enable", mailFormat.Enable),
                                            new SqlParameter("@ModifyDate", DateTime.Now)
                                            );
        }

        public static void InsertMailFormat(MailFormat mailFormat)
        {
            mailFormat.MailFormatID = SQLDataAccess.ExecuteScalar<int>("INSERT INTO [Settings].[MailFormat] (FormatName, FormatText, FormatType, SortOrder, Enable, AddDate, ModifyDate ) VALUES (@FormatName, @FormatText, @FormatType, @SortOrder, @Enable, GETDATE(), GETDATE()); SELECT SCOPE_IDENTITY()",
                                                                        CommandType.Text,
                                                                        new SqlParameter("@FormatName", mailFormat.FormatName),
                                                                        new SqlParameter("@FormatText", mailFormat.FormatText),
                                                                        new SqlParameter("@FormatType", (int)mailFormat.FormatType),
                                                                        new SqlParameter("@SortOrder", mailFormat.SortOrder),
                                                                        new SqlParameter("@Enable", mailFormat.Enable));
        }

        public static void DeleteMailFormat(int mailFormatId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[MailFormat] WHERE MailFormatID = @MailFormatID",
                                            CommandType.Text,
                                            new SqlParameter("@MailFormatID", mailFormatId));
        }

        public static List<MailFormatType> GetMailFormatTypeList()
        {
            List<MailFormatType> mailFormatTypes = SQLDataAccess.ExecuteReadList<MailFormatType>("SELECT * FROM [Settings].[MailFormatType]",
                                                                 CommandType.Text,
                                                                 reader => new MailFormatType
                                                                               {
                                                                                   MailFormatTypeID = SQLDataHelper.GetInt(reader, "MailFormatTypeID"),
                                                                                   TypeName = SQLDataHelper.GetString(reader, "TypeName"),
                                                                                   SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                                                                                   Comment = SQLDataHelper.GetString(reader, "Comment"),
                                                                               });
            return mailFormatTypes;
        }

        public static MailFormatType GetMailFormatType(int mailFormatTypeId)
        {
            var mailFormatType = SQLDataAccess.ExecuteReadOne<MailFormatType>("SELECT * FROM [Settings].[MailFormatType] WHERE MailFormatTypeID = @MailFormatTypeID",
                                                        CommandType.Text,
                                                        reader => new MailFormatType
                                                        {
                                                            MailFormatTypeID = SQLDataHelper.GetInt(reader, "MailFormatTypeID"),
                                                            TypeName = SQLDataHelper.GetString(reader, "TypeName"),
                                                            SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                                                            Comment = SQLDataHelper.GetString(reader, "Comment"),
                                                        }, new SqlParameter("@MailFormatTypeID", mailFormatTypeId)
                                                        );
            return mailFormatType;
        }

        public static void UpdateMailFormatType(MailFormatType mailFormatType)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Settings].[MailFormatType] SET TypeName = @TypeName, SortOrder = @SortOrder, Comment = @Comment WHERE (MailFormatTypeID = @MailFormatTypeID)",
                                        CommandType.Text,
                                        new SqlParameter("@MailFormatTypeID", mailFormatType.MailFormatTypeID),
                                        new SqlParameter("@TypeName", mailFormatType.TypeName),
                                        new SqlParameter("@SortOrder", mailFormatType.SortOrder),
                                        new SqlParameter("@Comment", mailFormatType.Comment)
                                        );
        }

        public static void InsertMailFormatType(MailFormatType mailFormatType)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Settings].[MailFormatType] (TypeName, SortOrder, Comment) VALUES (@TypeName, @SortOrder, @Comment); SELECT SCOPE_IDENTITY()",
                                            CommandType.Text,
                                            new SqlParameter("@TypeName", mailFormatType.TypeName),
                                            new SqlParameter("@SortOrder", mailFormatType.SortOrder),
                                            new SqlParameter("@Comment", mailFormatType.Comment));
        }

        public static void DeleteMailFormatType(int mailFormatTypeId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[MailFormatType] WHERE MailFormatTypeID = @MailFormatTypeID",
                                            CommandType.Text, new SqlParameter("@MailFormatTypeID", mailFormatTypeId));
        }
    }
}
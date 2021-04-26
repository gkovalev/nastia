//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//todo: without AdvantShop 
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Modules
{
    public class CallbackRepository
    {
        private const string _moduleName = "Callback";

        public static bool InstallCallbackModule()
        {
            bool isInstall;
            using (var da = new SQLDataAccess())
            {
                if (ModulesRepository.IsExistsModuleTable(da, "Module", _moduleName))
                {
                    return true;
                }

                da.cmd.CommandText = "CREATE TABLE Module." + _moduleName +
                                            @"(
	                                        ID int NOT NULL IDENTITY (1, 1),
                                            Name nvarchar(100) NOT NULL,
	                                        Phone nvarchar(50) NOT NULL,
                                            DateAdded nvarchar(50) not null,
	                                        Comment nvarchar(MAX) NOT NULL,
	                                        AdminComment nvarchar(MAX) NOT NULL,
                                            Processed bit NOT NULL,
	                                        )  ON [PRIMARY]
	                                         TEXTIMAGE_ON [PRIMARY]                                        
                                           ALTER TABLE Module." + _moduleName + @" ADD CONSTRAINT
	                                        PK_Callback PRIMARY KEY CLUSTERED 
	                                        (
	                                        ID
	                                        ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]                                        
                                        ALTER TABLE Module." + _moduleName + @" SET (LOCK_ESCALATION = TABLE)
                                        SET IDENTITY_INSERT Module." + _moduleName + " ON";
                da.cmd.CommandType = CommandType.Text;

                da.cnOpen();
                da.cmd.ExecuteNonQuery();
                da.cnClose();

                ModuleSettingsProvider.SetSettingValue("email4notify", "", _moduleName);
                ModuleSettingsProvider.SetSettingValue("emailFormat", "<h4>Поступил новый заказ обратного звонка</h4><br/> Имя: #NAME# <br/> Телефон: #PHONE# <br/> Комментарий: #COMMENT#", _moduleName);
                ModuleSettingsProvider.SetSettingValue("emailSubject", "Заказ обратного звонка", _moduleName);
                ModuleSettingsProvider.SetSettingValue("windowTitle", "Обратный звонок", _moduleName);
                ModuleSettingsProvider.SetSettingValue("windowText", "Укажите свое имя и номер телефона, и мы Вам обязательно перезвоним.", _moduleName);

                isInstall = ModulesRepository.IsExistsModuleTable(da, "Module", _moduleName);
            }
            return isInstall;
        }

        public static bool UninstallCallbackModule()
        {
            bool isInstall;

            ModuleSettingsProvider.RemoveSqlSetting("email4notify", _moduleName);
            ModuleSettingsProvider.RemoveSqlSetting("emailFormat", _moduleName);
            ModuleSettingsProvider.RemoveSqlSetting("emailSubject", _moduleName);
            ModuleSettingsProvider.RemoveSqlSetting("windowTitle", _moduleName);
            ModuleSettingsProvider.RemoveSqlSetting("windowText", _moduleName);

            using (var da = new SQLDataAccess())
            {
                if (!ModulesRepository.IsExistsModuleTable(da, "Module", _moduleName))
                {
                    return true;
                }

                da.cmd.CommandText = "DROP TABLE Module." + _moduleName;
                da.cmd.CommandType = CommandType.Text;

                da.cnOpen();
                da.cmd.ExecuteNonQuery();
                da.cnClose();

                isInstall = ModulesRepository.IsExistsModuleTable(da, "Module", _moduleName);
            }
            return isInstall;
        }

        public static bool IsAliveCallbackModule()
        {
            using (var da = new SQLDataAccess())
            {
                return ModulesRepository.IsExistsModuleTable(da, "Module", _moduleName);
            }
        }

        private static CallbackCustomer GetCallbackCustomerFromReader(SqlDataReader reader)
        {
            return new CallbackCustomer
                {
                    ID = SQLDataHelper.GetInt(reader, "ID"),
                    Name = SQLDataHelper.GetString(reader, "Name"),
                    Phone = SQLDataHelper.GetString(reader, "Phone"),
                    DateAdded = SQLDataHelper.GetDateTime(reader, "DateAdded"),
                    Comment = SQLDataHelper.GetString(reader, "Comment"),
                    AdminComment = SQLDataHelper.GetString(reader, "AdminComment"),
                    Processed = SQLDataHelper.GetBoolean(reader, "Processed"),
                };
        }


        public static CallbackCustomer GetCallbackCustomer(int id)
        {
            return SQLDataAccess.ExecuteReadOne<CallbackCustomer>("SELECT * FROM [Module].[" + _moduleName + "] Where ID=@ID",
                CommandType.Text, GetCallbackCustomerFromReader, new SqlParameter("@ID", id));
        }

        public static List<CallbackCustomer> GetCallbackCustomers()
        {
            return SQLDataAccess.ExecuteReadList<CallbackCustomer>("SELECT * FROM [Module].[" + _moduleName + "] ORDER BY [DateAdded] DESC",
                CommandType.Text, GetCallbackCustomerFromReader);
        }

        public static void DeleteCallbackById(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Module].[" + _moduleName + "] WHERE [ID] = @ID",
                CommandType.Text,
                new SqlParameter("@ID", id));
        }

        public static void AddCallbackCustomer(CallbackCustomer callbackCustomer)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Module].[" + _moduleName + "] ([Name], [Phone], [DateAdded], [Comment], [AdminComment], [Processed]) VALUES (@Name, @Phone, GETDATE(), @Comment, @AdminComment, @Processed)",
                CommandType.Text,
                new SqlParameter("@Name", callbackCustomer.Name),
                new SqlParameter("@Phone", callbackCustomer.Phone),
                new SqlParameter("@Comment", callbackCustomer.Comment ?? ""),
                new SqlParameter("@AdminComment", callbackCustomer.AdminComment ?? ""),
                new SqlParameter("@Processed", callbackCustomer.Processed));
        }

        public static void UpdateCallbackCustomer(CallbackCustomer callbackCustomer)
        {
            SQLDataAccess.ExecuteNonQuery(
                "update [Module].[" + _moduleName + "] set [Name]=@Name, [Phone]=@Phone, [Comment]=@Comment, [AdminComment]=@AdminComment, [Processed]=@Processed where id=@id",
                CommandType.Text,
                new SqlParameter("@id", callbackCustomer.ID),
                new SqlParameter("@Name", callbackCustomer.Name),
                new SqlParameter("@Phone", callbackCustomer.Phone),
                new SqlParameter("@Comment", callbackCustomer.Comment ?? ""),
                new SqlParameter("@AdminComment", callbackCustomer.AdminComment ?? ""),
                new SqlParameter("@Processed", callbackCustomer.Processed));
        }
        

        public static void SetCallbackProcessed(int id, bool state)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Module].[" + _moduleName + "] SET [Processed]=@Processed WHERE [ID]=@ID",
                CommandType.Text,
                new SqlParameter("@ID", id),
                new SqlParameter("@Processed", state));
        }

        public static void SendEmail(CallbackCustomer callbackCustomer)
        {
            string email = ModuleSettingsProvider.GetSettingValue<string>("email4notify", _moduleName);
            string subject = ModuleSettingsProvider.GetSettingValue<string>("emailSubject", _moduleName);
            string format = ModuleSettingsProvider.GetSettingValue<string>("emailFormat", _moduleName);

            format =
                format.Replace("#NAME#", callbackCustomer.Name)
                      .Replace("#PHONE#", callbackCustomer.Phone)
                      .Replace("#COMMENT#", callbackCustomer.Comment);

            Mails.SendMail.SendMailNow(email, subject, format, true);

        }

    }
}

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

namespace AdvantShop.CMS
{
    /// <summary>
    /// Summary description for VoiceService
    /// </summary>
    public static class VoiceService
    {
        public static Answer GetAnswer(int answerId)
        {
            var answer = SQLDataAccess.ExecuteReadOne<Answer>("SELECT * FROM [Voice].[Answer] WHERE [AnswerID] = @AnswerID",
                                                         CommandType.Text, GetAnswerFromReader, new SqlParameter("@AnswerID", answerId));
            return answer;
        }

        private static Answer GetAnswerFromReader(SqlDataReader reader)
        {
            return new Answer
                    {
                        AnswerId = SQLDataHelper.GetInt(reader["AnswerID"]),
                        FkidTheme = SQLDataHelper.GetInt(reader["FKIDTheme"]),
                        Name = SQLDataHelper.GetString(reader["Name"]).Trim(),
                        CountVoice = SQLDataHelper.GetInt(reader["CountVoice"]),
                        Sort = SQLDataHelper.GetInt(reader["Sort"]),
                        IsVisible = SQLDataHelper.GetBoolean(reader["IsVisible"]),
                        DateAdded = SQLDataHelper.GetDateTime(reader["DateAdded"]),
                        DateModify = SQLDataHelper.GetDateTime(reader["DateModify"])
                    };
        }

        public static List<Answer> GetAllAnswers(int voiceThemeId)
        {
            List<Answer> answers = SQLDataAccess.ExecuteReadList<Answer>("SELECT * FROM [Voice].[Answer] WHERE [FKIDTheme] = @VoiceThemeID ORDER BY [Sort]",
                                                                 CommandType.Text, GetAnswerFromReader, new SqlParameter("@VoiceThemeID", voiceThemeId));
            return answers;
        }

        public static void InsertAnswer(Answer answer)
        {
            answer.AnswerId = SQLDataAccess.ExecuteScalar<int>(@"INSERT INTO [Voice].[Answer] ([FKIDTheme], [Name], [CountVoice], [Sort], [IsVisible], [DateAdded], [DateModify]) VALUES (@FKIDTheme,  @Name,  @CountVoice,  @Sort,  @IsVisible,  @DateAdded,  @DateModify); SELECT scope_identity();",
                                                 CommandType.Text,
                                                 new SqlParameter("@FKIDTheme", answer.FkidTheme),
                                                 new SqlParameter("@Name", answer.Name),
                                                 new SqlParameter("@CountVoice", answer.CountVoice),
                                                 new SqlParameter("@Sort", answer.Sort),
                                                 new SqlParameter("@IsVisible", answer.IsVisible),
                                                 new SqlParameter("@DateAdded", DateTime.Now),
                                                 new SqlParameter("@DateModify", DateTime.Now));
        }

        public static void AddVote(int answerId)
        {
            SQLDataAccess.ExecuteNonQuery(@"UPDATE [Voice].[Answer] SET [CountVoice] = [CountVoice] + 1 WHERE [AnswerID] = @AnswerID",
                                            CommandType.Text, new SqlParameter("@AnswerID", answerId));
        }

        public static void UpdateAnswer(Answer answer)
        {
            SQLDataAccess.ExecuteNonQuery(@"UPDATE [Voice].[Answer] SET [FKIDTheme] = @FKIDTheme,[Name] = @Name,[CountVoice] = @CountVoice, [Sort] = @Sort, [IsVisible] = @IsVisible, [DateModify] = @DateModify WHERE [AnswerID] = @AnswerID",
                                            CommandType.Text,
                                            new SqlParameter("@AnswerID", answer.AnswerId),
                                            new SqlParameter("@FKIDTheme", answer.FkidTheme),
                                            new SqlParameter("@Name", answer.Name),
                                            new SqlParameter("@CountVoice", answer.CountVoice),
                                            new SqlParameter("@Sort", answer.Sort),
                                            new SqlParameter("@IsVisible", answer.IsVisible),
                                            new SqlParameter("@DateModify", DateTime.Now));
        }

        public static void DeleteAnswer(int answerId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Voice].[Answer] WHERE [AnswerID] = @AnswerID", CommandType.Text, new SqlParameter("@AnswerID", answerId));
        }

        public static int GetVoiceThemesCount()
        {
            var count = SQLDataAccess.ExecuteScalar<int>("SELECT COUNT([VoiceThemeID]) FROM [Voice].[Answer]", CommandType.Text);
            return count;
        }

        public static VoiceTheme GetVoiceThemeById(int voiceThemeId)
        {
            VoiceTheme theme = SQLDataAccess.ExecuteReadOne("SELECT [VoiceThemeID], [PSYID], [Name], [IsHaveNullVoice], [IsDefault], [IsClose], [DateAdded], [DateModify], (SELECT SUM([CountVoice]) FROM [Voice].[Answer] WHERE (FKIDTheme = [Voice].[VoiceTheme].[VoiceThemeID]) AND (IsVisible = 1)) AS [CountVoice] FROM [Voice].[VoiceTheme] WHERE [VoiceThemeID] = @VoiceThemeID ORDER BY [IsDefault] DESC, [PSYID] ASC",
                                                            CommandType.Text, GetVoiceThemeFromReader, new SqlParameter("@VoiceThemeID", voiceThemeId));
            return theme;
        }

        private static VoiceTheme GetVoiceThemeFromReader(SqlDataReader reader)
        {
            return new VoiceTheme
                       {
                           VoiceThemeId = SQLDataHelper.GetInt(reader["VoiceThemeID"]),
                           PsyId = SQLDataHelper.GetInt(reader["PSYID"]),
                           Name = SQLDataHelper.GetString(reader["Name"]).Trim(),
                           IsHaveNullVoice = SQLDataHelper.GetBoolean(reader["IsHaveNullVoice"]),
                           IsDefault = SQLDataHelper.GetBoolean(reader["IsDefault"]),
                           IsClose = SQLDataHelper.GetBoolean(reader["IsClose"]),
                           DateAdded = SQLDataHelper.GetDateTime(reader["DateAdded"]),
                           DateModify = SQLDataHelper.GetDateTime(reader["DateModify"]),
                           CountVoice = SQLDataHelper.GetInt(reader["CountVoice"])
                       };
        }



        public static VoiceTheme GetTopTheme()
        {
            VoiceTheme theme = SQLDataAccess.ExecuteReadOne(
                "SELECT TOP (1) [VoiceThemeID], [PSYID], [Name], [IsHaveNullVoice], [IsDefault], [IsClose], [DateAdded], [DateModify], (SELECT SUM([CountVoice]) FROM [Voice].[Answer] WHERE (FKIDTheme = [Voice].[VoiceTheme].[VoiceThemeID]) AND (IsVisible = 1)) AS [CountVoice] FROM [Voice].[VoiceTheme] ORDER BY [IsDefault] DESC, [PSYID] ASC",
                CommandType.Text, GetVoiceThemeFromReader);
            return theme;
        }

        public static VoiceTheme GetVoiceThemeByPsyId(int psyId)
        {
            VoiceTheme theme = SQLDataAccess.ExecuteReadOne("SELECT [VoiceThemeID], [PSYID], [Name], [IsHaveNullVoice], [IsDefault], [IsClose], [DateAdded], [DateModify], (SELECT SUM([CountVoice]) FROM [Voice].[Answer] WHERE (FKIDTheme = [Voice].[VoiceTheme].[VoiceThemeID]) AND (IsVisible = 1)) AS [CountVoice] FROM [Voice].[VoiceTheme] WHERE [PSYID] = @PSYID ORDER BY [IsDefault] DESC, [PSYID] ASC",
                                                            CommandType.Text, GetVoiceThemeFromReader, new SqlParameter("@PSYID", psyId));
            return theme;
        }

        public static List<int> GetThemeIDs()
        {
            List<int> themeIds = SQLDataAccess.ExecuteReadList<int>("SELECT [VoiceThemeID] FROM [Voice].[VoiceTheme]",
                                                               CommandType.Text,
                                                               reader => SQLDataHelper.GetInt(reader, "VoiceThemeID"));
            return themeIds;
        }


        public static List<VoiceTheme> GetAllVoiceThemes()
        {
            List<VoiceTheme> theme = SQLDataAccess.ExecuteReadList<VoiceTheme>("SELECT [VoiceThemeID], [PSYID], [Name], [IsHaveNullVoice], [IsDefault], [IsClose], [DateAdded], [DateModify], (SELECT SUM([CountVoice]) FROM [Voice].[Answer] WHERE (FKIDTheme = [Voice].[VoiceTheme].[VoiceThemeID]) AND (IsVisible = 1)) AS [CountVoice] FROM [Voice].[VoiceTheme] ORDER BY [IsDefault] DESC, [PSYID] ASC",
                                                                   CommandType.Text, GetVoiceThemeFromReader);
            return theme;
        }

        public static void InsertVoiceTheme(VoiceTheme voiceTheme)
        {

            voiceTheme.VoiceThemeId = SQLDataAccess.ExecuteScalar<int>(@"INSERT INTO [Voice].[VoiceTheme] ([PSYID], [Name], [IsHaveNullVoice], [IsDefault], [IsClose], [DateAdded], [DateModify]) VALUES(@PSYID,  @Name,  @IsHaveNullVoice,  @IsDefault,  @IsClose,  @DateAdded,  @DateModify); SELECT scope_identity();",
                                                 CommandType.Text,
                                                 new SqlParameter("@PSYID", voiceTheme.PsyId),
                                                 new SqlParameter("@Name", voiceTheme.Name),
                                                 new SqlParameter("@IsHaveNullVoice", voiceTheme.IsHaveNullVoice),
                                                 new SqlParameter("@IsDefault", voiceTheme.IsDefault),
                                                 new SqlParameter("@IsClose", voiceTheme.IsClose),
                                                 new SqlParameter("@DateAdded", voiceTheme.DateAdded),
                                                 new SqlParameter("@DateModify", voiceTheme.DateModify)
                                                 );
        }

        public static void UpdateVoiceTheme(VoiceTheme voiceTheme)
        {
            SQLDataAccess.ExecuteNonQuery(@"UPDATE [Voice].[VoiceTheme] SET [PSYID] = @PSYID, [Name] = @Name, [IsHaveNullVoice] = @IsHaveNullVoice, [IsDefault] = @IsDefault, [IsClose] = @IsClose, [DateAdded] = @DateAdded, [DateModify] = @DateModify WHERE [VoiceThemeID] = @VoiceThemeID",
                                            CommandType.Text,
                                            new SqlParameter("@VoiceThemeID", voiceTheme.VoiceThemeId),
                                            new SqlParameter("@PSYID", voiceTheme.PsyId),
                                            new SqlParameter("@Name", voiceTheme.Name),
                                            new SqlParameter("@IsHaveNullVoice", voiceTheme.IsHaveNullVoice),
                                            new SqlParameter("@IsDefault", voiceTheme.IsDefault),
                                            new SqlParameter("@IsClose", voiceTheme.IsClose),
                                            new SqlParameter("@DateAdded", voiceTheme.DateAdded),
                                            new SqlParameter("@DateModify", voiceTheme.DateModify)
                                            );
        }

        public static void DeleteVoiceTheme(int voiceThemeId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Voice].[VoiceTheme] WHERE VoiceThemeID = @VoiceThemeID", CommandType.Text,
                                            new SqlParameter("@VoiceThemeID", voiceThemeId));
        }

        public static void AddTheme(VoiceTheme voiceTheme)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Voice].[VoiceTheme] ([PsyID], [Name], [IsDefault], [IsHaveNullVoice], [IsClose], [DateAdded], [DateModify]) VALUES ( @PsyID, @Name, @IsDefault, @IsHaveNullVoice, @IsClose, GETDATE(), GETDATE())",
                                            CommandType.Text,
                                            new SqlParameter("@Name", voiceTheme.Name),
                                            new SqlParameter("@PsyID", voiceTheme.PsyId),
                                            new SqlParameter("@IsDefault", voiceTheme.IsDefault),
                                            new SqlParameter("@IsHaveNullVoice", voiceTheme.IsHaveNullVoice),
                                            new SqlParameter("@IsClose", voiceTheme.IsClose)
                                            );
        }

        public static void UpdateTheme(VoiceTheme voiceTheme)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Voice].[VoiceTheme] set [PsyID]=@PsyID, [Name]=@Name, [IsDefault]=@IsDefault, [IsHaveNullVoice]=@IsHaveNullVoice, [IsClose]=@IsClose, [DateModify]=GetDate() where VoiceThemeId = @VoiceThemeId",
                                            CommandType.Text,
                                            new SqlParameter("@PsyID", voiceTheme.PsyId),
                                            new SqlParameter("@Name", voiceTheme.Name),
                                            new SqlParameter("@IsDefault", voiceTheme.IsDefault),
                                            new SqlParameter("@IsHaveNullVoice", voiceTheme.IsHaveNullVoice),
                                            new SqlParameter("@IsClose", voiceTheme.IsClose),
                                            new SqlParameter("@VoiceThemeId", voiceTheme.VoiceThemeId)
                                            );
        }

        public static void DeleteTheme(int id)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Voice].[VoiceTheme] WHERE [VoiceThemeID] = @VoiceThemeID", CommandType.Text, new SqlParameter("@VoiceThemeID", id));
        }

        public static string GetVotingName(int themeId)
        {
            return SQLDataAccess.ExecuteScalar<string>("SELECT [Name] FROM [Voice].[VoiceTheme] WHERE [VoiceThemeID] = @Theme", CommandType.Text, new SqlParameter("@Theme", themeId));
        }
    }
}
//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.SEO
{
    public enum MetaType
    {
        Default,
        Product,
        Category,
        News,
        StaticPage,
        Brand
    }

    public static class MetaInfoService
    {
        public static MetaInfo GetFormatedMetaInfo(MetaInfo meta, string name)
        {
            if (meta != null)
            {
                if (string.IsNullOrEmpty(meta.Title))
                {
                    meta.Title = SettingsSEO.GetDefaultTitle(meta.Type) ?? SettingsSEO.DefaultMetaTitle;
                }
                if (string.IsNullOrEmpty(meta.MetaKeywords))
                {
                    meta.MetaKeywords = SettingsSEO.GetDefaultMetaKeywords(meta.Type) ?? SettingsSEO.DefaultMetaKeywords;
                }
                if (string.IsNullOrEmpty(meta.MetaDescription))
                {
                    meta.MetaDescription = SettingsSEO.GetDefaultMetaDescription(meta.Type) ?? SettingsSEO.DefaultMetaKeywords;
                }

                meta.Title = GlobalStringVariableService.TranslateExpression(meta.Title, meta.Type, name);
                meta.MetaKeywords = GlobalStringVariableService.TranslateExpression(meta.MetaKeywords, meta.Type, name);
                meta.MetaDescription = GlobalStringVariableService.TranslateExpression(meta.MetaDescription, meta.Type, name);
            }
            return meta;
        }

        /// <summary>
        /// Get metainfo by metaid and type
        /// </summary>
        /// <param name="metaid"></param>
        /// <returns></returns>
        public static MetaInfo GetMetaInfo(int metaid)
        {

            return SQLDataAccess.ExecuteReadOne<MetaInfo>("Select * from SEO.MetaInfo where MetaID=@MetaID", CommandType.Text,
                                                            reader => new MetaInfo(SQLDataHelper.GetInt(reader, "MetaID"),
                                                                                    SQLDataHelper.GetInt(reader, "ObjId"),
                                                                                    (MetaType)Enum.Parse(typeof(MetaType), SQLDataHelper.GetString(reader, "Type"), true),
                                                                                    SQLDataHelper.GetString(reader, "Title"),
                                                                                    SQLDataHelper.GetString(reader, "MetaKeywords"),
                                                                                    SQLDataHelper.GetString(reader, "MetaDescription")),
                                                            new SqlParameter { ParameterName = "@MetaID", Value = metaid });
        }

        public static MetaInfo GetMetaInfo(int objId, MetaType type)
        {
            return SQLDataAccess.ExecuteReadOne<MetaInfo>("Select * from SEO.MetaInfo where ObjId=@objId and Type=@type", CommandType.Text,
                                                            reader => new MetaInfo(SQLDataHelper.GetInt(reader, "MetaID"),
                                                                                    SQLDataHelper.GetInt(reader, "ObjId"),
                                                                                    (MetaType)Enum.Parse(typeof(MetaType), SQLDataHelper.GetString(reader, "Type"), true),
                                                                                    SQLDataHelper.GetString(reader, "Title"),
                                                                                    SQLDataHelper.GetString(reader, "MetaKeywords"),
                                                                                    SQLDataHelper.GetString(reader, "MetaDescription")),
                                                            new SqlParameter { ParameterName = "@objId", Value = objId },
                                                            new SqlParameter { ParameterName = "@type", Value = type.ToString() }
                                                            );
        }

        /// <summary>
        /// Get default metainfo
        /// </summary>
        /// <returns></returns>
        public static MetaInfo GetDefaultMetaInfo()
        {
            return GetDefaultMetaInfo(MetaType.Default);
        }

        public static MetaInfo GetDefaultMetaInfo(MetaType metaType)
        {
            return new MetaInfo(0, 0, metaType, SettingsSEO.GetDefaultTitle(metaType), SettingsSEO.GetDefaultMetaKeywords(metaType), SettingsSEO.GetDefaultMetaDescription(metaType));
        }


        public static void SetMeta(MetaInfo meta)
        {
            if (IsMetaExist(meta.ObjId, meta.Type))
            {
                UpdateMetaInfo(meta);
            }
            else
            {
                meta.MetaId = InsertMetaInfo(meta);
            }
        }

        public static bool IsMetaExist(int objId, MetaType type)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(MetaID) from [SEO].[MetaInfo] where ObjId=@ObjId and Type=@Type", CommandType.Text,
                                                    new SqlParameter { ParameterName = "@ObjId", Value = objId },
                                                    new SqlParameter { ParameterName = "@Type", Value = type.ToString() }) > 0;
        }

        /// <summary>
        /// Insert meta info to database and return metaid of new metaid
        /// </summary>
        /// <param name="meta"></param>
        /// <returns></returns>
        private static int InsertMetaInfo(MetaInfo meta)
        {
            var id = SQLDataAccess.ExecuteScalar<int>("[SEO].[sp_AddMetaInfo]", CommandType.StoredProcedure,
                                                      new[]
                                                          {
                                                              new SqlParameter{ParameterName ="@Title",Value =meta.Title ?? SettingsSEO .DefaultMetaTitle },
                                                              new SqlParameter{ParameterName ="@MetaKeywords",Value =meta.MetaKeywords ?? SettingsSEO .DefaultMetaKeywords  },
                                                              new SqlParameter{ParameterName ="@MetaDescription",Value =meta.MetaDescription ?? SettingsSEO .DefaultMetaDescription  },
                                                              new SqlParameter{ParameterName ="@ObjId",Value =meta.ObjId },
                                                              new SqlParameter{ParameterName ="@Type",Value =meta.Type.ToString( ) }
                                                          }
                );
            return id;
        }

        private static void UpdateMetaInfo(MetaInfo meta)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [SEO].[MetaInfo] SET [Title] = @Title,[MetaKeywords] = @MetaKeywords,[MetaDescription] = @MetaDescription where ObjId=@ObjId and Type=@Type",
                                            CommandType.Text,
                                            new[]
                                                {
                                                    new SqlParameter{ParameterName ="@Title",Value =meta.Title ?? SettingsSEO .DefaultMetaTitle},
                                                    new SqlParameter{ParameterName ="@MetaKeywords",Value =meta.MetaKeywords ?? SettingsSEO .DefaultMetaKeywords},
                                                    new SqlParameter{ParameterName ="@MetaDescription",Value =meta.MetaDescription ?? SettingsSEO .DefaultMetaDescription },
                                                    new SqlParameter{ParameterName ="@ObjId",Value =meta.ObjId },
                                                    new SqlParameter{ParameterName ="@Type",Value =meta.Type.ToString( ) }
                                                }
                                           );
        }

        /// <summary>
        /// Delete metainfo from database by metaid
        /// </summary>
        /// <param name="metaId"></param>
        public static void DeleteMetaInfo(int metaId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [SEO].[MetaInfo] WHERE MetaID=@MetaID", CommandType.Text,
                                                new SqlParameter { ParameterName = "@MetaID", Value = metaId }
                                                );
        }

        /// <summary>
        /// Delete metainfo from database by metaid
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="type"></param>
        public static void DeleteMetaInfo(int objId, MetaType type)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [SEO].[MetaInfo] WHERE ObjId=@objId and Type=@type", CommandType.Text,
                                                new SqlParameter { ParameterName = "@objId", Value = objId },
                                                new SqlParameter { ParameterName = "@type", Value = type.ToString() }
                                                );
        }
    }
}

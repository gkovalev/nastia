//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.FullSearch;
using AdvantShop.SaasData;
using AdvantShop.SEO;
using Resources;

public partial class Admin_ImportElbuzCsv : Page
{

    #region ImportLogMessageType enum

    public enum ImportLogMessageType
    {
        ProductAdded,
        ProductUpdated,
        InvalidData,
        SuccessImport,
        ImportedWithErrors
    }

    #endregion

    private readonly string _filePath;
    private readonly string _fullPath;
    private bool useMultiThreadImport = false;

    public Admin_ImportElbuzCsv()
    {
        _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
        _fullPath = string.Format("{0}{1}", _filePath, "products.csv");
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lblError.Visible = false;
            lblError.Text = "";
        }
        else
        {
            lblError.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        lblError.Visible = true;
        lblError.Text = @"<br/>" + messageText;
    }

    private void UpdateInsertProduct(Product product)
    {
        if (useMultiThreadImport)
        {
            //Threading.ThreadPool.QueueUserWorkItem(AddressOf UpdateInsertProductWorker, product)
            var added = false;
            while (!added)
            {
                int workerThreads;
                int asyncIoThreads;
                ThreadPool.GetAvailableThreads(out workerThreads, out asyncIoThreads);
                if (workerThreads != 0)
                {
                    ThreadPool.QueueUserWorkItem(UpdateInsertProductWorker, product);
                    added = true;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
        else
        {
            UpdateInsertProductWorker(product);
        }
    }

    private static void UpdateInsertProductWorker(object o)
    {
        var product = (Product)o;
        bool addingNew;
        Product p = null;

        if (string.IsNullOrEmpty(product.ArtNo))
        {
            addingNew = true;
            // to do generate new ArtNO
            product.ArtNo = null;
        }
        else
        {
            p = ProductService.GetProduct(product.ArtNo);
            addingNew = p == null;
        }
        if (!addingNew)
        {
            product.ProductId = p.ProductId;
            //product.MetaId = p.MetaId;

            product.Recomended = p.Recomended;
            product.OnSale = p.OnSale;
            product.BestSeller = p.BestSeller;
            product.New = p.New;

            ProductService.UpdateProductByArtNo(product, false);
            Log(string.Format(Resource.Admin_Import1C_Updated, p.Name, p.ArtNo), ImportLogMessageType.ProductUpdated);
            ImportElbuzCsvStatistic.TotalUpdateRow++;
        }
        else
        {
            ProductService.AddProduct(product, false);
            Log(string.Format(Resource.Admin_Import1C_Added, product.Name, product.ArtNo),
                ImportLogMessageType.ProductAdded);
            ImportElbuzCsvStatistic.TotalAddRow++;
        }

        if (ImportElbuzCsvStatistic.TotalRowExcel == ImportElbuzCsvStatistic.RowPosition)
        {
            ImportElbuzCsvStatistic.IsRun = false;
            LuceneSearch.CreateAllIndexInBackground();
        }
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            if (!FileUpload1.HasFile)
            {
                MsgErr(Resource.Admin_ImportXLS_ChooseFile);
                return;
            }

            if (ImportElbuzCsvStatistic.IsRun == false)
            {
                ImportElbuzCsvStatistic.Init();
                ImportElbuzCsvStatistic.IsRun = true;
                linkCancel.Visible = true;
                MsgErr(true);

                lblRes.Text = "";

                var boolSuccess = true;

                try
                {
                    if (Directory.Exists(_filePath) == false)
                    {
                        Directory.CreateDirectory(_filePath);
                    }

                    if (File.Exists(_fullPath))
                    {
                        File.Delete(_fullPath);
                    }
                }
                catch (Exception ex)
                {
                    MsgErr(ex.Message + " at Files");
                    boolSuccess = false;
                }

                if (boolSuccess == false)
                {
                    return;
                }
                pUploadExcel.Visible = false;
                // Save file

                FileUpload1.SaveAs(_fullPath);
                var tr = new Thread(ProcessData);
                ImportElbuzCsvStatistic.ThreadImport = tr;
                tr.Start();

                pUploadExcel.Visible = false;

                OutDiv.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private static void Log(string message, ImportLogMessageType type)
    {
        SQLDataAccess.ExecuteNonQuery("INSERT INTO [Catalog].[ImportLog] (message, mtype) VALUES (@message, @mtype)",
                                            CommandType.Text, new SqlParameter("@mtype", Convert.ToInt32(type)), new SqlParameter("@message", message));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExcel)
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ImportXLS_Title);

        if (!IsPostBack)
        {
            OutDiv.Visible = ImportElbuzCsvStatistic.IsRun;
            linkCancel.Visible = ImportElbuzCsvStatistic.IsRun;
            SQLDataAccess.ExecuteNonQuery("DELETE from [Catalog].[ImportLog]", CommandType.Text);
        }
    }

    private void ProcessData()
    {
        using (var streamReaderCount = new StreamReader(_fullPath, Encoding.GetEncoding(1251)))
        {
            int count = 0;
            while (!streamReaderCount.EndOfStream)
            {
                var s = streamReaderCount.ReadLine();
                if (!string.IsNullOrEmpty(s) && (s.StartsWith("g\t") || s.StartsWith("p\t")))
                    count++;
            }
            streamReaderCount.Close();
            ImportElbuzCsvStatistic.TotalRowExcel = count;
        }

        using (var streamReader = new StreamReader(_fullPath, Encoding.GetEncoding(1251)))
        {
            var elbazcatToAdvcat = new Dictionary<int, int>();// first parameter = [Elbuz categoryId], second parameter = [AdvantShop categoryId] 
            while (!streamReader.EndOfStream)
            {
                if (!ImportElbuzCsvStatistic.IsRun)
                {
                    streamReader.Dispose();
                    return;
                }
                var temp = streamReader.ReadLine();

                if (!String.IsNullOrEmpty(temp))
                {
                    var tempValue = temp.Split(new[] { "\t" }, StringSplitOptions.None);

                    if (tempValue[0] == "g")
                    {
                        if (ProcessCategory(tempValue, elbazcatToAdvcat))
                        {
                            ImportElbuzCsvStatistic.TotalUpdateRow++;
                        }
                        else
                        {
                            Log("InvalidData", ImportLogMessageType.InvalidData);
                        }
                        ImportElbuzCsvStatistic.RowPosition++;
                    }

                    else if (tempValue[0] == "p" && tempValue.Length >= 22)
                    {
                        ProcessProduct(tempValue, elbazcatToAdvcat);
                        ImportElbuzCsvStatistic.RowPosition++;
                    }
                    else if ((tempValue[0] == "m") || (tempValue[0] == "pg"))
                    {
                        //ImportElbuzCsvStatistic.RowPosition++;
                    }
                    else
                    {
                        ImportElbuzCsvStatistic.TotalErrorRow++;
                        Log("InvalidData in " + tempValue[0] + " " + tempValue[1], ImportLogMessageType.InvalidData);
                        ImportElbuzCsvStatistic.RowPosition++;
                    }
                }
            }
            streamReader.Close();
            ImportElbuzCsvStatistic.IsRun = false;
        }
        CategoryService.RecalculateProductsCountManual();
    }

    private void ProcessProduct(IList<string> temp, IDictionary<int, int> elbazcatToAdvcat)
    {
        #region Product E-Trade PriceList Importer Description
        /*
            1. Если тип записи "p", тогда колонки означают следующее:
            2. Код товара (только числовое значение). *
            3. Код категории, в которой находится товар (только числовое значение). *
            4. Артикул товара.
            5. Наименование товара. *
            6. Цена товара.
            7. Код поставщика.
            8. Код производителя.
            9. Тег meta_title для товара.
            10. Тег meta_description для товара.
            11. Тег meta_keywords для товара.
            12. Признак одного из статусов товара (Спецена, Распродажа, Новинка) = 0 или 1.
            13. Описание товара (краткое).
            14. Количество.
            15. Код описания товара в базе данных СС.
            16. Наименование производителя товара.
            17. Модель товара.
            18. Описание товара №2 (полное).
            19. Имя файла картинки товара №1.
            20. Имя файла картинки товара №2.
            21. Порядок (сортировка).
            22. "Статус наличия", символьное отображение наличия товара из учётной системы.
            23. Дополнительная цена на товар из учётной системы. Если дополнительных цен больше одной, тогда цены перечислены через точку с запятой (;), согласно порядка установленого в справочнике дополнительных цен.
            24. Признак о том, что товар необходимо экспортировать в файл XML для рекламных площадок (яндекс маркет и т.д.). Возможные значения: 1 или 0.
            25. SEO ссылка на товар (ЧПУ).
            26. Активность товара. Возможные значения: 1 или 0.
            27. Код товара UUID (только символьные значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            * - обязательное заполнение значения для этого поля в файле CSV
         */
        #endregion

        var product = new Product
                          {

                              ArtNo = temp[1].Contains("*.*") ? "" : temp[1],
                              Name = temp[4],
                              Offers = new List<Offer>
                                           {
                                               new Offer
                                                   {
                                                       OfferListId = 6,
                                                       Price =
                                                           Convert.ToDecimal(temp[5].Replace(",", "."),
                                                                             CultureInfo.InvariantCulture),
                                                       Amount = Convert.ToInt32(temp[13])
                                                   }
                                           },
                              BriefDescription = temp[12],
                              Description = string.Empty,
                              Meta = new MetaInfo
                                         {
                                             Title = string.IsNullOrEmpty(temp[8]) ? SettingsSEO.ProductMetaTitle : temp[8],
                                             MetaDescription =
                                                 string.IsNullOrEmpty(temp[9])
                                                     ? SettingsSEO.ProductMetaDescription
                                                     : temp[9],
                                             MetaKeywords =
                                                 string.IsNullOrEmpty(temp[10])
                                                     ? SettingsSEO.ProductMetaKeywords
                                                     : temp[10]
                                         },
                              New = temp[11] != "0",
                              Enabled = true,
                              UrlPath = UrlService.GetEvalibleValidUrl(0, ParamType.Product, temp[1].Contains("*.*") ? "" : temp[1])
                          };

        UpdateInsertProduct(product);

        if (elbazcatToAdvcat.ContainsKey(Convert.ToInt32(temp[2])))
        {
            ProductService.AddProductLink(product.ProductId, elbazcatToAdvcat[Convert.ToInt32(temp[2])]);
        }
    }

    private bool ProcessCategory(IList<string> temp, Dictionary<int, int> elbazcatToAdvcat)
    {
        #region Category E-Trade PriceList Importer Description
        /*
            1. Если тип записи "g", тогда колонки означают следующее:
            2. Код категории (только числовое значение). *
            3. Код родительской категории (только числовое значение). *
            4. Наименование категории. *
            5. Описание категории.
            6. Порядок (сортировка).
            7. Тег meta_title для категории.
            8. Тег meta_description для категории.
            9. Тег meta_keywords для категории.
            10. Имя файла картинки для категории.
            11. Полный путь категории в числовом виде (например 1/2/3).
            12. Уровень категории, в пределах дерева.
            13. Левый ключ (для использования метода хранения деревьев типа "Вложенные множества" - Nested Sets).
            14. Правый ключ (для использования метода хранения деревьев типа "Вложенные множества" - Nested Sets).
            15. Полный путь категории в символьном виде (например Категория / Подкатегория 1 / Подкатегория 2).
            16. SEO ссылка на товар (ЧПУ).
            17. Активность категории. Возможные значения: 1 или 0.
            18. Код категории UUID (только символьное значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            19. Код родительской категории UUID (только символьное значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            * - обязательное заполнение этого поля в файле CSV.
         */
        #endregion

        if (string.IsNullOrEmpty(temp[14]))
            return false;

        var catId = CategoryService.SubParseAndCreateCategory("[" + temp[14].Replace("/", ">>") + "]");
        var category = CategoryService.GetCategory(catId);

        var sortOrder = -1;
        category.Name = temp[3];
        category.Description = temp[4];
        category.SortOrder = Int32.TryParse(temp[5], out sortOrder) ? sortOrder : 0;
        category.Meta = new MetaInfo
                            {
                                Title =
                                    !string.IsNullOrEmpty(temp[6]) ? temp[6] : SettingsSEO.CategoryMetaTitle,
                                MetaDescription =
                                    !string.IsNullOrEmpty(temp[7])
                                        ? temp[7]
                                        : SettingsSEO.CategoryMetaDescription,
                                MetaKeywords =
                                    !string.IsNullOrEmpty(temp[8])
                                        ? temp[8]
                                        : SettingsSEO.CategoryMetaKeywords
                            };
        //category.Picture = temp[9];
        category.Enabled = temp[16] == "1" ? true : false;

        CategoryService.UpdateCategory(category, false);

        elbazcatToAdvcat.Add(Convert.ToInt32(temp[1]), catId);

        return true;
    }

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (ImportElbuzCsvStatistic.ThreadImport.IsAlive)
        {
            ImportElbuzCsvStatistic.IsRun = false;
            ImportElbuzCsvStatistic.Init();
            hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
        }
    }
}
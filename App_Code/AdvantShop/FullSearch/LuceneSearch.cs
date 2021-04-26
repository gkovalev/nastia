//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Directory = System.IO.Directory;
using Field = Lucene.Net.Documents.Field;
using Version = Lucene.Net.Util.Version;
using Lucene.Net.Analysis.Snowball;

namespace AdvantShop.FullSearch
{
    public class SampleData
    {
        public int Id { get; set; }
        public string ArtNo { get; set; }
        public string Name { get; set; }

        public SampleData(int id, string artNo, string name)
        {
            Id = id;
            ArtNo = artNo;
            Name = name;
        }
    }

    public static class LuceneSearch
    {
        private struct IndexPaging
        {
            public int StartPos;
            public int EndPos;
        }

        // properties
        public static readonly string LuceneDir = SettingsGeneral.AbsolutePath + "/App_Data/Lucene";
        private static FSDirectory LuceneDirectory
        {
            get
            {
                if (!Directory.Exists(LuceneDir)) Directory.CreateDirectory(LuceneDir);
                FSDirectory directoryTemp = FSDirectory.Open(new DirectoryInfo(LuceneDir));
                if (IndexWriter.IsLocked(directoryTemp)) IndexWriter.Unlock(directoryTemp);
                var lockFilePath = Path.Combine(LuceneDir, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return directoryTemp;
            }
        }


        private static readonly Version Version = Version.LUCENE_29;
        private const int HitsLimit = 150;

        public static IEnumerable<int> Search(string input, string fieldName = "")
        {
            //Added By Evgeni to improve search
            RuStemming rus = new RuStemming();

            input = (rus.Stem(input)).Replace(".", "").Replace(",", "");
            ///
            return Search(input, 1, HitsLimit, fieldName);
        }

        public static IEnumerable<int> Search(string input, int pageNumber, int pageSize, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<int>();

            var terms =
                //Replaced By Evgeni to perform search like 20-230
                // input.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
                input.Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");

            input = string.Join(" ", terms);

            //Added by Evgeni to improve search capabilities
            input = input.Replace("*", "");// +"*";
            if (!input.Contains(' '))
            {
                input = input + "*";
            }

            return _search(input, pageNumber, pageSize, fieldName);
        }

        public static IEnumerable<int> SearchDefault(string input, string fieldName = "")
        {
            return SearchDefault(input, 1, HitsLimit, fieldName);
        }

        public static IEnumerable<int> SearchDefault(string input, int pageNumber, int pageSize, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<int>() : _search(input, pageNumber, pageSize, fieldName);
        }

        // main search method
        /// <summary>
        /// return IEnumerable of interesting id
        /// </summary>
        /// <param name="searchQuery">that to find</param>
        /// <param name="pageNumber">number of paging</param>
        /// <param name="pageSize">size return collection</param>
        /// <param name="searchField">if set than return result only by fiekd</param>
        /// <returns></returns>
        private static IEnumerable<int> _search(string searchQuery, int pageNumber, int pageSize, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<int>();

            try
            {
                // set up lucene searcher
                using (LuceneDirectory)
                {
                    //var analyzer = new RuAnalyzer();
                    var analyzer = new StandardAnalyzer(Version);
                    using (var searcher = new IndexSearcher(LuceneDirectory, true))
                    {
                        // search by single field
                        if (!string.IsNullOrEmpty(searchField))
                        {
                            var parser = new QueryParser(Version, searchField, analyzer);
                            var query = ParseQuery(searchQuery, parser);
                            var hits = searcher.Search(query, HitsLimit).ScoreDocs;
                            //calc 
                            var paging = CalculateArrayLocation(hits.Length, pageNumber, pageSize);

                            var results = MapLuceneToDataList(hits, searcher, paging);
                            analyzer.Close();
                            searcher.Close();
                            searcher.Dispose();
                            LuceneDirectory.Close();
                            return results;
                        }
                        // search by multiple fields (ordered by RELEVANCE)
                        else
                        {
                            var parser = new MultiFieldQueryParser(Version, new[] { "ArtNo", "Name", "NameWithWiteSpace" }, analyzer);
                            var query = ParseQuery(searchQuery, parser);
                            //searcher.SetDefaultFieldSortScoring(true, true);
                            var hits = searcher.Search(query, null, HitsLimit, Sort.RELEVANCE).ScoreDocs;
                            //var hits = searcher.Search(query, HitsLimit).ScoreDocs;
                            //calc 
                            var paging = CalculateArrayLocation(hits.Length, pageNumber, pageSize);

                            var results = MapLuceneToDataList(hits, searcher, paging);
                            analyzer.Close();
                            searcher.Close();
                            searcher.Dispose();
                            LuceneDirectory.Close();
                            return results;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return new List<int>();
            }
        }

        /// <summary>
        /// safely parsing, if ParseException parsing remove spesial symbols
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        //region map Lucene search index to data
        /// <summary>
        /// take all index withous paging
        /// </summary>
        /// <param name="hits"></param>
        /// <returns></returns>
        private static IEnumerable<int> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        /// <summary>
        /// take by paging data
        /// </summary>
        /// <param name="hits"></param>
        /// <param name="searcher"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        private static IEnumerable<int> MapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher,
                                                            IndexPaging paging)
        {
            return
                hits.Skip(paging.StartPos).Take(paging.EndPos - paging.StartPos).Select(
                    hit => MapLuceneDocumentToData(searcher.Doc(hit.doc))).ToList();
        }

        //if you want return SampleData in lucene, but all index  field must be with Field.Store.YES
        //private static SampleData MapLuceneDocumentToData(Document doc)
        //{
        //    return new SampleData
        //    {
        //        Id = Convert.ToInt32(doc.Get("Id")),
        //        Name = doc.Get("Name"),
        //    };
        //}

        /// <summary>
        /// mapping lucene data to result
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static int MapLuceneDocumentToData(Document doc)
        {
            return Convert.ToInt32(doc.Get("Id"));
        }

        //region add/update/clear search index data 

        public static void CreateAllIndexInBackground()
        {
            var worker = new Thread(CreateIndexFromDb) { IsBackground = true };
            worker.Start();
        }

        public static void CreateIndexFromDb()
        {
            using (LuceneDirectory)
            {
                //var analyzer = new RuAnalyzer();
                var analyzer = new StandardAnalyzer(Version);
                using (var writer = new IndexWriter(LuceneDirectory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.SetRAMBufferSizeMB(20);

                    // add data to lucene search index (replaces older entries if any)
                    using (var db = new SQLDataAccess())
                    {
                       
                        //Replace by Evgeni to increase serach abilities
                        // db.cmd.CommandText = "select productId,ArtNo, Name from Catalog.Product";
                        //db.cmd.CommandText = "select productId, (Name + ' ' + ArtNo + ' ' + Description) as Name from Catalog.Product where Enabled = 1";
                        db.cmd.CommandText = "select productId,ArtNo, (Name +  ' ' + ISNULL(ManufactureArtNo,'') + ' '  + ISNULL(EAN,'') + ' '+ Replace(ArtNo,'.','') +' '+ + Replace(ArtNo,'-','')  + ' ' + SUBSTRING ( artno ,1 , 1 )+'-' +SUBSTRING ( artno ,2 , 3 ) + '-' + SUBSTRING ( artno ,5 , 3 ) +'-' +SUBSTRING ( artno ,8 , 3 ) + ' ' ) as Name from Catalog.Product where Enabled = 1";
                
                        db.cmd.CommandType = CommandType.Text;
                        db.cnOpen();
                        using (var reader = db.cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Thread.Sleep(0);
                                AddToLuceneIndex(new SampleData(SQLDataHelper.GetInt(reader["productId"]), SQLDataHelper.GetString(reader, "ArtNo"), SQLDataHelper.GetString(reader["Name"])), writer);
                            }
                        }
                        db.cnClose();
                    }
                    // close handles
                    analyzer.Close();
                    writer.Optimize();
                    writer.Close();
                    writer.Dispose();
                }
                LuceneDirectory.Close();
            }
        }

        public static void AddUpdateLuceneIndex(SampleData sampleData)
        {
            AddUpdateLuceneIndex(new List<SampleData> { sampleData });
        }

        /// <summary>
        /// add index 
        /// </summary>
        /// <param name="sampleDatas"></param>
        public static void AddUpdateLuceneIndex(IEnumerable<SampleData> sampleDatas)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version);
            using (LuceneDirectory)
            {
                using (var writer = new IndexWriter(LuceneDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.SetRAMBufferSizeMB(10);

                    // add data to lucene search index (replaces older entries if any)
                    foreach (var sampleData in sampleDatas) AddToLuceneIndex(sampleData, writer);

                    writer.Commit();
                    // close handles
                    analyzer.Close();
                    writer.Close();
                    writer.Dispose();
                }
                LuceneDirectory.Close();
            }
        }

        /// <summary>
        /// clean record by it's id
        /// </summary>
        /// <param name="recordId"></param>
        public static void ClearLuceneIndexRecord(int recordId)
        {
            // init lucene
            //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
            using (LuceneDirectory)
            {
                try
                {
                    var analyzer = new StandardAnalyzer(Version);
                    using (var writer = new IndexWriter(LuceneDirectory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        // remove older index entry
                        var searchQuery = new TermQuery(new Term("Id", recordId.ToString(CultureInfo.InvariantCulture)));
                        writer.DeleteDocuments(searchQuery);

                        // close handles
                        analyzer.Close();
                        writer.Close();
                        writer.Dispose();
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Debug.LogError(ex);
                }
                LuceneDirectory.Close();
            }
        }

        /// <summary>
        /// Clear all index
        /// </summary>
        /// <returns></returns>
        public static bool ClearLuceneIndex()
        {
            try
            {
                using (LuceneDirectory)
                {
                    var analyzer = new StandardAnalyzer(Version);
                    using (var writer = new IndexWriter(LuceneDirectory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        // remove older index entries
                        writer.DeleteAll();
                        // close handles
                        analyzer.Close();
                        writer.Close();
                        writer.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Optimize index
        /// </summary>
        public static void Optimize()
        {
            //var analyzer = new StandardAnalyzer(Version.LUCENE_29);
            using (LuceneDirectory)
            {
                var analyzer = new StandardAnalyzer(Version);
                using (var writer = new IndexWriter(LuceneDirectory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    analyzer.Close();
                    writer.Optimize();
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        /// <summary>
        /// Add document into index, delete if there was same term
        /// </summary>
        /// <param name="sampleData"></param>
        /// <param name="writer"></param>
        private static void AddToLuceneIndex(SampleData sampleData, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("Id", sampleData.Id.ToString(CultureInfo.InvariantCulture)));
            writer.DeleteDocuments(searchQuery);
            // add new index entry
            var doc = CreateDocument(sampleData);
            // add entry to index
            writer.AddDocument(doc);
        }

        /// <summary>
        /// create document from SampleData
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static Document CreateDocument(SampleData obj)
        {
            // add lucene fields mapped to db fields
            var doc = new Document();

            Field f = new Field("ArtNo", obj.ArtNo, Field.Store.NO, Field.Index.ANALYZED);
            f.SetBoost(2F);
            doc.Add(f);


            f = new Field("Id", obj.Id.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED);
            f.SetBoost(1);
            doc.Add(f);

            f = new Field("Name", obj.Name, Field.Store.NO, Field.Index.ANALYZED);
            f.SetBoost(.1F);
            doc.Add(f);

            f = new Field("NameWithWiteSpace", obj.Name.RemoveSymbols(" "), Field.Store.NO, Field.Index.ANALYZED);
            f.SetBoost(.1F);
            doc.Add(f);

            return doc;

        }

        /// <summary>
        /// Calculate paging 
        /// </summary>
        /// <param name="totalHits"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private static IndexPaging CalculateArrayLocation(int totalHits, int pageNumber, int pageSize)
        {
            IndexPaging al;

            if (totalHits < 1 || pageNumber < 1 || pageSize < 1)
            {
                al.StartPos = 0;
                al.EndPos = 0;
                return al;
            }

            //long start = 1 + (pageNumber - 1) * pageSize;
            int start = (pageNumber - 1) * pageSize;
            int end = Math.Min(pageNumber * pageSize, totalHits);
            if (start > end)
            {
                start = Math.Max(1, end - pageSize);
            }

            al.StartPos = start;
            al.EndPos = end;
            return al;
        }
    }
}
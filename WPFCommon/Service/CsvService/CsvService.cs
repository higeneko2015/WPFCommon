using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace WPFCommon
{
    /// <summary>
    /// CSVファイル操作サービスクラス
    /// </summary>
    public class CsvService : ICsvService
    {
        /// <inheritdoc/>
        public List<T> Read<T>(string fullFileName)
        {
            using (var reader = new StreamReader(fullFileName))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture);

                using (var csv = new CsvReader(reader, config))
                {
                    // ヘッダーあり
                    //                    csv.Configuration.HasHeaderRecord = true;
                    // CSV読込
                    var items = csv.GetRecords<T>();
                    return items.ToList();
                }
            }
        }

        /// <inheritdoc/>
        public void Write<T1, T2>(string fullFileName, List<T1> records)
            where T2 : CsvWriteMapper<T2>
        {
            using (var writer = new StreamWriter(fullFileName))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture);

                using (var csv = new CsvWriter(writer, config))
                {
                    // ヘッダーあり
                    //csv.Configuration.HasHeaderRecord = true;

                    // マッパーを登録
                    csv.Context.RegisterClassMap<T2>();
                    //                    csv.Configuration.RegisterClassMap<T2>();

                    // CSV出力
                    csv.WriteRecords(records);
                }
            }
        }
    }
}
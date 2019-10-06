using ConvertCoordinateUnit.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConvertCoordinateUnit.Models
{
    /// <summary>
    /// 出力用クラス
    /// </summary>
    public class OutputContainer
    {
        // newでインスタンスを生成させないためにprivate宣言
        private OutputContainer()
        {
        }

        /// <summary>
        /// 座標単位変換の実行
        /// </summary>
        /// <param name="coordinateList">座標リスト</param>
        /// <returns>単位変換後の座標リスト</returns>
        public static List<CoordinateListFileData> ConvertCoordinateUnit(List<CoordinateListFileData> coordinateList)
        {
            var ret = new List<CoordinateListFileData>();

            // 座標リストの件数分繰り返し
            foreach (CoordinateListFileData data in coordinateList)
            {
                // 度分秒形式の文字列作成
                string latitude = CreateDegreeMinuteSecondFormat(decimal.Parse(data.Latitude));
                string longitude = CreateDegreeMinuteSecondFormat(decimal.Parse(data.Longitude));

                // 地名,緯度,経度を出力用リストに格納                
                var convertedCoordinateData = new CoordinateListFileData(data.Place, latitude, longitude);
                ret.Add(convertedCoordinateData);
            }

            return ret;
        }


        /// <summary>
        /// 度分秒形式のフォーマット作成
        /// </summary>
        /// <param name="val">度形式の座標</param>
        /// <returns>度分秒形式の座標</returns>
        private static string CreateDegreeMinuteSecondFormat(decimal val)
        {
            // 度を取り出す
            int degree = Convert.ToInt32(Math.Truncate(val));

            // 分を取り出す
            int minute = Convert.ToInt32(Math.Truncate((val % 1) * 60));

            // 秒を取り出す（小数点第三位で四捨五入）
            decimal second = Math.Round((((val % 1) * 60) % 1) * 60, 2, MidpointRounding.AwayFromZero);

            // 度分秒形式の文字列を作成            
            string ret = string.Format("{0}° {1}' {2}\"", degree, minute, second);

            return ret;
        }



        /// <summary>
        /// 単位変換後の座標リスト出力
        /// </summary>
        /// <param name="coordinatelist">単位変換後の座標リスト</param>
        /// <param name="exportFolderPath">単位変換後の座標リストファイルの出力先フォルダパス</param>        
        public static void ExportCoordinateList(List<CoordinateListFileData> coordinatelist, string exportFolderPath)
        {
            string filename = string.Format("ConvertedCoordinateList_{0}.tsv", Program.getDateTime().ToString("yyyyMMddHHmmss"));
            string filepath = Path.Combine(exportFolderPath, filename);
            Encoding enc = Encoding.GetEncoding(Constants.SJIS);

            using (FileStream fs = new FileStream(filepath, FileMode.CreateNew))
            {
                using (StreamWriter sw = new StreamWriter(fs, enc))
                {
                    foreach (CoordinateListFileData row in coordinatelist)
                    {
                        string line = string.Join("\t", new string[] { row.Place, row.Latitude, row.Longitude });
                        sw.WriteLine(line);
                    }
                }
            }
        }
    }
}

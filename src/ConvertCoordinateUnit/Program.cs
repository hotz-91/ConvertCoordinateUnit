namespace ConvertCoordinateUnit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using ConvertCoordinateUnit.Data;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "For Japanese support")]

    /// <summary>
    /// メイン処理のクラス
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ログ出力機能を提供
        /// </summary>
        private static Logger logger;

        /// <summary>
        /// メイン処理
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>0：正常終了, 1：異常終了</returns>
        private static int Main(string[] args)
        {
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            logger = new Logger(date);

            // 引数検査
            if (CheckArgs(args) == false)
            {
                return Constants.FAILED;
            }

            // 設定ファイル読み込み
            SettingFileData setting = LoadSettingFile(args[0]);
            if (setting == null)
            {
                return Constants.FAILED;
            }

            // 座標リスト読み込み
            List<CoordinateListFileData> coordinatelist = LoadCoordinateListFileData(setting.CoordinateListFilePath);
            if (coordinatelist == null)
            {
                return Constants.FAILED;
            }

            // 座標リストフォーマット検査
            if (CheckCoordinateList(coordinatelist, setting.CoordinateListFilePath) == false)
            {
                return Constants.FAILED;
            }

            // 座標単位変換詳細
            List<CoordinateListFileData> convertedCoordinatelist = ExecuteConvertCoordinateUnit(coordinatelist);

            // 変換後の座標リスト出力
            OutputConvertedCoordinateList(convertedCoordinatelist, setting.OutputFolderPath, date);

            logger.WriteLog("【正常終了】座標単位変換ツールは正常終了しました。");

            return 0;
        }

        /// <summary>
        /// 引数検査    
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>true：正常, false：異常</returns>
        private static bool CheckArgs(string[] args)
        {
            if (args.Length == 1)
            {
                return true;
            }

            logger.WriteLog("【異常終了】[引数検査]引数の数が不正です。");
            return false;
        }

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        /// <param name="csvfilepath">CSVファイルパス</param>
        /// <returns>設定内容</returns>
        private static SettingFileData LoadSettingFile(string csvfilepath)
        {
            if (File.Exists(csvfilepath) == false)
            {
                string msg = string.Format("【異常終了】[設定ファイル読み込み]設定ファイルが見つかりません。\t{0}", csvfilepath);
                logger.WriteLog(msg);
                return null;
            }

            SettingFileData ret;
            try
            {
                using (var sr = new StreamReader(csvfilepath, Encoding.GetEncoding(Constants.SJIS)))
                {
                    string line = sr.ReadLine();
                    string[] contents = line.Split(',');
                    ret = new SettingFileData(contents[0], contents[1]);
                }
            }
            catch
            {
                string msg = string.Format("【異常終了】[設定ファイル読み込み]設定ファイルから各項目を取得できませんでした。\t{0}", csvfilepath);
                logger.WriteLog(msg);
                return null;
            }

            if (Directory.Exists(ret.OutputFolderPath) == false)
            {
                string msg = string.Format("【異常終了】[設定ファイル読み込み] 単位変換後の座標リストファイルの出力先フォルダが見つかりません。\t{0}", ret.OutputFolderPath);
                logger.WriteLog(msg);
                return null;
            }

            return ret;
        }

        /// <summary>
        /// 座標リストファイル読み込み
        /// </summary>
        /// <param name="tsvfilepath">TSVファイルパス</param>
        /// <returns>座標リスト </returns>
        private static List<CoordinateListFileData> LoadCoordinateListFileData(string tsvfilepath)
        {
            if (File.Exists(tsvfilepath) == false)
            {
                string msg = string.Format("【異常終了】[座標リスト読み込み]座標リストが見つかりません。\t{0}", tsvfilepath);
                logger.WriteLog(msg);
                return null;
            }

            List<CoordinateListFileData> ret = new List<CoordinateListFileData>();

            try
            {
                using (var sr = new StreamReader(tsvfilepath, Encoding.GetEncoding(Constants.SJIS)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] values = line.Split('\t');
                        CoordinateListFileData data = new CoordinateListFileData(values[0], values[1], values[2]);
                        ret.Add(data);
                    }
                }
            }
            catch
            {
                string msg = string.Format("【異常終了】[座標リスト読み込み]座標リストから各項目を取得できません。\t{0}", tsvfilepath);
                logger.WriteLog(msg);
                return null;
            }

            return ret;
        }

        /// <summary>
        /// 座標リストフォーマットチェック
        /// </summary>
        /// <param name="coordinatelist">座標リスト</param>        
        /// <param name="tsvfilepath">TSVファイルパス</param>
        /// <returns>true：正常, false：異常</returns>
        private static bool CheckCoordinateList(List<CoordinateListFileData> coordinatelist, string tsvfilepath)
        {
            bool succeed = true;

            // 座標リストの件数分繰り返し
            foreach (CoordinateListFileData data in coordinatelist)
            {
                // 緯度のフォーマット検査
                if (CheckLatitude(data.Latitude) == false)
                {
                    string msg = string.Format("【異常あり】[座標リストフォーマット検査]緯度が0以上90以下の数値ではありません。\t{0}", data.Latitude);
                    logger.WriteLog(msg);
                    succeed = false;
                }

                // 経度のフォーマット検査
                if (CheckLongitude(data.Longitude) == false)
                {
                    string msg = string.Format("【異常あり】[座標リストフォーマット検査] 経度が0以上180以下の数値ではありません。\t{0}", data.Longitude);
                    logger.WriteLog(msg);
                    succeed = false;
                }
            }

            if (succeed == false)
            {
                string msg = string.Format("【異常終了】[座標リストフォーマット検査]座標リストのフォーマットが不正です。\t{0}", tsvfilepath);
                logger.WriteLog(msg);
            }

            return succeed;
        }

        /// <summary>
        /// 経度のフォーマット検査
        /// </summary>
        /// <param name="latitude">経度</param>
        /// <returns>true：正常, false：異常</returns>
        private static bool CheckLatitude(string latitude)
        {
            decimal d = 0;
            if (decimal.TryParse(latitude, out d) == false)
            {
                return false;
            }

            if (d >= 0 && d <= 90)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 緯度のフォーマット検査
        /// </summary>
        /// <param name="longitude">緯度</param>
        /// <returns>true：正常, false：異常</returns>
        private static bool CheckLongitude(string longitude)
        {
            decimal d = 0;
            if (decimal.TryParse(longitude, out d) == false)
            {
                return false;
            }

            if (d >= 0 && d <= 180)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 座標単位変換の実行
        /// </summary>
        /// <param name="coordinateList">座標リスト</param>
        /// <returns>単位変換後の座標リスト</returns>
        private static List<CoordinateListFileData> ExecuteConvertCoordinateUnit(List<CoordinateListFileData> coordinateList)
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
        /// <param name="convertedCoordinatelist">単位変換後の座標リスト</param>
        /// <param name="outputFolderPath">単位変換後の座標リストファイルの出力先フォルダパス</param>
        /// <param name="date">日付</param>
        private static void OutputConvertedCoordinateList(List<CoordinateListFileData> convertedCoordinatelist, string outputFolderPath, string date)
        {
            string filename = string.Format("ConvertedCoordinateList_{0}.tsv", date);
            string filepath = Path.Combine(outputFolderPath, filename);
            Encoding enc = Encoding.GetEncoding(Constants.SJIS);

            using (FileStream fs = new FileStream(filepath, FileMode.CreateNew))
            {
                using (StreamWriter sw = new StreamWriter(fs, enc))
                {
                    foreach (CoordinateListFileData row in convertedCoordinatelist)
                    {
                        string line = string.Join("\t", new string[] { row.Place, row.Latitude, row.Longitude });
                        sw.WriteLine(line);
                    }
                }
            }
        }
    }
}

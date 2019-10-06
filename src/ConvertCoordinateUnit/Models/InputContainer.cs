using ConvertCoordinateUnit.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConvertCoordinateUnit.Models
{
    /// <summary>
    /// 入力用クラス
    /// </summary>
    public class InputContainer
    {
        // newでインスタンスを生成させないためにprivate宣言
        private InputContainer()
        {
        }

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        /// <param name="settingFilePath">設定ファイルパス</param>
        /// <returns>設定内容</returns>
        public static SettingFileData LoadSettingFile(string settingFilePath)
        {
            if (File.Exists(settingFilePath) == false)
            {
                Logger.WriteLog(LogSeverity.Failed, Constants.Message.NOT_FOUND_SETTING_FILE, settingFilePath);
                Environment.Exit(Constants.FAILED);
            }
            
            string[] contents = null;
            try
            {
                using (var sr = new StreamReader(settingFilePath, Encoding.GetEncoding(Constants.SJIS)))
                {
                    string line = sr.ReadLine();
                    contents = line.Split(',');                    
                }
            }
            catch
            {
                Logger.WriteLog(LogSeverity.Failed, Constants.Message.NOT_OBTAINED_SETTING_FILE_ITEM, settingFilePath);
                Environment.Exit(Constants.FAILED);
            }

            var settingFileData = new SettingFileData(contents[0], contents[1]);

            if (Directory.Exists(settingFileData.ExportFolderPath) == false)
            {
                Logger.WriteLog(LogSeverity.Failed , Constants.Message.NOT_FOUND_EXPORT_DIR, settingFileData.ExportFolderPath);
                Environment.Exit(Constants.FAILED);
            }

            return settingFileData;
        }
        
        /// <summary>
        /// 座標リストファイル読み込み
        /// </summary>
        /// <param name="coodinateListFilePath">TSVファイルパス</param>
        /// <returns>座標リスト </returns>
        public static List<CoordinateListFileData> LoadCoordinateListFileData(string coodinateListFilePath)
        {
            if (File.Exists(coodinateListFilePath) == false)
            {                
                Logger.WriteLog(LogSeverity.Failed, Constants.Message.NOT_FOUND_COORDINATE_LIST, coodinateListFilePath);
                Environment.Exit(Constants.FAILED);
            }
            
            var coordinateList = new List<CoordinateListFileData>();

            try
            {
                using (var sr = new StreamReader(coodinateListFilePath, Encoding.GetEncoding(Constants.SJIS)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] values = line.Split('\t');
                        var row = new CoordinateListFileData(values[0], values[1], values[2]);
                        coordinateList.Add(row);
                    }
                }
            }
            catch
            {
                Logger.WriteLog(LogSeverity.Failed, Constants.Message.NOT_OBTAINDED_COORDINATE_LIST_ITEM, coodinateListFilePath);
                Environment.Exit(Constants.FAILED);
            }

            return coordinateList;
        }

        /// <summary>
        /// 座標リストフォーマットチェック
        /// </summary>
        /// <param name="coordinateList">座標リスト</param>        
        /// <param name="coordinateListFilePath">座標リストファイルパス</param>        
        public static void ValidateCoordinateList(List<CoordinateListFileData> coordinateList, string coordinateListFilePath)
        {
            bool result = true;

            // 座標リストの件数分繰り返し
            foreach (CoordinateListFileData data in coordinateList)
            {
                // 緯度のフォーマット検査
                if (CheckLatitude(data.Latitude) == false)
                {
                    Logger.WriteLog(LogSeverity.ExistsInvalid, Constants.Message.INVALID_FORMAT_LATITUDE, data.Latitude);
                    result = false;
                }

                // 経度のフォーマット検査
                if (CheckLongitude(data.Longitude) == false)
                {
                    Logger.WriteLog(LogSeverity.ExistsInvalid, Constants.Message.INVALID_FORMAT_LONGITUDE, data.Longitude);
                    result = false;
                }
            }

            if (result == false)
            {
                Logger.WriteLog(LogSeverity.Failed, Constants.Message.INVALID_FORMAT_COORDINATE_LIST, coordinateListFilePath);
                Environment.Exit(Constants.FAILED);
            }            
        }

        /// <summary>
        /// 経度のフォーマット検査
        /// </summary>
        /// <param name="latitude">経度</param>
        /// <returns>true：正常, false：異常</returns>
        private static bool CheckLatitude(string latitude)
        {            
            if (decimal.TryParse(latitude, out decimal d) == false)
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
            if (decimal.TryParse(longitude, out decimal d) == false)
            {
                return false;
            }

            if (d >= 0 && d <= 180)
            {
                return true;
            }

            return false;
        }

    }
}

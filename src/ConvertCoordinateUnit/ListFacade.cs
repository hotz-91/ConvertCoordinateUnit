using ConvertCoordinateUnit.Data;
using ConvertCoordinateUnit.Models;
using System.Collections.Generic;

namespace ConvertCoordinateUnit
{
    public   class ListFacade
    {
        private ListFacade()
        {
        }

        public static void ConvertCoordinateUnitList(string settingFilePath)
        {
            // 設定ファイル読み込み
            SettingFileData settingFileData = InputContainer.LoadSettingFile(settingFilePath);

            // 座標リスト読み込み
            List<CoordinateListFileData> coordinateList = InputContainer.LoadCoordinateListFileData(settingFileData.CoordinateListFilePath);

            // 座標リストフォーマット検査
            InputContainer.ValidateCoordinateList(coordinateList, settingFileData.CoordinateListFilePath);

            // 座標単位変換
            List<CoordinateListFileData> result = OutputContainer.ConvertCoordinateUnit(coordinateList);

            // 変換後の座標リスト出力
            OutputContainer.ExportCoordinateList(result, settingFileData.ExportFolderPath);
        }
    }
}

namespace ConvertCoordinateUnit.Data
{
    public  class SettingFileData
    {
        /// <summary>
        /// 座標リストファイルパス
        /// </summary>
        public string CoordinateListFilePath { get; private set; }

        /// <summary>
        /// 変換後座標リストファイルの出力先フォルダパス
        /// </summary>
        public string ExportFolderPath { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="coordinateListFilePath">座標リストファイルパス</param>
        /// <param name="exportFolderPath">変換後座標リストファイルの出力先フォルダパス</param>
        public SettingFileData(string coordinateListFilePath, string exportFolderPath)
        {
            CoordinateListFilePath = coordinateListFilePath;
            ExportFolderPath = exportFolderPath;
        }
    }
}

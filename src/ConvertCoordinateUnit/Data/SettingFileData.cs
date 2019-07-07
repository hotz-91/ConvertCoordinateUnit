namespace ConvertCoordinateUnit.Data
{
    public  class SettingFileData
    {
        /// <summary>
        /// 座標リストファイルパス
        /// </summary>
        public readonly string CoordinateListFilePath;

        /// <summary>
        /// 変換後座標リストファイルの出力先フォルダパス
        /// </summary>
        public readonly string OutputFolderPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="coordinateListFilePath">座標リストファイルパス</param>
        /// <param name="outputFolderPath">変換後座標リストファイルの出力先フォルダパス</param>
        public SettingFileData(string coordinateListFilePath, string outputFolderPath)
        {
            CoordinateListFilePath = coordinateListFilePath;
            OutputFolderPath = outputFolderPath;
        }
    }
}

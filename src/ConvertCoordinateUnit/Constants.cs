namespace ConvertCoordinateUnit
{
    /// <summary>
    /// 定数
    /// </summary>
    public static class Constants
    {
        public const int SUCCEED = 0;

        public const int FAILED = 1;

        public const string SJIS = "shift_jis";

        public static class Message
        {
            public const string INVALID_ARGS = "[引数検査]引数の数が不正です。";

            public const string NOT_FOUND_SETTING_FILE = "[設定ファイル読み込み]設定ファイルが見つかりません。";

            public const string NOT_OBTAINED_SETTING_FILE_ITEM = "[設定ファイル読み込み]設定ファイルから各項目を取得できませんでした。";

            public const string NOT_FOUND_EXPORT_DIR = "[設定ファイル読み込み] 単位変換後の座標リストファイルの出力先フォルダが見つかりません。";

            public const string NOT_FOUND_COORDINATE_LIST = "[座標リスト読み込み]座標リストが見つかりません。";

            public const string NOT_OBTAINDED_COORDINATE_LIST_ITEM = "[座標リスト読み込み]座標リストから各項目を取得できません。";

            public const string INVALID_FORMAT_LATITUDE = "[座標リストフォーマット検査]緯度が0以上90以下の数値ではありません。";

            public const string INVALID_FORMAT_LONGITUDE = "[座標リストフォーマット検査] 経度が0以上180以下の数値ではありません。";

            public const string INVALID_FORMAT_COORDINATE_LIST = "[座標リストフォーマット検査]座標リストのフォーマットが不正です。";

            public const string COMPLETED = "座標単位変換ツールは正常終了しました。";

            
        }
    }
}

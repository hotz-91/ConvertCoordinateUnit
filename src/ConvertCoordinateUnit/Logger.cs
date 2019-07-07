namespace ConvertCoordinateUnit
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "For Japanese support")]

    /// <summary>
    /// ログ出力機能を提供するクラス
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// ログファイルパス
        /// </summary>
        private readonly string logFilePath = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="date">日付</param>
        public Logger(string date)
        {
            this.logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("result_{0}.tsv", date));
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="msg">出力内容</param>
        public void WriteLog(string msg)
        {
            using (FileStream fs = new FileStream(this.logFilePath, FileMode.Append))
            {
                Encoding enc = Encoding.GetEncoding(Constants.SJIS);
                using (StreamWriter sw = new StreamWriter(fs, enc))
                {
                    sw.WriteLine(msg);
                }
            }
        }
    }
}

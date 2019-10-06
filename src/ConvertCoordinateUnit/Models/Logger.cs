using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ConvertCoordinateUnit.Utilities;

namespace ConvertCoordinateUnit.Models
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "For Japanese support")]

    /// <summary>
    /// The severity of a log item
    /// </summary>
    public enum LogSeverity
    {
        [StringValue("【正常終了】")]
        Succeed = 0,

        [StringValue("【異常終了】")]
        Failed = 1,

        [StringValue("【異常あり】")]
        ExistsInvalid = 2,
    }

    /// <summary>
    /// ログ出力機能を提供するクラス  
    /// </summary>
    public  class Logger
    {                
        private Logger()
        {
        }

        private static string _logFilePath = string.Empty;

        public static void Init(DateTime dateTime)
        {
            _logFilePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                string.Format("result_{0}.tsv", dateTime.ToString("yyyyMMddHHmmss")));
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="msg">出力内容</param>
        public  static void WriteLog(LogSeverity severity, string msg, params object[] args)
        {
            using (FileStream fs = new FileStream(_logFilePath, FileMode.Append))
            {
                Encoding enc = Encoding.GetEncoding(Constants.SJIS);
                using (StreamWriter sw = new StreamWriter(fs, enc))
                {
                    var sb = new StringBuilder();
                    sb.Append(severity.GetStringValue());
                    sb.Append(msg);
                    if(args != null && args.Length > 0)
                    {
                        sb.Append("\t");
                        foreach(string param in args)
                        {
                            sb.Append(param);
                            if(args.Last().ToString() != param)
                            {
                                sb.Append(",");
                            }
                        }
                    }

                    sw.WriteLine(sb.ToString());
                }
            }
        }

        public static void WriteLog(Exception ex)
        {
            using (FileStream fs = new FileStream(_logFilePath, FileMode.Append))
            {
                Encoding enc = Encoding.GetEncoding(Constants.SJIS);
                using (StreamWriter sw = new StreamWriter(fs, enc))
                {
                    sw.WriteLine(ex);
                }
            }
        }
    }
}

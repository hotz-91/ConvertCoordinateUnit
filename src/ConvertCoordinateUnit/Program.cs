namespace ConvertCoordinateUnit
{
    using ConvertCoordinateUnit.Models;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "For Japanese support")]

    /// <summary>
    /// メイン処理のクラス
    /// </summary>
    public class Program
    {
        private static readonly DateTime _datetime = DateTime.Now;

        /// <summary>
        /// メイン処理
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>0：正常終了, 1：異常終了</returns>
        private static int Main(string[] args)
        {
            Logger.Init(_datetime);

            if(args.Length != 1)
            {
                // 引数が不正
                Logger.WriteLog(LogSeverity.Failed, Constants.Message.INVALID_ARGS);

                return Constants.FAILED;
            }

            // 座標単位変換処理
            try
            {
                ListFacade.ConvertCoordinateUnitList(args[0]);
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex);
            }

            Logger.WriteLog(LogSeverity.Succeed, Constants.Message.COMPLETED);

            return Constants.SUCCEED;
            
        }

        public static DateTime getDateTime()
        {
            return _datetime;
        }





    }
}

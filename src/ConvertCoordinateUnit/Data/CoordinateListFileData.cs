namespace ConvertCoordinateUnit.Data
{
    public class CoordinateListFileData
    {
        /// <summary>
        /// 地名
        /// </summary>
        public string Place { get; private set; }

        /// <summary>
        /// 緯度
        /// </summary>
        public string Latitude { get; private set; }

        /// <summary>
        /// 経度
        /// </summary>
        public string Longitude { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="place">地名</param>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">経度</param>
        public CoordinateListFileData(string place, string latitude, string longitude)
        {
            Place = place;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}

namespace ConvertCoordinateUnit.Data
{
    public class CoordinateListFileData
    {
        /// <summary>
        /// 地名
        /// </summary>
        public readonly string Place;

        /// <summary>
        /// 緯度
        /// </summary>
        public readonly string Latitude;

        /// <summary>
        /// 経度
        /// </summary>
        public readonly string Longitude;

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

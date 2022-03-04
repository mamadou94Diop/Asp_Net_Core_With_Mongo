using DriveMeShop.CustomAnnotations;
using Newtonsoft.Json;

namespace DriveMeShop.Model
{
    public class CarLastRevisionYearModel
    {
        /// <summary>
        ///  The year when the car receives its last technical revision
        /// </summary>
        /// <example>
        ///   2021
        /// </example>
        [YearStartingBy(StartingYear = 1950)]
        [JsonProperty("lastRevisionYear")]
        public int? LastRevisionYear { get; set; }
    }
}

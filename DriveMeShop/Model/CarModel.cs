using System.ComponentModel.DataAnnotations;
using DriveMeShop.CustomAnnotations;
using Newtonsoft.Json;

namespace DriveMeShop.Model
{
    public class CarModel
    {
        /// <summary>
        ///  Identifier of the car
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }


        /// <summary>
        ///   The make of the car
        /// </summary>
        /// <example>BMW</example>
        [Required]
        [JsonProperty("make")]
        [MinLength(1)]
        public string Make { get; set; }


        /// <summary>
        ///  The model of the car
        /// </summary>
        /// <example>X6</example>
        [Required]
        [JsonProperty("model")]
        [MinLength(1)]
        public string Model { get; set; }


        /// <summary>
        ///  The miles driven in the car
        /// </summary>
        /// <example>
        ///  13000 
        /// </example>
        [Required]
        [GreaterOrEqualThan(Minimum = 0)]
        [JsonProperty("mileage")]
        public int Mileage { get; set; }

        ///<summary>
        ///  The year when the car was release out of manufacture for sell
        /// </summary>
        /// <example>
        ///  2020
        /// </example>
        [Required]
        [YearStartingBy(StartingYear = 1950)]
        [JsonProperty("releasedYear")]
        public int ReleasedYear { get; set; }


        /// <summary>
        ///  The year when the car receives its first technical revision
        /// </summary>
        /// <example>
        ///   2021
        /// </example>
        [YearStartingBy(StartingYear = 1950)]
        [JsonProperty("lastRevisionYear")]
        public int? LastRevisionYear { get; set; }

        /// <summary>
        ///   Information about car engine transmission : true( automatic), false(manual)
        /// </summary>
        /// <example>
        ///   false
        ///  </example>
        [Required]
        [JsonProperty("isTransmissionAutomatic")]
        public bool IsTransmissionAutomatic { get; set; }
    }
}

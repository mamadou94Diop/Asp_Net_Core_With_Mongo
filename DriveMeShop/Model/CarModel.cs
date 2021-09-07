using System.ComponentModel.DataAnnotations;
using DriveMeShop.CustomAnnotations;
using Newtonsoft.Json;

namespace DriveMeShop.Model
{
    public class CarModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("make")]
        [MinLength(1)]
        public string Make { get; set; }

        [Required]
        [JsonProperty("model")]
        [MinLength(1)]
        public string Model { get; set; }


        [Required]
        [GreaterOrEqualThan(Minimum = 0)]
        [JsonProperty("mileage")]
        public int Mileage { get; set; }


        [Required]
        [YearStartingBy(StartingYear = 1950)]
        [JsonProperty("releasedYear")]
        public int ReleasedYear { get; set; }

        [YearStartingBy(StartingYear = 1950)]
        [JsonProperty("lastRevisionYear")]
        public int? LastRevisionYear { get; set; }

        [Required]
        [JsonProperty("isTransmissionAutomatic")]
        public bool IsTransmissionAutomatic { get; set; }
    }
}

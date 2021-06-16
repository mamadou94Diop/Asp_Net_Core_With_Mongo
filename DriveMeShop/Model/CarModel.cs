using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DriveMeShop.Model
{
    public class CarModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("make")]
        public string Make { get; set; }

        [Required]
        [JsonProperty("model")]
        public string Model { get; set; }


        [Required]
        [JsonProperty("mileage")]
        public int Mileage { get; set; }


        [Required]
        [JsonProperty("releasedYear")]
        public int ReleasedYear { get; set; }

        
        [JsonProperty("lastRevisionYear")]
        public int? LastRevisionYear { get; set; }

        [Required]
        [JsonProperty("isTransmissionAutomatic")]
        public bool IsTransmissionAutomatic { get; set; }
    }
}

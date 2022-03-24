using System.ComponentModel.DataAnnotations;
using DriveMeShop.CustomAnnotations;
using Newtonsoft.Json;

namespace DriveMeShop.Model.V2
{
    public class UnidentifiedCarModel : BaseCarModel
    {
        /// <summary>
        ///  Color of the car
        /// </summary>
        /// <example>
        /// Blue
        /// </example>
        [Required]
        [MinLength(3)]
        [JsonProperty("color")]
        public string Color { get; set; }


        /// <summary>
        ///  The maximum speed the car could reach 
        /// </summary>
        /// <example>
        ///  220
        /// </example>
        [Required]
        [GreaterOrEqualThan(Minimum = 50)]
        [JsonProperty("maxSpeed")]
        public int MaxSpeed { get; set; }
    }
}

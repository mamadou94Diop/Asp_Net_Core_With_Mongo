using Newtonsoft.Json;

namespace DriveMeShop.Model
{
    public class IdentifiedCarModel: UnidentifiedCarModel
    {
        /// <summary>
        ///  Identifier of the car
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

    }
}

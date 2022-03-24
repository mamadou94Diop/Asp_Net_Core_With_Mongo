using Newtonsoft.Json;

namespace DriveMeShop.Model.V1
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

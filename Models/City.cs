using Newtonsoft.Json;

namespace ZiptoCity.Models
{
    public class City
    {
        [JsonProperty("zipcode")]
        public int ZipCode { get; set; }

        [JsonProperty("cityname")]
        public string CityName { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("timezone")]
        public string TimeZone { get; set; }
    }
}

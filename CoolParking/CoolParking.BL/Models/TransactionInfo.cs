// TODO: implement struct TransactionInfo.
//       Necessarily implement the Sum property (decimal) - is used in tests.
//       Other implementation details are up to you, they just have to meet the requirements of the homework.
using System;
using Newtonsoft.Json;

namespace CoolParking.BL.Models
{
    public struct TransactionInfo
    {
        [JsonProperty("vehicleId")]
        public string VehicleId { get; set; }
        [JsonProperty("sum")]
        public decimal Sum { get; set; }
        [JsonProperty("transTime")]
        public DateTime TransTime { get; set; }
    }
}

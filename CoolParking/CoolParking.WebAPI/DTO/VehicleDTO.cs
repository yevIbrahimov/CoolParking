using CoolParking.BL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoolParking.WebAPI.DTO
{
	public class VehicleDTO
	{
		[JsonProperty("id")]
		//[Required]
		public string Id { get; set; }
		[JsonProperty("vehicleType")]
		[Required]
		public VehicleType VehicleType { get; set;  }
		[JsonProperty("balance")]
		[Required]
		public decimal Balance { get; set; }
	}
}

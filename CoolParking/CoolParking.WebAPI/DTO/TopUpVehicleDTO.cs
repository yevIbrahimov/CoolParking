using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoolParking.WebAPI.DTO
{
	public class TopUpVehicleDTO
	{
		[JsonProperty("id")]
		[Required]
		public string Id { get; set; }
		[JsonProperty("Sum")]
		[Required]
		public decimal Sum { get; set; }
	}
}

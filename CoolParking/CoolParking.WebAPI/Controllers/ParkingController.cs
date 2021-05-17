using CoolParking.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolParking.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ParkingController : ControllerBase
	{
		private readonly IParkingService _parkingService;
		public ParkingController(IParkingService parkingService)
		{
			_parkingService = parkingService;
		}

		[HttpGet("balance")]
		public IActionResult GetBalance()
		{
			return Ok(_parkingService.GetBalance()); 
		}

		[HttpGet("capacity")]
		public IActionResult GetCapacity()
		{
			return Ok(_parkingService.GetCapacity());
		}

		[HttpGet("freePlaces")]
		public IActionResult GetFreePlaces()
		{
			return Ok(_parkingService.GetFreePlaces());
		}
	}
}

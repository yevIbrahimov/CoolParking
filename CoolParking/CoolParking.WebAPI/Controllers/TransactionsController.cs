using CoolParking.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolParking.WebAPI.DTO;
using CoolParking.BL.Models;

namespace CoolParking.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionsController : ControllerBase
	{
		private readonly IParkingService _transaction;
		public TransactionsController(IParkingService transaction)
		{
			_transaction = transaction;
		}

		[HttpGet("last")]
		public IActionResult GetLastTransactions()
		{
			return Ok(_transaction.GetLastParkingTransactions());
		}

		[HttpGet("all")]
		public IActionResult AllFromLog()
		{
			try
			{
				return Ok(_transaction.ReadFromLog());
			}
			catch (InvalidOperationException)
			{
				return NotFound();
			}
			
		}

		[HttpPut("topUpVehicle")]
		public IActionResult TopUpVehicle(TopUpVehicleDTO topUpVehicleDTO)
		{
			if (!Vehicle.IdValidation(topUpVehicleDTO.Id) || topUpVehicleDTO.Sum < 0 || topUpVehicleDTO.Sum > decimal.MaxValue)
			{
				return BadRequest();
			}
			if (_transaction.GetVehicleById(topUpVehicleDTO.Id) == null)
			{
				return NotFound();
			}
			_transaction.TopUpVehicle(topUpVehicleDTO.Id, topUpVehicleDTO.Sum);
			return Ok();
		}
	}
}

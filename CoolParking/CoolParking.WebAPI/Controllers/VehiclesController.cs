using CoolParking.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoolParking.BL.Models;
using CoolParking.WebAPI.DTO;

namespace CoolParking.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VehiclesController : ControllerBase
	{
		private readonly IParkingService _vehicles;
		public VehiclesController(IParkingService vehicle)
		{
			_vehicles = vehicle;
		}

		[HttpGet]
		// api/vehicles
		public IActionResult GetVehicles()
		{
			return Ok(_vehicles.GetVehicles());
		}
		[HttpGet("{id}")]
		public IActionResult GetVehicleById(string id)
		{
			Vehicle vehicle;
			try
			{
				vehicle = _vehicles.GetVehicleById(id);
			}
			catch (ArgumentException)
			{
				return BadRequest();
			}
			if (vehicle == null)
			{
				return NotFound();
			}
			return Ok(vehicle);
		}

		[HttpPost]
		public IActionResult AddVehicle([FromBody] VehicleDTO vehicleDTO)
		{
			if (!((vehicleDTO.VehicleType > 0 && (int)vehicleDTO.VehicleType < 5) && (vehicleDTO.Balance > 0 && vehicleDTO.Balance <= decimal.MaxValue)))
			{
				return BadRequest();
			}

			var vehicle = new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), (VehicleType)vehicleDTO.VehicleType, vehicleDTO.Balance);

			_vehicles.AddVehicle(vehicle);

			return StatusCode(201, vehicle);
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteVehicle(string vehicleId)
		{
			if (!Vehicle.IdValidation(vehicleId))
			{
				return BadRequest();
			}
			try
			{
				_vehicles.RemoveVehicle(vehicleId);
			}
			catch (ArgumentException)
			{
				return NotFound();
			}

			return NoContent();
		}

	}
}

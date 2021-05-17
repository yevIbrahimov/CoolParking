using CoolParking.BL.Models;
using CoolParking.WebAPI;
using CoolParking.WebAPI.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserInterface.Factories;
using UserInterface.Interfaces;

namespace UserInterface.Services
{
	class VehiclesHttpService : IVehiclesHttpService
	{
		private readonly HttpClient _client;

		public VehiclesHttpService()
		{
			_client = HttpClientFactory.Create();
		}

		public async Task<List<VehicleDTO>> GetVehicles()
		{
			try
			{
				var response = await _client.GetAsync("vehicles");

				response.EnsureSuccessStatusCode();

				var vehiclesJson = await response.Content.ReadAsStringAsync();

				var vehicles = JsonConvert.DeserializeObject<List<VehicleDTO>>(vehiclesJson);

				return vehicles; 
			}
			catch (Exception ex)
			{
				LogError(ex.Message);
				return null;
			}
		}

		public async Task<VehicleDTO> GetVehicleById(string id)
		{
			try
			{
				var response = await _client.GetAsync($"vehicles/{id}");

				response.EnsureSuccessStatusCode();

				return await JsonParse(response);
			}
			catch (Exception ex)
			{
				LogError(ex.Message);

				return null;
			}
		}
		public async Task<VehicleDTO> AddVehicle(Vehicle vehicleToAdd)
		{
			try
			{
				var newVehicle = JsonConvert.SerializeObject(vehicleToAdd);

				var content = new StringContent(newVehicle, Encoding.UTF8, "application/json");

				var response = await _client.PostAsync("vehicles", content);

				response.EnsureSuccessStatusCode();

				return await JsonParse(response);
			}
			catch (Exception ex)
			{
				LogError(ex.Message);

				return null;
			}
		}

		public async Task<VehicleDTO> DeleteVehicle(string id)
		{
			try
			{
				var response =  await _client.DeleteAsync("vehicles/{id}");

				response.EnsureSuccessStatusCode();

				return await JsonParse(response);
			}
			catch (Exception ex)
			{
				LogError(ex.Message);

				return null;
			}
		}

		private async Task<VehicleDTO> JsonParse(HttpResponseMessage response)
		{
			var vehicleJson = await response.Content.ReadAsStringAsync();

			var vehicle = JsonConvert.DeserializeObject<VehicleDTO>(vehicleJson);

			return vehicle;
		}

		private void LogError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("\n");
			Console.WriteLine(message);
			Console.WriteLine("\n");
			Console.ForegroundColor = ConsoleColor.White;
		}

		public void Dispose()
		{
			_client.Dispose();
		}



	}
}

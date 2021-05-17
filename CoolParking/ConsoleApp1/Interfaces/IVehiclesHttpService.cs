using CoolParking.BL.Models;
using CoolParking.WebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Interfaces
{
	public interface IVehiclesHttpService: IDisposable
	{
		Task<List<VehicleDTO>> GetVehicles();
		Task<VehicleDTO> GetVehicleById(string id);
		Task<VehicleDTO> AddVehicle(Vehicle vehicle);
		Task<VehicleDTO> DeleteVehicle(string id);
	}
}

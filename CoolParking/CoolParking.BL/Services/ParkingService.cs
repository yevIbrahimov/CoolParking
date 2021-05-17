// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace CoolParking.BL.Services
{
    public class ParkingService : IParkingService
    {
        private readonly Parking parking = Parking.GetInstance();
        private readonly List<TransactionInfo> transactions = new List<TransactionInfo>(0);
        private readonly ITimerService withdrawTimer;
        private readonly ITimerService logTimer;
        private readonly ILogService logService;
        public ParkingService(ITimerService withdrawTimer, ITimerService logTimer, ILogService logService)
        {
            this.withdrawTimer = withdrawTimer;
            this.logTimer = logTimer;
            this.logService = logService;
            withdrawTimer.Elapsed += MakeTransaction;
            logTimer.Elapsed += WriteToLog;
            logTimer.Interval = Settings.LogWritePeriod;
            logTimer.Start();
            withdrawTimer.Interval = Settings.ChangeOffPeriod;
            withdrawTimer.Start();
        }
        public decimal GetTarrif(Vehicle item)
        {
            return Settings.UpToCarTariff(item.VehicleType);
        }
        public decimal GetBalance()
        {
            return parking.Balance;
        }
        public int GetCapacity()
        {
            return parking.Vehicles.Capacity;
        }
        public int GetFreePlaces()
        {
            return parking.Vehicles.Capacity - parking.Vehicles.Count;
        }
        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            return parking.Vehicles.AsReadOnly();
        }
        public void AddVehicle(Vehicle vehicle)
        {
            if (GetFreePlaces() == 0 || parking.Vehicles.Find(v => v.Id == vehicle.Id) != null)
            {
                throw new ArgumentException();
            }
            else
            {
                parking.Vehicles.Add(vehicle);
            }
        }
        public void RemoveVehicle(string vehicleId)
        {
            var vehicleToRemove = parking.Vehicles.Find(o => o.Id == vehicleId);
            if (vehicleToRemove == null || vehicleToRemove.Balance < 0)
            {
                throw new ArgumentException();
            }
            else
            {
                parking.Vehicles.Remove(vehicleToRemove);
            }
        }
        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            var vehicle = parking.Vehicles.Find(v => v.Id == vehicleId);
            if (vehicle != null && sum > 0 && sum < decimal.MaxValue)
            {
                parking.Vehicles.Find(v => v.Id == vehicleId).Balance += sum;
            }
            else
            {
                throw new ArgumentException();
            }

        }
        public void MakeTransaction(object sender, EventArgs e)
        {
            foreach (var i in parking.Vehicles)
            {
                decimal tarrif = GetTarrif(i);
                decimal sum;
                if (i.Balance > 0 && i.Balance - tarrif > 0)
                {
                    sum = tarrif;
                }
                else
                {
                    sum = tarrif * (decimal)Settings.PenaltyCoefficient;
                }
                parking.WithdrawSum(i.Id, sum);
                transactions.Add(new TransactionInfo() { VehicleId = i.Id, Sum = sum, TransTime = DateTime.Now });
            }
        }
        public void WriteToLog(object sender, EventArgs e)
        {
            string writeToFile = "";
			for (int i = 0; i < transactions.Count; i++)
			{
                writeToFile += transactions[i].TransTime.ToString() + "\t" + transactions[i].VehicleId + "\t" + transactions[i].Sum + "\n";
            }
            logService.Write(writeToFile);
            transactions.Clear();
        }
        public TransactionInfo[] GetLastParkingTransactions()
        {
            return transactions.ToArray();
        }
        public string ReadFromLog()
        {
            return logService.Read();
        }
        public void Dispose()
        {
            parking.Dispose();
            transactions.Clear();
        }

		public Vehicle GetVehicleById(string vehicleId)
		{
            if(!Vehicle.IdValidation(vehicleId))
			{
                throw new ArgumentException();
			}

            return parking.Vehicles.Find(v => v.Id == vehicleId);
		}
	}
}
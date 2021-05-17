using System;
using System.Text.RegularExpressions;
using CoolParking.BL.Services;
using CoolParking.BL.Models;
using CoolParking.BL.Interfaces;
using System.IO;
using System.Reflection;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using CoolParking.WebAPI.DTO;
using System.Text;
using System.Threading.Tasks;
using UserInterface.Services;
using System.Threading;
using System.Globalization;

namespace UserInterface
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			await Hello();
		}

		private static async Task Hello()
		{
			Console.WriteLine("\t\tHello! Welcome to Cool Parking APP!\n\tYou can get info using commands from menu.\n\tWARNING! Do not use other numbers.\n\tHere You can get info about:");
			await Menu();
		}
		private static async Task Menu()
		{
			var parkingHttpService = new ParkingHttpService();
			var vehiclesHttpService = new VehiclesHttpService();
			var transactionsHttpService = new TransactionsHttpService();

			do
			{
				Console.WriteLine("\t\tMenu");
				Console.WriteLine("\t - Parking balance - 1\n\t " +
					"- Capacity - 2\n\t " +
					"- Free places - 3\n\t " +
					"- Vehicles at the parking - 4\n\t " +
					"- Vehicle by id - 5\n\t " +
					"- Put vehicle - 6\n\t " +
					"- Get vehicle from the parking  - 7\n\t " +
					"- Transactions for a period - 8\n\t " +
					"- All transactions - 9\n\t " +
					"- Top up vehicle - 10\n\t " +
					" Or write exit to out");
				var input = Console.ReadLine();
				Regex reg = new Regex(@"^[1-9]{1}$");
				if (!reg.IsMatch(input) && input != "10" && input.ToUpper() != "EXIT")
				{
					Console.WriteLine("No such option");
				}
				else
				{
					if (input.ToUpper() == "EXIT")
					{
						break;
					}
					HttpClient client = new HttpClient();
					switch (Int32.Parse(input))
					{
						case 1:
							var balance = await parkingHttpService.GetBalance();
							Console.WriteLine($"Balance is {balance}");
							break;

						case 2:
							var capacity = await parkingHttpService.GetCapacity();
							Console.WriteLine($"Capacity is {capacity}");
							break;

						case 3:
							var freePlaces = await parkingHttpService.GetFreePlaces();
							Console.WriteLine($"Free places: {freePlaces}");
							break;

						case 4:
							var vehicles = await vehiclesHttpService.GetVehicles();
							if (vehicles.Count == 0)
							{
								Console.WriteLine("No vehicles at the parking ");
							}
							else
							{
								Console.WriteLine("Vehicles at the parking: ");
								foreach (var oneVehicle in vehicles)
								{
									Console.WriteLine($"{oneVehicle.Id} - {oneVehicle.VehicleType} - {oneVehicle.Balance}");
								}
							}
							break;

						case 5:
							var vehicle = await vehiclesHttpService.GetVehicleById(InputId());
							Console.WriteLine($"Vehicle: ID - {vehicle.Id} - Type - {vehicle.VehicleType} - Balance - {vehicle.Balance}");
							break;

						case 6:
							await vehiclesHttpService.AddVehicle(new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), ChooseVehicle(), PutBalance()));
							break;

						case 7:
							await vehiclesHttpService.DeleteVehicle(InputId());

							Console.WriteLine("Vehicle successfully get");
							break;

						case 8:
							var transactionsPeriod = await transactionsHttpService.GetLastTransactions();
							Console.WriteLine("Transactions for a period: ");
							if (transactionsPeriod.Count == 0)
							{
								Console.WriteLine("No transactions for a period");
							}
							else
							{
								foreach (var transaction in transactionsPeriod)
								{
									Console.WriteLine($"{transaction.VehicleId} - {transaction.Sum} - {transaction.TransTime}");
								}
							}
							break;

						case 9:
							var transactions = await transactionsHttpService.AllFromLog();
							Console.WriteLine("All transactions: ");
							if (transactions.Count == 0)
							{
								Console.WriteLine("No transactions at all");
							}
							else
							{
								foreach (var transaction in transactions)
								{
									Console.WriteLine($"{transaction.VehicleId} - {transaction.Sum} - {transaction.TransTime}");
								}
							}
							break;

						case 10:
							Console.WriteLine("Enter sum: ");
							decimal sum = decimal.Parse(Console.ReadLine());
							await transactionsHttpService.TopUpVehicle(new TopUpVehicleDTO() { Id = InputId(), Sum = sum });

							Console.WriteLine("Successfull operation");
							break;
					}
				}
			} 
			while (true);
		}

		private static string InputId()
		{
			string inputId;
			do
			{
				Console.WriteLine("Enter id: ");
				inputId = Console.ReadLine();
			} while (!Vehicle.IdValidation(inputId));

			return inputId;
		}
		private static decimal PutBalance()
		{
			Console.WriteLine("Write your balance: ");
			decimal balance = Decimal.Parse(Console.ReadLine());
			while (balance < 0 || balance > Decimal.MaxValue)
			{
				Console.WriteLine("Fake balance. Try again: ");
				balance = Decimal.Parse(Console.ReadLine());
			}
			return balance;
		}

		private static VehicleType ChooseVehicle()
		{
			Console.WriteLine("Choose vehicle: Passenger Car, Truck, Bus, Motorcycle: ");
			string vehicle = Console.ReadLine();
			while (vehicle.ToUpper() != "BUS" && vehicle.ToUpper() != "TRUCK" && vehicle.ToUpper() != "MOTORCYCLE" && vehicle.ToUpper() != "PASSENGER CAR")
			{
				Console.WriteLine("No such car, try again: ");
				vehicle = Console.ReadLine();
			}
			switch (vehicle.ToUpper())
			{
				case "PASSENGER CAR":
					return VehicleType.PassengerCar;
				case "TRUCK":
					return VehicleType.Truck;
				case "BUS":
					return VehicleType.Bus;
				case "MOTORCYCLE":
					return VehicleType.Motorcycle;
			}
			return 0;
		}
	}
}

using CoolParking.BL.Models;
using CoolParking.WebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Interfaces
{
	interface ITransactionsHttpService:IDisposable
	{
		Task<List<TransactionInfo>> GetLastTransactions();
		Task<List<TransactionInfo>> AllFromLog();
		Task<TopUpVehicleDTO> TopUpVehicle(TopUpVehicleDTO topUpVehicleDTO);
	}
}

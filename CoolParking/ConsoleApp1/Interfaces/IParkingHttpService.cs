using System;
using System.Threading.Tasks;

namespace UserInterface.Interfaces
{
	public interface IParkingHttpService : IDisposable
	{
		Task<decimal?> GetBalance();
		Task<int?> GetCapacity();
		Task<int?> GetFreePlaces();
	}
}

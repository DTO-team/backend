using Models.Models;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IUserService
	{
		User GetUserByUserEmail(string email);
	}
}

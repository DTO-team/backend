using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;
using System.Linq;

namespace CapstoneOnGoing.Services.Implements
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		public UserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public User GetUserByUserEmail(string email)
		{
			User user = null;
			if(string.IsNullOrEmpty(email)){

			}else{
				user = _unitOfWork.User.Get(x => x.Email == email,null,"Role").First();
			}
			return user;
		}
	}
}

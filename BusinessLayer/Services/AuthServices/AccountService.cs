using Microsoft.AspNet.Identity;

namespace BusinessLayer.Services.AuthServices
{
	public class AccountService : IAccountService
	{
		private IPasswordHasher _passwordHasher;
		private IDataMappingService<User> _dataMapper;

		public AccountService(IPasswordHasher passwordHasher, IDataMappingService<User> dataMapper)
		{
			_passwordHasher = passwordHasher;
			_dataMapper = dataMapper;
		}
		public async Task<User?> Login(string email, string password)
		{
			User u = await _dataMapper.SelectWithCondition(new Dictionary<string, object>() { {"email", email } });
			if (u == null)
			{
				return u;
			}

			PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(u.Password, password);
			if(result != PasswordVerificationResult.Success)
			{
				return null;
			}

			return u;
		}

		public async Task<bool> Register(string name, string surname, string email, string password)
		{
			if(await _dataMapper.SelectWithCondition(new Dictionary<string, object>() { { "email", email } }) is not null)
				return false;
			return await _dataMapper.Insert(new User(name, surname, email, password));
		}
	}
}

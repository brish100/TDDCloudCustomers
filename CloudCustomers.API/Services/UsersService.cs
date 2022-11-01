using CloudCustomers.API.Models;

public interface IUsersService {
    public Task<List<User>> GetAllUsers();
}

public class UsersService : IUsersService {
    private UsersService()
    {
    }

    public Task<List<User>> GetAllUsers()
    {
        throw new NotImplementedException();
    }
}
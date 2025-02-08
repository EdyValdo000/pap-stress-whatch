using pap.Model;
using pap.Repositore;

namespace pap.Service;
public class UserService
{
    private readonly UserRepository _repositore;

    public UserService(UserRepository repositore)
    {
        _repositore = repositore;
    }

    public Task<int> Save(User user) => _repositore.SaveUserAsync(user);
    public Task<User> GetById(int id) => _repositore.GetUserByIdAsync(id);
    public Task<List<User>> GetAll() => _repositore.GetUsersAsync();
    public Task<int> Update(User user) => _repositore.UpdateUserAsync(user);
    public Task<int> Delete(User user) => _repositore.DeleteUserAsync(user);
}

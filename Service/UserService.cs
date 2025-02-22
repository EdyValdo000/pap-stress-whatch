using pap.Model;
using pap.Repositore;

namespace pap.Service;
public class UserService
{
    private readonly UserRepository repositore;

    public UserService(UserRepository repositore)
    {
        this.repositore = repositore;
    }
    // Verifica se o nome e a senha existem no banco de dados
    public Task<bool> CheckCredentials(string name, string password) => repositore.CheckUserCredentialsAsync(name, password);

    // Retorna o Id do usuário pelo nome e senha
    public Task<User> GetUserByNameAndPassword(string name, string password) => repositore.GetUserByNameAndPasswordAsync(name, password)!;

    public Task<int> Save(User user) => repositore.SaveUserAsync(user);
    public Task<User> GetById(int id) => repositore.GetUserByIdAsync(id);
    public Task<List<User>> GetAll() => repositore.GetUsersAsync();
    public Task<int> Update(User user) => repositore.UpdateUserAsync(user);
    public Task<int> Delete(User user) => repositore.DeleteUserAsync(user);
}

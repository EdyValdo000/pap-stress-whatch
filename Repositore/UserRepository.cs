using SQLite;
using pap.Model;
using pap.Database;

namespace pap.Repositore;
public class UserRepository : ConectionDatabase
{   
    public UserRepository()
    {
    }

    // Retorna o Id do usuário pelo nome e senha
    public async Task<User?> GetUserByNameAndPasswordAsync(string name, string password)
    {
        var user = await database.Table<User>()
                                 .Where(u => u.Name == name && u.Password == password)
                                 .FirstOrDefaultAsync();
        return user; // Retorna o Id do usuário ou null se não existir
    }

    // Verifica se o nome e a senha existem no banco de dados
    public async Task<bool> CheckUserCredentialsAsync(string name, string password)
    {
        var user = await database.Table<User>()
                                 .Where(u => u.Name == name && u.Password == password)
                                 .FirstOrDefaultAsync();
        return user != null; // Retorna true se o usuário existir
    }

    // Create
    public Task<int> SaveUserAsync(User user)
    {
        return database.InsertAsync(user);
    }

    // Read - Todos os usuários
    public Task<List<User>> GetUsersAsync()
    {
        return database.Table<User>().ToListAsync();
    }

    // Read - Buscar usuário por ID
    public Task<User> GetUserByIdAsync(int id)
    {
        return database.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    // Update
    public Task<int> UpdateUserAsync(User user)
    {
        return database.UpdateAsync(user);
    }

    // Delete
    public Task<int> DeleteUserAsync(User user)
    {
        return database.DeleteAsync(user);
    }

    // Extra 1 - Buscar usuário pelo nome
    public Task<User> GetUserByNameAsync(string name)
    {
        return database.Table<User>().Where(u => u.Name == name).FirstOrDefaultAsync();
    }

    // Extra 2 - Contar número de usuários cadastrados
    public Task<int> GetUserCountAsync()
    {
        return database.Table<User>().CountAsync();
    }

    // Extra 3 - Obter os últimos usuários cadastrados (limit 5)
    public Task<List<User>> GetLastUsersAsync(int limit = 5)
    {
        return database.Table<User>().OrderByDescending(u => u.CreatedAt).Take(limit).ToListAsync();
    }
}

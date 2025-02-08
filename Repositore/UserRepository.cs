using SQLite;
using pap.Model;

namespace pap.Repositore;
public class UserRepository
{
    private readonly SQLiteAsyncConnection _database;

    public UserRepository(SQLiteAsyncConnection database)
    {
        _database = database;
    }

    // Create
    public Task<int> SaveUserAsync(User user)
    {
        return _database.InsertAsync(user);
    }

    // Read - Todos os usuários
    public Task<List<User>> GetUsersAsync()
    {
        return _database.Table<User>().ToListAsync();
    }

    // Read - Buscar usuário por ID
    public Task<User> GetUserByIdAsync(int id)
    {
        return _database.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    // Update
    public Task<int> UpdateUserAsync(User user)
    {
        return _database.UpdateAsync(user);
    }

    // Delete
    public Task<int> DeleteUserAsync(User user)
    {
        return _database.DeleteAsync(user);
    }

    // Extra 1 - Buscar usuário pelo nome
    public Task<User> GetUserByNameAsync(string name)
    {
        return _database.Table<User>().Where(u => u.Name == name).FirstOrDefaultAsync();
    }

    // Extra 2 - Contar número de usuários cadastrados
    public Task<int> GetUserCountAsync()
    {
        return _database.Table<User>().CountAsync();
    }

    // Extra 3 - Obter os últimos usuários cadastrados (limit 5)
    public Task<List<User>> GetLastUsersAsync(int limit = 5)
    {
        return _database.Table<User>().OrderByDescending(u => u.CreatedAt).Take(limit).ToListAsync();
    }
}

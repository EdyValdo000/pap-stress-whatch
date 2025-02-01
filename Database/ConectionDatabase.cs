using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pap.Database
{
    public class ConectionDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public ConectionDatabase()
        {
            var local = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pap.db3");
            _database = new SQLiteAsyncConnection(local);
            _database.CreateTableAsync<Users>().Wait();
        }

        // Método para salvar um usuário (Create)
        public Task<int> SaveUser(Users user)
        {
            return _database.InsertAsync(user);
        }

        // Método para obter todos os usuários (Read)
        public Task<List<Users>> GetUsersAsync()
        {
            return _database.Table<Users>().ToListAsync();
        }

        // Método para obter um usuário por ID (Read)
        public Task<Users> GetUserByIdAsync(int id)
        {
            return _database.Table<Users>().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        // Método para atualizar um usuário (Update)
        public Task<int> UpdateUserAsync(Users user)
        {
            return _database.UpdateAsync(user);
        }

        // Método para deletar um usuário (Delete)
        public Task<int> DeleteUserAsync(Users user)
        {
            return _database.DeleteAsync(user);
        }
    }
}
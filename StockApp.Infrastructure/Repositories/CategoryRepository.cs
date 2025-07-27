using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using StockApp.Core.Interfaces;
using StockApp.Core.Models;
using Microsoft.Extensions.Options;
using StockApp.Core.Options;

namespace StockApp.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public CategoryRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
        
        public async Task<int> AddAsync(Category category)
        {
            using var connection = CreateConnection();
            var sql = @"
            INSERT INTO public.""categories"" (""name"")
            VALUES (@Name)
            RETURNING ""id"";";
            return await connection.ExecuteScalarAsync<int>(sql, category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = @"DELETE FROM public.""categories"" WHERE ""id"" = @Id;";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM public.\"categories\"";
            return await connection.QueryAsync<Category>(sql);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM public.\"categories\" WHERE \"id\" = @Id";
            return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE public.""categories""
                SET ""name"" = @Name
                WHERE ""id"" = @Id;";
            var affectedRows = await connection.ExecuteAsync(sql, category);
            return affectedRows > 0;
        }

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_databaseSettings.DefaultConnection);
        }
    }
}
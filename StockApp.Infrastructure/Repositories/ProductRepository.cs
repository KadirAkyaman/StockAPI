using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using StockApp.Core.Interfaces;
using StockApp.Core.Models;
using StockApp.Core.Options;


namespace NewAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public ProductRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }

        public async Task<int> AddAsync(Product product)
        {
            using var connection = CreateConnection();

            var sql = @"
            INSERT INTO public.""products"" (""name"", ""price"", ""stock"", ""categoryid"")
            VALUES (@Name, @Price, @Stock, @CategoryId)
            RETURNING ""id"";";

            return await connection.ExecuteScalarAsync<int>(sql, product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = @"DELETE FROM public.""products"" WHERE ""id"" = @Id;";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM public.\"products\"";
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM public.\"products\" WHERE \"id\" = @Id";
            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE public.""products""
                SET
                    ""name"" = @Name,
                    ""price"" = @Price,
                    ""stock"" = @Stock,
                    ""categoryid"" = @CategoryId
                WHERE
                    ""id"" = @Id;";

            var affectedRows = await connection.ExecuteAsync(sql, product);
            return affectedRows > 0;
        }

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_databaseSettings.DefaultConnection);
        }
    }
}
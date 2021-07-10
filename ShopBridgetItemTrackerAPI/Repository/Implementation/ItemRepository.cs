using Dapper;
using Microsoft.Extensions.Configuration;
using ShopBridgeItemTrackerAPI.Entities;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Repository.Implementation
{
    public class ItemRepository : IItemRepository
    {
        private readonly IConfiguration _config;
        public ItemRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("MyConnectionString"));
            }
        }
        public async Task<int> AddUpdateItemAsync(Item item)
        {
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var parameters = new DynamicParameters(item);
                parameters.Output(item, x => x.Id);

                await conn.ExecuteAsync("[dbo].[usp_AddUpdateItem]", parameters, commandType: CommandType.StoredProcedure);

                return item.Id;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                await conn.ExecuteAsync("DELETE FROM dbo.ItemMaster WHERE Id = @Id", new { Id = id });
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
            
        }

        public async Task<Item> GetItemAsync(int id)
        {
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var item = await conn.QueryAsync<Item>("SELECT Id, Name, Price, Description FROM dbo.ItemMaster WHERE Id = @Id", new { Id = id });

            return item.FirstOrDefault();
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var clients = await conn.QueryAsync<Item>("SELECT Id, Name, Price, Description FROM dbo.ItemMaster").ConfigureAwait(false);

            return clients.ToList();
        }
    }
}

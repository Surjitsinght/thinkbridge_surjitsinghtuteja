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

        public async Task<Items> GetItemAsync(int id)
        {
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var item = await conn.QueryAsync<Items>("SELECT Id, ImgURL, [Name], Price, [Description], CreatedDate FROM dbo.ItemMaster WHERE Id = @Id", new { Id = id });

            return item.FirstOrDefault();
        }

        public async Task<List<Items>> GetItemsAsync(string keyword, int? pageNo, int? pageSize, string sortField, string sortExp)
        {
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var items = await conn.QueryAsync<Items>(
                "[dbo].[usp_GetItems]",
                new
                {
                    Keyword = keyword,
                    PageNo = pageNo,
                    PageSize = pageSize,
                    SortField = sortField,
                    SortExp = sortExp
                },
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }
    }
}

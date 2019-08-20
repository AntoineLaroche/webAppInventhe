using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;
using webAppInventhe.Shared.Model;
using System.Threading.Tasks;

namespace webAppInventhe.Shared
{
    public class Repository : IRepository
    {
        private readonly CloudStorageAccount _storageAccount;
        public Repository(CloudStorageAccount account)
        {
            _storageAccount = account;
        }

        public async Task<Products> GetProduct(string name)
        {
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Products");

            TableQuery<ProductsEntity> query = new TableQuery<ProductsEntity>()
                .Where(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name));

            var result = await table.ExecuteQuerySegmentedAsync(query, null);
            return new Products
            {
                Name = result.FirstOrDefault().RowKey,
                Quantity = result.FirstOrDefault().Quantity,
                Description = result.FirstOrDefault().Description
            };
        }

        public async Task<IList<Products>> GetProducts()
        {
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Products");

            var productsEntities = await Task.FromResult(table.ExecuteQuery(new TableQuery<ProductsEntity>()).ToList());
            var products = new List<Products>();
            foreach(var entity in productsEntities)
            {
                products.Add(new Products
                {
                    Name = entity.RowKey,
                    Quantity = entity.Quantity,
                    Description = entity.Description
                });
            }
            return products;
        }

        public async Task UpdateProduct(Products products)
        {
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Products");

            var productsEntity = new ProductsEntity
            {
                RowKey = products.Name,
                PartitionKey = "Ingredient",
                Description = products.Description,
                Quantity = products.Quantity
            };

            var operation = TableOperation.InsertOrReplace(productsEntity);
            await table.ExecuteAsync(operation);
        }

        public async Task DeleteProduct(string name)
        {
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Products");

            TableQuery<ProductsEntity> query = new TableQuery<ProductsEntity>()
                .Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Ingredient"),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name)
                ));

            var product = await table.ExecuteQuerySegmentedAsync(query, null);
            var productToDelete = product.First();
            var operation = TableOperation.Delete(productToDelete);
            await table.ExecuteAsync(operation);
        }
    }
}

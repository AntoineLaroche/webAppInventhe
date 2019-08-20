using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Cosmos.Table;

namespace webAppInventhe.Shared.Model
{
    public class ProductsEntity : TableEntity
    {
        public ProductsEntity()
        {
        }

        public ProductsEntity(string type,string name)
        {
            PartitionKey = type;
            RowKey = name;
        }

        public double Quantity { get; set; } = 0.0;
        public string Description { get; set; } = string.Empty;
    }
}

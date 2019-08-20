using System.Collections.Generic;
using System.Threading.Tasks;
using webAppInventhe.Shared.Model;

namespace webAppInventhe.Shared
{
    public interface IRepository
    {
        Task<Products> GetProduct(string name);
        Task<IList<Products>> GetProducts();
        Task UpdateProduct(Products products);
        Task DeleteProduct(string name);
    }
}
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _shopOnlineDbContext;
        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext) 
        {
            this._shopOnlineDbContext = shopOnlineDbContext;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categoryList = await _shopOnlineDbContext.ProductCategories.ToListAsync();
            return categoryList;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _shopOnlineDbContext.ProductCategories.FindAsync(id);
            return category;
        }

        public async Task<Product> GetItem(int id)
        {
            var item = await _shopOnlineDbContext.Products.FindAsync(id);
            return item;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await this._shopOnlineDbContext.Products.ToListAsync();
            return products;
        }
    }
}

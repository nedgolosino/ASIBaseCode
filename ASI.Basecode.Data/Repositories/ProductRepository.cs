using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AsiBasecodeDBContext _context;

        public ProductRepository(AsiBasecodeDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

        }
        public void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.Find(product.ProductId);
            if (existingProduct != null) 
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                
                _context.SaveChanges();
            }
        }

    public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null) 
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}

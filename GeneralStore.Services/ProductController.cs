using GeneralStore.Data;
using GeneralStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStore.Services
{
    public class ProductController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();
        //Create(POST)
        [HttpPost]
        public async Task<IHttpActionResult> AddProduct([FromBody] Product model)
        {
            if (model is null)
                return BadRequest("Request body cannot be empty.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            return Ok($"\"{model.Name}\" successfulled added.");

        }
        //Get All Products(GET)
        [HttpGet]
        public async Task<IHttpActionResult> GetAllProducts()
        {
            List<Product> _products = await _context.Products.ToListAsync();
            return Ok(_products);
        }

        //Get a Product by its SKU(GET)
        [HttpGet]
        public async Task<IHttpActionResult> GetProductBySKU([FromUri] int sku)
        {
            Product product = await _context.Products.FindAsync(sku);
            if (product != null)
                return Ok(product);

            return NotFound();
        }

        //Update an existing Product by its SKU(PUT)
        [HttpPut]
        public async Task<IHttpActionResult> UpdateProduct([FromUri] int sku, [FromBody] Product updatedModel)
        {
            if (sku != updatedModel?.SKU)
                return BadRequest("SKUs do not match.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product product = await _context.Products.FindAsync(sku);

            if (product is null)
                return NotFound();

            product.Name = updatedModel.Name;
            product.Cost = updatedModel.Cost;
            product.NumberInInventory = updatedModel.NumberInInventory;
            await _context.SaveChangesAsync();
            return Ok($"\"{product.Name}\" successfully updated.");
        }

        //Delete an existing Product by its SKU(DELETE)
        [HttpDelete]
        public async Task<IHttpActionResult> RemoveProduct([FromUri] int sku)
        {
            Product product = await _context.Products.FindAsync(sku);

            if (product is null)
                return NotFound();

            _context.Products.Remove(product);
            return Ok($"\"{product.Name}\" successfully removed.");
        }

    }
}

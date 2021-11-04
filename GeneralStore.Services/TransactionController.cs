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
    public class TransactionController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();
        
        //Create(POST)
            //When creating Transactions, make sure to:
            //-Verify that the product is in stock
            //-Check that there is enough product to complete the Transaction
            //-Remove the Products that were bought
        [HttpPost]
        public async Task<IHttpActionResult> CreateTransaction([FromBody] Transaction model)
        {
            if (model is null)
                return BadRequest("Request body cannot be empty.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (model.Product.NumberInInventory < model.ItemCount)
                return BadRequest("Amount of requested product not available.");

            Product product = await _context.Products.FindAsync(model.ProductSKU);
            product.NumberInInventory -= model.ItemCount;
            await _context.SaveChangesAsync();
            _context.Transactions.Add(model);
            return Ok("Transaction successfully created.");
        }

        //Get All Transactions(GET)
        [HttpGet]
        public async Task<IHttpActionResult> GetAllTransactions()
        {
            List<Transaction> _transactions = await _context.Transactions.ToListAsync();
            return Ok(_transactions);
        }

        //Get All Transactions by Customer ID(GET)
        [HttpGet]
        public async Task<IHttpActionResult> GetTransactionsByCustomerId([FromUri] int customerId)
        {
            List<Transaction> _transactions = await _context.Transactions.ToListAsync();

            foreach(Transaction t in _transactions)
            {
                if (t.CustomerId == customerId)
                    _transactions.Add(t);
            }
            if (_transactions.Count == 0)
                return NotFound();

            return Ok(_transactions);
        }

        //Get a Transaction by its ID(GET)
        [HttpGet]
        public async Task <IHttpActionResult> GetTransactionById([FromUri] int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction is null)
                return NotFound();

            return Ok(transaction);
        }

        //Update an existing Transaction by its ID(PUT)
            //When updating Transactions, make sure to:
            //-Verify any Product changes
            //-Update Product Inventory to reflect updated Transaction
        [HttpPut]
        public async Task<IHttpActionResult> UpdateTransaction([FromUri] int id, [FromBody] Transaction updatedModel)
        {
            if (id != updatedModel?.Id)
                return BadRequest("IDs do not match.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction is null)
                return NotFound();

            if (updatedModel.ItemCount > updatedModel.Product.NumberInInventory)
                return BadRequest("Amount of requested product not available.");

            if(updatedModel.Product != transaction.Product)
            {
                transaction.Product.NumberInInventory += transaction.ItemCount;
                updatedModel.Product.NumberInInventory -= updatedModel.ItemCount;
            }
            else
            {
                transaction.Product.NumberInInventory += transaction.ItemCount - updatedModel.ItemCount;
            }

            transaction.Customer = updatedModel.Customer;
            transaction.Product = updatedModel.Product;
            transaction.ItemCount = updatedModel.ItemCount;
            transaction.DateOfTransaction = updatedModel.DateOfTransaction;
            await _context.SaveChangesAsync();
            return Ok("Transaction updated.");
        }

        //Delete an existing Transaction by its ID(DELETE)
            //When deleting Transactions, make sure to:
            //-Update Product Inventory to reflect updated Transaction
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTransaction([FromUri] int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction is null)
                return NotFound();

            transaction.Product.NumberInInventory += transaction.ItemCount;
            _context.Transactions.Remove(transaction);
            return Ok("Transaction deleted.");
        }
    }
}

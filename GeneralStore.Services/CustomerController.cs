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
    public class CustomerController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();
        //Create(POST)
        [HttpPost]
        public async Task<IHttpActionResult> AddCustomer([FromBody] Customer model)
        {
            if (model is null)
                return BadRequest("Request body cannot be empty.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Customers.Add(model);
            await _context.SaveChangesAsync();
            return Ok($"\"{model.FullName}\" successfulled added.");

        }
        //Get All Customers(GET)
        [HttpGet]
        public async Task<IHttpActionResult> GetAllCustomers()
        {
            List<Customer> _customers = await _context.Customers.ToListAsync();
            return Ok(_customers);
        }

        //Get a Customer by its ID(GET)
        [HttpGet]
        public async Task<IHttpActionResult> GetCustomerById([FromUri] int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);
            if (customer != null)
                return Ok(customer);

            return NotFound();
        }

        //Update an existing Customer by its ID(PUT)
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCustomer([FromUri] int id, [FromBody]Customer updatedModel)
        {
            if (id != updatedModel?.Id)
                return BadRequest("IDs do not match.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Customer customer = await _context.Customers.FindAsync(id);

            if (customer is null)
                return NotFound();

            customer.FirstName = updatedModel.FirstName;
            customer.LastName = updatedModel.LastName;
            await _context.SaveChangesAsync();
            return Ok($"\"{customer.FullName}\" successfully updated.");
        }

        //Delete an existing Customer by its ID(DELETE)
        [HttpDelete]
        public async Task<IHttpActionResult> RemoveCustomer([FromUri] int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);

            if (customer is null)
                return NotFound();

            _context.Customers.Remove(customer);
            return Ok($"\"{customer.FullName}\" successfully removed.");
        }

    }
}

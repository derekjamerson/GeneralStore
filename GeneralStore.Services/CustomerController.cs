using GeneralStore.Data;
using GeneralStore.Models;
using System;
using System.Collections.Generic;
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

        //Get a Customer by its ID(GET)

        //Update an existing Customer by its ID(PUT)

        //Delete an existing Customer by its ID(DELETE)

    }
}

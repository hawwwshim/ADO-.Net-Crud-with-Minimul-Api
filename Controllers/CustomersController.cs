using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Controllers
{
    [Route("api/")]
    public class CustomersController : ControllerBase
    {
        private readonly TodoDb _Context;
        public CustomersController(TodoDb context)
        {
            _Context = context;
        }
        // get all customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            try
            {
                var customers = await _Context.Customers.ToListAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Internal server error");
            }
        }

        // get customers by id
        [HttpGet]
        [Route("customerbyid")]
        public async Task<ActionResult<Customers>> GetCustomersById(int id)
        {
            var customer = await _Context.Customers.FindAsync(id);
            if(customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // add customer
        [HttpPost]
        [Route("addcustomer")]

        public async Task<ActionResult<Customers>> AddCustomer([FromBody] Customers customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer object is null");
            }

            _Context.Customers.Add(customer);
            await _Context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddCustomer), new { id = customer.CustomerId }, customer);

        }

        // update customer
        [HttpPost]
        [Route("updatecustomer")]

        public async Task<IActionResult> updateCustomer(int id, [FromBody] Customers customer)
        {

            if (id != customer.CustomerId)
            {
                return BadRequest("Customer ID in the URL does not match the ID in the request body.");
            }
            var existingCustomer = await _Context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound(); 
            }

            existingCustomer.CustomerName   = customer.CustomerName;
            existingCustomer.ContactName = customer.ContactName;
            existingCustomer.Country    = customer.Country;

            _Context.Entry(existingCustomer).State = EntityState.Modified;

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound(); 
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("deletecustomer")]

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _Context.Customers.FindAsync(id);
            try
            {
                if (customer == null)
                {
                    return NotFound();
                }
                _Context.Customers.Remove(customer);
                await _Context.SaveChangesAsync();
                Console.WriteLine($"Record Deleted Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Internal server error");
            }
            //var customer = await _Context.Customers.FindAsync(id);

            //if (customer == null)
            //{
            //    return NotFound();
            //}
            //_Context.Customers.Remove(customer);
            //await _Context.SaveChangesAsync();

           return NoContent();
        }
        private bool CustomerExists(int id)
        {
            return _Context.Customers.Any(e => e.CustomerId == id);
        }
    }
  
}

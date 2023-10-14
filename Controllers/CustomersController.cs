using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;
using WebApiApp.DTO;

namespace WebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly EshopDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(EshopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            var customersDto = _mapper.Map<IEnumerable<CustomerReadOnlyDTO>>(customers);
            return Ok(customersDto);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerReadOnlyDTO>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (_context.Customers == null || customer is null)
            {
              return NotFound();
            }

            var customerDto = _mapper.Map<CustomerReadOnlyDTO>(customer);
            return Ok(customerDto);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerUpdateDTO customerUpdateDto)
        {
            if (id != customerUpdateDto.Id)
            {
                return BadRequest();
            }
            var customer = _context.Customers.FirstOrDefaultAsync(e => e.Id == id);
            if (customer is null) return NotFound();

            var updatedCustomer = _mapper.Map<CustomerUpdateDTO>(customer);
            var customerToUpdate = _mapper.Map<Customer>(updatedCustomer);

            customerToUpdate.PhoneNo = customerUpdateDto.PhoneNo;
            customerToUpdate.Address = customerUpdateDto.Address;

            try
            {
                _context.Attach(customerToUpdate);
                _context.Entry(customerToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            } catch ( DbUpdateConcurrencyException e)
            {
                return Problem("Internal Server Error");
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerCreateDTO customerCreateDto)
        {

            try
            {
                var customer = _mapper.Map<Customer>(customerCreateDto);
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                var dto = _mapper.Map<CustomerReadOnlyDTO>(customer);
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, dto);
                    
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

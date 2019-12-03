using H_Plus_Sports.Contracts;
using H_Plus_Sports.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H_Plus_Sports.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private H_Plus_SportsContext context;
        public CustomerRepository(H_Plus_SportsContext context)
        {
            this.context = context;
        }
        public async Task<Customer> Add(Customer customer)
        {
            await context.Customer.AddAsync(customer);
            await context.SaveChangesAsync();

            return customer;
        }

        public async Task<bool> Exist(int id)
        {
            return await context.Customer.AnyAsync(c => c.CustomerId == id);
        }

        public async Task<Customer> Find(int id)
        {
            return await context.Customer.Include(customer => customer.Order).SingleOrDefaultAsync();
        }

        public IEnumerable<Customer> GetAll()
        {
            return context.Customer;
        }

        public async Task<Customer> Remove(int id)
        {
            var customer = await context.Customer.SingleAsync(a => a.CustomerId == id);
            context.Customer.Remove(customer);
            await context.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> Update(Customer customer)
        {
            context.Customer.Update(customer);
            await context.SaveChangesAsync();

            return customer;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.DataLayer.Repositories;
using Cafe.ViewModels.Customer;

namespace Cafe.DataLayer.Services
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(Cafe_DBEntities db) : base(db){}

        public List<CustomersListViewModel> GetListOfCustomers()
        {
            return this._dbSet.Select(c => new CustomersListViewModel
            {
                CustomerID = c.CustomerID,
                CustomerName = c.CustomerName,
                Address = c.Address
            }).ToList();
        }
    }
}

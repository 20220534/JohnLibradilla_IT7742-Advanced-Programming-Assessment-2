using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace _20220534_Advanced_Programming_Assessment_1
{
    public class CustomerController
    {
      
        private Dictionary<string, Customer> customers = new Dictionary<string, Customer>();
        private Dictionary<string, List<Account>> customerAccounts = new Dictionary<string, List<Account>>();

         
        public Customer AddCustomer(string customerNumber, string name, string contact, bool isStaff)
        {
            if (customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer already exists.");

            Customer customer = new Customer(customerNumber, name, contact, isStaff);
            customers[customerNumber] = customer;
            customerAccounts[customerNumber] = new List<Account>();

            Console.WriteLine($"Customer {name} added successfully.");
            return customer;
        }

         
        public Customer GetCustomer(string customerNumber)
        {
            if (!customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");

            return customers[customerNumber];
        }

         
        public void UpdateCustomer(string customerNumber, string name = null, string contact = null, bool? isStaff = null)
        {
            if (!customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");

            Customer customer = customers[customerNumber];
            if (name != null) customer.name = name;
            if (contact != null) customer.contactDetails = contact;
            if (isStaff.HasValue) customer.isStaff = isStaff.Value;

            Console.WriteLine($"Customer {customerNumber} updated successfully.");
        }

         
        public void RemoveCustomer(string customerNumber)
        {
            if (!customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");

            customers.Remove(customerNumber);
            customerAccounts.Remove(customerNumber);

            Console.WriteLine($"Customer {customerNumber} removed successfully.");
        }

        
        public void AddAccountToCustomer(string customerNumber, Account account)
        {
            if (!customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");

            customerAccounts[customerNumber].Add(account);
            Console.WriteLine($"Account {account.uniqueID} added to customer {customerNumber}.");
        }

        
        public List<Account> GetCustomerAccounts(string customerNumber)
        {
            if (!customerAccounts.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");

            return customerAccounts[customerNumber];
        }

         
        public void DisplayCustomerInfo(string customerNumber)
        {
            if (!customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");

            Customer c = customers[customerNumber];
            Console.WriteLine($"Customer ID: {c.customerNumber}");
            Console.WriteLine($"Name: {c.name}");
            Console.WriteLine($"Contact: {c.contactDetails}");
            Console.WriteLine($"Staff: {c.isStaff}");
            Console.WriteLine("Accounts:");
            if (customerAccounts.ContainsKey(customerNumber))
            {
                foreach (var acc in customerAccounts[customerNumber])
                {
                    Console.WriteLine($" - {acc.uniqueID}: Balance {acc.Balance:C}");
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public class CustomerController
    {
        private Dictionary<string, Customer> customers = new Dictionary<string, Customer>();

        public List<Customer> GetAllCustomers()
        {
            return customers.Values.ToList();
        }


        public string DataPath { get; set; } = "bank_data.xml";

        #region Customer Management

        public Customer AddCustomer(string customerNumber, string name, string contact,
                                    bool isStaff, string addAmount, string accountType)
        {
            if (customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer already exists.");

            Customer customer = new Customer(customerNumber, name, contact, isStaff, addAmount, accountType);

            // Initialize accounts
            customer.EverydayAcc = new EverydayAccount("E-" + customerNumber, 0m);
            customer.InvestmentAcc = new InvestmentAccount("I-" + customerNumber, 0m, 5.0);
            customer.OmniAcc = new OmniAccount("O-" + customerNumber, 0m);

            customer.Accounts.Add(customer.EverydayAcc);
            customer.Accounts.Add(customer.InvestmentAcc);
            customer.Accounts.Add(customer.OmniAcc);

            customers[customerNumber] = customer;

            Save(); // Persist immediately
            return customer;
        }

        public Customer GetCustomer(string customerNumber)
        {
            if (!customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");
            return customers[customerNumber];
        }

        public void UpdateCustomer(string customerNumber, string name = null,
                                   string contact = null, bool? isStaff = null)
        {
            Customer c = GetCustomer(customerNumber);
            if (name != null) c.name = name;
            if (contact != null) c.contactDetails = contact;
            if (isStaff.HasValue) c.isStaff = isStaff.Value;

            Save();
        }

        public void RemoveCustomer(string customerNumber)
        {
            if (!customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");
            customers.Remove(customerNumber);

            Save();
        }

        #endregion

        #region Account Operations

        public void Deposit(string customerNumber, string accountId, decimal amount)
        {
            var acc = GetCustomer(customerNumber).GetAccount(accountId);
            if (amount <= 0) throw new ArgumentException("Deposit must be positive.");
            acc.deposit(amount);

            Save();
        }

        public void Withdraw(string customerNumber, string accountId, decimal amount)
        {
            var acc = GetCustomer(customerNumber).GetAccount(accountId);
            if (amount <= 0) throw new ArgumentException("Withdrawal must be positive.");
            acc.withdraw(amount); // Throws FailedWithdrawalException if it fails

            Save();
        }

        public void Transfer(string customerNumber, string fromAccountId, string toAccountId, decimal amount)
        {
            if (fromAccountId == toAccountId)
                throw new ArgumentException("Cannot transfer within the same account.");

            var customer = GetCustomer(customerNumber);
            var src = customer.GetAccount(fromAccountId);
            var dst = customer.GetAccount(toAccountId);

            try
            {
                src.withdraw(amount);
                dst.deposit(amount);
            }
            catch (FailedWithdrawalException)
            {
                src.deposit(amount); // Rollback
                throw;
            }

            Save();
        }

        public decimal ApplyInterest(string customerNumber, string accountId)
        {
            var acc = GetCustomer(customerNumber).GetAccount(accountId);
            decimal newBalance = acc.Balance;

            switch (acc)
            {
                case InvestmentAccount inv:
                    inv.AddInterest();
                    newBalance = inv.Balance;
                    break;
                case OmniAccount omni:
                    decimal interest = omni.calculateInterest();
                    omni.deposit(interest);
                    newBalance = omni.Balance;
                    break;
                    // EverydayAccount has no interest
            }

            Save();
            return newBalance;
        }

        public void AddAccountToCustomer(string customerNumber, Account account)
        {
            if (!customers.ContainsKey(customerNumber))
                throw new ArgumentException("Customer not found.");

            Customer c = customers[customerNumber];
            c.Accounts.Add(account);

            if (account is EverydayAccount)
                c.EverydayAcc = account;
            else if (account is InvestmentAccount)
                c.InvestmentAcc = account;
            else if (account is OmniAccount)
                c.OmniAcc = account;

            Save();
        }

        #endregion

        #region Persistence (XML)

        public void Save(string path = null)
        {
            if (path == null) path = DataPath;

            var doc = new XDocument(
                new XElement("BankData",
                    new XElement("Customers",
                        customers.Values.Select(c =>
                            new XElement("Customer",
                                new XElement("CustomerNumber", c.customerNumber),
                                new XElement("Name", c.name),
                                new XElement("ContactDetails", c.contactDetails),
                                new XElement("IsStaff", c.isStaff),
                                new XElement("Accounts",
                                    c.Accounts.Select(a =>
                                    {
                                        XElement el = new XElement("Account",
                                            new XAttribute("Type", a.GetType().Name),
                                            new XElement("ID", a.uniqueID),
                                            new XElement("Balance", a.Balance)
                                        );

                                        if (a is InvestmentAccount inv)
                                            el.Add(new XElement("InterestRate", inv.InterestRate));

                                        if (a is OmniAccount omni)
                                        {
                                            el.Add(new XElement("OverdraftAmount", omni.overdraftAmount));
                                            el.Add(new XElement("InterestRate", omni.InterestRate));
                                        }

                                        return el;
                                    })
                                )
                            )
                        )
                    )
                )
            );

            doc.Save(path);
        }

        public void Load(string path = null)
        {
            if (path == null) path = DataPath;
            if (!File.Exists(path)) return;

            customers.Clear();

            var doc = XDocument.Load(path);
            var custNodes = doc.Root?.Element("Customers")?.Elements("Customer") ?? Enumerable.Empty<XElement>();

            foreach (var cNode in custNodes)
            {
                string id = (string)cNode.Element("CustomerNumber");
                string name = (string)cNode.Element("Name");
                string contact = (string)cNode.Element("ContactDetails");
                bool isStaff = bool.Parse(cNode.Element("IsStaff")?.Value ?? "false");

                Customer c = new Customer(id, name, contact, isStaff, "0", "");

                var accounts = cNode.Element("Accounts")?.Elements("Account") ?? Enumerable.Empty<XElement>();
                foreach (var aNode in accounts)
                {
                    string type = (string)aNode.Attribute("Type");
                    string accId = (string)aNode.Element("ID");
                    decimal balance = decimal.Parse(aNode.Element("Balance")?.Value ?? "0");

                    Account acc = null;

                    switch (type)
                    {
                        case "EverydayAccount":
                            acc = new EverydayAccount(accId, balance);
                            c.EverydayAcc = acc;
                            break;

                        case "InvestmentAccount":
                            double interest = 5.0;
                            var interestNode = aNode.Element("InterestRate");
                            if (interestNode != null)
                                double.TryParse(interestNode.Value, out interest);
                            acc = new InvestmentAccount(accId, balance, interest);
                            c.InvestmentAcc = acc;
                            break;

                        case "OmniAccount":
                            decimal overdraft = 100;
                            double rate = 0;
                            var overdraftNode = aNode.Element("OverdraftAmount");
                            if (overdraftNode != null)
                                decimal.TryParse(overdraftNode.Value, out overdraft);
                            var rateNode = aNode.Element("InterestRate");
                            if (rateNode != null)
                                double.TryParse(rateNode.Value, out rate);
                            var omni = new OmniAccount(accId, balance);
                            omni.overdraftAmount = overdraft;
                            omni.InterestRate = rate;
                            acc = omni;
                            c.OmniAcc = acc;
                            break;
                    }

                    if (acc != null)
                        c.Accounts.Add(acc);
                }

                customers[id] = c;
            }
        }

        #endregion
    }
}

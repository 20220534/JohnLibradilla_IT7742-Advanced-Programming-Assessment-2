using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20220534_Advanced_Programming_Assessment_1
{
    // customer handles all customer info relate query
    public class Customer
    {
        public string customerNumber {  get; set; }
        public string name { get; set; }
        public string contactDetails {  get; set; }
        public bool isStaff { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();

        public string addamount { get; set; }
        public string accounttype { get; set; }

        public Account EverydayAcc { get; set; }
        public Account InvestmentAcc { get; set; }
        public Account OmniAcc { get; set; }

        public Customer(string number, string name, string contact, bool isStaff, string addamount, string accounttype)
        {
            customerNumber = number;
            this.name = name;
            contactDetails = contact;
            this.isStaff = isStaff;
            this.addamount = addamount;
            this.accounttype = accounttype;
        }
        public Account GetAccount(string accountId)
        {
            foreach (var acc in Accounts)
            {
                if (acc.uniqueID == accountId)
                    return acc;
            }
            throw new ArgumentException($"Account {accountId} not found for this customer.");
        }
    }
}


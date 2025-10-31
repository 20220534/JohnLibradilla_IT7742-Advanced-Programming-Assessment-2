using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public abstract class Account
    {

        public string uniqueID;
        protected decimal balance;
        protected int FailedTransactions;

        public Account(string uniqueID, decimal balance)
        { this.uniqueID = uniqueID; this.balance = balance; }

        public virtual decimal Balance
        {
            get { return balance; }
        }

        public abstract void withdraw(decimal amount);

        public virtual void deposit(decimal amount)
        {
            balance += amount;
        }
        public decimal failedTransactionfee(bool isStaff)
        {
            const decimal fee = 1.5m;
            const decimal staffDiscount = 0.50m;
            if (isStaff)
            {
                return fee * FailedTransactions * staffDiscount;
            }
            else
            {
                return fee * FailedTransactions;
            }
        }
    }
}

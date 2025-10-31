using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20220534_Advanced_Programming_Assessment_1
{
    
    public class InvestmentAccount : Account
    {
        public double InterestRate;
        public InvestmentAccount(string id, decimal b) :
           base(id, b)
        {
        }

        public void AddInterest() => balance += calculateInterest();

        public decimal calculateInterest() => Balance * (decimal)(InterestRate / 100);


        public override void withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                FailedTransactions++;
                throw new FailedWithdrawalException(uniqueID, amount);
            }
            balance -= amount;
        }
    }
} 
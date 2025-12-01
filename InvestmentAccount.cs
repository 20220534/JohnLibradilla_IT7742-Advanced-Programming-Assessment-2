using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20220534_Advanced_Programming_Assessment_1
{

    public class InvestmentAccount : Account
    {
        public double InterestRate { get; set; }

        public InvestmentAccount(string id, decimal balance, double interestRate)
            : base(id, balance)
        {
            InterestRate = interestRate;
        }

        public void AddInterest()
        {
            balance += CalculateInterest();
        }

        public decimal CalculateInterest()
        {
            return Balance * (decimal)(InterestRate / 100);
        }

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
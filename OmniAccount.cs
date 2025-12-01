using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public class OmniAccount : Account
    {
        public decimal overdraftAmount =100;

        public double InterestRate;


        public OmniAccount(string id, decimal b) :
           base(id, b)

        {
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
        public decimal calculateInterest()
        {

            if (Balance > 1000)
            {
                return Balance * (decimal)(InterestRate / 100);
            }
            else
            {
                return 0m;
            }
        }


    }
}
    

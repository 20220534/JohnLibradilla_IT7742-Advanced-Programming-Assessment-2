using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public class FailedWithdrawalException : Exception
    {
        public string AccountID { get; }
        public decimal AttemptedAmount { get; }

        public FailedWithdrawalException(string accountID, decimal amount)
            : base($"Withdrawal of {amount:C} from account {accountID} failed.")
        {
            AccountID = accountID;
            AttemptedAmount = amount;
        }

        public FailedWithdrawalException(string accountID, decimal amount, string message)
            : base(message)
        {
            AccountID = accountID;
            AttemptedAmount = amount;
        }

        public FailedWithdrawalException(string accountID, decimal amount, string message, Exception inner)
            : base(message, inner)
        {
            AccountID = accountID;
            AttemptedAmount = amount;
        }
    }
}
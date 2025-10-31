using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public class EverydayAccount : Account
    {
        public EverydayAccount(string id, decimal b) :
              base(id, b)

        {

        }
        public override void withdraw(decimal amount)
        {
            if (amount < 0) throw new NotImplementedException("Cannot withdraw a negative amount.");
            if (amount > Balance) throw new InvalidOperationException("Insufficiend funds.");
            balance -= amount;
        }
    }
}


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
       

        public Customer(string number, string name, string contact, bool isStaff)
        {
            customerNumber = number;
            this.name = name;
            contactDetails = contact;
            this.isStaff = isStaff;
        }
    }
}


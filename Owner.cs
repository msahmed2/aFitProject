using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Owner
    {
        public Owner( string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public string firstName { get; set; }  

        public string lastName { get; set; }

        public override string ToString()
        {
            string name = firstName + " " + lastName;
            return name;
        }


    }
}

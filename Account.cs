using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Account
    {
        public Account(int accountId, Owner owner, int accountType )
        {
            this.accountId = accountId;
            this.owner = owner;
            this.accountType = accountType;
        }

        public enum AccountType
        {
            Checking,
            CorporateInvestment,
            IndividualInvestment
        }

        public Owner owner { get; set; }

        public int accountId { get; set; }

        public float amount { get; set; }

        public int accountType { get; set; }

        public string getAccountTypeToString(int accountType)
        {

            string accountTypeString = "";

            if (accountType == (int)Account.AccountType.Checking)
            {
                accountTypeString = Account.AccountType.Checking.ToString();
            }
            else if (accountType == (int)Account.AccountType.CorporateInvestment)
            {
                accountTypeString = Account.AccountType.CorporateInvestment.ToString();
            }
            else
            {
                accountTypeString = Account.AccountType.IndividualInvestment.ToString();
            }

            return accountTypeString;
        }

        public string displayAccountDetails()
        {
            string details = "AccountDetails:\n" +
            "Account Id: " + accountId + "\n" +
            "Account Holder: " + owner.ToString() + "\n" +
            "Account Type: " + getAccountTypeToString(accountType) + "\n" +
            "Amount: $" + amount;

            return details;
        }

    }
}

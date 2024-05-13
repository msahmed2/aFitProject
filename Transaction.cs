using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Transaction
    {

        public Transaction(int transactionType, float amount)
        {
            this.transactionType = transactionType;
            this.amount = amount;
        }

        public int transactionType;

        public float amount;

        public enum TransactionTypes
        {
            Deposit, 
            Withdraw,
            Transfer
        }

        public bool CompleteTransaction(Account account1, Account account2)
        {
            if(account2 != null)
            {
                return transfer(account1, account2);
            }
            else
            {
                if (transactionType == (int)TransactionTypes.Deposit)
                {
                    return deposit(account1);
                }
                else
                {
                    return withdraw(account1);
                }
            }
        }

        public bool deposit(Account account)
        {
            account.amount += amount;
            return true;
        }

        public bool withdraw(Account account)
        {
            if(account.accountType == (int)Account.AccountType.IndividualInvestment) {
                if(amount > 1000)
                {
                    return false;
                }
                return true;
            }
            else if (account.amount > amount)
            {
                account.amount -= amount;
                return true;
            }
            else
            {
                return false;
            }

            
        }

        public bool transfer(Account account1, Account account2)
        {
            if (withdraw(account1))
            {
                deposit(account2);
                return true;
            }
            else
            {
                return false;
            }
                
            
        }



    }
}

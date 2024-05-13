using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Institution
    {
        public Institution(string name)
        {
            this.name = name;
            this.accounts = new List<Account>();
        }
        public string name { get; set; }

        public List<Account> accounts { get; set; }



        public Account searchByAccountId(int accountId)
        {
            foreach (Account account in accounts)
            {
                if (account.accountId == accountId)
                {
                    return account;
                }
            }

            return null;
        }

        public Account getAccount(int accountId)
        {
            return searchByAccountId(accountId);
        }

        public bool validateAccountType(int accountType)
        {
            if(accountType == (int)Account.AccountType.Checking
                    || accountType == (int)Account.AccountType.CorporateInvestment
                        || accountType == (int)Account.AccountType.IndividualInvestment)
            {
                return true;
            }

            return false;
        }

        public List<Account> getListOfAccountsByNameAndAccountType( string firstname, string lastname, int accountType )
        {
            List<Account> accountsOwnedByOwner = new List<Account>();
            foreach (Account account in accounts)
            {
                if(account.owner.firstName.Equals(firstname) && account.owner.lastName.Equals(lastname) && account.accountType == accountType)
                {
                    accountsOwnedByOwner.Add(account);
                }
            }

            return accountsOwnedByOwner;
        }

        public Account createAccount(Owner owner, int accountType )
        {
            bool accountFound = true;
            var accountId = 0;
            
            while (accountFound == true)
            {
                Random random = new Random();
                accountId = random.Next(100000, 1000000);

                if (searchByAccountId(accountId) == null)
                {
                    accountFound = false;
                }
            }



            Account newAccount = new Account(accountId, owner, accountType);

            accounts.Add(newAccount);

            return newAccount;
            
            
        }

        public Account validateTransfer(Account account1, string accountId2)
        {
            bool validAccountId = validateAccountId(accountId2);
            if (!validAccountId)
            {
                Console.WriteLine("Invalid Input");
                return null;
            }
            else if (account1.accountId.ToString().Equals(accountId2))
            {
                Console.WriteLine("Account To and Account From cannot be the same account");
                return null;
            }
            else
            {
                int accountIdTwo = Int32.Parse(accountId2);

                Account account2 = getAccount(accountIdTwo);

                if (account2 == null)
                {
                    Console.WriteLine("The Account does not exist");
                    return null;
                }
                else
                {
                    return account2;
                }
            }

        }


        //handler methods
        //************************************************************************************************************************************
        public string CapitalizeFirstLetter(string input)
        {
            char[] nameCorrection = input.ToCharArray();

            for( int i = 0; i < nameCorrection.Length; i++)
            {
                nameCorrection[i] = Char.ToLower(nameCorrection[i]);    
            }

            nameCorrection[0] = Char.ToUpper(nameCorrection[0]);

            string name = new string(nameCorrection);

            return name;

        }

        public string nameHandler(string nameType)
        {
            Console.WriteLine("Please enter your " + nameType + ":");
            string input = Console.ReadLine();
            while (!validateOnlyLetters(input))
            {
                Console.WriteLine("No numbers or special characaters allowed.");
                Console.WriteLine("Please enter your first name:");
                input = Console.ReadLine();
            }

            string name = CapitalizeFirstLetter(input);

            return name;

        }

        public bool validateOnlyLetters(string name)
        {
            if (name.Equals(""))
            {
                return false;
            }
            else
            {
                foreach (char c in name)
                {
                    if (!Char.IsLetter(c))
                    {
                        return false;
                    }
                }

                return true;
            }

        }

        public bool validateAmount(string amount)
        {
            if (amount.Equals(""))
            {
                return false;
            }
            else
            {
                bool decimalPointFound = false;
                int countAfterDecimal = 0;
                foreach (char c in amount)
                {
                    if (!decimalPointFound)
                    {
                        if (!Char.IsDigit(c) && c != '.')
                        {
                            return false;
                        }
                        else if (c == '.')
                        {
                            decimalPointFound = true;
                        }
                    }
                    else
                    {
                        if (c == '.')
                        {
                            return false;
                        }
                        else if (!Char.IsDigit(c))
                        {
                            return false;
                        }
                        else
                        {
                            if (countAfterDecimal == 2)
                            {
                                return false;
                            }
                            else
                            {
                                countAfterDecimal++;
                            }
                        }
                    }

                }

                return true;
            }

        }
        public bool validateAccountId(string accountId)
        {
            foreach(Account account in accounts)
            {
                if(account.accountId.ToString().Equals(accountId))
                {
                    return true;
                }
            }

            return false;
            
        }

        public int checkInput(string question, string options)
        {
            var input = checkValidOption(question, options);

            int validInput = Int32.Parse(input);

            return validInput;
        }

        public string checkValidOption(string question, string options)
        {
            bool isInputValid = false;
            var input = "";
            while (!isInputValid)
            {
                Console.WriteLine(question);
                Console.WriteLine(options);
                input = Console.ReadLine();
                if (input != null)
                {
                    if (input.Equals("1") || input.Equals("2") || input.Equals("3"))
                    {
                        isInputValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter valid input");
                    }
                }
                else
                {
                    Console.WriteLine("Input cannot be null. Please enter valid input");
                }
            }
            return input;
        }

        public void wasSuccessful(bool success, Account account, Account account2, string transaction)
        {
            if (success)
            {
                Console.WriteLine(transaction + " was successful!\n" +
                    "Below are your new account details:");
                Console.WriteLine(account.displayAccountDetails());
            }
            if (success && account2 != null)
            {
                Console.WriteLine(account2.displayAccountDetails());
            }

            goodBye();
        }

        public void goodBye()
        {
            Console.WriteLine("Have a nice day!");
        }

    }

}

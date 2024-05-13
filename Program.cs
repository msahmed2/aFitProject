// See https://aka.ms/new-console-template for more information
using Bank;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Xml.Linq;

Account createAccount(Institution bank)
{
    string firstName = bank.nameHandler("first name");

    string lastName = bank.nameHandler("last name");

    Owner accountOwner = new Owner(firstName, lastName);

    string question = "What kind of account would you like to create " + firstName + " " + lastName;

    string options = "1 for Checking\n2 for Corporate Investment\n3 for Individual Investment";

    int accountType = bank.checkInput(question, options) - 1;
    while(!bank.validateAccountType(accountType))
    {
        Console.WriteLine("Please enter a valid account type.");
        accountType = bank.checkInput(question, options) - 1;
    }
    Account account = bank.createAccount(accountOwner, accountType);

    Console.WriteLine("Congratulations!");
    Console.WriteLine("You have created an account with " + bank.name);
    Console.WriteLine(account.displayAccountDetails());

    return account;
}

bool deposit(Account account, Institution bank)
{
    Console.WriteLine("How much would you like to deposit?");
    var input = Console.ReadLine();

    while (!bank.validateAmount(input))
    {
        Console.WriteLine("Please enter Amount in correct format.");
        input = Console.ReadLine();
    }
    float amount = float.Parse(input);
    Transaction transaction = new Transaction((int)Transaction.TransactionTypes.Deposit, amount);
    bool success = transaction.CompleteTransaction(account, null);

    return success;
}

bool withdraw(Account account, Institution bank)
{
    Console.WriteLine("How much would you like to withdraw?");
    var input = Console.ReadLine();

    while (!bank.validateAmount(input))
    {
        Console.WriteLine("Please enter Amount in correct format.");
        input = Console.ReadLine();
    }
    float amount = float.Parse(input);
    Transaction transaction = new Transaction((int)Transaction.TransactionTypes.Withdraw, amount);
    bool success = transaction.CompleteTransaction(account, null);

    return success;
}

bool transfer(Account account1, Account account2, Institution bank)
{
    Console.WriteLine("How much would you like to transer?");
    var input = Console.ReadLine();

    while (!bank.validateAmount(input))
    {
        Console.WriteLine("Please enter Amount in correct format.");
        input = Console.ReadLine();
    }
    float amount = float.Parse(input);
    Transaction transaction = new Transaction((int)Transaction.TransactionTypes.Transfer, amount);
    bool success = transaction.CompleteTransaction(account1, account2);

    return success;
}




Institution bank = new Institution("Universal Credit Union");



Console.WriteLine("Welcome to the " + bank.name);
bank.createAccount(new Owner("Person", "One"), (int)Account.AccountType.IndividualInvestment);
bank.createAccount(new Owner("Person", "Two"), (int)Account.AccountType.CorporateInvestment);
bank.createAccount(new Owner("Person", "Three"), (int)Account.AccountType.Checking);
bank.createAccount(new Owner("Person", "Four"), (int)Account.AccountType.CorporateInvestment);
bank.createAccount(new Owner("Person", "Five"), (int)Account.AccountType.Checking);

foreach( Account account in bank.accounts)
{
    Transaction transaction = new Transaction((int)Transaction.TransactionTypes.Deposit, 2000);
    transaction.CompleteTransaction(account, null);
    Console.WriteLine(account.displayAccountDetails());
}

Console.WriteLine("The above information has been displayed for testing purposes.");
Console.WriteLine("For a more permanent usage, I could implement a database to store account data");

int validInput = bank.checkInput("Do you already have an account?", "1 for Yes\n2 for No\n3 to exit");

if ( validInput == 1 )
{
    Console.WriteLine("What is your account number?");
    var input = Console.ReadLine();
    int count = 0;
    while(!bank.validateAccountId(input))
    {
        count++;
        if(count > 3)
        {
            Console.WriteLine("Too many attempts. You have been locked out.");
            break;
        }
        Console.WriteLine("Account Id is not valid, please try again:");
        Console.WriteLine("You have " + (3 - count) + " tries left before you are locked out.");
        Console.WriteLine("What is your account number?");
        input = Console.ReadLine();
    }

    if(count > 3)
    {
        bank.goodBye();
        return;
    }

    int accountId = Int32.Parse(input);

    Account account = bank.getAccount(accountId);

    validInput = bank.checkInput("What kind of transaction would you like to make?", "1 for Deposit\n2 for Withdrawal\n3 for Transfer");

    if(validInput == 1)
    {
        bool success = deposit(account, bank);
        bank.wasSuccessful(success, account, null, Transaction.TransactionTypes.Deposit.ToString());

    }
    else if(validInput == 2)
    {
        bool success = withdraw(account, bank);
        if(!success && account.accountType == (int)Account.AccountType.IndividualInvestment)
        {
            Console.WriteLine("Unfortunately you cannot withdraw more than $1000 from an Individual Investment Account");
        }
        else if(!success)
        {
            Console.WriteLine("Unfortunately you have insufficient funds");
        }
        bank.wasSuccessful(success, account, null, Transaction.TransactionTypes.Withdraw.ToString());
    }
    else
    {
        Console.WriteLine("Where would you like to transfer the funds to?");
        input = Console.ReadLine();
        Account account2 = bank.validateTransfer(account, input);
        count = 0;
        while (account2 == null)
        {
            count++;
            if(count > 3)
            {
                Console.WriteLine("Too many attempts. You have been locked out.");
                break;
            }
            Console.WriteLine("Account Id is not valid, please try again:");
            Console.WriteLine("You have " + (3 - count) + " tries left before you are locked out.");
            Console.WriteLine("What is the account Id that you want to transfer funds to?");
            input = Console.ReadLine();
            account2 = bank.validateTransfer(account, input);
        }

        if (count > 3)
        {
            bank.goodBye();
            return;
        }

        bool success = transfer(account, account2, bank);
        if (!success && account.accountType == (int)Account.AccountType.IndividualInvestment)
        {
            Console.WriteLine("Transaction Failed. You cannot move more than $1000 from an Individual Investment account at a time.");
        }
        else if (!success)
        {
            Console.WriteLine("Transaction Failed. Insufficient Funds.");
        }
        bank.wasSuccessful(success, account, account2, Transaction.TransactionTypes.Transfer.ToString());

    }
}

else if( validInput == 2 )
{

    validInput = bank.checkInput("Would you like to create an account?", "1 for Yes\n2 for No\n3 to exit");

    if( validInput == 1 )
    {
        Account account = createAccount(bank);

        validInput = bank.checkInput("Now that the account has been created, would you like to deposit anyting?", "1 for Yes\n2 for No\n3 to exit");

        if(validInput == 1)
        {
            bool success = deposit(account, bank);
            bank.wasSuccessful(success, account, null, Transaction.TransactionTypes.Deposit.ToString());
        }

    }
    else
    {
        bank.goodBye();
    }
}
else
{
    bank.goodBye();
}

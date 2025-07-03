using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Principal;

namespace MiniBankProject
{
    internal class Program
    {
        // Constants
        const double MinimumBalance = 100.0;
        const string AccountsFilePath = "accounts.txt";
        const string ReviewsFilePath = "reviews.txt";
        const string RequestsFilePath = "requests.txt";
        const string TransactionsFilePath = "transactions.txt";
        const string LoanRequestsFilePath = "loan_requests.txt";
        const string ActiveLoansFilePath = "active_loans.txt";
        const string FeedbackFilePath = "feedback.txt";
        const string AppointmentsFilePath = "appointments.txt";

        // Exchange Rates
        const double USD_RATE = 3.8;
        const double EUR_RATE = 4.1;

        // Global lists (parallel)
        static List<int> accountNumbers = new List<int>();
        static List<string> accountNames = new List<string>();
        static List<string> nationalIds = new List<string>();
        static List<double> balances = new List<double>();
        static List<string> requestStatuse = new List<string>();
        static List<string> userTypes = new List<string>();
        static List<string> hashedPasswords = new List<string>();
        static List<string> isAccountLocked = new List<string>();
        static List<string> phoneNumbers = new List<string>();
        static List<string> addresses = new List<string>();
        // Loan 
        static Queue<string> loanRequests = new Queue<string>(); // Each = "NationalID:LoanAmount:InterestRate"
        static List<string> activeLoanIds = new List<string>();  // Track users with loans


        // Queues and Stacks
        static Queue<string> createAccountRequests = new Queue<string>();
        static Stack<string> reviewsStack = new Stack<string>();
        // Feedback ratings

        static List<int> FeedbackRatings = new List<int>();
        // appointment 
        static Queue<string> appointmentQueue = new Queue<string>();
        // Account number generator
        static int lastAccountNumber;


        // Main method
        static void Main(string[] args)
        {

            LoadAccountsInformationFromFile();
            LoadReviews();
            LoadRequsts();
            LoadLoanData();
            LoadFeedbackRatings();
            LoadAppointments();
            bool runAgain = true;
            while (runAgain)
            {
                try //handle the exception if the user enter invalid input
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to Mini Bank System!");
                    Console.WriteLine("1. create New account");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("0. Exit");

                    Console.WriteLine("Select Option ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            CreateNewAccount();
                            break;
                        case "2":
                            Login();
                            break;
                        case "0":
                            SaveAccountsInformationToFile();
                            //SaveRequsts();
                            Console.Write("\nDo you want to create a backup before exiting? (y/n): ");
                            string response = Console.ReadLine()?.Trim().ToLower();
                            if (response == "y")
                            {
                                BackupData();
                            }
                            Console.WriteLine("Exiting system...");
                            Console.WriteLine("Thank you for using Mini Bank System!");
                            Console.WriteLine("Press any key to exit.");
                            Console.ReadKey();
                            runAgain = false;
                            break;

                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);


                    Console.WriteLine("Invalid Choice! Try again.");
                    Console.WriteLine("Press any key  "); 
                    Console.ReadLine(); 
                }

            }
        }
        // End User Menu
        static void EndUserMenu()
        {
            bool runUser = true;
            while (runUser) 
            {
                try
                {


                    Console.Clear();
                    Console.WriteLine("End User Menu:");

                    Console.WriteLine("1. Deposit Money");
                    Console.WriteLine("2. Withdraw Money");
                    Console.WriteLine("3. Check Balance");
                    Console.WriteLine("4. submit a Review");
                    Console.WriteLine("5. View account Details");
                    Console.WriteLine("6. Transfer Between Accounts");
                    Console.WriteLine("7. Generate Monthly Statement");
                    Console.WriteLine("8. Display Transactions");
                    Console.WriteLine("9. Update Account Info");
                    Console.WriteLine("10. Request a Loan");
                    Console.WriteLine("11.Show Last ( N ) Transactions");
                    Console.WriteLine("12. Show Transactions After Date");
                    Console.WriteLine("13. Show all Transactions");
                    Console.WriteLine("14. Book an Appointment with Bank Manager");
                    Console.WriteLine("0. Exit to Main Menu");

                    string userChoice = Console.ReadLine();
                    switch (userChoice)
                    {

                        case "1":
                            DepositMoney();
                            break;
                        case "2":
                            WithdrawMoney();
                            break;
                        case "3":
                            CheckBalance();
                            break;
                        case "4":
                            SubmitReview();
                            break;
                        //case "5":
                        //    viewAccountDetails();
                        //    break;
                        case "6":
                            TransferBetweenAccounts();
                            break;
                        case "7":
                            GenerateMonthlyStatement();
                            break;
                        case "8":
                            DisplayTransactions();
                            break;
                        case "9":
                            UpdateAccountInfo();
                            break;
                        case "10":
                            RequestLoan();
                            break;
                        case "11":
                            ShowLastNTransactions();
                            break;
                        case "12":
                            ShowTransactionsAfterDate();
                            break;
                        case "13":
                            PrintAllTransactionsOfUser();
                            break;
                        case "14":
                            BookAppointment();
                            break;
                        case "0":
                            runUser = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);


                    Console.WriteLine("Invalid Choice! Try again.");
                    Console.WriteLine("Press any key  "); 
                    Console.ReadLine(); 
                }

            }
        }
        // Admin Menu//
        static void AdminMenu()
        {
            bool runAdmin = true;
            while (runAdmin)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Admin Menu:");
                    Console.WriteLine("1. View Account Requests");
                    Console.WriteLine("2. Approve Account Request");
                    Console.WriteLine("3. View Reviews");
                    Console.WriteLine("4. View All Account Requests");
                    Console.WriteLine("5. Delete Account");
                    Console.WriteLine("6. Search Account");
                    Console.WriteLine("7. Show Total Bank Balance");
                    Console.WriteLine("8. Show Top 3 Richest Customers");
                    Console.WriteLine("9. Unlock Locked Account");
                    Console.WriteLine("10. Process Loan Requests");
                    Console.WriteLine("11. View Average Feedback Score");
                    Console.WriteLine("12. Print All Transactions Of User");
                    Console.WriteLine("0. Exit to Main Menu");
                    string adminChoice = Console.ReadLine();
                    switch (adminChoice)
                    {
                        case "1":
                            ViewAccountRequests();
                            break;
                        case "2":
                            ApproveAccountRequest();
                            break;
                        case "3":
                            ViewReviews();
                            break;
                        case "4":
                            ViewAllAccountRequests();
                            break;
                        case "5":
                            DeleteAccount();
                            break;
                        case "6":
                            SearchAccount();
                            break;
                        case "7":
                            ShowTotalBankBalance();
                            break;
                        case "8":
                            ShowTop3RichestCustomers();
                            break;
                        case "9":
                            UnlockLockedAccount();
                            break;
                        case "10":
                            ProcessLoanRequests();
                            break;
                        case "11":
                            ShowAverageFeedback();
                            break;
                        case "12":
                            PrintAllTransactionsOfUser();
                            break;
                        case "0":
                            runAdmin = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }

                }
                catch (Exception e)//show exception message if the user enter invalid input
                {
                    Console.WriteLine(e.Message);


                    Console.WriteLine("Invalid Choice! Try again.");
                    Console.WriteLine("Press any key  "); //ask user to press any key to continue
                    Console.ReadLine(); //read the user inputConsole.ReadLine();
                }
            }

        }

        // Request Account Creation
        static void CreateNewAccount()
        {
            Console.Clear();
            Console.WriteLine("Create New Account");
            Console.WriteLine("-------------------------");

            bool isValidName = false;
            bool isValidNationalId = false;
            bool isValidInitialBalance = false;
            bool isValidUserType = false;
            bool isAccountLocked1 = false;
            string userName = "";
            string nationalId = "";
            double initialBalance = 0;
            string userType = "";
            string phone = "";
            string address = "";
            string requestStatuse1= "panding"; // Default status

            try
            {

                // Validate Name
                while (!isValidName)
                {
                    Console.Write("Enter your name: ");
                    userName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        Console.WriteLine("Name cannot be empty.");
                        isValidName = false;
                    }
                    else if (userName.Length < 3)
                    {
                        Console.WriteLine("Name must be at least 3 characters long.");
                        isValidName = false;
                    }
                    else if (int.TryParse(userName, out int result))
                    {
                        Console.WriteLine("Name cannot be a number.");
                        isValidName = false;
                    }
                    else
                        isValidName = true;
                }

                // Validate National ID
                while (!isValidNationalId)
                {
                    Console.Write("Enter your National ID: ");
                    nationalId = Console.ReadLine();

                    // git national id from the file
                    string[] lines = File.ReadAllLines(AccountsFilePath);
                    foreach (string line in lines)
                    {
                        string fileNationalId = "";
                        string[] parts = line.Split(':');
                        if (parts.Length >= 3)
                        {
                            //nationalIds.Add(parts[2]);
                            fileNationalId = parts[2];

                        }
                        if (fileNationalId == nationalId)
                        {

                            Console.WriteLine("This National ID already exists in the system. Cannot create duplicate account.");
                            isValidNationalId = false;
                            break;
                        }
                        else if (string.IsNullOrWhiteSpace(nationalId))
                        {
                            Console.WriteLine("National ID cannot be empty.");
                            isValidNationalId = false;
                        }
                        else if (!double.TryParse(nationalId, out double result))
                        {
                            Console.WriteLine("National ID must be numbers only.");
                            isValidNationalId = false;
                        }
                        else
                            isValidNationalId = true;
                    }

                }
                Console.Write("Set your password: ");
                string password = ReadPassword();
                // Hash the password
                string hashedPassword = HashPassword(password);

                // Phone
                while (true)
                {
                    Console.Write("Phone number (8‑12 digits): ");
                    phone = Console.ReadLine()?.Trim();
                    if (!long.TryParse(phone, out _) || phone.Length < 8 || phone.Length > 12)
                        Console.WriteLine("Enter a valid phone number.");
                    else break;
                }

                // Address
                while (true)
                {
                    Console.Write("Address: ");
                    address = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(address))
                        Console.WriteLine("Address cannot be empty.");
                    else break;
                }

                while (!isValidUserType)
                {
                    Console.Write("Enter your user type (user/admin): ");
                    userType = Console.ReadLine();

                    if (userType != "user" && userType != "admin")
                        Console.WriteLine("User type must be either 'user' or 'admin'.");
                    else
                    {
                        isValidUserType = true;
                        if (userType == "admin")
                        {
                            lastAccountNumber = GetTheLastAccountNumberFromAccountFile();
                            //string status = "panding"; // Default status

                            // Save the admin information to the file
                            
                            accountNames.Add(userName);
                            nationalIds.Add(nationalId);
                            balances.Add(initialBalance);
                            requestStatuse.Add(requestStatuse1);
                            userTypes.Add(userType);
                            hashedPasswords.Add(hashedPassword);
                            isAccountLocked.Add("false");
                            phoneNumbers.Add(phone);
                            addresses.Add(address);


                            //lastAccountNumber = GetTheLastAccountNumberFromAccountFile();
                            int newAccountNumber = lastAccountNumber + 1;


                            string adminAccountLine = newAccountNumber + ":" + userName + ":" + nationalId + ":" + "0.0" + ":" + requestStatuse1 + ":" + "admin" + ":" + hashedPassword + ":" + isAccountLocked1 + ":" + phone + ":" + address;
                            createAccountRequests.Enqueue(adminAccountLine);
                            Console.WriteLine("Admin account created successfully!");
                            Console.WriteLine($"Your new account number is: {lastAccountNumber + 1}");
                            File.AppendAllText("accounts.txt", adminAccountLine + Environment.NewLine);

                            Console.WriteLine("Press any key to return to the menu...");
                            Console.ReadKey();

                        }
                        if (userType == "user")
                        {


                            // Validate Initial Balance
                            while (!isValidInitialBalance)
                            {
                                Console.Write("Enter your initial balance: ");
                                string balanceInput = Console.ReadLine();

                                if (!double.TryParse(balanceInput, out initialBalance))
                                    Console.WriteLine("Please enter a valid number for balance.");
                                else if (initialBalance < MinimumBalance)
                                    Console.WriteLine($"Initial balance must be at least {MinimumBalance} OMR.");
                                else
                                    isValidInitialBalance = true;
                            }

                            // Get last account number
                            lastAccountNumber = GetTheLastAccountNumberFromAccountFile();
                            int newAccountNumber = lastAccountNumber + 1;

                            // string status = "panding"; // Default status

                            // Add to lists 

                            accountNames.Add(userName);
                            nationalIds.Add(nationalId);
                            balances.Add(initialBalance);
                            requestStatuse.Add(requestStatuse1);
                            userTypes.Add(userType);
                            hashedPasswords.Add(hashedPassword);
                            isAccountLocked.Add("false");


                            string request = newAccountNumber + ":" + userName + ":" + nationalId + ":" + initialBalance + ":" + requestStatuse1 + ":" + userType + ":" + hashedPassword + ":" + isAccountLocked1 + ":" + phone + ":" + address;
                            createAccountRequests.Enqueue(request);
                            Console.WriteLine("\nAccount created successfully!");
                            File.AppendAllText("accounts.txt", request + Environment.NewLine);
                            Console.WriteLine($"Your new account number is: {newAccountNumber}");


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }



        


        // Login
        static void Login()
        {
            Console.Clear();
            Console.WriteLine("Login");

            Console.Write("Enter your National ID if you user -- 'admin' As Admin: ");
            string inputId = Console.ReadLine();

            Console.Write("Enter your password: ");
            string inputPassword = ReadPassword();
            string hashedInput = HashPassword(inputPassword);

            // Check fixed admin first
            const string AdminID = "admin";
            const string AdminPasswordHash = "vLFfghR5tNV3K9DKhmwArV+SbjWAcgZZzIDTnJ0JgCo="; // Example SHA256 hash of "111111"

            if (inputId.Equals(AdminID, StringComparison.OrdinalIgnoreCase) && hashedInput == AdminPasswordHash) 
            {
                Console.WriteLine("\nAdmin login successful.");
                AdminMenu();
                return;
            }

            if (!File.Exists(AccountsFilePath))
            {
                Console.WriteLine("Accounts file not found.");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines(AccountsFilePath);
            bool accountFound = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(':');
                if (parts.Length < 8) continue;

                string fileNationalId = parts[2];
                string accountStatus = parts[4];
                string userType = parts[5];
                string storedHash = parts[6];
                bool isLocked = bool.Parse(parts[7]);

                if (fileNationalId == inputId)
                {
                    accountFound = true;

                    if (isLocked)
                    {
                        Console.WriteLine("\nYour account is locked. Please contact admin.");
                        Console.ReadKey();
                        return;
                    }

                    if (accountStatus != "Approved")
                    {
                        Console.WriteLine("\nYour account is not approved yet.");
                        Console.ReadKey();
                        return;
                    }

                    int loginAttempts = 0;
                    while (loginAttempts < 3)
                    {
                        if (hashedInput == storedHash)
                        {
                            Console.WriteLine("\nLogin successful.");
                            if (userType == "admin")
                                AdminMenu();
                            else
                                EndUserMenu();
                            return;
                        }
                        else
                        {
                            loginAttempts++;
                            if (loginAttempts < 3)
                            {
                                Console.Write("Incorrect password. Try again: ");
                                inputPassword = ReadPassword();
                                hashedInput = HashPassword(inputPassword);
                            }
                        }
                    }

                    // Lock the account after 3 failed attempts
                    Console.WriteLine("\nToo many failed attempts. Your account is now locked.");
                    parts[7] = "true";
                    lines[i] = string.Join(":", parts);
                    File.WriteAllLines(AccountsFilePath, lines);
                    Console.ReadKey();
                    return;
                }
            }

            if (!accountFound)
            {
                Console.WriteLine("\nNational ID not found.");
                Console.ReadKey();
            }
        }

        //------------------//
        // End User UseCases
        //------------------//



        //1. Deposit Money
        static void DepositMoney()
        {
            Console.Clear();
            Console.WriteLine("Deposit");

            Console.Write("Enter your Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accNum))
            {
                Console.WriteLine("Invalid account number.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Select Currency: ");
            Console.WriteLine("1. OMR (Local)");
            Console.WriteLine("2. USD");
            Console.WriteLine("3. EUR");
            Console.Write("Enter choice: ");
            string currencyChoice = Console.ReadLine();

            Console.Write("Enter deposit amount: ");
            if (!double.TryParse(Console.ReadLine(), out double originalAmount) || originalAmount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                Console.ReadKey();
                return;
            }

            string currency = "OMR";
            double convertedAmount = originalAmount;

            switch (currencyChoice)
            {
                case "2":
                    currency = "USD";
                    convertedAmount = originalAmount * USD_RATE;
                    break;
                case "3":
                    currency = "EUR";
                    convertedAmount = originalAmount * EUR_RATE;
                    break;
            }

            // Load accounts
            string[] lines = File.ReadAllLines("accounts.txt");
            int index = Array.FindIndex(lines, line => line.Split(':')[0] == accNum.ToString());

            if (index == -1)
            {
                Console.WriteLine("Account not found.");
                Console.ReadKey();
                return;
            }

            string[] parts = lines[index].Split(':');
            double currentBalance = double.Parse(parts[3]);
            currentBalance += convertedAmount;
            parts[3] = currentBalance.ToString("F2");
            lines[index] = string.Join(":", parts);
            File.WriteAllLines("accounts.txt", lines);

            // Log transaction
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string log = $"{accNum}|Deposit ({currency}) {originalAmount}|Converted {convertedAmount:F2}|Balance {currentBalance:F2}|{timestamp}";
            File.AppendAllText("transactions.txt", log + Environment.NewLine);

            Console.WriteLine($"{originalAmount} {currency} deposited (converted to {convertedAmount:F2} OMR).");
            Console.WriteLine($"New Balance: {currentBalance:F2} OMR");
            UserFeedback();
            Console.ReadKey();
            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }

        //2. Withdraw Money
        static void WithdrawMoney()
        {
            Console.Clear();
            Console.WriteLine("Withdraw Money");

            bool isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    if (!int.TryParse(Console.ReadLine(), out int enteredAccountNumber))
                    {
                        Console.WriteLine("Invalid account number. Please enter numbers only.");
                        continue;
                    }

                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }

                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];
                            string userType = parts[5];
                            string hashedPassword = parts[6];
                            string isAccountLocked = parts[7];
                            string phone = parts[8];
                            string address = parts[9];


                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;

                                if (status != "Approved")
                                {
                                    Console.WriteLine("Account is not approved yet. Cannot withdraw money.");
                                    break;
                                }

                                Console.Write("Enter amount to withdraw: ");
                                if (!double.TryParse(Console.ReadLine(), out double amount))
                                {
                                    Console.WriteLine("Invalid amount. Please enter numbers only.");
                                    break;
                                }

                                if (amount <= 0)
                                {
                                    Console.WriteLine("Amount must be greater than zero.");
                                    break;
                                }

                                if (balance - amount < MinimumBalance)
                                {
                                    Console.WriteLine($"Cannot withdraw {amount}. Minimum balance of {MinimumBalance} must be maintained.");
                                    break;
                                }

                                double newBalance = balance - amount;
                                parts[3] = newBalance.ToString();

                                // Manual way to rebuild the line
                                lines[i] = parts[0] + ":" + parts[1] + ":" + parts[2] + ":" + parts[3] + ":" + parts[4] + ":" + parts[5] + ":" + parts[6] + ":" + parts[7] + ":" + parts[8] + ":" + parts[9];

                                File.WriteAllLines(AccountsFilePath, lines);
                                LogTransaction(fileAccountNumber, "Withdraw", amount, newBalance);

                                Console.WriteLine($"Withdrew {amount} successfully!");
                                Console.WriteLine($"New Balance: {newBalance}");
                                isSuccess = true;
                                UserFeedback();
                                break;
                            }
                        }
                    }

                    if (!accountFound)
                    {
                        Console.WriteLine("Account number not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }
        //3. Check Balance
        static void CheckBalance()
        {
            Console.Clear();
            Console.WriteLine("-- Check Balance --");

            bool isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    if (!int.TryParse(Console.ReadLine(), out int enteredAccountNumber))
                    {
                        Console.WriteLine("Invalid account number. Please enter numbers only.");
                        continue;
                    }

                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }

                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];

                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;

                                if (status != "Approved")
                                {
                                    Console.WriteLine("Account is not approved yet. Cannot check balance.");
                                    break;
                                }

                                Console.WriteLine($"Account Number: {fileAccountNumber}");
                                Console.WriteLine($"Account Name: {name}");
                                Console.WriteLine($"Balance: {balance:F2}"); // Formatted to 2 decimal places
                                isSuccess = true;
                                break;
                            }
                        }
                    }

                    if (!accountFound)
                    {
                        Console.WriteLine("Account number not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }
        //4. Submit a Review
        static void SubmitReview()
        {
            Console.Clear();
            Console.WriteLine("Submit a Review:");
          
            Console.Write("Enter your Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accNum))
            {
                Console.WriteLine("Invalid account number.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists("accounts.txt"))
            {
                Console.WriteLine("Accounts file not found.");
                Console.ReadKey();
                return;
            }

            string[] accounts = File.ReadAllLines("accounts.txt");
            bool accountExists = accounts.Any(line => line.Split(':')[0] == accNum.ToString());

            if (!accountExists)
            {
                Console.WriteLine("Account not found.");
                Console.ReadKey();
                return;
            }

           
            Console.Write("Write your review: ");
            string comment = Console.ReadLine();

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string reviewEntry = $"{accNum}|{comment}|{timestamp}";

            File.AppendAllText("reviews.txt", reviewEntry + Environment.NewLine);

            Console.WriteLine("\nThank you! Your review has been submitted.");
            Console.ReadKey();
        }


        //5. Transfer Between Accounts
        static void TransferBetweenAccounts()
        {
            Console.Clear();
            Console.WriteLine("Transfer Between Accounts");
            bool isSuccess = false;
            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    int enteredAccountNumber = int.Parse(Console.ReadLine());
                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }
                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];
                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;
                                if (status != "Approved")
                                {
                                    Console.WriteLine("Account is not approved yet. Cannot transfer money.");
                                    break;
                                }
                                Console.Write("Enter the recipients account number: ");
                                int recipientAccountNumber = int.Parse(Console.ReadLine());
                                // Check if recipient account exists
                                bool recipientFound = false;
                                for (int j = 0; j < lines.Count; j++)
                                {
                                    string[] recipientParts = lines[j].Split(':');
                                    if (recipientParts.Length >= 5)
                                    {
                                        int fileRecipientAccountNumber = int.Parse(recipientParts[0]);
                                        if (fileRecipientAccountNumber == recipientAccountNumber)
                                        {
                                            recipientFound = true;
                                            break;
                                        }
                                    }
                                }
                                if (!recipientFound)
                                {
                                    Console.WriteLine("Recipient account not found.");
                                    break;
                                }
                                Console.Write("Enter amount to transfer: ");
                                double amount = double.Parse(Console.ReadLine());
                                if (amount <= 0)
                                {
                                    Console.WriteLine("Amount must be greater than zero.");
                                    break;
                                }
                                if (balance - amount < MinimumBalance)
                                {
                                    Console.WriteLine($"Cannot transfer {amount}. Minimum balance of {MinimumBalance} must be maintained.");
                                    break;
                                }
                                // Update balances
                                double newBalanceSender = balance - amount;
                                double newBalanceRecipient = 0;
                                // Update sender balance
                                parts[3] = newBalanceSender.ToString();
                                lines[i] = string.Join(":", parts);
                                // Update recipient balance
                                for (int j = 0; j < lines.Count; j++)
                                {
                                    string[] recipientParts = lines[j].Split(':');
                                    if (recipientParts.Length >= 5)
                                    {
                                        int fileRecipientAccountNumber = int.Parse(recipientParts[0]);
                                        if (fileRecipientAccountNumber == recipientAccountNumber)
                                        {
                                            newBalanceRecipient = double.Parse(recipientParts[3]) + amount;
                                            recipientParts[3] = newBalanceRecipient.ToString();
                                            lines[j] = string.Join(":", recipientParts);
                                            break;
                                        }
                                    }
                                }
                                File.WriteAllLines(AccountsFilePath, lines);
                                // Store transaction logs
                                LogTransaction(enteredAccountNumber, "Transfer Sent", amount, newBalanceSender);
                                LogTransaction(recipientAccountNumber, "Transfer Received", amount, newBalanceRecipient);

                                Console.WriteLine($"Transferred {amount} successfully!");
                                Console.WriteLine($"New Balance: {newBalanceSender}");
                                Console.WriteLine($"Recipient New Balance: {newBalanceRecipient}");
                                isSuccess = true;
                                UserFeedback();
                                break;
                            }
                        }

                    }
                    if (!accountFound)
                    {
                        Console.WriteLine("Account number not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                Console.WriteLine("Press any key to return to the End User Menu.");
                Console.ReadKey();
            }

        }
        // 7. Generate Monthly Statement
        static void GenerateMonthlyStatement()
        {
            Console.Clear();
            Console.WriteLine("Monthly Statement Generator");

            Console.Write("Enter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accNum))
            {
                Console.WriteLine("Invalid account number.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter month (1-12): ");
            if (!int.TryParse(Console.ReadLine(), out int month) || month < 1 || month > 12)
            {
                Console.WriteLine("Invalid month.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter year (e.g. 2025): ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Invalid year.");
                Console.ReadKey();
                return;
            }

            string transactionFile = "transactions.txt";
            if (!File.Exists(transactionFile))
            {
                Console.WriteLine("Transaction file not found.");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines(transactionFile);
            List<string> statement = new List<string>();

            foreach (string line in lines)
            {

                string[] parts = line.Split('|');
                if (parts.Length != 5) continue;

                if (!int.TryParse(parts[0], out int logAcc)) continue;
                if (!DateTime.TryParse(parts[4], out DateTime logDate)) continue;

                if (logAcc == accNum && logDate.Month == month && logDate.Year == year)
                {
                    statement.Add(line);
                }
            }

            if (statement.Count == 0)
            {
                Console.WriteLine("No transactions found for this account in that month.");
            }
            else
            {
                string filename = $"Statement_Acc{accNum}_{year}-{month:D2}.txt";
                File.WriteAllLines(filename, statement);
                Console.WriteLine($"\nStatement saved to: {filename}");
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }


        // 8. Display Transactions
        static void DisplayTransactions()
        {
            Console.Clear();
            Console.WriteLine("-- View Transactions --");

            Console.Write("Enter your account number: ");
            
            if (!int.TryParse(Console.ReadLine(), out int accNum))
            {
                Console.WriteLine("Invalid account number.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists("transactions.txt"))
            {
                Console.WriteLine("No transactions found.");
                Console.ReadKey();
                return;
            }

            string[] logs = File.ReadAllLines("transactions.txt");
            bool any = false;

            Console.WriteLine($"\nTransactions for Account: {accNum}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Type           | Amount   | Balance | Date & Time");
            Console.WriteLine("----------------------------------------");

            foreach (string log in logs)
            {
                string[] parts = log.Split('|');
                if (parts.Length != 5) continue;

                if (int.TryParse(parts[0], out int logAccNum) && logAccNum == accNum)
                {
                    string type = parts[1];
                    string amount = parts[2];
                    string balance = parts[3];
                    string timestamp = parts[4];

                    Console.WriteLine($"{type,-14} {amount,8}   {balance,7}   {timestamp}");
                    any = true;
                }
            }

            if (!any)
            {
                Console.WriteLine("No transactions found for this account.");
            }

            Console.WriteLine("\nPress any key to return to the menu.");
            Console.ReadKey();
        }


        // insert transaction log
        static void LogTransaction(int accountNumber, string type, double amount, double balanceAfter)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string log = $"{accountNumber}|{type}|{amount}|{balanceAfter}|{timestamp}";

            File.AppendAllText(TransactionsFilePath, log + Environment.NewLine);


        }


        //9. Update Account Info
        static void UpdateAccountInfo()
        {
            Console.Clear();
            Console.WriteLine("Update Account Info");

            Console.Write("Enter your National ID: ");
            string inputNationalId = Console.ReadLine();

            string[] lines = File.ReadAllLines(AccountsFilePath);
            bool found = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(':');
                if (parts.Length >= 10 && parts[2] == inputNationalId)
                {
                    found = true;

                    Console.WriteLine($"\nCurrent Phone   : {parts[8]}");
                    Console.WriteLine($"Current Address : {parts[9]}");

                    Console.Write("Enter new phone number (or press Enter to keep current): ");
                    string newPhone = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newPhone))
                        parts[8] = newPhone;

                    Console.Write("Enter new address (or press Enter to keep current): ");
                    string newAddress = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newAddress))
                        parts[9] = newAddress;

                    // Rebuild and update line
                    lines[i] = string.Join(":", parts);

                    // Write all lines back to file
                    File.WriteAllLines(AccountsFilePath, lines);

                    Console.WriteLine("\nAccount info updated successfully.");
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    
                    return;
                }
            }

            if (!found)
            {
                Console.WriteLine("Account not found.");
                Console.ReadKey();
            }
            
        }

        //10. Request a Loan

        static void RequestLoan()
        {
            Console.Clear();
            Console.WriteLine("Request a Loan");

            Console.Write("Enter your National ID: ");
            string inputNationalId = Console.ReadLine();

            if (!File.Exists("accounts.txt"))
            {
                Console.WriteLine("Accounts file not found.");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines("accounts.txt");
            bool found = false;

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');

                if (parts.Length >= 10 && parts[2] == inputNationalId)
                {
                    found = true;

                    // Check balance
                    if (!double.TryParse(parts[3], out double balance))
                    {
                        Console.WriteLine("Error reading balance.");
                        Console.ReadKey();
                        return;
                    }

                    if (balance < 5000)
                    {
                        Console.WriteLine("You must have at least 5000 OMR to request a loan.");
                        Console.ReadKey();
                        return;
                    }

                    // Check if already has active loan
                    if (File.Exists("active_loans.txt"))
                    {
                        string[] activeLoans = File.ReadAllLines("active_loans.txt");
                        if (activeLoans.Contains(inputNationalId))
                        {
                            Console.WriteLine("You already have an active loan.");
                            Console.ReadKey();
                            return;
                        }
                    }

                    // Enter loan amount
                    Console.Write("Enter loan amount: ");
                    if (!double.TryParse(Console.ReadLine(), out double loanAmount) || loanAmount <= 0)
                    {
                        Console.WriteLine("Invalid amount.");
                        Console.ReadKey();
                        return;
                    }

                    // Enter interest rate
                    Console.Write("Enter interest rate (%): ");
                    if (!double.TryParse(Console.ReadLine(), out double interestRate) || interestRate < 0)
                    {
                        Console.WriteLine("Invalid interest rate.");
                        Console.ReadKey();
                        return;
                    }

                    // Enqueue and save
                    string loanLine = $"{inputNationalId}:{loanAmount}:{interestRate}";
                    loanRequests.Enqueue(loanLine);
                    SaveLoanRequestsToFile();

                    Console.WriteLine("Loan request submitted. Waiting admin approval.");
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }
            }

            if (!found)
            {
                Console.WriteLine("Account not found.");
                Console.ReadKey();
            }
        }
        //11. Show Last N Transactions
        static void ShowLastNTransactions()
        {
            Console.Clear();
            Console.Write("Enter your National ID: ");
            string nationalId = Console.ReadLine();

            if (!File.Exists(AccountsFilePath) || !File.Exists("transactions.txt"))
            {
                Console.WriteLine("Required files not found.");
                Console.ReadKey();
                return;
            }

            string[] accountLines = File.ReadAllLines(AccountsFilePath);
            int? accountNumber = null;

            foreach (var line in accountLines)
            {
                var parts = line.Split(':');
                if (parts.Length >= 3 && parts[2] == nationalId)
                {
                    if (int.TryParse(parts[0], out int accNum))
                    {
                        accountNumber = accNum;
                        break;
                    }
                }
            }

            if (accountNumber == null)
            {
                Console.WriteLine("No account found for this National ID.");
                Console.ReadKey();
                return;
            }

            Console.Write("How many recent transactions to display? ");
            if (!int.TryParse(Console.ReadLine(), out int n))
            {
                Console.WriteLine("Invalid number.");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines("transactions.txt");
            var userTx = lines
                .Where(line =>
                {
                    var parts = line.Split('|');
                    return parts.Length == 5 && int.TryParse(parts[0], out int id) && id == accountNumber;
                })
                .Reverse()
                .Take(n)
                .Reverse();

            if (!userTx.Any())
            {
                Console.WriteLine("No transactions found for this National ID.");
            }
            else
            {
                Console.WriteLine($"\nLast {n} transactions for National ID {nationalId}:\n");
                foreach (var line in userTx)
                    Console.WriteLine(line);
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        //12. Show Transactions After Date
        static void ShowTransactionsAfterDate()
        {
            Console.Clear();
            Console.Write("Enter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accNum))
            {
                Console.WriteLine("Invalid account number.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter starting date (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fromDate))
            {
                Console.WriteLine("Invalid date.");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines("transactions.txt");
            var filtered = lines
                .Select(line => line.Split('|'))
                .Where(parts => parts.Length == 5 && int.TryParse(parts[0], out int id) && id == accNum)
                .Where(parts => DateTime.TryParse(parts[4], out DateTime date) && date >= fromDate);

            Console.WriteLine($"\nTransactions since {fromDate:yyyy-MM-dd} for Account {accNum}:\n");
            foreach (var parts in filtered)
                Console.WriteLine(string.Join("|", parts));

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        //13. Book Appointment
        static void BookAppointment()
        {
            Console.Clear();
            Console.WriteLine("Book Bank Appointment");

            Console.Write("Enter your Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accNum))
            {
                Console.WriteLine("Invalid account number.");
                Console.ReadKey();
                return;
            }

            // Check if already has an appointment
            bool alreadyBooked = appointmentQueue.Any(entry =>
            {
                string[] parts = entry.Split('|');
                return parts.Length >= 1 && parts[0] == accNum.ToString();
            });

            if (alreadyBooked)
            {
                Console.WriteLine("You already have an appointment booked.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter service type (Loan Discussion, Consultation, etc.): ");
            string service = Console.ReadLine();

            Console.Write("Enter date & time (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime appointmentDate))
            {
                Console.WriteLine("Invalid date/time.");
                Console.ReadKey();
                return;
            }

            string entry = $"{accNum}|{service}|{appointmentDate:yyyy-MM-dd HH:mm}";
            appointmentQueue.Enqueue(entry);
            SaveAppointments();

            Console.WriteLine("Appointment booked successfully.");
            Console.ReadKey();
        }






        //-------------------//
        // Admin UseCases // 
        //-------------------//


        //1. Approve Account Request
        static void ApproveAccountRequest()
        {
            Console.Clear();
            Console.WriteLine("Request:");
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No requests available.");

            }
            else
            {


                string request = createAccountRequests.Peek();

                string[] splitlineOfRequest = request.Split(":");
                string account = splitlineOfRequest[0];
                string userName = splitlineOfRequest[1];
                string nationalId = splitlineOfRequest[2];
                string initialBalance = splitlineOfRequest[3];
                string inialRequestStatus = splitlineOfRequest[4];
                string userType = splitlineOfRequest[5]; 
                string hashPassword = splitlineOfRequest[6];



                //Console.WriteLine("view account Request");
                Console.WriteLine("account Number: " + account);
                Console.WriteLine("user name : " + userName);
                Console.WriteLine("national Id : " + nationalId);
                Console.WriteLine("initial balance : " + initialBalance);
                //Console.WriteLine("request status : " + inialRequestStatus);
                Console.WriteLine("user Type : " + userType);
                Console.WriteLine("Do you want to approve this request? (y/n)");
                string requestStatus = Console.ReadLine();
                request = createAccountRequests.Dequeue(); // remove from memory
                if (requestStatus.ToLower() == "y")
                {



                    

                    // Add the account to the system
                    string initialRequestStatus1 = "Approved";
                    int accountNumber = GitTheLastAccountNumberFromAccountFile();
                    int newAccountNumber = accountNumber + 1;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    accountNumbers.Add(newAccountNumber);
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add(initialRequestStatus1);
                    userTypes.Add(userType);
                    hashedPasswords.Add(hashPassword);

                    Console.WriteLine($"Account for {userName} has been created with account number {account}.{initialRequestStatus1}");

                    string requestToRemove = request; // remove from memory
                    string first = GitTheFirstRequestInFileAndDelete().ToString(); // remove from file

                }
                else
                {

                    //// Add the account to the system
                    string initialRequestStatus2 = "Not Approved";
                    int accountNumber = GitTheLastAccountNumberFromAccountFile() + 1;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    accountNumbers.Add(accountNumber);
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add(initialRequestStatus2);
                    userTypes.Add(userType);
                    hashedPasswords.Add(hashPassword);
                    Console.WriteLine("Account Request NotApproved");
                    string requestToRemove = request; // remove from memory

                    string first = GitTheFirstRequestInFileAndDelete().ToString(); // remove from file


                    Console.WriteLine($"Account for {userName} has been rejected.");



                }

                // Save the update request status

                // Load account file
                var lines = File.ReadAllLines("accounts.txt").ToList();

                // Find the index of this account (by National ID)
                int index = lines.FindIndex(line =>
                {
                    var parts = line.Split(':');
                    return parts.Length >= 3 && parts[2] == nationalId;
                });

                if (index != -1)
                {
                    var parts = lines[index].Split(':');
                    if (parts.Length >= 5)
                    {
                        parts[4] = (requestStatus.ToLower() == "y") ? "Approved" : "Not Approved"; // update status
                        lines[index] = string.Join(":", parts);
                        File.WriteAllLines("accounts.txt", lines); // save updated file
                    }
                }

                Console.WriteLine("Press any key to return to the admin menu.");
                Console.ReadKey();
            }
        }
         //2. view account requests
         static void ViewAccountRequests()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Request : ");
                if (createAccountRequests.Count == 0)
                {
                    Console.WriteLine("No requests available.");

                }
                else
                {

               
                    string request = createAccountRequests.Peek();

                    string[] splitlineOfRequest = request.Split(":");
                    string account = splitlineOfRequest[0];
                    string userName = splitlineOfRequest[1];
                    string nationalId = splitlineOfRequest[2];
                    string initialBalance = splitlineOfRequest[3];
                    string inialRequestStatus = splitlineOfRequest[4];
                    string userType = splitlineOfRequest[5];
                    string hashPassword = splitlineOfRequest[6];


                    //Console.WriteLine("view account Request");
                    Console.WriteLine("account number : " + account);
                    Console.WriteLine("user name : " + userName);
                    Console.WriteLine("national Id : " + nationalId);
                    Console.WriteLine("initial balance : " + initialBalance);
                    Console.WriteLine("request status : " + inialRequestStatus);
                    Console.WriteLine("user type : " + userType);
                    //Console.WriteLine("password : " + hashPassword);

                }

                Console.WriteLine("Press any key to return to the admin menu.");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //3. view all account requests
        static void ViewAllAccountRequests()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("\n--- All Accounts in File ---");
                // vie all account requests from the file
                Console.WriteLine("--------------------------------------------------");

           


                using (StreamReader reader = new StreamReader(AccountsFilePath, true))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content);
                 

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {


                        string[] lines = File.ReadAllLines(AccountsFilePath);

                        string[] strings = line.Split(':');
                        int accountNum = int.Parse(strings[0]);
                        string accountName = strings[1];
                        string nationalId = strings[2];
                        double balance = double.Parse(strings[3]);
                        string requestStatus = strings[4];
                        string userType = strings[5];
                        string inialRequestStatus = strings[6];
                        Console.WriteLine("--------------------------------------------------");
                        Console.WriteLine("Account Number: " + accountNum);
                        Console.WriteLine("Account Name: " + accountName);
                        Console.WriteLine("National ID: " + nationalId);
                        Console.WriteLine("Account Balance: " + balance);
                        Console.WriteLine("Account Status: " + requestStatus);
                        Console.WriteLine("User Type: " + userType);
                        Console.WriteLine("Request Status: " + inialRequestStatus);
                        Console.WriteLine("--------------------------------------------------");


                        if (accountNum > lastAccountNumber)
                            lastAccountNumber = accountNum;

                        string valu = reader.ReadToEnd();
                        Console.WriteLine(valu);
                    }

                    // value from queue
                   Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("Account Requests in Queue:");
                    foreach (string request in createAccountRequests)
                    {
                        Console.WriteLine(request);
                    }

                }
                Console.WriteLine("Press any key to return to the admin menu.");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //4. View Reviews   
        static void ViewReviews()
        {
            Console.Clear();
            Console.WriteLine("Reviews:");
            if (reviewsStack.Count == 0)
            {
                Console.WriteLine("No reviews available.");
            }
            else
            {
                foreach (string review in reviewsStack)
                {
                    Console.WriteLine(review);
                    
                }
            }
            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
        }

        // 5.Delete Account
        static void DeleteAccount()
        {
            Console.Clear();
            Console.WriteLine("-- Delete Account --");
            bool isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    int enteredAccountNumber = int.Parse(Console.ReadLine());

                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }

                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];

                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;

                                if (status == "Approved")
                                {
                                    //Console.WriteLine("Account is approved.");
                                    Console.Write("Are you sure you want to delete this account? (y/n): ");
                                    string confirmation = Console.ReadLine();
                                    if (confirmation.ToLower() == "y")
                                    {
                                        lines.RemoveAt(i);
                                        File.WriteAllLines(AccountsFilePath, lines);
                                        Console.WriteLine("Account deleted successfully!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Account deletion canceled.");
                                    }

                                    isSuccess = true;
                                    break;
                                }
                                else if (status == "Not Approved")
                                {
                                    Console.WriteLine("Account is not approved yet.");
                                    break;
                                }
                            }
                        }
                    }

                    if (!accountFound)
                    {
                        Console.WriteLine("Account is not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }

        //6. Search Account

        static void SearchAccount()
        {
            Console.Clear();
            Console.WriteLine("-- Search Account --");
            try
            {
                Console.Write("Enter your account number: ");
                int enteredAccountNumber = int.Parse(Console.ReadLine());
                bool accountFound = false;
                if (!File.Exists(AccountsFilePath))
                {
                    Console.WriteLine("Accounts file not found.");
                    return;
                }
                List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                foreach (string line in lines)
                {
                   
                    string[] parts = line.Split(':');
                    if (parts.Length >=5)
                    {
                        int fileAccountNumber = int.Parse(parts[0]);
                        string name = parts[1];
                        string nationalId = parts[2];
                        double balance = double.Parse(parts[3]);
                        string status = parts[4];
                        if (fileAccountNumber == enteredAccountNumber)
                        {
                            accountFound = true;
                            Console.WriteLine("Account Number: " + fileAccountNumber);
                            Console.WriteLine("Account Name: " + name);
                            Console.WriteLine("National ID: " + nationalId);
                            Console.WriteLine("Account Balance: " + balance);
                            Console.WriteLine("Account Status: " + status);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        
        }
        //7. Show Total Bank Balance
        static void ShowTotalBankBalance()
        {
            Console.Clear();
            Console.WriteLine("-- Total Bank Balance --");
            double totalBalance = 0;
            if (!File.Exists(AccountsFilePath))
            {
                Console.WriteLine("Accounts file not found.");
                return;
            }
            List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length >= 5)
                {
                    double balance = double.Parse(parts[3]);
                    totalBalance += balance;
                }
            }
            Console.WriteLine("Total Bank Balance: " + totalBalance);

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }
      
        //8. Show Top 3 Richest Customers
        static void ShowTop3RichestCustomers()
        {
            Console.Clear();
            Console.WriteLine("-- Top 3 Richest Customers --");

            if (!File.Exists(AccountsFilePath))
            {
                Console.WriteLine("Accounts file not found.");
                return;
            }

            List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();

            List<string> accountNumbers = new List<string>();
            List<double> balances = new List<double>();

            // Loop through each line and extract account numbers and balances
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length >= 5)
                {
                    string accountNumber = parts[0];
                    if (double.TryParse(parts[3], out double balance))
                    {
                        accountNumbers.Add(accountNumber);
                        balances.Add(balance);
                    }
                }
            }

            // Find top 3 richest 
            for (int i = 0; i < 3; i++)
            {
                double maxBalance = double.MinValue; // Initialize to the smallest value
                int maxIndex = -1;

                for (int j = 0; j < balances.Count; j++)
                {
                    if (balances[j] > maxBalance)
                    {
                        maxBalance = balances[j];
                        maxIndex = j;
                    }
                }

                if (maxIndex != -1)
                {
                    string accountNumber = accountNumbers[maxIndex];
                    string accountName = GetAccountName(accountNumber);
                    Console.WriteLine($"Account Number: {accountNumber}, Account Name: {accountName}, Balance: {balances[maxIndex]} OMR ");

                    // Remove the richest customer from the list to display the next richest
                    balances.RemoveAt(maxIndex);
                    accountNumbers.RemoveAt(maxIndex);
                }
            }


            // Top 5 Richest Accounts using LINQ
            //Console.Clear();
            //Console.WriteLine("Top 5 Richest Accounts");

            //if (!File.Exists(AccountsFilePath))
            //{
            //    Console.WriteLine("Accounts file not found.");
            //    Console.ReadKey();
            //    return;
            //}

            //var top5Accounts = File.ReadAllLines(AccountsFilePath)
            //    .OrderByDescending(p => double.Parse(p.Split(':')[3]))
            //    .Take(5);

            //Console.WriteLine("{0,-10} {1,-20} {2,15}", "Acc#", "Name", "Balance");
            //foreach (var acc in top5Accounts)
            //{
            //    var parts = acc.Split(':');
            //    Console.WriteLine("{0,-10} {1,-20} {2,15} OMR", parts[0], parts[1], parts[3]);
            //}


            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }

        //9. Unlock Account
        static void UnlockLockedAccount()
        {
            Console.Clear();
            Console.WriteLine("Unlock Locked Account");

            Console.Write("Enter the National ID to unlock: ");
            string inputNationalId = Console.ReadLine();

            if (!File.Exists(AccountsFilePath))
            {
                Console.WriteLine("Accounts file not found.");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines(AccountsFilePath);
            bool found = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(':');
                if (parts.Length >= 8)
                {
                    string nationalId = parts[2];
                    string isLocked = parts[7];

                    if (nationalId == inputNationalId)
                    {
                        found = true;

                        if (isLocked.ToLower() == "true")
                        {
                            parts[7] = "false";
                            lines[i] = string.Join(":", parts);
                            File.WriteAllLines(AccountsFilePath, lines);

                            Console.WriteLine($"\nAccount with National ID {nationalId} has been unlocked.");
                        }
                        else
                        {
                            Console.WriteLine($"\nAccount with National ID {nationalId} is not locked.");
                        }

                        break;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("\nNo account found with that National ID.");
            }

            Console.WriteLine("\nPress any key to return to the admin menu.");
            Console.ReadKey();
        }

        //10. admin process loan requests
        static void ProcessLoanRequests()
        {
            Console.Clear();
            Console.WriteLine("Process Loan Requests");

            if (!File.Exists(LoanRequestsFilePath))
            {
                Console.WriteLine("No loan requests found.");
                Console.ReadKey();
                return;
            }

            List<string> requests = File.ReadAllLines(LoanRequestsFilePath).ToList();
            if (requests.Count == 0)
            {
                Console.WriteLine("No pending loan requests.");
                Console.ReadKey();
                return;
            }

            List<string> accountLines = File.ReadAllLines(AccountsFilePath).ToList();
            List<string> activeLoans = File.Exists(ActiveLoansFilePath)
                ? File.ReadAllLines(ActiveLoansFilePath).ToList()
                : new List<string>();

            List<string> updatedAccounts = new List<string>(accountLines);
            List<string> updatedRequests = new List<string>();

            foreach (string request in requests)
            {
                string[] parts = request.Split(':');
                if (parts.Length != 3) continue;

                string nationalId = parts[0];
                if (!double.TryParse(parts[1], out double loanAmount) || !double.TryParse(parts[2], out double interestRate))
                {
                    Console.WriteLine($"Invalid request: {request}");
                    continue;
                }

                int accIndex = updatedAccounts.FindIndex(line =>
                {
                    string[] accParts = line.Split(':');
                    return accParts.Length >= 10 && accParts[2] == nationalId;
                });

                if (accIndex == -1)
                {
                    Console.WriteLine($"Account not found for ID {nationalId}");
                    continue;
                }

                string[] acc = updatedAccounts[accIndex].Split(':');
                double currentBalance = double.Parse(acc[3]);

                Console.WriteLine($"\nLoan Request: {nationalId}");
                Console.WriteLine($"Amount     : {loanAmount} OMR");
                Console.WriteLine($"Interest   : {interestRate}%");
                Console.WriteLine($"Balance    : {currentBalance} OMR");

                Console.Write("Approve this loan? (y/n): ");
                string decision = Console.ReadLine()?.ToLower();

                if (decision == "y")
                {
                    acc[3] = (currentBalance + loanAmount).ToString("F2");
                    updatedAccounts[accIndex] = string.Join(":", acc);

                    if (!activeLoans.Contains(nationalId))
                        activeLoans.Add(nationalId);

                    Console.WriteLine($"Approved. New balance: {acc[3]} OMR");
                }
                else
                {
                    Console.WriteLine("Rejected.");
                }
            }

            // Save updated files
            File.WriteAllLines(AccountsFilePath, updatedAccounts);
            File.WriteAllLines(ActiveLoansFilePath, activeLoans);
            File.WriteAllText(LoanRequestsFilePath, string.Empty); // Clear all requests after processing
            Console.WriteLine("\nAll requests processed and saved.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        //11. show average feedback
        static void ShowAverageFeedback()
        {
            if (FeedbackRatings.Count == 0)
            {
                Console.WriteLine("No feedback ratings available.");
                return;
            }

            double average = FeedbackRatings.Average();

            Console.WriteLine($"Average Feedback Score: {average:F2} out of 5");
            Console.ReadKey();
        }



        // Common Methods

        static void PrintAllTransactionsOfUser()
        {

            Console.Clear();
            Console.WriteLine("View All Transactions");

            Console.Write("Enter Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accNum))
            {
                Console.WriteLine("Invalid account number.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists("transactions.txt"))
            {
                Console.WriteLine("Transaction file not found.");
                Console.ReadKey();
                return;
            }

            string[] allTx = File.ReadAllLines("transactions.txt");
            var userTx = allTx
                .Select(line => line.Split('|'))
                .Where(parts => parts.Length == 5 && int.TryParse(parts[0], out int a) && a == accNum)
                .Select(parts => new
                {
                    Type = parts[1],
                    Amount = parts[2],
                    Balance = parts[3],
                    Date = parts[4]
                })
                .ToList();

            if (userTx.Count == 0)
            {
                Console.WriteLine("No transactions found for this account.");
            }
            else
            {
                Console.WriteLine($"\nAll Transactions for Account #{accNum}:\n");
                Console.WriteLine("{0,-20} {1,-15} {2,10} {3,15}", "Date", "Type", "Amount", "Balance After");

                foreach (var tx in userTx)
                {
                    Console.WriteLine("{0,-20} {1,-15} {2,10} {3,15}",
                        tx.Date, tx.Type, tx.Amount, tx.Balance);
                }
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }




        //------------------//
        //Save and Load Methods
        //------------------//


        // save accounts information to file
        static void SaveAccountsInformationToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath, true))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {

                        string dataLine = accountNumbers[i] + ":" + accountNames[i] + ":" + nationalIds[i] + ":" + balances[i] + ":" + requestStatuse[i]+ ":"+ userTypes[i]+":"+ hashedPasswords[i]+":" + isAccountLocked[i] + ":" +
                  phoneNumbers[i] + ":" + addresses[i];
                        writer.WriteLine(dataLine);
                    }
                }
                Console.WriteLine("Accounts saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }
        }
      
        // load accounts information from file
        static void LoadAccountsInformationFromFile()
        {
            try
            {

                if (!File.Exists(AccountsFilePath))
                {
                    Console.WriteLine("No saved data found.");
                    return;
                }


                using (StreamReader reader = new StreamReader(AccountsFilePath, true))
                {
                    string content = reader.ReadToEnd();
                   

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {


                        string[] lines = File.ReadAllLines(AccountsFilePath);

                        int accountNum = int.Parse(lines[0]);
                        accountNumbers.Add(accountNum);
                        accountNames.Add(lines[1]);
                        nationalIds.Add(lines[2]);
                        balances.Add(Convert.ToDouble(lines[3]));
                        requestStatuse.Add(lines[4]);
                        userTypes.Add(lines[5]);
                        hashedPasswords.Add(lines[6]);
                        isAccountLocked.Add(lines[7]);
                        phoneNumbers.Add(lines[8]);
                        addresses.Add(lines[9]);

                        if (accountNum > lastAccountNumber)
                            lastAccountNumber = accountNum;

                        string valu = reader.ReadToEnd();
                        Console.WriteLine(valu);
                    }
                }

                Console.WriteLine("Accounts loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }
        }
        // load reviews from file
        static void LoadReviews()
        {
            try
            {
                if (!File.Exists(ReviewsFilePath))
                {
                    Console.WriteLine("No saved reviews found.");
                    return;
                }
                using (StreamReader reader = new StreamReader(ReviewsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        reviewsStack.Push(line);
                    }
                }
                Console.WriteLine("Reviews loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }
        }

        // save requests to file
        static void SaveRequsts()
        {
            try
            {


                if (!File.Exists(RequestsFilePath))
                {
                    Console.WriteLine("No saved requests found.");

                }
                if (createAccountRequests.Count == 0)
                {
                    Console.WriteLine("No requests to save.");

                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(RequestsFilePath))
                    {
                        //foreach (string request in createAccountRequests)
                        //{
                        //    writer.WriteLine(request);
                        //}
                        string request = createAccountRequests.Peek();
                        for (int i = 0; i < createAccountRequests.Count; i++)
                        {
                            
                            string[] splitlineOfRequest = request.Split(":");
                            string accountNum = splitlineOfRequest[0];
                            string userName = splitlineOfRequest[1];
                            string initialBalance = splitlineOfRequest[2];
                            string inialRequestStatus = splitlineOfRequest[3];
                            string userType = splitlineOfRequest[4];
                            string hashPassword = splitlineOfRequest[5];


                            string requestInOneLine = accountNum + ":" + userName + ":" + initialBalance + ":" + inialRequestStatus+":"+ userType + ":" + hashPassword;
                            writer.WriteLine(requestInOneLine);
                        }
                        request = createAccountRequests.Dequeue();
                    }
                    Console.WriteLine("Requests saved successfully.");
                }
            }
            catch
            {
                Console.WriteLine("Error saving request file.");
            }

        }
        // load requests from file
        static void LoadRequsts()
        {
            try
            {
                if (!File.Exists(RequestsFilePath))
                {
                    Console.WriteLine("No saved requests found.");
                    return;
                }
                using (StreamReader reader = new StreamReader(RequestsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        createAccountRequests.Enqueue(line);

                    }
                }
                Console.WriteLine("Requests loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading Request file.");
            }

        }

        // save loan requests to file
        static void SaveLoanRequestsToFile()
        {
           File.WriteAllLines(LoanRequestsFilePath, loanRequests.ToArray());
        }
        // load loan requests / active from file
        static void LoadLoanData()
        {
            if (File.Exists(ActiveLoansFilePath))
                activeLoanIds = File.ReadAllLines(ActiveLoansFilePath).ToList();

            if (File.Exists(LoanRequestsFilePath))
                loanRequests = new Queue<string>(File.ReadAllLines(LoanRequestsFilePath));
        }

        // Load Feedback Ratings
        static void LoadFeedbackRatings()
        {
            if (File.Exists(FeedbackFilePath))
            {
                string[] lines = File.ReadAllLines(FeedbackFilePath);
                foreach (string line in lines)
                {
                    if (int.TryParse(line, out int rating) && rating >= 1 && rating <= 5)
                    {
                        FeedbackRatings.Add(rating);
                    }
                }
            }
        }
        // save appointment
        static void SaveAppointments()
        {
            File.WriteAllLines(AppointmentsFilePath, appointmentQueue);
        }
        // Load appointment
        static void LoadAppointments()
        {
            if (File.Exists(AppointmentsFilePath))
            {
                var lines = File.ReadAllLines(AppointmentsFilePath);
                foreach (var line in lines)
                    appointmentQueue.Enqueue(line);
            }
        }


        // Additional Methods


        // get the last account number from the file
        static int GitTheLastAccountNumberFromAccountFile()
        {
            int lastAccNumber = 0;
            string lastLine = null;

            if (File.Exists(AccountsFilePath))
            {
                string[] lines = File.ReadAllLines(AccountsFilePath);

                // Find last non-empty line
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        lastLine = lines[i];
                        break;
                    }
                }

                if (lastLine != null)
                {
                    string[] parts = lastLine.Split(':');

                    if (parts.Length > 0 && int.TryParse(parts[0], out lastAccNumber))
                    {
                        return lastAccNumber;
                    }

                }
                else
                {
                    // if File is empty
                    return lastAccNumber = 0;
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }

            return 0;
        }

        // get the first request in the file and remove it from the file
        static string GitTheFirstRequestInFileAndDelete()
        {
            
                if (File.Exists(RequestsFilePath))
                {
                List<string> lines = File.ReadAllLines(RequestsFilePath).ToList(); //read all lines from file then store in list .

                if (lines.Count > 0) // check if the file is not empty
                {
                        string firstRequest = lines[0]; // get the first line

                        lines.RemoveAt(0); // remove the first line

                        File.WriteAllLines(RequestsFilePath, lines); // overwrite the file without first line

                        return firstRequest;
                    }
                    else
                    {
                        return "File is empty"; // file is empty
                    }
                }
                else
                {
                   
                    return "Requests file does not exist."; // file does not exist


            }

               


            
        }

        // get the last account number from the file
        static int GetTheLastAccountNumberFromAccountFile()
        {
            if (!File.Exists(AccountsFilePath))
                return 0;

            string lastLine = File.ReadLines(AccountsFilePath).LastOrDefault();

            if (string.IsNullOrEmpty(lastLine))
                return 0;

            string[] parts = lastLine.Split(':');

            if (parts.Length > 0 && int.TryParse(parts[0], out int lastAcc))
                return lastAcc;

            return 0;
        }

        // get the account name from the file based on the account number
        static string GetAccountName(string accountNumber)
        {
            string[] lines = File.ReadAllLines(AccountsFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length >= 2 && parts[0] == accountNumber)
                {
                    return parts[1];
                }
            }
            return null;
        }
        // Read password from console without echoing characters
        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
        // Hash the password using SHA256
        static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // User Feedback Method
        static void UserFeedback()
        {
            Console.Write("\nPlease rate our service (1 to 5): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int rating) && rating >= 1 && rating <= 5)
            {
                FeedbackRatings.Add(rating);
                File.AppendAllText(FeedbackFilePath, rating + Environment.NewLine);
                Console.WriteLine("Thank you for your feedback!");
            }
            else
            {
                Console.WriteLine("Invalid rating. Feedback must be a number between 1 and 5.");
            }
        }

        // Backup Data
        static void BackupData()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
            string backupFileName = $"Backup_{timestamp}.txt";

            List<string> backupContent = new List<string>();

            if (File.Exists(AccountsFilePath))
            {
                backupContent.Add("ACCOUNTS");
                backupContent.AddRange(File.ReadAllLines(AccountsFilePath));
                backupContent.Add("");
            }

            if (File.Exists("transactions.txt"))
            {
                backupContent.Add("TRANSACTIONS");
                backupContent.AddRange(File.ReadAllLines("transactions.txt"));
                backupContent.Add("");
            }

            if (File.Exists("loan_requests.txt"))
            {
                backupContent.Add("LOAN REQUESTS");
                backupContent.AddRange(File.ReadAllLines("loan_requests.txt"));
                backupContent.Add("");
            }

            if (File.Exists("active_loans.txt"))
            {
                backupContent.Add("ACTIVE LOANS");
                backupContent.AddRange(File.ReadAllLines("active_loans.txt"));
                backupContent.Add("");
            }
            if (File.Exists("appointments.txt"))
            {
                backupContent.Add("APPOINTMENT");
                backupContent.AddRange(File.ReadAllLines("appointments.txt"));
                backupContent.Add("");
            }
            
            if (File.Exists(ReviewsFilePath))
            {
                backupContent.Add("REVIEWS");
                backupContent.AddRange(File.ReadAllLines(ReviewsFilePath));
                backupContent.Add("");
            }
            File.WriteAllLines(backupFileName, backupContent);

            Console.WriteLine($"\nBackup created");
            Console.ReadKey();
        }

    }
}
